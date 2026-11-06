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
	// Token: 0x02000077 RID: 119
	[GenerateTestsForBurstCompatibility]
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 4096)]
	public struct FixedString4096Bytes : INativeList<byte>, IIndexable<byte>, IUTF8Bytes, IComparable<string>, IEquatable<string>, IComparable<FixedString32Bytes>, IEquatable<FixedString32Bytes>, IComparable<FixedString64Bytes>, IEquatable<FixedString64Bytes>, IComparable<FixedString128Bytes>, IEquatable<FixedString128Bytes>, IComparable<FixedString512Bytes>, IEquatable<FixedString512Bytes>, IComparable<FixedString4096Bytes>, IEquatable<FixedString4096Bytes>
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0000C2D3 File Offset: 0x0000A4D3
		public static int UTF8MaxLengthInBytes
		{
			get
			{
				return 4093;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0000C2DA File Offset: 0x0000A4DA
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

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe readonly byte* GetUnsafePtr()
		{
			fixed (FixedBytes4094* ptr = &this.bytes)
			{
				return (byte*)ptr;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000C300 File Offset: 0x0000A500
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x0000C308 File Offset: 0x0000A508
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

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000C2D3 File Offset: 0x0000A4D3
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x00002C47 File Offset: 0x00000E47
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return 4093;
			}
			set
			{
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000C324 File Offset: 0x0000A524
		public unsafe bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory)
		{
			if (newLength < 0 || newLength > 4093)
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x0000C3A2 File Offset: 0x0000A5A2
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.utf8LengthInBytes == 0;
			}
		}

		// Token: 0x170000A4 RID: 164
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

		// Token: 0x060004DF RID: 1247 RVA: 0x0000C3C4 File Offset: 0x0000A5C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref byte ElementAt(int index)
		{
			return ref this.GetUnsafePtr()[index];
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0000C3CE File Offset: 0x0000A5CE
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000C3D8 File Offset: 0x0000A5D8
		public void Add(in byte value)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = value;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000C3FE File Offset: 0x0000A5FE
		public FixedString4096Bytes.Enumerator GetEnumerator()
		{
			return new FixedString4096Bytes.Enumerator(this);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0000C40B File Offset: 0x0000A60B
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public int CompareTo(string other)
		{
			return this.ToString().CompareTo(other);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000C420 File Offset: 0x0000A620
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public unsafe bool Equals(string other)
		{
			int num = (int)this.utf8LengthInBytes;
			int length = other.Length;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in this.bytes);
			char* ptr2 = other;
			if (ptr2 != null)
			{
				ptr2 += RuntimeHelpers.OffsetToStringData / 2;
			}
			return UTF8ArrayUnsafeUtility.StrCmp(ptr, num, ptr2, length) == 0;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0000C465 File Offset: 0x0000A665
		public ref FixedList4096Bytes<byte> AsFixedList()
		{
			return UnsafeUtility.AsRef<FixedList4096Bytes<byte>>(UnsafeUtility.AddressOf<FixedString4096Bytes>(ref this));
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0000C472 File Offset: 0x0000A672
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public FixedString4096Bytes(string source)
		{
			this = default(FixedString4096Bytes);
			this.Initialize(source);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000C483 File Offset: 0x0000A683
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		internal CopyError Initialize(string source)
		{
			return (ref this).CopyFromTruncated<FixedString4096Bytes>(source);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000C48C File Offset: 0x0000A68C
		public FixedString4096Bytes(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString4096Bytes);
			this.Initialize(rune, count);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0000C49E File Offset: 0x0000A69E
		internal FormatError Initialize(Unicode.Rune rune, int count = 1)
		{
			this = default(FixedString4096Bytes);
			return (ref this).Append<FixedString4096Bytes>(rune, count);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
		internal unsafe FormatError Initialize(byte* srcBytes, int srcLength)
		{
			this.bytes = default(FixedBytes4094);
			this.utf8LengthInBytes = 0;
			int num = 0;
			FormatError formatError = UTF8ArrayUnsafeUtility.AppendUTF8Bytes(this.GetUnsafePtr(), ref num, 4093, srcBytes, srcLength);
			if (formatError != FormatError.None)
			{
				return formatError;
			}
			this.Length = num;
			return FormatError.None;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0000C4F4 File Offset: 0x0000A6F4
		public FixedString4096Bytes(NativeText.ReadOnly other)
		{
			this = default(FixedString4096Bytes);
			this.Initialize(other.GetUnsafePtr(), other.Length);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000C514 File Offset: 0x0000A714
		public FixedString4096Bytes(in UnsafeText other)
		{
			this = default(FixedString4096Bytes);
			UnsafeText unsafeText = other;
			this.Initialize(unsafeText.GetUnsafePtr(), other.Length);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0000C543 File Offset: 0x0000A743
		public int CompareTo(FixedString32Bytes other)
		{
			return (ref this).CompareTo<FixedString4096Bytes, FixedString32Bytes>(in other);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000C54D File Offset: 0x0000A74D
		public FixedString4096Bytes(in FixedString32Bytes other)
		{
			this = default(FixedString4096Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000C55E File Offset: 0x0000A75E
		internal unsafe FormatError Initialize(in FixedString32Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000C578 File Offset: 0x0000A778
		public unsafe static bool operator ==(in FixedString4096Bytes a, in FixedString32Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000C5B4 File Offset: 0x0000A7B4
		public static bool operator !=(in FixedString4096Bytes a, in FixedString32Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000C5C0 File Offset: 0x0000A7C0
		public bool Equals(FixedString32Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000C5CA File Offset: 0x0000A7CA
		public int CompareTo(FixedString64Bytes other)
		{
			return (ref this).CompareTo<FixedString4096Bytes, FixedString64Bytes>(in other);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
		public FixedString4096Bytes(in FixedString64Bytes other)
		{
			this = default(FixedString4096Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000C5E5 File Offset: 0x0000A7E5
		internal unsafe FormatError Initialize(in FixedString64Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000C600 File Offset: 0x0000A800
		public unsafe static bool operator ==(in FixedString4096Bytes a, in FixedString64Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000C63C File Offset: 0x0000A83C
		public static bool operator !=(in FixedString4096Bytes a, in FixedString64Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000C648 File Offset: 0x0000A848
		public bool Equals(FixedString64Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0000C652 File Offset: 0x0000A852
		public int CompareTo(FixedString128Bytes other)
		{
			return (ref this).CompareTo<FixedString4096Bytes, FixedString128Bytes>(in other);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000C65C File Offset: 0x0000A85C
		public FixedString4096Bytes(in FixedString128Bytes other)
		{
			this = default(FixedString4096Bytes);
			this.Initialize(in other);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000C66D File Offset: 0x0000A86D
		internal unsafe FormatError Initialize(in FixedString128Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000C688 File Offset: 0x0000A888
		public unsafe static bool operator ==(in FixedString4096Bytes a, in FixedString128Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000C6C4 File Offset: 0x0000A8C4
		public static bool operator !=(in FixedString4096Bytes a, in FixedString128Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0000C6D0 File Offset: 0x0000A8D0
		public bool Equals(FixedString128Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0000C6DA File Offset: 0x0000A8DA
		public int CompareTo(FixedString512Bytes other)
		{
			return (ref this).CompareTo<FixedString4096Bytes, FixedString512Bytes>(in other);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
		public FixedString4096Bytes(in FixedString512Bytes other)
		{
			this = default(FixedString4096Bytes);
			this.Initialize(in other);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0000C6F5 File Offset: 0x0000A8F5
		internal unsafe FormatError Initialize(in FixedString512Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0000C710 File Offset: 0x0000A910
		public unsafe static bool operator ==(in FixedString4096Bytes a, in FixedString512Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000C74C File Offset: 0x0000A94C
		public static bool operator !=(in FixedString4096Bytes a, in FixedString512Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000C758 File Offset: 0x0000A958
		public bool Equals(FixedString512Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000C762 File Offset: 0x0000A962
		public int CompareTo(FixedString4096Bytes other)
		{
			return (ref this).CompareTo<FixedString4096Bytes, FixedString4096Bytes>(in other);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0000C76C File Offset: 0x0000A96C
		public FixedString4096Bytes(in FixedString4096Bytes other)
		{
			this = default(FixedString4096Bytes);
			this.Initialize(in other);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0000C77D File Offset: 0x0000A97D
		internal unsafe FormatError Initialize(in FixedString4096Bytes other)
		{
			return this.Initialize((byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in other.bytes), (int)other.utf8LengthInBytes);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0000C798 File Offset: 0x0000A998
		public unsafe static bool operator ==(in FixedString4096Bytes a, in FixedString4096Bytes b)
		{
			int num = (int)a.utf8LengthInBytes;
			int num2 = (int)b.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in a.bytes);
			byte* ptr2 = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(ptr, num, ptr2, num2);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		public static bool operator !=(in FixedString4096Bytes a, in FixedString4096Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0000C7E0 File Offset: 0x0000A9E0
		public bool Equals(FixedString4096Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0000C7EA File Offset: 0x0000A9EA
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public static implicit operator FixedString4096Bytes(string b)
		{
			return new FixedString4096Bytes(b);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0000C7F2 File Offset: 0x0000A9F2
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public override string ToString()
		{
			return (ref this).ConvertToString<FixedString4096Bytes>();
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000C7FA File Offset: 0x0000A9FA
		public override int GetHashCode()
		{
			return (ref this).ComputeHashCode<FixedString4096Bytes>();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0000C804 File Offset: 0x0000AA04
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

		// Token: 0x0600050F RID: 1295 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
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
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range in FixedString4096Bytes of '{1}' Length.", index, this.utf8LengthInBytes));
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckLengthInRange(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} must be positive.", length));
			}
			if (length > 4093)
			{
				throw new ArgumentOutOfRangeException(string.Format("Length {0} is out of range in FixedString4096Bytes of '{1}' Capacity.", length, 4093));
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0000C943 File Offset: 0x0000AB43
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckCapacityInRange(int capacity)
		{
			if (capacity > 4093)
			{
				throw new ArgumentOutOfRangeException(string.Format("Capacity {0} must be lower than {1}.", capacity, 4093));
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0000C96D File Offset: 0x0000AB6D
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckCopyError(CopyError error, string source)
		{
			if (error != CopyError.None)
			{
				throw new ArgumentException(string.Format("FixedString4096Bytes: {0} while copying \"{1}\"", error, source));
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0000AC9F File Offset: 0x00008E9F
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckFormatError(FormatError error)
		{
			if (error != FormatError.None)
			{
				throw new ArgumentException("Source is too long to fit into fixed string of this size");
			}
		}

		// Token: 0x0400039D RID: 925
		internal const ushort utf8MaxLengthInBytes = 4093;

		// Token: 0x0400039E RID: 926
		[SerializeField]
		internal ushort utf8LengthInBytes;

		// Token: 0x0400039F RID: 927
		[SerializeField]
		internal FixedBytes4094 bytes;

		// Token: 0x02000078 RID: 120
		public struct Enumerator : IEnumerator
		{
			// Token: 0x06000514 RID: 1300 RVA: 0x0000C989 File Offset: 0x0000AB89
			public Enumerator(FixedString4096Bytes other)
			{
				this.target = other;
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x06000515 RID: 1301 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x06000516 RID: 1302 RVA: 0x0000C9A5 File Offset: 0x0000ABA5
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

			// Token: 0x06000517 RID: 1303 RVA: 0x0000C9E5 File Offset: 0x0000ABE5
			public void Reset()
			{
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x06000518 RID: 1304 RVA: 0x0000C9FA File Offset: 0x0000ABFA
			public Unicode.Rune Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.current;
				}
			}

			// Token: 0x170000A6 RID: 166
			// (get) Token: 0x06000519 RID: 1305 RVA: 0x0000CA02 File Offset: 0x0000AC02
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x040003A0 RID: 928
			private FixedString4096Bytes target;

			// Token: 0x040003A1 RID: 929
			private int offset;

			// Token: 0x040003A2 RID: 930
			private Unicode.Rune current;
		}
	}
}
