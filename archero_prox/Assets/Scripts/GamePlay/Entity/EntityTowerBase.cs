




public class EntityTowerBase : EntityCallBase
{
	protected override void OnInit()
	{
		base.OnInit();
		m_MoveCtrl = new MoveControl();
		m_AttackCtrl = new AttackControl();
		m_MoveCtrl.Init(this);
		m_AttackCtrl.Init(this);
		GameLogic.Release.Entity.AddTower(this);
		SetCollider(enable: false);
		m_AttackCtrl.SetRotate(0f);
	}

	protected override void StartInit()
	{
		base.StartInit();
	}

	protected override void OnDeInitLogic()
	{
		base.OnDeInitLogic();
	}

	protected override void OnCreateModel()
	{
		base.OnCreateModel();
		ShowHP(show: false);
	}

	protected override long GetBossHP()
	{
		return m_EntityData.MaxHP;
	}
}
