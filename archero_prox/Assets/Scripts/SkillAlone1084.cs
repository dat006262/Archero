




using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;

public class SkillAlone1084 : SkillAloneBase
{
	private float time;

	private string att;

	private bool bAddAttribute;

	private Goods_goods.GoodData mAdd;

	private Goods_goods.GoodData mRemove;

	private SequencePool mSeqPool = new SequencePool();

	protected override void OnInstall()
	{
		if (base.m_SkillData.Args.Length != 2)
		{
			SdkManager.Bugly_Report("SkillAlone1084", Utils.FormatString("SkillID:{0} args.length:{1} != 2", base.m_SkillData.SkillID, base.m_SkillData.Args.Length));
			return;
		}
		if (!float.TryParse(base.m_SkillData.Args[0], out time))
		{
			SdkManager.Bugly_Report("SkillAlone1084", Utils.FormatString("SkillID:{0} args[0] is not a float type.", base.m_SkillData.SkillID));
			return;
		}
		if (string.IsNullOrEmpty(base.m_SkillData.Args[1]))
		{
			SdkManager.Bugly_Report("SkillAlone1084", Utils.FormatString("SkillID:{0} args[1] is null.", base.m_SkillData.SkillID));
			return;
		}
		att = base.m_SkillData.Args[1];
		mAdd = Goods_goods.GetGoodData(att);
		mRemove = Goods_goods.GetGoodData(att);
		mRemove.value *= -1L;
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
	}

	private void update_attribute(bool value)
	{
		if (value)
		{
			if (!bAddAttribute)
			{
				bAddAttribute = true;
				m_Entity.m_EntityData.ExcuteAttributes(mAdd);
			}
		}
		else if (bAddAttribute)
		{
			bAddAttribute = false;
			m_Entity.m_EntityData.ExcuteAttributes(mRemove);
		}
	}

	protected override void OnUninstall()
	{
		mSeqPool.Clear();
		update_attribute(value: false);
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
	}

	private void OnGotoNextRoom(RoomGenerateBase.Room room)
	{
		update_attribute(value: true);
		mSeqPool.Get().AppendInterval(time).AppendCallback(delegate
		{
			update_attribute(value: false);
		});
	}
}
