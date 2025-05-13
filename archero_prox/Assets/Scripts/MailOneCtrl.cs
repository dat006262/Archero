




using Dxx.Util;
using GameProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MailOneCtrl : MonoBehaviour
{
	public Text Text_Name;

	public Text Text_Info;

	public Text Text_Time;

	public Text Text_New;

	public RedNodeCtrl m_RedCtrl;

	public ButtonCtrl Button_Open;

	public CanvasGroup mCanvasGroup;

	public ContentSizeFitter InfoFitter;

	public Action<int, MailOneCtrl> OnClickButton;

	private CMailInfo mData;

	private int mIndex;

	private void Awake()
	{
		Button_Open.onClick = delegate
		{
			if (OnClickButton != null)
			{
				OnClickButton(mIndex, this);
				UpdateMail();
			}
		};
	}

	public void Init(int index, CMailInfo data)
	{
		mData = data;
		mCanvasGroup.alpha = 1f;
		mIndex = index;
		Text_Name.text = mData.m_strTitle;
		Text_Time.text = Utils.GetTimeGo(mData.m_i64PubTime);
		Text_Info.text = Utils.CutString(mData.m_strContent, 25);
		Text_New.text = string.Empty;
		UpdateMail();
	}

	public void UpdateMail()
	{
		SetRedShow(mData.IsShowRed);
	}

	public void SetRedShow(bool value)
	{
		m_RedCtrl.SetType(RedNodeType.eRedNew);
		m_RedCtrl.Value = (value ? 1 : 0);
	}
}
