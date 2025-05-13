




using Dxx.Util;
using UnityEngine;

public class GoodsCreate1102 : GoodsCreateBase
{
	private float speed = 15f;

	private float currentdis;

	private float maxdis = 100f;

	private float movex;

	private float movey;

	private Vector3 move;

	protected override void OnAwake()
	{
	}

	protected override void OnStart()
	{
	}

	protected override void OnInit()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		movex = MathDxx.Sin(eulerAngles.y);
		Vector3 eulerAngles2 = base.transform.eulerAngles;
		movey = MathDxx.Cos(eulerAngles2.y);
		move = new Vector3(movex, 0f, movey);
		currentdis = 0f;
	}

	protected override void OnUpdate()
	{
		float num = speed * Updater.delta;
		currentdis += num;
		base.transform.position += move * num;
		if (currentdis >= maxdis)
		{
			Caches();
		}
	}

	protected override void TriggerEnter(Collider o)
	{
		if (o.gameObject.layer == LayerManager.MapOutWall || o.gameObject.layer == LayerManager.Stone)
		{
			Caches();
		}
		else if (o.gameObject.layer == LayerManager.Player && o.gameObject == GameLogic.Self.gameObject)
		{
			Caches();
		}
	}

	private void Caches()
	{
		GameLogic.Self.PlayEffect(1001004, base.transform.position);
		Cache();
	}
}
