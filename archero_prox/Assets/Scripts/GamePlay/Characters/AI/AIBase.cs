




using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class AIBase : ActionBasic
{
	public class ActionChoose : ActionBase
	{
		public Func<bool> Condition;

		public ActionBase ResultTrue;

		public ActionBase ResultFalse;

		private bool bResult;

		protected override void OnInit()
		{
			bResult = Condition();
			ExcuteResultInit();
		}

		private void ExcuteResultInit()
		{
			if (bResult)
			{
				if (ResultTrue != null)
				{
					ResultTrue.Init();
					if (ResultTrue.IsEnd)
					{
						End();
					}
				}
				else
				{
					End();
				}
			}
			else if (ResultFalse != null)
			{
				ResultFalse.Init();
				if (ResultFalse.IsEnd)
				{
					End();
				}
			}
			else
			{
				End();
			}
		}

		private void ExcuteResultUpdate()
		{
			if (bResult)
			{
				if (ResultTrue != null)
				{
					ResultTrue.Update();
					if (ResultTrue.IsEnd)
					{
						End();
					}
				}
			}
			else if (ResultFalse != null)
			{
				ResultFalse.Update();
				if (ResultFalse.IsEnd)
				{
					End();
				}
			}
		}

		protected override void OnForceEnd()
		{
			if (bResult)
			{
				if (ResultTrue != null)
				{
					ResultTrue.ForceEnd();
				}
			}
			else if (ResultFalse != null)
			{
				ResultFalse.ForceEnd();
			}
		}

		protected override void OnUpdate()
		{
			ExcuteResultUpdate();
		}
	}

	public class ActionSequence : ActionBase
	{
		public List<ActionBase> list = new List<ActionBase>();

		private int count;

		private int index;

		protected override void OnInit()
		{
			index = 0;
		}

		protected override void OnUpdate()
		{
			if (index < count)
			{
				ActionBase actionBase = list[index];
				actionBase.Init();
				actionBase.Update();
				if (list[index].IsEnd)
				{
					index++;
				}
			}
			else
			{
				End();
			}
		}

		protected override void OnForceEnd()
		{
			if (index < count)
			{
				ActionBase actionBase = list[index];
				actionBase.ForceEnd();
			}
		}

		public void AddAction(ActionBase action)
		{
			list.Add(action);
			count++;
		}
	}

	public class ActionWaitRandom : ActionBase
	{
		public int min;

		public int max;

		private float startTime;

		private float waitTime;

		protected override void OnInit()
		{
			startTime = Updater.AliveTime;
			waitTime = (float)GetRandomInt(min, max) / 1000f;
		}

		protected override void OnUpdate()
		{
			if (Updater.AliveTime - startTime >= waitTime)
			{
				End();
			}
		}

		private int GetRandomInt(int min, int max)
		{
			return GameLogic.Random(min, max);
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionMove : ActionBase
	{
		public int moveId;

		public Action<int> action;

		protected override void OnInit()
		{
			if (action != null)
			{
				action(moveId);
			}
			End();
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionDivide : ActionBase
	{
		public int entityId;

		public int count;

		public Action<int, int> action;

		protected override void OnInit()
		{
			if (action != null)
			{
				action(entityId, count);
			}
			End();
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionCall : ActionBase
	{
		public class ActionCallData
		{
			public int entityId;
		}

		private ActionCallData data;

		public Action<ActionCallData> action;

		public void InitData(int entityId)
		{
			data = new ActionCallData
			{
				entityId = entityId
			};
		}

		protected override void OnInit()
		{
			if (action != null)
			{
				action(data);
			}
			End();
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionRotate : ActionBase
	{
		public float angle;

		private bool bRotate;

		protected override void OnInit()
		{
			bRotate = true;
		}

		protected override void OnUpdate()
		{
			if (bRotate)
			{
				m_Entity.m_AttackCtrl.RotateHero(angle);
				bRotate = false;
			}
			if (m_Entity.m_AttackCtrl.RotateOver())
			{
				End();
			}
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionRotateToEntity : ActionBase
	{
		public EntityBase target;

		private bool bRotate;

		protected override void OnInit()
		{
			bRotate = true;
		}

		protected override void OnUpdate()
		{
			if (bRotate)
			{
				m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(target.position - m_Entity.position));
				bRotate = false;
			}
			if (m_Entity.m_AttackCtrl.RotateOver())
			{
				End();
			}
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionRotateToPos : ActionBase
	{
		public Vector3 pos;

		private bool bRotate;

		protected override void OnInit()
		{
			bRotate = true;
		}

		protected override void OnUpdate()
		{
			if (bRotate)
			{
				m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(pos - m_Entity.position));
				bRotate = false;
			}
			if (m_Entity.m_AttackCtrl.RotateOver())
			{
				End();
			}
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionRotateTime : ActionBase
	{
		public EntityBase target;

		public float time;

		private float mTime;

		protected override void OnInit()
		{
			mTime = time;
		}

		protected override void OnUpdate()
		{
			if (mTime > 0f)
			{
				m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(target.position - m_Entity.position));
				mTime -= Updater.delta;
			}
			else
			{
				End();
			}
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionAttack : ActionBase
	{
		public int attackId;

		public AIBase m_AIBase;

		public bool bAttackSpecial;

		public bool bRotate = true;

		private bool bPlayAttack;

		private float test_time;

		protected override void OnInit()
		{
			bPlayAttack = false;
			m_AIBase.RemoveAttack();
		}

		protected override void OnUpdate()
		{
			if (m_Entity == null)
			{
				End();
				return;
			}
			if (m_Entity != null && !bPlayAttack)
			{
				if (bRotate)
				{
					m_Entity.m_AttackCtrl.RotateUpdate(m_Entity.m_HatredTarget);
				}
				if (m_Entity.m_AttackCtrl.RotateOver() || !bRotate)
				{
					bPlayAttack = true;
					Attack();
				}
			}
			if (bPlayAttack)
			{
				test_time += Updater.delta;
				if (test_time > 1f)
				{
					test_time -= 1f;
				}
				if (m_AIBase.GetAttackEnd())
				{
					End();
				}
			}
		}

		private void Attack()
		{
			if (!bAttackSpecial)
			{
				m_AIBase.Attack(attackId, bRotate);
			}
			else
			{
				m_AIBase.AttackSpecial(attackId, bRotate);
			}
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionChooseRandom : ActionBase
	{
		private List<ActionBase> actionList = new List<ActionBase>();

		private List<int> weightList = new List<int>();

		private int allWeight;

		private int actionCount;

		private int currentIndex = -1;

		protected override void OnInit()
		{
		}

		protected override void OnUpdate()
		{
			if (currentIndex == -1)
			{
				currentIndex = GetRandomWeight();
			}
			ActionBase actionBase = actionList[currentIndex];
			actionBase.Init();
			actionBase.Update();
			if (actionBase.IsEnd)
			{
				End();
				currentIndex = -1;
			}
		}

		public void AddAction(int weight, ActionBase action)
		{
			weightList.Add(weight);
			allWeight += weight;
			actionList.Add(action);
			actionCount++;
		}

		private int GetRandomWeight()
		{
			int num = GameLogic.Random(0, allWeight);
			int num2 = 0;
			for (int i = 0; i < actionCount; i++)
			{
				num2 += weightList[i];
				if (num < num2)
				{
					return i;
				}
			}
			return 0;
		}

		protected override void OnForceEnd()
		{
			if (currentIndex >= 0)
			{
				ActionBase actionBase = actionList[currentIndex];
				actionBase.ForceEnd();
			}
		}
	}

	public class ActionChooseIf : ActionBase
	{
		private List<ActionBase> list = new List<ActionBase>();

		private int count;

		private int index;

		protected override void OnInit()
		{
			index = count;
			int num = 0;
			while (true)
			{
				if (num < count)
				{
					ActionBase actionBase = list[num];
					if (actionBase.ConditionBase == null || actionBase.ConditionBase())
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			index = num;
		}

		protected override void OnUpdate()
		{
			if (index < count)
			{
				ActionBase actionBase = list[index];
				actionBase.Init();
				actionBase.Update();
				if (actionBase.IsEnd)
				{
					End();
				}
			}
			else
			{
				End();
			}
		}

		protected override void OnForceEnd()
		{
			if (index >= 0 && index < count)
			{
				ActionBase actionBase = list[index];
				actionBase.ForceEnd();
			}
		}

		public void AddAction(ActionBase action)
		{
			list.Add(action);
			count++;
		}
	}

	public class CallData
	{
		public int CallID;

		public int MaxAliveCount;

		public int MaxCount;

		public int perCount;

		public int radiusmin;

		public int radiusmax;

		public int CurAliveCount;

		public int CurAllCount;

		public CallData(int callid, int alivecount, int count, int percount, int radiusmin, int radiusmax)
		{
			CallID = callid;
			MaxAliveCount = alivecount;
			MaxCount = count;
			perCount = percount;
			this.radiusmin = radiusmin;
			this.radiusmax = radiusmax;
		}

		public void AddCall()
		{
			CurAllCount++;
			CurAliveCount++;
		}

		public void RemoveCall()
		{
			CurAliveCount--;
		}

		public bool GetCanCall()
		{
			return CurAliveCount < MaxAliveCount && CurAllCount < MaxCount;
		}

		public int GetCallCount()
		{
			int num = perCount;
			if (MaxCount - CurAllCount < num)
			{
				num = (MaxCount = CurAllCount);
			}
			if (MaxAliveCount - CurAliveCount < num)
			{
				num = MaxAliveCount - CurAliveCount;
			}
			return num;
		}
	}

	public EntityBase m_Entity;

	public EntityMonsterBase m_MonsterEntity;

	protected string ClassName;

	private int pClassID;

	private float actionTime;

	private EntityAttackBase mEntityAttack;

	protected float mRoomTime;

	private float mCreateNewTime;

	private float mStartTime;

	protected bool IsDelayTime = true;

	protected bool bReRandom;

	private Dictionary<int, CallData> mCallList = new Dictionary<int, CallData>();

	public int ClassID => pClassID;

	public void SetEntity(EntityBase entity)
	{
		m_Entity = entity;
		m_MonsterEntity = (entity as EntityMonsterBase);
	}

	protected sealed override void OnInit1()
	{
		ClassName = GetType().ToString();
		string s = ClassName.Substring(ClassName.Length - 4, 4);
		int.TryParse(s, out pClassID);
		actionTime = Updater.AliveTime;
		mRoomTime = GameLogic.Random(0.5f, 0.7f);
		mCreateNewTime = 0.3f;
		if (m_Entity.IsElite)
		{
			OnElite();
		}
		OnInitOnce();
		OnInit();
		if (bReRandom)
		{
			AddAction(GetActionDelegate(ReRandomAI));
		}
	}

	protected virtual void OnInitOnce()
	{
	}

	protected virtual void OnInit()
	{
	}

	protected virtual void OnElite()
	{
	}

	protected sealed override void OnDeInit()
	{
		base.OnDeInit();
		RemoveAttack();
		RemoveCurrentAction();
		OnAIDeInit();
	}

	protected virtual void OnAIDeInit()
	{
	}

	protected void ReRandomAI()
	{
		ActionClear();
		OnInit();
		AddAction(GetActionDelegate(ReRandomAI));
	}

	protected override void OnUpdate(float delta)
	{
		if ((bool)m_Entity && (!m_Entity.gameObject.activeInHierarchy || m_Entity.GetIsDead()))
		{
			return;
		}
		if (IsDelayTime)
		{
			if (m_Entity.bDivide || m_Entity.bCall)
			{
				if (Updater.AliveTime - actionTime < mCreateNewTime)
				{
					return;
				}
			}
			else if (Updater.AliveTime - actionTime < mRoomTime)
			{
				return;
			}
		}
		if (((bool)m_Entity && m_Entity.m_EntityData.IsDizzy()) || actionCount <= 0)
		{
			return;
		}
		ActionBase actionBase = actionList[base.actionIndex];
		int actionIndex = base.actionIndex;
		if (actionIndex == base.actionIndex && actionBase.IsEnd)
		{
			base.actionIndex++;
			if (base.actionIndex >= actionCount)
			{
				for (int i = 0; i < actionCount; i++)
				{
					actionList[i].Reset();
				}
			}
			base.actionIndex %= actionCount;
			actionBase = actionList[base.actionIndex];
			actionIndex = base.actionIndex;
		}
		actionBase.Init();
		actionBase.Update();
		if (actionIndex != base.actionIndex || !actionBase.IsEnd)
		{
			return;
		}
		base.actionIndex++;
		if (base.actionIndex >= actionCount)
		{
			for (int j = 0; j < actionCount; j++)
			{
				actionList[j].Reset();
			}
		}
		base.actionIndex %= actionCount;
	}

	protected ActionBase GetActionRotate(float angle)
	{
		ActionRotate actionRotate = new ActionRotate();
		actionRotate.m_Entity = m_Entity;
		actionRotate.angle = angle;
		return actionRotate;
	}

	protected ActionBase GetActionRotateToEntity(EntityBase target)
	{
		ActionRotateToEntity actionRotateToEntity = new ActionRotateToEntity();
		actionRotateToEntity.m_Entity = m_Entity;
		actionRotateToEntity.target = target;
		return actionRotateToEntity;
	}

	protected ActionBase GetActionRotateToPos(Vector3 pos)
	{
		ActionRotateToPos actionRotateToPos = new ActionRotateToPos();
		actionRotateToPos.m_Entity = m_Entity;
		actionRotateToPos.pos = pos;
		return actionRotateToPos;
	}

	protected ActionWait GetActionWait(string name, int waitTime)
	{
		ActionWait actionWait = new ActionWait();
		actionWait.name = name;
		actionWait.waitTime = (float)waitTime / 1000f;
		actionWait.m_Entity = m_Entity;
		return actionWait;
	}

	protected ActionWaitRandom GetActionWaitRandom(string name, int min, int max)
	{
		ActionWaitRandom actionWaitRandom = new ActionWaitRandom();
		actionWaitRandom.name = name;
		actionWaitRandom.min = min;
		actionWaitRandom.max = max;
		actionWaitRandom.m_Entity = m_Entity;
		return actionWaitRandom;
	}

	protected ActionDivide GetActionDivide(string name, int entityId, int count)
	{
		ActionDivide actionDivide = new ActionDivide();
		actionDivide.name = name;
		actionDivide.entityId = entityId;
		actionDivide.count = count;
		actionDivide.action = Divide;
		actionDivide.m_Entity = m_Entity;
		return actionDivide;
	}

	protected ActionDelegate GetActionDelegate(Action action)
	{
		ActionDelegate actionDelegate = new ActionDelegate();
		actionDelegate.action = action;
		return actionDelegate;
	}

	protected ActionBase GetActionWaitDelegate(int time, Action action)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionWait(string.Empty, time));
		actionSequence2.AddAction(GetActionDelegate(action));
		return actionSequence2;
	}

	protected ActionDelegate GetActionRemoveMove()
	{
		return GetActionDelegate(delegate
		{
			RemoveMove();
		});
	}

	public void RemoveMove()
	{
		if (actionCount > 0)
		{
			ActionBase actionBase = actionList[actionIndex];
			if (actionBase is AIMoveBase)
			{
				actionBase.ForceEnd();
			}
		}
	}

	protected void RemoveCurrentAction()
	{
		if (actionCount > 0)
		{
			ActionBase actionBase = actionList[actionIndex];
			actionBase.ForceEnd();
		}
	}

	public void Divide(int entityid, int count)
	{
		m_Entity.PlayEffect(3100009, m_Entity.m_Body.EffectMask.transform.position);
		GameLogic.Release.Entity.Remove(m_Entity);
		Vector3 position = m_Entity.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		float z = position2.z;
		if (count <= 1)
		{
			EntityBase entityBase = GameLogic.Release.MapCreatorCtrl.CreateDivideEntity(m_Entity, entityid, x, z);
			entityBase.DivideID = m_Entity.DivideID;
			return;
		}
		float num = 1f;
		float num2 = 360f / (float)count;
		float num3 = -180f - num2;
		for (int i = 0; i < count; i++)
		{
			num3 += num2;
			float x2 = MathDxx.Cos(num3) * num;
			float z2 = MathDxx.Sin(num3) * num;
			EntityBase entityBase2 = GameLogic.Release.MapCreatorCtrl.CreateDivideEntity(m_Entity, entityid, x, z);
			entityBase2.DivideID = m_Entity.DivideID;
			entityBase2.DivideAction(x2, z2);
		}
	}

	protected bool GetIsAlive()
	{
		if ((bool)m_Entity && !m_Entity.GetIsDead())
		{
			return true;
		}
		return false;
	}

	protected bool GetHaveHatred()
	{
		return m_Entity.m_HatredTarget != null;
	}

	public void RemoveCall(RemoveCallData data)
	{
		RemoveCallCount(data.entityId);
		if (IsCallStand(data.entityId))
		{
			GameLogic.Release.MapCreatorCtrl.CallPositionRecover(data.deadpos);
		}
	}

	public void DeadBefore()
	{
		OnDeadBefore();
	}

	protected virtual void OnDeadBefore()
	{
	}

	protected override void OnActionClear()
	{
		actionIndex = 0;
	}

	private void RemoveAttack()
	{
		if (mEntityAttack != null)
		{
			mEntityAttack.UnInstall();
			mEntityAttack = null;
		}
	}

	public void Attack(int AttackID, bool bRotate)
	{
		RemoveMove();
		RemoveAttack();
		mEntityAttack = new EntityAttack();
		mEntityAttack.SetRotate(bRotate);
		mEntityAttack.Init(m_Entity, AttackID);
	}

	public void AttackSpecial(int AttackID, bool bRotate)
	{
		RemoveMove();
		Type type = Type.GetType(Utils.GetString("EntityAttack", AttackID));
		mEntityAttack = (type.Assembly.CreateInstance(Utils.GetString("EntityAttack", AttackID)) as EntityAttackBase);
		mEntityAttack.SetRotate(bRotate);
		mEntityAttack.Init(m_Entity, AttackID);
	}

	public bool GetAttackEnd()
	{
		if (mEntityAttack != null && !mEntityAttack.GetIsEnd())
		{
			return false;
		}
		return !m_Entity.m_AttackCtrl.GetAttacking();
	}

	protected ActionDelegate GetActionRemoveAttack()
	{
		return GetActionDelegate(delegate
		{
			RemoveAttack();
		});
	}

	protected ActionSequence GetActionAttackWait(int attackID, int waittime, int waitmaxtime = -1)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack(string.Empty, attackID));
		if (waitmaxtime == -1)
		{
			waitmaxtime = waittime;
		}
		actionSequence2.AddAction(GetActionWaitRandom(string.Empty, waittime, waitmaxtime));
		return actionSequence2;
	}

	protected ActionAttack GetActionAttack(string name, int attackId, bool rotate = true)
	{
		ActionAttack actionAttack = new ActionAttack();
		actionAttack.name = name;
		actionAttack.attackId = attackId;
		actionAttack.bAttackSpecial = false;
		actionAttack.bRotate = rotate;
		actionAttack.m_AIBase = this;
		actionAttack.m_Entity = m_Entity;
		return actionAttack;
	}

	protected ActionAttack GetActionAttackSpecial(string name, int attackId, bool rotate = true)
	{
		ActionAttack actionAttack = new ActionAttack();
		actionAttack.name = name;
		actionAttack.attackId = attackId;
		actionAttack.bAttackSpecial = true;
		actionAttack.bRotate = rotate;
		actionAttack.m_AIBase = this;
		actionAttack.m_Entity = m_Entity;
		return actionAttack;
	}

	protected void InitCallData(CallData data)
	{
		if (mCallList.ContainsKey(data.CallID))
		{
			mCallList[data.CallID] = data;
		}
		else
		{
			mCallList.Add(data.CallID, data);
		}
	}

	protected void InitCallData(int callid, int alivecount, int count, int percount, int radiusmin, int radiusmax)
	{
		if (mCallList.TryGetValue(callid, out CallData value))
		{
			value.CallID = callid;
			value.MaxAliveCount = alivecount;
			value.MaxCount = count;
			value.perCount = percount;
			value.radiusmin = radiusmin;
			value.radiusmax = radiusmax;
		}
		else
		{
			value = new CallData(callid, alivecount, count, percount, radiusmin, radiusmax);
			mCallList.Add(callid, value);
		}
	}

	protected void AddCallCount(int callid)
	{
		if (mCallList.TryGetValue(callid, out CallData value))
		{
			value.AddCall();
		}
	}

	protected void RemoveCallCount(int callid)
	{
		if (mCallList.TryGetValue(callid, out CallData value))
		{
			value.RemoveCall();
		}
	}

	protected bool GetCanCall(object callid)
	{
		int key = (int)callid;
		if (mCallList.TryGetValue(key, out CallData value))
		{
			return value.GetCanCall();
		}
		return false;
	}

	protected int GetCallCount(int callid)
	{
		if (mCallList.TryGetValue(callid, out CallData value))
		{
			return value.GetCallCount();
		}
		return 0;
	}

	protected int GetAliveCount(int callid, bool over = false)
	{
		if (mCallList.TryGetValue(callid, out CallData value))
		{
			return value.CurAliveCount;
		}
		return 0;
	}

	protected ActionBase GetActionCall(int entityId)
	{
		return GetActionCallInternal(entityId, Call);
	}

	protected ActionBase GetActionCallInternal(int entityId, Action<ActionCall.ActionCallData> call)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.ConditionBase = GetIsAlive;
		actionSequence.AddAction(new ActionDelegate
		{
			action = delegate
			{
				m_Entity.m_AniCtrl.SendEvent("Call");
			}
		});
		float animationTime = m_Entity.mAniCtrlBase.GetAnimationTime("Call");
		actionSequence.AddAction(new ActionWait
		{
			waitTime = animationTime * 0.4f
		});
		ActionCall actionCall = new ActionCall();
		actionCall.InitData(entityId);
		actionCall.action = call;
		actionSequence.AddAction(actionCall);
		actionSequence.AddAction(new ActionWait
		{
			waitTime = animationTime * 0.6f
		});
		return actionSequence;
	}

	private Vector3 GetRandomCall(int entityid, CallData data)
	{
		Vector3 vector;
		if (IsCallStand(entityid))
		{
			GameLogic.Release.MapCreatorCtrl.RandomCallSide(m_Entity, data.radiusmin, data.radiusmax, out float endx, out float endz);
			vector = new Vector3(endx, 0f, endz);
		}
		else
		{
			GameLogic.Release.MapCreatorCtrl.RandomItemSides(m_Entity, data.radiusmin, data.radiusmax, out float endx2, out float endz2);
			vector = new Vector3(endx2, 0f, endz2);
		}
		Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(vector);
		while (!GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY))
		{
			if (IsCallStand(entityid))
			{
				GameLogic.Release.MapCreatorCtrl.RandomCallSide(m_Entity, data.radiusmin, data.radiusmax, out float endx3, out float endz3);
				vector = new Vector3(endx3, 0f, endz3);
			}
			else
			{
				GameLogic.Release.MapCreatorCtrl.RandomItemSides(m_Entity, data.radiusmin, data.radiusmax, out float endx4, out float endz4);
				vector = new Vector3(endx4, 0f, endz4);
			}
			roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(vector);
		}
		return vector;
	}

	protected void AddActionAddCall(int entityId, int bulletid)
	{
		if (mCallList.TryGetValue(entityId, out CallData value))
		{
			Vector3 pos = GetRandomCall(entityId, value);
			AddActionDelegate(delegate
			{
				AddCallCount(entityId);
				if ((bool)m_MonsterEntity)
				{
					m_MonsterEntity.SetCallID(entityId, pos);
				}
			});
			if ((bool)m_MonsterEntity)
			{
				AddAction(GetActionRotateToPos(pos));
			}
			AddAction(GetActionAttack(string.Empty, bulletid, rotate: false));
		}
	}

	public void Call(ActionCall.ActionCallData data)
	{
		int callCount = GetCallCount(data.entityId);
		for (int i = 0; i < callCount; i++)
		{
			if (IsCallStand(data.entityId))
			{
				CallStand(data);
			}
			else
			{
				CallMove(data);
			}
			AddCallCount(data.entityId);
		}
	}

	public void CallOne(Vector3 pos)
	{
		int callID = m_Entity.CallID;
		CallOneInternal(callID, pos, showcalleffect: true);
	}

	public void CallOne(Vector3 pos, bool showeffect)
	{
		int callID = m_Entity.CallID;
		CallOneInternal(callID, pos, showeffect);
	}

	private void CallOne(int callid, Vector3 pos)
	{
		CallOneInternal(callid, pos, showcalleffect: true);
	}

	private bool IsCallStand(int entityid)
	{
		return LocalModelManager.Instance.Character_Char.GetBeanById(entityid).Speed == 0;
	}

	private void CallStand(ActionCall.ActionCallData data)
	{
		if (mCallList.TryGetValue(data.entityId, out CallData value) && GameLogic.Release.MapCreatorCtrl.RandomCallSide(m_Entity, value.radiusmin, value.radiusmax, out float endx, out float endz))
		{
			float x = endx;
			Vector3 position = m_Entity.m_Body.EffectMask.transform.position;
			Vector3 pos = new Vector3(x, position.y, endz);
			CallOne(data.entityId, pos);
		}
	}

	private void CallMove(ActionCall.ActionCallData data)
	{
		if (mCallList.TryGetValue(data.entityId, out CallData value))
		{
			GameLogic.Release.MapCreatorCtrl.RandomItemSides(m_Entity, value.radiusmin, value.radiusmax, out float endx, out float endz);
			float x = endx;
			Vector3 position = m_Entity.m_Body.EffectMask.transform.position;
			Vector3 pos = new Vector3(x, position.y, endz);
			CallOne(data.entityId, pos);
		}
	}

	protected void CallOneInternal(int callid, Vector3 pos, bool showcalleffect)
	{
		EntityMonsterBase entityMonsterBase = GameLogic.Release.MapCreatorCtrl.CreateEntityCall(callid, pos.x, pos.z);
		if (showcalleffect)
		{
			GameLogic.PlayEffect(3100008, new Vector3(pos.x, 0f, pos.z));
		}
		entityMonsterBase.SetParent(m_Entity);
	}
}
