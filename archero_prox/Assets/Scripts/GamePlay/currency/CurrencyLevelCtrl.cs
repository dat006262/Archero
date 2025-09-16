




using UnityEngine;
using UnityEngine.UI;

public class CurrencyLevelCtrl : MonoBehaviour
{
	public Text Text_Level;

	public ProgressCtrl mProgressCtrl;

	public void UpdateUI()
	{
		int level = LocalSave.Instance.GetLevel();
		Text_Level.text = level.ToString();
		int num = (int)LocalSave.Instance.GetExp();
		int expByLevel = LocalSave.Instance.GetExpByLevel(level);
		mProgressCtrl.Value = (float)num / (float)expByLevel;
	}
}
