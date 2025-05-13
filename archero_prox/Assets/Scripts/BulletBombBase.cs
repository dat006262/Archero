




using Dxx.Util;
using UnityEngine;

public class BulletBombBase : BulletBase
{
	[Header("延迟时间")]
	public float DelayTime = 1f;

	[Header("爆炸冲击时间")]
	public float BombTime = 0.5f;

	private float showCircleTime = 0.5f;

	private const float MaxColliderSize = 11f;

	private float mDelaytime;

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
		mDelaytime = DelayTime;
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

	protected override void UpdateProcess()
	{
		base.UpdateProcess();
		mDelaytime -= Updater.delta;
		if (mDelaytime <= 0f && !bColliderUpdate)
		{
			bColliderUpdate = true;
			mDelaytime = 0f;
			GameLogic.PlayEffect(1005015, mTransform.position);
			childMesh.gameObject.SetActive(value: false);
			if ((bool)shadowGameObject)
			{
				shadowGameObject.SetActive(value: false);
			}
			BoxEnable(enable: true);
		}
		if (bColliderUpdate)
		{
			mBombTime += Updater.delta;
			if (boxListCount == 2)
			{
				boxList[0].size = new Vector3(mBombTime / BombTime * 11f, 1f, 1f);
				boxList[1].size = new Vector3(1f, 1f, mBombTime / BombTime * 11f);
			}
			if (mBombTime >= BombTime)
			{
				overDistance();
			}
		}
	}
}
