




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillAlone1003 : SkillAloneBase
{
	private List<SkillCreateBase> mClouds = new List<SkillCreateBase>();

	protected override void OnInstall()
	{
		EntityBase entity = m_Entity;
		entity.OnMonsterDeadAction = (Action<EntityBase>)Delegate.Combine(entity.OnMonsterDeadAction, new Action<EntityBase>(DeadAction));
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnMonsterDeadAction = (Action<EntityBase>)Delegate.Remove(entity.OnMonsterDeadAction, new Action<EntityBase>(DeadAction));
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		OnGotoNextRoom(null);
	}

	private void DeadAction(EntityBase entity)
	{
		if (!(entity == null))
		{
			SkillCreateBase cloud = GetCloud();
			cloud.transform.position = entity.position;
			cloud.Init(m_Entity, base.m_SkillData.Args);
			cloud.SetTimeCallback(CloudTimeOver);
			mClouds.Add(cloud);
		}
	}

	private SkillCreateBase GetCloud()
	{
		GameObject gameObject = GameLogic.EffectGet(Utils.FormatString("Game/SkillPrefab/{0}", base.ClassName));
		return gameObject.GetComponent<SkillCreateBase>();
	}

	private void CloudTimeOver(SkillCreateBase cloud)
	{
		cloud.Deinit();
		mClouds.Remove(cloud);
	}

	private void OnGotoNextRoom(RoomGenerateBase.Room room)
	{
		int i = 0;
		for (int count = mClouds.Count; i < count; i++)
		{
			SkillCreateBase skillCreateBase = mClouds[i];
			skillCreateBase.Deinit();
		}
		mClouds.Clear();
	}
}
