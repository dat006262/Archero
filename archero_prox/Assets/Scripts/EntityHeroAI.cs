




using Dxx.Util;
using System;
using UnityEngine;

public class EntityHeroAI : EntityBase
{
	private AIBase m_AIBase;

	private float mMoveStartAngle;

	private float mMoveTime;

	private float mNextMoveMaxTime;

	private int width;

	private int height;

	private Vector2Int mPrevV = default(Vector2Int);

	private float mJoyMoveAngle;

	private float mJoyTime;

	private float mJoyMoveMaxTime;

	private bool bAttack;

	private JoyData mJoyData;

	public static EntityHeroAI mHeroAI
	{
		get;
		private set;
	}

	protected override void OnInitBefore()
	{
		SetEntityType(EntityType.Soldier);
	}

	protected override void OnInit()
	{
		base.OnInit();
		mHeroAI = this;
		GameLogic.Release.Entity.Add(this);
		m_MoveCtrl = new MoveControl();
		m_AttackCtrl = new AIHeroAttackControl();
		m_MoveCtrl.Init(this);
		m_AttackCtrl.Init(this);
		m_EntityData.HittedInterval = 0.5f;
		m_AttackCtrl.SetRotate(0f);
		mJoyData = default(JoyData);
		mJoyData.action = "Run";
		mJoyData.name = "MoveJoy";
	}

	protected override void StartInit()
	{
		InitWeapon(m_Data.WeaponID);
		width = GameLogic.Release.MapCreatorCtrl.width - 1;
		height = GameLogic.Release.MapCreatorCtrl.height - 1;
		RandomMove();
		WeaponBase weapon = m_Weapon;
		weapon.OnAttackStartEndAction = (Action)Delegate.Combine(weapon.OnAttackStartEndAction, new Action(OnAttackStartEnd));
		m_AIBase = new AIHero();
		m_AIBase.SetEntity(this);
		m_AIBase.Init();
	}

	protected override void InitCharacter()
	{
		m_EntityData.Init(this, m_Data.CharID);
	}

	private void DeInitAI()
	{
		if (m_AIBase != null)
		{
			m_AIBase.DeInit();
			m_AIBase = null;
		}
	}

	protected override void OnDeInitLogic()
	{
		DeInitAI();
		base.OnDeInitLogic();
	}

	protected override void UpdateProcess(float delta)
	{
		base.UpdateProcess(delta);
		m_AttackCtrl.UpdateProgress();
		UpdateMove();
	}

	protected override void UpdateFixed()
	{
		m_MoveCtrl.UpdateProgress();
	}

	protected override void OnChangeHP(EntityBase entity, long HP)
	{
	}

	private void FellGround()
	{
		GameLogic.Hold.Sound.PlayMonsterSkill(5000004, base.position);
	}

	private void RandomMove()
	{
		float num = mNextMoveMaxTime - (Updater.AliveTime - mMoveTime);
		if (num <= 0f)
		{
			mMoveTime = Updater.AliveTime;
			int num2 = GameLogic.Random(0, 100);
			if (num2 < 80)
			{
				mNextMoveMaxTime = GameLogic.Random(0.3f, 0.6f);
			}
			else
			{
				mNextMoveMaxTime = GameLogic.Random(0.6f, 1f);
			}
		}
		Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(base.position);
		if (roomXY.x == 0 && roomXY.y < height && roomXY.y > 0)
		{
			mMoveStartAngle = GameLogic.Random(0f, 180f);
		}
		else if (roomXY.x == 0 && roomXY.y == 0)
		{
			mMoveStartAngle = GameLogic.Random(90f, 180f);
		}
		else if (roomXY.x == 0 && roomXY.y == height)
		{
			mMoveStartAngle = GameLogic.Random(0f, 90f);
		}
		else if (roomXY.x == width && roomXY.x < height && roomXY.y > 0)
		{
			mMoveStartAngle = GameLogic.Random(180f, 360f);
		}
		else if (roomXY.x == width && roomXY.y == 0)
		{
			mMoveStartAngle = GameLogic.Random(180f, 270f);
		}
		else if (roomXY.x == width && roomXY.y == height)
		{
			mMoveStartAngle = GameLogic.Random(270f, 360f);
		}
		else if (roomXY.x > 0 && roomXY.x < width && roomXY.y == height)
		{
			mMoveStartAngle = GameLogic.Random(-90f, 90f);
		}
		else if (roomXY.x > 0 && roomXY.x < width && roomXY.y == 0)
		{
			mMoveStartAngle = GameLogic.Random(90f, 270f);
		}
		else
		{
			mMoveStartAngle = GameLogic.Random(0f, 360f);
		}
	}

	private void RandomJoyAngle()
	{
		if (Updater.AliveTime - mJoyTime > mJoyMoveMaxTime)
		{
			int num = GameLogic.Random(0, 100);
			if (num < 70)
			{
				mJoyMoveAngle = 0f;
				mJoyMoveMaxTime = GameLogic.Random(0f, 0.5f);
			}
			else
			{
				RandomAngleAndTime(out mJoyMoveAngle, out mJoyMoveMaxTime);
			}
			mJoyTime = Updater.AliveTime;
		}
	}

	private void RandomAngleAndTime(out float angle, out float time)
	{
		int num = GameLogic.Random(0, 100);
		if (num < 60)
		{
			angle = GameLogic.Random(0.5f, 1.5f) * (float)MathDxx.RandomSymbol();
			time = GameLogic.Random(0f, 0.2f);
		}
		else
		{
			angle = GameLogic.Random(2f, 4f) * (float)MathDxx.RandomSymbol();
			time = GameLogic.Random(0f, 0.15f);
		}
	}

	private void CheckJoyMove()
	{
		Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(base.position);
		if ((mPrevV.x > 0 && roomXY.x == 0) || (mPrevV.x < width && roomXY.x == width) || (mPrevV.y > 0 && roomXY.y == 0) || (mPrevV.y < height && roomXY.y == height))
		{
			RandomMove();
		}
		mPrevV = roomXY;
	}

	private void OnAttackStartEnd()
	{
		m_MoveCtrl.OnMoveStart(mJoyData);
		RandomMove();
		bAttack = false;
	}

	private void UpdateMove()
	{
		if (Updater.AliveTime - mMoveTime > mNextMoveMaxTime)
		{
			if (!bAttack)
			{
				bAttack = true;
				m_MoveCtrl.OnMoveEnd();
			}
		}
		else
		{
			CheckJoyMove();
			RandomJoyAngle();
			mMoveStartAngle += mJoyMoveAngle;
			mJoyData.UpdateDirectionByAngle(mMoveStartAngle);
			m_MoveCtrl.OnMoving(mJoyData);
		}
	}
}
