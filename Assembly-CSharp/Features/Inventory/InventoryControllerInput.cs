using System;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010F4 RID: 4340
	public static class InventoryControllerInput
	{
		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x06008350 RID: 33616 RVA: 0x00358D7A File Offset: 0x00356F7A
		public static bool CanRead
		{
			get
			{
				return !GameplayInputGate.BlocksGameplayInput && !GraphicsSettingsRuntimeUI.AnyOpen && !TextInputFocus.BlocksGameplayInput && Time.timeScale > 0f && ControllerGameplayBridge.AllowsAxes;
			}
		}

		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x06008351 RID: 33617 RVA: 0x00358DA4 File Offset: 0x00356FA4
		public static bool PlacementHeld
		{
			get
			{
				return InventoryControllerInput.CanRead && (Input.GetMouseButton(1) || (!ControllerInput.Modifier && ControllerInput.GetButton(ControllerButton.LeftShoulder)));
			}
		}

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x06008352 RID: 33618 RVA: 0x00358DC8 File Offset: 0x00356FC8
		public static bool PlacementDown
		{
			get
			{
				return InventoryControllerInput.CanRead && (Input.GetMouseButtonDown(1) || (!ControllerInput.Modifier && ControllerInput.GetButtonDown(ControllerButton.LeftShoulder)));
			}
		}

		// Token: 0x06008353 RID: 33619 RVA: 0x00358DEC File Offset: 0x00356FEC
		public static bool TryConfirmPlacement()
		{
			if (!InventoryControllerInput.CanRead)
			{
				return false;
			}
			bool flag = !ControllerInput.Modifier && ControllerInput.UseItemDown;
			if (!flag && !Input.GetMouseButtonDown(0))
			{
				return false;
			}
			if (flag)
			{
				ControllerInput.ConsumeUIInput();
			}
			return true;
		}

		// Token: 0x06008354 RID: 33620 RVA: 0x00358E28 File Offset: 0x00357028
		public static bool TryCancelPlacement(Object owner)
		{
			if (!InventoryControllerInput.CanRead || !EscapeInputCoordinator.TryConsume(owner))
			{
				return false;
			}
			if (ControllerInput.IsActive)
			{
				ControllerInput.ConsumeUIInput();
			}
			return true;
		}

		// Token: 0x06008355 RID: 33621 RVA: 0x00358E48 File Offset: 0x00357048
		public static bool SpeedModifierHeld(GenericInput keyboardModifier = null)
		{
			if (!InventoryControllerInput.CanRead)
			{
				return false;
			}
			if (ControllerInput.IsActive)
			{
				return ControllerInput.Modifier;
			}
			if (keyboardModifier == null)
			{
				return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			}
			return keyboardModifier.GetButton();
		}

		// Token: 0x06008356 RID: 33622 RVA: 0x00358E84 File Offset: 0x00357084
		public static int SpeedStep(bool modifier, ref int direction, ref float nextRepeat)
		{
			if (!InventoryControllerInput.CanRead || !modifier)
			{
				direction = 0;
				return 0;
			}
			if (ControllerInput.IsActive)
			{
				return InventoryControllerInput.RepeatAxis(ControllerInput.Look.y, Time.unscaledTime, ref direction, ref nextRepeat);
			}
			direction = 0;
			float axisRaw = Input.GetAxisRaw("Mouse ScrollWheel");
			if (Mathf.Approximately(axisRaw, 0f))
			{
				return 0;
			}
			if (axisRaw <= 0f)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06008357 RID: 33623 RVA: 0x00358EE8 File Offset: 0x003570E8
		public static int RepeatAxis(float axis, float now, ref int direction, ref float nextRepeat)
		{
			int num = ((Mathf.Abs(axis) < 0.45f) ? 0 : ((axis > 0f) ? 1 : (-1)));
			if (num == 0)
			{
				direction = 0;
				return 0;
			}
			bool flag = direction != num;
			if (!flag && now < nextRepeat)
			{
				return 0;
			}
			direction = num;
			nextRepeat = now + (flag ? 0.3f : 0.12f);
			return num;
		}
	}
}
