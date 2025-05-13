




using Dxx.Util;
using TableTool;
using UnityEngine;

public class Weapon1082 : WeaponBase
{
	protected float distance = 4f;

	protected float delaytime = 0.4f;

	protected float updatetime = 0.2f;

	private bool bStart;

	private float starttime;

	private float percent;

	private float x;

	private float y;

	private Vector3 startpos;

	private Vector3 endpos;

	private float percentbefore;

	private float percentchange;

	private AnimationCurve curve;

	protected override void OnInstall()
	{
		if (m_Entity.IsElite)
		{
			distance = 5f;
		}
		Updater.AddUpdate("Weapon1082", OnUpdate);
		OnAttackStartStartAction = OnAttackStartStart;
		OnAttackStartEndAction = OnAttackStartEnd;
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100025);
		base.OnInstall();
	}

	protected override void OnUnInstall()
	{
		Updater.RemoveUpdate("Weapon1082", OnUpdate);
		OnAttackStartStartAction = null;
		OnAttackStartEndAction = null;
		base.OnUnInstall();
	}

	private void OnAttackStartStart()
	{
		bStart = true;
		percentbefore = 0f;
		starttime = Updater.AliveTime + delaytime;
		Vector3 eulerAngles = m_Entity.eulerAngles;
		x = MathDxx.Sin(eulerAngles.y);
		Vector3 eulerAngles2 = m_Entity.eulerAngles;
		y = MathDxx.Cos(eulerAngles2.y);
		startpos = m_Entity.position;
		endpos = new Vector3(x, 0f, y).normalized * distance + m_Entity.position;
	}

	private void OnAttackStartEnd()
	{
		bStart = false;
	}

	private void OnUpdate(float delta)
	{
		if (bStart && Updater.AliveTime >= starttime)
		{
			if (percentbefore == 0f)
			{
				GameLogic.Hold.Sound.PlayBattleSpecial(5100004, m_Entity.position);
			}
			percent = (Updater.AliveTime - starttime) / updatetime;
			percent = MathDxx.Clamp01(percent);
			percentchange = percent - percentbefore;
			percentbefore = percent;
			m_Entity.SetPositionBy((endpos - startpos) * percentchange);
			EntityBase entity = m_Entity;
			Vector3 position = m_Entity.position;
			float num = position.x;
			float num2 = curve.Evaluate(percent) * 3f;
			Vector3 position2 = m_Entity.position;
			entity.SetPosition(new Vector3(num, num2, position2.z));
			if (percent == 1f)
			{
				bStart = false;
			}
		}
	}
}
