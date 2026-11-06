using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000157 RID: 343
	public class MMStateMachine<T> : MMIStateMachine where T : struct, IComparable, IConvertible, IFormattable
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0002437D File Offset: 0x0002257D
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x00024385 File Offset: 0x00022585
		public virtual bool TriggerEvents { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0002438E File Offset: 0x0002258E
		// (set) Token: 0x06000798 RID: 1944 RVA: 0x00024396 File Offset: 0x00022596
		public virtual T CurrentState { get; protected set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x0002439F File Offset: 0x0002259F
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x000243A7 File Offset: 0x000225A7
		public virtual T PreviousState { get; protected set; }

		// Token: 0x0600079B RID: 1947 RVA: 0x000243B0 File Offset: 0x000225B0
		public MMStateMachine(GameObject target, bool triggerEvents)
		{
			this.Target = target;
			this.TriggerEvents = triggerEvents;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000243C8 File Offset: 0x000225C8
		public virtual void ChangeState(T newState)
		{
			if (EqualityComparer<T>.Default.Equals(newState, this.CurrentState))
			{
				return;
			}
			this.PreviousState = this.CurrentState;
			this.CurrentState = newState;
			MMStateMachine<T>.OnStateChangeDelegate onStateChange = this.OnStateChange;
			if (onStateChange != null)
			{
				onStateChange();
			}
			if (this.TriggerEvents)
			{
				MMEventManager.TriggerEvent<MMStateChangeEvent<T>>(new MMStateChangeEvent<T>(this));
			}
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00024420 File Offset: 0x00022620
		public virtual void RestorePreviousState()
		{
			this.CurrentState = this.PreviousState;
			MMStateMachine<T>.OnStateChangeDelegate onStateChange = this.OnStateChange;
			if (onStateChange != null)
			{
				onStateChange();
			}
			if (this.TriggerEvents)
			{
				MMEventManager.TriggerEvent<MMStateChangeEvent<T>>(new MMStateChangeEvent<T>(this));
			}
		}

		// Token: 0x04000931 RID: 2353
		public GameObject Target;

		// Token: 0x04000934 RID: 2356
		public MMStateMachine<T>.OnStateChangeDelegate OnStateChange;

		// Token: 0x02000459 RID: 1113
		// (Invoke) Token: 0x06001A83 RID: 6787
		public delegate void OnStateChangeDelegate();
	}
}
