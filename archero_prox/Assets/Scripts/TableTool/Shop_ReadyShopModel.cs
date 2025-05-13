




namespace TableTool
{
	public class Shop_ReadyShopModel : LocalModel<Shop_ReadyShop, int>
	{
		private const string _Filename = "Shop_ReadyShop";

		protected override string Filename => "Shop_ReadyShop";

		protected override int GetBeanKey(Shop_ReadyShop bean)
		{
			return bean.ID;
		}
	}
}
