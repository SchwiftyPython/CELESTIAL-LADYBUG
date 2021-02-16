using UnityEngine;

namespace Assets.Scripts
{
    //todo can we make this happen automagically somehow without having to attach it to every button in the inspector?
    public class ButtonClick : MonoBehaviour
    {
        public void Clicked()
        {
            EventMediator.Instance.Broadcast(GlobalHelper.ButtonClick, this);
        }
    }
}
