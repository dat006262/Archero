




namespace TableTool
{
	public class Box_TimeBoxModel : LocalModel<Box_TimeBox, int>
	{
		private const string _Filename = "Box_TimeBox";

		protected override string Filename => "Box_TimeBox";

		protected override int GetBeanKey(Box_TimeBox bean)
		{
			return bean.ID;
		}
	}
}
