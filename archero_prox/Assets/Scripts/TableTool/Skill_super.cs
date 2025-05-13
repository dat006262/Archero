




namespace TableTool
{
	public class Skill_super : LocalBean
	{
		public int SkillID
		{
			get;
			private set;
		}

		public float CD
		{
			get;
			private set;
		}

		public float[] Args
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			SkillID = readInt();
			CD = readFloat();
			Args = readArrayfloat();
			return true;
		}

		public Skill_super Copy()
		{
			Skill_super skill_super = new Skill_super();
			skill_super.SkillID = SkillID;
			skill_super.CD = CD;
			skill_super.Args = Args;
			return skill_super;
		}
	}
}
