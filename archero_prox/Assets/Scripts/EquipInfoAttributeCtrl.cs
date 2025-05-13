




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoAttributeCtrl : MonoBehaviour
{
	public class UpExcute
	{
		public static string GetChangeValue(string value1, string value2)
		{
			int result;
			int result2;
			switch (value1.Substring(value1.Length - 1, 1))
			{
			case "%":
				value1 = value1.Substring(0, value1.Length - 1);
				value2 = value2.Substring(0, value2.Length - 1);
				int.TryParse(value1, out result);
				int.TryParse(value2, out result2);
				return Utils.FormatString("{0}%", result2 - result);
			case "f":
				value1 = value1.Substring(0, value1.Length - 1);
				value2 = value2.Substring(0, value2.Length - 1);
				int.TryParse(value1, out result);
				int.TryParse(value2, out result2);
				return Utils.FormatString("{0}", (float)(result2 - result) / 1000f);
			default:
				int.TryParse(value1, out result);
				int.TryParse(value2, out result2);
				return Utils.FormatString("{0}", result2 - result);
			}
		}

		public static string GetValue(string value)
		{
			string text = value.Substring(value.Length - 1, 1);
			if (text != null && text == "f")
			{
				value = value.Substring(0, value.Length - 1);
				int.TryParse(value, out int result);
				return Utils.FormatString("{0}", (float)result / 1000f);
			}
			return value;
		}
	}

	public float pery = 40f;

	public List<Text> mList;

	private LocalSave.EquipOne mEquipData;

	public void UpdateUI(LocalSave.EquipOne one)
	{
		mEquipData = one;
	}
}
