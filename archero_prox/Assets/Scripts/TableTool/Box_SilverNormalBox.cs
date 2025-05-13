




namespace TableTool
{
	public class Box_SilverNormalBox : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int Type
		{
			get;
			private set;
		}

		public int[] Price1
		{
			get;
			private set;
		}

		public int Time
		{
			get;
			private set;
		}

		public int SingleDrop
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Type = readInt();
			Price1 = readArrayint();
			Time = readInt();
			SingleDrop = readInt();
			return true;
		}

		public Box_SilverNormalBox Copy()
		{
			Box_SilverNormalBox box_SilverNormalBox = new Box_SilverNormalBox();
			box_SilverNormalBox.ID = ID;
			box_SilverNormalBox.Type = Type;
			box_SilverNormalBox.Price1 = Price1;
			box_SilverNormalBox.Time = Time;
			box_SilverNormalBox.SingleDrop = SingleDrop;
			return box_SilverNormalBox;
		}
	}
}
