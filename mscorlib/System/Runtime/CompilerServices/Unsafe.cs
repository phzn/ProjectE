using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000855 RID: 2133
	internal static class Unsafe
	{
		// Token: 0x06004711 RID: 18193 RVA: 0x000E7EFE File Offset: 0x000E60FE
		public static ref T Add<T>(ref T source, int elementOffset)
		{
			return (ref source) + (IntPtr)elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x000E7F0B File Offset: 0x000E610B
		public static ref T Add<T>(ref T source, IntPtr elementOffset)
		{
			return (ref source) + elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x000E7EFE File Offset: 0x000E60FE
		public unsafe static void* Add<T>(void* source, int elementOffset)
		{
			return (void*)((byte*)source + (IntPtr)elementOffset * (IntPtr)sizeof(T));
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x000E7F17 File Offset: 0x000E6117
		public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
		{
			return (ref source) + byteOffset;
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x0002842A File Offset: 0x0002662A
		public static bool AreSame<T>(ref T left, ref T right)
		{
			return (ref left) == (ref right);
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x0000270D File Offset: 0x0000090D
		public static T As<T>(object o) where T : class
		{
			return o;
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x0000270D File Offset: 0x0000090D
		public static ref TTo As<TFrom, TTo>(ref TFrom source)
		{
			return ref source;
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x000E7F1C File Offset: 0x000E611C
		public unsafe static void* AsPointer<T>(ref T value)
		{
			return (void*)(&value);
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x0000270D File Offset: 0x0000090D
		public unsafe static ref T AsRef<T>(void* source)
		{
			return ref *(T*)source;
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0000270D File Offset: 0x0000090D
		public static ref T AsRef<T>(in T source)
		{
			return ref source;
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x000E7F20 File Offset: 0x000E6120
		public static IntPtr ByteOffset<T>(ref T origin, ref T target)
		{
			return (ref target) - (ref origin);
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x000E7F25 File Offset: 0x000E6125
		public static void CopyBlock(ref byte destination, ref byte source, uint byteCount)
		{
			cpblk(ref destination, ref source, byteCount);
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x000E7F2C File Offset: 0x000E612C
		public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
		{
			initblk(ref startAddress, value, byteCount);
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x000E7F2C File Offset: 0x000E612C
		public unsafe static void InitBlockUnaligned(void* startAddress, byte value, uint byteCount)
		{
			initblk(startAddress, value, byteCount);
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x000E7F36 File Offset: 0x000E6136
		public unsafe static T Read<T>(void* source)
		{
			return *(T*)source;
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x000E7F3E File Offset: 0x000E613E
		public unsafe static T ReadUnaligned<T>(void* source)
		{
			return *(T*)source;
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x000E7F3E File Offset: 0x000E613E
		public static T ReadUnaligned<T>(ref byte source)
		{
			return source;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x000E7F49 File Offset: 0x000E6149
		public static int SizeOf<T>()
		{
			return sizeof(T);
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x000E7F51 File Offset: 0x000E6151
		public static ref T Subtract<T>(ref T source, int elementOffset)
		{
			return (ref source) - (IntPtr)elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x000E7F5E File Offset: 0x000E615E
		public static void WriteUnaligned<T>(ref byte destination, T value)
		{
			destination = value;
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x000E7F5E File Offset: 0x000E615E
		public unsafe static void WriteUnaligned<T>(void* destination, T value)
		{
			*(T*)destination = value;
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x000E7F6A File Offset: 0x000E616A
		public static bool IsAddressGreaterThan<T>(ref T left, ref T right)
		{
			return (ref left) != (ref right);
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x000E7F70 File Offset: 0x000E6170
		public static bool IsAddressLessThan<T>(ref T left, ref T right)
		{
			return (ref left) < (ref right);
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x000E7F76 File Offset: 0x000E6176
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ref T AddByteOffset<T>(ref T source, ulong byteOffset)
		{
			return Unsafe.AddByteOffset<T>(ref source, (IntPtr)byteOffset);
		}
	}
}
