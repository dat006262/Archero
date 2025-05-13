




using TableTool;
using UnityEngine;

public class AIMove1038 : AIMoveBase
{
	private Weapon_weapon weapondata;

	private int attackcount = 8;

	private ActionBattle action = new ActionBattle();

	public AIMove1038(EntityBase entity)
		: base(entity)
	{
		weapondata = LocalModelManager.Instance.Weapon_weapon.GetBeanById(5039);
	}

	protected override void OnInitBase()
	{
		action.Init(m_Entity);
		m_Entity.m_AniCtrl.SendEvent("Skill");
		CreateFire();
	}

	private void CreateFire()
	{
		action.AddActionWait(0.3f);
		for (int i = 0; i < attackcount; i++)
		{
			action.AddActionDelegate(delegate
			{
				Vector3 a = GameLogic.Release.MapCreatorCtrl.RandomPosition();
				GameLogic.Release.Bullet.CreateBullet(m_Entity, 5039, a + new Vector3(0f, 40f, 0f), 0f);
			});
			if (i < attackcount - 1)
			{
				action.AddActionWait(0.3f);
			}
		}
		action.AddActionDelegate(base.End);
	}

	protected override void OnEnd()
	{
		action.DeInit();
	}
}
