using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000069 RID: 105
	internal abstract class DecalChunk : IDisposable
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0001691C File Offset: 0x00014B1C
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x00016924 File Offset: 0x00014B24
		public int count { get; protected set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0001692D File Offset: 0x00014B2D
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x00016935 File Offset: 0x00014B35
		public int capacity { get; protected set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0001693E File Offset: 0x00014B3E
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x00016946 File Offset: 0x00014B46
		public JobHandle currentJobHandle { get; set; }

		// Token: 0x060003F0 RID: 1008 RVA: 0x00016950 File Offset: 0x00014B50
		public virtual void Push()
		{
			int count = this.count;
			this.count = count + 1;
		}

		// Token: 0x060003F1 RID: 1009
		public abstract void RemoveAtSwapBack(int index);

		// Token: 0x060003F2 RID: 1010
		public abstract void SetCapacity(int capacity);

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001696D File Offset: 0x00014B6D
		public virtual void Dispose()
		{
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00016970 File Offset: 0x00014B70
		protected void ResizeNativeArray(ref TransformAccessArray array, DecalProjector[] decalProjectors, int capacity)
		{
			TransformAccessArray transformAccessArray = new TransformAccessArray(capacity, -1);
			if (array.isCreated)
			{
				for (int i = 0; i < array.length; i++)
				{
					transformAccessArray.Add(decalProjectors[i].transform);
				}
				array.Dispose();
			}
			array = transformAccessArray;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000169BB File Offset: 0x00014BBB
		protected void RemoveAtSwapBack<T>(ref NativeArray<T> array, int index, int count) where T : struct
		{
			array[index] = array[count - 1];
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000169CD File Offset: 0x00014BCD
		protected void RemoveAtSwapBack<T>(ref T[] array, int index, int count)
		{
			array[index] = array[count - 1];
		}
	}
}
