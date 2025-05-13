




namespace TableTool
{
	public class Box_SilverBox : LocalBean
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

		public int Price10
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

		public int GiftDrop
		{
			get;
			private set;
		}

		public int PurpleDrop
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Type = readInt();
			Price1 = readArrayint();
			Price10 = readInt();
			Time = readInt();
			SingleDrop = readInt();
			GiftDrop = readInt();
			PurpleDrop = readInt();
			return true;
		}

		public Box_SilverBox Copy()
		{
			Box_SilverBox box_SilverBox = new Box_SilverBox();
			box_SilverBox.ID = ID;
			box_SilverBox.Type = Type;
			box_SilverBox.Price1 = Price1;
			box_SilverBox.Price10 = Price10;
			box_SilverBox.Time = Time;
			box_SilverBox.SingleDrop = SingleDrop;
			box_SilverBox.GiftDrop = GiftDrop;
			box_SilverBox.PurpleDrop = PurpleDrop;
			return box_SilverBox;
		}
	}
}
