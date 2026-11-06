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
	// Token: 0x02000074 RID: 116
	[GenerateTestsForBurstCompatibility]
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 512)]
	public struct FixedString512Bytes : INativeList<byte>, IIndexable<byte>, IUTF8Bytes, IComparable<string>, IEquatable<string>, IComparable<FixedString32Bytes>, IEquatable<FixedString32Bytes>, IComparable<FixedString64Bytes>, IEquatable<FixedString64Bytes>, IComparable<FixedString128Bytes>, IEquatable<FixedString128Bytes>, IComparable<FixedString512Bytes>, IEquatable<FixedString512Bytes>, IComparable<FixedString4096Bytes>, IEquatable<FixedString4096Bytes>
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x0000BB8D File Offset: 0x00009D8D
		public static int UTF8MaxLengthInBytes
		{
			get
			{
				return 509;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0000BB94 File Offset: 0x00009D94
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

		// Token: 0x0600048F RID: 1167 RVA: 0x0000BBA4 File Offset: 0x00009DA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe readonly byte* GetUnsafePtr()
		{
			fixed (FixedBytes510* ptr = &this.bytes)
			{
				return (byte*)ptr;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0000BBBC File Offset: 0x00009DBC
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x0000BBC4 File Offset: 0x00009DC4
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

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x0000BB8D File Offset: 0x00009D8D
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return 509;
			}
			set
			{
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000BBE0 File Offset: 0x00009DE0
		public unsafe bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory)
		{
			if (newLength < 0 || newLength > 509)
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000BC5E File Offset: 0x00009E5E
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.utf8LengthInBytes == 0;
			}
		}

		// Token: 0x1700009C RID: 156
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

		// Token: 0x06000498 RID: 1176 RVA: 0x0000BC80 File Offset: 0x00009E80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref byte ElementAt(int index)
		{
			return ref this.GetUnsafePtr()[index];
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000BC8A File Offset: 0x00009E8A
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000BC94 File Offset: 0x00009E94
		public void Add(in byte value)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = value;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000BCBA File Offset: 0x00009EBA
		public FixedString512Bytes.Enumerator GetEnumerator()
		{
			return new FixedString512Bytes.Enumerator(this);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000BCC7 File Offset: 0x00009EC7
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public int CompareTo(string other)
		{
			return this.ToString().CompareTo(other);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000BCDC File Offset: 0x00009EDC
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public unsafe bool Equals(string other)
		{
			int num = (int)this.utf8LengthInBytes;
			int length = other.Length;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in this.bytes);
			char* ptr2 = other;
			if (ptr2 != null)
			{
				ptr2 += RuntimeHelpers.OffsetToStringData / 2;
			}
			return UTF8ArrayUnsafeUtility.StrCmp(ptr, num, ptr2, length) == 0;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000BD21 File Offset: 0x00009F21
		public ref FixedList512Bytes<byte> AsFixedList()
		{
			return UnsafeUtility.AsRef<FixedList512Bytes<byte>>(UnsafeUtility.AddressOf<FixedString512Bytes>(ref this));
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000BD2E File Offset: 0x00009F2E
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public FixedString512Bytes(string source)
		{
			this = default(FixedString512Bytes);
			this.Initialize(source);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000BD3F File Offset: 0x00009F3F
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		internal CopyError Initialize(string source)
		{
			return (ref this).CopyFromTruncated<FixedString512Bytes>(source);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000BD48 File Offset: 0x00009F48
		public FixedString512Bytes(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString512Bytes);
			this.Initialize(rune, count);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000BD5A File Offset: 0x00009F5A
		internal FormatError Initialize(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString512Bytes);
			return (ref this).Append<FixedString512Bytes>(rune, count);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000BD6C File Offset: 0x00009F6C
		internal unsafe FormatError Initialize(byte* srcBytes, int srcLength)
		{
			this.bytes = default(FixedBytes510);
			this.utf8LengthInBytes = 0;
			int num = 0;
			FormatError formatError = UTF8ArrayUnsafeUtility.AppendUTF8Bytes(this.GetUnsafePtr(), ref num, 509, srcBytes, srcLength);
			if (formatError != FormatError.None)
			{
				return formatError;
			}
			this.Length = num;
			return FormatError.None;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000BDB0 File Offset: 0x00009FB0
		public FixedString512Bytes(NativeText.ReadOnly other)
		{
			this = default(FixedString512Bytes);
			this.Initialize(other.GetUnsafePtr(), other.Length);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000BDD0 File Offset: 0x00009FD0
		public FixedString512Bytes(in UnsafeText other)
		{
			this = default(FixedString512Bytes);
			UnsafeText unsafeText = other;
			this.Initialize(unsafeText.GetUnsafePtr(), other.Length);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000BDFF File Offset: 0x00009FFF
		public int CompareTo(FixedString32Bytes other)
		{
			return (ref this).CompareTo<FixedString512Bytes, FixedString32Bytes>(in other);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000BE09 File Offset: 0x0000A009
		public FixedString512Bytes(in FixedString32Bytes other)
		{
			this = default(FixedString512Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000BE1A File Offset: 0x0000A01A
		internal unsafe FormatError Initialize(in FixedString32Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000BE34 File Offset: 0x0000A034
		public unsafe static bool operator ==(in FixedString512Bytes a, in FixedString32Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000BE70 File Offset: 0x0000A070
		public static bool operator !=(in FixedString512Bytes a, in FixedString32Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000BE7C File Offset: 0x0000A07C
		public bool Equals(FixedString32Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000BE86 File Offset: 0x0000A086
		public int CompareTo(FixedString64Bytes other)
		{
			return (ref this).CompareTo<FixedString512Bytes, FixedString64Bytes>(in other);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000BE90 File Offset: 0x0000A090
		public FixedString512Bytes(in FixedString64Bytes other)
		{
			this = default(FixedString512Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000BEA1 File Offset: 0x0000A0A1
		internal unsafe FormatError Initialize(in FixedString64Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000BEBC File Offset: 0x0000A0BC
		public unsafe static bool operator ==(in FixedString512Bytes a, in FixedString64Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000BEF8 File Offset: 0x0000A0F8
		public static bool operator !=(in FixedString512Bytes a, in FixedString64Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000BF04 File Offset: 0x0000A104
		public bool Equals(FixedString64Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000BF0E File Offset: 0x0000A10E
		public int CompareTo(FixedString128Bytes other)
		{
			return (ref this).CompareTo<FixedString512Bytes, FixedString128Bytes>(in other);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000BF18 File Offset: 0x0000A118
		public FixedString512Bytes(in FixedString128Bytes other)
		{
			this = default(FixedString512Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000BF29 File Offset: 0x0000A129
		internal unsafe FormatError Initialize(in FixedString128Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000BF44 File Offset: 0x0000A144
		public unsafe static bool operator ==(in FixedString512Bytes a, in FixedString128Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000BF80 File Offset: 0x0000A180
		public static bool operator !=(in FixedString512Bytes a, in FixedString128Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000BF8C File Offset: 0x0000A18C
		public bool Equals(FixedString128Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000BF96 File Offset: 0x0000A196
		public int CompareTo(FixedString512Bytes other)
		{
			return (ref this).CompareTo<FixedString512Bytes, FixedString512Bytes>(in other);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		public FixedString512Bytes(in FixedString512Bytes other)
		{
			this = default(FixedString512Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000BFB1 File Offset: 0x0000A1B1
		internal unsafe FormatError Initialize(in FixedString512Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		public unsafe static bool operator ==(in FixedString512Bytes a, in FixedString512Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000C008 File Offset: 0x0000A208
		public static bool operator !=(in FixedString512Bytes a, in FixedString512Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000C014 File Offset: 0x0000A214
		public bool Equals(FixedString512Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000C01E File Offset: 0x0000A21E
		public int CompareTo(FixedString4096Bytes other)
		{
			return (ref this).CompareTo<FixedString512Bytes, FixedString4096Bytes>(in other);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000C028 File Offset: 0x0000A228
		public FixedString512Bytes(in FixedString4096Bytes other)
		{
			this = default(FixedString512Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000C039 File Offset: 0x0000A239
		internal unsafe FormatError Initialize(in FixedString4096Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000C054 File Offset: 0x0000A254
		public unsafe static bool operator ==(in FixedString512Bytes a, in FixedString4096Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000C090 File Offset: 0x0000A290
		public static bool operator !=(in FixedString512Bytes a, in FixedString4096Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000C09C File Offset: 0x0000A29C
		public bool Equals(FixedString4096Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000C0A6 File Offset: 0x0000A2A6
		public static implicit operator FixedString4096Bytes(in FixedString512Bytes fs)
		{
			return new FixedString4096Bytes(in fs);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000C0AE File Offset: 0x0000A2AE
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static implicit operator FixedString512Bytes(string b)
		{
			return new FixedString512Bytes(b);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000C0B6 File Offset: 0x0000A2B6
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public override string ToString()
		{
			return (ref this).ConvertToString<FixedString512Bytes>();
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0000C0BE File Offset: 0x0000A2BE
		public override int GetHashCode()
		{
			return (ref this).ComputeHashCode<FixedString512Bytes>();
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000C0C8 File Offset: 0x0000A2C8
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

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000C164 File Offset: 0x0000A364
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
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range in FixedString512Bytes of '{1}' Length.", index, this.utf8LengthInBytes));
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000C1B8 File Offset: 0x0000A3B8
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckLengthInRange(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} must be positive.", length));
			}
			if (length > 509)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} is out of range in FixedString512Bytes of '{1}' Capacity.", length, 509));
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0000C207 File Offset: 0x0000A407
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckCapacityInRange(int capacity)
		{
			if (capacity > 509)
			{
				throw new ArgumentOutOfRangeException(string.Format("Capacity {0} must be lower than {1}.", capacity, 509));
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000C231 File Offset: 0x0000A431
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckCopyError(CopyError error, string source)
		{
			if (error != CopyError.None)
			{
				throw new ArgumentException(string.Format("FixedString512Bytes: {0} while copying \"{1}\"", error, source));
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000AC9F File Offset: 0x00008E9F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckFormatError(FormatError error)
		{
			if (error != FormatError.None)
			{
				throw new ArgumentException("Source is too long to fit into fixed string of this size");
			}
		}

		// Token: 0x0400028A RID: 650
		internal const ushort utf8MaxLengthInBytes = 509;

		// Token: 0x0400028B RID: 651
		[SerializeField]
		internal ushort utf8LengthInBytes;

		// Token: 0x0400028C RID: 652
		[SerializeField]
		internal FixedBytes510 bytes;

		// Token: 0x02000075 RID: 117
		public struct Enumerator : IEnumerator
		{
			// Token: 0x060004CE RID: 1230 RVA: 0x0000C24D File Offset: 0x0000A44D
			public Enumerator(FixedString512Bytes other)
			{
				this.target = other;
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x060004CF RID: 1231 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x060004D0 RID: 1232 RVA: 0x0000C269 File Offset: 0x0000A469
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

			// Token: 0x060004D1 RID: 1233 RVA: 0x0000C2A9 File Offset: 0x0000A4A9
			public void Reset()
			{
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0000C2BE File Offset: 0x0000A4BE
			public Unicode.Rune Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.current;
				}
			}

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x060004D3 RID: 1235 RVA: 0x0000C2C6 File Offset: 0x0000A4C6
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0400028D RID: 653
			private FixedString512Bytes target;

			// Token: 0x0400028E RID: 654
			private int offset;

			// Token: 0x0400028F RID: 655
			private Unicode.Rune current;
		}
	}
}
