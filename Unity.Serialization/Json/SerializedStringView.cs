using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Serialization.Json.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x0200008C RID: 140
	public readonly struct SerializedStringView : ISerializedView, IEquatable<string>
	{
		// Token: 0x06000355 RID: 853 RVA: 0x0000EA86 File Offset: 0x0000CC86
		internal unsafe SerializedStringView(UnsafePackedBinaryStream* stream, Handle handle)
		{
			this.m_Stream = stream;
			this.m_Handle = handle;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000EA96 File Offset: 0x0000CC96
		public unsafe int Length()
		{
			return *this.m_Stream->GetBufferPtr<int>(this.m_Handle);
		}

		// Token: 0x17000063 RID: 99
		public unsafe char this[int index]
		{
			get
			{
				byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
				if ((ulong)index > (ulong)((long)(*(int*)bufferPtr)))
				{
					throw new IndexOutOfRangeException();
				}
				char* ptr = (char*)(bufferPtr + 4);
				return ptr[index];
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000EAE4 File Offset: 0x0000CCE4
		public unsafe bool Equals(string other)
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
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

		// Token: 0x06000359 RID: 857 RVA: 0x0000EB40 File Offset: 0x0000CD40
		public unsafe bool Equals<[IsUnmanaged] T>(T other) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			int num = *(int*)bufferPtr;
			char* ptr = (char*)(bufferPtr + 4);
			return UTF8ArrayUnsafeUtility.StrCmp(other.GetUnsafePtr(), other.Length, ptr, num) == 0;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000EB88 File Offset: 0x0000CD88
		public unsafe override string ToString()
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			char* ptr = (char*)(bufferPtr + 4);
			int num = *(int*)bufferPtr;
			return new string(ptr, 0, num);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
		public unsafe T AsFixedString<[IsUnmanaged] T>() where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			char* ptr = (char*)(bufferPtr + 4);
			int num = *(int*)bufferPtr;
			T t = new T();
			int num2;
			if (Unicode.Utf16ToUtf8(ptr, num, t.GetUnsafePtr(), out num2, t.Capacity) != ConversionError.None)
			{
				throw new Exception("ConversionError");
			}
			t.Length = num2;
			return t;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000EC20 File Offset: 0x0000CE20
		public unsafe NativeText AsNativeText(Allocator allocator)
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			char* ptr = (char*)(bufferPtr + 4);
			int num = *(int*)bufferPtr;
			NativeText nativeText = new NativeText(num, allocator);
			int num2;
			bool flag = Unicode.Utf16ToUtf8(ptr, num, nativeText.GetUnsafePtr(), out num2, num) != ConversionError.None;
			nativeText.Length = num2;
			if (flag)
			{
				throw new Exception("ConversionError");
			}
			return nativeText;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000EC74 File Offset: 0x0000CE74
		public unsafe UnsafeText AsUnsafeText(Allocator allocator)
		{
			byte* bufferPtr = this.m_Stream->GetBufferPtr<byte>(this.m_Handle);
			char* ptr = (char*)(bufferPtr + 4);
			int num = *(int*)bufferPtr;
			UnsafeText unsafeText = new UnsafeText(num, allocator);
			int num2;
			bool flag = Unicode.Utf16ToUtf8(ptr, num, unsafeText.GetUnsafePtr(), out num2, num) != ConversionError.None;
			unsafeText.Length = num2;
			if (flag)
			{
				throw new Exception("ConversionError");
			}
			return unsafeText;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000ECCD File Offset: 0x0000CECD
		internal unsafe UnsafeStringView AsUnsafe()
		{
			return new UnsafeStringView(this.m_Stream, this.m_Stream->GetTokenIndex(this.m_Handle));
		}

		// Token: 0x040001C0 RID: 448
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001C1 RID: 449
		private readonly Handle m_Handle;
	}
}
