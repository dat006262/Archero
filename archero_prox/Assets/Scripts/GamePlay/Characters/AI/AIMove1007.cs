




using Dxx.Util;
using UnityEngine;

public class AIMove1007 : AIMoveBase
{
	private const string COLLIDER_RESOURCE = "Game/SkillPrefab/CollisionCtrl1007";

	protected float Move_NextX;

	protected float Move_NextY;

	private GameObject lastwall;

	private GameObject mCollision;

	public AIMove1007(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		lastwall = null;
		CreateCollisionCtrl();
		Move_NextX = ((GameLogic.Random(0, 2) == 0) ? 1 : (-1));
		Move_NextY = ((GameLogic.Random(0, 2) == 0) ? 1 : (-1));
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
		m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
		m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY);
		m_Entity.m_AttackCtrl.SetRotate(m_MoveData.angle);
	}

	private void AIMoving()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
	}

	private void CreateCollisionCtrl()
	{
		if (m_Entity != null)
		{
			GameObject gameObject = GameObjectPool.Instantiate("Game/SkillPrefab/CollisionCtrl1007");
			gameObject.SetParentNormal(m_Entity.transform);
			CapsuleCollider component = gameObject.GetComponent<CapsuleCollider>();
			component.radius = m_Entity.GetCollidersSize();
			mCollision = gameObject;
			gameObject.GetComponent<EntityMove1007Ctrl>().CollisionEnterAction = CollisionEnter;
		}
	}

	private void CollisionEnter(Collision o)
	{
		if ((!m_Entity.GetFlying() || (o.gameObject.layer != LayerManager.Stone && o.gameObject.layer != LayerManager.Waters)) && lastwall != o.gameObject)
		{
			lastwall = o.gameObject;
			Vector3 eulerAngles = m_Entity.eulerAngles;
			float angle = Utils.ExcuteReboundWallSkill(eulerAngles.y, m_Entity.position, m_Entity.GetComponent<SphereCollider>(), o.collider);
			Move_NextX = MathDxx.Sin(angle);
			Move_NextY = MathDxx.Cos(angle);
			UpdateMoveData();
		}
	}

	protected override void OnEnd()
	{
		if (mCollision != null)
		{
			UnityEngine.Object.Destroy(mCollision);
		}
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
