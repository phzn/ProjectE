using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000012 RID: 18
	public ref struct BlobBuilderArray<T> where T : struct
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002300 File Offset: 0x00000500
		public unsafe BlobBuilderArray(void* data, int length)
		{
			this.m_data = data;
			this.m_length = length;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002310 File Offset: 0x00000510
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckIndexOutOfRange(int index)
		{
			if (0 > index || index >= this.m_length)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of '{1}' Length.", index, this.m_length));
			}
		}

		// Token: 0x17000003 RID: 3
		public ref T this[int index]
		{
			get
			{
				return UnsafeUtility.ArrayElementAsRef<T>(this.m_data, index);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000234E File Offset: 0x0000054E
		public int Length
		{
			get
			{
				return this.m_length;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002356 File Offset: 0x00000556
		public unsafe void* GetUnsafePtr()
		{
			return this.m_data;
		}

		// Token: 0x0400000E RID: 14
		private unsafe void* m_data;

		// Token: 0x0400000F RID: 15
		private int m_length;
	}
}
