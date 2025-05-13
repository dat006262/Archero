




namespace TableTool
{
	public class Character_Baby : LocalBean
	{
		public string BabyID
		{
			get;
			private set;
		}

		public int AttackValue
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			BabyID = readLocalString();
			AttackValue = readInt();
			return true;
		}

		public Character_Baby Copy()
		{
			Character_Baby character_Baby = new Character_Baby();
			character_Baby.BabyID = BabyID;
			character_Baby.AttackValue = AttackValue;
			return character_Baby;
		}
	}
}
