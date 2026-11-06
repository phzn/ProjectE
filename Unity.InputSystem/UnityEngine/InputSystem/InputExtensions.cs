using System;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000058 RID: 88
	public static class InputExtensions
	{
		// Token: 0x06000865 RID: 2149 RVA: 0x0002EE13 File Offset: 0x0002D013
		public static bool IsInProgress(this InputActionPhase phase)
		{
			return phase == InputActionPhase.Started || phase == InputActionPhase.Performed;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0002EE1F File Offset: 0x0002D01F
		public static bool IsEndedOrCanceled(this TouchPhase phase)
		{
			return phase == TouchPhase.Canceled || phase == TouchPhase.Ended;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0002EE2B File Offset: 0x0002D02B
		public static bool IsActive(this TouchPhase phase)
		{
			return phase - TouchPhase.Began <= 1 || phase == TouchPhase.Stationary;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0002EE3A File Offset: 0x0002D03A
		public static bool IsModifierKey(this Key key)
		{
			return key - Key.LeftShift <= 7;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0002EE46 File Offset: 0x0002D046
		public static bool IsTextInputKey(this Key key)
		{
			return key > Key.Tab && key - Key.LeftShift > 26 && key - Key.F1 > 29;
		}
	}
}
