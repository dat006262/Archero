




using DG.Tweening;
using Dxx.Util;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EquipCombineAttCtrl : MonoBehaviour
{
	[SerializeField]
	private Text Text_Name;

	[SerializeField]
	private Text Text_Before;

	[SerializeField]
	private Text Text_After;

	public GameObject baseatt;

	public GameObject skills;

	public Text Text_Down;

	private int type;

	private LocalSave.EquipOne mAfter;

	private LocalSave.EquipOne mBefore;

	public void UpdateUI(LocalSave.EquipOne mBefore, LocalSave.EquipOne mAfter, int index)
	{
		this.mAfter = mAfter;
		this.mBefore = mBefore;
		baseatt.SetActive(value: false);
		skills.SetActive(value: false);
		if (index < mBefore.data.Attributes.Length)
		{
			type = 0;
			baseatt.SetActive(value: true);
			string attName = mAfter.GetAttName(index);
			string currentAttributeString = mBefore.GetCurrentAttributeString(index);
			string currentAttributeString2 = mAfter.GetCurrentAttributeString(index);
			Text_Name.text = attName;
			Text_Before.text = Utils.FormatString("{0}", currentAttributeString);
			Text_After.text = Utils.FormatString("{0}", currentAttributeString2);
		}
		else if (mAfter.data.AdditionSkills.Length > mBefore.data.AdditionSkills.Length)
		{
			skills.SetActive(value: true);
			type = 1;
			Text_Before.text = string.Empty;
			Text_After.text = string.Empty;
			string text = mAfter.data.AdditionSkills[mAfter.data.AdditionSkills.Length - 1];
			Text_Name.text = GameLogic.Hold.Language.GetLanguageByTID("equip_combine_unlock_newatt");
			if (!int.TryParse(text, out int result))
			{
				Goods_goods.GoodShowData goodShowData = Goods_goods.GetGoodShowData(text);
				Text_Down.text = goodShowData.ToString();
			}
			else
			{
				string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("技能描述{0}", result));
				Text_Down.text = languageByTID;
			}
		}
	}

	public void UpdateMaxLevel(LocalSave.EquipOne mBefore, LocalSave.EquipOne mAfter)
	{
		type = 0;
		baseatt.SetActive(value: true);
		skills.SetActive(value: false);
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_MaxLevel");
		Text_Name.text = languageByTID;
		Text_Before.text = Utils.FormatString("{0}", mBefore.CurrentMaxLevel);
		Text_After.text = Utils.FormatString("{0}", mAfter.CurrentMaxLevel);
	}

	public float GetHeight()
	{
		if (type == 0)
		{
			return 100f;
		}
		return 80f;
	}

	public Sequence GetTweener()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOScale(Vector3.one * 1.3f, 0.2f));
		sequence.Append(base.transform.DOScale(Vector3.one, 0.1f));
		return sequence;
	}
}
