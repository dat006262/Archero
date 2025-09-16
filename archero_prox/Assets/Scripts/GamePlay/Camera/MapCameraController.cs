




using UnityEngine;
using UnityEngine.EventSystems;

namespace com.F4A.MobileThird
{
    public class MapCameraController : MonoBehaviour
    {
        public Camera cameraMap;
        public Bounds Bounds;

        private Vector2 _prevPosition;

        private Vector2 firstV, deltaV;
        private float currentTime, speed;
        private bool touched;

        private void Awake()
        {
            currentTime = 0;
            speed = 0;

            if (!cameraMap)
                cameraMap = GetComponent<Camera>();
            if (!cameraMap)
                cameraMap = Camera.main;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }

        public void Update()
        {

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
            HandleTouchInput();
#else
            HandleMouseInput();
#endif
        }

        private void LateUpdate()
        {
            SetPosition(transform.position);
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touched = true;
                    deltaV = Vector2.zero;
                    _prevPosition = touch.position;
                    firstV = _prevPosition;
                    currentTime = Time.time;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 curPosition = touch.position;
                    MoveCamera(_prevPosition, curPosition);
                    deltaV = _prevPosition - curPosition;
                    _prevPosition = curPosition;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touched = false;
					//MapViewManager.Instance.CreateLevelOnMap();
                }
            }
            else if (!touched)
            {
                deltaV -= deltaV * Time.deltaTime * 10;
                transform.Translate(deltaV.x / 30, deltaV.y / 30, 0);
            }

        }

        private void HandleMouseInput()
        {
            // press mouse
            if (Input.GetMouseButtonDown(0))
            {
                deltaV = Vector2.zero;
                _prevPosition = Input.mousePosition;
                firstV = _prevPosition;
                currentTime = Time.time;
            }
            // move mouse
            else if (Input.GetMouseButton(0))
            {
                Vector2 curMousePosition = Input.mousePosition;
                MoveCamera(_prevPosition, curMousePosition);
                deltaV = _prevPosition - curMousePosition;

                _prevPosition = curMousePosition;
                speed = Time.time;
            }
            // release the mouse
            else if (Input.GetMouseButtonUp(0))
            {
                //speed = (Time.time - currentTime);
                //Vector3 diffV = (transform.position - (Vector3)deltaV);
                //Vector3 destination = (transform.position - diffV / 20);
				//MapViewManager.Instance.CreateLevelOnMap();
            }
            else
            {
                deltaV -= deltaV * Time.deltaTime * 10;
                transform.Translate(deltaV.x / 30, deltaV.y / 30, 0);
            }

        }
        private void MoveCamera(Vector2 prevPosition, Vector2 curPosition)
        {
            if (EventSystem.current.IsPointerOverGameObject(-1))
                return;
            var deltaMove = cameraMap.ScreenToWorldPoint(prevPosition) - cameraMap.ScreenToWorldPoint(curPosition);
            SetPosition(transform.localPosition + deltaMove);
			//MapViewManager.Instance.CreateLevelOnMap();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            Vector2 validatedPosition = ApplyBounds(position);
            transform.position = new Vector3(validatedPosition.x, validatedPosition.y, transform.position.z);
        }

        private Vector2 ApplyBounds(Vector2 position)
        {
            float cameraHeight = cameraMap.orthographicSize * 2f;
            float cameraWidth = (Screen.width * 1f / Screen.height) * cameraHeight;
            position.x = Mathf.Max(position.x, Bounds.min.x + cameraWidth / 2f);
            position.y = Mathf.Max(position.y, Bounds.min.y + cameraHeight / 2f);
            position.x = Mathf.Min(position.x, Bounds.max.x - cameraWidth / 2f);
            position.y = Mathf.Min(position.y, Bounds.max.y - cameraHeight / 2f);
            return position;
        }
    }
}
