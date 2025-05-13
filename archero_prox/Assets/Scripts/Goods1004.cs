




using Dxx.Util;
using UnityEngine;

public class Goods1004 : GoodsBase
{
	private Animator ani;

	private const string ShowAni = "spike_trap_show";

	private const string HideAni = "spike_trap_hide";

	private const string ShowPara = "Show";

	protected bool bStartTrap;

	protected float trapTime;

	protected int framecount;

	protected override void Init()
	{
		ani = GetComponent<Animator>();
		GetComponentInChildren<GoodsColliderBase>().SetGoods(this);
	}

	protected override void AwakeInit()
	{
	}

	public override void ChildTriggerEnter(GameObject o)
	{
		if ((bool)GameLogic.Self && o == GameLogic.Self.gameObject && !GameLogic.Self.GetFlying() && (bool)GameLogic.Self && !GameLogic.Self.GetIsDead() && GameLogic.Self.m_EntityData.GetCanTrapHit() && (Updater.AliveTime - trapTime > 1f || Time.frameCount - framecount > 1))
		{
			trapTime = Updater.AliveTime;
			long beforehit = -GameConfig.MapGood.GetTrapHit();
			GameLogic.SendHit_Trap(GameLogic.Self, beforehit);
		}
	}

	protected override void UpdateProcess()
	{
		framecount = Time.frameCount;
	}
}
