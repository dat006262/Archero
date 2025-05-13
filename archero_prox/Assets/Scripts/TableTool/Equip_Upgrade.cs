




namespace TableTool
{
	public class Equip_Upgrade : LocalBean
	{
		public int LevelId
		{
			get;
			private set;
		}

		public int UpMaterials
		{
			get;
			private set;
		}

		public int UpCoins
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			LevelId = readInt();
			UpMaterials = readInt();
			UpCoins = readInt();
			return true;
		}

		public Equip_Upgrade Copy()
		{
			Equip_Upgrade equip_Upgrade = new Equip_Upgrade();
			equip_Upgrade.LevelId = LevelId;
			equip_Upgrade.UpMaterials = UpMaterials;
			equip_Upgrade.UpCoins = UpCoins;
			return equip_Upgrade;
		}
	}
}
