




using DG.Tweening;
using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class EntityHero : EntityBase
{
	public class EventMoveStartData
	{
		public Action mEvent;

		public float delay;
	}

	public class EventMovingData
	{
		public Action<JoyData> mEvent;

		public float delay;
	}

	public class EventMoveEndData
	{
		public Action mEvent;

		public float delay;
	}

	private class LevelUpData
	{
		public string name;

		public long value;
	}

	public Transform Coin_Absorb;

	private int mAbsorb = 1;

	private List<int> equipskills;

	private static Dictionary<int, int> AbsorbDic = new Dictionary<int, int>
	{
		{
			3001,
			1200001
		},
		{
			3002,
			1200001
		},
		{
			2001,
			1200004
		},
		{
			2002,
			1200004
		},
		{
			2003,
			1200004
		},
		{
			2004,
			1200004
		},
		{
			1001,
			1200002
		}
	};

	private Dictionary<int, float> mAbsorbTimes = new Dictionary<int, float>();

	private float mAbsorbInterval = 0.3f;

	public List<Sequence> mSequenceList = new List<Sequence>();

	public List<EventMoveStartData> mMoveStartList = new List<EventMoveStartData>();

	public List<EventMovingData> mMovingList = new List<EventMovingData>();

	public List<EventMoveEndData> mMoveEndList = new List<EventMoveEndData>();

	private List<LevelUpData> mLevelUps = new List<LevelUpData>();

	private float mBubbleDistance;

	private bool bFrontShield;

	private List<Skill_slotin> mSkillList = new List<Skill_slotin>();

	private List<int> mLearnSkillList = new List<int>();

	private List<int> mExtraLearnSkillList = new List<int>();

	private int WeightAll;

	private SuperSkillBase mSuperSkill;

	public GameObject FootDirection
	{
		get;
		private set;
	}

	protected override void OnInitBefore()
	{
		GameLogic.Release.Entity.SetSelf(this);
	}

	protected override void OnInit()
	{
		base.OnInit();
		m_MoveCtrl = new MoveControlHero();
		m_AttackCtrl = new HeroAttackControl();
		m_MoveCtrl.Init(this);
		m_AttackCtrl.Init(this);
		m_EntityData.HittedInterval = 0.5f;
		SetSuperArmor(value: true);
		Coin_Absorb = base.transform.Find("Coin_Absorb");
		m_AttackCtrl.SetRotate(0f);
		OnLevelUp = (Action<int>)Delegate.Combine(OnLevelUp, new Action<int>(OnLevelUpEvent));
	}

	protected override void StartInit()
	{
		m_EntityData.InitExp();
		m_EntityData.ExcuteAttributes("AttackSpeed%", (long)(m_EntityData.attribute.AttackSpeed.Value * 10000f));
		InitEquipSkills();
		int num = LocalSave.Instance.Equip_GetWeapon();
		if (num == 0)
		{
			num = 1000;
		}
		InitWeapon(num);
		InitCards();
		InitSkillList();
	}

	protected override void OnDeInitLogic()
	{
		DeInitSuperSkill();
		base.OnDeInitLogic();
	}

	protected void InitEquipSkills()
	{
		equipskills = LocalSave.Instance.Equip_GetSkills();
		int i = 0;
		for (int count = equipskills.Count; i < count; i++)
		{
			AddSkillOverLying(equipskills[i]);
		}
	}

	protected override void OnCreateModel()
	{
		bool flag = true;
		GameMode mode = GameLogic.Hold.BattleData.GetMode();
		if (mode == GameMode.eBomberman)
		{
			flag = false;
		}
		if (flag)
		{
			CreateFootDirection();
		}
	}

	private void CreateFootDirection()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("Game/Player/FootDirection"));
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		gameObject.transform.localScale = Vector3.one;
		FootDirection = gameObject;
	}

	protected override void InitCharacter()
	{
		m_EntityData.Init(this, m_Data.CharID);
	}

	protected override void UpdateProcess(float delta)
	{
		base.UpdateProcess(delta);
		if (m_AttackCtrl != null)
		{
			m_AttackCtrl.UpdateProgress();
		}
	}

	protected override void UpdateFixed()
	{
		if (m_MoveCtrl != null)
		{
			m_MoveCtrl.UpdateProgress();
		}
	}

	protected override void OnTriggerEnterExtra(Collider o)
	{
		int layer = o.gameObject.layer;
		string tag = o.gameObject.tag;
		if (layer == LayerManager.Map && tag == "Map_Door")
		{
			TriggerDoor(o.gameObject);
		}
	}

	private void TriggerDoor(GameObject o)
	{
		if (GameLogic.Release.Mode.RoomGenerate.IsBattleLoad())
		{
			WindowUI.ShowLoading(delegate
			{
				GameLogic.Release.Game.SetRoomState(RoomState.Throughing);
				GameLogic.Release.Mode.EnterDoor();
			}, delegate
			{
				GameLogic.Release.Game.JoyEnable(enable: false);
				CameraControlM.Instance.SetCameraPosition(GameLogic.Self.position - new Vector3(0f, 0f, 1.2f));
				CameraControlM.Instance.SetCameraSpeed(5f);
			}, delegate
			{
				GameLogic.Release.Game.JoyEnable(enable: true);
				CameraControlM.Instance.ResetCameraSpeed();
				GameLogic.Release.Game.SetRoomState(RoomState.Runing);
			});
		}
	}

	public void AbsorbEquips(EquipBase good)
	{
		GameObject gameObject = GameLogic.EffectGet("Effect/AbsorbExp");
		gameObject.transform.SetParent(m_Body.EffectMask.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		good.GetGoods(this);
		if (good.m_Data.GetSound != 0)
		{
			GameLogic.Hold.Sound.PlayGetGoods(good.m_Data.GetSound, base.transform.position);
		}
	}

	private float GetAbsorbTime(int foodid)
	{
		if (mAbsorbTimes.TryGetValue(foodid, out float value))
		{
			return value;
		}
		return 0f;
	}

	private void UpdateAbsorbTime(int foodid)
	{
		if (!mAbsorbTimes.ContainsKey(foodid))
		{
			mAbsorbTimes.Add(foodid, Time.time);
		}
		else
		{
			mAbsorbTimes[foodid] = Time.time;
		}
	}

	public void AbsorbFoods(FoodBase good)
	{
		int value = 1200001;
		int num;
		if (good is FoodEquipBase)
		{
			num = 9999;
			GameLogic.Hold.BattleData.AddEquip(good.GetData() as LocalSave.EquipOne);
			value = 1200003;
		}
		else
		{
			num = good.FoodID;
			AbsorbDic.TryGetValue(num, out value);
			good.GetGoods(this);
			if (Time.time - GetAbsorbTime(num) > mAbsorbInterval && good.m_Data.GetSound != 0)
			{
				GameLogic.Hold.Sound.PlayGetGoods(good.m_Data.GetSound, base.transform.position);
			}
		}
		if (Time.time - GetAbsorbTime(num) > mAbsorbInterval)
		{
			PlayEffect(value);
			UpdateAbsorbTime(num);
		}
	}

	protected override void OnChangeHP(EntityBase entity, long HP)
	{
		if (GetIsDead())
		{
			Reborn_Dead();
		}
		LocalSave.Instance.BattleIn_UpdateHP(m_EntityData.CurrentHP);
		if (GameLogic.Release.Mode != null && GameLogic.Release.Mode.RoomGenerate != null)
		{
			GameLogic.Release.Mode.RoomGenerate.PlayerHitted(HP);
		}
	}

	protected override void OnDeInit()
	{
		Reborn_DeadEndInternal();
	}

	private void FellGround()
	{
		GameLogic.Hold.Sound.PlayMonsterSkill(5000004, base.position);
	}

	public override void DeadCallBack()
	{
		m_AniCtrl.SendEvent("Dead");
		if (mDeadSeq != null)
		{
			mDeadSeq.Kill();
			mDeadSeq = null;
		}
		mDeadSeq = DOTween.Sequence().AppendInterval(1.5f).AppendCallback(delegate
		{
			GameMode mode = GameLogic.Hold.BattleData.GetMode();
			if (mode == GameMode.eMatchDefenceTime)
			{
				Facade.Instance.SendNotification("MatchDefenceTime_me_dead");
			}
			else if (GameLogic.Hold.BattleData.GetCanReborn())
			{
				WindowUI.ShowWindow(WindowID.WindowID_BattleReborn);
			}
			else
			{
				Reborn_Dead();
				Reborn_DeadEnd();
			}
		});
	}

	public override void SetCollider(bool enable)
	{
		base.SetCollider(enable);
	}

	public void SetAbsorb(bool enable)
	{
		mAbsorb += (enable ? 1 : (-1));
		if ((bool)Coin_Absorb)
		{
			if (mAbsorb == 1 && enable)
			{
				Coin_Absorb.gameObject.SetActive(value: true);
			}
			else if (mAbsorb == 0 && !enable)
			{
				Coin_Absorb.gameObject.SetActive(value: false);
			}
		}
	}

	public bool GetAbsorbEnable()
	{
		return mAbsorb > 0;
	}

	public void SetAbsorbRangeMax(bool value)
	{
	}

	protected override void OnSetFlying(bool fly)
	{
	}

	public void LevelUp()
	{
		GameNode.CameraShake(CameraShakeType.Crit);
		PlayEffect(3000012);
	}

	public void AddMoveStart(EventMoveStartData data)
	{
		if (!mMoveStartList.Contains(data))
		{
			mMoveStartList.Add(data);
		}
	}

	public void RemoveMoveStart(Action callback)
	{
		int num = 0;
		int count = mMoveStartList.Count;
		while (true)
		{
			if (num < count)
			{
				if (mMoveStartList[num].mEvent == callback)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		mMoveStartList.RemoveAt(num);
	}

	public void AddMoving(EventMovingData data)
	{
		if (!mMovingList.Contains(data))
		{
			mMovingList.Add(data);
		}
	}

	public void RemoveMoving(Action<JoyData> callback)
	{
		int num = 0;
		int count = mMovingList.Count;
		while (true)
		{
			if (num < count)
			{
				if (mMovingList[num].mEvent == callback)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		mMovingList.RemoveAt(num);
	}

	public void AddMoveEnd(EventMoveEndData data)
	{
		if (!mMoveEndList.Contains(data))
		{
			mMoveEndList.Add(data);
		}
	}

	public void RemoveMoveEnd(Action callback)
	{
		int num = 0;
		int count = mMoveEndList.Count;
		while (true)
		{
			if (num < count)
			{
				if (mMoveEndList[num].mEvent == callback)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		mMoveEndList.RemoveAt(num);
	}

	public void DoMoveStart()
	{
		int i = 0;
		for (int count = mMoveStartList.Count; i < count; i++)
		{
			int index = i;
			float delay = mMoveStartList[index].delay;
			if (delay <= 0f)
			{
				mMoveStartList[index].mEvent();
				continue;
			}
			Sequence item = DOTween.Sequence().AppendInterval(delay).AppendCallback(delegate
			{
				mMoveStartList[index].mEvent();
			});
			mSequenceList.Add(item);
		}
	}

	public void DoMoving(JoyData data)
	{
		int i = 0;
		for (int count = mMovingList.Count; i < count; i++)
		{
			int index = i;
			float delay = mMovingList[index].delay;
			if (delay <= 0f)
			{
				mMovingList[index].mEvent(data);
				continue;
			}
			Sequence item = DOTween.Sequence().AppendInterval(delay).AppendCallback(delegate
			{
				mMovingList[index].mEvent(data);
			});
			mSequenceList.Add(item);
		}
	}

	public void DoMoveEnd()
	{
		int i = 0;
		for (int count = mMoveEndList.Count; i < count; i++)
		{
			int index = i;
			float delay = mMoveEndList[index].delay;
			if (delay <= 0f)
			{
				mMoveEndList[index].mEvent();
				continue;
			}
			Sequence item = DOTween.Sequence().AppendInterval(delay).AppendCallback(delegate
			{
				mMoveEndList[index].mEvent();
			});
			mSequenceList.Add(item);
		}
	}

	private void Action_OngotoNextRoom()
	{
		int i = 0;
		for (int count = mSequenceList.Count; i < count; i++)
		{
			Sequence sequence = mSequenceList[i];
			if (sequence != null)
			{
				sequence.Kill();
				sequence = null;
			}
		}
		mSequenceList.Clear();
	}

	private void InitCards()
	{
		List<LocalSave.CardOne> wearCards = LocalSave.Instance.GetWearCards();
		int i = 0;
		for (int count = wearCards.Count; i < count; i++)
		{
			InitCard(wearCards[i]);
		}
	}

	private void InitCard(LocalSave.CardOne one)
	{
	}

	public void ExcuteLevelUpAttributes(string name, long value)
	{
		mLevelUps.Add(new LevelUpData
		{
			name = name,
			value = value
		});
	}

	public void OnLevelUpEvent(int level)
	{
		int i = 0;
		for (int count = mLevelUps.Count; i < count; i++)
		{
			LevelUpData levelUpData = mLevelUps[i];
			m_EntityData.ExcuteAttributes(levelUpData.name, levelUpData.value);
		}
	}

	protected override void OnSetPositionBy(Vector3 pos)
	{
		DoRunBubble(pos.magnitude);
	}

	public void DoRunBubble(float dis)
	{
		mBubbleDistance += dis;
		if (mBubbleDistance >= 2f)
		{
			mBubbleDistance -= 2f;
			showRunBubble();
		}
	}

	private void showRunBubble()
	{
		Transform transform = GameLogic.EffectGet("Game/Player/RunBubble").transform;
		transform.transform.SetParent(m_Body.FootMask.transform);
		transform.transform.localPosition = Vector3.zero;
		transform.transform.localScale = Vector3.one;
		transform.transform.localRotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
		transform.transform.SetParent(GameNode.m_PoolParent);
	}

	protected override HittedData OnHittedData(HittedData data, bool bulletthrough, float bulletangle)
	{
		bFrontShield = m_EntityData.GetFrontShield();
		if (!bFrontShield)
		{
			return data;
		}
		if (bulletthrough)
		{
			return data;
		}
		Vector3 eulerAngles = base.eulerAngles;
		float y = eulerAngles.y;
		if (MathDxx.Abs(y - bulletangle) < 90f || MathDxx.Abs(y - bulletangle + 360f) < 90f || MathDxx.Abs(y - bulletangle - 360f) < 90f)
		{
			return data;
		}
		data.type = EHittedType.eDefence;
		data.hitratio = 0.4f;
		return data;
	}

	protected override void OnDeadBefore()
	{
	}

	public void DoReborn()
	{
		if (GameLogic.Hold.BattleData.GetCanReborn())
		{
			DoRebornInternal();
		}
		else
		{
			Reborn_DeadEnd();
		}
	}

	public void DoRebornInternal()
	{
		Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eReborn);
		GameLogic.Hold.BattleData.UseReborn();
		if ((bool)FootDirection)
		{
			FootDirection.SetActive(value: true);
		}
		GameLogic.Release.Game.JoyEnable(enable: true);
		GameLogic.Hold.Sound.PlayWalk();
		m_EntityData.ChangeHP(null, m_EntityData.MaxHP);
		m_AniCtrl.Reborn();
		m_MoveCtrl.OnMoveEnd();
		m_AttackCtrl.Reset();
		ShowHP(show: true);
		SetCollider(enable: true);
		GameLogic.SendBuff(this, this, 1003);
		SetAbsorb(enable: true);
		LocalSave.Instance.BattleIn_SetHaveBattle(value: true);
	}

	public void Reborn_Dead()
	{
		Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eDead);
		SetAbsorb(enable: false);
		ShowHP(show: false);
		LocalSave.Instance.BattleIn_SetHaveBattle(value: false);
		m_AniCtrl.SendEvent("Dead");
		mAction.ActionClear();
		mAction.AddActionWaitDelegate(0.5f, FellGround);
		m_AniCtrl.DeadDown();
		GameLogic.Release.Game.JoyEnable(enable: false);
		GameLogic.Hold.Sound.StopWalk();
		if ((bool)FootDirection)
		{
			FootDirection.SetActive(value: false);
		}
		GameLogic.Release.Mode.PlayerDead();
		SdkManager.send_deadlayer(GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID());
		m_MoveCtrl.ResetRigidBody();
	}

	public void Reborn_DeadEnd()
	{
		Reborn_DeadEndInternal();
		UnityEngine.Debug.Log("Reborn_DeadEnd Reborn_DeadEnd");
		Facade.Instance.SendNotification("BATTLE_GAMEOVER");
	}

	private void Reborn_DeadEndInternal()
	{
		base.OnDeadBefore();
		LocalSave.Instance.BattleIn_DeInit();
		GameLogic.Release.Game.RemoveJoy();
		GameLogic.Hold.BattleData.SetWin(value: false);
		if ((bool)m_HPSlider)
		{
			m_HPSlider.DeInit();
		}
		DeInitLogic();
		DeInitMesh(showeffect: true);
	}

	public void OnGotoNextRoom()
	{
		Action_OngotoNextRoom();
	}

	private void InitSkillList()
	{
		int num = 0;
		int key = LocalSave.Instance.Equip_GetWeapon();
		Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById(key);
		if (beanById != null)
		{
			switch (beanById.Type)
			{
			case 101:
				num = 1000036;
				break;
			case 102:
				num = 1000033;
				break;
			case 103:
				num = 1000032;
				break;
			case 104:
				num = 1000031;
				break;
			}
		}
		IList<Skill_slotin> allBeans = LocalModelManager.Instance.Skill_slotin.GetAllBeans();
		WeightAll = 0;
		IEnumerator<Skill_slotin> enumerator = allBeans.GetEnumerator();
		int num2 = 0;
		while (enumerator.MoveNext())
		{
			Skill_slotin current = enumerator.Current;
			if (current.SkillID != num && !equipskills.Contains(current.SkillID) && current.UnlockStage <= GameLogic.Hold.BattleData.Level_CurrentStage && current.Weight > 0)
			{
				mSkillList.Add(current);
				WeightAll += current.Weight;
				num2++;
			}
		}
	}

	public List<int> GetSkill9()
	{
		List<Skill_slotin> list = new List<Skill_slotin>();
		List<int> list2 = new List<int>();
		int num = WeightAll;
		for (int i = 0; i < 9; i++)
		{
			int num2 = GameLogic.Random(0, num);
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int j = 0;
			for (int count = mSkillList.Count; j < count; j++)
			{
				num4 = mSkillList[j].Weight;
				if (num2 < num4)
				{
					num3 = mSkillList[j].SkillID;
					list.Add(mSkillList[j]);
					list2.Add(num3);
					mSkillList.RemoveAt(j);
					break;
				}
				num2 -= num4;
			}
			num -= num4;
		}
		int k = 0;
		for (int count2 = list.Count; k < count2; k++)
		{
			mSkillList.Add(list[k]);
		}
		return list2;
	}

	public int GetRandomSkill()
	{
		int index = UnityEngine.Random.Range(0, mSkillList.Count);
		return mSkillList[index].SkillID;
	}

	public void LearnSkill(int skillid)
	{
		if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel && mExtraLearnSkillList.Count < GameLogic.Self.m_EntityData.attribute.ExtraSkill.Value)
		{
			LearnExtraSkill(skillid);
			return;
		}
		mLearnSkillList.Add(skillid);
		OnLearnSkill(skillid);
	}

	public void LearnExtraSkill(int skillid)
	{
		mExtraLearnSkillList.Add(skillid);
		OnLearnSkill(skillid);
	}

	private void OnLearnSkill(int skillid)
	{
		Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eLearnSkill, skillid);
		AddSkillInternal(skillid);
		int num = 0;
		int count = mSkillList.Count;
		while (true)
		{
			if (num < count)
			{
				int skillID = mSkillList[num].SkillID;
				if (skillID == skillid)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		WeightAll -= mSkillList[num].Weight;
		mSkillList.RemoveAt(num);
	}

	public int GetLearnSkillCount()
	{
		return mLearnSkillList.Count;
	}

	public List<int> GetFirstSkill9()
	{
		int num = 0;
		IList<Skill_slotfirst> allBeans = LocalModelManager.Instance.Skill_slotfirst.GetAllBeans();
		List<Skill_slotin> list = new List<Skill_slotin>();
		List<int> list2 = new List<int>();
		int i = 0;
		for (int count = allBeans.Count; i < count; i++)
		{
			int skillID = allBeans[i].SkillID;
			Skill_slotin beanById = LocalModelManager.Instance.Skill_slotin.GetBeanById(skillID);
			list.Add(beanById);
			num += beanById.Weight;
		}
		for (int j = 0; j < 9; j++)
		{
			int num2 = GameLogic.Random(0, num);
			int k = 0;
			for (int count2 = list.Count; k < count2; k++)
			{
				Skill_slotin skill_slotin = list[k];
				if (num2 < skill_slotin.Weight)
				{
					list2.Add(skill_slotin.SkillID);
					num -= skill_slotin.Weight;
					list.RemoveAt(k);
					break;
				}
				num2 -= skill_slotin.Weight;
			}
		}
		SdkManager.Bugly_Report(list2.Count == 9, "EntityHero_Skill.cs", Utils.GetString("GetFirstSkill9 count = ", list2.Count));
		return list2;
	}

	private void InitSuperSkill(int skillid)
	{
		ScrollCircle.OnDoubleClick = OnDoubleClick;
		RemoveOldSuperSkill();
		string typeName = Utils.FormatString("SuperSkill{0}", skillid);
		Type type = System.Type.GetType(typeName);
		SuperSkillBase superSkillBase = mSuperSkill = (type.Assembly.CreateInstance(typeName) as SuperSkillBase);
		mSuperSkill.Init(this);
	}

	private void DeInitSuperSkill()
	{
		ScrollCircle.OnDoubleClick = null;
		RemoveOldSuperSkill();
	}

	private void RemoveOldSuperSkill()
	{
		if (mSuperSkill != null)
		{
			mSuperSkill.DeInit();
			mSuperSkill = null;
		}
	}

	private void OnDoubleClick()
	{
		if (mSuperSkill != null && mSuperSkill.CanUseSkill)
		{
			mSuperSkill.UseSkill();
		}
	}
}
