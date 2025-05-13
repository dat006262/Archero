




using Dxx.Util;
using System;
using System.Collections.Generic;

namespace TableTool
{
	public class Shop_MysticShopModel : LocalModel<Shop_MysticShop, int>
	{
		[Serializable]
		public class MysticShopData : LocalSaveBase
		{
			public int stage = 1;

			public int rate;

			public void Reset(int stage, int rate)
			{
				this.stage = stage;
				this.rate = rate;
				Refresh();
			}

			public void ResetRate(int rate)
			{
				this.rate = rate;
				Refresh();
			}

			public void AddRate(int rate)
			{
				this.rate += rate;
				Refresh();
			}

			protected override void OnRefresh()
			{
				FileUtils.WriteXml("File_MysticShop", this);
			}
		}

		public class ShopData
		{
			public int stageid;

			public Dictionary<int, Dictionary<int, WeightRandom>> mList = new Dictionary<int, Dictionary<int, WeightRandom>>();

			public void Add(Shop_MysticShop data)
			{
				Dictionary<int, WeightRandom> value = null;
				WeightRandom value2 = null;
				int i = 0;
				for (int num = data.Position.Length; i < num; i++)
				{
					if (!mList.TryGetValue(data.Position[i], out value))
					{
						value = new Dictionary<int, WeightRandom>();
						mList.Add(data.Position[i], value);
					}
					if (!value.TryGetValue(data.ShopType, out value2))
					{
						value2 = new WeightRandom();
						value.Add(data.ShopType, value2);
					}
					value2.Add(data.ID, data.Weights);
				}
			}

			public List<Shop_MysticShop> GetList(int shoptype)
			{
				List<Shop_MysticShop> list = new List<Shop_MysticShop>();
				int sellCount = GetSellCount(shoptype);
				for (int i = 1; i <= sellCount; i++)
				{
					Dictionary<int, WeightRandom> dictionary = mList[i];
					int random = dictionary[shoptype].GetRandom();
					list.Add(LocalModelManager.Instance.Shop_MysticShop.GetBeanById(random));
				}
				return list;
			}

			public override string ToString()
			{
				string text = string.Empty;
				for (int i = 1; i <= mList.Count; i++)
				{
					string text2 = text;
					text = text2 + "Pos:" + i + " : " + mList[i].ToString() + "\n";
				}
				return text;
			}
		}

		private const string _Filename = "Shop_MysticShop";

		public static Dictionary<int, int> mSellCounts = new Dictionary<int, int>
		{
			{
				1,
				1
			},
			{
				2,
				2
			},
			{
				3,
				2
			},
			{
				4,
				3
			},
			{
				5,
				3
			}
		};

		private MysticShopData mMysticShopData;

		private Dictionary<int, ShopData> mEquipList = new Dictionary<int, ShopData>();

		private Dictionary<int, WeightRandom> mCountList = new Dictionary<int, WeightRandom>();

		protected override string Filename => "Shop_MysticShop";

		protected override int GetBeanKey(Shop_MysticShop bean)
		{
			return bean.ID;
		}

		public static int GetSellCount(int shoptype)
		{
			int value = 0;
			if (mSellCounts.TryGetValue(shoptype, out value))
			{
				return value;
			}
			SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("the shoptype:{0} is not in mSellCounts!", shoptype));
			return value;
		}

		public void Init()
		{
			init_show_prop_weight();
			mMysticShopData = LocalSave.Instance.mSaveData.mMysticShopData;
			IList<Shop_MysticShop> allBeans = GetAllBeans();
			int i = 0;
			for (int count = allBeans.Count; i < count; i++)
			{
				Shop_MysticShop shop_MysticShop = allBeans[i];
				int j = 0;
				for (int num = shop_MysticShop.Stage.Length; j < num; j++)
				{
					if (!mEquipList.TryGetValue(shop_MysticShop.Stage[j], out ShopData value))
					{
						value = new ShopData();
						value.stageid = shop_MysticShop.Stage[j];
						mEquipList.Add(shop_MysticShop.Stage[j], value);
					}
					value.Add(shop_MysticShop);
				}
			}
			IList<Shop_MysticShopShow> allBeans2 = LocalModelManager.Instance.Shop_MysticShopShow.GetAllBeans();
			int k = 0;
			for (int count2 = allBeans2.Count; k < count2; k++)
			{
				Shop_MysticShopShow shop_MysticShopShow = allBeans2[k];
				WeightRandom weightRandom = new WeightRandom();
				int l = 0;
				for (int num2 = shop_MysticShopShow.ShopTypeProb.Length; l < num2; l++)
				{
					weightRandom.Add(l + 1, shop_MysticShopShow.ShopTypeProb[l]);
				}
				mCountList.Add(shop_MysticShopShow.ID, weightRandom);
			}
		}

		private void init_show_prop_weight()
		{
			IEnumerator<Shop_MysticShopShow> enumerator = LocalModelManager.Instance.Shop_MysticShopShow.GetAllBeans().GetEnumerator();
			while (enumerator.MoveNext())
			{
				int[] shopTypeProb = enumerator.Current.ShopTypeProb;
				if (shopTypeProb.Length != 2)
				{
					continue;
				}
				WeightRandom weightRandom = new WeightRandom();
				for (int i = 0; i < shopTypeProb.Length; i++)
				{
					if (shopTypeProb[i] > 0)
					{
						weightRandom.Add(i, shopTypeProb[i]);
					}
				}
			}
		}

		public int GetRandomShopType()
		{
			int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
			WeightRandom value = null;
			if (mCountList.TryGetValue(level_CurrentStage, out value))
			{
				return value.GetRandom();
			}
			SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("stage ��{0} is not in mCountList!", level_CurrentStage));
			return 0;
		}

		public List<Shop_MysticShop> GetListByStage(int stage, int shoptype)
		{
			if (!mEquipList.TryGetValue(stage, out ShopData value))
			{
				SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("GetListByStage[{0}] is not in mList.", stage));
				return new List<Shop_MysticShop>();
			}
			return value.GetList(shoptype);
		}

		public bool RandomShop(int stage, int roomid, RoomGenerateBase.RoomType roomtype)
		{
			if (roomtype == RoomGenerateBase.RoomType.eBoss)
			{
				return false;
			}
			Shop_MysticShopShow beanById = LocalModelManager.Instance.Shop_MysticShopShow.GetBeanById(stage);
			if (beanById == null)
			{
				SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("RandomShop stage:[{0}] is not in Shop_MysticShopShow.xls", stage));
				return false;
			}
			if (beanById.ShowRoom.Length != 2)
			{
				SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("RandomShop stage:[{0}].ShowRoom.Length[{1}] != 2", stage, beanById.ShowRoom.Length));
				return false;
			}
			if (roomid == 0 || roomid > beanById.ShowRoom[1] || roomid < beanById.ShowRoom[0])
			{
				return false;
			}
			if (mMysticShopData.stage != stage)
			{
				mMysticShopData.Reset(stage, beanById.ShowProb);
			}
			int rate = mMysticShopData.rate;
			if (GameLogic.Random(0, 10000) < rate)
			{
				mMysticShopData.ResetRate(beanById.ShowProb);
				return true;
			}
			mMysticShopData.AddRate(beanById.AddProb);
			return false;
		}

		public void AddRatio(int stage)
		{
			Shop_MysticShopShow beanById = LocalModelManager.Instance.Shop_MysticShopShow.GetBeanById(stage);
			mMysticShopData.AddRate(beanById.AddProb);
		}
	}
}
