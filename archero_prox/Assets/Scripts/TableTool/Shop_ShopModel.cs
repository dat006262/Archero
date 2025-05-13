




namespace TableTool
{
	public class Shop_ShopModel : LocalModel<Shop_Shop, int>
	{
		private const string _Filename = "Shop_Shop";

		protected override string Filename => "Shop_Shop";

		protected override int GetBeanKey(Shop_Shop bean)
		{
			return bean.ID;
		}

		public int get_buy_gold_diamond(int index)
		{
			if (index < 0 || index >= 3)
			{
				return 0;
			}
			return (int)GetBeanById(101 + index).Price;
		}
	}
}
