using System;
using Features.Inventory;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B6F RID: 2927
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-100)]
	[RequireComponent(typeof(WantedPosterBreakController))]
	public class WantedPosterHeldActions : MonoBehaviour
	{
		// Token: 0x0600532E RID: 21294 RVA: 0x00204824 File Offset: 0x00202A24
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.heldConfig, this.destroyKey.ToString(), "LT + Y", false);
			ControllerContextBindings.SetLabel(this.heldConfig, this.faceToggleKey.ToString(), "LT + A", false);
			ControllerContextBindings.SetLabel(this.heldConfig, "RMB", "Hold LB", false);
			this._controller = base.GetComponent<WantedPosterBreakController>();
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x00204897 File Offset: 0x00202A97
		private void OnDisable()
		{
			if (this._shown != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._shown);
			}
			this._shown = null;
			this.ResetShowFace();
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x002048D2 File Offset: 0x00202AD2
		private bool IsPosterActive()
		{
			return this.CanPresentActions() && this.IsPosterEquipped();
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x002048E4 File Offset: 0x00202AE4
		private bool IsPosterEquipped()
		{
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			if (instance == null)
			{
				return false;
			}
			InventoryItemSO activeItem = instance.GetActiveItem();
			return this.posterItem != null && activeItem == this.posterItem;
		}

		// Token: 0x06005332 RID: 21298 RVA: 0x00204928 File Offset: 0x00202B28
		private bool CanPresentActions()
		{
			if (this._controller == null)
			{
				this._controller = base.GetComponent<WantedPosterBreakController>();
			}
			if (this._controller == null || !this._controller.IsActiveInstance)
			{
				return false;
			}
			if (!GameSystemsBootstrapper.IsRunReady(false))
			{
				return false;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			return (!(instance != null) || !instance.IsTransitioning) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) && PlayerActivityGate.Allows(PlayerActivity.Interact | PlayerActivity.Inventory);
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x002049A4 File Offset: 0x00202BA4
		private void Update()
		{
			if (!this.IsPosterActive())
			{
				return;
			}
			bool isActive = ControllerInput.IsActive;
			if (isActive ? WantedPosterHeldActions.TryConsumeControllerChord(ControllerButton.North) : GameplayInput.GetKeyDown(this.destroyKey))
			{
				if (WantedPosterBreakController.Instance != null)
				{
					WantedPosterBreakController.Instance.TriggerDiscard();
				}
				return;
			}
			if (isActive ? WantedPosterHeldActions.TryConsumeControllerChord(ControllerButton.South) : GameplayInput.GetKeyDown(this.faceToggleKey))
			{
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				Animator animator = ((instance != null) ? instance.PlayerAnimator : null);
				if (animator != null && WantedPosterHeldActions.HasParam(animator, this.showFaceParam))
				{
					this._showingFace = !this._showingFace;
					animator.SetBool(this.showFaceParam, this._showingFace);
				}
			}
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x00204A5A File Offset: 0x00202C5A
		private static bool TryConsumeControllerChord(ControllerButton button)
		{
			if (!ControllerGameplayBridge.AllowsAxes || ControllerWorldPointer.BlocksContextInput || !ControllerInput.Modifier || !ControllerInput.GetButtonDown(button))
			{
				return false;
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x00204A84 File Offset: 0x00202C84
		private void LateUpdate()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			InteractionConfigSO interactionConfigSO = (this.IsPosterActive() ? this.heldConfig : null);
			if (instance == null)
			{
				if (interactionConfigSO == null)
				{
					this._shown = null;
					if (!this.IsPosterEquipped())
					{
						this.ResetShowFace();
					}
				}
				return;
			}
			if (interactionConfigSO == this._shown)
			{
				if (interactionConfigSO != null && instance.CurrentConfig == null)
				{
					instance.Show(interactionConfigSO);
				}
				return;
			}
			if (interactionConfigSO == null)
			{
				if (this._shown != null)
				{
					instance.HideIfCurrent(this._shown);
				}
				if (!this.IsPosterEquipped())
				{
					this.ResetShowFace();
				}
			}
			else
			{
				instance.Show(interactionConfigSO);
			}
			this._shown = interactionConfigSO;
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x00204B40 File Offset: 0x00202D40
		private static bool HasParam(Animator a, string name)
		{
			AnimatorControllerParameter[] parameters = a.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].name == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x00204B78 File Offset: 0x00202D78
		private void ResetShowFace()
		{
			if (!this._showingFace)
			{
				return;
			}
			PlayerInventoryManager instance = PlayerInventoryManager.Instance;
			Animator animator = ((instance != null) ? instance.PlayerAnimator : null);
			if (animator != null && WantedPosterHeldActions.HasParam(animator, this.showFaceParam))
			{
				animator.SetBool(this.showFaceParam, false);
			}
			this._showingFace = false;
		}

		// Token: 0x040053B0 RID: 21424
		[Header("Item")]
		[Tooltip("L'item del poster: la HUD/azioni sono attive solo se è questo l'item attivo.")]
		[SerializeField]
		private InventoryItemSO posterItem;

		// Token: 0x040053B1 RID: 21425
		[Header("HUD")]
		[Tooltip("Config con le azioni mostrate mentre il poster è in mano (X/G/Place).")]
		[SerializeField]
		private InteractionConfigSO heldConfig;

		// Token: 0x040053B2 RID: 21426
		[Header("Input")]
		[SerializeField]
		private KeyCode destroyKey = KeyCode.X;

		// Token: 0x040053B3 RID: 21427
		[SerializeField]
		private KeyCode faceToggleKey = KeyCode.G;

		// Token: 0x040053B4 RID: 21428
		[Header("Animator")]
		[Tooltip("Bool sull'animator di Ciel che alterna HoldPoster(HideFace) ⇄ ShowFace.")]
		[SerializeField]
		private string showFaceParam = "PosterShowFace";

		// Token: 0x040053B5 RID: 21429
		private InteractionConfigSO _shown;

		// Token: 0x040053B6 RID: 21430
		private bool _showingFace;

		// Token: 0x040053B7 RID: 21431
		private WantedPosterBreakController _controller;
	}
}
