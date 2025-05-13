




using DG.Tweening;
using Dxx.Util;

public class SkillAlone1082 : SkillAloneBase
{
	private float time;

	private int buffid;

	private SequencePool mSeqPool = new SequencePool();

	protected override void OnInstall()
	{
		if (base.m_SkillData.Args.Length != 2)
		{
			SdkManager.Bugly_Report("SkillAlone1082", Utils.FormatString("SkillID:{0} args.length:{1} != 2", base.m_SkillData.SkillID, base.m_SkillData.Args.Length));
		}
		else if (!float.TryParse(base.m_SkillData.Args[0], out time))
		{
			SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args[0] is not a float type.", base.m_SkillData.SkillID));
		}
		else if (!int.TryParse(base.m_SkillData.Args[1], out buffid))
		{
			SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args[1] is not a int type.", base.m_SkillData.SkillID));
		}
		else
		{
			mSeqPool.Get().AppendInterval(time).AppendCallback(delegate
			{
				GameLogic.SendBuff(m_Entity, buffid);
			})
				.SetLoops(-1);
		}
	}

	protected override void OnUninstall()
	{
		mSeqPool.Clear();
	}
}
