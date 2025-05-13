




using UnityEngine;

public class MoveControlHero : MoveControl
{
	private GameObject footDirection;

	private EntityHero _EntityHero;

	private EntityHero mEntityHero
	{
		get
		{
			if (_EntityHero == null)
			{
				_EntityHero = (m_Entity as EntityHero);
			}
			return _EntityHero;
		}
	}

	protected override void OnInit()
	{
		EntityHero entityHero = m_Entity as EntityHero;
		if ((bool)entityHero && (bool)entityHero.FootDirection)
		{
			footDirection = (m_Entity as EntityHero).FootDirection.transform.Find("Direction").gameObject;
			footDirection.SetActive(value: false);
		}
	}

	protected override void MoveStartVirtual()
	{
		base.MoveStartVirtual();
		if ((bool)footDirection)
		{
			footDirection.SetActive(value: true);
		}
		GameLogic.Release.Mode.RoomGenerate.PlayerMove();
		mEntityHero.DoMoveStart();
	}

	protected override void MovingVirtual(JoyData data)
	{
		base.MovingVirtual(data);
		if ((bool)footDirection)
		{
			footDirection.transform.localPosition = new Vector3(data.direction.x, 0f, data.direction.z / 1.23f) * data.length / 60f;
		}
		mEntityHero.DoMoving(data);
	}

	protected override void MoveEndVirtual()
	{
		base.MoveEndVirtual();
		if ((bool)footDirection)
		{
			footDirection.SetActive(value: false);
		}
		mEntityHero.DoMoveEnd();
	}
}
