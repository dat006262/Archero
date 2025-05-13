




using Dxx.Util;
using System;
using UnityEngine;

public class CameraStartCtrl
{
	public Action OnEnd;

	public Camera camera;

	private float endsize;

	private float startsize;

	private bool bInit;

	private float time = 0.35f;

	private float starttime;

	public void SetCamera(Camera camera)
	{
		this.camera = camera;
		float num = (float)GameLogic.DesignWidth / (float)GameLogic.DesignHeight / ((float)GameLogic.Width / (float)GameLogic.Height);
		startsize = 6f * num;
		endsize = 10.5f * num;
	}

	public void Begin()
	{
		if (!bInit)
		{
			bInit = true;
			starttime = Updater.AliveTime;
			Updater.AddUpdate("CameraStartCtrl", OnUpdate);
		}
	}

	public void DeInit()
	{
		if (bInit)
		{
			bInit = false;
			Updater.RemoveUpdate("CameraStartCtrl", OnUpdate);
		}
	}

	private void OnUpdate(float delta)
	{
		float num = (Updater.AliveTime - starttime) / time;
		if (num < 1f)
		{
			camera.orthographicSize = (endsize - startsize) * num + startsize;
			return;
		}
		camera.orthographicSize = endsize;
		DeInit();
		if (OnEnd != null)
		{
			OnEnd();
		}
	}
}
