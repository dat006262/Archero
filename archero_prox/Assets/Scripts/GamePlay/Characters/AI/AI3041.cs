




using Dxx.Util;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class AI3041 : AIBase
{
	private List<BulletRedLineCtrl> mLines = new List<BulletRedLineCtrl>();

	private ActionBattle action = new ActionBattle();

	private int bulletid;

	private int attack;

	private float startangle;

	private static float[] angles = new float[4]
	{
		40f,
		140f,
		220f,
		320f
	};

	protected override void OnInit()
	{
		bulletid = m_Entity.m_Data.WeaponID;
		attack = LocalModelManager.Instance.Weapon_weapon.GetBeanById(bulletid).Attack;
		action.Init(m_Entity);
		AddAction(GetActionWait(string.Empty, 2000));
		AddAction(GetActionDelegate(delegate
		{
			startangle = ((!MathDxx.RandomBool()) ? 45f : 0f);
			showlines(show: true);
		}));
		AddAction(GetActionWait(string.Empty, 1000));
		AddAction(GetActionDelegate(delegate
		{
			showlines(show: false);
			CreateBullets();
		}));
		AddAction(GetActionWait(string.Empty, 2000));
	}

	private void showlines(bool show)
	{
		if (mLines.Count == 0)
		{
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>("Game/Bullet/Bullet1102_RedLine"));
				gameObject.SetParentNormal(m_Entity.m_Body.LeftBullet.transform);
				BulletRedLineCtrl component = gameObject.GetComponent<BulletRedLineCtrl>();
				mLines.Add(component);
			}
		}
		if (show)
		{
			int j = 0;
			for (int count = mLines.Count; j < count; j++)
			{
				BulletRedLineCtrl bulletRedLineCtrl = mLines[j];
				bulletRedLineCtrl.gameObject.SetActive(value: true);
				if (startangle == 0f)
				{
					bulletRedLineCtrl.transform.rotation = Quaternion.Euler(0f, (float)j * 90f + startangle, 0f);
				}
				else
				{
					bulletRedLineCtrl.transform.rotation = Quaternion.Euler(0f, angles[j], 0f);
				}
				bulletRedLineCtrl.UpdateLine(throughinsidewall: false, 0.5f);
				bulletRedLineCtrl.PlayLineWidth();
			}
		}
		else
		{
			int k = 0;
			for (int count2 = mLines.Count; k < count2; k++)
			{
				mLines[k].gameObject.SetActive(value: false);
			}
		}
	}

	private void ClearLines()
	{
		for (int i = 0; i < mLines.Count; i++)
		{
			BulletRedLineCtrl bulletRedLineCtrl = mLines[i];
			if ((bool)bulletRedLineCtrl)
			{
				UnityEngine.Object.Destroy(bulletRedLineCtrl.gameObject);
			}
		}
		mLines.Clear();
	}

	protected override void OnAIDeInit()
	{
		action.DeInit();
		ClearLines();
	}

	private void CreateBullets()
	{
		for (int i = 0; i < 4; i++)
		{
			float num = 0f;
			num = ((startangle != 0f) ? angles[i] : ((float)i * 90f + startangle));
			GameLogic.Release.Bullet.CreateBullet(m_Entity, bulletid, m_Entity.m_Body.LeftBullet.transform.position, num);
		}
	}
}
