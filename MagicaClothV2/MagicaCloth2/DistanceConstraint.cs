using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000032 RID: 50
	public class DistanceConstraint : IDisposable
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000977A File Offset: 0x0000797A
		public int DataCount
		{
			get
			{
				ExNativeArray<uint> exNativeArray = this.indexArray;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000978D File Offset: 0x0000798D
		public DistanceConstraint()
		{
			this.indexArray = new ExNativeArray<uint>(0, true);
			this.dataArray = new ExNativeArray<ushort>(0, true);
			this.distanceArray = new ExNativeArray<float>(0, true);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000097BC File Offset: 0x000079BC
		public void Dispose()
		{
			ExNativeArray<uint> exNativeArray = this.indexArray;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			ExNativeArray<ushort> exNativeArray2 = this.dataArray;
			if (exNativeArray2 != null)
			{
				exNativeArray2.Dispose();
			}
			ExNativeArray<float> exNativeArray3 = this.distanceArray;
			if (exNativeArray3 == null)
			{
				return;
			}
			exNativeArray3.Dispose();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000097F0 File Offset: 0x000079F0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[DistanceConstraint]");
			stringBuilder.AppendLine("  -indexArray:" + this.indexArray.ToSummary());
			stringBuilder.AppendLine("  -dataArray:" + this.dataArray.ToSummary());
			stringBuilder.AppendLine("  -distanceArray:" + this.distanceArray.ToSummary());
			return stringBuilder.ToString();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00009868 File Offset: 0x00007A68
		public static DistanceConstraint.ConstraintData CreateData(VirtualMesh proxyMesh, in ClothParameters parameters)
		{
			DistanceConstraint.ConstraintData constraintData = new DistanceConstraint.ConstraintData();
			NativeParallelMultiHashMap<int, ushort> nativeParallelMultiHashMap = default(NativeParallelMultiHashMap<int, ushort>);
			try
			{
				int vertexCount = proxyMesh.VertexCount;
				nativeParallelMultiHashMap = JobUtility.ToNativeMultiHashMap(in proxyMesh.vertexToVertexIndexArray, in proxyMesh.vertexToVertexDataArray);
				HashSet<uint> hashSet = new HashSet<uint>();
				using (MultiDataBuilder<ushort> multiDataBuilder = new MultiDataBuilder<ushort>(vertexCount, vertexCount * 2))
				{
					using (MultiDataBuilder<ushort> multiDataBuilder2 = new MultiDataBuilder<ushort>(vertexCount, vertexCount * 2))
					{
						for (int i = 0; i < vertexCount; i++)
						{
							if (nativeParallelMultiHashMap.ContainsKey(i))
							{
								VertexAttribute vertexAttribute = proxyMesh.attributes[i];
								int num = proxyMesh.vertexParentIndices[i];
								foreach (ushort num2 in nativeParallelMultiHashMap.GetValuesForKey(i))
								{
									int num3 = (int)num2;
									VertexAttribute vertexAttribute2 = proxyMesh.attributes[num3];
									int num4 = proxyMesh.vertexParentIndices[num3];
									if ((vertexAttribute.IsMove() || vertexAttribute2.IsMove()) && !vertexAttribute.IsInvalid() && !vertexAttribute2.IsInvalid())
									{
										if (num3 == num || i == num4)
										{
											multiDataBuilder.Add(i, num2);
										}
										else
										{
											multiDataBuilder2.Add(i, num2);
										}
										uint num5 = DataUtility.Pack32Sort(i, num3);
										hashSet.Add(num5);
									}
								}
							}
						}
						if (proxyMesh.edgeToTriangles.IsCreated)
						{
							int edgeCount = proxyMesh.EdgeCount;
							for (int j = 0; j < edgeCount; j++)
							{
								int2 @int = proxyMesh.edges[j];
								FixedList128Bytes<ushort> fixedList128Bytes = (ref proxyMesh.edgeToTriangles).MC2ToFixedList128Bytes(@int);
								int length = fixedList128Bytes.Length;
								if (length >= 2)
								{
									float3 @float = proxyMesh.localPositions[@int.x];
									float3 float2 = proxyMesh.localPositions[@int.y];
									float num6 = math.length(@float - float2);
									if (num6 >= 1E-08f)
									{
										math.normalize(@float - float2);
										(@float + float2) * 0.5f;
										for (int k = 0; k < length - 1; k++)
										{
											int unuseTriangleIndex = MathUtility.GetUnuseTriangleIndex(proxyMesh.triangles[(int)fixedList128Bytes[k]], @int);
											float3 float3 = proxyMesh.localPositions[unuseTriangleIndex];
											VertexAttribute vertexAttribute3 = proxyMesh.attributes[unuseTriangleIndex];
											float3 float4 = MathUtility.TriangleNormal(in @float, in float2, in float3);
											for (int l = k + 1; l < length; l++)
											{
												int unuseTriangleIndex2 = MathUtility.GetUnuseTriangleIndex(proxyMesh.triangles[(int)fixedList128Bytes[l]], @int);
												float3 float5 = proxyMesh.localPositions[unuseTriangleIndex2];
												VertexAttribute vertexAttribute4 = proxyMesh.attributes[unuseTriangleIndex2];
												float3 float6 = MathUtility.TriangleNormal(in @float, in float2, in float5);
												if ((vertexAttribute3.IsMove() || vertexAttribute4.IsMove()) && math.abs(math.dot(float4, float6)) >= 0.9396926f && math.abs(math.length(float3 - float5) / num6 - 1f) <= 0.3f)
												{
													uint num7 = DataUtility.Pack32Sort(unuseTriangleIndex, unuseTriangleIndex2);
													if (!hashSet.Contains(num7))
													{
														hashSet.Add(num7);
														multiDataBuilder2.Add(unuseTriangleIndex, (ushort)unuseTriangleIndex2);
														multiDataBuilder2.Add(unuseTriangleIndex2, (ushort)unuseTriangleIndex);
													}
												}
											}
										}
									}
								}
							}
						}
						ValueTuple<ushort[], uint[]> valueTuple = multiDataBuilder.ToArray();
						ushort[] item = valueTuple.Item1;
						uint[] item2 = valueTuple.Item2;
						ValueTuple<ushort[], uint[]> valueTuple2 = multiDataBuilder2.ToArray();
						ushort[] item3 = valueTuple2.Item1;
						uint[] item4 = valueTuple2.Item2;
						int num8 = ((item != null) ? item.Length : 0) + ((item3 != null) ? item3.Length : 0);
						if (num8 > 0)
						{
							List<uint> list = new List<uint>(vertexCount);
							List<ushort> list2 = new List<ushort>(num8);
							List<float> list3 = new List<float>(num8);
							for (int m = 0; m < vertexCount; m++)
							{
								int count = list2.Count;
								int num9 = 0;
								float3 float7 = proxyMesh.localPositions[m];
								for (int n = 0; n < 2; n++)
								{
									int num10;
									int num11;
									DataUtility.Unpack12_20((n == 0) ? item2[m] : item4[m], out num10, out num11);
									for (int num12 = 0; num12 < num10; num12++)
									{
										ushort num13 = ((n == 0) ? item[num11 + num12] : item3[num11 + num12]);
										float3 float8 = proxyMesh.localPositions[(int)num13];
										float num14 = math.distance(float7, float8);
										if (num14 >= 1E-06f)
										{
											list2.Add(num13);
											list3.Add((n == 0) ? num14 : (-num14));
											num9++;
										}
									}
								}
								uint num15 = DataUtility.Pack12_20(num9, count);
								list.Add(num15);
							}
							constraintData.indexArray = list.ToArray();
							constraintData.dataArray = list2.ToArray();
							constraintData.distanceArray = list3.ToArray();
						}
						constraintData.result.SetSuccess();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				constraintData.result.SetError(Define.Result.Constraint_CreateDistanceException);
				throw;
			}
			finally
			{
				if (nativeParallelMultiHashMap.IsCreated)
				{
					nativeParallelMultiHashMap.Dispose();
				}
			}
			return constraintData;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00009DFC File Offset: 0x00007FFC
		internal void Register(ClothProcess cprocess)
		{
			bool? flag;
			if (cprocess == null)
			{
				flag = null;
			}
			else
			{
				DistanceConstraint.ConstraintData distanceConstraintData = cprocess.distanceConstraintData;
				flag = ((distanceConstraintData != null) ? new bool?(distanceConstraintData.IsValid()) : null);
			}
			bool? flag2 = flag;
			if (flag2.GetValueOrDefault())
			{
				ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(cprocess.TeamId);
				teamDataRef.distanceStartChunk = this.indexArray.AddRange(cprocess.distanceConstraintData.indexArray);
				teamDataRef.distanceDataChunk = this.dataArray.AddRange(cprocess.distanceConstraintData.dataArray);
				this.distanceArray.AddRange(cprocess.distanceConstraintData.distanceArray);
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00009EA0 File Offset: 0x000080A0
		internal void Exit(ClothProcess cprocess)
		{
			if (cprocess != null && cprocess.TeamId > 0)
			{
				ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(cprocess.TeamId);
				this.indexArray.Remove(teamDataRef.distanceStartChunk);
				this.dataArray.Remove(teamDataRef.distanceDataChunk);
				this.distanceArray.Remove(teamDataRef.distanceDataChunk);
				teamDataRef.distanceStartChunk.Clear();
				teamDataRef.distanceDataChunk.Clear();
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00009F14 File Offset: 0x00008114
		internal static void SolverConstraint(DataChunk chunk, float4 simulationPower, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float> depthArray, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> basePosArray, ref NativeArray<float3> velocityPosArray, ref NativeArray<float> frictionArray, ref NativeArray<uint> indexArray, ref NativeArray<ushort> dataArray, ref NativeArray<float> distanceArray)
		{
			DataChunk distanceStartChunk = tdata.distanceStartChunk;
			DataChunk distanceDataChunk = tdata.distanceDataChunk;
			if (distanceStartChunk.dataLength == 0)
			{
				return;
			}
			int startIndex = distanceStartChunk.startIndex;
			int startIndex2 = distanceDataChunk.startIndex;
			float animationPoseRatio = tdata.animationPoseRatio;
			float num = tdata.InitScale * tdata.scaleRatio;
			bool isSpring = tdata.IsSpring;
			int startIndex3 = tdata.particleChunk.startIndex;
			int num2 = startIndex3 + chunk.startIndex;
			int startIndex4 = tdata.proxyCommonChunk.startIndex;
			int num3 = startIndex4 + chunk.startIndex;
			int num4 = chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				float3 @float = nextPosArray[num2];
				VertexAttribute vertexAttribute = attributes[num3];
				float num5 = depthArray[num3];
				float num6 = frictionArray[num2];
				if (!vertexAttribute.IsInvalid() && (!vertexAttribute.IsDontMove() || isSpring))
				{
					float num7 = (isSpring ? 10f : 50f);
					float num8 = MathUtility.CalcInverseMass(num6, num5, vertexAttribute.IsDontMove(), num7);
					float num9 = (in param.distanceConstraint.restorationStiffness).MC2EvaluateCurveClamp01(num5);
					num9 *= simulationPower.y;
					int num10;
					int num11;
					DataUtility.Unpack12_20(indexArray[startIndex + num4], out num10, out num11);
					if (num10 > 0)
					{
						float3 float2 = basePosArray[num2];
						float3 float3 = 0;
						int num12 = 0;
						int num13 = startIndex2 + num11;
						for (int j = 0; j < num10; j++)
						{
							int num14 = (int)dataArray[num13 + j];
							float num15 = distanceArray[num13 + j];
							float num16 = math.saturate((num15 >= 0f) ? num9 : (num9 * 0.5f));
							int num17 = startIndex3 + num14;
							int num18 = startIndex4 + num14;
							float3 float4 = nextPosArray[num17];
							float3 float5 = basePosArray[num17];
							float num19 = depthArray[num18];
							float num20 = MathUtility.CalcInverseMass(frictionArray[num17], num19, attributes[num18].IsDontMove(), num7);
							float num21 = math.lerp(math.abs(num15) * num, math.distance(float2, float5), animationPoseRatio);
							float3 float6 = float4 - @float;
							float num22 = math.length(float6);
							if (num22 >= 1E-08f)
							{
								float3 float7 = math.normalize(float6);
								float3 float8 = (num22 - num21) * num16 * float7 / (num8 + num20);
								float3 float9 = num8 * float8;
								float3 += float9;
								num12++;
							}
						}
						if (num12 > 0)
						{
							float3 /= (float)num12;
							@float += float3;
							nextPosArray[num2] = @float;
							float velocityAttenuation = param.distanceConstraint.velocityAttenuation;
							velocityPosArray[num2] += float3 * velocityAttenuation;
						}
					}
				}
				i++;
				num2++;
				num3++;
				num4++;
			}
		}

		// Token: 0x04000122 RID: 290
		public const int TypeCount = 2;

		// Token: 0x04000123 RID: 291
		public ExNativeArray<uint> indexArray;

		// Token: 0x04000124 RID: 292
		public ExNativeArray<ushort> dataArray;

		// Token: 0x04000125 RID: 293
		public ExNativeArray<float> distanceArray;

		// Token: 0x02000033 RID: 51
		[Serializable]
		public class SerializeData : IDataValidate
		{
			// Token: 0x060000EA RID: 234 RVA: 0x0000A1FC File Offset: 0x000083FC
			public SerializeData()
			{
				this.stiffness = new CurveSerializeData(1f, 1f, 0.5f, false);
			}

			// Token: 0x060000EB RID: 235 RVA: 0x0000A21F File Offset: 0x0000841F
			public void DataValidate()
			{
				this.stiffness.DataValidate(0f, 1f);
			}

			// Token: 0x060000EC RID: 236 RVA: 0x0000A236 File Offset: 0x00008436
			public DistanceConstraint.SerializeData Clone()
			{
				return new DistanceConstraint.SerializeData
				{
					stiffness = this.stiffness.Clone()
				};
			}

			// Token: 0x04000126 RID: 294
			public CurveSerializeData stiffness;
		}

		// Token: 0x02000034 RID: 52
		public struct DistanceConstraintParams
		{
			// Token: 0x060000ED RID: 237 RVA: 0x0000A24E File Offset: 0x0000844E
			public void Convert(DistanceConstraint.SerializeData sdata, ClothProcess.ClothType clothType)
			{
				if (clothType > ClothProcess.ClothType.BoneCloth)
				{
					if (clothType == ClothProcess.ClothType.BoneSpring)
					{
						this.restorationStiffness = 0.5f;
					}
				}
				else
				{
					this.restorationStiffness = sdata.stiffness.ConvertFloatArray();
				}
				this.velocityAttenuation = 0.3f;
			}

			// Token: 0x04000127 RID: 295
			public float4x4 restorationStiffness;

			// Token: 0x04000128 RID: 296
			public float velocityAttenuation;
		}

		// Token: 0x02000035 RID: 53
		[Serializable]
		public class ConstraintData : IValid
		{
			// Token: 0x060000EE RID: 238 RVA: 0x0000A289 File Offset: 0x00008489
			public bool IsValid()
			{
				return this.indexArray != null && this.indexArray.Length != 0;
			}

			// Token: 0x04000129 RID: 297
			public ResultCode result;

			// Token: 0x0400012A RID: 298
			public uint[] indexArray;

			// Token: 0x0400012B RID: 299
			public ushort[] dataArray;

			// Token: 0x0400012C RID: 300
			public float[] distanceArray;
		}
	}
}
