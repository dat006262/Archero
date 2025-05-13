




using DG.Tweening;

public class Weapon5103 : WeaponBase
{
	private Sequence seq;

	protected override void OnInstall()
	{
		KillSequence();
	}

	protected override void OnUnInstall()
	{
		KillSequence();
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	protected override void OnAttack(object[] args)
	{
		seq = DOTween.Sequence();
		for (int i = 0; i < 2; i++)
		{
			seq.AppendCallback(delegate
			{
				CreateBulletOverride();
			});
			seq.AppendInterval(0.4f);
		}
	}
}
