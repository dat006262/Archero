




namespace TableTool
{
	public class Box_ActivityModel : LocalModel<Box_Activity, int>
	{
		private const string _Filename = "Box_Activity";

		protected override string Filename => "Box_Activity";

		protected override int GetBeanKey(Box_Activity bean)
		{
			return bean.ID;
		}
	}
}
