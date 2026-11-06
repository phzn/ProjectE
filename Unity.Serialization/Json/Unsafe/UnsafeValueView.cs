using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json.Unsafe
{
	// Token: 0x02000095 RID: 149
	internal readonly struct UnsafeValueView
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x0000F9EE File Offset: 0x0000DBEE
		internal unsafe UnsafeValueView(UnsafePackedBinaryStream* stream, int tokenIndex)
		{
			this.m_Stream = stream;
			this.m_TokenIndex = tokenIndex;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000F9FE File Offset: 0x0000DBFE
		public unsafe TokenType Type
		{
			get
			{
				return this.m_Stream->GetToken(this.m_TokenIndex).Type;
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000FA18 File Offset: 0x0000DC18
		public unsafe bool IsMember()
		{
			BinaryToken token = this.m_Stream->GetToken(this.m_TokenIndex);
			return (token.Parent == -1 || token.Type == TokenType.Object) && (token.Type == TokenType.String || token.Type == TokenType.Primitive);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000FA5F File Offset: 0x0000DC5F
		public UnsafeStringView AsStringView()
		{
			return new UnsafeStringView(this.m_Stream, this.m_TokenIndex);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000FA72 File Offset: 0x0000DC72
		public UnsafeArrayView AsArrayView()
		{
			return new UnsafeArrayView(this.m_Stream, this.m_TokenIndex);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000FA85 File Offset: 0x0000DC85
		public UnsafeObjectView AsObjectView()
		{
			return new UnsafeObjectView(this.m_Stream, this.m_TokenIndex);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000FA98 File Offset: 0x0000DC98
		public UnsafePrimitiveView AsPrimitiveView()
		{
			return new UnsafePrimitiveView(this.m_Stream, this.m_TokenIndex);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000FAAC File Offset: 0x0000DCAC
		public int AsInt32()
		{
			return (int)this.AsPrimitiveView().AsInt64();
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000FAC8 File Offset: 0x0000DCC8
		public long AsInt64()
		{
			return this.AsPrimitiveView().AsInt64();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000FAE4 File Offset: 0x0000DCE4
		public override string ToString()
		{
			return this.AsStringView().ToString();
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000FB05 File Offset: 0x0000DD05
		public unsafe SerializedValueView AsSafe()
		{
			return new SerializedValueView(this.m_Stream, this.m_Stream->GetHandle(this.m_TokenIndex));
		}

		// Token: 0x040001D6 RID: 470
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001D7 RID: 471
		private readonly int m_TokenIndex;
	}
}
