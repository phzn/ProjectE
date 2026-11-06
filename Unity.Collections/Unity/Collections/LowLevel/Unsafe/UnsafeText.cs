using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Jobs;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x0200014A RID: 330
	[GenerateTestsForBurstCompatibility]
	[DebuggerDisplay("Length = {Length}, Capacity = {Capacity}, IsCreated = {IsCreated}, IsEmpty = {IsEmpty}")]
	public struct UnsafeText : INativeDisposable, IDisposable, IUTF8Bytes, INativeList<byte>, IIndexable<byte>
	{
		// Token: 0x06000DBA RID: 3514 RVA: 0x0002A3F8 File Offset: 0x000285F8
		public unsafe UnsafeText(int capacity, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_UntypedListData = default(UntypedUnsafeList);
			*(ref this).AsUnsafeListOfBytes() = new UnsafeList<byte>(capacity + 1, allocator, NativeArrayOptions.UninitializedMemory);
			this.Length = 0;
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0002A424 File Offset: 0x00028624
		public readonly bool IsCreated
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.AsUnsafeListOfBytesRO().IsCreated;
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0002A444 File Offset: 0x00028644
		internal unsafe static UnsafeText* Alloc(AllocatorManager.AllocatorHandle allocator)
		{
			return (UnsafeText*)Memory.Unmanaged.Allocate((long)sizeof(UnsafeText), UnsafeUtility.AlignOf<UnsafeText>(), allocator);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0002A468 File Offset: 0x00028668
		internal unsafe static void Free(UnsafeText* data)
		{
			if (data == null)
			{
				throw new InvalidOperationException("UnsafeText has yet to be created or has been destroyed!");
			}
			AllocatorManager.AllocatorHandle allocator = data->m_UntypedListData.Allocator;
			data->Dispose();
			Memory.Unmanaged.Free<UnsafeText>(data, allocator);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0002A49E File Offset: 0x0002869E
		public void Dispose()
		{
			(ref this).AsUnsafeListOfBytes().Dispose();
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0002A4AB File Offset: 0x000286AB
		public JobHandle Dispose(JobHandle inputDeps)
		{
			return (ref this).AsUnsafeListOfBytes().Dispose(inputDeps);
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0002A4B9 File Offset: 0x000286B9
		public readonly bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return !this.IsCreated || this.Length == 0;
			}
		}

		// Token: 0x170001A1 RID: 417
		public byte this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return UnsafeUtility.ReadArrayElement<byte>(this.m_UntypedListData.Ptr, index);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				UnsafeUtility.WriteArrayElement<byte>(this.m_UntypedListData.Ptr, index, value);
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0002A4F5 File Offset: 0x000286F5
		public ref byte ElementAt(int index)
		{
			return UnsafeUtility.ArrayElementAsRef<byte>(this.m_UntypedListData.Ptr, index);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0002A508 File Offset: 0x00028708
		public void Clear()
		{
			this.Length = 0;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0002A511 File Offset: 0x00028711
		public unsafe byte* GetUnsafePtr()
		{
			return (byte*)this.m_UntypedListData.Ptr;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0002A51E File Offset: 0x0002871E
		public bool TryResize(int newLength, NativeArrayOptions clearOptions = NativeArrayOptions.ClearMemory)
		{
			(ref this).AsUnsafeListOfBytes().Resize(newLength + 1, clearOptions);
			(ref this).AsUnsafeListOfBytes()[newLength] = 0;
			return true;
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x0002A540 File Offset: 0x00028740
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x0002A562 File Offset: 0x00028762
		public int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.AsUnsafeListOfBytesRO().Capacity - 1;
			}
			set
			{
				(ref this).AsUnsafeListOfBytes().SetCapacity(value + 1);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0002A574 File Offset: 0x00028774
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x0002A596 File Offset: 0x00028796
		public int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get
			{
				return this.AsUnsafeListOfBytesRO().Length - 1;
			}
			set
			{
				(ref this).AsUnsafeListOfBytes().Resize(value + 1, NativeArrayOptions.UninitializedMemory);
				(ref this).AsUnsafeListOfBytes()[value] = 0;
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0002A5B4 File Offset: 0x000287B4
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public override string ToString()
		{
			if (!this.IsCreated)
			{
				return "";
			}
			return (ref this).ConvertToString<UnsafeText>();
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0002A5CC File Offset: 0x000287CC
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckIndexInRange(int index)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} must be positive.", index));
			}
			if (index >= this.Length)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range in UnsafeText of {1} length.", index, this.Length));
			}
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0002A61D File Offset: 0x0002881D
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void ThrowCopyError(CopyError error, string source)
		{
			throw new ArgumentException(string.Format("UnsafeText: {0} while copying \"{1}\"", error, source));
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0002A635 File Offset: 0x00028835
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void CheckCapacityInRange(int value, int length)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException(string.Format("Value {0} must be positive.", value));
			}
			if (value < length)
			{
				throw new ArgumentOutOfRangeException(string.Format("Value {0} is out of range in NativeList of '{1}' Length.", value, length));
			}
		}

		// Token: 0x0400054B RID: 1355
		internal UntypedUnsafeList m_UntypedListData;
	}
}
