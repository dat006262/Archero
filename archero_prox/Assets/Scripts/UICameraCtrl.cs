




using UnityEngine;

public class UICameraCtrl : MonoBehaviour
{
	public Camera mCamera;

	private void Start()
	{
		mCamera.orthographicSize = GameLogic.Height / 2;
		base.transform.localPosition = new Vector3(GameLogic.Width / 2, GameLogic.Height / 2, -100f);
		UnityEngine.Object.Destroy(this);
	}
}
