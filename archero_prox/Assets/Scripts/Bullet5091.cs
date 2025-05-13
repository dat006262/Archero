




using System.Collections.Generic;
using UnityEngine;

public class Bullet5091 : BulletBase
{
	private GameObject lines;

	private float angle;

	private float createdis;

	private float perdis = 7f;

	private bool bCreateRedLine;

	private List<BulletBase> bullets = new List<BulletBase>();

	protected override void OnInit()
	{
		if (lines == null)
		{
			Transform transform = mTransform.Find("lines");
			if ((bool)transform)
			{
				lines = transform.gameObject;
			}
		}
		if ((bool)lines)
		{
			lines.transform.rotation = Quaternion.identity;
		}
		bFlyRotate = false;
		base.OnInit();
		createdis = perdis;
		DeInitBullets();
		bCreateRedLine = false;
		LineShow(value: false);
	}

	protected override void OnDeInit()
	{
		DeInitBullets();
		base.OnDeInit();
	}

	private void DeInitBullets()
	{
		int i = 0;
		for (int count = bullets.Count; i < count; i++)
		{
			bullets[i].DeInit();
			GameLogic.BulletCache(bullets[i].m_Data.WeaponID, bullets[i].gameObject);
		}
		bullets.Clear();
	}

	protected override void UpdateProcess()
	{
		base.UpdateProcess();
		if (base.CurrentDistance > createdis - 3.5f && !bCreateRedLine)
		{
			bCreateRedLine = true;
			LineShow(value: true);
		}
		if (base.CurrentDistance > createdis)
		{
			CreateBullets();
			createdis += perdis;
			bCreateRedLine = false;
			LineShow(value: false);
		}
	}

	private void LineShow(bool value)
	{
		if ((bool)lines)
		{
			lines.SetActive(value);
		}
	}

	private void CreateBullets()
	{
		for (int i = 0; i < 4; i++)
		{
			BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 5092, Vector3.zero, 0f);
			bulletBase.transform.SetParentNormal(mTransform);
			bulletBase.transform.rotation = Quaternion.Euler(0f, i * 90, 0f);
			bullets.Add(bulletBase);
		}
	}
}
