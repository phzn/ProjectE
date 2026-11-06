using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x0200009E RID: 158
	internal sealed class NativeListDebugView<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x060007D0 RID: 2000 RVA: 0x000183CB File Offset: 0x000165CB
		public NativeListDebugView(NativeList<T> array)
		{
			this.Data = array.m_ListData;
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x000183E0 File Offset: 0x000165E0
		public unsafe T[] Items
		{
			get
			{
				if (this.Data == null)
				{
					return null;
				}
				int length = this.Data->Length;
				T[] array = new T[length];
				fixed (T* ptr = &array[0])
				{
					UnsafeUtility.MemCpy((void*)ptr, (void*)this.Data->Ptr, (long)(length * UnsafeUtility.SizeOf<T>()));
				}
				return array;
			}
		}

		// Token: 0x040003C7 RID: 967
		private unsafe UnsafeList<T>* Data;
	}
}
