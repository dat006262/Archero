




namespace TableTool
{
	public class Shop_Shop : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int ShopType
		{
			get;
			private set;
		}

		public int[] ShowCond
		{
			get;
			private set;
		}

		public int[] CloseCond
		{
			get;
			private set;
		}

		public int Position
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

		public float Price
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			ShopType = readInt();
			ShowCond = readArrayint();
			CloseCond = readArrayint();
			Position = readInt();
			ProductType = readInt();
			ProductId = readInt();
			ProductNum = readInt();
			PriceType = readInt();
			Price = readFloat();
			return true;
		}

		public Shop_Shop Copy()
		{
			Shop_Shop shop_Shop = new Shop_Shop();
			shop_Shop.ID = ID;
			shop_Shop.ShopType = ShopType;
			shop_Shop.ShowCond = ShowCond;
			shop_Shop.CloseCond = CloseCond;
			shop_Shop.Position = Position;
			shop_Shop.ProductType = ProductType;
			shop_Shop.ProductId = ProductId;
			shop_Shop.ProductNum = ProductNum;
			shop_Shop.PriceType = PriceType;
			shop_Shop.Price = Price;
			return shop_Shop;
		}
	}
}
