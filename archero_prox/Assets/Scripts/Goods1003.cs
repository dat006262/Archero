




using Dxx.Util;
using UnityEngine;

public class Goods1003 : GoodsBase
{
	protected bool bStartTrap;

	protected float trapTime;

	private float delaytime = 4f;

	private float firetime = 2f;

	private float currenttime;

	private bool bStartFire;

	private float firecurrenttime;

	private int state;

	private BoxCollider box;

	protected override void Init()
	{
		base.Init();
		box = GetComponent<BoxCollider>();
	}

	public override void ChildTriggerEnter(GameObject o)
	{
		if ((bool)GameLogic.Self && o == GameLogic.Self.gameObject)
		{
			GameLogic.SendHit_Trap(GameLogic.Self, -20L);
		}
	}

	protected override void UpdateProcess()
	{
		currenttime += Updater.delta;
		if (currenttime >= delaytime)
		{
			currenttime -= delaytime;
			bStartFire = true;
			BoxShow(show: true);
			state = 1;
			CreateEffect();
		}
		if (bStartFire)
		{
			firecurrenttime += Updater.delta;
			if (firecurrenttime >= firetime)
			{
				firecurrenttime = 0f;
				state = 0;
				bStartFire = false;
				BoxShow(show: false);
			}
		}
	}

	private void CreateEffect()
	{
		GameObject gameObject = GameLogic.Release.MapEffect.Get("Effect/Goods/Goods1003");
		gameObject.transform.position = base.transform.position;
	}

	private void BoxShow(bool show)
	{
		BoxCollider boxCollider = box;
		Vector3 center = box.center;
		float x = center.x;
		Vector3 center2 = box.center;
		boxCollider.center = new Vector3(x, center2.y, (!show) ? 10000 : 0);
	}
}
