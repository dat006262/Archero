




namespace TableTool
{
	public class Skill_dropin : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int Weight
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Weight = readInt();
			return true;
		}

		public Skill_dropin Copy()
		{
			Skill_dropin skill_dropin = new Skill_dropin();
			skill_dropin.ID = ID;
			skill_dropin.Weight = Weight;
			return skill_dropin;
		}
	}
}
