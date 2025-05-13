




namespace TableTool
{
	public class Drop_harvest : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int GoldDrop
		{
			get;
			private set;
		}

		public int EquipExp
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			GoldDrop = readInt();
			EquipExp = readInt();
			return true;
		}

		public Drop_harvest Copy()
		{
			Drop_harvest drop_harvest = new Drop_harvest();
			drop_harvest.ID = ID;
			drop_harvest.GoldDrop = GoldDrop;
			drop_harvest.EquipExp = EquipExp;
			return drop_harvest;
		}
	}
}
