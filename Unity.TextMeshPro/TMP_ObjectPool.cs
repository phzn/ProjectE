using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro
{
	// Token: 0x02000048 RID: 72
	internal class TMP_ObjectPool<T> where T : new()
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0002493D File Offset: 0x00022B3D
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00024945 File Offset: 0x00022B45
		public int countAll { get; private set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0002494E File Offset: 0x00022B4E
		public int countActive
		{
			get
			{
				return this.countAll - this.countInactive;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0002495D File Offset: 0x00022B5D
		public int countInactive
		{
			get
			{
				return this.m_Stack.Count;
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0002496A File Offset: 0x00022B6A
		public TMP_ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
		{
			this.m_ActionOnGet = actionOnGet;
			this.m_ActionOnRelease = actionOnRelease;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0002498C File Offset: 0x00022B8C
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

		// Token: 0x06000358 RID: 856 RVA: 0x000249E0 File Offset: 0x00022BE0
		public void Release(T element)
		{
			if (this.m_Stack.Count > 0 && this.m_Stack.Peek() == element)
			{
				Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
			}
			if (this.m_ActionOnRelease != null)
			{
				this.m_ActionOnRelease(element);
			}
			this.m_Stack.Push(element);
		}

		// Token: 0x0400028A RID: 650
		private readonly Stack<T> m_Stack = new Stack<T>();

		// Token: 0x0400028B RID: 651
		private readonly UnityAction<T> m_ActionOnGet;

		// Token: 0x0400028C RID: 652
		private readonly UnityAction<T> m_ActionOnRelease;
	}
}
