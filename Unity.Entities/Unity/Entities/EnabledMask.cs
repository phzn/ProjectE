using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x0200020C RID: 524
	public struct EnabledMask
	{
		// Token: 0x06000C4C RID: 3148 RVA: 0x00038815 File Offset: 0x00036A15
		internal unsafe EnabledMask(SafeBitRef enableBitRef, int* ptrChunkDisabledCount)
		{
			this.m_EnableBitRef = enableBitRef;
			this.m_PtrChunkDisabledCount = ptrChunkDisabledCount;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00038825 File Offset: 0x00036A25
		public SafeBitRef EnableBit
		{
			get
			{
				return this.m_EnableBitRef;
			}
		}

		// Token: 0x170000EA RID: 234
		public unsafe bool this[int index]
		{
			get
			{
				return this.m_EnableBitRef.Offset(index).GetBit();
			}
			set
			{
				bool bit = this.m_EnableBitRef.Offset(index).GetBit();
				this.m_EnableBitRef.Offset(index).SetBit(value);
				if (Hint.Likely(bit != value))
				{
					int num = math.select(1, -1, value);
					Interlocked.Add(UnsafeUtility.AsRef<int>((void*)this.m_PtrChunkDisabledCount), num);
				}
			}
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x000388B4 File Offset: 0x00036AB4
		public bool GetBit(int index)
		{
			return this.m_EnableBitRef.Offset(index).GetBit();
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x000388D5 File Offset: 0x00036AD5
		public EnabledRefRW<T> GetEnabledRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int index) where T : struct, ValueType, IEnableableComponent
		{
			return new EnabledRefRW<T>(this.m_EnableBitRef.Offset(index), this.m_PtrChunkDisabledCount);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x000388EE File Offset: 0x00036AEE
		public EnabledRefRO<T> GetEnabledRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int index) where T : struct, ValueType, IEnableableComponent
		{
			return new EnabledRefRO<T>(this.m_EnableBitRef.Offset(index));
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00038901 File Offset: 0x00036B01
		public EnabledRefRW<T> GetOptionalEnabledRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int index) where T : struct, ValueType, IEnableableComponent
		{
			if (this.m_EnableBitRef.IsValid)
			{
				return new EnabledRefRW<T>(this.m_EnableBitRef.Offset(index), this.m_PtrChunkDisabledCount);
			}
			return EnabledRefRW<T>.Null;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0003892D File Offset: 0x00036B2D
		public EnabledRefRO<T> GetOptionalEnabledRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int index) where T : struct, ValueType, IEnableableComponent
		{
			if (this.m_EnableBitRef.IsValid)
			{
				return new EnabledRefRO<T>(this.m_EnableBitRef.Offset(index));
			}
			return EnabledRefRO<T>.Null;
		}

		// Token: 0x04000560 RID: 1376
		private readonly SafeBitRef m_EnableBitRef;

		// Token: 0x04000561 RID: 1377
		private unsafe readonly int* m_PtrChunkDisabledCount;
	}
}
