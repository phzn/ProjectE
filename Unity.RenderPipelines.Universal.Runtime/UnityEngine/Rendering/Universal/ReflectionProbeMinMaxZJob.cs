using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000DD RID: 221
	[BurstCompile]
	internal struct ReflectionProbeMinMaxZJob : IJobFor
	{
		// Token: 0x060006EC RID: 1772 RVA: 0x000295E8 File Offset: 0x000277E8
		public void Execute(int index)
		{
			float2 @float = math.float2(float.MaxValue, float.MinValue);
			int num = index % this.reflectionProbes.Length;
			VisibleReflectionProbe visibleReflectionProbe = this.reflectionProbes[num];
			int num2 = index / this.reflectionProbes.Length;
			float4x4 float4x = this.worldToViews[num2];
			float3 float2 = visibleReflectionProbe.bounds.center;
			float3 float3 = visibleReflectionProbe.bounds.extents;
			for (int i = 0; i < 8; i++)
			{
				int num3 = ((i << 1) & 2) - 1;
				int num4 = (i & 2) - 1;
				int num5 = ((i >> 1) & 2) - 1;
				float4 float4 = math.mul(float4x, math.float4(float2 + float3 * math.float3((float)num3, (float)num4, (float)num5), 1f));
				float4.z *= -1f;
				@float.x = math.min(@float.x, float4.z);
				@float.y = math.max(@float.y, float4.z);
			}
			this.minMaxZs[index] = @float;
		}

		// Token: 0x04000573 RID: 1395
		public Fixed2<float4x4> worldToViews;

		// Token: 0x04000574 RID: 1396
		[ReadOnly]
		public NativeArray<VisibleReflectionProbe> reflectionProbes;

		// Token: 0x04000575 RID: 1397
		public NativeArray<float2> minMaxZs;
	}
}
