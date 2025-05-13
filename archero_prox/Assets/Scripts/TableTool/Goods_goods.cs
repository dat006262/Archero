




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

namespace TableTool
{
	public class Goods_goods : LocalBean
	{
		public class GoodData
		{
			public string goodType;

			public long value;

			public bool percent;

			public override string ToString()
			{
				return Utils.FormatString("GoodData:{0} {1}", goodType, value);
			}

			public string GetSymbolString()
			{
				return (value < 0) ? "-" : "+";
			}
		}

		public class GoodShowData
		{
			public string goodType;

			public string iconname;

			public string symbol;

			public string value;

			public override string ToString()
			{
				return Utils.FormatString("{0} {1} {2}", goodType, symbol, value);
			}
		}

		private List<GoodData> list = new List<GoodData>();

		public int GoodID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public int GoodsType
		{
			get;
			private set;
		}

		public int Ground
		{
			get;
			private set;
		}

		public string DropSound
		{
			get;
			private set;
		}

		public int GetSound
		{
			get;
			private set;
		}

		public int SizeX
		{
			get;
			private set;
		}

		public int SizeY
		{
			get;
			private set;
		}

		public float OffsetX
		{
			get;
			private set;
		}

		public float OffsetY
		{
			get;
			private set;
		}

		public string[] Args
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			GoodID = readInt();
			Notes = readLocalString();
			GoodsType = readInt();
			Ground = readInt();
			DropSound = readLocalString();
			GetSound = readInt();
			SizeX = readInt();
			SizeY = readInt();
			OffsetX = readFloat();
			OffsetY = readFloat();
			Args = readArraystring();
			return true;
		}

		public Goods_goods Copy()
		{
			Goods_goods goods_goods = new Goods_goods();
			goods_goods.GoodID = GoodID;
			goods_goods.Notes = Notes;
			goods_goods.GoodsType = GoodsType;
			goods_goods.Ground = Ground;
			goods_goods.DropSound = DropSound;
			goods_goods.GetSound = GetSound;
			goods_goods.SizeX = SizeX;
			goods_goods.SizeY = SizeY;
			goods_goods.OffsetX = OffsetX;
			goods_goods.OffsetY = OffsetY;
			goods_goods.Args = Args;
			return goods_goods;
		}

		public static GoodShowData GetGoodShowData(string value)
		{
			return GetGoodShowData(GetGoodData(value));
		}

		public static GoodShowData GetGoodShowData(GoodData data)
		{
			GoodShowData goodShowData = new GoodShowData();
			goodShowData.iconname = Utils.FormatString("Attr_{0}", data.goodType);
			string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(goodShowData.iconname);
			string symbol = (data.value <= 0) ? "-" : "+";
			string text = (!data.percent) ? string.Empty : "%";
			goodShowData.goodType = languageByTID;
			goodShowData.symbol = symbol;
			if (data.percent)
			{
				goodShowData.value = Utils.FormatString("{0}{1}", (data.value <= 0) ? ((float)(-data.value) / 100f) : ((float)data.value / 100f), text);
			}
			else
			{
				goodShowData.value = Utils.FormatString("{0}", data.value);
			}
			return goodShowData;
		}

		private void DealGoodsData()
		{
			if (list.Count == 0)
			{
				for (int i = 0; i < Args.Length; i++)
				{
					list.Add(GetGoodData(Args[i]));
				}
			}
		}

		public static GoodData GetGoodData(string str)
		{
			string[] array = str.Split(' ');
			if (array.Length != 3)
			{
				SdkManager.Bugly_Report("Goods_goods_Extra", Utils.FormatString("GetGoodData(string str)[{0}] is invalid, ���\u0736���ո���ٸ��ո�.", str));
			}
			GoodData goodData = new GoodData();
			if (str == string.Empty)
			{
				return goodData;
			}
			goodData.goodType = array[0];
			goodData.percent = array[0].Contains("%");
			if (goodData.percent)
			{
				goodData.value = GetSymbol(array[1]) * (long)(float.Parse(array[2]) * 100f);
			}
			else
			{
				float num = float.Parse(array[2]);
				goodData.value = GetSymbol(array[1]) * (long)num;
			}
			return goodData;
		}

		public static string GoodDataToString(GoodData data)
		{
			return Utils.FormatString("{0} {1} {2}", data.goodType, (data.value <= 0) ? "-" : "+", Mathf.Abs(data.value));
		}

		public static int GetSymbol(string s)
		{
			if (s != null)
			{
				if (s == "+")
				{
					return 1;
				}
				if (s == "-")
				{
					return -1;
				}
			}
			return 0;
		}

		public static void GetAttribute(EntityBase entity, GoodData data)
		{
			entity.m_EntityData.ExcuteAttributes(data);
		}

		public static void GetAttribute(EntityBase entity, string str)
		{
			GoodData goodData = GetGoodData(str);
			GetAttribute(entity, goodData);
		}

		private void DeadGoods(EntityBase entity)
		{
			for (int i = 0; i < list.Count; i++)
			{
				GoodData data = list[i];
				GetAttribute(entity, data);
			}
		}

		public void GetGoods(EntityBase entity)
		{
			if (list.Count == 0)
			{
				DealGoodsData();
			}
			DeadGoods(entity);
		}
	}
}
