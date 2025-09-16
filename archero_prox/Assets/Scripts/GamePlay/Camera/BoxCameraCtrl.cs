




using UnityEngine;

public class BoxCameraCtrl : MonoBehaviour
{
	private Camera mCamera;

	private void Awake()
	{
		mCamera = GetComponent<Camera>();
		mCamera.orthographicSize *= GameLogic.WidthScale;
	}

	private void Update()
	{
	}
}
