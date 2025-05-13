




using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1039 : WeaponBase
{
	private GameObject effect;

	protected virtual string AttackEffect => "WeaponHand1024Effect";

	protected override void OnAttack(object[] args)
	{
		int num = 5;
		for (int i = 0; i < num; i++)
		{
			action.AddActionDelegate(delegate
			{
				CreateBulletOverride(Vector3.zero, 0f);
			});
			action.AddActionWait(0.3f);
		}
	}

	protected override void OnInstall()
	{
		CreateEffect();
		OnAttackStartEndAction = (Action)Delegate.Combine(OnAttackStartEndAction, new Action(AttackStartEnd));
	}

	protected override void OnUnInstall()
	{
		RemoveEffect();
		OnAttackStartEndAction = (Action)Delegate.Remove(OnAttackStartEndAction, new Action(AttackStartEnd));
	}

	private void AttackStartEnd()
	{
		RemoveEffect();
	}

	private void CreateEffect()
	{
		effect = GameLogic.EffectGet(Utils.GetString("Game/WeaponHand/", AttackEffect));
		if ((bool)effect)
		{
			effect.transform.SetParent(m_Entity.m_Body.LeftWeapon.transform);
			effect.transform.localPosition = Vector3.zero;
		}
	}

	private void RemoveEffect()
	{
		if (effect != null)
		{
			GameLogic.EffectCache(effect);
			effect = null;
		}
	}
}
