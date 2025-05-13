




namespace TableTool
{
	public class Character_Level : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int Exp
		{
			get;
			private set;
		}

		public string[] Rewards
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Exp = readInt();
			Rewards = readArraystring();
			return true;
		}

		public Character_Level Copy()
		{
			Character_Level character_Level = new Character_Level();
			character_Level.ID = ID;
			character_Level.Exp = Exp;
			character_Level.Rewards = Rewards;
			return character_Level;
		}
	}
}
