using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Properties;
using UnityEngine;

namespace Unity.Collections
{
	// Token: 0x02000071 RID: 113
	[GenerateTestsForBurstCompatibility]
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 128)]
	public struct FixedString128Bytes : INativeList<byte>, IIndexable<byte>, IUTF8Bytes, IComparable<string>, IEquatable<string>, IComparable<FixedString32Bytes>, IEquatable<FixedString32Bytes>, IComparable<FixedString64Bytes>, IEquatable<FixedString64Bytes>, IComparable<FixedString128Bytes>, IEquatable<FixedString128Bytes>, IComparable<FixedString512Bytes>, IEquatable<FixedString512Bytes>, IComparable<FixedString4096Bytes>, IEquatable<FixedString4096Bytes>
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x0000B465 File Offset: 0x00009665
		public static int UTF8MaxLengthInBytes
		{
			get
			{
				return 125;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x0000B469 File Offset: 0x00009669
		[CreateProperty]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public string Value
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000B478 File Offset: 0x00009678
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe readonly byte* GetUnsafePtr()
		{
			fixed (FixedBytes126* ptr = &this.bytes)
			{
				return (byte*)ptr;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000B490 File Offset: 0x00009690
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x0000B498 File Offset: 0x00009698
		public unsafe int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return (int)this.utf8LengthInBytes;
			}
			set
			{
				this.utf8LengthInBytes = (ushort)value;
				this.GetUnsafePtr()[this.utf8LengthInBytes] = 0;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000B465 File Offset: 0x00009665
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return 125;
			}
			set
			{
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000B4B4 File Offset: 0x000096B4
		public unsafe bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory)
		{
			if (newLength < 0 || newLength > 125)
			{
				return false;
			}
			if (newLength == (int)this.utf8LengthInBytes)
			{
				return true;
			}
			if (clearOptions == NativeArrayOptions.ClearMemory)
			{
				if (newLength > (int)this.utf8LengthInBytes)
				{
					UnsafeUtility.MemClear((void*)(this.GetUnsafePtr() + this.utf8LengthInBytes), (long)(newLength - (int)this.utf8LengthInBytes));
				}
				else
				{
					UnsafeUtility.MemClear((void*)(this.GetUnsafePtr() + newLength), (long)((int)this.utf8LengthInBytes - newLength));
				}
			}
			this.utf8LengthInBytes = (ushort)newLength;
			this.GetUnsafePtr()[this.utf8LengthInBytes] = 0;
			return true;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x0000B52F File Offset: 0x0000972F
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.utf8LengthInBytes == 0;
			}
		}

		// Token: 0x17000094 RID: 148
		public unsafe byte this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.GetUnsafePtr()[index];
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.GetUnsafePtr()[index] = value;
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000B551 File Offset: 0x00009751
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref byte ElementAt(int index)
		{
			return ref this.GetUnsafePtr()[index];
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000B55B File Offset: 0x0000975B
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000B564 File Offset: 0x00009764
		public void Add(in byte value)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = value;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000B58A File Offset: 0x0000978A
		public FixedString128Bytes.Enumerator GetEnumerator()
		{
			return new FixedString128Bytes.Enumerator(this);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000B597 File Offset: 0x00009797
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public int CompareTo(string other)
		{
			return this.ToString().CompareTo(other);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000B5AC File Offset: 0x000097AC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public unsafe bool Equals(string other)
		{
			int num = (int)this.utf8LengthInBytes;
			int length = other.Length;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in this.bytes);
			char* ptr2 = other;
			if (ptr2 != null)
			{
				ptr2 += RuntimeHelpers.OffsetToStringData / 2;
			}
			return UTF8ArrayUnsafeUtility.StrCmp(ptr, num, ptr2, length) == 0;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000B5F1 File Offset: 0x000097F1
		public ref FixedList128Bytes<byte> AsFixedList()
		{
			return UnsafeUtility.AsRef<FixedList128Bytes<byte>>(UnsafeUtility.AddressOf<FixedString128Bytes>(ref this));
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000B5FE File Offset: 0x000097FE
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public FixedString128Bytes(string source)
		{
			this = default(FixedString128Bytes);
			this.Initialize(source);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000B60F File Offset: 0x0000980F
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		internal CopyError Initialize(string source)
		{
			return (ref this).CopyFromTruncated<FixedString128Bytes>(source);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000B618 File Offset: 0x00009818
		public FixedString128Bytes(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString128Bytes);
			this.Initialize(rune, count);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000B62A File Offset: 0x0000982A
		internal FormatError Initialize(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString128Bytes);
			return (ref this).Append<FixedString128Bytes>(rune, count);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000B63C File Offset: 0x0000983C
		internal unsafe FormatError Initialize(byte* srcBytes, int srcLength)
		{
			this.bytes = default(FixedBytes126);
			this.utf8LengthInBytes = 0;
			int num = 0;
			FormatError formatError = UTF8ArrayUnsafeUtility.AppendUTF8Bytes(this.GetUnsafePtr(), ref num, 125, srcBytes, srcLength);
			if (formatError != FormatError.None)
			{
				return formatError;
			}
			this.Length = num;
			return FormatError.None;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000B67D File Offset: 0x0000987D
		public FixedString128Bytes(NativeText.ReadOnly other)
		{
			this = default(FixedString128Bytes);
			this.Initialize(other.GetUnsafePtr(), other.Length);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000B69C File Offset: 0x0000989C
		public FixedString128Bytes(in UnsafeText other)
		{
			this = default(FixedString128Bytes);
			UnsafeText unsafeText = other;
			this.Initialize(unsafeText.GetUnsafePtr(), other.Length);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000B6CB File Offset: 0x000098CB
		public int CompareTo(FixedString32Bytes other)
		{
			return (ref this).CompareTo<FixedString128Bytes, FixedString32Bytes>(in other);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000B6D5 File Offset: 0x000098D5
		public FixedString128Bytes(in FixedString32Bytes other)
		{
			this = default(FixedString128Bytes);
			this.Initialize(in other);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000B6E6 File Offset: 0x000098E6
		internal unsafe FormatError Initialize(in FixedString32Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000B700 File Offset: 0x00009900
		public unsafe static bool operator ==(in FixedString128Bytes a, in FixedString32Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000B73C File Offset: 0x0000993C
		public static bool operator !=(in FixedString128Bytes a, in FixedString32Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000B748 File Offset: 0x00009948
		public bool Equals(FixedString32Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000B752 File Offset: 0x00009952
		public int CompareTo(FixedString64Bytes other)
		{
			return (ref this).CompareTo<FixedString128Bytes, FixedString64Bytes>(in other);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000B75C File Offset: 0x0000995C
		public FixedString128Bytes(in FixedString64Bytes other)
		{
			this = default(FixedString128Bytes);
			this.Initialize(in other);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000B76D File Offset: 0x0000996D
		internal unsafe FormatError Initialize(in FixedString64Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000B788 File Offset: 0x00009988
		public unsafe static bool operator ==(in FixedString128Bytes a, in FixedString64Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000B7C4 File Offset: 0x000099C4
		public static bool operator !=(in FixedString128Bytes a, in FixedString64Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000B7D0 File Offset: 0x000099D0
		public bool Equals(FixedString64Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000B7DA File Offset: 0x000099DA
		public int CompareTo(FixedString128Bytes other)
		{
			return (ref this).CompareTo<FixedString128Bytes, FixedString128Bytes>(in other);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000B7E4 File Offset: 0x000099E4
		public FixedString128Bytes(in FixedString128Bytes other)
		{
			this = default(FixedString128Bytes);
			this.Initialize(in other);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000B7F5 File Offset: 0x000099F5
		internal unsafe FormatError Initialize(in FixedString128Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000B810 File Offset: 0x00009A10
		public unsafe static bool operator ==(in FixedString128Bytes a, in FixedString128Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000B84C File Offset: 0x00009A4C
		public static bool operator !=(in FixedString128Bytes a, in FixedString128Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000B858 File Offset: 0x00009A58
		public bool Equals(FixedString128Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000B862 File Offset: 0x00009A62
		public int CompareTo(FixedString512Bytes other)
		{
			return (ref this).CompareTo<FixedString128Bytes, FixedString512Bytes>(in other);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000B86C File Offset: 0x00009A6C
		public FixedString128Bytes(in FixedString512Bytes other)
		{
			this = default(FixedString128Bytes);
			this.Initialize(in other);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000B87D File Offset: 0x00009A7D
		internal unsafe FormatError Initialize(in FixedString512Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000B898 File Offset: 0x00009A98
		public unsafe static bool operator ==(in FixedString128Bytes a, in FixedString512Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000B8D4 File Offset: 0x00009AD4
		public static bool operator !=(in FixedString128Bytes a, in FixedString512Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000B8E0 File Offset: 0x00009AE0
		public bool Equals(FixedString512Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000B8EA File Offset: 0x00009AEA
		public static implicit operator FixedString512Bytes(in FixedString128Bytes fs)
		{
			return new FixedString512Bytes(in fs);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000B8F2 File Offset: 0x00009AF2
		public int CompareTo(FixedString4096Bytes other)
		{
			return (ref this).CompareTo<FixedString128Bytes, FixedString4096Bytes>(in other);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000B8FC File Offset: 0x00009AFC
		public FixedString128Bytes(in FixedString4096Bytes other)
		{
			this = default(FixedString128Bytes);
			this.Initialize(in other);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000B90D File Offset: 0x00009B0D
		internal unsafe FormatError Initialize(in FixedString4096Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000B928 File Offset: 0x00009B28
		public unsafe static bool operator ==(in FixedString128Bytes a, in FixedString4096Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000B964 File Offset: 0x00009B64
		public static bool operator !=(in FixedString128Bytes a, in FixedString4096Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000B970 File Offset: 0x00009B70
		public bool Equals(FixedString4096Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000B97A File Offset: 0x00009B7A
		public static implicit operator FixedString4096Bytes(in FixedString128Bytes fs)
		{
			return new FixedString4096Bytes(in fs);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000B982 File Offset: 0x00009B82
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static implicit operator FixedString128Bytes(string b)
		{
			return new FixedString128Bytes(b);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000B98A File Offset: 0x00009B8A
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public override string ToString()
		{
			return (ref this).ConvertToString<FixedString128Bytes>();
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000B992 File Offset: 0x00009B92
		public override int GetHashCode()
		{
			return (ref this).ComputeHashCode<FixedString128Bytes>();
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000B99C File Offset: 0x00009B9C
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			string text = obj as string;
			if (text != null)
			{
				return this.Equals(text);
			}
			if (obj is FixedString32Bytes)
			{
				FixedString32Bytes fixedString32Bytes = (FixedString32Bytes)obj;
				return this.Equals(fixedString32Bytes);
			}
			if (obj is FixedString64Bytes)
			{
				FixedString64Bytes fixedString64Bytes = (FixedString64Bytes)obj;
				return this.Equals(fixedString64Bytes);
			}
			if (obj is FixedString128Bytes)
			{
				FixedString128Bytes fixedString128Bytes = (FixedString128Bytes)obj;
				return this.Equals(fixedString128Bytes);
			}
			if (obj is FixedString512Bytes)
			{
				FixedString512Bytes fixedString512Bytes = (FixedString512Bytes)obj;
				return this.Equals(fixedString512Bytes);
			}
			if (obj is FixedString4096Bytes)
			{
				FixedString4096Bytes fixedString4096Bytes = (FixedString4096Bytes)obj;
				return this.Equals(fixedString4096Bytes);
			}
			return false;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000BA38 File Offset: 0x00009C38
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckIndexInRange(int index)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} must be positive.", index));
			}
			if (index >= (int)this.utf8LengthInBytes)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range in FixedString128Bytes of '{1}' Length.", index, this.utf8LengthInBytes));
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000BA89 File Offset: 0x00009C89
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckLengthInRange(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} must be positive.", length));
			}
			if (length > 125)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} is out of range in FixedString128Bytes of '{1}' Capacity.", length, 125));
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000BAC7 File Offset: 0x00009CC7
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckCapacityInRange(int capacity)
		{
			if (capacity > 125)
			{
				throw new ArgumentOutOfRangeException(string.Format("Capacity {0} must be lower than {1}.", capacity, 125));
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000BAEB File Offset: 0x00009CEB
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckCopyError(CopyError error, string source)
		{
			if (error != CopyError.None)
			{
				throw new ArgumentException(string.Format("FixedString128Bytes: {0} while copying \"{1}\"", error, source));
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000AC9F File Offset: 0x00008E9F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckFormatError(FormatError error)
		{
			if (error != FormatError.None)
			{
				throw new ArgumentException("Source is too long to fit into fixed string of this size");
			}
		}

		// Token: 0x04000257 RID: 599
		internal const ushort utf8MaxLengthInBytes = 125;

		// Token: 0x04000258 RID: 600
		[SerializeField]
		internal ushort utf8LengthInBytes;

		// Token: 0x04000259 RID: 601
		[SerializeField]
		internal FixedBytes126 bytes;

		// Token: 0x02000072 RID: 114
		public struct Enumerator : IEnumerator
		{
			// Token: 0x06000487 RID: 1159 RVA: 0x0000BB07 File Offset: 0x00009D07
			public Enumerator(FixedString128Bytes other)
			{
				this.target = other;
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000489 RID: 1161 RVA: 0x0000BB23 File Offset: 0x00009D23
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				if (this.offset >= this.target.Length)
				{
					return false;
				}
				Unicode.Utf8ToUcs(out this.current, this.target.GetUnsafePtr(), ref this.offset, this.target.Length);
				return true;
			}

			// Token: 0x0600048A RID: 1162 RVA: 0x0000BB63 File Offset: 0x00009D63
			public void Reset()
			{
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x0600048B RID: 1163 RVA: 0x0000BB78 File Offset: 0x00009D78
			public Unicode.Rune Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.current;
				}
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x0600048C RID: 1164 RVA: 0x0000BB80 File Offset: 0x00009D80
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0400025A RID: 602
			private FixedString128Bytes target;

			// Token: 0x0400025B RID: 603
			private int offset;

			// Token: 0x0400025C RID: 604
			private Unicode.Rune current;
		}
	}
}
