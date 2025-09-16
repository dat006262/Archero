




using PureMVC.Patterns;
using System;
using TableTool;

public class BoxOpenSingleProxy : Proxy
{
	public class Transfer
	{
		public EquipSource source;

		public LocalSave.TimeBoxType boxtype;

		public Drop_DropModel.DropData data;

		public int[] diamonds;

		public int count = 1;

		public Action retry_callback;

		public int GetCurrentDiamond()
		{
			if (diamonds.Length > 0)
			{
				string arg = string.Empty;
				for (int i = 0; i < diamonds.Length; i++)
				{
					arg = arg + diamonds[i] + ",";
				}
				if (count < diamonds.Length)
				{
					return diamonds[count];
				}
				return diamonds[diamonds.Length - 1];
			}
			SdkManager.Bugly_Report("BoxOpenSingleProxy", "diamonds is null");
			return 0;
		}

		public int GetStartDiamond()
		{
			if (diamonds.Length > 0)
			{
				return diamonds[0];
			}
			SdkManager.Bugly_Report("BoxOpenSingleProxy", "diamonds is null");
			return 0;
		}

		public void AddCount()
		{
			count++;
		}

		public void ResetCount()
		{
			count = 0;
		}
	}

	public new const string NAME = "BoxOpenSingleProxy";

	public BoxOpenSingleProxy(object data)
		: base("BoxOpenSingleProxy", data)
	{
	}
}
