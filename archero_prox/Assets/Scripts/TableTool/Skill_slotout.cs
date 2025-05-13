




namespace TableTool
{
	public class Skill_slotout : LocalBean
	{
		public int GroupID
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

		public string[] BaseAttributes
		{
			get;
			private set;
		}

		public float[] AddAttributes
		{
			get;
			private set;
		}

		public int LevelLimit
		{
			get;
			private set;
		}

		public string InitialPower
		{
			get;
			private set;
		}

		public string AddPower
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			GroupID = readInt();
			Type = readInt();
			Quality = readInt();
			BaseAttributes = readArraystring();
			AddAttributes = readArrayfloat();
			LevelLimit = readInt();
			InitialPower = readLocalString();
			AddPower = readLocalString();
			return true;
		}

		public Skill_slotout Copy()
		{
			Skill_slotout skill_slotout = new Skill_slotout();
			skill_slotout.GroupID = GroupID;
			skill_slotout.Type = Type;
			skill_slotout.Quality = Quality;
			skill_slotout.BaseAttributes = BaseAttributes;
			skill_slotout.AddAttributes = AddAttributes;
			skill_slotout.LevelLimit = LevelLimit;
			skill_slotout.InitialPower = InitialPower;
			skill_slotout.AddPower = AddPower;
			return skill_slotout;
		}
	}
}
