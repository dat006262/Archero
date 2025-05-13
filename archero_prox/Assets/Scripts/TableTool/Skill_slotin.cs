




namespace TableTool
{
	public class Skill_slotin : LocalBean
	{
		public int SkillID
		{
			get;
			private set;
		}

		public int Weight
		{
			get;
			private set;
		}

		public int UnlockStage
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			SkillID = readInt();
			Weight = readInt();
			UnlockStage = readInt();
			return true;
		}

		public Skill_slotin Copy()
		{
			Skill_slotin skill_slotin = new Skill_slotin();
			skill_slotin.SkillID = SkillID;
			skill_slotin.Weight = Weight;
			skill_slotin.UnlockStage = UnlockStage;
			return skill_slotin;
		}
	}
}
