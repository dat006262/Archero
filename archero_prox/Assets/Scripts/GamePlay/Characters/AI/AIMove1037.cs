




using System;
using TableTool;
using UnityEngine;

public class AIMove1037 : AIMoveBase
{
	private string skillname;

	private Weapon_weapon weapondata;

	public AIMove1037(EntityBase entity)
		: base(entity)
	{
		weapondata = LocalModelManager.Instance.Weapon_weapon.GetBeanById(5037);
	}

	protected override void OnInitBase()
	{
		skillname = m_Entity.m_AniCtrl.GetString("Skill");
		m_Entity.m_AniCtrl.SetString("Skill", "FireAttackPrev1");
		m_Entity.m_AniCtrl.SendEvent("Skill");
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Combine(entity.OnSkillActionEnd, new Action(OnSkillEnd));
	}

	private void OnSkillEnd()
	{
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Remove(entity.OnSkillActionEnd, new Action(OnSkillEnd));
		EntityBase entity2 = m_Entity;
		entity2.OnSkillActionEnd = (Action)Delegate.Combine(entity2.OnSkillActionEnd, new Action(OnSkillAfterEnd));
		m_Entity.m_AniCtrl.SetString("Skill", "FireAttackEnd1");
		m_Entity.m_AniCtrl.SendEvent("Skill");
		CreateFire();
	}

	private void CreateFire()
	{
		for (int i = 0; i < 3; i++)
		{
			Vector3 pos = GameLogic.Release.MapCreatorCtrl.RandomPosition();
			float rota = (GameLogic.Random(0, 2) != 0) ? 90 : 0;
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 5037, pos, rota);
		}
	}

	private void OnSkillAfterEnd()
	{
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Remove(entity.OnSkillActionEnd, new Action(OnSkillAfterEnd));
		m_Entity.m_AniCtrl.SetString("Skill", skillname);
		End();
	}

	protected override void OnEnd()
	{
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Remove(entity.OnSkillActionEnd, new Action(OnSkillEnd));
		EntityBase entity2 = m_Entity;
		entity2.OnSkillActionEnd = (Action)Delegate.Remove(entity2.OnSkillActionEnd, new Action(OnSkillAfterEnd));
	}
}
