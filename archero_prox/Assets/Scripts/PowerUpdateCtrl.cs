




using DG.Tweening;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpdateCtrl : MonoBehaviour
{
	public RectTransform child;

	public RectTransform image_bg;

	public Text Text_Value;

	public Text Text_Change;

	private float change_y_init;

	private int m_before;

	private int m_after;

	private int m_current;

	private static Color color_add = Color.green;

	private static Color color_reduce = Color.red;

	private void Awake()
	{
		Vector2 anchoredPosition = Text_Change.rectTransform.anchoredPosition;
		change_y_init = anchoredPosition.y;
	}

	public void Init(int before, int after)
	{
		m_before = before;
		m_after = after;
		m_current = m_before;
		SetTextValue(m_before);
		child.localScale = new Vector3(0f, 1f, 1f);
		RectTransform rectTransform = child;
		float preferredWidth = Text_Value.preferredWidth;
		Vector2 sizeDelta = child.sizeDelta;
		rectTransform.sizeDelta = new Vector2(preferredWidth, sizeDelta.y);
		RectTransform rectTransform2 = image_bg;
		float x = Text_Value.preferredWidth + 300f;
		Vector2 sizeDelta2 = image_bg.sizeDelta;
		rectTransform2.sizeDelta = new Vector2(x, sizeDelta2.y);
		int num = after - before;
		bool flag = num >= 0;
		Text_Change.color = ((!flag) ? color_reduce : color_add);
		string text = (!flag) ? "-" : "+";
		Text_Change.text = Utils.FormatString("{0}{1}", text, MathDxx.Abs(num));
		Text_Change.rectTransform.anchoredPosition = new Vector2(Text_Value.preferredWidth + 10f, 0f);
		PlayAnimation();
	}

	private void SetTextValue(int value)
	{
		string text = "战斗力";
		Text_Value.text = Utils.FormatString("{0} {1}", text, value);
	}

	private void PlayAnimation()
	{
		Sequence s = DOTween.Sequence();
		s.Append(child.DOScaleX(1f, 0.2f).SetEase(Ease.OutBack));
		s.AppendInterval(0.3f);
		s.Append(Text_Change.DOFade(0f, 0.5f));
		s.Join(DOTween.To(() => m_current, delegate(int x)
		{
			m_current = x;
		}, m_after, 0.5f).SetEase(Ease.OutSine).OnUpdate(delegate
		{
			SetTextValue(m_current);
		}));
		s.AppendInterval(0.3f);
		s.Append(child.DOScaleX(0f, 0.15f).SetEase(Ease.InQuad));
		s.AppendCallback(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}
}
