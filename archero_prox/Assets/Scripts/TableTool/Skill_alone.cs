




namespace TableTool
{
	public class Skill_alone : LocalBean
	{
		public int SkillID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public string[] Attributes
		{
			get;
			private set;
		}

		public int[] DeBuffs
		{
			get;
			private set;
		}

		public int CreateEffectID
		{
			get;
			private set;
		}

		public string[] Args
		{
			get;
			private set;
		}

		public string ArgsNote
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			SkillID = readInt();
			Notes = readLocalString();
			Attributes = readArraystring();
			DeBuffs = readArrayint();
			CreateEffectID = readInt();
			Args = readArraystring();
			ArgsNote = readLocalString();
			return true;
		}

		public Skill_alone Copy()
		{
			Skill_alone skill_alone = new Skill_alone();
			skill_alone.SkillID = SkillID;
			skill_alone.Notes = Notes;
			skill_alone.Attributes = Attributes;
			skill_alone.DeBuffs = DeBuffs;
			skill_alone.CreateEffectID = CreateEffectID;
			skill_alone.Args = Args;
			skill_alone.ArgsNote = ArgsNote;
			return skill_alone;
		}
	}
}
