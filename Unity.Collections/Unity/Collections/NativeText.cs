using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Properties;

namespace Unity.Collections
{
	// Token: 0x020000D2 RID: 210
	[NativeContainer]
	[DebuggerDisplay("Length = {Length}")]
	[GenerateTestsForBurstCompatibility]
	public struct NativeText : INativeList<byte>, IIndexable<byte>, INativeDisposable, IDisposable, IUTF8Bytes, IComparable<string>, IEquatable<string>, IComparable<NativeText>, IEquatable<NativeText>, IComparable<FixedString32Bytes>, IEquatable<FixedString32Bytes>, IComparable<FixedString64Bytes>, IEquatable<FixedString64Bytes>, IComparable<FixedString128Bytes>, IEquatable<FixedString128Bytes>, IComparable<FixedString512Bytes>, IEquatable<FixedString512Bytes>, IComparable<FixedString4096Bytes>, IEquatable<FixedString4096Bytes>
	{
		// Token: 0x06000975 RID: 2421 RVA: 0x0001C5C2 File Offset: 0x0001A7C2
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public NativeText(string source, Allocator allocator)
		{
			this = new NativeText(source, allocator);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0001C5D4 File Offset: 0x0001A7D4
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public unsafe NativeText(string source, AllocatorManager.AllocatorHandle allocator)
		{
			this = new NativeText(source.Length * 2, allocator);
			this.Length = source.Length * 2;
			fixed (string text = source)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				int num;
				if (UTF8ArrayUnsafeUtility.Copy(this.GetUnsafePtr(), out num, this.Capacity, ptr, source.Length) != CopyError.None)
				{
					this.m_Data->Dispose();
					this.m_Data = UnsafeText.Alloc(allocator);
					*this.m_Data = default(UnsafeText);
				}
				this.Length = num;
			}
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001C656 File Offset: 0x0001A856
		public NativeText(int capacity, Allocator allocator)
		{
			this = new NativeText(capacity, allocator);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001C665 File Offset: 0x0001A865
		public unsafe NativeText(int capacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_Data = UnsafeText.Alloc(allocator);
			*this.m_Data = new UnsafeText(capacity, allocator);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0001C685 File Offset: 0x0001A885
		public NativeText(Allocator allocator)
		{
			this = new NativeText(allocator);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0001C693 File Offset: 0x0001A893
		public NativeText(AllocatorManager.AllocatorHandle allocator)
		{
			this = new NativeText(512, allocator);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0001C6A4 File Offset: 0x0001A8A4
		public unsafe NativeText(in FixedString32Bytes source, AllocatorManager.AllocatorHandle allocator)
		{
			this = new NativeText((int)source.utf8LengthInBytes, allocator);
			this.Length = (int)source.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in source.bytes);
			UnsafeUtility.MemCpy((void*)this.m_Data->GetUnsafePtr(), (void*)ptr, (long)((ulong)source.utf8LengthInBytes));
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0001C6EE File Offset: 0x0001A8EE
		public NativeText(in FixedString32Bytes source, Allocator allocator)
		{
			this = new NativeText(in source, allocator);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0001C700 File Offset: 0x0001A900
		public unsafe NativeText(in FixedString64Bytes source, AllocatorManager.AllocatorHandle allocator)
		{
			this = new NativeText((int)source.utf8LengthInBytes, allocator);
			this.Length = (int)source.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in source.bytes);
			UnsafeUtility.MemCpy((void*)this.m_Data->GetUnsafePtr(), (void*)ptr, (long)((ulong)source.utf8LengthInBytes));
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0001C74A File Offset: 0x0001A94A
		public NativeText(in FixedString64Bytes source, Allocator allocator)
		{
			this = new NativeText(in source, allocator);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0001C75C File Offset: 0x0001A95C
		public unsafe NativeText(in FixedString128Bytes source, AllocatorManager.AllocatorHandle allocator)
		{
			this = new NativeText((int)source.utf8LengthInBytes, allocator);
			this.Length = (int)source.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in source.bytes);
			UnsafeUtility.MemCpy((void*)this.m_Data->GetUnsafePtr(), (void*)ptr, (long)((ulong)source.utf8LengthInBytes));
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0001C7A6 File Offset: 0x0001A9A6
		public NativeText(in FixedString128Bytes source, Allocator allocator)
		{
			this = new NativeText(in source, allocator);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0001C7B8 File Offset: 0x0001A9B8
		public unsafe NativeText(in FixedString512Bytes source, AllocatorManager.AllocatorHandle allocator)
		{
			this = new NativeText((int)source.utf8LengthInBytes, allocator);
			this.Length = (int)source.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in source.bytes);
			UnsafeUtility.MemCpy((void*)this.m_Data->GetUnsafePtr(), (void*)ptr, (long)((ulong)source.utf8LengthInBytes));
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0001C802 File Offset: 0x0001AA02
		public NativeText(in FixedString512Bytes source, Allocator allocator)
		{
			this = new NativeText(in source, allocator);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0001C814 File Offset: 0x0001AA14
		public unsafe NativeText(in FixedString4096Bytes source, AllocatorManager.AllocatorHandle allocator)
		{
			this = new NativeText((int)source.utf8LengthInBytes, allocator);
			this.Length = (int)source.utf8LengthInBytes;
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in source.bytes);
			UnsafeUtility.MemCpy((void*)this.m_Data->GetUnsafePtr(), (void*)ptr, (long)((ulong)source.utf8LengthInBytes));
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0001C85E File Offset: 0x0001AA5E
		public NativeText(in FixedString4096Bytes source, Allocator allocator)
		{
			this = new NativeText(in source, allocator);
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0001C86D File Offset: 0x0001AA6D
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x0001C87A File Offset: 0x0001AA7A
		public unsafe int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_Data->Length;
			}
			set
			{
				this.m_Data->Length = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x0001C888 File Offset: 0x0001AA88
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x0001C895 File Offset: 0x0001AA95
		public unsafe int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.m_Data->Capacity;
			}
			set
			{
				this.m_Data->Capacity = value;
			}
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0001C8A3 File Offset: 0x0001AAA3
		public bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory)
		{
			this.Length = newLength;
			return true;
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0001C8AD File Offset: 0x0001AAAD
		public unsafe readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || this.m_Data->IsEmpty;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0001C8C4 File Offset: 0x0001AAC4
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.m_Data != null;
			}
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0001C8D3 File Offset: 0x0001AAD3
		public unsafe byte* GetUnsafePtr()
		{
			return this.m_Data->GetUnsafePtr();
		}

		// Token: 0x17000115 RID: 277
		public unsafe byte this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return *this.m_Data->ElementAt(index);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				*this.m_Data->ElementAt(index) = value;
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0001C8FF File Offset: 0x0001AAFF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ref byte ElementAt(int index)
		{
			return this.m_Data->ElementAt(index);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0001C90D File Offset: 0x0001AB0D
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0001C918 File Offset: 0x0001AB18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(in byte value)
		{
			int length = this.Length;
			this.Length = length + 1;
			this[length] = value;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0001C93E File Offset: 0x0001AB3E
		public unsafe int CompareTo(NativeText other)
		{
			return (ref this).CompareTo<NativeText, UnsafeText>(in *other.m_Data);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0001C94C File Offset: 0x0001AB4C
		public unsafe bool Equals(NativeText other)
		{
			return (ref this).Equals<NativeText, UnsafeText>(in *other.m_Data);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0001C95A File Offset: 0x0001AB5A
		public int CompareTo(NativeText.ReadOnly other)
		{
			return (ref this).CompareTo<NativeText, NativeText.ReadOnly>(in other);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0001C964 File Offset: 0x0001AB64
		public unsafe bool Equals(NativeText.ReadOnly other)
		{
			return (ref this).Equals<NativeText, UnsafeText>(in *other.m_Data);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0001C972 File Offset: 0x0001AB72
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeText.Free(this.m_Data);
			this.m_Data = null;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0001C990 File Offset: 0x0001AB90
		public JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new NativeTextDisposeJob
			{
				Data = new NativeTextDispose
				{
					m_TextData = this.m_Data
				}
			}.Schedule(inputDeps);
			this.m_Data = null;
			return jobHandle;
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x0001C9DB File Offset: 0x0001ABDB
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

		// Token: 0x06000999 RID: 2457 RVA: 0x0001C9E9 File Offset: 0x0001ABE9
		public NativeText.Enumerator GetEnumerator()
		{
			return new NativeText.Enumerator(this);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0001C9F6 File Offset: 0x0001ABF6
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public int CompareTo(string other)
		{
			return this.ToString().CompareTo(other);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0001CA0A File Offset: 0x0001AC0A
		[ExcludeFromBurstCompatTesting("Takes managed string")]
		public bool Equals(string other)
		{
			return this.ToString().Equals(other);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0001CA1E File Offset: 0x0001AC1E
		public int CompareTo(FixedString32Bytes other)
		{
			return (ref this).CompareTo<NativeText, FixedString32Bytes>(in other);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0001CA28 File Offset: 0x0001AC28
		public unsafe static bool operator ==(in NativeText a, in FixedString32Bytes b)
		{
			NativeText nativeText = *UnsafeUtilityExtensions.AsRef<NativeText>(in a);
			int length = nativeText.Length;
			int utf8LengthInBytes = (int)b.utf8LengthInBytes;
			byte* unsafePtr = nativeText.GetUnsafePtr();
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001CA6B File Offset: 0x0001AC6B
		public static bool operator !=(in NativeText a, in FixedString32Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0001CA77 File Offset: 0x0001AC77
		public bool Equals(FixedString32Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001CA81 File Offset: 0x0001AC81
		public int CompareTo(FixedString64Bytes other)
		{
			return (ref this).CompareTo<NativeText, FixedString64Bytes>(in other);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001CA8C File Offset: 0x0001AC8C
		public unsafe static bool operator ==(in NativeText a, in FixedString64Bytes b)
		{
			NativeText nativeText = *UnsafeUtilityExtensions.AsRef<NativeText>(in a);
			int length = nativeText.Length;
			int utf8LengthInBytes = (int)b.utf8LengthInBytes;
			byte* unsafePtr = nativeText.GetUnsafePtr();
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001CACF File Offset: 0x0001ACCF
		public static bool operator !=(in NativeText a, in FixedString64Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001CADB File Offset: 0x0001ACDB
		public bool Equals(FixedString64Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001CAE5 File Offset: 0x0001ACE5
		public int CompareTo(FixedString128Bytes other)
		{
			return (ref this).CompareTo<NativeText, FixedString128Bytes>(in other);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001CAF0 File Offset: 0x0001ACF0
		public unsafe static bool operator ==(in NativeText a, in FixedString128Bytes b)
		{
			NativeText nativeText = *UnsafeUtilityExtensions.AsRef<NativeText>(in a);
			int length = nativeText.Length;
			int utf8LengthInBytes = (int)b.utf8LengthInBytes;
			byte* unsafePtr = nativeText.GetUnsafePtr();
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0001CB33 File Offset: 0x0001AD33
		public static bool operator !=(in NativeText a, in FixedString128Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0001CB3F File Offset: 0x0001AD3F
		public bool Equals(FixedString128Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0001CB49 File Offset: 0x0001AD49
		public int CompareTo(FixedString512Bytes other)
		{
			return (ref this).CompareTo<NativeText, FixedString512Bytes>(in other);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0001CB54 File Offset: 0x0001AD54
		public unsafe static bool operator ==(in NativeText a, in FixedString512Bytes b)
		{
			NativeText nativeText = *UnsafeUtilityExtensions.AsRef<NativeText>(in a);
			int length = nativeText.Length;
			int utf8LengthInBytes = (int)b.utf8LengthInBytes;
			byte* unsafePtr = nativeText.GetUnsafePtr();
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0001CB97 File Offset: 0x0001AD97
		public static bool operator !=(in NativeText a, in FixedString512Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0001CBA3 File Offset: 0x0001ADA3
		public bool Equals(FixedString512Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0001CBAD File Offset: 0x0001ADAD
		public int CompareTo(FixedString4096Bytes other)
		{
			return (ref this).CompareTo<NativeText, FixedString4096Bytes>(in other);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0001CBB8 File Offset: 0x0001ADB8
		public unsafe static bool operator ==(in NativeText a, in FixedString4096Bytes b)
		{
			NativeText nativeText = *UnsafeUtilityExtensions.AsRef<NativeText>(in a);
			int length = nativeText.Length;
			int utf8LengthInBytes = (int)b.utf8LengthInBytes;
			byte* unsafePtr = nativeText.GetUnsafePtr();
			byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in b.bytes);
			return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0001CBFB File Offset: 0x0001ADFB
		public static bool operator !=(in NativeText a, in FixedString4096Bytes b)
		{
			return !((in a) == (in b));
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0001CC07 File Offset: 0x0001AE07
		public bool Equals(FixedString4096Bytes other)
		{
			return (in this) == (in other);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0001CC11 File Offset: 0x0001AE11
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public override string ToString()
		{
			if (this.m_Data == null)
			{
				return "";
			}
			return (ref this).ConvertToString<NativeText>();
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0001CC29 File Offset: 0x0001AE29
		public override int GetHashCode()
		{
			return (ref this).ComputeHashCode<NativeText>();
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0001CC34 File Offset: 0x0001AE34
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			string text = other as string;
			if (text != null)
			{
				return this.Equals(text);
			}
			if (other is NativeText)
			{
				NativeText nativeText = (NativeText)other;
				return this.Equals(nativeText);
			}
			if (other is NativeText.ReadOnly)
			{
				NativeText.ReadOnly readOnly = (NativeText.ReadOnly)other;
				return this.Equals(readOnly);
			}
			if (other is FixedString32Bytes)
			{
				FixedString32Bytes fixedString32Bytes = (FixedString32Bytes)other;
				return this.Equals(fixedString32Bytes);
			}
			if (other is FixedString64Bytes)
			{
				FixedString64Bytes fixedString64Bytes = (FixedString64Bytes)other;
				return this.Equals(fixedString64Bytes);
			}
			if (other is FixedString128Bytes)
			{
				FixedString128Bytes fixedString128Bytes = (FixedString128Bytes)other;
				return this.Equals(fixedString128Bytes);
			}
			if (other is FixedString512Bytes)
			{
				FixedString512Bytes fixedString512Bytes = (FixedString512Bytes)other;
				return this.Equals(fixedString512Bytes);
			}
			if (other is FixedString4096Bytes)
			{
				FixedString4096Bytes fixedString4096Bytes = (FixedString4096Bytes)other;
				return this.Equals(fixedString4096Bytes);
			}
			return false;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0001CD02 File Offset: 0x0001AF02
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CheckNull(void* dataPtr)
		{
			if (dataPtr == null)
			{
				throw new InvalidOperationException("NativeText has yet to be created or has been destroyed!");
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckRead()
		{
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckWrite()
		{
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void CheckWriteAndBumpSecondaryVersion()
		{
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001CD14 File Offset: 0x0001AF14
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckIndexInRange(int index)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} must be positive.", index));
			}
			if (index >= this.Length)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range in NativeText of {1} length.", index, this.Length));
			}
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0001CD65 File Offset: 0x0001AF65
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowCopyError(CopyError error, string source)
		{
			throw new ArgumentException(string.Format("NativeText: {0} while copying \"{1}\"", error, source));
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0001CD7D File Offset: 0x0001AF7D
		public NativeText.ReadOnly AsReadOnly()
		{
			return new NativeText.ReadOnly(this.m_Data);
		}

		// Token: 0x0400040F RID: 1039
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeText* m_Data;

		// Token: 0x020000D3 RID: 211
		public struct Enumerator : IEnumerator<Unicode.Rune>, IEnumerator, IDisposable
		{
			// Token: 0x060009BA RID: 2490 RVA: 0x0001CD8A File Offset: 0x0001AF8A
			public Enumerator(NativeText source)
			{
				this.target = source.AsReadOnly();
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x060009BB RID: 2491 RVA: 0x0001CDAC File Offset: 0x0001AFAC
			public Enumerator(NativeText.ReadOnly source)
			{
				this.target = source;
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x060009BC RID: 2492 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Dispose()
			{
			}

			// Token: 0x060009BD RID: 2493 RVA: 0x0001CDC8 File Offset: 0x0001AFC8
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

			// Token: 0x060009BE RID: 2494 RVA: 0x0001CE08 File Offset: 0x0001B008
			public void Reset()
			{
				this.offset = 0;
				this.current = default(Unicode.Rune);
			}

			// Token: 0x17000117 RID: 279
			// (get) Token: 0x060009BF RID: 2495 RVA: 0x0001CE1D File Offset: 0x0001B01D
			object IEnumerator.Current
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.Current;
				}
			}

			// Token: 0x17000118 RID: 280
			// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0001CE2A File Offset: 0x0001B02A
			public Unicode.Rune Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x04000410 RID: 1040
			private NativeText.ReadOnly target;

			// Token: 0x04000411 RID: 1041
			private int offset;

			// Token: 0x04000412 RID: 1042
			private Unicode.Rune current;
		}

		// Token: 0x020000D4 RID: 212
		[NativeContainer]
		[NativeContainerIsReadOnly]
		public struct ReadOnly : INativeList<byte>, IIndexable<byte>, IUTF8Bytes, IComparable<string>, IEquatable<string>, IComparable<NativeText>, IEquatable<NativeText>, IComparable<FixedString32Bytes>, IEquatable<FixedString32Bytes>, IComparable<FixedString64Bytes>, IEquatable<FixedString64Bytes>, IComparable<FixedString128Bytes>, IEquatable<FixedString128Bytes>, IComparable<FixedString512Bytes>, IEquatable<FixedString512Bytes>, IComparable<FixedString4096Bytes>, IEquatable<FixedString4096Bytes>
		{
			// Token: 0x060009C1 RID: 2497 RVA: 0x0001CE32 File Offset: 0x0001B032
			internal unsafe ReadOnly(UnsafeText* text)
			{
				this.m_Data = text;
			}

			// Token: 0x17000119 RID: 281
			// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0001CE3B File Offset: 0x0001B03B
			// (set) Token: 0x060009C3 RID: 2499 RVA: 0x00002C47 File Offset: 0x00000E47
			public unsafe int Capacity
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				readonly get
				{
					return this.m_Data->Capacity;
				}
				set
				{
				}
			}

			// Token: 0x1700011A RID: 282
			// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0001CE48 File Offset: 0x0001B048
			// (set) Token: 0x060009C5 RID: 2501 RVA: 0x00002C47 File Offset: 0x00000E47
			public unsafe bool IsEmpty
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				readonly get
				{
					return this.m_Data == null || this.m_Data->IsEmpty;
				}
				set
				{
				}
			}

			// Token: 0x1700011B RID: 283
			// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0001CE61 File Offset: 0x0001B061
			// (set) Token: 0x060009C7 RID: 2503 RVA: 0x00002C47 File Offset: 0x00000E47
			public unsafe int Length
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				readonly get
				{
					return this.m_Data->Length;
				}
				set
				{
				}
			}

			// Token: 0x1700011C RID: 284
			public unsafe byte this[int index]
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				readonly get
				{
					return *this.m_Data->ElementAt(index);
				}
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				set
				{
				}
			}

			// Token: 0x060009CA RID: 2506 RVA: 0x00002C47 File Offset: 0x00000E47
			public void Clear()
			{
			}

			// Token: 0x060009CB RID: 2507 RVA: 0x0001CE7D File Offset: 0x0001B07D
			public ref byte ElementAt(int index)
			{
				throw new NotSupportedException("Trying to retrieve non-readonly ref to NativeText.ReadOnly data. This is not permitted.");
			}

			// Token: 0x060009CC RID: 2508 RVA: 0x0001CE89 File Offset: 0x0001B089
			public unsafe byte* GetUnsafePtr()
			{
				return this.m_Data->GetUnsafePtr();
			}

			// Token: 0x060009CD RID: 2509 RVA: 0x0000349B File Offset: 0x0000169B
			public bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory)
			{
				return false;
			}

			// Token: 0x060009CE RID: 2510 RVA: 0x0001CE96 File Offset: 0x0001B096
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void CheckNull(void* dataPtr)
			{
				if (dataPtr == null)
				{
					throw new InvalidOperationException("NativeText.ReadOnly has yet to be created or has been destroyed!");
				}
			}

			// Token: 0x060009CF RID: 2511 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private readonly void CheckRead()
			{
			}

			// Token: 0x060009D0 RID: 2512 RVA: 0x0001CEA8 File Offset: 0x0001B0A8
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void ErrorWrite()
			{
				throw new NotSupportedException("Trying to write to a NativeText.ReadOnly. Write operations are not permitted and are ignored.");
			}

			// Token: 0x060009D1 RID: 2513 RVA: 0x0001CEB4 File Offset: 0x0001B0B4
			[ExcludeFromBurstCompatTesting("Takes managed string")]
			public unsafe int CompareTo(string other)
			{
				return this.m_Data->ToString().CompareTo(other);
			}

			// Token: 0x060009D2 RID: 2514 RVA: 0x0001CECD File Offset: 0x0001B0CD
			[ExcludeFromBurstCompatTesting("Takes managed string")]
			public unsafe bool Equals(string other)
			{
				return this.m_Data->ToString().Equals(other);
			}

			// Token: 0x060009D3 RID: 2515 RVA: 0x0001CEE6 File Offset: 0x0001B0E6
			public unsafe int CompareTo(NativeText.ReadOnly other)
			{
				return (ref *this.m_Data).CompareTo<UnsafeText, UnsafeText>(in *other.m_Data);
			}

			// Token: 0x060009D4 RID: 2516 RVA: 0x0001CEF9 File Offset: 0x0001B0F9
			public unsafe bool Equals(NativeText.ReadOnly other)
			{
				return (ref *this.m_Data).Equals<UnsafeText, UnsafeText>(in *other.m_Data);
			}

			// Token: 0x060009D5 RID: 2517 RVA: 0x0001CF0C File Offset: 0x0001B10C
			public unsafe int CompareTo(NativeText other)
			{
				return (ref this).CompareTo<NativeText.ReadOnly, UnsafeText>(in *other.m_Data);
			}

			// Token: 0x060009D6 RID: 2518 RVA: 0x0001CF1A File Offset: 0x0001B11A
			public unsafe bool Equals(NativeText other)
			{
				return (ref this).Equals<NativeText.ReadOnly, UnsafeText>(in *other.m_Data);
			}

			// Token: 0x060009D7 RID: 2519 RVA: 0x0001CF28 File Offset: 0x0001B128
			public int CompareTo(FixedString32Bytes other)
			{
				return (ref this).CompareTo<NativeText.ReadOnly, FixedString32Bytes>(in other);
			}

			// Token: 0x060009D8 RID: 2520 RVA: 0x0001CF34 File Offset: 0x0001B134
			public unsafe static bool operator ==(in NativeText.ReadOnly a, in FixedString32Bytes b)
			{
				UnsafeText unsafeText = *a.m_Data;
				int length = unsafeText.Length;
				int utf8LengthInBytes = (int)b.utf8LengthInBytes;
				byte* unsafePtr = unsafeText.GetUnsafePtr();
				byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes30>(in b.bytes);
				return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
			}

			// Token: 0x060009D9 RID: 2521 RVA: 0x0001CF77 File Offset: 0x0001B177
			public static bool operator !=(in NativeText.ReadOnly a, in FixedString32Bytes b)
			{
				return !((in a) == (in b));
			}

			// Token: 0x060009DA RID: 2522 RVA: 0x0001CF83 File Offset: 0x0001B183
			public bool Equals(FixedString32Bytes other)
			{
				return (in this) == (in other);
			}

			// Token: 0x060009DB RID: 2523 RVA: 0x0001CF8D File Offset: 0x0001B18D
			public int CompareTo(FixedString64Bytes other)
			{
				return (ref this).CompareTo<NativeText.ReadOnly, FixedString64Bytes>(in other);
			}

			// Token: 0x060009DC RID: 2524 RVA: 0x0001CF98 File Offset: 0x0001B198
			public unsafe static bool operator ==(in NativeText.ReadOnly a, in FixedString64Bytes b)
			{
				UnsafeText unsafeText = *a.m_Data;
				int length = unsafeText.Length;
				int utf8LengthInBytes = (int)b.utf8LengthInBytes;
				byte* unsafePtr = unsafeText.GetUnsafePtr();
				byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes62>(in b.bytes);
				return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
			}

			// Token: 0x060009DD RID: 2525 RVA: 0x0001CFDB File Offset: 0x0001B1DB
			public static bool operator !=(in NativeText.ReadOnly a, in FixedString64Bytes b)
			{
				return !((in a) == (in b));
			}

			// Token: 0x060009DE RID: 2526 RVA: 0x0001CFE7 File Offset: 0x0001B1E7
			public bool Equals(FixedString64Bytes other)
			{
				return (in this) == (in other);
			}

			// Token: 0x060009DF RID: 2527 RVA: 0x0001CFF1 File Offset: 0x0001B1F1
			public int CompareTo(FixedString128Bytes other)
			{
				return (ref this).CompareTo<NativeText.ReadOnly, FixedString128Bytes>(in other);
			}

			// Token: 0x060009E0 RID: 2528 RVA: 0x0001CFFC File Offset: 0x0001B1FC
			public unsafe static bool operator ==(in NativeText.ReadOnly a, in FixedString128Bytes b)
			{
				UnsafeText unsafeText = *a.m_Data;
				int length = unsafeText.Length;
				int utf8LengthInBytes = (int)b.utf8LengthInBytes;
				byte* unsafePtr = unsafeText.GetUnsafePtr();
				byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes126>(in b.bytes);
				return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
			}

			// Token: 0x060009E1 RID: 2529 RVA: 0x0001D03F File Offset: 0x0001B23F
			public static bool operator !=(in NativeText.ReadOnly a, in FixedString128Bytes b)
			{
				return !((in a) == (in b));
			}

			// Token: 0x060009E2 RID: 2530 RVA: 0x0001D04B File Offset: 0x0001B24B
			public bool Equals(FixedString128Bytes other)
			{
				return (in this) == (in other);
			}

			// Token: 0x060009E3 RID: 2531 RVA: 0x0001D055 File Offset: 0x0001B255
			public int CompareTo(FixedString512Bytes other)
			{
				return (ref this).CompareTo<NativeText.ReadOnly, FixedString512Bytes>(in other);
			}

			// Token: 0x060009E4 RID: 2532 RVA: 0x0001D060 File Offset: 0x0001B260
			public unsafe static bool operator ==(in NativeText.ReadOnly a, in FixedString512Bytes b)
			{
				UnsafeText unsafeText = *a.m_Data;
				int length = unsafeText.Length;
				int utf8LengthInBytes = (int)b.utf8LengthInBytes;
				byte* unsafePtr = unsafeText.GetUnsafePtr();
				byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes510>(in b.bytes);
				return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
			}

			// Token: 0x060009E5 RID: 2533 RVA: 0x0001D0A3 File Offset: 0x0001B2A3
			public static bool operator !=(in NativeText.ReadOnly a, in FixedString512Bytes b)
			{
				return !((in a) == (in b));
			}

			// Token: 0x060009E6 RID: 2534 RVA: 0x0001D0AF File Offset: 0x0001B2AF
			public bool Equals(FixedString512Bytes other)
			{
				return (in this) == (in other);
			}

			// Token: 0x060009E7 RID: 2535 RVA: 0x0001D0B9 File Offset: 0x0001B2B9
			public int CompareTo(FixedString4096Bytes other)
			{
				return (ref this).CompareTo<NativeText.ReadOnly, FixedString4096Bytes>(in other);
			}

			// Token: 0x060009E8 RID: 2536 RVA: 0x0001D0C4 File Offset: 0x0001B2C4
			public unsafe static bool operator ==(in NativeText.ReadOnly a, in FixedString4096Bytes b)
			{
				UnsafeText unsafeText = *a.m_Data;
				int length = unsafeText.Length;
				int utf8LengthInBytes = (int)b.utf8LengthInBytes;
				byte* unsafePtr = unsafeText.GetUnsafePtr();
				byte* ptr = (byte*)UnsafeUtilityExtensions.AddressOf<FixedBytes4094>(in b.bytes);
				return UTF8ArrayUnsafeUtility.EqualsUTF8Bytes(unsafePtr, length, ptr, utf8LengthInBytes);
			}

			// Token: 0x060009E9 RID: 2537 RVA: 0x0001D107 File Offset: 0x0001B307
			public static bool operator !=(in NativeText.ReadOnly a, in FixedString4096Bytes b)
			{
				return !((in a) == (in b));
			}

			// Token: 0x060009EA RID: 2538 RVA: 0x0001D113 File Offset: 0x0001B313
			public bool Equals(FixedString4096Bytes other)
			{
				return (in this) == (in other);
			}

			// Token: 0x060009EB RID: 2539 RVA: 0x0001D11D File Offset: 0x0001B31D
			[ExcludeFromBurstCompatTesting("Returns managed string")]
			public override string ToString()
			{
				if (this.m_Data == null)
				{
					return "";
				}
				return (ref this).ConvertToString<NativeText.ReadOnly>();
			}

			// Token: 0x060009EC RID: 2540 RVA: 0x0001D135 File Offset: 0x0001B335
			public override int GetHashCode()
			{
				return (ref this).ComputeHashCode<NativeText.ReadOnly>();
			}

			// Token: 0x060009ED RID: 2541 RVA: 0x0001D140 File Offset: 0x0001B340
			[ExcludeFromBurstCompatTesting("Takes managed object")]
			public override bool Equals(object other)
			{
				if (other == null)
				{
					return false;
				}
				string text = other as string;
				if (text != null)
				{
					return this.Equals(text);
				}
				if (other is NativeText)
				{
					NativeText nativeText = (NativeText)other;
					return this.Equals(nativeText);
				}
				if (other is NativeText.ReadOnly)
				{
					NativeText.ReadOnly readOnly = (NativeText.ReadOnly)other;
					return this.Equals(readOnly);
				}
				if (other is FixedString32Bytes)
				{
					FixedString32Bytes fixedString32Bytes = (FixedString32Bytes)other;
					return this.Equals(fixedString32Bytes);
				}
				if (other is FixedString64Bytes)
				{
					FixedString64Bytes fixedString64Bytes = (FixedString64Bytes)other;
					return this.Equals(fixedString64Bytes);
				}
				if (other is FixedString128Bytes)
				{
					FixedString128Bytes fixedString128Bytes = (FixedString128Bytes)other;
					return this.Equals(fixedString128Bytes);
				}
				if (other is FixedString512Bytes)
				{
					FixedString512Bytes fixedString512Bytes = (FixedString512Bytes)other;
					return this.Equals(fixedString512Bytes);
				}
				if (other is FixedString4096Bytes)
				{
					FixedString4096Bytes fixedString4096Bytes = (FixedString4096Bytes)other;
					return this.Equals(fixedString4096Bytes);
				}
				return false;
			}

			// Token: 0x1700011D RID: 285
			// (get) Token: 0x060009EE RID: 2542 RVA: 0x0001D20E File Offset: 0x0001B40E
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

			// Token: 0x060009EF RID: 2543 RVA: 0x0001D21C File Offset: 0x0001B41C
			public NativeText.Enumerator GetEnumerator()
			{
				return new NativeText.Enumerator(this);
			}

			// Token: 0x04000413 RID: 1043
			[NativeDisableUnsafePtrRestriction]
			internal unsafe UnsafeText* m_Data;
		}
	}
}
