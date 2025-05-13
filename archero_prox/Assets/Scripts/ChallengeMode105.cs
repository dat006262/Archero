




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class ChallengeMode105 : ChallengeMode102
{
	private int updatetime;

	private int createtime = 4;

	private float percent;

	private SequencePool m_pool;

	private bool bSync = true;

	private int[,] rects;

	protected override void OnInit()
	{
		base.OnInit();
		m_pool = new SequencePool();
	}

	protected override void OnUpdate()
	{
		updatetime++;
		if (updatetime == createtime)
		{
			updatetime -= createtime;
			percent = (float)(alltime - currenttime) / (float)alltime;
			CreateBombs();
		}
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
		ClearPool();
	}

	private void ClearPool()
	{
		m_pool.Clear();
	}

	private void CreateBombs()
	{
		ClearPool();
		rects = (int[,])GameLogic.Release.MapCreatorCtrl.GetRects().Clone();
		float num = 1f + percent * 3f;
		int num2 = (int)GameLogic.Random(5f * num, 8f * num);
		if (bSync)
		{
			for (int i = 0; i < num2; i++)
			{
				CreateBombOne();
			}
			return;
		}
		Sequence s = m_pool.Get();
		for (int j = 0; j < num2; j++)
		{
			s.AppendCallback(CreateBombOne);
			s.AppendInterval(0.3f);
		}
	}

	private void CreateBombOne()
	{
		Vector3 random_position = get_random_position();
		BulletBombDodge bulletBombDodge = GameLogic.Release.Bullet.CreateBullet(null, 3034, random_position + new Vector3(0f, 21f, 0f), 0f) as BulletBombDodge;
		bulletBombDodge.SetEndPos(random_position);
		bulletBombDodge.SetTarget(null);
		bulletBombDodge.mBulletTransmit.SetAttack(MathDxx.CeilToInt(0.35f * (float)GameLogic.Self.m_EntityData.MaxHP));
	}

	private Vector3 get_random_position()
	{
		if (!bSync)
		{
			return GameLogic.Release.MapCreatorCtrl.RandomPosition();
		}
		Vector2Int empty = get_empty();
		while (check_full(empty))
		{
			empty = get_empty();
		}
		rects[empty.x, empty.y] = 1;
		return GameLogic.Release.MapCreatorCtrl.GetWorldPosition(empty);
	}

	private Vector2Int get_empty()
	{
		int num = GameLogic.Random(0, rects.GetLength(0));
		int num2 = GameLogic.Random(0, rects.GetLength(1));
		while (rects[num, num2] != 0)
		{
			num = GameLogic.Random(0, rects.GetLength(0));
			num2 = GameLogic.Random(0, rects.GetLength(1));
		}
		return new Vector2Int(num, num2);
	}

	private bool check_full(Vector2Int trypos)
	{
		int length = rects.GetLength(0);
		int length2 = rects.GetLength(1);
		int[,] array = (int[,])rects.Clone();
		excute_checks(array, trypos);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				if (rects[i, j] == 1)
				{
					excute_checks(array, new Vector2Int(i, j));
				}
			}
		}
		for (int k = 0; k < length; k++)
		{
			for (int l = 0; l < length2; l++)
			{
				if (array[k, l] == 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	private void debug()
	{
		int length = rects.GetLength(0);
		int length2 = rects.GetLength(1);
		int[,] array = (int[,])rects.Clone();
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				if (rects[i, j] == 1)
				{
					excute_checks(array, new Vector2Int(i, j));
				}
			}
		}
		string empty = string.Empty;
		for (int k = 0; k < length; k++)
		{
			string arg = k + " : ";
			for (int l = 0; l < length2; l++)
			{
				arg = arg + array[k, l] + " ";
				if (array[k, l] == 0)
				{
					return;
				}
			}
		}
	}

	private void excute_checks(int[,] checks, Vector2Int v)
	{
		int length = checks.GetLength(0);
		int length2 = checks.GetLength(1);
		int num = v.x;
		while (num >= 0 && checks[num, v.y] <= 1)
		{
			checks[num, v.y] = 1;
			num--;
		}
		for (int i = v.x + 1; i < length && checks[i, v.y] <= 1; i++)
		{
			checks[i, v.y] = 1;
		}
		int num2 = v.y;
		while (num2 >= 0 && checks[v.x, num2] <= 1)
		{
			checks[v.x, num2] = 1;
			num2--;
		}
		for (int j = v.y + 1; j < length && checks[v.x, j] <= 1; j++)
		{
			checks[v.x, j] = 1;
		}
	}
}
