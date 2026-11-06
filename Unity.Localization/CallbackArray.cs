using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace UnityEngine.Localization
{
	// Token: 0x02000021 RID: 33
	internal struct CallbackArray<TDelegate> where TDelegate : Delegate
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004501 File Offset: 0x00002701
		public TDelegate SingleDelegate
		{
			get
			{
				return this.m_SingleDelegate;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004509 File Offset: 0x00002709
		public TDelegate[] MultiDelegates
		{
			get
			{
				return this.m_MultipleDelegates;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00004511 File Offset: 0x00002711
		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000451C File Offset: 0x0000271C
		public void Add(TDelegate callback, int capacityIncrement = 5)
		{
			if (callback == null)
			{
				return;
			}
			if (this.m_CannotMutateCallbacksArray)
			{
				if (this.m_AddCallbacks == null)
				{
					this.m_AddCallbacks = CollectionPool<List<TDelegate>, TDelegate>.Get();
				}
				this.m_AddCallbacks.Add(callback);
				this.m_MutatedDuringCallback = true;
				return;
			}
			if (this.m_Length == 0)
			{
				this.m_SingleDelegate = callback;
			}
			else if (this.m_Length == 1)
			{
				this.m_MultipleDelegates = new TDelegate[capacityIncrement];
				this.m_MultipleDelegates[0] = this.m_SingleDelegate;
				this.m_MultipleDelegates[1] = callback;
				this.m_SingleDelegate = default(TDelegate);
			}
			else
			{
				if (this.m_MultipleDelegates.Length == this.m_Length)
				{
					Array.Resize<TDelegate>(ref this.m_MultipleDelegates, this.m_Length + capacityIncrement);
				}
				this.m_MultipleDelegates[this.m_Length] = callback;
			}
			this.m_Length++;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004600 File Offset: 0x00002800
		public void RemoveByMovingTail(TDelegate callback)
		{
			if (callback == null)
			{
				return;
			}
			if (this.m_CannotMutateCallbacksArray)
			{
				if (this.m_RemoveCallbacks == null)
				{
					this.m_RemoveCallbacks = CollectionPool<List<TDelegate>, TDelegate>.Get();
				}
				this.m_RemoveCallbacks.Add(callback);
				this.m_MutatedDuringCallback = true;
				return;
			}
			if (this.m_Length <= 1)
			{
				if (object.Equals(this.m_SingleDelegate, callback))
				{
					this.m_SingleDelegate = default(TDelegate);
					this.m_Length = 0;
					return;
				}
			}
			else
			{
				for (int i = 0; i < this.m_Length; i++)
				{
					if (object.Equals(this.m_MultipleDelegates[i], callback))
					{
						this.m_MultipleDelegates[i] = this.m_MultipleDelegates[this.m_Length - 1];
						this.m_Length--;
						break;
					}
				}
				if (this.m_Length == 1)
				{
					this.m_SingleDelegate = this.m_MultipleDelegates[0];
					this.m_MultipleDelegates = null;
				}
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004703 File Offset: 0x00002903
		public void LockForChanges()
		{
			this.m_CannotMutateCallbacksArray = true;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000470C File Offset: 0x0000290C
		public void UnlockForChanges()
		{
			this.m_CannotMutateCallbacksArray = false;
			if (this.m_MutatedDuringCallback)
			{
				if (this.m_AddCallbacks != null)
				{
					foreach (TDelegate tdelegate in this.m_AddCallbacks)
					{
						this.Add(tdelegate, 5);
					}
					CollectionPool<List<TDelegate>, TDelegate>.Release(this.m_AddCallbacks);
					this.m_AddCallbacks = null;
				}
				if (this.m_RemoveCallbacks != null)
				{
					foreach (TDelegate tdelegate2 in this.m_RemoveCallbacks)
					{
						this.RemoveByMovingTail(tdelegate2);
					}
					CollectionPool<List<TDelegate>, TDelegate>.Release(this.m_RemoveCallbacks);
					this.m_RemoveCallbacks = null;
				}
				this.m_MutatedDuringCallback = true;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000047F0 File Offset: 0x000029F0
		public void Clear()
		{
			this.m_SingleDelegate = default(TDelegate);
			this.m_MultipleDelegates = null;
			this.m_Length = 0;
		}

		// Token: 0x04000035 RID: 53
		private const int k_AllocationIncrement = 5;

		// Token: 0x04000036 RID: 54
		private TDelegate m_SingleDelegate;

		// Token: 0x04000037 RID: 55
		private TDelegate[] m_MultipleDelegates;

		// Token: 0x04000038 RID: 56
		private List<TDelegate> m_AddCallbacks;

		// Token: 0x04000039 RID: 57
		private List<TDelegate> m_RemoveCallbacks;

		// Token: 0x0400003A RID: 58
		private int m_Length;

		// Token: 0x0400003B RID: 59
		private bool m_CannotMutateCallbacksArray;

		// Token: 0x0400003C RID: 60
		private bool m_MutatedDuringCallback;
	}
}
