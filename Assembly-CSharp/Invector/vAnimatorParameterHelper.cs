using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200044E RID: 1102
	public static class vAnimatorParameterHelper
	{
		// Token: 0x06001B88 RID: 7048 RVA: 0x000AD860 File Offset: 0x000ABA60
		public static AnimatorControllerParameter GetValidParameter(this Animator animator, string paramenterName)
		{
			if (null == animator)
			{
				return null;
			}
			return Array.Find<AnimatorControllerParameter>(animator.parameters, (AnimatorControllerParameter p) => p.name.Equals(paramenterName));
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000AD89C File Offset: 0x000ABA9C
		public static bool GetValidParameter(this Animator animator, string paramenterName, out AnimatorControllerParameter parameter)
		{
			parameter = animator.GetValidParameter(paramenterName);
			return parameter != null;
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x000AD8AC File Offset: 0x000ABAAC
		public static bool ContainsParameter(this Animator animator, string paramenterName)
		{
			return !(null == animator) && Array.Exists<AnimatorControllerParameter>(animator.parameters, (AnimatorControllerParameter p) => p.name.Equals(paramenterName));
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000AD8E8 File Offset: 0x000ABAE8
		public static bool ContainsParameter(this Animator animator, string parameterName, AnimatorControllerParameterType parameterType)
		{
			return !(null == animator) && Array.Exists<AnimatorControllerParameter>(animator.parameters, (AnimatorControllerParameter p) => p.name.Equals(parameterName) && p.type.Equals(parameterType));
		}
	}
}
