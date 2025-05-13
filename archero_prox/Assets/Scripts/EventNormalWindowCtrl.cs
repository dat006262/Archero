




using DG.Tweening;
using UnityEngine;

public class EventNormalWindowCtrl : MonoBehaviour
{
	private string MissAction = "Event_Angel_Miss";

	public WindowID windowID;

	private GoodsEventEmojiCtrl eMojiCtrl;

	private Animation ani;

	private void Awake()
	{
		eMojiCtrl = base.transform.Find("child/child/body/Emotion_BG").GetComponent<GoodsEventEmojiCtrl>();
		ani = base.transform.Find("child").GetComponent<Animation>();
		ani[MissAction].time = ani[MissAction].clip.length;
		ani[MissAction].speed = -1f;
		ani.Play(MissAction);
	}

	private void OnTriggerEnter(Collider o)
	{
		if (GameLogic.Release.Entity.IsSelfObject(o.gameObject))
		{
			OnEnter();
		}
	}

	private void OnTriggerExit(Collider o)
	{
		if (GameLogic.Release.Entity.IsSelfObject(o.gameObject))
		{
			OnExit();
		}
	}

	private void OnEnter()
	{
		eMojiCtrl.Near();
		DOTween.Sequence().AppendInterval(0.3f).AppendCallback(delegate
		{
			WindowUI.ShowWindow(windowID);
		});
	}

	private void OnExit()
	{
		eMojiCtrl.Far();
	}
}
