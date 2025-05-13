




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoNeedCtrl : MonoBehaviour
{
	public Text Text_Title;

	public Transform itemparent;

	public Text Text_Info;

	private LocalSave.EquipOne mMaterialData;

	private EquipOneCtrl mMaterial;

	public void Init(LocalSave.EquipOne one)
	{
		LocalSave.EquipOne equipOne = mMaterialData = LocalSave.Instance.GetPropShowByID(one.data.UpgradeNeed);
		init_equip();
		int num = 0;
		if (equipOne != null)
		{
			num = equipOne.Count;
		}
		int needMatCount = one.NeedMatCount;
		Text_Info.text = Utils.FormatString("{0}: {1}/{2}", mMaterialData.NameString, num, needMatCount);
		if (num >= needMatCount)
		{
			Text_Info.text = Utils.FormatString("{0}: {1}/{2}", mMaterialData.NameString, num, needMatCount);
		}
		else
		{
			Text_Info.text = Utils.FormatString("{0}: <color=#ff0000ff>{1}/{2}</color>", mMaterialData.NameString, num, needMatCount);
		}
	}

	private void init_equip()
	{
		if (mMaterial == null)
		{
			mMaterial = CInstance<UIResourceCreator>.Instance.GetEquip(itemparent);
		}
		mMaterial.Init(mMaterialData);
		mMaterial.SetCountShow(value: false);
		mMaterial.ShowAniEnable(value: false);
	}

	public void OnLanguageChange()
	{
		Text_Title.text = "升级材料1";
	}
}
