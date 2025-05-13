




using System;

public class ChallengeCondition201 : ChallengeConditionBase
{
	protected override void OnInit()
	{
	}

	protected override void OnStart()
	{
		if ((bool)GameLogic.Self)
		{
			EntityHero self = GameLogic.Self;
			self.OnHitted = (Action<EntityBase, long>)Delegate.Combine(self.OnHitted, new Action<EntityBase, long>(OnHitted));
		}
	}

	protected override void OnDeInit()
	{
		if ((bool)GameLogic.Self)
		{
			EntityHero self = GameLogic.Self;
			self.OnHitted = (Action<EntityBase, long>)Delegate.Remove(self.OnHitted, new Action<EntityBase, long>(OnHitted));
		}
	}

	private void OnHitted(EntityBase entity, long hit)
	{
		OnFailure();
	}
}
