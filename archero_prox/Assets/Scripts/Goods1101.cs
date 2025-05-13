




using UnityEngine;

public class Goods1101 : GoodsBase
{
	private Transform button;

	private float y_up = 0.128f;

	private float y_down = 0.07f;

	protected override void Init()
	{
		base.Init();
		button = base.transform.Find("button");
	}

	protected override void StartInit()
	{
		base.StartInit();
	}

	public override void ChildTriggerEnter(GameObject o)
	{
		if ((bool)GameLogic.Self && o == GameLogic.Self.gameObject)
		{
			Transform transform = button.transform;
			Vector3 localPosition = button.transform.localPosition;
			float x = localPosition.x;
			float y = y_down;
			Vector3 localPosition2 = button.transform.localPosition;
			transform.localPosition = new Vector3(x, y, localPosition2.z);
			if (GameLogic.Release.MapCreatorCtrl.Event_Button1101 != null)
			{
				GameLogic.Release.MapCreatorCtrl.Event_Button1101();
			}
		}
	}

	public override void ChildTriggetExit(GameObject o)
	{
		if (o == GameLogic.Self.gameObject)
		{
			Transform transform = button.transform;
			Vector3 localPosition = button.transform.localPosition;
			float x = localPosition.x;
			float y = y_up;
			Vector3 localPosition2 = button.transform.localPosition;
			transform.localPosition = new Vector3(x, y, localPosition2.z);
		}
	}
}
