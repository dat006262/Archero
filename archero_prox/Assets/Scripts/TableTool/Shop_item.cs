




namespace TableTool
{
	public class Shop_item : LocalBean
	{
		public int ItemID
		{
			get;
			private set;
		}

		public int Type
		{
			get;
			private set;
		}

		public int Quality
		{
			get;
			private set;
		}

		public int EffectType
		{
			get;
			private set;
		}

		public string[] EffectArgs
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ItemID = readInt();
			Type = readInt();
			Quality = readInt();
			EffectType = readInt();
			EffectArgs = readArraystring();
			return true;
		}

		public Shop_item Copy()
		{
			Shop_item shop_item = new Shop_item();
			shop_item.ItemID = ItemID;
			shop_item.Type = Type;
			shop_item.Quality = Quality;
			shop_item.EffectType = EffectType;
			shop_item.EffectArgs = EffectArgs;
			return shop_item;
		}
	}
}
