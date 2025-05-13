




namespace TableTool
{
	public class Goods_water : LocalBean
	{
		public string CheckID
		{
			get;
			private set;
		}

		public int[] WaterID
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			CheckID = readLocalString();
			WaterID = readArrayint();
			return true;
		}

		public Goods_water Copy()
		{
			Goods_water goods_water = new Goods_water();
			goods_water.CheckID = CheckID;
			goods_water.WaterID = WaterID;
			return goods_water;
		}
	}
}
