




using UnityEngine;
using UnityEngine.UI;

public class EquipWearCtrl : MonoBehaviour
{
	public Text Text_Wear;

	private void OnEnable()
	{
		Text_Wear.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Equiped");
	}
}
