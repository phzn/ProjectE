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
	// Token: 0x0200006E RID: 110
	[GenerateTestsForBurstCompatibility]
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct FixedString64Bytes : INativeList<byte>, IIndexable<byte>, IUTF8Bytes, IComparable<string>, IEquatable<string>, IComparable<FixedString32Bytes>, IEquatable<FixedString32Bytes>, IComparable<FixedString64Bytes>, IEquatable<FixedString64Bytes>, IComparable<FixedString128Bytes>, IEquatable<FixedString128Bytes>, IComparable<FixedString512Bytes>, IEquatable<FixedString512Bytes>, IComparable<FixedString4096Bytes>, IEquatable<FixedString4096Bytes>
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0000AD35 File Offset: 0x00008F35
		public static int UTF8MaxLengthInBytes
		{
			get
			{
				return 61;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0000AD39 File Offset: 0x00008F39
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

		// Token: 0x060003FE RID: 1022 RVA: 0x0000AD48 File Offset: 0x00008F48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe readonly byte* GetUnsafePtr()
		{
			fixed (FixedBytes62* ptr = &this.bytes)
			{
				return (byte*)ptr;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0000AD60 File Offset: 0x00008F60
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0000AD68 File Offset: 0x00008F68
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000AD35 File Offset: 0x00008F35
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return 61;
			}
			set
			{
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000AD84 File Offset: 0x00008F84
		public unsafe bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory)
		{
			if (newLength < 0 || newLength > 61)
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

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000ADFF File Offset: 0x00008FFF
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.utf8LengthInBytes == 0;
			}
		}

		// Token: 0x1700008C RID: 140
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

		// Token: 0x06000407 RID: 1031 RVA: 0x0000AE21 File Offset: 0x00009021
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref byte ElementAt(int index)
		{
			return ref this.GetUnsafePtr()[index];
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000AE2B File Offset: 0x0000902B
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000AE34 File Offset: 0x00009034
		public void Add(in byte value)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = value;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000AE5A File Offset: 0x0000905A
		public FixedString64Bytes.Enumerator GetEnumerator()
		{
			return new FixedString64Bytes.Enumerator(this);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000AE67 File Offset: 0x00009067
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public int CompareTo(string other)
		{
			return this.ToString().CompareTo(other);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000AE7C File Offset: 0x0000907C
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public unsafe bool Equals(string other)
		{
			int num = (int)this.utf8LengthInBytes;
			int length = other.Length;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in this.bytes);
			char* ptr2 = other;
			if (ptr2 != null)
			{
				ptr2 += RuntimeHelpers.OffsetToStringData / 2;
			}
			return UTF8ArrayUnsafeUtility.StrCmp(ptr, num, ptr2, length) == 0;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000AEC1 File Offset: 0x000090C1
		public ref FixedList64Bytes<byte> AsFixedList()
		{
			return UnsafeUtility.AsRef<FixedList64Bytes<byte>>(UnsafeUtility.AddressOf<FixedString64Bytes>(ref this));
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000AECE File Offset: 0x000090CE
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public FixedString64Bytes(string source)
		{
			this = default(FixedString64Bytes);
			this.Initialize(source);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000AEDF File Offset: 0x000090DF
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		internal CopyError Initialize(string source)
		{
			return (ref this).CopyFromTruncated<FixedString64Bytes>(source);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000AEE8 File Offset: 0x000090E8
		public FixedString64Bytes(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString64Bytes);
			this.Initialize(rune, count);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000AEFA File Offset: 0x000090FA
		internal FormatError Initialize(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString64Bytes);
			return (ref this).Append<FixedString64Bytes>(rune, count);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000AF0C File Offset: 0x0000910C
		internal unsafe FormatError Initialize(byte* srcBytes, int srcLength)
		{
			this.bytes = default(FixedBytes62);
			this.utf8LengthInBytes = 0;
			int num = 0;
			FormatError formatError = UTF8ArrayUnsafeUtility.AppendUTF8Bytes(this.GetUnsafePtr(), ref num, 61, srcBytes, srcLength);
			if (formatError != FormatError.None)
			{
				return formatError;
			}
			this.Length = num;
			return FormatError.None;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000AF4D File Offset: 0x0000914D
		public FixedString64Bytes(NativeText.ReadOnly other)
		{
			this = default(FixedString64Bytes);
			this.Initialize(other.GetUnsafePtr(), other.Length);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000AF6C File Offset: 0x0000916C
		public FixedString64Bytes(in UnsafeText other)
		{
			this = default(FixedString64Bytes);
			UnsafeText unsafeText = other;
			this.Initialize(unsafeText.GetUnsafePtr(), other.Length);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000AF9B File Offset: 0x0000919B
		public int CompareTo(FixedString32Bytes other)
		{
			return (ref this).CompareTo<FixedString64Bytes, FixedString32Bytes>(in other);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000AFA5 File Offset: 0x000091A5
		public FixedString64Bytes(in FixedString32Bytes other)
		{
			this = default(FixedString64Bytes);
			this.Initialize(in other);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000AFB6 File Offset: 0x000091B6
		internal unsafe FormatError Initialize(in FixedString32Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000AFD0 File Offset: 0x000091D0
		public unsafe static bool operator ==(in FixedString64Bytes a, in FixedString32Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000B00C File Offset: 0x0000920C
		public static bool operator !=(in FixedString64Bytes a, in FixedString32Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000B018 File Offset: 0x00009218
		public bool Equals(FixedString32Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000B022 File Offset: 0x00009222
		public int CompareTo(FixedString64Bytes other)
		{
			return (ref this).CompareTo<FixedString64Bytes, FixedString64Bytes>(in other);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000B02C File Offset: 0x0000922C
		public FixedString64Bytes(in FixedString64Bytes other)
		{
			this = default(FixedString64Bytes);
			this.Initialize(in other);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000B03D File Offset: 0x0000923D
		internal unsafe FormatError Initialize(in FixedString64Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000B058 File Offset: 0x00009258
		public unsafe static bool operator ==(in FixedString64Bytes a, in FixedString64Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000B094 File Offset: 0x00009294
		public static bool operator !=(in FixedString64Bytes a, in FixedString64Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000B0A0 File Offset: 0x000092A0
		public bool Equals(FixedString64Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000B0AA File Offset: 0x000092AA
		public int CompareTo(FixedString128Bytes other)
		{
			return (ref this).CompareTo<FixedString64Bytes, FixedString128Bytes>(in other);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000B0B4 File Offset: 0x000092B4
		public FixedString64Bytes(in FixedString128Bytes other)
		{
			this = default(FixedString64Bytes);
			this.Initialize(in other);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000B0C5 File Offset: 0x000092C5
		internal unsafe FormatError Initialize(in FixedString128Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000B0E0 File Offset: 0x000092E0
		public unsafe static bool operator ==(in FixedString64Bytes a, in FixedString128Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000B11C File Offset: 0x0000931C
		public static bool operator !=(in FixedString64Bytes a, in FixedString128Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000B128 File Offset: 0x00009328
		public bool Equals(FixedString128Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000B132 File Offset: 0x00009332
		public static implicit operator FixedString128Bytes(in FixedString64Bytes fs)
		{
			return new FixedString128Bytes(in fs);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000B13A File Offset: 0x0000933A
		public int CompareTo(FixedString512Bytes other)
		{
			return (ref this).CompareTo<FixedString64Bytes, FixedString512Bytes>(in other);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000B144 File Offset: 0x00009344
		public FixedString64Bytes(in FixedString512Bytes other)
		{
			this = default(FixedString64Bytes);
			this.Initialize(in other);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000B155 File Offset: 0x00009355
		internal unsafe FormatError Initialize(in FixedString512Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000B170 File Offset: 0x00009370
		public unsafe static bool operator ==(in FixedString64Bytes a, in FixedString512Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000B1AC File Offset: 0x000093AC
		public static bool operator !=(in FixedString64Bytes a, in FixedString512Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000B1B8 File Offset: 0x000093B8
		public bool Equals(FixedString512Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000B1C2 File Offset: 0x000093C2
		public static implicit operator FixedString512Bytes(in FixedString64Bytes fs)
		{
			return new FixedString512Bytes(in fs);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000B1CA File Offset: 0x000093CA
		public int CompareTo(FixedString4096Bytes other)
		{
			return (ref this).CompareTo<FixedString64Bytes, FixedString4096Bytes>(in other);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000B1D4 File Offset: 0x000093D4
		public FixedString64Bytes(in FixedString4096Bytes other)
		{
			this = default(FixedString64Bytes);
			this.Initialize(in other);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000B1E5 File Offset: 0x000093E5
		internal unsafe FormatError Initialize(in FixedString4096Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000B200 File Offset: 0x00009400
		public unsafe static bool operator ==(in FixedString64Bytes a, in FixedString4096Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000B23C File Offset: 0x0000943C
		public static bool operator !=(in FixedString64Bytes a, in FixedString4096Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000B248 File Offset: 0x00009448
		public bool Equals(FixedString4096Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000B252 File Offset: 0x00009452
		public static implicit operator FixedString4096Bytes(in FixedString64Bytes fs)
		{
			return new FixedString4096Bytes(in fs);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000B25A File Offset: 0x0000945A
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static implicit operator FixedString64Bytes(string b)
		{
			return new FixedString64Bytes(b);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000B262 File Offset: 0x00009462
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public override string ToString()
		{
			return (ref this).ConvertToString<FixedString64Bytes>();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000B26A File Offset: 0x0000946A
		public override int GetHashCode()
		{
			return (ref this).ComputeHashCode<FixedString64Bytes>();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000B274 File Offset: 0x00009474
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

		// Token: 0x0600043A RID: 1082 RVA: 0x0000B310 File Offset: 0x00009510
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
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range in FixedString64Bytes of '{1}' Length.", index, this.utf8LengthInBytes));
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000B361 File Offset: 0x00009561
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckLengthInRange(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} must be positive.", length));
			}
			if (length > 61)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} is out of range in FixedString64Bytes of '{1}' Capacity.", length, 61));
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000B39F File Offset: 0x0000959F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckCapacityInRange(int capacity)
		{
			if (capacity > 61)
			{
				throw new ArgumentOutOfRangeException(string.Format("Capacity {0} must be lower than {1}.", capacity, 61));
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000B3C3 File Offset: 0x000095C3
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckCopyError(CopyError error, string source)
		{
			if (error != CopyError.None)
			{
				throw new ArgumentException(string.Format("FixedString64Bytes: {0} while copying \"{1}\"", error, source));
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000AC9F File Offset: 0x00008E9F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckFormatError(FormatError error)
		{
			if (error != FormatError.None)
			{
				throw new ArgumentException("Source is too long to fit into fixed string of this size");
			}
		}

		// Token: 0x0400023C RID: 572
		internal const ushort utf8MaxLengthInBytes = 61;

		// Token: 0x0400023D RID: 573
		[SerializeField]
		internal ushort utf8LengthInBytes;

		// Token: 0x0400023E RID: 574
		[SerializeField]
		internal FixedBytes62 bytes;

		// Token: 0x0200006F RID: 111
		public struct Enumerator : IEnumerator
		{
			// Token: 0x0600043F RID: 1087 RVA: 0x0000B3DF File Offset: 0x000095DF
			public Enumerator(FixedString64Bytes other)
			{
				this.target = other;
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x06000440 RID: 1088 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000441 RID: 1089 RVA: 0x0000B3FB File Offset: 0x000095FB
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

			// Token: 0x06000442 RID: 1090 RVA: 0x0000B43B File Offset: 0x0000963B
			public void Reset()
			{
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x1700008D RID: 141
			// (get) Token: 0x06000443 RID: 1091 RVA: 0x0000B450 File Offset: 0x00009650
			public Unicode.Rune Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.current;
				}
			}

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000B458 File Offset: 0x00009658
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0400023F RID: 575
			private FixedString64Bytes target;

			// Token: 0x04000240 RID: 576
			private int offset;

			// Token: 0x04000241 RID: 577
			private Unicode.Rune current;
		}
	}
}
