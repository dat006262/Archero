




using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUIGoldAddCtrl : MonoBehaviour
{
	public Text text;

	public RectTransform imageRect;

	public Transform child;

	public CanvasGroup mCanvasGroup;

	public Action<MainUIGoldAddCtrl> OnFinish;

	private Sequence seq;

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	public void SetGold(long gold)
	{
		KillSequence();
		seq = DOTween.Sequence();
		child.localPosition = Vector3.zero;
		mCanvasGroup.alpha = 1f;
		seq.Append(child.DOLocalMoveY(100f, 1f));
		seq.Join(DOTween.Sequence().AppendInterval(0.5f).Append(mCanvasGroup.DOFade(0f, 0.5f)));
		seq.AppendCallback(delegate
		{
			if (OnFinish != null)
			{
				OnFinish(this);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		});
		text.text = Utils.FormatString("+{0}", gold);
		Vector2 sizeDelta = imageRect.sizeDelta;
		float num = sizeDelta.x + text.preferredWidth;
		float num2 = (0f - num) / 2f;
		float num3 = num2;
		Vector2 sizeDelta2 = imageRect.sizeDelta;
		float num4 = num3 + sizeDelta2.x;
		RectTransform rectTransform = imageRect;
		float x = num2;
		Vector2 anchoredPosition = imageRect.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition.y);
		RectTransform rectTransform2 = text.rectTransform;
		float x2 = num4;
		Vector2 anchoredPosition2 = text.rectTransform.anchoredPosition;
		rectTransform2.anchoredPosition = new Vector2(x2, anchoredPosition2.y);
	}
}
