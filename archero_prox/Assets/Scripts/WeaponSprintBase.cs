




using Dxx.Util;
using UnityEngine;

public class WeaponSprintBase : WeaponBase
{
	protected float distance = 2f;

	protected float delaytime = 0.4f;

	protected float updatetime = 0.1f;

	private bool bStart;

	private float starttime;

	private float percent;

	private float x;

	private float y;

	private Vector3 startpos;

	private Vector3 endpos;

	private Vector3 currentmove;

	private float percentbefore;

	private float percentchange;

	protected override void OnInstall()
	{
		Updater.AddUpdate("WeaponSprintBase", OnUpdate);
		OnAttackStartStartAction = OnAttackStartStart;
		OnAttackStartEndAction = OnAttackStartEnd;
		base.OnInstall();
	}

	protected override void OnUnInstall()
	{
		Updater.RemoveUpdate("WeaponSprintBase", OnUpdate);
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
			percent = (Updater.AliveTime - starttime) / updatetime;
			percent = MathDxx.Clamp01(percent);
			percentchange = percent - percentbefore;
			percentbefore = percent;
			currentmove = (endpos - startpos) * percentchange;
			m_Entity.SetPositionBy(currentmove);
			OnUpdateMove(currentmove.magnitude);
			if (percent == 1f)
			{
				bStart = false;
			}
		}
	}

	protected virtual void OnUpdateMove(float currentdis)
	{
	}
}
