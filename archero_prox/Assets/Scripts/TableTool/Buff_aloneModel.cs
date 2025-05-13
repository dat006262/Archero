




namespace TableTool
{
	public class Buff_aloneModel : LocalModel<Buff_alone, int>
	{
		private const string _Filename = "Buff_alone";

		protected override string Filename => "Buff_alone";

		protected override int GetBeanKey(Buff_alone bean)
		{
			return bean.BuffID;
		}
	}
}
