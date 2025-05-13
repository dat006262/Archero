




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class EntityLifeCtrl : EntityCtrlBase
{
	public override void OnStart(List<EBattleAction> actIds)
	{
		actIds.Add(EBattleAction.EBattle_Action_Hitted_Once);
		actIds.Add(EBattleAction.EBattle_Action_Dead_Before);
		actIds.Add(EBattleAction.EBattle_Action_Dead);
	}

	public override void ExcuteCommend(EBattleAction id, object action)
	{
		switch (id)
		{
		case EBattleAction.EBattle_Action_Hitted_Once:
			OnEntityHittedOnce((HitStruct)action);
			break;
		case EBattleAction.EBattle_Action_Dead_Before:
			OnEntityDeadBefore((BattleStruct.DeadStruct)action);
			break;
		case EBattleAction.EBattle_Action_Dead:
			OnEntityDead((BattleStruct.DeadStruct)action);
			break;
		}
	}

	private void OnEntityHittedOnce(HitStruct data)
	{
		if (!m_Entity || m_Entity.GetIsDead() || m_Entity.Type == EntityType.Baby || (m_Entity.m_EntityData.GetCanShieldCount() && data.before_hit < 0))
		{
			return;
		}
		bool bulletthrough = false;
		float bulletangle = 0f;
		if (data.bulletdata != null && data.bulletdata.weapon != null)
		{
			bulletthrough = data.bulletdata.weapon.bThroughEntity;
		}
		if (data.bulletdata != null && data.bulletdata.bullet != null)
		{
			Vector3 eulerAngles = data.bulletdata.bullet.transform.eulerAngles;
			bulletangle = eulerAngles.y;
		}
		HittedData hittedData = m_Entity.GetHittedData(bulletthrough, bulletangle);
		if (data.type == HitType.Rebound)
		{
			data.real_hit = data.before_hit;
		}
		else if (data.before_hit < 0)
		{
			if (!hittedData.GetCanHitted())
			{
				return;
			}
			switch (data.sourcetype)
			{
			case HitSourceType.eBullet:
			{
				if (!m_Entity || data.bulletdata == null || data.bulletdata.weapon == null)
				{
					return;
				}
				float num = data.before_hit;
				num *= hittedData.hitratio;
				if (data.source != null)
				{
					float num2 = Vector3.Distance(m_Entity.position, data.source.position);
					float num3 = num2 / (float)data.source.m_EntityData.attribute.DistanceAttackValueDis.Value;
					if (num3 < 1f)
					{
						float num4 = (1f - num3) * data.source.m_EntityData.attribute.DistanceAttackValuePercent.Value;
						num *= 1f + num4;
					}
				}
				data.before_hit = (long)num;
				data = m_Entity.m_EntityData.GetHurt(data);
				if (data.type != HitType.Rebound)
				{
					bool flag = false;
					if (!flag)
					{
						flag = m_Entity.m_EntityData.GetHeadShot();
					}
					if (!flag && m_Entity.Type != EntityType.Boss && data.source != null && m_Entity.m_EntityData.GetHPPercent() < data.source.m_EntityData.attribute.KillMonsterLessHP.Value && GameLogic.Random(0f, 1f) < data.source.m_EntityData.attribute.KillMonsterLessHPRatio.Value)
					{
						flag = true;
					}
					if (flag)
					{
						data.real_hit = -9223372036854775807L;
						data.type = HitType.HeadShot;
					}
					if (data.source != null)
					{
						data.source.m_EntityData.ExcuteHitAdd();
					}
				}
				break;
			}
			case HitSourceType.eBuff:
				data = m_Entity.m_EntityData.GetHurt(data);
				break;
			case HitSourceType.eTrap:
				data = m_Entity.m_EntityData.GetHurt(data);
				break;
			case HitSourceType.eBody:
				data = m_Entity.m_EntityData.GetHurt(data);
				break;
			case HitSourceType.eSkill:
				data = m_Entity.m_EntityData.GetHurt(data);
				break;
			}
		}
		else
		{
			data.real_hit = data.before_hit;
		}
		if (data.real_hit == 0)
		{
			if (data.type == HitType.Miss)
			{
				GameLogic.CreateHPChanger(data.source, m_Entity, data);
			}
			return;
		}
		if ((float)data.real_hit < 0f)
		{
			if (data.sourcetype == HitSourceType.eBullet && data.bulletdata != null && data.bulletdata.bullet != null && data.bulletdata.weapon != null)
			{
				hittedData.AddBackRatio(data.bulletdata.weapon.BackRatio);
				hittedData.AddBackRatio(m_Entity.m_Data.BackRatio);
				hittedData.SetBullet(data.bulletdata.bullet);
				hittedData.hittype = data.type;
				m_Entity.SetHitted(hittedData);
			}
			if ((data.sourcetype == HitSourceType.eBullet || data.sourcetype == HitSourceType.eTrap || data.sourcetype == HitSourceType.eBody) && GameLogic.Hold.BattleData.Challenge_ismainchallenge() && m_Entity.IsSelf)
			{
				GameLogic.Hold.BattleData.AddHittedCount(GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID());
			}
			long shieldHitValue = m_Entity.m_EntityData.GetShieldHitValue(-data.real_hit);
			data.real_hit += shieldHitValue;
			if (m_Entity.OnHitted != null)
			{
				m_Entity.OnHitted(data.source, data.real_hit);
			}
			if ((data.sourcetype == HitSourceType.eBullet || data.sourcetype == HitSourceType.eBody) && data.source != null && data.real_hit < 0 && data.type != HitType.Rebound)
			{
				int num5 = 0;
				if (m_Entity.m_EntityData.attribute.ReboundHit.Value > 0)
				{
					num5 += (int)m_Entity.m_EntityData.attribute.ReboundHit.Value;
				}
				if (m_Entity.m_EntityData.attribute.ReboundTargetPercent.Value > 0f && data.source.Type != EntityType.Boss)
				{
					num5 += (int)((float)data.source.m_EntityData.MaxHP * m_Entity.m_EntityData.attribute.ReboundTargetPercent.Value);
				}
				if (num5 > 0)
				{
					data.before_hit = -num5;
					GameLogic.SendHit_Rebound(data.source, m_Entity, data);
				}
			}
		}
		if (m_Entity.IsSelf && (float)data.real_hit < 0f && m_Entity.m_EntityData.mDeadRecover > 0 && m_Entity.m_EntityData.CurrentHP <= 10)
		{
			m_Entity.m_EntityData.UseDeadRecover();
			return;
		}
		if ((GameLogic.Hold.BattleData.Challenge_RecoverHP() && data.real_hit > 0) || data.real_hit < 0)
		{
			GameLogic.CreateHPChanger(data.source, m_Entity, data);
		}
		if (data.type == HitType.Crit && (bool)data.source && data.source.OnCrit != null)
		{
			data.source.OnCrit(MathDxx.Abs(data.real_hit));
		}
		if (data.sourcetype == HitSourceType.eBullet || data.sourcetype == HitSourceType.eBody || data.sourcetype == HitSourceType.eSkill || data.sourcetype == HitSourceType.eTrap)
		{
			if (data.real_hit < 0)
			{
				m_Entity.PlayEffect(m_Entity.m_Data.HittedEffectID);
			}
			if (data.real_hit < 0 && data.bulletdata != null)
			{
				if (data.source != null && data.source.m_EntityData.GetLight45() && data.bulletdata != null && data.bulletdata.bullet != null && !data.bulletdata.bullet.GetLight45())
				{
					if (data.source.OnLight45 != null)
					{
						data.source.OnLight45(m_Entity);
					}
				}
				else
				{
					m_Entity.PlayEffect(data.bulletdata.weapon.HittedEffectID, m_Entity.m_Body.EffectMask.transform.position, Quaternion.Euler(0f, 90f - Utils.getAngle(m_Entity.GetHittedDirection()), 0f));
				}
			}
			m_Entity.PlaySound(data.soundid);
		}
		if (data.buffid > 0)
		{
			m_Entity.ChangeHPMust(data.source, data.real_hit);
		}
		else
		{
			m_Entity.ChangeHP(data.source, data.real_hit);
		}
	}

	private void OnEntityDeadBefore(BattleStruct.DeadStruct data)
	{
		m_Entity.DeadBefore();
	}

	private void OnEntityDead(BattleStruct.DeadStruct data)
	{
	}
}
