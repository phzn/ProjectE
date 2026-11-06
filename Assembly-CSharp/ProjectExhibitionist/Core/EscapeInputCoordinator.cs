using System;
using Features.DebugTools;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A51 RID: 2641
	public static class EscapeInputCoordinator
	{
		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06004B30 RID: 19248 RVA: 0x001D4611 File Offset: 0x001D2811
		public static bool ConsumedThisFrame
		{
			get
			{
				return EscapeInputCoordinator._consumedFrame == Time.frameCount;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06004B31 RID: 19249 RVA: 0x001D461F File Offset: 0x001D281F
		public static Object ConsumerThisFrame
		{
			get
			{
				if (!EscapeInputCoordinator.ConsumedThisFrame)
				{
					return null;
				}
				return EscapeInputCoordinator._consumer;
			}
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x001D462F File Offset: 0x001D282F
		public static void Consume(Object consumer)
		{
			EscapeInputCoordinator._consumedFrame = Time.frameCount;
			EscapeInputCoordinator._consumer = consumer;
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x001D4641 File Offset: 0x001D2841
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntimeState()
		{
			EscapeInputCoordinator._consumedFrame = -1;
			EscapeInputCoordinator._consumer = null;
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x001D464F File Offset: 0x001D284F
		public static bool TryConsume(Object consumer)
		{
			return EscapeInputCoordinator.TryConsume(KeyCode.Escape, consumer, false);
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x001D465A File Offset: 0x001D285A
		public static bool TryConsume(KeyCode key, Object consumer)
		{
			return EscapeInputCoordinator.TryConsume(key, consumer, false);
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x001D4664 File Offset: 0x001D2864
		public static bool TryConsumeForSettings(KeyCode key, Object consumer)
		{
			return EscapeInputCoordinator.TryConsume(key, consumer, true);
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x001D4670 File Offset: 0x001D2870
		public static bool TryConsumeForPhotoMode(Object consumer)
		{
			if (EscapeInputCoordinator.ConsumedThisFrame || TextInputFocus.HasFocusedField || GameplayInputGate.IsLoading || GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return false;
			}
			bool flag = Input.GetKeyDown(KeyCode.Escape) | (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame);
			bool flag2 = ControllerInput.IsActive && !ControllerInput.UiInputConsumedThisFrame && !ControllerWorldPointer.WantsUIFocus && (ControllerInput.GetButtonDown(ControllerButton.East) || ControllerInput.GetButtonDown(ControllerButton.Start));
			if (!flag && !flag2)
			{
				return false;
			}
			EscapeInputCoordinator.Consume(consumer);
			if (flag2)
			{
				ControllerInput.ConsumeUIInput();
			}
			return true;
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x001D4700 File Offset: 0x001D2900
		private static bool TryConsume(KeyCode key, Object consumer, bool settingsOwner)
		{
			if (TextInputFocus.HasFocusedField || GameplayInputGate.IsLoading)
			{
				return false;
			}
			bool flag = Input.GetKeyDown(key);
			if (key == KeyCode.Escape)
			{
				flag |= Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
			}
			bool flag2 = !ControllerInput.UiInputConsumedThisFrame && ((settingsOwner && !GraphicsSettingsRuntimeUI.AnyOpen && key == KeyCode.Escape) ? ControllerInput.GetButtonDown(ControllerButton.Start) : ControllerInput.GetKeyDown(key));
			if (!flag && !flag2)
			{
				return false;
			}
			if (key != KeyCode.Escape)
			{
				return true;
			}
			if (EscapeInputCoordinator._consumedFrame == Time.frameCount)
			{
				return false;
			}
			if (!GraphicsSettingsRuntimeUI.AnyOpen && (FlyCamera.IsActive || FlyCameraPhotoMode.AnyOpen))
			{
				return false;
			}
			if (!settingsOwner && GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return false;
			}
			EscapeInputCoordinator.Consume(consumer);
			return true;
		}

		// Token: 0x04004D17 RID: 19735
		private static int _consumedFrame = -1;

		// Token: 0x04004D18 RID: 19736
		private static Object _consumer;
	}
}
