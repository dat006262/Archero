




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class EventWindowCtrl : MonoBehaviour
{
	public WindowID windowID;

	private GameObject shadow;

	private GoodsEventEmojiCtrl eMojiCtrl;

	private SphereCollider mSphere;

	private Sequence seq;

	private Animation ani;

	[SerializeField]
	private bool bEvent;

	private bool bDelay;

	private bool bOpenUI;

	private float delaystarttime;

	private float starttime;

	private float anispeed = 1f;

	protected virtual string MissAction => string.Empty;

	private void Awake()
	{
		mSphere = GetComponent<SphereCollider>();
		eMojiCtrl = base.transform.Find("child/child/body/Emotion_BG").GetComponent<GoodsEventEmojiCtrl>();
		shadow = base.transform.Find("child/shadow").gameObject;
		ani = base.transform.Find("child").GetComponent<Animation>();
		starttime = Updater.AliveTime;
		if (windowID == WindowID.WindowID_EventBlackShop)
		{
			anispeed = 1.5f;
			SdkManager.send_event_mysteries("APPEAR", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, string.Empty);
		}
	}

	private void OnDestroy()
	{
		KillSequence();
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	private void Update()
	{
		if (bDelay && Updater.AliveTime - delaystarttime > 1f)
		{
			bDelay = false;
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (!bEvent && Updater.AliveTime - starttime > 1.5f && (bool)mSphere && !mSphere.enabled)
		{
			mSphere.enabled = true;
		}
	}

	private void OnEnable()
	{
		if ((bool)ani)
		{
			ani.enabled = true;
			shadow.SetActive(value: true);
			ani[MissAction].time = ani[MissAction].clip.length;
			ani[MissAction].speed = 0f - anispeed;
			ani.Play(MissAction);
		}
	}

	private void OnDisable()
	{
		if ((bool)ani)
		{
			ani.enabled = false;
		}
	}

	private void OnTriggerEnter(Collider o)
	{
		Enter(o.gameObject);
	}

	private void OnCollisionEnter(Collision o)
	{
		Enter(o.gameObject);
	}

	private void Enter(GameObject o)
	{
		if (!bEvent && GameLogic.Release.Entity.IsSelfObject(o))
		{
			bEvent = true;
			Miss();
			OnEnter();
		}
	}

	private void OnEnter()
	{
		eMojiCtrl.Near();
		KillSequence();
		seq = DOTween.Sequence().AppendInterval(0.3f).AppendCallback(delegate
		{
			if (!bOpenUI)
			{
				bOpenUI = true;
				WindowUI.ShowWindow(windowID);
				shadow.SetActive(value: false);
				ani[MissAction].speed = anispeed;
				ani.Play(MissAction);
			}
		});
	}

	private void Miss()
	{
		bDelay = true;
		delaystarttime = Updater.AliveTime;
	}
}
