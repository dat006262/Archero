




using Dxx.Util;
using UnityEngine;

public class Bullet1031 : BulletBase
{
	private ConditionTime mCondition1;

	protected float playtime = 0.25f;

	protected float centerz = 11.8f;

	private float starttime;

	protected override void OnInit()
	{
		base.OnInit();
		starttime = 0f;
		mCondition1 = new ConditionTime
		{
			time = playtime
		};
		boxList[0].center = new Vector3(0f, 0f, 0f);
		BoxEnable(enable: true);
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void OnUpdate()
	{
		if (mCondition1 != null && mCondition1.IsEnd())
		{
			mCondition1 = null;
			overDistance();
		}
		starttime += Updater.delta;
		if (starttime > playtime)
		{
			starttime = playtime;
		}
		boxList[0].center = new Vector3(0f, 0f, starttime / playtime * centerz);
		if (starttime >= playtime)
		{
			BoxEnable(enable: false);
		}
	}
}
