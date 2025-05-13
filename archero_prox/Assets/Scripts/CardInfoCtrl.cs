




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoCtrl : MonoBehaviour
{
	public GameObject child;

	public RectTransform bgparent;

	public RectTransform arrowparent;

	public Text Text_Name;

	public Text Text_Info;

	public RectTransform left;

	public RectTransform right;

	public Animation ani;

	public void Init(CardOneCtrl ctrl)
	{
		child.transform.position = ctrl.transform.position;
		if (!ctrl.carddata.Unlock)
		{
			Text_Name.text = "?";
			Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Lock");
		}
		else
		{
			Text_Name.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物名称{0}", ctrl.carddata.CardID));
			string[] array = new string[ctrl.carddata.data.BaseAttributes.Length];
			bool flag = false;
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				array[i] = ctrl.carddata.GetNextAttribute(i);
				if (ctrl.carddata.data.BaseAttributes[i].Contains("Global_HarvestLevel"))
				{
					flag = true;
				}
			}
			if (flag)
			{
				Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物描述{0}", ctrl.carddata.CardID));
			}
			else
			{
				Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物描述{0}", ctrl.carddata.CardID), array);
			}
		}
		float num2 = 0f;
		Vector3 position = left.position;
		if (position.x < 0f)
		{
			Vector3 position2 = left.position;
			num2 = 0f - position2.x;
		}
		else
		{
			Vector3 position3 = right.position;
			if (position3.x > (float)GameLogic.DesignWidth)
			{
				float num3 = GameLogic.DesignWidth;
				Vector3 position4 = right.position;
				num2 = num3 - position4.x;
			}
		}
		RectTransform rectTransform = bgparent;
		Vector3 position5 = ctrl.transform.position;
		float x = position5.x + num2;
		Vector3 position6 = ctrl.transform.position;
		rectTransform.position = new Vector3(x, position6.y, 0f);
	}

	public void Show(bool value)
	{
		child.SetActive(value);
		if (value)
		{
			ani.Play("Card_InfoShow");
		}
	}
}
