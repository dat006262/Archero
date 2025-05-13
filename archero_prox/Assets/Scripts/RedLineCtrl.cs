




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class RedLineCtrl
{
	private EntityBase m_Entity;

	private bool bEnd;

	private List<GameObject> RedLineList = new List<GameObject>();

	private List<BulletRedLineCtrl> lineCtrlList = new List<BulletRedLineCtrl>();

	private float resultangle;

	private Vector3 resultpos;

	private Vector3 nextpos;

	private float lastangle;

	private int ReboundCount;

	private float offsetangle;

	private bool bThroughWall;

	private int layerMask;

	public void Init(EntityBase entity, bool throughwall, int ReboundCount, float offsetangle)
	{
		m_Entity = entity;
		this.ReboundCount = ReboundCount;
		this.offsetangle = offsetangle;
		bThroughWall = throughwall;
		if (bThroughWall)
		{
			layerMask = 1 << LayerManager.MapOutWall;
		}
		else
		{
			layerMask = ((1 << LayerManager.Bullet2Map) | (1 << LayerManager.MapOutWall));
		}
		CreateRedLine();
	}

	public void DeInit()
	{
		RemoveRedLine();
	}

	private void CreateRedLine()
	{
		while (RedLineList.Count < ReboundCount + 1)
		{
			GameObject gameObject = GameLogic.EffectGet("Game/Bullet/Bullet1007_RedLine");
			gameObject.transform.SetParent(GameNode.m_PoolParent);
			gameObject.transform.localPosition = m_Entity.m_Body.EffectMask.transform.position;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.rotation = m_Entity.m_Body.EffectMask.transform.rotation;
			Transform transform = gameObject.transform;
			Vector3 eulerAngles = gameObject.transform.eulerAngles;
			float x = eulerAngles.x;
			Vector3 eulerAngles2 = gameObject.transform.eulerAngles;
			float y = eulerAngles2.y + offsetangle;
			Vector3 eulerAngles3 = gameObject.transform.eulerAngles;
			transform.rotation = Quaternion.Euler(x, y, eulerAngles3.z);
			RedLineList.Add(gameObject);
			lineCtrlList.Add(gameObject.GetComponent<BulletRedLineCtrl>());
		}
		UpdateLinesLength();
	}

	private void UpdateLinesData()
	{
		if (!(m_Entity.m_HatredTarget == null))
		{
			Vector3 position = m_Entity.m_HatredTarget.position;
			float x = position.x;
			Vector3 position2 = m_Entity.position;
			float x2 = x - position2.x;
			Vector3 position3 = m_Entity.m_HatredTarget.position;
			float z = position3.z;
			Vector3 position4 = m_Entity.position;
			float y = z - position4.z;
			resultangle = Utils.getAngle(x2, y) + offsetangle;
			resultpos = m_Entity.m_Body.LeftBullet.transform.position;
			float x3 = MathDxx.Sin(resultangle);
			float z2 = MathDxx.Cos(resultangle);
			Vector3 normalized = new Vector3(x3, 0f, z2).normalized;
			nextpos = m_Entity.position + normalized * 40f;
			lastangle = resultangle;
		}
	}

	private void UpdateLinesLength()
	{
		UpdateLinesData();
		int i = 0;
		for (int count = RedLineList.Count; i < count; i++)
		{
			UpdateLineLength(i);
		}
	}

	private void UpdateLineLength(int index)
	{
		Transform transform = RedLineList[index].transform;
		float x = resultpos.x;
		Vector3 position = m_Entity.m_Body.LeftBullet.transform.position;
		transform.position = new Vector3(x, position.y, resultpos.z);
		RedLineList[index].transform.rotation = Quaternion.Euler(0f, resultangle, 0f);
		RayCastManager.CastMinDistance(dir: (nextpos - resultpos).normalized, startpos: RedLineList[index].transform.position, fly: bThroughWall, mindis: out float mindis, minpos: out resultpos, minCollider: out Collider minCollider);
		lineCtrlList[index].SetLine(index == RedLineList.Count - 1, mindis);
		if (minCollider != null)
		{
			resultangle = Utils.ExcuteReboundWallRedLine(RedLineList[index].transform, minCollider);
		}
		float x2 = MathDxx.Sin(resultangle);
		float z = MathDxx.Cos(resultangle);
		nextpos = resultpos + new Vector3(x2, 0f, z) * 40f;
	}

	private void RemoveRedLine()
	{
		int i = 0;
		for (int count = RedLineList.Count; i < count; i++)
		{
			GameObject gameObject = RedLineList[i];
			if ((bool)gameObject)
			{
				GameLogic.EffectCache(gameObject);
			}
		}
		RedLineList.Clear();
		lineCtrlList.Clear();
	}

	public void Update()
	{
		if ((bool)m_Entity && (bool)m_Entity.m_HatredTarget)
		{
			UpdateLinesLength();
		}
	}
}
