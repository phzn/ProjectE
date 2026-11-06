using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x02000089 RID: 137
	[NativeContainer]
	[DebuggerDisplay("Length = {Length}, IsCreated = {IsCreated}")]
	[GenerateTestsForBurstCompatibility]
	public struct NativeBitArray : INativeDisposable, IDisposable
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x00017275 File Offset: 0x00015475
		public unsafe NativeBitArray(int numBits, AllocatorManager.AllocatorHandle allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
		{
			this.m_BitArray = UnsafeBitArray.Alloc(allocator);
			this.m_Allocator = allocator;
			*this.m_BitArray = new UnsafeBitArray(numBits, allocator, options);
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001729D File Offset: 0x0001549D
		public unsafe readonly bool IsCreated
		{
			get
			{
				return this.m_BitArray != null && this.m_BitArray->IsCreated;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x000172B6 File Offset: 0x000154B6
		public readonly bool IsEmpty
		{
			get
			{
				return !this.IsCreated || this.Length == 0;
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000172CB File Offset: 0x000154CB
		public unsafe void Resize(int numBits, NativeArrayOptions options = NativeArrayOptions.UninitializedMemory)
		{
			this.m_BitArray->Resize(numBits, options);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x000172DA File Offset: 0x000154DA
		public unsafe void SetCapacity(int capacityInBits)
		{
			this.m_BitArray->SetCapacity(capacityInBits);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x000172E8 File Offset: 0x000154E8
		public unsafe void TrimExcess()
		{
			this.m_BitArray->TrimExcess();
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000172F5 File Offset: 0x000154F5
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				return;
			}
			UnsafeBitArray.Free(this.m_BitArray, this.m_Allocator);
			this.m_BitArray = null;
			this.m_Allocator = AllocatorManager.Invalid;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00017324 File Offset: 0x00015524
		public JobHandle Dispose(JobHandle inputDeps)
		{
			if (!this.IsCreated)
			{
				return inputDeps;
			}
			JobHandle jobHandle = new NativeBitArrayDisposeJob
			{
				Data = new NativeBitArrayDispose
				{
					m_BitArrayData = this.m_BitArray,
					m_Allocator = this.m_Allocator
				}
			}.Schedule(inputDeps);
			this.m_BitArray = null;
			this.m_Allocator = AllocatorManager.Invalid;
			return jobHandle;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00017387 File Offset: 0x00015587
		public unsafe readonly int Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return CollectionHelper.AssumePositive(this.m_BitArray->Length);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x00017399 File Offset: 0x00015599
		public unsafe readonly int Capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return CollectionHelper.AssumePositive(this.m_BitArray->Capacity);
			}
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x000173AB File Offset: 0x000155AB
		public unsafe void Clear()
		{
			this.m_BitArray->Clear();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000173B8 File Offset: 0x000155B8
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(int) })]
		public unsafe NativeArray<T> AsNativeArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<T>() * 8;
			int num2 = this.m_BitArray->Length / num;
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.m_BitArray->Ptr, num2, Allocator.None);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x000173ED File Offset: 0x000155ED
		public unsafe void Set(int pos, bool value)
		{
			this.m_BitArray->Set(pos, value);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000173FC File Offset: 0x000155FC
		public unsafe void SetBits(int pos, bool value, int numBits)
		{
			this.m_BitArray->SetBits(pos, value, numBits);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001740C File Offset: 0x0001560C
		public unsafe void SetBits(int pos, ulong value, int numBits = 1)
		{
			this.m_BitArray->SetBits(pos, value, numBits);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001741C File Offset: 0x0001561C
		public unsafe ulong GetBits(int pos, int numBits = 1)
		{
			return this.m_BitArray->GetBits(pos, numBits);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001742B File Offset: 0x0001562B
		public unsafe bool IsSet(int pos)
		{
			return this.m_BitArray->IsSet(pos);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00017439 File Offset: 0x00015639
		public unsafe void Copy(int dstPos, int srcPos, int numBits)
		{
			this.m_BitArray->Copy(dstPos, srcPos, numBits);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00017449 File Offset: 0x00015649
		public unsafe void Copy(int dstPos, ref NativeBitArray srcBitArray, int srcPos, int numBits)
		{
			this.m_BitArray->Copy(dstPos, ref *srcBitArray.m_BitArray, srcPos, numBits);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00017460 File Offset: 0x00015660
		public unsafe int Find(int pos, int numBits)
		{
			return this.m_BitArray->Find(pos, numBits);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001746F File Offset: 0x0001566F
		public unsafe int Find(int pos, int count, int numBits)
		{
			return this.m_BitArray->Find(pos, count, numBits);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001747F File Offset: 0x0001567F
		public unsafe bool TestNone(int pos, int numBits = 1)
		{
			return this.m_BitArray->TestNone(pos, numBits);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001748E File Offset: 0x0001568E
		public unsafe bool TestAny(int pos, int numBits = 1)
		{
			return this.m_BitArray->TestAny(pos, numBits);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001749D File Offset: 0x0001569D
		public unsafe bool TestAll(int pos, int numBits = 1)
		{
			return this.m_BitArray->TestAll(pos, numBits);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x000174AC File Offset: 0x000156AC
		public unsafe int CountBits(int pos, int numBits = 1)
		{
			return this.m_BitArray->CountBits(pos, numBits);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000174BB File Offset: 0x000156BB
		public NativeBitArray.ReadOnly AsReadOnly()
		{
			return new NativeBitArray.ReadOnly(ref this);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private readonly void CheckRead()
		{
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000174C4 File Offset: 0x000156C4
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void CheckReadBounds<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<T>() * 8;
			int num2 = this.m_BitArray->Length / num;
			if (num2 == 0)
			{
				throw new InvalidOperationException(string.Format("Number of bits in the NativeBitArray {0} is not sufficient to cast to NativeArray<T> {1}.", this.m_BitArray->Length, UnsafeUtility.SizeOf<T>() * 8));
			}
			if (this.m_BitArray->Length != num * num2)
			{
				throw new InvalidOperationException(string.Format("Number of bits in the NativeBitArray {0} couldn't hold multiple of T {1}. Output array would be truncated.", this.m_BitArray->Length, UnsafeUtility.SizeOf<T>()));
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00002C47 File Offset: 0x00000E47
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void CheckWrite()
		{
		}

		// Token: 0x040003AF RID: 943
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeBitArray* m_BitArray;

		// Token: 0x040003B0 RID: 944
		internal AllocatorManager.AllocatorHandle m_Allocator;

		// Token: 0x0200008A RID: 138
		[NativeContainer]
		[NativeContainerIsReadOnly]
		public struct ReadOnly
		{
			// Token: 0x170000AE RID: 174
			// (get) Token: 0x0600071A RID: 1818 RVA: 0x00017550 File Offset: 0x00015750
			public readonly bool IsCreated
			{
				get
				{
					return this.m_BitArray.IsCreated;
				}
			}

			// Token: 0x170000AF RID: 175
			// (get) Token: 0x0600071B RID: 1819 RVA: 0x0001755D File Offset: 0x0001575D
			public readonly bool IsEmpty
			{
				get
				{
					return this.m_BitArray.IsEmpty;
				}
			}

			// Token: 0x0600071C RID: 1820 RVA: 0x0001756A File Offset: 0x0001576A
			internal unsafe ReadOnly(ref NativeBitArray data)
			{
				this.m_BitArray = data.m_BitArray->AsReadOnly();
			}

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x0600071D RID: 1821 RVA: 0x0001757D File Offset: 0x0001577D
			public readonly int Length
			{
				get
				{
					return CollectionHelper.AssumePositive(this.m_BitArray.Length);
				}
			}

			// Token: 0x0600071E RID: 1822 RVA: 0x0001758F File Offset: 0x0001578F
			public readonly ulong GetBits(int pos, int numBits = 1)
			{
				return this.m_BitArray.GetBits(pos, numBits);
			}

			// Token: 0x0600071F RID: 1823 RVA: 0x0001759E File Offset: 0x0001579E
			public readonly bool IsSet(int pos)
			{
				return this.m_BitArray.IsSet(pos);
			}

			// Token: 0x06000720 RID: 1824 RVA: 0x000175AC File Offset: 0x000157AC
			public readonly int Find(int pos, int numBits)
			{
				return this.m_BitArray.Find(pos, numBits);
			}

			// Token: 0x06000721 RID: 1825 RVA: 0x000175BB File Offset: 0x000157BB
			public readonly int Find(int pos, int count, int numBits)
			{
				return this.m_BitArray.Find(pos, count, numBits);
			}

			// Token: 0x06000722 RID: 1826 RVA: 0x000175CB File Offset: 0x000157CB
			public readonly bool TestNone(int pos, int numBits = 1)
			{
				return this.m_BitArray.TestNone(pos, numBits);
			}

			// Token: 0x06000723 RID: 1827 RVA: 0x000175DA File Offset: 0x000157DA
			public readonly bool TestAny(int pos, int numBits = 1)
			{
				return this.m_BitArray.TestAny(pos, numBits);
			}

			// Token: 0x06000724 RID: 1828 RVA: 0x000175E9 File Offset: 0x000157E9
			public readonly bool TestAll(int pos, int numBits = 1)
			{
				return this.m_BitArray.TestAll(pos, numBits);
			}

			// Token: 0x06000725 RID: 1829 RVA: 0x000175F8 File Offset: 0x000157F8
			public readonly int CountBits(int pos, int numBits = 1)
			{
				return this.m_BitArray.CountBits(pos, numBits);
			}

			// Token: 0x06000726 RID: 1830 RVA: 0x00002C47 File Offset: 0x00000E47
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private readonly void CheckRead()
			{
			}

			// Token: 0x040003B1 RID: 945
			[NativeDisableUnsafePtrRestriction]
			internal UnsafeBitArray.ReadOnly m_BitArray;
		}
	}
}
