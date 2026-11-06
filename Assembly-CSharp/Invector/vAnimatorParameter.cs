using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200044D RID: 1101
	public class vAnimatorParameter
	{
		// Token: 0x06001B86 RID: 7046 RVA: 0x000AD814 File Offset: 0x000ABA14
		public static implicit operator int(vAnimatorParameter a)
		{
			if (a.isValid)
			{
				return a._parameter.nameHash;
			}
			return -1;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000AD82B File Offset: 0x000ABA2B
		public vAnimatorParameter(Animator animator, string parameter)
		{
			if (animator && animator.ContainsParameter(parameter))
			{
				this._parameter = animator.GetValidParameter(parameter);
				this.isValid = true;
				return;
			}
			this.isValid = false;
		}

		// Token: 0x04001CD3 RID: 7379
		private readonly AnimatorControllerParameter _parameter;

		// Token: 0x04001CD4 RID: 7380
		public readonly bool isValid;
	}
}
