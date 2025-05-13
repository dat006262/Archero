




using Dxx.Util;
using UnityEngine;

public class ChooseSkillButtonCtrl : MonoBehaviour
{
	private ChooseSkillOneCtrl[] list = new ChooseSkillOneCtrl[3];

	private void Awake()
	{
		for (int i = 0; i < 3; i++)
		{
			list[i] = base.transform.Find(Utils.GetString("fg/child/columnctrl/", i)).GetComponent<ChooseSkillOneCtrl>();
		}
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		for (int i = 0; i < 3; i++)
		{
			list[i].OnClick();
		}
	}
}
