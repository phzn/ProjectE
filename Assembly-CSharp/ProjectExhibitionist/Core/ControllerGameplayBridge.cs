using System;
using Features.Gym;
using Features.PhoneCompanion;
using ProjectExhibitionist.Minimap;
using ProjectExhibitionist.PhotoBooth;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A48 RID: 2632
	public static class ControllerGameplayBridge
	{
		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06004AE2 RID: 19170 RVA: 0x001D2B74 File Offset: 0x001D0D74
		public static bool UIHasFocus
		{
			get
			{
				return ControllerGameplayBridge.UIFocusQuery != null && ControllerGameplayBridge.UIFocusQuery();
			}
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x001D2B8C File Offset: 0x001D0D8C
		public static bool Allows(KeyCode key)
		{
			ControllerButton controllerButton;
			bool flag;
			return !GameplayInputGate.BlocksGameplayInput && !ControllerInput.UiInputConsumedThisFrame && (key == KeyCode.Escape || ((!PhotoBoothController.AnySessionActive || key == KeyCode.W || key == KeyCode.A || key == KeyCode.S || key == KeyCode.D) && (key != KeyCode.F || !PhotoBoothPrintHolder.ControllerOwnsPhotoPan) && (!GymExerciseStation.ControllerOwnsShoulders || !ControllerBindings.TryButton(key, out controllerButton, out flag) || (controllerButton != ControllerButton.LeftShoulder && controllerButton != ControllerButton.RightShoulder)) && !ControllerWorldPointer.IsWorldPointerActive && ((key == KeyCode.P && SmartphoneController.IsPhonePointerActive) || (key == KeyCode.M && ProceduralMiniMapController.IsMapExpanded) || (!ControllerGameplayBridge.UIHasFocus && !TextInputFocus.HasFocusedField))));
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06004AE4 RID: 19172 RVA: 0x001D2C2E File Offset: 0x001D0E2E
		public static bool AllowsAxes
		{
			get
			{
				return !GameplayInputGate.BlocksGameplayInput && !ControllerInput.UiInputConsumedThisFrame && !ControllerWorldPointer.IsWorldPointerActive && !ControllerGameplayBridge.UIHasFocus && !TextInputFocus.HasFocusedField;
			}
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x001D2C58 File Offset: 0x001D0E58
		public static bool TryKey(string binding, out KeyCode key)
		{
			if (!string.IsNullOrEmpty(binding) && binding.Length == 1 && binding[0] >= '1' && binding[0] <= '9')
			{
				key = KeyCode.Alpha1 + (int)(binding[0] - '1');
				return true;
			}
			return Enum.TryParse<KeyCode>(binding, true, out key);
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x001D2CA5 File Offset: 0x001D0EA5
		public static bool Read(KeyCode key, int phase)
		{
			if (!ControllerGameplayBridge.Allows(key))
			{
				return false;
			}
			if (phase == 0)
			{
				return ControllerInput.GetKey(key);
			}
			if (phase != 1)
			{
				return ControllerInput.GetKeyUp(key);
			}
			return ControllerInput.GetKeyDown(key);
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x001D2CCC File Offset: 0x001D0ECC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			ControllerGameplayBridge.UIFocusQuery = null;
		}

		// Token: 0x04004CD7 RID: 19671
		public static Func<bool> UIFocusQuery;
	}
}
