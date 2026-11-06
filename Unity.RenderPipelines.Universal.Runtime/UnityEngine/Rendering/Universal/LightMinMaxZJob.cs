using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000DC RID: 220
	[BurstCompile]
	internal struct LightMinMaxZJob : IJobFor
	{
		// Token: 0x060006EB RID: 1771 RVA: 0x000293C0 File Offset: 0x000275C0
		public void Execute(int index)
		{
			int num = index % this.lights.Length;
			VisibleLight visibleLight = this.lights[num];
			float4x4 float4x = visibleLight.localToWorldMatrix;
			float3 xyz = float4x.c3.xyz;
			int num2 = index / this.lights.Length;
			float4x4 float4x2 = this.worldToViews[num2];
			float3 xyz2 = math.mul(float4x2, math.float4(xyz, 1f)).xyz;
			xyz2.z *= -1f;
			float2 @float = math.float2(xyz2.z - visibleLight.range, xyz2.z + visibleLight.range);
			if (visibleLight.lightType == LightType.Spot)
			{
				float num3 = math.radians(visibleLight.spotAngle) * 0.5f;
				float num4 = math.cos(num3);
				float num5 = visibleLight.range * num4;
				float3 xyz3 = float4x.c2.xyz;
				float3 float2 = xyz + xyz3 * num5;
				float3 xyz4 = math.mul(float4x2, math.float4(float2, 1f)).xyz;
				xyz4.z *= -1f;
				float num6 = 1.5707964f - num3;
				float num7 = visibleLight.range * num4 * math.sin(num3) / math.sin(num6);
				float3 float3 = xyz4 - xyz2;
				float num8 = math.sqrt(1f - float3.z * float3.z / math.dot(float3, float3));
				if (-float3.z < num5 * num4)
				{
					@float.x = math.min(xyz2.z, xyz4.z - num8 * num7);
				}
				if (float3.z < num5 * num4)
				{
					@float.y = math.max(xyz2.z, xyz4.z + num8 * num7);
				}
			}
			@float.x = math.max(@float.x, 0f);
			@float.y = math.max(@float.y, 0f);
			this.minMaxZs[index] = @float;
		}

		// Token: 0x04000570 RID: 1392
		public Fixed2<float4x4> worldToViews;

		// Token: 0x04000571 RID: 1393
		[ReadOnly]
		public NativeArray<VisibleLight> lights;

		// Token: 0x04000572 RID: 1394
		public NativeArray<float2> minMaxZs;
	}
}
