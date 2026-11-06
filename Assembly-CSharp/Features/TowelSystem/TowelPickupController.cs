using System;
using System.Collections;
using Features.DebugTools;
using Features.Detection;
using Features.Slip;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Features.TowelSystem
{
	// Token: 0x02000C06 RID: 3078
	public class TowelPickupController : MonoBehaviour
	{
		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060058F6 RID: 22774 RVA: 0x0022CB83 File Offset: 0x0022AD83
		// (set) Token: 0x060058F7 RID: 22775 RVA: 0x0022CB8B File Offset: 0x0022AD8B
		public bool CanPickUp { get; private set; }

		// Token: 0x060058F8 RID: 22776 RVA: 0x0022CB94 File Offset: 0x0022AD94
		public void SetPickupAction(Func<bool> onPickup)
		{
			this._onPickup = onPickup;
		}

		// Token: 0x060058F9 RID: 22777 RVA: 0x0022CB9D File Offset: 0x0022AD9D
		public void PickUpNow()
		{
			if (this._pickingUp || this.slipController == null)
			{
				return;
			}
			base.StartCoroutine(this.PickupRoutine());
		}

		// Token: 0x060058FA RID: 22778 RVA: 0x0022CBC4 File Offset: 0x0022ADC4
		private void Awake()
		{
			if (this.slipController == null)
			{
				this.slipController = base.GetComponent<ClothSlipController>();
			}
			if (this.slipController == null)
			{
				this.slipController = base.GetComponentInParent<ClothSlipController>();
			}
			Animator componentInParent = base.GetComponentInParent<Animator>();
			this._player = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			if (this.playerActionAnimator == null && this._player != null)
			{
				this.playerActionAnimator = this._player.GetComponentInChildren<PlayerActionAnimator>(true);
			}
			if (this.playerActionAnimator == null)
			{
				Debug.LogWarning("[TowelPickupController] PlayerActionAnimator not found — pickup will restore the towel without animation.", this);
			}
		}

		// Token: 0x060058FB RID: 22779 RVA: 0x0022CC75 File Offset: 0x0022AE75
		private void Update()
		{
			this.CanPickUp = this.EvaluateCanPickUp();
			if (this.CanPickUp && !FlyCamera.IsActive && this.pickupInput != null && this.pickupInput.GetButtonDown())
			{
				base.StartCoroutine(this.PickupRoutine());
			}
		}

		// Token: 0x060058FC RID: 22780 RVA: 0x0022CCB4 File Offset: 0x0022AEB4
		private bool EvaluateCanPickUp()
		{
			if (this._pickingUp || this.slipController == null || !this.slipController.HasSlippedPiece(this.wornPieceName))
			{
				return false;
			}
			GameObject gameObject = this.slipController.EnvInstanceFor(this.wornPieceName);
			return !(gameObject == null) && !(this._player == null) && Vector3.Distance(this._player.position, gameObject.transform.position) <= this.pickupRange;
		}

		// Token: 0x060058FD RID: 22781 RVA: 0x0022CD3D File Offset: 0x0022AF3D
		private bool DoPickup()
		{
			if (this._onPickup != null)
			{
				return this._onPickup();
			}
			return this.slipController.RestoreWorn(this.wornPieceName);
		}

		// Token: 0x060058FE RID: 22782 RVA: 0x0022CD64 File Offset: 0x0022AF64
		private IEnumerator PickupRoutine()
		{
			this._pickingUp = true;
			this.CanPickUp = false;
			float duration = 0f;
			if (this.playerActionAnimator != null && this.pickupAction != null && this.pickupAction.TriggerPrefab != null)
			{
				duration = this.playerActionAnimator.TriggerAction(this.pickupAction);
			}
			float restoreDelay = duration * Mathf.Clamp01(this.restoreAtNormalizedTime);
			if (restoreDelay > 0f)
			{
				yield return new WaitForSeconds(restoreDelay);
			}
			if (this.DoPickup())
			{
				UnityEvent onTowelPickedUp = this.OnTowelPickedUp;
				if (onTowelPickedUp != null)
				{
					onTowelPickedUp.Invoke();
				}
			}
			float emotionRestoreAt = duration * Mathf.Clamp01(this.emotionRestoreNormalizedTime);
			float num = emotionRestoreAt - restoreDelay;
			if (num > 0f)
			{
				yield return new WaitForSeconds(num);
			}
			if (PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.RestoreEmotionLayers();
				PlayerEmotionController.Instance.ForceLayerWeightUpdate();
			}
			float num2 = duration - Mathf.Max(emotionRestoreAt, restoreDelay);
			if (num2 > 0f)
			{
				yield return new WaitForSeconds(num2);
			}
			this._pickingUp = false;
			yield break;
		}

		// Token: 0x04005984 RID: 22916
		[Header("Refs")]
		[Tooltip("Generic cloth slip controller (the same used by the swimsuit). Hosts the towel SlipPiece.")]
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04005985 RID: 22917
		[Tooltip("Worn-mesh name della piece towel nel ClothSlipController.")]
		[SerializeField]
		private string wornPieceName = "Bath_Towel";

		// Token: 0x04005986 RID: 22918
		[Tooltip("Same PlayerActionAnimator used by HideFlash/Dildo pickups. Auto-found on the player if not assigned.")]
		[SerializeField]
		private PlayerActionAnimator playerActionAnimator;

		// Token: 0x04005987 RID: 22919
		[Header("Action")]
		[Tooltip("Action entry configured by Tools/Towel/Setup Towel System (plays the PickUpFloor animator state).")]
		[SerializeField]
		private PlayerActionEntry pickupAction;

		// Token: 0x04005988 RID: 22920
		[Tooltip("Fraction of the pickup animation after which the towel snaps back onto the body.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float restoreAtNormalizedTime = 0.55f;

		// Token: 0x04005989 RID: 22921
		[Tooltip("Fraction of the pickup animation at which the emotion layers (Confident/Embarrassed) are restored. The pickup state exits on its own at ~76% with a 0.25s crossfade: restoring during that blend hides the layer-weight snap.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float emotionRestoreNormalizedTime = 0.8f;

		// Token: 0x0400598A RID: 22922
		[Header("Input")]
		[SerializeField]
		private GenericInput pickupInput = new GenericInput("E", "A", "A");

		// Token: 0x0400598B RID: 22923
		[Tooltip("Max distance from the fallen towel to allow picking it up.")]
		[SerializeField]
		private float pickupRange = 1.8f;

		// Token: 0x0400598C RID: 22924
		[Header("Events")]
		public UnityEvent OnTowelPickedUp;

		// Token: 0x0400598D RID: 22925
		private Transform _player;

		// Token: 0x0400598E RID: 22926
		private bool _pickingUp;

		// Token: 0x0400598F RID: 22927
		private Func<bool> _onPickup;
	}
}
