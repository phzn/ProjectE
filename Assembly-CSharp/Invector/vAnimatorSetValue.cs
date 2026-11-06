using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000463 RID: 1123
	[Serializable]
	public abstract class vAnimatorSetValue<T> : StateMachineBehaviour where T : IConvertible
	{
		// Token: 0x06001BE3 RID: 7139 RVA: 0x000AF424 File Offset: 0x000AD624
		protected virtual T GetEnterValue()
		{
			return this.enterValue;
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000AF42C File Offset: 0x000AD62C
		protected virtual T GetExitValue()
		{
			return this.exitValue;
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000AF434 File Offset: 0x000AD634
		public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.setOnEnter)
			{
				if (typeof(T).Equals(typeof(int)))
				{
					animator.SetInteger(this.animatorParameter, (int)((object)this.GetEnterValue()));
					return;
				}
				if (typeof(T).Equals(typeof(float)))
				{
					animator.SetFloat(this.animatorParameter, (float)((object)this.GetEnterValue()));
					return;
				}
				if (typeof(T).Equals(typeof(bool)))
				{
					animator.SetBool(this.animatorParameter, (bool)((object)this.GetEnterValue()));
				}
			}
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000AF4F4 File Offset: 0x000AD6F4
		public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.setOnExit)
			{
				if (typeof(T).Equals(typeof(int)))
				{
					animator.SetInteger(this.animatorParameter, (int)((object)this.GetExitValue()));
					return;
				}
				if (typeof(T).Equals(typeof(float)))
				{
					animator.SetFloat(this.animatorParameter, (float)((object)this.GetExitValue()));
					return;
				}
				if (typeof(T).Equals(typeof(bool)))
				{
					animator.SetBool(this.animatorParameter, (bool)((object)this.GetExitValue()));
				}
			}
		}

		// Token: 0x04001D2E RID: 7470
		public string animatorParameter = "My Animator Parameter";

		// Token: 0x04001D2F RID: 7471
		public bool setOnEnter;

		// Token: 0x04001D30 RID: 7472
		[vHideInInspector("setOnEnter", false)]
		public T enterValue;

		// Token: 0x04001D31 RID: 7473
		public bool setOnExit;

		// Token: 0x04001D32 RID: 7474
		[vHideInInspector("setOnExit", false)]
		public T exitValue;
	}
}
