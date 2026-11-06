using System;
using Unity.Collections;
using UnityEngine;

namespace Unity.Entities.Content
{
	// Token: 0x02000473 RID: 1139
	[Serializable]
	public struct RemoteContentId : IEquatable<RemoteContentId>
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06001C1B RID: 7195 RVA: 0x000624CD File Offset: 0x000606CD
		// (set) Token: 0x06001C1A RID: 7194 RVA: 0x000624C4 File Offset: 0x000606C4
		public Hash128 Hash { readonly get; private set; }

		// Token: 0x06001C1C RID: 7196 RVA: 0x000624D5 File Offset: 0x000606D5
		public unsafe RemoteContentId(in FixedString512Bytes name)
		{
			this.Name = name;
			this.Hash = Hash128.Compute((void*)this.Name.GetUnsafePtr(), (ulong)this.Name.utf8LengthInBytes);
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x0006250A File Offset: 0x0006070A
		public RemoteContentId(in FixedString512Bytes name, Hash128 hash)
		{
			this.Name = name;
			this.Hash = hash;
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x00062520 File Offset: 0x00060720
		public bool IsValid
		{
			get
			{
				return this.Hash.IsValid;
			}
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0006253C File Offset: 0x0006073C
		public bool Equals(RemoteContentId other)
		{
			return this.Hash.Equals(other.Hash);
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x00062560 File Offset: 0x00060760
		public override int GetHashCode()
		{
			return this.Hash.GetHashCode();
		}

		// Token: 0x040009ED RID: 2541
		public FixedString512Bytes Name;
	}
}
