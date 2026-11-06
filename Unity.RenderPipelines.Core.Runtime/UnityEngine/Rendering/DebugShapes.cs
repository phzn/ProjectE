using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000067 RID: 103
	public class DebugShapes
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000EC76 File Offset: 0x0000CE76
		public static DebugShapes instance
		{
			get
			{
				if (DebugShapes.s_Instance == null)
				{
					DebugShapes.s_Instance = new DebugShapes();
				}
				return DebugShapes.s_Instance;
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000EC90 File Offset: 0x0000CE90
		private void BuildSphere(ref Mesh outputMesh, float radius, uint longSubdiv, uint latSubdiv)
		{
			outputMesh.Clear();
			Vector3[] array = new Vector3[(longSubdiv + 1U) * latSubdiv + 2U];
			float num = 3.1415927f;
			float num2 = num * 2f;
			array[0] = Vector3.up * radius;
			int num3 = 0;
			while ((long)num3 < (long)((ulong)latSubdiv))
			{
				float num4 = num * (float)(num3 + 1) / (latSubdiv + 1U);
				float num5 = Mathf.Sin(num4);
				float num6 = Mathf.Cos(num4);
				int num7 = 0;
				while ((long)num7 <= (long)((ulong)longSubdiv))
				{
					float num8 = num2 * (float)(((long)num7 == (long)((ulong)longSubdiv)) ? 0 : num7) / longSubdiv;
					float num9 = Mathf.Sin(num8);
					float num10 = Mathf.Cos(num8);
					array[(int)(checked((IntPtr)(unchecked((long)num7 + (long)num3 * (long)((ulong)(longSubdiv + 1U)) + 1L))))] = new Vector3(num5 * num10, num6, num5 * num9) * radius;
					num7++;
				}
				num3++;
			}
			array[array.Length - 1] = Vector3.up * -radius;
			Vector3[] array2 = new Vector3[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i].normalized;
			}
			Vector2[] array3 = new Vector2[array.Length];
			array3[0] = Vector2.up;
			array3[array3.Length - 1] = Vector2.zero;
			int num11 = 0;
			while ((long)num11 < (long)((ulong)latSubdiv))
			{
				int num12 = 0;
				while ((long)num12 <= (long)((ulong)longSubdiv))
				{
					array3[(int)(checked((IntPtr)(unchecked((long)num12 + (long)num11 * (long)((ulong)(longSubdiv + 1U)) + 1L))))] = new Vector2((float)num12 / longSubdiv, 1f - (float)(num11 + 1) / (latSubdiv + 1U));
					num12++;
				}
				num11++;
			}
			int[] array4 = new int[array.Length * 2 * 3];
			int num13 = 0;
			int num14 = 0;
			while ((long)num14 < (long)((ulong)longSubdiv))
			{
				array4[num13++] = num14 + 2;
				array4[num13++] = num14 + 1;
				array4[num13++] = 0;
				num14++;
			}
			for (uint num15 = 0U; num15 < latSubdiv - 1U; num15 += 1U)
			{
				for (uint num16 = 0U; num16 < longSubdiv; num16 += 1U)
				{
					uint num17 = num16 + num15 * (longSubdiv + 1U) + 1U;
					uint num18 = num17 + longSubdiv + 1U;
					array4[num13++] = (int)num17;
					array4[num13++] = (int)(num17 + 1U);
					array4[num13++] = (int)(num18 + 1U);
					array4[num13++] = (int)num17;
					array4[num13++] = (int)(num18 + 1U);
					array4[num13++] = (int)num18;
				}
			}
			int num19 = 0;
			while ((long)num19 < (long)((ulong)longSubdiv))
			{
				array4[num13++] = array.Length - 1;
				array4[num13++] = array.Length - (num19 + 2) - 1;
				array4[num13++] = array.Length - (num19 + 1) - 1;
				num19++;
			}
			outputMesh.vertices = array;
			outputMesh.normals = array2;
			outputMesh.uv = array3;
			outputMesh.triangles = array4;
			outputMesh.RecalculateBounds();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000EF78 File Offset: 0x0000D178
		private void BuildBox(ref Mesh outputMesh, float length, float width, float height)
		{
			outputMesh.Clear();
			Vector3 vector = new Vector3(-length * 0.5f, -width * 0.5f, height * 0.5f);
			Vector3 vector2 = new Vector3(length * 0.5f, -width * 0.5f, height * 0.5f);
			Vector3 vector3 = new Vector3(length * 0.5f, -width * 0.5f, -height * 0.5f);
			Vector3 vector4 = new Vector3(-length * 0.5f, -width * 0.5f, -height * 0.5f);
			Vector3 vector5 = new Vector3(-length * 0.5f, width * 0.5f, height * 0.5f);
			Vector3 vector6 = new Vector3(length * 0.5f, width * 0.5f, height * 0.5f);
			Vector3 vector7 = new Vector3(length * 0.5f, width * 0.5f, -height * 0.5f);
			Vector3 vector8 = new Vector3(-length * 0.5f, width * 0.5f, -height * 0.5f);
			Vector3[] array = new Vector3[]
			{
				vector, vector2, vector3, vector4, vector8, vector5, vector, vector4, vector5, vector6,
				vector2, vector, vector7, vector8, vector4, vector3, vector6, vector7, vector3, vector2,
				vector8, vector7, vector6, vector5
			};
			Vector3 up = Vector3.up;
			Vector3 down = Vector3.down;
			Vector3 forward = Vector3.forward;
			Vector3 back = Vector3.back;
			Vector3 left = Vector3.left;
			Vector3 right = Vector3.right;
			Vector3[] array2 = new Vector3[]
			{
				down, down, down, down, left, left, left, left, forward, forward,
				forward, forward, back, back, back, back, right, right, right, right,
				up, up, up, up
			};
			Vector2 vector9 = new Vector2(0f, 0f);
			Vector2 vector10 = new Vector2(1f, 0f);
			Vector2 vector11 = new Vector2(0f, 1f);
			Vector2 vector12 = new Vector2(1f, 1f);
			Vector2[] array3 = new Vector2[]
			{
				vector12, vector11, vector9, vector10, vector12, vector11, vector9, vector10, vector12, vector11,
				vector9, vector10, vector12, vector11, vector9, vector10, vector12, vector11, vector9, vector10,
				vector12, vector11, vector9, vector10
			};
			int[] array4 = new int[]
			{
				3, 1, 0, 3, 2, 1, 7, 5, 4, 7,
				6, 5, 11, 9, 8, 11, 10, 9, 15, 13,
				12, 15, 14, 13, 19, 17, 16, 19, 18, 17,
				23, 21, 20, 23, 22, 21
			};
			outputMesh.vertices = array;
			outputMesh.normals = array2;
			outputMesh.uv = array3;
			outputMesh.triangles = array4;
			outputMesh.RecalculateBounds();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000F3F4 File Offset: 0x0000D5F4
		private void BuildCone(ref Mesh outputMesh, float height, float topRadius, float bottomRadius, int nbSides)
		{
			outputMesh.Clear();
			int num = nbSides + 1;
			Vector3[] array = new Vector3[num + num + nbSides * 2 + 2];
			int i = 0;
			float num2 = 6.2831855f;
			array[i++] = new Vector3(0f, 0f, 0f);
			while (i <= nbSides)
			{
				float num3 = (float)i / (float)nbSides * num2;
				array[i] = new Vector3(Mathf.Sin(num3) * bottomRadius, Mathf.Cos(num3) * bottomRadius, 0f);
				i++;
			}
			array[i++] = new Vector3(0f, 0f, height);
			while (i <= nbSides * 2 + 1)
			{
				float num4 = (float)(i - nbSides - 1) / (float)nbSides * num2;
				array[i] = new Vector3(Mathf.Sin(num4) * topRadius, Mathf.Cos(num4) * topRadius, height);
				i++;
			}
			int num5 = 0;
			while (i <= array.Length - 4)
			{
				float num6 = (float)num5 / (float)nbSides * num2;
				array[i] = new Vector3(Mathf.Sin(num6) * topRadius, Mathf.Cos(num6) * topRadius, height);
				array[i + 1] = new Vector3(Mathf.Sin(num6) * bottomRadius, Mathf.Cos(num6) * bottomRadius, 0f);
				i += 2;
				num5++;
			}
			array[i] = array[nbSides * 2 + 2];
			array[i + 1] = array[nbSides * 2 + 3];
			Vector3[] array2 = new Vector3[array.Length];
			i = 0;
			while (i <= nbSides)
			{
				array2[i++] = new Vector3(0f, 0f, -1f);
			}
			while (i <= nbSides * 2 + 1)
			{
				array2[i++] = new Vector3(0f, 0f, 1f);
			}
			num5 = 0;
			while (i <= array.Length - 4)
			{
				float num7 = (float)num5 / (float)nbSides * num2;
				float num8 = Mathf.Cos(num7);
				float num9 = Mathf.Sin(num7);
				array2[i] = new Vector3(num9, num8, 0f);
				array2[i + 1] = array2[i];
				i += 2;
				num5++;
			}
			array2[i] = array2[nbSides * 2 + 2];
			array2[i + 1] = array2[nbSides * 2 + 3];
			Vector2[] array3 = new Vector2[array.Length];
			int j = 0;
			array3[j++] = new Vector2(0.5f, 0.5f);
			while (j <= nbSides)
			{
				float num10 = (float)j / (float)nbSides * num2;
				array3[j] = new Vector2(Mathf.Cos(num10) * 0.5f + 0.5f, Mathf.Sin(num10) * 0.5f + 0.5f);
				j++;
			}
			array3[j++] = new Vector2(0.5f, 0.5f);
			while (j <= nbSides * 2 + 1)
			{
				float num11 = (float)j / (float)nbSides * num2;
				array3[j] = new Vector2(Mathf.Cos(num11) * 0.5f + 0.5f, Mathf.Sin(num11) * 0.5f + 0.5f);
				j++;
			}
			int num12 = 0;
			while (j <= array3.Length - 4)
			{
				float num13 = (float)num12 / (float)nbSides;
				array3[j] = new Vector3(num13, 1f);
				array3[j + 1] = new Vector3(num13, 0f);
				j += 2;
				num12++;
			}
			array3[j] = new Vector2(1f, 1f);
			array3[j + 1] = new Vector2(1f, 0f);
			int num14 = nbSides + nbSides + nbSides * 2;
			int[] array4 = new int[num14 * 3 + 3];
			int k = 0;
			int num15 = 0;
			while (k < nbSides - 1)
			{
				array4[num15] = 0;
				array4[num15 + 1] = k + 1;
				array4[num15 + 2] = k + 2;
				k++;
				num15 += 3;
			}
			array4[num15] = 0;
			array4[num15 + 1] = k + 1;
			array4[num15 + 2] = 1;
			k++;
			num15 += 3;
			while (k < nbSides * 2)
			{
				array4[num15] = k + 2;
				array4[num15 + 1] = k + 1;
				array4[num15 + 2] = num;
				k++;
				num15 += 3;
			}
			array4[num15] = num + 1;
			array4[num15 + 1] = k + 1;
			array4[num15 + 2] = num;
			k++;
			num15 += 3;
			k++;
			while (k <= num14)
			{
				array4[num15] = k + 2;
				array4[num15 + 1] = k + 1;
				array4[num15 + 2] = k;
				k++;
				num15 += 3;
				array4[num15] = k + 1;
				array4[num15 + 1] = k + 2;
				array4[num15 + 2] = k;
				k++;
				num15 += 3;
			}
			outputMesh.vertices = array;
			outputMesh.normals = array2;
			outputMesh.uv = array3;
			outputMesh.triangles = array4;
			outputMesh.RecalculateBounds();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000F914 File Offset: 0x0000DB14
		private void BuildPyramid(ref Mesh outputMesh, float width, float height, float depth)
		{
			outputMesh.Clear();
			Vector3[] array = new Vector3[]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(-width / 2f, height / 2f, depth),
				new Vector3(width / 2f, height / 2f, depth),
				new Vector3(0f, 0f, 0f),
				new Vector3(width / 2f, height / 2f, depth),
				new Vector3(width / 2f, -height / 2f, depth),
				new Vector3(0f, 0f, 0f),
				new Vector3(width / 2f, -height / 2f, depth),
				new Vector3(-width / 2f, -height / 2f, depth),
				new Vector3(0f, 0f, 0f),
				new Vector3(-width / 2f, -height / 2f, depth),
				new Vector3(-width / 2f, height / 2f, depth),
				new Vector3(-width / 2f, height / 2f, depth),
				new Vector3(-width / 2f, -height / 2f, depth),
				new Vector3(width / 2f, -height / 2f, depth),
				new Vector3(width / 2f, height / 2f, depth)
			};
			Vector3[] array2 = new Vector3[array.Length];
			Vector2[] array3 = new Vector2[array.Length];
			int[] array4 = new int[18];
			for (int i = 0; i < 12; i++)
			{
				array4[i] = i;
			}
			array4[12] = 12;
			array4[13] = 13;
			array4[14] = 14;
			array4[15] = 12;
			array4[16] = 14;
			array4[17] = 15;
			outputMesh.vertices = array;
			outputMesh.normals = array2;
			outputMesh.uv = array3;
			outputMesh.triangles = array4;
			outputMesh.RecalculateBounds();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000FB7C File Offset: 0x0000DD7C
		private void BuildShapes()
		{
			this.m_sphereMesh = new Mesh();
			this.BuildSphere(ref this.m_sphereMesh, 1f, 24U, 16U);
			this.m_boxMesh = new Mesh();
			this.BuildBox(ref this.m_boxMesh, 1f, 1f, 1f);
			this.m_coneMesh = new Mesh();
			this.BuildCone(ref this.m_coneMesh, 1f, 1f, 0f, 16);
			this.m_pyramidMesh = new Mesh();
			this.BuildPyramid(ref this.m_pyramidMesh, 1f, 1f, 1f);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000FC1D File Offset: 0x0000DE1D
		private void RebuildResources()
		{
			if (this.m_sphereMesh == null || this.m_boxMesh == null || this.m_coneMesh == null || this.m_pyramidMesh == null)
			{
				this.BuildShapes();
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000FC5D File Offset: 0x0000DE5D
		public Mesh RequestSphereMesh()
		{
			this.RebuildResources();
			return this.m_sphereMesh;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000FC6B File Offset: 0x0000DE6B
		public Mesh RequestBoxMesh()
		{
			this.RebuildResources();
			return this.m_boxMesh;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000FC79 File Offset: 0x0000DE79
		public Mesh RequestConeMesh()
		{
			this.RebuildResources();
			return this.m_coneMesh;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000FC87 File Offset: 0x0000DE87
		public Mesh RequestPyramidMesh()
		{
			this.RebuildResources();
			return this.m_pyramidMesh;
		}

		// Token: 0x040001EA RID: 490
		private static DebugShapes s_Instance;

		// Token: 0x040001EB RID: 491
		private Mesh m_sphereMesh;

		// Token: 0x040001EC RID: 492
		private Mesh m_boxMesh;

		// Token: 0x040001ED RID: 493
		private Mesh m_coneMesh;

		// Token: 0x040001EE RID: 494
		private Mesh m_pyramidMesh;
	}
}
