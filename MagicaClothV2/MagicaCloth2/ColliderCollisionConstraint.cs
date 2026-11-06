using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x0200002C RID: 44
	public class ColliderCollisionConstraint : IDisposable
	{
		// Token: 0x060000CB RID: 203 RVA: 0x0000706D File Offset: 0x0000526D
		public void Dispose()
		{
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000083E5 File Offset: 0x000065E5
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[ColliderCollisionConstraint]");
			return stringBuilder.ToString();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00008400 File Offset: 0x00006600
		internal static void SolverPointConstraint(DataChunk chunk, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float> vertexDepths, ref NativeArray<float3> nextPosArray, ref NativeArray<float> frictionArray, ref NativeArray<float3> collisionNormalArray, ref NativeArray<float3> velocityPosArray, ref NativeArray<float3> basePosArray, ref NativeArray<ExBitFlag16> colliderFlagArray, ref NativeArray<ColliderManager.WorkData> colliderWorkDataArray)
		{
			if (tdata.UseColliderCount == 0)
			{
				return;
			}
			if (param.colliderCollisionConstraint.mode != ColliderCollisionConstraint.Mode.Point)
			{
				return;
			}
			if (!chunk.IsValid)
			{
				return;
			}
			bool isSpring = tdata.IsSpring;
			int num = tdata.particleChunk.startIndex + chunk.startIndex;
			int num2 = tdata.proxyCommonChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				float3 @float = nextPosArray[num];
				VertexAttribute vertexAttribute = attributes[num2];
				if (!vertexAttribute.IsInvalid() && !vertexAttribute.IsDisableCollision() && (vertexAttribute.IsMove() || tdata.IsSpring))
				{
					float num3 = vertexDepths[num2];
					float3 float2 = (isSpring ? basePosArray[num] : float3.zero);
					float num4 = math.max((in param.radiusCurveData).MC2EvaluateCurve(num3), 0.0001f);
					num4 *= tdata.scaleRatio;
					float num5 = float.MaxValue;
					int num6 = -1;
					float3 float3 = 0;
					float3 float4 = 0;
					float3 float5 = 0;
					int num7 = 0;
					float3 float6 = 0;
					float num8 = num4 * 1f;
					float3 float7 = @float - num4;
					float3 float8 = @float + num4;
					AABB aabb = new AABB(in float7, in float8);
					aabb.Expand(num8);
					float num9 = (isSpring ? (math.max((in param.colliderCollisionConstraint.limitDistance).MC2EvaluateCurve(num3), 0.0001f) * tdata.scaleRatio) : (-1f));
					int num10 = tdata.colliderChunk.startIndex;
					int colliderCount = tdata.colliderCount;
					int j = 0;
					while (j < colliderCount)
					{
						ExBitFlag16 exBitFlag = colliderFlagArray[num10];
						if (exBitFlag.IsSet(256) && exBitFlag.IsSet(512))
						{
							ColliderManager.ColliderType colliderType = DataUtility.GetColliderType(in exBitFlag);
							ColliderManager.WorkData workData = colliderWorkDataArray[num10];
							float num11 = 100f;
							float3 float9 = @float;
							switch (colliderType)
							{
							case ColliderManager.ColliderType.Sphere:
								num11 = ColliderCollisionConstraint.PointSphereColliderDetection(ref float9, in float2, num4, in aabb, in workData, isSpring, num9, out float4);
								break;
							case ColliderManager.ColliderType.CapsuleX_Center:
							case ColliderManager.ColliderType.CapsuleY_Center:
							case ColliderManager.ColliderType.CapsuleZ_Center:
							case ColliderManager.ColliderType.CapsuleX_Start:
							case ColliderManager.ColliderType.CapsuleY_Start:
							case ColliderManager.ColliderType.CapsuleZ_Start:
								num11 = ColliderCollisionConstraint.PointCapsuleColliderDetection(ref float9, num4, in aabb, in workData, out float4);
								break;
							case ColliderManager.ColliderType.Plane:
								num11 = ColliderCollisionConstraint.PointPlaneColliderDetction(ref float9, num4, in workData, out float4);
								break;
							default:
								Debug.LogError(string.Format("unknown collider type:{0}", colliderType));
								break;
							}
							if (num11 <= 0f)
							{
								float5 += float9 - @float;
								float6 += float4;
								num7++;
							}
							if (num11 <= num8)
							{
								num6 = num10;
								float3 += float4;
								num5 = math.min(num5, num11);
							}
						}
						j++;
						num10++;
					}
					if (num7 > 0)
					{
						float6 /= (float)num7;
						float num12 = math.length(float6);
						if (num12 < 1E-08f)
						{
							float5 = 0;
						}
						else
						{
							float num13 = math.min(num12, 1f);
							float5 /= (float)num7;
							@float += float5 * num13;
						}
					}
					if (num6 >= 0 && num8 > 0f && math.lengthsq(float3) > 1E-06f)
					{
						float num14 = 1f - math.saturate(num5 / num8);
						frictionArray[num] = math.max(num14, frictionArray[num]);
						float3 = math.normalize(float3);
					}
					collisionNormalArray[num] = float3;
					nextPosArray[num] = @float;
					if (isSpring && num7 > 0)
					{
						velocityPosArray[num] += float5;
					}
				}
				i++;
				num++;
				num2++;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000087B0 File Offset: 0x000069B0
		private static float PointSphereColliderDetection(ref float3 nextpos, in float3 basePos, float radius, in AABB aabb, in ColliderManager.WorkData cwork, bool isSpring, float maxLength, out float3 normal)
		{
			normal = 0;
			AABB aabb2 = aabb;
			if (!aabb2.Overlaps(in cwork.aabb))
			{
				return float.MaxValue;
			}
			float3 @float = nextpos;
			float3 c = cwork.oldPos.c0;
			float3 c2 = cwork.nextPos.c0;
			float x = cwork.radius.x;
			float3 float2 = math.normalize(nextpos - c);
			float3 float3 = c2 + float2 * (x + radius);
			normal = float2;
			float num = MathUtility.IntersectPointPlaneDist(in float3, in float2, in nextpos, out nextpos);
			if (maxLength > 0f)
			{
				nextpos = MathUtility.ClampDistance(basePos, nextpos, maxLength);
				float num2 = math.saturate(math.distance(basePos, nextpos) / radius);
				num2 = math.lerp(0f, 0.85f, num2);
				nextpos = math.lerp(nextpos, @float, num2);
				num *= 3f;
			}
			return num;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000088C0 File Offset: 0x00006AC0
		private static float PointPlaneColliderDetction(ref float3 nextpos, float radius, in ColliderManager.WorkData cwork, out float3 normal)
		{
			float3 c = cwork.nextPos.c0;
			float3 c2 = cwork.oldPos.c0;
			normal = c2;
			float3 @float = c + c2 * radius;
			return MathUtility.IntersectPointPlaneDist(in @float, in c2, in nextpos, out nextpos);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00008904 File Offset: 0x00006B04
		private static float PointCapsuleColliderDetection(ref float3 nextpos, float radius, in AABB aabb, in ColliderManager.WorkData cwork, out float3 normal)
		{
			normal = 0;
			AABB aabb2 = aabb;
			if (!aabb2.Overlaps(in cwork.aabb))
			{
				return float.MaxValue;
			}
			float3 c = cwork.oldPos.c0;
			float3 c2 = cwork.oldPos.c1;
			float3 c3 = cwork.nextPos.c0;
			float3 c4 = cwork.nextPos.c1;
			float x = cwork.radius.x;
			float y = cwork.radius.y;
			float num = MathUtility.ClosestPtPointSegmentRatio(in nextpos, in c, in c2);
			float num2 = math.lerp(x, y, num);
			float3 @float = math.lerp(c, c2, num);
			float3 float2 = nextpos - @float;
			float3 float3 = math.mul(cwork.inverseOldRot, float2);
			@float = math.lerp(c3, c4, num);
			float2 = math.mul(cwork.rot, float3);
			float3 float4 = math.normalize(float2);
			float3 float5 = @float + float4 * (num2 + radius);
			normal = float4;
			return MathUtility.IntersectPointPlaneDist(in float5, in float4, in nextpos, out nextpos);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00008A0C File Offset: 0x00006C0C
		internal unsafe static void SolverEdgeConstraint(DataChunk chunk, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<VertexAttribute> attributes, ref NativeArray<float> vertexDepths, ref NativeArray<int2> edges, ref NativeArray<float3> nextPosArray, ref NativeArray<ExBitFlag16> colliderFlagArray, ref NativeArray<ColliderManager.WorkData> colliderWorkDataArray, ref NativeArray<float3> tempVectorBufferA, ref NativeArray<float3> tempVectorBufferB, ref NativeArray<int> tempCountBuffer, ref NativeArray<float> tempFloatBufferA)
		{
			if (tdata.UseColliderCount == 0)
			{
				return;
			}
			if (param.colliderCollisionConstraint.mode != ColliderCollisionConstraint.Mode.Edge)
			{
				return;
			}
			if (!chunk.IsValid)
			{
				return;
			}
			int* unsafePtr = (int*)tempVectorBufferA.GetUnsafePtr<float3>();
			int* unsafePtr2 = (int*)tempVectorBufferB.GetUnsafePtr<float3>();
			int* unsafePtr3 = (int*)tempCountBuffer.GetUnsafePtr<int>();
			int* unsafePtr4 = (int*)tempFloatBufferA.GetUnsafePtr<float>();
			int startIndex = tdata.proxyCommonChunk.startIndex;
			int num = tdata.proxyEdgeChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				int2 @int = edges[num];
				int2 int2 = @int + startIndex;
				VertexAttribute vertexAttribute = attributes[int2.x];
				VertexAttribute vertexAttribute2 = attributes[int2.y];
				bool flag = vertexAttribute.IsMove();
				bool flag2 = vertexAttribute2.IsMove();
				if (flag || flag2)
				{
					int startIndex2 = tdata.particleChunk.startIndex;
					int2 int3 = @int + startIndex2;
					float3x2 float3x = new float3x2(nextPosArray[int3.x], nextPosArray[int3.y]);
					float2 @float = new float2(vertexDepths[int2.x], vertexDepths[int2.y]);
					float2 float2 = new float2((in param.radiusCurveData).MC2EvaluateCurve(@float.x), (in param.radiusCurveData).MC2EvaluateCurve(@float.y));
					float2 *= tdata.scaleRatio;
					float num2 = (float2.x + float2.y) * 0.5f * 1f;
					float num3 = float.MaxValue;
					int num4 = -1;
					float3 float3 = 0;
					float3 float4 = 0;
					float3 float5 = float3x.c0 - float2.x;
					float3 float6 = float3x.c0 + float2.x;
					AABB aabb = new AABB(in float5, in float6);
					float5 = float3x.c1 - float2.y;
					float6 = float3x.c1 + float2.y;
					AABB aabb2 = new AABB(in float5, in float6);
					aabb.Encapsulate(in aabb2);
					aabb.Expand(num2);
					float3x2 float3x2 = 0;
					int num5 = 0;
					float3 float7 = 0;
					int num6 = tdata.colliderChunk.startIndex;
					int colliderCount = tdata.colliderCount;
					int j = 0;
					while (j < colliderCount)
					{
						ExBitFlag16 exBitFlag = colliderFlagArray[num6];
						if (exBitFlag.IsSet(256) && exBitFlag.IsSet(512))
						{
							ColliderManager.ColliderType colliderType = DataUtility.GetColliderType(in exBitFlag);
							ColliderManager.WorkData workData = colliderWorkDataArray[num6];
							float num7 = 100f;
							float3x2 float3x3 = float3x;
							switch (colliderType)
							{
							case ColliderManager.ColliderType.Sphere:
								num7 = ColliderCollisionConstraint.EdgeSphereColliderDetection(ref float3x3, in float2, in aabb, num2, in workData, out float4);
								break;
							case ColliderManager.ColliderType.CapsuleX_Center:
							case ColliderManager.ColliderType.CapsuleY_Center:
							case ColliderManager.ColliderType.CapsuleZ_Center:
							case ColliderManager.ColliderType.CapsuleX_Start:
							case ColliderManager.ColliderType.CapsuleY_Start:
							case ColliderManager.ColliderType.CapsuleZ_Start:
								num7 = ColliderCollisionConstraint.EdgeCapsuleColliderDetection(ref float3x3, in float2, in aabb, num2, in workData, out float4);
								break;
							case ColliderManager.ColliderType.Plane:
								num7 = ColliderCollisionConstraint.EdgePlaneColliderDetection(ref float3x3, in float2, in workData, out float4);
								break;
							default:
								Debug.LogError(string.Format("Unknown collider type:{0}", colliderType));
								break;
							}
							if (num7 <= 0f)
							{
								float3x2 += float3x3 - float3x;
								float7 += float4;
								num5++;
							}
							if (num7 <= num2)
							{
								num4 = num6;
								float3 += float4;
								num3 = math.min(num3, num7);
							}
						}
						j++;
						num6++;
					}
					if (num5 > 0)
					{
						float7 /= (float)num5;
						float num8 = math.length(float7);
						if (num8 > 1E-08f)
						{
							float num9 = math.min(num8, 1f);
							float3x2 /= (float)num5;
							float3x2 *= num9;
							InterlockUtility.AddFloat3(int3.x, float3x2.c0, unsafePtr3, unsafePtr);
							InterlockUtility.AddFloat3(int3.y, float3x2.c1, unsafePtr3, unsafePtr);
						}
					}
					if (num4 >= 0 && num2 > 0f && math.lengthsq(float3) > 1E-06f)
					{
						float num10 = 1f - math.saturate(num3 / num2);
						float3 = math.normalize(float3);
						if (flag)
						{
							InterlockUtility.Max(int3.x, num10, unsafePtr4);
							InterlockUtility.AddFloat3(int3.x, float3, unsafePtr2);
						}
						if (flag2)
						{
							InterlockUtility.Max(int3.y, num10, unsafePtr4);
							InterlockUtility.AddFloat3(int3.y, float3, unsafePtr2);
						}
					}
				}
				i++;
				num++;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00008E94 File Offset: 0x00007094
		internal unsafe static void SumEdgeConstraint(DataChunk chunk, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<float3> nextPosArray, ref NativeArray<float> frictionArray, ref NativeArray<float3> collisionNormalArray, ref NativeArray<float3> tempVectorBufferA, ref NativeArray<float3> tempVectorBufferB, ref NativeArray<int> tempCountBuffer, ref NativeArray<float> tempFloatBufferA)
		{
			if (tdata.UseColliderCount == 0)
			{
				return;
			}
			if (param.colliderCollisionConstraint.mode != ColliderCollisionConstraint.Mode.Edge)
			{
				return;
			}
			if (!chunk.IsValid)
			{
				return;
			}
			int* unsafePtr = (int*)tempVectorBufferA.GetUnsafePtr<float3>();
			int* unsafePtr2 = (int*)tempVectorBufferB.GetUnsafePtr<float3>();
			int* unsafePtr3 = (int*)tempCountBuffer.GetUnsafePtr<int>();
			int* unsafePtr4 = (int*)tempFloatBufferA.GetUnsafePtr<float>();
			int num = tdata.particleChunk.startIndex + chunk.startIndex;
			int num2 = tdata.proxyCommonChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				if (tempCountBuffer[num] > 0)
				{
					float3 @float = InterlockUtility.ReadAverageFloat3(num, unsafePtr3, unsafePtr);
					nextPosArray[num] += @float;
				}
				float num3 = InterlockUtility.ReadFloat(num, unsafePtr4);
				if (num3 > 0f && num3 > frictionArray[num])
				{
					frictionArray[num] = num3;
				}
				float3 float2 = InterlockUtility.ReadFloat3(num, unsafePtr2);
				if (math.lengthsq(float2) > 0f)
				{
					float2 = math.normalize(float2);
					collisionNormalArray[num] = float2;
				}
				tempVectorBufferA[num] = 0;
				tempVectorBufferB[num] = 0;
				tempCountBuffer[num] = 0;
				tempFloatBufferA[num] = 0f;
				i++;
				num++;
				num2++;
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00009004 File Offset: 0x00007204
		private static float EdgeSphereColliderDetection(ref float3x2 nextPosE, in float2 radiusE, in AABB aabbE, float cfr, in ColliderManager.WorkData cwork, out float3 normal)
		{
			normal = 0;
			AABB aabb = aabbE;
			if (!aabb.Overlaps(in cwork.aabb))
			{
				return float.MaxValue;
			}
			float3 c = cwork.oldPos.c0;
			float3 c2 = cwork.nextPos.c0;
			float x = cwork.radius.x;
			float num = MathUtility.ClosestPtPointSegmentRatio(in c, in nextPosE.c0, in nextPosE.c1);
			float3 @float = math.lerp(nextPosE.c0, nextPosE.c1, num);
			float3 float2 = @float - c;
			float num2 = math.length(float2);
			if (num2 < 1E-09f)
			{
				return float.MaxValue;
			}
			float3 float3 = float2 / num2;
			normal = float3;
			float3 float4 = c2 - c;
			float num3 = math.dot(float3, float4);
			float num4 = num2 - num3;
			float num5 = math.lerp(radiusE.x, radiusE.y, num);
			float num6 = x;
			float num7 = num5 + num6;
			if (num4 > num7 + cfr)
			{
				return float.MaxValue;
			}
			float2 = @float - c2;
			num4 = math.dot(float3, float2);
			if (num4 > num7)
			{
				return num4 - num7;
			}
			float num8 = num7 - num4;
			float2 float5 = new float2(1f - num, num);
			float3x2 float3x = new float3x2(float3 * float5.x, float3 * float5.y);
			float num9 = math.dot(float5, float5);
			if (num9 == 0f)
			{
				return float.MaxValue;
			}
			num9 = num8 / num9;
			float3x2 float3x2 = float3x * num9;
			nextPosE += float3x2;
			return -num8;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000091A0 File Offset: 0x000073A0
		private static float EdgeCapsuleColliderDetection(ref float3x2 nextPosE, in float2 radiusE, in AABB aabbE, float cfr, in ColliderManager.WorkData cwork, out float3 normal)
		{
			normal = 0;
			AABB aabb = aabbE;
			if (!aabb.Overlaps(in cwork.aabb))
			{
				return float.MaxValue;
			}
			float3 c = cwork.oldPos.c0;
			float3 c2 = cwork.oldPos.c1;
			float3 c3 = cwork.nextPos.c0;
			float3 c4 = cwork.nextPos.c1;
			float x = cwork.radius.x;
			float y = cwork.radius.y;
			float num2;
			float num3;
			float3 @float;
			float3 float2;
			float num = math.sqrt(MathUtility.ClosestPtSegmentSegment(in nextPosE.c0, in nextPosE.c1, in c, in c2, out num2, out num3, out @float, out float2));
			if (num < 1E-09f)
			{
				return float.MaxValue;
			}
			float3 float3 = @float - float2;
			float3 float4 = float3 / num;
			normal = float4;
			if (x != y)
			{
				float3 float5 = c + float4 * x;
				float3 float6 = c2 + float4 * y;
				MathUtility.ClosestPtSegmentSegment2(in nextPosE.c0, in nextPosE.c1, in float5, in float6, out num2, out num3);
				@float = math.lerp(nextPosE.c0, nextPosE.c1, num2);
				float2 = math.lerp(c, c2, num3);
				float3 = @float - float2;
				num = math.length(float3);
				float4 = float3 / num;
				normal = float4;
			}
			float3 float7 = c3 - c;
			float3 float8 = c4 - c2;
			float3 float9 = math.lerp(float7, float8, num3);
			float num4 = math.dot(float4, float9);
			float num5 = num - num4;
			float num6 = math.lerp(radiusE.x, radiusE.y, num2);
			float num7 = math.lerp(x, y, num3);
			float num8 = num6 + num7;
			if (num5 > num8 + cfr)
			{
				return float.MaxValue;
			}
			float3 float10 = math.lerp(c3, c4, num3);
			float3 = @float - float10;
			num5 = math.dot(float4, float3);
			if (num5 > num8)
			{
				return num5 - num8;
			}
			float num9 = num8 - num5;
			float2 float11 = new float2(1f - num2, num2);
			float3x2 float3x = new float3x2(float4 * float11.x, float4 * float11.y);
			float num10 = math.dot(float11, float11);
			if (num10 == 0f)
			{
				return float.MaxValue;
			}
			num10 = num9 / num10;
			float3x2 float3x2 = float3x * num10;
			nextPosE += float3x2;
			return -num9;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00009410 File Offset: 0x00007610
		private static float EdgePlaneColliderDetection(ref float3x2 nextPosE, in float2 radiusE, in ColliderManager.WorkData cwork, out float3 normal)
		{
			float3 c = cwork.nextPos.c0;
			float3 c2 = cwork.oldPos.c0;
			normal = c2;
			float3 @float = c + c2 * radiusE.x;
			float num = MathUtility.IntersectPointPlaneDist(in @float, in c2, in nextPosE.c0, out nextPosE.c0);
			@float = c + c2 * radiusE.y;
			float num2 = MathUtility.IntersectPointPlaneDist(in @float, in c2, in nextPosE.c1, out nextPosE.c1);
			return math.min(num, num2);
		}

		// Token: 0x0200002D RID: 45
		public enum Mode
		{
			// Token: 0x04000114 RID: 276
			None,
			// Token: 0x04000115 RID: 277
			Point,
			// Token: 0x04000116 RID: 278
			Edge
		}

		// Token: 0x0200002E RID: 46
		[Serializable]
		public class SerializeData : IDataValidate, ITransform
		{
			// Token: 0x060000D6 RID: 214 RVA: 0x00009493 File Offset: 0x00007693
			public SerializeData()
			{
				this.mode = ColliderCollisionConstraint.Mode.Point;
				this.friction = 0.05f;
			}

			// Token: 0x060000D7 RID: 215 RVA: 0x000094D3 File Offset: 0x000076D3
			public void DataValidate()
			{
				this.friction = Mathf.Clamp(this.friction, 0f, 0.5f);
				this.limitDistance.DataValidate(0f, 1f);
			}

			// Token: 0x060000D8 RID: 216 RVA: 0x00009508 File Offset: 0x00007708
			public ColliderCollisionConstraint.SerializeData Clone()
			{
				return new ColliderCollisionConstraint.SerializeData
				{
					mode = this.mode,
					friction = this.friction,
					colliderList = new List<ColliderComponent>(this.colliderList),
					collisionBones = new List<Transform>(this.collisionBones)
				};
			}

			// Token: 0x060000D9 RID: 217 RVA: 0x00009554 File Offset: 0x00007754
			public override int GetHashCode()
			{
				int num = 0;
				foreach (Transform transform in this.collisionBones)
				{
					if (transform)
					{
						num += transform.GetInstanceID();
					}
				}
				return num;
			}

			// Token: 0x060000DA RID: 218 RVA: 0x000095B4 File Offset: 0x000077B4
			public void GetUsedTransform(HashSet<Transform> transformSet)
			{
				this.colliderList.ForEach(delegate(ColliderComponent x)
				{
					if (x)
					{
						x.GetUsedTransform(transformSet);
					}
				});
				foreach (Transform transform in this.collisionBones)
				{
					if (transform)
					{
						transformSet.Add(transform);
					}
				}
			}

			// Token: 0x060000DB RID: 219 RVA: 0x0000963C File Offset: 0x0000783C
			public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
			{
				this.colliderList.ForEach(delegate(ColliderComponent x)
				{
					if (x)
					{
						x.ReplaceTransform(replaceDict);
					}
				});
				for (int i = 0; i < this.collisionBones.Count; i++)
				{
					Transform transform = this.collisionBones[i];
					if (transform && replaceDict.ContainsKey(transform.GetInstanceID()))
					{
						this.collisionBones[i] = replaceDict[transform.GetInstanceID()];
					}
				}
			}

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x060000DC RID: 220 RVA: 0x000096C8 File Offset: 0x000078C8
			public int ColliderLength
			{
				get
				{
					return this.colliderList.Count;
				}
			}

			// Token: 0x04000117 RID: 279
			public ColliderCollisionConstraint.Mode mode;

			// Token: 0x04000118 RID: 280
			[Range(0f, 0.5f)]
			public float friction;

			// Token: 0x04000119 RID: 281
			public List<ColliderComponent> colliderList = new List<ColliderComponent>();

			// Token: 0x0400011A RID: 282
			public List<Transform> collisionBones = new List<Transform>();

			// Token: 0x0400011B RID: 283
			public CurveSerializeData limitDistance = new CurveSerializeData(0.05f);
		}

		// Token: 0x02000031 RID: 49
		public struct ColliderCollisionConstraintParams
		{
			// Token: 0x060000E1 RID: 225 RVA: 0x00009704 File Offset: 0x00007904
			public void Convert(ColliderCollisionConstraint.SerializeData sdata, ClothProcess.ClothType clothType)
			{
				if (clothType <= ClothProcess.ClothType.BoneCloth)
				{
					this.mode = sdata.mode;
					this.dynamicFriction = sdata.friction * 1f;
					this.staticFriction = sdata.friction * 1f;
					return;
				}
				if (clothType != ClothProcess.ClothType.BoneSpring)
				{
					return;
				}
				this.mode = ColliderCollisionConstraint.Mode.Point;
				this.limitDistance = sdata.limitDistance.ConvertFloatArray();
				this.dynamicFriction = 0.5f;
				this.staticFriction = 0.5f;
			}

			// Token: 0x0400011E RID: 286
			public ColliderCollisionConstraint.Mode mode;

			// Token: 0x0400011F RID: 287
			public float dynamicFriction;

			// Token: 0x04000120 RID: 288
			public float staticFriction;

			// Token: 0x04000121 RID: 289
			public float4x4 limitDistance;
		}
	}
}
