using System;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200038A RID: 906
	[Serializable]
	public struct RuntimeGlobalObjectId : IEquatable<RuntimeGlobalObjectId>, IComparable<RuntimeGlobalObjectId>
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600181F RID: 6175 RVA: 0x0005A416 File Offset: 0x00058616
		public bool IsValid
		{
			get
			{
				return this.AssetGUID.IsValid;
			}
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0005A424 File Offset: 0x00058624
		public int CompareTo(RuntimeGlobalObjectId other)
		{
			int num = this.AssetGUID.CompareTo(other.AssetGUID);
			if (num != 0)
			{
				return num;
			}
			return this.SceneObjectIdentifier0.CompareTo(other.SceneObjectIdentifier0);
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0005A459 File Offset: 0x00058659
		public bool Equals(RuntimeGlobalObjectId other)
		{
			return this.SceneObjectIdentifier0 == other.SceneObjectIdentifier0 && this.AssetGUID.Equals(other.AssetGUID);
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0005A47C File Offset: 0x0005867C
		public override int GetHashCode()
		{
			return (this.SceneObjectIdentifier0.GetHashCode() * 397) ^ this.AssetGUID.GetHashCode();
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0005A4A1 File Offset: 0x000586A1
		public override string ToString()
		{
			return string.Format("{0}:{1}", this.AssetGUID, this.SceneObjectIdentifier0);
		}

		// Token: 0x04000935 RID: 2357
		public long SceneObjectIdentifier0;

		// Token: 0x04000936 RID: 2358
		public long SceneObjectIdentifier1;

		// Token: 0x04000937 RID: 2359
		public Hash128 AssetGUID;

		// Token: 0x04000938 RID: 2360
		public int IdentifierType;
	}
}
