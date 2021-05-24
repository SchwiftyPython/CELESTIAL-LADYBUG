using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EndTurnButton : MonoBehaviour
    {
        public void OnClick()
        {
            var eventMediator = Object.FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.EndTurn, this);
        }
    }
}
