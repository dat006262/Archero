




using Dxx.Util;
using UnityEngine;

public class Bullet5060 : BulletBase
{
	private bool bShow;

	private GameObject circle;

	private const float BombTime = 0.3f;

	private const float MaxColliderSize = 11f;

	private float mBombTime;

	private bool bColliderUpdate;

	protected override void AwakeInit()
	{
		Parabola_MaxHeight = 4f;
	}

	public override void SetTarget(EntityBase entity, int size = 1)
	{
		base.SetTarget(entity, size);
		if (PosFromStart2Target < 3f)
		{
			PosFromStart2Target = 3f;
		}
		mBombTime = 0f;
		bColliderUpdate = false;
		BoxEnable(enable: false);
		childMesh.gameObject.SetActive(value: true);
	}

	protected override void OnInit()
	{
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void overDistance()
	{
		if (!bColliderUpdate)
		{
			base.bMoveEnable = false;
			bColliderUpdate = true;
			base.m_Entity.PlayEffect(1005015, mTransform.position);
			mTransform.rotation = Quaternion.identity;
			childMesh.gameObject.SetActive(value: false);
			if ((bool)shadowGameObject)
			{
				shadowGameObject.SetActive(value: false);
			}
			BoxEnable(enable: true);
		}
	}

	protected override void UpdateProcess()
	{
		base.UpdateProcess();
		if (bColliderUpdate)
		{
			mBombTime += Updater.delta;
			if (boxListCount == 2)
			{
				boxList[0].size = new Vector3(mBombTime / 0.3f * 11f, 1f, 1f);
				boxList[1].size = new Vector3(1f, 1f, mBombTime / 0.3f * 11f);
			}
			if (mBombTime >= 0.3f)
			{
				base.overDistance();
			}
		}
	}
}
