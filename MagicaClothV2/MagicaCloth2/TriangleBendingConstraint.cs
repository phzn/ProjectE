using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000059 RID: 89
	public class TriangleBendingConstraint : IDisposable
	{
		// Token: 0x0600013D RID: 317 RVA: 0x0000DB9B File Offset: 0x0000BD9B
		public TriangleBendingConstraint()
		{
			this.trianglePairArray = new ExNativeArray<ulong>(0, true);
			this.restAngleOrVolumeArray = new ExNativeArray<float>(0, true);
			this.signOrVolumeArray = new ExNativeArray<sbyte>(0, true);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000DBCC File Offset: 0x0000BDCC
		public void Dispose()
		{
			ExNativeArray<ulong> exNativeArray = this.trianglePairArray;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			ExNativeArray<float> exNativeArray2 = this.restAngleOrVolumeArray;
			if (exNativeArray2 != null)
			{
				exNativeArray2.Dispose();
			}
			ExNativeArray<sbyte> exNativeArray3 = this.signOrVolumeArray;
			if (exNativeArray3 != null)
			{
				exNativeArray3.Dispose();
			}
			this.trianglePairArray = null;
			this.restAngleOrVolumeArray = null;
			this.signOrVolumeArray = null;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000DC24 File Offset: 0x0000BE24
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[TriangleBendConstraint]");
			stringBuilder.AppendLine("  -trianglePairArray:" + this.trianglePairArray.ToSummary());
			stringBuilder.AppendLine("  -restAngleOrVolumeArray:" + this.restAngleOrVolumeArray.ToSummary());
			stringBuilder.AppendLine("  -signOrVolumeArray:" + this.signOrVolumeArray.ToSummary());
			return stringBuilder.ToString();
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000DC9C File Offset: 0x0000BE9C
		public static TriangleBendingConstraint.ConstraintData CreateData(VirtualMesh proxyMesh, in ClothParameters parameters)
		{
			TriangleBendingConstraint.ConstraintData constraintData = new TriangleBendingConstraint.ConstraintData();
			try
			{
				if (proxyMesh.TriangleCount == 0)
				{
					return null;
				}
				int edgeCount = proxyMesh.EdgeCount;
				if (edgeCount == 0)
				{
					return null;
				}
				List<ulong> list = new List<ulong>(edgeCount * 2);
				List<float> list2 = new List<float>(edgeCount * 2);
				List<sbyte> list3 = new List<sbyte>(edgeCount * 2);
				List<uint> list4 = new List<uint>(edgeCount * 2);
				HashSet<int4> hashSet = new HashSet<int4>();
				int num = 0;
				int num2 = 0;
				int vertexCount = proxyMesh.VertexCount;
				using (MultiDataBuilder<byte> multiDataBuilder = new MultiDataBuilder<byte>(vertexCount, vertexCount * 2))
				{
					for (int i = 0; i < edgeCount; i++)
					{
						int2 @int = proxyMesh.edges[i];
						if (proxyMesh.edgeToTriangles.ContainsKey(@int))
						{
							FixedList128Bytes<ushort> fixedList128Bytes = (ref proxyMesh.edgeToTriangles).MC2ToFixedList128Bytes(@int);
							int length = fixedList128Bytes.Length;
							for (int j = 0; j < length - 1; j++)
							{
								int num3 = (int)fixedList128Bytes[j];
								int3 int2 = proxyMesh.triangles[num3];
								for (int k = j + 1; k < length; k++)
								{
									int num4 = (int)fixedList128Bytes[k];
									int3 int3 = proxyMesh.triangles[num4];
									int2 restTriangleVertex = MathUtility.GetRestTriangleVertex(int2, int3, @int);
									int4 int4 = new int4(restTriangleVertex.x, restTriangleVertex.y, @int.x, @int.y);
									VertexAttribute vertexAttribute = proxyMesh.attributes[int4.x];
									VertexAttribute vertexAttribute2 = proxyMesh.attributes[int4.y];
									VertexAttribute vertexAttribute3 = proxyMesh.attributes[int4.z];
									VertexAttribute vertexAttribute4 = proxyMesh.attributes[int4.w];
									if ((!vertexAttribute.IsDontMove() || !vertexAttribute2.IsDontMove() || !vertexAttribute3.IsDontMove() || !vertexAttribute4.IsDontMove()) && !vertexAttribute.IsInvalid() && !vertexAttribute2.IsInvalid() && !vertexAttribute3.IsInvalid() && !vertexAttribute4.IsInvalid())
									{
										ulong num5 = DataUtility.Pack64(in int4);
										float num6;
										sbyte b;
										TriangleBendingConstraint.InitDihedralAngle(proxyMesh, int4.x, int4.y, int4.z, int4.w, out num6, out b);
										float num7 = math.abs(math.degrees(num6));
										if (num7 < 120f)
										{
											list.Add(num5);
											list2.Add(num6);
											list3.Add(b);
											uint num8 = DataUtility.Pack32(multiDataBuilder.CountValuesForKey(int4.x), multiDataBuilder.CountValuesForKey(int4.y), multiDataBuilder.CountValuesForKey(int4.z), multiDataBuilder.CountValuesForKey(int4.w));
											list4.Add(num8);
											multiDataBuilder.Add(int4.x, 0);
											multiDataBuilder.Add(int4.y, 0);
											multiDataBuilder.Add(int4.z, 0);
											multiDataBuilder.Add(int4.w, 0);
											num++;
										}
										if (num7 >= 90f && num7 <= 179f)
										{
											int4 int5 = DataUtility.PackInt4(int4);
											if (!hashSet.Contains(int5))
											{
												TriangleBendingConstraint.InitVolume(proxyMesh, int4.x, int4.y, int4.z, int4.w, out num6, out b);
												list.Add(num5);
												list2.Add(num6);
												list3.Add(b);
												hashSet.Add(int5);
												uint num9 = DataUtility.Pack32(multiDataBuilder.CountValuesForKey(int4.x), multiDataBuilder.CountValuesForKey(int4.y), multiDataBuilder.CountValuesForKey(int4.z), multiDataBuilder.CountValuesForKey(int4.w));
												list4.Add(num9);
												multiDataBuilder.Add(int4.x, 0);
												multiDataBuilder.Add(int4.y, 0);
												multiDataBuilder.Add(int4.z, 0);
												multiDataBuilder.Add(int4.w, 0);
												num2++;
											}
										}
									}
								}
							}
						}
					}
					constraintData.trianglePairArray = ((list.Count > 0) ? list.ToArray() : null);
					constraintData.restAngleOrVolumeArray = ((list2.Count > 0) ? list2.ToArray() : null);
					constraintData.signOrVolumeArray = ((list3.Count > 0) ? list3.ToArray() : null);
					constraintData.writeDataArray = ((list4.Count > 0) ? list4.ToArray() : null);
					constraintData.writeBufferCount = multiDataBuilder.Count();
					constraintData.writeIndexArray = multiDataBuilder.ToIndexArray();
					constraintData.result.SetSuccess();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				constraintData.result.SetError(Define.Result.Constraint_CreateTriangleBendingException);
				throw;
			}
			return constraintData;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000E178 File Offset: 0x0000C378
		private static void InitVolume(VirtualMesh proxyMesh, int v0, int v1, int v2, int v3, out float volumeRest, out sbyte signFlag)
		{
			float3 @float = proxyMesh.localPositions[v0];
			float3 float2 = MathUtility.TransformPoint(in @float, in proxyMesh.initLocalToWorld);
			@float = proxyMesh.localPositions[v1];
			float3 float3 = MathUtility.TransformPoint(in @float, in proxyMesh.initLocalToWorld);
			@float = proxyMesh.localPositions[v2];
			float3 float4 = MathUtility.TransformPoint(in @float, in proxyMesh.initLocalToWorld);
			@float = proxyMesh.localPositions[v3];
			float3 float5 = MathUtility.TransformPoint(in @float, in proxyMesh.initLocalToWorld);
			volumeRest = 0.16666667f * math.dot(math.cross(float3 - float2, float4 - float2), float5 - float2);
			volumeRest *= 1000f;
			signFlag = 100;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000E230 File Offset: 0x0000C430
		private static void InitDihedralAngle(VirtualMesh proxyMesh, int v0, int v1, int v2, int v3, out float restAngle, out sbyte signFlag)
		{
			float3 @float = proxyMesh.localPositions[v0];
			float3 float2 = proxyMesh.localPositions[v1];
			float3 float3 = proxyMesh.localPositions[v2];
			float3 float4 = proxyMesh.localPositions[v3];
			float3 float5 = math.cross(float3 - @float, float4 - @float);
			float3 float6 = math.cross(float4 - float2, float3 - float2);
			float3 float7 = math.normalize(float5);
			float6 = math.normalize(float6);
			float num = math.dot(float7, float6);
			num = MathUtility.Clamp1(num);
			restAngle = math.acos(num);
			float3 float8 = float4 - float3;
			float num2 = math.sign(math.dot(math.cross(float7, float6), float8));
			signFlag = ((num2 < 0f) ? -1 : 1);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000E2F4 File Offset: 0x0000C4F4
		internal void Register(ClothProcess cprocess)
		{
			bool? flag;
			if (cprocess == null)
			{
				flag = null;
			}
			else
			{
				TriangleBendingConstraint.ConstraintData bendingConstraintData = cprocess.bendingConstraintData;
				flag = ((bendingConstraintData != null) ? new bool?(bendingConstraintData.IsValid()) : null);
			}
			bool? flag2 = flag;
			if (flag2.GetValueOrDefault())
			{
				ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(cprocess.TeamId);
				TriangleBendingConstraint.ConstraintData bendingConstraintData2 = cprocess.bendingConstraintData;
				teamDataRef.bendingPairChunk = this.trianglePairArray.AddRange(bendingConstraintData2.trianglePairArray);
				this.restAngleOrVolumeArray.AddRange(bendingConstraintData2.restAngleOrVolumeArray);
				this.signOrVolumeArray.AddRange(bendingConstraintData2.signOrVolumeArray);
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000E38C File Offset: 0x0000C58C
		internal void Exit(ClothProcess cprocess)
		{
			if (cprocess != null && cprocess.TeamId > 0)
			{
				ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(cprocess.TeamId);
				this.trianglePairArray.Remove(teamDataRef.bendingPairChunk);
				this.restAngleOrVolumeArray.Remove(teamDataRef.bendingPairChunk);
				this.signOrVolumeArray.Remove(teamDataRef.bendingPairChunk);
				teamDataRef.bendingPairChunk.Clear();
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000E3F4 File Offset: 0x0000C5F4
		internal unsafe static void SolverConstraint(DataChunk chunk, in float4 simulationPower, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float> depthArray, ref NativeArray<float3> nextPosArray, ref NativeArray<float> frictionArray, ref NativeArray<ulong> trianglePairArray, ref NativeArray<float> restAngleOrVolumeArray, ref NativeArray<sbyte> signOrVolumeArray, ref NativeArray<float3> tempVectorBufferA, ref NativeArray<int> tempCountBuffer)
		{
			if (param.triangleBendingConstraint.method == TriangleBendingConstraint.Method.None)
			{
				return;
			}
			if (!tdata.bendingPairChunk.IsValid)
			{
				return;
			}
			float num = param.triangleBendingConstraint.stiffness;
			if (num < 1E-06f)
			{
				return;
			}
			num = math.saturate(num * simulationPower.y);
			int startIndex = tdata.particleChunk.startIndex;
			int startIndex2 = tdata.proxyCommonChunk.startIndex;
			int* unsafePtr = (int*)tempVectorBufferA.GetUnsafePtr<float3>();
			int* unsafePtr2 = (int*)tempCountBuffer.GetUnsafePtr<int>();
			int num2 = tdata.bendingPairChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				ulong num3 = trianglePairArray[num2];
				int4 @int = DataUtility.Unpack64(in num3);
				int4 int2 = @int + startIndex;
				int4 int3 = @int + startIndex2;
				float3x4 float3x = 0;
				float3x4 float3x2 = 0;
				float4 @float = 1;
				for (int j = 0; j < 4; j++)
				{
					int num4 = int2[j];
					int num5 = int3[j];
					*float3x[j] = nextPosArray[num4];
					float num6 = frictionArray[num4];
					float num7 = depthArray[num5];
					bool flag = attributes[num5].IsDontMove();
					@float[j] = (flag ? 0.01f : MathUtility.CalcInverseMass(num6, num7));
				}
				int num8 = num2 - tdata.bendingPairChunk.startIndex;
				int num9 = tdata.bendingPairChunk.startIndex + num8;
				float num10 = restAngleOrVolumeArray[num9];
				sbyte b = signOrVolumeArray[num9];
				bool flag2 = false;
				if (b == 100)
				{
					float num11 = num10 * tdata.scaleRatio;
					num11 *= tdata.negativeScaleSign;
					flag2 = TriangleBendingConstraint.CalcVolume(in float3x, in @float, num11, num, ref float3x2);
				}
				else if (param.triangleBendingConstraint.method == TriangleBendingConstraint.Method.DihedralAngle)
				{
					flag2 = TriangleBendingConstraint.CalcDihedralAngle(0f, in float3x, in @float, num10, num, ref float3x2);
				}
				else if (param.triangleBendingConstraint.method == TriangleBendingConstraint.Method.DirectionDihedralAngle)
				{
					float num12 = (float)((b < 0) ? (-1) : 1);
					num10 *= num12;
					num10 *= tdata.negativeScaleSign;
					flag2 = TriangleBendingConstraint.CalcDihedralAngle(num12, in float3x, in @float, num10, num, ref float3x2);
				}
				if (flag2)
				{
					for (int k = 0; k < 4; k++)
					{
						int num4 = int2[k];
						InterlockUtility.AddFloat3(num4, *float3x2[k], unsafePtr2, unsafePtr);
					}
				}
				i++;
				num2++;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000E668 File Offset: 0x0000C868
		internal unsafe static void SumConstraint(DataChunk chunk, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> tempVectorBufferA, ref NativeArray<int> tempCountBuffer)
		{
			if (param.triangleBendingConstraint.method == TriangleBendingConstraint.Method.None)
			{
				return;
			}
			if (!tdata.bendingPairChunk.IsValid)
			{
				return;
			}
			if (param.triangleBendingConstraint.stiffness < 1E-06f)
			{
				return;
			}
			int startIndex = tdata.particleChunk.startIndex;
			int startIndex2 = tdata.proxyCommonChunk.startIndex;
			int* unsafePtr = (int*)tempVectorBufferA.GetUnsafePtr<float3>();
			int* unsafePtr2 = (int*)tempCountBuffer.GetUnsafePtr<int>();
			int num = startIndex + chunk.startIndex;
			int num2 = startIndex2 + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				if (!attributes[num2].IsDontMove())
				{
					int num3 = unsafePtr2[num];
					if (num3 > 0)
					{
						int num4 = num * 3;
						float3 @float = new float3((float)unsafePtr[num4], (float)unsafePtr[num4 + 1], (float)unsafePtr[num4 + 2]);
						@float /= (float)num3;
						@float *= 1E-06f;
						nextPosArray[num] += @float;
					}
				}
				tempCountBuffer[num] = 0;
				tempVectorBufferA[num] = 0;
				i++;
				num++;
				num2++;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
		private unsafe static bool CalcVolume(in float3x4 nextPosBuffer, in float4 invMassBuffer, float volumeRest, float stiffness, ref float3x4 addPosBuffer)
		{
			float3x4 float3x = nextPosBuffer;
			float3 @float = *float3x[0];
			float3x = nextPosBuffer;
			float3 float2 = *float3x[1];
			float3x = nextPosBuffer;
			float3 float3 = *float3x[2];
			float3x = nextPosBuffer;
			float3 float4 = *float3x[3];
			float4 float5 = invMassBuffer;
			float num = float5[0];
			float5 = invMassBuffer;
			float num2 = float5[1];
			float5 = invMassBuffer;
			float num3 = float5[2];
			float5 = invMassBuffer;
			float num4 = float5[3];
			float num5 = 0.16666667f * math.dot(math.cross(float2 - @float, float3 - @float), float4 - @float);
			num5 *= 1000f;
			float3 float6 = math.cross(float2 - float3, float4 - float3);
			float3 float7 = math.cross(float3 - @float, float4 - @float);
			float3 float8 = math.cross(@float - float2, float4 - float2);
			float3 float9 = math.cross(float2 - @float, float3 - @float);
			float num6 = num * math.lengthsq(float6) + num2 * math.lengthsq(float7) + num3 * math.lengthsq(float8) + num4 * math.lengthsq(float9);
			num6 *= 1000f;
			if (math.abs(num6) < 1E-06f)
			{
				return false;
			}
			num6 = stiffness * (volumeRest - num5) / num6;
			*addPosBuffer[0] = num6 * num * float6;
			*addPosBuffer[1] = num6 * num2 * float7;
			*addPosBuffer[2] = num6 * num3 * float8;
			*addPosBuffer[3] = num6 * num4 * float9;
			return true;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000E98C File Offset: 0x0000CB8C
		private unsafe static bool CalcDihedralAngle(float sign, in float3x4 nextPosBuffer, in float4 invMassBuffer, float restAngle, float stiffness, ref float3x4 addPosBuffer)
		{
			float3x4 float3x = nextPosBuffer;
			float3 @float = *float3x[0];
			float3x = nextPosBuffer;
			float3 float2 = *float3x[1];
			float3x = nextPosBuffer;
			float3 float3 = *float3x[2];
			float3x = nextPosBuffer;
			float3 float4 = *float3x[3];
			float4 float5 = invMassBuffer;
			float num = float5[0];
			float5 = invMassBuffer;
			float num2 = float5[1];
			float5 = invMassBuffer;
			float num3 = float5[2];
			float5 = invMassBuffer;
			float num4 = float5[3];
			float3 float6 = float4 - float3;
			float num5 = math.length(float6);
			if (num5 < 1E-08f)
			{
				return false;
			}
			float num6 = 1f / num5;
			float3 float7 = math.cross(float3 - @float, float4 - @float);
			float3 float8 = math.cross(float4 - float2, float3 - float2);
			float num7 = math.lengthsq(float7);
			float num8 = math.lengthsq(float8);
			if (num7 == 0f || num8 == 0f)
			{
				return false;
			}
			float7 /= num7;
			float8 /= num8;
			float3 float9 = num5 * float7;
			float3 float10 = num5 * float8;
			float3 float11 = math.dot(@float - float4, float6) * num6 * float7 + math.dot(float2 - float4, float6) * num6 * float8;
			float3 float12 = math.dot(float3 - @float, float6) * num6 * float7 + math.dot(float3 - float2, float6) * num6 * float8;
			float7 = math.normalize(float7);
			float8 = math.normalize(float8);
			float num9 = math.acos(MathUtility.Clamp1(math.dot(float7, float8)));
			float num10 = num * math.lengthsq(float9) + num2 * math.lengthsq(float10) + num3 * math.lengthsq(float11) + num4 * math.lengthsq(float12);
			if (num10 == 0f)
			{
				return false;
			}
			float num11 = math.sign(math.dot(math.cross(float7, float8), float6));
			if (sign != 0f)
			{
				num9 *= num11;
			}
			else
			{
				num10 *= num11;
			}
			num10 = (restAngle - num9) / num10 * stiffness;
			float3 float13 = -num * num10 * float9;
			float3 float14 = -num2 * num10 * float10;
			float3 float15 = -num3 * num10 * float11;
			float3 float16 = -num4 * num10 * float12;
			*addPosBuffer[0] = float13;
			*addPosBuffer[1] = float14;
			*addPosBuffer[2] = float15;
			*addPosBuffer[3] = float16;
			return true;
		}

		// Token: 0x04000218 RID: 536
		private const sbyte VOLUME_SIGN = 100;

		// Token: 0x04000219 RID: 537
		public ExNativeArray<ulong> trianglePairArray;

		// Token: 0x0400021A RID: 538
		public ExNativeArray<float> restAngleOrVolumeArray;

		// Token: 0x0400021B RID: 539
		public ExNativeArray<sbyte> signOrVolumeArray;

		// Token: 0x0400021C RID: 540
		private const float VolumeScale = 1000f;

		// Token: 0x0200005A RID: 90
		public enum Method
		{
			// Token: 0x0400021E RID: 542
			None,
			// Token: 0x0400021F RID: 543
			DihedralAngle,
			// Token: 0x04000220 RID: 544
			DirectionDihedralAngle
		}

		// Token: 0x0200005B RID: 91
		[Serializable]
		public class SerializeData : IDataValidate
		{
			// Token: 0x06000149 RID: 329 RVA: 0x0000EC5D File Offset: 0x0000CE5D
			public SerializeData()
			{
				this.stiffness = 1f;
			}

			// Token: 0x0600014A RID: 330 RVA: 0x0000EC70 File Offset: 0x0000CE70
			public void DataValidate()
			{
				this.stiffness = Mathf.Clamp01(this.stiffness);
			}

			// Token: 0x0600014B RID: 331 RVA: 0x0000EC83 File Offset: 0x0000CE83
			public TriangleBendingConstraint.SerializeData Clone()
			{
				return new TriangleBendingConstraint.SerializeData
				{
					stiffness = this.stiffness
				};
			}

			// Token: 0x04000221 RID: 545
			[Range(0f, 1f)]
			public float stiffness;
		}

		// Token: 0x0200005C RID: 92
		public struct TriangleBendingConstraintParams
		{
			// Token: 0x0600014C RID: 332 RVA: 0x0000EC96 File Offset: 0x0000CE96
			public void Convert(TriangleBendingConstraint.SerializeData sdata)
			{
				this.method = ((sdata.stiffness > 1E-08f) ? TriangleBendingConstraint.Method.DirectionDihedralAngle : TriangleBendingConstraint.Method.None);
				this.stiffness = sdata.stiffness;
			}

			// Token: 0x04000222 RID: 546
			public TriangleBendingConstraint.Method method;

			// Token: 0x04000223 RID: 547
			public float stiffness;
		}

		// Token: 0x0200005D RID: 93
		[Serializable]
		public class ConstraintData : IValid
		{
			// Token: 0x0600014D RID: 333 RVA: 0x0000ECBB File Offset: 0x0000CEBB
			public bool IsValid()
			{
				return this.trianglePairArray != null && this.trianglePairArray.Length != 0;
			}

			// Token: 0x04000224 RID: 548
			public ResultCode result;

			// Token: 0x04000225 RID: 549
			public ulong[] trianglePairArray;

			// Token: 0x04000226 RID: 550
			public float[] restAngleOrVolumeArray;

			// Token: 0x04000227 RID: 551
			public sbyte[] signOrVolumeArray;

			// Token: 0x04000228 RID: 552
			public int writeBufferCount;

			// Token: 0x04000229 RID: 553
			public uint[] writeDataArray;

			// Token: 0x0400022A RID: 554
			public uint[] writeIndexArray;
		}
	}
}
