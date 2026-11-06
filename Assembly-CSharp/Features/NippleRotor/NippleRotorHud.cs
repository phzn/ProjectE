using System;
using Features.CielOS;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.NippleRotor
{
	// Token: 0x02000F5C RID: 3932
	[DisallowMultipleComponent]
	public sealed class NippleRotorHud : MonoBehaviour
	{
		// Token: 0x0600766E RID: 30318 RVA: 0x00301CDC File Offset: 0x002FFEDC
		private void Awake()
		{
			this._controller = base.GetComponent<NippleRotorController>();
		}

		// Token: 0x0600766F RID: 30319 RVA: 0x00301CEC File Offset: 0x002FFEEC
		private void LateUpdate()
		{
			if (this._controller == null)
			{
				this._controller = base.GetComponent<NippleRotorController>();
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			bool flag = instance != null && this.CanOffer();
			if (this._hud != null && (this._hud != instance || !flag))
			{
				this._hud.HideSupplemental(this);
				this._hud = null;
				this._shownLevel = -1;
			}
			if (!flag)
			{
				return;
			}
			this.EnsureConfig();
			int num = NippleRotorIds.ClampLevel(this._controller.Level);
			if (num != this._shownLevel || this._hud != instance)
			{
				this._config.usableActions[0].actionName = NippleRotorIds.LevelActionNames[num];
				this._hud = instance;
				this._shownLevel = num;
				this._hud.ShowSupplemental(this, this._config);
			}
			if (!this._hud.IsShowing || ControllerInput.UiInputConsumedThisFrame || !this._hud.IsActionVisible(this._config, "L"))
			{
				return;
			}
			bool isActive = ControllerInput.IsActive;
			if (!NippleRotorHud.Pressed(isActive))
			{
				return;
			}
			this._controller.CycleLevel();
			if (isActive)
			{
				ControllerInput.ConsumeUIInput();
			}
		}

		// Token: 0x06007670 RID: 30320 RVA: 0x00301E24 File Offset: 0x00300024
		private bool CanOffer()
		{
			return !(this._controller == null) && this._controller.isActiveAndEnabled && this._controller.IsWorn && !this._controller.IsSuspended && (!TextInputFocus.BlocksGameplayInput && !GameplayInputGate.BlocksGameplayInput && !CielOSController.AnyOpen && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI)) && Time.timeScale > 0f;
		}

		// Token: 0x06007671 RID: 30321 RVA: 0x00301E94 File Offset: 0x00300094
		private void EnsureConfig()
		{
			if (this._config != null)
			{
				return;
			}
			this._config = ScriptableObject.CreateInstance<InteractionConfigSO>();
			this._config.name = "Nipple rotor actions";
			this._config.hideFlags = HideFlags.HideAndDontSave;
			this._config.usableActions.Add(new UsableAction
			{
				keyDisplayLabel = "L",
				actionName = NippleRotorIds.LevelActionNames[0]
			});
			ControllerContextBindings.SetLabel(this._config, "L", "LT + R3", false);
		}

		// Token: 0x06007672 RID: 30322 RVA: 0x00301F1B File Offset: 0x0030011B
		private static bool Pressed(bool controller)
		{
			if (!controller)
			{
				return !GameplayInputGate.BlocksGameplayInput && Input.GetKeyDown(KeyCode.L);
			}
			return ControllerGameplayBridge.AllowsAxes && ControllerInput.Modifier && ControllerInput.GetButtonDown(ControllerButton.RightStick);
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x00301F48 File Offset: 0x00300148
		private void OnDisable()
		{
			if (this._hud != null)
			{
				this._hud.HideSupplemental(this);
			}
			this._hud = null;
			this._shownLevel = -1;
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x00301F72 File Offset: 0x00300172
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

		// Token: 0x04007999 RID: 31129
		private NippleRotorController _controller;

		// Token: 0x0400799A RID: 31130
		private InteractionConfigSO _config;

		// Token: 0x0400799B RID: 31131
		private InteractionHUDController _hud;

		// Token: 0x0400799C RID: 31132
		private int _shownLevel = -1;
	}
}
