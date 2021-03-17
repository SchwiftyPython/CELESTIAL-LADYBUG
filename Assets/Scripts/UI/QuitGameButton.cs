using UnityEngine;

namespace Assets.Scripts.UI
{
    public class QuitGameButton : MonoBehaviour
    {
        public void QuitToDesktop()
        {
            Application.Quit();
        }
    }
}
