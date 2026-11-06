using System;
using UnityEngine;

namespace Features.Audio
{
	// Token: 0x02001531 RID: 5425
	public static class AnimatorExtensions
	{
		// Token: 0x0600A80B RID: 43019 RVA: 0x00462D3C File Offset: 0x00460F3C
		public static bool HasParameter(this Animator animator, string paramName)
		{
			AnimatorControllerParameter[] parameters = animator.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].name == paramName)
				{
					return true;
				}
			}
			return false;
		}
	}
}
