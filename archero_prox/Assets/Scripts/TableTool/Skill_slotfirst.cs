




namespace TableTool
{
	public class Skill_slotfirst : LocalBean
	{
		public int SkillID
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			SkillID = readInt();
			return true;
		}

		public Skill_slotfirst Copy()
		{
			Skill_slotfirst skill_slotfirst = new Skill_slotfirst();
			skill_slotfirst.SkillID = SkillID;
			return skill_slotfirst;
		}
	}
}
