




public class Weapon5074 : Weapon5073
{
	protected override void OnInit()
	{
		if (m_Entity.m_Body.BulletList.Count > 0)
		{
			effectparent = m_Entity.m_Body.BulletList[0].transform;
		}
		base.OnInit();
	}
}
