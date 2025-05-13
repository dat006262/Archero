




using Dxx.Util;
using UnityEngine;

public class EntitySoldier3039 : EntityMonsterBase
{
	protected override HittedData OnHittedData(HittedData data, bool bulletthrough, float bulletangle)
	{
		if (bulletthrough)
		{
			return data;
		}
		Vector3 eulerAngles = base.eulerAngles;
		float y = eulerAngles.y;
		if (MathDxx.Abs(y - bulletangle) < 90f || MathDxx.Abs(y - bulletangle + 360f) < 90f || MathDxx.Abs(y - bulletangle - 360f) < 90f)
		{
			return data;
		}
		if (m_MoveCtrl.GetMoving())
		{
			PlayEffect(1300001, m_Body.SpecialHitMask.transform.position);
			data.type = EHittedType.eDefence;
			data.hitratio = 0.4f;
			data.backtatio = 0.7f;
			return data;
		}
		return data;
	}
}
