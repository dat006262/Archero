




namespace TableTool
{
	public class Box_SilverNormalBoxModel : LocalModel<Box_SilverNormalBox, int>
	{
		private const string _Filename = "Box_SilverNormalBox";

		protected override string Filename => "Box_SilverNormalBox";

		protected override int GetBeanKey(Box_SilverNormalBox bean)
		{
			return bean.ID;
		}
	}
}
