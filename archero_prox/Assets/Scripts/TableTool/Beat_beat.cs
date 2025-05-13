




namespace TableTool
{
	public class Beat_beat : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public int Score
		{
			get;
			private set;
		}

		public float Rate
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Score = readInt();
			Rate = readFloat();
			return true;
		}

		public Beat_beat Copy()
		{
			Beat_beat beat_beat = new Beat_beat();
			beat_beat.ID = ID;
			beat_beat.Score = Score;
			beat_beat.Rate = Rate;
			return beat_beat;
		}
	}
}
