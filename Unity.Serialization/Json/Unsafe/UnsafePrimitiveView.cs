using System;
using System.Globalization;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json.Unsafe
{
	// Token: 0x02000093 RID: 147
	internal readonly struct UnsafePrimitiveView
	{
		// Token: 0x0600039B RID: 923 RVA: 0x0000F5BC File Offset: 0x0000D7BC
		internal unsafe UnsafePrimitiveView(UnsafePackedBinaryStream* stream, int tokenIndex)
		{
			this.m_Stream = stream;
			this.m_TokenIndex = tokenIndex;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000F5CC File Offset: 0x0000D7CC
		public UnsafeStringView AsStringView()
		{
			return new UnsafeStringView(this.m_Stream, this.m_TokenIndex);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000F5E0 File Offset: 0x0000D7E0
		public unsafe bool IsInfinity()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			if (Convert.IsSigned(ptr, num))
			{
				ptr++;
				num--;
			}
			return Convert.MatchesInfinity(ptr, num);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000F61C File Offset: 0x0000D81C
		public unsafe bool IsNaN()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			return Convert.MatchesNaN(ptr, num);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000F648 File Offset: 0x0000D848
		public unsafe bool IsNull()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			return Convert.MatchesNull(ptr, num);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000F674 File Offset: 0x0000D874
		public unsafe bool IsIntegral()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			return Convert.IsIntegral((char*)(bufferPtr + 4), *(int*)bufferPtr);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000F6A0 File Offset: 0x0000D8A0
		public unsafe bool IsDecimal()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			return Convert.IsDecimal((char*)(bufferPtr + 4), *(int*)bufferPtr);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000F6CC File Offset: 0x0000D8CC
		public unsafe bool IsSigned()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			return Convert.IsSigned((char*)(bufferPtr + 4), *(int*)bufferPtr);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000F6F8 File Offset: 0x0000D8F8
		public unsafe bool IsBoolean()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			return Convert.MatchesTrue(ptr, num) || Convert.MatchesFalse(ptr, num);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000F730 File Offset: 0x0000D930
		public unsafe long AsInt64()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			long num;
			Convert.ParseError parseError = Convert.StrToInt64((char*)(bufferPtr + 4), *(int*)bufferPtr, out num);
			if (parseError != Convert.ParseError.None)
			{
				throw new ParseErrorException(string.Format("Failed to parse Value=[{0}] as Type=[{1}] ParseError=[{2}]", this.AsString(), typeof(long), parseError));
			}
			return num;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000F788 File Offset: 0x0000D988
		public unsafe ulong AsUInt64()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			ulong num;
			Convert.ParseError parseError = Convert.StrToUInt64((char*)(bufferPtr + 4), *(int*)bufferPtr, out num);
			if (parseError != Convert.ParseError.None)
			{
				throw new ParseErrorException(string.Format("Failed to parse Value=[{0}] as Type=[{1}] ParseError=[{2}]", this.AsString(), typeof(ulong), parseError));
			}
			return num;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000F7E0 File Offset: 0x0000D9E0
		public unsafe float AsFloat()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			float num;
			Convert.ParseError parseError = Convert.StrToFloat32((char*)(bufferPtr + 4), *(int*)bufferPtr, out num);
			if (parseError != Convert.ParseError.None)
			{
				throw new ParseErrorException(string.Format("Failed to parse Value=[{0}] as Type=[{1}] ParseError=[{2}]", this.AsString(), typeof(float), parseError));
			}
			return num;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000F836 File Offset: 0x0000DA36
		public double AsDouble()
		{
			return double.Parse(this.AsString(), NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000F850 File Offset: 0x0000DA50
		public unsafe bool AsBoolean()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			if (Convert.MatchesTrue(ptr, num))
			{
				return true;
			}
			if (Convert.MatchesFalse(ptr, num))
			{
				return false;
			}
			throw new ParseErrorException(string.Format("Failed to parse Value=[{0}] as Type=[{1}]", this.AsString(), typeof(bool)));
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000F8AC File Offset: 0x0000DAAC
		public string AsString()
		{
			return this.AsStringView().ToString();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000F8CD File Offset: 0x0000DACD
		public unsafe SerializedPrimitiveView AsSafe()
		{
			return new SerializedPrimitiveView(this.m_Stream, this.m_Stream->GetHandle(this.m_TokenIndex));
		}

		// Token: 0x040001D2 RID: 466
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001D3 RID: 467
		private readonly int m_TokenIndex;
	}
}
