using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CompassNavigatorPro
{
	// Token: 0x0200029F RID: 671
	[AddComponentMenu("")]
	public class CompassBarMeshModifier : BaseMeshEffect
	{
		// Token: 0x06001333 RID: 4915 RVA: 0x0007C89C File Offset: 0x0007AA9C
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive() || vh == null || vh.currentVertCount < 1)
			{
				return;
			}
			Mesh mesh = new Mesh();
			vh.FillMesh(mesh);
			List<Vector3> list = new List<Vector3>();
			List<int> list2 = new List<int>();
			List<Vector2> list3 = new List<Vector2>();
			mesh.GetVertices(list);
			mesh.GetTriangles(list2, 0);
			mesh.GetUVs(0, list3);
			List<Vector3> list4 = new List<Vector3>();
			List<int> list5 = new List<int>();
			List<Vector2> list6 = new List<Vector2>();
			int count = list2.Count;
			int num = 0;
			for (int i = 0; i < count; i += 6)
			{
				int num2 = list2[i];
				int num3 = list2[i + 1];
				int num4 = list2[i + 2];
				int num5 = list2[i + 4];
				Vector3 vector = list[num2];
				Vector3 vector2 = list[num3];
				Vector3 vector3 = list[num4];
				Vector3 vector4 = list[num5];
				if (vector.x != vector4.x && vector.y != vector2.y)
				{
					Vector2 vector5 = list3[num2];
					Vector2 vector6 = list3[num3];
					Vector2 vector7 = list3[num4];
					Vector2 vector8 = list3[num5];
					float num6 = vector4.x - vector.x;
					float num7 = vector8.x - vector5.x;
					int num8 = (int)(num6 / 50f);
					float num9 = num6 / (float)(num8 + 1);
					float num10 = num7 / (float)(num8 + 1);
					for (int j = 0; j < num8; j++)
					{
						float num11 = vector.x + num9;
						float num12 = vector5.x + num10;
						Vector3 vector9 = new Vector3(num11, vector2.y, 0f);
						Vector2 vector10 = new Vector2(num12, vector6.y);
						Vector3 vector11 = new Vector3(num11, vector.y, 0f);
						Vector2 vector12 = new Vector2(num12, vector5.y);
						list4.Add(vector);
						list4.Add(vector2);
						list4.Add(vector9);
						list4.Add(vector11);
						list6.Add(vector5);
						list6.Add(vector6);
						list6.Add(vector10);
						list6.Add(vector12);
						list5.Add(num);
						list5.Add(num + 1);
						list5.Add(num + 2);
						list5.Add(num);
						list5.Add(num + 2);
						list5.Add(num + 3);
						num += 4;
						vector = vector11;
						vector2 = vector9;
						vector5 = vector12;
						vector6 = vector10;
					}
					list4.Add(vector);
					list4.Add(vector2);
					list4.Add(vector3);
					list4.Add(vector4);
					list6.Add(vector5);
					list6.Add(vector6);
					list6.Add(vector7);
					list6.Add(vector8);
					list5.Add(num);
					list5.Add(num + 1);
					list5.Add(num + 2);
					list5.Add(num);
					list5.Add(num + 2);
					list5.Add(num + 3);
					num += 4;
				}
			}
			int count2 = list4.Count;
			List<UIVertex> list7 = new List<UIVertex>(count2);
			UIVertex uivertex = default(UIVertex);
			vh.PopulateUIVertex(ref uivertex, 0);
			for (int k = 0; k < count2; k++)
			{
				uivertex.position = list4[k];
				uivertex.uv0 = list6[k];
				list7.Add(uivertex);
			}
			vh.Clear();
			vh.AddUIVertexStream(list7, list5);
		}
	}
}
