using System;
using Unity.Collections;

namespace Unity.Entities.Content
{
	// Token: 0x02000474 RID: 1140
	[Serializable]
	public struct RemoteContentLocation : IEquatable<RemoteContentLocation>
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06001C21 RID: 7201 RVA: 0x00062581 File Offset: 0x00060781
		public bool IsValid
		{
			get
			{
				return this.Hash.IsValid;
			}
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x0006258E File Offset: 0x0006078E
		public bool Equals(RemoteContentLocation other)
		{
			if (!this.Hash.IsValid && !other.Hash.IsValid)
			{
				return this.Path.Equals(other.Path);
			}
			return this.Hash.Equals(other.Hash);
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x000625CE File Offset: 0x000607CE
		public override int GetHashCode()
		{
			if (!this.Hash.IsValid)
			{
				return this.Path.GetHashCode();
			}
			return this.Hash.GetHashCode();
		}

		// Token: 0x040009EF RID: 2543
		public RemoteContentLocation.LocationType Type;

		// Token: 0x040009F0 RID: 2544
		public FixedString512Bytes Path;

		// Token: 0x040009F1 RID: 2545
		public Hash128 Hash;

		// Token: 0x040009F2 RID: 2546
		public uint Crc;

		// Token: 0x040009F3 RID: 2547
		public long Size;

		// Token: 0x02000475 RID: 1141
		public enum LocationType
		{
			// Token: 0x040009F5 RID: 2549
			RemoteURL
		}
	}
}
