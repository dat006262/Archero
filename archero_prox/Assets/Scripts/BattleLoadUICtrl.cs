




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleLoadUICtrl : MediatorCtrlBase
{
	private const string ShowAnimationName = "LoadShow";

	private const string MissAnimationName = "LoadMiss";

	public Animator ani;

	public CanvasGroup ani_canvasgroup;

	public Text Text_Content;

	public GameObject loadingparent;

	private float anitime;

	private BattleLoadProxy.BattleLoadData loaddata;

	private WaitForSecondsRealtime wait01;

	private WaitForSecondsRealtime opentime;

	private int startframe;

	private bool bStart;

	protected override void OnInit()
	{
		//anitime = ani.GetCurrentAnimatorClipInfo(0)[0].clip.length;
		wait01 = new WaitForSecondsRealtime(0.1f);
		opentime = new WaitForSecondsRealtime(anitime);
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("BattleLoadProxy");
		if (proxy == null)
		{
			throw new Exception(Utils.FormatString("BattleLoadMediator.Proxy is null"));
		}
		loaddata = (proxy.Data as BattleLoadProxy.BattleLoadData);
		if (loaddata == null)
		{
			throw new Exception(Utils.FormatString("BattleLoadMediator.Proxy BattleLoadData is null"));
		}
		GameLogic.SetPause(pause: true);
		ani_canvasgroup.alpha = 0f;
		ani.enabled = true;
		loadingparent.SetActive(loaddata.showLoading);
		PlayOpen();
	}

	protected override void OnClose()
	{
		ani.enabled = false;
	}

	private void Update()
	{
		if (!bStart)
		{
			return;
		}
		switch (Time.frameCount - startframe)
		{
		case 0:
			ani.Play("LoadShow");
			break;
		case 12:
			if (loaddata.LoadingDo != null)
			{
				loaddata.LoadingDo();
			}
			break;
		case 14:
			show_camera(value: false);
			GameLogic.UpdateResolution();
			break;
		case 15:
			show_camera(value: true);
			ani.Play("LoadMiss");
			if (loaddata.LoadEnd1Do != null)
			{
				loaddata.LoadEnd1Do();
			}
			GameLogic.SetPause(pause: false);
			break;
		case 27:
			if (loaddata.LoadEnd2Do != null)
			{
				loaddata.LoadEnd2Do();
			}
			WindowUI.CloseWindow(WindowID.WindowID_BattleLoad);
			bStart = false;
			break;
		}
	}

	private IEnumerator start_play()
	{
		yield return null;
		ani.Play("LoadShow");
		yield return opentime;
		if (loaddata.LoadingDo != null)
		{
			loaddata.LoadingDo();
		}
		yield return null;
		show_camera(value: false);
		yield return null;
		GameLogic.UpdateResolution();
		yield return null;
		ani.Play("LoadMiss");
		if (loaddata.LoadEnd1Do != null)
		{
			loaddata.LoadEnd1Do();
		}
		GameLogic.SetPause(pause: false);
		yield return opentime;
		yield return null;
		show_camera(value: true);
		yield return null;
		if (loaddata.LoadEnd2Do != null)
		{
			loaddata.LoadEnd2Do();
		}
		WindowUI.CloseWindow(WindowID.WindowID_BattleLoad);
	}

	private void PlayOpen()
	{
		base.transform.parent.SetAsLastSibling();
		Sequence sequence = DOTween.Sequence();
		sequence.SetUpdate(isIndependentUpdate: true);
		sequence.AppendInterval(0.01f);
		sequence.AppendCallback(delegate
		{
			ani.Play("LoadShow");
		});
		sequence.AppendInterval(anitime);
		sequence.AppendCallback(delegate
		{
			if (loaddata.LoadingDo != null)
			{
				loaddata.LoadingDo();
			}
		});
		sequence.AppendInterval(0.1f);
		sequence.AppendCallback(delegate
		{
			PlayClose();
		});
	}

	private void PlayClose()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.SetUpdate(isIndependentUpdate: true);
		sequence.AppendCallback(delegate
		{
			if (loaddata.showLoading)
			{
				show_camera(value: false);
			}
		});
		sequence.AppendInterval(0.03f);
		sequence.AppendCallback(delegate
		{
			GameLogic.UpdateResolution();
		});
		sequence.AppendInterval(0.1f);
		sequence.AppendCallback(delegate
		{
			ani.Play("LoadMiss");
			if (loaddata.LoadEnd1Do != null)
			{
				loaddata.LoadEnd1Do();
			}
			GameLogic.SetPause(pause: false);
		});
		sequence.AppendInterval(0.03f);
		sequence.AppendCallback(delegate
		{
			show_camera(value: true);
		});
		sequence.AppendInterval(anitime);
		sequence.AppendCallback(delegate
		{
			if (loaddata.LoadEnd2Do != null)
			{
				loaddata.LoadEnd2Do();
			}
			WindowUI.CloseWindow(WindowID.WindowID_BattleLoad);
		});
	}

	private void show_camera(bool value)
	{
		if (loaddata.showLoading)
		{
			GameNode.m_Camera.enabled = value;
			GameNode.m_UICamera.enabled = value;
		}
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
		Text_Content.text = Utils.FormatString("{0}...", GameLogic.Hold.Language.GetLanguageByTID("battleloading_content"));
	}
}
