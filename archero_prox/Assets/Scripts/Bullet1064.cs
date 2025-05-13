




using DG.Tweening;
using UnityEngine;

public class Bullet1064 : BulletBase
{
	private GameObject fire;

	protected override bool bFlyCantHit => true;

	protected override void AwakeInit()
	{
		Parabola_MaxHeight = 4f;
		fire = mTransform.Find("fire").gameObject;
	}

	public override void SetTarget(EntityBase entity, int size = 1)
	{
		base.SetTarget(entity, size);
		if (PosFromStart2Target < 3f)
		{
			PosFromStart2Target = 3f;
		}
	}

	protected override void OnInit()
	{
		BulletModelShow(value: true);
		fire.SetActive(value: false);
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void UpdateProcess()
	{
		base.UpdateProcess();
		if (base.childMesh != null)
		{
			Transform childMesh = base.childMesh;
			Vector3 localEulerAngles = base.childMesh.localEulerAngles;
			childMesh.localRotation = Quaternion.Euler(0f, 0f, localEulerAngles.z + 20f);
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		base.Speed += 0.1f;
	}

	protected override void ParabolaOver()
	{
		mTransform.rotation = Quaternion.identity;
		base.bMoveEnable = false;
		BulletModelShow(value: false);
		fire.SetActive(value: true);
		BoxEnable(enable: true);
		Sequence s = mSeqPool.Get();
		s.AppendInterval((float)m_Data.AliveTime - 0.4f).AppendCallback(delegate
		{
			BoxEnable(enable: false);
		});
		mCondition = AIMoveBase.GetConditionTime(m_Data.AliveTime);
	}
}
