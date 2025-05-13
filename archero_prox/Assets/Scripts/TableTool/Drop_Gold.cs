




namespace TableTool
{
	public class Drop_Gold : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public string[] GoldDropLevel
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			GoldDropLevel = readArraystring();
			return true;
		}

		public Drop_Gold Copy()
		{
			Drop_Gold drop_Gold = new Drop_Gold();
			drop_Gold.ID = ID;
			drop_Gold.GoldDropLevel = GoldDropLevel;
			return drop_Gold;
		}
	}
}
