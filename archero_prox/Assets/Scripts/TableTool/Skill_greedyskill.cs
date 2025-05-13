




namespace TableTool
{
	public class Skill_greedyskill : LocalBean
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

		public int Type
		{
			get;
			private set;
		}

		public int Weight
		{
			get;
			private set;
		}

		public int Gold
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			SkillID = readInt();
			Notes = readLocalString();
			Type = readInt();
			Weight = readInt();
			Gold = readInt();
			return true;
		}

		public Skill_greedyskill Copy()
		{
			Skill_greedyskill skill_greedyskill = new Skill_greedyskill();
			skill_greedyskill.SkillID = SkillID;
			skill_greedyskill.Notes = Notes;
			skill_greedyskill.Type = Type;
			skill_greedyskill.Weight = Weight;
			skill_greedyskill.Gold = Gold;
			return skill_greedyskill;
		}
	}
}
