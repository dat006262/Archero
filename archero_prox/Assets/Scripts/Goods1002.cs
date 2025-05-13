




using Dxx.Util;
using UnityEngine;

public class Goods1002 : GoodsBase
{
	protected bool bStartTrap;

	protected float trapTime;

	public override void ChildTriggerEnter(GameObject o)
	{
		if ((bool)GameLogic.Self && o == GameLogic.Self.gameObject && !GameLogic.Self.GetFlying())
		{
			bStartTrap = true;
			trapTime = -1f;
		}
	}

	public override void ChildTriggetExit(GameObject o)
	{
		if (o == GameLogic.Self.gameObject)
		{
			bStartTrap = false;
		}
	}

	protected override void UpdateProcess()
	{
		if (bStartTrap && (bool)GameLogic.Self && !GameLogic.Self.GetIsDead() && Updater.AliveTime - trapTime > 0.1f)
		{
			trapTime = Updater.AliveTime;
			GameLogic.SendBuff(GameLogic.Self, 1020);
		}
	}
}
