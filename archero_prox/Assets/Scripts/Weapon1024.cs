




using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1024 : WeaponBase
{
	protected string AttackEffect = "WeaponHand1024Effect";

	protected Transform effectparent;

	private GameObject effect;

	protected override void OnInstall()
	{
		base.OnInstall();
		if (effectparent == null)
		{
			effectparent = m_Entity.m_Body.LeftWeapon.transform;
		}
		CreateEffect();
		OnAttackStartEndAction = (Action)Delegate.Combine(OnAttackStartEndAction, new Action(AttackStartEnd));
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
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
			effect.transform.SetParent(effectparent);
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
