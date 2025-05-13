




using Dxx.Util;
using UnityEngine;

public class AIMove1057 : AIMove1009
{
	private int mTimeID;

	private float angle;

	protected override float moveRatio => 24f;

	public AIMove1057(EntityBase entity)
		: base(entity)
	{
		name = "AIMove1057";
		runString = "CastSpell";
		Move_BackTime = 0.8f;
		Move_NextDurationTime = 1.6f;
		runAniSpeed = 0.55f;
	}

	protected override void OnInitBase()
	{
		base.OnInitBase();
	}

	protected override void OnBackEvent()
	{
		mTimeID = TimeRegister.Register("AIMove1057", 0.2f, CreateBullets);
		Vector3 eulerAngles = m_Entity.eulerAngles;
		angle = eulerAngles.y;
	}

	protected override void OnEnd()
	{
		base.OnEnd();
		TimeRegister.UnRegister(mTimeID);
	}

	private void CreateBullets()
	{
		angle += 44f;
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 5061, m_Entity.m_Body.LeftBullet.transform.position, angle);
	}
}
