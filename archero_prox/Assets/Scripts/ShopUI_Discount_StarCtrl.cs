




using DG.Tweening;
using UnityEngine;

public class ShopUI_Discount_StarCtrl : MonoBehaviour
{
	public float delaytime_min;

	public float delaytime_max;

	private Sequence seq;

	private float delaytime;

	private void Start()
	{
		base.transform.localScale = Vector3.zero;
		if (delaytime_min < 0f)
		{
			delaytime_min = 0f;
		}
		if (delaytime_max < delaytime_min)
		{
			delaytime_max = delaytime_min;
		}
		delaytime = GameLogic.Random(delaytime_min, delaytime_max);
	}

	private void Update()
	{
		if (seq == null)
		{
			delaytime -= Time.deltaTime;
			if (delaytime <= 0f)
			{
				seq = DOTween.Sequence();
				float duration = 1f;
				seq.Append(base.transform.DOScale(1f, duration).SetEase(Ease.Linear));
				seq.Join(base.transform.DORotate(new Vector3(0f, 0f, 180f), duration).SetEase(Ease.Linear));
				seq.Append(base.transform.DOScale(0f, duration).SetEase(Ease.Linear));
				seq.Join(base.transform.DORotate(new Vector3(0f, 0f, 360f), duration).SetEase(Ease.Linear));
				seq.AppendInterval(0.33f);
				seq.SetLoops(-1);
			}
		}
	}

	private void OnDestroy()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}
}
