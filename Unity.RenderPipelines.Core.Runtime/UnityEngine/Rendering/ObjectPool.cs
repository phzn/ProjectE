using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.Rendering
{
	// Token: 0x0200004C RID: 76
	public class ObjectPool<T> where T : new()
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000C317 File Offset: 0x0000A517
		// (set) Token: 0x0600028F RID: 655 RVA: 0x0000C31F File Offset: 0x0000A51F
		public int countAll { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000C328 File Offset: 0x0000A528
		public int countActive
		{
			get
			{
				return this.countAll - this.countInactive;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000C337 File Offset: 0x0000A537
		public int countInactive
		{
			get
			{
				return this.m_Stack.Count;
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000C344 File Offset: 0x0000A544
		public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease, bool collectionCheck = true)
		{
			this.m_ActionOnGet = actionOnGet;
			this.m_ActionOnRelease = actionOnRelease;
			this.m_CollectionCheck = collectionCheck;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000C374 File Offset: 0x0000A574
		public T Get()
		{
			T t;
			if (this.m_Stack.Count == 0)
			{
				t = new T();
				int countAll = this.countAll;
				this.countAll = countAll + 1;
			}
			else
			{
				t = this.m_Stack.Pop();
			}
			if (this.m_ActionOnGet != null)
			{
				this.m_ActionOnGet(t);
			}
			return t;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		public ObjectPool<T>.PooledObject Get(out T v)
		{
			return new ObjectPool<T>.PooledObject(v = this.Get(), this);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000C3EA File Offset: 0x0000A5EA
		public void Release(T element)
		{
			if (this.m_ActionOnRelease != null)
			{
				this.m_ActionOnRelease(element);
			}
			this.m_Stack.Push(element);
		}

		// Token: 0x04000190 RID: 400
		private readonly Stack<T> m_Stack = new Stack<T>();

		// Token: 0x04000191 RID: 401
		private readonly UnityAction<T> m_ActionOnGet;

		// Token: 0x04000192 RID: 402
		private readonly UnityAction<T> m_ActionOnRelease;

		// Token: 0x04000193 RID: 403
		private readonly bool m_CollectionCheck = true;

		// Token: 0x0200015D RID: 349
		public struct PooledObject : IDisposable
		{
			// Token: 0x060009E1 RID: 2529 RVA: 0x0002C445 File Offset: 0x0002A645
			internal PooledObject(T value, ObjectPool<T> pool)
			{
				this.m_ToReturn = value;
				this.m_Pool = pool;
			}

			// Token: 0x060009E2 RID: 2530 RVA: 0x0002C455 File Offset: 0x0002A655
			void IDisposable.Dispose()
			{
				this.m_Pool.Release(this.m_ToReturn);
			}

			// Token: 0x040005E8 RID: 1512
			private readonly T m_ToReturn;

			// Token: 0x040005E9 RID: 1513
			private readonly ObjectPool<T> m_Pool;
		}
	}
}
