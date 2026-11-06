using System;
using System.Collections.Generic;

namespace Pathfinding.Pooling
{
	// Token: 0x0200025A RID: 602
	public static class PathPool
	{
		// Token: 0x06000EF5 RID: 3829 RVA: 0x0005C3A8 File Offset: 0x0005A5A8
		public static void Pool(Path path)
		{
			if (((IPathInternals)path).Pooled)
			{
				throw new ArgumentException("The path is already pooled.");
			}
			((IPathInternals)path).Pooled = true;
			((IPathInternals)path).OnEnterPool();
			Type type = path.GetType();
			Dictionary<Type, Stack<Path>> dictionary = PathPool.pool;
			lock (dictionary)
			{
				Stack<Path> stack;
				if (!PathPool.pool.TryGetValue(type, out stack))
				{
					stack = new Stack<Path>();
					PathPool.pool[type] = stack;
				}
				stack.Push(path);
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0005C430 File Offset: 0x0005A630
		public static int GetTotalCreated(Type type)
		{
			int num;
			if (PathPool.totalCreated.TryGetValue(type, out num))
			{
				return num;
			}
			return 0;
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0005C450 File Offset: 0x0005A650
		public static int GetSize(Type type)
		{
			Stack<Path> stack;
			if (PathPool.pool.TryGetValue(type, out stack))
			{
				return stack.Count;
			}
			return 0;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0005C474 File Offset: 0x0005A674
		public static T GetPath<T>() where T : Path, new()
		{
			Dictionary<Type, Stack<Path>> dictionary = PathPool.pool;
			T t;
			lock (dictionary)
			{
				Stack<Path> stack;
				if (PathPool.pool.TryGetValue(typeof(T), out stack) && stack.Count > 0)
				{
					t = stack.Pop() as T;
				}
				else
				{
					t = new T();
					if (!PathPool.totalCreated.ContainsKey(typeof(T)))
					{
						PathPool.totalCreated[typeof(T)] = 0;
					}
					Dictionary<Type, int> dictionary2 = PathPool.totalCreated;
					Type typeFromHandle = typeof(T);
					int num = dictionary2[typeFromHandle];
					dictionary2[typeFromHandle] = num + 1;
				}
			}
			t.Pooled = false;
			t.Reset();
			return t;
		}

		// Token: 0x04000B2E RID: 2862
		private static readonly Dictionary<Type, Stack<Path>> pool = new Dictionary<Type, Stack<Path>>();

		// Token: 0x04000B2F RID: 2863
		private static readonly Dictionary<Type, int> totalCreated = new Dictionary<Type, int>();
	}
}
