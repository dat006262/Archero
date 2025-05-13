




namespace TableTool
{
	public class Drop_FakeDropModel : LocalModel<Drop_FakeDrop, int>
	{
		private const string _Filename = "Drop_FakeDrop";

		protected override string Filename => "Drop_FakeDrop";

		protected override int GetBeanKey(Drop_FakeDrop bean)
		{
			return bean.ID;
		}
	}
}
