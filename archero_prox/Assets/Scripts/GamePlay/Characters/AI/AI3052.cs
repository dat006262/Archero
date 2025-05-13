




using Dxx.Util;
using UnityEngine;

public class AI3052 : AIBase
{
	private ActionBattle action = new ActionBattle();

	private int bulletid;

	private float startangle;

	protected override void OnInit()
	{
		bulletid = m_Entity.m_Data.WeaponID;
		action.Init(m_Entity);
		AddAction(GetActionWait(string.Empty, 2000));
		AddAction(GetActionDelegate(delegate
		{
			CreateBullets();
		}));
		AddAction(GetActionWait(string.Empty, 2000));
	}

	protected override void OnAIDeInit()
	{
		action.DeInit();
	}

	private void CreateBullets()
	{
		CreateBulletsByStartAngle(startangle);
		CreateBulletsByStartAngle(startangle + 120f);
		CreateBulletsByStartAngle(startangle + 240f);
		startangle += 30f;
	}

	private void CreateBulletsByStartAngle(float angle)
	{
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			float num2 = Utils.GetBulletAngle(i, num, 60f) + angle;
			float x = MathDxx.Sin(num2);
			float z = MathDxx.Cos(num2);
			Vector3 b = new Vector3(x, 0f, z) * 1f;
			GameLogic.Release.Bullet.CreateBullet(m_Entity, bulletid, m_Entity.m_Body.LeftBullet.transform.position + b, num2);
		}
	}
}
