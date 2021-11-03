using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Abilities;
using Assets.Scripts.AI;
using Assets.Scripts.Combat;
using Assets.Scripts.Effects;
using Assets.Scripts.Effects.Args;
using Assets.Scripts.Entities.Names;
using Assets.Scripts.Items;
using Assets.Scripts.Saving;
using Assets.Scripts.UI;
using DG.Tweening;
using GoRogue;
using GoRogue.DiceNotation;
using UnityEngine;
using GameObject = GoRogue.GameFramework.GameObject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities
{
    public class Entity : GameObject, ISaveable
    {
        private const float BaseCombatDifficulty = 10;

        private struct EntityDto
        {
            public bool IsPlayer;
            public Equipment Equipment;
            public bool Moved;
            public bool MovedLastTurn;
            public string Id;
            public string Name;
            public Race Race;
            public EntityClass EntityClass;
            public Attributes Attributes;
            public Stats Stats;
            public Skills Skills;
            public Dictionary<Portrait.Slot, string> Portrait;
            public Dictionary<string, Ability> Abilities;
            public List<Effect> Effects;
            public UnityEngine.GameObject CombatSpritePrefab;
            public Texture IdleSkinSwap;
            public Texture AttackSkinSwap;
            public Texture HitSkinSwap;
            public Texture DeadSkinSwap;
            public string HurtSound;
            public string DieSound;
            public string AttackSound;
            public Vector2Int Position;
        }

        private int _level;
        private int _xp;
        private bool _isPlayer;

        protected Equipment Equipment;

        //todo use this instead of get component if possible
        private AiController _aiController;

        private bool _moved;
        private bool _movedLastTurn;

        public string Id { get; private set; }
        public string Name { get; set; }
        public Sex Sex { get; }
        public Race Race
        {
            get;
            set;
        } //todo this and class are likely modifier providers so we should make classes for these
                                  // will be easier to define starting equipment and whatnot I think
        public EntityClass EntityClass { get; set; }
        public Attributes Attributes { get; set; }
        public Stats Stats { get; set; }
        public Skills Skills { get; set; }
        public Dictionary<Portrait.Slot, string> Portrait { get; private set; }
        
        public Dictionary<string, Ability> Abilities { get; private set; } 

        public List<Effect> Effects { get; set; }

        public UnityEngine.GameObject CombatSpritePrefab { get; protected set; }
        public UnityEngine.GameObject CombatSpriteInstance { get; private set; }

        public Texture IdleSkinSwap;
        public Texture AttackSkinSwap;
        public Texture HitSkinSwap;
        public Texture DeadSkinSwap;

        public string HurtSound;
        public string DieSound;
        public string AttackSound;

        public Entity() : base((-1, -1), 1, null, false, false, true)
        {

        }

        public Entity(Race.RaceType rType, EntityClass eClass, bool isPlayer) : base((-1, -1), 1, null, false, false, true)
        {
            Id = Guid.NewGuid().ToString();

            Sex = PickSex();

            if (rType != Race.RaceType.Derpus && string.IsNullOrEmpty(Name))
            {
                Name = GenerateName(null, Sex);
            }
            else
            {
                Name = "Derpus";
            }

            Race = new Race(rType);

            EntityClass = eClass;

            _isPlayer = isPlayer;

            Attributes = new Attributes(this);
            Skills = new Skills(this);

            Abilities = new Dictionary<string, Ability>();

            Stats = new Stats(this, Attributes, Skills);

            Effects = new List<Effect>();

            if (IsDerpus())
            {
                GetDerpusPortrait();
            }
            else
            {
                GeneratePortrait();
            }

            _level = 1;
            _xp = 0;
        }

        public void SetSpriteInstance(UnityEngine.GameObject instance)
        {
            CombatSpriteInstance = instance;
        }

        public void SetSpritePosition(Vector3 newPosition)
        {
            if (CombatSpriteInstance == null)
            {
                return;
            }

            CombatSpriteInstance.transform.position = newPosition;
        }

        public bool IsPlayer()
        {
            return _isPlayer;
        }

        public bool IsDead()
        {
            return Stats.CurrentHealth <= 0;
        }

        public bool IsDerpus()
        {
            return EntityClass == EntityClass.Derpus || Race.GetRaceType() == Race.RaceType.Derpus;
        }

        public string FirstName()
        {
            return Regex.Replace(Name.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
        }

        public bool MovedLastTurn() //todo this won't work - maybe use a boolean instead
        {
            return _movedLastTurn;
        }

        public bool MovedThisTurn()
        {
            return _moved;
        }

        public void UpdateMovedFlags()
        {
            _movedLastTurn = _moved;
            _moved = false;
        }

        public void RefillActionPoints()
        {
            Stats.CurrentActionPoints = Stats.MaxActionPoints;
        }

        public void AddAbility(Ability ability)
        {
            if (Abilities == null)
            {
                Abilities = new Dictionary<string, Ability>();
            }

            if (Abilities.ContainsKey(ability.Name))
            {
                return;
            }

            Abilities.Add(ability.Name, ability);
        }
        
        public void MoveTo(Tile tile, int apMovementCost, List<Tile> path = null)
        {
            if (tile == null)
            {
                return;
            }

            if (apMovementCost > Stats.CurrentActionPoints)
            {
                Debug.Log("AP movement cost greater than current AP!");
                return;
            }

            var combatManager = Object.FindObjectOfType<CombatManager>();
            var map = combatManager.Map;

            var currentTile = map.GetTileAt(Position);

            //this will not update the position if blocked
            Position = tile.Position;

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            if (Position == tile.Position)
            {
                if (tile.RetreatTile)
                {
                    combatManager.RemoveEntity(this);
                    
                    eventMediator.Broadcast(GlobalHelper.EndTurn, this);
                    return;
                }

                _moved = true;

                Stats.CurrentActionPoints -= apMovementCost;

                currentTile.SpriteInstance.GetComponent<TerrainSlotUi>().SetEntity(null);

                const float durationMod = 0.25f;

                if (path != null)
                {
                    var waypoints = new Queue<Vector3>();

                    foreach (var step in path)
                    {
                        if (IsPlayer())
                        {
                            waypoints.Enqueue(new Vector2(step.Position.X, step.Position.Y));
                        }
                        else
                        {
                            waypoints.Enqueue(new Vector2(step.Position.X + 1, step.Position.Y));
                        }
                        
                    }

                    var duration = waypoints.Count * durationMod;

                    var tween = CombatSpriteInstance.transform.DOPath(waypoints.ToArray(), duration, PathType.Linear,
                        PathMode.TopDown2D);

                    var animationHelper = CombatSpriteInstance.transform.GetComponent<CombatAnimationHelper>();

                    animationHelper.StartSmoothMove(tween);
                }
                else
                {
                    var waypoint = new Vector3[1];

                    if (IsPlayer())
                    {
                        waypoint[0] = new Vector2(tile.Position.X, tile.Position.Y);
                    }
                    else
                    {
                        waypoint[0] = new Vector2(tile.Position.X + 1, tile.Position.Y);
                    }

                    CombatSpriteInstance.transform.DOPath(waypoint, durationMod, PathType.Linear,
                        PathMode.TopDown2D);

                    var tween = CombatSpriteInstance.transform.DOPath(waypoint, durationMod, PathType.Linear,
                        PathMode.TopDown2D);

                    var animationHelper = CombatSpriteInstance.transform.GetComponent<CombatAnimationHelper>();

                    animationHelper.StartSmoothMove(tween);
                }

                tile.SpriteInstance.GetComponent<TerrainSlotUi>().SetEntity(this);

                eventMediator.Broadcast(GlobalHelper.ActiveEntityMoved, this);

                var tileEffects = tile.GetEffects();

                if (Effects.Count > 0)
                {
                    foreach (var effect in Effects.ToArray())
                    {
                        if (!effect.IsLocationDependent())
                        {
                            continue;
                        }

                        if (tileEffects != null && tileEffects.Any())
                        {
                            foreach (var tileEffect in tileEffects)
                            {
                                if (ReferenceEquals(tileEffect, effect))
                                {
                                    continue;
                                }

                                RemoveEffect(effect);
                            }
                        }
                        else
                        {
                            RemoveEffect(effect);
                        }
                    }
                }

                if (tileEffects != null && tileEffects.Any())
                {
                    foreach (var effect in tileEffects)
                    {
                        if (CanApplyEffect(effect))
                        {
                            ApplyEffect(effect);
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"Movement Blocked for {Name}");
            }

        }

        public void GenerateStartingEquipment(EntityClass eClass, Dictionary<EquipLocation, List<string>> startingTable)
        {
            var itemStore = Object.FindObjectOfType<ItemStore>();

            Equipment = new Equipment(eClass);

            foreach (var equipment in startingTable)
            {
                var itemName = equipment.Value[Random.Range(0, equipment.Value.Count)];

                if (string.IsNullOrEmpty(itemName))
                {
                    continue;
                }

                var item = (EquipableItem) itemStore.GetItemTypeByName(itemName).NewItem();

                Equip(item);
            }
        }

        public void Equip(EquipableItem item)
        {
            if (!Equipment.ItemValidForEntityClass(item))
            {
                return;
            }

            Equipment.AddItem(item.GetAllowedEquipLocation(), item);

            if (Abilities == null)
            {
                Abilities = new Dictionary<string, Ability>();
            }

            foreach (var ability in item.GetAbilities(this))
            {
                if (!Abilities.ContainsKey(ability.Name))
                {
                    Abilities.Add(ability.Name, ability);
                }
            }

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EquipmentUpdated, this);
        }

        public void UnEquip(EquipLocation slot, bool swapAttempt)
        {
            var item = Equipment.GetItemInSlot(slot);

            Equipment.RemoveItem(slot);

            foreach (var ability in item.GetAbilities(this))
            {
                if (!Equipment.AbilityEquipped(ability))
                {
                    Abilities.Remove(ability.Name);
                }
            }

            if (swapAttempt)
            {
                return;
            }

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.EquipmentUpdated, this);
        }

        public void UnEquipAll()
        {
            Equipment.RemoveAllItems();
        }

        public bool HasAbility(string abilityName)
        {
            foreach (var ability in Abilities)
            {
                if (abilityName == ability.Key)
                {
                    return true;
                }
            }

            return false;
        }

        public void ResetOneUseCombatAbilities()
        {
            if (HasAbility("Endangered Endurance") &&
                !((EndangeredEndurance)Abilities["Endangered Endurance"])
                    .SavedFromDeathThisBattle())
            {
                ((EndangeredEndurance)Abilities["Endangered Endurance"]).Reset();
            }
        }

        public void MeleeAttack(Entity target, IModifierProvider modifierProvider = null)
        {
            var hitDifficulty = CalculateCombatDifficulty(target, EntitySkillTypes.Melee);

            int toHitMod = 0;
            if (modifierProvider != null)
            {
                toHitMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.MeleeToHit);
            }

            if (AttackHit((int) hitDifficulty, target, EntitySkillTypes.Melee, toHitMod, out bool criticalHit)) 
            {
                PlayAttackAnimation(target, true);

                int damageMod = 0;
                if (modifierProvider != null)
                {
                    damageMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.Damage);
                }

                ApplyDamageWithEquipment(target, false, damageMod, criticalHit);

                if (target.IsDead())
                {
                    var message = $"{Name} killed {target.Name}!";

                    var eventMediator = Object.FindObjectOfType<EventMediator>();

                    eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
                    eventMediator.Broadcast(GlobalHelper.KilledTarget, this);
                }
                else
                {
                    //todo check for abilities that respond to attack hit

                    if (target.HasAbility("Riposte")) //todo hail mary not sure if this will work
                    {
                        target.Abilities["Riposte"].Use(this);
                    }

                }
            }
            else
            {
                PlayAttackAnimation(target, false);
            }
        }

        public void MeleeAttackWithSlot(Entity target, EquipLocation slot, IModifierProvider modifierProvider = null)
        {
            var hitDifficulty = CalculateCombatDifficulty(target, EntitySkillTypes.Melee);

            int toHitMod = 0;
            if (modifierProvider != null)
            {
                toHitMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.MeleeToHit);
            }

            if (AttackHit((int) hitDifficulty, target, EntitySkillTypes.Melee, toHitMod, out bool criticalHit))
            {
                PlayAttackAnimation(target, true);

                int damageMod = 0;
                if (modifierProvider != null)
                {
                    damageMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.Damage);
                }

                ApplyDamageWithEquipment(target, false, damageMod, criticalHit, slot);

                if (target.IsDead())
                {
                    var message = $"{Name} killed {target.Name}!";

                    var eventMediator = Object.FindObjectOfType<EventMediator>();

                    eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
                    eventMediator.Broadcast(GlobalHelper.KilledTarget, this);
                }
                else
                {
                    //todo check for abilities that respond to attack hit

                    if (target.HasAbility("Riposte")) //todo hail mary not sure if this will work
                    {
                        target.Abilities["Riposte"].Use(this);
                    }

                }
            }
            else
            {
                PlayAttackAnimation(target, false);
            }
        }

        public void RangedAttack(Entity target, IModifierProvider modifierProvider = null)
        {
            var hitDifficulty = CalculateCombatDifficulty(target, EntitySkillTypes.Ranged);

            int toHitMod = 0;
            if (modifierProvider != null)
            {
                toHitMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.MeleeToHit);
            }

            if (AttackHit((int) hitDifficulty, target, EntitySkillTypes.Ranged, toHitMod, out bool criticalHit))
            {
                PlayAttackAnimation(target, true);

                int damageMod = 0;
                if (modifierProvider != null)
                {
                    damageMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.Damage);
                }

                ApplyDamageWithEquipment(target, true, damageMod, criticalHit);

                if (target.IsDead())
                {
                    var message = $"{Name} killed {target.Name}!";

                    var eventMediator = Object.FindObjectOfType<EventMediator>();

                    eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
                    eventMediator.Broadcast(GlobalHelper.KilledTarget, this);
                }
                else
                {
                    //todo check for abilities that respond to attack hit

                    if (target.HasAbility("Riposte")) //todo hail mary not sure if this will work
                    {
                        target.Abilities["Riposte"].Use(this);
                    }

                }
            }
            else
            {
                PlayAttackAnimation(target, false);
            }
        }

        public void AttackWithAbility(Entity target, Ability ability)
        {
            IModifierProvider modifierProvider = ability as IModifierProvider;

            int toHitMod = 0;

            float hitDifficulty;
            bool attackHit;
            bool criticalHit;

            if (ability.IsRanged())
            {
                hitDifficulty = CalculateCombatDifficulty(target, EntitySkillTypes.Ranged);

                if (modifierProvider != null)
                {
                    toHitMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.RangedToHit);
                }

                attackHit = AttackHit((int) hitDifficulty, target, EntitySkillTypes.Ranged, toHitMod, out criticalHit);
            }
            else
            {
                hitDifficulty = CalculateCombatDifficulty(target, EntitySkillTypes.Melee);

                if (modifierProvider != null)
                {
                    toHitMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.MeleeToHit);
                }

                attackHit = AttackHit((int) hitDifficulty, target, EntitySkillTypes.Melee, toHitMod, out criticalHit);
            }

            if (attackHit)
            {
                PlayAttackAnimation(target, true);

                int damageMod = 0;
                if (modifierProvider != null)
                {
                    damageMod = (int)modifierProvider.GetAdditiveModifiers(CombatModifierTypes.Damage);
                }

                ApplyDamageWithAbility(target, ability, damageMod, criticalHit);

                if (target.IsDead())
                {
                    var message = $"{Name} killed {target.Name}!";

                    var eventMediator = Object.FindObjectOfType<EventMediator>();

                    eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
                    eventMediator.Broadcast(GlobalHelper.KilledTarget, this);
                }
                else
                {
                    //todo check for abilities that respond to attack hit

                    if (target.HasAbility("Riposte")) //todo hail mary not sure if this will work
                    {
                        target.Abilities["Riposte"].Use(this);
                    }

                }
            }
            else
            {
                PlayAttackAnimation(target, false);
            }
        }

        private void PlayAttackAnimation(Entity target, bool attackHit)
        {
            var animationHelper = CombatSpriteInstance.GetComponent<CombatAnimationHelper>();

            animationHelper.Target = target;

            animationHelper.attackHit = attackHit;

            var animator = CombatSpriteInstance.GetComponent<Animator>();

            animator.SetBool("IsAttacking", true);

            var swapper = CombatSpriteInstance.GetComponentInChildren<ColorSwapper>();

            if (swapper != null)
            {
                swapper.ChangeTexture(AttackSkinSwap);
            }

            var ai = CombatSpriteInstance.GetComponent<AiController>();

            if (ai == null)
            {
                return;
            }

            ai.animating = true;
        }

        public void PlayIdleAnimation()
        {
            var animator = CombatSpriteInstance.GetComponent<Animator>();

            animator.SetBool("IsAttacking", false);

            var swapper = CombatSpriteInstance.GetComponentInChildren<ColorSwapper>();

            if (swapper != null)
            {
                swapper.ChangeTexture(IdleSkinSwap);
            }

            var ai = CombatSpriteInstance.GetComponent<AiController>();

            if (ai == null)
            {
                return;
            }

            ai.animating = false;
        }

        public void PlayHitAnimation()
        {
            var animator = CombatSpriteInstance.GetComponent<Animator>();

            animator.SetTrigger("Hit");

            var swapper = CombatSpriteInstance.GetComponentInChildren<ColorSwapper>();

            if (swapper != null)
            {
                swapper.ChangeTexture(HitSkinSwap);
            }

            var ai = CombatSpriteInstance.GetComponent<AiController>();

            if (ai == null)
            {
                return;
            }

            ai.animating = false;
        }

        public IEnumerator PlayDeathAnimation()
        {
            if (CombatSpriteInstance == null)
            {
                yield return null;
            }

            var animator = CombatSpriteInstance.GetComponent<Animator>();

            animator.SetTrigger("Dead");

            var swapper = CombatSpriteInstance.GetComponentInChildren<ColorSwapper>();

            if (swapper != null)
            {
                swapper.ChangeTexture(DeadSkinSwap);
            }

            var ai = CombatSpriteInstance.GetComponent<AiController>();

            if (ai != null)
            {
                ai.animating = false;
            }

            yield return Delay();

            var combatManager = Object.FindObjectOfType<CombatManager>();

            combatManager.RemoveEntity(this);
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.0f);
        }

        public void ApplyDamageWithAbility(Entity target, Ability ability, int damageMod, bool criticalHit)
        {
            int minDamage = 0;
            int maxDamage = 0;

            (minDamage, maxDamage) = ability.GetAbilityDamageRange();

            ApplyDamage(target, (minDamage, maxDamage), damageMod, criticalHit);
        }

        public void ApplyDamageWithEquipment(Entity target, bool ranged, int damageMod, bool criticalHit,
            EquipLocation slot = EquipLocation.Weapon)
        {
            var equippedItem = Equipment.GetItemInSlot(slot);

            int minDamage = 0;
            int maxDamage = 0;

            if (equippedItem != null)
            {
                if (ranged)
                {
                    (minDamage, maxDamage) = equippedItem.GetRangedDamageRange();
                }
                else
                {
                    (minDamage, maxDamage) = equippedItem.GetMeleeDamageRange();
                }
            }

            ApplyDamage(target, (minDamage, maxDamage), damageMod, criticalHit);
        }

        private void ApplyDamage(Entity target, (int, int) damageRange, int damageMod, bool criticalHit)
        {
            var (minDamage, maxDamage) = damageRange;

            var damage = Random.Range(minDamage, maxDamage + 1) + damageMod;

            damage = GlobalHelper.ModifyNewValueForStat(this, CombatModifierTypes.Damage, damage);

            if (criticalHit)
            {
                var wildRoll = GlobalHelper.RollWildDie();

                damage += wildRoll;
            }

            var damageReduction = target.GetDamageReduction();

            damage -= damageReduction;

            string message;
            if (damage <= 0)
            {
                message = $"{target.Name} resisted all damage!"; //todo see if we can communicate to player if there is no chance for attack to do damage
            }
            else
            {
                target.SubtractHealth(damage);

                message = $"{Name} dealt {damage} damage to {target.Name}!";
            }

            var eventMediator = Object.FindObjectOfType<EventMediator>();

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
            eventMediator.Broadcast(GlobalHelper.DamageDealt, this, damage);
            eventMediator.Broadcast(GlobalHelper.DamageReceived, target, damage);

            if (!target.HasAbility("Demonic Intervention"))
            {
                return;
            }

            if (!DemonicIntervention.Intervened())
            {
                return;
            }

            SubtractHealth(damage);

            message = $"Demonic Intervention! {target.Name} dealt {damage} damage to {Name}!";

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);
            eventMediator.Broadcast(GlobalHelper.DamageDealt, target, damage);
            eventMediator.Broadcast(GlobalHelper.DamageReceived, this, damage);
        }

        public bool HasMissileWeaponEquipped()
        {
            if (Equipment == null)
            {
                return false;
            }

            var equippedWeapon = Equipment.GetItemInSlot(EquipLocation.Weapon);

            if (equippedWeapon == null)
            {
                return false;
            }

            var (min, max) = equippedWeapon.GetRangedDamageRange();

            return min > 0 && max > 0;
        }

        public bool IsStunned()
        {
            return HasEffect(new Stun(this));
        }

        private int GetTotalArmorToughness()
        {
            if (Equipment == null)
            {
                return 0;
            }

            return Equipment.GetTotalArmorToughness();
        }

        private int GetDodgeTotal()
        {
            var dodgeTotal = 0;

            if (Effects != null && Effects.Count > 0)
            {
                foreach (var effect in Effects)
                {
                    if (effect is IModifierProvider provider)
                    {
                        dodgeTotal += (int)provider.GetAdditiveModifiers(EntitySkillTypes.Dodge);
                    }
                }
            }

            if (Equipment == null)
            {
                return dodgeTotal;
            }

            dodgeTotal += Equipment.GetDodgeTotal();

            return dodgeTotal;
        }

        public EquipableItem GetEquippedWeapon()
        {
            return Equipment.GetItemInSlot(EquipLocation.Weapon);
        }

        public EquipableItem GetEquippedItemInSlot(EquipLocation slot)
        {
            return Equipment.GetItemInSlot(slot);
        }

        public Equipment GetEquipment()
        {
            return Equipment;
        }

        private float CalculateCombatDifficulty(Entity target, EntitySkillTypes skillType)
        {
            var totalDifficulty = BaseCombatDifficulty / 2; //todo testing to make easier for now

            totalDifficulty += target.GetDodgeTotal();

            //Debug.Log($"Base Combat Difficulty: {totalDifficulty}");

            return totalDifficulty;
        }

        public Tuple<int, List<string>> GetHitChancePositives(Entity target, EntitySkillTypes skillType)
        {
            var posMod = 0;
            var posMessages = new List<string>();

            if (skillType == EntitySkillTypes.Melee)
            {
                if (target.IsSurrounded())
                {
                    posMod += 10;
                    posMessages.Add("Target surrounded");
                }
            }
            else if (skillType == EntitySkillTypes.Ranged)
            {
                var distanceToTarget = Distance.CHEBYSHEV.Calculate(Position, target.Position);

                if (distanceToTarget < 6) 
                {
                    posMod += 3;
                    posMessages.Add($"Distance of {distanceToTarget}");
                }

                if (!MovedLastTurn()) 
                {
                    posMod += 10;
                    posMessages.Add("Attacker didn't move last turn");
                }

                var rangedToHitMod = (int)GlobalHelper.GetAdditiveModifiers(this, CombatModifierTypes.RangedToHit);

                if (rangedToHitMod > 0)
                {
                    posMod += rangedToHitMod;
                    posMessages.Add("From equipment");
                }
            }

            return new Tuple<int, List<string>>(posMod, posMessages.Distinct().ToList());
        }

        public Tuple<int, List<string>> GetHitChanceNegatives(Entity target, EntitySkillTypes skillType)
        {
            var negMod = 0;
            var negMessages = new List<string>();

            if (skillType == EntitySkillTypes.Melee)
            {

            }
            else if (skillType == EntitySkillTypes.Ranged)
            {
                var distanceToTarget = Distance.CHEBYSHEV.Calculate(Position, target.Position);

                if (distanceToTarget >= 6)
                {
                    negMod -= 10;
                    negMessages.Add($"Distance of {distanceToTarget}");
                }

                if (MovedThisTurn())
                {
                    negMod -= 10;
                    negMessages.Add("Attacker moved");
                } 
                else if (MovedLastTurn())
                {
                    negMod -= 10;
                    negMessages.Add("Attacker moved last turn");
                }

                if (target.MovedLastTurn())
                {
                    negMod -= 8;
                    negMessages.Add("Target moved last turn");
                }

                var rangedToHitMod = (int)GlobalHelper.GetAdditiveModifiers(this, CombatModifierTypes.RangedToHit);

                if (rangedToHitMod < 0)
                {
                    negMod += rangedToHitMod;
                    negMessages.Add("From equipment");
                }
            }

            return new Tuple<int, List<string>>(negMod, negMessages.Distinct().ToList());
        }

        public (int hitChance, List<string> positives, List<string> negatives) CalculateChanceToHitMelee(Entity target)
        {
            Debug.Log($"Attacker Melee Skill: {Skills.Melee}");

            var totalDifficulty = CalculateCombatDifficulty(target, EntitySkillTypes.Melee);

            var dicePotential = Skills.Melee * 6;

            var chanceToHit = (int)((dicePotential - totalDifficulty) / dicePotential * 100);

            var hitChancePositives = GetHitChancePositives(target, EntitySkillTypes.Melee);

            var pos = hitChancePositives.Item1;

            var hitChanceNegatives = GetHitChanceNegatives(target, EntitySkillTypes.Melee);

            var neg = hitChanceNegatives.Item1;

            chanceToHit += pos + neg;

            return (chanceToHit, hitChancePositives.Item2, hitChanceNegatives.Item2);
        }

        public (int hitChance, List<string> positives, List<string> negatives) CalculateChanceToHitRanged(Entity target)
        {
            Debug.Log($"Attacker Ranged Skill: {Skills.Ranged}");

            var totalDifficulty = CalculateCombatDifficulty(target, EntitySkillTypes.Ranged);

            var dicePotential = Skills.Ranged * 6;

            var chanceToHit = (int)((dicePotential - totalDifficulty) / dicePotential * 100);

            var hitChancePositives = GetHitChancePositives(target, EntitySkillTypes.Ranged);

            var pos = hitChancePositives.Item1;

            var hitChanceNegatives = GetHitChanceNegatives(target, EntitySkillTypes.Ranged);

            var neg = hitChanceNegatives.Item1;

            chanceToHit += pos + neg;

            return (chanceToHit, hitChancePositives.Item2, hitChanceNegatives.Item2);
        }

        private bool AttackHit(int hitDifficulty, Entity target, EntitySkillTypes skillType, int toHitMod, out bool criticalHit)
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();

            int coreRoll;
            
            if (skillType == EntitySkillTypes.Melee)
            {
                coreRoll = Dice.Roll($"{Skills.Melee - 1}d6");
                //coreRoll += (int)GlobalHelper.GetAdditiveModifiers(this, CombatModifierTypes.MeleeToHit);

                var hitChancePositives = GetHitChancePositives(target, EntitySkillTypes.Melee);

                var pos = hitChancePositives.Item1;

                var hitChanceNegatives = GetHitChanceNegatives(target, EntitySkillTypes.Melee);

                var neg = hitChanceNegatives.Item1;

                coreRoll += pos + neg;
            }
            else if (skillType == EntitySkillTypes.Ranged)
            {
                coreRoll = Dice.Roll($"{Skills.Ranged - 1}d6");
                //coreRoll += (int)GlobalHelper.GetAdditiveModifiers(this, CombatModifierTypes.RangedToHit);

                var hitChancePositives = GetHitChancePositives(target, EntitySkillTypes.Ranged);

                var pos = hitChancePositives.Item1;

                var hitChanceNegatives = GetHitChanceNegatives(target, EntitySkillTypes.Ranged);

                var neg = hitChanceNegatives.Item1;

                coreRoll += pos + neg;
            }
            else
            {
                Debug.LogError($"Invalid SkillType used to attack: {skillType}");
                criticalHit = false;
                return false;
            }

            var wildRoll = GlobalHelper.RollWildDie();

            var totalRoll = coreRoll + wildRoll + toHitMod;

            Debug.Log($"Total Roll: {totalRoll}");
            Debug.Log($"Difficulty: {hitDifficulty}");

            string message;
            if (totalRoll >= hitDifficulty)
            {
                if (target.HasAbility("Divine Intervention"))
                {
                    if (DivineIntervention.Intervened())
                    {
                        message = $"Divine Intervention! Attack missed!";

                        eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

                        criticalHit = false;
                        return false;
                    }
                }

                if (wildRoll > 6)  //todo need to apply crit to damage somehow
                {
                    criticalHit = true;
                    message = "CRITICAL HIT!";
                }
                else
                {
                    criticalHit = false;
                    message = $"Attack hit!";
                }

                eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

                eventMediator.Broadcast(GlobalHelper.TargetHit, this, target);

                return true;
            }

            message = $"Attack missed!";

            eventMediator.Broadcast(GlobalHelper.SendMessageToConsole, this, message);

            eventMediator.Broadcast(GlobalHelper.TargetMiss, this);

            criticalHit = false;
            return false;
        }

        public bool IsSurrounded()
        {
            var combatManager = Object.FindObjectOfType<CombatManager>();
            var map = combatManager.Map;

            var currentTile = map.GetTileAt(Position);

            var numEnemies = 0;

            foreach (var tile in currentTile.GetAdjacentTiles())
            {
                var entity = tile.GetEntity();

                if (entity == null)
                {
                    continue;
                }

                if (IsPlayer() != entity.IsPlayer())
                {
                    numEnemies++;

                    if (numEnemies > 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int AddHealth(int amount)
        {
            var startingHealth = Stats.CurrentHealth;

            Stats.CurrentHealth += amount;

            return Stats.CurrentHealth - startingHealth;
        }

        public void SubtractHealth(int amount)
        {
            Stats.CurrentHealth -= amount;
        }

        public int AddEnergy(int amount)
        {
            var startingEnergy = Stats.CurrentEnergy;

            Stats.CurrentEnergy += amount;

            return Stats.CurrentEnergy - startingEnergy;
        }

        public void SubtractEnergy(int amount)
        {
            Stats.CurrentEnergy -= amount;
        }

        public int AddMorale(int amount)
        {
            var startingMorale = Stats.CurrentMorale;

            Stats.CurrentMorale += amount;

            return Stats.CurrentMorale - startingMorale;
        }

        public void SubtractMorale(int amount)
        {
            Stats.CurrentMorale -= amount;
        }

        public void AddActionPoints(int amount)
        {
            Stats.CurrentActionPoints += amount;
        }

        public void SubtractActionPoints(int amount)
        {
            Stats.CurrentActionPoints -= amount;
        }

        public void AddMight(int amount)
        {
            Attributes.Physique += amount;
        }

        public void SubtractMight(int amount)
        {
            Attributes.Physique -= amount;
        }

        public void UseHealthPotion()
        {
            Heal(40); //todo make a constant somewhere
        }

        public void Heal(int amount)
        {
            AddHealth(amount);
        }

        public void ApplyEffect(Effect effect)
        {
            if (Effects == null)
            {
                Effects = new List<Effect>();
            }

            if (!effect.CanStack() && HasEffect(effect))
            {
                return;
            }

            Effects.Add(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            if (Effects == null || Effects.Count < 1)
            {
                return;
            }

            Effects.Remove(effect);
        }

        public void TriggerEffects()
        {
            if (Effects == null)
            {
                return;
            }

            foreach (var effect in Effects.ToArray())
            {
                if (effect.Duration != Effect.INFINITE && effect.Duration < 1)
                {
                    Effects.Remove(effect);
                }

                effect.Trigger(new BasicEffectArgs(this));
            }
        }

        public bool HasEffect(Effect effect)
        {
            foreach (var existingEffect in Effects)
            {
                if (existingEffect.GetType() == effect.GetType())
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanApplyEffect(Effect effect)
        {
            foreach (var ability in Abilities)
            {
                if (ability.Value.ExemptFromEffect(effect))
                {
                    return false;
                }
            }

            if (effect.GetTargetType() == TargetType.Hostile)
            {
                return effect.GetOwner().IsPlayer() != IsPlayer();
            }

            if (effect.GetTargetType() == TargetType.Friendly)
            {
                return effect.GetOwner().IsPlayer() == IsPlayer();
            }

            return true;
        }

        public int RollForInitiative()
        {
            //todo
            return -1;
        }

        public void Rest()
        {
            AddEnergy(45); //todo make a constant somewhere
        }

        private int GetDamageReduction()
        {
            var armor = GetTotalArmorToughness();

            var physiqueRoll = Dice.Roll($"{Attributes.Physique}d6");

            var reduction = armor; //+ physiqueRoll; //might be too much with physique roll

            Debug.Log($"Damage reduction = {reduction}");

            return reduction;
        }

        private string GenerateName(List<string> possibleNameFiles, Sex sex)
        {
            if (NameStore.Instance == null || !NameStore.Instance.FilesLoaded)
            {
                return GlobalHelper.RandomString(6);
            }

            return NameStore.Instance.GenerateFullName(possibleNameFiles, sex);
        }

        private void GetDerpusPortrait()
        {
            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            Portrait = spriteStore.GetDerpusPortrait();
        }

        private void GeneratePortrait()
        {
            var spriteStore = Object.FindObjectOfType<SpriteStore>();

            Portrait = spriteStore.GetRandomPortraitPreset();
        }

        private Sex PickSex()
        {
            return GlobalHelper.GetRandomEnumValue<Sex>();
        }

        private Race.RaceType PickRace()
        {
            return GlobalHelper.GetRandomEnumValue<Race.RaceType>();
        }

        private EntityClass PickEntityClass()
        {
            return GlobalHelper.GetRandomEnumValue<EntityClass>();
        }

        public object CaptureState()
        {
            var dto = new EntityDto
            {
                Position = new Vector2Int(Position.X, Position.Y),
                Name = Name,
                Id = Id,
                Abilities = Abilities,
                Attributes = Attributes,
                AttackSkinSwap = AttackSkinSwap,
                AttackSound = AttackSound,
                CombatSpritePrefab = CombatSpritePrefab,
                DeadSkinSwap = DeadSkinSwap,
                DieSound = DieSound,
                Effects = Effects,
                EntityClass = EntityClass,
                Equipment = Equipment,
                HitSkinSwap = HitSkinSwap,
                HurtSound = HurtSound,
                IdleSkinSwap = IdleSkinSwap,
                IsPlayer = _isPlayer,
                Moved = _moved,
                MovedLastTurn = _movedLastTurn,
                Portrait = Portrait,
                Race = Race,
                Skills = Skills,
                Stats = Stats
            };

            return dto;
        }

        public void RestoreState(object state)
        {
            if (state == null)
            {
                return;
            }

            var entityDto = (EntityDto)state;

            Position = new Coord(entityDto.Position.x, entityDto.Position.y);
            Name = entityDto.Name;
            Id = entityDto.Id;
            
            Abilities = entityDto.Abilities;

            if (Abilities != null && Abilities.Count > 0)
            {
                foreach (var ability in Abilities.Values)
                {
                    ability.AbilityOwner = this;
                }
            }

            Attributes = entityDto.Attributes;
            Attributes.SetParent(this);

            AttackSkinSwap = entityDto.AttackSkinSwap;
            AttackSound = entityDto.AttackSound;
            CombatSpritePrefab = entityDto.CombatSpritePrefab;
            DeadSkinSwap = entityDto.DeadSkinSwap;
            DieSound = entityDto.DieSound;
            
            Effects = entityDto.Effects;

            if (Effects != null && Effects.Count > 0)
            {
                foreach (var effect in Effects)
                {
                    effect.SetOwner(this);
                }
            }

            EntityClass = entityDto.EntityClass;
            Equipment = entityDto.Equipment;
            HitSkinSwap = entityDto.HitSkinSwap;
            HurtSound = entityDto.HurtSound;
            IdleSkinSwap = entityDto.IdleSkinSwap;
            _isPlayer = entityDto.IsPlayer;
            _moved = entityDto.Moved;
            _movedLastTurn = entityDto.MovedLastTurn;
            Portrait = entityDto.Portrait;
            Race = entityDto.Race;

            Skills = entityDto.Skills;
            Skills.SetParent(this);

            Stats = entityDto.Stats;
            Stats.SetParent(this);
        }
    }
}
