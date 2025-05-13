




namespace TableTool
{
	public class Drop_harvestModel : LocalModel<Drop_harvest, int>
	{
		private const string _Filename = "Drop_harvest";

		protected override string Filename => "Drop_harvest";

		protected override int GetBeanKey(Drop_harvest bean)
		{
			return bean.ID;
		}
	}
}
