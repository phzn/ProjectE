using System;
using System.Security;

namespace System.Collections.Generic
{
	// Token: 0x02000AC9 RID: 2761
	[Serializable]
	internal class ByteEqualityComparer : EqualityComparer<byte>
	{
		// Token: 0x060062A6 RID: 25254 RVA: 0x0014A336 File Offset: 0x00148536
		public override bool Equals(byte x, byte y)
		{
			return x == y;
		}

		// Token: 0x060062A7 RID: 25255 RVA: 0x0014A33C File Offset: 0x0014853C
		public override int GetHashCode(byte b)
		{
			return b.GetHashCode();
		}

		// Token: 0x060062A8 RID: 25256 RVA: 0x0014A348 File Offset: 0x00148548
		[SecuritySafeCritical]
		internal unsafe override int IndexOf(byte[] array, byte value, int startIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("Index was out of range. Must be non-negative and less than the size of the collection."));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("Count must be positive and count must refer to a location within the string/array/collection."));
			}
			if (count > array.Length - startIndex)
			{
				throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
			}
			if (count == 0)
			{
				return -1;
			}
			byte* ptr;
			if (array == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			return Buffer.IndexOfByte(ptr, value, startIndex, count);
		}

		// Token: 0x060062A9 RID: 25257 RVA: 0x0014A3D8 File Offset: 0x001485D8
		internal override int LastIndexOf(byte[] array, byte value, int startIndex, int count)
		{
			int num = startIndex - count + 1;
			for (int i = startIndex; i >= num; i--)
			{
				if (array[i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060062AA RID: 25258 RVA: 0x0014A401 File Offset: 0x00148601
		public override bool Equals(object obj)
		{
			return obj is ByteEqualityComparer;
		}

		// Token: 0x060062AB RID: 25259 RVA: 0x00149C92 File Offset: 0x00147E92
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
