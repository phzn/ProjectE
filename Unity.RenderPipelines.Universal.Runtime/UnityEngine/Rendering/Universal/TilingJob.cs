using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000E1 RID: 225
	[BurstCompile(FloatMode = FloatMode.Default, DisableSafetyChecks = true, OptimizeFor = OptimizeFor.Performance)]
	internal struct TilingJob : IJobFor
	{
		// Token: 0x060006EF RID: 1775 RVA: 0x000298B8 File Offset: 0x00027AB8
		public void Execute(int jobIndex)
		{
			int num = jobIndex % this.itemsPerTile;
			this.m_ViewIndex = jobIndex / this.itemsPerTile;
			this.m_Offset = jobIndex * this.rangesPerItem;
			this.m_TileYRange = new InclusiveRange(short.MaxValue, short.MinValue);
			for (int i = 0; i < this.rangesPerItem; i++)
			{
				this.tileRanges[this.m_Offset + i] = new InclusiveRange(short.MaxValue, short.MinValue);
			}
			if (num >= this.lights.Length)
			{
				this.TileReflectionProbe(num);
				return;
			}
			if (this.isOrthographic)
			{
				this.TileLightOrthographic(num);
				return;
			}
			this.TileLight(num);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00029960 File Offset: 0x00027B60
		private void TileLight(int lightIndex)
		{
			TilingJob.<>c__DisplayClass19_0 CS$<>8__locals1;
			CS$<>8__locals1.light = this.lights[lightIndex];
			float4x4 float4x = CS$<>8__locals1.light.localToWorldMatrix;
			CS$<>8__locals1.lightPositionVS = math.mul(this.worldToViews[this.m_ViewIndex], math.float4(float4x.c3.xyz, 1f)).xyz;
			CS$<>8__locals1.lightPositionVS.z = CS$<>8__locals1.lightPositionVS.z * -1f;
			if (CS$<>8__locals1.lightPositionVS.z >= this.near)
			{
				this.ExpandY(CS$<>8__locals1.lightPositionVS);
			}
			CS$<>8__locals1.lightDirectionVS = math.normalize(math.mul(this.worldToViews[this.m_ViewIndex], math.float4(float4x.c2.xyz, 0f)).xyz);
			CS$<>8__locals1.lightDirectionVS.z = CS$<>8__locals1.lightDirectionVS.z * -1f;
			float num = math.radians(CS$<>8__locals1.light.spotAngle * 0.5f);
			float range = CS$<>8__locals1.light.range;
			float num2 = TilingJob.square(range);
			CS$<>8__locals1.cosHalfAngle = math.cos(num);
			CS$<>8__locals1.coneHeight = CS$<>8__locals1.cosHalfAngle * range;
			float num3 = math.sqrt(num2 - TilingJob.square(this.near - CS$<>8__locals1.lightPositionVS.z));
			float2 @float;
			float2 float2;
			TilingJob.GetSphereHorizon(CS$<>8__locals1.lightPositionVS.yz, range, this.near, num3, out @float, out float2);
			float3 float3 = math.float3(CS$<>8__locals1.lightPositionVS.x, @float);
			float3 float4 = math.float3(CS$<>8__locals1.lightPositionVS.x, float2);
			if (TilingJob.<TileLight>g__SpherePointIsValid|19_0(float3, ref CS$<>8__locals1))
			{
				this.ExpandY(float3);
			}
			if (TilingJob.<TileLight>g__SpherePointIsValid|19_0(float4, ref CS$<>8__locals1))
			{
				this.ExpandY(float4);
			}
			float2 float5;
			float2 float6;
			TilingJob.GetSphereHorizon(CS$<>8__locals1.lightPositionVS.xz, range, this.near, num3, out float5, out float6);
			float3 float7 = math.float3(float5.x, CS$<>8__locals1.lightPositionVS.y, float5.y);
			float3 float8 = math.float3(float6.x, CS$<>8__locals1.lightPositionVS.y, float6.y);
			if (TilingJob.<TileLight>g__SpherePointIsValid|19_0(float7, ref CS$<>8__locals1))
			{
				this.ExpandY(float7);
			}
			if (TilingJob.<TileLight>g__SpherePointIsValid|19_0(float8, ref CS$<>8__locals1))
			{
				this.ExpandY(float8);
			}
			if (CS$<>8__locals1.light.lightType == LightType.Spot)
			{
				float num4 = math.sqrt(range * range - CS$<>8__locals1.coneHeight * CS$<>8__locals1.coneHeight);
				float3 float9 = CS$<>8__locals1.lightPositionVS + CS$<>8__locals1.lightDirectionVS * CS$<>8__locals1.coneHeight;
				float3 float10 = ((math.abs(math.abs(CS$<>8__locals1.lightDirectionVS.x) - 1f) < 1E-06f) ? math.float3(0f, 1f, 0f) : math.normalize(math.cross(CS$<>8__locals1.lightDirectionVS, math.float3(1f, 0f, 0f))));
				float3 float11 = math.cross(CS$<>8__locals1.lightDirectionVS, float10);
				float2 float12;
				float2 float13;
				TilingJob.GetProjectedCircleHorizon(float9.yz, num4, float10.yz, float11.yz, out float12, out float13);
				float3 float14 = float9 + float12.x * float10 + float12.y * float11;
				float3 float15 = float9 + float13.x * float10 + float13.y * float11;
				if (float14.z >= this.near)
				{
					this.ExpandY(float14);
				}
				if (float15.z >= this.near)
				{
					this.ExpandY(float15);
				}
				float3 float16 = ((math.abs(math.abs(CS$<>8__locals1.lightDirectionVS.y) - 1f) < 1E-06f) ? math.float3(1f, 0f, 0f) : math.normalize(math.cross(CS$<>8__locals1.lightDirectionVS, math.float3(0f, 1f, 0f))));
				float3 float17 = math.cross(CS$<>8__locals1.lightDirectionVS, float16);
				float2 float18;
				float2 float19;
				TilingJob.GetProjectedCircleHorizon(float9.xz, num4, float16.xz, float17.xz, out float18, out float19);
				float3 float20 = float9 + float18.x * float16 + float18.y * float17;
				float3 float21 = float9 + float19.x * float16 + float19.y * float17;
				if (float20.z >= this.near)
				{
					this.ExpandY(float20);
				}
				if (float21.z >= this.near)
				{
					this.ExpandY(float21);
				}
				float3 float22;
				float3 float23;
				if (TilingJob.GetCircleClipPoints(float9, CS$<>8__locals1.lightDirectionVS, num4, this.near, out float22, out float23))
				{
					this.ExpandY(float22);
					this.ExpandY(float23);
				}
				float num5 = num4 * math.sqrt(1f - TilingJob.square(CS$<>8__locals1.lightDirectionVS.z));
				bool flag = this.near >= math.min(float9.z - num5, CS$<>8__locals1.lightPositionVS.z) && this.near <= math.max(float9.z + num5, CS$<>8__locals1.lightPositionVS.z);
				float3 float24 = math.cross(CS$<>8__locals1.lightDirectionVS, CS$<>8__locals1.lightPositionVS);
				float24 = ((math.csum(float24) != 0f) ? math.normalize(float24) : math.float3(1f, 0f, 0f));
				float3 float25 = math.cross(CS$<>8__locals1.lightDirectionVS, float24);
				if (flag)
				{
					float num6 = num4 / CS$<>8__locals1.coneHeight;
					float2 float26 = TilingJob.FindNearConicTangentTheta(CS$<>8__locals1.lightPositionVS.yz, CS$<>8__locals1.lightDirectionVS.yz, num6, float24.yz, float25.yz);
					float3 float27 = TilingJob.EvaluateNearConic(this.near, CS$<>8__locals1.lightPositionVS, CS$<>8__locals1.lightDirectionVS, num6, float24, float25, float26.x);
					float3 float28 = TilingJob.EvaluateNearConic(this.near, CS$<>8__locals1.lightPositionVS, CS$<>8__locals1.lightDirectionVS, num6, float24, float25, float26.y);
					if (TilingJob.<TileLight>g__ConicPointIsValid|19_1(float27, ref CS$<>8__locals1))
					{
						this.ExpandY(float27);
					}
					if (TilingJob.<TileLight>g__ConicPointIsValid|19_1(float28, ref CS$<>8__locals1))
					{
						this.ExpandY(float28);
					}
					float2 float29 = TilingJob.FindNearConicTangentTheta(CS$<>8__locals1.lightPositionVS.xz, CS$<>8__locals1.lightDirectionVS.xz, num6, float24.xz, float25.xz);
					float3 float30 = TilingJob.EvaluateNearConic(this.near, CS$<>8__locals1.lightPositionVS, CS$<>8__locals1.lightDirectionVS, num6, float24, float25, float29.x);
					float3 float31 = TilingJob.EvaluateNearConic(this.near, CS$<>8__locals1.lightPositionVS, CS$<>8__locals1.lightDirectionVS, num6, float24, float25, float29.y);
					if (TilingJob.<TileLight>g__ConicPointIsValid|19_1(float30, ref CS$<>8__locals1))
					{
						this.ExpandY(float30);
					}
					if (TilingJob.<TileLight>g__ConicPointIsValid|19_1(float31, ref CS$<>8__locals1))
					{
						this.ExpandY(float31);
					}
				}
				float3 float32;
				float3 float33;
				TilingJob.GetConeSideTangentPoints(CS$<>8__locals1.lightPositionVS, CS$<>8__locals1.lightDirectionVS, CS$<>8__locals1.cosHalfAngle, num4, CS$<>8__locals1.coneHeight, range, float24, float25, out float32, out float33);
				float3 float34 = math.float3(0f, 1f, this.viewPlaneBottoms[this.m_ViewIndex]);
				float num7 = math.dot(-CS$<>8__locals1.lightPositionVS, float34) / math.dot(float32, float34);
				float3 float35 = CS$<>8__locals1.lightPositionVS + float32 * num7;
				if (num7 >= 0f && num7 <= 1f && float35.z >= this.near)
				{
					this.ExpandY(float35);
				}
				float3 float36 = math.float3(0f, 1f, this.viewPlaneTops[this.m_ViewIndex]);
				float num8 = math.dot(-CS$<>8__locals1.lightPositionVS, float36) / math.dot(float32, float36);
				float3 float37 = CS$<>8__locals1.lightPositionVS + float32 * num8;
				if (num8 >= 0f && num8 <= 1f && float37.z >= this.near)
				{
					this.ExpandY(float37);
				}
				this.m_TileYRange.Clamp(0, (short)(this.tileCount.y - 1));
				for (int i = (int)(this.m_TileYRange.start + 1); i <= (int)this.m_TileYRange.end; i++)
				{
					InclusiveRange empty = InclusiveRange.empty;
					float num9 = math.lerp(this.viewPlaneBottoms[this.m_ViewIndex], this.viewPlaneTops[this.m_ViewIndex], (float)i * this.tileScaleInv.y);
					float3 float38 = math.float3(0f, 1f, -num9);
					float num10 = math.dot(-CS$<>8__locals1.lightPositionVS, float38) / math.dot(float32, float38);
					float3 float39 = CS$<>8__locals1.lightPositionVS + float32 * num10;
					if (num10 >= 0f && num10 <= 1f && float39.z >= this.near)
					{
						empty.Expand((short)math.clamp(this.ViewToTileSpace(float39).x, 0f, (float)(this.tileCount.x - 1)));
					}
					float num11 = math.dot(-CS$<>8__locals1.lightPositionVS, float38) / math.dot(float33, float38);
					float3 float40 = CS$<>8__locals1.lightPositionVS + float33 * num11;
					if (num11 >= 0f && num11 <= 1f && float40.z >= this.near)
					{
						empty.Expand((short)math.clamp(this.ViewToTileSpace(float40).x, 0f, (float)(this.tileCount.x - 1)));
					}
					float3 float41;
					float3 float42;
					if (TilingJob.IntersectCircleYPlane(num9, float9, CS$<>8__locals1.lightDirectionVS, float10, float11, num4, out float41, out float42))
					{
						if (float41.z >= this.near)
						{
							empty.Expand((short)math.clamp(this.ViewToTileSpace(float41).x, 0f, (float)(this.tileCount.x - 1)));
						}
						if (float42.z >= this.near)
						{
							empty.Expand((short)math.clamp(this.ViewToTileSpace(float42).x, 0f, (float)(this.tileCount.x - 1)));
						}
					}
					if (flag)
					{
						float num12 = num9 * this.near;
						float num13 = num4 / CS$<>8__locals1.coneHeight;
						float2 float43 = TilingJob.FindNearConicYTheta(this.near, CS$<>8__locals1.lightPositionVS, CS$<>8__locals1.lightDirectionVS, num13, float24, float25, num12);
						float3 float44 = math.float3(TilingJob.EvaluateNearConic(this.near, CS$<>8__locals1.lightPositionVS, CS$<>8__locals1.lightDirectionVS, num13, float24, float25, float43.x).x, num12, this.near);
						float3 float45 = math.float3(TilingJob.EvaluateNearConic(this.near, CS$<>8__locals1.lightPositionVS, CS$<>8__locals1.lightDirectionVS, num13, float24, float25, float43.y).x, num12, this.near);
						if (TilingJob.<TileLight>g__ConicPointIsValid|19_1(float44, ref CS$<>8__locals1))
						{
							empty.Expand((short)math.clamp(this.ViewToTileSpace(float44).x, 0f, (float)(this.tileCount.x - 1)));
						}
						if (TilingJob.<TileLight>g__ConicPointIsValid|19_1(float45, ref CS$<>8__locals1))
						{
							empty.Expand((short)math.clamp(this.ViewToTileSpace(float45).x, 0f, (float)(this.tileCount.x - 1)));
						}
					}
					int num14 = this.m_Offset + 1 + i;
					this.tileRanges[num14] = InclusiveRange.Merge(this.tileRanges[num14], empty);
					this.tileRanges[num14 - 1] = InclusiveRange.Merge(this.tileRanges[num14 - 1], empty);
				}
			}
			this.m_TileYRange.Clamp(0, (short)(this.tileCount.y - 1));
			for (int j = (int)(this.m_TileYRange.start + 1); j <= (int)this.m_TileYRange.end; j++)
			{
				InclusiveRange empty2 = InclusiveRange.empty;
				float num15 = math.lerp(this.viewPlaneBottoms[this.m_ViewIndex], this.viewPlaneTops[this.m_ViewIndex], (float)j * this.tileScaleInv.y);
				float3 float46;
				float3 float47;
				TilingJob.GetSphereYPlaneHorizon(CS$<>8__locals1.lightPositionVS, range, this.near, num3, num15, out float46, out float47);
				if (TilingJob.<TileLight>g__SpherePointIsValid|19_0(float46, ref CS$<>8__locals1))
				{
					empty2.Expand((short)math.clamp(this.ViewToTileSpace(float46).x, 0f, (float)(this.tileCount.x - 1)));
				}
				if (TilingJob.<TileLight>g__SpherePointIsValid|19_0(float47, ref CS$<>8__locals1))
				{
					empty2.Expand((short)math.clamp(this.ViewToTileSpace(float47).x, 0f, (float)(this.tileCount.x - 1)));
				}
				int num16 = this.m_Offset + 1 + j;
				this.tileRanges[num16] = InclusiveRange.Merge(this.tileRanges[num16], empty2);
				this.tileRanges[num16 - 1] = InclusiveRange.Merge(this.tileRanges[num16 - 1], empty2);
			}
			this.tileRanges[this.m_Offset] = this.m_TileYRange;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0002A690 File Offset: 0x00028890
		private void TileLightOrthographic(int lightIndex)
		{
			TilingJob.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1.light = this.lights[lightIndex];
			float4x4 float4x = CS$<>8__locals1.light.localToWorldMatrix;
			CS$<>8__locals1.lightPosVS = math.mul(this.worldToViews[this.m_ViewIndex], math.float4(float4x.c3.xyz, 1f)).xyz;
			CS$<>8__locals1.lightPosVS.z = CS$<>8__locals1.lightPosVS.z * -1f;
			this.ExpandOrthographic(CS$<>8__locals1.lightPosVS);
			CS$<>8__locals1.lightDirVS = math.mul(this.worldToViews[this.m_ViewIndex], math.float4(float4x.c2.xyz, 0f)).xyz;
			CS$<>8__locals1.lightDirVS.z = CS$<>8__locals1.lightDirVS.z * -1f;
			CS$<>8__locals1.lightDirVS = math.normalize(CS$<>8__locals1.lightDirVS);
			float num = math.radians(CS$<>8__locals1.light.spotAngle * 0.5f);
			float range = CS$<>8__locals1.light.range;
			float num2 = TilingJob.square(range);
			CS$<>8__locals1.cosHalfAngle = math.cos(num);
			float num3 = CS$<>8__locals1.cosHalfAngle * range;
			float num4 = TilingJob.square(num3);
			float num5 = 1f / num3;
			float num6 = TilingJob.square(num5);
			float3 @float = CS$<>8__locals1.lightPosVS - math.float3(0f, range, 0f);
			float3 float2 = CS$<>8__locals1.lightPosVS + math.float3(0f, range, 0f);
			float3 float3 = CS$<>8__locals1.lightPosVS - math.float3(range, 0f, 0f);
			float3 float4 = CS$<>8__locals1.lightPosVS + math.float3(range, 0f, 0f);
			if (TilingJob.<TileLightOrthographic>g__SpherePointIsValid|20_0(@float, ref CS$<>8__locals1))
			{
				this.ExpandOrthographic(@float);
			}
			if (TilingJob.<TileLightOrthographic>g__SpherePointIsValid|20_0(float2, ref CS$<>8__locals1))
			{
				this.ExpandOrthographic(float2);
			}
			if (TilingJob.<TileLightOrthographic>g__SpherePointIsValid|20_0(float3, ref CS$<>8__locals1))
			{
				this.ExpandOrthographic(float3);
			}
			if (TilingJob.<TileLightOrthographic>g__SpherePointIsValid|20_0(float4, ref CS$<>8__locals1))
			{
				this.ExpandOrthographic(float4);
			}
			float3 float5 = CS$<>8__locals1.lightPosVS + CS$<>8__locals1.lightDirVS * num3;
			float num7 = math.sqrt(num2 - num4);
			float num8 = TilingJob.square(num7);
			float3 float6 = math.normalize(math.float3(0f, 1f, 0f) - CS$<>8__locals1.lightDirVS * CS$<>8__locals1.lightDirVS.y);
			float3 float7 = math.normalize(math.float3(1f, 0f, 0f) - CS$<>8__locals1.lightDirVS * CS$<>8__locals1.lightDirVS.x);
			float3 float8 = float5 - float6 * num7;
			float3 float9 = float5 + float6 * num7;
			if (CS$<>8__locals1.light.lightType == LightType.Spot)
			{
				float3 float10 = float5 - float7 * num7;
				float3 float11 = float5 + float7 * num7;
				this.ExpandOrthographic(float8);
				this.ExpandOrthographic(float9);
				this.ExpandOrthographic(float10);
				this.ExpandOrthographic(float11);
			}
			this.m_TileYRange.Clamp(0, (short)(this.tileCount.y - 1));
			float num9 = 0f;
			float num10 = 0f;
			float num11 = 0f;
			float num12 = 0f;
			if (CS$<>8__locals1.light.lightType == LightType.Spot)
			{
				float num13 = num3 + num8 * num5;
				float num14 = math.sqrt(TilingJob.square(num8) * num6 + num8);
				float num15 = math.rcp(math.lengthsq(CS$<>8__locals1.lightDirVS.xy));
				float2 float12 = -num8 * num5 * num15 * CS$<>8__locals1.lightDirVS.xy;
				float2 float13 = math.sqrt((TilingJob.square(num14) - math.lengthsq(float12)) * num15) * math.float2(CS$<>8__locals1.lightDirVS.y, -CS$<>8__locals1.lightDirVS.x);
				float2 float14 = CS$<>8__locals1.lightPosVS.xy + num13 * CS$<>8__locals1.lightDirVS.xy + float12;
				float2 float15 = float14 - float13;
				float2 float16 = float14 + float13;
				num9 = float15.x - CS$<>8__locals1.lightPosVS.x;
				num10 = math.rcp(float15.y - CS$<>8__locals1.lightPosVS.y);
				num11 = float16.x - CS$<>8__locals1.lightPosVS.x;
				num12 = math.rcp(float16.y - CS$<>8__locals1.lightPosVS.y);
			}
			for (int i = (int)(this.m_TileYRange.start + 1); i <= (int)this.m_TileYRange.end; i++)
			{
				InclusiveRange empty = InclusiveRange.empty;
				float num16 = math.lerp(this.viewPlaneBottoms[this.m_ViewIndex], this.viewPlaneTops[this.m_ViewIndex], (float)i * this.tileScaleInv.y);
				float num17 = math.sqrt(num2 - TilingJob.square(num16 - CS$<>8__locals1.lightPosVS.y));
				float3 float17 = math.float3(CS$<>8__locals1.lightPosVS.x - num17, num16, CS$<>8__locals1.lightPosVS.z);
				float3 float18 = math.float3(CS$<>8__locals1.lightPosVS.x + num17, num16, CS$<>8__locals1.lightPosVS.z);
				if (TilingJob.<TileLightOrthographic>g__SpherePointIsValid|20_0(float17, ref CS$<>8__locals1))
				{
					this.ExpandRangeOrthographic(ref empty, float17.x);
				}
				if (TilingJob.<TileLightOrthographic>g__SpherePointIsValid|20_0(float18, ref CS$<>8__locals1))
				{
					this.ExpandRangeOrthographic(ref empty, float18.x);
				}
				if (CS$<>8__locals1.light.lightType == LightType.Spot)
				{
					if (num16 >= float8.y && num16 <= float9.y)
					{
						float num18 = (num16 - float5.y) / float6.y;
						float num19 = float5.x + num18 * float6.x;
						float num20 = -CS$<>8__locals1.lightDirVS.z / math.length(math.float3(-CS$<>8__locals1.lightDirVS.z, 0f, CS$<>8__locals1.lightDirVS.x));
						float num21 = math.sqrt(TilingJob.square(num7) - TilingJob.square(num18));
						float num22 = num19 - num21 * num20;
						float num23 = num19 + num21 * num20;
						this.ExpandRangeOrthographic(ref empty, num22);
						this.ExpandRangeOrthographic(ref empty, num23);
					}
					float num24 = num16 - CS$<>8__locals1.lightPosVS.y;
					float num25 = num24 * num10;
					float num26 = num24 * num12;
					if (num25 >= 0f && num25 <= 1f)
					{
						this.ExpandRangeOrthographic(ref empty, CS$<>8__locals1.lightPosVS.x + num25 * num9);
					}
					if (num26 >= 0f && num26 <= 1f)
					{
						this.ExpandRangeOrthographic(ref empty, CS$<>8__locals1.lightPosVS.x + num26 * num11);
					}
				}
				int num27 = this.m_Offset + 1 + i;
				this.tileRanges[num27] = InclusiveRange.Merge(this.tileRanges[num27], empty);
				this.tileRanges[num27 - 1] = InclusiveRange.Merge(this.tileRanges[num27 - 1], empty);
			}
			this.tileRanges[this.m_Offset] = this.m_TileYRange;
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0002ADB8 File Offset: 0x00028FB8
		private void TileReflectionProbe(int index)
		{
			VisibleReflectionProbe visibleReflectionProbe = this.reflectionProbes[index - this.lights.Length];
			float3 @float = visibleReflectionProbe.bounds.center;
			float3 float2 = visibleReflectionProbe.bounds.extents;
			NativeArray<float3> nativeArray = new NativeArray<float3>(TilingJob.k_CubePoints.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<float2> nativeArray2 = new NativeArray<float2>(TilingJob.k_CubePoints.Length + TilingJob.k_CubeLineIndices.Length * 3, Allocator.Temp, NativeArrayOptions.ClearMemory);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < TilingJob.k_CubePoints.Length; i++)
			{
				float3 xyz = math.mul(this.worldToViews[this.m_ViewIndex], math.float4(@float + float2 * TilingJob.k_CubePoints[i], 1f)).xyz;
				xyz.z *= -1f;
				nativeArray[i] = xyz;
				if (xyz.z >= this.near)
				{
					float2 float3 = (this.isOrthographic ? xyz.xy : (xyz.xy / xyz.z));
					int num3 = num++;
					nativeArray2[num3] = float3;
					if (float3.x < nativeArray2[num2].x)
					{
						num2 = num3;
					}
				}
			}
			for (int j = 0; j < TilingJob.k_CubeLineIndices.Length; j++)
			{
				int4 @int = TilingJob.k_CubeLineIndices[j];
				float3 float4 = nativeArray[@int.x];
				for (int k = 0; k < 3; k++)
				{
					float3 float5 = nativeArray[@int[k + 1]];
					if ((float4.z >= this.near || float5.z >= this.near) && (float4.z < this.near || float5.z < this.near))
					{
						float num4 = (this.near - float4.z) / (float5.z - float4.z);
						float3 float6 = math.lerp(float4, float5, num4);
						float2 float7 = (this.isOrthographic ? float6.xy : (float6.xy / float6.z));
						int num5 = num++;
						nativeArray2[num5] = float7;
						if (float7.x < nativeArray2[num2].x)
						{
							num2 = num5;
						}
					}
				}
			}
			NativeArray<float2> nativeArray3 = new NativeArray<float2>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			int num6 = 0;
			if (num > 0)
			{
				int num7 = num2;
				do
				{
					float2 float8 = nativeArray2[num7];
					this.ExpandY(math.float3(float8, 1f));
					nativeArray3[num6++] = float8;
					int num8 = 0;
					float2 float9 = nativeArray2[num8] - float8;
					for (int l = 0; l < num; l++)
					{
						float2 float10 = nativeArray2[l] - float8;
						float num9 = math.determinant(math.float2x2(float9, float10));
						if (num8 == num7 || num9 > 0f || (num9 == 0f && math.lengthsq(float10) > math.lengthsq(float9)))
						{
							num8 = l;
							float9 = float10;
						}
					}
					num7 = num8;
				}
				while (num7 != num2 && num6 < num);
				this.m_TileYRange.Clamp(0, (short)(this.tileCount.y - 1));
				for (int m = (int)(this.m_TileYRange.start + 1); m <= (int)this.m_TileYRange.end; m++)
				{
					InclusiveRange empty = InclusiveRange.empty;
					float num10 = math.lerp(this.viewPlaneBottoms[this.m_ViewIndex], this.viewPlaneTops[this.m_ViewIndex], (float)m * this.tileScaleInv.y);
					for (int n = 0; n < num6; n++)
					{
						float2 float11 = nativeArray3[n];
						float2 float12 = nativeArray3[(n + 1) % num6];
						float num11 = (num10 - float11.y) / (float12.y - float11.y);
						if (num11 >= 0f && num11 <= 1f)
						{
							float3 float13 = math.float3(math.lerp(float11.x, float12.x, num11), num10, 1f);
							float2 float14 = (this.isOrthographic ? this.ViewToTileSpaceOrthographic(float13) : this.ViewToTileSpace(float13));
							empty.Expand((short)math.clamp(float14.x, 0f, (float)(this.tileCount.x - 1)));
						}
					}
					int num12 = this.m_Offset + 1 + m;
					this.tileRanges[num12] = InclusiveRange.Merge(this.tileRanges[num12], empty);
					this.tileRanges[num12 - 1] = InclusiveRange.Merge(this.tileRanges[num12 - 1], empty);
				}
				this.tileRanges[this.m_Offset] = this.m_TileYRange;
			}
			nativeArray3.Dispose();
			nativeArray2.Dispose();
			nativeArray.Dispose();
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0002B2DC File Offset: 0x000294DC
		private float2 ViewToTileSpace(float3 positionVS)
		{
			return (positionVS.xy / positionVS.z * this.viewToViewportScaleBiases[this.m_ViewIndex].xy + this.viewToViewportScaleBiases[this.m_ViewIndex].zw) * this.tileScale;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0002B344 File Offset: 0x00029544
		private float2 ViewToTileSpaceOrthographic(float3 positionVS)
		{
			return (positionVS.xy * this.viewToViewportScaleBiases[this.m_ViewIndex].xy + this.viewToViewportScaleBiases[this.m_ViewIndex].zw) * this.tileScale;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002B3A0 File Offset: 0x000295A0
		private void ExpandY(float3 positionVS)
		{
			float2 @float = this.ViewToTileSpace(positionVS);
			int num = (int)@float.y;
			int num2 = (int)@float.x;
			this.m_TileYRange.Expand((short)math.clamp(num, 0, this.tileCount.y - 1));
			if (num >= 0 && num < this.tileCount.y && num2 >= 0 && num2 < this.tileCount.x)
			{
				InclusiveRange inclusiveRange = this.tileRanges[this.m_Offset + 1 + num];
				inclusiveRange.Expand((short)num2);
				this.tileRanges[this.m_Offset + 1 + num] = inclusiveRange;
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0002B43C File Offset: 0x0002963C
		private void ExpandOrthographic(float3 positionVS)
		{
			float2 @float = this.ViewToTileSpaceOrthographic(positionVS);
			int num = (int)@float.y;
			int num2 = (int)@float.x;
			this.m_TileYRange.Expand((short)math.clamp(num, 0, this.tileCount.y - 1));
			if (num >= 0 && num < this.tileCount.y && num2 >= 0 && num2 < this.tileCount.x)
			{
				InclusiveRange inclusiveRange = this.tileRanges[this.m_Offset + 1 + num];
				inclusiveRange.Expand((short)num2);
				this.tileRanges[this.m_Offset + 1 + num] = inclusiveRange;
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002B4D8 File Offset: 0x000296D8
		private void ExpandRangeOrthographic(ref InclusiveRange range, float xVS)
		{
			range.Expand((short)math.clamp(this.ViewToTileSpaceOrthographic(xVS).x, 0f, (float)(this.tileCount.x - 1)));
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002B50A File Offset: 0x0002970A
		private static float square(float x)
		{
			return x * x;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002B510 File Offset: 0x00029710
		private static void GetSphereHorizon(float2 center, float radius, float near, float clipRadius, out float2 p0, out float2 p1)
		{
			float2 @float = math.normalize(center);
			float num = math.length(center);
			float num2 = math.sqrt(num * num - radius * radius);
			float num3 = num2 * radius / num;
			float2 float2 = @float * (num2 * num3 / radius);
			p0 = math.float2(float.MinValue, 1f);
			p1 = math.float2(float.MaxValue, 1f);
			if (center.y - radius < near)
			{
				p0 = math.float2(center.x + clipRadius, near);
				p1 = math.float2(center.x - clipRadius, near);
			}
			float2 float3 = float2 + math.float2(-@float.y, @float.x) * num3;
			if (TilingJob.square(num) >= TilingJob.square(radius) && float3.y >= near)
			{
				if (float3.x > p0.x)
				{
					p0 = float3;
				}
				if (float3.x < p1.x)
				{
					p1 = float3;
				}
			}
			float2 float4 = float2 + math.float2(@float.y, -@float.x) * num3;
			if (TilingJob.square(num) >= TilingJob.square(radius) && float4.y >= near)
			{
				if (float4.x > p0.x)
				{
					p0 = float4;
				}
				if (float4.x < p1.x)
				{
					p1 = float4;
				}
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002B680 File Offset: 0x00029880
		private static void GetSphereYPlaneHorizon(float3 center, float sphereRadius, float near, float clipRadius, float y, out float3 left, out float3 right)
		{
			float num = y * near;
			float num2 = math.sqrt(TilingJob.square(clipRadius) - TilingJob.square(num - center.y));
			left = math.float3(center.x - num2, num, near);
			right = math.float3(center.x + num2, num, near);
			float3 @float = math.normalize(math.float3(0f, y, 1f));
			float3 float2 = math.float3(1f, 0f, 0f);
			float num3 = math.abs(math.dot(math.normalize(math.float3(0f, 1f, -y)), center));
			float2 float3 = math.float2(math.dot(center, @float), math.dot(center, float2));
			float num4 = math.length(float3);
			float2 float4 = float3 / num4;
			float num5 = math.sqrt(TilingJob.square(sphereRadius) - TilingJob.square(num3));
			if (TilingJob.square(num3) <= TilingJob.square(sphereRadius) && TilingJob.square(num5) <= TilingJob.square(num4))
			{
				float num6 = math.sqrt(TilingJob.square(num4) - TilingJob.square(num5));
				float num7 = num6 * num5 / num4;
				float2 float5 = float4 * (num6 * num7 / num5);
				float2 float6 = float5 + math.float2(float4.y, -float4.x) * num7;
				float2 float7 = float5 + math.float2(-float4.y, float4.x) * num7;
				float3 float8 = float6.x * @float + float6.y * float2;
				if (float8.z >= near)
				{
					left = float8;
				}
				float3 float9 = float7.x * @float + float7.y * float2;
				if (float9.z >= near)
				{
					right = float9;
				}
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002B864 File Offset: 0x00029A64
		private static bool GetCircleClipPoints(float3 circleCenter, float3 circleNormal, float circleRadius, float near, out float3 p0, out float3 p1)
		{
			float3 @float = math.normalize(math.cross(circleNormal, math.float3(0f, 0f, 1f)));
			float3 float2 = math.cross(@float, circleNormal);
			float num = (near - circleCenter.z) / float2.z;
			float3 float3 = circleCenter + float2 * num;
			float num2 = math.sqrt(TilingJob.square(circleRadius) - TilingJob.square(num));
			p0 = float3 + @float * num2;
			p1 = float3 - @float * num2;
			return math.abs(num) <= circleRadius;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002B904 File Offset: 0x00029B04
		private static ValueTuple<float, float> IntersectEllipseLine(float a, float b, float3 line)
		{
			float num = math.rcp(TilingJob.square(line.y) * TilingJob.square(b));
			float num2 = 1f / TilingJob.square(a) + TilingJob.square(line.x) * num;
			float num3 = 2f * line.x * line.z * num;
			float num4 = TilingJob.square(line.z) * num - 1f;
			float num5 = math.sqrt(num3 * num3 - 4f * num2 * num4);
			float num6 = (-num3 + num5) / (2f * num2);
			float num7 = (-num3 - num5) / (2f * num2);
			return new ValueTuple<float, float>(num6, num7);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002B9A4 File Offset: 0x00029BA4
		private static void GetProjectedCircleHorizon(float2 center, float radius, float2 U, float2 V, out float2 uv1, out float2 uv2)
		{
			float num = math.length(V);
			if (num < 1E-06f)
			{
				uv1 = math.float2(radius, 0f);
				uv2 = math.float2(-radius, 0f);
				return;
			}
			float num2 = math.length(U);
			float num3 = math.rcp(num2);
			float num4 = math.rcp(num);
			float2 @float = U * num3;
			float2 float2 = V * num4;
			float num5 = num2 * radius;
			float num6 = num * radius;
			float2 float3 = math.float2(math.dot(-center, @float), math.dot(-center, float2));
			float3 float4 = math.float3(float3.x / TilingJob.square(num5), float3.y / TilingJob.square(num6), -1f);
			ValueTuple<float, float> valueTuple = TilingJob.IntersectEllipseLine(num5, num6, float4);
			float item = valueTuple.Item1;
			float item2 = valueTuple.Item2;
			uv1 = math.float2(item * num3, (-float4.x / float4.y * item - float4.z / float4.y) * num4);
			uv2 = math.float2(item2 * num3, (-float4.x / float4.y * item2 - float4.z / float4.y) * num4);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002BAE4 File Offset: 0x00029CE4
		private static bool IntersectCircleYPlane(float y, float3 circleCenter, float3 circleNormal, float3 circleU, float3 circleV, float circleRadius, out float3 p1, out float3 p2)
		{
			p1 = (p2 = 0);
			float num = math.dot(circleCenter, circleNormal);
			float3 @float = math.float3(1f, y, 1f) * num / math.dot(math.float3(1f, y, 1f), circleNormal) - circleCenter;
			float2 float2 = math.float2(math.dot(@float, circleU), math.dot(@float, circleV));
			float3 float3 = math.float3(-1f, y, 1f) * num / math.dot(math.float3(-1f, y, 1f), circleNormal) - circleCenter;
			float2 float4 = math.normalize(math.float2(math.dot(float3, circleU), math.dot(float3, circleV)) - float2);
			float2 float5 = math.float2(float4.y, -float4.x);
			float num2 = math.dot(float2, float5);
			float2 float6 = float5 * num2;
			if (num2 > circleRadius)
			{
				return false;
			}
			float num3 = math.sqrt(circleRadius * circleRadius - num2 * num2);
			float2 float7 = float6 + num3 * float4;
			float2 float8 = float6 - num3 * float4;
			p1 = circleCenter + float7.x * circleU + float7.y * circleV;
			p2 = circleCenter + float8.x * circleU + float8.y * circleV;
			return true;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002BC7C File Offset: 0x00029E7C
		private static void GetConeSideTangentPoints(float3 vertex, float3 axis, float cosHalfAngle, float circleRadius, float coneHeight, float range, float3 circleU, float3 circleV, out float3 l1, out float3 l2)
		{
			l1 = (l2 = 0);
			if (math.dot(math.normalize(-vertex), axis) >= cosHalfAngle)
			{
				return;
			}
			float num = -math.dot(vertex, axis);
			if (num == 0f)
			{
				num = 1E-06f;
			}
			float num2 = ((num < 0f) ? (-1f) : 1f);
			float3 @float = vertex + axis * num;
			float num3 = math.abs(num) * circleRadius / coneHeight;
			float3 float2 = math.float3(math.float2(math.dot(circleU, -@float), math.dot(circleV, -@float)), -TilingJob.square(num3));
			float2 float3 = math.float2(-1f, -float2.x / float2.y * -1f - float2.z / float2.y);
			float2 float4 = math.normalize(math.float2(1f, -float2.x / float2.y * 1f - float2.z / float2.y) - float3);
			float2 float5 = math.float2(float4.y, -float4.x);
			float num4 = math.dot(float3, float5);
			float2 float6 = float5 * num4;
			float num5 = math.sqrt(num3 * num3 - num4 * num4);
			float2 float7 = float6 + num5 * float4;
			float2 float8 = float6 - num5 * float4;
			float3 float9 = math.normalize(@float + float7.x * circleU + float7.y * circleV - vertex) * num2;
			float3 float10 = math.normalize(@float + float8.x * circleU + float8.y * circleV - vertex) * num2;
			l1 = float9 * range;
			l2 = float10 * range;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002BE8C File Offset: 0x0002A08C
		private static float3 EvaluateNearConic(float near, float3 o, float3 d, float r, float3 u, float3 v, float theta)
		{
			float num = (near - o.z) / (d.z + r * u.z * math.cos(theta) + r * v.z * math.sin(theta));
			return math.float3(o.xy + num * (d.xy + r * u.xy * math.cos(theta) + r * v.xy * math.sin(theta)), near);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0002BF28 File Offset: 0x0002A128
		private static float2 FindNearConicTangentTheta(float2 o, float2 d, float r, float2 u, float2 v)
		{
			float num = math.sqrt(TilingJob.square(d.x) * TilingJob.square(u.y) + TilingJob.square(d.x) * TilingJob.square(v.y) - 2f * d.x * d.y * u.x * u.y - 2f * d.x * d.y * v.x * v.y + TilingJob.square(d.y) * TilingJob.square(u.x) + TilingJob.square(d.y) * TilingJob.square(v.x) - TilingJob.square(r) * TilingJob.square(u.x) * TilingJob.square(v.y) + 2f * TilingJob.square(r) * u.x * u.y * v.x * v.y - TilingJob.square(r) * TilingJob.square(u.y) * TilingJob.square(v.x));
			float num2 = d.x * v.y - d.y * v.x - r * u.x * v.y + r * u.y * v.x;
			return 2f * math.atan((-d.x * u.y + d.y * u.x + math.float2(1f, -1f) * num) / num2);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002C0D8 File Offset: 0x0002A2D8
		private static float2 FindNearConicYTheta(float near, float3 o, float3 d, float r, float3 u, float3 v, float y)
		{
			float num = math.sqrt(-TilingJob.square(d.y) * TilingJob.square(o.z) + 2f * TilingJob.square(d.y) * o.z * near - TilingJob.square(d.y) * TilingJob.square(near) + 2f * d.y * d.z * o.y * o.z - 2f * d.y * d.z * o.y * near - 2f * d.y * d.z * o.z * y + 2f * d.y * d.z * y * near - TilingJob.square(d.z) * TilingJob.square(o.y) + 2f * TilingJob.square(d.z) * o.y * y - TilingJob.square(d.z) * TilingJob.square(y) + TilingJob.square(o.y) * TilingJob.square(r) * TilingJob.square(u.z) + TilingJob.square(o.y) * TilingJob.square(r) * TilingJob.square(v.z) - 2f * o.y * o.z * TilingJob.square(r) * u.y * u.z - 2f * o.y * o.z * TilingJob.square(r) * v.y * v.z - 2f * o.y * y * TilingJob.square(r) * TilingJob.square(u.z) - 2f * o.y * y * TilingJob.square(r) * TilingJob.square(v.z) + 2f * o.y * TilingJob.square(r) * u.y * u.z * near + 2f * o.y * TilingJob.square(r) * v.y * v.z * near + TilingJob.square(o.z) * TilingJob.square(r) * TilingJob.square(u.y) + TilingJob.square(o.z) * TilingJob.square(r) * TilingJob.square(v.y) + 2f * o.z * y * TilingJob.square(r) * u.y * u.z + 2f * o.z * y * TilingJob.square(r) * v.y * v.z - 2f * o.z * TilingJob.square(r) * TilingJob.square(u.y) * near - 2f * o.z * TilingJob.square(r) * TilingJob.square(v.y) * near + TilingJob.square(y) * TilingJob.square(r) * TilingJob.square(u.z) + TilingJob.square(y) * TilingJob.square(r) * TilingJob.square(v.z) - 2f * y * TilingJob.square(r) * u.y * u.z * near - 2f * y * TilingJob.square(r) * v.y * v.z * near + TilingJob.square(r) * TilingJob.square(u.y) * TilingJob.square(near) + TilingJob.square(r) * TilingJob.square(v.y) * TilingJob.square(near));
			float num2 = d.y * o.z - d.y * near - d.z * o.y + d.z * y + o.y * r * u.z - o.z * r * u.y - y * r * u.z + r * u.y * near;
			return 2f * math.atan((r * (o.y * v.z - o.z * v.y - y * v.z + v.y * near) + math.float2(1f, -1f) * num) / num2);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002C6A7 File Offset: 0x0002A8A7
		[CompilerGenerated]
		internal static bool <TileLight>g__SpherePointIsValid|19_0(float3 p, ref TilingJob.<>c__DisplayClass19_0 A_1)
		{
			return A_1.light.lightType == LightType.Point || math.dot(math.normalize(p - A_1.lightPositionVS), A_1.lightDirectionVS) >= A_1.cosHalfAngle;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0002C6E0 File Offset: 0x0002A8E0
		[CompilerGenerated]
		internal static bool <TileLight>g__ConicPointIsValid|19_1(float3 p, ref TilingJob.<>c__DisplayClass19_0 A_1)
		{
			return math.dot(math.normalize(p - A_1.lightPositionVS), A_1.lightDirectionVS) >= 0f && math.dot(p - A_1.lightPositionVS, A_1.lightDirectionVS) <= A_1.coneHeight;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002C734 File Offset: 0x0002A934
		[CompilerGenerated]
		internal static bool <TileLightOrthographic>g__SpherePointIsValid|20_0(float3 p, ref TilingJob.<>c__DisplayClass20_0 A_1)
		{
			return A_1.light.lightType == LightType.Point || math.dot(math.normalize(p - A_1.lightPosVS), A_1.lightDirVS) >= A_1.cosHalfAngle;
		}

		// Token: 0x04000581 RID: 1409
		[ReadOnly]
		public NativeArray<VisibleLight> lights;

		// Token: 0x04000582 RID: 1410
		[ReadOnly]
		public NativeArray<VisibleReflectionProbe> reflectionProbes;

		// Token: 0x04000583 RID: 1411
		[NativeDisableParallelForRestriction]
		public NativeArray<InclusiveRange> tileRanges;

		// Token: 0x04000584 RID: 1412
		public int itemsPerTile;

		// Token: 0x04000585 RID: 1413
		public int rangesPerItem;

		// Token: 0x04000586 RID: 1414
		public Fixed2<float4x4> worldToViews;

		// Token: 0x04000587 RID: 1415
		public float2 tileScale;

		// Token: 0x04000588 RID: 1416
		public float2 tileScaleInv;

		// Token: 0x04000589 RID: 1417
		public Fixed2<float> viewPlaneBottoms;

		// Token: 0x0400058A RID: 1418
		public Fixed2<float> viewPlaneTops;

		// Token: 0x0400058B RID: 1419
		public Fixed2<float4> viewToViewportScaleBiases;

		// Token: 0x0400058C RID: 1420
		public int2 tileCount;

		// Token: 0x0400058D RID: 1421
		public float near;

		// Token: 0x0400058E RID: 1422
		public bool isOrthographic;

		// Token: 0x0400058F RID: 1423
		private InclusiveRange m_TileYRange;

		// Token: 0x04000590 RID: 1424
		private int m_Offset;

		// Token: 0x04000591 RID: 1425
		private int m_ViewIndex;

		// Token: 0x04000592 RID: 1426
		private float2 m_CenterOffset;

		// Token: 0x04000593 RID: 1427
		private static readonly float3[] k_CubePoints = new float3[]
		{
			new float3(-1f, -1f, -1f),
			new float3(-1f, -1f, 1f),
			new float3(-1f, 1f, -1f),
			new float3(-1f, 1f, 1f),
			new float3(1f, -1f, -1f),
			new float3(1f, -1f, 1f),
			new float3(1f, 1f, -1f),
			new float3(1f, 1f, 1f)
		};

		// Token: 0x04000594 RID: 1428
		private static readonly int4[] k_CubeLineIndices = new int4[]
		{
			new int4(0, 4, 2, 1),
			new int4(3, 7, 1, 2),
			new int4(5, 1, 7, 4),
			new int4(6, 2, 4, 7)
		};
	}
}
