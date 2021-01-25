using System;
using System.Collections.Generic;
using GoRogue;
using GoRogue.GameFramework;
using GameObject = GoRogue.GameFramework.GameObject;

namespace Assets.Scripts.Combat
{
    public class Tile : IGameObject
    {
        private IGameObject _backingField;

        private string _prefabName;

        public UnityEngine.GameObject PrefabTexture { get; private set; }

        public UnityEngine.GameObject SpriteInstance { get; private set; }

        public TileType TileType { get; private set; }

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

        public Tile(TileType tileType, UnityEngine.GameObject texture, Coord position, bool isWalkable, bool isTransparent)
        {
            _backingField = new GameObject(position, 0, this, true,
                isWalkable, isTransparent);

            PrefabTexture = texture;
            _prefabName = texture.name;
        }

        public void SetSpriteInstance(UnityEngine.GameObject instance)
        {
            SpriteInstance = instance;
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
