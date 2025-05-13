




using UnityEngine;

public class MapGood1001 : MapGoodBase
{
	protected int elementid = 1;

	protected override void OnAwake()
	{
		DoGood();
		DoShadow();
		UnityEngine.Object.Destroy(this);
	}

	private void DoGood()
	{
		Transform transform = base.transform.Find("child/good");
		if (!(transform != null))
		{
			return;
		}
		SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			Sprite randomElement = GameLogic.Release.MapCreatorCtrl.GetRandomElement(elementid);
			if (randomElement != null)
			{
				component.sprite = randomElement;
			}
		}
	}

	private void DoShadow()
	{
		Transform transform = base.transform.Find("child/shadow");
		if (!transform)
		{
			return;
		}
		SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			Sprite elementShadow = GameLogic.Release.MapCreatorCtrl.GetElementShadow(elementid);
			if (elementShadow != null)
			{
				component.sprite = elementShadow;
			}
		}
	}
}
