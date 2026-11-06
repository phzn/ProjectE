using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Serialization.Json.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000086 RID: 134
	public readonly struct SerializedMemberView
	{
		// Token: 0x0600030A RID: 778 RVA: 0x0000DE68 File Offset: 0x0000C068
		internal unsafe SerializedMemberView(UnsafePackedBinaryStream* stream, Handle handle)
		{
			this.m_Stream = stream;
			this.m_Handle = handle;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000DE78 File Offset: 0x0000C078
		public SerializedStringView Name()
		{
			return new SerializedStringView(this.m_Stream, this.m_Handle);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000DE8B File Offset: 0x0000C08B
		public unsafe SerializedValueView Value()
		{
			return new SerializedValueView(this.m_Stream, this.m_Stream->GetFirstChild(this.m_Handle));
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000DEA9 File Offset: 0x0000C0A9
		internal unsafe UnsafeMemberView AsUnsafe()
		{
			return new UnsafeMemberView(this.m_Stream, this.m_Stream->GetTokenIndex(this.m_Handle));
		}

		// Token: 0x040001B4 RID: 436
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001B5 RID: 437
		private readonly Handle m_Handle;
	}
}
