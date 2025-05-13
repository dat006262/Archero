




using Dxx.Util;
using System;
using UnityEngine;

public class GoodsDrop : MonoBehaviour
{
	protected BoxCollider m_Box;

	private Func<int> callback;

	private Action mDropEndAction;

	private Animator Drop_ani;

	private bool Drop_Update;

	private float Drop_x;

	private float Drop_y;

	private float Drop_StartTime;

	private float Drop_startx;

	private float Drop_starty;

	protected float Drop_jumpTime = 0.67f;

	private bool bDelay;

	private float Delay_Time;

	private bool bDropEndAction;

	private float percent;

	protected virtual string JumpAnimation => "GoodsJump";

	public void SetCallBack(Func<int> callback)
	{
		this.callback = callback;
	}

	private void Update()
	{
		DropUpdateProcess();
	}

	private void Awake()
	{
		m_Box = GetComponent<BoxCollider>();
		OnAwake();
	}

	protected void OnAwake()
	{
	}

	private void InitDropAni()
	{
		if (Drop_ani == null)
		{
			Transform child = base.transform.GetChild(0);
			Drop_ani = child.gameObject.AddComponent<Animator>();
			Drop_ani.runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("Game/Animator/GoodsJump");
		}
	}

	private void OnEnable()
	{
		if ((bool)Drop_ani)
		{
			Drop_ani.enabled = true;
		}
	}

	private void OnDisable()
	{
		if ((bool)Drop_ani)
		{
			Drop_ani.enabled = false;
		}
	}

	protected virtual void OnInit()
	{
	}

	public void SetDropEnd(Action callback)
	{
		mDropEndAction = callback;
	}

	public void DropInitLast(Vector3 startpos, Vector3 endpos)
	{
		OnInit();
		InitDropAni();
		if (Drop_ani == null && (bool)base.gameObject)
		{
			string name = base.gameObject.name;
			string message = Utils.FormatString("{0} don't have Drop_ani!!!", name);
			SdkManager.Bugly_Report("GoodsDrop.cs", message);
		}
		m_Box.enabled = false;
		if ((bool)Drop_ani)
		{
			Drop_ani.Play(JumpAnimation);
		}
		Drop_Update = true;
		Drop_x = endpos.x - startpos.x;
		Drop_y = endpos.z - startpos.z;
		Drop_StartTime = Updater.AliveTime;
		Drop_startx = startpos.x;
		Drop_starty = startpos.z;
		base.transform.position = new Vector3(Drop_startx, 0f, Drop_starty);
		bDelay = false;
		bDropEndAction = false;
	}

	private void DropUpdateProcess()
	{
		if (Drop_Update)
		{
			percent = (Updater.AliveTime - Drop_StartTime) / Drop_jumpTime;
			if (percent > 0.8f && !bDropEndAction)
			{
				bDropEndAction = true;
				mDropEndAction();
			}
			if (percent <= 1f)
			{
				Transform transform = base.transform;
				float x = Drop_startx + Drop_x * percent;
				Vector3 position = base.transform.position;
				transform.position = new Vector3(x, position.y, Drop_starty + Drop_y * percent);
			}
			else
			{
				base.transform.position = new Vector3(Drop_startx + Drop_x, 0f, Drop_starty + Drop_y);
				Drop_Update = false;
				bDelay = true;
				Delay_Time = Updater.AliveTime;
				if (callback != null)
				{
					callback();
				}
			}
		}
		if (bDelay && Updater.AliveTime - Delay_Time > (float)SettingDebugMediator.AbsorbDelay / 1000f)
		{
			bDelay = false;
			m_Box.enabled = true;
		}
	}
}
