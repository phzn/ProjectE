using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000029 RID: 41
	public static class BlobStringExtensions
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00003794 File Offset: 0x00001994
		public unsafe static void AllocateString(this BlobBuilder builder, ref BlobString blobStr, string value)
		{
			fixed (string text = value)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				int num = value.Length * 2 + 1;
				byte* ptr2 = (byte*)UnsafeUtility.Malloc((long)num, 1, Allocator.Temp);
				int num2;
				Unicode.Utf16ToUtf8(ptr, value.Length, ptr2, out num2, num);
				ptr2[num2] = 0;
				UnsafeUtility.MemCpy(builder.Allocate<byte>(ref blobStr.Data, num2 + 1).GetUnsafePtr(), (void*)ptr2, (long)(num2 + 1));
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003804 File Offset: 0x00001A04
		public unsafe static void AllocateString<T>(this BlobBuilder builder, ref BlobString blobStr, ref T value) where T : INativeList<byte>
		{
			int length = value.Length;
			byte* ptr = (byte*)UnsafeUtility.AddressOf<byte>(value.ElementAt(0));
			UnsafeUtility.MemCpy(builder.Allocate<byte>(ref blobStr.Data, length + 1).GetUnsafePtr(), (void*)ptr, (long)(length + 1));
		}
	}
}
