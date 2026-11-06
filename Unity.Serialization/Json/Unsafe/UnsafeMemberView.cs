using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json.Unsafe
{
	// Token: 0x02000090 RID: 144
	internal readonly struct UnsafeMemberView
	{
		// Token: 0x06000387 RID: 903 RVA: 0x0000F270 File Offset: 0x0000D470
		internal unsafe UnsafeMemberView(UnsafePackedBinaryStream* stream, int tokenIndex)
		{
			this.m_Stream = stream;
			this.m_TokenIndex = tokenIndex;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000F280 File Offset: 0x0000D480
		public UnsafeValueView Key()
		{
			return new UnsafeValueView(this.m_Stream, this.m_TokenIndex);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000F293 File Offset: 0x0000D493
		public unsafe UnsafeValueView Value()
		{
			return new UnsafeValueView(this.m_Stream, this.m_Stream->GetFirstChildIndex(this.m_TokenIndex));
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000F2B1 File Offset: 0x0000D4B1
		public unsafe SerializedMemberView AsSafe()
		{
			return new SerializedMemberView(this.m_Stream, this.m_Stream->GetHandle(this.m_TokenIndex));
		}

		// Token: 0x040001CA RID: 458
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001CB RID: 459
		private readonly int m_TokenIndex;
	}
}
