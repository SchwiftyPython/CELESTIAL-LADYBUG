using UnityEngine;

namespace Assets.Scripts
{
    public class ButtonClick : MonoBehaviour
    {
        public void Clicked()
        {
            EventMediator.Instance.Broadcast(GlobalHelper.ButtonClick, this);
        }
    }
}
