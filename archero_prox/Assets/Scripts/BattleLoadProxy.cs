




using PureMVC.Patterns;
using System;

public class BattleLoadProxy : Proxy
{
	public enum LoadingType
	{
		eMiss,
		eFirstBattle
	}

	public class BattleLoadData
	{
		public Action LoadingDo;

		public Action LoadEnd1Do;

		public Action LoadEnd2Do;

		public LoadingType loadingType;

		public bool showLoading => loadingType == LoadingType.eFirstBattle;
	}

	public new const string NAME = "BattleLoadProxy";

	public BattleLoadProxy(object data)
		: base("BattleLoadProxy", data)
	{
	}
}
