using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Pooling
{
	// Token: 0x0200025B RID: 603
	public static class StackPool<T>
	{
		// Token: 0x06000EFB RID: 3835 RVA: 0x0005C574 File Offset: 0x0005A774
		public static Stack<T> Claim()
		{
			List<Stack<T>> list = StackPool<T>.pool;
			lock (list)
			{
				if (StackPool<T>.pool.Count > 0)
				{
					Stack<T> stack = StackPool<T>.pool[StackPool<T>.pool.Count - 1];
					StackPool<T>.pool.RemoveAt(StackPool<T>.pool.Count - 1);
					return stack;
				}
			}
			return new Stack<T>();
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0005C5F0 File Offset: 0x0005A7F0
		public static void Warmup(int count)
		{
			Stack<T>[] array = new Stack<T>[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = StackPool<T>.Claim();
			}
			for (int j = 0; j < count; j++)
			{
				StackPool<T>.Release(array[j]);
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0005C62C File Offset: 0x0005A82C
		public static void Release(Stack<T> stack)
		{
			stack.Clear();
			List<Stack<T>> list = StackPool<T>.pool;
			lock (list)
			{
				for (int i = 0; i < StackPool<T>.pool.Count; i++)
				{
					if (StackPool<T>.pool[i] == stack)
					{
						Debug.LogError("The Stack is released even though it is inside the pool");
					}
				}
				StackPool<T>.pool.Add(stack);
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0005C6A4 File Offset: 0x0005A8A4
		public static void Clear()
		{
			List<Stack<T>> list = StackPool<T>.pool;
			lock (list)
			{
				StackPool<T>.pool.Clear();
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0005C6E8 File Offset: 0x0005A8E8
		public static int GetSize()
		{
			return StackPool<T>.pool.Count;
		}

		// Token: 0x04000B30 RID: 2864
		private static readonly List<Stack<T>> pool = new List<Stack<T>>();
	}
}
