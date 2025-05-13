




using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dxx.UI
{
	[AddComponentMenu("UI/Effects/TextColor3Dxx")]
	[RequireComponent(typeof(Text))]
	public class TextColor3Dxx : Shadow
	{
		private const float DOWNWIDTH = 6f;

		private Text m_Text;

		public Color topColor = new Color(242f / 255f, 84f / 85f, 227f / 255f, 1f);

		public Color bottomColor = new Color(163f / 255f, 0.882352948f, 103f / 255f, 1f);

		[SerializeField]
		private List<Vector2> shadowPosList = new List<Vector2>
		{
			new Vector2(0f, 0f),
			new Vector2(0f, -6f),
			new Vector2(0f, -6f)
		};

		[SerializeField]
		public List<Color32> colorList = new List<Color32>
		{
			new Color(1f, 1f, 1f, 1f),
			new Color(86f / 255f, 23f / 51f, 59f / 255f, 1f),
			new Color(37f / 85f, 37f / 85f, 37f / 85f, 1f)
		};

		public Color32 middleoutline = Color.black;

		[SerializeField]
		private Vector2 middleoutlineoffset = new Vector2(0.5f, -0.5f);

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
			int currentVertCount = vh.currentVertCount;
			if (currentVertCount == 0)
			{
				return;
			}
			for (int i = 0; i < currentVertCount; i++)
			{
				UIVertex vertex = default(UIVertex);
				vh.PopulateUIVertex(ref vertex, i);
				list.Add(vertex);
			}
			UIVertex uIVertex = list[0];
			float num = uIVertex.position.y;
			UIVertex uIVertex2 = list[0];
			float num2 = uIVertex2.position.y;
			for (int j = 0; j < currentVertCount; j++)
			{
				UIVertex uIVertex3 = list[j];
				float y = uIVertex3.position.y;
				if (y > num)
				{
					num = y;
				}
				else if (y < num2)
				{
					num2 = y;
				}
			}
			float num3 = num - num2;
			for (int k = 0; k < currentVertCount; k++)
			{
				UIVertex vertex2 = list[k];
				Color32 color = vertex2.color = Color32.Lerp(bottomColor, topColor, (vertex2.position.y - num2) / num3);
				vh.SetUIVertex(vertex2, k);
			}
			vh.GetUIVertexStream(list);
			int num4 = list.Count * shadowPosList.Count * 5;
			if (list.Capacity < num4)
			{
				list.Capacity = num4;
			}
			for (int l = 0; l < shadowPosList.Count; l++)
			{
				Vector2 vector = shadowPosList[l] * text.fontSize * 0.02f;
				switch (l)
				{
				case 1:
				{
					Color32 color7 = colorList[l];
					ApplyShadowZeroAlloc(list, color7, 0, list.Count, vector.x, vector.y);
					ApplyShadow(list, middleoutline, 0, list.Count, middleoutlineoffset.x, middleoutlineoffset.y);
					break;
				}
				case 2:
				{
					Color32 color6 = colorList[l];
					ApplyShadowZeroAlloc(list, color6, 0, list.Count, vector.x, vector.y);
					break;
				}
				case 0:
				{
					Color effectColor = base.effectColor;
					List<UIVertex> verts = list;
					Color32 color2 = effectColor;
					int count = list.Count;
					float x = vector.x;
					Vector2 effectDistance = base.effectDistance;
					float x2 = x + effectDistance.x;
					float y2 = vector.y;
					Vector2 effectDistance2 = base.effectDistance;
					ApplyShadowZeroAlloc(verts, color2, 0, count, x2, y2 + effectDistance2.y);
					List<UIVertex> verts2 = list;
					Color32 color3 = effectColor;
					int count2 = list.Count;
					float x3 = vector.x;
					Vector2 effectDistance3 = base.effectDistance;
					float x4 = x3 + effectDistance3.x;
					float y3 = vector.y;
					Vector2 effectDistance4 = base.effectDistance;
					ApplyShadowZeroAlloc(verts2, color3, 0, count2, x4, y3 - effectDistance4.y);
					List<UIVertex> verts3 = list;
					Color32 color4 = effectColor;
					int count3 = list.Count;
					float x5 = vector.x;
					Vector2 effectDistance5 = base.effectDistance;
					float x6 = x5 - effectDistance5.x;
					float y4 = vector.y;
					Vector2 effectDistance6 = base.effectDistance;
					ApplyShadowZeroAlloc(verts3, color4, 0, count3, x6, y4 + effectDistance6.y);
					List<UIVertex> verts4 = list;
					Color32 color5 = effectColor;
					int count4 = list.Count;
					float x7 = vector.x;
					Vector2 effectDistance7 = base.effectDistance;
					float x8 = x7 - effectDistance7.x;
					float y5 = vector.y;
					Vector2 effectDistance8 = base.effectDistance;
					ApplyShadowZeroAlloc(verts4, color5, 0, count4, x8, y5 - effectDistance8.y);
					break;
				}
				}
			}
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
			ListPool<UIVertex>.Release(list);
		}

		private byte GetAlpha(Color c, List<UIVertex> verts)
		{
			float a = c.a;
			UIVertex uIVertex = verts[0];
			return (byte)(a * (float)(int)uIVertex.color.a / 255f);
		}

		private void ApplyShadowZeroAllocSelf(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			for (int i = start; i < end; i++)
			{
				UIVertex value = verts[i];
				Vector3 position = value.position;
				position.x += x;
				position.y += y;
				value.position = position;
				Color32 color2 = color;
				byte a = color2.a;
				UIVertex uIVertex = verts[i];
				color2.a = (byte)(a * uIVertex.color.a / 255);
				value.color = color2;
				verts[i] = value;
			}
			ApplyShadowZeroAlloc(verts, color, start, end, x, y);
		}
	}
}
