




using UnityEngine;
using UnityEngine.UI;

namespace Dxx.UI
{
	[AddComponentMenu("UI/Effects/UI Flip")]
	[RequireComponent(typeof(Graphic))]
	[DisallowMultipleComponent]
	public class UIFlip : BaseMeshEffect
	{
		[SerializeField]
		private bool m_Horizontal;

		[SerializeField]
		private bool m_Veritical;

		public bool horizontal
		{
			get
			{
				return m_Horizontal;
			}
			set
			{
				m_Horizontal = value;
			}
		}

		public bool vertical
		{
			get
			{
				return m_Veritical;
			}
			set
			{
				m_Veritical = value;
			}
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			RectTransform rectTransform = base.graphic.rectTransform;
			UIVertex vertex = default(UIVertex);
			Vector2 center = rectTransform.rect.center;
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				vh.PopulateUIVertex(ref vertex, i);
				Vector3 position = vertex.position;
				vertex.position = new Vector3((!m_Horizontal) ? position.x : (0f - position.x), (!m_Veritical) ? position.y : (0f - position.y));
				vh.SetUIVertex(vertex, i);
			}
		}
	}
}
