




using UnityEngine;

public class MapGood1004 : MapGoodBase
{
	protected int elementid = 1;

	protected override void OnAwake()
	{
		DoGood();
		UnityEngine.Object.Destroy(this);
	}

	private void DoGood()
	{
		Transform transform = base.transform.Find("child/spike/spikes/sprite");
		if (!(transform != null))
		{
			return;
		}
		SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			Sprite map = SpriteManager.GetMap("element1201");
			if (map != null)
			{
				component.sprite = map;
			}
		}
	}
}
