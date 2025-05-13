




namespace TableTool
{
	public class Box_Activity : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public float PayId
		{
			get;
			private set;
		}

		public string[] ShowCond
		{
			get;
			private set;
		}

		public string[] CloseCond
		{
			get;
			private set;
		}

		public string[] Reward
		{
			get;
			private set;
		}

		public int Multiple
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			PayId = readFloat();
			ShowCond = readArraystring();
			CloseCond = readArraystring();
			Reward = readArraystring();
			Multiple = readInt();
			return true;
		}

		public Box_Activity Copy()
		{
			Box_Activity box_Activity = new Box_Activity();
			box_Activity.ID = ID;
			box_Activity.PayId = PayId;
			box_Activity.ShowCond = ShowCond;
			box_Activity.CloseCond = CloseCond;
			box_Activity.Reward = Reward;
			box_Activity.Multiple = Multiple;
			return box_Activity;
		}
	}
}
