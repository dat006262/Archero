




using Dxx.Util;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class AIMoveBomberman : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private Vector3 endpos;

	private Weapon_weapon weaponData;

	private int range;

	private Vector2Int checkpos = new Vector2Int(-1, -1);

	public AIMoveBomberman(EntityBase entity, int range)
		: base(entity)
	{
		this.range = range;
		weaponData = LocalModelManager.Instance.Weapon_weapon.GetBeanById(3033);
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

	private bool issamepos(Grid.NodeItem item)
	{
		return item.x == checkpos.x && item.y == checkpos.y;
	}

	private void UpdateMoveData()
	{
		if (findpath.Count > 0 && !issamepos(findpath[0]) && GameLogic.Release.MapCreatorCtrl.Bomberman_is_danger(findpath[0]))
		{
			Find();
		}
		if (findpath.Count > 0)
		{
			UpdateDirection();
			if ((nextpos - m_Entity.position).magnitude < 0.1f)
			{
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
			Vector2Int v = GameLogic.Release.MapCreatorCtrl.Bomberman_get_safe_near(m_Entity.position);
			findpath = GameLogic.Release.Path.FindingPath(m_Entity.position, GameLogic.Release.MapCreatorCtrl.GetWorldPosition(v));
			if (findpath.Count == 0)
			{
				GameLogic.Release.MapCreatorCtrl.RandomItemSide(m_Entity, range, out float endx, out float endz);
				endpos = new Vector3(endx, 0f, endz);
				findpath = GameLogic.Release.MapCreatorCtrl.Bomberman_find_path(m_Entity.position, endpos);
			}
			if (findpath.Count > 0)
			{
				Grid.NodeItem nodeItem = findpath[0];
				checkpos = new Vector2Int(nodeItem.x, nodeItem.y);
				nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(nodeItem.x, nodeItem.y);
				UpdateDirection();
			}
		}
	}

	private void AIMoving()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
