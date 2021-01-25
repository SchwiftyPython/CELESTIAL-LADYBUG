using Assets.Scripts.Combat;
using UnityEngine;

namespace Assets.Scripts.UI
{
    //todo can probably handle highlighting here too -- but don't fix it if it ain't broke
    public class OnMouseOverTile : MonoBehaviour
    {
        public Tile Tile { get; set; }

        private void OnMouseEnter()
        {
            //yield return new WaitForSeconds(0.5f);

            /*var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var mouseCoord = new Coord((int) mousePosition.x, (int) mousePosition.y);

            var map = CombatManager.Instance.Map;

            if (map.OutOfBounds(mouseCoord))
            {
                yield break;
            }

            var targetTile = map.GetTerrain<Floor>(mouseCoord);

            if (targetTile == null || !targetTile.IsWalkable)
            {
                yield break;
            }*/

            //todo need to not do this if a tile is selected

            EventMediator.Instance.Broadcast(GlobalHelper.TileHovered, Tile);
        }

        private void OnMouseExit()
        {
            EventMediator.Instance.Broadcast(GlobalHelper.HidePopup, this);
        }

        private void OnMouseDown()
        {
            //todo maybe we can do cost here if combat input controller doesn't work out
        }
    }
}
