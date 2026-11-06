using System;
using Features.CielOS;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x02001230 RID: 4656
	[DisallowMultipleComponent]
	public sealed class EggRotorHud : MonoBehaviour
	{
		// Token: 0x06009091 RID: 37009 RVA: 0x003B3371 File Offset: 0x003B1571
		private void Awake()
		{
			this._controller = base.GetComponent<EggRotorController>();
		}

		// Token: 0x06009092 RID: 37010 RVA: 0x003B3380 File Offset: 0x003B1580
		private void LateUpdate()
		{
			if (this._controller == null)
			{
				this._controller = base.GetComponent<EggRotorController>();
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			bool flag = instance != null && this.CanOffer();
			if (this._hud != null && (this._hud != instance || !flag))
			{
				this._hud.HideSupplemental(this);
				this._hud = null;
				this._shownLevel = (this._shownPlacement = -1);
			}
			if (!flag)
			{
				this._press = default(EggRotorPressState);
				return;
			}
			this.EnsureConfig();
			bool flag2 = !this._controller.IsNippleRotorWorn;
			this.ApplyControllerLabels(flag2);
			int num = EggRotorIds.ClampLevel(this._controller.Level);
			int num2 = (int)EggRotorIds.ClampPlacement((int)this._controller.Placement);
			if (num != this._shownLevel || num2 != this._shownPlacement || this._hud != instance)
			{
				this._config.usableActions[0].actionName = EggRotorIds.LevelActionNames[num];
				this._config.usableActions[1].actionName = EggRotorIds.PlacementActionNames[num2];
				this._hud = instance;
				this._shownLevel = num;
				this._shownPlacement = num2;
				this._hud.ShowSupplemental(this, this._config);
			}
			bool isActive = ControllerInput.IsActive;
			if (this._press.Active && isActive != this._pressFromController)
			{
				this._press = default(EggRotorPressState);
			}
			bool flag3 = this._hud.IsShowing && this._hud.IsActionVisible(this._config, "Y") && this._hud.IsActionVisible(this._config, "Hold Y") && EggRotorHud.InputAllowed(isActive, flag2);
			if (flag3 && isActive && this._press.Active && ControllerInput.GetButton(ControllerButton.RightStick) && !ControllerInput.Modifier)
			{
				flag3 = false;
			}
			bool flag4 = flag3 && !ControllerInput.UiInputConsumedThisFrame && EggRotorHud.PressedDown(isActive);
			if (flag4)
			{
				this._pressFromController = isActive;
			}
			EggRotorPressResult eggRotorPressResult = EggRotorPressClassifier.Step(ref this._press, flag4, EggRotorHud.Held(isActive), flag3, Time.unscaledTime, 0.45f);
			if (flag4 && isActive)
			{
				ControllerInput.ConsumeUIInput();
			}
			if (eggRotorPressResult == EggRotorPressResult.Tap)
			{
				this._controller.CycleLevel();
				return;
			}
			if (eggRotorPressResult == EggRotorPressResult.Hold)
			{
				this._controller.CyclePlacement();
			}
		}

		// Token: 0x06009093 RID: 37011 RVA: 0x003B35E4 File Offset: 0x003B17E4
		private bool CanOffer()
		{
			return !(this._controller == null) && this._controller.isActiveAndEnabled && this._controller.IsWorn && !this._controller.IsSuspended && (!TextInputFocus.BlocksGameplayInput && !GameplayInputGate.BlocksGameplayInput && !CielOSController.AnyOpen && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI)) && Time.timeScale > 0f;
		}

		// Token: 0x06009094 RID: 37012 RVA: 0x003B3654 File Offset: 0x003B1854
		private static bool InputAllowed(bool controller, bool padAllowed)
		{
			if (!controller)
			{
				return !GameplayInputGate.BlocksGameplayInput;
			}
			return padAllowed && ControllerGameplayBridge.AllowsAxes;
		}

		// Token: 0x06009095 RID: 37013 RVA: 0x003B366C File Offset: 0x003B186C
		private static bool PressedDown(bool controller)
		{
			if (!controller)
			{
				return Input.GetKeyDown(KeyCode.Y);
			}
			return ControllerInput.Modifier && ControllerInput.GetButtonDown(ControllerButton.RightStick);
		}

		// Token: 0x06009096 RID: 37014 RVA: 0x003B3689 File Offset: 0x003B1889
		private static bool Held(bool controller)
		{
			if (!controller)
			{
				return Input.GetKey(KeyCode.Y);
			}
			return ControllerInput.GetButton(ControllerButton.RightStick);
		}

		// Token: 0x06009097 RID: 37015 RVA: 0x003B36A0 File Offset: 0x003B18A0
		private void EnsureConfig()
		{
			if (this._config != null)
			{
				return;
			}
			this._config = ScriptableObject.CreateInstance<InteractionConfigSO>();
			this._config.name = "Egg rotor actions";
			this._config.hideFlags = HideFlags.HideAndDontSave;
			this._config.usableActions.Add(new UsableAction
			{
				keyDisplayLabel = "Y",
				actionName = EggRotorIds.LevelActionNames[0]
			});
			this._config.usableActions.Add(new UsableAction
			{
				keyDisplayLabel = "Hold Y",
				actionName = EggRotorIds.PlacementActionNames[1]
			});
			this._labelsForPad = -1;
		}

		// Token: 0x06009098 RID: 37016 RVA: 0x003B3748 File Offset: 0x003B1948
		private void ApplyControllerLabels(bool padAllowed)
		{
			int num = (padAllowed ? 1 : 0);
			if (this._labelsForPad == num)
			{
				return;
			}
			this._labelsForPad = num;
			ControllerContextBindings.SetLabel(this._config, "Y", padAllowed ? "LT + R3" : "-", false);
			ControllerContextBindings.SetLabel(this._config, "Hold Y", padAllowed ? "Hold LT + R3" : "-", false);
		}

		// Token: 0x06009099 RID: 37017 RVA: 0x003B37B0 File Offset: 0x003B19B0
		private void OnDisable()
		{
			if (this._hud != null)
			{
				this._hud.HideSupplemental(this);
			}
			this._hud = null;
			this._shownLevel = (this._shownPlacement = -1);
			this._press = default(EggRotorPressState);
		}

		// Token: 0x0600909A RID: 37018 RVA: 0x003B37FA File Offset: 0x003B19FA
		private void OnDestroy()
		{
			if (this._config == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._config);
				return;
			}
			Object.DestroyImmediate(this._config);
		}

		// Token: 0x040094A5 RID: 38053
		private const int LevelRow = 0;

		// Token: 0x040094A6 RID: 38054
		private const int PlacementRow = 1;

		// Token: 0x040094A7 RID: 38055
		private EggRotorController _controller;

		// Token: 0x040094A8 RID: 38056
		private InteractionConfigSO _config;

		// Token: 0x040094A9 RID: 38057
		private InteractionHUDController _hud;

		// Token: 0x040094AA RID: 38058
		private int _shownLevel = -1;

		// Token: 0x040094AB RID: 38059
		private int _shownPlacement = -1;

		// Token: 0x040094AC RID: 38060
		private int _labelsForPad = -1;

		// Token: 0x040094AD RID: 38061
		private EggRotorPressState _press;

		// Token: 0x040094AE RID: 38062
		private bool _pressFromController;
	}
}
