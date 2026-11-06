using System;
using System.Text;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000028 RID: 40
	public class AngleConstraint : IDisposable
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x0000706D File Offset: 0x0000526D
		public void Dispose()
		{
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00007A2D File Offset: 0x00005C2D
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[AngleConstraint]");
			return stringBuilder.ToString();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00007A48 File Offset: 0x00005C48
		internal static void SolverConstraint(DataChunk chunk, in float4 simulationPower, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float> vertexDepths, ref NativeArray<int> vertexParentIndices, ref NativeArray<ushort> baseLineStartDataIndices, ref NativeArray<ushort> baseLineDataCounts, ref NativeArray<ushort> baseLineData, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> velocityPosArray, ref NativeArray<float> frictionArray, ref NativeArray<float3> stepBasicPositionBuffer, ref NativeArray<quaternion> stepBasicRotationBuffer, ref NativeArray<float> lengthBufferArray, ref NativeArray<float3> localPosBufferArray, ref NativeArray<quaternion> localRotBufferArray, ref NativeArray<quaternion> rotationBufferArray, ref NativeArray<float3> restorationVectorBufferArray)
		{
			AngleConstraint.AngleConstraintParams angleConstraint = param.angleConstraint;
			if (!angleConstraint.useAngleLimit && !angleConstraint.useAngleRestoration)
			{
				return;
			}
			int startIndex = tdata.baseLineDataChunk.startIndex;
			int startIndex2 = tdata.particleChunk.startIndex;
			int startIndex3 = tdata.proxyCommonChunk.startIndex;
			bool useAngleLimit = angleConstraint.useAngleLimit;
			bool useAngleRestoration = angleConstraint.useAngleRestoration;
			float limitstiffness = angleConstraint.limitstiffness;
			float restorationVelocityAttenuation = angleConstraint.restorationVelocityAttenuation;
			float num = math.lerp(1f - angleConstraint.restorationGravityFalloff, 1f, tdata.gravityDot);
			int num2 = tdata.baseLineChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				int num3 = (int)baseLineStartDataIndices[num2];
				int num4 = (int)baseLineDataCounts[num2];
				int num5 = num3 + startIndex;
				int j = 0;
				while (j < num4)
				{
					int num6 = (int)baseLineData[num5];
					int num7 = startIndex2 + num6;
					int num8 = startIndex3 + num6;
					float3 @float = nextPosArray[num7];
					float3 float2 = stepBasicPositionBuffer[num7];
					quaternion quaternion = stepBasicRotationBuffer[num7];
					rotationBufferArray[num7] = quaternion;
					if (j > 0)
					{
						int num9 = vertexParentIndices[num8] + startIndex2;
						float3 float3 = nextPosArray[num9];
						float3 float4 = stepBasicPositionBuffer[num9];
						quaternion quaternion2 = stepBasicRotationBuffer[num9];
						if (useAngleLimit)
						{
							float num10 = math.distance(@float, float3);
							float3 float5 = float2 - float4;
							float num11 = math.length(float5);
							if (num10 < 1E-08f || num11 < 1E-08f)
							{
								lengthBufferArray[num7] = 0f;
								localPosBufferArray[num7] = 0;
								localRotBufferArray[num7] = quaternion.identity;
							}
							else
							{
								float3 float6 = float5 / num11;
								quaternion quaternion3 = math.inverse(quaternion2);
								float3 float7 = math.mul(quaternion3, float6);
								quaternion quaternion4 = math.mul(quaternion3, quaternion);
								lengthBufferArray[num7] = num10;
								localPosBufferArray[num7] = float7;
								localRotBufferArray[num7] = quaternion4;
							}
						}
						if (useAngleRestoration)
						{
							float3 float8 = float2 - float4;
							restorationVectorBufferArray[num7] = float8;
						}
					}
					j++;
					num5++;
				}
				for (int k = 0; k < 3; k++)
				{
					float num12 = (float)k / 2f;
					float num13 = 0.4f;
					float num14 = math.lerp(0.1f, 0.5f, num12);
					num5 = num3 + startIndex;
					int l = 0;
					while (l < num4)
					{
						int num15 = (int)baseLineData[num5];
						int num16 = startIndex2 + num15;
						int num17 = startIndex3 + num15;
						float3 float9 = nextPosArray[num16];
						float num18 = vertexDepths[num17];
						VertexAttribute vertexAttribute = attributes[num17];
						float num19 = MathUtility.CalcInverseMass(frictionArray[num16]);
						if (vertexAttribute.IsMove())
						{
							int num20 = vertexParentIndices[num17] + startIndex2;
							int num21 = vertexParentIndices[num17] + startIndex3;
							float3 float10 = nextPosArray[num20];
							VertexAttribute vertexAttribute2 = attributes[num21];
							float num22 = MathUtility.CalcInverseMass(frictionArray[num20]);
							if (useAngleLimit)
							{
								quaternion quaternion5 = rotationBufferArray[num20];
								float3 float11 = localPosBufferArray[num16];
								quaternion quaternion6 = localRotBufferArray[num16];
								float3 float12 = float9 - float10;
								float num23 = math.length(float12);
								if (num23 >= 1E-08f)
								{
									float3 float13 = math.mul(quaternion5, float11);
									float num24 = math.length(float13);
									if (num24 < 1E-08f)
									{
										float3 float14 = float10 - float9;
										nextPosArray[num16] = float10;
										velocityPosArray[num16] += float14;
										rotationBufferArray[num16] = math.mul(quaternion5, quaternion6);
									}
									else
									{
										float12 /= num23;
										float13 /= num24;
										float num25 = lengthBufferArray[num16];
										num23 = math.lerp(num23, num25, 0.5f);
										if (num25 >= 1E-08f && num23 >= 1E-08f)
										{
											float12 *= num23;
											float num26 = math.radians((in angleConstraint.limitCurveData).MC2EvaluateCurve(num18));
											float num27 = MathUtility.Angle(in float12, in float13);
											float3 float15 = float12;
											if (num27 > num26)
											{
												float num28 = math.lerp(num27, num26, limitstiffness);
												MathUtility.ClampAngle(in float12, in float13, num28, out float15);
											}
											float3 float16 = float10 + float12 * num13;
											float3 float17 = float16 - float15 * num13;
											float3 float18 = float16 + float15 * (1f - num13);
											float3 float19 = float17 - float10;
											float3 float20 = float18 - float9;
											float20 *= num19;
											float19 *= num22;
											if (vertexAttribute.IsMove())
											{
												float9 += float20;
												nextPosArray[num16] = float9;
												velocityPosArray[num16] += float20 * 0.9f;
											}
											if (vertexAttribute2.IsMove())
											{
												float10 += float19;
												nextPosArray[num20] = float10;
												velocityPosArray[num20] += float19 * 0.9f;
											}
											float12 = float9 - float10;
											num23 = math.length(float12);
											if (num23 >= 1E-08f)
											{
												float12 /= num23;
												quaternion quaternion7 = math.mul(quaternion5, quaternion6);
												quaternion7 = math.mul(MathUtility.FromToRotationWithoutNormalize(in float13, in float12, 1f), quaternion7);
												rotationBufferArray[num16] = quaternion7;
											}
										}
									}
								}
							}
							if (useAngleRestoration)
							{
								float3 float21 = restorationVectorBufferArray[num16];
								float num29 = math.length(float21);
								if (num29 < 1E-08f)
								{
									float3 float22 = float10 - float9;
									nextPosArray[num16] = float10;
									velocityPosArray[num16] += float22;
								}
								else
								{
									float3 float23 = float9 - float10;
									float num30 = math.length(float23);
									if (num30 >= 1E-08f)
									{
										float num31 = (in angleConstraint.restorationStiffness).MC2EvaluateCurveClamp01(num18);
										num31 = math.saturate(num31 * simulationPower.w);
										num31 *= num;
										float3 float24 = float23 / num30;
										float3 float25 = float21 / num29;
										float3 float26 = math.mul(MathUtility.FromToRotationWithoutNormalize(in float24, in float25, num31), float23);
										float3 float27 = float10 + float23 * num14;
										float3 float28 = float27 - float26 * num14;
										float3 float29 = float27 + float26 * (1f - num14);
										float3 float30 = float28 - float10;
										float3 float31 = float29 - float9;
										float30 *= num19;
										float31 *= num22;
										if (vertexAttribute.IsMove())
										{
											float9 += float31;
											nextPosArray[num16] = float9;
											velocityPosArray[num16] += float31 * restorationVelocityAttenuation;
										}
										if (vertexAttribute2.IsMove())
										{
											float10 += float30;
											nextPosArray[num20] = float10;
											velocityPosArray[num20] += float30 * restorationVelocityAttenuation;
										}
									}
								}
							}
						}
						l++;
						num5++;
					}
				}
				i++;
				num2++;
			}
			num2 = tdata.baseLineChunk.startIndex + chunk.startIndex;
			int m = 0;
			while (m < chunk.dataLength)
			{
				int num32 = (int)baseLineStartDataIndices[num2];
				int num33 = (int)baseLineDataCounts[num2];
				int num34 = num32 + startIndex;
				int n = 0;
				while (n < num33)
				{
					int num35 = (int)baseLineData[num34];
					int num36 = startIndex2 + num35;
					lengthBufferArray[num36] = 0f;
					localPosBufferArray[num36] = 0;
					restorationVectorBufferArray[num36] = 0;
					n++;
					num34++;
				}
				m++;
				num2++;
			}
		}

		// Token: 0x02000029 RID: 41
		[Serializable]
		public class RestorationSerializeData : IDataValidate
		{
			// Token: 0x060000C3 RID: 195 RVA: 0x0000822D File Offset: 0x0000642D
			public RestorationSerializeData()
			{
				this.useAngleRestoration = true;
				this.stiffness = new CurveSerializeData(0.2f, 1f, 0.2f, true);
				this.velocityAttenuation = 0.8f;
				this.gravityFalloff = 0f;
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x0000826D File Offset: 0x0000646D
			public void DataValidate()
			{
				this.stiffness.DataValidate(0f, 1f);
				this.velocityAttenuation = Mathf.Clamp01(this.velocityAttenuation);
				this.gravityFalloff = Mathf.Clamp01(this.gravityFalloff);
			}

			// Token: 0x060000C5 RID: 197 RVA: 0x000082A6 File Offset: 0x000064A6
			public AngleConstraint.RestorationSerializeData Clone()
			{
				return new AngleConstraint.RestorationSerializeData
				{
					useAngleRestoration = this.useAngleRestoration,
					stiffness = this.stiffness.Clone(),
					velocityAttenuation = this.velocityAttenuation,
					gravityFalloff = this.gravityFalloff
				};
			}

			// Token: 0x04000105 RID: 261
			public bool useAngleRestoration;

			// Token: 0x04000106 RID: 262
			public CurveSerializeData stiffness;

			// Token: 0x04000107 RID: 263
			[Range(0f, 1f)]
			public float velocityAttenuation;

			// Token: 0x04000108 RID: 264
			[Range(0f, 1f)]
			public float gravityFalloff;
		}

		// Token: 0x0200002A RID: 42
		[Serializable]
		public class LimitSerializeData : IDataValidate
		{
			// Token: 0x060000C6 RID: 198 RVA: 0x000082E2 File Offset: 0x000064E2
			public LimitSerializeData()
			{
				this.useAngleLimit = false;
				this.limitAngle = new CurveSerializeData(60f, 0f, 1f, true);
				this.stiffness = 1f;
			}

			// Token: 0x060000C7 RID: 199 RVA: 0x00008317 File Offset: 0x00006517
			public void DataValidate()
			{
				this.limitAngle.DataValidate(0f, 180f);
				this.stiffness = Mathf.Clamp01(this.stiffness);
			}

			// Token: 0x060000C8 RID: 200 RVA: 0x0000833F File Offset: 0x0000653F
			public AngleConstraint.LimitSerializeData Clone()
			{
				return new AngleConstraint.LimitSerializeData
				{
					useAngleLimit = this.useAngleLimit,
					limitAngle = this.limitAngle.Clone(),
					stiffness = this.stiffness
				};
			}

			// Token: 0x04000109 RID: 265
			public bool useAngleLimit;

			// Token: 0x0400010A RID: 266
			public CurveSerializeData limitAngle;

			// Token: 0x0400010B RID: 267
			[Range(0f, 1f)]
			public float stiffness;
		}

		// Token: 0x0200002B RID: 43
		public struct AngleConstraintParams
		{
			// Token: 0x060000C9 RID: 201 RVA: 0x00008370 File Offset: 0x00006570
			public void Convert(AngleConstraint.RestorationSerializeData restorationData, AngleConstraint.LimitSerializeData limitData)
			{
				this.useAngleRestoration = restorationData.useAngleRestoration;
				this.restorationStiffness = restorationData.stiffness.ConvertFloatArray() * 0.2f;
				this.restorationVelocityAttenuation = restorationData.velocityAttenuation;
				this.restorationGravityFalloff = restorationData.gravityFalloff;
				this.useAngleLimit = limitData.useAngleLimit;
				this.limitCurveData = limitData.limitAngle.ConvertFloatArray();
				this.limitstiffness = limitData.stiffness;
			}

			// Token: 0x0400010C RID: 268
			public bool useAngleRestoration;

			// Token: 0x0400010D RID: 269
			public float4x4 restorationStiffness;

			// Token: 0x0400010E RID: 270
			public float restorationVelocityAttenuation;

			// Token: 0x0400010F RID: 271
			public float restorationGravityFalloff;

			// Token: 0x04000110 RID: 272
			public bool useAngleLimit;

			// Token: 0x04000111 RID: 273
			public float4x4 limitCurveData;

			// Token: 0x04000112 RID: 274
			public float limitstiffness;
		}
	}
}
