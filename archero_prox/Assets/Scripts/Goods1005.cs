




using UnityEngine;

public class Goods1005 : GoodsBase
{
	private Goods1005EffectCtrl effect;

	protected override void Init()
	{
		base.Init();
		Transform transform = base.transform.Find("Goods1005");
		if ((bool)transform)
		{
			effect = transform.GetComponent<Goods1005EffectCtrl>();
		}
		if ((bool)effect)
		{
			effect.Event_TriggerEnter = Trigger;
		}
	}

	protected override void StartInit()
	{
		base.StartInit();
	}

	protected override void UpdateProcess()
	{
		if ((bool)effect)
		{
			Transform transform = effect.transform;
			Vector3 eulerAngles = effect.transform.eulerAngles;
			transform.rotation = Quaternion.Euler(0f, eulerAngles.y + 2f, 0f);
		}
	}

	private void Trigger(EntityBase entity)
	{
		GameLogic.SendHit_Trap(GameLogic.Self, -20L);
	}
}
