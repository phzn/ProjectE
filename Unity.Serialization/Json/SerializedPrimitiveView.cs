using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Serialization.Json.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x0200008B RID: 139
	public readonly struct SerializedPrimitiveView : ISerializedView
	{
		// Token: 0x06000341 RID: 833 RVA: 0x0000E6DC File Offset: 0x0000C8DC
		internal unsafe SerializedPrimitiveView(UnsafePackedBinaryStream* stream, Handle handle)
		{
			this.m_Stream = stream;
			this.m_Handle = handle;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000E6EC File Offset: 0x0000C8EC
		public unsafe bool IsInfinity()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			if (Convert.IsSigned(ptr, num))
			{
				ptr++;
				num--;
			}
			return Convert.MatchesInfinity(ptr, num);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000E728 File Offset: 0x0000C928
		public unsafe bool IsNaN()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			return Convert.MatchesNaN(ptr, num);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000E754 File Offset: 0x0000C954
		public unsafe bool IsNull()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			return Convert.MatchesNull(ptr, num);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000E780 File Offset: 0x0000C980
		public unsafe bool IsIntegral()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			return Convert.IsIntegral((char*)(bufferPtr + 4), *(int*)bufferPtr);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000E7AC File Offset: 0x0000C9AC
		public unsafe bool IsDecimal()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			return Convert.IsDecimal((char*)(bufferPtr + 4), *(int*)bufferPtr);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
		public unsafe bool IsSigned()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			return Convert.IsSigned((char*)(bufferPtr + 4), *(int*)bufferPtr);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000E804 File Offset: 0x0000CA04
		public unsafe bool IsBoolean()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			return Convert.MatchesTrue(ptr, num) || Convert.MatchesFalse(ptr, num);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000E83A File Offset: 0x0000CA3A
		public SerializedStringView AsStringView()
		{
			return new SerializedStringView(this.m_Stream, this.m_Handle);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000E850 File Offset: 0x0000CA50
		public string AsString()
		{
			return this.AsStringView().ToString();
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000E874 File Offset: 0x0000CA74
		public unsafe long AsInt64()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			long num;
			Convert.ParseError parseError = Convert.StrToInt64((char*)(bufferPtr + 4), *(int*)bufferPtr, out num);
			if (parseError != Convert.ParseError.None)
			{
				throw new ParseErrorException(string.Format("Failed to parse Value=[{0}] as Type=[{1}] ParseError=[{2}]", this.AsString(), typeof(long), parseError));
			}
			return num;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000E8CC File Offset: 0x0000CACC
		public unsafe ulong AsUInt64()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			ulong num;
			Convert.ParseError parseError = Convert.StrToUInt64((char*)(bufferPtr + 4), *(int*)bufferPtr, out num);
			if (parseError != Convert.ParseError.None)
			{
				throw new ParseErrorException(string.Format("Failed to parse Value=[{0}] as Type=[{1}] ParseError=[{2}]", this.AsString(), typeof(ulong), parseError));
			}
			return num;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000E924 File Offset: 0x0000CB24
		public unsafe float AsFloat()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			float num;
			Convert.ParseError parseError = Convert.StrToFloat32((char*)(bufferPtr + 4), *(int*)bufferPtr, out num);
			if (parseError != Convert.ParseError.None)
			{
				throw new ParseErrorException(string.Format("Failed to parse Value=[{0}] as Type=[{1}] ParseError=[{2}]", this.AsString(), typeof(float), parseError));
			}
			return num;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000E97A File Offset: 0x0000CB7A
		public double AsDouble()
		{
			return double.Parse(this.AsString(), NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000E994 File Offset: 0x0000CB94
		public unsafe bool AsBoolean()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
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

		// Token: 0x06000350 RID: 848 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
		public override string ToString()
		{
			return this.AsStringView().ToString();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000EA14 File Offset: 0x0000CC14
		public T AsFixedString<[IsUnmanaged] T>() where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			return this.AsStringView().AsFixedString<T>();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000EA30 File Offset: 0x0000CC30
		public NativeText AsNativeText(Allocator allocator)
		{
			return this.AsStringView().AsNativeText(allocator);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000EA4C File Offset: 0x0000CC4C
		public UnsafeText AsUnsafeText(Allocator allocator)
		{
			return this.AsStringView().AsUnsafeText(allocator);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000EA68 File Offset: 0x0000CC68
		internal unsafe UnsafePrimitiveView AsUnsafe()
		{
			return new UnsafePrimitiveView(this.m_Stream, this.m_Stream->GetTokenIndex(this.m_Handle));
		}

		// Token: 0x040001BE RID: 446
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001BF RID: 447
		private readonly Handle m_Handle;
	}
}
