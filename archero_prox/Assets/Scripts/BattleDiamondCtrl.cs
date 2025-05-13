




using UnityEngine;
using UnityEngine.UI;

public class BattleDiamondCtrl : MonoBehaviour
{
	public RectTransform child;

	public Image Image_Diamond;

	public Text Text_Diamond;

	private float get_x(bool value)
	{
		return (!value) ? 200 : 0;
	}

	public void Show(bool value, bool rightnow)
	{
	}

	public void UpdateDiamond()
	{
		SetValue(GameLogic.Hold.BattleData.GetDiamond());
	}

	public void SetValue(long value)
	{
		if ((bool)Text_Diamond)
		{
			Text_Diamond.text = value.ToString();
		}
	}

	public Vector3 GetDiamondPosition()
	{
		return Image_Diamond.transform.position;
	}
}
