




using Dxx.Util;
using UnityEngine;

public class BattleUIBossHPCtrl
{
	private RectTransform t1;

	private RectTransform t2;

	private int width;

	private float speed = 70f;

	private bool bStart;

	private float starttime;

	private float mEndX;

	public void Init(RectTransform t1, RectTransform t2, int width)
	{
		this.t1 = t1;
		this.t2 = t2;
		this.width = width;
		update_add();
	}

	private void update_add()
	{
		update_remove();
		Updater.AddUpdate("BattleUIBossHPCtrl", OnUpdate);
	}

	private void update_remove()
	{
		Updater.RemoveUpdate("BattleUIBossHPCtrl", OnUpdate);
	}

	public void DeInit()
	{
		update_remove();
	}

	private void OnUpdate(float delta)
	{
		if (bStart && !(Updater.AliveTime - starttime < 0.2f) && (bool)t1 && (bool)t2)
		{
			Vector2 sizeDelta = t1.sizeDelta;
			float num = sizeDelta.x - Updater.delta * speed;
			if (num < mEndX)
			{
				num = mEndX;
				bStart = false;
			}
			RectTransform rectTransform = t1;
			float x = num;
			Vector2 sizeDelta2 = t1.sizeDelta;
			rectTransform.sizeDelta = new Vector2(x, sizeDelta2.y);
			t2.sizeDelta = t1.sizeDelta;
		}
	}

	public void Reduce(float endx)
	{
		if (!bStart)
		{
			starttime = Updater.AliveTime;
		}
		bStart = true;
		update_add();
		mEndX = endx;
	}
}
