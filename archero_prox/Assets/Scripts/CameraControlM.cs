




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class CameraControlM : PauseObject
{
	public static CameraControlM Instance;

	public static bool CameraFollow;

	private const float Speed = 30f;

	private float speed = 30f;

	[SerializeField]
	private float minx;

	[SerializeField]
	private float maxx;

	[SerializeField]
	private float miny;

	[SerializeField]
	private float maxy;

	private const float OffsetY = -5f;

	private int RoomStateBoss;

	private float RoomStateBoss_Time;

	private Camera m_Camera;

	private float mCameraStartSize;

	private CameraStartCtrl mStartCtrl;

	public const float CameraStartSize = 6f;

	public const float CameraEndSize = 10.5f;

	[SerializeField]
	private Camera uiCamera;

	private void Start()
	{
		Instance = this;
		m_Camera = base.transform.Find("Child/Camera").GetComponent<Camera>();
		m_Camera.orthographicSize = 6f * ((float)GameLogic.DesignWidth / (float)GameLogic.DesignHeight) / ((float)GameLogic.Width / (float)GameLogic.Height);
		mCameraStartSize = m_Camera.orthographicSize;
		Transform transform = uiCamera.transform;
		float x = GameLogic.Width;
		float y = GameLogic.Height;
		Vector3 localPosition = uiCamera.transform.localPosition;
		transform.localPosition = new Vector3(x, y, localPosition.z) * 0.5f;
	}

	public void DeInit()
	{
		RemoveStartAnimate();
	}

	public void ResetCameraSize()
	{
		m_Camera.orthographicSize = mCameraStartSize;
	}

	private void RemoveStartAnimate()
	{
		if (mStartCtrl != null)
		{
			mStartCtrl.DeInit();
			mStartCtrl = null;
		}
	}

	public void PlayStartAnimate(Action callback = null)
	{
		Instance.ResetCameraSize();
		RemoveStartAnimate();
		mStartCtrl = new CameraStartCtrl();
		mStartCtrl.SetCamera(m_Camera);
		mStartCtrl.OnEnd = callback;
		mStartCtrl.Begin();
	}

	private void FixedUpdate()
	{
		if (!GameLogic.Paused && GameLogic.InGame)
		{
			RoomState roomState = GameLogic.Release.Game.RoomState;
			if (roomState == RoomState.Runing || roomState == RoomState.Throughing)
			{
				Update_Running();
			}
		}
	}

	private void Update_Running()
	{
		if ((bool)GameLogic.Self && !GameLogic.Self.GetIsDead())
		{
			Update_Runnings();
		}
	}

	private void Update_Runnings()
	{
		if (CameraFollow)
		{
			base.transform.position = GameLogic.Self.position;
			return;
		}
		Vector3 position = base.transform.position;
		float z = position.z;
		Vector3 position2 = GameLogic.Self.position;
		float num = MathDxx.Abs(z - position2.z);
		if (num < 0.2f)
		{
			base.transform.position = GameLogic.Self.position;
		}
		else
		{
			float num2 = speed * Updater.delta / num;
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			Vector3 position3 = GameLogic.Self.position;
			float z2 = position3.z;
			Vector3 position4 = base.transform.position;
			float num3 = (z2 - position4.z) * num2;
			Vector3 position5 = base.transform.position;
			float z3 = num3 + position5.z;
			base.transform.position = new Vector3(0f, 0f, z3);
			base.transform.position = Vector3.Lerp(base.transform.position, GameLogic.Self.position, num2);
		}
		SetCameraRound();
	}

	private void SetCameraRound()
	{
		Transform transform = base.transform;
		Vector3 position = base.transform.position;
		float x = Mathf.Clamp(position.x, minx, maxx);
		Vector3 position2 = base.transform.position;
		transform.position = new Vector3(x, 0f, position2.z);
	}

	public void ResetCameraPosition()
	{
		if ((bool)GameLogic.Self)
		{
			Transform transform = base.transform;
			Vector3 position = GameLogic.Self.position;
			transform.position = new Vector3(0f, 0f, position.z);
			SetCameraRound();
		}
		else
		{
			CameraPositionZero();
		}
	}

	public void SetCameraSpeed(float speed)
	{
		this.speed = speed;
	}

	public void ResetCameraSpeed()
	{
		speed = 30f;
	}

	public void SetCameraPosition(Vector3 pos)
	{
		base.transform.position = pos;
	}

	public Vector3 GetCameraPosition()
	{
		return base.transform.position;
	}

	public Vector3 GetCameraEndPosition()
	{
		Vector3 position = base.transform.position;
		if (position.x == minx)
		{
			float x = maxx;
			Vector3 position2 = base.transform.position;
			return new Vector3(x, position2.y, 0f);
		}
		float x2 = minx;
		Vector3 position3 = base.transform.position;
		return new Vector3(x2, position3.y, 0f);
	}

	public void CameraPositionZero()
	{
		base.transform.position = Vector3.zero;
	}

	public void SetCurrentRoom(int roomid)
	{
		Room_room beanById = LocalModelManager.Instance.Room_room.GetBeanById(roomid);
		minx = beanById.CameraRound[0];
		maxx = beanById.CameraRound[1];
		miny = beanById.CameraRound[2];
		maxy = beanById.CameraRound[3];
	}
}
