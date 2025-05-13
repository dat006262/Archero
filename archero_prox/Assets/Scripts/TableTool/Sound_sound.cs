




namespace TableTool
{
	public class Sound_sound : LocalBean
	{
		public int ID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public string Path
		{
			get;
			private set;
		}

		public float Volumn
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Notes = readLocalString();
			Path = readLocalString();
			Volumn = readFloat();
			return true;
		}

		public Sound_sound Copy()
		{
			Sound_sound sound_sound = new Sound_sound();
			sound_sound.ID = ID;
			sound_sound.Notes = Notes;
			sound_sound.Path = Path;
			sound_sound.Volumn = Volumn;
			return sound_sound;
		}
	}
}
