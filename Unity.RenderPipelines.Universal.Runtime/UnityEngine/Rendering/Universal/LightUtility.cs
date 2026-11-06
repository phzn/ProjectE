using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Rendering.Universal.UTess;
using UnityEngine.U2D;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200002B RID: 43
	internal static class LightUtility
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000CB80 File Offset: 0x0000AD80
		public static bool CheckForChange(Light2D.LightType a, ref Light2D.LightType b)
		{
			bool flag = a != b;
			b = a;
			return flag;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000CB8D File Offset: 0x0000AD8D
		public static bool CheckForChange(int a, ref int b)
		{
			bool flag = a != b;
			b = a;
			return flag;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000CB9A File Offset: 0x0000AD9A
		public static bool CheckForChange(float a, ref float b)
		{
			bool flag = a != b;
			b = a;
			return flag;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000CBA7 File Offset: 0x0000ADA7
		public static bool CheckForChange(bool a, ref bool b)
		{
			bool flag = a != b;
			b = a;
			return flag;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000CBB4 File Offset: 0x0000ADB4
		private static bool TestPivot(List<IntPoint> path, int activePoint, long lastPoint)
		{
			for (int i = activePoint; i < path.Count; i++)
			{
				if (path[i].N > lastPoint)
				{
					return true;
				}
			}
			return path[activePoint].N == -1L;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		private static List<IntPoint> DegeneratePivots(List<IntPoint> path, List<IntPoint> inPath, ref int interiorStart)
		{
			List<IntPoint> list = new List<IntPoint>();
			long num = path[0].N;
			long num2 = path[0].N;
			for (int i = 1; i < path.Count; i++)
			{
				if (path[i].N != -1L)
				{
					num = Math.Min(num, path[i].N);
					num2 = Math.Max(num2, path[i].N);
				}
			}
			for (long num3 = 0L; num3 < num; num3 += 1L)
			{
				IntPoint intPoint = path[(int)num];
				intPoint.N = num3;
				list.Add(intPoint);
			}
			list.AddRange(path.GetRange(0, path.Count));
			interiorStart = list.Count;
			for (long num4 = num2 + 1L; num4 < (long)inPath.Count; num4 += 1L)
			{
				IntPoint intPoint2 = inPath[(int)num4];
				intPoint2.N = num4;
				list.Add(intPoint2);
			}
			return list;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		private static List<IntPoint> SortPivots(List<IntPoint> outPath, List<IntPoint> inPath)
		{
			List<IntPoint> list = new List<IntPoint>();
			IntPoint intPoint = outPath[0];
			long num = outPath[0].N;
			int num2 = 0;
			bool flag = true;
			for (int i = 1; i < outPath.Count; i++)
			{
				if (num > outPath[i].N && flag && outPath[i].N != -1L)
				{
					num = outPath[i].N;
					num2 = i;
					flag = false;
				}
				else if (outPath[i].N >= num)
				{
					num = outPath[i].N;
					flag = true;
				}
			}
			list.AddRange(outPath.GetRange(num2, outPath.Count - num2));
			list.AddRange(outPath.GetRange(0, num2));
			return list;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
		private static List<IntPoint> FixPivots(List<IntPoint> outPath, List<IntPoint> inPath, ref int interiorStart)
		{
			List<IntPoint> list = LightUtility.SortPivots(outPath, inPath);
			long num = list[0].N;
			for (int i = 1; i < list.Count; i++)
			{
				int num2 = ((i == list.Count - 1) ? 0 : (i + 1));
				IntPoint intPoint = list[i - 1];
				IntPoint intPoint2 = list[i];
				IntPoint intPoint3 = list[num2];
				if (intPoint.N > intPoint2.N && LightUtility.TestPivot(list, i, num))
				{
					if (intPoint.N == intPoint3.N)
					{
						intPoint2.N = intPoint.N;
					}
					else
					{
						intPoint2.N = ((num + 1L < (long)inPath.Count) ? (num + 1L) : 0L);
					}
					intPoint2.D = 3L;
					list[i] = intPoint2;
				}
				num = list[i].N;
			}
			int j = 1;
			while (j < list.Count - 1)
			{
				IntPoint intPoint4 = list[j - 1];
				IntPoint intPoint5 = list[j];
				IntPoint intPoint6 = list[j + 1];
				if (intPoint5.N - intPoint4.N > 1L)
				{
					if (intPoint5.N == intPoint6.N)
					{
						IntPoint intPoint7 = intPoint5;
						intPoint7.N -= 1L;
						list[j] = intPoint7;
					}
					else
					{
						IntPoint intPoint8 = intPoint5;
						intPoint8.N -= 1L;
						list.Insert(j, intPoint8);
					}
				}
				else
				{
					j++;
				}
			}
			return LightUtility.DegeneratePivots(list, inPath, ref interiorStart);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000CF34 File Offset: 0x0000B134
		internal static List<Vector2> GetOutlinePath(Vector3[] shapePath, float offsetDistance)
		{
			List<IntPoint> list = new List<IntPoint>();
			List<Vector2> list2 = new List<Vector2>();
			for (int i = 0; i < shapePath.Length; i++)
			{
				Vector2 vector = new Vector2(shapePath[i].x, shapePath[i].y) * 10000f;
				list.Add(new IntPoint((long)vector.x, (long)vector.y));
			}
			List<List<IntPoint>> list3 = new List<List<IntPoint>>();
			ClipperOffset clipperOffset = new ClipperOffset(24.0);
			clipperOffset.AddPath(list, JoinType.jtRound, EndType.etClosedPolygon);
			clipperOffset.Execute(ref list3, (double)(10000f * offsetDistance), list.Count);
			if (list3.Count > 0)
			{
				int num = 0;
				List<IntPoint> list4 = list3[0];
				list4 = LightUtility.FixPivots(list4, list, ref num);
				for (int j = 0; j < list4.Count; j++)
				{
					list2.Add(new Vector2((float)list4[j].X / 10000f, (float)list4[j].Y / 10000f));
				}
			}
			return list2;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000D040 File Offset: 0x0000B240
		private static void TransferToMesh(NativeArray<LightUtility.LightMeshVertex> vertices, int vertexCount, NativeArray<ushort> indices, int indexCount, Light2D light)
		{
			Mesh lightMesh = light.lightMesh;
			lightMesh.SetVertexBufferParams(vertexCount, LightUtility.LightMeshVertex.VertexLayout);
			lightMesh.SetVertexBufferData<LightUtility.LightMeshVertex>(vertices, 0, 0, vertexCount, 0, MeshUpdateFlags.Default);
			lightMesh.SetIndices<ushort>(indices, 0, indexCount, MeshTopology.Triangles, 0, true, 0);
			light.vertices = new LightUtility.LightMeshVertex[vertexCount];
			NativeArray<LightUtility.LightMeshVertex>.Copy(vertices, light.vertices, vertexCount);
			light.indices = new ushort[indexCount];
			NativeArray<ushort>.Copy(indices, light.indices, indexCount);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		public static Bounds GenerateShapeMesh(Light2D light, Vector3[] shapePath, float falloffDistance)
		{
			Random.State state = Random.state;
			Random.InitState(123456);
			Color color = new Color(0f, 0f, 0f, 1f);
			Color color2 = new Color(0f, 0f, 0f, 0f);
			int num = shapePath.Length;
			NativeArray<int2> nativeArray = new NativeArray<int2>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<float2> nativeArray2 = new NativeArray<float2>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < num; i++)
			{
				int num2 = i + 1;
				if (num2 == num)
				{
					num2 = 0;
				}
				int2 @int = new int2(i, num2);
				nativeArray[i] = @int;
				int x = @int.x;
				nativeArray2[x] = new float2(shapePath[x].x, shapePath[x].y);
			}
			NativeArray<int> nativeArray3 = new NativeArray<int>(nativeArray.Length * 8, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<float2> nativeArray4 = new NativeArray<float2>(nativeArray.Length * 8, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<int2> nativeArray5 = new NativeArray<int2>(nativeArray.Length * 8, Allocator.Temp, NativeArrayOptions.ClearMemory);
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			ModuleHandle.Tessellate(Allocator.Temp, nativeArray2, nativeArray, ref nativeArray4, ref num3, ref nativeArray3, ref num4, ref nativeArray5, ref num5);
			int num6 = shapePath.Length;
			List<IntPoint> list = new List<IntPoint>();
			for (int j = 0; j < num6; j++)
			{
				long num7 = (long)((double)shapePath[j].x * 10000.0);
				long num8 = (long)((double)shapePath[j].y * 10000.0);
				list.Add(new IntPoint(num7 + (long)Random.Range(-10, 10), num8 + (long)Random.Range(-10, 10))
				{
					N = (long)j,
					D = -1L
				});
			}
			int num9 = num6 - 1;
			int num10 = 0;
			List<List<IntPoint>> list2 = new List<List<IntPoint>>();
			ClipperOffset clipperOffset = new ClipperOffset(24.0);
			clipperOffset.AddPath(list, JoinType.jtRound, EndType.etClosedPolygon);
			clipperOffset.Execute(ref list2, (double)(10000f * falloffDistance), list.Count);
			if (list2.Count > 0)
			{
				List<IntPoint> list3 = list2[0];
				long num11 = (long)num6;
				for (int k = 0; k < list3.Count; k++)
				{
					num11 = ((list3[k].N != -1L) ? Math.Min(num11, list3[k].N) : num11);
				}
				bool flag = num11 == 0L;
				list3 = LightUtility.FixPivots(list3, list, ref num10);
				int num12 = num3 + list3.Count + num6;
				int num13 = num4 + list3.Count * 6 + 6;
				NativeArray<LightUtility.LightMeshVertex> nativeArray6 = new NativeArray<LightUtility.LightMeshVertex>(num12, Allocator.Temp, NativeArrayOptions.ClearMemory);
				NativeArray<ushort> nativeArray7 = new NativeArray<ushort>(num13, Allocator.Temp, NativeArrayOptions.ClearMemory);
				for (int l = 0; l < num4; l++)
				{
					nativeArray7[l] = (ushort)nativeArray3[l];
				}
				for (int m = 0; m < num3; m++)
				{
					int num14 = m;
					LightUtility.LightMeshVertex lightMeshVertex = new LightUtility.LightMeshVertex
					{
						position = new float3(nativeArray4[m].x, nativeArray4[m].y, 0f),
						color = color
					};
					nativeArray6[num14] = lightMeshVertex;
				}
				int num15 = num3;
				int num16 = num4;
				ushort[] array = new ushort[num6];
				for (int n = 0; n < num6; n++)
				{
					int num17 = num15++;
					LightUtility.LightMeshVertex lightMeshVertex = new LightUtility.LightMeshVertex
					{
						position = new float3(shapePath[n].x, shapePath[n].y, 0f),
						color = color
					};
					nativeArray6[num17] = lightMeshVertex;
					array[n] = (ushort)(num15 - 1);
				}
				ushort num18 = (ushort)num15;
				ushort num19 = num18;
				long num20 = ((list3[0].N == -1L) ? 0L : list3[0].N);
				for (int num21 = 0; num21 < list3.Count; num21++)
				{
					IntPoint intPoint = list3[num21];
					float2 @float = new float2((float)intPoint.X / 10000f, (float)intPoint.Y / 10000f);
					long num22 = ((intPoint.N == -1L) ? 0L : intPoint.N);
					int num23 = num15++;
					LightUtility.LightMeshVertex lightMeshVertex = new LightUtility.LightMeshVertex
					{
						position = new float3(@float.x, @float.y, 0f),
						color = ((num10 > num21) ? color2 : color)
					};
					nativeArray6[num23] = lightMeshVertex;
					if (num20 != num22)
					{
						nativeArray7[num16++] = array[(int)(checked((IntPtr)num20))];
						nativeArray7[num16++] = array[(int)(checked((IntPtr)num22))];
						nativeArray7[num16++] = (ushort)(num15 - 1);
					}
					nativeArray7[num16++] = array[(int)(checked((IntPtr)num20))];
					nativeArray7[num16++] = num18;
					num18 = (nativeArray7[num16++] = (ushort)(num15 - 1));
					num20 = num22;
				}
				nativeArray7[num16++] = num19;
				nativeArray7[num16++] = array[(int)(checked((IntPtr)num11))];
				nativeArray7[num16++] = (flag ? array[num9] : num18);
				nativeArray7[num16++] = (flag ? num19 : num18);
				nativeArray7[num16++] = (flag ? num18 : array[(int)(checked((IntPtr)num11))]);
				if (flag)
				{
					float num24 = 0.001f;
					ushort num25 = array[num9];
					bool flag2 = MathF.Abs(nativeArray6[(int)num25].position.x - nativeArray6[(int)nativeArray7[num16 - 1]].position.x) > num24 || MathF.Abs(nativeArray6[(int)num25].position.y - nativeArray6[(int)nativeArray7[num16 - 1]].position.y) > num24;
					bool flag3 = MathF.Abs(nativeArray6[(int)num25].position.x - nativeArray6[(int)nativeArray7[num16 - 2]].position.x) > num24 || MathF.Abs(nativeArray6[(int)num25].position.y - nativeArray6[(int)nativeArray7[num16 - 2]].position.y) > num24;
					if (!flag2 || !flag3)
					{
						num25 = (ushort)(num10 + num6 + num3 - 1);
					}
					nativeArray7[num16++] = num25;
				}
				else
				{
					nativeArray7[num16++] = array[(int)(checked((IntPtr)(unchecked(num11 - 1L))))];
				}
				LightUtility.TransferToMesh(nativeArray6, num15, nativeArray7, num16, light);
			}
			Random.state = state;
			return light.lightMesh.GetSubMesh(0).bounds;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000D780 File Offset: 0x0000B980
		public static Bounds GenerateParametricMesh(Light2D light, float radius, float falloffDistance, float angle, int sides)
		{
			float num = 1.5707964f + 0.017453292f * angle;
			if (sides < 3)
			{
				radius = 0.70710677f * radius;
				sides = 4;
			}
			if (sides == 4)
			{
				num = 0.7853982f + 0.017453292f * angle;
			}
			int num2 = 1 + 2 * sides;
			int num3 = 9 * sides;
			NativeArray<LightUtility.LightMeshVertex> nativeArray = new NativeArray<LightUtility.LightMeshVertex>(num2, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<ushort> nativeArray2 = new NativeArray<ushort>(num3, Allocator.Temp, NativeArrayOptions.ClearMemory);
			ushort num4 = (ushort)(2 * sides);
			Mesh lightMesh = light.lightMesh;
			Color color = new Color(0f, 0f, 0f, 1f);
			nativeArray[(int)num4] = new LightUtility.LightMeshVertex
			{
				position = float3.zero,
				color = color
			};
			float num5 = 6.2831855f / (float)sides;
			float3 @float = new float3(float.MaxValue, float.MaxValue, 0f);
			float3 float2 = new float3(float.MinValue, float.MinValue, 0f);
			for (int i = 0; i < sides; i++)
			{
				float num6 = (float)(i + 1) * num5;
				float3 float3 = new float3(math.cos(num6 + num), math.sin(num6 + num), 0f);
				float3 float4 = radius * float3;
				int num7 = (2 * i + 2) % (2 * sides);
				nativeArray[num7] = new LightUtility.LightMeshVertex
				{
					position = float4,
					color = new Color(float3.x, float3.y, 0f, 0f)
				};
				nativeArray[num7 + 1] = new LightUtility.LightMeshVertex
				{
					position = float4,
					color = color
				};
				int num8 = 9 * i;
				nativeArray2[num8] = (ushort)(num7 + 1);
				nativeArray2[num8 + 1] = (ushort)(2 * i + 1);
				nativeArray2[num8 + 2] = num4;
				nativeArray2[num8 + 3] = (ushort)num7;
				nativeArray2[num8 + 4] = (ushort)(2 * i);
				nativeArray2[num8 + 5] = (ushort)(2 * i + 1);
				nativeArray2[num8 + 6] = (ushort)(num7 + 1);
				nativeArray2[num8 + 7] = (ushort)num7;
				nativeArray2[num8 + 8] = (ushort)(2 * i + 1);
				@float = math.min(@float, float4 + float3 * falloffDistance);
				float2 = math.max(float2, float4 + float3 * falloffDistance);
			}
			lightMesh.SetVertexBufferParams(num2, LightUtility.LightMeshVertex.VertexLayout);
			lightMesh.SetVertexBufferData<LightUtility.LightMeshVertex>(nativeArray, 0, 0, num2, 0, MeshUpdateFlags.Default);
			lightMesh.SetIndices<ushort>(nativeArray2, MeshTopology.Triangles, 0, false, 0);
			light.vertices = new LightUtility.LightMeshVertex[num2];
			NativeArray<LightUtility.LightMeshVertex>.Copy(nativeArray, light.vertices, num2);
			light.indices = new ushort[num3];
			NativeArray<ushort>.Copy(nativeArray2, light.indices, num3);
			return new Bounds
			{
				min = @float,
				max = float2
			};
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000DA7C File Offset: 0x0000BC7C
		public static Bounds GenerateSpriteMesh(Light2D light, Sprite sprite)
		{
			Mesh lightMesh = light.lightMesh;
			if (sprite == null)
			{
				lightMesh.Clear();
				return new Bounds(Vector3.zero, Vector3.zero);
			}
			Vector2[] uv = sprite.uv;
			NativeSlice<Vector3> vertexAttribute = sprite.GetVertexAttribute<Vector3>(VertexAttribute.Position);
			NativeSlice<Vector2> vertexAttribute2 = sprite.GetVertexAttribute<Vector2>(VertexAttribute.TexCoord0);
			NativeArray<ushort> indices = sprite.GetIndices();
			0.5f * (sprite.bounds.min + sprite.bounds.max);
			NativeArray<LightUtility.LightMeshVertex> nativeArray = new NativeArray<LightUtility.LightMeshVertex>(indices.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			Color color = new Color(0f, 0f, 0f, 1f);
			for (int i = 0; i < vertexAttribute.Length; i++)
			{
				nativeArray[i] = new LightUtility.LightMeshVertex
				{
					position = new Vector3(vertexAttribute[i].x, vertexAttribute[i].y, 0f),
					color = color,
					uv = vertexAttribute2[i]
				};
			}
			lightMesh.SetVertexBufferParams(nativeArray.Length, LightUtility.LightMeshVertex.VertexLayout);
			lightMesh.SetVertexBufferData<LightUtility.LightMeshVertex>(nativeArray, 0, 0, nativeArray.Length, 0, MeshUpdateFlags.Default);
			lightMesh.SetIndices<ushort>(indices, MeshTopology.Triangles, 0, true, 0);
			light.vertices = new LightUtility.LightMeshVertex[nativeArray.Length];
			NativeArray<LightUtility.LightMeshVertex>.Copy(nativeArray, light.vertices, nativeArray.Length);
			light.indices = new ushort[indices.Length];
			NativeArray<ushort>.Copy(indices, light.indices, indices.Length);
			return lightMesh.GetSubMesh(0).bounds;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000DC20 File Offset: 0x0000BE20
		public static int GetShapePathHash(Vector3[] path)
		{
			int num = -2128831035;
			if (path != null)
			{
				foreach (Vector3 vector in path)
				{
					num = (num * 16777619) ^ vector.GetHashCode();
				}
			}
			else
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x0200015A RID: 346
		private enum PivotType
		{
			// Token: 0x04000991 RID: 2449
			PivotBase,
			// Token: 0x04000992 RID: 2450
			PivotCurve,
			// Token: 0x04000993 RID: 2451
			PivotIntersect,
			// Token: 0x04000994 RID: 2452
			PivotSkip,
			// Token: 0x04000995 RID: 2453
			PivotClip
		}

		// Token: 0x0200015B RID: 347
		[Serializable]
		internal struct LightMeshVertex
		{
			// Token: 0x04000996 RID: 2454
			public Vector3 position;

			// Token: 0x04000997 RID: 2455
			public Color color;

			// Token: 0x04000998 RID: 2456
			public Vector2 uv;

			// Token: 0x04000999 RID: 2457
			public static readonly VertexAttributeDescriptor[] VertexLayout = new VertexAttributeDescriptor[]
			{
				new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
				new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 0)
			};
		}
	}
}
