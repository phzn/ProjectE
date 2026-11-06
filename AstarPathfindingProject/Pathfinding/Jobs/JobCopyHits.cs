using System;
using Pathfinding.Graphs.Grid;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x02000180 RID: 384
	[BurstCompile(FloatMode = FloatMode.Fast)]
	public struct JobCopyHits : IJob, GridIterationUtilities.ISliceAction
	{
		// Token: 0x06000BC4 RID: 3012 RVA: 0x00041360 File Offset: 0x0003F560
		public void Execute()
		{
			this.slice.AssertMatchesOuter<Vector3>(this.points);
			this.slice.AssertMatchesOuter<float4>(this.normals);
			GridIterationUtilities.ForEachCellIn3DSlice<JobCopyHits>(this.slice, ref this);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00041390 File Offset: 0x0003F590
		public void Execute(uint outerIdx, uint innerIdx)
		{
			Aliasing.ExpectNotAliased<NativeArray<Vector3>, NativeArray<float4>>(in this.points, in this.normals);
			Vector3 normal = this.hits[(int)innerIdx].normal;
			float4 @float = new float4(normal.x, normal.y, normal.z, 0f);
			this.normals[(int)outerIdx] = math.normalizesafe(@float, default(float4));
			if (math.lengthsq(@float) > 1.1754944E-38f)
			{
				this.points[(int)outerIdx] = this.hits[(int)innerIdx].point;
			}
		}

		// Token: 0x040007B6 RID: 1974
		[ReadOnly]
		public NativeArray<RaycastHit> hits;

		// Token: 0x040007B7 RID: 1975
		[WriteOnly]
		public NativeArray<Vector3> points;

		// Token: 0x040007B8 RID: 1976
		[WriteOnly]
		public NativeArray<float4> normals;

		// Token: 0x040007B9 RID: 1977
		public Slice3D slice;
	}
}
