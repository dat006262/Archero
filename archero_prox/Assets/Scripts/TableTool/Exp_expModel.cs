




namespace TableTool
{
	public class Exp_expModel : LocalModel<Exp_exp, int>
	{
		private const string _Filename = "Exp_exp";

		protected override string Filename => "Exp_exp";

		protected override int GetBeanKey(Exp_exp bean)
		{
			return bean.LevelID;
		}
	}
}
