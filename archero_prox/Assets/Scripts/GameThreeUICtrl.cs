




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameThreeUICtrl : MonoBehaviour
{
	private List<Transform> list = new List<Transform>();

	private List<Animation> animationlist = new List<Animation>();

	private List<Transform> shadowlist = new List<Transform>();

	private List<ButtonCtrl> buttonlist = new List<ButtonCtrl>();

	private List<Vector3> listpos = new List<Vector3>();

	private GameObject sieve;

	private GameThreeActionCtrl ctrl;

	private Text textcontentcount;

	private int count;

	private Action mCallback;

	private ActionBasic action1 = new ActionBasic();

	private void Awake()
	{
		for (int i = 0; i < 3; i++)
		{
			list.Add(base.transform.Find(Utils.GetString("child/three/", i)));
			animationlist.Add(list[i].GetComponent<Animation>());
			shadowlist.Add(base.transform.Find(Utils.GetString("child/three/shadow", i)));
			int index = i;
			buttonlist.Add(list[index].Find("cup").GetComponent<ButtonCtrl>());
			buttonlist[index].onClick = delegate
			{
				OnClick(list[index]);
			};
			listpos.Add(list[i].localPosition);
		}
		ButtonEnable(enable: false);
		textcontentcount = base.transform.Find("Title/Text_Content_Count").GetComponent<Text>();
		sieve = base.transform.Find("child/three/sieve").gameObject;
		ctrl = new GameThreeActionCtrl();
		ctrl.Init(IgnoreTimeScale: true);
		action1.Init(IgnoreTimeScale: true);
	}

	public void UpdateCountUI()
	{
		textcontentcount.text = Utils.GetString(GameLogic.Hold.Language.GetLanguageByTID("剩余次数"), " : ", count);
	}

	public void SetCount(int count)
	{
		this.count = count;
		UpdateCountUI();
	}

	public void SetEndCallback(Action callback)
	{
		mCallback = callback;
	}

	public void DoAllActions()
	{
		DoAction();
	}

	private void DoAction()
	{
		action1.ActionClear();
		action1.AddAction(new ActionBasic.ActionWaitIgnoreTime
		{
			waitTime = 0.6f
		});
		action1.AddAction(new ActionBasic.ActionDelegate
		{
			action = DoActionMust
		});
	}

	private void DoActionMust()
	{
		for (int i = 0; i < 3; i++)
		{
			list[i].localPosition = listpos[i];
		}
		sieve.transform.localPosition = Vector3.zero;
		ctrl.DoAction(list, shadowlist, sieve, ActionEnd);
	}

	private void ActionEnd()
	{
		ButtonEnable(enable: true);
		CupRotatePlay(play: true);
	}

	private void OnClick(Transform t)
	{
		ButtonEnable(enable: false);
		CupRotatePlay(play: false);
		ctrl.OnClickOne(t, sieve.transform, OnClickEnd);
	}

	private void OnClickEnd(bool result)
	{
		Debugger.Log(Utils.GetString("result ", result));
		count--;
		UpdateCountUI();
		if (count > 0)
		{
			DoAction();
		}
		else
		{
			mCallback();
		}
	}

	private void ButtonEnable(bool enable)
	{
		for (int i = 0; i < 3; i++)
		{
			buttonlist[i].enabled = enable;
		}
	}

	private void CupRotatePlay(bool play)
	{
		if (play)
		{
			for (int i = 0; i < 3; i++)
			{
				animationlist[i].Play("CupRotate");
			}
		}
		else
		{
			for (int j = 0; j < 3; j++)
			{
				animationlist[j].Stop();
			}
		}
	}

	public void RemoveAction()
	{
		ctrl.DeInit();
		action1.DeInit();
	}
}
