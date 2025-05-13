




namespace TableTool
{
	public class Box_TimeBox : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int Time
		{
			get;
			private set;
		}

		public int DropId
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Time = readInt();
			DropId = readInt();
			return true;
		}

		public Box_TimeBox Copy()
		{
			Box_TimeBox box_TimeBox = new Box_TimeBox();
			box_TimeBox.ID = ID;
			box_TimeBox.Time = Time;
			box_TimeBox.DropId = DropId;
			return box_TimeBox;
		}
	}
}
