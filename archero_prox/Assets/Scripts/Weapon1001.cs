




using UnityEngine;

public class Weapon1001 : WeaponBase
{
	private Animation weaponAni;

	private const string PrevAction = "weapon1001_prev";

	private const string EndAction = "weapon1001_end";

	private const string ResetAction = "weapon1001_reset";

	protected override void OnInstall()
	{
		base.OnInstall();
		weaponAni = m_Entity.m_WeaponHand.transform.Find("child/gong").GetComponent<Animation>();
		weaponAni.enabled = true;
		OnAttackStartStartAction = OnAttackStartStartActions;
		OnAttackEndStartAction = OnAttackEndStartActions;
		OnAttackInterruptAction = OnAttackInterruptActions;
	}

	protected override void OnUnInstall()
	{
		if ((bool)weaponAni)
		{
			weaponAni.enabled = false;
		}
		base.OnUnInstall();
		OnAttackStartStartAction = null;
		OnAttackEndStartAction = null;
		OnAttackInterruptAction = null;
	}

	public override void AttackJoyTouchDown()
	{
	}

	public override void AttackJoyTouchUp()
	{
        //Debug.LogError("@LOG AttackJoyTouchUp");
    }

    private void OnAttackStartStartActions()
	{
		weaponAni.Play("weapon1001_prev");
	}

	private void OnAttackEndStartActions()
	{
		weaponAni.Play("weapon1001_end");
	}

	private void OnAttackInterruptActions()
	{
		weaponAni.Play("weapon1001_reset");
	}
}
