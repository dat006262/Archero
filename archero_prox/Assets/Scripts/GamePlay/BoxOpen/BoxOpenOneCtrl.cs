




using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenOneCtrl : MonoBehaviour
{
	[NonSerialized]
	public RectTransform mRectTransform;

	public GameObject child;

	public Image Image_BG;

	public Image Image_Icon;

	public Text Text_Value;

	private void Awake()
	{
		mRectTransform = (base.transform as RectTransform);
	}

	public void Init(int equipid)
	{
		Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById(equipid);
		Image_Icon.sprite = SpriteManager.GetEquip(beanById.EquipIcon);
		Text_Value.text = equipid.ToString();
	}
}
