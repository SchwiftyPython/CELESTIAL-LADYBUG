using UnityEngine;

namespace Assets.Scripts.UI
{
    public class EndTurnButton : MonoBehaviour
    {
        public void OnClick()
        {
            EventMediator.Instance.Broadcast(GlobalHelper.EndTurn, this);
        }
    }
}
