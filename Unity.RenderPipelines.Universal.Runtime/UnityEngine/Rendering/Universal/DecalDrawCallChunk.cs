using System;
using Unity.Collections;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200006B RID: 107
	internal class DecalDrawCallChunk : DecalChunk
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00016A07 File Offset: 0x00014C07
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x000169F8 File Offset: 0x00014BF8
		public int subCallCount
		{
			get
			{
				return this.subCallCounts[0];
			}
			set
			{
				this.subCallCounts[0] = value;
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00016A18 File Offset: 0x00014C18
		public override void RemoveAtSwapBack(int entityIndex)
		{
			base.RemoveAtSwapBack<float4x4>(ref this.decalToWorlds, entityIndex, base.count);
			base.RemoveAtSwapBack<float4x4>(ref this.normalToDecals, entityIndex, base.count);
			base.RemoveAtSwapBack<float>(ref this.renderingLayerMasks, entityIndex, base.count);
			base.RemoveAtSwapBack<DecalSubDrawCall>(ref this.subCalls, entityIndex, base.count);
			int count = base.count;
			base.count = count - 1;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00016A81 File Offset: 0x00014C81
		public override void SetCapacity(int newCapacity)
		{
			(ref this.decalToWorlds).ResizeArray<float4x4>(newCapacity);
			(ref this.normalToDecals).ResizeArray<float4x4>(newCapacity);
			(ref this.renderingLayerMasks).ResizeArray<float>(newCapacity);
			(ref this.subCalls).ResizeArray<DecalSubDrawCall>(newCapacity);
			base.capacity = newCapacity;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00016ABC File Offset: 0x00014CBC
		public override void Dispose()
		{
			this.subCallCounts.Dispose();
			if (base.capacity == 0)
			{
				return;
			}
			this.decalToWorlds.Dispose();
			this.normalToDecals.Dispose();
			this.renderingLayerMasks.Dispose();
			this.subCalls.Dispose();
			base.count = 0;
			base.capacity = 0;
		}

		// Token: 0x040002F1 RID: 753
		public NativeArray<float4x4> decalToWorlds;

		// Token: 0x040002F2 RID: 754
		public NativeArray<float4x4> normalToDecals;

		// Token: 0x040002F3 RID: 755
		public NativeArray<float> renderingLayerMasks;

		// Token: 0x040002F4 RID: 756
		public NativeArray<DecalSubDrawCall> subCalls;

		// Token: 0x040002F5 RID: 757
		public NativeArray<int> subCallCounts;
	}
}
