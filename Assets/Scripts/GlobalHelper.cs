using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Assets.Scripts.Abilities;
using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using GoRogue;
using GoRogue.DiceNotation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GlobalHelper : MonoBehaviour
    {
        public const string DoubleSpace = "\n\n";
        public const string SingleSpace = "\n";
        public const float DefaultTextSpeed = 0.015f;

        #region EventNames  //todo would it be better to make these enums?

        public const string EncounterOptionSelected = "EncounterOptionSelected";
        public const string FourOptionEncounter = "FourOptionEncounter";
        public const string TwoOptionEncounter = "TwoOptionEncounter";
        public const string NoOptionEncounter = "NoOptionEncounter";
        public const string CombatEncounter = "CombatEncounter";
        public const string RetreatEncounterFailed = "RetreatEncounterFailed";
        public const string EncounterResult = "EncounterResult";
        public const string EncounterFinished = "EncounterFinished";
        public const string WritingFinished = "WritingFinished";
        public const string TravelMessagesFinished = "TravelMessagesFinished";
        public const string CampingEncounterFinished = "CampingEncounterFinished";
        public const string PartyEatAndHeal = "PartyEatAndHeal";
        public const string PlayerTurn = "PlayerTurn";
        public const string AiTurn = "AiTurn";
        public const string CombatSceneLoaded = "CombatSceneLoaded";
        public const string RefreshCombatUi = "RefreshCombatUi";
        public const string TileSelected = "TileSelected";
        public const string TileDeselected = "TileDeselected";
        public const string TileHovered = "TileHovered";
        public const string AbilityHovered = "AbilityHovered";
        public const string HidePopup = "HidePopup";
        public const string ManageParty = "ManageParty";
        public const string HidePartyManagement = "HidePartyManagement";
        public const string PopulateCharacterSheet = "PopulateCharacterSheet";
        public const string EquipmentUpdated = "EquipmentUpdated";
        public const string InventoryUpdated = "InventoryUpdated";
        public const string EndTurn = "EndTurn";
        public const string NextTarget = "NextTarget";
        public const string EntityTargeted = "EntityTargeted";
        public const string TargetHit = "TargetHit";
        public const string TargetMiss = "TargetMiss";
        public const string CombatFinished = "CombatFinished";
        public const string EntityDead = "EntityDead";
        public const string MentalBreak = "MentalBreak";
        public const string DerpusNoEnergy = "DerpusNoEnergy";
        public const string DerpusNoMorale = "DerpusNoMorale";
        public const string SendMessageToConsole = "SendMessageToConsole";
        public const string GameOver = "GameOver";
        public const string YouWon = "YouWon";
        public const string ButtonClick = "ButtonClick";
        public const string MeleeHit = "MeleeHit";
        public const string MeleeMiss = "MeleeMiss";
        public const string DamageDealt = "DamageDealt";
        public const string DamageReceived = "DamageReceived";
        public const string KilledTarget = "KilledTarget";
        public const string ActiveEntityMoved = "ActiveEntityMoved";
        public const string PauseTimer = "PauseTimer";
        public const string ResumeTimer = "ResumeTimer";
        public const string ShowPauseMenu = "ShowPauseMenu";
        public const string HidePauseMenu = "HidePauseMenu";
        public const string SpritesLoaded = "SpritesLoaded";
        public const string ShowCombatPreview = "ShowCombatPreview";
        public const string BiomeChanged = "BiomeChanged";
        public const string SaveLoaded = "SaveLoaded";
        public const string MoveAnimationComplete = "MoveAnimationComplete";

        #endregion EventNames

        #region SceneNames

        public const string CombatScene = "Combat";
        public const string TravelScene = "Travel";
        public const string TitleScreenScene = "TitleScreen";

        #endregion SceneNames

        #region SoundPaths //todo move these to an Audio Controller/Store -- See MusicController.cs

        public const string MonsterHitOne = "event:/Monster Hit 1";
        public const string MonsterDieOne = "event:/Monster Die 1";

        public const string CompanionHitOne = "event:/Companion Hit 1";
        public const string CompanionDieOne = "event:/Companion Die 1";

        #endregion SoundPaths

        private static readonly System.Random SysRandom = new System.Random();

        public static void DestroyAllChildren(GameObject parent)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                Destroy(parent.transform.GetChild(i).gameObject);
            }
        }

        public static void DestroyAllChildren(RectTransform parent)
        {
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                Destroy(parent.transform.GetChild(i).gameObject);
            }
        }

        public static void DestroyObject(GameObject go)
        {
            Destroy(go);
        }

        public static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string CapitalizeAllWords(string s)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static string CapitalizeAllLetters(string s)
        {
            return s.ToUpper();
        }

        public static string SplitStringByCapitalLetters(string s)
        {
            var splitString = "";

            foreach (var letter in s)
            {
                if (char.IsUpper(letter) && splitString.Length > 0)
                {
                    splitString += " " + letter;
                }
                else
                {
                    splitString += letter;
                }
            }

            return splitString;
        }

        public static Color GetColorFromString(string color)
        {
            float red = Hex_to_Dec01(color.Substring(0, 2));
            float green = Hex_to_Dec01(color.Substring(2, 2));
            float blue = Hex_to_Dec01(color.Substring(4, 2));
            float alpha = 1f;
            if (color.Length >= 8)
            {
                // Color string contains alpha
                alpha = Hex_to_Dec01(color.Substring(6, 2));
            }
            return new Color(red, green, blue, alpha);
        }

        public static float Hex_to_Dec01(string hex)
        {
            return Hex_to_Dec(hex) / 255f;
        }

        public static int Hex_to_Dec(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }

        public static IEnumerator Delay(int time)
        {
            yield return new WaitForSecondsRealtime(time);
        }

        //<Summary>
        // Returns a random enum value of type T
        //</Summary>
        public static T GetRandomEnumValue<T>()
        {
            var values = Enum.GetValues(typeof(T));

            return (T)values.GetValue(Random.Range(0, values.Length));
        }

        public static string GetEnumDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description
                ?? value.ToString();
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            throw new ArgumentException($@"Not found: {description}", nameof(description));
        }

        public static T NextEnum<T>(T src) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            }

            var arr = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf(arr, src) + 1;
            return arr.Length == j ? arr[0] : arr[j];
        }

        /// <summary>
        /// Applies all modifiers to a new value for the given ModType.
        /// </summary>
        public static int ModifyNewValueForStat(Entity parent, Enum modType, int value)
        {
            return (int)(GetAdditiveModifiers(parent, modType) + value * (1 + GetPercentageModifiers(parent, modType) / 100));
        }

        /// <summary>
        /// Returns all additive modifiers in equipment, abilities, and effects for the given ModType.
        /// </summary>
        public static float GetAdditiveModifiers(Entity parent, Enum modType)
        {
            float total = 0;

            var equipment = parent.GetEquipment();

            if (equipment == null)
            {
                return total;
            }

            foreach (EquipLocation slot in Enum.GetValues(typeof(EquipLocation)))
            {
                var item = equipment.GetItemInSlot(slot);

                if (item == null)
                {
                    continue;
                }

                total += item.GetAdditiveModifiers(modType);
            }

            List<Ability> passiveAbilities = parent.Abilities.Values.Where(a => a.IsPassive).ToList();

            total += GetAdditiveModifiersInCollection(passiveAbilities, modType);
            total += GetAdditiveModifiersInCollection(parent.Effects, modType);

            return total;
        }

        /// <summary>
        /// Returns all percentage modifiers in equipment, abilities, and effects for the given ModType.
        /// </summary>
        public static float GetPercentageModifiers(Entity parent, Enum modType)
        {
            float total = 0;

            var equipment = parent.GetEquipment();

            if (equipment == null)
            {
                return total;
            }

            foreach (EquipLocation slot in Enum.GetValues(typeof(EquipLocation)))
            {
                var item = equipment.GetItemInSlot(slot);

                if (item == null)
                {
                    continue;
                }

                total += item.GetPercentageModifiers(modType);
            }

            List<Ability> passiveAbilities = parent.Abilities.Values.Where(a => a.IsPassive).ToList();

            total += GetPercentageModifiersInCollection(passiveAbilities, modType);
            total += GetPercentageModifiersInCollection(parent.Effects, modType);

            return total;
        }

        private static int GetAdditiveModifiersInCollection<T>(IReadOnlyCollection<T> collection, Enum modType)
        {
            var total = 0f;

            if (collection == null || !collection.Any())
            {
                return (int) total;
            }

            foreach (var item in collection)
            {
                if (!(item is IModifierProvider provider))
                {
                    continue;
                }

                total += provider.GetAdditiveModifiers(modType); 
            }

            return (int) total;
        }

        private static int GetPercentageModifiersInCollection<T>(IReadOnlyCollection<T> collection, Enum modType)
        {
            var total = 0f;

            if (collection == null || !collection.Any())
            {
                return (int)total;
            }

            foreach (var item in collection)
            {
                if (!(item is IModifierProvider provider))
                {
                    continue;
                }

                total += provider.GetPercentageModifiers(modType);
            }

            return (int)total;
        }

        public static List<T> ShuffleList<T>(List<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var n = Random.Range(0, i + 1);
                var temp = list[i];
                list[i] = list[n];
                list[n] = temp;
            }

            return list;
        }

        public static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;   

            for (var i = 0; i < size; i++)
            {
                var @char = (char)SysRandom.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public static int RollWildDie()
        {
            var roll = Dice.Roll("1d6");

            var total = roll;

            while (roll == 6)
            {
                roll = Dice.Roll("1d6");
                total += roll;
            }

            Debug.Log($"Wild Roll Total: {total}");

            return total;
        }

        public static bool HasLineOfSight(Coord startPos, Coord targetCoord)
        {
            var line = Lines.Get(startPos, targetCoord).ToList();

            var combatManager = FindObjectOfType<CombatManager>();

            var map = combatManager.Map;

            foreach (var coord in line)
            {
                if (coord == line.First() || coord == line.Last())
                {
                    continue;
                }

                if (!map.GetTileAt(coord).IsWalkable)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
