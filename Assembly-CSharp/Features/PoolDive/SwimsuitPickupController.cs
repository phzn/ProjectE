using System;
using System.Collections;
using Features.DebugTools;
using Features.Detection;
using Features.Slip;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.PoolDive
{
	// Token: 0x02000DE1 RID: 3553
	public class SwimsuitPickupController : MonoBehaviour
	{
		// Token: 0x06006945 RID: 26949 RVA: 0x002A2463 File Offset: 0x002A0663
		public void SetPickupAction(Func<bool> onPickup)
		{
			this._onPickup = onPickup;
		}

		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x06006946 RID: 26950 RVA: 0x002A246C File Offset: 0x002A066C
		// (set) Token: 0x06006947 RID: 26951 RVA: 0x002A2474 File Offset: 0x002A0674
		public bool CanPickUp { get; private set; }

		// Token: 0x06006948 RID: 26952 RVA: 0x002A2480 File Offset: 0x002A0680
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
				Debug.LogWarning("[SwimsuitPickupController] PlayerActionAnimator not found — pickup will restore the swimsuit without animation.", this);
			}
			if (this.swimming == null)
			{
				this.swimming = base.GetComponentInParent<vSwimming>();
			}
			if (this.swimming == null)
			{
				this.swimming = base.GetComponentInChildren<vSwimming>();
			}
		}

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x06006949 RID: 26953 RVA: 0x002A2565 File Offset: 0x002A0765
		private bool InWater
		{
			get
			{
				return this.swimming != null && this.swimming.inTheWater;
			}
		}

		// Token: 0x0600694A RID: 26954 RVA: 0x002A2584 File Offset: 0x002A0784
		private void Update()
		{
			this.CanPickUp = this.EvaluateCanPickUp();
			if (this.listenForOwnInput && this.CanPickUp && !FlyCamera.IsActive && this.pickupInput != null && this.pickupInput.GetButtonDown())
			{
				base.StartCoroutine(this.PickupRoutine());
			}
		}

		// Token: 0x0600694B RID: 26955 RVA: 0x002A25D6 File Offset: 0x002A07D6
		public void PickUpNow()
		{
			if (this._pickingUp || this.slipController == null)
			{
				return;
			}
			base.StartCoroutine(this.PickupRoutine());
		}

		// Token: 0x0600694C RID: 26956 RVA: 0x002A25FC File Offset: 0x002A07FC
		private bool DoPickup()
		{
			if (this._onPickup != null)
			{
				return this._onPickup();
			}
			return this.slipController.RestoreWorn("School_Swimsuit");
		}

		// Token: 0x0600694D RID: 26957 RVA: 0x002A2624 File Offset: 0x002A0824
		private bool EvaluateCanPickUp()
		{
			if (this._pickingUp || this.slipController == null || !this.slipController.HasSlippedPiece("School_Swimsuit"))
			{
				return false;
			}
			GameObject gameObject = this.slipController.EnvInstanceFor("School_Swimsuit");
			return !(gameObject == null) && !(this._player == null) && Vector3.Distance(this._player.position, gameObject.transform.position) <= this.pickupRange;
		}

		// Token: 0x0600694E RID: 26958 RVA: 0x002A26AB File Offset: 0x002A08AB
		private IEnumerator PickupRoutine()
		{
			this._pickingUp = true;
			this.CanPickUp = false;
			if (this.InWater)
			{
				if (this.DoPickup())
				{
					UnityEvent onSwimsuitPickedUp = this.OnSwimsuitPickedUp;
					if (onSwimsuitPickedUp != null)
					{
						onSwimsuitPickedUp.Invoke();
					}
				}
				this._pickingUp = false;
				yield break;
			}
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
				UnityEvent onSwimsuitPickedUp2 = this.OnSwimsuitPickedUp;
				if (onSwimsuitPickedUp2 != null)
				{
					onSwimsuitPickedUp2.Invoke();
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
			if (this.playerActionAnimator != null)
			{
				this.playerActionAnimator.FinishCurrentAction();
			}
			this._pickingUp = false;
			yield break;
		}

		// Token: 0x04006BF6 RID: 27638
		[Header("Refs")]
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04006BF7 RID: 27639
		[Tooltip("Same PlayerActionAnimator used by the towel/HideFlash/Dildo pickups. Auto-found on the player if not assigned.")]
		[SerializeField]
		private PlayerActionAnimator playerActionAnimator;

		// Token: 0x04006BF8 RID: 27640
		[Tooltip("Player swimming controller. While in the water the pickup is INSTANT (no PickUpFloor animation) so the swimming animator isn't disturbed. Auto-found if not assigned.")]
		[SerializeField]
		private vSwimming swimming;

		// Token: 0x04006BF9 RID: 27641
		[Header("Action")]
		[Tooltip("Action entry configured by Tools/SwimsuitDive/Setup (plays the PickUpFloor animator state).")]
		[SerializeField]
		private PlayerActionEntry pickupAction;

		// Token: 0x04006BFA RID: 27642
		[Tooltip("Fraction of the pickup animation after which the swimsuit snaps back onto the body.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float restoreAtNormalizedTime = 0.55f;

		// Token: 0x04006BFB RID: 27643
		[Tooltip("Fraction of the pickup animation at which the emotion layers are restored (during the exit crossfade, ~0.8).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float emotionRestoreNormalizedTime = 0.8f;

		// Token: 0x04006BFC RID: 27644
		[Header("Input")]
		[SerializeField]
		private GenericInput pickupInput = new GenericInput("E", "A", "A");

		// Token: 0x04006BFD RID: 27645
		[Tooltip("Max distance from the fallen swimsuit to allow picking it up.")]
		[SerializeField]
		private float pickupRange = 1.1f;

		// Token: 0x04006BFE RID: 27646
		[Tooltip("When true this controller reads its OWN E key (legacy). When false the unified world pickup (InventoryPickup in restore mode, wired by the cloth bridge on the slip env) owns E and calls PickUpNow() instead — so slip and drop share one E owner.")]
		[SerializeField]
		private bool listenForOwnInput;

		// Token: 0x04006BFF RID: 27647
		[Header("Events")]
		public UnityEvent OnSwimsuitPickedUp;

		// Token: 0x04006C00 RID: 27648
		private Transform _player;

		// Token: 0x04006C01 RID: 27649
		private bool _pickingUp;

		// Token: 0x04006C02 RID: 27650
		private Func<bool> _onPickup;
	}
}
