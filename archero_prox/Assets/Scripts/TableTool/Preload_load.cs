




namespace TableTool
{
	public class Preload_load : LocalBean
	{
		public int RoomID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public string[] PlayerBulletsPath
		{
			get;
			private set;
		}

		public string[] BulletsPath
		{
			get;
			private set;
		}

		public string[] EffectsPath
		{
			get;
			private set;
		}

		public string[] MapEffectsPath
		{
			get;
			private set;
		}

		public string[] GoodsPath
		{
			get;
			private set;
		}

		public int[] SoundPath
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			RoomID = readInt();
			Notes = readLocalString();
			PlayerBulletsPath = readArraystring();
			BulletsPath = readArraystring();
			EffectsPath = readArraystring();
			MapEffectsPath = readArraystring();
			GoodsPath = readArraystring();
			SoundPath = readArrayint();
			return true;
		}

		public Preload_load Copy()
		{
			Preload_load preload_load = new Preload_load();
			preload_load.RoomID = RoomID;
			preload_load.Notes = Notes;
			preload_load.PlayerBulletsPath = PlayerBulletsPath;
			preload_load.BulletsPath = BulletsPath;
			preload_load.EffectsPath = EffectsPath;
			preload_load.MapEffectsPath = MapEffectsPath;
			preload_load.GoodsPath = GoodsPath;
			preload_load.SoundPath = SoundPath;
			return preload_load;
		}
	}
}
