using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class CombatInputController : MonoBehaviour, ISubscriber
    {
        private const string CombatSceneLoaded = GlobalHelper.CombatSceneLoaded;

        private bool _isPlayerTurn;

        private CombatMap _map;

        private void Start()
        {
        
        }

        private void Update()
        {
            if (_isPlayerTurn)
            {
                HighlightPathUnderMouse();

                if (Input.GetMouseButtonDown(0))
                {

                }
                else if (Input.GetMouseButtonDown(1))
                {

                }
            }
        }

        private void HighlightPathUnderMouse()
        {
            //todo want to update path for current tile that mouse is over in real time

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void HighlightPathToTarget(Tile tile)
        {
            //todo highlight path to tile
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(CombatSceneLoaded))
            {
                _map = parameter as CombatMap;
            }
        }
    }
}
