using System;
using System.Collections;
using Features.DebugTools;
using Features.Detection;
using Features.Slip;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.GoldBikini
{
	// Token: 0x020011C0 RID: 4544
	public class BikiniPickupController : MonoBehaviour
	{
		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x06008B84 RID: 35716 RVA: 0x00390355 File Offset: 0x0038E555
		// (set) Token: 0x06008B85 RID: 35717 RVA: 0x0039035D File Offset: 0x0038E55D
		public bool CanPickUp { get; private set; }

		// Token: 0x06008B86 RID: 35718 RVA: 0x00390366 File Offset: 0x0038E566
		public void SetPickupAction(Func<bool> onPickup)
		{
			this._onPickup = onPickup;
		}

		// Token: 0x06008B87 RID: 35719 RVA: 0x0039036F File Offset: 0x0038E56F
		public void PickUpNow()
		{
			if (this._pickingUp || this.slipController == null)
			{
				return;
			}
			base.StartCoroutine(this.PickupRoutine());
		}

		// Token: 0x06008B88 RID: 35720 RVA: 0x00390395 File Offset: 0x0038E595
		private bool DoPickup()
		{
			if (this._onPickup != null)
			{
				return this._onPickup();
			}
			return this.slipController.RestoreWorn(null);
		}

		// Token: 0x06008B89 RID: 35721 RVA: 0x003903B8 File Offset: 0x0038E5B8
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
				Debug.LogWarning("[BikiniPickupController] PlayerActionAnimator not found — pickup will restore the bikini without animation.", this);
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

		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x06008B8A RID: 35722 RVA: 0x0039049D File Offset: 0x0038E69D
		private bool InWater
		{
			get
			{
				return this.swimming != null && this.swimming.inTheWater;
			}
		}

		// Token: 0x06008B8B RID: 35723 RVA: 0x003904BA File Offset: 0x0038E6BA
		private void Update()
		{
			this.CanPickUp = this.EvaluateCanPickUp();
			if (this.CanPickUp && !FlyCamera.IsActive && this.pickupInput != null && this.pickupInput.GetButtonDown())
			{
				base.StartCoroutine(this.PickupRoutine());
			}
		}

		// Token: 0x06008B8C RID: 35724 RVA: 0x003904FC File Offset: 0x0038E6FC
		private bool EvaluateCanPickUp()
		{
			if (this._pickingUp || this.slipController == null || !this.slipController.AnySlipped || this._player == null)
			{
				return false;
			}
			Vector3? vector = this.slipController.NearestEnvPosition(this._player.position);
			return vector != null && Vector3.Distance(this._player.position, vector.Value) <= this.pickupRange;
		}

		// Token: 0x06008B8D RID: 35725 RVA: 0x00390580 File Offset: 0x0038E780
		private IEnumerator PickupRoutine()
		{
			this._pickingUp = true;
			this.CanPickUp = false;
			if (this.InWater)
			{
				if (this.DoPickup())
				{
					UnityEvent onBikiniPickedUp = this.OnBikiniPickedUp;
					if (onBikiniPickedUp != null)
					{
						onBikiniPickedUp.Invoke();
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
				UnityEvent onBikiniPickedUp2 = this.OnBikiniPickedUp;
				if (onBikiniPickedUp2 != null)
				{
					onBikiniPickedUp2.Invoke();
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

		// Token: 0x04008E5B RID: 36443
		[Header("Refs")]
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04008E5C RID: 36444
		[Tooltip("Same PlayerActionAnimator used by the towel/swimsuit pickups. Auto-found on the player if not assigned.")]
		[SerializeField]
		private PlayerActionAnimator playerActionAnimator;

		// Token: 0x04008E5D RID: 36445
		[Tooltip("Player swimming controller. While in the water the pickup is INSTANT (no PickUpFloor animation) so the swimming animator isn't disturbed. Auto-found if not assigned.")]
		[SerializeField]
		private vSwimming swimming;

		// Token: 0x04008E5E RID: 36446
		[Header("Action")]
		[Tooltip("Action entry that plays the PickUpFloor animator state (the towel/swimsuit setup authors this).")]
		[SerializeField]
		private PlayerActionEntry pickupAction;

		// Token: 0x04008E5F RID: 36447
		[Tooltip("Fraction of the pickup animation after which the bikini snaps back onto the body.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float restoreAtNormalizedTime = 0.55f;

		// Token: 0x04008E60 RID: 36448
		[Tooltip("Fraction of the pickup animation at which the emotion layers are restored (~0.8).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float emotionRestoreNormalizedTime = 0.8f;

		// Token: 0x04008E61 RID: 36449
		[Header("Input")]
		[SerializeField]
		private GenericInput pickupInput = new GenericInput("E", "A", "A");

		// Token: 0x04008E62 RID: 36450
		[Tooltip("Max distance from the fallen bikini to allow picking it up.")]
		[SerializeField]
		private float pickupRange = 1.1f;

		// Token: 0x04008E63 RID: 36451
		[Header("Events")]
		public UnityEvent OnBikiniPickedUp;

		// Token: 0x04008E64 RID: 36452
		private Transform _player;

		// Token: 0x04008E65 RID: 36453
		private bool _pickingUp;

		// Token: 0x04008E66 RID: 36454
		private Func<bool> _onPickup;
	}
}
