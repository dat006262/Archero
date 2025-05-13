




using PureMVC.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class MatchDefenceTimeUICtrl : MediatorCtrlBase
{
	public ButtonCtrl Button_Match;

	public Text Text_Match;

	public GameObject match_obj;

	protected override void OnInit()
	{
		Button_Match.onClick = delegate
		{
			switch (Singleton<MatchDefenceTimeSocketCtrl>.Instance.State)
			{
			case MatchDefenceTimeSocketCtrl.ConnectState.eConnected:
				StopMatch();
				Singleton<MatchDefenceTimeSocketCtrl>.Instance.Close();
				break;
			case MatchDefenceTimeSocketCtrl.ConnectState.eClose:
				StartMatch();
				Singleton<MatchDefenceTimeSocketCtrl>.Instance.Connect();
				break;
			}
		};
		RectTransform rectTransform = Button_Match.transform.parent as RectTransform;
		rectTransform.anchoredPosition = new Vector2(0f, (float)GameLogic.Height * 0.23f);
	}

	protected override void OnOpen()
	{
		StopMatch();
		InitUI();
	}

	private void InitUI()
	{
	}

	private void StartMatch()
	{
		match_obj.SetActive(value: true);
		Text_Match.text = "取消匹配";
	}

	private void StopMatch()
	{
		match_obj.SetActive(value: false);
		Text_Match.text = "匹配";
	}

	protected override void OnClose()
	{
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override void OnLanguageChange()
	{
	}
}
