using UnityEngine;

namespace Assets.Scripts
{
    public class WagonMover : MonoBehaviour, ISubscriber
    {
        [Range(0f, 5f)] [SerializeField] private float walkSpeed = 1f;

        private const string PauseEvent = GlobalHelper.PauseTimer;
        private const string ResumeEvent = GlobalHelper.ResumeTimer;

        private bool _paused;

        private void Start()
        {
            _paused = false;

            // var eventMediator = FindObjectOfType<EventMediator>();
            //
            // eventMediator.SubscribeToEvent(PauseEvent, this);
            // eventMediator.SubscribeToEvent(ResumeEvent, this);
        }

        private void Update()
        {
            if (_paused)
            {
                return;
            }

            transform.Translate(Vector2.left * (walkSpeed * Time.deltaTime));
        }

        public void Animate()
        {
            _paused = false;
        }

        public void Stop()
        {
            _paused = true;
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if(eventName.Equals(PauseEvent))
            {
                _paused = true;
            }
            else if (eventName.Equals(ResumeEvent))
            {
                _paused = false;
            }
        }
    }
}
