




using Dxx.Util;

public class PlayerPrefsMgr : CInstance<PlayerPrefsMgr>
{
	public abstract class PrefDataBase
	{
		protected string name
		{
			get;
			private set;
		}

		public PrefDataBase(string name)
		{
			this.name = Utils.FormatString("PlayerPrefsMgr_{0}", name);
		}

		public void flush()
		{
			OnFlush();
		}

		protected abstract void OnFlush();
	}

	public class PrefDataInt : PrefDataBase
	{
		private int value;

		public PrefDataInt(string name)
			: base(name)
		{
			value = PlayerPrefsEncrypt.GetInt(base.name);
		}

		public int get_value()
		{
			return value;
		}

		public void set_value(int t)
		{
			value = t;
		}

		protected override void OnFlush()
		{
			PlayerPrefsEncrypt.SetInt(base.name, value);
		}
	}

	public PrefDataInt gametime = new PrefDataInt("gametime");

	public PrefDataInt apptime = new PrefDataInt("apptime");

	public void Init()
	{
	}
}
