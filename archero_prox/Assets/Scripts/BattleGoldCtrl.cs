




using UnityEngine;
using UnityEngine.UI;

public class BattleGoldCtrl : MonoBehaviour
{
	public Text Text_Gold;

	public void SetGold(long gold)
	{
		if ((bool)Text_Gold)
		{
			Text_Gold.text = gold.ToString();
		}
	}
}
