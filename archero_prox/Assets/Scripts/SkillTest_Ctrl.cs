




using UnityEngine;
using UnityEngine.UI;

public class SkillTest_Ctrl : MonoBehaviour
{
	private InputField mInput;

	private ButtonCtrl Button_Sure;

	private int skillid;

	private void Awake()
	{
		mInput = base.transform.Find("InputField").GetComponent<InputField>();
		Button_Sure = base.transform.Find("Button_Sure").GetComponent<ButtonCtrl>();
		Button_Sure.onClick = OnButtonClick;
		mInput.onValueChanged.AddListener(delegate(string value)
		{
			int.TryParse(value, out skillid);
		});
	}

	private void OnButtonClick()
	{
		GameLogic.Self.AddSkillTest(skillid);
		mInput.text = string.Empty;
		mInput.ActivateInputField();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			OnButtonClick();
		}
	}
}
