




namespace TableTool
{
	public class Skill_skill : LocalBean
	{
		public int SkillID
		{
			get;
			private set;
		}

		public int SkillIcon
		{
			get;
			private set;
		}

		public string[] Attributes
		{
			get;
			private set;
		}

		public int[] Effects
		{
			get;
			private set;
		}

		public int[] Buffs
		{
			get;
			private set;
		}

		public int[] Debuffs
		{
			get;
			private set;
		}

		public int LearnEffectID
		{
			get;
			private set;
		}

		public string[] Args
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			SkillID = readInt();
			SkillIcon = readInt();
			Attributes = readArraystring();
			Effects = readArrayint();
			Buffs = readArrayint();
			Debuffs = readArrayint();
			LearnEffectID = readInt();
			Args = readArraystring();
			return true;
		}

		public Skill_skill Copy()
		{
			Skill_skill skill_skill = new Skill_skill();
			skill_skill.SkillID = SkillID;
			skill_skill.SkillIcon = SkillIcon;
			skill_skill.Attributes = Attributes;
			skill_skill.Effects = Effects;
			skill_skill.Buffs = Buffs;
			skill_skill.Debuffs = Debuffs;
			skill_skill.LearnEffectID = LearnEffectID;
			skill_skill.Args = Args;
			return skill_skill;
		}
	}
}
