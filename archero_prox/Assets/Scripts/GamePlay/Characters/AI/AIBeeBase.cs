




using DG.Tweening;

public class AIBeeBase : AIBase
{
	private Sequence seq;

	protected override void OnInitOnce()
	{
		seq = DOTween.Sequence();
		seq.AppendCallback(delegate
		{
			GameLogic.Hold.Sound.PlayMonsterSkill(5100005, m_Entity.position);
		}).AppendInterval(2.2f).SetLoops(-1);
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	protected override void OnAIDeInit()
	{
		KillSequence();
	}
}
