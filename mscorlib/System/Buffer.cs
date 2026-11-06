using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System
{
	/// <summary>Manipulates arrays of primitive types.</summary>
	// Token: 0x020001EC RID: 492
	[ComVisible(true)]
	public static class Buffer
	{
		// Token: 0x0600153A RID: 5434
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalBlockCopy(Array src, int srcOffsetBytes, Array dst, int dstOffsetBytes, int byteCount);

		// Token: 0x0600153B RID: 5435 RVA: 0x000533B4 File Offset: 0x000515B4
		[SecurityCritical]
		internal unsafe static int IndexOfByte(byte* src, byte value, int index, int count)
		{
			byte* ptr = src + index;
			while ((ptr & 3) != 0)
			{
				if (count == 0)
				{
					return -1;
				}
				if (*ptr == value)
				{
					return (int)((long)(ptr - src));
				}
				count--;
				ptr++;
			}
			uint num = (uint)(((int)value << 8) + (int)value);
			num = (num << 16) + num;
			while (count > 3)
			{
				uint num2 = *(uint*)ptr;
				num2 ^= num;
				uint num3 = 2130640639U + num2;
				num2 ^= uint.MaxValue;
				num2 ^= num3;
				num2 &= 2164326656U;
				if (num2 != 0U)
				{
					int num4 = (int)((long)(ptr - src));
					if (*ptr == value)
					{
						return num4;
					}
					if (ptr[1] == value)
					{
						return num4 + 1;
					}
					if (ptr[2] == value)
					{
						return num4 + 2;
					}
					if (ptr[3] == value)
					{
						return num4 + 3;
					}
				}
				count -= 4;
				ptr += 4;
			}
			while (count > 0)
			{
				if (*ptr == value)
				{
					return (int)((long)(ptr - src));
				}
				count--;
				ptr++;
			}
			return -1;
		}

		// Token: 0x0600153C RID: 5436
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _ByteLength(Array array);

		// Token: 0x0600153D RID: 5437 RVA: 0x00053478 File Offset: 0x00051678
		[SecurityCritical]
		internal unsafe static void ZeroMemory(byte* src, long len)
		{
			for (;;)
			{
				long num = len;
				len = num - 1L;
				if (num <= 0L)
				{
					break;
				}
				src[len] = 0;
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00053490 File Offset: 0x00051690
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal unsafe static void Memcpy(byte[] dest, int destIndex, byte* src, int srcIndex, int len)
		{
			if (len == 0)
			{
				return;
			}
			fixed (byte[] array = dest)
			{
				byte* ptr;
				if (dest == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				Buffer.Memcpy(ptr + destIndex, src + srcIndex, len);
			}
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x000534CC File Offset: 0x000516CC
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal unsafe static void Memcpy(byte* pDest, int destIndex, byte[] src, int srcIndex, int len)
		{
			if (len == 0)
			{
				return;
			}
			fixed (byte[] array = src)
			{
				byte* ptr;
				if (src == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				Buffer.Memcpy(pDest + destIndex, ptr + srcIndex, len);
			}
		}

		// Token: 0x06001540 RID: 5440
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void InternalMemcpy(byte* dest, byte* src, int count);

		/// <summary>Returns the number of bytes in the specified array.</summary>
		/// <param name="array">An array.</param>
		/// <returns>The number of bytes in the array.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is not a primitive.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="array" /> is larger than 2 gigabytes (GB).</exception>
		// Token: 0x06001541 RID: 5441 RVA: 0x00053505 File Offset: 0x00051705
		public static int ByteLength(Array array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = Buffer._ByteLength(array);
			if (num < 0)
			{
				throw new ArgumentException("Object must be an array of primitives.");
			}
			return num;
		}

		/// <summary>Retrieves the byte at the specified location in the specified array.</summary>
		/// <param name="array">An array.</param>
		/// <param name="index">A location in the array.</param>
		/// <returns>The byte at the specified location in the array.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is not a primitive.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is negative or greater than the length of <paramref name="array" />.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="array" /> is larger than 2 gigabytes (GB).</exception>
		// Token: 0x06001542 RID: 5442 RVA: 0x0005352A File Offset: 0x0005172A
		public unsafe static byte GetByte(Array array, int index)
		{
			if (index < 0 || index >= Buffer.ByteLength(array))
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return *(byte*)Unsafe.AsPointer<byte>(Unsafe.Add<byte>(array.GetRawSzArrayData(), index));
		}

		/// <summary>Assigns a specified value to a byte at a particular location in a specified array.</summary>
		/// <param name="array">An array.</param>
		/// <param name="index">A location in the array.</param>
		/// <param name="value">A value to assign.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is not a primitive.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is negative or greater than the length of <paramref name="array" />.</exception>
		/// <exception cref="T:System.OverflowException">
		///   <paramref name="array" /> is larger than 2 gigabytes (GB).</exception>
		// Token: 0x06001543 RID: 5443 RVA: 0x00053556 File Offset: 0x00051756
		public unsafe static void SetByte(Array array, int index, byte value)
		{
			if (index < 0 || index >= Buffer.ByteLength(array))
			{
				throw new ArgumentOutOfRangeException("index");
			}
			*(byte*)Unsafe.AsPointer<byte>(Unsafe.Add<byte>(array.GetRawSzArrayData(), index)) = value;
		}

		/// <summary>Copies a specified number of bytes from a source array starting at a particular offset to a destination array starting at a particular offset.</summary>
		/// <param name="src">The source buffer.</param>
		/// <param name="srcOffset">The zero-based byte offset into <paramref name="src" />.</param>
		/// <param name="dst">The destination buffer.</param>
		/// <param name="dstOffset">The zero-based byte offset into <paramref name="dst" />.</param>
		/// <param name="count">The number of bytes to copy.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="src" /> or <paramref name="dst" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="src" /> or <paramref name="dst" /> is not an array of primitives.  
		/// -or-  
		/// The number of bytes in <paramref name="src" /> is less than <paramref name="srcOffset" /> plus <paramref name="count" />.  
		/// -or-  
		/// The number of bytes in <paramref name="dst" /> is less than <paramref name="dstOffset" /> plus <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="srcOffset" />, <paramref name="dstOffset" />, or <paramref name="count" /> is less than 0.</exception>
		// Token: 0x06001544 RID: 5444 RVA: 0x00053584 File Offset: 0x00051784
		public static void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count)
		{
			if (src == null)
			{
				throw new ArgumentNullException("src");
			}
			if (dst == null)
			{
				throw new ArgumentNullException("dst");
			}
			if (srcOffset < 0)
			{
				throw new ArgumentOutOfRangeException("srcOffset", "Non-negative number required.");
			}
			if (dstOffset < 0)
			{
				throw new ArgumentOutOfRangeException("dstOffset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (!Buffer.InternalBlockCopy(src, srcOffset, dst, dstOffset, count) && (srcOffset > Buffer.ByteLength(src) - count || dstOffset > Buffer.ByteLength(dst) - count))
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
		}

		/// <summary>Copies a number of bytes specified as a long integer value from one address in memory to another.  
		///  This API is not CLS-compliant.</summary>
		/// <param name="source">The address of the bytes to copy.</param>
		/// <param name="destination">The target address.</param>
		/// <param name="destinationSizeInBytes">The number of bytes available in the destination memory block.</param>
		/// <param name="sourceBytesToCopy">The number of bytes to copy.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="sourceBytesToCopy" /> is greater than <paramref name="destinationSizeInBytes" />.</exception>
		// Token: 0x06001545 RID: 5445 RVA: 0x0005361C File Offset: 0x0005181C
		[CLSCompliant(false)]
		public unsafe static void MemoryCopy(void* source, void* destination, long destinationSizeInBytes, long sourceBytesToCopy)
		{
			if (sourceBytesToCopy > destinationSizeInBytes)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sourceBytesToCopy);
			}
			byte* ptr = (byte*)source;
			byte* ptr2 = (byte*)destination;
			while (sourceBytesToCopy > (long)((ulong)(-1)))
			{
				Buffer.Memmove(ptr2, ptr, uint.MaxValue);
				sourceBytesToCopy -= (long)((ulong)(-1));
				ptr += -1;
				ptr2 += -1;
			}
			Buffer.Memmove(ptr2, ptr, (uint)sourceBytesToCopy);
		}

		/// <summary>Copies a number of bytes specified as an unsigned long integer value from one address in memory to another.  
		///  This API is not CLS-compliant.</summary>
		/// <param name="source">The address of the bytes to copy.</param>
		/// <param name="destination">The target address.</param>
		/// <param name="destinationSizeInBytes">The number of bytes available in the destination memory block.</param>
		/// <param name="sourceBytesToCopy">The number of bytes to copy.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="sourceBytesToCopy" /> is greater than <paramref name="destinationSizeInBytes" />.</exception>
		// Token: 0x06001546 RID: 5446 RVA: 0x00053660 File Offset: 0x00051860
		[CLSCompliant(false)]
		public unsafe static void MemoryCopy(void* source, void* destination, ulong destinationSizeInBytes, ulong sourceBytesToCopy)
		{
			if (sourceBytesToCopy > destinationSizeInBytes)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sourceBytesToCopy);
			}
			byte* ptr = (byte*)source;
			byte* ptr2 = (byte*)destination;
			while (sourceBytesToCopy > (ulong)(-1))
			{
				Buffer.Memmove(ptr2, ptr, uint.MaxValue);
				sourceBytesToCopy -= (ulong)(-1);
				ptr += -1;
				ptr2 += -1;
			}
			Buffer.Memmove(ptr2, ptr, (uint)sourceBytesToCopy);
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x000536A4 File Offset: 0x000518A4
		internal unsafe static void memcpy4(byte* dest, byte* src, int size)
		{
			while (size >= 16)
			{
				*(int*)dest = *(int*)src;
				*(int*)(dest + 4) = *(int*)(src + 4);
				*(int*)(dest + (IntPtr)2 * 4) = *(int*)(src + (IntPtr)2 * 4);
				*(int*)(dest + (IntPtr)3 * 4) = *(int*)(src + (IntPtr)3 * 4);
				dest += 16;
				src += 16;
				size -= 16;
			}
			while (size >= 4)
			{
				*(int*)dest = *(int*)src;
				dest += 4;
				src += 4;
				size -= 4;
			}
			while (size > 0)
			{
				*dest = *src;
				dest++;
				src++;
				size--;
			}
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x00053724 File Offset: 0x00051924
		internal unsafe static void memcpy2(byte* dest, byte* src, int size)
		{
			while (size >= 8)
			{
				*(short*)dest = *(short*)src;
				*(short*)(dest + 2) = *(short*)(src + 2);
				*(short*)(dest + (IntPtr)2 * 2) = *(short*)(src + (IntPtr)2 * 2);
				*(short*)(dest + (IntPtr)3 * 2) = *(short*)(src + (IntPtr)3 * 2);
				dest += 8;
				src += 8;
				size -= 8;
			}
			while (size >= 2)
			{
				*(short*)dest = *(short*)src;
				dest += 2;
				src += 2;
				size -= 2;
			}
			if (size > 0)
			{
				*dest = *src;
			}
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00053790 File Offset: 0x00051990
		private unsafe static void memcpy1(byte* dest, byte* src, int size)
		{
			while (size >= 8)
			{
				*dest = *src;
				dest[1] = src[1];
				dest[2] = src[2];
				dest[3] = src[3];
				dest[4] = src[4];
				dest[5] = src[5];
				dest[6] = src[6];
				dest[7] = src[7];
				dest += 8;
				src += 8;
				size -= 8;
			}
			while (size >= 2)
			{
				*dest = *src;
				dest[1] = src[1];
				dest += 2;
				src += 2;
				size -= 2;
			}
			if (size > 0)
			{
				*dest = *src;
			}
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x00053818 File Offset: 0x00051A18
		internal unsafe static void Memcpy(byte* dest, byte* src, int len)
		{
			if (len > 32)
			{
				long num = (long)((ulong)(*dest));
				Interlocked.Read(ref num);
				Buffer.InternalMemcpy(dest, src, len);
				return;
			}
			if (((dest | src) & 3) != 0)
			{
				if ((dest & 1) != 0 && (src & 1) != 0 && len >= 1)
				{
					*dest = *src;
					dest++;
					src++;
					len--;
				}
				if ((dest & 2) != 0 && (src & 2) != 0 && len >= 2)
				{
					*(short*)dest = *(short*)src;
					dest += 2;
					src += 2;
					len -= 2;
				}
				if (((dest | src) & 1) != 0)
				{
					Buffer.memcpy1(dest, src, len);
					return;
				}
				if (((dest | src) & 2) != 0)
				{
					Buffer.memcpy2(dest, src, len);
					return;
				}
			}
			Buffer.memcpy4(dest, src, len);
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x000538BA File Offset: 0x00051ABA
		internal unsafe static void Memmove(byte* dest, byte* src, uint len)
		{
			if ((ulong)(dest - src) >= (ulong)len && (ulong)(src - dest) >= (ulong)len)
			{
				Buffer.Memcpy(dest, src, (int)len);
				return;
			}
			RuntimeImports.Memmove(dest, src, len);
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x000538E0 File Offset: 0x00051AE0
		internal unsafe static void Memmove<T>(ref T destination, ref T source, ulong elementCount)
		{
			if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				fixed (byte* ptr = Unsafe.As<T, byte>(ref destination))
				{
					byte* ptr2 = ptr;
					fixed (byte* ptr3 = Unsafe.As<T, byte>(ref source))
					{
						byte* ptr4 = ptr3;
						Buffer.Memmove(ptr2, ptr4, (uint)elementCount * (uint)Unsafe.SizeOf<T>());
						ptr = null;
					}
					return;
				}
			}
			fixed (byte* ptr3 = Unsafe.As<T, byte>(ref destination))
			{
				byte* ptr5 = ptr3;
				fixed (byte* ptr = Unsafe.As<T, byte>(ref source))
				{
					byte* ptr6 = ptr;
					RuntimeImports.Memmove_wbarrier(ptr5, ptr6, (uint)elementCount, typeof(T).TypeHandle.Value);
					ptr3 = null;
				}
				return;
			}
		}
	}
}
