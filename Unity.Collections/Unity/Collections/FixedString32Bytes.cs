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
	// Token: 0x0200006B RID: 107
	[GenerateTestsForBurstCompatibility]
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 32)]
	public struct FixedString32Bytes : INativeList<byte>, IIndexable<byte>, IUTF8Bytes, IComparable<string>, IEquatable<string>, IComparable<FixedString32Bytes>, IEquatable<FixedString32Bytes>, IComparable<FixedString64Bytes>, IEquatable<FixedString64Bytes>, IComparable<FixedString128Bytes>, IEquatable<FixedString128Bytes>, IComparable<FixedString512Bytes>, IEquatable<FixedString512Bytes>, IComparable<FixedString4096Bytes>, IEquatable<FixedString4096Bytes>
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000A5EE File Offset: 0x000087EE
		public static int UTF8MaxLengthInBytes
		{
			get
			{
				return 29;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000A5F2 File Offset: 0x000087F2
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

		// Token: 0x060003B4 RID: 948 RVA: 0x0000A600 File Offset: 0x00008800
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe readonly byte* GetUnsafePtr()
		{
			fixed (FixedBytes30* ptr = &this.bytes)
			{
				return (byte*)ptr;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000A618 File Offset: 0x00008818
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0000A620 File Offset: 0x00008820
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000A5EE File Offset: 0x000087EE
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return 29;
			}
			set
			{
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000A63C File Offset: 0x0000883C
		public unsafe bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory)
		{
			if (newLength < 0 || newLength > 29)
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0000A6B7 File Offset: 0x000088B7
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.utf8LengthInBytes == 0;
			}
		}

		// Token: 0x17000084 RID: 132
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

		// Token: 0x060003BD RID: 957 RVA: 0x0000A6D9 File Offset: 0x000088D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref byte ElementAt(int index)
		{
			return ref this.GetUnsafePtr()[index];
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000A6E3 File Offset: 0x000088E3
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000A6EC File Offset: 0x000088EC
		public void Add(in byte value)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = value;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000A712 File Offset: 0x00008912
		public FixedString32Bytes.Enumerator GetEnumerator()
		{
			return new FixedString32Bytes.Enumerator(this);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000A71F File Offset: 0x0000891F
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public int CompareTo(string other)
		{
			return this.ToString().CompareTo(other);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000A734 File Offset: 0x00008934
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public unsafe bool Equals(string other)
		{
			int num = (int)this.utf8LengthInBytes;
			int length = other.Length;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in this.bytes);
			char* ptr2 = other;
			if (ptr2 != null)
			{
				ptr2 += RuntimeHelpers.OffsetToStringData / 2;
			}
			return UTF8ArrayUnsafeUtility.StrCmp(ptr, num, ptr2, length) == 0;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000A779 File Offset: 0x00008979
		public ref FixedList32Bytes<byte> AsFixedList()
		{
			return UnsafeUtility.AsRef<FixedList32Bytes<byte>>(UnsafeUtility.AddressOf<FixedString32Bytes>(ref this));
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000A786 File Offset: 0x00008986
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public FixedString32Bytes(string source)
		{
			this = default(FixedString32Bytes);
			this.Initialize(source);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000A797 File Offset: 0x00008997
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		internal CopyError Initialize(string source)
		{
			return (ref this).CopyFromTruncated<FixedString32Bytes>(source);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000A7A0 File Offset: 0x000089A0
		public FixedString32Bytes(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString32Bytes);
			this.Initialize(rune, count);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000A7B2 File Offset: 0x000089B2
		internal FormatError Initialize(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString32Bytes);
			return (ref this).Append<FixedString32Bytes>(rune, count);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000A7C4 File Offset: 0x000089C4
		internal unsafe FormatError Initialize(byte* srcBytes, int srcLength)
		{
			this.bytes = default(FixedBytes30);
			this.utf8LengthInBytes = 0;
			int num = 0;
			FormatError formatError = UTF8ArrayUnsafeUtility.AppendUTF8Bytes(this.GetUnsafePtr(), ref num, 29, srcBytes, srcLength);
			if (formatError != FormatError.None)
			{
				return formatError;
			}
			this.Length = num;
			return FormatError.None;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000A805 File Offset: 0x00008A05
		public FixedString32Bytes(NativeText.ReadOnly other)
		{
			this = default(FixedString32Bytes);
			this.Initialize(other.GetUnsafePtr(), other.Length);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000A824 File Offset: 0x00008A24
		public FixedString32Bytes(in UnsafeText other)
		{
			this = default(FixedString32Bytes);
			UnsafeText unsafeText = other;
			this.Initialize(unsafeText.GetUnsafePtr(), other.Length);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000A853 File Offset: 0x00008A53
		public int CompareTo(FixedString32Bytes other)
		{
			return (ref this).CompareTo<FixedString32Bytes, FixedString32Bytes>(in other);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000A85D File Offset: 0x00008A5D
		public FixedString32Bytes(in FixedString32Bytes other)
		{
			this = default(FixedString32Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000A86E File Offset: 0x00008A6E
		internal unsafe FormatError Initialize(in FixedString32Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000A888 File Offset: 0x00008A88
		public unsafe static bool operator ==(in FixedString32Bytes a, in FixedString32Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000A8C4 File Offset: 0x00008AC4
		public static bool operator !=(in FixedString32Bytes a, in FixedString32Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		public bool Equals(FixedString32Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000A8DA File Offset: 0x00008ADA
		public int CompareTo(FixedString64Bytes other)
		{
			return (ref this).CompareTo<FixedString32Bytes, FixedString64Bytes>(in other);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000A8E4 File Offset: 0x00008AE4
		public FixedString32Bytes(in FixedString64Bytes other)
		{
			this = default(FixedString32Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000A8F5 File Offset: 0x00008AF5
		internal unsafe FormatError Initialize(in FixedString64Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000A910 File Offset: 0x00008B10
		public unsafe static bool operator ==(in FixedString32Bytes a, in FixedString64Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000A94C File Offset: 0x00008B4C
		public static bool operator !=(in FixedString32Bytes a, in FixedString64Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000A958 File Offset: 0x00008B58
		public bool Equals(FixedString64Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000A962 File Offset: 0x00008B62
		public static implicit operator FixedString64Bytes(in FixedString32Bytes fs)
		{
			return new FixedString64Bytes(in fs);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000A96A File Offset: 0x00008B6A
		public int CompareTo(FixedString128Bytes other)
		{
			return (ref this).CompareTo<FixedString32Bytes, FixedString128Bytes>(in other);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000A974 File Offset: 0x00008B74
		public FixedString32Bytes(in FixedString128Bytes other)
		{
			this = default(FixedString32Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000A985 File Offset: 0x00008B85
		internal unsafe FormatError Initialize(in FixedString128Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000A9A0 File Offset: 0x00008BA0
		public unsafe static bool operator ==(in FixedString32Bytes a, in FixedString128Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000A9DC File Offset: 0x00008BDC
		public static bool operator !=(in FixedString32Bytes a, in FixedString128Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000A9E8 File Offset: 0x00008BE8
		public bool Equals(FixedString128Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000A9F2 File Offset: 0x00008BF2
		public static implicit operator FixedString128Bytes(in FixedString32Bytes fs)
		{
			return new FixedString128Bytes(in fs);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000A9FA File Offset: 0x00008BFA
		public int CompareTo(FixedString512Bytes other)
		{
			return (ref this).CompareTo<FixedString32Bytes, FixedString512Bytes>(in other);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000AA04 File Offset: 0x00008C04
		public FixedString32Bytes(in FixedString512Bytes other)
		{
			this = default(FixedString32Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000AA15 File Offset: 0x00008C15
		internal unsafe FormatError Initialize(in FixedString512Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000AA30 File Offset: 0x00008C30
		public unsafe static bool operator ==(in FixedString32Bytes a, in FixedString512Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000AA6C File Offset: 0x00008C6C
		public static bool operator !=(in FixedString32Bytes a, in FixedString512Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000AA78 File Offset: 0x00008C78
		public bool Equals(FixedString512Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000AA82 File Offset: 0x00008C82
		public static implicit operator FixedString512Bytes(in FixedString32Bytes fs)
		{
			return new FixedString512Bytes(in fs);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000AA8A File Offset: 0x00008C8A
		public int CompareTo(FixedString4096Bytes other)
		{
			return (ref this).CompareTo<FixedString32Bytes, FixedString4096Bytes>(in other);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000AA94 File Offset: 0x00008C94
		public FixedString32Bytes(in FixedString4096Bytes other)
		{
			this = default(FixedString32Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000AAA5 File Offset: 0x00008CA5
		internal unsafe FormatError Initialize(in FixedString4096Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000AAC0 File Offset: 0x00008CC0
		public unsafe static bool operator ==(in FixedString32Bytes a, in FixedString4096Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000AAFC File Offset: 0x00008CFC
		public static bool operator !=(in FixedString32Bytes a, in FixedString4096Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000AB08 File Offset: 0x00008D08
		public bool Equals(FixedString4096Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000AB12 File Offset: 0x00008D12
		public static implicit operator FixedString4096Bytes(in FixedString32Bytes fs)
		{
			return new FixedString4096Bytes(in fs);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000AB1A File Offset: 0x00008D1A
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static implicit operator FixedString32Bytes(string b)
		{
			return new FixedString32Bytes(b);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000AB22 File Offset: 0x00008D22
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public override string ToString()
		{
			return (ref this).ConvertToString<FixedString32Bytes>();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000AB2A File Offset: 0x00008D2A
		public override int GetHashCode()
		{
			return (ref this).ComputeHashCode<FixedString32Bytes>();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000AB34 File Offset: 0x00008D34
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

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000ABD0 File Offset: 0x00008DD0
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
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range in FixedString32Bytes of '{1}' Length.", index, this.utf8LengthInBytes));
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000AC21 File Offset: 0x00008E21
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckLengthInRange(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} must be positive.", length));
			}
			if (length > 29)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} is out of range in FixedString32Bytes of '{1}' Capacity.", length, 29));
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000AC5F File Offset: 0x00008E5F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckCapacityInRange(int capacity)
		{
			if (capacity > 29)
			{
				throw new ArgumentOutOfRangeException(string.Format("Capacity {0} must be lower than {1}.", capacity, 29));
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000AC83 File Offset: 0x00008E83
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckCopyError(CopyError error, string source)
		{
			if (error != CopyError.None)
			{
				throw new ArgumentException(string.Format("FixedString32Bytes: {0} while copying \"{1}\"", error, source));
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000AC9F File Offset: 0x00008E9F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckFormatError(FormatError error)
		{
			if (error != FormatError.None)
			{
				throw new ArgumentException("Source is too long to fit into fixed string of this size");
			}
		}

		// Token: 0x04000225 RID: 549
		internal const ushort utf8MaxLengthInBytes = 29;

		// Token: 0x04000226 RID: 550
		[SerializeField]
		internal ushort utf8LengthInBytes;

		// Token: 0x04000227 RID: 551
		[SerializeField]
		internal FixedBytes30 bytes;

		// Token: 0x0200006C RID: 108
		public struct Enumerator : IEnumerator
		{
			// Token: 0x060003F6 RID: 1014 RVA: 0x0000ACAF File Offset: 0x00008EAF
			public Enumerator(FixedString32Bytes other)
			{
				this.target = other;
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x060003F7 RID: 1015 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x060003F8 RID: 1016 RVA: 0x0000ACCB File Offset: 0x00008ECB
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

			// Token: 0x060003F9 RID: 1017 RVA: 0x0000AD0B File Offset: 0x00008F0B
			public void Reset()
			{
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x17000085 RID: 133
			// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000AD20 File Offset: 0x00008F20
			public Unicode.Rune Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.current;
				}
			}

			// Token: 0x17000086 RID: 134
			// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000AD28 File Offset: 0x00008F28
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04000228 RID: 552
			private FixedString32Bytes target;

			// Token: 0x04000229 RID: 553
			private int offset;

			// Token: 0x0400022A RID: 554
			private Unicode.Rune current;
		}
	}
}
