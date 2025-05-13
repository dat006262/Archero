




using UnityEngine;

public class AIMove1033 : AIMoveBase
{
	private EntityBase target;

	private int range;

	private float maxdis;

	private bool move2target;

	private bool bExcuteShow;

	private Vector3 endpos;

	private ActionBattle action;

	private ConditionTime mCondition;

	public AIMove1033(EntityBase entity, float maxdis, int range, bool move2target)
		: base(entity)
	{
		this.range = range;
		this.maxdis = maxdis;
		this.move2target = move2target;
		target = GameLogic.Self;
	}

	protected override void OnInitBase()
	{
		bExcuteShow = false;
		if ((target.position - m_Entity.position).magnitude < maxdis)
		{
			End();
			return;
		}
		m_Entity.m_AniCtrl.SetString("Skill", "MoveMiss");
		m_Entity.m_AniCtrl.SendEvent("Skill");
		KillAction();
		action = new ActionBattle();
		action.Init(m_Entity);
		action.AddActionWaitDelegate(0.5f, delegate
		{
			m_Entity.PlayEffect(3100018);
		});
		action.AddActionWaitDelegate(0.4f, delegate
		{
			Show(show: false);
		});
		action.AddActionWaitDelegate(0.6f, delegate
		{
			float endx = 0f;
			float endz = 0f;
			if (move2target)
			{
				GameLogic.Release.MapCreatorCtrl.RandomItemSide(GameLogic.Self, range, out endx, out endz);
			}
			else
			{
				Vector3 vector = GameLogic.Release.MapCreatorCtrl.RandomPosition();
				endx = vector.x;
				endz = vector.z;
			}
			endpos = new Vector3(endx, 0f, endz);
		});
		action.AddActionWaitDelegate(0.4f, delegate
		{
			m_Entity.SetPosition(endpos);
			Show(show: true);
			m_Entity.m_AniCtrl.SetString("Skill", "MoveShow");
			m_Entity.m_AniCtrl.SendEvent("Skill");
			m_Entity.PlayEffect(3100018);
		});
		mCondition = new ConditionTime
		{
			time = 2.5f
		};
	}

	protected override void OnUpdate()
	{
		if (mCondition != null && mCondition.IsEnd())
		{
			mCondition = null;
			End();
		}
	}

	protected override void OnEnd()
	{
		if (bExcuteShow)
		{
			Show(show: true);
		}
		KillAction();
	}

	private void KillAction()
	{
		if (action != null)
		{
			action.DeInit();
			action = null;
		}
	}

	private void Show(bool show)
	{
		bExcuteShow = !show;
		m_Entity.ShowEntity(show);
	}
}
