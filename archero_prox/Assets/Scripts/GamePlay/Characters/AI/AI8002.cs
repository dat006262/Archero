




using System;

public class AI8002 : AIBase
{
	private ActionBasic action = new ActionBasic();

	protected virtual float BulletAngle => 270f;

	protected override void OnInit()
	{
		action.Init();
		MapCreator mapCreatorCtrl = GameLogic.Release.MapCreatorCtrl;
		mapCreatorCtrl.Event_Button1101 = (Action)Delegate.Combine(mapCreatorCtrl.Event_Button1101, new Action(OnAttack));
	}

	protected override void OnAIDeInit()
	{
		action.DeInit();
		MapCreator mapCreatorCtrl = GameLogic.Release.MapCreatorCtrl;
		mapCreatorCtrl.Event_Button1101 = (Action)Delegate.Remove(mapCreatorCtrl.Event_Button1101, new Action(OnAttack));
	}

	private void OnAttack()
	{
		CreateBullet();
		for (int i = 0; i < 2; i++)
		{
			action.AddActionWaitDelegate(0.2f, delegate
			{
				CreateBullet();
			});
		}
	}

	private void CreateBullet()
	{
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 4003, m_Entity.m_Body.LeftBullet.transform.position, BulletAngle);
	}
}
