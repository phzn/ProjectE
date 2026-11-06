using System;

namespace System.Buffers
{
	// Token: 0x02000AD1 RID: 2769
	public abstract class ArrayPool<T>
	{
		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x060062CD RID: 25293 RVA: 0x0014A578 File Offset: 0x00148778
		public static ArrayPool<T> Shared { get; } = new TlsOverPerCoreLockedStacksArrayPool<T>();

		// Token: 0x060062CE RID: 25294 RVA: 0x0014A57F File Offset: 0x0014877F
		public static ArrayPool<T> Create()
		{
			return new ConfigurableArrayPool<T>();
		}

		// Token: 0x060062CF RID: 25295 RVA: 0x0014A586 File Offset: 0x00148786
		public static ArrayPool<T> Create(int maxArrayLength, int maxArraysPerBucket)
		{
			return new ConfigurableArrayPool<T>(maxArrayLength, maxArraysPerBucket);
		}

		// Token: 0x060062D0 RID: 25296
		public abstract T[] Rent(int minimumLength);

		// Token: 0x060062D1 RID: 25297
		public abstract void Return(T[] array, bool clearArray = false);
	}
}
