




using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_NoNetCtrl : MonoBehaviour
{
	public GameObject child;

	public Image Image_NoNet;

	public Text Text_NoNet;

	private void Awake()
	{
		Image_NoNet.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo).SetUpdate(isIndependentUpdate: true);
	}

	public void SetShow(bool value)
	{
		if ((bool)child)
		{
			child.SetActive(value);
		}
	}

	public void OnLanguageUpdate()
	{
		Text_NoNet.text = GameLogic.Hold.Language.GetLanguageByTID("GameOver_NoNet");
	}
}
