using System;

namespace UnityEngine.ResourceManagement.Profiling
{
	// Token: 0x0200004F RID: 79
	internal struct AssetFrameData
	{
		// Token: 0x060001DF RID: 479 RVA: 0x00007190 File Offset: 0x00005390
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is AssetFrameData)
			{
				AssetFrameData assetFrameData = (AssetFrameData)obj;
				return this.AssetCode == assetFrameData.AssetCode && this.BundleCode == assetFrameData.BundleCode;
			}
			return false;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000071D4 File Offset: 0x000053D4
		public override int GetHashCode()
		{
			return HashCode.Combine<int, int, int, int, int>(this.AssetCode.GetHashCode(), this.BundleCode.GetHashCode(), this.ReferenceCount.GetHashCode(), this.PercentComplete.GetHashCode(), this.Status.GetHashCode());
		}

		// Token: 0x040000C0 RID: 192
		public int AssetCode;

		// Token: 0x040000C1 RID: 193
		public int BundleCode;

		// Token: 0x040000C2 RID: 194
		public int ReferenceCount;

		// Token: 0x040000C3 RID: 195
		public float PercentComplete;

		// Token: 0x040000C4 RID: 196
		public ContentStatus Status;
	}
}
