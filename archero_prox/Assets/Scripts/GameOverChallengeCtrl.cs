




using UnityEngine;
using UnityEngine.UI;

public class GameOverChallengeCtrl : MonoBehaviour
{
	public GameObject child;

	public Text Text_Content;

	public void Show(bool value)
	{
		child.SetActive(value);
	}

	public void SetContent(string value)
	{
		Text_Content.text = value;
	}
}
