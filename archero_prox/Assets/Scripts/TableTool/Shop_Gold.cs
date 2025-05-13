




namespace TableTool
{
	public class Shop_Gold : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int Level
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
			Level = readInt();
			Price = readInt();
			return true;
		}

		public Shop_Gold Copy()
		{
			Shop_Gold shop_Gold = new Shop_Gold();
			shop_Gold.ID = ID;
			shop_Gold.Level = Level;
			shop_Gold.Price = Price;
			return shop_Gold;
		}
	}
}
