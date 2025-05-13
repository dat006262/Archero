




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class AI3098 : AIBase
{
	private SequencePool mSeqPool = new SequencePool();

	private int index;

	private float startangle;

	protected override void OnInitOnce()
	{
		GameObject child = GameLogic.EffectGet("Effect/Monster/3097_blue");
		child.SetParentNormal(m_Entity.m_Body.Body);
		Entity3097BaseCtrl entity3097BaseCtrl = CInstance<BattleResourceCreator>.Instance.Get3097Base(m_Entity.m_Body.EffectMask.transform.parent);
		entity3097BaseCtrl.SetTexture("3097_blue");
		startangle = (float)GameLogic.Random(0, 8) * 45f;
		KillSequence();
		mSeqPool.Get().AppendInterval(GameLogic.Random(1.5f, 2f)).AppendCallback(delegate
		{
			mSeqPool.Get().AppendCallback(delegate
			{
				float num = (float)index * 45f + startangle;
				float x = MathDxx.Sin(num);
				float z = MathDxx.Cos(num);
				GameLogic.Release.Bullet.CreateBullet(m_Entity, m_Entity.m_Data.WeaponID, m_Entity.m_Body.LeftBullet.transform.position + new Vector3(x, 0f, z) * 0.5f, num);
				index++;
			}).AppendInterval(0.5f)
				.SetLoops(-1);
		});
	}

	protected override void OnInit()
	{
	}

	protected override void OnAIDeInit()
	{
		KillSequence();
	}

	private void KillSequence()
	{
		mSeqPool.Clear();
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}
}
