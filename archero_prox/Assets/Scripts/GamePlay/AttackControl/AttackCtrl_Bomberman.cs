




using Dxx.Util;
using System;
using UnityEngine;

public class AttackCtrl_Bomberman
{
	private Transform groundlight;

	private RectTransform progress;

	private ProgressCtrl mProgressCtrl;

	private EntityBase m_Entity;

	private float updatetime;

	private float starttime;

	private bool bStand;

	private bool bProgressShow = true;

	private Vector3 bombpos;

	public void Init(EntityBase entity)
	{
		m_Entity = entity;
		EntityBase entity2 = m_Entity;
		entity2.OnMoveEvent = (Action<bool>)Delegate.Combine(entity2.OnMoveEvent, new Action<bool>(OnMove));
		updatetime = GameLogic.Hold.BattleData.Challenge_BombermanTime();
		starttime = Updater.AliveTime;
		if (m_Entity.IsSelf)
		{
			groundlight = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("Game/Bomberman/groundlight")).transform;
			progress = (UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("Game/Bomberman/bomb_progress")).transform as RectTransform);
			progress.SetParentNormal(GameNode.m_InGame);
			mProgressCtrl = progress.GetComponent<ProgressCtrl>();
			SetProgressShow(value: false);
		}
	}

	public void DeInit()
	{
		if (groundlight != null)
		{
			UnityEngine.Object.Destroy(groundlight.gameObject);
		}
		if (progress != null)
		{
			UnityEngine.Object.Destroy(progress.gameObject);
		}
	}

	private void SetProgressShow(bool value)
	{
		if (bProgressShow != value)
		{
			bProgressShow = value;
			if (progress != null)
			{
				progress.gameObject.SetActive(value);
			}
		}
	}

	private void SetProgressValue(float value)
	{
		if (mProgressCtrl != null)
		{
			mProgressCtrl.Value = value;
		}
	}

	private void OnMove(bool value)
	{
		bStand = !value;
		if (bStand)
		{
			starttime = Updater.AliveTime;
		}
	}

	public void Update()
	{
		Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(m_Entity.position);
		bombpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(roomXY);
		if ((bool)groundlight)
		{
			groundlight.position = bombpos;
		}
		if (bStand)
		{
			if (!GameLogic.Release.MapCreatorCtrl.Bomberman_is_empty(m_Entity.position))
			{
				starttime = Updater.AliveTime;
				SetProgressShow(value: false);
				return;
			}
			float value = (Updater.AliveTime - starttime) / updatetime;
			value = MathDxx.Clamp01(value);
			SetProgressShow(value: true);
			SetProgressValue(value);
			if (progress != null)
			{
				Vector3 vector = Utils.World2Screen(bombpos);
				float x = vector.x;
				float y = vector.y;
				progress.position = new Vector3(x, y - 50f, 0f);
			}
			if (value >= 1f)
			{
				starttime += updatetime;
				CreateBomb();
			}
		}
		else
		{
			SetProgressShow(value: false);
		}
	}

	private void CreateBomb()
	{
		BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(null, 3033, bombpos, 0f);
		if ((bool)bulletBase)
		{
			bulletBase.transform.rotation = Quaternion.identity;
			bulletBase.SetTarget(null);
			bulletBase.mBulletTransmit.SetAttack(20L);
		}
	}
}
