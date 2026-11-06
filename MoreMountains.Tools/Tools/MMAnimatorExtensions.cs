using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000E1 RID: 225
	public static class MMAnimatorExtensions
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x00016748 File Offset: 0x00014948
		public static bool MMHasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in self.parameters)
			{
				if (animatorControllerParameter.type == type && animatorControllerParameter.name == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00016792 File Offset: 0x00014992
		public static void AddAnimatorParameterIfExists(Animator animator, string parameterName, out int parameter, AnimatorControllerParameterType type, HashSet<int> parameterList)
		{
			if (string.IsNullOrEmpty(parameterName))
			{
				parameter = -1;
				return;
			}
			parameter = Animator.StringToHash(parameterName);
			if (animator.MMHasParameterOfType(parameterName, type))
			{
				parameterList.Add(parameter);
			}
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x000167BC File Offset: 0x000149BC
		public static void AddAnimatorParameterIfExists(Animator animator, string parameterName, AnimatorControllerParameterType type, HashSet<string> parameterList)
		{
			if (animator.MMHasParameterOfType(parameterName, type))
			{
				parameterList.Add(parameterName);
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x000167D0 File Offset: 0x000149D0
		public static void UpdateAnimatorBool(Animator animator, string parameterName, bool value)
		{
			animator.SetBool(parameterName, value);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x000167DA File Offset: 0x000149DA
		public static void UpdateAnimatorInteger(Animator animator, string parameterName, int value)
		{
			animator.SetInteger(parameterName, value);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000167E4 File Offset: 0x000149E4
		public static void UpdateAnimatorFloat(Animator animator, string parameterName, float value, bool performSanityCheck = true)
		{
			animator.SetFloat(parameterName, value);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000167EE File Offset: 0x000149EE
		public static bool UpdateAnimatorBool(Animator animator, int parameter, bool value, HashSet<int> parameterList, bool performSanityCheck = true)
		{
			if (performSanityCheck && !parameterList.Contains(parameter))
			{
				return false;
			}
			animator.SetBool(parameter, value);
			return true;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00016808 File Offset: 0x00014A08
		public static bool UpdateAnimatorTrigger(Animator animator, int parameter, HashSet<int> parameterList, bool performSanityCheck = true)
		{
			if (performSanityCheck && !parameterList.Contains(parameter))
			{
				return false;
			}
			animator.SetTrigger(parameter);
			return true;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00016820 File Offset: 0x00014A20
		public static bool SetAnimatorTrigger(Animator animator, int parameter, HashSet<int> parameterList, bool performSanityCheck = true)
		{
			if (performSanityCheck && !parameterList.Contains(parameter))
			{
				return false;
			}
			animator.SetTrigger(parameter);
			return true;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00016838 File Offset: 0x00014A38
		public static bool UpdateAnimatorFloat(Animator animator, int parameter, float value, HashSet<int> parameterList, bool performSanityCheck = true)
		{
			if (performSanityCheck && !parameterList.Contains(parameter))
			{
				return false;
			}
			animator.SetFloat(parameter, value);
			return true;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00016852 File Offset: 0x00014A52
		public static bool UpdateAnimatorInteger(Animator animator, int parameter, int value, HashSet<int> parameterList, bool performSanityCheck = true)
		{
			if (performSanityCheck && !parameterList.Contains(parameter))
			{
				return false;
			}
			animator.SetInteger(parameter, value);
			return true;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001686C File Offset: 0x00014A6C
		public static void UpdateAnimatorBool(Animator animator, string parameterName, bool value, HashSet<string> parameterList, bool performSanityCheck = true)
		{
			if (parameterList.Contains(parameterName))
			{
				animator.SetBool(parameterName, value);
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001687F File Offset: 0x00014A7F
		public static void UpdateAnimatorTrigger(Animator animator, string parameterName, HashSet<string> parameterList, bool performSanityCheck = true)
		{
			if (parameterList.Contains(parameterName))
			{
				animator.SetTrigger(parameterName);
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00016891 File Offset: 0x00014A91
		public static void SetAnimatorTrigger(Animator animator, string parameterName, HashSet<string> parameterList, bool performSanityCheck = true)
		{
			if (parameterList.Contains(parameterName))
			{
				animator.SetTrigger(parameterName);
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000168A3 File Offset: 0x00014AA3
		public static void UpdateAnimatorFloat(Animator animator, string parameterName, float value, HashSet<string> parameterList, bool performSanityCheck = true)
		{
			if (parameterList.Contains(parameterName))
			{
				animator.SetFloat(parameterName, value);
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000168B6 File Offset: 0x00014AB6
		public static void UpdateAnimatorInteger(Animator animator, string parameterName, int value, HashSet<string> parameterList, bool performSanityCheck = true)
		{
			if (parameterList.Contains(parameterName))
			{
				animator.SetInteger(parameterName, value);
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x000168C9 File Offset: 0x00014AC9
		public static void UpdateAnimatorBoolIfExists(Animator animator, string parameterName, bool value, bool performSanityCheck = true)
		{
			if (animator.MMHasParameterOfType(parameterName, AnimatorControllerParameterType.Bool))
			{
				animator.SetBool(parameterName, value);
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000168DD File Offset: 0x00014ADD
		public static void UpdateAnimatorTriggerIfExists(Animator animator, string parameterName, bool performSanityCheck = true)
		{
			if (animator.MMHasParameterOfType(parameterName, AnimatorControllerParameterType.Trigger))
			{
				animator.SetTrigger(parameterName);
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000168F1 File Offset: 0x00014AF1
		public static void SetAnimatorTriggerIfExists(Animator animator, string parameterName, bool performSanityCheck = true)
		{
			if (animator.MMHasParameterOfType(parameterName, AnimatorControllerParameterType.Trigger))
			{
				animator.SetTrigger(parameterName);
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00016905 File Offset: 0x00014B05
		public static void UpdateAnimatorFloatIfExists(Animator animator, string parameterName, float value, bool performSanityCheck = true)
		{
			if (animator.MMHasParameterOfType(parameterName, AnimatorControllerParameterType.Float))
			{
				animator.SetFloat(parameterName, value);
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00016919 File Offset: 0x00014B19
		public static void UpdateAnimatorIntegerIfExists(Animator animator, string parameterName, int value, bool performSanityCheck = true)
		{
			if (animator.MMHasParameterOfType(parameterName, AnimatorControllerParameterType.Int))
			{
				animator.SetInteger(parameterName, value);
			}
		}
	}
}
