




namespace TableTool
{
	public class Soldier_soldier : LocalBean
	{
		public int CharID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public int GoldDropLevel
		{
			get;
			private set;
		}

		public int ScrollDropLevel
		{
			get;
			private set;
		}

		public int GoldDropGold1
		{
			get;
			private set;
		}

		public int GoldDropGold2
		{
			get;
			private set;
		}

		public float EquipRate
		{
			get;
			private set;
		}

		public int Exp
		{
			get;
			private set;
		}

		public int DropRadius
		{
			get;
			private set;
		}

		public int HPDrop1
		{
			get;
			private set;
		}

		public int HPDrop2
		{
			get;
			private set;
		}

		public int HPDrop3
		{
			get;
			private set;
		}

		public int BodyHitSoundID
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			CharID = readInt();
			Notes = readLocalString();
			GoldDropLevel = readInt();
			ScrollDropLevel = readInt();
			GoldDropGold1 = readInt();
			GoldDropGold2 = readInt();
			EquipRate = readFloat();
			Exp = readInt();
			DropRadius = readInt();
			HPDrop1 = readInt();
			HPDrop2 = readInt();
			HPDrop3 = readInt();
			BodyHitSoundID = readInt();
			return true;
		}

		public Soldier_soldier Copy()
		{
			Soldier_soldier soldier_soldier = new Soldier_soldier();
			soldier_soldier.CharID = CharID;
			soldier_soldier.Notes = Notes;
			soldier_soldier.GoldDropLevel = GoldDropLevel;
			soldier_soldier.ScrollDropLevel = ScrollDropLevel;
			soldier_soldier.GoldDropGold1 = GoldDropGold1;
			soldier_soldier.GoldDropGold2 = GoldDropGold2;
			soldier_soldier.EquipRate = EquipRate;
			soldier_soldier.Exp = Exp;
			soldier_soldier.DropRadius = DropRadius;
			soldier_soldier.HPDrop1 = HPDrop1;
			soldier_soldier.HPDrop2 = HPDrop2;
			soldier_soldier.HPDrop3 = HPDrop3;
			soldier_soldier.BodyHitSoundID = BodyHitSoundID;
			return soldier_soldier;
		}
	}
}
