




using Dxx.Util;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class AIMove1025 : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private Vector3 endpos;

	private Weapon_weapon weaponData;

	private int range;

	public AIMove1025(EntityBase entity, int range)
		: base(entity)
	{
		this.range = range;
		weaponData = LocalModelManager.Instance.Weapon_weapon.GetBeanById(5015);
	}

	private void InitPos()
	{
		GameLogic.Release.MapCreatorCtrl.RandomItemSide(m_Entity, range, out float endx, out float endz);
		endpos = new Vector3(endx, 0f, endz);
	}

	protected override void OnInitBase()
	{
		target = GameLogic.Self;
		InitPos();
		Find();
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		UpdateMoveData();
		AIMoving();
	}

	private void AIMoveStart()
	{
		UpdateMoveData();
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void UpdateMoveData()
	{
		if (findpath.Count > 0)
		{
			UpdateDirection();
			if ((nextpos - m_Entity.position).magnitude < 0.5f)
			{
				CreateBullet(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(findpath[0].x, findpath[0].y));
				findpath.RemoveAt(0);
				if (findpath.Count > 0)
				{
					Grid.NodeItem nodeItem = findpath[0];
					nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(nodeItem.x, nodeItem.y);
					UpdateDirection();
				}
			}
		}
		else
		{
			End();
		}
	}

	private void UpdateDirection()
	{
		float x = nextpos.x;
		Vector3 position = m_Entity.position;
		float x2 = x - position.x;
		float z = nextpos.z;
		Vector3 position2 = m_Entity.position;
		float num = z - position2.z;
		Vector3 normalized = new Vector3(x2, 0f, num).normalized;
		m_MoveData.angle = Utils.getAngle(x2, num);
		m_MoveData.direction = normalized;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	private void Find()
	{
		if ((bool)target)
		{
			findpath = GameLogic.Release.Path.FindingPath(m_Entity.position, endpos);
			if (findpath.Count > 0)
			{
				Grid.NodeItem nodeItem = findpath[0];
				nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(nodeItem.x, nodeItem.y);
				UpdateDirection();
			}
		}
	}

	private void AIMoving()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
	}

	private void CreateBullet(Vector3 pos)
	{
		BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, 5015, pos + new Vector3(0f, 0.4f, 0f), 0f);
		if ((bool)bulletBase)
		{
			bulletBase.transform.rotation = Quaternion.identity;
			bulletBase.SetTarget(GameLogic.Self);
		}
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
