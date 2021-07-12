using UnityEngine;

namespace Assets.Scripts
{
    public class InputController : MonoBehaviour
    {
        /*public static InputController Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }*/

        private void Update()
        {
            //todo this is temp -- should be able to configure the hotkeys on each window and popup
            //game dev tv inventory course shows how
            if (Input.GetKeyDown(KeyCode.I))
            {
                var pauseMenu = GameObject.Find("PauseMenuMask");

                if (GameManager.Instance.WindowActive(pauseMenu))
                {
                    return;
                }

                var partyWindow = GameObject.Find("PartyManagementWindowMask");

                EventMediator eventMediator = FindObjectOfType<EventMediator>();

                if (GameManager.Instance.WindowActive(partyWindow))
                {
                    eventMediator.Broadcast(GlobalHelper.HidePartyManagement, this);
                }
                else
                {
                    eventMediator.Broadcast(GlobalHelper.ManageParty, this);
                }
            }
            /*else if (Input.GetKeyDown(KeyCode.Escape))
            {
                var pauseMenu = GameObject.Find("PauseMenuMask");

                EventMediator eventMediator = FindObjectOfType<EventMediator>();

                if (GameManager.Instance.WindowActive(pauseMenu))
                {
                    eventMediator.Broadcast(GlobalHelper.HidePauseMenu, this);
                }
                else
                {
                    eventMediator.Broadcast(GlobalHelper.ShowPauseMenu, this);
                }
            }*/
        }
    }
}
