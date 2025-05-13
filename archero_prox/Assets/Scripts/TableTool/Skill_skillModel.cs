




namespace TableTool
{
	public class Skill_skillModel : LocalModel<Skill_skill, int>
	{
		private const string _Filename = "Skill_skill";

		protected override string Filename => "Skill_skill";

		protected override int GetBeanKey(Skill_skill bean)
		{
			return bean.SkillID;
		}
	}
}
