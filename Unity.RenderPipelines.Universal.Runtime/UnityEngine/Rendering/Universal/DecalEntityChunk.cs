using System;
using Unity.Collections;
using UnityEngine.Jobs;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000070 RID: 112
	internal class DecalEntityChunk : DecalChunk
	{
		// Token: 0x0600041A RID: 1050 RVA: 0x0001768C File Offset: 0x0001588C
		public override void Push()
		{
			int count = base.count;
			base.count = count + 1;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000176AC File Offset: 0x000158AC
		public override void RemoveAtSwapBack(int entityIndex)
		{
			base.RemoveAtSwapBack<DecalEntity>(ref this.decalEntities, entityIndex, base.count);
			base.RemoveAtSwapBack<DecalProjector>(ref this.decalProjectors, entityIndex, base.count);
			this.transformAccessArray.RemoveAtSwapBack(entityIndex);
			int count = base.count;
			base.count = count - 1;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000176FB File Offset: 0x000158FB
		public override void SetCapacity(int newCapacity)
		{
			(ref this.decalEntities).ResizeArray<DecalEntity>(newCapacity);
			base.ResizeNativeArray(ref this.transformAccessArray, this.decalProjectors, newCapacity);
			ArrayExtensions.ResizeArray<DecalProjector>(ref this.decalProjectors, newCapacity);
			base.capacity = newCapacity;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001772F File Offset: 0x0001592F
		public override void Dispose()
		{
			if (base.capacity == 0)
			{
				return;
			}
			this.decalEntities.Dispose();
			this.transformAccessArray.Dispose();
			this.decalProjectors = null;
			base.count = 0;
			base.capacity = 0;
		}

		// Token: 0x04000304 RID: 772
		public Material material;

		// Token: 0x04000305 RID: 773
		public NativeArray<DecalEntity> decalEntities;

		// Token: 0x04000306 RID: 774
		public DecalProjector[] decalProjectors;

		// Token: 0x04000307 RID: 775
		public TransformAccessArray transformAccessArray;
	}
}
