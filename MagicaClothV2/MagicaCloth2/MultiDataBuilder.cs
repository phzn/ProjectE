using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace MagicaCloth2
{
	// Token: 0x020000ED RID: 237
	public class MultiDataBuilder<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x0002A359 File Offset: 0x00028559
		public MultiDataBuilder(int indexCount, int dataCapacity)
		{
			this.indexCount = indexCount;
			this.Map = new NativeParallelMultiHashMap<int, T>(dataCapacity, Allocator.Persistent);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0002A37A File Offset: 0x0002857A
		public void Dispose()
		{
			if (this.Map.IsCreated)
			{
				this.Map.Dispose();
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0002A394 File Offset: 0x00028594
		public int Count()
		{
			return this.Map.Count();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0002A3A1 File Offset: 0x000285A1
		public int GetDataCount(int index)
		{
			if (!this.Map.ContainsKey(index))
			{
				return 0;
			}
			return this.Map.CountValuesForKey(index);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0002A3BF File Offset: 0x000285BF
		public void Add(int key, T data)
		{
			this.Map.Add(key, data);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0002A3CE File Offset: 0x000285CE
		public int CountValuesForKey(int key)
		{
			return this.Map.CountValuesForKey(key);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0002A3DC File Offset: 0x000285DC
		public ValueTuple<T[], uint[]> ToArray()
		{
			if (!this.Map.IsCreated || this.indexCount == 0)
			{
				return new ValueTuple<T[], uint[]>(null, null);
			}
			uint[] array = new uint[this.indexCount];
			List<T> list = new List<T>(this.Map.Capacity);
			for (int i = 0; i < this.indexCount; i++)
			{
				int count = list.Count;
				int num = 0;
				if (this.Map.ContainsKey(i))
				{
					foreach (T t in this.Map.GetValuesForKey(i))
					{
						list.Add(t);
						num++;
					}
				}
				array[i] = DataUtility.Pack12_20(num, count);
			}
			return new ValueTuple<T[], uint[]>(list.ToArray(), array);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0002A4BC File Offset: 0x000286BC
		public uint[] ToIndexArray()
		{
			return this.ToArray().Item2;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0002A4CC File Offset: 0x000286CC
		public void ToNativeArray(out NativeArray<uint> indexArray, out NativeArray<T> dataArray)
		{
			indexArray = new NativeArray<uint>(this.indexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			List<T> list = new List<T>(this.Map.Capacity);
			for (int i = 0; i < this.indexCount; i++)
			{
				int count = list.Count;
				int num = 0;
				if (this.Map.ContainsKey(i))
				{
					foreach (T t in this.Map.GetValuesForKey(i))
					{
						list.Add(t);
						num++;
					}
				}
				indexArray[i] = DataUtility.Pack12_20(num, count);
			}
			dataArray = new NativeArray<T>(list.ToArray(), Allocator.Persistent);
		}

		// Token: 0x04000860 RID: 2144
		private int indexCount;

		// Token: 0x04000861 RID: 2145
		public NativeParallelMultiHashMap<int, T> Map;
	}
}
