using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.Rendering
{
	// Token: 0x0200003E RID: 62
	public static class CoreUnsafeUtils
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000A95C File Offset: 0x00008B5C
		public unsafe static void CopyTo<T>(this List<T> list, void* dest, int count) where T : struct
		{
			int num = Mathf.Min(count, list.Count);
			for (int i = 0; i < num; i++)
			{
				UnsafeUtility.WriteArrayElement<T>(dest, i, list[i]);
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000A990 File Offset: 0x00008B90
		public unsafe static void CopyTo<T>(this T[] list, void* dest, int count) where T : struct
		{
			int num = Mathf.Min(count, list.Length);
			for (int i = 0; i < num; i++)
			{
				UnsafeUtility.WriteArrayElement<T>(dest, i, list[i]);
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000A9C1 File Offset: 0x00008BC1
		private static void CalculateRadixParams(int radixBits, out int bitStates)
		{
			if (radixBits != 2 && radixBits != 4 && radixBits != 8)
			{
				throw new Exception("Radix bits must be 2, 4 or 8 for uint radix sort.");
			}
			bitStates = 1 << radixBits;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000A9E2 File Offset: 0x00008BE2
		private static int CalculateRadixSupportSize(int bitStates, int arrayLength)
		{
			return bitStates * 3 + arrayLength;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A9E9 File Offset: 0x00008BE9
		private unsafe static void CalculateRadixSortSupportArrays(int bitStates, int arrayLength, uint* supportArray, out uint* bucketIndices, out uint* bucketSizes, out uint* bucketPrefix, out uint* arrayOutput)
		{
			bucketIndices = supportArray;
			bucketSizes = bucketIndices + (IntPtr)bitStates * 4;
			bucketPrefix = bucketSizes + (IntPtr)bitStates * 4;
			arrayOutput = bucketPrefix + (IntPtr)bitStates * 4;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000AA10 File Offset: 0x00008C10
		private unsafe static void MergeSort(uint* array, uint* support, int length)
		{
			for (int i = 1; i < length; i *= 2)
			{
				int num = 0;
				while (num + i < length)
				{
					int num2 = num + i;
					int num3 = num2 + i;
					if (num3 > length)
					{
						num3 = length;
					}
					int j = num;
					int k = num;
					int l = num2;
					while (k < num2)
					{
						if (l >= num3)
						{
							break;
						}
						if (array[k] <= array[l])
						{
							support[j] = array[k++];
						}
						else
						{
							support[j] = array[l++];
						}
						j++;
					}
					while (k < num2)
					{
						support[j] = array[k++];
						j++;
					}
					while (l < num3)
					{
						support[j] = array[l++];
						j++;
					}
					for (j = num; j < num3; j++)
					{
						array[j] = support[j];
					}
					num += i * 2;
				}
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000AB10 File Offset: 0x00008D10
		public unsafe static void MergeSort(uint[] arr, int sortSize, ref uint[] supportArray)
		{
			sortSize = Math.Min(sortSize, arr.Length);
			if (arr == null || sortSize == 0)
			{
				return;
			}
			if (supportArray == null || supportArray.Length < sortSize)
			{
				supportArray = new uint[sortSize];
			}
			fixed (uint[] array = arr)
			{
				uint* ptr;
				if (arr == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				uint[] array2;
				uint* ptr2;
				if ((array2 = supportArray) == null || array2.Length == 0)
				{
					ptr2 = null;
				}
				else
				{
					ptr2 = &array2[0];
				}
				CoreUnsafeUtils.MergeSort(ptr, ptr2, sortSize);
				array2 = null;
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000AB80 File Offset: 0x00008D80
		public unsafe static void MergeSort(NativeArray<uint> arr, int sortSize, ref NativeArray<uint> supportArray)
		{
			sortSize = Math.Min(sortSize, arr.Length);
			if (!arr.IsCreated || sortSize == 0)
			{
				return;
			}
			if (!supportArray.IsCreated || supportArray.Length < sortSize)
			{
				(ref supportArray).ResizeArray<uint>(arr.Length);
			}
			CoreUnsafeUtils.MergeSort((uint*)arr.GetUnsafePtr<uint>(), (uint*)supportArray.GetUnsafePtr<uint>(), sortSize);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000ABE0 File Offset: 0x00008DE0
		private unsafe static void InsertionSort(uint* arr, int length)
		{
			for (int i = 0; i < length; i++)
			{
				int num = i;
				while (num >= 1 && arr[num] < arr[num - 1])
				{
					uint num2 = arr[num];
					arr[num] = arr[num - 1];
					arr[num - 1] = num2;
					num--;
				}
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000AC3C File Offset: 0x00008E3C
		public unsafe static void InsertionSort(uint[] arr, int sortSize)
		{
			sortSize = Math.Min(arr.Length, sortSize);
			if (arr == null || sortSize == 0)
			{
				return;
			}
			fixed (uint[] array = arr)
			{
				uint* ptr;
				if (arr == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				CoreUnsafeUtils.InsertionSort(ptr, sortSize);
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000AC7C File Offset: 0x00008E7C
		public unsafe static void InsertionSort(NativeArray<uint> arr, int sortSize)
		{
			sortSize = Math.Min(arr.Length, sortSize);
			if (!arr.IsCreated || sortSize == 0)
			{
				return;
			}
			CoreUnsafeUtils.InsertionSort((uint*)arr.GetUnsafePtr<uint>(), sortSize);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000ACA8 File Offset: 0x00008EA8
		private unsafe static void RadixSort(uint* array, uint* support, int radixBits, int bitStates, int length)
		{
			uint num = (uint)(bitStates - 1);
			uint* ptr;
			uint* ptr2;
			uint* ptr3;
			uint* ptr4;
			CoreUnsafeUtils.CalculateRadixSortSupportArrays(bitStates, length, support, out ptr, out ptr2, out ptr3, out ptr4);
			int num2 = 32 / radixBits;
			uint* ptr5 = ptr4;
			uint* ptr6 = array;
			for (int i = 0; i < num2; i++)
			{
				int num3 = i * radixBits;
				for (int j = 0; j < 3 * bitStates; j++)
				{
					ptr[j] = 0U;
				}
				for (int k = 0; k < length; k++)
				{
					ptr2[(ulong)((ptr6[k] >> num3) & num) * 4UL / 4UL] += 1U;
				}
				for (int l = 1; l < bitStates; l++)
				{
					ptr3[l] = ptr3[l - 1] + ptr2[l - 1];
				}
				for (int m = 0; m < length; m++)
				{
					uint num4 = ptr6[m];
					uint num5 = (num4 >> num3) & num;
					ref int ptr7 = ref *(int*)ptr5;
					uint num6 = ptr3[(ulong)num5 * 4UL / 4UL];
					uint* ptr8 = ptr + (ulong)num5 * 4UL / 4UL;
					uint num7 = *ptr8;
					*ptr8 = num7 + 1U;
					*((ref ptr7) + (IntPtr)((ulong)(num6 + num7) * 4UL)) = (int)num4;
				}
				uint* ptr9 = ptr6;
				ptr6 = ptr5;
				ptr5 = ptr9;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000ADC4 File Offset: 0x00008FC4
		public unsafe static void RadixSort(uint[] arr, int sortSize, ref uint[] supportArray, int radixBits = 8)
		{
			sortSize = Math.Min(sortSize, arr.Length);
			int num;
			CoreUnsafeUtils.CalculateRadixParams(radixBits, out num);
			if (arr == null || sortSize == 0)
			{
				return;
			}
			int num2 = CoreUnsafeUtils.CalculateRadixSupportSize(num, sortSize);
			if (supportArray == null || supportArray.Length < num2)
			{
				supportArray = new uint[num2];
			}
			fixed (uint[] array = arr)
			{
				uint* ptr;
				if (arr == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				uint[] array2;
				uint* ptr2;
				if ((array2 = supportArray) == null || array2.Length == 0)
				{
					ptr2 = null;
				}
				else
				{
					ptr2 = &array2[0];
				}
				CoreUnsafeUtils.RadixSort(ptr, ptr2, radixBits, num, sortSize);
				array2 = null;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000AE4C File Offset: 0x0000904C
		public unsafe static void RadixSort(NativeArray<uint> array, int sortSize, ref NativeArray<uint> supportArray, int radixBits = 8)
		{
			sortSize = Math.Min(sortSize, array.Length);
			int num;
			CoreUnsafeUtils.CalculateRadixParams(radixBits, out num);
			if (!array.IsCreated || sortSize == 0)
			{
				return;
			}
			int num2 = CoreUnsafeUtils.CalculateRadixSupportSize(num, sortSize);
			if (!supportArray.IsCreated || supportArray.Length < num2)
			{
				(ref supportArray).ResizeArray<uint>(num2);
			}
			CoreUnsafeUtils.RadixSort((uint*)array.GetUnsafePtr<uint>(), (uint*)supportArray.GetUnsafePtr<uint>(), radixBits, num, sortSize);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000AEB8 File Offset: 0x000090B8
		public unsafe static void QuickSort(uint[] arr, int left, int right)
		{
			fixed (uint[] array = arr)
			{
				uint* ptr;
				if (arr == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				CoreUnsafeUtils.QuickSort<uint, uint, CoreUnsafeUtils.UintKeyGetter>((void*)ptr, left, right);
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000AEE7 File Offset: 0x000090E7
		public unsafe static void QuickSort<T>(int count, void* data) where T : struct, IComparable<T>
		{
			CoreUnsafeUtils.QuickSort<T, T, CoreUnsafeUtils.DefaultKeyGetter<T>>(data, 0, count - 1);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000AEF3 File Offset: 0x000090F3
		public unsafe static void QuickSort<TValue, TKey, TGetter>(int count, void* data) where TValue : struct where TKey : struct, IComparable<TKey> where TGetter : struct, CoreUnsafeUtils.IKeyGetter<TValue, TKey>
		{
			CoreUnsafeUtils.QuickSort<TValue, TKey, TGetter>(data, 0, count - 1);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000AF00 File Offset: 0x00009100
		public unsafe static void QuickSort<TValue, TKey, TGetter>(void* data, int left, int right) where TValue : struct where TKey : struct, IComparable<TKey> where TGetter : struct, CoreUnsafeUtils.IKeyGetter<TValue, TKey>
		{
			if (left < right)
			{
				int num = CoreUnsafeUtils.Partition<TValue, TKey, TGetter>(data, left, right);
				if (num >= 1)
				{
					CoreUnsafeUtils.QuickSort<TValue, TKey, TGetter>(data, left, num);
				}
				if (num + 1 < right)
				{
					CoreUnsafeUtils.QuickSort<TValue, TKey, TGetter>(data, num + 1, right);
				}
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000AF38 File Offset: 0x00009138
		public unsafe static int IndexOf<T>(void* data, int count, T v) where T : struct, IEquatable<T>
		{
			for (int i = 0; i < count; i++)
			{
				T t = UnsafeUtility.ReadArrayElement<T>(data, i);
				if (t.Equals(v))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000AF6C File Offset: 0x0000916C
		public unsafe static int CompareHashes<TOldValue, TOldGetter, TNewValue, TNewGetter>(int oldHashCount, void* oldHashes, int newHashCount, void* newHashes, int* addIndices, int* removeIndices, out int addCount, out int remCount) where TOldValue : struct where TOldGetter : struct, CoreUnsafeUtils.IKeyGetter<TOldValue, Hash128> where TNewValue : struct where TNewGetter : struct, CoreUnsafeUtils.IKeyGetter<TNewValue, Hash128>
		{
			TOldGetter toldGetter = new TOldGetter();
			TNewGetter tnewGetter = new TNewGetter();
			addCount = 0;
			remCount = 0;
			if (oldHashCount == newHashCount)
			{
				Hash128 hash = default(Hash128);
				Hash128 hash2 = default(Hash128);
				CoreUnsafeUtils.CombineHashes<TOldValue, TOldGetter>(oldHashCount, oldHashes, &hash);
				CoreUnsafeUtils.CombineHashes<TNewValue, TNewGetter>(newHashCount, newHashes, &hash2);
				if (hash == hash2)
				{
					return 0;
				}
			}
			int num = 0;
			int i = 0;
			int j = 0;
			while (i < oldHashCount || j < newHashCount)
			{
				if (i == oldHashCount)
				{
					while (j < newHashCount)
					{
						int num2 = addCount;
						addCount = num2 + 1;
						addIndices[num2] = j;
						num++;
						j++;
					}
				}
				else if (j == newHashCount)
				{
					while (i < oldHashCount)
					{
						int num2 = remCount;
						remCount = num2 + 1;
						removeIndices[num2] = i;
						num++;
						i++;
					}
				}
				else
				{
					TNewValue tnewValue = UnsafeUtility.ReadArrayElement<TNewValue>(newHashes, j);
					TOldValue toldValue = UnsafeUtility.ReadArrayElement<TOldValue>(oldHashes, i);
					Hash128 hash3 = tnewGetter.Get(ref tnewValue);
					Hash128 hash4 = toldGetter.Get(ref toldValue);
					if (hash3 == hash4)
					{
						j++;
						i++;
					}
					else if (hash3 < hash4)
					{
						while (j < newHashCount)
						{
							if (!(hash3 < hash4))
							{
								break;
							}
							int num2 = addCount;
							addCount = num2 + 1;
							addIndices[num2] = j;
							j++;
							num++;
							tnewValue = UnsafeUtility.ReadArrayElement<TNewValue>(newHashes, j);
							hash3 = tnewGetter.Get(ref tnewValue);
						}
					}
					else
					{
						while (i < oldHashCount && hash4 < hash3)
						{
							int num2 = remCount;
							remCount = num2 + 1;
							removeIndices[num2] = i;
							num++;
							i++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000B114 File Offset: 0x00009314
		public unsafe static int CompareHashes(int oldHashCount, Hash128* oldHashes, int newHashCount, Hash128* newHashes, int* addIndices, int* removeIndices, out int addCount, out int remCount)
		{
			return CoreUnsafeUtils.CompareHashes<Hash128, CoreUnsafeUtils.DefaultKeyGetter<Hash128>, Hash128, CoreUnsafeUtils.DefaultKeyGetter<Hash128>>(oldHashCount, (void*)oldHashes, newHashCount, (void*)newHashes, addIndices, removeIndices, out addCount, out remCount);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000B128 File Offset: 0x00009328
		public unsafe static void CombineHashes<TValue, TGetter>(int count, void* hashes, Hash128* outHash) where TValue : struct where TGetter : struct, CoreUnsafeUtils.IKeyGetter<TValue, Hash128>
		{
			TGetter tgetter = new TGetter();
			for (int i = 0; i < count; i++)
			{
				TValue tvalue = UnsafeUtility.ReadArrayElement<TValue>(hashes, i);
				Hash128 hash = tgetter.Get(ref tvalue);
				HashUtilities.AppendHash(ref hash, ref *outHash);
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B167 File Offset: 0x00009367
		public unsafe static void CombineHashes(int count, Hash128* hashes, Hash128* outHash)
		{
			CoreUnsafeUtils.CombineHashes<Hash128, CoreUnsafeUtils.DefaultKeyGetter<Hash128>>(count, (void*)hashes, outHash);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000B174 File Offset: 0x00009374
		private unsafe static int Partition<TValue, TKey, TGetter>(void* data, int left, int right) where TValue : struct where TKey : struct, IComparable<TKey> where TGetter : struct, CoreUnsafeUtils.IKeyGetter<TValue, TKey>
		{
			TGetter tgetter = default(TGetter);
			TValue tvalue = UnsafeUtility.ReadArrayElement<TValue>(data, left);
			TKey tkey = tgetter.Get(ref tvalue);
			left--;
			right++;
			for (;;)
			{
				TValue tvalue2 = default(TValue);
				TKey tkey2 = default(TKey);
				int num;
				do
				{
					left++;
					tvalue2 = UnsafeUtility.ReadArrayElement<TValue>(data, left);
					tkey2 = tgetter.Get(ref tvalue2);
					num = tkey2.CompareTo(tkey);
				}
				while (num < 0);
				TValue tvalue3 = default(TValue);
				TKey tkey3 = default(TKey);
				do
				{
					right--;
					tvalue3 = UnsafeUtility.ReadArrayElement<TValue>(data, right);
					tkey3 = tgetter.Get(ref tvalue3);
					num = tkey3.CompareTo(tkey);
				}
				while (num > 0);
				if (left >= right)
				{
					break;
				}
				UnsafeUtility.WriteArrayElement<TValue>(data, right, tvalue2);
				UnsafeUtility.WriteArrayElement<TValue>(data, left, tvalue3);
			}
			return right;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000B250 File Offset: 0x00009450
		public unsafe static bool HaveDuplicates(int[] arr)
		{
			int* ptr;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)arr.Length) * 4];
				arr.CopyTo<int>((void*)ptr, arr.Length);
				CoreUnsafeUtils.QuickSort<int>(arr.Length, (void*)ptr);
			}
			for (int i = arr.Length - 1; i > 0; i--)
			{
				if (UnsafeUtility.ReadArrayElement<int>((void*)ptr, i).CompareTo(UnsafeUtility.ReadArrayElement<int>((void*)ptr, i - 1)) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x02000155 RID: 341
		public struct FixedBufferStringQueue
		{
			// Token: 0x1700014B RID: 331
			// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0002C26D File Offset: 0x0002A46D
			// (set) Token: 0x060009D4 RID: 2516 RVA: 0x0002C275 File Offset: 0x0002A475
			public int Count { readonly get; private set; }

			// Token: 0x060009D5 RID: 2517 RVA: 0x0002C280 File Offset: 0x0002A480
			public unsafe FixedBufferStringQueue(byte* ptr, int length)
			{
				this.m_BufferStart = ptr;
				this.m_BufferLength = length;
				this.m_BufferEnd = this.m_BufferStart + this.m_BufferLength;
				this.m_ReadCursor = this.m_BufferStart;
				this.m_WriteCursor = this.m_BufferStart;
				this.Count = 0;
				this.Clear();
			}

			// Token: 0x060009D6 RID: 2518 RVA: 0x0002C2D4 File Offset: 0x0002A4D4
			public unsafe bool TryPush(string v)
			{
				int num = v.Length * 2 + 4;
				if (this.m_WriteCursor + num >= this.m_BufferEnd)
				{
					return false;
				}
				*(int*)this.m_WriteCursor = v.Length;
				this.m_WriteCursor += 4;
				char* ptr = (char*)this.m_WriteCursor;
				int i = 0;
				while (i < v.Length)
				{
					*ptr = v[i];
					i++;
					ptr++;
				}
				this.m_WriteCursor += 2 * v.Length;
				int num2 = this.Count + 1;
				this.Count = num2;
				return true;
			}

			// Token: 0x060009D7 RID: 2519 RVA: 0x0002C364 File Offset: 0x0002A564
			public unsafe bool TryPop(out string v)
			{
				int num = *(int*)this.m_ReadCursor;
				if (num != 0)
				{
					this.m_ReadCursor += 4;
					v = new string((char*)this.m_ReadCursor, 0, num);
					this.m_ReadCursor += num * 2;
					return true;
				}
				v = null;
				return false;
			}

			// Token: 0x060009D8 RID: 2520 RVA: 0x0002C3AF File Offset: 0x0002A5AF
			public unsafe void Clear()
			{
				this.m_WriteCursor = this.m_BufferStart;
				this.m_ReadCursor = this.m_BufferStart;
				this.Count = 0;
				UnsafeUtility.MemClear((void*)this.m_BufferStart, (long)this.m_BufferLength);
			}

			// Token: 0x040005D9 RID: 1497
			private unsafe byte* m_ReadCursor;

			// Token: 0x040005DA RID: 1498
			private unsafe byte* m_WriteCursor;

			// Token: 0x040005DB RID: 1499
			private unsafe readonly byte* m_BufferEnd;

			// Token: 0x040005DC RID: 1500
			private unsafe readonly byte* m_BufferStart;

			// Token: 0x040005DD RID: 1501
			private readonly int m_BufferLength;
		}

		// Token: 0x02000156 RID: 342
		public interface IKeyGetter<TValue, TKey>
		{
			// Token: 0x060009D9 RID: 2521
			TKey Get(ref TValue v);
		}

		// Token: 0x02000157 RID: 343
		internal struct DefaultKeyGetter<T> : CoreUnsafeUtils.IKeyGetter<T, T>
		{
			// Token: 0x060009DA RID: 2522 RVA: 0x0002C3E2 File Offset: 0x0002A5E2
			public T Get(ref T v)
			{
				return v;
			}
		}

		// Token: 0x02000158 RID: 344
		internal struct UintKeyGetter : CoreUnsafeUtils.IKeyGetter<uint, uint>
		{
			// Token: 0x060009DB RID: 2523 RVA: 0x0002C3EA File Offset: 0x0002A5EA
			public uint Get(ref uint v)
			{
				return v;
			}
		}
	}
}
