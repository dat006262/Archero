




namespace TableTool
{
	public class Goods_waterModel : LocalModel<Goods_water, string>
	{
		private const string _Filename = "Goods_water";

		protected override string Filename => "Goods_water";

		protected override string GetBeanKey(Goods_water bean)
		{
			return bean.CheckID;
		}
	}
}
