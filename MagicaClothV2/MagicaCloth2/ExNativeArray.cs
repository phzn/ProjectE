using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x0200010F RID: 271
	public class ExNativeArray<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
	{
		// Token: 0x06000523 RID: 1315 RVA: 0x0002DD23 File Offset: 0x0002BF23
		public void Dispose()
		{
			if (this.nativeArray.IsCreated)
			{
				this.nativeArray.Dispose();
			}
			this.emptyChunks.Clear();
			this.useCount = 0;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0002DD4F File Offset: 0x0002BF4F
		public bool IsValid
		{
			get
			{
				return this.nativeArray.IsCreated;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0002DD5C File Offset: 0x0002BF5C
		public int Length
		{
			get
			{
				if (!this.nativeArray.IsCreated)
				{
					return 0;
				}
				return this.nativeArray.Length;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0002DD78 File Offset: 0x0002BF78
		public int Count
		{
			get
			{
				return this.useCount;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0002DD80 File Offset: 0x0002BF80
		public ExNativeArray()
		{
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0002DD94 File Offset: 0x0002BF94
		public ExNativeArray(int emptyLength, bool create = false)
			: this()
		{
			if (emptyLength > 0)
			{
				this.nativeArray = new NativeArray<T>(emptyLength, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				DataChunk dataChunk = new DataChunk(0, emptyLength);
				this.emptyChunks.Add(dataChunk);
				if (create)
				{
					this.AddRange(emptyLength);
					return;
				}
			}
			else if (create)
			{
				this.nativeArray = new NativeArray<T>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0002DDEB File Offset: 0x0002BFEB
		public ExNativeArray(int emptyLength, T fillData)
			: this(emptyLength, false)
		{
			if (emptyLength > 0)
			{
				this.Fill(fillData);
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0002DE00 File Offset: 0x0002C000
		public ExNativeArray(NativeArray<T> dataArray)
			: this()
		{
			this.AddRange<T>(dataArray);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0002DE10 File Offset: 0x0002C010
		public ExNativeArray(T[] dataArray)
			: this()
		{
			this.AddRange(dataArray);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0002DE20 File Offset: 0x0002C020
		public DataChunk AddRange(int dataLength)
		{
			if (dataLength == 0)
			{
				if (!this.nativeArray.IsCreated)
				{
					this.nativeArray = new NativeArray<T>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}
				return DataChunk.Empty;
			}
			DataChunk emptyChunk = this.GetEmptyChunk(dataLength);
			if (!emptyChunk.IsValid)
			{
				int length = this.Length;
				int num = this.Length + math.max(dataLength, length);
				if (length == 0)
				{
					if (this.nativeArray.IsCreated)
					{
						this.nativeArray.Dispose();
					}
					this.nativeArray = new NativeArray<T>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
					emptyChunk.dataLength = dataLength;
				}
				else
				{
					NativeArray<T> nativeArray = new NativeArray<T>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
					NativeArray<T>.Copy(this.nativeArray, nativeArray, length);
					this.nativeArray.Dispose();
					this.nativeArray = nativeArray;
					emptyChunk.startIndex = length;
					emptyChunk.dataLength = dataLength;
					int num2 = length + dataLength;
					if (num2 < num)
					{
						DataChunk dataChunk = new DataChunk(num2, num - num2);
						this.AddEmptyChunk(dataChunk);
					}
				}
			}
			this.useCount = math.max(this.useCount, emptyChunk.startIndex + emptyChunk.dataLength);
			return emptyChunk;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0002DF28 File Offset: 0x0002C128
		public DataChunk AddRange(int dataLength, T fillData = default(T))
		{
			DataChunk dataChunk = this.AddRange(dataLength);
			this.Fill(dataChunk, fillData);
			return dataChunk;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0002DF48 File Offset: 0x0002C148
		public DataChunk AddRange(T[] array)
		{
			if (array == null || array.Length == 0)
			{
				return DataChunk.Empty;
			}
			int num = array.Length;
			DataChunk dataChunk = this.AddRange(num);
			NativeArray<T>.Copy(array, 0, this.nativeArray, dataChunk.startIndex, num);
			return dataChunk;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0002DF84 File Offset: 0x0002C184
		public DataChunk AddRange(NativeArray<T> narray, int length = 0)
		{
			if (!narray.IsCreated || narray.Length == 0)
			{
				return DataChunk.Empty;
			}
			int num = ((length > 0) ? length : narray.Length);
			DataChunk dataChunk = this.AddRange(num);
			NativeArray<T>.Copy(narray, 0, this.nativeArray, dataChunk.startIndex, num);
			return dataChunk;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0002DFD5 File Offset: 0x0002C1D5
		public DataChunk AddRange(ExNativeArray<T> exarray)
		{
			return this.AddRange(exarray.GetNativeArray(), exarray.Count);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0002DFE9 File Offset: 0x0002C1E9
		public DataChunk AddRange(ExSimpleNativeArray<T> exarray)
		{
			return this.AddRange(exarray.GetNativeArray(), exarray.Count);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0002E000 File Offset: 0x0002C200
		public unsafe DataChunk AddRange<U>(U[] array) where U : struct
		{
			if (array == null || array.Length == 0)
			{
				return DataChunk.Empty;
			}
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = array.Length;
			DataChunk dataChunk = this.AddRange(num2);
			ulong num3;
			void* ptr = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num3);
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			UnsafeUtility.MemCpy((void*)(unsafePtr + dataChunk.startIndex * num), ptr, (long)(num2 * num));
			UnsafeUtility.ReleaseGCObject(num3);
			return dataChunk;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0002E060 File Offset: 0x0002C260
		public DataChunk AddRange<U>(NativeArray<U> udata) where U : struct
		{
			if (!udata.IsCreated || udata.Length == 0)
			{
				return DataChunk.Empty;
			}
			int length = udata.Length;
			DataChunk dataChunk = this.AddRange(length);
			NativeArray<T>.Copy(udata.Reinterpret<T>(), 0, this.nativeArray, dataChunk.startIndex, length);
			return dataChunk;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0002E0B0 File Offset: 0x0002C2B0
		public unsafe DataChunk AddRangeTypeChange<U>(U[] array) where U : struct
		{
			if (array == null || array.Length == 0)
			{
				return DataChunk.Empty;
			}
			int num = UnsafeUtility.SizeOf<U>();
			int num2 = UnsafeUtility.SizeOf<T>();
			int num3 = array.Length * num / num2;
			DataChunk dataChunk = this.AddRange(num3);
			ulong num4;
			void* ptr = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num4);
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			UnsafeUtility.MemCpy((void*)(unsafePtr + dataChunk.startIndex * num2), ptr, (long)(num3 * num2));
			UnsafeUtility.ReleaseGCObject(num4);
			return dataChunk;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0002E11C File Offset: 0x0002C31C
		public unsafe DataChunk AddRangeStride<U>(U[] array) where U : struct
		{
			if (array == null || array.Length == 0)
			{
				return DataChunk.Empty;
			}
			int num = UnsafeUtility.SizeOf<U>();
			int num2 = UnsafeUtility.SizeOf<T>();
			int num3 = array.Length;
			DataChunk dataChunk = this.AddRange(num3);
			ulong num4;
			void* ptr = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num4);
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			int num5 = math.min(num, num2);
			UnsafeUtility.MemCpyStride((void*)(unsafePtr + dataChunk.startIndex * num2), num2, ptr, num, num5, num3);
			UnsafeUtility.ReleaseGCObject(num4);
			return dataChunk;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0002E190 File Offset: 0x0002C390
		public DataChunk Add(T data)
		{
			DataChunk dataChunk = this.AddRange(1);
			this.nativeArray[dataChunk.startIndex] = data;
			return dataChunk;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0002E1B8 File Offset: 0x0002C3B8
		public DataChunk Expand(DataChunk c, int newDataLength)
		{
			if (!c.IsValid)
			{
				return c;
			}
			if (newDataLength <= c.dataLength)
			{
				return c;
			}
			DataChunk dataChunk = this.AddRange(newDataLength);
			NativeArray<T>.Copy(this.nativeArray, c.startIndex, this.nativeArray, dataChunk.startIndex, c.dataLength);
			this.Remove(c);
			return dataChunk;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0002E210 File Offset: 0x0002C410
		public DataChunk ExpandAndFill(DataChunk c, int newDataLength, T fillData = default(T), T clearData = default(T))
		{
			if (!c.IsValid)
			{
				return c;
			}
			if (newDataLength <= c.dataLength)
			{
				return c;
			}
			DataChunk dataChunk = this.AddRange(newDataLength, fillData);
			NativeArray<T>.Copy(this.nativeArray, c.startIndex, this.nativeArray, dataChunk.startIndex, c.dataLength);
			this.RemoveAndFill(c, clearData);
			return dataChunk;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0002E269 File Offset: 0x0002C469
		public T[] ToArray()
		{
			return this.nativeArray.ToArray();
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0002E276 File Offset: 0x0002C476
		public void CopyTo(T[] array)
		{
			NativeArray<T>.Copy(this.nativeArray, array);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0002E284 File Offset: 0x0002C484
		public void CopyTo(T[] array, int startIndex)
		{
			NativeArray<T>.Copy(this.nativeArray, startIndex, array, 0, array.Length);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0002E297 File Offset: 0x0002C497
		public void CopyTo<U>(U[] array) where U : struct
		{
			NativeArray<U>.Copy(this.nativeArray.Reinterpret<U>(), array);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0002E2AA File Offset: 0x0002C4AA
		public void CopyFrom(NativeArray<T> array)
		{
			NativeArray<T>.Copy(array, this.nativeArray);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0002E2B8 File Offset: 0x0002C4B8
		public void CopyFrom(T[] array, int startIndex)
		{
			NativeArray<T>.Copy(array, 0, this.nativeArray, startIndex, array.Length);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0002E2CB File Offset: 0x0002C4CB
		public void CopyFrom<U>(NativeArray<U> array) where U : struct
		{
			NativeArray<T>.Copy(array.Reinterpret<T>(), this.nativeArray);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0002E2DF File Offset: 0x0002C4DF
		public void CopyFrom<U>(NativeArray<U> array, int dstIndex, int length) where U : struct
		{
			NativeArray<T>.Copy(array.Reinterpret<T>(), 0, this.nativeArray, dstIndex, length);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0002E2F8 File Offset: 0x0002C4F8
		public unsafe void CopyTypeChange<U>(U[] array) where U : struct
		{
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.SizeOf<U>();
			int num3 = this.Length * num / num2;
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			ulong num4;
			UnsafeUtility.MemCpy(UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num4), (void*)unsafePtr, (long)(num3 * num2));
			UnsafeUtility.ReleaseGCObject(num4);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0002E344 File Offset: 0x0002C544
		public unsafe void CopyTypeChangeStride<U>(U[] array) where U : struct
		{
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.SizeOf<U>();
			int length = this.Length;
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			ulong num3;
			void* ptr = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num3);
			int num4 = num;
			UnsafeUtility.MemCpyStride(ptr, num2, (void*)unsafePtr, num, num4, length);
			UnsafeUtility.ReleaseGCObject(num3);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0002E390 File Offset: 0x0002C590
		public void AddEmpty(int dataLength)
		{
			DataChunk dataChunk = this.AddRange(dataLength);
			this.Remove(dataChunk);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0002E3AC File Offset: 0x0002C5AC
		public void Remove(DataChunk chunk)
		{
			if (!chunk.IsValid)
			{
				return;
			}
			this.AddEmptyChunk(chunk);
			if (chunk.startIndex + chunk.dataLength == this.useCount)
			{
				this.useCount = 0;
				foreach (DataChunk dataChunk in this.emptyChunks)
				{
					this.useCount = math.max(this.useCount, dataChunk.startIndex);
				}
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0002E43C File Offset: 0x0002C63C
		public void Remove(int index)
		{
			this.Remove(new DataChunk(index));
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0002E44A File Offset: 0x0002C64A
		public void RemoveAndFill(DataChunk chunk, T clearData = default(T))
		{
			this.Remove(chunk);
			this.Fill(chunk, clearData);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0002E45B File Offset: 0x0002C65B
		public void Fill(T fillData = default(T))
		{
			if (!this.IsValid)
			{
				return;
			}
			this.FillInternal(0, this.nativeArray.Length, fillData);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0002E479 File Offset: 0x0002C679
		public void Fill(DataChunk chunk, T fillData = default(T))
		{
			if (!this.IsValid || !chunk.IsValid)
			{
				return;
			}
			this.FillInternal(chunk.startIndex, chunk.dataLength, fillData);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0002E4A0 File Offset: 0x0002C6A0
		private unsafe void FillInternal(int start, int size, T fillData = default(T))
		{
			void* unsafePtr = this.nativeArray.GetUnsafePtr<T>();
			int num = start;
			int i = 0;
			while (i < size)
			{
				UnsafeUtility.WriteArrayElement<T>(unsafePtr, num, fillData);
				i++;
				num++;
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0002E4D4 File Offset: 0x0002C6D4
		public void Clear()
		{
			this.emptyChunks.Clear();
			this.useCount = 0;
			if (this.IsValid && this.Length > 0)
			{
				DataChunk dataChunk = new DataChunk(0, this.Length);
				this.emptyChunks.Add(dataChunk);
			}
		}

		// Token: 0x17000090 RID: 144
		public T this[int index]
		{
			get
			{
				return this.nativeArray[index];
			}
			set
			{
				this.nativeArray[index] = value;
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0002E53C File Offset: 0x0002C73C
		public unsafe ref T GetRef(int index)
		{
			T* unsafePtr = (T*)this.nativeArray.GetUnsafePtr<T>();
			return ref unsafePtr[(IntPtr)index * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0002E560 File Offset: 0x0002C760
		public NativeArray<T> GetNativeArray()
		{
			return this.nativeArray;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0002E568 File Offset: 0x0002C768
		public NativeArray<U> GetNativeArray<U>() where U : struct
		{
			return this.nativeArray.Reinterpret<U>();
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0002E578 File Offset: 0x0002C778
		private DataChunk GetEmptyChunk(int dataLength)
		{
			if (dataLength <= 0)
			{
				return default(DataChunk);
			}
			for (int i = 0; i < this.emptyChunks.Count; i++)
			{
				DataChunk dataChunk = this.emptyChunks[i];
				if (dataLength == dataChunk.dataLength)
				{
					this.emptyChunks.RemoveAtSwapBack<DataChunk>(i);
					return dataChunk;
				}
				if (dataLength < dataChunk.dataLength)
				{
					DataChunk dataChunk2 = default(DataChunk);
					dataChunk2.startIndex = dataChunk.startIndex;
					dataChunk2.dataLength = dataLength;
					dataChunk.startIndex += dataLength;
					dataChunk.dataLength -= dataLength;
					this.emptyChunks[i] = dataChunk;
					return dataChunk2;
				}
			}
			return default(DataChunk);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0002E628 File Offset: 0x0002C828
		private void AddEmptyChunk(DataChunk chunk)
		{
			if (!chunk.IsValid)
			{
				return;
			}
			for (int i = 0; i < this.emptyChunks.Count; i++)
			{
				DataChunk dataChunk = this.emptyChunks[i];
				if (dataChunk.startIndex + dataChunk.dataLength == chunk.startIndex)
				{
					dataChunk.dataLength += chunk.dataLength;
					chunk = dataChunk;
					this.emptyChunks.RemoveAtSwapBack<DataChunk>(i);
					break;
				}
			}
			for (int j = 0; j < this.emptyChunks.Count; j++)
			{
				DataChunk dataChunk2 = this.emptyChunks[j];
				if (dataChunk2.startIndex == chunk.startIndex + chunk.dataLength)
				{
					chunk.dataLength += dataChunk2.dataLength;
					this.emptyChunks.RemoveAtSwapBack<DataChunk>(j);
					break;
				}
			}
			this.emptyChunks.Add(chunk);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0002E6FC File Offset: 0x0002C8FC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("ExNativeArray Length:{0} Count:{1} IsValid:{2}", this.Length, this.Count, this.IsValid));
			stringBuilder.AppendLine("---- Datas[100] ----");
			if (this.IsValid)
			{
				int num = 0;
				while (num < this.Length && num < 100)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					T t = this.nativeArray[num];
					stringBuilder2.AppendLine(t.ToString());
					num++;
				}
			}
			stringBuilder.AppendLine("---- Empty Chunks ----");
			foreach (DataChunk dataChunk in this.emptyChunks)
			{
				stringBuilder.AppendLine(dataChunk.ToString());
			}
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0002E7FC File Offset: 0x0002C9FC
		public string ToSummary()
		{
			return string.Format("ExNativeArray Length:{0} Count:{1} IsValid:{2}", this.Length, this.Count, this.IsValid);
		}

		// Token: 0x040008A3 RID: 2211
		private NativeArray<T> nativeArray;

		// Token: 0x040008A4 RID: 2212
		private List<DataChunk> emptyChunks = new List<DataChunk>();

		// Token: 0x040008A5 RID: 2213
		private int useCount;
	}
}
