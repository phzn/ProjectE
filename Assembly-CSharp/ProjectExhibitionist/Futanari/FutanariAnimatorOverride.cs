using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist.Futanari
{
	// Token: 0x020009DA RID: 2522
	[DisallowMultipleComponent]
	[RequireComponent(typeof(FutanariModeController))]
	public class FutanariAnimatorOverride : MonoBehaviour
	{
		// Token: 0x06004731 RID: 18225 RVA: 0x001AEAB4 File Offset: 0x001ACCB4
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInChildren<Animator>();
			}
			if (this.futanariMode == null)
			{
				this.futanariMode = base.GetComponent<FutanariModeController>();
			}
			if (this.animator == null)
			{
				if (this.debugLog)
				{
					Debug.LogWarning("[FutanariAnimatorOverride] No Animator found, skipping.");
				}
				return;
			}
			if (this.futanariMode != null && this.futanariMode.FutanariMode)
			{
				this.ApplyOverride();
			}
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x001AEB38 File Offset: 0x001ACD38
		private void OnEnable()
		{
			this.Unsubscribe();
			if (!this.respondToRuntimeToggle)
			{
				return;
			}
			if (this.futanariMode == null)
			{
				this.futanariMode = base.GetComponent<FutanariModeController>();
			}
			if (this.futanariMode == null)
			{
				return;
			}
			this._subscribedMode = this.futanariMode;
			UnityEvent<bool> onFutanariModeChanged = this._subscribedMode.OnFutanariModeChanged;
			if (onFutanariModeChanged != null)
			{
				onFutanariModeChanged.AddListener(new UnityAction<bool>(this.HandleModeChanged));
			}
			this.HandleModeChanged(this.futanariMode.FutanariMode);
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x001AEBBC File Offset: 0x001ACDBC
		private void OnDisable()
		{
			this.Unsubscribe();
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x001AEBC4 File Offset: 0x001ACDC4
		private void OnDestroy()
		{
			this.Unsubscribe();
			this.RevertOverride();
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x001AEBD2 File Offset: 0x001ACDD2
		private void Unsubscribe()
		{
			if (this._subscribedMode != null)
			{
				UnityEvent<bool> onFutanariModeChanged = this._subscribedMode.OnFutanariModeChanged;
				if (onFutanariModeChanged != null)
				{
					onFutanariModeChanged.RemoveListener(new UnityAction<bool>(this.HandleModeChanged));
				}
			}
			this._subscribedMode = null;
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x001AEC0B File Offset: 0x001ACE0B
		private void HandleModeChanged(bool active)
		{
			if (active)
			{
				this.ApplyOverride();
				return;
			}
			this.RevertOverride();
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x001AEC20 File Offset: 0x001ACE20
		private void ApplyOverride()
		{
			if (this._appliedController != null)
			{
				if (this._ownedAnimator == this.animator && this._appliedController == this.futaOverrideController && this.OwnsCurrentOverride())
				{
					return;
				}
				this.RevertOverride();
			}
			if (this.animator == null || this.futaOverrideController == null)
			{
				return;
			}
			RuntimeAnimatorController runtimeAnimatorController = this.animator.runtimeAnimatorController;
			if (runtimeAnimatorController == this.futaOverrideController)
			{
				return;
			}
			RuntimeAnimatorController runtimeAnimatorController2 = FutanariAnimatorOverride.BaseController(this.futaOverrideController);
			if (runtimeAnimatorController2 == null || FutanariAnimatorOverride.BaseController(runtimeAnimatorController) != runtimeAnimatorController2)
			{
				if (!this._warnedMismatch)
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"[FutanariAnimatorOverride] Override ignorato: base '",
						(runtimeAnimatorController2 != null) ? runtimeAnimatorController2.name : null,
						"' incompatibile con il controller attuale '",
						(runtimeAnimatorController != null) ? runtimeAnimatorController.name : null,
						"'. Il controller attuale resta invariato."
					}), this);
				}
				this._warnedMismatch = true;
				return;
			}
			this.animator.runtimeAnimatorController = this.futaOverrideController;
			this._originalController = runtimeAnimatorController;
			this._ownedAnimator = this.animator;
			this._appliedController = this.futaOverrideController;
			this._warnedMismatch = false;
			if (this.debugLog)
			{
				Debug.Log("[FutanariAnimatorOverride] Override applicato: '" + this.futaOverrideController.name + "'.");
			}
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x001AED80 File Offset: 0x001ACF80
		private void RevertOverride()
		{
			if (this.OwnsCurrentOverride() && this._originalController != null)
			{
				this._ownedAnimator.runtimeAnimatorController = this._originalController;
				if (this.debugLog)
				{
					Debug.Log("[FutanariAnimatorOverride] Override rimosso, ripristinato '" + this._originalController.name + "'.");
				}
			}
			this._originalController = null;
			this._ownedAnimator = null;
			this._appliedController = null;
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x001AEDF0 File Offset: 0x001ACFF0
		private bool OwnsCurrentOverride()
		{
			return this._ownedAnimator != null && this._appliedController != null && this._ownedAnimator.runtimeAnimatorController == this._appliedController;
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x001AEE28 File Offset: 0x001AD028
		private static RuntimeAnimatorController BaseController(RuntimeAnimatorController controller)
		{
			HashSet<RuntimeAnimatorController> hashSet = new HashSet<RuntimeAnimatorController>();
			for (;;)
			{
				AnimatorOverrideController animatorOverrideController = controller as AnimatorOverrideController;
				if (animatorOverrideController == null)
				{
					return controller;
				}
				if (!hashSet.Add(controller))
				{
					break;
				}
				controller = animatorOverrideController.runtimeAnimatorController;
			}
			return null;
		}

		// Token: 0x040046C4 RID: 18116
		[Header("References")]
		[Tooltip("Animator da pilotare. Se null, viene preso con GetComponentInChildren<Animator>() in Awake.")]
		[SerializeField]
		private Animator animator;

		// Token: 0x040046C5 RID: 18117
		[Tooltip("FutanariModeController da osservare. Se null, viene preso dal GameObject corrente.")]
		[SerializeField]
		private FutanariModeController futanariMode;

		// Token: 0x040046C6 RID: 18118
		[Header("Override")]
		[Tooltip("AnimatorOverrideController con CielAnimatorController come base. Riempi solo gli slot delle clip che vuoi sostituire in Futa Mode.")]
		[SerializeField]
		private AnimatorOverrideController futaOverrideController;

		// Token: 0x040046C7 RID: 18119
		[Header("Behavior")]
		[Tooltip("Riapplica l'override quando FutanariModeController cambia stato a runtime. Deve restare attivo per i preset caricati dopo Awake e per la character customization.")]
		[SerializeField]
		private bool respondToRuntimeToggle = true;

		// Token: 0x040046C8 RID: 18120
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x040046C9 RID: 18121
		private RuntimeAnimatorController _originalController;

		// Token: 0x040046CA RID: 18122
		private Animator _ownedAnimator;

		// Token: 0x040046CB RID: 18123
		private AnimatorOverrideController _appliedController;

		// Token: 0x040046CC RID: 18124
		private FutanariModeController _subscribedMode;

		// Token: 0x040046CD RID: 18125
		private bool _warnedMismatch;
	}
}
