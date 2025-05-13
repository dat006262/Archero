




using Dxx.Util;
using UnityEngine;

public class AIMove1047 : AIMove1009
{
	private int mTimeID;

	private float angle;

	protected override float moveRatio => 24f;

	public AIMove1047(EntityBase entity)
		: base(entity)
	{
		name = "AIMove1047";
		runString = "CastSpell";
		Move_BackTime = 0.8f;
		Move_NextDurationTime = 1.6f;
		runAniSpeed = 0.55f;
	}

	protected override void OnInitBase()
	{
		base.OnInitBase();
		m_Entity.SetSuperArmor(value: true);
	}

	protected override void OnBackEvent()
	{
		mTimeID = TimeRegister.Register("AIMove1047", 0.05f, CreateBullets);
		Vector3 eulerAngles = m_Entity.eulerAngles;
		angle = eulerAngles.y;
	}

	protected override void OnEnd()
	{
		base.OnEnd();
		TimeRegister.UnRegister(mTimeID);
		m_Entity.SetSuperArmor(value: false);
	}

	private void CreateBullets()
	{
		angle += 22f;
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 5061, m_Entity.m_Body.LeftBullet.transform.position, angle);
	}
}
