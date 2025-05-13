




using DG.Tweening;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class CardLevelUpAttCtrl : MonoBehaviour
{
	[SerializeField]
	private Text Text_Name;

	[SerializeField]
	private Text Text_Before;

	[SerializeField]
	private Text Text_After;

	[SerializeField]
	private Image Image_Arrow;

	private LocalSave.CardOne mData;

	public void UpdateUI(LocalSave.CardOne data, int index)
	{
		mData = data;
		string typeName = mData.GetTypeName(index);
		Text_Name.text = typeName;
		if (mData.data.BaseAttributes[0].Contains("Global_HarvestLevel"))
		{
			Image_Arrow.gameObject.SetActive(value: false);
			Text_Before.text = string.Empty;
			Text_After.text = string.Empty;
		}
		else
		{
			Image_Arrow.gameObject.SetActive(value: true);
			string currentAttribute = mData.GetCurrentAttribute(index);
			string nextAttribute = mData.GetNextAttribute(index);
			Text_Before.text = Utils.FormatString("+{0}", currentAttribute);
			Text_After.text = Utils.FormatString("+{0}", nextAttribute);
		}
	}

	public Sequence GetTweener()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOScale(Vector3.one * 1.3f, 0.2f));
		sequence.Append(base.transform.DOScale(Vector3.one, 0.1f));
		return sequence;
	}
}
