using System;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000111 RID: 273
	public class ExSimpleNativeArray<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
	{
		// Token: 0x0600055A RID: 1370 RVA: 0x0002E93B File Offset: 0x0002CB3B
		public ExSimpleNativeArray()
		{
			this.count = 0;
			this.length = 0;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0002E951 File Offset: 0x0002CB51
		public ExSimpleNativeArray(int dataLength, bool areaOnly = false)
			: this()
		{
			this.nativeArray = new NativeArray<T>(dataLength, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.length = dataLength;
			if (!areaOnly)
			{
				this.count = this.length;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0002E97D File Offset: 0x0002CB7D
		public ExSimpleNativeArray(T[] dataArray)
			: this()
		{
			this.nativeArray = new NativeArray<T>(dataArray, Allocator.Persistent);
			this.length = dataArray.Length;
			this.count = this.length;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0002E9A7 File Offset: 0x0002CBA7
		public ExSimpleNativeArray(NativeArray<T> array)
			: this()
		{
			this.nativeArray = new NativeArray<T>(array, Allocator.Persistent);
			this.length = array.Length;
			this.count = this.length;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0002E9D5 File Offset: 0x0002CBD5
		public ExSimpleNativeArray(NativeList<T> array)
			: this()
		{
			this.nativeArray = new NativeArray<T>(array.AsArray(), Allocator.Persistent);
			this.length = array.Length;
			this.count = this.length;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0002EA09 File Offset: 0x0002CC09
		public ExSimpleNativeArray(ExSimpleNativeArray<T>.SerializationData sdata)
		{
			this.Deserialize(sdata);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0002EA19 File Offset: 0x0002CC19
		public void Dispose()
		{
			if (this.nativeArray.IsCreated)
			{
				this.nativeArray.Dispose();
			}
			this.count = 0;
			this.length = 0;
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0002EA41 File Offset: 0x0002CC41
		public bool IsValid
		{
			get
			{
				return this.nativeArray.IsCreated;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0002EA4E File Offset: 0x0002CC4E
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0002EA56 File Offset: 0x0002CC56
		public int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0002EA5E File Offset: 0x0002CC5E
		public void SetCount(int newCount)
		{
			this.count = newCount;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0002EA67 File Offset: 0x0002CC67
		public void SetLength(int newLength)
		{
			if (newLength > this.length)
			{
				this.Expand(newLength - this.length, true, true);
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0002EA82 File Offset: 0x0002CC82
		public void AddRange(int dataLength)
		{
			this.Expand(dataLength, false, true);
			this.count += dataLength;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0002EA9C File Offset: 0x0002CC9C
		public void AddRange(T[] dataArray)
		{
			if (this.length == 0)
			{
				if (this.nativeArray.IsCreated)
				{
					this.nativeArray.Dispose();
				}
				this.nativeArray = new NativeArray<T>(dataArray, Allocator.Persistent);
				this.length = dataArray.Length;
				this.count = this.length;
				return;
			}
			int num = dataArray.Length;
			this.Expand(num, false, true);
			NativeArray<T>.Copy(dataArray, 0, this.nativeArray, this.count, num);
			this.count += num;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0002EB1C File Offset: 0x0002CD1C
		public void AddRange(T[] dataArray, int cnt)
		{
			if (this.length == 0)
			{
				if (this.nativeArray.IsCreated)
				{
					this.nativeArray.Dispose();
				}
				this.nativeArray = new NativeArray<T>(cnt, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				NativeArray<T>.Copy(dataArray, 0, this.nativeArray, 0, cnt);
				this.length = cnt;
				this.count = this.length;
				return;
			}
			this.Expand(cnt, false, true);
			NativeArray<T>.Copy(dataArray, 0, this.nativeArray, this.count, cnt);
			this.count += cnt;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0002EBA7 File Offset: 0x0002CDA7
		public void AddRange(int dataLength, T fillData = default(T))
		{
			this.Expand(dataLength, false, true);
			this.Fill(this.count, dataLength, fillData);
			this.count += dataLength;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0002EBD0 File Offset: 0x0002CDD0
		public void AddRange(NativeArray<T> narray)
		{
			if (this.length == 0)
			{
				if (this.nativeArray.IsCreated)
				{
					this.nativeArray.Dispose();
				}
				this.nativeArray = new NativeArray<T>(narray, Allocator.Persistent);
				this.length = narray.Length;
				this.count = this.length;
				return;
			}
			int num = narray.Length;
			this.Expand(num, false, true);
			NativeArray<T>.Copy(narray, 0, this.nativeArray, this.count, num);
			this.count += num;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0002EC57 File Offset: 0x0002CE57
		public void AddRange(NativeArray<T> narray, int start, int length)
		{
			if (length > 0)
			{
				this.Expand(length, false, true);
				NativeArray<T>.Copy(narray, start, this.nativeArray, this.count, length);
				this.count += length;
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0002EC88 File Offset: 0x0002CE88
		public void AddRange(NativeList<T> nlist)
		{
			this.AddRange(nlist.AsArray());
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0002EC97 File Offset: 0x0002CE97
		public void AddRange(ExSimpleNativeArray<T> exarray)
		{
			this.AddRange(exarray.GetNativeArray());
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0002ECA8 File Offset: 0x0002CEA8
		public unsafe void AddRange<U>(U[] array) where U : struct
		{
			int num = array.Length;
			this.Expand(num, false, true);
			int num2 = UnsafeUtility.SizeOf<T>();
			ulong num3;
			void* ptr = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num3);
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			UnsafeUtility.MemCpy((void*)(unsafePtr + this.count * num2), ptr, (long)(num * num2));
			UnsafeUtility.ReleaseGCObject(num3);
			this.count += num;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0002ED08 File Offset: 0x0002CF08
		public unsafe void AddRangeTypeChange<U>(U[] array) where U : struct
		{
			int num = UnsafeUtility.SizeOf<U>();
			int num2 = UnsafeUtility.SizeOf<T>();
			int num3 = array.Length * num / num2;
			this.Expand(num3, false, true);
			ulong num4;
			void* ptr = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num4);
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			UnsafeUtility.MemCpy((void*)(unsafePtr + this.count * num2), ptr, (long)(num3 * num2));
			UnsafeUtility.ReleaseGCObject(num4);
			this.count += num3;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0002ED74 File Offset: 0x0002CF74
		public unsafe void AddRangeTypeChange<U>(NativeArray<U> array) where U : struct
		{
			int num = UnsafeUtility.SizeOf<U>();
			int num2 = UnsafeUtility.SizeOf<T>();
			int num3 = array.Length * num / num2;
			this.Expand(num3, false, true);
			byte* unsafePtr = (byte*)array.GetUnsafePtr<U>();
			byte* unsafePtr2 = (byte*)this.nativeArray.GetUnsafePtr<T>();
			UnsafeUtility.MemCpy((void*)(unsafePtr2 + this.count * num2), (void*)unsafePtr, (long)(num3 * num2));
			this.count += num3;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0002EDDC File Offset: 0x0002CFDC
		public unsafe void AddRangeStride<U>(U[] array) where U : struct
		{
			int num = array.Length;
			this.Expand(num, false, true);
			int num2 = UnsafeUtility.SizeOf<U>();
			int num3 = UnsafeUtility.SizeOf<T>();
			ulong num4;
			void* ptr = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num4);
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			int num5 = math.min(num2, num3);
			UnsafeUtility.MemCpyStride((void*)(unsafePtr + this.count * num3), num3, ptr, num2, num5, num);
			UnsafeUtility.ReleaseGCObject(num4);
			this.count += num;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0002EE50 File Offset: 0x0002D050
		public void Add(T data)
		{
			if (this.Length == 0)
			{
				this.Expand(16, false, true);
			}
			else if (this.count == this.Length)
			{
				this.Expand(this.Length, false, true);
			}
			this.nativeArray[this.count] = data;
			this.count++;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0002EEAD File Offset: 0x0002D0AD
		public T[] ToArray()
		{
			return this.nativeArray.ToArray();
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0002EEBA File Offset: 0x0002D0BA
		public void CopyTo(T[] array)
		{
			NativeArray<T>.Copy(this.nativeArray, array);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0002EEC8 File Offset: 0x0002D0C8
		public void CopyTo<U>(U[] array) where U : struct
		{
			NativeArray<U>.Copy(this.nativeArray.Reinterpret<U>(), array);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0002EEDC File Offset: 0x0002D0DC
		public unsafe void CopyToWithTypeChange<U>(U[] array) where U : struct
		{
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.SizeOf<U>();
			int num3 = this.Length * num / num2;
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			ulong num4;
			UnsafeUtility.MemCpy(UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num4), (void*)unsafePtr, (long)(num3 * num2));
			UnsafeUtility.ReleaseGCObject(num4);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0002EF28 File Offset: 0x0002D128
		public unsafe void CopyToWithTypeChangeStride<U>(U[] array) where U : struct
		{
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = UnsafeUtility.SizeOf<U>();
			int num3 = this.Length;
			byte* unsafePtr = (byte*)this.nativeArray.GetUnsafePtr<T>();
			ulong num4;
			void* ptr = UnsafeUtility.PinGCArrayAndGetDataAddress(array, out num4);
			int num5 = num;
			UnsafeUtility.MemCpyStride(ptr, num2, (void*)unsafePtr, num, num5, num3);
			UnsafeUtility.ReleaseGCObject(num4);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0002EF71 File Offset: 0x0002D171
		public void CopyFrom(NativeArray<T> array)
		{
			NativeArray<T>.Copy(array, this.nativeArray);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0002EF7F File Offset: 0x0002D17F
		public void CopyFrom<U>(NativeArray<U> array) where U : struct
		{
			NativeArray<T>.Copy(array.Reinterpret<T>(), this.nativeArray);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0002EF94 File Offset: 0x0002D194
		public unsafe void CopyFromWithTypeChangeStride<U>(NativeArray<U> array) where U : struct
		{
			int num = UnsafeUtility.SizeOf<U>();
			int num2 = UnsafeUtility.SizeOf<T>();
			int num3 = array.Length;
			byte* unsafePtr = (byte*)array.GetUnsafePtr<U>();
			byte* unsafePtr2 = (byte*)this.nativeArray.GetUnsafePtr<T>();
			int num4 = num2;
			UnsafeUtility.MemCpyStride((void*)unsafePtr2, num2, (void*)unsafePtr, num, num4, num3);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0002EFD9 File Offset: 0x0002D1D9
		public void Fill(int startIndex, int dataLength, T fillData = default(T))
		{
			this.FillInternal(startIndex, dataLength, fillData);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0002EFE4 File Offset: 0x0002D1E4
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

		// Token: 0x17000094 RID: 148
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

		// Token: 0x0600057F RID: 1407 RVA: 0x0002F034 File Offset: 0x0002D234
		public NativeArray<T> GetNativeArray()
		{
			return this.nativeArray;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0002F03C File Offset: 0x0002D23C
		public NativeArray<U> GetNativeArray<U>() where U : struct
		{
			return this.nativeArray.Reinterpret<U>();
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0002F04C File Offset: 0x0002D24C
		private void Expand(int dataLength, bool force = false, bool copy = true)
		{
			int num = (force ? (this.length + dataLength) : (this.count + dataLength));
			if (this.length == 0)
			{
				if (this.nativeArray.IsCreated)
				{
					this.nativeArray.Dispose();
				}
				this.nativeArray = new NativeArray<T>(dataLength, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.length = dataLength;
				return;
			}
			if (num > this.Length)
			{
				NativeArray<T> nativeArray = new NativeArray<T>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				if (copy)
				{
					NativeArray<T>.Copy(this.nativeArray, nativeArray, this.count);
				}
				this.nativeArray.Dispose();
				this.nativeArray = nativeArray;
				this.length = num;
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0002F0E8 File Offset: 0x0002D2E8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("ExSimpleNativeArray Length:{0} Count:{1} IsValid:{2}", this.Length, this.Count, this.IsValid));
			stringBuilder.AppendLine("---- Datas[~100] ----");
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
			return stringBuilder.ToString();
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0002F180 File Offset: 0x0002D380
		public ExSimpleNativeArray<T>.SerializationData Serialize()
		{
			ExSimpleNativeArray<T>.SerializationData serializationData = new ExSimpleNativeArray<T>.SerializationData();
			serializationData.count = this.count;
			serializationData.length = this.length;
			if (this.nativeArray.IsCreated && this.nativeArray.Length > 0)
			{
				serializationData.arrayBytes = (ref this.nativeArray).MC2ToRawBytes<T>();
			}
			return serializationData;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0002F1D8 File Offset: 0x0002D3D8
		public bool Deserialize(ExSimpleNativeArray<T>.SerializationData data)
		{
			bool flag;
			try
			{
				this.Dispose();
				this.count = data.count;
				this.length = data.length;
				if (data.length > 0 && data.arrayBytes != null)
				{
					this.nativeArray = NativeArrayExtensions.MC2FromRawBytes<T>(data.arrayBytes, Allocator.Persistent);
				}
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				flag = false;
			}
			return flag;
		}

		// Token: 0x040008A8 RID: 2216
		private NativeArray<T> nativeArray;

		// Token: 0x040008A9 RID: 2217
		private int count;

		// Token: 0x040008AA RID: 2218
		private int length;

		// Token: 0x02000112 RID: 274
		[Serializable]
		public class SerializationData
		{
			// Token: 0x040008AB RID: 2219
			public int count;

			// Token: 0x040008AC RID: 2220
			public int length;

			// Token: 0x040008AD RID: 2221
			public byte[] arrayBytes;
		}
	}
}
