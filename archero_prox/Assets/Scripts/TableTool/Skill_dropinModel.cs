




namespace TableTool
{
	public class Skill_dropinModel : LocalModel<Skill_dropin, int>
	{
		private const string _Filename = "Skill_dropin";

		protected override string Filename => "Skill_dropin";

		protected override int GetBeanKey(Skill_dropin bean)
		{
			return bean.ID;
		}
	}
}
