




namespace TableTool
{
	public class Shop_MysticShopShow : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int ShowProb
		{
			get;
			private set;
		}

		public int AddProb
		{
			get;
			private set;
		}

		public int[] ShowRoom
		{
			get;
			private set;
		}

		public int[] ShopTypeProb
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			ShowProb = readInt();
			AddProb = readInt();
			ShowRoom = readArrayint();
			ShopTypeProb = readArrayint();
			return true;
		}

		public Shop_MysticShopShow Copy()
		{
			Shop_MysticShopShow shop_MysticShopShow = new Shop_MysticShopShow();
			shop_MysticShopShow.ID = ID;
			shop_MysticShopShow.ShowProb = ShowProb;
			shop_MysticShopShow.AddProb = AddProb;
			shop_MysticShopShow.ShowRoom = ShowRoom;
			shop_MysticShopShow.ShopTypeProb = ShopTypeProb;
			return shop_MysticShopShow;
		}
	}
}
