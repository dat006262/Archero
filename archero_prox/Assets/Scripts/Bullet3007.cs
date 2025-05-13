




using Dxx.Util;
using UnityEngine;

public class Bullet3007 : BulletBase
{
	private new ConditionTime mCondition;

	private float playtime = 0.5f;

	private float starttime;

	private float centerz = 8f;

	protected override void OnInit()
	{
		base.OnInit();
		starttime = 0f;
		mCondition = new ConditionTime
		{
			time = 0.6f
		};
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void OnUpdate()
	{
		if (mCondition != null && mCondition.IsEnd())
		{
			mCondition = null;
			overDistance();
		}
		starttime += Updater.delta;
		if (starttime > playtime)
		{
			starttime = playtime;
		}
		boxList[0].center = new Vector3(0f, 0f, starttime / playtime * centerz);
	}
}
