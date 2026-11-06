using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers.Binary
{
	// Token: 0x02000AF1 RID: 2801
	public static class BinaryPrimitives
	{
		// Token: 0x060063C6 RID: 25542 RVA: 0x0000270D File Offset: 0x0000090D
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte ReverseEndianness(sbyte value)
		{
			return value;
		}

		// Token: 0x060063C7 RID: 25543 RVA: 0x0014E317 File Offset: 0x0014C517
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ReverseEndianness(short value)
		{
			return (short)(((int)(value & 255) << 8) | (((int)value & 65280) >> 8));
		}

		// Token: 0x060063C8 RID: 25544 RVA: 0x0014E32D File Offset: 0x0014C52D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReverseEndianness(int value)
		{
			return (int)BinaryPrimitives.ReverseEndianness((uint)value);
		}

		// Token: 0x060063C9 RID: 25545 RVA: 0x0014E335 File Offset: 0x0014C535
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ReverseEndianness(long value)
		{
			return (long)BinaryPrimitives.ReverseEndianness((ulong)value);
		}

		// Token: 0x060063CA RID: 25546 RVA: 0x0000270D File Offset: 0x0000090D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte ReverseEndianness(byte value)
		{
			return value;
		}

		// Token: 0x060063CB RID: 25547 RVA: 0x0014E33D File Offset: 0x0014C53D
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ReverseEndianness(ushort value)
		{
			return (ushort)((value >> 8) + ((int)value << 8));
		}

		// Token: 0x060063CC RID: 25548 RVA: 0x0014E348 File Offset: 0x0014C548
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ReverseEndianness(uint value)
		{
			uint num = value & 16711935U;
			uint num2 = value & 4278255360U;
			return ((num >> 8) | (num << 24)) + ((num2 << 8) | (num2 >> 24));
		}

		// Token: 0x060063CD RID: 25549 RVA: 0x0014E376 File Offset: 0x0014C576
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ReverseEndianness(ulong value)
		{
			return ((ulong)BinaryPrimitives.ReverseEndianness((uint)value) << 32) + (ulong)BinaryPrimitives.ReverseEndianness((uint)(value >> 32));
		}

		// Token: 0x060063CE RID: 25550 RVA: 0x0014E390 File Offset: 0x0014C590
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ReadInt16BigEndian(ReadOnlySpan<byte> source)
		{
			short num = MemoryMarshal.Read<short>(source);
			if (BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063CF RID: 25551 RVA: 0x0014E3B4 File Offset: 0x0014C5B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadInt32BigEndian(ReadOnlySpan<byte> source)
		{
			int num = MemoryMarshal.Read<int>(source);
			if (BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063D0 RID: 25552 RVA: 0x0014E3D8 File Offset: 0x0014C5D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ReadInt64BigEndian(ReadOnlySpan<byte> source)
		{
			long num = MemoryMarshal.Read<long>(source);
			if (BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063D1 RID: 25553 RVA: 0x0014E3FC File Offset: 0x0014C5FC
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ReadUInt16BigEndian(ReadOnlySpan<byte> source)
		{
			ushort num = MemoryMarshal.Read<ushort>(source);
			if (BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063D2 RID: 25554 RVA: 0x0014E420 File Offset: 0x0014C620
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ReadUInt32BigEndian(ReadOnlySpan<byte> source)
		{
			uint num = MemoryMarshal.Read<uint>(source);
			if (BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063D3 RID: 25555 RVA: 0x0014E444 File Offset: 0x0014C644
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ReadUInt64BigEndian(ReadOnlySpan<byte> source)
		{
			ulong num = MemoryMarshal.Read<ulong>(source);
			if (BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063D4 RID: 25556 RVA: 0x0014E467 File Offset: 0x0014C667
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadInt16BigEndian(ReadOnlySpan<byte> source, out short value)
		{
			bool flag = MemoryMarshal.TryRead<short>(source, out value);
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063D5 RID: 25557 RVA: 0x0014E480 File Offset: 0x0014C680
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadInt32BigEndian(ReadOnlySpan<byte> source, out int value)
		{
			bool flag = MemoryMarshal.TryRead<int>(source, out value);
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063D6 RID: 25558 RVA: 0x0014E499 File Offset: 0x0014C699
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadInt64BigEndian(ReadOnlySpan<byte> source, out long value)
		{
			bool flag = MemoryMarshal.TryRead<long>(source, out value);
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063D7 RID: 25559 RVA: 0x0014E4B2 File Offset: 0x0014C6B2
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadUInt16BigEndian(ReadOnlySpan<byte> source, out ushort value)
		{
			bool flag = MemoryMarshal.TryRead<ushort>(source, out value);
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063D8 RID: 25560 RVA: 0x0014E4CB File Offset: 0x0014C6CB
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadUInt32BigEndian(ReadOnlySpan<byte> source, out uint value)
		{
			bool flag = MemoryMarshal.TryRead<uint>(source, out value);
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063D9 RID: 25561 RVA: 0x0014E4E4 File Offset: 0x0014C6E4
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadUInt64BigEndian(ReadOnlySpan<byte> source, out ulong value)
		{
			bool flag = MemoryMarshal.TryRead<ulong>(source, out value);
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063DA RID: 25562 RVA: 0x0014E500 File Offset: 0x0014C700
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short ReadInt16LittleEndian(ReadOnlySpan<byte> source)
		{
			short num = MemoryMarshal.Read<short>(source);
			if (!BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063DB RID: 25563 RVA: 0x0014E524 File Offset: 0x0014C724
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ReadInt32LittleEndian(ReadOnlySpan<byte> source)
		{
			int num = MemoryMarshal.Read<int>(source);
			if (!BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063DC RID: 25564 RVA: 0x0014E548 File Offset: 0x0014C748
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ReadInt64LittleEndian(ReadOnlySpan<byte> source)
		{
			long num = MemoryMarshal.Read<long>(source);
			if (!BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063DD RID: 25565 RVA: 0x0014E56C File Offset: 0x0014C76C
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ReadUInt16LittleEndian(ReadOnlySpan<byte> source)
		{
			ushort num = MemoryMarshal.Read<ushort>(source);
			if (!BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063DE RID: 25566 RVA: 0x0014E590 File Offset: 0x0014C790
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ReadUInt32LittleEndian(ReadOnlySpan<byte> source)
		{
			uint num = MemoryMarshal.Read<uint>(source);
			if (!BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063DF RID: 25567 RVA: 0x0014E5B4 File Offset: 0x0014C7B4
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong ReadUInt64LittleEndian(ReadOnlySpan<byte> source)
		{
			ulong num = MemoryMarshal.Read<ulong>(source);
			if (!BitConverter.IsLittleEndian)
			{
				num = BinaryPrimitives.ReverseEndianness(num);
			}
			return num;
		}

		// Token: 0x060063E0 RID: 25568 RVA: 0x0014E5D7 File Offset: 0x0014C7D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadInt16LittleEndian(ReadOnlySpan<byte> source, out short value)
		{
			bool flag = MemoryMarshal.TryRead<short>(source, out value);
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063E1 RID: 25569 RVA: 0x0014E5F0 File Offset: 0x0014C7F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadInt32LittleEndian(ReadOnlySpan<byte> source, out int value)
		{
			bool flag = MemoryMarshal.TryRead<int>(source, out value);
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063E2 RID: 25570 RVA: 0x0014E609 File Offset: 0x0014C809
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadInt64LittleEndian(ReadOnlySpan<byte> source, out long value)
		{
			bool flag = MemoryMarshal.TryRead<long>(source, out value);
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063E3 RID: 25571 RVA: 0x0014E622 File Offset: 0x0014C822
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadUInt16LittleEndian(ReadOnlySpan<byte> source, out ushort value)
		{
			bool flag = MemoryMarshal.TryRead<ushort>(source, out value);
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063E4 RID: 25572 RVA: 0x0014E63B File Offset: 0x0014C83B
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadUInt32LittleEndian(ReadOnlySpan<byte> source, out uint value)
		{
			bool flag = MemoryMarshal.TryRead<uint>(source, out value);
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063E5 RID: 25573 RVA: 0x0014E654 File Offset: 0x0014C854
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryReadUInt64LittleEndian(ReadOnlySpan<byte> source, out ulong value)
		{
			bool flag = MemoryMarshal.TryRead<ulong>(source, out value);
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return flag;
		}

		// Token: 0x060063E6 RID: 25574 RVA: 0x0014E66D File Offset: 0x0014C86D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteInt16BigEndian(Span<byte> destination, short value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<short>(destination, ref value);
		}

		// Token: 0x060063E7 RID: 25575 RVA: 0x0014E686 File Offset: 0x0014C886
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteInt32BigEndian(Span<byte> destination, int value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<int>(destination, ref value);
		}

		// Token: 0x060063E8 RID: 25576 RVA: 0x0014E69F File Offset: 0x0014C89F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteInt64BigEndian(Span<byte> destination, long value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<long>(destination, ref value);
		}

		// Token: 0x060063E9 RID: 25577 RVA: 0x0014E6B8 File Offset: 0x0014C8B8
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt16BigEndian(Span<byte> destination, ushort value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<ushort>(destination, ref value);
		}

		// Token: 0x060063EA RID: 25578 RVA: 0x0014E6D1 File Offset: 0x0014C8D1
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt32BigEndian(Span<byte> destination, uint value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<uint>(destination, ref value);
		}

		// Token: 0x060063EB RID: 25579 RVA: 0x0014E6EA File Offset: 0x0014C8EA
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt64BigEndian(Span<byte> destination, ulong value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<ulong>(destination, ref value);
		}

		// Token: 0x060063EC RID: 25580 RVA: 0x0014E703 File Offset: 0x0014C903
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteInt16BigEndian(Span<byte> destination, short value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<short>(destination, ref value);
		}

		// Token: 0x060063ED RID: 25581 RVA: 0x0014E71C File Offset: 0x0014C91C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteInt32BigEndian(Span<byte> destination, int value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<int>(destination, ref value);
		}

		// Token: 0x060063EE RID: 25582 RVA: 0x0014E735 File Offset: 0x0014C935
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteInt64BigEndian(Span<byte> destination, long value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<long>(destination, ref value);
		}

		// Token: 0x060063EF RID: 25583 RVA: 0x0014E74E File Offset: 0x0014C94E
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteUInt16BigEndian(Span<byte> destination, ushort value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<ushort>(destination, ref value);
		}

		// Token: 0x060063F0 RID: 25584 RVA: 0x0014E767 File Offset: 0x0014C967
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteUInt32BigEndian(Span<byte> destination, uint value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<uint>(destination, ref value);
		}

		// Token: 0x060063F1 RID: 25585 RVA: 0x0014E780 File Offset: 0x0014C980
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteUInt64BigEndian(Span<byte> destination, ulong value)
		{
			if (BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<ulong>(destination, ref value);
		}

		// Token: 0x060063F2 RID: 25586 RVA: 0x0014E799 File Offset: 0x0014C999
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteInt16LittleEndian(Span<byte> destination, short value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<short>(destination, ref value);
		}

		// Token: 0x060063F3 RID: 25587 RVA: 0x0014E7B2 File Offset: 0x0014C9B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteInt32LittleEndian(Span<byte> destination, int value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<int>(destination, ref value);
		}

		// Token: 0x060063F4 RID: 25588 RVA: 0x0014E7CB File Offset: 0x0014C9CB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteInt64LittleEndian(Span<byte> destination, long value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<long>(destination, ref value);
		}

		// Token: 0x060063F5 RID: 25589 RVA: 0x0014E7E4 File Offset: 0x0014C9E4
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt16LittleEndian(Span<byte> destination, ushort value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<ushort>(destination, ref value);
		}

		// Token: 0x060063F6 RID: 25590 RVA: 0x0014E7FD File Offset: 0x0014C9FD
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt32LittleEndian(Span<byte> destination, uint value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<uint>(destination, ref value);
		}

		// Token: 0x060063F7 RID: 25591 RVA: 0x0014E816 File Offset: 0x0014CA16
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt64LittleEndian(Span<byte> destination, ulong value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			MemoryMarshal.Write<ulong>(destination, ref value);
		}

		// Token: 0x060063F8 RID: 25592 RVA: 0x0014E82F File Offset: 0x0014CA2F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteInt16LittleEndian(Span<byte> destination, short value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<short>(destination, ref value);
		}

		// Token: 0x060063F9 RID: 25593 RVA: 0x0014E848 File Offset: 0x0014CA48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteInt32LittleEndian(Span<byte> destination, int value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<int>(destination, ref value);
		}

		// Token: 0x060063FA RID: 25594 RVA: 0x0014E861 File Offset: 0x0014CA61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteInt64LittleEndian(Span<byte> destination, long value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<long>(destination, ref value);
		}

		// Token: 0x060063FB RID: 25595 RVA: 0x0014E87A File Offset: 0x0014CA7A
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteUInt16LittleEndian(Span<byte> destination, ushort value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<ushort>(destination, ref value);
		}

		// Token: 0x060063FC RID: 25596 RVA: 0x0014E893 File Offset: 0x0014CA93
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteUInt32LittleEndian(Span<byte> destination, uint value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<uint>(destination, ref value);
		}

		// Token: 0x060063FD RID: 25597 RVA: 0x0014E8AC File Offset: 0x0014CAAC
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWriteUInt64LittleEndian(Span<byte> destination, ulong value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				value = BinaryPrimitives.ReverseEndianness(value);
			}
			return MemoryMarshal.TryWrite<ulong>(destination, ref value);
		}
	}
}
