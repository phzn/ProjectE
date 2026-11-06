using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000056 RID: 86
	public class TetherConstraint : IDisposable
	{
		// Token: 0x06000136 RID: 310 RVA: 0x0000706D File Offset: 0x0000526D
		public void Dispose()
		{
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000D958 File Offset: 0x0000BB58
		internal static void SolverConstraint(DataChunk chunk, ref TeamManager.TeamData tdata, ref ClothParameters param, ref InertiaConstraint.CenterData cdata, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float> vertexDepths, ref NativeArray<int> vertexRootIndices, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> velocityPosArray, ref NativeArray<float> frictionArray, ref NativeArray<float3> stepBasicPositionBuffer)
		{
			int startIndex = tdata.particleChunk.startIndex;
			int num = startIndex + chunk.startIndex;
			int num2 = tdata.proxyCommonChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				if (attributes[num2].IsMove())
				{
					int num3 = vertexRootIndices[num2];
					if (num3 >= 0)
					{
						float3 @float = nextPosArray[num];
						float3 float2 = nextPosArray[num3 + startIndex];
						float num4 = vertexDepths[num2];
						float num5 = frictionArray[num];
						float3 float3 = float2 - @float;
						float num6 = math.length(float3);
						if (num6 >= 1E-08f)
						{
							float3 float4 = stepBasicPositionBuffer[num];
							float3 float5 = stepBasicPositionBuffer[num3 + startIndex];
							float num7 = math.distance(float4, float5);
							if (num7 != 0f)
							{
								float num8 = num6 / num7;
								float num9 = 1f - param.tetherConstraint.compressionLimit;
								float num10 = 1f + param.tetherConstraint.stretchLimit;
								float num11;
								float num13;
								float num14;
								if (num8 < num9)
								{
									num11 = num6 - num9 * num7;
									float num12 = math.saturate((num9 - num8) / 0.3f);
									num13 = 1f * num12;
									num14 = 0.7f;
								}
								else
								{
									if (num8 <= num10)
									{
										goto IL_01AA;
									}
									num11 = num6 - num10 * num7;
									float num15 = math.saturate((num8 - num10) / 0.3f);
									num13 = 1f * num15;
									num14 = 0.7f;
								}
								float3 float6 = float3 / num6 * (num11 * num13);
								@float += float6;
								nextPosArray[num] = @float;
								velocityPosArray[num] += float6 * num14;
							}
						}
					}
				}
				IL_01AA:
				i++;
				num++;
				num2++;
			}
		}

		// Token: 0x02000057 RID: 87
		[Serializable]
		public class SerializeData : IDataValidate
		{
			// Token: 0x06000139 RID: 313 RVA: 0x0000DB27 File Offset: 0x0000BD27
			public SerializeData()
			{
				this.distanceCompression = 0.4f;
			}

			// Token: 0x0600013A RID: 314 RVA: 0x0000DB3A File Offset: 0x0000BD3A
			public void DataValidate()
			{
				this.distanceCompression = Mathf.Clamp(this.distanceCompression, 0f, 1f);
			}

			// Token: 0x0600013B RID: 315 RVA: 0x0000DB57 File Offset: 0x0000BD57
			public TetherConstraint.SerializeData Clone()
			{
				return new TetherConstraint.SerializeData
				{
					distanceCompression = this.distanceCompression
				};
			}

			// Token: 0x04000215 RID: 533
			[Range(0f, 1f)]
			public float distanceCompression;
		}

		// Token: 0x02000058 RID: 88
		public struct TetherConstraintParams
		{
			// Token: 0x0600013C RID: 316 RVA: 0x0000DB6A File Offset: 0x0000BD6A
			public void Convert(TetherConstraint.SerializeData sdata, ClothProcess.ClothType clothType)
			{
				if (clothType > ClothProcess.ClothType.BoneCloth)
				{
					if (clothType == ClothProcess.ClothType.BoneSpring)
					{
						this.compressionLimit = 0.8f;
					}
				}
				else
				{
					this.compressionLimit = sdata.distanceCompression;
				}
				this.stretchLimit = 0.03f;
			}

			// Token: 0x04000216 RID: 534
			public float compressionLimit;

			// Token: 0x04000217 RID: 535
			public float stretchLimit;
		}
	}
}
