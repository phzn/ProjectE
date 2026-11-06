using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000155 RID: 341
	public struct MMStateChangeEvent<T> where T : struct, IComparable, IConvertible, IFormattable
	{
		// Token: 0x06000792 RID: 1938 RVA: 0x00024350 File Offset: 0x00022550
		public MMStateChangeEvent(MMStateMachine<T> stateMachine)
		{
			this.Target = stateMachine.Target;
			this.TargetStateMachine = stateMachine;
			this.NewState = stateMachine.CurrentState;
			this.PreviousState = stateMachine.PreviousState;
		}

		// Token: 0x0400092C RID: 2348
		public GameObject Target;

		// Token: 0x0400092D RID: 2349
		public MMStateMachine<T> TargetStateMachine;

		// Token: 0x0400092E RID: 2350
		public T NewState;

		// Token: 0x0400092F RID: 2351
		public T PreviousState;
	}
}
