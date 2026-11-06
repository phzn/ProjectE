using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json.Unsafe
{
	// Token: 0x02000094 RID: 148
	public readonly struct UnsafeStringView : IEquatable<string>
	{
		// Token: 0x060003AB RID: 939 RVA: 0x0000F8EB File Offset: 0x0000DAEB
		internal unsafe UnsafeStringView(UnsafePackedBinaryStream* stream, int tokenIndex)
		{
			this.m_Stream = stream;
			this.m_TokenIndex = tokenIndex;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000F8FB File Offset: 0x0000DAFB
		public unsafe int Length()
		{
			return *this.m_Stream->GetBufferPtr<int>(this.m_TokenIndex);
		}

		// Token: 0x1700006E RID: 110
		public unsafe char this[int index]
		{
			get
			{
				byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
				if ((ulong)index > (ulong)((long)(*(int*)bufferPtr)))
				{
					throw new IndexOutOfRangeException();
				}
				char* ptr = (char*)(bufferPtr + 4);
				return ptr[index];
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000F948 File Offset: 0x0000DB48
		public unsafe bool Equals(string other)
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			if (other == null)
			{
				return *(int*)bufferPtr == 0;
			}
			if (other.Length != *(int*)bufferPtr)
			{
				return false;
			}
			char* ptr = (char*)(bufferPtr + 4);
			for (int i = 0; i < other.Length; i++)
			{
				if (ptr[i] != other[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
		public unsafe override string ToString()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_TokenIndex);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			return new string(ptr, 0, num);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000F9D0 File Offset: 0x0000DBD0
		public unsafe SerializedStringView AsSafe()
		{
			return new SerializedStringView(this.m_Stream, this.m_Stream->GetHandle(this.m_TokenIndex));
		}

		// Token: 0x040001D4 RID: 468
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001D5 RID: 469
		private readonly int m_TokenIndex;
	}
}
