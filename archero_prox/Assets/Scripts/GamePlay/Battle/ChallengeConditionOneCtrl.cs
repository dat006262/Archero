




using UnityEngine;
using UnityEngine.UI;

public class ChallengeConditionOneCtrl : MonoBehaviour
{
	public Text Text_Condition;

	public void Init(string str)
	{
		Text_Condition.text = str;
	}
}
