




using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ThunderContinueMgr
{
	public class ThunderContinueData
	{
		public EntityBase entity;

		public int bulletid;

		public int count;

		public float delay;

		public float prev_scale = 1f;

		public string prev_effect = "Game/PrevEffect/BulletPrev_1083";
	}

	public class ThunderContinueReceive
	{
		public List<Sequence> seqs;

		public List<GameObject> prev_effects;

		public void Deinit()
		{
			int i = 0;
			for (int count = seqs.Count; i < count; i++)
			{
				seqs[i]?.Kill();
			}
			int j = 0;
			for (int count2 = prev_effects.Count; j < count2; j++)
			{
				prev_effects[j].transform.DOKill();
				GameLogic.EffectCache(prev_effects[j]);
			}
			prev_effects.Clear();
			seqs.Clear();
		}
	}

	public static ThunderContinueReceive GetThunderContinue(ThunderContinueData data)
	{
		ThunderContinueReceive receive = new ThunderContinueReceive();
		receive.seqs = new List<Sequence>();
		List<Vector3> poslist = new List<Vector3>();
		receive.prev_effects = new List<GameObject>();
		for (int i = 0; i < data.count; i++)
		{
			int index = i;
			Sequence sequence = DOTween.Sequence();
			sequence.AppendInterval((float)index * data.delay);
			sequence.AppendCallback(delegate
			{
				Vector3 vector = GameLogic.Release.MapCreatorCtrl.RandomPosition(index % 4);
				for (int j = 0; j < 20; j++)
				{
					if ((vector - data.entity.position).magnitude < 2f)
					{
						vector = GameLogic.Release.MapCreatorCtrl.RandomPosition(index % 4);
					}
				}
				poslist.Add(vector);
				GameObject gameObject = GameLogic.EffectGet("Game/PrevEffect/BulletPrev_1083");
				gameObject.transform.position = data.entity.position;
				gameObject.transform.localScale = Vector3.one * data.prev_scale;
				receive.prev_effects.Add(gameObject);
				gameObject.transform.DOMove(poslist[index], data.delay);
			});
			sequence.AppendInterval(1.4f);
			sequence.AppendCallback(delegate
			{
				GameLogic.Release.Bullet.CreateBullet(data.entity, data.bulletid, poslist[index], 0f);
			});
			receive.seqs.Add(sequence);
		}
		return receive;
	}
}
