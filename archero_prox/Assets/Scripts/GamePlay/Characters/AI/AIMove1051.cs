




using Dxx.Util;
using UnityEngine;

public class AIMove1051 : AIMoveBase
{
	private EntityBase target;

	private float redshowtime;

	private float delaytime;

	private float updatetime;

	private RedLineCtrl mLineCtrl;

	private GameObject effect;

	private bool bCreateBullet;

	public AIMove1051(EntityBase entity, float redshowtime, float delaytime)
		: base(entity)
	{
		name = "AIMove1051";
		this.redshowtime = redshowtime;
		this.delaytime = delaytime;
	}

	protected override void OnInitBase()
	{
		bCreateBullet = false;
		target = GameLogic.FindTarget(m_Entity);
		updatetime = 0f;
		effect = GameLogic.EffectGet(Utils.FormatString("Game/WeaponHand/WeaponHand{0}Effect", m_Entity.m_Data.WeaponID));
		effect.SetParentNormal(m_Entity.m_Body.LeftWeapon);
	}

	protected override void OnUpdate()
	{
		if (!target || !m_Entity)
		{
			return;
		}
		updatetime += Updater.delta;
		if (updatetime < redshowtime)
		{
			m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(target.position - m_Entity.position));
			if (updatetime > 0.2f)
			{
				if (mLineCtrl == null)
				{
					mLineCtrl = new RedLineCtrl();
					mLineCtrl.Init(m_Entity, throughwall: true, 0, 0f);
				}
				if (mLineCtrl != null)
				{
					mLineCtrl.Update();
				}
			}
		}
		else if (updatetime > redshowtime + delaytime)
		{
			if (!bCreateBullet)
			{
				bCreateBullet = true;
				EffectCache();
				LineCache();
				BulletManager bullet = GameLogic.Release.Bullet;
				EntityBase entity = m_Entity;
				Vector3 position = m_Entity.m_Body.LeftBullet.transform.position;
				Vector3 eulerAngles = m_Entity.eulerAngles;
				bullet.CreateBullet(entity, 1074, position, eulerAngles.y);
			}
			if (updatetime > redshowtime + delaytime + 0.4f)
			{
				End();
			}
		}
	}

	private void EffectCache()
	{
		if (effect != null)
		{
			GameLogic.EffectCache(effect);
			effect = null;
		}
	}

	private void LineCache()
	{
		if (mLineCtrl != null)
		{
			mLineCtrl.DeInit();
			mLineCtrl = null;
		}
	}

	protected override void OnEnd()
	{
		EffectCache();
		LineCache();
	}
}
