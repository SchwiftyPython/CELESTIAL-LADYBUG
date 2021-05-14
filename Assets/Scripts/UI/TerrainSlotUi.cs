using Assets.Scripts.Combat;
using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using UnityEngine;

public class TerrainSlotUi : MonoBehaviour, ITileHolder, IEntityHolder
{
    private Tile _tile;
    private Entity _entity;

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }

    public void SetEntity(Entity entity)
    {
        _entity = entity;
    }

    public Tile GetTile()
    {
        return _tile;
    }

    public Entity GetEntity()
    {
        return _entity;
    }
}
