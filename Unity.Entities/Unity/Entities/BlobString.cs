using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000028 RID: 40
	[MayOnlyLiveInBlobStorage]
	[DebuggerDisplay("Cannot display the value of a BlobString by itself. Please inspect the root BlobAssetReference instead.")]
	public struct BlobString
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000036C8 File Offset: 0x000018C8
		public int Length
		{
			get
			{
				return math.max(0, this.Data.Length - 1);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000036DD File Offset: 0x000018DD
		public new unsafe string ToString()
		{
			return BlobString.ToString((byte*)this.Data.GetUnsafePtr(), this.Length);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000036F8 File Offset: 0x000018F8
		internal unsafe static string ToString(byte* data, int lengthInBytes)
		{
			int num = math.max(1, lengthInBytes * 2);
			checked
			{
				char* ptr = stackalloc char[unchecked((UIntPtr)num) * 2];
				int num2;
				Unicode.Utf8ToUtf16(data, lengthInBytes, ptr, out num2, num);
				return new string(ptr, 0, num2);
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000372C File Offset: 0x0000192C
		public unsafe ConversionError CopyTo<T>(ref T dest) where T : INativeList<byte>
		{
			byte* unsafePtr = (byte*)this.Data.GetUnsafePtr();
			int length = this.Length;
			dest.Length = length;
			byte* ptr = (byte*)UnsafeUtility.AddressOf<byte>(dest.ElementAt(0));
			int capacity = dest.Capacity;
			int num;
			ConversionError conversionError = Unicode.Utf8ToUtf8(unsafePtr, length, ptr, out num, capacity);
			dest.Length = num;
			return conversionError;
		}

		// Token: 0x04000037 RID: 55
		internal BlobArray<byte> Data;
	}
}
