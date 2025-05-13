




using Dxx.Util;
using UnityEngine;

public class Goods2001 : GoodsBase
{
	private Vector3 startpos;

	private Vector3 endpos;

	private Vector3 dir;

	private float dis;

	private int state;

	private float speed = 3f;

	private Transform child;

	protected override void Init()
	{
		child = base.transform.Find("trapEntity");
	}

	protected override void AwakeInit()
	{
	}

	public void SetEndPosition(Vector3 endpos)
	{
		startpos = base.transform.localPosition;
		this.endpos = endpos;
		dis = (endpos - startpos).magnitude;
		dir = (endpos - startpos).normalized;
	}

	public override void ChildTriggerEnter(GameObject o)
	{
		if ((bool)GameLogic.Self && o == GameLogic.Self.gameObject && !GameLogic.Self.GetFlying())
		{
			long beforehit = -GameConfig.MapGood.GetTrapHit();
			GameLogic.SendHit_Trap(GameLogic.Self, beforehit);
		}
	}

	public override void ChildTriggetExit(GameObject o)
	{
	}

	protected override void UpdateProcess()
	{
		if (state == 0)
		{
			float num = (base.transform.localPosition - startpos).magnitude / dis;
			if (num < 1f)
			{
				base.transform.localPosition += dir * speed * Updater.delta;
				return;
			}
			state = 1;
			child.localRotation = Quaternion.Euler(0f, 180f, 0f);
		}
		else
		{
			float num2 = (base.transform.localPosition - endpos).magnitude / dis;
			if (num2 < 1f)
			{
				base.transform.localPosition += -dir * speed * Updater.delta;
				return;
			}
			state = 0;
			child.localRotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}
}
