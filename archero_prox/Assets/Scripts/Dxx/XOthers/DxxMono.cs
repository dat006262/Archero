




using UnityEngine;

namespace Dxx
{
	public class DxxMono : MonoBehaviour
	{
		private Transform m_transform;

		public Transform trans
		{
			get
			{
				if (m_transform == null)
				{
					m_transform = GetComponent<Transform>();
				}
				return m_transform;
			}
		}
	}
}
