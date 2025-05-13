




using System;

public class SkillAlone3001 : SkillAloneBase
{
	private int level;

	private float speedratio = 0.5f;

	private bool bRemove = true;

	protected override void OnInstall()
	{
		level = int.Parse(base.m_SkillData.Args[0]);
		bRemove = false;
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
	}

	protected override void OnUninstall()
	{
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		RemoveAttribute();
	}

	private void RemoveAttribute()
	{
		if (!bRemove)
		{
			bRemove = true;
		}
	}

	private void OnGotoNextRoom(RoomGenerateBase.Room room)
	{
		if (room.RoomID > level)
		{
			Uninstall();
		}
	}
}
