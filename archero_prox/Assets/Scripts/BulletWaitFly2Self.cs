




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class BulletWaitFly2Self : BulletBase
{
	private bool bStart;

	[Header("等待时间")]
	public float waitTime = 0.5f;

	protected override void OnInit()
	{
		base.OnInit();
		bStart = false;
		Sequence s = mSeqPool.Get();
		s.AppendInterval(waitTime).AppendCallback(delegate
		{
			bStart = true;
		});
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void UpdateProcess()
	{
		if (!bStart)
		{
			return;
		}
		base.UpdateProcess();
		if (!base.m_Entity)
		{
			overDistance();
			return;
		}
		bulletAngle = Utils.getAngle(base.m_Entity.position - base.transform.position);
		UpdateMoveDirection();
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = base.transform.position;
		Vector3 a = new Vector3(x, 0f, position2.z);
		Vector3 position3 = base.m_Entity.position;
		float x2 = position3.x;
		Vector3 position4 = base.m_Entity.position;
		if (Vector3.Distance(a, new Vector3(x2, 0f, position4.z)) < 0.5f)
		{
			overDistance();
		}
	}
}
