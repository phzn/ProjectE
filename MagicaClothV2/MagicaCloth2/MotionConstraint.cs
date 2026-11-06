using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x0200003C RID: 60
	public class MotionConstraint : IDisposable
	{
		// Token: 0x060000FC RID: 252 RVA: 0x0000706D File Offset: 0x0000526D
		public void Dispose()
		{
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000A968 File Offset: 0x00008B68
		internal static void SolverConstraint(DataChunk chunk, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float> vertexDepths, ref NativeArray<float3> basePosArray, ref NativeArray<quaternion> baseRotArray, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> velocityPosArray, ref NativeArray<float> frictionArray, ref NativeArray<float3> collisionNormalArray)
		{
			if (!param.motionConstraint.useMaxDistance && !param.motionConstraint.useBackstop)
			{
				return;
			}
			float stiffness = param.motionConstraint.stiffness;
			float backstopRadius = param.motionConstraint.backstopRadius;
			int num = tdata.particleChunk.startIndex + chunk.startIndex;
			int num2 = tdata.proxyCommonChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				VertexAttribute vertexAttribute = attributes[num2];
				if (vertexAttribute.IsMove())
				{
					float3 @float = nextPosArray[num];
					float3 float2 = basePosArray[num];
					float num3 = vertexDepths[num2];
					if (vertexAttribute.IsMotion())
					{
						float3 float3 = @float;
						float num4 = math.max((in param.radiusCurveData).MC2EvaluateCurve(num3), 0.0001f) * 1f;
						num3 *= num3;
						quaternion quaternion = baseRotArray[num];
						float3 float4 = math.up();
						switch (param.normalAxis)
						{
						case ClothNormalAxis.Right:
							float4 = math.right();
							break;
						case ClothNormalAxis.Up:
							float4 = math.up();
							break;
						case ClothNormalAxis.Forward:
							float4 = math.forward();
							break;
						case ClothNormalAxis.InverseRight:
							float4 = -math.right();
							break;
						case ClothNormalAxis.InverseUp:
							float4 = -math.up();
							break;
						case ClothNormalAxis.InverseForward:
							float4 = -math.forward();
							break;
						}
						float4 = math.mul(quaternion, float4);
						if (param.motionConstraint.useMaxDistance)
						{
							float num5 = (in param.motionConstraint.maxDistanceCurveData).MC2EvaluateCurve(num3);
							float3 float5 = float2;
							float3 float6 = MathUtility.ClampVector(@float - float5, num5);
							@float = float5 + float6;
						}
						if (param.motionConstraint.useBackstop)
						{
							float num6 = (in param.motionConstraint.backstopDistanceCurveData).MC2EvaluateCurve(num3);
							if (backstopRadius > 0f)
							{
								float3 float7 = float2 + -float4 * (num6 + backstopRadius);
								float3 float8 = @float - float7;
								float num7 = math.length(float8);
								if (num7 > 1E-08f && num7 < backstopRadius + num4)
								{
									float3 float9 = float8 / num7;
									if (num7 < backstopRadius)
									{
										@float = float7 + float9 * backstopRadius;
									}
								}
							}
						}
						@float = math.lerp(float3, @float, stiffness);
						nextPosArray[num] = @float;
						float3 float10 = @float - float3;
						velocityPosArray[num] += float10 * 0.95f;
					}
				}
				i++;
				num++;
				num2++;
			}
		}

		// Token: 0x0200003D RID: 61
		[Serializable]
		public class SerializeData : IDataValidate
		{
			// Token: 0x060000FF RID: 255 RVA: 0x0000ABF0 File Offset: 0x00008DF0
			public SerializeData()
			{
				this.useMaxDistance = false;
				this.maxDistance = new CurveSerializeData(0.3f);
				this.useBackstop = false;
				this.backstopRadius = 10f;
				this.backstopDistance = new CurveSerializeData(0f);
				this.stiffness = 1f;
			}

			// Token: 0x06000100 RID: 256 RVA: 0x0000AC48 File Offset: 0x00008E48
			public void DataValidate()
			{
				this.maxDistance.DataValidate(0f, 5f);
				this.backstopRadius = Mathf.Clamp(this.backstopRadius, 0f, 10f);
				this.backstopDistance.DataValidate(0f, 1f);
				this.stiffness = Mathf.Clamp01(this.stiffness);
			}

			// Token: 0x06000101 RID: 257 RVA: 0x0000ACAC File Offset: 0x00008EAC
			public MotionConstraint.SerializeData Clone()
			{
				return new MotionConstraint.SerializeData
				{
					useMaxDistance = this.useMaxDistance,
					maxDistance = this.maxDistance.Clone(),
					useBackstop = this.useBackstop,
					backstopRadius = this.backstopRadius,
					backstopDistance = this.backstopDistance.Clone(),
					stiffness = this.stiffness
				};
			}

			// Token: 0x0400017A RID: 378
			public bool useMaxDistance;

			// Token: 0x0400017B RID: 379
			public CurveSerializeData maxDistance;

			// Token: 0x0400017C RID: 380
			public bool useBackstop;

			// Token: 0x0400017D RID: 381
			[Range(0.1f, 10f)]
			public float backstopRadius;

			// Token: 0x0400017E RID: 382
			public CurveSerializeData backstopDistance;

			// Token: 0x0400017F RID: 383
			[Range(0f, 1f)]
			public float stiffness;
		}

		// Token: 0x0200003E RID: 62
		public struct MotionConstraintParams
		{
			// Token: 0x06000102 RID: 258 RVA: 0x0000AD10 File Offset: 0x00008F10
			public void Convert(MotionConstraint.SerializeData sdata, ClothProcess.ClothType clothType)
			{
				this.useMaxDistance = clothType != ClothProcess.ClothType.BoneSpring && sdata.useMaxDistance;
				this.maxDistanceCurveData = sdata.maxDistance.ConvertFloatArray();
				this.useBackstop = clothType != ClothProcess.ClothType.BoneSpring && sdata.useBackstop;
				this.backstopRadius = sdata.backstopRadius;
				this.backstopDistanceCurveData = sdata.backstopDistance.ConvertFloatArray();
				this.stiffness = sdata.stiffness;
			}

			// Token: 0x04000180 RID: 384
			public bool useMaxDistance;

			// Token: 0x04000181 RID: 385
			public float4x4 maxDistanceCurveData;

			// Token: 0x04000182 RID: 386
			public bool useBackstop;

			// Token: 0x04000183 RID: 387
			public float backstopRadius;

			// Token: 0x04000184 RID: 388
			public float4x4 backstopDistanceCurveData;

			// Token: 0x04000185 RID: 389
			public float stiffness;
		}
	}
}
