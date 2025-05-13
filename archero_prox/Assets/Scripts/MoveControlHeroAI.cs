




public class MoveControlHeroAI : MoveControl
{
	protected override void OnMoveSpeedUpdate()
	{
		MoveDirection = m_JoyData.MoveDirection * m_Entity.m_EntityData.GetSpeed();
	}
}
