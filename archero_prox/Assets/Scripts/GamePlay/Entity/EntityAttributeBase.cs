




using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;

public class EntityAttributeBase
{
	public class ValueBase
	{
		private long mValueCount;

		private long mValuePercent;

		public long ValueLong => Value;

		public long ValueCount => mValueCount;

		public long ValuePercent => mValuePercent;

		public long Value
		{
			get;
			private set;
		}

		public bool Enable => ValueLong > 0;

		public ValueBase()
		{
		}

		public ValueBase(long count)
		{
			mValueCount = count;
			mValuePercent = 0L;
			UpdateValue();
		}

		public ValueBase(long count, long percent)
		{
			mValueCount = count;
			mValuePercent = percent;
			UpdateValue();
		}

		public void InitValueCount(long count)
		{
			mValueCount = count;
			mValuePercent = 0L;
			UpdateValue();
		}

		public void UpdateValueCount(long count)
		{
			mValueCount += count;
			UpdateValue();
		}

		public void UpdateValuePercent(long percent)
		{
			mValuePercent += percent;
			UpdateValue();
		}

		private void UpdateValue()
		{
			Value = (long)((float)(mValueCount * (10000 + mValuePercent)) / 10000f);
			OnUpdateValue();
		}

		protected virtual void OnUpdateValue()
		{
		}
	}

	public class ValueFloatBase
	{
		private long mValueCountInit;

		private long mValueCount;

		private long mValuePercent;

		public long ValueLong
		{
			get
			{
				if (mValueCountInit == 0 && mValueCount == 0)
				{
					return mValuePercent;
				}
				return mValueCount * (10000 + mValuePercent) / 10000;
			}
		}

		public long ValueCount => mValueCount;

		public long ValuePercent => mValuePercent;

		public float Value
		{
			get;
			private set;
		}

		public ValueFloatBase()
		{
			InitValueCount(0L);
		}

		public ValueFloatBase(long count)
		{
			InitValueCount(count);
		}

		public ValueFloatBase(long count, long percent)
		{
			mValueCount = count;
			mValuePercent = percent;
			mValueCountInit = mValueCount;
			UpdateValue();
		}

		public void InitValueCount(long count)
		{
			InitValue(count, 0L);
		}

		public void InitValuePercent(long percent)
		{
			InitValue(0L, percent);
		}

		public void InitValue(long count, long percent)
		{
			mValueCount = count;
			mValuePercent = percent;
			mValueCountInit = mValueCount;
			UpdateValue();
		}

		public void UpdateValueCount(long count)
		{
			mValueCount += count;
			UpdateValue();
		}

		public void UpdateValuePercent(long percent)
		{
			mValuePercent += percent;
			UpdateValue();
		}

		private void UpdateValue()
		{
			if (mValueCountInit == 0 && mValueCount == 0)
			{
				Value = (float)mValuePercent / 10000f;
			}
			else
			{
				Value = (float)(mValueCount * (10000 + mValuePercent)) / 1E+08f;
			}
		}
	}

	public class ValueMult
	{
		private long mValue;

		public float Value => (float)mValue / 10000f;

		public long ValueLong => mValue;

		public ValueMult()
		{
			mValue = 10000L;
		}

		public ValueMult(long value)
		{
			InitValue(value);
		}

		public void InitValue(long value)
		{
			mValue = value;
		}

		public void UpdateValue(long value)
		{
			if (value > 0)
			{
				mValue = (long)((float)(mValue * (10000 + value)) / 10000f);
			}
			else
			{
				mValue = (long)((float)mValue / ((float)(10000 - value) / 10000f));
			}
		}
	}

	public class ValueReduce
	{
		public List<long> mList = new List<long>();

		public float Value
		{
			get;
			private set;
		}

		public ValueReduce()
		{
			UpdateValue();
		}

		public ValueReduce(long value)
		{
			UpdateValue(value);
		}

		public void InitValue(List<long> list)
		{
			mList = list;
			UpdateValue();
		}

		public void UpdateValue(long value)
		{
			if (value > 0)
			{
				mList.Add(value);
			}
			else
			{
				value *= -1;
				if (mList.Contains(value))
				{
					mList.Remove(value);
				}
			}
			UpdateValue();
		}

		private void UpdateValue()
		{
			if (mList.Count == 0)
			{
				Value = 0f;
				return;
			}
			Value = 1f;
			int i = 0;
			for (int count = mList.Count; i < count; i++)
			{
				Value *= 1f - (float)mList[i] / 10000f;
			}
			Value = 1f - Value;
		}
	}

	public class ValueTime : ValueBase
	{
		public new float Value
		{
			get;
			private set;
		}

		public ValueTime()
		{
		}

		public ValueTime(long count)
			: base(count)
		{
		}

		public ValueTime(long count, long percent)
			: base(count, percent)
		{
		}

		protected override void OnUpdateValue()
		{
			Value = (float)base.Value / 1000f;
		}
	}

	public class ValueRange
	{
		private int count;

		private int min;

		private int max;

		public int Count => count;

		public int Min => min;

		public int Max => max;

		public bool Enable => count > 0;

		public int Value
		{
			get
			{
				if (Enable)
				{
					return GameLogic.Random(min, max + 1);
				}
				return 0;
			}
		}

		public ValueRange()
		{
			count = 0;
			min = 0;
			max = 0;
		}

		public ValueRange(int count, int min, int max)
		{
			this.count = count;
			this.min = min;
			if (max < min)
			{
				max = min;
			}
			this.max = max;
		}

		public void UpdateCount(int count)
		{
			this.count += count;
		}

		public void UpdateMin(int min)
		{
			this.min += min;
		}

		public void UpdateMax(int max)
		{
			this.max += max;
		}

		public void InitValue(ValueRange data)
		{
			count = data.Count;
			min = data.Min;
			max = data.Max;
		}
	}

	public Action<long> OnHPUpdate;

	public Action OnMoveSpeedUpdate;

	public Action<long> Shield_ValueAction;

	public Action<long> OnAttackUpdate;

	public ValueBase Bullet_Forward = new ValueBase(1L);

	public ValueBase Bullet_Backward = new ValueBase();

	public ValueBase Bullet_Side = new ValueBase();

	public ValueBase Bullet_ForSide = new ValueBase();

	public ValueBase Bullet_Continue = new ValueBase(1L);

	public ValueBase HPValue = new ValueBase();

	public ValueBase HPAdd = new ValueBase();

	public ValueFloatBase HPAddPercent = new ValueFloatBase();

	public ValueBase AttackValue = new ValueBase();

	public ValueBase DefenceValue = new ValueBase();

	public ValueBase MoveSpeed = new ValueBase();

	public ValueFloatBase Attack_Value = new ValueFloatBase();

	public ValueFloatBase Damage_Resistance = new ValueFloatBase();

	public ValueFloatBase HitRate = new ValueFloatBase(0L, 10000L);

	public ValueReduce MissRate = new ValueReduce();

	public ValueFloatBase CritRate = new ValueFloatBase();

	public ValueFloatBase CritDefRate = new ValueFloatBase();

	public ValueFloatBase BlockRate = new ValueFloatBase();

	public ValueFloatBase CritValue = new ValueFloatBase(0L, 20000L);

	public ValueFloatBase AttackSpeed = new ValueFloatBase();

	public ValueBase HitVampire = new ValueBase();

	public ValueFloatBase HitVampirePercent = new ValueFloatBase();

	public ValueFloatBase HitVampireAddPercent = new ValueFloatBase();

	public long HitVampireResult;

	public ValueBase KillVampire = new ValueBase();

	public ValueFloatBase KillVampirePercent = new ValueFloatBase();

	public ValueFloatBase KillVampireAddPercent = new ValueFloatBase();

	public long KillVampireResult;

	public ValueBase TrapDefCount = new ValueBase();

	public ValueFloatBase TrapDef = new ValueFloatBase();

	public ValueFloatBase BulletDef = new ValueFloatBase();

	public ValueFloatBase HitFromFly = new ValueFloatBase();

	public ValueFloatBase HitToFlyPercent = new ValueFloatBase();

	public ValueBase HitToFly = new ValueBase();

	public ValueBase HitToGround = new ValueBase();

	public ValueFloatBase HitToGroundPercent = new ValueFloatBase();

	public ValueBase HitToNear = new ValueBase();

	public ValueFloatBase HitToNearPercent = new ValueFloatBase();

	public ValueBase HitToFar = new ValueBase();

	public ValueFloatBase HitToFarPercent = new ValueFloatBase();

	public ValueFloatBase HitFromBoss = new ValueFloatBase();

	public ValueFloatBase HitToBossPercent = new ValueFloatBase();

	public ValueBase HitToBoss = new ValueBase();

	public ValueBase BodyHittedCount = new ValueBase();

	public ValueFloatBase BodyHitted = new ValueFloatBase();

	public ValueFloatBase HeadShot = new ValueFloatBase();

	public ValueBase ReboundHit = new ValueBase();

	public ValueFloatBase ReboundTargetPercent = new ValueFloatBase();

	public ValueMult AttackModify = new ValueMult(10000L);

	public ValueBase ExtraSkill = new ValueBase();

	public ValueFloatBase ExpGet = new ValueFloatBase();

	public ValueBase RebornCount = new ValueBase();

	public ValueBase RebornHP = new ValueBase();

	public ValueFloatBase RebornHPPercent = new ValueFloatBase();

	public ValueBase BodyHit = new ValueBase();

	public ValueFloatBase HitBack = new ValueFloatBase();

	public ValueFloatBase BodyScale = new ValueFloatBase(0L, 10000L);

	public ValueBase RotateSpeed = new ValueBase();

	public ValueRange ArrowEject = new ValueRange();

	public ValueBase ArrowTrack = new ValueBase();

	public ValueRange ReboundWall = new ValueRange();

	public ValueFloatBase CritAddHP = new ValueFloatBase();

	public ValueFloatBase CritSuperRate = new ValueFloatBase();

	public ValueFloatBase CritSuperValue = new ValueFloatBase(0L, 10000L);

	public ValueFloatBase AngelR2Rate = new ValueFloatBase();

	public ValueFloatBase HP2AttackSpeed = new ValueFloatBase();

	public ValueFloatBase HP2HPAddPercent = new ValueFloatBase();

	public ValueFloatBase HP2Miss = new ValueFloatBase();

	public ValueFloatBase BabyCountAttack = new ValueFloatBase();

	public ValueFloatBase BabyCountAttackSpeed = new ValueFloatBase();

	public ValueFloatBase StaticReducePercent = new ValueFloatBase();

	public ValueBase KillBossShield = new ValueBase();

	public ValueFloatBase KillBossShieldPercent = new ValueFloatBase();

	public ValueFloatBase KillMonsterLessHP = new ValueFloatBase();

	public ValueFloatBase KillMonsterLessHPRatio = new ValueFloatBase();

	public ValueBase DistanceAttackValueDis = new ValueBase();

	public ValueFloatBase DistanceAttackValuePercent = new ValueFloatBase();

	public ValueFloatBase WeaponRoundBackAttackPercent = new ValueFloatBase();

	public ValueBase Shield = new ValueBase();

	public ValueFloatBase EquipDrop = new ValueFloatBase();

	public ValueBase Att_Fire_Add = new ValueBase();

	public ValueFloatBase Att_Fire_AddPercent = new ValueFloatBase();

	public ValueBase Att_Fire_Resist = new ValueBase();

	public ValueFloatBase Att_Fire_ResistPercent = new ValueFloatBase();

	public ValueBase Att_Poison_Add = new ValueBase();

	public ValueFloatBase Att_Poison_AddPercent = new ValueFloatBase();

	public ValueBase Att_Poison_Resist = new ValueBase();

	public ValueFloatBase Att_Poison_ResistPercent = new ValueFloatBase();

	public ValueFloatBase Monster_ExpPercent = new ValueFloatBase();

	public ValueTime Monster_DizzyDelay = new ValueTime(3000L);

	public ValueFloatBase Monster_HPDrop = new ValueFloatBase();

	public ValueFloatBase Monster_GoldDrop = new ValueFloatBase();

	public EntityAttributeBase()
	{
	}

	public EntityAttributeBase(int CharID)
	{
		Character_Char beanById = LocalModelManager.Instance.Character_Char.GetBeanById(CharID);
		Excute("MoveSpeed", beanById.Speed);
		Excute("HPMax", beanById.HP);
		Excute("RotateSpeed", beanById.RotateSpeed);
		Excute("BodyHit", beanById.BodyAttack);
	}

	public void UpdateKillVampireResult()
	{
		KillVampireResult = MathDxx.FloorToInt(((float)KillVampire.Value + KillVampirePercent.Value * (float)GetHPBase()) * (1f + KillVampireAddPercent.Value));
	}

	public void UpdateHitVampireResult()
	{
		HitVampireResult = MathDxx.FloorToInt(((float)HitVampire.Value + HitVampirePercent.Value * (float)GetHPBase()) * (1f + HitVampireAddPercent.Value));
	}

	public long GetHPBase()
	{
		return HPValue.ValueCount;
	}

	public bool Excute(string str)
	{
		Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
		return Excute(goodData);
	}

	public bool Excute(Goods_goods.GoodData data)
	{
		return Excute(data.goodType, data.value);
	}

    //@TODO CANNOT DECODE _003C_003Ef__switch_0024map3
    Dictionary<string, int> _003C_003Ef__switch_0024map3;
    public bool Excute(string type, long value)
	{
		bool result = true;
		if (type != null)
		{
			if (_003C_003Ef__switch_0024map3 == null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(102);
				dictionary.Add("BulletForward", 0);
				dictionary.Add("BulletBackward", 1);
				dictionary.Add("BulletSide", 2);
				dictionary.Add("BulletForSide", 3);
				dictionary.Add("BulletContinue", 4);
				dictionary.Add("HPMax", 5);
				dictionary.Add("HPMax%", 6);
				dictionary.Add("HPAdd", 7);
				dictionary.Add("HPAdd%", 8);
				dictionary.Add("Attack", 9);
				dictionary.Add("Attack%", 10);
				dictionary.Add("Defence", 11);
				dictionary.Add("Defence%", 12);
				dictionary.Add("MoveSpeed", 13);
				dictionary.Add("MoveSpeed%", 14);
				dictionary.Add("AttackValue%", 15);
				dictionary.Add("DamageResist%", 16);
				dictionary.Add("HitRate%", 17);
				dictionary.Add("MissRate%", 18);
				dictionary.Add("CritRate%", 19);
				dictionary.Add("CritDefRate%", 20);
				dictionary.Add("BlockRate%", 21);
				dictionary.Add("CritValue%", 22);
				dictionary.Add("AttackSpeed%", 23);
				dictionary.Add("HitVampire", 24);
				dictionary.Add("HitVampire%", 25);
				dictionary.Add("HitVampireAdd%", 26);
				dictionary.Add("KillVampire", 27);
				dictionary.Add("KillVampire%", 28);
				dictionary.Add("KillVampireAdd%", 29);
				dictionary.Add("TrapHittedReduce", 30);
				dictionary.Add("TrapHittedReduce%", 31);
				dictionary.Add("BulletReduce%", 32);
				dictionary.Add("HitFromFly%", 33);
				dictionary.Add("HitToFly%", 34);
				dictionary.Add("HitToFly", 35);
				dictionary.Add("HitToGround", 36);
				dictionary.Add("HitToGround%", 37);
				dictionary.Add("HitToNear", 38);
				dictionary.Add("HitToNear%", 39);
				dictionary.Add("HitToFar", 40);
				dictionary.Add("HitToFar%", 41);
				dictionary.Add("HitFromBoss%", 42);
				dictionary.Add("HitToBoss%", 43);
				dictionary.Add("HitToBoss", 44);
				dictionary.Add("BodyHittedReduce", 45);
				dictionary.Add("BodyHittedReduce%", 46);
				dictionary.Add("HeadShot%", 47);
				dictionary.Add("ReboundHit", 48);
				dictionary.Add("ReboundHit%", 49);
				dictionary.Add("ReboundTarget%", 50);
				dictionary.Add("AttackModify%", 51);
				dictionary.Add("ExtraSkill", 52);
				dictionary.Add("ExpGet%", 53);
				dictionary.Add("RebornCount", 54);
				dictionary.Add("RebornHP", 55);
				dictionary.Add("RebornHP%", 56);
				dictionary.Add("BodyHit", 57);
				dictionary.Add("BodyHit%", 58);
				dictionary.Add("HitBack%", 59);
				dictionary.Add("BodyScale%", 60);
				dictionary.Add("RotateSpeed", 61);
				dictionary.Add("RotateSpeed%", 62);
				dictionary.Add("ArrowEjectCount", 63);
				dictionary.Add("ArrowEjectMin", 64);
				dictionary.Add("ArrowEjectMax", 65);
				dictionary.Add("ArrowTrack", 66);
				dictionary.Add("ReboundWall", 67);
				dictionary.Add("ReboundWallMin", 68);
				dictionary.Add("ReboundWallMax", 69);
				dictionary.Add("CritAddHP%", 70);
				dictionary.Add("CritSuperRate%", 71);
				dictionary.Add("CritSuperValue%", 72);
				dictionary.Add("AngelRecover2Rate%", 73);
				dictionary.Add("HP2AttackSpeed%", 74);
				dictionary.Add("HP2HPAdd%", 75);
				dictionary.Add("HP2Miss%", 76);
				dictionary.Add("BabyCountAttack%", 77);
				dictionary.Add("BabyCountAttackSpeed%", 78);
				dictionary.Add("StaticReduce%", 79);
				dictionary.Add("KillBossShield", 80);
				dictionary.Add("KillBossShield%", 81);
				dictionary.Add("KillMonsterLessHP%", 82);
				dictionary.Add("KillMonsterLessHPRatio%", 83);
				dictionary.Add("DistanceAttackValueDis", 84);
				dictionary.Add("DistanceAttackValue%", 85);
				dictionary.Add("WeaponRoundBackAttack%", 86);
				dictionary.Add("AddShieldValue", 87);
				dictionary.Add("EquipDrop%", 88);
				dictionary.Add("Att_Fire_Add", 89);
				dictionary.Add("Att_Fire_Add%", 90);
				dictionary.Add("Att_Fire_Resist", 91);
				dictionary.Add("Att_Fire_Resist%", 92);
				dictionary.Add("Att_Poison_Add", 93);
				dictionary.Add("Att_Poison_Add%", 94);
				dictionary.Add("Att_Poison_Resist", 95);
				dictionary.Add("Att_Poison_Resist%", 96);
				dictionary.Add("MonsterExp%", 97);
				dictionary.Add("DizzyDelay", 98);
				dictionary.Add("HPDrop%", 99);
				dictionary.Add("GoldDrop%", 100);
				dictionary.Add("TrapHit%", 101);
				_003C_003Ef__switch_0024map3 = dictionary;
			}
			if (_003C_003Ef__switch_0024map3.TryGetValue(type, out int value2))
			{
				switch (value2)
				{
				case 0:
					Bullet_Forward.UpdateValueCount(value);
					goto IL_0ecf;
				case 1:
					Bullet_Backward.UpdateValueCount(value);
					goto IL_0ecf;
				case 2:
					Bullet_Side.UpdateValueCount(value);
					goto IL_0ecf;
				case 3:
					Bullet_ForSide.UpdateValueCount(value);
					goto IL_0ecf;
				case 4:
					Bullet_Continue.UpdateValueCount(value);
					goto IL_0ecf;
				case 5:
				{
					long value3 = HPValue.Value;
					HPValue.UpdateValueCount(value);
					UpdateHitVampireResult();
					UpdateKillVampireResult();
					if (OnHPUpdate != null)
					{
						OnHPUpdate(value3);
					}
					goto IL_0ecf;
				}
				case 6:
				{
					long value4 = HPValue.Value;
					HPValue.UpdateValuePercent(value);
					UpdateHitVampireResult();
					UpdateKillVampireResult();
					if (OnHPUpdate != null)
					{
						OnHPUpdate(value4);
					}
					goto IL_0ecf;
				}
				case 7:
					HPAdd.UpdateValueCount(value);
					goto IL_0ecf;
				case 8:
					HPAddPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 9:
					AttackValue.UpdateValueCount(value);
					goto IL_0ecf;
				case 10:
					AttackValue.UpdateValuePercent(value);
					if (OnAttackUpdate != null)
					{
						OnAttackUpdate(AttackValue.Value);
					}
					goto IL_0ecf;
				case 11:
					DefenceValue.UpdateValueCount(value);
					goto IL_0ecf;
				case 12:
					DefenceValue.UpdateValuePercent(value);
					goto IL_0ecf;
				case 13:
					MoveSpeed.UpdateValueCount(value);
					if (OnMoveSpeedUpdate != null)
					{
						OnMoveSpeedUpdate();
					}
					goto IL_0ecf;
				case 14:
					MoveSpeed.UpdateValuePercent(value);
					if (OnMoveSpeedUpdate != null)
					{
						OnMoveSpeedUpdate();
					}
					goto IL_0ecf;
				case 15:
					Attack_Value.UpdateValuePercent(value);
					goto IL_0ecf;
				case 16:
					Damage_Resistance.UpdateValuePercent(value);
					goto IL_0ecf;
				case 17:
					HitRate.UpdateValuePercent(value);
					goto IL_0ecf;
				case 18:
					MissRate.UpdateValue(value);
					goto IL_0ecf;
				case 19:
					CritRate.UpdateValuePercent(value);
					goto IL_0ecf;
				case 20:
					CritDefRate.UpdateValuePercent(value);
					goto IL_0ecf;
				case 21:
					BlockRate.UpdateValuePercent(value);
					goto IL_0ecf;
				case 22:
					CritValue.UpdateValuePercent(value);
					goto IL_0ecf;
				case 23:
					AttackSpeed.UpdateValuePercent(value);
					goto IL_0ecf;
				case 24:
					HitVampire.UpdateValueCount(value);
					UpdateHitVampireResult();
					goto IL_0ecf;
				case 25:
					HitVampirePercent.UpdateValueCount(value);
					UpdateHitVampireResult();
					goto IL_0ecf;
				case 26:
					HitVampireAddPercent.UpdateValueCount(value);
					UpdateHitVampireResult();
					goto IL_0ecf;
				case 27:
					KillVampire.UpdateValueCount(value);
					UpdateKillVampireResult();
					goto IL_0ecf;
				case 28:
					KillVampirePercent.UpdateValueCount(value);
					UpdateKillVampireResult();
					goto IL_0ecf;
				case 29:
					KillVampireAddPercent.UpdateValueCount(value);
					UpdateKillVampireResult();
					goto IL_0ecf;
				case 30:
					TrapDefCount.UpdateValueCount(value);
					goto IL_0ecf;
				case 31:
					TrapDef.UpdateValuePercent(value);
					goto IL_0ecf;
				case 32:
					BulletDef.UpdateValuePercent(value);
					goto IL_0ecf;
				case 33:
					HitFromFly.UpdateValuePercent(value);
					goto IL_0ecf;
				case 34:
					HitToFlyPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 35:
					HitToFly.UpdateValueCount(value);
					goto IL_0ecf;
				case 36:
					HitToGround.UpdateValueCount(value);
					goto IL_0ecf;
				case 37:
					HitToGroundPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 38:
					HitToNear.UpdateValueCount(value);
					goto IL_0ecf;
				case 39:
					HitToNearPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 40:
					HitToFar.UpdateValueCount(value);
					goto IL_0ecf;
				case 41:
					HitToFarPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 42:
					HitFromBoss.UpdateValuePercent(value);
					goto IL_0ecf;
				case 43:
					HitToBossPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 44:
					HitToBoss.UpdateValueCount(value);
					goto IL_0ecf;
				case 45:
					BodyHittedCount.UpdateValueCount(value);
					goto IL_0ecf;
				case 46:
					BodyHitted.UpdateValuePercent(value);
					goto IL_0ecf;
				case 47:
					HeadShot.UpdateValuePercent(value);
					goto IL_0ecf;
				case 48:
					ReboundHit.UpdateValueCount(value);
					goto IL_0ecf;
				case 49:
					ReboundHit.UpdateValuePercent(value);
					goto IL_0ecf;
				case 50:
					ReboundTargetPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 51:
					AttackModify.UpdateValue(value);
					goto IL_0ecf;
				case 52:
					ExtraSkill.UpdateValueCount(value);
					goto IL_0ecf;
				case 53:
					ExpGet.UpdateValuePercent(value);
					goto IL_0ecf;
				case 54:
					RebornCount.UpdateValueCount(value);
					goto IL_0ecf;
				case 55:
					RebornHP.UpdateValueCount(value);
					goto IL_0ecf;
				case 56:
					RebornHPPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 57:
					BodyHit.UpdateValueCount(value);
					goto IL_0ecf;
				case 58:
					BodyHit.UpdateValuePercent(value);
					goto IL_0ecf;
				case 59:
					HitBack.UpdateValuePercent(value);
					goto IL_0ecf;
				case 60:
					BodyScale.UpdateValuePercent(value);
					goto IL_0ecf;
				case 61:
					RotateSpeed.UpdateValueCount(value);
					goto IL_0ecf;
				case 62:
					RotateSpeed.UpdateValuePercent(value);
					goto IL_0ecf;
				case 63:
					ArrowEject.UpdateCount((int)value);
					goto IL_0ecf;
				case 64:
					ArrowEject.UpdateMin((int)value);
					goto IL_0ecf;
				case 65:
					ArrowEject.UpdateMax((int)value);
					goto IL_0ecf;
				case 66:
					ArrowTrack.UpdateValueCount(value);
					goto IL_0ecf;
				case 67:
					ReboundWall.UpdateCount((int)value);
					goto IL_0ecf;
				case 68:
					ReboundWall.UpdateMin((int)value);
					goto IL_0ecf;
				case 69:
					ReboundWall.UpdateMax((int)value);
					goto IL_0ecf;
				case 70:
					CritAddHP.UpdateValuePercent(value);
					goto IL_0ecf;
				case 71:
					CritSuperRate.UpdateValuePercent(value);
					goto IL_0ecf;
				case 72:
					CritSuperValue.UpdateValuePercent(value);
					goto IL_0ecf;
				case 73:
					AngelR2Rate.UpdateValuePercent(value);
					goto IL_0ecf;
				case 74:
					HP2AttackSpeed.UpdateValuePercent(value);
					goto IL_0ecf;
				case 75:
					HP2HPAddPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 76:
					HP2Miss.UpdateValuePercent(value);
					goto IL_0ecf;
				case 77:
					BabyCountAttack.UpdateValuePercent(value);
					goto IL_0ecf;
				case 78:
					BabyCountAttackSpeed.UpdateValuePercent(value);
					goto IL_0ecf;
				case 79:
					StaticReducePercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 80:
					KillBossShield.UpdateValueCount(value);
					goto IL_0ecf;
				case 81:
					KillBossShieldPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 82:
					if (KillMonsterLessHP.ValuePercent < value)
					{
						KillMonsterLessHP.InitValuePercent(value);
					}
					goto IL_0ecf;
				case 83:
					if (KillMonsterLessHPRatio.ValuePercent < value)
					{
						KillMonsterLessHPRatio.InitValuePercent(value);
					}
					goto IL_0ecf;
				case 84:
					DistanceAttackValueDis.UpdateValueCount(value);
					goto IL_0ecf;
				case 85:
					DistanceAttackValuePercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 86:
					WeaponRoundBackAttackPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 87:
					Shield.UpdateValueCount(value);
					if (Shield_ValueAction != null)
					{
						Shield_ValueAction(value);
					}
					goto IL_0ecf;
				case 88:
					EquipDrop.UpdateValuePercent(value);
					goto IL_0ecf;
				case 89:
					Att_Fire_Add.UpdateValueCount(value);
					goto IL_0ecf;
				case 90:
					Att_Fire_AddPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 91:
					Att_Fire_Resist.UpdateValueCount(value);
					goto IL_0ecf;
				case 92:
					Att_Fire_ResistPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 93:
					Att_Poison_Add.UpdateValueCount(value);
					goto IL_0ecf;
				case 94:
					Att_Poison_AddPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 95:
					Att_Poison_Resist.UpdateValueCount(value);
					goto IL_0ecf;
				case 96:
					Att_Poison_ResistPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 97:
					Monster_ExpPercent.UpdateValuePercent(value);
					goto IL_0ecf;
				case 98:
					Monster_DizzyDelay.UpdateValueCount(value);
					goto IL_0ecf;
				case 99:
					Monster_HPDrop.UpdateValuePercent(value);
					goto IL_0ecf;
				case 100:
					Monster_GoldDrop.UpdateValuePercent(value);
					goto IL_0ecf;
				case 101:
					goto IL_0ecf;
				}
			}
		}
		result = false;
		goto IL_0ecf;
		IL_0ecf:
		return result;
	}

	public void DebugValue()
	{
		Debugger.Log("生命值 : " + HPValue.Value);
		Debugger.Log("生命回复增加值：" + HPAdd.Value);
		Debugger.Log("生命回复% : " + HPAddPercent.Value);
		Debugger.Log("攻击力 : " + AttackValue.Value);
		Debugger.Log("防御力 : " + DefenceValue.Value);
		Debugger.Log("移动速度 : " + MoveSpeed.Value);
		Debugger.Log("伤害加成% : " + Attack_Value.Value);
		Debugger.Log("伤害减免% : " + Damage_Resistance.Value);
		Debugger.Log("命中率 : " + HitRate.Value);
		Debugger.Log("闪避率 : " + MissRate.Value);
		Debugger.Log("暴击率 : " + CritRate.Value);
		Debugger.Log("抗暴击率 : " + CritDefRate.Value);
		Debugger.Log("格挡率 : " + BlockRate.Value);
		Debugger.Log("暴击伤害% : " + CritValue.Value);
		Debugger.Log("攻击速度% : " + AttackSpeed.Value);
		Debugger.Log("命中吸血 : " + HitVampireResult);
		Debugger.Log("击杀吸血 : " + KillVampireResult);
		Debugger.Log("陷阱伤害减免% : " + TrapDef.Value);
		Debugger.Log("飞行单位伤害减免% : " + HitFromFly.Value);
		Debugger.Log("对飞行单位伤害% : " + HitToFlyPercent.Value);
		Debugger.Log("对飞行单位伤害数值 : " + HitToFly.Value);
		Debugger.Log("Boss伤害减免% : " + HitFromBoss.Value);
		Debugger.Log("对Boss伤害加成% : " + HitToBossPercent.Value);
		Debugger.Log("对Boss伤害加成数值 : " + HitToBoss.Value);
		Debugger.Log("碰撞减伤% : " + BodyHitted.Value);
		Debugger.Log("秒杀小怪% : " + HeadShot.Value);
		Debugger.Log("反弹伤害% : " + ReboundHit.Value);
		Debugger.Log("反弹伤害敌方% : " + ReboundTargetPercent.Value);
		Debugger.Log("攻击修正% : " + AttackModify.Value);
		Debugger.Log("额外技能 : " + ExtraSkill.Value);
		Debugger.Log("经验获取% : " + ExpGet.Value);
		Debugger.Log("复活次数 : " + RebornCount.Value);
		Debugger.Log("击退系数% : " + HitBack.Value);
		Debugger.Log("碰撞伤害 : " + BodyHit.Value);
		Debugger.Log("体积% : " + BodyScale.Value);
		Debugger.Log("转头速率 : " + RotateSpeed.Value);
		Debugger.Log("超级暴击率% : " + CritSuperRate.Value);
		Debugger.Log("超级暴击伤害% : " + CritSuperValue.Value);
	}

	public void AttributeTo(EntityAttributeBase attribute)
	{
		attribute.HPValue.InitValueCount(HPValue.ValueLong);
		long count = (long)((float)HPAdd.ValueLong * (1f + HPAddPercent.Value));
		attribute.HPAdd.InitValueCount(count);
		attribute.AttackValue.InitValueCount(AttackValue.ValueLong);
		attribute.DefenceValue.InitValueCount(DefenceValue.ValueLong);
		attribute.MoveSpeed.InitValueCount(MoveSpeed.ValueLong);
		attribute.Attack_Value.InitValuePercent(Attack_Value.ValueLong);
		attribute.Damage_Resistance.InitValueCount(Damage_Resistance.ValueLong);
		attribute.HitRate.InitValuePercent(HitRate.ValueLong);
		attribute.MissRate.InitValue(MissRate.mList);
		attribute.CritRate.InitValuePercent(CritRate.ValueLong);
		attribute.CritDefRate.InitValuePercent(CritDefRate.ValueLong);
		attribute.BlockRate.InitValuePercent(BlockRate.ValueLong);
		attribute.CritValue.InitValuePercent(CritValue.ValueLong);
		attribute.AttackSpeed.InitValuePercent(AttackSpeed.ValueLong);
		attribute.HitVampire.InitValueCount(HitVampireResult);
		attribute.UpdateHitVampireResult();
		attribute.KillVampire.InitValueCount(KillVampireResult);
		attribute.UpdateKillVampireResult();
		attribute.TrapDefCount.InitValueCount(TrapDefCount.ValueLong);
		attribute.TrapDef.InitValueCount(TrapDef.ValueLong);
		attribute.BulletDef.InitValueCount(BulletDef.ValueLong);
		attribute.HitFromFly.InitValueCount(HitFromFly.ValueLong);
		attribute.HitToFlyPercent.InitValuePercent(HitToFlyPercent.ValueLong);
		attribute.HitToFly.InitValueCount(HitToFly.ValueLong);
		attribute.HitToGround.InitValueCount(HitToGround.ValueLong);
		attribute.HitToGroundPercent.InitValuePercent(HitToGroundPercent.ValueLong);
		attribute.HitToNear.InitValueCount(HitToNear.ValueLong);
		attribute.HitToNearPercent.InitValuePercent(HitToNearPercent.ValueLong);
		attribute.HitToFar.InitValueCount(HitToFar.ValueLong);
		attribute.HitToFarPercent.InitValuePercent(HitToFarPercent.ValueLong);
		attribute.HitFromBoss.InitValueCount(HitFromBoss.ValueLong);
		attribute.HitToBossPercent.InitValuePercent(HitToBossPercent.ValueLong);
		attribute.HitToBoss.InitValueCount(HitToBoss.ValueLong);
		attribute.BodyHittedCount.InitValueCount(BodyHittedCount.ValueLong);
		attribute.BodyHitted.InitValueCount(BodyHitted.ValueLong);
		attribute.HeadShot.InitValuePercent(HeadShot.ValueLong);
		attribute.ReboundHit.InitValueCount(ReboundHit.ValueLong);
		attribute.ReboundTargetPercent.InitValuePercent(ReboundTargetPercent.ValueLong);
		attribute.AttackModify.InitValue(AttackModify.ValueLong);
		attribute.ExtraSkill.InitValueCount(ExtraSkill.ValueLong);
		attribute.ExpGet.InitValuePercent(ExpGet.ValueLong);
		attribute.RebornCount.InitValueCount(RebornCount.ValueLong);
		attribute.BodyHit.InitValueCount(BodyHit.ValueLong);
		attribute.HitBack.InitValuePercent(HitBack.ValueLong);
		attribute.BodyScale.InitValuePercent(BodyScale.ValueLong);
		attribute.RotateSpeed.InitValueCount(RotateSpeed.ValueLong);
		attribute.ArrowEject.InitValue(ArrowEject);
		attribute.ArrowTrack.InitValueCount(ArrowTrack.ValueLong);
		attribute.ReboundWall.InitValue(ReboundWall);
		attribute.CritAddHP.InitValue(CritAddHP.ValueLong, CritAddHP.ValuePercent);
		attribute.CritSuperRate.InitValuePercent(CritSuperRate.ValueLong);
		attribute.CritSuperValue.InitValuePercent(CritSuperValue.ValueLong);
		attribute.AngelR2Rate.InitValuePercent(AngelR2Rate.ValueLong);
		attribute.HP2AttackSpeed.InitValuePercent(HP2AttackSpeed.ValueLong);
		attribute.HP2HPAddPercent.InitValuePercent(HP2HPAddPercent.ValueLong);
		attribute.HP2Miss.InitValuePercent(HP2Miss.ValueLong);
		attribute.BabyCountAttack.InitValuePercent(BabyCountAttack.ValueLong);
		attribute.BabyCountAttackSpeed.InitValuePercent(BabyCountAttackSpeed.ValueLong);
		attribute.StaticReducePercent.InitValuePercent(StaticReducePercent.ValueLong);
		attribute.KillMonsterLessHP.InitValuePercent(KillMonsterLessHP.ValueLong);
		attribute.KillMonsterLessHPRatio.InitValuePercent(KillMonsterLessHPRatio.ValueLong);
		attribute.Shield.InitValueCount(Shield.ValueLong);
		attribute.KillBossShield.InitValueCount(KillBossShield.ValueLong);
		attribute.KillBossShieldPercent.InitValuePercent(KillBossShieldPercent.ValueLong);
		attribute.EquipDrop.InitValuePercent(EquipDrop.ValueLong);
		attribute.Att_Fire_Add.InitValueCount(Att_Fire_Add.ValueLong);
		attribute.Att_Fire_AddPercent.InitValuePercent(Att_Fire_AddPercent.ValueLong);
		attribute.Att_Fire_Resist.InitValueCount(Att_Fire_Resist.ValueLong);
		attribute.Att_Fire_ResistPercent.InitValuePercent(Att_Fire_ResistPercent.ValueLong);
		attribute.Att_Poison_Add.InitValueCount(Att_Poison_Add.ValueLong);
		attribute.Att_Poison_AddPercent.InitValuePercent(Att_Poison_AddPercent.ValueLong);
		attribute.Att_Poison_Resist.InitValueCount(Att_Poison_Resist.ValueLong);
		attribute.Att_Poison_ResistPercent.InitValuePercent(Att_Poison_ResistPercent.ValueLong);
		attribute.Monster_ExpPercent.InitValuePercent(Monster_ExpPercent.ValueLong);
		attribute.Monster_HPDrop.InitValuePercent(Monster_HPDrop.ValueLong);
		attribute.Monster_ExpPercent.InitValuePercent(Monster_ExpPercent.ValueLong);
		attribute.Monster_GoldDrop.InitValuePercent(Monster_GoldDrop.ValueLong);
	}

	public bool GetCanReborn()
	{
		if (RebornCount.Value > 0)
		{
			RebornCount.UpdateValueCount(-1L);
			return true;
		}
		return false;
	}

	public void Reborn_Refresh_Count(int usecount)
	{
		RebornCount.UpdateValueCount(-usecount);
	}
}
