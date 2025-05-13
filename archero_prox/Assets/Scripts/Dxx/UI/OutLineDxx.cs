




using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dxx.UI
{
	[AddComponentMenu("UI/Effects/OutLineDxx")]
	[RequireComponent(typeof(Text))]
	public class OutLineDxx : Shadow
	{
		private const float DOWNWIDTH = 6f;

		private Text m_Text;

		private List<Vector2> shadowPosList = new List<Vector2>
		{
			new Vector2(0f, 0f),
			new Vector2(0f, -6f)
		};

		public Text text
		{
			get
			{
				if (m_Text == null)
				{
					m_Text = GetComponent<Text>();
				}
				return m_Text;
			}
		}

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
			{
				return;
			}
			List<UIVertex> list = ListPool<UIVertex>.Get();
			vh.GetUIVertexStream(list);
			int num = list.Count * shadowPosList.Count * 5;
			if (list.Capacity < num)
			{
				list.Capacity = num;
			}
			int num2 = 0;
			int count = list.Count;
			for (int i = 0; i < shadowPosList.Count; i++)
			{
				Vector2 vector = shadowPosList[i] * text.fontSize * 0.02f;
				if (shadowPosList[i] != Vector2.zero)
				{
					ApplyShadowZeroAlloc(list, base.effectColor, num2, list.Count, vector.x, vector.y);
					num2 = count;
					count = list.Count;
				}
				List<UIVertex> verts = list;
				Color32 color = base.effectColor;
				int start = num2;
				int count2 = list.Count;
				float x = vector.x;
				Vector2 effectDistance = base.effectDistance;
				float x2 = x + effectDistance.x;
				float y = vector.y;
				Vector2 effectDistance2 = base.effectDistance;
				ApplyShadowZeroAlloc(verts, color, start, count2, x2, y + effectDistance2.y);
				num2 = count;
				count = list.Count;
				List<UIVertex> verts2 = list;
				Color32 color2 = base.effectColor;
				int start2 = num2;
				int count3 = list.Count;
				float x3 = vector.x;
				Vector2 effectDistance3 = base.effectDistance;
				float x4 = x3 + effectDistance3.x;
				float y2 = vector.y;
				Vector2 effectDistance4 = base.effectDistance;
				ApplyShadowZeroAlloc(verts2, color2, start2, count3, x4, y2 - effectDistance4.y);
				num2 = count;
				count = list.Count;
				List<UIVertex> verts3 = list;
				Color32 color3 = base.effectColor;
				int start3 = num2;
				int count4 = list.Count;
				float x5 = vector.x;
				Vector2 effectDistance5 = base.effectDistance;
				float x6 = x5 - effectDistance5.x;
				float y3 = vector.y;
				Vector2 effectDistance6 = base.effectDistance;
				ApplyShadowZeroAlloc(verts3, color3, start3, count4, x6, y3 + effectDistance6.y);
				num2 = count;
				count = list.Count;
				List<UIVertex> verts4 = list;
				Color32 color4 = base.effectColor;
				int start4 = num2;
				int count5 = list.Count;
				float x7 = vector.x;
				Vector2 effectDistance7 = base.effectDistance;
				float x8 = x7 - effectDistance7.x;
				float y4 = vector.y;
				Vector2 effectDistance8 = base.effectDistance;
				ApplyShadowZeroAlloc(verts4, color4, start4, count5, x8, y4 - effectDistance8.y);
				num2 = count;
				count = list.Count;
			}
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
			ListPool<UIVertex>.Release(list);
		}
	}
}
