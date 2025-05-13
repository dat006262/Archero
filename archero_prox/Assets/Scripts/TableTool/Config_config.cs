




namespace TableTool
{
	public class Config_config : LocalBean
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

		public float Value
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Notes = readLocalString();
			Value = readFloat();
			return true;
		}

		public Config_config Copy()
		{
			Config_config config_config = new Config_config();
			config_config.ID = ID;
			config_config.Notes = Notes;
			config_config.Value = Value;
			return config_config;
		}
	}
}
