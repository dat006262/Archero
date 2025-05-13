




using System;
using System.Collections.Generic;
using UnityEngine;

public class Goods1151 : GoodsBase
{
	public class Door1151
	{
		public List<Goods1151> Doors = new List<Goods1151>();

		private ActionBasic action = new ActionBasic();

		public Door1151()
		{
			ReleaseModeManager mode = GameLogic.Release.Mode;
			mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		}

		public void AddDoor(Goods1151 d)
		{
			Doors.Add(d);
			action.Init();
		}

		public void GotoDoor(Goods1151 d)
		{
			GameLogic.PlayEffect(3200001, d.transform.position);
			GameLogic.Self.ShowEntity(show: false);
			GameLogic.Release.Game.ShowJoy(show: false);
			Goods1151 other = GetOtherDoor(d);
			other.SetState(DoorState.eThrough);
			action.AddActionWaitDelegate(0.4f, delegate
			{
				GameLogic.Self.SetPosition(other.transform.position);
			});
			action.AddActionWaitDelegate(0.5f, delegate
			{
				GameLogic.PlayEffect(3200001, other.transform.position);
			});
			action.AddActionWaitDelegate(0.5f, delegate
			{
				GameLogic.Self.ShowEntity(show: true);
				GameLogic.Release.Game.ShowJoy(show: true);
			});
		}

		private Goods1151 GetOtherDoor(Goods1151 d)
		{
			int i = 0;
			for (int count = Doors.Count; i < count; i++)
			{
				Goods1151 goods = Doors[i];
				if (goods != d)
				{
					return goods;
				}
			}
			return null;
		}

		private void OnGotoNextRoom(RoomGenerateBase.Room room)
		{
			Clear();
		}

		public void Clear()
		{
			Doors.Clear();
			action.DeInit();
		}

		public void DeInit()
		{
			Clear();
			ReleaseModeManager mode = GameLogic.Release.Mode;
			mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		}
	}

	public enum DoorState
	{
		eNormal,
		eThrough
	}

	private static Door1151 mDoorData;

	private DoorState state;

	public static Door1151 DoorData
	{
		get
		{
			if (mDoorData == null)
			{
				mDoorData = new Door1151();
			}
			return mDoorData;
		}
		set
		{
			mDoorData = value;
		}
	}

	public void SetState(DoorState state)
	{
		this.state = state;
	}

	protected override void Init()
	{
		base.Init();
	}

	protected override void StartInit()
	{
		base.StartInit();
		DoorData.AddDoor(this);
	}

	public override void ChildTriggerEnter(GameObject o)
	{
		if ((bool)GameLogic.Self && o == GameLogic.Self.gameObject && state == DoorState.eNormal)
		{
			DoorData.GotoDoor(this);
		}
	}

	public override void ChildTriggetExit(GameObject o)
	{
		if (o == GameLogic.Self.gameObject && state == DoorState.eThrough)
		{
			state = DoorState.eNormal;
		}
	}
}
