using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200001B RID: 27
	public sealed class RenderGraphObjectPool
	{
		// Token: 0x0600011A RID: 282 RVA: 0x0000756F File Offset: 0x0000576F
		internal RenderGraphObjectPool()
		{
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00007598 File Offset: 0x00005798
		public T[] GetTempArray<T>(int size)
		{
			Stack<object> stack;
			if (!this.m_ArrayPool.TryGetValue(new ValueTuple<Type, int>(typeof(T), size), out stack))
			{
				stack = new Stack<object>();
				this.m_ArrayPool.Add(new ValueTuple<Type, int>(typeof(T), size), stack);
			}
			T[] array = ((stack.Count > 0) ? ((T[])stack.Pop()) : new T[size]);
			this.m_AllocatedArrays.Add(new ValueTuple<object, ValueTuple<Type, int>>(array, new ValueTuple<Type, int>(typeof(T), size)));
			return array;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007628 File Offset: 0x00005828
		public MaterialPropertyBlock GetTempMaterialPropertyBlock()
		{
			MaterialPropertyBlock materialPropertyBlock = RenderGraphObjectPool.SharedObjectPool<MaterialPropertyBlock>.sharedPool.Get();
			materialPropertyBlock.Clear();
			this.m_AllocatedMaterialPropertyBlocks.Add(materialPropertyBlock);
			return materialPropertyBlock;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00007654 File Offset: 0x00005854
		internal void ReleaseAllTempAlloc()
		{
			foreach (ValueTuple<object, ValueTuple<Type, int>> valueTuple in this.m_AllocatedArrays)
			{
				Stack<object> stack;
				this.m_ArrayPool.TryGetValue(valueTuple.Item2, out stack);
				stack.Push(valueTuple.Item1);
			}
			this.m_AllocatedArrays.Clear();
			foreach (MaterialPropertyBlock materialPropertyBlock in this.m_AllocatedMaterialPropertyBlocks)
			{
				RenderGraphObjectPool.SharedObjectPool<MaterialPropertyBlock>.sharedPool.Release(materialPropertyBlock);
			}
			this.m_AllocatedMaterialPropertyBlocks.Clear();
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007720 File Offset: 0x00005920
		internal T Get<T>() where T : new()
		{
			return RenderGraphObjectPool.SharedObjectPool<T>.sharedPool.Get();
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000772C File Offset: 0x0000592C
		internal void Release<T>(T value) where T : new()
		{
			RenderGraphObjectPool.SharedObjectPool<T>.sharedPool.Release(value);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00007739 File Offset: 0x00005939
		internal void Cleanup()
		{
			this.m_AllocatedArrays.Clear();
			this.m_AllocatedMaterialPropertyBlocks.Clear();
			this.m_ArrayPool.Clear();
			RenderGraphObjectPool.SharedObjectPoolBase.ClearAll();
		}

		// Token: 0x040000A7 RID: 167
		private Dictionary<ValueTuple<Type, int>, Stack<object>> m_ArrayPool = new Dictionary<ValueTuple<Type, int>, Stack<object>>();

		// Token: 0x040000A8 RID: 168
		private List<ValueTuple<object, ValueTuple<Type, int>>> m_AllocatedArrays = new List<ValueTuple<object, ValueTuple<Type, int>>>();

		// Token: 0x040000A9 RID: 169
		private List<MaterialPropertyBlock> m_AllocatedMaterialPropertyBlocks = new List<MaterialPropertyBlock>();

		// Token: 0x0200014D RID: 333
		private abstract class SharedObjectPoolBase
		{
			// Token: 0x060009B5 RID: 2485
			protected abstract void Clear();

			// Token: 0x060009B6 RID: 2486 RVA: 0x0002C020 File Offset: 0x0002A220
			public static void ClearAll()
			{
				foreach (RenderGraphObjectPool.SharedObjectPoolBase sharedObjectPoolBase in RenderGraphObjectPool.SharedObjectPoolBase.s_AllocatedPools)
				{
					sharedObjectPoolBase.Clear();
				}
			}

			// Token: 0x040005CC RID: 1484
			protected static List<RenderGraphObjectPool.SharedObjectPoolBase> s_AllocatedPools = new List<RenderGraphObjectPool.SharedObjectPoolBase>();
		}

		// Token: 0x0200014E RID: 334
		private class SharedObjectPool<T> : RenderGraphObjectPool.SharedObjectPoolBase where T : new()
		{
			// Token: 0x060009B9 RID: 2489 RVA: 0x0002C084 File Offset: 0x0002A284
			public T Get()
			{
				if (this.m_Pool.Count != 0)
				{
					return this.m_Pool.Pop();
				}
				return new T();
			}

			// Token: 0x060009BA RID: 2490 RVA: 0x0002C0A4 File Offset: 0x0002A2A4
			public void Release(T value)
			{
				this.m_Pool.Push(value);
			}

			// Token: 0x060009BB RID: 2491 RVA: 0x0002C0B4 File Offset: 0x0002A2B4
			private static RenderGraphObjectPool.SharedObjectPool<T> AllocatePool()
			{
				RenderGraphObjectPool.SharedObjectPool<T> sharedObjectPool = new RenderGraphObjectPool.SharedObjectPool<T>();
				RenderGraphObjectPool.SharedObjectPoolBase.s_AllocatedPools.Add(sharedObjectPool);
				return sharedObjectPool;
			}

			// Token: 0x060009BC RID: 2492 RVA: 0x0002C0D3 File Offset: 0x0002A2D3
			protected override void Clear()
			{
				this.m_Pool.Clear();
			}

			// Token: 0x1700014A RID: 330
			// (get) Token: 0x060009BD RID: 2493 RVA: 0x0002C0E0 File Offset: 0x0002A2E0
			public static RenderGraphObjectPool.SharedObjectPool<T> sharedPool
			{
				get
				{
					return RenderGraphObjectPool.SharedObjectPool<T>.s_Instance.Value;
				}
			}

			// Token: 0x040005CD RID: 1485
			private Stack<T> m_Pool = new Stack<T>();

			// Token: 0x040005CE RID: 1486
			private static readonly Lazy<RenderGraphObjectPool.SharedObjectPool<T>> s_Instance = new Lazy<RenderGraphObjectPool.SharedObjectPool<T>>(new Func<RenderGraphObjectPool.SharedObjectPool<T>>(RenderGraphObjectPool.SharedObjectPool<T>.AllocatePool));
		}
	}
}
