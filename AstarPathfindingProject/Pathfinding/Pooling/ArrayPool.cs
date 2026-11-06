using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pathfinding.Pooling
{
	// Token: 0x02000254 RID: 596
	public static class ArrayPool<T>
	{
		// Token: 0x06000EDE RID: 3806 RVA: 0x0005BB8C File Offset: 0x00059D8C
		static ArrayPool()
		{
			if (!ArrayPool<T>.containsReferenceTypes)
			{
				foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (fieldInfo.FieldType.IsClass || fieldInfo.FieldType.IsInterface || fieldInfo.FieldType.IsArray)
					{
						ArrayPool<T>.containsReferenceTypes = true;
						return;
					}
				}
			}
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0005BC48 File Offset: 0x00059E48
		public static T[] Claim(int minimumLength)
		{
			if (minimumLength <= 0)
			{
				return ArrayPool<T>.ClaimWithExactLength(0);
			}
			int num = 0;
			while (1 << num < minimumLength && num < 30)
			{
				num++;
			}
			if (num == 30)
			{
				throw new ArgumentException("Too high minimum length");
			}
			Stack<T[]>[] array = ArrayPool<T>.pool;
			lock (array)
			{
				if (ArrayPool<T>.pool[num] == null)
				{
					ArrayPool<T>.pool[num] = new Stack<T[]>();
				}
				if (ArrayPool<T>.pool[num].Count > 0)
				{
					return ArrayPool<T>.pool[num].Pop();
				}
			}
			return new T[1 << num];
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0005BCF4 File Offset: 0x00059EF4
		public static T[] ClaimWithExactLength(int length)
		{
			if (length != 0 && (length & (length - 1)) == 0)
			{
				return ArrayPool<T>.Claim(length);
			}
			if (length <= 256)
			{
				Stack<T[]>[] array = ArrayPool<T>.pool;
				lock (array)
				{
					Stack<T[]> stack = ArrayPool<T>.exactPool[length];
					if (stack != null && stack.Count > 0)
					{
						return stack.Pop();
					}
				}
			}
			return new T[length];
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0005BD74 File Offset: 0x00059F74
		public static void Release(ref T[] array, bool allowNonPowerOfTwo = false)
		{
			if (array == null)
			{
				return;
			}
			if (array.GetType() != typeof(T[]))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Expected array type ",
					typeof(T[]).Name,
					" but found ",
					array.GetType().Name,
					"\nAre you using the correct generic class?\n"
				}));
			}
			bool flag = array.Length != 0 && (array.Length & (array.Length - 1)) == 0;
			if (!flag && !allowNonPowerOfTwo && array.Length != 0)
			{
				throw new ArgumentException("Length is not a power of 2");
			}
			if (ArrayPool<T>.containsReferenceTypes)
			{
				Array.Fill<T>(array, default(T));
			}
			Stack<T[]>[] array2 = ArrayPool<T>.pool;
			lock (array2)
			{
				if (flag)
				{
					int num = 0;
					while (1 << num < array.Length && num < 30)
					{
						num++;
					}
					if (ArrayPool<T>.pool[num] == null)
					{
						ArrayPool<T>.pool[num] = new Stack<T[]>();
					}
					ArrayPool<T>.pool[num].Push(array);
				}
				else if (array.Length <= 256)
				{
					Stack<T[]> stack = ArrayPool<T>.exactPool[array.Length];
					if (stack == null)
					{
						stack = (ArrayPool<T>.exactPool[array.Length] = new Stack<T[]>());
					}
					stack.Push(array);
				}
			}
			array = null;
		}

		// Token: 0x04000B22 RID: 2850
		private const int MaximumExactArrayLength = 256;

		// Token: 0x04000B23 RID: 2851
		private static readonly bool containsReferenceTypes = typeof(T).IsClass || typeof(T).IsInterface || typeof(T).IsArray;

		// Token: 0x04000B24 RID: 2852
		private static readonly Stack<T[]>[] pool = new Stack<T[]>[31];

		// Token: 0x04000B25 RID: 2853
		private static readonly Stack<T[]>[] exactPool = new Stack<T[]>[257];
	}
}
