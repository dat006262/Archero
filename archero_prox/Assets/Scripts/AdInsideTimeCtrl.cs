




using UnityEngine;
using UnityEngine.UI;

public class AdInsideTimeCtrl : MonoBehaviour
{
	public Image Image_Circle;

	public Text Text_Time;

	private float maxtime;

	public void SetMax(float maxtime)
	{
		this.maxtime = maxtime;
	}

	public void SetCurrent(float current)
	{
		Text_Time.text = ((int)(maxtime - current)).ToString();
		Image_Circle.fillAmount = 1f - current / maxtime;
	}
}
