




namespace TableTool
{
	public class Box_SilverBoxModel : LocalModel<Box_SilverBox, int>
	{
		private const string _Filename = "Box_SilverBox";

		protected override string Filename => "Box_SilverBox";

		protected override int GetBeanKey(Box_SilverBox bean)
		{
			return bean.ID;
		}
	}
}
