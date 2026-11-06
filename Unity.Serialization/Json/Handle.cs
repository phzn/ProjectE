using System;

namespace Unity.Serialization.Json
{
	// Token: 0x0200004C RID: 76
	internal struct Handle : IEquatable<Handle>
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x0000770E File Offset: 0x0000590E
		public bool Equals(Handle other)
		{
			return this.Index == other.Index && this.Version == other.Version;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007730 File Offset: 0x00005930
		public override bool Equals(object obj)
		{
			if (obj is Handle)
			{
				Handle handle = (Handle)obj;
				return this.Equals(handle);
			}
			return false;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00007755 File Offset: 0x00005955
		public override int GetHashCode()
		{
			return (this.Index * 397) ^ this.Version;
		}

		// Token: 0x040000D1 RID: 209
		public static readonly Handle Null = new Handle
		{
			Index = -1,
			Version = -1
		};

		// Token: 0x040000D2 RID: 210
		public int Index;

		// Token: 0x040000D3 RID: 211
		public int Version;
	}
}
