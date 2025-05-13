




using PureMVC.Patterns;
using System;

public class AI8001 : AIBase
{
	private ActionBattle action = new ActionBattle();

	private int ran;

	private int waveid;

	private Action<int> onWaveUpdate;

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		action.Init(m_Entity);
		onWaveUpdate = OnWaveUpdate;
		Facade.Instance.SendNotification("UpdateWave", onWaveUpdate);
	}

	private void OnWaveUpdate(int waveid)
	{
		this.waveid = waveid;
		ReRandomAI();
	}

	protected override void OnInit()
	{
		if (waveid != 0)
		{
			switch (waveid)
			{
			case 1:
				CreateWave1();
				break;
			case 2:
				CreateWave2();
				break;
			case 3:
				CreateWave3();
				break;
			}
			bReRandom = true;
		}
	}

	protected override void OnAIDeInit()
	{
		action.DeInit();
	}

	private void CreateWave1()
	{
		AddAction(GetActionWaitDelegate(500, delegate
		{
			CreateBullets1_0();
		}));
	}

	private void CreateWave2()
	{
		AddAction(GetActionWaitDelegate(500, delegate
		{
			CreateBullets2_0();
		}));
	}

	private void CreateWave3()
	{
		AddAction(GetActionWaitDelegate(500, delegate
		{
			CreateBullets3_0();
		}));
	}

	private void CreateBullets1_0()
	{
		float num = GameLogic.Random(0f, 100f);
		int num2 = 4;
		float num3 = 360f / (float)num2;
		for (int i = 0; i < num2; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 1038, m_Entity.m_Body.LeftBullet.transform.position, num3 * (float)i + num);
		}
	}

	private void CreateBullets2_0()
	{
		float num = GameLogic.Random(0f, 100f);
		int num2 = 8;
		float num3 = 360f / (float)num2;
		for (int i = 0; i < num2; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 1038, m_Entity.m_Body.LeftBullet.transform.position, num3 * (float)i + num);
		}
	}

	private void CreateBullets3_0()
	{
		float num = GameLogic.Random(0f, 100f);
		int num2 = 16;
		float num3 = 360f / (float)num2;
		for (int i = 0; i < num2; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 1038, m_Entity.m_Body.LeftBullet.transform.position, num3 * (float)i + num);
		}
	}
}
