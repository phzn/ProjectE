using System;
using System.Collections;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Detection;
using Features.Flash;
using Features.Inventory;
using Features.PoolDive;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vCover;
using PoolWaterTools;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.GoldBikini
{
	// Token: 0x020011C2 RID: 4546
	public class BikiniSlipDiveController : MonoBehaviour
	{
		// Token: 0x06008B95 RID: 35733 RVA: 0x003907DD File Offset: 0x0038E9DD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetInputOwners()
		{
			BikiniSlipDiveController.ActiveControllers.Clear();
		}

		// Token: 0x06008B96 RID: 35734 RVA: 0x003907EC File Offset: 0x0038E9EC
		private void OnEnable()
		{
			if (this.coverInput != null && this.coverInput.keyboard == "P")
			{
				this.coverInput = new GenericInput("B", "Y", "B");
			}
			BikiniSlipDiveController.ActiveControllers.Add(this);
			PlayerActivityGate.Changed -= this.OnActivityChanged;
			PlayerActivityGate.Changed += this.OnActivityChanged;
		}

		// Token: 0x06008B97 RID: 35735 RVA: 0x00390860 File Offset: 0x0038EA60
		private void OnActivityChanged(PlayerActivity blocked)
		{
			if (this._readjustActivity != null && (blocked & PlayerActivity.Flash) != PlayerActivity.None)
			{
				this.EndReadjust(true);
			}
		}

		// Token: 0x06008B98 RID: 35736 RVA: 0x00390878 File Offset: 0x0038EA78
		private void EndReadjust(bool interrupted)
		{
			PlayerActivityLease readjustActivity = this._readjustActivity;
			this._readjustActivity = null;
			ClothActionGate.Release(this);
			if (interrupted && readjustActivity != null)
			{
				this.ResetCoverLayer(this._coverLeft);
				this.ResetCoverLayer(this._coverRight);
			}
			if (readjustActivity != null)
			{
				readjustActivity.Dispose();
			}
		}

		// Token: 0x06008B99 RID: 35737 RVA: 0x003908C0 File Offset: 0x0038EAC0
		private void ResetCoverLayer(BikiniSlipDiveController.CoverLayer layer)
		{
			layer.targetWeight = 0f;
			layer.waitingForActivation = false;
			if (this.animator == null || this.animator.runtimeAnimatorController == null || !this.animator.isInitialized || layer.index < 0 || layer.index >= this.animator.layerCount)
			{
				return;
			}
			this.animator.SetLayerWeight(layer.index, 0f);
			if (this.animator.HasState(layer.index, this._coverEmptyHash))
			{
				this.animator.Play(this._coverEmptyHash, layer.index, 0f);
			}
		}

		// Token: 0x06008B9A RID: 35738 RVA: 0x00390978 File Offset: 0x0038EB78
		public static bool ReservesKeyboardInput(KeyCode key)
		{
			foreach (BikiniSlipDiveController bikiniSlipDiveController in BikiniSlipDiveController.ActiveControllers)
			{
				if (!(bikiniSlipDiveController == null) && bikiniSlipDiveController.enabled && bikiniSlipDiveController.coverInput != null && bikiniSlipDiveController.coverInput.useInput && !bikiniSlipDiveController.coverInput.keyboardAxis && string.Equals(bikiniSlipDiveController.coverInput.keyboard, key.ToString(), StringComparison.OrdinalIgnoreCase) && (bikiniSlipDiveController._readjustInputFrame == Time.frameCount || bikiniSlipDiveController.CanReadjust))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x06008B9B RID: 35739 RVA: 0x00390A34 File Offset: 0x0038EC34
		public bool HasActiveSlip
		{
			get
			{
				return this._activeSlips.Count > 0;
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x06008B9C RID: 35740 RVA: 0x00390A44 File Offset: 0x0038EC44
		public bool HasSelectedSlip
		{
			get
			{
				return this.FindSelectedSlipIndex() >= 0;
			}
		}

		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x06008B9D RID: 35741 RVA: 0x00390A52 File Offset: 0x0038EC52
		public string ReadjustKey
		{
			get
			{
				if (this.coverInput == null)
				{
					return "B";
				}
				return this.coverInput.buttonName;
			}
		}

		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x06008B9E RID: 35742 RVA: 0x00390A6D File Offset: 0x0038EC6D
		public bool ReadjustOwnsAction
		{
			get
			{
				return this._readjustInputFrame == Time.frameCount || this.HasSelectedSlip || ClothActionGate.Holds(this);
			}
		}

		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x06008B9F RID: 35743 RVA: 0x00390A8C File Offset: 0x0038EC8C
		private bool CanReadjust
		{
			get
			{
				return this.HasSelectedSlip && Time.time >= this._coverCooldownUntil && !ClothActionGate.IsBusy && !GameplayInputGate.BlocksGameplayInput && !FlyCamera.IsActive && PlayerActivityGate.Allows(PlayerActivity.Flash);
			}
		}

		// Token: 0x06008BA0 RID: 35744 RVA: 0x00390AC0 File Offset: 0x0038ECC0
		private int FindSelectedSlipIndex()
		{
			if (this._activeSlips.Count == 0)
			{
				return -1;
			}
			if (this._inventory == null)
			{
				this._inventory = base.GetComponentInParent<PlayerInventoryManager>() ?? PlayerInventoryManager.Instance;
			}
			InventoryItemSO inventoryItemSO = ((this._inventory != null) ? this._inventory.GetActiveCloth() : null);
			if (inventoryItemSO == null)
			{
				return -1;
			}
			bool flag = inventoryItemSO.itemId == "cloth_bikini_bra";
			if (!flag && inventoryItemSO.itemId != "cloth_bikini_panties")
			{
				return -1;
			}
			if (this.braFlash == null || this.pantiesFlash == null)
			{
				this.ResolveFlashControllers();
			}
			ArmsFlashController armsFlashController = (flag ? this.braFlash : this.pantiesFlash);
			if (armsFlashController == null || !armsFlashController.IsWorn || armsFlashController.IsEngaged)
			{
				return -1;
			}
			for (int i = this._activeSlips.Count - 1; i >= 0; i--)
			{
				bool flag2 = this._activeSlips[i] == BikiniSlipDiveController.SlipKind.NippleLeft || this._activeSlips[i] == BikiniSlipDiveController.SlipKind.NippleRight;
				if (flag == flag2)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x06008BA1 RID: 35745 RVA: 0x00390BE0 File Offset: 0x0038EDE0
		public bool PlayerInZone
		{
			get
			{
				return this.playerInZone;
			}
		}

		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x06008BA2 RID: 35746 RVA: 0x00390BE8 File Offset: 0x0038EDE8
		public bool IsDiving
		{
			get
			{
				return this._diving;
			}
		}

		// Token: 0x17001649 RID: 5705
		// (get) Token: 0x06008BA3 RID: 35747 RVA: 0x00390BF0 File Offset: 0x0038EDF0
		public bool CanDive
		{
			get
			{
				return this.playerInZone && !this._diving && Time.time >= this._cooldownUntil;
			}
		}

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x06008BA4 RID: 35748 RVA: 0x00390C14 File Offset: 0x0038EE14
		public bool HasWornPiece
		{
			get
			{
				if (this.braFlash == null || this.pantiesFlash == null)
				{
					this.ResolveFlashControllers();
				}
				return (this.braFlash != null && this.braFlash.IsWorn) || (this.pantiesFlash != null && this.pantiesFlash.IsWorn);
			}
		}

		// Token: 0x06008BA5 RID: 35749 RVA: 0x00390C7C File Offset: 0x0038EE7C
		private void ResolveFlashControllers()
		{
			foreach (ArmsFlashController armsFlashController in ((this.animator != null) ? this.animator.transform : base.transform).GetComponentsInChildren<ArmsFlashController>(true))
			{
				string text = ((armsFlashController.Config != null) ? armsFlashController.Config.flashLayerName : null);
				if (this.braFlash == null && text == this.braLayerName)
				{
					this.braFlash = armsFlashController;
				}
				else if (this.pantiesFlash == null && text == this.pantiesLayerName)
				{
					this.pantiesFlash = armsFlashController;
				}
			}
		}

		// Token: 0x06008BA6 RID: 35750 RVA: 0x00390D24 File Offset: 0x0038EF24
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInParent<Animator>();
			}
			if (this.braFlash == null || this.pantiesFlash == null)
			{
				this.ResolveFlashControllers();
			}
			this._swimming = base.GetComponentInParent<vSwimming>() ?? base.GetComponentInChildren<vSwimming>();
			this._cc = base.GetComponentInParent<vThirdPersonController>();
			if (this._cc == null && this._swimming != null)
			{
				this._cc = this._swimming.GetComponentInParent<vThirdPersonController>();
			}
			if (this.playerController == null)
			{
				this.playerController = this._cc;
			}
			if (this.playerController == null && this.animator != null)
			{
				this.playerController = this.animator.GetComponentInParent<vThirdPersonController>();
			}
			if (this.playerController != null)
			{
				this._input = this.playerController.GetComponent<vThirdPersonInput>();
				this._genericAction = this.playerController.GetComponent<vGenericAction>();
				this._cover = this.playerController.GetComponent<vBasicCoverController>();
			}
			this._stateGuard = base.GetComponentInParent<PoolEntryStateGuard>();
			if (this._stateGuard == null && this._swimming != null)
			{
				this._stateGuard = this._swimming.GetComponentInParent<PoolEntryStateGuard>();
			}
			this._coverEmptyHash = Animator.StringToHash(this.coverEmptyStateName);
			if (this.animator != null)
			{
				this.ResolveCoverLayer(this._coverLeft, this.coverLayerLeftName);
				this.ResolveCoverLayer(this._coverRight, this.coverLayerRightName);
			}
		}

		// Token: 0x06008BA7 RID: 35751 RVA: 0x00390EC0 File Offset: 0x0038F0C0
		private void ResolveCoverLayer(BikiniSlipDiveController.CoverLayer cl, string layerName)
		{
			cl.index = this.animator.GetLayerIndex(layerName);
			if (cl.index >= 0)
			{
				this.animator.SetLayerWeight(cl.index, 0f);
				return;
			}
			Debug.LogWarning("[BikiniSlipDiveController] Cover layer '" + layerName + "' not found. Run Tools/GoldBikini/Setup.", this);
		}

		// Token: 0x06008BA8 RID: 35752 RVA: 0x00390F18 File Offset: 0x0038F118
		public void SetPlayerInZone(DiveZone zone, bool inZone)
		{
			if (zone == null)
			{
				this.SetPlayerInZone(inZone);
				return;
			}
			if (inZone)
			{
				this._activeDiveZones.Add(zone);
			}
			else
			{
				this._activeDiveZones.Remove(zone);
			}
			this.playerInZone = this._activeDiveZones.Count > 0;
		}

		// Token: 0x06008BA9 RID: 35753 RVA: 0x00390F69 File Offset: 0x0038F169
		public void SetPlayerInZone(bool inZone)
		{
			this._activeDiveZones.Clear();
			this.playerInZone = inZone;
		}

		// Token: 0x06008BAA RID: 35754 RVA: 0x00390F80 File Offset: 0x0038F180
		private void Update()
		{
			this.UpdateCoverLayerWeight();
			if (ClothActionGate.Holds(this) && Time.time >= this._coverCooldownUntil && ((!this._coverLeft.waitingForActivation && !this._coverRight.waitingForActivation && this._coverLeft.targetWeight <= 0f && this._coverRight.targetWeight <= 0f) || Time.time >= this._coverCooldownUntil + 5f))
			{
				this.EndReadjust(false);
			}
			if (this.CanReadjust && this.coverInput != null && this.coverInput.GetButtonDown())
			{
				this._readjustInputFrame = Time.frameCount;
				this.TryCover();
			}
			if (FlyCamera.IsActive)
			{
				this._runSlipTimer = 0f;
				return;
			}
			this.UpdateRunSlip();
			if (!this.allowOwnDiveKey)
			{
				return;
			}
			if (!this.CanDive)
			{
				return;
			}
			if (this.diveInput != null && this.diveInput.useInput && (ControllerInput.IsActive ? (ControllerInput.Modifier && ControllerWorldPointer.Down(ControllerButton.DpadDown)) : this.diveInput.GetButtonDown()))
			{
				base.StartCoroutine(this.DiveRoutine());
			}
		}

		// Token: 0x06008BAB RID: 35755 RVA: 0x003910A2 File Offset: 0x0038F2A2
		private void UpdateRunSlip()
		{
			this.AdvanceRunSlip(Time.deltaTime);
		}

		// Token: 0x06008BAC RID: 35756 RVA: 0x003910B0 File Offset: 0x0038F2B0
		private void AdvanceRunSlip(float deltaTime)
		{
			if (!this.CanRollRunSlip())
			{
				this._runSlipTimer = 0f;
				return;
			}
			this._runSlipTimer += deltaTime;
			if (this._runSlipTimer < Mathf.Max(0.1f, this.runSlipCheckInterval))
			{
				return;
			}
			this._runSlipTimer = 0f;
			if (Random.value <= this.runSlipChance)
			{
				this.TrySlipOneCoveredPiece();
			}
		}

		// Token: 0x06008BAD RID: 35757 RVA: 0x00391118 File Offset: 0x0038F318
		private bool CanRollRunSlip()
		{
			return this.enableRunSlip && !(this.playerController == null) && this.playerController.enabled && !this._diving && (!(this._swimming != null) || !this._swimming.inTheWater) && (this.playerController.isRunning || this.playerController.isSprinting) && this.playerController.isGrounded && this.playerController.input.sqrMagnitude > 0.01f && !this.playerController.isDead && !this.playerController.ragdolled && !this.playerController.isCrouching && !this.playerController.customAction && !this.playerController.isRolling && !this.playerController.isJumping && !this.playerController.inJumpStarted && !this.playerController.isSliding && !this.playerController.lockMovement && !this.playerController.lockAnimMovement && (!(this._input != null) || (this._input.enabled && !this._input.lockInput && !this._input.lockMoveInput)) && (!(this._genericAction != null) || !this._genericAction.doingAction) && (!(this._cover != null) || (!this._cover.inCover && !this._cover.goingToCoverPoint)) && !GameplayInputGate.BlocksGameplayInput && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.Rider);
		}

		// Token: 0x06008BAE RID: 35758 RVA: 0x003912CC File Offset: 0x0038F4CC
		public void ResetPieceSlips(string wornPieceName)
		{
			if (wornPieceName == this.braObjectName)
			{
				this._activeSlips.RemoveAll((BikiniSlipDiveController.SlipKind kind) => kind == BikiniSlipDiveController.SlipKind.NippleLeft || kind == BikiniSlipDiveController.SlipKind.NippleRight);
				return;
			}
			if (wornPieceName == this.pantiesObjectName)
			{
				this._activeSlips.RemoveAll((BikiniSlipDiveController.SlipKind kind) => kind == BikiniSlipDiveController.SlipKind.PussyLeft || kind == BikiniSlipDiveController.SlipKind.PussyRight);
			}
		}

		// Token: 0x06008BAF RID: 35759 RVA: 0x0039134C File Offset: 0x0038F54C
		public void ResetForHomeReturn()
		{
			base.StopAllCoroutines();
			this.EndReadjust(true);
			this.ResetCoverLayer(this._coverLeft);
			this.ResetCoverLayer(this._coverRight);
			this._activeSlips.Clear();
			this._activeDiveZones.Clear();
			this.playerInZone = false;
			this._runSlipTimer = (this._cooldownUntil = (this._coverCooldownUntil = 0f));
			this._readjustInputFrame = -1;
			if (this._emotionSuppressed)
			{
				PlayerEmotionController instance = PlayerEmotionController.Instance;
				if (instance != null)
				{
					instance.RestoreEmotionLayers();
				}
				this._emotionSuppressed = false;
			}
			if (this._diving && this._cc != null && this._cc._rigidbody != null)
			{
				this._cc._rigidbody.isKinematic = this._rbWasKinematic;
			}
			this._diving = false;
		}

		// Token: 0x06008BB0 RID: 35760 RVA: 0x00391424 File Offset: 0x0038F624
		private void TrySlipOneCoveredPiece()
		{
			List<BikiniSlipDiveController.SlipKind> list = this.BuildCoveredCandidates();
			if (list.Count == 0)
			{
				return;
			}
			this.FireSlip(list[Random.Range(0, list.Count)]);
		}

		// Token: 0x06008BB1 RID: 35761 RVA: 0x0039145C File Offset: 0x0038F65C
		private List<BikiniSlipDiveController.SlipKind> BuildCoveredCandidates()
		{
			if (this.braFlash == null || this.pantiesFlash == null)
			{
				this.ResolveFlashControllers();
			}
			bool flag = this.braFlash != null && this.braFlash.IsWorn && !this.braFlash.IsEngaged && !this.HasPieceSlip(true);
			bool flag2 = this.pantiesFlash != null && this.pantiesFlash.IsWorn && !this.pantiesFlash.IsEngaged && !this.HasPieceSlip(false);
			List<BikiniSlipDiveController.SlipKind> list = new List<BikiniSlipDiveController.SlipKind>(4);
			if (flag)
			{
				if (!this._activeSlips.Contains(BikiniSlipDiveController.SlipKind.NippleLeft))
				{
					list.Add(BikiniSlipDiveController.SlipKind.NippleLeft);
				}
				if (!this._activeSlips.Contains(BikiniSlipDiveController.SlipKind.NippleRight))
				{
					list.Add(BikiniSlipDiveController.SlipKind.NippleRight);
				}
			}
			if (flag2)
			{
				if (!this._activeSlips.Contains(BikiniSlipDiveController.SlipKind.PussyLeft))
				{
					list.Add(BikiniSlipDiveController.SlipKind.PussyLeft);
				}
				if (!this._activeSlips.Contains(BikiniSlipDiveController.SlipKind.PussyRight))
				{
					list.Add(BikiniSlipDiveController.SlipKind.PussyRight);
				}
			}
			return list;
		}

		// Token: 0x06008BB2 RID: 35762 RVA: 0x00391557 File Offset: 0x0038F757
		private bool HasPieceSlip(bool bra)
		{
			if (!bra)
			{
				return this._activeSlips.Contains(BikiniSlipDiveController.SlipKind.PussyLeft) || this._activeSlips.Contains(BikiniSlipDiveController.SlipKind.PussyRight);
			}
			return this._activeSlips.Contains(BikiniSlipDiveController.SlipKind.NippleLeft) || this._activeSlips.Contains(BikiniSlipDiveController.SlipKind.NippleRight);
		}

		// Token: 0x06008BB3 RID: 35763 RVA: 0x00391598 File Offset: 0x0038F798
		public int TrySlipCoveredPieces(float chance, int maxPieces = 2)
		{
			if (Random.value > Mathf.Clamp01(chance))
			{
				return 0;
			}
			List<BikiniSlipDiveController.SlipKind> list = this.BuildCoveredCandidates();
			if (list.Count == 0)
			{
				return 0;
			}
			int num = Mathf.Clamp(Random.Range(1, maxPieces + 1), 1, list.Count);
			int num2 = 0;
			int num3 = 0;
			while (num3 < num && list.Count > 0)
			{
				int num4 = Random.Range(0, list.Count);
				if (this.FireSlip(list[num4]))
				{
					num2++;
				}
				list.RemoveAt(num4);
				num3++;
			}
			return num2;
		}

		// Token: 0x06008BB4 RID: 35764 RVA: 0x00391620 File Offset: 0x0038F820
		private bool FireSlip(BikiniSlipDiveController.SlipKind kind)
		{
			if (this.HasPieceSlip(kind == BikiniSlipDiveController.SlipKind.NippleLeft || kind == BikiniSlipDiveController.SlipKind.NippleRight))
			{
				return false;
			}
			bool flag;
			switch (kind)
			{
			case BikiniSlipDiveController.SlipKind.NippleLeft:
				flag = this.SetTrigger((this.braFlash != null) ? this.braFlash.EnsureDetailAnimator() : null, this.nippleSlipLeftTrigger, "bra");
				break;
			case BikiniSlipDiveController.SlipKind.NippleRight:
				flag = this.SetTrigger((this.braFlash != null) ? this.braFlash.EnsureDetailAnimator() : null, this.nippleSlipRightTrigger, "bra");
				break;
			case BikiniSlipDiveController.SlipKind.PussyLeft:
				flag = this.SetTrigger((this.pantiesFlash != null) ? this.pantiesFlash.EnsureDetailAnimator() : null, this.pussySlipLeftTrigger, "panties");
				break;
			default:
				flag = this.SetTrigger((this.pantiesFlash != null) ? this.pantiesFlash.EnsureDetailAnimator() : null, this.pussySlipRightTrigger, "panties");
				break;
			}
			if (flag)
			{
				this._activeSlips.Add(kind);
			}
			return flag;
		}

		// Token: 0x06008BB5 RID: 35765 RVA: 0x00391729 File Offset: 0x0038F929
		public bool TryFireRandomSlipsOnDive()
		{
			if (!this.AnyBikiniPieceWorn())
			{
				return false;
			}
			if (Random.value > this.slipChanceOnDive)
			{
				return false;
			}
			this.FireRandomSlips();
			return true;
		}

		// Token: 0x06008BB6 RID: 35766 RVA: 0x0039174C File Offset: 0x0038F94C
		private bool AnyBikiniPieceWorn()
		{
			bool flag = this.braFlash != null && this.braFlash.IsWorn;
			bool flag2 = this.pantiesFlash != null && this.pantiesFlash.IsWorn;
			return flag || flag2;
		}

		// Token: 0x06008BB7 RID: 35767 RVA: 0x00391794 File Offset: 0x0038F994
		private IEnumerator DiveRoutine()
		{
			if (this.animator == null)
			{
				yield break;
			}
			this._diving = true;
			this._cooldownUntil = Time.time + this.diveCooldown + this.fallbackDiveDuration;
			if (this._stateGuard != null)
			{
				this._stateGuard.NotifyDiveStarting();
			}
			Transform playerRoot = this.animator.transform;
			Collider water = this.FindOverlappingWater(playerRoot.position);
			float waterlineY = ((water != null) ? water.transform.position.y : this.fallbackWaterlineY);
			PlayerEmotionController emo = PlayerEmotionController.Instance;
			if (emo != null)
			{
				emo.DisableEmotionLayersForAction();
				this._emotionSuppressed = true;
			}
			this.animator.CrossFadeInFixedTime(this.diveStateName, this.diveCrossfade);
			bool rbWasKinematic = false;
			if (this._cc != null && this._cc._rigidbody != null)
			{
				rbWasKinematic = this._cc._rigidbody.isKinematic;
				this._cc._rigidbody.velocity = Vector3.zero;
				this._cc._rigidbody.isKinematic = true;
			}
			float duration = this.ResolveDiveDuration();
			float elapsed = 0f;
			bool slipped = false;
			while (elapsed < duration + 0.5f)
			{
				if (!slipped && this.BodyCenterY(playerRoot) <= waterlineY)
				{
					this.FireRandomSlips();
					slipped = true;
				}
				if (this.DiveNormalizedTime() >= this.swimHandoffNormalizedTime)
				{
					break;
				}
				elapsed += Time.deltaTime;
				yield return null;
			}
			if (!slipped)
			{
				this.FireRandomSlips();
			}
			if (this._emotionSuppressed && emo != null)
			{
				emo.RestoreEmotionLayers();
				this._emotionSuppressed = false;
			}
			this._rbWasKinematic = rbWasKinematic;
			if (this.BodyCenterY(playerRoot) <= waterlineY + this.submergeDepth)
			{
				this.EngageSwimming(water, playerRoot.position);
			}
			this._diving = false;
			this._cooldownUntil = Time.time + this.diveCooldown;
			yield break;
		}

		// Token: 0x06008BB8 RID: 35768 RVA: 0x003917A4 File Offset: 0x0038F9A4
		private void FireRandomSlips()
		{
			bool flag = this.braFlash != null && this.braFlash.IsWorn;
			bool flag2 = this.pantiesFlash != null && this.pantiesFlash.IsWorn;
			if (!flag && !flag2)
			{
				return;
			}
			bool flag3 = flag && Random.value < 0.5f;
			bool flag4 = flag2 && Random.value < 0.5f;
			if (!flag3 && !flag4)
			{
				if (flag && flag2)
				{
					if (Random.value < 0.5f)
					{
						flag3 = true;
					}
					else
					{
						flag4 = true;
					}
				}
				else if (flag)
				{
					flag3 = true;
				}
				else
				{
					flag4 = true;
				}
			}
			Debug.Log(string.Format("[SlipDiveDIAG] FireRandomSlips: doNipple={0} doPussy={1} braFlash={2} pantiesFlash={3}", new object[]
			{
				flag3,
				flag4,
				this.braFlash != null,
				this.pantiesFlash != null
			}));
			if (flag3)
			{
				this.FireSlip((Random.value < 0.5f) ? BikiniSlipDiveController.SlipKind.NippleLeft : BikiniSlipDiveController.SlipKind.NippleRight);
			}
			if (flag4)
			{
				this.FireSlip((Random.value < 0.5f) ? BikiniSlipDiveController.SlipKind.PussyLeft : BikiniSlipDiveController.SlipKind.PussyRight);
			}
		}

		// Token: 0x06008BB9 RID: 35769 RVA: 0x003918D0 File Offset: 0x0038FAD0
		private void TryCover()
		{
			if (!this.CanReadjust)
			{
				return;
			}
			int num = this.FindSelectedSlipIndex();
			if (num < 0 || !ClothActionGate.TryBegin(this))
			{
				return;
			}
			if (!PlayerActivityGate.TryAcquire("BikiniReadjust", this, PlayerActivity.Flash, PlayerContextResource.WorldInteraction | PlayerContextResource.FullBodyAnimation, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Interact | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.ArmsPose, out this._readjustActivity, false, null))
			{
				ClothActionGate.Release(this);
				return;
			}
			this._readjustInputFrame = Time.frameCount;
			this._coverCooldownUntil = Time.time + Mathf.Max(0f, this.coverCooldown);
			string text;
			string text2;
			Animator animator;
			bool flag;
			switch (this._activeSlips[num])
			{
			case BikiniSlipDiveController.SlipKind.NippleLeft:
				text = this.coverNippleLeftBodyTrigger;
				text2 = this.nippleCoverLeftTrigger;
				animator = ((this.braFlash != null) ? this.braFlash.EnsureDetailAnimator() : null);
				flag = true;
				break;
			case BikiniSlipDiveController.SlipKind.NippleRight:
				text = this.coverNippleRightBodyTrigger;
				text2 = this.nippleCoverRightTrigger;
				animator = ((this.braFlash != null) ? this.braFlash.EnsureDetailAnimator() : null);
				flag = false;
				break;
			case BikiniSlipDiveController.SlipKind.PussyLeft:
				text = this.coverPussyLeftBodyTrigger;
				text2 = this.pussyCoverLeftTrigger;
				animator = ((this.pantiesFlash != null) ? this.pantiesFlash.EnsureDetailAnimator() : null);
				flag = true;
				break;
			default:
				text = this.coverPussyRightBodyTrigger;
				text2 = this.pussyCoverRightTrigger;
				animator = ((this.pantiesFlash != null) ? this.pantiesFlash.EnsureDetailAnimator() : null);
				flag = false;
				break;
			}
			if (!this.SetTrigger(animator, text2, "cover-shape"))
			{
				this.EndReadjust(true);
				return;
			}
			this._activeSlips.RemoveAt(num);
			this.PulseCoverGesture(flag ? this._coverLeft : this._coverRight, text);
		}

		// Token: 0x06008BBA RID: 35770 RVA: 0x00391A6C File Offset: 0x0038FC6C
		private void PulseCoverGesture(BikiniSlipDiveController.CoverLayer cl, string bodyTrig)
		{
			if (this.animator == null || cl.index < 0)
			{
				return;
			}
			this.animator.ResetTrigger(this.coverNippleLeftBodyTrigger);
			this.animator.ResetTrigger(this.coverNippleRightBodyTrigger);
			this.animator.ResetTrigger(this.coverPussyLeftBodyTrigger);
			this.animator.ResetTrigger(this.coverPussyRightBodyTrigger);
			this.animator.SetTrigger(bodyTrig);
			cl.targetWeight = 1f;
			cl.fadeSpeed = ((this.coverFadeInDuration > 0f) ? (1f / this.coverFadeInDuration) : float.PositiveInfinity);
			cl.waitingForActivation = true;
		}

		// Token: 0x06008BBB RID: 35771 RVA: 0x00391B19 File Offset: 0x0038FD19
		private void UpdateCoverLayerWeight()
		{
			if (this.animator == null)
			{
				return;
			}
			this.UpdateOneCoverLayer(this._coverLeft);
			this.UpdateOneCoverLayer(this._coverRight);
		}

		// Token: 0x06008BBC RID: 35772 RVA: 0x00391B44 File Offset: 0x0038FD44
		private void UpdateOneCoverLayer(BikiniSlipDiveController.CoverLayer cl)
		{
			if (cl.index < 0)
			{
				return;
			}
			if (cl.waitingForActivation)
			{
				if (this.animator.GetCurrentAnimatorStateInfo(cl.index).shortNameHash != this._coverEmptyHash || this.animator.IsInTransition(cl.index))
				{
					cl.waitingForActivation = false;
				}
			}
			else if (cl.targetWeight > 0f && (this.animator.GetCurrentAnimatorStateInfo(cl.index).shortNameHash == this._coverEmptyHash && !this.animator.IsInTransition(cl.index)))
			{
				cl.targetWeight = 0f;
				cl.fadeSpeed = ((this.coverFadeOutDuration > 0f) ? (1f / this.coverFadeOutDuration) : float.PositiveInfinity);
			}
			float layerWeight = this.animator.GetLayerWeight(cl.index);
			if (!Mathf.Approximately(layerWeight, cl.targetWeight))
			{
				this.animator.SetLayerWeight(cl.index, Mathf.MoveTowards(layerWeight, cl.targetWeight, cl.fadeSpeed * Time.deltaTime));
			}
		}

		// Token: 0x06008BBD RID: 35773 RVA: 0x00391C6C File Offset: 0x0038FE6C
		private bool SetTrigger(Animator a, string trigger, string label)
		{
			if (a == null || !a.isActiveAndEnabled || a.runtimeAnimatorController == null)
			{
				return false;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in a.parameters)
			{
				if (!(animatorControllerParameter.name != trigger) && animatorControllerParameter.type == AnimatorControllerParameterType.Trigger)
				{
					a.SetTrigger(trigger);
					return true;
				}
			}
			Debug.LogWarning(string.Concat(new string[]
			{
				"[BikiniSlipDiveController] ",
				label,
				": missing trigger '",
				trigger,
				"' on '",
				a.gameObject.name,
				"'."
			}), this);
			return false;
		}

		// Token: 0x06008BBE RID: 35774 RVA: 0x00391D1C File Offset: 0x0038FF1C
		private float DiveNormalizedTime()
		{
			if (this.animator == null)
			{
				return 0f;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.IsName(this.diveStateName))
			{
				return currentAnimatorStateInfo.normalizedTime;
			}
			AnimatorStateInfo nextAnimatorStateInfo = this.animator.GetNextAnimatorStateInfo(0);
			if (this.animator.IsInTransition(0) && nextAnimatorStateInfo.IsName(this.diveStateName))
			{
				return nextAnimatorStateInfo.normalizedTime;
			}
			return 0f;
		}

		// Token: 0x06008BBF RID: 35775 RVA: 0x00391D98 File Offset: 0x0038FF98
		private void EngageSwimming(Collider knownWater, Vector3 playerPos)
		{
			if (this._swimming == null)
			{
				return;
			}
			Collider collider = ((knownWater != null) ? knownWater : this.FindOverlappingWater(playerPos));
			if (collider == null)
			{
				return;
			}
			Transform transform = ((this.animator != null) ? this.animator.transform : base.transform);
			if (this.animator != null)
			{
				this.animator.CrossFadeInFixedTime("Swimming.Swimming", 0.1f, 0);
			}
			if (this._cc != null)
			{
				this._cc.customAction = false;
				this._cc.verticalVelocity = 0f;
			}
			if (this._cc != null)
			{
				float num = collider.transform.position.y - (this._cc.colliderHeightDefault * 0.5f + this._swimming.heightOffset);
				Vector3 position = transform.position;
				if (position.y > num)
				{
					position.y = num;
					transform.position = position;
				}
				if (this._cc._rigidbody != null)
				{
					this._cc._rigidbody.position = transform.position;
					this._cc._rigidbody.velocity = Vector3.zero;
				}
			}
			this._swimming.OnActionEnter(collider);
			float num2 = 0.35f;
			float num3 = collider.transform.position.y - ((this._cc != null) ? (this._cc.colliderHeightDefault * 0.5f + this._swimming.heightOffset) : this._swimming.heightOffset) - num2;
			base.StartCoroutine(this.LockSwimUntilStable(transform, collider, num3));
		}

		// Token: 0x06008BC0 RID: 35776 RVA: 0x00391F53 File Offset: 0x00390153
		private IEnumerator LockSwimUntilStable(Transform playerRoot, Collider water, float surfaceTargetY)
		{
			int stable = 0;
			int num;
			for (int i = 0; i < 40; i = num + 1)
			{
				if (this._swimming == null || this._cc == null || this._cc._rigidbody == null)
				{
					yield break;
				}
				this._cc._rigidbody.isKinematic = true;
				this._cc._rigidbody.velocity = Vector3.zero;
				this._cc._rigidbody.useGravity = false;
				Vector3 position = playerRoot.position;
				position.y = surfaceTargetY;
				playerRoot.position = position;
				this._cc._rigidbody.position = position;
				this._cc.customAction = false;
				if (water != null)
				{
					this._swimming.water = water.gameObject;
				}
				this._swimming.inTheWater = true;
				stable = ((!this.IsAnimatorInDive() && !this._cc.customAction && this._swimming.isSwimming && this._swimming.currentCharacterDepth >= 0f) ? (stable + 1) : 0);
				if (stable >= 3)
				{
					break;
				}
				yield return new WaitForFixedUpdate();
				num = i;
			}
			if (this._cc != null && this._cc._rigidbody != null)
			{
				this._cc._rigidbody.isKinematic = this._rbWasKinematic;
				this._cc._rigidbody.velocity = Vector3.zero;
			}
			yield break;
		}

		// Token: 0x06008BC1 RID: 35777 RVA: 0x00391F78 File Offset: 0x00390178
		private bool IsAnimatorInDive()
		{
			return !(this.animator == null) && (this.animator.GetCurrentAnimatorStateInfo(0).IsName(this.diveStateName) || (this.animator.IsInTransition(0) && this.animator.GetNextAnimatorStateInfo(0).IsName(this.diveStateName)));
		}

		// Token: 0x06008BC2 RID: 35778 RVA: 0x0029F2C4 File Offset: 0x0029D4C4
		private float BodyCenterY(Transform playerRoot)
		{
			return playerRoot.position.y + 0.9f;
		}

		// Token: 0x06008BC3 RID: 35779 RVA: 0x00391FE0 File Offset: 0x003901E0
		private Collider FindOverlappingWater(Vector3 near)
		{
			foreach (Collider collider in Physics.OverlapSphere(near + Vector3.up, this.waterProbeRadius, -1, QueryTriggerInteraction.Collide))
			{
				if (collider != null && collider.CompareTag(this.waterTag))
				{
					return collider;
				}
			}
			return null;
		}

		// Token: 0x06008BC4 RID: 35780 RVA: 0x00392034 File Offset: 0x00390234
		private float ResolveDiveDuration()
		{
			if (this.animator.runtimeAnimatorController != null)
			{
				foreach (AnimationClip animationClip in this.animator.runtimeAnimatorController.animationClips)
				{
					if (animationClip != null && animationClip.name.Contains("dive"))
					{
						return animationClip.length;
					}
				}
			}
			return this.fallbackDiveDuration;
		}

		// Token: 0x06008BC5 RID: 35781 RVA: 0x003920A0 File Offset: 0x003902A0
		private void OnDisable()
		{
			BikiniSlipDiveController.ActiveControllers.Remove(this);
			PlayerActivityGate.Changed -= this.OnActivityChanged;
			this.EndReadjust(true);
			this._runSlipTimer = 0f;
			this._activeDiveZones.Clear();
			this.playerInZone = false;
			if (this._emotionSuppressed && PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.RestoreEmotionLayers();
				this._emotionSuppressed = false;
			}
			if (this._diving && this._cc != null && this._cc._rigidbody != null && this._cc._rigidbody.isKinematic)
			{
				this._cc._rigidbody.isKinematic = false;
			}
		}

		// Token: 0x04008E6E RID: 36462
		private static readonly HashSet<BikiniSlipDiveController> ActiveControllers = new HashSet<BikiniSlipDiveController>();

		// Token: 0x04008E6F RID: 36463
		private int _readjustInputFrame = -1;

		// Token: 0x04008E70 RID: 36464
		private PlayerActivityLease _readjustActivity;

		// Token: 0x04008E71 RID: 36465
		[Header("Refs")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04008E72 RID: 36466
		[Tooltip("Set by the DiveZone trigger; true while the player can dive.")]
		[SerializeField]
		private bool playerInZone;

		// Token: 0x04008E73 RID: 36467
		[Header("Worn meshes")]
		[SerializeField]
		private string braObjectName = "EPBikini_Bra";

		// Token: 0x04008E74 RID: 36468
		[SerializeField]
		private string pantiesObjectName = "EPBikini_Panties";

		// Token: 0x04008E75 RID: 36469
		[Tooltip("Flash controllers — used to resolve the SAME worn-mesh blendShape animators the flash uses (so the slip triggers land on the right animator). Auto-found by layer name.")]
		[SerializeField]
		private ArmsFlashController braFlash;

		// Token: 0x04008E76 RID: 36470
		[SerializeField]
		private ArmsFlashController pantiesFlash;

		// Token: 0x04008E77 RID: 36471
		[SerializeField]
		private string braLayerName = "BikiniBraFlash";

		// Token: 0x04008E78 RID: 36472
		[SerializeField]
		private string pantiesLayerName = "BikiniPantiesFlash";

		// Token: 0x04008E79 RID: 36473
		[Header("Animator (dive)")]
		[SerializeField]
		private string diveStateName = "PoolDive";

		// Token: 0x04008E7A RID: 36474
		[SerializeField]
		private float diveCrossfade = 0.1f;

		// Token: 0x04008E7B RID: 36475
		[SerializeField]
		private float fallbackDiveDuration = 6.5f;

		// Token: 0x04008E7C RID: 36476
		[SerializeField]
		[Range(0.5f, 0.9f)]
		private float swimHandoffNormalizedTime = 0.85f;

		// Token: 0x04008E7D RID: 36477
		[Header("Slip triggers (on the piece-only controllers)")]
		[SerializeField]
		private string nippleSlipLeftTrigger = "NippleSlipLeft";

		// Token: 0x04008E7E RID: 36478
		[SerializeField]
		private string nippleSlipRightTrigger = "NippleSlipRight";

		// Token: 0x04008E7F RID: 36479
		[SerializeField]
		private string pussySlipLeftTrigger = "PussySlipLeft";

		// Token: 0x04008E80 RID: 36480
		[SerializeField]
		private string pussySlipRightTrigger = "PussySlipRight";

		// Token: 0x04008E81 RID: 36481
		[Header("Cover (re-adjust) — blendShape un-slip triggers on the piece controllers")]
		[SerializeField]
		private string nippleCoverLeftTrigger = "NippleCoverLeft";

		// Token: 0x04008E82 RID: 36482
		[SerializeField]
		private string nippleCoverRightTrigger = "NippleCoverRight";

		// Token: 0x04008E83 RID: 36483
		[SerializeField]
		private string pussyCoverLeftTrigger = "PussyCoverLeft";

		// Token: 0x04008E84 RID: 36484
		[SerializeField]
		private string pussyCoverRightTrigger = "PussyCoverRight";

		// Token: 0x04008E85 RID: 36485
		[Header("Cover — Ciel arm gesture (single-arm layers on CielAnimatorController)")]
		[Tooltip("LEFT-arm cover layer (LeftArm mask) — holds the *Left cover gestures.")]
		[SerializeField]
		private string coverLayerLeftName = "BikiniSlipCoverLeft";

		// Token: 0x04008E86 RID: 36486
		[Tooltip("RIGHT-arm cover layer (RightArm mask) — holds the *Right cover gestures.")]
		[SerializeField]
		private string coverLayerRightName = "BikiniSlipCoverRight";

		// Token: 0x04008E87 RID: 36487
		[SerializeField]
		private string coverNippleLeftBodyTrigger = "CoverNippleLeft";

		// Token: 0x04008E88 RID: 36488
		[SerializeField]
		private string coverNippleRightBodyTrigger = "CoverNippleRight";

		// Token: 0x04008E89 RID: 36489
		[SerializeField]
		private string coverPussyLeftBodyTrigger = "CoverPussyLeft";

		// Token: 0x04008E8A RID: 36490
		[SerializeField]
		private string coverPussyRightBodyTrigger = "CoverPussyRight";

		// Token: 0x04008E8B RID: 36491
		[SerializeField]
		private string coverEmptyStateName = "Empty";

		// Token: 0x04008E8C RID: 36492
		[SerializeField]
		private float coverFadeInDuration = 0.15f;

		// Token: 0x04008E8D RID: 36493
		[SerializeField]
		private float coverFadeOutDuration = 0.25f;

		// Token: 0x04008E8E RID: 36494
		[SerializeField]
		private float coverCooldown = 0.5f;

		// Token: 0x04008E8F RID: 36495
		[Header("Water handoff")]
		[SerializeField]
		private string waterTag = "Water";

		// Token: 0x04008E90 RID: 36496
		[SerializeField]
		private float waterProbeRadius = 1.2f;

		// Token: 0x04008E91 RID: 36497
		[SerializeField]
		private float fallbackWaterlineY = -5.66f;

		// Token: 0x04008E92 RID: 36498
		[SerializeField]
		private float submergeDepth = 0.25f;

		// Token: 0x04008E93 RID: 36499
		[Header("Slip chance")]
		[Tooltip("When the normal pool dive (G) is taken WHILE the bikini is worn, the probability (0..1) that a wardrobe slip fires at all. If it fires, which/how-many pieces slip stays random (see FireRandomSlips). 0 = never, 1 = always.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float slipChanceOnDive = 0.6f;

		// Token: 0x04008E94 RID: 36500
		[Header("Slip while running")]
		[Tooltip("Enable the random wardrobe slip happening WHILE Ciel runs/sprints (the bounce works a piece loose). Uses the same blendShape slip as the dive, re-adjusted with P.")]
		[SerializeField]
		private bool enableRunSlip = true;

		// Token: 0x04008E95 RID: 36501
		[Tooltip("Probability (0..1) rolled every 'Run slip check interval' of continuous running. Only pieces NOT already slipped are eligible, so each side slips at most once until the player re-adjusts it (then it can happen again).")]
		[SerializeField]
		[Range(0f, 1f)]
		private float runSlipChance = 0.25f;

		// Token: 0x04008E96 RID: 36502
		[Tooltip("Seconds of continuous running between probability rolls.")]
		[SerializeField]
		private float runSlipCheckInterval = 2f;

		// Token: 0x04008E97 RID: 36503
		[Tooltip("Player controller used to read isRunning/isSprinting. Auto-found from the animator.")]
		[SerializeField]
		private vThirdPersonController playerController;

		// Token: 0x04008E98 RID: 36504
		[Header("Input")]
		[Tooltip("Dedicated slip-dive key. DISABLED by default: the normal pool dive (G) now triggers the (probabilistic) slip via PoolDiveController → TryFireRandomSlipsOnDive(). Leave off to avoid a second dive key.")]
		[SerializeField]
		private bool allowOwnDiveKey;

		// Token: 0x04008E99 RID: 36505
		[SerializeField]
		private GenericInput diveInput = new GenericInput("O", "", "O");

		// Token: 0x04008E9A RID: 36506
		[SerializeField]
		private float diveCooldown = 1f;

		// Token: 0x04008E9B RID: 36507
		[Tooltip("Re-adjust key: covers ONE active slip per press (most-recent first), arms + blendShape.")]
		[SerializeField]
		private GenericInput coverInput = new GenericInput("B", "Y", "B");

		// Token: 0x04008E9C RID: 36508
		private bool _diving;

		// Token: 0x04008E9D RID: 36509
		private float _cooldownUntil;

		// Token: 0x04008E9E RID: 36510
		private bool _emotionSuppressed;

		// Token: 0x04008E9F RID: 36511
		private bool _rbWasKinematic;

		// Token: 0x04008EA0 RID: 36512
		private readonly HashSet<DiveZone> _activeDiveZones = new HashSet<DiveZone>();

		// Token: 0x04008EA1 RID: 36513
		private readonly List<BikiniSlipDiveController.SlipKind> _activeSlips = new List<BikiniSlipDiveController.SlipKind>();

		// Token: 0x04008EA2 RID: 36514
		private readonly BikiniSlipDiveController.CoverLayer _coverLeft = new BikiniSlipDiveController.CoverLayer();

		// Token: 0x04008EA3 RID: 36515
		private readonly BikiniSlipDiveController.CoverLayer _coverRight = new BikiniSlipDiveController.CoverLayer();

		// Token: 0x04008EA4 RID: 36516
		private int _coverEmptyHash;

		// Token: 0x04008EA5 RID: 36517
		private float _coverCooldownUntil;

		// Token: 0x04008EA6 RID: 36518
		private float _runSlipTimer;

		// Token: 0x04008EA7 RID: 36519
		private PlayerInventoryManager _inventory;

		// Token: 0x04008EA8 RID: 36520
		private vSwimming _swimming;

		// Token: 0x04008EA9 RID: 36521
		private vThirdPersonController _cc;

		// Token: 0x04008EAA RID: 36522
		private vThirdPersonInput _input;

		// Token: 0x04008EAB RID: 36523
		private vGenericAction _genericAction;

		// Token: 0x04008EAC RID: 36524
		private vBasicCoverController _cover;

		// Token: 0x04008EAD RID: 36525
		private PoolEntryStateGuard _stateGuard;

		// Token: 0x020011C3 RID: 4547
		private enum SlipKind
		{
			// Token: 0x04008EAF RID: 36527
			NippleLeft,
			// Token: 0x04008EB0 RID: 36528
			NippleRight,
			// Token: 0x04008EB1 RID: 36529
			PussyLeft,
			// Token: 0x04008EB2 RID: 36530
			PussyRight
		}

		// Token: 0x020011C4 RID: 4548
		private class CoverLayer
		{
			// Token: 0x04008EB3 RID: 36531
			public int index = -1;

			// Token: 0x04008EB4 RID: 36532
			public float targetWeight;

			// Token: 0x04008EB5 RID: 36533
			public float fadeSpeed;

			// Token: 0x04008EB6 RID: 36534
			public bool waitingForActivation;
		}
	}
}
