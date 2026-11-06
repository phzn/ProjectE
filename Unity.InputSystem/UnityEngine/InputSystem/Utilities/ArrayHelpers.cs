using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200012B RID: 299
	internal static class ArrayHelpers
	{
		// Token: 0x060010C6 RID: 4294 RVA: 0x00050733 File Offset: 0x0004E933
		public static int LengthSafe<TValue>(this TValue[] array)
		{
			if (array == null)
			{
				return 0;
			}
			return array.Length;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0005073D File Offset: 0x0004E93D
		public static void Clear<TValue>(this TValue[] array)
		{
			if (array == null)
			{
				return;
			}
			Array.Clear(array, 0, array.Length);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0005074D File Offset: 0x0004E94D
		public static void Clear<TValue>(this TValue[] array, int count)
		{
			if (array == null)
			{
				return;
			}
			Array.Clear(array, 0, count);
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0005075B File Offset: 0x0004E95B
		public static void Clear<TValue>(this TValue[] array, ref int count)
		{
			if (array == null)
			{
				return;
			}
			Array.Clear(array, 0, count);
			count = 0;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0005076D File Offset: 0x0004E96D
		public static void EnsureCapacity<TValue>(ref TValue[] array, int count, int capacity, int capacityIncrement = 10)
		{
			if (capacity == 0)
			{
				return;
			}
			if (array == null)
			{
				array = new TValue[Math.Max(capacity, capacityIncrement)];
				return;
			}
			if (array.Length - count >= capacity)
			{
				return;
			}
			ArrayHelpers.DuplicateWithCapacity<TValue>(ref array, count, capacity, capacityIncrement);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0005079C File Offset: 0x0004E99C
		public static void DuplicateWithCapacity<TValue>(ref TValue[] array, int count, int capacity, int capacityIncrement = 10)
		{
			if (array == null)
			{
				array = new TValue[Math.Max(capacity, capacityIncrement)];
				return;
			}
			TValue[] array2 = new TValue[count + Math.Max(capacity, capacityIncrement)];
			Array.Copy(array, array2, count);
			array = array2;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x000507D8 File Offset: 0x0004E9D8
		public static bool Contains<TValue>(TValue[] array, TValue value)
		{
			if (array == null)
			{
				return false;
			}
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			for (int i = 0; i < array.Length; i++)
			{
				if (@default.Equals(array[i], value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00050811 File Offset: 0x0004EA11
		public static bool ContainsReference<TValue>(this TValue[] array, TValue value) where TValue : class
		{
			return array != null && array.ContainsReference(array.Length, value);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00050822 File Offset: 0x0004EA22
		public static bool ContainsReference<TFirst, TSecond>(this TFirst[] array, int count, TSecond value) where TFirst : TSecond where TSecond : class
		{
			return array.IndexOfReference(value, count) != -1;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00050832 File Offset: 0x0004EA32
		public static bool ContainsReference<TFirst, TSecond>(this TFirst[] array, int startIndex, int count, TSecond value) where TFirst : TSecond where TSecond : class
		{
			return array.IndexOfReference(value, startIndex, count) != -1;
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00050844 File Offset: 0x0004EA44
		public static bool HaveDuplicateReferences<TFirst>(this TFirst[] first, int index, int count)
		{
			for (int i = 0; i < count; i++)
			{
				TFirst tfirst = first[i];
				for (int j = i + 1; j < count - i; j++)
				{
					if (tfirst == first[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0005088C File Offset: 0x0004EA8C
		public static bool HaveEqualElements<TValue>(TValue[] first, TValue[] second, int count = 2147483647)
		{
			if (first == null || second == null)
			{
				return second == first;
			}
			int num = Math.Min(count, first.Length);
			int num2 = Math.Min(count, second.Length);
			if (num != num2)
			{
				return false;
			}
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			for (int i = 0; i < num; i++)
			{
				if (!@default.Equals(first[i], second[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x000508EC File Offset: 0x0004EAEC
		public static int IndexOf<TValue>(TValue[] array, TValue value, int startIndex = 0, int count = -1)
		{
			if (array == null)
			{
				return -1;
			}
			if (count < 0)
			{
				count = array.Length - startIndex;
			}
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (@default.Equals(array[i], value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00050930 File Offset: 0x0004EB30
		public static int IndexOf<TValue>(this TValue[] array, Predicate<TValue> predicate)
		{
			if (array == null)
			{
				return -1;
			}
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				if (predicate(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00050964 File Offset: 0x0004EB64
		public static int IndexOf<TValue>(this TValue[] array, Predicate<TValue> predicate, int startIndex = 0, int count = -1)
		{
			if (array == null)
			{
				return -1;
			}
			int num = startIndex + ((count < 0) ? (array.Length - startIndex) : count);
			for (int i = startIndex; i < num; i++)
			{
				if (predicate(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x000509A3 File Offset: 0x0004EBA3
		public static int IndexOfReference<TFirst, TSecond>(this TFirst[] array, TSecond value, int count = -1) where TFirst : TSecond where TSecond : class
		{
			return array.IndexOfReference(value, 0, count);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000509B0 File Offset: 0x0004EBB0
		public static int IndexOfReference<TFirst, TSecond>(this TFirst[] array, TSecond value, int startIndex, int count) where TFirst : TSecond where TSecond : class
		{
			if (array == null)
			{
				return -1;
			}
			if (count < 0)
			{
				count = array.Length - startIndex;
			}
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (array[i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x000509F4 File Offset: 0x0004EBF4
		public static int IndexOfValue<TValue>(this TValue[] array, TValue value, int startIndex = 0, int count = -1) where TValue : struct, IEquatable<TValue>
		{
			if (array == null)
			{
				return -1;
			}
			if (count < 0)
			{
				count = array.Length - startIndex;
			}
			for (int i = startIndex; i < startIndex + count; i++)
			{
				if (value.Equals(array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00050A38 File Offset: 0x0004EC38
		public static void Resize<TValue>(ref NativeArray<TValue> array, int newSize, Allocator allocator) where TValue : struct
		{
			int length = array.Length;
			if (length == newSize)
			{
				return;
			}
			if (newSize == 0)
			{
				if (array.IsCreated)
				{
					array.Dispose();
				}
				array = default(NativeArray<TValue>);
				return;
			}
			NativeArray<TValue> nativeArray = new NativeArray<TValue>(newSize, allocator, NativeArrayOptions.ClearMemory);
			if (length != 0)
			{
				UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<TValue>(), array.GetUnsafeReadOnlyPtr<TValue>(), (long)(UnsafeUtility.SizeOf<TValue>() * ((newSize < length) ? newSize : length)));
				array.Dispose();
			}
			array = nativeArray;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00050AAC File Offset: 0x0004ECAC
		public static int Append<TValue>(ref TValue[] array, TValue value)
		{
			if (array == null)
			{
				array = new TValue[1];
				array[0] = value;
				return 0;
			}
			int num = array.Length;
			Array.Resize<TValue>(ref array, num + 1);
			array[num] = value;
			return num;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00050AE8 File Offset: 0x0004ECE8
		public static int Append<TValue>(ref TValue[] array, IEnumerable<TValue> values)
		{
			if (array == null)
			{
				array = values.ToArray<TValue>();
				return 0;
			}
			int num = array.Length;
			int num2 = values.Count<TValue>();
			Array.Resize<TValue>(ref array, num + num2);
			int num3 = num;
			foreach (TValue tvalue in values)
			{
				array[num3++] = tvalue;
			}
			return num;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00050B60 File Offset: 0x0004ED60
		public static int AppendToImmutable<TValue>(ref TValue[] array, TValue[] values)
		{
			if (array == null)
			{
				array = values;
				return 0;
			}
			if (values != null && values.Length != 0)
			{
				int num = array.Length;
				int num2 = values.Length;
				Array.Resize<TValue>(ref array, num + num2);
				Array.Copy(values, 0, array, num, num2);
				return num;
			}
			return array.Length;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00050BA0 File Offset: 0x0004EDA0
		public static int AppendWithCapacity<TValue>(ref TValue[] array, ref int count, TValue value, int capacityIncrement = 10)
		{
			if (array == null)
			{
				array = new TValue[capacityIncrement];
				array[0] = value;
				count++;
				return 0;
			}
			int num = array.Length;
			if (num == count)
			{
				num += capacityIncrement;
				Array.Resize<TValue>(ref array, num);
			}
			int num2 = count;
			array[num2] = value;
			count++;
			return num2;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00050BF4 File Offset: 0x0004EDF4
		public static int AppendListWithCapacity<TValue, TValues>(ref TValue[] array, ref int length, TValues values, int capacityIncrement = 10) where TValues : IReadOnlyList<TValue>
		{
			int count = values.Count;
			if (array == null)
			{
				int num = Math.Max(count, capacityIncrement);
				array = new TValue[num];
				for (int i = 0; i < count; i++)
				{
					array[i] = values[i];
				}
				length += count;
				return 0;
			}
			int num2 = array.Length;
			if (num2 < length + count)
			{
				num2 += Math.Max(length + count, capacityIncrement);
				Array.Resize<TValue>(ref array, num2);
			}
			int num3 = length;
			for (int j = 0; j < count; j++)
			{
				array[num3 + j] = values[j];
			}
			length += count;
			return num3;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00050CA8 File Offset: 0x0004EEA8
		public static int AppendWithCapacity<TValue>(ref NativeArray<TValue> array, ref int count, TValue value, int capacityIncrement = 10, Allocator allocator = Allocator.Persistent) where TValue : struct
		{
			if (array.Length == count)
			{
				ArrayHelpers.GrowBy<TValue>(ref array, (capacityIncrement > 1) ? capacityIncrement : 1, allocator);
			}
			int num = count;
			array[num] = value;
			count++;
			return num;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00050CE4 File Offset: 0x0004EEE4
		public static void InsertAt<TValue>(ref TValue[] array, int index, TValue value)
		{
			if (array != null)
			{
				int num = array.Length;
				Array.Resize<TValue>(ref array, num + 1);
				if (index != num)
				{
					Array.Copy(array, index, array, index + 1, num - index);
				}
				array[index] = value;
				return;
			}
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			array = new TValue[1];
			array[0] = value;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00050D40 File Offset: 0x0004EF40
		public static void InsertAtWithCapacity<TValue>(ref TValue[] array, ref int count, int index, TValue value, int capacityIncrement = 10)
		{
			ArrayHelpers.EnsureCapacity<TValue>(ref array, count, count + 1, capacityIncrement);
			if (index != count)
			{
				Array.Copy(array, index, array, index + 1, count - index);
			}
			array[index] = value;
			count++;
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00050D78 File Offset: 0x0004EF78
		public static void PutAtIfNotSet<TValue>(ref TValue[] array, int index, Func<TValue> valueFn)
		{
			if (array.LengthSafe<TValue>() < index + 1)
			{
				Array.Resize<TValue>(ref array, index + 1);
			}
			if (EqualityComparer<TValue>.Default.Equals(array[index], default(TValue)))
			{
				array[index] = valueFn();
			}
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00050DC8 File Offset: 0x0004EFC8
		public static int GrowBy<TValue>(ref TValue[] array, int count)
		{
			if (array == null)
			{
				array = new TValue[count];
				return 0;
			}
			int num = array.Length;
			Array.Resize<TValue>(ref array, num + count);
			return num;
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00050DF4 File Offset: 0x0004EFF4
		public static int GrowBy<TValue>(ref NativeArray<TValue> array, int count, Allocator allocator = Allocator.Persistent) where TValue : struct
		{
			int length = array.Length;
			if (length == 0)
			{
				array = new NativeArray<TValue>(count, allocator, NativeArrayOptions.ClearMemory);
				return 0;
			}
			NativeArray<TValue> nativeArray = new NativeArray<TValue>(length + count, allocator, NativeArrayOptions.ClearMemory);
			UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<TValue>(), array.GetUnsafeReadOnlyPtr<TValue>(), (long)length * (long)UnsafeUtility.SizeOf<TValue>());
			array.Dispose();
			array = nativeArray;
			return length;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00050E54 File Offset: 0x0004F054
		public static int GrowWithCapacity<TValue>(ref TValue[] array, ref int count, int growBy, int capacityIncrement = 10)
		{
			if (((array != null) ? array.Length : 0) < count + growBy)
			{
				if (capacityIncrement < growBy)
				{
					capacityIncrement = growBy;
				}
				ArrayHelpers.GrowBy<TValue>(ref array, capacityIncrement);
			}
			int num = count;
			count += growBy;
			return num;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00050E7E File Offset: 0x0004F07E
		public static int GrowWithCapacity<TValue>(ref NativeArray<TValue> array, ref int count, int growBy, int capacityIncrement = 10, Allocator allocator = Allocator.Persistent) where TValue : struct
		{
			if (array.Length < count + growBy)
			{
				if (capacityIncrement < growBy)
				{
					capacityIncrement = growBy;
				}
				ArrayHelpers.GrowBy<TValue>(ref array, capacityIncrement, allocator);
			}
			int num = count;
			count += growBy;
			return num;
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x00050EA8 File Offset: 0x0004F0A8
		public static TValue[] Join<TValue>(TValue value, params TValue[] values)
		{
			int num = 0;
			if (value != null)
			{
				num++;
			}
			if (values != null)
			{
				num += values.Length;
			}
			if (num == 0)
			{
				return null;
			}
			TValue[] array = new TValue[num];
			int num2 = 0;
			if (value != null)
			{
				array[num2++] = value;
			}
			if (values != null)
			{
				Array.Copy(values, 0, array, num2, values.Length);
			}
			return array;
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00050F00 File Offset: 0x0004F100
		public static TValue[] Merge<TValue>(TValue[] first, TValue[] second) where TValue : IEquatable<TValue>
		{
			if (first == null)
			{
				return second;
			}
			if (second == null)
			{
				return first;
			}
			List<TValue> list = new List<TValue>();
			list.AddRange(first);
			for (int i = 0; i < second.Length; i++)
			{
				TValue secondValue = second[i];
				if (!list.Exists((TValue x) => x.Equals(secondValue)))
				{
					list.Add(secondValue);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00050F6C File Offset: 0x0004F16C
		public static TValue[] Merge<TValue>(TValue[] first, TValue[] second, IEqualityComparer<TValue> comparer)
		{
			if (first == null)
			{
				return second;
			}
			if (second == null)
			{
				return null;
			}
			List<TValue> list = new List<TValue>();
			list.AddRange(first);
			for (int i = 0; i < second.Length; i++)
			{
				TValue secondValue = second[i];
				if (!list.Exists((TValue x) => comparer.Equals(secondValue)))
				{
					list.Add(secondValue);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00050FEC File Offset: 0x0004F1EC
		public static void EraseAt<TValue>(ref TValue[] array, int index)
		{
			int num = array.Length;
			if (index == 0 && num == 1)
			{
				array = null;
				return;
			}
			if (index < num - 1)
			{
				Array.Copy(array, index + 1, array, index, num - index - 1);
			}
			Array.Resize<TValue>(ref array, num - 1);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0005102C File Offset: 0x0004F22C
		public static void EraseAtWithCapacity<TValue>(this TValue[] array, ref int count, int index)
		{
			if (index < count - 1)
			{
				Array.Copy(array, index + 1, array, index, count - index - 1);
			}
			array[count - 1] = default(TValue);
			count--;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0005106C File Offset: 0x0004F26C
		public unsafe static void EraseAtWithCapacity<TValue>(NativeArray<TValue> array, ref int count, int index) where TValue : struct
		{
			if (index < count - 1)
			{
				int num = UnsafeUtility.SizeOf<TValue>();
				byte* unsafePtr = (byte*)array.GetUnsafePtr<TValue>();
				UnsafeUtility.MemCpy((void*)(unsafePtr + num * index), (void*)(unsafePtr + num * (index + 1)), (long)((count - index - 1) * num));
			}
			count--;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000510B0 File Offset: 0x0004F2B0
		public static bool Erase<TValue>(ref TValue[] array, TValue value)
		{
			int num = ArrayHelpers.IndexOf<TValue>(array, value, 0, -1);
			if (num != -1)
			{
				ArrayHelpers.EraseAt<TValue>(ref array, num);
				return true;
			}
			return false;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x000510D8 File Offset: 0x0004F2D8
		public static void EraseAtByMovingTail<TValue>(TValue[] array, ref int count, int index)
		{
			if (index != count - 1)
			{
				array[index] = array[count - 1];
			}
			if (count >= 1)
			{
				array[count - 1] = default(TValue);
			}
			count--;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0005111C File Offset: 0x0004F31C
		public static TValue[] Copy<TValue>(TValue[] array)
		{
			if (array == null)
			{
				return null;
			}
			int num = array.Length;
			TValue[] array2 = new TValue[num];
			Array.Copy(array, array2, num);
			return array2;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00051144 File Offset: 0x0004F344
		public static TValue[] Clone<TValue>(TValue[] array) where TValue : ICloneable
		{
			if (array == null)
			{
				return null;
			}
			int num = array.Length;
			TValue[] array2 = new TValue[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = (TValue)((object)array[i].Clone());
			}
			return array2;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00051190 File Offset: 0x0004F390
		public static TNew[] Select<TOld, TNew>(TOld[] array, Func<TOld, TNew> converter)
		{
			if (array == null)
			{
				return null;
			}
			int num = array.Length;
			TNew[] array2 = new TNew[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = converter(array[i]);
			}
			return array2;
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x000511D0 File Offset: 0x0004F3D0
		private static void Swap<TValue>(ref TValue first, ref TValue second)
		{
			TValue tvalue = first;
			first = second;
			second = tvalue;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x000511F8 File Offset: 0x0004F3F8
		public static void MoveSlice<TValue>(TValue[] array, int sourceIndex, int destinationIndex, int count)
		{
			if (count <= 0 || sourceIndex == destinationIndex)
			{
				return;
			}
			int num;
			if (destinationIndex > sourceIndex)
			{
				num = destinationIndex + count - sourceIndex;
			}
			else
			{
				num = sourceIndex + count - destinationIndex;
			}
			if (num == count * 2)
			{
				for (int i = 0; i < count; i++)
				{
					ArrayHelpers.Swap<TValue>(ref array[sourceIndex + i], ref array[destinationIndex + i]);
				}
				return;
			}
			int num2 = num - 1;
			int num3 = destinationIndex;
			for (int j = 0; j < num2; j++)
			{
				ArrayHelpers.Swap<TValue>(ref array[num3], ref array[sourceIndex]);
				if (destinationIndex > sourceIndex)
				{
					num3 -= count;
					if (num3 < sourceIndex)
					{
						num3 = destinationIndex + count - Math.Abs(sourceIndex - num3);
					}
				}
				else
				{
					num3 += count;
					if (num3 >= sourceIndex + count)
					{
						num3 = destinationIndex + (num3 - (sourceIndex + count));
					}
				}
			}
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x000512A0 File Offset: 0x0004F4A0
		public static void EraseSliceWithCapacity<TValue>(ref TValue[] array, ref int length, int index, int count)
		{
			if (count < length)
			{
				Array.Copy(array, index + count, array, index, length - index - count);
			}
			for (int i = 0; i < count; i++)
			{
				array[length - i - 1] = default(TValue);
			}
			length -= count;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x000512ED File Offset: 0x0004F4ED
		public static void SwapElements<TValue>(this TValue[] array, int index1, int index2)
		{
			MemoryHelpers.Swap<TValue>(ref array[index1], ref array[index2]);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00051304 File Offset: 0x0004F504
		public static void SwapElements<TValue>(this NativeArray<TValue> array, int index1, int index2) where TValue : struct
		{
			TValue tvalue = array[index1];
			array[index1] = array[index2];
			array[index2] = tvalue;
		}
	}
}
