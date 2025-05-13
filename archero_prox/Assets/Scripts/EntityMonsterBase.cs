




using DG.Tweening;
using Dxx.Util;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class EntityMonsterBase : EntityCallBase
{
	public class DropData
	{
		public int GoodID;

		public int Weight;
	}

	public class DropRandomData
	{
		private int DropWeight;

		private List<DropData> mDropList = new List<DropData>();

		public void InitDrop(string[] s)
		{
			int i = 0;
			for (int num = s.Length; i < num; i++)
			{
				string text = s[i];
				string[] array = text.Split(',');
				DropData dropData = new DropData();
				dropData.GoodID = int.Parse(array[0]);
				dropData.Weight = int.Parse(array[1]);
				mDropList.Add(dropData);
				DropWeight += dropData.Weight;
			}
		}

		public int GetRandom()
		{
			int num = GameLogic.Random(0, DropWeight);
			int i = 0;
			for (int count = mDropList.Count; i < count; i++)
			{
				DropData dropData = mDropList[i];
				if (num < dropData.Weight)
				{
					if (dropData.GoodID > 0)
					{
						return dropData.GoodID;
					}
					break;
				}
				num -= dropData.Weight;
			}
			return 0;
		}
	}

	private List<DropRandomData> mDropList = new List<DropRandomData>();

	private bool bDeadDown;

	private DropBase mDrop;

	private SequencePool mSequencePool = new SequencePool();

	private const float HittedMax = 30f;

	private float HittedReal;

	private int HittedBackIndex;

	private EntityBase triggerentity;

	private float triggertime;

	private bool isinTrigger;

	protected override string ModelPath => "Game/Soldier/Soldier";

	public Soldier_soldier m_SoldierData
	{
		get;
		protected set;
	}

	private int HittedArgsLength => 3;

	protected List<BattleDropData> goodsList
	{
		get
		{
			List<BattleDropData> list = new List<BattleDropData>();
			if (GameLogic.Release.Game.RoomState != RoomState.Runing)
			{
				return list;
			}
			if (bCall)
			{
				return list;
			}
			if (GameLogic.Hold.BattleData.Challenge_DropExp())
			{
				int exp = (int)((float)m_SoldierData.Exp * (1f + m_EntityData.attribute.Monster_ExpPercent.Value));
				list = GameLogic.GetExpList(exp);
			}
			List<BattleDropData> dropDead = mDrop.GetDropDead();
			list.AddRange(dropDead);
			float num = 1f;
			if (GameLogic.Hold.BattleData.Challenge_RecoverHP())
			{
				BattleDropData item = new BattleDropData(FoodType.eHP, FoodOneType.eHP0020, 0);
				if (GameLogic.Random(0f, 100f) < (float)m_SoldierData.HPDrop1 * num * (1f + m_EntityData.attribute.Monster_HPDrop.Value))
				{
					list.Add(item);
				}
				if (GameLogic.Random(0f, 100f) < (float)m_SoldierData.HPDrop2 * num * (1f + m_EntityData.attribute.Monster_HPDrop.Value))
				{
					list.Add(item);
				}
				if (GameLogic.Random(0f, 100f) < (float)m_SoldierData.HPDrop3 * num * (1f + m_EntityData.attribute.Monster_HPDrop.Value))
				{
					list.Add(item);
				}
			}
			return list;
		}
	}

	protected override void OnInitBefore()
	{
		base.OnInitBefore();
		if (base.IsElite)
		{
			HPSliderName = "HPSlider_Elite";
		}
		else
		{
			HPSliderName = "HPSlider_Monster";
		}
	}

	protected override void OnInit()
	{
		base.OnInit();
		m_MoveCtrl = new MoveControl();
		m_AttackCtrl = new AttackControl();
		m_SoldierData = LocalModelManager.Instance.Soldier_soldier.GetBeanById(ClassID);
		SdkManager.Bugly_Report(m_SoldierData != null, "EntityMonsterBase.cs", Utils.GetString("Soldier_soldier dont have ", ClassID, " monster"));
		m_MoveCtrl.Init(this);
		m_AttackCtrl.Init(this);
		GameLogic.Release.Entity.Add(this);
		m_AttackCtrl.SetRotate(180f);
		SetCollider(enable: false);
	}

	protected override void StartInit()
	{
		base.StartInit();
		SetCollider(enable: true);
		switch (GameLogic.Hold.BattleData.GetMode())
		{
		case GameMode.eGold1:
			mDrop = new DropGold();
			break;
		case GameMode.eChallenge101:
		case GameMode.eChallenge102:
		case GameMode.eChallenge103:
		case GameMode.eChallenge104:
			mDrop = new DropChallenge101();
			break;
		default:
			mDrop = new DropDefault();
			break;
		}
		mDrop.Init(m_SoldierData, m_EntityData.MaxHP);
	}

	protected override void OnDeInitLogic()
	{
		mSequencePool.Clear();
		TriggerEnd();
		base.OnDeInitLogic();
	}

	protected override void OnCreateModel()
	{
		base.OnCreateModel();
		ExcuteSoldierUp();
	}

	private void ExcuteSoldierUp()
	{
		if (GameLogic.Hold.BattleData.mModeData == null)
		{
			return;
		}
		string[] monsterTmxAttributes = GameLogic.Hold.BattleData.mModeData.GetMonsterTmxAttributes();
		int i = 0;
		for (int num = monsterTmxAttributes.Length; i < num; i++)
		{
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(monsterTmxAttributes[i]);
			if (base.IsElite && goodData.goodType == "HPMax%")
			{
				goodData.value *= 2L;
				goodData.value += 10000L;
			}
			m_EntityData.ExcuteAttributes(goodData);
		}
	}

	protected override void OnChangeHP(EntityBase entity, long HP)
	{
		List<BattleDropData> hittedList = mDrop.GetHittedList(HP);
		if (hittedList != null && hittedList.Count > 0)
		{
			CreateDeadGoods(hittedList);
		}
		if (GetIsDead())
		{
			if (base.Type == EntityType.Boss)
			{
				GameLogic.Hold.BattleData.AddKillBoss(m_Data.CharID);
			}
			else
			{
				GameLogic.Hold.BattleData.AddKillMonsters(m_Data.CharID);
			}
			TriggerEnd();
			if (!bCall)
			{
			}
			if (GameLogic.Release.Mode != null && GameLogic.Release.Mode.RoomGenerate != null)
			{
				GameLogic.Release.Mode.RoomGenerate.CheckOpenDoor();
			}
			bool flag = false;
			EntityType type = base.Type;
			if (m_Data.Divide == 0)
			{
				flag = true;
			}
			if (base.DivideID != string.Empty)
			{
				GameLogic.Release.Entity.RemoveDivide(base.DivideID);
			}
			if (flag)
			{
				CreateGoods();
			}
			RemoveMove();
		}
	}

	private void CreateGoods()
	{
		OnCreateDeadGoods();
		CreateDeadGoods(OnGetGoodList());
	}

	protected virtual void OnCreateDeadGoods()
	{
	}

	private void CreateDeadGoods(List<BattleDropData> list)
	{
		if (list.Count > 0)
		{
			GameLogic.Release.Mode.CreateGoods(base.position, list, m_SoldierData.DropRadius);
		}
	}

	public override void DeadCallBack()
	{
		if (m_Data.Divide != 0)
		{
			m_AniCtrl.DeadDown();
			DeInit();
		}
		else
		{
			base.DeadCallBack();
		}
	}

	protected override void UpdateProcess(float delta)
	{
		base.UpdateProcess(delta);
		if (!GetIsDead())
		{
			m_AttackCtrl.UpdateProgress();
			OnTriggerUpdate();
		}
	}

	protected override void UpdateFixed()
	{
		UpdateHittedBack();
		if (!GetIsDead())
		{
			m_MoveCtrl.UpdateProgress();
		}
	}

	public override bool SetHitted(HittedData data)
	{
		bool flag = base.SetHitted(data);
		if (flag && data.backtatio > 0f)
		{
			StartHittedBack(data.backtatio);
		}
		return flag;
	}

	private void StartHittedBack(float backRatio)
	{
		if (m_State == EntityState.Normal)
		{
			HittedReal = 30f * backRatio;
			HittedBackIndex = HittedArgsLength;
			m_State = EntityState.Hitted;
		}
	}

	private void UpdateHittedBack()
	{
		if (m_State == EntityState.Hitted)
		{
			if (HittedBackIndex < 0)
			{
				HittedV = 0f;
				m_MoveCtrl.ResetRigidBody();
				m_State = EntityState.Normal;
			}
			else
			{
				HittedV = HittedReal * (float)HittedBackIndex / (float)HittedArgsLength;
				HittedBackIndex--;
			}
		}
	}

	protected override long GetBossHP()
	{
		return m_EntityData.MaxHP;
	}

	public void StartCall()
	{
		ShowHP(show: false);
		Sequence seq = DOTween.Sequence().AppendInterval(0.3f).AppendCallback(delegate
		{
			ShowHP(show: true);
		});
		mSequencePool.Add(seq);
	}

	protected void InitDivideID()
	{
		base.DivideID = GetInstanceID().ToString();
	}

	protected override List<BattleDropData> OnGetGoodList()
	{
		return goodsList;
	}

	protected override void CollisionEnterExtra(Collision o)
	{
		TriggerEnter(o.gameObject);
	}

	protected override void CollisionExitExtra(Collision o)
	{
		TriggerExit(o.gameObject);
	}

	protected override void OnTriggerEnterExtra(Collider o)
	{
		if (GetColliderTrigger())
		{
			TriggerEnter(o.gameObject);
		}
	}

	protected override void OnTriggerExitExtra(Collider o)
	{
		if (GetColliderTrigger())
		{
			TriggerExit(o.gameObject);
		}
	}

	private void TriggerEnd()
	{
		isinTrigger = false;
	}

	private void TriggerStart()
	{
		isinTrigger = true;
	}

	private void TriggerEnter(GameObject o)
	{
		if (o.layer == LayerManager.Player || o.layer == LayerManager.Fly)
		{
			EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
			if (!GameLogic.IsSameTeam(this, entityByChild))
			{
				triggerentity = entityByChild;
				TriggerStart();
			}
		}
	}

	private void TriggerExit(GameObject o)
	{
		if (triggerentity != null && o == triggerentity.gameObject)
		{
			TriggerEnd();
		}
	}

	private void OnTriggerUpdate()
	{
		if (!isinTrigger)
		{
			return;
		}
		if (GetIsDead() || ((bool)triggerentity && triggerentity.GetIsDead()) || !GetMeshShow())
		{
			TriggerEnd();
		}
		else if ((bool)triggerentity && Updater.AliveTime - triggertime > 1.2f)
		{
			triggertime = Updater.AliveTime;
			if (triggerentity != null)
			{
				HitEntity(triggerentity);
			}
		}
	}

	private void HitEntity(EntityBase e)
	{
		if (!GetIsDead() && (!triggerentity || !triggerentity.GetIsDead()) && GetColliderEnable())
		{
			int num = -m_EntityData.GetBodyHit();
			GameLogic.SendHit_Body(e, this, num, m_SoldierData.BodyHitSoundID);
			OnHitEntity(e);
		}
	}

	protected virtual void OnHitEntity(EntityBase e)
	{
	}
}
