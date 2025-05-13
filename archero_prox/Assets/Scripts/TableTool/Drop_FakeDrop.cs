




namespace TableTool
{
	public class Drop_FakeDrop : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int DropID
		{
			get;
			private set;
		}

		public int RandNum
		{
			get;
			private set;
		}

		public int JumpDrop
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			DropID = readInt();
			RandNum = readInt();
			JumpDrop = readInt();
			return true;
		}

		public Drop_FakeDrop Copy()
		{
			Drop_FakeDrop drop_FakeDrop = new Drop_FakeDrop();
			drop_FakeDrop.ID = ID;
			drop_FakeDrop.DropID = DropID;
			drop_FakeDrop.RandNum = RandNum;
			drop_FakeDrop.JumpDrop = JumpDrop;
			return drop_FakeDrop;
		}
	}
}
