




using UnityEngine;

public class MainBottomUICtrl : MonoBehaviour
{
	public RectTransform bottomline;

	private void Start()
	{
		float bottomHeight = PlatformHelper.GetBottomHeight();
		(base.transform as RectTransform).anchoredPosition = new Vector2(0f, bottomHeight);
		if ((bool)bottomline)
		{
			RectTransform rectTransform = bottomline;
			rectTransform.anchoredPosition = new Vector2(0f, (0f - bottomHeight) / GameLogic.WidthScaleAll);
		}
	}
}
