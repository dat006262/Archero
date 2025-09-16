




using UnityEngine;

public class EntityMonsterStrenghBase : EntityMonsterBase
{
	private GameObject effect_strengh;

	protected override void StartInit()
	{
		base.StartInit();
		m_Body.SetStrengh();
		effect_strengh = GameLogic.EffectGet("Effect/Battle/effect_strengh");
		effect_strengh.SetParentNormal(m_Body.AnimatorBodyObj);
		StrenghEffectCtrl component = effect_strengh.GetComponent<StrenghEffectCtrl>();
		component.InitMesh(this);
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
		if ((bool)effect_strengh)
		{
			GameLogic.EffectCache(effect_strengh);
		}
	}
}
