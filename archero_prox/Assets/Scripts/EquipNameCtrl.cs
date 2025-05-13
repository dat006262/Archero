




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class EquipNameCtrl : MonoBehaviour
{
	public GameObject child;

	public Image Image_BG;

	public Text Text_Name;

	private FoodEquipBase equipitem;

	private void LateUpdate()
	{
		if ((bool)equipitem)
		{
			Vector3 vector = Utils.World2Screen(equipitem.transform.position);
			float x = vector.x;
			float y = vector.y + 70f * GameLogic.HeightScale;
			base.transform.position = new Vector3(x, y, 0f);
		}
	}

	public void Init(FoodEquipBase equip)
	{
		equipitem = equip;
		if (equip.equipone.Overlying)
		{
			Text_Name.text = Utils.FormatString("{0}x{1}", equipitem.equipone.NameString, equip.equipone.Count);
		}
		else
		{
			Text_Name.text = equipitem.equipone.NameString;
		}
		Text_Name.color = LocalSave.QualityColors[equipitem.equipone.Quality];
		RectTransform rectTransform = Image_BG.rectTransform;
		float x = Text_Name.preferredWidth + 20f;
		Vector2 sizeDelta = Image_BG.rectTransform.sizeDelta;
		rectTransform.sizeDelta = new Vector2(x, sizeDelta.y);
	}
}
