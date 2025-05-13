




using Dxx.Util;
using TableTool;
using UnityEngine;

public class RoomGateCtrl : MonoBehaviour
{
	public SpriteRenderer gata1;

	private GameObject effect;

	public void OpenDoor(bool value)
	{
		if (!effect && (bool)gata1)
		{
			string path = Utils.FormatString("Game/Map/DoorEffect/dooreffect{0:D2}", LocalModelManager.Instance.Stage_Level_stagechapter.GetStyleID());
			effect = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(path));
			effect.SetParentNormal(gata1.transform);
			effect.SetActive(value: false);
		}
		if (value)
		{
			if (gata1 != null)
			{
				Sprite map = SpriteManager.GetMap("gateopen");
				if (map != null)
				{
					gata1.sprite = map;
				}
				else
				{
					gata1.sprite = null;
				}
			}
		}
		else if (gata1 != null)
		{
			Sprite map2 = SpriteManager.GetMap("gateclose");
			if (map2 != null)
			{
				gata1.sprite = map2;
			}
			else
			{
				gata1.sprite = null;
			}
		}
		if ((bool)effect)
		{
			effect.SetActive(value);
		}
	}
}
