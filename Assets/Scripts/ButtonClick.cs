using UnityEngine;

namespace Assets.Scripts
{
    //todo can we make this happen automagically somehow without having to attach it to every button in the inspector?
    public class ButtonClick : MonoBehaviour
    {
        public void Clicked()
        {
            var eventMediator = FindObjectOfType<EventMediator>();
            eventMediator.Broadcast(GlobalHelper.ButtonClick, this);
        }
    }
}
