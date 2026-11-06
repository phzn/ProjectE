using System;
using System.Collections.Generic;
using Features.Gym;
using Features.InsertableToys;
using Invector.vCharacterController;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A47 RID: 2631
	public static class ControllerContextBindings
	{
		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06004AD5 RID: 19157 RVA: 0x001D283E File Offset: 0x001D0A3E
		// (set) Token: 0x06004AD6 RID: 19158 RVA: 0x001D2845 File Offset: 0x001D0A45
		public static int LabelVersion { get; private set; }

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06004AD7 RID: 19159 RVA: 0x001D2850 File Offset: 0x001D0A50
		public static bool CoverPoseInputReserved
		{
			get
			{
				if (GymExerciseStation.ControllerOwnsShoulders)
				{
					return true;
				}
				InsertableToyController resourceOwner = PlayerActivityGate.GetResourceOwner<InsertableToyController>(PlayerContextResource.BodyAttachment);
				return resourceOwner != null && resourceOwner.ReservesControllerCoverInput;
			}
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x001D2882 File Offset: 0x001D0A82
		public static void MarkControllerLabels(InteractionConfigSO config, bool controller)
		{
			if (config == null)
			{
				return;
			}
			if (controller ? ControllerContextBindings.ControllerLabels.Add(config) : ControllerContextBindings.ControllerLabels.Remove(config))
			{
				ControllerContextBindings.LabelVersion++;
			}
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x001D28B8 File Offset: 0x001D0AB8
		public static void SetLabel(InteractionConfigSO config, string keyboard, string controller, bool worldPrompt = false)
		{
			if (config == null || string.IsNullOrEmpty(keyboard))
			{
				return;
			}
			keyboard = (worldPrompt ? "world:" : "hud:") + keyboard;
			Dictionary<string, string> dictionary;
			if (!ControllerContextBindings.Labels.TryGetValue(config, out dictionary))
			{
				ControllerContextBindings.Labels.Add(config, dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
			}
			string text;
			if (dictionary.TryGetValue(keyboard, out text) && text == controller)
			{
				return;
			}
			dictionary[keyboard] = controller;
			ControllerContextBindings.LabelVersion++;
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x001D293D File Offset: 0x001D0B3D
		public static string KeyLabel(InteractionConfigSO config, string source, bool translateUnmapped)
		{
			return ControllerContextBindings.ResolveLabel(config, source, translateUnmapped, ControllerInput.IsActive);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x001D294C File Offset: 0x001D0B4C
		public static void SetInputLabel(InteractionConfigSO config, string source, GenericInput input)
		{
			if (input == null)
			{
				return;
			}
			KeyCode keyCode;
			if (!input.preserveJoystickBinding && ControllerGameplayBridge.TryKey(input.keyboard, out keyCode) && ControllerBindings.HasKey(keyCode))
			{
				ControllerContextBindings.SetLabel(config, source, ControllerBindings.Label(keyCode), false);
				return;
			}
			ControllerButton controllerButton;
			if (ControllerBindings.TryLegacyButton(input.joystick, out controllerButton))
			{
				ControllerContextBindings.SetLabel(config, source, ControllerBindings.Label(controllerButton), false);
			}
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x001D29A8 File Offset: 0x001D0BA8
		public static void CopyLabels(InteractionConfigSO source, InteractionConfigSO target)
		{
			if (source == null || target == null)
			{
				return;
			}
			Dictionary<string, string> dictionary;
			if (ControllerContextBindings.Labels.TryGetValue(source, out dictionary))
			{
				ControllerContextBindings.Labels[target] = new Dictionary<string, string>(dictionary, StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				ControllerContextBindings.Labels.Remove(target);
			}
			if (ControllerContextBindings.ControllerLabels.Contains(source))
			{
				ControllerContextBindings.ControllerLabels.Add(target);
			}
			else
			{
				ControllerContextBindings.ControllerLabels.Remove(target);
			}
			ControllerContextBindings.LabelVersion++;
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x001D2A30 File Offset: 0x001D0C30
		public static string ResolveLabel(InteractionConfigSO config, string source, bool translateUnmapped, bool controllerActive)
		{
			if (!controllerActive || string.IsNullOrEmpty(source))
			{
				return source;
			}
			Dictionary<string, string> dictionary;
			string text;
			if (config != null && ControllerContextBindings.Labels.TryGetValue(config, out dictionary) && dictionary.TryGetValue((translateUnmapped ? "world:" : "hud:") + source, out text))
			{
				return text;
			}
			if (config != null && ControllerContextBindings.ControllerLabels.Contains(config))
			{
				return source;
			}
			if (source.Equals("LMB", StringComparison.OrdinalIgnoreCase))
			{
				return ControllerInput.ForKey(KeyCode.Mouse0);
			}
			KeyCode keyCode;
			if (ControllerGameplayBridge.TryKey(source, out keyCode) && ControllerBindings.HasKey(keyCode))
			{
				return ControllerInput.ForKey(keyCode);
			}
			return source;
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x001D2ACE File Offset: 0x001D0CCE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetLabels()
		{
			ControllerContextBindings.Labels.Clear();
			ControllerContextBindings.ControllerLabels.Clear();
			ControllerContextBindings.LabelVersion++;
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x001D2AF0 File Offset: 0x001D0CF0
		public static bool ExitPressed(GenericInput keyboardInput)
		{
			if (keyboardInput == null || !keyboardInput.useInput)
			{
				return false;
			}
			if (!ControllerInput.IsActive)
			{
				return keyboardInput.GetButtonDown();
			}
			if (!ControllerGameplayBridge.AllowsAxes || EscapeInputCoordinator.ConsumedThisFrame || !ControllerWorldPointer.Down(ControllerButton.East))
			{
				return false;
			}
			EscapeInputCoordinator.Consume(null);
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x001D2B3C File Offset: 0x001D0D3C
		public static void Set(GenericInput input, string binding)
		{
			if (input == null)
			{
				return;
			}
			input.joystick = binding;
			input.joystickAxis = false;
			input.joystickAxisInvert = false;
			input.preserveJoystickBinding = true;
		}

		// Token: 0x04004CD4 RID: 19668
		private static readonly Dictionary<InteractionConfigSO, Dictionary<string, string>> Labels = new Dictionary<InteractionConfigSO, Dictionary<string, string>>();

		// Token: 0x04004CD5 RID: 19669
		private static readonly HashSet<InteractionConfigSO> ControllerLabels = new HashSet<InteractionConfigSO>();
	}
}
