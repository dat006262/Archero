




using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TipsUICtrl : MonoBehaviour
{
	private const float time = 1.5f;

	private static Vector3 endpos = new Vector3(0f, 100f, 0f);

	private Sequence seq;

	private Text text1;

	private CanvasGroup canvasgroup;

	private void Awake()
	{
		text1 = base.transform.Find("Image_BG/Text1").GetComponent<Text>();
		canvasgroup = GetComponent<CanvasGroup>();
	}

	public void Init(string value)
	{
		text1.text = value;
		Init();
	}

	public void Init(string value, Color color)
	{
		text1.color = color;
		Init(value);
	}

	public void InitNotAni(string value)
	{
		text1.text = value;
		if (seq != null)
		{
			seq.Kill();
		}
		seq = DOTween.Sequence();
		seq.SetUpdate(isIndependentUpdate: true);
		seq.AppendInterval(1f);
		seq.AppendCallback(delegate
		{
			CInstance<TipsUIManager>.Instance.Cache(base.gameObject);
		});
	}

	public void Init()
	{
		base.transform.localPosition = Vector3.zero;
		canvasgroup.alpha = 1f;
		if (seq != null)
		{
			seq.Kill();
		}
		Tweener t = base.transform.DOLocalMove(endpos, 1.5f).SetEase(Ease.OutQuint).SetUpdate(isIndependentUpdate: true);
		Sequence sequence = DOTween.Sequence();
		sequence.SetUpdate(isIndependentUpdate: true);
		sequence.Append(canvasgroup.DOFade(0.6f, 0.900000036f));
		sequence.Append(canvasgroup.DOFade(0f, 0.6f));
		base.transform.localScale = Vector3.one * 0.5f;
		Tweener t2 = base.transform.DOScale(1f, 0.450000018f);
		seq = DOTween.Sequence();
		seq.SetUpdate(isIndependentUpdate: true);
		seq.Append(t);
		seq.Join(sequence);
		seq.Join(t2);
		seq.AppendCallback(delegate
		{
			CInstance<TipsUIManager>.Instance.Cache(base.gameObject);
		});
	}
}
