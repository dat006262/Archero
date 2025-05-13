




using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dxx.UI
{
	[AddComponentMenu("UI/Effects/Gradient Angle Dxx")]
	public class GradientAngle : BaseMeshEffect
	{
		[SerializeField]
		private Color32 startColor = Color.white;

		[SerializeField]
		private Color32 endColor = Color.black;

		[SerializeField]
		[Range(0f, 360f)]
		[Tooltip("渐变旋转角度")]
		private float angle;

		[SerializeField]
		[Tooltip("渐变偏移")]
		private float offset;

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
			{
				return;
			}
			List<UIVertex> list = ListPool<UIVertex>.Get();
			vh.GetUIVertexStream(list);
			int count = list.Count;
			if (count > 0)
			{
				UIVertex uIVertex = list[0];
				float num = uIVertex.position.y;
				UIVertex uIVertex2 = list[0];
				float num2 = uIVertex2.position.y;
				UIVertex uIVertex3 = list[0];
				float num3 = uIVertex3.position.x;
				UIVertex uIVertex4 = list[0];
				float num4 = uIVertex4.position.x;
				for (int i = 1; i < count; i++)
				{
					UIVertex uIVertex5 = list[i];
					float x = uIVertex5.position.x;
					UIVertex uIVertex6 = list[i];
					float y = uIVertex6.position.y;
					if (y > num2)
					{
						num2 = y;
					}
					if (y < num)
					{
						num = y;
					}
					if (x < num3)
					{
						num3 = x;
					}
					if (x > num4)
					{
						num4 = x;
					}
				}
				float num5 = num2 - num;
				float num6 = num4 - num3;
				int num7 = (int)(angle / 90f);
				float num8 = angle - (float)num7 * 90f;
				if (num7 % 2 == 1)
				{
					num8 = 90f - num8;
				}
				float k = Mathf.Tan(num8 / 180f * (float)Math.PI);
				Vector2 pOut = new Vector2(num6 * 0.5f, num5 * 0.5f);
				Vector2 pOut2 = new Vector2((0f - num6) * 0.5f, (0f - num5) * 0.5f);
				if (num7 % 2 == 1)
				{
					pOut = new Vector2((0f - num6) * 0.5f, num5 * 0.5f);
					pOut2 = new Vector2(num6 * 0.5f, (0f - num5) * 0.5f);
				}
				Vector2 projectivePoint = GetProjectivePoint(Vector2.zero, k, pOut);
				Vector2 projectivePoint2 = GetProjectivePoint(Vector2.zero, k, pOut2);
				float num9 = Vector2.Distance(projectivePoint, projectivePoint2);
				for (int j = 0; j < count; j++)
				{
					UIVertex uIVertex7 = list[j];
					float x2 = uIVertex7.position.x - (num3 + num6 * 0.5f);
					UIVertex uIVertex8 = list[j];
					float y2 = uIVertex8.position.y - (num + num5 * 0.5f);
					Vector2 projectivePoint3 = GetProjectivePoint(Vector2.zero, k, new Vector2(x2, y2));
					float num10 = Vector2.Distance(projectivePoint3, projectivePoint);
					Color white = Color.white;
					float num11 = num10 / num9;
					num11 += Mathf.Abs(offset);
					num11 -= (float)(int)num11;
					white = ((num7 % 4 >= 2) ? ((Color)Color32.Lerp(endColor, startColor, num11)) : ((Color)Color32.Lerp(startColor, endColor, num11)));
					UIVertex value = default(UIVertex);
					value.color = white;
					UIVertex uIVertex9 = list[j];
					value.normal = uIVertex9.normal;
					UIVertex uIVertex10 = list[j];
					value.position = uIVertex10.position;
					UIVertex uIVertex11 = list[j];
					value.tangent = uIVertex11.tangent;
					UIVertex uIVertex12 = list[j];
					value.uv0 = uIVertex12.uv0;
					UIVertex uIVertex13 = list[j];
					value.uv1 = uIVertex13.uv1;
					list[j] = value;
				}
				vh.Clear();
				vh.AddUIVertexTriangleStream(list);
			}
			ListPool<UIVertex>.Release(list);
		}

		protected Vector2 GetProjectivePoint(Vector2 pLine, float k, Vector2 pOut)
		{
			Vector2 zero = Vector2.zero;
			if (k == 0f || k == float.NaN)
			{
				zero.x = pOut.x;
				zero.y = pLine.y;
			}
			else
			{
				zero.x = (k * pLine.x + pOut.x / k + pOut.y - pLine.y) / (1f / k + k);
				zero.y = -1f / k * (zero.x - pOut.x) + pOut.y;
			}
			return zero;
		}
	}
}
