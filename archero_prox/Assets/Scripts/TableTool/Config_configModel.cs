




using System;

namespace TableTool
{
	public class Config_configModel : LocalModel<Config_config, int>
	{
		private const string _Filename = "Config_config";

		protected override string Filename => "Config_config";

		protected override int GetBeanKey(Config_config bean)
		{
			return bean.ID;
		}

		public T GetValue<T>(int id)
		{
			Config_config beanById = GetBeanById(id);
			if (beanById != null)
			{
				return (T)Convert.ChangeType(beanById.Value, typeof(T));
			}
			return default(T);
		}
	}
}
