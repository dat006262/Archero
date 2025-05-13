




namespace TableTool
{
	public class Shop_ReadyShop : LocalBean
	{
		public int ID
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

		protected override bool ReadImpl()
		{
			ID = readInt();
			ProductType = readInt();
			ProductId = readInt();
			ProductNum = readInt();
			PriceType = readInt();
			Price = readInt();
			return true;
		}

		public Shop_ReadyShop Copy()
		{
			Shop_ReadyShop shop_ReadyShop = new Shop_ReadyShop();
			shop_ReadyShop.ID = ID;
			shop_ReadyShop.ProductType = ProductType;
			shop_ReadyShop.ProductId = ProductId;
			shop_ReadyShop.ProductNum = ProductNum;
			shop_ReadyShop.PriceType = PriceType;
			shop_ReadyShop.Price = Price;
			return shop_ReadyShop;
		}
	}
}
