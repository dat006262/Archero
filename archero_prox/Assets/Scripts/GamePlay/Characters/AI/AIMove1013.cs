




using Dxx.Util;
using UnityEngine;

public class AIMove1013 : AIMoveBase
{
	private EntityBase target;

	private Vector3 dir;

	private float g = 6f;

	private float endx;

	private float endz;

	private float perendx;

	private float perendz;

	private float delaytime;

	private float starttime;

	private float alltime = 1f;

	private float halftime;

	private Vector3 startpos;

	private bool bPlaySkill;

	public AIMove1013(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		bPlaySkill = false;
		delaytime = 0.3f;
		starttime = 0f;
		halftime = alltime / 2f;
		startpos = m_Entity.position;
		target = GameLogic.Self;
		GameLogic.Release.MapCreatorCtrl.RandomItem(m_Entity, 2, out endx, out endz);
		float num = endx;
		Vector3 position = m_Entity.position;
		perendx = num - position.x;
		float num2 = endz;
		Vector3 position2 = m_Entity.position;
		perendz = num2 - position2.z;
		m_Entity.m_AttackCtrl.RotateHero(180f);
		m_Entity.m_AniCtrl.SendEvent("Skill");
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		starttime += Updater.delta;
		if (starttime >= delaytime)
		{
			OnFly();
		}
	}

	private void OnFly()
	{
		if (!bPlaySkill)
		{
			bPlaySkill = true;
			m_Entity.m_AniCtrl.SendEvent("Call", force: true);
			GameLogic.Hold.Sound.PlayMonsterSkill(5100002, m_Entity.position);
		}
		float num = 1f - (delaytime + alltime - starttime) / alltime;
		float num2 = -4f * g * (num - 0.5f) * (num - 0.5f) + g;
		if (starttime < delaytime + halftime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num, startpos.y + num2, startpos.z + perendz * num));
			return;
		}
		if (starttime < delaytime + alltime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num, startpos.y + num2, startpos.z + perendz * num));
			return;
		}
		EntityBase entity = m_Entity;
		Vector3 position = m_Entity.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		entity.SetPosition(new Vector3(x, 0f, position2.z));
		if (m_Entity.m_Data.WeaponID != 0)
		{
			m_Entity.ChangeWeapon(m_Entity.m_Data.WeaponID);
		}
		else
		{
			SdkManager.Bugly_Report("AIMove1013.cs", Utils.FormatString("AIMove1013 {0} weapon is 0", m_Entity.m_Data.WeaponID));
		}
		if (m_Entity.m_Weapon != null)
		{
			m_Entity.m_Weapon.Attack();
		}
		End();
	}

	private void UpdateDirection()
	{
		m_MoveData.angle = Utils.getAngle(dir.x, dir.z);
		m_MoveData.direction = dir;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}
}
