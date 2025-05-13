




namespace TableTool
{
	public class Box_ChapterBox : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int Chapter
		{
			get;
			private set;
		}

		public string[] Reward
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Chapter = readInt();
			Reward = readArraystring();
			return true;
		}

		public Box_ChapterBox Copy()
		{
			Box_ChapterBox box_ChapterBox = new Box_ChapterBox();
			box_ChapterBox.ID = ID;
			box_ChapterBox.Chapter = Chapter;
			box_ChapterBox.Reward = Reward;
			return box_ChapterBox;
		}
	}
}
