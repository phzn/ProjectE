using System;
using System.Diagnostics;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.EnhancedTouch
{
	// Token: 0x0200009D RID: 157
	public static class EnhancedTouchSupport
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x00041883 File Offset: 0x0003FA83
		public static bool enabled
		{
			get
			{
				return EnhancedTouchSupport.s_Enabled > 0;
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00041890 File Offset: 0x0003FA90
		public static void Enable()
		{
			EnhancedTouchSupport.s_Enabled++;
			if (EnhancedTouchSupport.s_Enabled > 1)
			{
				return;
			}
			InputSystem.onDeviceChange += EnhancedTouchSupport.OnDeviceChange;
			InputSystem.onBeforeUpdate += Touch.BeginUpdate;
			InputSystem.onSettingsChange += EnhancedTouchSupport.OnSettingsChange;
			EnhancedTouchSupport.SetUpState();
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x000418EC File Offset: 0x0003FAEC
		public static void Disable()
		{
			if (!EnhancedTouchSupport.enabled)
			{
				return;
			}
			EnhancedTouchSupport.s_Enabled--;
			if (EnhancedTouchSupport.s_Enabled > 0)
			{
				return;
			}
			InputSystem.onDeviceChange -= EnhancedTouchSupport.OnDeviceChange;
			InputSystem.onBeforeUpdate -= Touch.BeginUpdate;
			InputSystem.onSettingsChange -= EnhancedTouchSupport.OnSettingsChange;
			EnhancedTouchSupport.TearDownState();
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0004194E File Offset: 0x0003FB4E
		internal static void Reset()
		{
			Touch.s_GlobalState.touchscreens = default(InlinedArray<Touchscreen>);
			Touch.s_GlobalState.playerState.Destroy();
			Touch.s_GlobalState.playerState = default(Touch.FingerAndTouchState);
			EnhancedTouchSupport.s_Enabled = 0;
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00041988 File Offset: 0x0003FB88
		private static void SetUpState()
		{
			Touch.s_GlobalState.playerState.updateMask = InputUpdateType.Dynamic | InputUpdateType.Fixed | InputUpdateType.Manual;
			EnhancedTouchSupport.s_UpdateMode = InputSystem.settings.updateMode;
			foreach (InputDevice inputDevice in InputSystem.devices)
			{
				EnhancedTouchSupport.OnDeviceChange(inputDevice, InputDeviceChange.Added);
			}
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000419FC File Offset: 0x0003FBFC
		internal static void TearDownState()
		{
			foreach (InputDevice inputDevice in InputSystem.devices)
			{
				EnhancedTouchSupport.OnDeviceChange(inputDevice, InputDeviceChange.Removed);
			}
			Touch.s_GlobalState.playerState.Destroy();
			Touch.s_GlobalState.playerState = default(Touch.FingerAndTouchState);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00041A70 File Offset: 0x0003FC70
		private static void OnDeviceChange(InputDevice device, InputDeviceChange change)
		{
			if (change != InputDeviceChange.Added)
			{
				if (change != InputDeviceChange.Removed)
				{
					return;
				}
				Touchscreen touchscreen = device as Touchscreen;
				if (touchscreen != null)
				{
					Touch.RemoveTouchscreen(touchscreen);
				}
			}
			else
			{
				Touchscreen touchscreen2 = device as Touchscreen;
				if (touchscreen2 != null)
				{
					Touch.AddTouchscreen(touchscreen2);
					return;
				}
			}
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00041AA8 File Offset: 0x0003FCA8
		private static void OnSettingsChange()
		{
			InputSettings.UpdateMode updateMode = InputSystem.settings.updateMode;
			if (EnhancedTouchSupport.s_UpdateMode == updateMode)
			{
				return;
			}
			EnhancedTouchSupport.TearDownState();
			EnhancedTouchSupport.SetUpState();
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00041AD3 File Offset: 0x0003FCD3
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("UNITY_EDITOR")]
		internal static void CheckEnabled()
		{
			if (!EnhancedTouchSupport.enabled)
			{
				throw new InvalidOperationException("EnhancedTouch API is not enabled; call EnhancedTouchSupport.Enable()");
			}
		}

		// Token: 0x0400047A RID: 1146
		private static int s_Enabled;

		// Token: 0x0400047B RID: 1147
		private static InputSettings.UpdateMode s_UpdateMode;
	}
}
