




using DG.Tweening;
using Dxx.Util;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class CardLevelUpAtt2Ctrl : MonoBehaviour
{
	[SerializeField]
	private Text Text_Value;

	private LocalSave.CardOne mData;

	public void UpdateUI(LocalSave.CardOne data, int index)
	{
		mData = data;
		string typeName = mData.GetTypeName(index);
		if (mData.data.BaseAttributes[0].Contains("Global_HarvestLevel"))
		{
			Text_Value.text = typeName;
			return;
		}
		string currentAttribute = mData.GetCurrentAttribute(index);
		Goods_goods.GoodData goodData = Goods_goods.GetGoodData(mData.data.BaseAttributes[index]);
		string text = (goodData.value <= 0) ? "-" : "+";
		Text_Value.text = Utils.FormatString("{0} {1} {2}", typeName, text, currentAttribute);
	}

	public Sequence GetTweener()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOScale(Vector3.one * 1.3f, 0.2f));
		sequence.Append(base.transform.DOScale(Vector3.one, 0.1f));
		return sequence;
	}
}
