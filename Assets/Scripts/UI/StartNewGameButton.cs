using UnityEngine;

namespace Assets.Scripts.UI
{
    public class StartNewGameButton : MonoBehaviour
    {
        public void StartNewGame()
        {
            var gameManager = GameObject.Find("GameManager");

            if (gameManager == null)
            {
                Debug.LogError("Can't find GameManager to start new game!");
                return;
            }

            gameManager.GetComponent<GameManager>().StartNewGame();
        }
    }
}
