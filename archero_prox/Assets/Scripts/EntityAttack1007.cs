




using Dxx.Util;

public class EntityAttack1007 : EntityAttackBase
{
	private ActionBattle action = new ActionBattle();

	private bool bInstall;

	protected float delaytime = 0.65f;

	private RedLinesCtrl mRedLinesCtrl;

	protected virtual float linetimemin => 1f;

	protected virtual float linetimemax => 1f;

	protected virtual int count => 1;

	protected virtual float perangle => 0f;

	protected virtual int ReboundCount => 0;

	protected virtual bool ThroughWall => false;

	protected override void OnInit()
	{
		m_Entity.ChangeWeapon(AttackID);
		mRedLinesCtrl = new RedLinesCtrl();
		mRedLinesCtrl.Init(m_Entity, ThroughWall, ReboundCount, count, perangle);
		action.Init(m_Entity);
		string skillname = m_Entity.m_AniCtrl.GetAnimationValue("Skill");
		m_Entity.m_AniCtrl.SetString("Skill", "AttackPrevReady");
		m_Entity.m_AniCtrl.SendEvent("Skill");
		SetIsEnd(isend: false);
		action.AddActionWait(GameLogic.Random(linetimemax, linetimemax));
		action.AddActionDelegate(delegate
		{
			m_Entity.m_AniCtrl.SetString("Skill", skillname);
		});
		action.AddActionDelegate(AttackEnd);
		bInstall = true;
		OnUnInstall = OnUnInstalls;
	}

	protected override void DeInit()
	{
		RedLineDeInit();
	}

	private void RedLineDeInit()
	{
		if (mRedLinesCtrl != null)
		{
			mRedLinesCtrl.DeInit();
			mRedLinesCtrl = null;
		}
	}

	public override void SetData(object[] args)
	{
	}

	private void AttackStart()
	{
		m_Entity.m_AttackCtrl.OnMoveStart(m_AttackData);
		m_Entity.m_Weapon.SetTarget(m_Entity.m_HatredTarget);
	}

	protected override void UpdateProcess(float delta)
	{
		base.UpdateProcess(delta);
		delaytime -= Updater.delta;
		if (delaytime > 0f)
		{
			UpdateAttackAngle();
			m_Entity.m_AttackCtrl.OnMoving(m_AttackData);
			if (mRedLinesCtrl != null)
			{
				mRedLinesCtrl.Update();
			}
		}
		else
		{
			RedLineDeInit();
		}
	}

	private void AttackEnd()
	{
		SetIsEnd(isend: true);
		AttackStart();
		UnInstall();
	}

	public override void Install()
	{
	}

	private void OnUnInstalls()
	{
		action.DeInit();
		if (bInstall)
		{
			bInstall = false;
			if (m_Entity.m_AttackCtrl != null)
			{
				m_Entity.m_AttackCtrl.OnMoveEnd(m_AttackData);
			}
			m_Entity.m_AniCtrl.SendEvent("Idle");
			RedLineDeInit();
		}
	}
}
