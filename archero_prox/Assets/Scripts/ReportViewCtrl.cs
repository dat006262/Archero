




using UnityEngine;
using UnityEngine.UI;

public class ReportViewCtrl : MonoBehaviour
{
	public Canvas mCanvas;

	public CanvasScaler mScaler;

	public RectTransform view;

	private void Awake()
	{
		mCanvas.worldCamera = GameNode.m_UICamera;
	}

	public void Init(float width, float height)
	{
		mScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
		float num = width * (float)Screen.width / (float)GameLogic.DesignWidth;
		float num2 = height * (float)Screen.height / (float)GameLogic.DesignHeight;
		float x = ((float)Screen.width - num) / 2f;
		float y = ((float)Screen.height - num2) / 2f;
		view.sizeDelta = new Vector2(num, num2);
		view.anchoredPosition = new Vector2(x, y);
	}
}
