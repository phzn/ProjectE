using System;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x020001E4 RID: 484
	public class MUIPInternalTools : MonoBehaviour
	{
		// Token: 0x06000DF3 RID: 3571 RVA: 0x000601BC File Offset: 0x0005E3BC
		public static float GetAnimatorClipLength(Animator _animator, string _clipName)
		{
			float num = -1f;
			RuntimeAnimatorController runtimeAnimatorController = _animator.runtimeAnimatorController;
			for (int i = 0; i < runtimeAnimatorController.animationClips.Length; i++)
			{
				if (runtimeAnimatorController.animationClips[i].name == _clipName)
				{
					num = runtimeAnimatorController.animationClips[i].length;
					break;
				}
			}
			return num;
		}

		// Token: 0x04000E8D RID: 3725
		public static string modalWindowStateName = "Fade-out";

		// Token: 0x04000E8E RID: 3726
		public static string windowManagerStateName = "WM Window Out";
	}
}
