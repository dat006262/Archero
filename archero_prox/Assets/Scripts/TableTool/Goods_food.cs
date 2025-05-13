




using System.Collections.Generic;

namespace TableTool
{
	public class Goods_food : LocalBean
	{
		private List<Goods_goods.GoodData> list = new List<Goods_goods.GoodData>();

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

		public string[] Values
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			GoodID = readInt();
			Notes = readLocalString();
			DropSound = readLocalString();
			GetSound = readInt();
			Values = readArraystring();
			return true;
		}

		public Goods_food Copy()
		{
			Goods_food goods_food = new Goods_food();
			goods_food.GoodID = GoodID;
			goods_food.Notes = Notes;
			goods_food.DropSound = DropSound;
			goods_food.GetSound = GetSound;
			goods_food.Values = Values;
			return goods_food;
		}

		private void DealGoodsData()
		{
			if (list.Count == 0)
			{
				for (int i = 0; i < Values.Length; i++)
				{
					list.Add(Goods_goods.GetGoodData(Values[i]));
				}
			}
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

		public static void GetAttribute(EntityBase entity, Goods_goods.GoodData data)
		{
			entity.m_EntityData.ExcuteAttributes(data);
		}

		public static void GetAttribute(EntityBase entity, string str)
		{
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
			GetAttribute(entity, goodData);
		}

		private void DeadGoods(EntityBase entity)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Goods_goods.GoodData data = list[i];
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
