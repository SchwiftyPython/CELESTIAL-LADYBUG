using UnityEngine;

namespace Assets.Scripts
{
    public class Parallax : MonoBehaviour, ISubscriber
    {
        private const string PauseEvent = GlobalHelper.PauseTimer;
        private const string ResumeEvent = GlobalHelper.ResumeTimer;

        private bool _paused;

        //Parallax Scroll Variables
        public Camera cam;//the camera
        public Transform subject;//the subject (usually the player character)


        //Instance variables
        private float _zPosition;
        private Vector2 _startPos;


        //Properties
        private float TwoAspect => cam.aspect * 2;

        private float TileWidth = 10;
        private float ViewWidth => loopSpriteRenderer.sprite.rect.width / loopSpriteRenderer.sprite.pixelsPerUnit;
        private Vector2 Travel => (Vector2)cam.transform.position - _startPos; //2D distance travelled from our starting position
        private float DistanceFromSubject => transform.position.z - subject.position.z;
        private float ClippingPlane => (cam.transform.position.z + (DistanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
        private float ParallaxFactor => Mathf.Abs(DistanceFromSubject) / ClippingPlane;


        //User options
        public bool xAxis = true; //parallax on x?
        public bool yAxis = true; //parallax on y?
        public bool infiniteLoop = false; //are we looping?


        //Loop requirement
        public SpriteRenderer loopSpriteRenderer;

        // Start is called before the first frame update
        private void Awake()
        {
            _paused = false;

            var eventMediator = FindObjectOfType<EventMediator>();

            eventMediator.SubscribeToEvent(PauseEvent, this);
            eventMediator.SubscribeToEvent(ResumeEvent, this);

            cam = Camera.main;
            _startPos = transform.position;
            _zPosition = transform.position.z;

            if (loopSpriteRenderer != null && infiniteLoop)
            {
                float spriteSizeX = loopSpriteRenderer.sprite.rect.width / loopSpriteRenderer.sprite.pixelsPerUnit;
                float spriteSizeY = loopSpriteRenderer.sprite.rect.height / loopSpriteRenderer.sprite.pixelsPerUnit;

                loopSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
                loopSpriteRenderer.size = new Vector2(spriteSizeX * TileWidth, spriteSizeY);
                transform.localScale = Vector3.one / 2;
            }
        }


        // Update is called once per frame
        private void Update()
        {
            if (_paused)
            {
                return;
            }

            Vector2 newPos = _startPos + Travel * ParallaxFactor;
            transform.position = new Vector3(xAxis ? newPos.x : _startPos.x, yAxis ? newPos.y : _startPos.y, _zPosition);

            if (infiniteLoop)
            {
                Vector2 totalTravel = cam.transform.position - transform.position;
                float boundsOffset = (ViewWidth / 2) * (totalTravel.x > 0 ? 1 : -1);
                float screens = (int)((totalTravel.x + boundsOffset) / ViewWidth);
                transform.position += new Vector3(screens * ViewWidth, 0);
            }
        }

        public void OnNotify(string eventName, object broadcaster, object parameter = null)
        {
            if (eventName.Equals(PauseEvent))
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
