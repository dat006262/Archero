




public class EntityBoss5021 : EntityBossBase
{
	protected override void StartInit()
	{
		base.StartInit();
		m_AniCtrl.ClearString("Hitted");
	}

	protected override bool GetCanPositionBy()
	{
		return !mAniCtrlBase.GetPlayHittedCallback();
	}
}
