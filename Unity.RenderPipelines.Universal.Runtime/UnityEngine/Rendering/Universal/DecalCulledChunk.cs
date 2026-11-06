using System;
using Unity.Collections;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000076 RID: 118
	internal class DecalCulledChunk : DecalChunk
	{
		// Token: 0x06000438 RID: 1080 RVA: 0x00018C50 File Offset: 0x00016E50
		public override void RemoveAtSwapBack(int entityIndex)
		{
			base.RemoveAtSwapBack<int>(ref this.visibleDecalIndexArray, entityIndex, base.count);
			base.RemoveAtSwapBack<int>(ref this.visibleDecalIndices, entityIndex, base.count);
			int count = base.count;
			base.count = count - 1;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00018C93 File Offset: 0x00016E93
		public override void SetCapacity(int newCapacity)
		{
			ArrayExtensions.ResizeArray<int>(ref this.visibleDecalIndexArray, newCapacity);
			(ref this.visibleDecalIndices).ResizeArray<int>(newCapacity);
			if (this.cullingGroups == null)
			{
				this.cullingGroups = new CullingGroup();
			}
			base.capacity = newCapacity;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00018CC7 File Offset: 0x00016EC7
		public override void Dispose()
		{
			if (base.capacity == 0)
			{
				return;
			}
			this.visibleDecalIndices.Dispose();
			this.visibleDecalIndexArray = null;
			base.count = 0;
			base.capacity = 0;
			this.cullingGroups.Dispose();
			this.cullingGroups = null;
		}

		// Token: 0x04000336 RID: 822
		public Vector3 cameraPosition;

		// Token: 0x04000337 RID: 823
		public ulong sceneCullingMask;

		// Token: 0x04000338 RID: 824
		public int cullingMask;

		// Token: 0x04000339 RID: 825
		public CullingGroup cullingGroups;

		// Token: 0x0400033A RID: 826
		public int[] visibleDecalIndexArray;

		// Token: 0x0400033B RID: 827
		public NativeArray<int> visibleDecalIndices;

		// Token: 0x0400033C RID: 828
		public int visibleDecalCount;
	}
}
