using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Effects;
using Assets.Scripts.Entities;
using GoRogue;
using GoRogue.GameFramework;
using UnityEngine;
using GameObject = GoRogue.GameFramework.GameObject;

namespace Assets.Scripts.Combat
{
    public class Tile : IGameObject
    {
        private IGameObject _backingField;

        private List<Effect> _effects; //todo have a playerEffects list and a nonPlayerEffects list to differentiate who should get effect applied

        public Sprite Texture { get; protected set; }

        public UnityEngine.GameObject SpriteInstance { get; private set; }

        public TileType TileType { get; protected set; }

        public Map CurrentMap => _backingField.CurrentMap;

        public bool IsStatic => _backingField.IsStatic;

        public bool IsTransparent 
        {
            get => _backingField.IsTransparent;
            set => _backingField.IsTransparent = value;
        }

        public bool IsWalkable 
        {
            get => _backingField.IsWalkable;
            set => _backingField.IsWalkable = value;
        }

        public Coord Position 
        {
            get => _backingField.Position;
            set => _backingField.Position = value;
        }

        public event EventHandler<ItemMovedEventArgs<IGameObject>> Moved 
        {
            add => _backingField.Moved += value;
            remove => _backingField.Moved -= value;
        }

        public uint ID => _backingField.ID;

        public int Layer => _backingField.Layer;

        public Tile(Coord position, bool isWalkable, bool isTransparent)
        {
            _backingField = new GameObject(position, 0, this, true,
                isWalkable, isTransparent);
        }

        public void SetSpriteInstance(UnityEngine.GameObject instance)
        {
            SpriteInstance = instance;
        }

        public Tile GetAdjacentTileByDirection(Direction direction)
        {
            var neighbors = AdjacencyRule.EIGHT_WAY.NeighborsClockwise(Position, direction);

            return ((CombatMap)CurrentMap).GetTileAt(neighbors.First());
        }

        public List<Tile> GetAdjacentTiles()
        {
            var neighbors = AdjacencyRule.EIGHT_WAY.NeighborsClockwise(Position);

            var tiles = new List<Tile>();

            foreach (var coord in neighbors)
            {
                var tile = ((CombatMap)CurrentMap).GetTileAt(coord);

                if (tile == null)
                {
                    continue;
                }

                tiles.Add(tile);
            }

            return tiles;
        }

        public void AddComponent(object component)
        {
            _backingField.AddComponent(component);
        }

        public T GetComponent<T>()
        {
            return _backingField.GetComponent<T>();
        }

        public IEnumerable<T> GetComponents<T>()
        {
            return _backingField.GetComponents<T>();
        }

        public bool HasComponent(Type componentType)
        {
            return _backingField.HasComponent(componentType);
        }

        public bool HasComponent<T>()
        {
            return _backingField.HasComponent<T>();
        }

        public bool HasComponents(params Type[] componentTypes)
        {
            return _backingField.HasComponents(componentTypes);
        }

        public void RemoveComponent(object component)
        {
            _backingField.RemoveComponent(component);
        }

        public void RemoveComponents(params object[] components)
        {
            _backingField.RemoveComponents(components);
        }

        public void AddEffect(Effect effect)
        {
            if (_effects == null)
            {
                _effects = new List<Effect>();
            }

            _effects.Add(effect);

            var presentEntity = (Entity)CurrentMap.Entities.GetItems(Position).FirstOrDefault();

            presentEntity?.ApplyEffect(effect);
        }

        public void RemoveEffect(Effect effect)
        {
            _effects.Remove(effect);

            var presentEntity = (Entity)CurrentMap.Entities.GetItems(Position).FirstOrDefault();

            if (presentEntity == null)
            {
                return;
            }

            foreach (var entityEffect in presentEntity.Effects.ToArray())
            {
                if (ReferenceEquals(entityEffect, effect))
                {
                    presentEntity.RemoveEffect(effect);
                }
            }

            //this is to account for more than one instance of a location based effect that doesn't stack
            foreach (var tileEffect in _effects)
            {
                if (!tileEffect.CanStack() && tileEffect.GetType() == effect.GetType())
                {
                    presentEntity.ApplyEffect(tileEffect);
                    break;
                }
            }
        }

        public bool HasEffect(Effect effect)
        {
            if (_effects == null || _effects.Count < 1)
            {
                return false;
            }

            foreach (var activeEffect in _effects)
            {
                if (ReferenceEquals(activeEffect, effect))
                {
                    return true;
                }
            }

            return false;
        }

        public List<Effect> GetEffects()
        {
            return _effects;
        }

        public bool MoveIn(Direction direction)
        {
            return _backingField.MoveIn(direction);
        }

        public void OnMapChanged(Map newMap)
        {
            _backingField.OnMapChanged(newMap);
        }
    }
}
