




using Dxx.Util;
using UnityEngine;

public class AIMove1028 : AIMoveBase
{
	private const string COLLIDER_RESOURCE = "Game/SkillPrefab/CollisionCtrl1007";

	private bool bRotateOver;

	private int reboundcount;

	private const int ReboundMaxCount = 3;

	protected float Move_NextX;

	protected float Move_NextY;

	private GameObject lastwall;

	private GameObject mCollision;

	private float mEndStartTime;

	private float mEndTime = 1f;

	public AIMove1028(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		bRotateOver = false;
		lastwall = null;
		reboundcount = 0;
		Vector3 position = m_Entity.m_HatredTarget.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		Move_NextX = x - position2.x;
		Vector3 position3 = m_Entity.m_HatredTarget.position;
		float z = position3.z;
		Vector3 position4 = m_Entity.position;
		Move_NextY = z - position4.z;
		float angle = Utils.getAngle(Move_NextX, Move_NextY);
		m_Entity.m_AttackCtrl.RotateHero(angle);
		CreateCollisionCtrl();
	}

	private void CreateCollisionCtrl()
	{
		GameObject gameObject = GameObjectPool.Instantiate("Game/SkillPrefab/CollisionCtrl1007");
		gameObject.transform.SetParent(m_Entity.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		CapsuleCollider component = gameObject.GetComponent<CapsuleCollider>();
		component.radius = m_Entity.GetCollidersSize();
		mCollision = gameObject;
		gameObject.GetComponent<EntityMove1007Ctrl>().CollisionEnterAction = CollisionEnter;
	}

	private void AIMoveStart()
	{
		UpdateMoveData();
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void UpdateMoveData()
	{
		m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
		m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY) * (3f - (float)reboundcount * 0.4f);
		m_Entity.m_AttackCtrl.SetRotate(m_MoveData.angle);
	}

	private void AIMoving()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
	}

	protected override void OnUpdate()
	{
		if (!bRotateOver)
		{
			if (m_Entity.m_AttackCtrl.RotateOver())
			{
				bRotateOver = true;
				Vector3 eulerAngles = m_Entity.eulerAngles;
				Move_NextX = MathDxx.Sin(eulerAngles.y);
				Vector3 eulerAngles2 = m_Entity.eulerAngles;
				Move_NextY = MathDxx.Cos(eulerAngles2.y);
				AIMoveStart();
				m_Entity.m_AniCtrl.SetString("Skill", "Hitted");
				m_Entity.m_AniCtrl.SendEvent("Skill");
			}
		}
		else
		{
			AIMoving();
		}
		if (reboundcount == 3 && Updater.AliveTime > mEndStartTime)
		{
			End();
		}
	}

	private void CollisionEnter(Collision o)
	{
		if ((o.gameObject.layer != LayerManager.MapOutWall && o.gameObject.layer != LayerManager.Stone) || !(lastwall != o.gameObject))
		{
			return;
		}
		lastwall = o.gameObject;
		if (reboundcount < 3)
		{
			reboundcount++;
			Vector3 eulerAngles = m_Entity.eulerAngles;
			float angle = Utils.ExcuteReboundWallSkill(eulerAngles.y, o.contacts[0].point, m_Entity.GetComponent<SphereCollider>(), o.collider);
			Move_NextX = MathDxx.Sin(angle);
			Move_NextY = MathDxx.Cos(angle);
			UpdateMoveData();
			if (reboundcount == 3)
			{
				mEndStartTime = Updater.AliveTime + mEndTime;
			}
		}
		else
		{
			End();
		}
	}

	protected override void OnEnd()
	{
		m_Entity.m_AniCtrl.SetString("Skill", "Skill");
		UnityEngine.Object.Destroy(mCollision);
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
