




using Dxx.Util;
using UnityEngine;

public class Weapon1066 : Weapon1024
{
	private GameObject redline;

	private BulletRedLineCtrl ctrl;

	private float time;

	private float alltime = 0.5f;

	private float mindis;

	protected override void OnInstall()
	{
		AttackEffect = "WeaponHand1066Effect";
		redline = Object.Instantiate(ResourceManager.Load<GameObject>("Game/Bullet/Bullet1066_RedLine"));
		redline.SetParentNormal(m_Entity.m_Body.transform);
		ctrl = redline.GetComponent<BulletRedLineCtrl>();
		ctrl.SetLine(islast: true, 0f);
		time = 0f;
		Vector3 eulerAngles = m_Entity.eulerAngles;
		float x = MathDxx.Sin(eulerAngles.y + 90f);
		Vector3 eulerAngles2 = m_Entity.eulerAngles;
		Vector3 b = new Vector3(x, 0f, MathDxx.Cos(eulerAngles2.y + 90f)) * 0.5f;
		Vector3 startpos = m_Entity.m_Body.transform.position + b;
		Vector3 eulerAngles3 = m_Entity.eulerAngles;
		RayCastManager.CastMinDistance(startpos, eulerAngles3.y, fly: false, out float num);
		Vector3 startpos2 = m_Entity.m_Body.transform.position - b;
		Vector3 eulerAngles4 = m_Entity.eulerAngles;
		RayCastManager.CastMinDistance(startpos2, eulerAngles4.y, fly: false, out float num2);
		mindis = ((!(num < num2)) ? num2 : num);
		Updater.AddUpdate("Weapon1066", OnUpdate);
		base.OnInstall();
	}

	protected override void OnUnInstall()
	{
		DeinitRedLine();
		Updater.RemoveUpdate("Weapon1066", OnUpdate);
		base.OnUnInstall();
	}

	private void OnUpdate(float delta)
	{
		time += delta;
		time = MathDxx.Clamp(time, 0f, alltime);
		ctrl.SetLine(islast: true, mindis * (time / alltime));
	}

	private void DeinitRedLine()
	{
		if ((bool)redline)
		{
			UnityEngine.Object.Destroy(redline);
		}
	}

	protected override void OnAttack(object[] args)
	{
		DeinitRedLine();
		base.OnAttack(args);
	}
}
