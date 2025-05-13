




using Dxx.Util;

public class SkillMastery101 : SkillMasteryBase
{
	private int wearweapontype;

	protected override void OnInit()
	{
		Split();
	}

	private void Split()
	{
		Debugger.Log("武器精通");
		LocalSave.EquipOne equipOne = null;
		if (equipOne == null)
		{
			return;
		}
		wearweapontype = equipOne.data.Type;
		string[] array = mData.Split(',');
		if (array.Length != 2)
		{
			SdkManager.Bugly_Report(GetType().ToString(), Utils.FormatString("{0} is invalid.", mData));
			return;
		}
		int num = int.Parse(array[0]);
		string text = array[1];
		if (num == wearweapontype)
		{
			Debugger.Log("武器类型相同 " + num + " 增加属性 " + text);
			m_Entity.m_EntityData.ExcuteAttributes(text);
		}
	}
}
