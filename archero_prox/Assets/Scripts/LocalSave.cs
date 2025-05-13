




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Threading;
using TableTool;
using UnityEngine;

public class LocalSave
{
	[Serializable]
	public class AchieveDataOne
	{
		public int achieveid;

		public int currentcount;

		public bool isgot;

		[JsonIgnore]
		private Achieve_Achieve _data;

		private int _maxcount = -1;

		[JsonIgnore]
		private AchieveConditionBase _condition;

		[JsonIgnore]
		public Achieve_Achieve mData
		{
			get
			{
				if (_data == null)
				{
					_data = LocalModelManager.Instance.Achieve_Achieve.GetBeanById(achieveid);
				}
				return _data;
			}
		}

		[JsonIgnore]
		public int maxcount
		{
			get
			{
				if (_maxcount < 0)
				{
					if (mData.CondTypeArgs.Length > 0)
					{
						int.TryParse(mData.CondTypeArgs[0], out _maxcount);
					}
					else
					{
						SdkManager.Bugly_Report("LocalSave_Achieve", Utils.FormatString(" id:{0}  CondTypeArgs.Length == 0", achieveid));
					}
				}
				return _maxcount;
			}
		}

		[JsonIgnore]
		public bool isfinish => currentcount >= maxcount;

		[JsonIgnore]
		public AchieveConditionBase mCondition
		{
			get
			{
				if (_condition == null)
				{
					int condType = mData.CondType;
					Type type = Type.GetType(Utils.GetString("AchieveCondition", condType));
					_condition = (type.Assembly.CreateInstance(Utils.GetString("AchieveCondition", condType)) as AchieveConditionBase);
					_condition.Init(this);
				}
				return _condition;
			}
		}
	}

	[Serializable]
	public class AchieveData : LocalSaveBase
	{
		public Dictionary<int, AchieveDataOne> list = new Dictionary<int, AchieveDataOne>();

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_Achieve1", this);
		}

		public void Init()
		{
		}

		private void InitID(int id)
		{
			if (!list.ContainsKey(id))
			{
				list.Add(id, new AchieveDataOne
				{
					achieveid = id
				});
				Refresh();
			}
		}

		public void AddProgress(int id, int count)
		{
			InitID(id);
			list[id].currentcount += count;
			Refresh();
		}

		public bool IsFinish(int id)
		{
			InitID(id);
			return list[id].isfinish;
		}

		public bool Isgot(int id)
		{
			InitID(id);
			return list[id].isgot;
		}

		public AchieveDataOne Get(int id)
		{
			InitID(id);
			return list[id];
		}
	}

	[Serializable]
	public class ActiveOne
	{
		public int Index;

		public int Count;
	}

	[Serializable]
	public class ActiveData : LocalSaveBase
	{
		public List<ActiveOne> list = new List<ActiveOne>();

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_Active", this);
		}

		public void Init()
		{
			if (list.Count == 0)
			{
				List<Stage_Level_activityModel.ActivityTypeData> difficults = LocalModelManager.Instance.Stage_Level_activity.GetDifficults();
				int i = 0;
				for (int count = difficults.Count; i < count; i++)
				{
					Stage_Level_activityModel.ActivityTypeData activityTypeData = difficults[i];
					int count2 = activityTypeData.GetCount(0);
					list.Add(new ActiveOne
					{
						Index = i,
						Count = count2
					});
				}
				Refresh();
			}
		}
	}

	[Serializable]
	public class BattleInBase : LocalSaveBase
	{
		public bool bHaveBattle;

		public ulong serveruserid;

		public int level;

		public float exp;

		public float gold;

		public List<int> skillids = new List<int>();

		public List<int> goodids = new List<int>();

		public List<EquipOne> equips = new List<EquipOne>();

		public long hp;

		public int RoomID;

		public int ResourcesID;

		public string TmxID;

		public int reborn_skill_count;

		public int reborn_ui_count;

		public int leveluptype;

		public List<int> levelupskills = new List<int>();

		public List<int> learnskills = new List<int>();

		public bool bGoldTurn;

		public int stage;

		public List<bool> firstshopbuy;

		public List<int> potions;

		private Sequence seq;

		public static string GetFileName(ulong serveruserid)
		{
			return Utils.FormatString("{0}-battle.txt", serveruserid);
		}

		protected override void OnRefresh()
		{
			if (seq != null)
			{
				seq.Kill();
				seq = null;
			}
			seq = DOTween.Sequence().AppendInterval(0.1f).AppendCallback(delegate
			{
				FileUtils.WriteBattleInThread(this);
			})
				.SetUpdate(isIndependentUpdate: true);
		}

		public void DeInit()
		{
			bGoldTurn = false;
			SetHaveBattle(value: false);
			reborn_skill_count = 0;
			reborn_ui_count = 0;
			level = 1;
			exp = 0f;
			hp = 0L;
			RoomID = 0;
			ResourcesID = 1;
			TmxID = string.Empty;
			levelupskills = null;
			equips.Clear();
			learnskills.Clear();
			skillids.Clear();
			goodids.Clear();
			LevelInit();
			firstshopbuy = (firstshopbuy = new List<bool>
			{
				false,
				false
			});
			potions.Clear();
			OnDeInit();
			Refresh();
		}

		protected virtual void OnDeInit()
		{
		}

		public void AddRebornSkill()
		{
			reborn_skill_count++;
			Refresh();
		}

		public void AddRebornUI()
		{
			reborn_ui_count++;
			Refresh();
		}

		public void AddEquip(EquipOne one)
		{
			equips.Add(one);
			Refresh();
		}

		public void LevelInit()
		{
			if (firstshopbuy == null || firstshopbuy.Count < 2)
			{
				firstshopbuy = new List<bool>
				{
					false,
					false
				};
			}
			if (potions == null)
			{
				potions = new List<int>();
			}
		}

		public void AddPotion(int id)
		{
			if (potions == null)
			{
				potions = new List<int>();
			}
			potions.Add(id);
		}

		public override string ToString()
		{
			if (GetHaveBattle())
			{
				return Utils.FormatString("Level:{1}, Exp:{2}, HP:{3}", level, exp, hp);
			}
			return Utils.FormatString("BattleInData don't in battle.");
		}

		public bool GetHaveBattle()
		{
			if (serveruserid == Instance.GetServerUserID())
			{
				return bHaveBattle;
			}
			return false;
		}

		public void SetHaveBattle(bool value)
		{
			bHaveBattle = value;
			Refresh();
		}

		public void CheckDifferentID()
		{
			if (serveruserid != Instance.GetServerUserID())
			{
				DeInit();
			}
		}

		public static BattleInBase Get()
		{
			BattleInBase battleIn = FileUtils.GetBattleIn();
			if (!battleIn.GetHaveBattle())
			{
				battleIn.DeInit();
			}
			return battleIn;
		}
	}

	[Serializable]
	public class BoxDropEquip : LocalSaveBase
	{
		public int stage = -1;

		public int count;

		public int dropid;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_BoxDrop", this);
		}

		private void UpdateStage()
		{
			int num = Instance.Stage_GetStage();
			if (num != stage)
			{
				stage = num;
				count = 0;
				dropid = OnGetDropID(num);
				Refresh();
			}
		}

		protected int OnGetDropID(int currentstage)
		{
			return LocalModelManager.Instance.Box_SilverBox.GetBeanById(currentstage).SingleDrop;
		}

		public Drop_DropModel.DropData GetRandom()
		{
			UpdateStage();
			Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(dropid);
			count++;
			List<Drop_DropModel.DropData> list = null;
			if (count >= beanById.RandNum)
			{
				count = 0;
				dropid = beanById.JumpDrop;
			}
			Refresh();
			list = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.DropID);
			return list[0];
		}
	}

	[Serializable]
	public class DropCard : LocalSaveBase
	{
		public int count;

		public int dropid = 1001;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_CardDrop", this);
		}

		public void InitCount(int allcount)
		{
			int num = allcount;
			dropid = 1001;
			count = 0;
			while (allcount > 0)
			{
				Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(dropid);
				int randNum = beanById.RandNum;
				allcount -= randNum;
				if (allcount >= 0)
				{
					dropid = beanById.JumpDrop;
				}
				else
				{
					count = randNum + allcount;
				}
			}
		}

		public Drop_DropModel.DropData GetRandom()
		{
			Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(dropid);
			List<Drop_DropModel.DropData> list = null;
			list = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.DropID);
			if (list.Count == 0)
			{
				SdkManager.Bugly_Report("LocalSave_BoxDrop", Utils.FormatString("Drop_Drop[{0}] get null.", beanById.DropID));
			}
			if (Instance.GetCardByID(list[0].id).IsMaxLevel)
			{
				return GetRandom();
			}
			return list[0];
		}

		public void GetSucceed()
		{
			count++;
			Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(dropid);
			if (count >= beanById.RandNum)
			{
				dropid = beanById.JumpDrop;
				count = 0;
			}
		}
	}

	[Serializable]
	public class CardOne
	{
		public int CardID;

		public int level;

		public int HaveCount;

		[JsonIgnore]
		private Skill_slotout _data;

		[JsonIgnore]
		public Skill_slotout data
		{
			get
			{
				if (_data == null)
				{
					_data = LocalModelManager.Instance.Skill_slotout.GetBeanById(CardID);
				}
				return _data;
			}
		}

		[JsonIgnore]
		public bool Unlock => level > 0;

		[JsonIgnore]
		public bool IsMaxLevel => level >= data.LevelLimit;

		public CardOne()
		{
		}

		public CardOne(int cardid, int level, int count)
		{
			CardID = cardid;
			this.level = 1;
			HaveCount = count;
		}

		public override string ToString()
		{
			return Utils.FormatString("ID:{0}", CardID);
		}

		public string GetValue(string value)
		{
			string text = value.Substring(value.Length - 1, 1);
			if (text != null && text == "f")
			{
				value = value.Substring(0, value.Length - 1);
				int.TryParse(value, out int result);
				return Utils.FormatString("{0}", (float)result / 1000f);
			}
			return value;
		}

		public string GetTypeName(int index)
		{
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(data.BaseAttributes[index]);
			return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("Up_{0}", goodData.goodType));
		}

		public string GetCurrentAttribute(int index)
		{
			return GetAttribute(index, level - 1);
		}

		public string GetNextAttribute(int index)
		{
			return GetAttribute(index, level);
		}

		private string GetAttribute(int index, int addlevel)
		{
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(data.BaseAttributes[index]);
			if (goodData.percent)
			{
				if (addlevel > 0)
				{
					float num = data.AddAttributes[index] * 100f;
					goodData.value += (addlevel - 1) * (long)num;
				}
				return Utils.FormatString("{0}%", (float)goodData.value / 100f);
			}
			if (addlevel > 0)
			{
				float num2 = data.AddAttributes[index];
				goodData.value += (addlevel - 1) * (long)num2;
			}
			return goodData.value.ToString();
		}
	}

	[Serializable]
	public class CardData : LocalSaveBase
	{
		public Dictionary<int, CardOne> mList = new Dictionary<int, CardOne>();

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_Card", this);
		}

		public void Init()
		{
			if (!IsEmpty())
			{
				return;
			}
			IList<Skill_slotout> allBeans = LocalModelManager.Instance.Skill_slotout.GetAllBeans();
			for (int i = 0; i < allBeans.Count; i++)
			{
				Skill_slotout skill_slotout = allBeans[i];
				if (skill_slotout.GroupID > 100)
				{
					CardOne cardOne = new CardOne(skill_slotout.GroupID, 1, 0);
					cardOne.CardID = skill_slotout.GroupID;
					cardOne.HaveCount = 0;
					cardOne.level = 0;
					mList.Add(skill_slotout.GroupID, cardOne);
				}
			}
			Refresh();
		}

		private bool IsEmpty()
		{
			return mList.Count == 0;
		}

		public int GetCount()
		{
			return mList.Count;
		}

		public CardOne AddOne(int cardid, int count)
		{
			if (mList.TryGetValue(cardid, out CardOne value))
			{
				value.level++;
			}
			else
			{
				value = new CardOne(cardid, 1, count);
				value.CardID = cardid;
				value.HaveCount = count;
				value.level = 1;
				mList.Add(cardid, value);
			}
			Refresh();
			if (CardUpdateEvent != null)
			{
				CardUpdateEvent();
			}
			return value;
		}

		public void SetOne(int cardid, int level)
		{
			if (mList.TryGetValue(cardid, out CardOne value))
			{
				value.level = level;
				Refresh();
			}
		}

		public Dictionary<int, CardOne> GetCards()
		{
			return mList;
		}

		public CardOne GetCardByID(int id)
		{
			CardOne value = null;
			mList.TryGetValue(id, out value);
			return value;
		}

		public bool HaveCard(int id)
		{
			CardOne value = null;
			mList.TryGetValue(id, out value);
			if (value == null)
			{
				return false;
			}
			return value.level > 0;
		}

		private int GetIndex(CardOne one)
		{
			int num = 0;
			Dictionary<int, CardOne>.Enumerator enumerator = mList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.CardID == one.CardID)
				{
					return num;
				}
				num++;
			}
			SdkManager.Bugly_Report("LocalSave_Card.cs", Utils.FormatString("CardData.GetIndex {0} is dont have!", one.CardID));
			return 0;
		}

		public bool GetAllMax()
		{
			Dictionary<int, CardOne>.Enumerator enumerator = mList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.Value.IsMaxLevel)
				{
					return false;
				}
			}
			return true;
		}

		public void Clear()
		{
			mList.Clear();
			Init();
		}
	}

	[Serializable]
	public class ChallengeData : LocalSaveBase
	{
		public int ChallengeID = 2101;

		public bool bFirstIn;

		public bool isinit;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_Challenge", this);
		}
	}

	[Serializable]
	public class EquipOne
	{
		[JsonIgnore]
		public string UniqueID;

		public ulong RowID;

		public int EquipID;

		public int Level = 1;

		public int Count;

		public int WearIndex = -1;

		public bool bNew = true;

		[JsonIgnore]
		private int _qualityUpState = -1;

		[JsonIgnore]
		private Equip_equip _data;

		[JsonIgnore]
		public int Position => data.Position;

		[JsonIgnore]
		public int Quality => data.Quality;

		[JsonIgnore]
		public bool QualityCanUp
		{
			get
			{
				if (_qualityUpState < 0)
				{
					if (LocalModelManager.Instance.Equip_equip.GetBeanById(EquipID + 1) == null)
					{
						_qualityUpState = 2;
					}
					else
					{
						_qualityUpState = 1;
					}
				}
				return _qualityUpState == 1;
			}
		}

		[JsonIgnore]
		public Color qualityColor => QualityColors[Quality];

		[JsonIgnore]
		public int IdBase
		{
			get
			{
				int result = data.Id;
				if (!Overlying)
				{
					result = data.Id / 100 * 100 + 1;
				}
				return result;
			}
		}

		[JsonIgnore]
		public int IconBase
		{
			get
			{
				int result = data.EquipIcon;
				if (!Overlying)
				{
					result = data.EquipIcon / 100 * 100 + 1;
				}
				return result;
			}
		}

		[JsonIgnore]
		public bool IsWear => WearIndex >= 0;

		[JsonIgnore]
		public Equip_equip data
		{
			get
			{
				if (_data == null || _data.Id != EquipID)
				{
					_data = LocalModelManager.Instance.Equip_equip.GetBeanById(EquipID);
				}
				return _data;
			}
		}

		[JsonIgnore]
		public bool IsBaby => Position == 6;

		[JsonIgnore]
		public Sprite TypeIcon
		{
			get
			{
				if (Overlying)
				{
					return null;
				}
				return SpriteManager.GetCharUI(Utils.FormatString("equip_type_{0}", Position));
			}
		}

		[JsonIgnore]
		public bool ShowQualityGoldImage
		{
			get
			{
				if (Overlying)
				{
					return false;
				}
				return Quality == 5;
			}
		}

		[JsonIgnore]
		public int CurrentMaxLevel => data.MaxLevel;

		[JsonIgnore]
		public bool CanLevelUp
		{
			get
			{
				if (!Overlying && !IsMax && HaveMatCount >= NeedMatCount)
				{
					return true;
				}
				return false;
			}
		}

		[JsonIgnore]
		public bool CanCombine => Instance.Equip_can_combine(this);

		[JsonIgnore]
		public int NeedMatCount
		{
			get
			{
				Equip_Upgrade beanById = LocalModelManager.Instance.Equip_Upgrade.GetBeanById(Level);
				if (beanById != null)
				{
					return beanById.UpMaterials;
				}
				SdkManager.Bugly_Report("LocalSave_Equip", Utils.FormatString("NeedMatCount level:{0} is not in excel.", Level));
				return 9999;
			}
		}

		[JsonIgnore]
		public int NeedMatID => data.UpgradeNeed;

		[JsonIgnore]
		public string NeedMatUniqueID => Instance.GetPropByID(data.UpgradeNeed)?.UniqueID;

		[JsonIgnore]
		public int HaveMatCount => Instance.GetPropByID(data.UpgradeNeed)?.Count ?? 0;

		[JsonIgnore]
		public int BreakNeed => data.BreakNeed;

		[JsonIgnore]
		public EquipType PropType
		{
			get
			{
				if (data == null)
				{
					UnityEngine.Debug.Log(Utils.FormatString("UniqueID:{0} EquipID:{1} Level:{2}", UniqueID, EquipID, Level));
				}
				return (EquipType)data.PropType;
			}
		}

		[JsonIgnore]
		public bool Overlying
		{
			get
			{
				if (data == null)
				{
					SdkManager.Bugly_Report("LocalSave_Equip", Utils.FormatString("UniqueID:{0} EquipID:{1} Level:{2} data is null", UniqueID, EquipID, Level));
				}
				return data.Overlying != 0;
			}
		}

		[JsonIgnore]
		public int NeedGold
		{
			get
			{
				Equip_Upgrade beanById = LocalModelManager.Instance.Equip_Upgrade.GetBeanById(Level);
				if (beanById != null)
				{
					return beanById.UpCoins;
				}
				SdkManager.Bugly_Report("LocalSave_Equip", Utils.FormatString("NeedGold level:{0} is not in excel.", Level));
				return 0;
			}
		}

		[JsonIgnore]
		public bool GoldEnough => Instance.GetGold() >= NeedGold;

		[JsonIgnore]
		public bool IsMax => Level >= CurrentMaxLevel;

		[JsonIgnore]
		public bool CountEnough => HaveMatCount >= NeedMatCount;

		[JsonIgnore]
		public string NameString
		{
			get
			{
				if (Overlying)
				{
					return NameOnlyString;
				}
				string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("装备名称{0}", IdBase));
				return Utils.FormatString("{0}·{1}", QualityString, languageByTID);
			}
		}

		[JsonIgnore]
		public string QualityString
		{
			get
			{
				if (Quality > 0)
				{
					return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("equip_quality_{0}", Quality));
				}
				return string.Empty;
			}
		}

		[JsonIgnore]
		public string NameOnlyString => GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("装备名称{0}", IdBase));

		[JsonIgnore]
		public string InfoString => GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("装备描述{0}", IdBase));

		[JsonIgnore]
		public string SpecialInfoString => GameLogic.Hold.Language.GetEquipSpecialInfo(IdBase);

		[JsonIgnore]
		public Sprite Icon => SpriteManager.GetEquip(data.EquipIcon);

		public void EquipWear(SelfAttributeData data)
		{
			List<Goods_goods.GoodData> equipAttributes = LocalModelManager.Instance.Equip_equip.GetEquipAttributes(this);
			int i = 0;
			for (int count = equipAttributes.Count; i < count; i++)
			{
				equipAttributes[i].value = (long)((float)equipAttributes[i].value * (1f + data.GetUpPercent(Position)));
				data.attribute.Excute(equipAttributes[i]);
			}
			List<string> equipAddAttributes = LocalModelManager.Instance.Equip_equip.GetEquipAddAttributes(this);
			int j = 0;
			for (int count2 = equipAddAttributes.Count; j < count2; j++)
			{
				data.attribute.Excute(equipAddAttributes[j]);
			}
		}

		public List<int> GetSkills()
		{
			return LocalModelManager.Instance.Equip_equip.GetSkills(this);
		}

		public void QualityUp()
		{
			EquipID++;
			_qualityUpState = -1;
			_data = null;
		}

		public string GetAttName(int index)
		{
			if (index < 0 || index >= data.Attributes.Length)
			{
				return string.Empty;
			}
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(data.Attributes[index]);
			return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("Attr_{0}", goodData.goodType));
		}

		public string GetCurrentAttributeString(int index)
		{
			if (index < 0 || index >= data.Attributes.Length)
			{
				return string.Empty;
			}
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(data.Attributes[index]);
			string empty = string.Empty;
			if (goodData.percent)
			{
				goodData.value += data.AttributesUp[index] * (Level - 1) * 10000;
				return Utils.FormatString("{0}{1}%", MathDxx.GetSymbolString(goodData.value), (float)goodData.value / 10000f);
			}
			goodData.value += data.AttributesUp[index] * (Level - 1);
			return Utils.FormatString("{0}{1}", MathDxx.GetSymbolString(goodData.value), goodData.value);
		}

		public void CombineReturn(List<Drop_DropModel.DropData> list)
		{
			if (Level <= 1)
			{
				return;
			}
			EquipOne propByID = Instance.GetPropByID(data.UpgradeNeed);
			if (propByID == null)
			{
				return;
			}
			for (int i = 1; i < Level; i++)
			{
				int upCoins = LocalModelManager.Instance.Equip_Upgrade.GetBeanById(i).UpCoins;
				int upMaterials = LocalModelManager.Instance.Equip_Upgrade.GetBeanById(i).UpMaterials;
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					Drop_DropModel.DropData dropData = list[j];
					if (dropData.type == global::PropType.eCurrency && dropData.id == 1)
					{
						dropData.count += upCoins;
						flag = true;
					}
				}
				if (!flag)
				{
					list.Add(new Drop_DropModel.DropData
					{
						type = global::PropType.eCurrency,
						id = 1,
						count = upCoins
					});
				}
				bool flag2 = false;
				for (int k = 0; k < list.Count; k++)
				{
					Drop_DropModel.DropData dropData2 = list[k];
					if (dropData2.type == global::PropType.eEquip && dropData2.id == propByID.EquipID)
					{
						dropData2.count += upMaterials;
						flag2 = true;
					}
				}
				if (!flag2)
				{
					list.Add(new Drop_DropModel.DropData
					{
						type = global::PropType.eEquip,
						id = propByID.EquipID,
						count = upMaterials
					});
				}
			}
		}

		public List<Goods_goods.GoodData> GetBabyAttributes()
		{
			List<Goods_goods.GoodData> list = new List<Goods_goods.GoodData>();
			int i = 0;
			for (int num = data.Attributes.Length; i < num; i++)
			{
				string text = data.Attributes[i];
				if (text.Contains("EquipBaby:"))
				{
					text = text.Replace("EquipBaby:", string.Empty);
					Goods_goods.GoodData goodData = Goods_goods.GetGoodData(text);
					if (goodData.percent)
					{
						goodData.value += (Level - 1) * data.AttributesUp[i] * 100;
					}
					else
					{
						goodData.value += (Level - 1) * data.AttributesUp[i];
					}
					list.Add(goodData);
				}
			}
			int j = 0;
			for (int num2 = data.AdditionSkills.Length; j < num2; j++)
			{
				string text2 = data.AdditionSkills[j];
				if (!int.TryParse(text2, out int _) && text2.Contains("EquipBaby:"))
				{
					text2 = text2.Replace("EquipBaby:", string.Empty);
					Goods_goods.GoodData goodData2 = Goods_goods.GetGoodData(text2);
					list.Add(goodData2);
				}
			}
			return list;
		}

		public List<int> GetBabySkills()
		{
			List<int> list = new List<int>();
			int i = 0;
			for (int num = data.AdditionSkills.Length; i < num; i++)
			{
				string s = data.AdditionSkills[i];
				if (int.TryParse(s, out int result))
				{
					list.Add(result);
				}
			}
			return list;
		}

		public void Clear()
		{
			_data = null;
		}

		public override string ToString()
		{
			string arg = string.Empty;
			List<Goods_goods.GoodData> equipAttributes = LocalModelManager.Instance.Equip_equip.GetEquipAttributes(this);
			int i = 0;
			for (int count = equipAttributes.Count; i < count; i++)
			{
				arg = arg + equipAttributes[i] + "|";
			}
			return Utils.FormatString("UniqueID:{0} EquipID:{1} Level:{2}", UniqueID, EquipID, Level);
		}
	}

	[Serializable]
	public class EquipData : LocalSaveBase
	{
		public Dictionary<string, EquipOne> list = new Dictionary<string, EquipOne>();

		public List<string> wears = new List<string>();

		[JsonIgnore]
		public List<string> invalids = new List<string>();

		[JsonIgnore]
		public bool bRefresh;

		[JsonIgnore]
		private bool bInitWear;

		[JsonIgnore]
		public Action mUpdateAction;

		[JsonIgnore]
		private int equipidd = 1010101;

		[JsonIgnore]
		public List<int> mEquipExpCanDropList = new List<int>();

		[JsonIgnore]
		private Dictionary<int, int> mCombines = new Dictionary<int, int>();

		[JsonIgnore]
		public bool wear_enable
		{
			get
			{
				if (wears == null || wears.Count < 6)
				{
					return false;
				}
				return true;
			}
		}

		protected override void OnRefresh()
		{
			bRefresh = true;
			FileUtils.WriteEquip("localequip.txt", this);
		}

		public void Init(List<CEquipmentItem> equips)
		{
			if (equips == null)
			{
				return;
			}
			invalids.Clear();
			bRefresh = true;
			check_rowid_same();
			string uniqueid = null;
			int i = 0;
			for (int count = equips.Count; i < count; i++)
			{
				CEquipmentItem cEquipmentItem = equips[i];
				EquipOne equipOne = get_by_row_id(cEquipmentItem.m_nRowID);
				if (equipOne != null)
				{
					equipOne.EquipID = (int)cEquipmentItem.m_nEquipID;
					equipOne.Count = (int)cEquipmentItem.m_nFragment;
					equipOne.Level = (int)cEquipmentItem.m_nLevel;
					continue;
				}
				Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById((int)cEquipmentItem.m_nEquipID);
				if (beanById == null)
				{
					continue;
				}
				bool flag = true;
				if (beanById.Overlying == 1)
				{
					EquipOne propByID = GetPropByID(beanById.Id);
					if (propByID != null)
					{
						propByID.RowID = cEquipmentItem.m_nRowID;
						propByID.Count = (int)cEquipmentItem.m_nFragment;
						flag = false;
					}
				}
				if (flag)
				{
					equipOne = new EquipOne();
					equipOne.UniqueID = Utils.GenerateUUID();
					equipOne.RowID = cEquipmentItem.m_nRowID;
					equipOne.EquipID = (int)cEquipmentItem.m_nEquipID;
					equipOne.Level = (int)cEquipmentItem.m_nLevel;
					equipOne.Count = (int)cEquipmentItem.m_nFragment;
					equipOne.bNew = false;
					list.Add(equipOne.UniqueID, equipOne);
				}
			}
			Dictionary<string, EquipOne>.Enumerator enumerator = list.GetEnumerator();
			bool flag2 = false;
			while (enumerator.MoveNext())
			{
				flag2 = false;
				EquipOne equipOne = enumerator.Current.Value;
				int j = 0;
				for (int count2 = equips.Count; j < count2; j++)
				{
					CEquipmentItem cEquipmentItem = equips[j];
					if (equipOne.RowID == cEquipmentItem.m_nRowID)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					invalids.Add(equipOne.UniqueID);
				}
				else if (equipOne != null)
				{
					uniqueid = equipOne.UniqueID;
				}
			}
			if (wears.Count == 0)
			{
				for (int k = 0; k < 6; k++)
				{
					wears.Add(null);
				}
			}
			bool flag3 = !string.IsNullOrEmpty(wears[0]);
			if (NetManager.mNetCache.IsEmpty)
			{
				check_invalid_internal(force: true);
			}
			if (equips.Count == 1)
			{
				check_invalid_internal(force: true);
				if (flag3)
				{
					EquipOne equipByUniqueID = GetEquipByUniqueID(uniqueid);
					if (equipByUniqueID != null)
					{
						int index = -1;
						if (Instance.Equip_GetCanWearIndex(equipByUniqueID, out index))
						{
							EquipWear(equipByUniqueID, index);
						}
					}
				}
			}
			if (!bInitWear)
			{
				bInitWear = true;
			}
			else
			{
				CheckWears();
			}
			combine_refresh();
			Refresh_EquipExp_CanDrop();
			Refresh();
			Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
		}

		public void init_equipone_uniqueid()
		{
			Dictionary<string, EquipOne>.Enumerator enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.UniqueID = enumerator.Current.Key;
			}
		}

		public void SetWears(bool value)
		{
			bInitWear = value;
		}

		private void CheckWears()
		{
			int i = 0;
			for (int num = wears.Count - 1; i < num; i++)
			{
				string text = wears[i];
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				int j = i + 1;
				for (int count = wears.Count; j < count; j++)
				{
					string b = wears[j];
					if (text == b)
					{
						wears[j] = null;
					}
				}
			}
			int k = 0;
			for (int count2 = wears.Count; k < count2; k++)
			{
				string text2 = wears[k];
				if (!string.IsNullOrEmpty(text2) && !list.ContainsKey(text2))
				{
					wears[k] = null;
				}
			}
			int l = 0;
			for (int count3 = wears.Count; l < count3; l++)
			{
				string text3 = wears[l];
				if (!string.IsNullOrEmpty(text3))
				{
					EquipOne value = null;
					if (list.TryGetValue(text3, out value))
					{
						value.WearIndex = l;
					}
				}
			}
		}

		public void Init()
		{
			bRefresh = true;
			if (wears.Count == 0)
			{
				for (int i = 0; i < 6; i++)
				{
					wears.Add(null);
				}
				EquipOne equipOne = new EquipOne();
				equipOne.EquipID = equipidd;
				equipOne.Level = 1;
				equipOne.Count = 1;
				equipOne.bNew = false;
				EquipOne equipOne2 = equipOne;
				equipOne2.UniqueID = Utils.GenerateUUID();
				list.Add(equipOne2.UniqueID, equipOne2);
				EquipWear(equipOne2, 0);
			}
			else
			{
				bInitWear = true;
			}
			CheckWears();
			combine_refresh();
			Refresh_EquipExp_CanDrop();
			UpdateCallBack();
			Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
		}

		private void check_rowid_same()
		{
		}

		public void check_invalid()
		{
			check_invalid_internal();
		}

		private void check_invalid_internal(bool force = false)
		{
			int i = 0;
			for (int count = invalids.Count; i < count; i++)
			{
				if (list.TryGetValue(invalids[i], out EquipOne value) && (value.RowID == 0 || force))
				{
					RemoveEquip(value.UniqueID);
				}
			}
			invalids.Clear();
			CheckWears();
			combine_refresh();
		}

		private EquipOne get_by_row_id(ulong rowid)
		{
			Dictionary<string, EquipOne>.Enumerator enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.RowID == rowid)
				{
					return enumerator.Current.Value;
				}
			}
			return null;
		}

		public bool IsEmpty()
		{
			return list.Count == 0;
		}

        //@TODO ADD Equipment Item
        public void AddEquipInternal(EquipOne data)
		{
			if (data.UniqueID.Length == 0)
			{
				SdkManager.Bugly_Report("AddEquipInternal", Utils.FormatString("m_nRowID:{0} uuid is null", data.RowID));
				data.UniqueID = Utils.GenerateUUID();
			}
			if (data.Overlying)
			{
				data.bNew = false;
				EquipOne propByID = GetPropByID(data.EquipID);
				if (propByID != null)
				{
					propByID.Count += data.Count;
				}
				else
				{
					list.Add(data.UniqueID, data);
				}
			}
			else
			{
				bool flag = false;
				if (data.RowID != 0)
				{
					EquipOne equipOne = get_by_rowid(data.RowID);
					if (equipOne != null)
					{
						equipOne.RowID = data.RowID;
						equipOne.EquipID = data.EquipID;
						equipOne.Level = data.Level;
						equipOne.Count = data.Count;
						flag = true;
					}
				}
				if (!flag)
				{
					if (!list.TryGetValue(data.UniqueID, out EquipOne value))
					{
						Debugger.LogEquipGet("UniqueID = " + data.UniqueID + " rowid " + data.RowID + " equipid " + data.EquipID + " dont in list");
						list.Add(data.UniqueID, data);
					}
					else
					{
						value.RowID = data.RowID;
						value.EquipID = data.EquipID;
						value.Level = data.Level;
						value.Count = data.Count;
						Debugger.LogEquipGet("UniqueID = " + data.UniqueID + " rowid " + data.RowID + " equipid " + data.EquipID + " is in list");
					}
				}
			}
#if ENABLE_NET_MANAGER
			GameLogic.Hold.Guide.mEquip.StartGuide();
#endif
			Refresh_EquipExp_CanDrop(data);
			combine_refresh();
			Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
			Refresh();
		}

		private EquipOne get_by_rowid(ulong rowid)
		{
			Dictionary<string, EquipOne>.Enumerator enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.RowID == rowid)
				{
					return enumerator.Current.Value;
				}
			}
			return null;
		}

		public void SetEquips(CEquipmentItem[] data)
		{
			int i = 0;
			for (int num = data.Length; i < num; i++)
			{
				CEquipmentItem cEquipmentItem = data[i];
				if (cEquipmentItem != null)
				{
					Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById((int)cEquipmentItem.m_nEquipID);
					if (beanById != null && beanById.Overlying == 0)
					{
						EquipOne equipOne = new EquipOne();
						equipOne.UniqueID = cEquipmentItem.m_nUniqueID;
						equipOne.RowID = cEquipmentItem.m_nRowID;
						equipOne.EquipID = (int)cEquipmentItem.m_nEquipID;
						equipOne.Level = (int)cEquipmentItem.m_nLevel;
						equipOne.Count = (int)cEquipmentItem.m_nFragment;
						AddEquipInternal(equipOne);
					}
				}
			}
			Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
			combine_refresh();
			Refresh();
		}

		public void RemoveEquip(string uniqueid)
		{
			EquipOne value = null;
			if (!list.TryGetValue(uniqueid, out value))
			{
				return;
			}
			if (value != null)
			{
				if (!value.Overlying)
				{
					EquipUnwear(uniqueid);
					list.Remove(uniqueid);
				}
				else
				{
					value.Count = 0;
				}
			}
			combine_refresh();
			Refresh();
		}

		public void EquipWear(EquipOne data, int index)
		{
			if (list.TryGetValue(data.UniqueID, out EquipOne value))
			{
				if (!string.IsNullOrEmpty(wears[index]))
				{
					EquipUnwear(wears[index]);
				}
				value.WearIndex = index;
				wears[index] = data.UniqueID;
				Refresh();
				UpdateCallBack();
				Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
			}
			else
			{
				SdkManager.Bugly_Report("LocalSave_Equip", Utils.FormatString("UniqueID:{0} EquipID:{1} don't int bags.", data.UniqueID, data.EquipID));
			}
		}

		public void EquipUnwear(string uniqueid)
		{
			if (list.TryGetValue(uniqueid, out EquipOne value))
			{
				if (value.WearIndex >= 0 && value.WearIndex < wears.Count && value.WearIndex >= 0 && value.WearIndex < wears.Count)
				{
					wears[value.WearIndex] = null;
				}
				value.WearIndex = -1;
				Refresh();
				UpdateCallBack();
				Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
			}
		}

		public void UpdateEquip(EquipOne data)
		{
			if (list.ContainsKey(data.UniqueID))
			{
				list[data.UniqueID] = data;
				Refresh();
			}
		}

		public EquipOne GetEquipByUniqueID(string uniqueid)
		{
			if (string.IsNullOrEmpty(uniqueid))
			{
				return null;
			}
			if (list.TryGetValue(uniqueid, out EquipOne value))
			{
				return value;
			}
			return null;
		}

		public EquipOne GetPropByID(int equipid)
		{
			Dictionary<string, EquipOne>.Enumerator enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.EquipID == equipid)
				{
					return enumerator.Current.Value;
				}
			}
			return null;
		}

		public void EquipLevelUp(EquipOne one)
		{
			EquipOne equipByUniqueID = GetEquipByUniqueID(one.UniqueID);
			if (equipByUniqueID != null)
			{
				equipByUniqueID.Level++;
				Refresh();
				UpdateCallBack();
			}
		}

		public void SetNew(string uniqueid)
		{
			if (list.TryGetValue(uniqueid, out EquipOne value))
			{
				value.bNew = false;
				Refresh();
				Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
			}
		}

		public int GetNewCount()
		{
			int num = 0;
			Dictionary<string, EquipOne>.Enumerator enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EquipOne value = enumerator.Current.Value;
				if (value.bNew)
				{
					num++;
				}
			}
			return num;
		}

		public List<EquipOne> GetHaveEquips(bool havewear)
		{
			List<EquipOne> list = new List<EquipOne>();
			Dictionary<string, EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EquipOne value = enumerator.Current.Value;
				if (value.PropType == EquipType.eEquip && ((value.WearIndex < 0 && !havewear) || havewear))
				{
					list.Add(value);
				}
			}
			return list;
		}

		public List<EquipOne> GetProps(EquipType type, bool havewear)
		{
			List<EquipOne> list = new List<EquipOne>();
			Dictionary<string, EquipOne>.Enumerator enumerator = this.list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EquipOne value = enumerator.Current.Value;
				if (value.PropType != type && type != EquipType.eAll)
				{
					continue;
				}
				if (value.PropType == EquipType.eEquip)
				{
					if ((value.WearIndex < 0 && !havewear) || havewear)
					{
						list.Add(value);
					}
				}
				else if (value.Count > 0)
				{
					list.Add(value);
				}
			}
			return list;
		}

		private void UpdateCallBack()
		{
			if (mUpdateAction != null)
			{
				mUpdateAction();
			}
		}

		public int GetCanWearCount()
		{
			int num = 0;
			List<EquipOne> haveEquips = GetHaveEquips(havewear: false);
			int i = 0;
			for (int count = haveEquips.Count; i < count; i++)
			{
				if (Instance.Equip_GetIsEmpty(haveEquips[i]))
				{
					num++;
				}
			}
			return num;
		}

		public int GetCanUpCount()
		{
			int num = 0;
			int i = 0;
			for (int count = wears.Count; i < count; i++)
			{
				EquipOne equipByUniqueID = GetEquipByUniqueID(wears[i]);
				if (equipByUniqueID != null && equipByUniqueID.CanLevelUp)
				{
					num++;
				}
			}
			return num;
		}

		public void Refresh_EquipExp_CanDrop(int equipexpid)
		{
			if (equipexpid > 0 && !mEquipExpCanDropList.Contains(equipexpid))
			{
				mEquipExpCanDropList.Add(equipexpid);
			}
		}

		public void Refresh_EquipExp_CanDrop(EquipOne one)
		{
			if (one != null && one.data != null && one.NeedMatID > 0)
			{
				Refresh_EquipExp_CanDrop(one.NeedMatID);
			}
		}

		public void Refresh_EquipExp_CanDrop()
		{
			Dictionary<string, EquipOne>.Enumerator enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				int needMatID = enumerator.Current.Value.NeedMatID;
				Refresh_EquipExp_CanDrop(needMatID);
			}
		}

		public bool Get_EquipExp_CanDrop(int equipexpid)
		{
			return mEquipExpCanDropList.Contains(equipexpid);
		}

		public bool Get_EquipExp_CanDrop(EquipOne one)
		{
			if (one != null && one.data != null && one.NeedMatID > 0)
			{
				return Get_EquipExp_CanDrop(one.NeedMatID);
			}
			return false;
		}

		private void combine_refresh()
		{
			mCombines.Clear();
			Dictionary<string, EquipOne>.Enumerator enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.Value.Overlying)
				{
					int equipID = enumerator.Current.Value.EquipID;
					if (!mCombines.ContainsKey(equipID))
					{
						mCombines.Add(equipID, 0);
					}
					Dictionary<int, int> dictionary;
					int key;
					(dictionary = mCombines)[key = equipID] = dictionary[key] + 1;
				}
			}
		}

		public int combine_can_count()
		{
			int num = 0;
			Dictionary<int, int>.Enumerator enumerator = mCombines.GetEnumerator();
			while (enumerator.MoveNext())
			{
				int breakNeed = LocalModelManager.Instance.Equip_equip.GetBeanById(enumerator.Current.Key).BreakNeed;
				if (enumerator.Current.Value >= breakNeed)
				{
					num++;
				}
			}
			return num;
		}

		public bool combine_can(EquipOne one)
		{
			int value = 0;
			if (mCombines.TryGetValue(one.EquipID, out value))
			{
				return value >= one.BreakNeed;
			}
			return false;
		}

		public void DebugLog()
		{
		}
	}

	[Serializable]
	public class LocalSaveExtra : LocalSaveBase
	{
		public int stage = 1;

		public Dictionary<int, int> list = new Dictionary<int, int>();

		public int overopencount;

		public int battleinmode = 1001;

		public int guideequipalllayer;

		public int guidebattleProcess;

		public long EquipDropRate;

		public uint mTransID;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_Extra", this);
		}

		public void AddLayerCount(int stage, int layer)
		{
			InitData(stage, layer);
			if (list[layer] == 0)
			{
				Dictionary<int, int> dictionary;
				int key;
				(dictionary = list)[key = layer] = dictionary[key] + 1;
				Refresh();
			}
		}

		public int GetLayerCount(int stage, int layer)
		{
			InitData(stage, layer);
			return list[layer];
		}

		public void AddEquipAllLayer()
		{
			guideequipalllayer++;
			Refresh();
		}

		public bool Get_Equip_Drop()
		{
			return Instance.Card_GetLevel() >= GameConfig.GetEquipUnlockTalentLevel() || Instance.GetHaveEquips(havewear: true).Count > 1 || Instance.Stage_GetStage() > 1 || guideequipalllayer >= GameConfig.GetEquipGuide_alllayer();
		}

		public bool Get_EquipExp_Drop()
		{
			if (Get_Equip_Drop() && Instance.Card_GetLevel() >= GameConfig.GetEquipExpUnlockTalentLevel())
			{
				return true;
			}
			return false;
		}

		public void SetGuideBattleProcess(int value)
		{
			guidebattleProcess = value;
			Refresh();
		}

		public void InitTransID(uint id)
		{
			if (mTransID < id)
			{
				mTransID = id;
				Refresh();
			}
		}

		public uint GetTransID()
		{
			mTransID++;
			Refresh();
			return mTransID;
		}

		public void SetEquipDropRate(long value)
		{
			EquipDropRate = value;
			Refresh();
		}

		private void InitData(int stage, int layer)
		{
			if (this.stage != stage)
			{
				list.Clear();
			}
			if (!list.ContainsKey(layer))
			{
				list.Add(layer, 0);
			}
		}

		public void Init()
		{
			if (Instance.mStage.MaxLevel > 0)
			{
				SetGuideBattleProcess(2);
			}
		}
	}

	[Serializable]
	public class FakeStageDrop : LocalSaveBase
	{
		public int stage = -1;

		public int count;

		public int fakerid = -1;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_FakerStageDrop", this);
		}

		public void UpdateStage(int stage, int fakerid)
		{
			if (this.stage != stage)
			{
				this.stage = stage;
				count = 0;
				this.fakerid = fakerid;
				Refresh();
			}
		}

		public List<Drop_DropModel.DropData> GetDropList()
		{
			int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
			int equipDropID = LocalModelManager.Instance.Stage_Level_stagechapter.GetEquipDropID(level_CurrentStage);
			if (equipDropID == 0)
			{
				return null;
			}
			UpdateStage(level_CurrentStage, equipDropID);
			Drop_FakeDrop beanById = LocalModelManager.Instance.Drop_FakeDrop.GetBeanById(fakerid);
			count++;
			List<Drop_DropModel.DropData> list = null;
			if (count >= beanById.RandNum)
			{
				count = 0;
				fakerid = beanById.JumpDrop;
			}
			Refresh();
			return LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.DropID);
		}
	}

	[Serializable]
	public class FakeCardCost : LocalSaveBase
	{
		public int count;

		public int fakerid = 1;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_FakerCardDrop", this);
		}

		public void InitCount(int count)
		{
			this.count = count;
			fakerid = count + 1;
			Refresh();
			Facade.Instance.SendNotification("MainUI_CardRedCountUpdate");
		}

		public void AddCount()
		{
			count++;
			Skill_slotoutcost beanById = LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(fakerid);
			if (count >= beanById.LowerLimit)
			{
				fakerid++;
				beanById = LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(fakerid);
			}
			Refresh();
		}

		public int GetCost()
		{
			Skill_slotoutcost beanById = LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(fakerid);
			return beanById.CoinCost;
		}

		public int GetNeedLevel()
		{
			Skill_slotoutcost beanById = LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(fakerid);
			return beanById.NeedLevel;
		}
	}

	public class GuideData : LocalSaveBase
	{
		public const ushort GAME_SYSTEM_DIAMONDBOX = 1;

		public int mDiamondBox;

		[JsonIgnore]
		private GuideNoMaskCtrl mCtrl;

		public long mGameSystemMask;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_Achieve1", this);
		}

		public void SetIndex(int index)
		{
			if (mDiamondBox == index - 1)
			{
				mDiamondBox = index;
				remove();
				Refresh();
			}
		}

		private void remove()
		{
			if (mCtrl != null)
			{
				UnityEngine.Object.Destroy(mCtrl.gameObject);
			}
		}

		public bool CheckDiamondBox(RectTransform t, int index)
		{
			if (mDiamondBox != index)
			{
				return false;
			}
			if (index == 0 && Instance.GetDiamondBoxFreeCount(TimeBoxType.BoxChoose_DiamondNormal) > 0)
			{
				create_mask(t);
				return true;
			}
			if (index == 1 && Instance.GetDiamondBoxFreeCount(TimeBoxType.BoxChoose_DiamondNormal) > 0)
			{
				create_mask(t);
				return true;
			}
			return false;
		}

		private void create_mask(RectTransform t)
		{
			remove();
			mCtrl = CInstance<UIResourceCreator>.Instance.GetGuideNoMask(t);
		}

		public void check_diamondbox_first_open()
		{
			if (!is_system_open(1))
			{
				UnityEngine.Debug.Log("发送给服务器 钻石宝箱系统开启");
				system_open(1);
				Instance.SetTimeBoxTime(TimeBoxType.BoxChoose_DiamondNormal, Utils.GetTimeStamp());
				Instance.SetTimeBoxTime(TimeBoxType.BoxChoose_DiamondLarge, Utils.GetTimeStamp());
				send_system_open(1, delegate
				{
				});
			}
		}

		private void send_system_open(ushort index, Action<NetResponse> callback)
		{
			CReqSyncGameSystemMask cReqSyncGameSystemMask = new CReqSyncGameSystemMask();
			cReqSyncGameSystemMask.m_syncMsg = new CCommonRespMsg();
			cReqSyncGameSystemMask.m_syncMsg.m_nStatusCode = index;
			cReqSyncGameSystemMask.m_syncMsg.m_strInfo = string.Empty;
			NetManager.SendInternal(cReqSyncGameSystemMask, SendType.eCache, delegate(NetResponse response)
			{
				if (callback != null)
				{
					callback(response);
				}
			});
		}

		public void SetGameSystemMask(long mask)
		{
			mGameSystemMask = mask;
			Refresh();
		}

		public void system_open(int index)
		{
			mGameSystemMask += 1 << index;
			Refresh();
		}

		public bool is_system_open(int index)
		{
			string text = Convert.ToString(mGameSystemMask, 2);
			Debugger.Log("is_system_open " + text);
			int length = text.Length;
			if (!string.IsNullOrEmpty(text) && length > index && index >= 0)
			{
				int result = 0;
				string s = text.Substring(length - 1 - index, 1);
				int.TryParse(s, out result);
				return result != 0;
			}
			return false;
		}
	}

	[Serializable]
	public class HarvestData : LocalSaveBase
	{
		public long beforeexcutetime = -1L;

		public long startservertime;

		public int gold;

		public Dictionary<int, Drop_DropModel.DropData> mItems = new Dictionary<int, Drop_DropModel.DropData>();

		[JsonIgnore]
		private WeightRandom mEquipExpWeight = new WeightRandom();

		[JsonIgnore]
		private long mMaxTime;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_Harvest", this);
		}

		public void Init()
		{
			mEquipExpWeight.Add(30101, 10);
			mEquipExpWeight.Add(30102, 10);
			mEquipExpWeight.Add(30103, 17);
			mEquipExpWeight.Add(30104, 17);
			mMaxTime = 86400 * GameConfig.GetHarvestMaxDay();
		}

		private bool is_available()
		{
			return startservertime > 0 && beforeexcutetime > 0 && NetManager.NetTime > 0;
		}

		public void AddGold(int gold)
		{
			this.gold += gold;
			Refresh();
		}

		public void AddItem(Drop_DropModel.DropData item)
		{
			if (mItems.TryGetValue(item.id, out Drop_DropModel.DropData value))
			{
				value.count += item.count;
			}
			else
			{
				mItems.Add(item.id, item);
			}
			Refresh();
		}

		public void init_last_time(long time)
		{
			startservertime = time;
			if (beforeexcutetime < 0)
			{
				beforeexcutetime = startservertime;
			}
			Refresh();
		}

		private int get_current_refresh_minutes()
		{
			if (!is_available())
			{
				return 0;
			}
			long num = Utils.GetTimeStamp();
			if (num - startservertime > mMaxTime)
			{
				num = startservertime + mMaxTime;
			}
			long num2 = num - beforeexcutetime;
			int num3 = (int)(num2 / 60);
			beforeexcutetime += num3 * 60;
			return num3;
		}

		public bool get_can_reward()
		{
			if (!is_available())
			{
				return false;
			}
			long harvest_time = get_harvest_time();
			int num = (int)(harvest_time / 60);
			return num >= 60;
		}

		public long get_harvest_time()
		{
			return Utils.GetTimeStamp() - startservertime;
		}

		public bool refresh_rewards()
		{
			if (NetManager.NetTime == 0)
			{
				return false;
			}
			int num = Instance.Card_GetHarvestLevel();
			if (num == 0)
			{
				return false;
			}
			int current_refresh_minutes = get_current_refresh_minutes();
			if (current_refresh_minutes <= 0)
			{
				return false;
			}
			Drop_harvest beanById = LocalModelManager.Instance.Drop_harvest.GetBeanById(num);
			if (beanById == null)
			{
				SdkManager.Bugly_Report("LocalSave_Harvest", Utils.FormatString("refresh_rewards Drop_harvest ID:{0} is not in excel.", num));
				return false;
			}
			float num2 = 1f;
			gold += MathDxx.CeilToInt((float)(beanById.GoldDrop * current_refresh_minutes) * num2);
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			for (int i = 0; i < current_refresh_minutes; i++)
			{
				if (GameLogic.Random(0, 10000) < beanById.EquipExp)
				{
					int random = mEquipExpWeight.GetRandom();
					bool value = false;
					if (!dictionary.TryGetValue(random, out value))
					{
						value = Instance.Equip_can_drop_equipexp(random);
						dictionary.Add(random, value);
					}
					if (value)
					{
						AddItem(new Drop_DropModel.DropData
						{
							type = PropType.eEquip,
							id = random,
							count = 1
						});
					}
				}
			}
			return true;
		}

		public void Get_to_pack()
		{
			if (gold > 0)
			{
				Instance.Modify_Gold(gold, updateui: false);
				CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, gold);
			}
			Dictionary<int, Drop_DropModel.DropData>.Enumerator enumerator = mItems.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.count > 0)
				{
					Instance.AddProp(enumerator.Current.Value);
				}
			}
			Clear();
			Facade.Instance.SendNotification("MainUI_HarvestUpdate");
		}

		public List<Drop_DropModel.DropData> GetList()
		{
			List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
			if (gold > 0)
			{
				list.Add(new Drop_DropModel.DropData
				{
					type = PropType.eCurrency,
					id = 1,
					count = gold
				});
			}
			Dictionary<int, Drop_DropModel.DropData>.Enumerator enumerator = mItems.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.count > 0)
				{
					list.Add(enumerator.Current.Value);
				}
			}
			return list;
		}

		public void Unlock()
		{
			Clear();
			beforeexcutetime = Utils.GetTimeStamp();
			startservertime = beforeexcutetime;
			Facade.Instance.SendNotification("MainUI_HarvestUpdate");
		}

		public void Clear()
		{
			gold = 0;
			startservertime = Utils.GetTimeStamp();
			mItems.Clear();
			Refresh();
		}
	}

	[Serializable]
	public class LocalMail : LocalSaveBase
	{
		public uint mLastMailID;

		public List<CMailInfo> list = new List<CMailInfo>();

		private float time;

		private const int interval = 600;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("mail.txt", this);
		}

		public void AddMail(CMailInfo mail)
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].m_nMailID == mail.m_nMailID)
				{
					return;
				}
			}
			list.Add(mail);
			SetMailID(mail.m_nMailID);
			mailListUpdate();
		}

		public void SetMailID(uint id)
		{
			if (mLastMailID < id)
			{
				mLastMailID = id;
			}
		}

		public void MailReaded(CMailInfo mail)
		{
			if (!mail.IsReaded)
			{
				mail.IsReaded = true;
				mailListUpdate();
				Refresh();
			}
		}

		public void MailGot(CMailInfo mail)
		{
			if (!mail.IsGot)
			{
				mail.IsGot = true;
				mailListUpdate();
				Refresh();
			}
		}

		private void mailListUpdate()
		{
			list.Sort(delegate(CMailInfo a, CMailInfo b)
			{
				if (a.IsShowRed && !b.IsShowRed)
				{
					return -1;
				}
				if (!a.IsShowRed && b.IsShowRed)
				{
					return 1;
				}
				return (a.m_i64PubTime > b.m_i64PubTime) ? (-1) : 1;
			});
			Facade.Instance.SendNotification("MailUI_MailUpdate");
			Facade.Instance.SendNotification("MainUI_MailUpdate");
		}

		public int GetRedCount()
		{
			int num = 0;
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				if (list[i].IsShowRed)
				{
					num++;
				}
			}
			return num;
		}

		public void Init()
		{
			time = Time.realtimeSinceStartup;
			DOTween.Sequence().AppendInterval(1f).AppendCallback(Update)
				.SetUpdate(isIndependentUpdate: true)
				.SetLoops(-1);
		}

		public bool CheckMainPop()
		{
			if (Facade.Instance.RetrieveMediator("MailInfoMediator") != null)
			{
				return false;
			}
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				CMailInfo cMailInfo = list[i];
				if (cMailInfo.m_nMailType == 3 && !cMailInfo.IsReaded)
				{
					Facade.Instance.RegisterProxy(new MailInfoProxy(new MailInfoProxy.Transfer
					{
						data = cMailInfo,
						poptype = MailInfoProxy.EMailPopType.eMain
					}));
					WindowUI.ShowWindow(WindowID.WindowID_MailInfo);
					return true;
				}
			}
			return false;
		}

		public void SendMail()
		{
			SendMailInternal(null);
		}

		private void SendMailInternal(Action callback)
		{
			CReqAnnonceMailList cReqAnnonceMailList = new CReqAnnonceMailList();
			cReqAnnonceMailList.m_nLastMailID = mLastMailID;
			NetManager.SendInternal(cReqAnnonceMailList, SendType.eLoop, delegate(NetResponse response)
			{
				if (response.data != null)
				{
					CRespMailList cRespMailList = response.data as CRespMailList;
					if (cRespMailList != null && cRespMailList.mailList != null)
					{
						int i = 0;
						for (int num = cRespMailList.mailList.Length; i < num; i++)
						{
							AddMail(cRespMailList.mailList[i]);
						}
						Facade.Instance.SendNotification("MainUI_MailUpdate");
						Refresh();
					}
				}
				else if ((response.error == null || response.error.m_nStatusCode != 2) && callback != null)
				{
					callback();
				}
			});
		}

		private void Update()
		{
			if (Time.realtimeSinceStartup - time > 600f)
			{
				time = Time.realtimeSinceStartup;
				SendMailInternal(delegate
				{
					time = Time.realtimeSinceStartup - 570f;
				});
			}
		}
	}

	public enum NetCacheType
	{
		eBattleBegin,
		eBattleEnd
	}

	[Serializable]
	public class NetCache
	{
		public List<NetCacheBase> list = new List<NetCacheBase>();

		public void SendAllCache()
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				SendOne(list[i]);
			}
		}

		public void SendOne(NetCacheBase data)
		{
			switch (data.type)
			{
			case NetCacheType.eBattleBegin:
			{
				NetCacheBattleBegin netCacheBattleBegin = data as NetCacheBattleBegin;
				if (netCacheBattleBegin == null)
				{
				}
				break;
			}
			case NetCacheType.eBattleEnd:
			{
				NetCacheBattleEnd netCacheBattleEnd = data as NetCacheBattleEnd;
				if (netCacheBattleEnd == null)
				{
				}
				break;
			}
			}
		}
	}

	[Serializable]
	public class NetCacheBase
	{
		public NetCacheType type;
	}

	[Serializable]
	public class NetCacheBattleBegin : NetCacheBase
	{
	}

	[Serializable]
	public class NetCacheBattleEnd : NetCacheBase
	{
	}

	[Serializable]
	public class PurchaseData : LocalSaveBase
	{
		public List<string> mList = new List<string>();

		protected override void OnRefresh()
		{
			FileUtils.WriteXml(string.Empty, this);
		}

		public void AddPurchase(string data)
		{
			data = data.ToLower();
			mList.Add(data);
			Refresh();
		}

		public void RemovePurchase(string actionid)
		{
			actionid = actionid.ToLower();
			int num = 0;
			int count = mList.Count;
			while (true)
			{
				if (num < count)
				{
					JObject jObject = (JObject)JsonConvert.DeserializeObject(mList[num]);
					if (actionid.Equals(jObject["transactionid"]))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			mList.RemoveAt(num);
			Refresh();
		}
	}

	[Serializable]
	public class ShopLocal : LocalSaveBase
	{
		[JsonIgnore]
		public bool bRefresh = true;

		[JsonIgnore]
		private static int[] mTimes = new int[3]
		{
			8,
			24,
			72
		};

		protected override void OnRefresh()
		{
			bRefresh = true;
		}

		public void Init()
		{
			bRefresh = true;
		}

		public int get_buy_golds(int index)
		{
			if (index < 0 || index >= mTimes.Length)
			{
				return 0;
			}
			int num = Instance.Card_GetHarvestGold();
			return num * 60 * mTimes[index];
		}

		public int get_gold_time(int index)
		{
			if (index < 0 || index >= mTimes.Length)
			{
				return 0;
			}
			return mTimes[index];
		}
	}

	[Serializable]
	public class Stage : LocalSaveBase
	{
		public int CurrentStage;

		public bool FirstIn;

		public int MaxLevel;

		public int BoxLayerID;

		public bool bRefresh;

		[JsonIgnore]
		public bool bNewBestLevel;

		protected override void OnRefresh()
		{
			bRefresh = true;
			FileUtils.WriteXml("File_Stage", this);
		}

		public void InitMaxLevel(int max)
		{
			bRefresh = true;
			MaxLevel = max;
			int num = 1;
			int maxChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
			int allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(num);
			while (MaxLevel >= allMaxLevel && num < maxChapter)
			{
				num++;
				allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(num);
			}
			CurrentStage = num;
			Instance.Stage_SetStage(CurrentStage);
			GameLogic.Hold.BattleData.InitState();
			Debugger.Log("stage " + num + " stagemax " + allMaxLevel + " currentmax " + MaxLevel + " current show " + GameLogic.Hold.BattleData.Level_CurrentStage);
			Refresh();
			Facade.Instance.SendNotification("MainUI_LayerUpdate");
		}

		public void GetStageLayer(int currentlayer, out int stage, out int layer)
		{
			int num = stage = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
			layer = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(num);
			int num2 = 1;
			while (true)
			{
				if (num2 <= num)
				{
					int allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(num2);
					if (currentlayer < allMaxLevel)
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			if (num2 == 1)
			{
				stage = num2;
				layer = currentlayer;
			}
			else
			{
				stage = num2;
				layer = currentlayer - LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(num2 - 1);
			}
		}

		public void GetLayerBoxStageLayer(int currentlayer, out int stage, out int layer)
		{
			int num = stage = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
			layer = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(num);
			int num2 = 1;
			while (true)
			{
				if (num2 <= num)
				{
					int allMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(num2);
					if (currentlayer <= allMaxLevel)
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			if (num2 == 1)
			{
				stage = num2;
				layer = currentlayer;
			}
			else
			{
				stage = num2;
				layer = currentlayer - LocalModelManager.Instance.Stage_Level_stagechapter.GetAllMaxLevel(num2 - 1);
			}
		}

		public void UpdateMaxLevel(int max)
		{
			if (MaxLevel < max)
			{
				MaxLevel = max;
				InitMaxLevel(MaxLevel);
				bNewBestLevel = true;
				Refresh();
			}
		}

		public int GetCurrentMaxLevel()
		{
			GetStageLayer(MaxLevel, out int _, out int layer);
			return layer;
		}

		public void GetStageBoxEnd()
		{
			BoxLayerID++;
			Refresh();
		}

		public override string ToString()
		{
			return Utils.FormatString("Stage:{0}, FirstIn:{1}, MaxLevel:{2}", CurrentStage, FirstIn, MaxLevel);
		}
	}

	[Serializable]
	public class StageDiscountBody
	{
		public StageDiscountInfo purchased_info;

		public StageDiscountCurrent current_purchase;

		public bool IsValid
		{
			get
			{
				if (current_purchase == null)
				{
					return false;
				}
				if (current_purchase.reward_info == null)
				{
					return false;
				}
				if (string.IsNullOrEmpty(current_purchase.product_id))
				{
					return false;
				}
				return current_purchase.reward_info.Length > 0;
			}
		}

		public bool Is_Ad_Free => Get_LastID() > 0;

		public List<Drop_DropModel.DropData> GetList()
		{
			List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
			if (!IsValid)
			{
				return list;
			}
			if (current_purchase == null || current_purchase.reward_info == null || current_purchase.reward_info.Length == 0)
			{
				return list;
			}
			int i = 0;
			for (int num = current_purchase.reward_info.Length; i < num; i++)
			{
				string text = current_purchase.reward_info[i];
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				string[] array = text.Split(',');
				if (array.Length == 3)
				{
					int result = 0;
					int.TryParse(array[0], out result);
					int result2 = 0;
					int.TryParse(array[1], out result2);
					int result3 = 0;
					int.TryParse(array[2], out result3);
					if (result != 0 && result2 != 0 && result3 != 0)
					{
						Drop_DropModel.DropData dropData = new Drop_DropModel.DropData();
						dropData.type = (PropType)result;
						dropData.id = result2;
						dropData.count = result3;
						list.Add(dropData);
					}
				}
			}
			return list;
		}

		public int Get_CurrentID()
		{
			int result = 0;
			if (current_purchase != null && !string.IsNullOrEmpty(current_purchase.product_id) && current_purchase.product_id.Length > 3)
			{
				string s = current_purchase.product_id.Substring(current_purchase.product_id.Length - 3, 3);
				int.TryParse(s, out result);
			}
			return result;
		}

		public int Get_LastID()
		{
			int result = 0;
			if (purchased_info != null && !string.IsNullOrEmpty(purchased_info.product_id) && purchased_info.product_id.Length > 3)
			{
				string s = purchased_info.product_id.Substring(purchased_info.product_id.Length - 3, 3);
				int.TryParse(s, out result);
			}
			return result;
		}

		public override string ToString()
		{
			string text = string.Empty;
			if (purchased_info != null)
			{
				text = purchased_info.product_id;
			}
			string text2 = string.Empty;
			string text3 = string.Empty;
			if (current_purchase != null)
			{
				text2 = current_purchase.product_id;
				if (current_purchase.reward_info != null)
				{
					for (int i = 0; i < current_purchase.reward_info.Length; i++)
					{
						text3 = ((i == current_purchase.reward_info.Length - 1) ? (text3 + current_purchase.reward_info[i]) : (text3 + current_purchase.reward_info[i] + ","));
					}
				}
			}
			return Utils.FormatString("StageDiscount: boughtid:{0} nextid:{1} next rewards:{2}", text, text2, text3);
		}
	}

	[Serializable]
	public class StageDiscountInfo
	{
		public string product_id;
	}

	[Serializable]
	public class StageDiscountCurrent
	{
		public string product_id;

		public string[] reward_info;
	}

	[Serializable]
	public class SaveData
	{
		public bool bInit;

		public UserInfo userInfo = new UserInfo();

		public CardData mCardData = new CardData();

		public ChallengeData mChallengeData = new ChallengeData();

		public TimeBoxData mTimeBoxData = new TimeBoxData();

		public Stage mStage = new Stage();

		public AchieveData mAchieveData = new AchieveData();

		public Shop_MysticShopModel.MysticShopData mMysticShopData = new Shop_MysticShopModel.MysticShopData();

		public LocalSaveExtra mExtra = new LocalSaveExtra();

		public FakeStageDrop mFakeStage = new FakeStageDrop();

		public FakeCardCost mFakeCardCost = new FakeCardCost();

		public ShopLocal mShopLocal = new ShopLocal();

		public ActiveData mActiveData = new ActiveData();

		public LocalMail mMail = new LocalMail();

		public DropCard mDropCard = new DropCard();

		public PurchaseData mPurchase = new PurchaseData();

		public HarvestData mHarvest = new HarvestData();

		public GuideData mGuideData = new GuideData();
	}

	public enum EThreadWriteType
	{
		eBattle,
		eEquip,
		eNet,
		eLocal
	}

	public enum TimeBoxType
	{
		BoxChoose_DiamondLarge = 0x3FF,
		BoxChoose_DiamondNormal = 1026
	}

	[Serializable]
	public class TimeBoxOne
	{
		public int maxcount = 1;

		public int count = 1;

		public long time;

		[JsonIgnore]
		public bool IsMax => count >= maxcount;

		public void UpdateCount(int value, bool over)
		{
			count += value;
			if (!over && value > 0)
			{
				count = MathDxx.Clamp(count, 0, maxcount);
			}
		}

		public void SetCount(int value)
		{
			count = value;
		}
	}

	[Serializable]
	public class TimeBoxData : LocalSaveBase
	{
		public Dictionary<TimeBoxType, TimeBoxOne> list = new Dictionary<TimeBoxType, TimeBoxOne>();

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_TimeBox", this);
		}

		public void Init()
		{
			long time = long.MaxValue;
			if (!list.ContainsKey(TimeBoxType.BoxChoose_DiamondLarge))
			{
				list.Add(TimeBoxType.BoxChoose_DiamondLarge, new TimeBoxOne
				{
					maxcount = 1,
					count = 0,
					time = time
				});
				Refresh();
			}
			if (!list.ContainsKey(TimeBoxType.BoxChoose_DiamondNormal))
			{
				list.Add(TimeBoxType.BoxChoose_DiamondNormal, new TimeBoxOne
				{
					maxcount = 1,
					count = 0,
					time = time
				});
				Refresh();
			}
		}

		public bool IsBoxOpenFree(TimeBoxType type)
		{
			if (list[type].maxcount == 0)
			{
				return false;
			}
			int id = (int)(type + 10);
			long value = GameConfig.GetValue<long>(id);
			long timeStamp = Utils.GetTimeStamp();
			if (timeStamp - value >= list[type].time)
			{
				SetTime(type, timeStamp);
				return true;
			}
			return false;
		}

		public long GetTime(TimeBoxType type)
		{
			return list[type].time;
		}

		public void SetTime(TimeBoxType type, long time)
		{
			list[type].time = time;
			Debugger.Log("SetTime " + type + " time -> " + time);
			Refresh();
		}

		public int GetCount(TimeBoxType type)
		{
			return list[type].count;
		}

		public bool IsMaxCount(TimeBoxType type)
		{
			return list[type].IsMax;
		}

		public void UpdateCount(TimeBoxType type, int value, bool over)
		{
			list[type].UpdateCount(value, over);
			UpdateRedNode(type);
			Refresh();
		}

		public void SetCount(TimeBoxType type, int value)
		{
			list[type].SetCount(value);
			UpdateRedNode(type);
			Refresh();
		}

		private void UpdateRedNode(TimeBoxType type)
		{
			if (type == TimeBoxType.BoxChoose_DiamondNormal || type == TimeBoxType.BoxChoose_DiamondLarge)
			{
				Facade.Instance.SendNotification("MainUI_ShopRedCountUpdate");
			}
		}
	}

	[Serializable]
	public class UserInfo : LocalSaveBase
	{
		public string UserID = string.Empty;

		[JsonIgnore]
		public string UserID_Temp = string.Empty;

		public string UserName = string.Empty;

		public string UserName_Temp = string.Empty;

		public ulong ServerUserID;

		public LoginType loginType;

		public long NetID;

		public long Gold;

		public long Diamond;

		public long Resource;

		public int Key;

		public int reborncount;

		public int StageDiscountID;

		public int Level = 1;

		public long Exp;

		public int Score;

		public bool isInit;

		public long Show_Gold = 100L;

		public long Show_Diamond = 100L;

		public long Show_Exp;

		public short KeyTrustCount;

		public int AdKeyCount;

		public int DiamondNormalExtraCount;

		public int DiamondLargeExtraCount;

		public bool guide_diamondbox;

		[JsonIgnore]
		public int BattleAdCount;

		[JsonIgnore]
		public bool bLogined;

		[JsonIgnore]
		public bool bLoginedSDK;

		protected override void OnRefresh()
		{
			FileUtils.WriteXml("File_Currency", this);
		}
	}

	private static LocalSave _instance = null;

	private UserInfo userInfo;

	private CardData mCardData;

	private ActiveData mActiveData;

	private ChallengeData mChallengeData;

	private AchieveData mAchieveData;

	public const string BattleInString = "BattleInString";

	public const string BattleInModeString = "BattleInModeString";

	private int BattleIn_Mode;

	private BattleInBase mBattleIn;

	private DropCard mBoxDropCard;

	private BoxDropEquip mBoxDropEquip;

	public static Action CardUpdateEvent;

	private const string ChallengeConst = "ChallengeConstLocal";

	public const int EquipCount = 6;

	public static Dictionary<int, Color> QualityColors = new Dictionary<int, Color>
	{
		{
			0,
			new Color(226f / 255f, 226f / 255f, 226f / 255f)
		},
		{
			1,
			new Color(226f / 255f, 226f / 255f, 226f / 255f)
		},
		{
			2,
			new Color(0.4117647f, 217f / 255f, 33f / 85f)
		},
		{
			3,
			new Color(58f / 255f, 191f / 255f, 227f / 255f)
		},
		{
			4,
			new Color(43f / 51f, 36f / 85f, 1f)
		},
		{
			5,
			new Color(43f / 51f, 36f / 85f, 1f)
		},
		{
			6,
			new Color(254f / 255f, 76f / 85f, 71f / (339f * (float)Math.PI))
		},
		{
			7,
			new Color(1f, 47f / 51f, 14f / 255f)
		},
		{
			8,
			new Color(1f, 47f / 51f, 14f / 255f)
		},
		{
			9,
			new Color(1f, 47f / 51f, 14f / 255f)
		}
	};

	public static Dictionary<int, int> EquipPositions = new Dictionary<int, int>
	{
		{
			0,
			1
		},
		{
			1,
			2
		},
		{
			2,
			5
		},
		{
			3,
			5
		},
		{
			4,
			6
		},
		{
			5,
			6
		}
	};

	public LocalSaveExtra _saveExtra;

	private FakeStageDrop _fakestagedrop;

	private FakeCardCost _fakecardcost;

	private GuideData _guidedata;

	private HarvestData _harvest;

	private PurchaseData _purchase;

	private ShopLocal _shop;

	public Action OnMaxLevelUpdate;

	private Stage _stage;

	private StageDiscountBody mStageDiscount;

	public SaveData mSaveData;

	private Dictionary<EThreadWriteType, bool> mThreadList = new Dictionary<EThreadWriteType, bool>
	{
		{
			EThreadWriteType.eBattle,
			false
		},
		{
			EThreadWriteType.eEquip,
			false
		},
		{
			EThreadWriteType.eNet,
			false
		},
		{
			EThreadWriteType.eLocal,
			false
		}
	};

	private object mThreadDoing = new object();

	private TimeBoxData mTimeBox;

	private const string CurrencyConst = "Currency";

	public static Action<long, long> GoldUpdateEvent;

	private long mCurrencyKeyTime;

	public static LocalSave Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new LocalSave();
				_instance.Init();
			}
			return _instance;
		}
	}

	public bool BattleIn_In
	{
		get;
		private set;
	}

	public BattleInBase BattleIn => mBattleIn;

	public EquipData mEquip
	{
		get;
		private set;
	}

	public LocalSaveExtra SaveExtra
	{
		get
		{
			if (_saveExtra == null)
			{
				_saveExtra = mSaveData.mExtra;
			}
			return _saveExtra;
		}
	}

	public FakeStageDrop mFakeStageDrop
	{
		get
		{
			if (_fakestagedrop == null)
			{
				_fakestagedrop = mSaveData.mFakeStage;
			}
			return _fakestagedrop;
		}
	}

	public FakeCardCost mFakeCardCost
	{
		get
		{
			if (_fakecardcost == null)
			{
				_fakecardcost = mSaveData.mFakeCardCost;
			}
			return _fakecardcost;
		}
	}

	public GuideData mGuideData
	{
		get
		{
			if (_guidedata == null)
			{
				_guidedata = mSaveData.mGuideData;
			}
			return _guidedata;
		}
	}

	public HarvestData mHarvest
	{
		get
		{
			if (_harvest == null)
			{
				_harvest = mSaveData.mHarvest;
				_harvest.Init();
			}
			return _harvest;
		}
		set
		{
			_harvest = value;
		}
	}

	public LocalMail Mail
	{
		get;
		private set;
	}

	public PurchaseData mPurchase
	{
		get
		{
			if (_purchase == null)
			{
				_purchase = mSaveData.mPurchase;
			}
			return _purchase;
		}
	}

	public ShopLocal mShop
	{
		get
		{
			if (_shop == null)
			{
				_shop = mSaveData.mShopLocal;
				_shop.Init();
			}
			return _shop;
		}
	}

	public Stage mStage
	{
		get
		{
			if (_stage == null)
			{
				_stage = mSaveData.mStage;
			}
			return _stage;
		}
		set
		{
			_stage = value;
		}
	}

	public void InitData()
	{
		InitBoxDrop();
		InitCard();
		InitActive();
		InitUserInfo();
		InitChallenge();
		InitKeyTime();
		InitTimeBoxTime();
		InitStage();
		InitEquips();
		InitAchieve();
		InitMail();
		InitHarvest();
		SaveExtra.Init();
	}

	private void Init()
	{
		CreateThread();
	}

	public int GetExpByLevel(int level)
	{
		return LocalModelManager.Instance.Character_Level.GetExp(level);
	}

	public void ExcuteModeChest(int dropid)
	{
		List<Drop_DropModel.DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(dropid);
		Facade.Instance.RegisterProxy(new BoxOpenProxy(dropList));
	}

	public void AddProp(CEquipmentItem item)
	{
		EquipOne equipOne = new EquipOne();
		equipOne.UniqueID = item.m_nUniqueID;
		equipOne.EquipID = (int)item.m_nEquipID;
		equipOne.Level = (int)item.m_nLevel;
		equipOne.Count = (int)item.m_nFragment;
		mEquip.AddEquipInternal(equipOne);
	}

	public void AddProp(Drop_DropModel.DropData item)
	{
		EquipOne equipOne = new EquipOne();
		equipOne.UniqueID = Utils.GenerateUUID();
		equipOne.EquipID = item.id;
		equipOne.Level = 1;
		equipOne.Count = item.count;
		mEquip.AddEquipInternal(equipOne);
	}

	public void AddProps(List<Drop_DropModel.DropData> list)
	{
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			Drop_DropModel.DropData dropData = list[i];
			switch (dropData.type)
			{
			case PropType.eCurrency:
				if (dropData.id == 1)
				{
					Modify_Gold(dropData.count);
				}
				else if (dropData.id == 2)
				{
					Modify_Diamond(dropData.count);
				}
				else if (dropData.id == 3)
				{
					Modify_Key(dropData.count);
				}
				else if (dropData.id == 4)
				{
					Modify_RebornCount(dropData.count);
				}
				break;
			case PropType.eEquip:
			{
				EquipOne equipOne = new EquipOne();
				equipOne.UniqueID = Utils.GenerateUUID();
				equipOne.EquipID = dropData.id;
				equipOne.Level = 1;
				equipOne.Count = dropData.count;
				if (equipOne.Overlying)
				{
					mEquip.AddEquipInternal(equipOne);
				}
				break;
			}
			}
		}
	}

	private void InitAchieve()
	{
		mAchieveData = mSaveData.mAchieveData;
		mAchieveData.Init();
	}

	public AchieveDataOne Achieve_Get(int id)
	{
		return mAchieveData.Get(id);
	}

	public void Achieve_AddProgress(int id, int count)
	{
		mAchieveData.AddProgress(id, count);
	}

	public bool Achieve_IsFinish(int id)
	{
		return mAchieveData.IsFinish(id);
	}

	public bool Achieve_Isgot(int id)
	{
		return mAchieveData.Isgot(id);
	}

	public void Achieve_ExcuteCurrentStage()
	{
		if (GameLogic.Hold.BattleData.Challenge_ismainchallenge())
		{
			int activeID = GameLogic.Hold.BattleData.ActiveID;
			Achieve_Get(activeID)?.mCondition.Excute();
		}
	}

	public int Achieve_get_finish_count(int stage)
	{
		int num = 0;
		List<int> stageList = LocalModelManager.Instance.Achieve_Achieve.GetStageList(stage, haveglobal: true);
		int i = 0;
		for (int count = stageList.Count; i < count; i++)
		{
			AchieveDataOne achieveDataOne = Achieve_Get(stageList[i]);
			if (achieveDataOne != null && achieveDataOne.isfinish)
			{
				num++;
			}
		}
		return num;
	}

	public int Achieve_get_finish_notgot_count(int stage)
	{
		int num = 0;
		List<int> stageList = LocalModelManager.Instance.Achieve_Achieve.GetStageList(stage, haveglobal: true);
		int i = 0;
		for (int count = stageList.Count; i < count; i++)
		{
			AchieveDataOne achieveDataOne = Achieve_Get(stageList[i]);
			if (achieveDataOne != null && achieveDataOne.isfinish && !achieveDataOne.isgot)
			{
				num++;
			}
		}
		return num;
	}

	public int Achieve_get_finish_notgot_count()
	{
		int maxChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
		int num = 0;
		for (int i = 1; i <= maxChapter; i++)
		{
			num += Achieve_get_finish_notgot_count(i);
		}
		return num;
	}

	private void InitActive()
	{
		mActiveData = mSaveData.mActiveData;
		mActiveData.Init();
	}

	public List<ActiveOne> GetActives()
	{
		return mActiveData.list;
	}

	public int GetActiveCount(int index)
	{
		if (index < 0 || index >= mActiveData.list.Count)
		{
			SdkManager.Bugly_Report("LocaSave_Active", Utils.FormatString("GetActiveCount[{0}] is out of range, mActiveData.list.Count = {1}", index, mActiveData.list.Count));
		}
		return mActiveData.list[index].Count;
	}

	public void UseActiveCount(ActiveOne one)
	{
		one.Count--;
		mAchieveData.Refresh();
	}

	public void BattleIn_Check()
	{
		if (mBattleIn != null && BattleIn_GetIn())
		{
			GameMode battleIn_Mode = (GameMode)BattleIn_Mode;
			GameLogic.Hold.BattleData.SetMode(battleIn_Mode, BattleSource.eWorld);
			WindowUI.ShowWindow(WindowID.WindowID_Battle);
		}
	}

	public void BattleIn_Restore()
	{
        Debug.Log("@LOG BattleIn_Restore");
        if (mBattleIn == null || !BattleIn_GetIn())
		{
			return;
		}
		GameMode battleIn_Mode = (GameMode)BattleIn_Mode;
		if (battleIn_Mode == GameMode.eLevel && mBattleIn.potions != null)
		{
			int i = 0;
			for (int count = mBattleIn.potions.Count; i < count; i++)
			{
				Shop_item beanById = LocalModelManager.Instance.Shop_item.GetBeanById(mBattleIn.potions[i]);
				FirstItemOnectrl.GetOnePotion(beanById);
			}
		}
		int j = 0;
		for (int count2 = mBattleIn.skillids.Count; j < count2; j++)
		{
			GameLogic.Self.BattleInInitSkill(mBattleIn.skillids[j]);
		}
		int k = 0;
		for (int count3 = mBattleIn.learnskills.Count; k < count3; k++)
		{
			GameLogic.Self.LearnSkill(mBattleIn.learnskills[k]);
		}
		int l = 0;
		for (int count4 = mBattleIn.goodids.Count; l < count4; l++)
		{
			GameLogic.Self.BattleInGetGoods(mBattleIn.goodids[l]);
		}
		int m = 0;
		for (int count5 = mBattleIn.equips.Count; m < count5; m++)
		{
			GameLogic.Hold.BattleData.AddEquipInternal(mBattleIn.equips[m]);
		}
		GameLogic.Self.m_EntityData.SetCurrentExpLevel(mBattleIn.exp, mBattleIn.level);
		long num = BattleIn_GetHP();
		if (num > 0)
		{
			long maxHP = GameLogic.Self.m_EntityData.MaxHP;
			long num2 = num - maxHP;
			if (num2 < 0)
			{
				GameLogic.Self.ChangeHPMust(null, num2);
			}
		}
	}

	public void BattleIn_CheckInit()
	{
		BattleIn_InitInternal();
		if (mBattleIn != null)
		{
			BattleIn_In = mBattleIn.GetHaveBattle();
		}
	}

	public void BattleIn_Init()
	{
		BattleIn_InitInternal();
		BattleIn_InGame();
	}

	private void BattleIn_InitInternal()
	{
		if (mBattleIn == null)
		{
			BattleIn_Mode = SaveExtra.battleinmode;
			GameMode battleIn_Mode = (GameMode)BattleIn_Mode;
			if (battleIn_Mode == GameMode.eLevel)
			{
				BattleInBase battleInBase = BattleInBase.Get();
				battleInBase.LevelInit();
				mBattleIn = battleInBase;
			}
		}
	}

	public void BattleIn_DeInit()
	{
		if (mBattleIn != null)
		{
			mBattleIn.DeInit();
		}
		BattleIn_In = false;
	}

	public void BattleIn_SetHaveBattle(bool value)
	{
		if (mBattleIn != null)
		{
			mBattleIn.SetHaveBattle(value);
		}
	}

	public void BattleIn_InGame()
	{
		if (mBattleIn != null)
		{
			if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel)
			{
				mBattleIn.CheckDifferentID();
				mBattleIn.SetHaveBattle(value: true);
				mBattleIn.serveruserid = Instance.GetServerUserID();
				int battleinmode = BattleIn_Mode = (int)GameLogic.Hold.BattleData.GetMode();
				SaveExtra.battleinmode = battleinmode;
				SaveExtra.Refresh();
				mBattleIn.Refresh();
			}
			else
			{
				mBattleIn.SetHaveBattle(value: false);
				mBattleIn.Refresh();
			}
		}
	}

	public void BattleIn_UpdateGoldTurn()
	{
		if (mBattleIn != null)
		{
			mBattleIn.bGoldTurn = true;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateExp(float exp)
	{
		if (mBattleIn != null)
		{
			mBattleIn.exp = exp;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateLevel(int level)
	{
		if (mBattleIn != null)
		{
			mBattleIn.level = level;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateGold(float gold)
	{
		if (mBattleIn != null)
		{
			mBattleIn.gold = gold;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_AddRebornSkill()
	{
		if (mBattleIn != null)
		{
			mBattleIn.AddRebornSkill();
		}
	}

	public int BattleIn_GetRebornSkill()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.reborn_skill_count;
		}
		return 0;
	}

	public void BattleIn_AddRebornUI()
	{
		if (mBattleIn != null)
		{
			mBattleIn.AddRebornUI();
		}
	}

	public int BattleIn_GetRebornUI()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.reborn_ui_count;
		}
		return 0;
	}

	public void BattleIn_UpdateSkill(int skillid)
	{
		if (mBattleIn != null)
		{
			mBattleIn.skillids.Add(skillid);
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateGood(int goodid)
	{
		if (mBattleIn != null)
		{
			mBattleIn.goodids.Add(goodid);
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateHP(long hp)
	{
		if (mBattleIn != null)
		{
			mBattleIn.hp = hp;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateRoomID(int roomid)
	{
		if (mBattleIn != null)
		{
			mBattleIn.RoomID = roomid;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateResourcesID(int id)
	{
		if (mBattleIn != null)
		{
			mBattleIn.ResourcesID = id;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateTmxID(string tmxid)
	{
		if (mBattleIn != null)
		{
			mBattleIn.TmxID = tmxid;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateStage()
	{
		if (mBattleIn != null)
		{
			mBattleIn.stage = GameLogic.Hold.BattleData.Level_CurrentStage;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateLevelUpSkills(int type, List<int> skills)
	{
		if (mBattleIn != null)
		{
			mBattleIn.leveluptype = type;
			mBattleIn.levelupskills = skills;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateLearnSkill(int skillid)
	{
		if (mBattleIn != null)
		{
			mBattleIn.learnskills.Add(skillid);
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdateFirstShop(List<bool> list)
	{
		if (mBattleIn != null)
		{
			mBattleIn.firstshopbuy = list;
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_UpdatePotions(int id)
	{
		if (mBattleIn != null)
		{
			mBattleIn.AddPotion(id);
			mBattleIn.Refresh();
		}
	}

	public void BattleIn_AddEquip(EquipOne one)
	{
		if (mBattleIn != null)
		{
			mBattleIn.AddEquip(one);
		}
	}

	public void BattleIn_UpdateIn()
	{
		BattleIn_In = false;
	}

	public bool BattleIn_GetGoldTurn()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.bGoldTurn;
		}
		return true;
	}

	public float BattleIn_GetExp()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.exp;
		}
		return 0f;
	}

	public int BattleIn_GetLevel()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.level;
		}
		return 1;
	}

	public float BattleIn_GetGold()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.gold;
		}
		return 0f;
	}

	public long BattleIn_GetHP()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.hp;
		}
		return 0L;
	}

	public int BattleIn_GetRoomID()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.RoomID;
		}
		return 0;
	}

	public int BattleIn_GetResourcesID()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.ResourcesID;
		}
		return 1;
	}

	public string BattleIn_GetTmxID()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.TmxID;
		}
		return string.Empty;
	}

	public List<int> BattleIn_GetLevelUpSkills()
	{
		if (mBattleIn != null && mBattleIn.levelupskills != null && mBattleIn.levelupskills.Count > 0)
		{
			return mBattleIn.levelupskills;
		}
		return null;
	}

	public int BattleIn_GetLevelUpType()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.leveluptype;
		}
		return 0;
	}

	public List<bool> BattleIn_GetFirstShop()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.firstshopbuy;
		}
		List<bool> list = new List<bool>();
		list.Add(item: false);
		list.Add(item: false);
		return list;
	}

	public List<int> BattleIn_GetPotions()
	{
		if (mBattleIn != null)
		{
			return mBattleIn.potions;
		}
		return null;
	}

	public bool BattleIn_GetIn()
	{
		if (mBattleIn != null && string.IsNullOrEmpty(mBattleIn.TmxID))
		{
			BattleIn_DeInit();
			BattleIn_In = false;
			return false;
		}
		return BattleIn_In;
	}

	private void InitBoxDrop()
	{
		if (mBoxDropCard == null)
		{
			mBoxDropCard = mSaveData.mDropCard;
		}
		if (mBoxDropEquip == null)
		{
			mBoxDropEquip = new BoxDropEquip();
		}
	}

	public void DropCard_Init(int allcount)
	{
		mBoxDropCard.InitCount(allcount);
	}

	public Drop_DropModel.DropData GetDropCardRandom()
	{
		return mBoxDropCard.GetRandom();
	}

	public void GetCardSucceed()
	{
		mBoxDropCard.GetSucceed();
	}

	public Drop_DropModel.DropData GetDropEquipRandom()
	{
		return mBoxDropEquip.GetRandom();
	}

	private void InitCard()
	{
		mCardData = mSaveData.mCardData;
		mCardData.Init();
	}

	public void Card_Set(List<CEquipmentItem> cards)
	{
		mCardData.Clear();
		int i = 0;
		for (int count = cards.Count; i < count; i++)
		{
			CEquipmentItem cEquipmentItem = cards[i];
			mCardData.SetOne((int)cEquipmentItem.m_nEquipID, (int)cEquipmentItem.m_nLevel);
		}
	}

	public CardOne AddCard(int cardid, int count)
	{
		return mCardData.AddOne(cardid, count);
	}

	public Dictionary<int, CardOne> GetCards()
	{
		return mCardData.GetCards();
	}

	public List<CardOne> GetCardsList()
	{
		List<CardOne> list = new List<CardOne>();
		Dictionary<int, CardOne> cards = mCardData.GetCards();
		Dictionary<int, CardOne>.Enumerator enumerator = cards.GetEnumerator();
		while (enumerator.MoveNext())
		{
			list.Add(enumerator.Current.Value);
		}
		return list;
	}

	public bool GetNoCard()
	{
		Dictionary<int, CardOne>.Enumerator enumerator = mCardData.mList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.Unlock)
			{
				return false;
			}
		}
		return true;
	}

	public bool GetCardMaxLevel(int id)
	{
		if (mCardData.mList.TryGetValue(id, out CardOne value))
		{
			return value.IsMaxLevel;
		}
		return true;
	}

	public int GetCardsCount()
	{
		return mCardData.GetCards().Count;
	}

	public List<CardOne> GetWearCards()
	{
		List<CardOne> list = new List<CardOne>();
		Dictionary<int, CardOne>.Enumerator enumerator = GetCards().GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.level > 0)
			{
				list.Add(enumerator.Current.Value);
			}
		}
		return list;
	}

	public CardOne GetCardByID(int id)
	{
		return mCardData.GetCardByID(id);
	}

	public bool Card_Have(int id)
	{
		return mCardData.HaveCard(id);
	}

	public int Card_GetRandomGold()
	{
		return mFakeCardCost.GetCost();
	}

	public int Card_GetNeedLevel()
	{
		return mFakeCardCost.GetNeedLevel();
	}

	public int Card_GetLevel()
	{
		return Card_GetRandomCount();
	}

	public int Card_GetRandomCount()
	{
		return mFakeCardCost.count;
	}

	public bool Card_GetAllMax()
	{
		return mCardData.GetAllMax();
	}

	public CardOne Card_GetRandom()
	{
		Drop_DropModel.DropData dropData = Card_GetRandomOnly();
		mFakeCardCost.AddCount();
		return AddCard(dropData.id, dropData.count);
	}

	public Drop_DropModel.DropData Card_GetRandomOnly()
	{
		return Instance.GetDropCardRandom();
	}

	public CardOne Card_ReceiveCard(Drop_DropModel.DropData drop)
	{
		mFakeCardCost.AddCount();
		return AddCard(drop.id, drop.count);
	}

	public int Card_GetHarvestID()
	{
		return 108;
	}

	public int Card_GetHarvestLevel()
	{
		return GetCardByID(Card_GetHarvestID())?.level ?? 0;
	}

	public bool Card_GetHarvestAvailable()
	{
		return Card_GetHarvestLevel() > 0;
	}

	public int Card_GetHarvestGold()
	{
		int key = Card_GetHarvestLevel();
		return LocalModelManager.Instance.Drop_harvest.GetBeanById(key)?.GoldDrop ?? 0;
	}

	private void InitChallenge()
	{
		mChallengeData = mSaveData.mChallengeData;
		if (!mChallengeData.isinit)
		{
			mChallengeData.isinit = true;
			mChallengeData.Refresh();
		}
	}

	public int Challenge_GetID()
	{
		return mChallengeData.ChallengeID;
	}

	public int Challenge_GetPassCount()
	{
		return Challenge_GetID() - 2101;
	}

	public bool Challenge_IsFirstIn()
	{
		return mChallengeData.bFirstIn;
	}

	public void Challenge_SetFirstIn()
	{
		mChallengeData.bFirstIn = false;
		mChallengeData.Refresh();
	}

	public void ChallengeSucceed()
	{
		mChallengeData.ChallengeID++;
		mChallengeData.bFirstIn = true;
		mChallengeData.Refresh();
	}

	private void InitEquips()
	{
		mEquip = FileUtils.GetXml<EquipData>("localequip.txt");
		mEquip.Init();
		mEquip.init_equipone_uniqueid();
	}

	public int Equip_GetCanWearCount()
	{
		return mEquip.GetCanWearCount();
	}

	public int Equip_GetCanUpCount()
	{
		return mEquip.GetCanUpCount();
	}

	public void Equip_Set(List<CEquipmentItem> equips)
	{
		mEquip.Init(equips);
	}

	public void Equip_Add(CEquipmentItem[] equips)
	{
		if (equips != null)
		{
			int i = 0;
			for (int num = equips.Length; i < num; i++)
			{
				CEquipmentItem cEquipmentItem = equips[i];
				EquipOne equipOne = new EquipOne();
				equipOne.UniqueID = cEquipmentItem.m_nUniqueID;
				equipOne.RowID = cEquipmentItem.m_nRowID;
				equipOne.EquipID = (int)cEquipmentItem.m_nEquipID;
				equipOne.Count = (int)cEquipmentItem.m_nFragment;
				equipOne.Level = (int)cEquipmentItem.m_nLevel;
				mEquip.AddEquipInternal(equipOne);
			}
		}
	}

	public EquipOne GetNewEquipByID(int equipid, int count)
	{
		Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById(equipid);
		if (beanById == null)
		{
			SdkManager.Bugly_Report("LocalSave_Equip.GetNewEquipByID", Utils.FormatString("Equip_equip dont have [{0}]", equipid));
		}
		EquipOne equipOne = new EquipOne();
		equipOne.EquipID = beanById.Id;
		equipOne.Level = 1;
		equipOne.Count = count;
		equipOne.WearIndex = -1;
		return equipOne;
	}

	public int Equip_GetNewCount()
	{
		return mEquip.GetNewCount();
	}

	public void SetEquips(CRespItemPacket data)
	{
		if (data != null && data.m_arrayEquipItems != null)
		{
			mEquip.SetEquips(data.m_arrayEquipItems);
		}
	}

	public EquipData GetEquips()
	{
		return mEquip;
	}

	public EquipOne GetEquipByUniqueID(string uniqueid)
	{
		return mEquip.GetEquipByUniqueID(uniqueid);
	}

	public EquipOne GetPropByID(int equipid)
	{
		return mEquip.GetPropByID(equipid);
	}

	public EquipOne GetPropShowByID(int equipid)
	{
		EquipOne equipOne = GetPropByID(equipid);
		if (equipOne == null)
		{
			equipOne = new EquipOne();
			equipOne.EquipID = equipid;
			equipOne.Level = 1;
			equipOne.Count = 0;
		}
		return equipOne;
	}

	public void EquipWear(EquipOne equip, int wearindex)
	{
		mEquip.EquipWear(equip, wearindex);
	}

	public void EquipUnwear(string uniqueid)
	{
		mEquip.EquipUnwear(uniqueid);
	}

	public void EquipLevelUp(EquipOne equip)
	{
		mEquip.EquipLevelUp(equip);
	}

	public void UpdateEquip(int position, int uniqueid, EquipOne equip)
	{
		mEquip.UpdateEquip(equip);
	}

	public List<EquipOne> GetHaveEquips(bool havewear)
	{
		return mEquip.GetHaveEquips(havewear);
	}

	public List<EquipOne> GetProps(EquipType type, bool havewear = true)
	{
		return mEquip.GetProps(type, havewear);
	}

	public bool GetEquipGuide_mustdrop()
	{
		if (GameLogic.Hold.Guide.mEquip.process > 0)
		{
			return false;
		}
		if (GetHaveEquips(havewear: true).Count > 1)
		{
			return false;
		}
		if (GameLogic.Hold.BattleData.GetEquips().Count > 0)
		{
			return false;
		}
		return Instance.SaveExtra.Get_Equip_Drop();
	}

	private int Equip_GetPositionCount(int position)
	{
		int num = 0;
		Dictionary<int, int>.Enumerator enumerator = EquipPositions.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value == position)
			{
				num++;
			}
		}
		return num;
	}

	public void Equip_Remove(string uniqueid)
	{
		mEquip.RemoveEquip(uniqueid);
	}

	public bool Equip_GetCanWearIndex(EquipOne one, out int index)
	{
		index = -1;
		if (Equip_GetPositionCount(one.Position) == 1)
		{
			int i = 0;
			for (int count = mEquip.wears.Count; i < count && EquipPositions.ContainsKey(i); i++)
			{
				if (EquipPositions[i] == one.Position)
				{
					index = i;
					return true;
				}
			}
		}
		int j = 0;
		for (int count2 = mEquip.wears.Count; j < count2 && EquipPositions.ContainsKey(j); j++)
		{
			if (EquipPositions[j] == one.Position && string.IsNullOrEmpty(mEquip.wears[j]))
			{
				index = j;
				return true;
			}
		}
		return false;
	}

	public bool Equip_GetCanWear(EquipOne one, int index)
	{
		int i = 0;
		for (int count = mEquip.wears.Count; i < count && EquipPositions.ContainsKey(i); i++)
		{
			if (EquipPositions[i] == one.Position && i != index)
			{
				EquipOne equipByUniqueID = GetEquipByUniqueID(mEquip.wears[i]);
				if (equipByUniqueID != null && equipByUniqueID.EquipID / 100 == one.EquipID / 100)
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool Equip_GetIsEmpty(EquipOne one)
	{
		int i = 0;
		for (int count = mEquip.wears.Count; i < count && EquipPositions.ContainsKey(i); i++)
		{
			if (EquipPositions[i] == one.Position && string.IsNullOrEmpty(mEquip.wears[i]))
			{
				return true;
			}
		}
		return false;
	}

	public bool Equip_is_same_wear(EquipOne one)
	{
		return false;
	}

	public List<int> Equip_GetCanWears(int position)
	{
		List<int> list = new List<int>();
		int i = 0;
		for (int count = mEquip.wears.Count; i < count; i++)
		{
			if (EquipPositions[i] == position)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public int Equip_GetWeapon()
	{
		if (!mEquip.wear_enable)
		{
			return 0;
		}
		string uniqueid = mEquip.wears[0];
		return GetEquipByUniqueID(uniqueid)?.data.EquipIcon ?? 0;
	}

	public int Equip_GetCloth()
	{
		if (!mEquip.wear_enable)
		{
			return 0;
		}
		string uniqueid = mEquip.wears[1];
		return GetEquipByUniqueID(uniqueid)?.data.EquipIcon ?? 0;
	}

	public int Equip_GetPet(int index)
	{
		if (!mEquip.wear_enable)
		{
			return 0;
		}
		string uniqueid = mEquip.wears[4 + index];
		return GetEquipByUniqueID(uniqueid)?.data.EquipIcon ?? 0;
	}

	public void Equip_Attribute2(SelfAttributeData attribute)
	{
		int i = 0;
		for (int count = mEquip.wears.Count; i < count; i++)
		{
			string text = mEquip.wears[i];
			if (!string.IsNullOrEmpty(text))
			{
				mEquip.GetEquipByUniqueID(text)?.EquipWear(attribute);
			}
		}
	}

	public void Equip_GetUniqueidByEquipID(int equipid)
	{
	}

	public bool Equip_GetHaveEquips()
	{
		return GetHaveEquips(havewear: true).Count > 0;
	}

	public List<int> Equip_GetSkills()
	{
		List<int> list = new List<int>();
		int i = 0;
		for (int count = mEquip.wears.Count; i < count; i++)
		{
			string text = mEquip.wears[i];
			if (!string.IsNullOrEmpty(text))
			{
				EquipOne equipByUniqueID = mEquip.GetEquipByUniqueID(text);
				if (equipByUniqueID != null)
				{
					List<int> skills = equipByUniqueID.GetSkills();
					list.AddRange(skills);
				}
			}
		}
		return list;
	}

	public bool Equip_GetRefresh()
	{
		return mEquip.bRefresh;
	}

	public void Equip_SetRefresh()
	{
		mEquip.bRefresh = false;
	}

	public void Equip_AddUpdateAction(Action callback)
	{
		EquipData mEquip = this.mEquip;
		mEquip.mUpdateAction = (Action)Delegate.Combine(mEquip.mUpdateAction, callback);
	}

	public void Equip_RemoveUpdateAction(Action callback)
	{
		EquipData mEquip = this.mEquip;
		mEquip.mUpdateAction = (Action)Delegate.Remove(mEquip.mUpdateAction, callback);
	}

	public int Equip_can_combine_count()
	{
		return mEquip.combine_can_count();
	}

	public bool Equip_can_combine(EquipOne one)
	{
		return mEquip.combine_can(one);
	}

	public List<EquipOne> Equip_get_equip_babies()
	{
		List<EquipOne> list = new List<EquipOne>();
		int i = 0;
		for (int count = mEquip.wears.Count; i < count; i++)
		{
			string uniqueid = mEquip.wears[i];
			EquipOne equipByUniqueID = GetEquipByUniqueID(uniqueid);
			if (equipByUniqueID != null && equipByUniqueID.data.Position == 6)
			{
				list.Add(equipByUniqueID);
			}
		}
		return list;
	}

	public bool Equip_can_drop_equipexp(int id)
	{
		Dictionary<string, EquipOne>.Enumerator enumerator = mEquip.list.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.NeedMatID == id)
			{
				return true;
			}
		}
		return false;
	}

	private void EquipAchieve_GetNewEquip(int pos, int quality)
	{
		PlayerPrefsEncrypt.SetInt(EquipAchieve_GetNewEquipString(pos, quality), EquipAchieve_GetNewEquipLocal(pos, quality) + 1);
	}

	private string EquipAchieve_GetNewEquipString(int pos, int quality)
	{
		return Utils.FormatString("EquipAcheveCount_{0}_{1}", pos, quality);
	}

	public int EquipAchieve_GetNewEquipLocal(int pos, int quality)
	{
		return PlayerPrefsEncrypt.GetInt(EquipAchieve_GetNewEquipString(pos, quality));
	}

	private void InitHarvest()
	{
	}

	private void InitMail()
	{
		Mail = mSaveData.mMail;
		Mail.Init();
	}

	private void InitStage()
	{
		if (mStage.CurrentStage == 0)
		{
			mStage.CurrentStage = 1;
			mStage.FirstIn = false;
			mStage.Refresh();
		}
	}

	public int Stage_GetStage()
	{
		if (mStage != null)
		{
			return mStage.CurrentStage;
		}
		FileUtils.WriteError("Stage_GetStage stage is null.");
		return 1;
	}

	public void Stage_SetStage(int stageid)
	{
		if (mStage != null)
		{
			mStage.CurrentStage = stageid;
		}
	}

	public bool Stage_GetFirstIn()
	{
		if (mStage != null)
		{
			return mStage.FirstIn;
		}
		FileUtils.WriteError("Stage_GetFirstIn stage is null.");
		return false;
	}

	public void Stage_SetFirstIn()
	{
		if (mStage != null)
		{
			mStage.FirstIn = false;
			mStage.Refresh();
		}
	}

	public void Stage_InitNextID(int id)
	{
		if (mStage != null)
		{
			mStage.BoxLayerID = id;
			mStage.Refresh();
		}
	}

	public int Stage_GetNextID()
	{
		if (mStage != null)
		{
			return mStage.BoxLayerID + 1;
		}
		return 1;
	}

	public void Stage_GetNextEnd()
	{
		if (mStage != null)
		{
			mStage.BoxLayerID++;
			mStage.Refresh();
		}
	}

	public void Stage_CheckUnlockNext(int roomID)
	{
		int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		int maxChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
		if (level_CurrentStage < maxChapter && level_CurrentStage == Stage_GetStage())
		{
			Stage_Level_stagechapter beanByChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(level_CurrentStage + 1);
			int argsOpen = beanByChapter.ArgsOpen;
			if (beanByChapter != null && roomID >= argsOpen)
			{
				mStage.CurrentStage++;
				mStage.FirstIn = true;
				mStage.Refresh();
				GameLogic.Hold.BattleData.Level_CurrentStage = mStage.CurrentStage;
			}
		}
	}

	public void StageDiscount_Init(string data)
	{
		Debugger.Log("StageDiscount_Init " + data);
		bool flag = true;
		if (string.IsNullOrEmpty(data))
		{
			if (mStageDiscount == null)
			{
				flag = false;
			}
			mStageDiscount = null;
		}
		else
		{
			try
			{
				mStageDiscount = JsonConvert.DeserializeObject<StageDiscountBody>(data);
				Debugger.Log(mStageDiscount.ToString());
			}
			catch
			{
				mStageDiscount = null;
				Debugger.Log("StageDiscount_Init init failed! ::: " + data);
			}
		}
		if (flag)
		{
			Facade.Instance.SendNotification("ShopUI_Update");
		}
	}

	public string StageDiscount_GetProductID()
	{
		if (mStageDiscount != null && mStageDiscount.current_purchase != null)
		{
			return mStageDiscount.current_purchase.product_id;
		}
		return string.Empty;
	}

	public int StageDiscount_GetCurrentID()
	{
		if (mStageDiscount != null)
		{
			return mStageDiscount.Get_CurrentID();
		}
		return 9999;
	}

	public int StageDiscount_GetLastID()
	{
		if (mStageDiscount != null)
		{
			return mStageDiscount.Get_LastID();
		}
		return 100;
	}

	public bool StageDiscount_IsValid()
	{
		if (mStageDiscount != null)
		{
			return mStageDiscount.IsValid;
		}
		return false;
	}

	public bool IsAdFree()
	{
		return false;
	}

	public List<Drop_DropModel.DropData> StageDiscount_GetList()
	{
		if (mStageDiscount != null)
		{
			return mStageDiscount.GetList();
		}
		return null;
	}

	public void StageDiscount_Send(Action<string> callback)
	{
		CQueryFirstRewardInfoPacket packet = new CQueryFirstRewardInfoPacket();
		NetManager.SendInternal(packet, SendType.eLoop, delegate(NetResponse response)
		{
			if (response.error != null)
			{
				CCommonRespMsg error = response.error;
				if (error != null)
				{
					if (callback != null)
					{
						callback(error.m_strInfo);
					}
				}
				else
				{
					SdkManager.Bugly_Report("LocalSave_StageDiscount", "OnLoginCallback_back response is not a CRespFirstRewardInfo Type");
				}
			}
		});
	}

	public void DoThreadSave(EThreadWriteType type)
	{
		lock (mThreadDoing)
		{
			mThreadList[type] = true;
		}
	}

	private void CreateThread()
	{
		mSaveData = FileUtils.GetXml<SaveData>("localsave.txt");
		Thread thread = new Thread((ThreadStart)delegate
		{
			do
			{
				IL_0000:
				lock (mThreadDoing)
				{
					if (mThreadList[EThreadWriteType.eBattle])
					{
						mThreadList[EThreadWriteType.eBattle] = false;
						FileUtils.WriteBattleInThread(mBattleIn);
					}
					else if (mThreadList[EThreadWriteType.eEquip])
					{
						mThreadList[EThreadWriteType.eEquip] = false;
						FileUtils.WriteXmlThread("localequip.txt", mEquip);
					}
					else if (mThreadList[EThreadWriteType.eLocal])
					{
						mThreadList[EThreadWriteType.eLocal] = false;
#if ENABLE_TEST_GAME
                        mSaveData.mStage.CurrentStage = 10;
#endif
                        FileUtils.WriteXmlThread("localsave.txt", mSaveData);
					}
					else
					{
						if (!mThreadList[EThreadWriteType.eNet])
						{
							goto IL_00f2;
						}
						mThreadList[EThreadWriteType.eNet] = false;
						string fileName = NetCaches.GetFileName(Instance.GetServerUserID());
						FileUtils.WriteXmlThread(fileName, NetManager.mNetCache);
					}
				}
				goto IL_0000;
				IL_00f2:
				Thread.Sleep(2000);
			}
			while (!ApplicationEvent.bQuit);
		});
		thread.IsBackground = true;
		thread.Start();
	}

	private void InitTimeBoxTime()
	{
		mTimeBox = mSaveData.mTimeBoxData;
		mTimeBox.Init();
	}

	public long GetTimeBoxTime(TimeBoxType type)
	{
		if (mTimeBox == null)
		{
			return 0L;
		}
		return mTimeBox.GetTime(type);
	}

	public void SetTimeBoxTime(TimeBoxType type, long time = 0L)
	{
		if (mTimeBox != null)
		{
			if (time == 0)
			{
				time = Utils.GetTimeStamp();
			}
			Debugger.Log("SetTimeBoxTime " + type + " -> " + time);
			mTimeBox.SetTime(type, time);
		}
	}

	public int GetTimeBoxCount(TimeBoxType type)
	{
		return mTimeBox.GetCount(type);
	}

	public void UsserInfo_SetTimeBoxCount(TimeBoxType type, int count)
	{
		if (mTimeBox != null)
		{
			mTimeBox.SetCount(type, count);
		}
	}

	public void Modify_TimeBoxCount(TimeBoxType type, int count, bool over = false)
	{
		if (mTimeBox != null)
		{
			if (mTimeBox.IsMaxCount(type) && count < 0)
			{
				SetTimeBoxTime(type, Utils.GetTimeStamp());
			}
			mTimeBox.UpdateCount(type, count, over);
		}
	}

	public bool IsTimeBoxMax(TimeBoxType type)
	{
		if (mTimeBox == null)
		{
			return false;
		}
		return mTimeBox.IsMaxCount(type);
	}

	public bool IsBoxOpenFree(TimeBoxType type)
	{
		if (mTimeBox == null)
		{
			return false;
		}
		return mTimeBox.IsBoxOpenFree(type);
	}

	public List<Drop_DropModel.DropData> GetDropTimeBoxRandom()
	{
		int dropId = LocalModelManager.Instance.Box_TimeBox.GetBeanById(Stage_GetStage()).DropId;
		return LocalModelManager.Instance.Drop_Drop.GetDropList(dropId);
	}

	public void UserInfo_Set(int gold, int diamond, int exp, int key, int level, int diamondnormal, int diamondlarge)
	{
		userInfo.Gold = gold;
		userInfo.Show_Gold = gold;
		userInfo.Diamond = diamond;
		userInfo.Show_Diamond = diamond;
		userInfo.Exp = exp;
		userInfo.Show_Exp = exp;
		userInfo.Key = key;
		userInfo.Level = level;
		userInfo.DiamondNormalExtraCount = diamondnormal;
		userInfo.DiamondLargeExtraCount = diamondlarge;
		if (userInfo.Key > 0)
		{
			TrustCount_Refresh();
		}
		userInfo.Refresh();
		Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
	}

	public void UserInfo_SetGold(int gold)
	{
		userInfo.Gold = gold;
		userInfo.Show_Gold = gold;
		Modify_Gold(0L);
		Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
	}

	public void UserInfo_SetDiamond(int diamond)
	{
		userInfo.Diamond = diamond;
		userInfo.Show_Diamond = diamond;
		Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
	}

	public void UserInfo_SetKey(int key)
	{
		userInfo.Key = key;
		Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
	}

	public bool GetUserInfoInit()
	{
		return userInfo.isInit;
	}

	public void SetUserInfoInit()
	{
		userInfo.isInit = true;
		Modify_Gold(0L);
		Modify_TimeBoxCount(TimeBoxType.BoxChoose_DiamondNormal, 0, over: true);
		SaveUserInfo(updateui: false);
	}

	public void SetServerUserID(ulong id)
	{
		userInfo.ServerUserID = id;
		if (userInfo.ServerUserID != 0)
		{
			SdkManager.ShuShu_Login(GetServerUserIDSub());
		}
		userInfo.Refresh();
	}

	public ulong GetServerUserID()
	{
		return userInfo.ServerUserID;
	}

	public string GetServerUserIDSub()
	{
		string result = string.Empty;
		try
		{
			byte[] bytes = BitConverter.GetBytes(userInfo.ServerUserID);
			bytes[5] = 0;
			bytes[6] = 0;
			bytes[7] = 0;
			ulong num = BitConverter.ToUInt64(bytes, 0);
			result = Utils.FormatString("{0}{1}", bytes[7], num);
			return result;
		}
		catch
		{
			return result;
		}
	}

	public void SetDiamondBoxGuide()
	{
		userInfo.guide_diamondbox = true;
		userInfo.Refresh();
	}

	public bool GetDiamondBoxGuide()
	{
		return userInfo.guide_diamondbox;
	}

	public void Modify_Gold(long gold, bool updateui = true)
	{
		userInfo.Gold += gold;
		if (updateui)
		{
			userInfo.Show_Gold = userInfo.Gold;
		}
		int num = 200;
		Skill_slotoutcost beanById = LocalModelManager.Instance.Skill_slotoutcost.GetBeanById(1);
		if (beanById != null)
		{
			num = beanById.CoinCost;
		}
		if (userInfo.Gold >= num && GameLogic.Hold.Guide.mCard != null)
		{
			GameLogic.Hold.Guide.mCard.StartGuide();
		}
		SaveUserInfo(updateui);
		if (GoldUpdateEvent != null)
		{
			GoldUpdateEvent(userInfo.Gold, gold);
		}
	}

	public void Modify_ShowGold(long gold)
	{
		userInfo.Show_Gold += gold;
		Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
	}

	public void Modify_ShowDiamond(long diamond)
	{
		userInfo.Show_Diamond += diamond;
		Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
	}

	public void Modify_Diamond(long diamond, bool updateui = true)
	{
		userInfo.Diamond += diamond;
		if (updateui)
		{
			userInfo.Show_Diamond = userInfo.Diamond;
			Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
		}
		SaveUserInfo(updateui);
	}

	public void Modify_Resource(long resource, bool updateui = true)
	{
		userInfo.Resource += resource;
		SaveUserInfo(updateui);
	}

	public void Modify_Exp(long exp, bool updateui = true)
	{
		userInfo.Exp += exp;
		if (updateui)
		{
			userInfo.Show_Exp = userInfo.Exp;
			Facade.Instance.SendNotification("MainUI_UpdateExp");
		}
		SaveUserInfo(updateui: false);
	}

	public void Modify_ShowExp(long exp)
	{
		userInfo.Show_Exp += exp;
		Facade.Instance.SendNotification("MainUI_UpdateExp");
	}

	public void SetDiamondExtraCount(TimeBoxType type, int count)
	{
		if (userInfo != null)
		{
			if (type == TimeBoxType.BoxChoose_DiamondNormal)
			{
				userInfo.DiamondNormalExtraCount = count;
			}
			else
			{
				userInfo.DiamondLargeExtraCount = count;
			}
			Facade.Instance.SendNotification("MainUI_ShopRedCountUpdate");
			userInfo.Refresh();
		}
	}

	public void Modify_DiamondExtraCount(TimeBoxType type, int count)
	{
		if (userInfo == null)
		{
			return;
		}
		if (type == TimeBoxType.BoxChoose_DiamondNormal)
		{
			if (userInfo.DiamondNormalExtraCount == 0 && count > 0)
			{
				Instance.mGuideData.check_diamondbox_first_open();
			}
			userInfo.DiamondNormalExtraCount += count;
		}
		else
		{
			userInfo.DiamondLargeExtraCount += count;
		}
		Facade.Instance.SendNotification("MainUI_ShopRedCountUpdate");
		userInfo.Refresh();
	}

	public int GetDiamondExtraCount(TimeBoxType type)
	{
		if (userInfo != null)
		{
			if (type == TimeBoxType.BoxChoose_DiamondNormal)
			{
				return userInfo.DiamondNormalExtraCount;
			}
			return userInfo.DiamondLargeExtraCount;
		}
		return 0;
	}

	public int GetDiamondBoxFreeCount(TimeBoxType type)
	{
		int num = 0;
		num += GetTimeBoxCount(type);
		return num + GetDiamondExtraCount(type);
	}

	public void SetLevel(int level)
	{
		userInfo.Level = level;
		SaveUserInfo(updateui: false);
	}

	public void SetExp(int exp)
	{
		userInfo.Exp = exp;
		SaveUserInfo(updateui: false);
	}

	public void Modify_drop(string str)
	{
		try
		{
			Drop_DropModel.DropSaveOneData dropOne = Drop_DropModel.GetDropOne(str);
			if (dropOne.type == 1)
			{
				switch (dropOne.id)
				{
				case 1:
					Modify_Gold(dropOne.count);
					break;
				case 2:
					Modify_Diamond(dropOne.count);
					break;
				case 3:
					Modify_Key(dropOne.count);
					break;
				case 4:
					Modify_RebornCount(dropOne.count);
					break;
				default:
					SdkManager.Bugly_Report("LocalSave_UserInfo", Utils.FormatString("Modify_drop currency : {0} is not a valid currency type.", str));
					break;
				}
			}
			else
			{
				SdkManager.Bugly_Report("LocalSave_UserInfo", Utils.FormatString("Modify_drop other : {0} is not a valid type.", str));
			}
		}
		catch
		{
			SdkManager.Bugly_Report("LocalSave_UserInfo", Utils.FormatString("Modify_drop : {0} is error.", str));
		}
	}

	public void Modify_drop(string[] strs)
	{
		int i = 0;
		for (int num = strs.Length; i < num; i++)
		{
			Modify_drop(strs[i]);
		}
	}

	public void BattleAd_Set(int count)
	{
		if (userInfo != null)
		{
			userInfo.BattleAdCount = count;
			userInfo.Refresh();
		}
	}

	public void BattleAd_Use()
	{
		if (userInfo != null)
		{
			userInfo.BattleAdCount--;
			userInfo.Refresh();
		}
		GameLogic.Hold.BattleData.Battle_ad_use();
	}

	public int BattleAd_Get()
	{
		if (userInfo != null)
		{
			return userInfo.BattleAdCount;
		}
		return 0;
	}

	public bool BattleAd_CanShow()
	{
		if (userInfo != null)
		{
			return userInfo.BattleAdCount > 0 && Card_GetLevel() >= GameConfig.BattleAdUnlockTalentLevel;
		}
		return false;
	}

	public void Set_Gold(long gold, bool updateui = true)
	{
		userInfo.Gold = gold;
		if (updateui)
		{
			userInfo.Show_Gold = gold;
			Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
		}
	}

	public bool Use_Gold(long gold)
	{
		if (userInfo.Gold >= gold)
		{
			userInfo.Gold -= gold;
			SaveUserInfo();
			return true;
		}
		return false;
	}

	public void UserInfo_SetBestScore(int score)
	{
		if (userInfo.Score < score)
		{
			userInfo.Score = score;
			Facade.Instance.SendNotification("MainUI_LayerUpdate");
		}
	}

	public void UserInfo_SetRebornCount(int count)
	{
		userInfo.reborncount = count;
		userInfo.Refresh();
	}

	public void UserInfo_SetAdKeyCount(int count)
	{
		userInfo.AdKeyCount = count;
		userInfo.Refresh();
	}

	public int UserInfo_GetAdKeyCount()
	{
		if (userInfo != null)
		{
			return userInfo.AdKeyCount;
		}
		return 0;
	}

	public bool UserInfo_UseAdKeyCount()
	{
		if (userInfo != null && userInfo.AdKeyCount > 0)
		{
			userInfo.AdKeyCount--;
			userInfo.Refresh();
			return true;
		}
		return false;
	}

	public int GetRebornCount()
	{
		return userInfo.reborncount;
	}

	public void Modify_RebornCount(int value)
	{
		userInfo.reborncount += value;
		userInfo.Refresh();
	}

	public void TrustCount_Refresh()
	{
		userInfo.KeyTrustCount = GameConfig.GetKeyTrustCount();
		userInfo.Refresh();
	}

	public bool TrustCount_Use(short count)
	{
		if (userInfo.KeyTrustCount >= count)
		{
			userInfo.KeyTrustCount -= count;
			userInfo.Refresh();
			return true;
		}
		return false;
	}

	public int GetScore()
	{
		return userInfo.Score;
	}

	public UserInfo GetUserInfo()
	{
		return userInfo;
	}

	public bool GetUserLoginSDK()
	{
		return userInfo.bLoginedSDK;
	}

	public void SetUserLoginSDK(bool value)
	{
		userInfo.bLoginedSDK = value;
	}

	public string GetUserName()
	{
		if (userInfo != null)
		{
			if (!string.IsNullOrEmpty(userInfo.UserName_Temp))
			{
				return userInfo.UserName_Temp;
			}
			return userInfo.UserName;
		}
		return string.Empty;
	}

	public void SetUserName_Temp(string name)
	{
		if (userInfo != null)
		{
			userInfo.UserName_Temp = name;
		}
	}

	public long GetGold()
	{
		return userInfo.Gold;
	}

	public int GetKey()
	{
		return userInfo.Key;
	}

	public long GetResource()
	{
		return userInfo.Resource;
	}

	public long GetDiamond()
	{
		return userInfo.Diamond;
	}

	public long GetShowDiamond()
	{
		return userInfo.Show_Diamond;
	}

	public long GetExp()
	{
		return userInfo.Exp;
	}

	public long GetShowExp()
	{
		return userInfo.Show_Exp;
	}

	public int GetLevel()
	{
		return userInfo.Level;
	}

	public bool GetCanLevelUp()
	{
		return userInfo.Exp >= GetExpByLevel(userInfo.Level);
	}

	public void LevelUp()
	{
		if (GetCanLevelUp())
		{
			userInfo.Exp -= GetExpByLevel(userInfo.Level);
			userInfo.Show_Exp = userInfo.Exp;
			userInfo.Level++;
			userInfo.Refresh();
			Facade.Instance.SendNotification("MainUI_UpdateExp");
		}
	}

	public string GetUserID()
	{
		if (userInfo != null)
		{
			if (!string.IsNullOrEmpty(userInfo.UserID_Temp))
			{
				return userInfo.UserID_Temp;
			}
			return userInfo.UserID;
		}
		return string.Empty;
	}

	public void SetUserID(LoginType type, string id, string name)
	{
		userInfo.loginType = type;
		userInfo.UserID = id;
		userInfo.UserName = name;
		SaveUserInfo(updateui: false);
	}

	public void SetUserName(string name)
	{
		userInfo.UserName = name;
		SaveUserInfo(updateui: false);
	}

	public void RefreshUserIDFromTemp()
	{
		userInfo.UserID = userInfo.UserID_Temp;
		userInfo.UserName = userInfo.UserName_Temp;
		userInfo.UserID_Temp = string.Empty;
		userInfo.UserName_Temp = string.Empty;
	}

	public void SetUserTemp(string id, string name)
	{
		if (userInfo != null)
		{
			userInfo.UserID_Temp = id;
			userInfo.UserName_Temp = name;
		}
	}

	public LoginType GetLoginType()
	{
		return userInfo.loginType;
	}

	public long GetNetID()
	{
		if (userInfo != null)
		{
			long netID = userInfo.NetID;
			UpdateNetID();
			return netID;
		}
		return 0L;
	}

	private void UpdateNetID()
	{
		if (userInfo != null)
		{
			userInfo.NetID++;
			userInfo.Refresh();
		}
	}

	private void SaveUserInfo(bool updateui = true)
	{
		userInfo.Refresh();
		if (updateui)
		{
			Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
		}
	}

	private void InitUserInfo()
	{
		userInfo = mSaveData.userInfo;
		if (!GetUserInfoInit())
		{
			userInfo.Key = GameConfig.GetMaxKeyStartCount();
			userInfo.Gold = 100L;
			userInfo.Diamond = 100L;
			userInfo.KeyTrustCount = GameConfig.GetKeyTrustCount();
			SetUserInfoInit();
		}
		userInfo.Show_Gold = userInfo.Gold;
		userInfo.Show_Diamond = userInfo.Diamond;
		userInfo.Show_Exp = userInfo.Exp;
	}

	private void InitKeyTime()
	{
		long num = PlayerPrefsEncrypt.GetLong("Currency_Key_Time", 0L);
		if (num == 0)
		{
			num = Utils.GetTimeStamp();
			PlayerPrefsEncrypt.SetLong("Currency_Key_Time", num);
		}
		mCurrencyKeyTime = num;
	}

	public void UserInfo_SetKeyTime(long time)
	{
		mCurrencyKeyTime = time;
		PlayerPrefsEncrypt.SetLong("Currency_Key_Time", time);
	}

	public long GetKeyTime()
	{
		return mCurrencyKeyTime;
	}

	public void SetKeyTime(long time)
	{
		if (mCurrencyKeyTime < time)
		{
			UserInfo_SetKeyTime(time);
		}
	}

	public void Modify_Key(long key, bool over = true)
	{
		int maxKeyCount = GameConfig.GetMaxKeyCount();
		if (key < 0)
		{
			if (userInfo.Key >= maxKeyCount && userInfo.Key + key < maxKeyCount)
			{
				SetKeyTime(Utils.GetTimeStamp());
			}
			userInfo.Key += (int)key;
		}
		else if (userInfo.Key + key > maxKeyCount)
		{
			if (over)
			{
				userInfo.Key += (int)key;
			}
			else
			{
				userInfo.Key = maxKeyCount;
			}
		}
		else
		{
			userInfo.Key += (int)key;
		}
		Facade.Instance.SendNotification("PUB_UI_UPDATE_CURRENCY");
		SaveUserInfo();
	}

	public bool IsKeyMax()
	{
		return userInfo.Key >= GameConfig.GetMaxKeyCount();
	}

	public void DoLogin_Start(Action callback)
	{
		SdkManager.Login(delegate
		{
			DoLogin(SendType.eLoop, delegate
			{
				Debugger.Log("====DoLogin_Start DoLogin callback");
				if (callback != null)
				{
					callback();
				}
			});
		});
	}

	public void DoLogin(SendType sendType, Action callback)
	{
		CUserLoginPacket packet = new CUserLoginPacket();
		NetManager.SendInternal(packet, sendType, delegate(NetResponse response)
		{
#if ENABLE_NET_MANAGER
			if (response != null && response.error != null)
			{
				CCommonRespMsg error = response.error;
				if (error.m_nStatusCode == 16)
				{
					WindowUI.ShowWindow(WindowID.WindowID_ForceUpdate);
				}
			}
			else if (response.IsSuccess && response.data != null && response.data is CRespUserLoginPacket)
			{
				CRespUserLoginPacket cRespUserLoginPacket = response.data as CRespUserLoginPacket;
				if (Instance.GetServerUserID() != 0 && Instance.GetServerUserID() != cRespUserLoginPacket.m_nUserRawId)
				{
					DoLoginCallBack(cRespUserLoginPacket, delegate
					{
						if (callback != null)
						{
							callback();
						}
						WindowUI.ReOpenMain();
					});
				}
				else
				{
					DoLoginCallBack(cRespUserLoginPacket, callback);
				}
				if (cRespUserLoginPacket == null)
				{
					DoLogin(sendType, callback);
				}
			}
			else
			{
				DoLoginCallBack(null, callback);
			}
#endif
        }
        );
	}

	public void DoLoginCallBack(CRespUserLoginPacket data, Action callback)
	{
		if (data != null)
		{
			Debugger.Log(Utils.FormatString("m_nCoins : {0}", data.m_nCoins));
			Debugger.Log(Utils.FormatString("m_nDiamonds : {0}", data.m_nDiamonds));
			Debugger.Log(Utils.FormatString("m_nExperince : {0}", data.m_nExperince));
			Debugger.Log(Utils.FormatString("m_nLevel : {0}", data.m_nLevel));
			Debugger.Log(Utils.FormatString("m_nMaxLayer : {0}", data.m_nMaxLayer));
			Debugger.Log(Utils.FormatString("m_nUserRawId : {0}", data.m_nUserRawId));
			Debugger.Log(Utils.FormatString("m_lHarvestTimestamp : {0}", data.GetHarvestTime()));
			Debugger.Log(Utils.FormatString("m_nExtraNormalDiamondItem : {0}", data.m_nExtraNormalDiamondItem));
			Debugger.Log(Utils.FormatString("m_nExtraLargeDiamondItem : {0}", data.m_nExtraLargeDiamondItem));
			Instance.mGuideData.SetGameSystemMask(data.m_nGameSystemMask);
			Instance.SetServerUserID(data.m_nUserRawId);
			Instance.GetUserInfo().bLogined = true;
			Instance.UserInfo_SetRebornCount(data.m_nBattleRebornCount);
			Instance.DropCard_Init((int)data.m_nTreasureRandomCount);
			Instance.mFakeCardCost.InitCount((int)data.m_nTreasureRandomCount);
			NetManager.SetNetTime((long)data.GetServerTime());
			Debugger.Log("server time = " + data.GetServerTime());
			CRestoreItem restore = data.GetRestore(CRestoreItem.EItemIndex.ENormalDiamondItemIndex);
			Instance.UsserInfo_SetTimeBoxCount(TimeBoxType.BoxChoose_DiamondNormal, restore.m_nMin);
			Instance.SetTimeBoxTime(TimeBoxType.BoxChoose_DiamondNormal, (long)restore.m_i64Timestamp);
			Debugger.Log("BoxChoose_DiamondNormal time = " + restore.m_i64Timestamp);
			CRestoreItem restore2 = data.GetRestore(CRestoreItem.EItemIndex.ELargeDiamondItemIndex);
			Debugger.Log("BoxChoose_DiamondLarge time = " + restore2.m_i64Timestamp);
			Instance.UsserInfo_SetTimeBoxCount(TimeBoxType.BoxChoose_DiamondLarge, restore2.m_nMin);
			Instance.SetTimeBoxTime(TimeBoxType.BoxChoose_DiamondLarge, (long)restore2.m_i64Timestamp);
			CRestoreItem restore3 = data.GetRestore(CRestoreItem.EItemIndex.ELifeItemIndex);
			Instance.UserInfo_SetKeyTime((long)restore3.m_i64Timestamp);
			CRestoreItem restore4 = data.GetRestore(CRestoreItem.EItemIndex.EAdGetLifeItemIndex);
			Instance.UserInfo_SetAdKeyCount(restore4.m_nMin);
			CRestoreItem restore5 = data.GetRestore(CRestoreItem.EItemIndex.EAdGetLuckyItemIndex);
			Instance.BattleAd_Set(restore5.m_nMin);
			UnityEngine.Debug.Log(Utils.FormatString("m_nLife : {0}", restore3.m_nMin));
			Instance.UserInfo_Set((int)data.m_nCoins, (int)data.m_nDiamonds, (int)data.m_nExperince, restore3.m_nMin, data.m_nLevel, data.m_nExtraNormalDiamondItem, data.m_nExtraLargeDiamondItem);
			Instance.SaveExtra.InitTransID(data.m_nTransID);
			Instance.Stage_InitNextID(data.m_nLayerBoxID);
			Instance.mStage.InitMaxLevel(data.m_nMaxLayer);
			Instance.mHarvest.init_last_time((long)data.GetHarvestTime());
			Instance.mShop.bRefresh = true;
			Instance.mEquip.bRefresh = true;
			List<CEquipmentItem> list = new List<CEquipmentItem>();
			List<CEquipmentItem> list2 = new List<CEquipmentItem>();
			Debugger.Log(Utils.FormatString("arrayEquipItems : {0}", data.m_arrayEquipData.Length));
			int i = 0;
			for (int num = data.m_arrayEquipData.Length; i < num; i++)
			{
				CEquipmentItem cEquipmentItem = data.m_arrayEquipData[i];
				if (cEquipmentItem.m_nEquipID < 1000)
				{
					if (LocalModelManager.Instance.Skill_slotout.GetBeanById((int)cEquipmentItem.m_nEquipID) != null)
					{
						list2.Add(cEquipmentItem);
					}
				}
				else if (LocalModelManager.Instance.Equip_equip.GetBeanById((int)cEquipmentItem.m_nEquipID) != null)
				{
					list.Add(cEquipmentItem);
				}
			}
			Instance.Equip_Set(list);
			Instance.Card_Set(list2);
			GameLogic.Hold.Guide.Init();
			Instance.SetUserInfoInit();
		}
		callback?.Invoke();
		Instance.Mail.SendMail();
		Instance.StageDiscount_Send(delegate(string d)
		{
			Instance.StageDiscount_Init(d);
		});
	}

	public void TryLogin(Action<bool, CRespUserLoginPacket> callback)
	{
		Action<LoginType, SdkManager.LoginData> callback2 = delegate(LoginType logintype, SdkManager.LoginData userdata)
		{
			Instance.SetUserTemp(userdata.userid, userdata.username);
			TryLoginServer(callback);
		};
		SdkManager.TryLogin(callback2);
	}

	private void TryLoginServer(Action<bool, CRespUserLoginPacket> callback)
	{
		CUserLoginPacket packet = new CUserLoginPacket();
		NetManager.SendInternal(packet, SendType.eUDP, delegate(NetResponse response)
		{
			if (response.IsSuccess && response.data != null)
			{
				CRespUserLoginPacket cRespUserLoginPacket = response.data as CRespUserLoginPacket;
				if (Instance.GetServerUserID() != cRespUserLoginPacket.m_nUserRawId)
				{
					if (callback != null)
					{
						callback(arg1: false, cRespUserLoginPacket);
					}
				}
				else if (callback != null)
				{
					callback(arg1: true, null);
				}
			}
		});
	}
}
