using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000089 RID: 137
	internal struct Fixed2<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x0600054E RID: 1358 RVA: 0x0001F159 File Offset: 0x0001D359
		public Fixed2(T item1)
		{
			this = new Fixed2<T>(item1, item1);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001F163 File Offset: 0x0001D363
		public Fixed2(T item1, T item2)
		{
			this.item1 = item1;
			this.item2 = item2;
		}

		// Token: 0x17000176 RID: 374
		public unsafe T this[int index]
		{
			get
			{
				fixed (T* ptr = &this.item1)
				{
					return ptr[(IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
				}
			}
			set
			{
				fixed (T* ptr = &this.item1)
				{
					ptr[(IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = value;
				}
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001F1C5 File Offset: 0x0001D3C5
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private static void CheckRange(int index)
		{
		}

		// Token: 0x040003C8 RID: 968
		public T item1;

		// Token: 0x040003C9 RID: 969
		public T item2;
	}
}
