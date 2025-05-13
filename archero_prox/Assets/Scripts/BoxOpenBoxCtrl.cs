




using DG.Tweening;
using UnityEngine;

public class BoxOpenBoxCtrl : MonoBehaviour
{
	public BoxOpenBoxAniCtrl mBoxCtrl;

	public BoxOpenScrollCtrl mScrollCtrl;

	private Sequence seq;

	public void Init()
	{
		mBoxCtrl.Init();
		mScrollCtrl.Init();
	}

	public Sequence PlayScrollShow(bool value)
	{
		if (mScrollCtrl == null)
		{
			return null;
		}
		seq = DOTween.Sequence();
		if (value)
		{
			mScrollCtrl.transform.localScale = Vector3.zero;
			seq.Append(mScrollCtrl.transform.DOScale(1f, 0.3f));
		}
		else
		{
			mScrollCtrl.transform.localScale = Vector3.zero;
		}
		return seq;
	}
}
