




using UnityEngine;

public class UIResourceCreator : CInstance<UIResourceCreator>
{
	private GameObject get_gameobject(string path)
	{
		return Object.Instantiate(ResourceManager.Load<GameObject>(path));
	}

	public T Get<T>(string path) where T : Component
	{
		GameObject gameObject = get_gameobject(path);
		return gameObject.GetComponent<T>();
	}

	public EquipOneCtrl GetEquip(Transform parent = null)
	{
		EquipOneCtrl equipOneCtrl = Get<EquipOneCtrl>("UIPanel/CharUI/EquipOne");
		if (parent != null)
		{
			equipOneCtrl.transform.SetParentNormal(parent);
		}
		return equipOneCtrl;
	}

	public PropOneEquip GetPropOneEquip(Transform parent = null)
	{
		PropOneEquip propOneEquip = Get<PropOneEquip>("UIPanel/CharUI/EquipPropOne");
		if (parent != null)
		{
			propOneEquip.transform.SetParentNormal(parent);
		}
		return propOneEquip;
	}

	public BlackItemOnectrl GetBlackShopOne(Transform parent = null)
	{
		BlackItemOnectrl blackItemOnectrl = Get<BlackItemOnectrl>("UIPanel/CharUI/BlackItemOne");
		if (parent != null)
		{
			blackItemOnectrl.transform.SetParentNormal(parent);
		}
		return blackItemOnectrl;
	}

	public GuideNoMaskCtrl GetGuideNoMask(Transform parent = null)
	{
		GuideNoMaskCtrl guideNoMaskCtrl = Get<GuideNoMaskCtrl>("UIPanel/GuideUI/guide_nomask");
		if (parent != null)
		{
			guideNoMaskCtrl.transform.SetParentNormal(parent);
		}
		return guideNoMaskCtrl;
	}

	public EquipWearCtrl GetEquipWear(Transform parent = null)
	{
		EquipWearCtrl equipWearCtrl = Get<EquipWearCtrl>("UIPanel/CharUI/equipwear");
		if (parent != null)
		{
			equipWearCtrl.transform.SetParentNormal(parent);
		}
		return equipWearCtrl;
	}

	public UILineCtrlOne GetUILineOne(Transform parent = null)
	{
		UILineCtrlOne uILineCtrlOne = Get<UILineCtrlOne>("UIPanel/ACommon/UILineOne");
		if (parent != null)
		{
			uILineCtrlOne.transform.SetParentNormal(parent);
		}
		return uILineCtrlOne;
	}

	public GameObject GetEquipOne_UP(Transform parent = null)
	{
		GameObject gameObject = get_gameobject("UIPanel/CharUI/Equip_Up");
		if (parent != null)
		{
			gameObject.SetParentNormal(parent);
		}
		return gameObject;
	}
}
