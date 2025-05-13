




using Dxx.Util;
using UnityEngine;

public class BulletBombDodge : BulletBase
{
	[Header("延迟时间")]
	public float DelayTime = 1f;

	[Header("爆炸冲击时间")]
	public float BombTime = 0.5f;

	private Transform effect;

	private Vector3 shadowScaleInit = Vector3.one * -1f;

	private float height;

	private Vector3 endpos;

	private Vector3 dir;

	private bool bStartBomb;

	private float showCircleTime = 0.5f;

	private const float MaxColliderSize = 20f;

	private float mDelaytime;

	private float mBombTime;

	private bool bColliderUpdate;

	private float addspeed;

	protected override void AwakeInit()
	{
		Parabola_MaxHeight = 4f;
		effect = base.transform.Find("effect");
	}

	public void SetEndPos(Vector3 endpos)
	{
		this.endpos = endpos;
	}

	protected override void OnInit()
	{
		base.OnInit();
		if (shadowScaleInit.x < 0f && (bool)shadow)
		{
			shadowScaleInit = shadow.localScale;
		}
		Vector3 position = base.transform.position;
		height = position.y;
		mDelaytime = DelayTime;
		mBombTime = 0f;
		bColliderUpdate = false;
		BoxEnable(enable: false);
		childMesh.gameObject.SetActive(value: true);
		bStartBomb = false;
		addspeed = 1f;
		SetEffectScale(0f);
		SetEffectShow(value: false);
		SpriteRenderer component = mBulletModel.Find("shadow/child/shadow").GetComponent<SpriteRenderer>();
		component.sortingLayerName = "Default";
		component.sortingOrder = 1;
		SpriteRenderer component2 = mBulletModel.Find("child/bomb/sprite").GetComponent<SpriteRenderer>();
		component2.sortingLayerName = "Default";
		component2.sortingOrder = 1;
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	private void SetEffectScale(float value)
	{
		if ((bool)effect)
		{
			effect.localScale = new Vector3(value, 1f, value);
		}
	}

	private void SetEffectShow(bool value)
	{
		if ((bool)effect)
		{
			effect.gameObject.SetActive(value);
		}
	}

	protected override void OnUpdate()
	{
		if (!bStartBomb)
		{
			dir = (mTransform.position - endpos).normalized;
			addspeed *= 1.05f;
			mTransform.position = Vector3.MoveTowards(mTransform.position, endpos, base.FrameDistance * addspeed);
			if ((mTransform.position - endpos).magnitude < 0.1f)
			{
				bStartBomb = true;
			}
			Vector3 position = base.transform.position;
			float num = position.y / height;
			if ((bool)shadow)
			{
				shadow.localScale = shadowScaleInit * (1f - num) * 0.7f + shadowScaleInit * 0.3f;
			}
		}
	}

	private void create_divide()
	{
		int num = 4;
		float num2 = 360f / (float)num;
		for (int i = 0; i < num; i++)
		{
			BulletManager bullet = GameLogic.Release.Bullet;
			EntityBase entity = base.m_Entity;
			Vector3 position = mTransform.position;
			float x = position.x;
			Vector3 position2 = mTransform.position;
			BulletBase bulletBase = bullet.CreateBulletInternal(entity, 3035, new Vector3(x, 0f, position2.z), (float)i * num2, clear: true);
			bulletBase.SetTarget(null);
			bulletBase.mBulletTransmit.SetAttack(mBulletTransmit.GetAttack());
		}
	}

	protected override void UpdateProcess()
	{
		base.UpdateProcess();
		if (!bStartBomb)
		{
			return;
		}
		mDelaytime -= Updater.delta;
		if (mDelaytime <= 0f && !bColliderUpdate)
		{
			bColliderUpdate = true;
			mDelaytime = 0f;
			childMesh.gameObject.SetActive(value: false);
			if ((bool)shadowGameObject)
			{
				shadowGameObject.SetActive(value: false);
			}
			SetEffectShow(value: true);
			GameLogic.Hold.Sound.PlayBulletDead(2301003, base.transform.position);
			create_divide();
			overDistance();
		}
		if (bColliderUpdate)
		{
			mBombTime += Updater.delta;
			SetEffectScale(mBombTime / BombTime * 2f);
			if (boxListCount == 2)
			{
				BoxCollider obj = boxList[0];
				float x = mBombTime / BombTime * 20f;
				Vector3 size = boxList[0].size;
				obj.size = new Vector3(x, 1f, size.z);
				BoxCollider obj2 = boxList[1];
				Vector3 size2 = boxList[1].size;
				obj2.size = new Vector3(size2.x, 1f, mBombTime / BombTime * 20f);
			}
			if (mBombTime >= 1f)
			{
				overDistance();
			}
			else if (mBombTime >= BombTime)
			{
				BoxEnable(enable: false);
			}
		}
	}
}
