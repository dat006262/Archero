




using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1014 : SkillAloneBase
{
	private GameObject good;

	private ParticleSystem mParticle;

	private SkillAlone1014Ctrl ctrl = new SkillAlone1014Ctrl();

	private AutoDespawn mAutoDespawn;

	private Sequence seq;

	protected override void OnInstall()
	{
		CreateSkillAlone();
		ctrl.Init(m_Entity, this);
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		m_Entity.Event_PositionBy += OnPositionBy;
	}

	protected override void OnUninstall()
	{
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		m_Entity.Event_PositionBy -= OnPositionBy;
		ctrl.DeInit();
		mAutoDespawn = good.GetComponent<AutoDespawn>();
		if (!mAutoDespawn)
		{
			mAutoDespawn = good.AddComponent<AutoDespawn>();
		}
		mAutoDespawn.SetDespawnTime(5f);
		mAutoDespawn.enabled = true;
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

	private void OnGotoNextRoom(RoomGenerateBase.Room room)
	{
		good.transform.position = m_Entity.position;
		KillSequence();
		good.SetActive(value: false);
		mParticle.Clear();
		ctrl.RemoveGoods();
	}

	private void OnPositionBy(Vector3 p)
	{
		if (!good.activeSelf)
		{
			good.SetActive(value: true);
		}
		good.transform.position = m_Entity.position;
	}

	private void CreateSkillAlone()
	{
		good = GameLogic.EffectGet(Utils.GetString("Game/SkillPrefab/SkillAlone", base.ClassID, "Effect"));
		good.transform.SetParent(GameNode.m_PoolParent);
		mAutoDespawn = good.GetComponent<AutoDespawn>();
		if ((bool)mAutoDespawn)
		{
			mAutoDespawn.enabled = false;
		}
		mParticle = good.GetComponentInChildren<ParticleSystem>();
		good.SetActive(value: false);
	}
}
