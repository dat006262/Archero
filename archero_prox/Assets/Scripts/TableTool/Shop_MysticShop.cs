




namespace TableTool
{
	public class Shop_MysticShop : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int[] Stage
		{
			get;
			private set;
		}

		public int ShopType
		{
			get;
			private set;
		}

		public int[] Position
		{
			get;
			private set;
		}

		public int ProductType
		{
			get;
			private set;
		}

		public int ProductId
		{
			get;
			private set;
		}

		public int ProductNum
		{
			get;
			private set;
		}

		public int PriceType
		{
			get;
			private set;
		}

		public int Price
		{
			get;
			private set;
		}

		public int Weights
		{
			get;
			private set;
		}

		public int AdProb
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Stage = readArrayint();
			ShopType = readInt();
			Position = readArrayint();
			ProductType = readInt();
			ProductId = readInt();
			ProductNum = readInt();
			PriceType = readInt();
			Price = readInt();
			Weights = readInt();
			AdProb = readInt();
			return true;
		}

		public Shop_MysticShop Copy()
		{
			Shop_MysticShop shop_MysticShop = new Shop_MysticShop();
			shop_MysticShop.ID = ID;
			shop_MysticShop.Stage = Stage;
			shop_MysticShop.ShopType = ShopType;
			shop_MysticShop.Position = Position;
			shop_MysticShop.ProductType = ProductType;
			shop_MysticShop.ProductId = ProductId;
			shop_MysticShop.ProductNum = ProductNum;
			shop_MysticShop.PriceType = PriceType;
			shop_MysticShop.Price = Price;
			shop_MysticShop.Weights = Weights;
			shop_MysticShop.AdProb = AdProb;
			return shop_MysticShop;
		}
	}
}
