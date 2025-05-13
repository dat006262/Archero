




using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dxx.UI
{
	[AddComponentMenu("UI/Effects/TextColorDxx")]
	[RequireComponent(typeof(Text))]
	public class TextColorDxx : BaseMeshEffect
	{
		public Color32 topColor;

		public Color32 bottomColor;

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
			{
				return;
			}
			int currentVertCount = vh.currentVertCount;
			if (currentVertCount == 0)
			{
				return;
			}
			List<UIVertex> list = ListPool<UIVertex>.Get();
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
		}
	}
}
