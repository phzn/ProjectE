using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Features.BoobTouch;
using Features.Detection;
using Features.Rank;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist
{
	// Token: 0x020008ED RID: 2285
	public class ShowerInteraction : MonoBehaviour, IInteractionInputCandidate, IInteractionCandidate
	{
		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06003DBE RID: 15806 RVA: 0x0016F48C File Offset: 0x0016D68C
		public static bool IsAnyShowerActionActive
		{
			get
			{
				using (HashSet<ShowerInteraction>.Enumerator enumerator = ShowerInteraction.ActiveActions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current != null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x0016F4E8 File Offset: 0x0016D6E8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetActiveActions()
		{
			ShowerInteraction.ActiveActions.Clear();
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x0016F4F4 File Offset: 0x0016D6F4
		public float CandidateSqrDistance
		{
			get
			{
				return this._candidateSqrDistance;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06003DC1 RID: 15809 RVA: 0x0016F4FC File Offset: 0x0016D6FC
		public bool IsCandidateEligible
		{
			get
			{
				return this._playerInRange != null && !this._isShowering && !this._soapModeObserved;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06003DC3 RID: 15811 RVA: 0x0016F51F File Offset: 0x0016D71F
		public string CandidateInputBinding
		{
			get
			{
				return InteractionCandidateRegistry.ActiveBinding(this.interactInput);
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x0016F52C File Offset: 0x0016D72C
		public bool IsShowering
		{
			get
			{
				return this._isShowering;
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06003DC5 RID: 15813 RVA: 0x0016F534 File Offset: 0x0016D734
		public bool IsSoapEnvironmentActive
		{
			get
			{
				return this._soapEnvironmentActive;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06003DC6 RID: 15814 RVA: 0x0016F53C File Offset: 0x0016D73C
		public AudioSource RuntimeAudioSource
		{
			get
			{
				return this.audioSource;
			}
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x0016F544 File Offset: 0x0016D744
		private void Reset()
		{
			this.SetWaterEmitting(false);
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x0016F550 File Offset: 0x0016D750
		private void Awake()
		{
			this._wetOwner = string.Format("ShowerInteraction:{0}", base.GetInstanceID());
			this._interactionZone = base.GetComponent<Collider>();
			if (!this.HasAnyWater())
			{
				Transform transform = ((base.transform.parent != null) ? base.transform.parent : base.transform);
				this.ResolveNearestShowerParticles(transform);
			}
			if (this.promptUI == null)
			{
				Transform transform2 = ((base.transform.parent != null) ? base.transform.parent : base.transform);
				this.promptUI = transform2.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponent<AudioSource>();
			}
			if (this.audioSource == null)
			{
				this.audioSource = base.gameObject.AddComponent<AudioSource>();
			}
			this.audioSource.playOnAwake = false;
			this.audioSource.loop = true;
			this.audioSource.spatialBlend = 1f;
			this.audioSource.dopplerLevel = 0f;
			this.audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
			this.audioSource.minDistance = Mathf.Max(0.1f, this.showerMinDistance);
			this.audioSource.maxDistance = Mathf.Max(this.audioSource.minDistance + 0.1f, this.showerMaxDistance);
			if (this.audioSource.clip == null)
			{
				this.audioSource.clip = this.showerLoopClip;
			}
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x0016F6E0 File Offset: 0x0016D8E0
		private bool HasAnyWater()
		{
			if (this.waterParticles == null)
			{
				return false;
			}
			ParticleSystem[] array = this.waterParticles;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x0016F71C File Offset: 0x0016D91C
		private void ResolveNearestShowerParticles(Transform owner)
		{
			ParticleSystem particleSystem = null;
			ParticleSystem particleSystem2 = null;
			float num = float.MaxValue;
			float num2 = float.MaxValue;
			foreach (ParticleSystem particleSystem3 in owner.GetComponentsInChildren<ParticleSystem>(true))
			{
				if (!(particleSystem3 == null))
				{
					float sqrMagnitude = (particleSystem3.transform.position - base.transform.position).sqrMagnitude;
					if (particleSystem3.gameObject.name == "ShowerWater" && sqrMagnitude < num)
					{
						particleSystem = particleSystem3;
						num = sqrMagnitude;
					}
					else if (particleSystem3.gameObject.name == "ShowerSteam" && sqrMagnitude < num2)
					{
						particleSystem2 = particleSystem3;
						num2 = sqrMagnitude;
					}
				}
			}
			if (particleSystem != null && particleSystem2 != null)
			{
				this.waterParticles = new ParticleSystem[] { particleSystem, particleSystem2 };
				return;
			}
			if (particleSystem != null)
			{
				this.waterParticles = new ParticleSystem[] { particleSystem };
				return;
			}
			if (particleSystem2 != null)
			{
				this.waterParticles = new ParticleSystem[] { particleSystem2 };
				return;
			}
			this.waterParticles = new ParticleSystem[0];
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x0016F842 File Offset: 0x0016DA42
		private void Start()
		{
			this.SetWaterEmitting(false);
			if (this.promptUI != null)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x0016F86C File Offset: 0x0016DA6C
		private void Update()
		{
			if (this._playerInRange == null || this.interactInput == null)
			{
				return;
			}
			this._candidateSqrDistance = (((this._interactionZone != null) ? this._interactionZone.bounds.center : base.transform.position) - this._playerInRange.transform.position).sqrMagnitude;
			BoobTouchModeController boobTouchModeController = this.ResolveSoapModeController();
			if (boobTouchModeController != null && boobTouchModeController.IsActive)
			{
				if (!this._soapModeObserved)
				{
					this._soapModeObserved = true;
					this.BeginSoapModeEnvironment();
				}
				this.ResetPendingInteraction();
				this.HidePrompt();
				return;
			}
			if (this._soapModeObserved)
			{
				this._soapModeObserved = false;
				this.EndSoapModeEnvironment();
				this.RefreshPrompt();
			}
			if (this._suppressInteractUntilRelease)
			{
				if (this.interactInput.GetButtonUp() || !this.interactInput.GetButton())
				{
					this._suppressInteractUntilRelease = false;
				}
				return;
			}
			if (this._isShowering)
			{
				this.ResetPendingInteraction();
				if (this.interactInput.GetButtonDown() && InteractionCandidateRegistry.TryConsumeContextInput(this.interactInput))
				{
					this.StopShower();
				}
				return;
			}
			if (!InteractionCandidateRegistry.IsWinner(this))
			{
				this.ResetPendingInteraction();
				this.HidePrompt();
				return;
			}
			this.RefreshPrompt();
			if (!this.enableSoapModeOnHold || boobTouchModeController == null)
			{
				this.ResetPendingInteraction();
				if (this.interactInput.GetButtonDown() && InteractionCandidateRegistry.TryConsumeContextInput(this.interactInput))
				{
					this.StartShower();
				}
				return;
			}
			bool buttonDown = this.interactInput.GetButtonDown();
			bool button = this.interactInput.GetButton();
			bool buttonUp = this.interactInput.GetButtonUp();
			if (buttonDown && InteractionCandidateRegistry.TryConsumeContextInput(this.interactInput))
			{
				this._interactPressPending = true;
				this._interactPressStartedAt = Time.unscaledTime;
			}
			if (!this._interactPressPending)
			{
				return;
			}
			if (Time.unscaledTime - this._interactPressStartedAt < Mathf.Max(0.1f, this.soapHoldDuration) || (!button && !buttonUp))
			{
				if (buttonUp)
				{
					this.ResetPendingInteraction();
					if (InteractionCandidateRegistry.TryConsumeContextInput(this.interactInput))
					{
						this.StartShower();
					}
				}
				return;
			}
			this.ResetPendingInteraction();
			this._suppressInteractUntilRelease = true;
			this.HidePrompt();
			if (!InteractionCandidateRegistry.TryConsumeContextInput(this.interactInput))
			{
				return;
			}
			if (!this.TryStartSoapMode(boobTouchModeController))
			{
				this.RefreshPrompt();
			}
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x0016FAA8 File Offset: 0x0016DCA8
		private void OnTriggerEnter(Collider other)
		{
			GameObject gameObject;
			if (!this.IsPlayer(other, out gameObject))
			{
				return;
			}
			this._playerColliderCount++;
			this._playerInRange = gameObject;
			this.RefreshPrompt();
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x0016FADC File Offset: 0x0016DCDC
		private void OnTriggerExit(Collider other)
		{
			GameObject gameObject;
			if (!this.IsPlayer(other, out gameObject))
			{
				return;
			}
			if (this._playerInRange != null && gameObject != this._playerInRange)
			{
				return;
			}
			if (this._playerColliderCount > 0)
			{
				this._playerColliderCount--;
			}
			if (this._playerColliderCount > 0)
			{
				return;
			}
			this._playerColliderCount = 0;
			this.ResetInteractionState();
			if (this._isShowering)
			{
				this.StopShower();
			}
			this.EndSoapModeEnvironment();
			this._playerInRange = null;
			this._soapModeController = null;
			this._clothing = null;
			this._soapModeObserved = false;
			this.HidePrompt();
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x0016FB78 File Offset: 0x0016DD78
		private bool IsPlayer(Collider other, out GameObject playerRoot)
		{
			playerRoot = null;
			if (other == null)
			{
				return false;
			}
			Transform player = PlayerRegistry.Player;
			if (player != null)
			{
				if (other.transform == player || other.transform.IsChildOf(player))
				{
					playerRoot = player.gameObject;
					return true;
				}
				return false;
			}
			else
			{
				Transform root = other.transform.root;
				if (root.CompareTag(this.playerTag))
				{
					playerRoot = root.gameObject;
					return true;
				}
				return false;
			}
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x0016FBF0 File Offset: 0x0016DDF0
		private void RefreshPrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			BoobTouchModeController boobTouchModeController = this.ResolveSoapModeController();
			if (this._isShowering || (boobTouchModeController != null && boobTouchModeController.IsActive) || !InteractionCandidateRegistry.IsPromptWinner(this))
			{
				if (this._promptShown)
				{
					this.promptUI.Hide();
					this._promptShown = false;
				}
				return;
			}
			if (!this._promptShown)
			{
				InteractionConfigSO interactionConfigSO = ((this.enableSoapModeOnHold && boobTouchModeController != null && this.configSoapStart != null) ? this.configSoapStart : this.configStart);
				if (interactionConfigSO != null)
				{
					this.promptUI.Show(interactionConfigSO);
				}
				else
				{
					this.promptUI.gameObject.SetActive(true);
				}
				this._promptShown = true;
				this._promptShownIsShowering = false;
			}
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x0016FCBB File Offset: 0x0016DEBB
		private void HidePrompt()
		{
			if (this.promptUI != null && this._promptShown)
			{
				this.promptUI.Hide();
			}
			this._promptShown = false;
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x0016FCE5 File Offset: 0x0016DEE5
		private BoobTouchModeController ResolveSoapModeController()
		{
			if (!this.enableSoapModeOnHold || this._playerInRange == null)
			{
				return null;
			}
			if (this._soapModeController == null)
			{
				this._soapModeController = this._playerInRange.GetComponentInChildren<BoobTouchModeController>(true);
			}
			return this._soapModeController;
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x0016FD25 File Offset: 0x0016DF25
		public bool TryStartSoapMode()
		{
			return this.TryStartSoapMode(this.ResolveSoapModeController());
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x0016FD34 File Offset: 0x0016DF34
		private bool TryStartSoapMode(BoobTouchModeController soapMode)
		{
			if (soapMode == null)
			{
				return false;
			}
			if (this.BlockIfDressed())
			{
				return false;
			}
			this.SnapPlayerToSoapAnchor();
			if (!soapMode.TryEnterMode())
			{
				this.RestorePlayerFromSoapAnchor();
				return false;
			}
			this._soapModeObserved = true;
			this.HidePrompt();
			this.BeginSoapModeEnvironment();
			return true;
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x0016FD80 File Offset: 0x0016DF80
		private void ResetPendingInteraction()
		{
			this._interactPressPending = false;
			this._interactPressStartedAt = 0f;
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x0016FD94 File Offset: 0x0016DF94
		private void ResetInteractionState()
		{
			this.ResetPendingInteraction();
			this._suppressInteractUntilRelease = false;
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x0016FDA4 File Offset: 0x0016DFA4
		private bool BlockIfDressed()
		{
			if (this._playerInRange == null)
			{
				return false;
			}
			if (this._clothing == null)
			{
				this._clothing = this._playerInRange.GetComponentInChildren<ClothingToggleController>(true);
			}
			if (this._clothing == null || !this._clothing.IsWearingAnything(true))
			{
				return false;
			}
			BlockedActionFeedback.Show("You need to be naked to take a shower");
			return true;
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x0016FE0C File Offset: 0x0016E00C
		public void StartShower()
		{
			if (this._isShowering || this._playerInRange == null)
			{
				return;
			}
			if (this.BlockIfDressed())
			{
				return;
			}
			if (!this.TryAcquireShowerContext())
			{
				BlockedActionFeedback.Show("Stop what you're doing first");
				return;
			}
			this.ResetInteractionState();
			this.EndSoapModeEnvironment();
			if (!this.ResolvePlayerComponents(this._playerInRange))
			{
				Debug.LogError("[ShowerInteraction] Could not resolve ShowerBubbleSpawner / PlayerActionAnimator on the player.", this);
				this.ReleaseShowerContext();
				return;
			}
			this._isShowering = true;
			this.HidePrompt();
			this.ShowStopHud();
			if (this.bubbleSpawner != null)
			{
				this.bubbleSpawner.ResetToFull();
			}
			this.SnapPlayerToSoapAnchor();
			this.BeginShowerView();
			this.LockPlayerControl(true);
			this.SetWaterEmitting(true);
			this.SetShowerAudio(true);
			this.SetWetEffects(true);
			if (this.forceShowerFace)
			{
				this.ApplyBaseFace(this._playerInRange);
			}
			this.StartShowerAnimation();
			UnityEvent onShowerStarted = this.OnShowerStarted;
			if (onShowerStarted != null)
			{
				onShowerStarted.Invoke();
			}
			this._routine = base.StartCoroutine(this.ShowerRoutine());
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x0016FF0C File Offset: 0x0016E10C
		public void StopShower()
		{
			if (!this._isShowering)
			{
				this.ReleaseShowerContext();
				return;
			}
			this.ResetInteractionState();
			this._isShowering = false;
			if (this._routine != null)
			{
				base.StopCoroutine(this._routine);
				this._routine = null;
			}
			this.StopShowerAnimation();
			if (this.bubbleSpawner != null)
			{
				this.bubbleSpawner.BeginDissolve();
			}
			this.SetWaterEmitting(false);
			this.SetShowerAudio(false);
			if (this.bubbleSpawner == null || !this.bubbleSpawner.BubblesActive)
			{
				this.SetWetEffects(false);
			}
			this.RestoreFace();
			this.LockPlayerControl(false);
			this.RestorePlayerFromSoapAnchor();
			this.HideStopHud();
			if (this._playerInRange != null)
			{
				this.RefreshPrompt();
			}
			UnityEvent onShowerEnded = this.OnShowerEnded;
			if (onShowerEnded != null)
			{
				onShowerEnded.Invoke();
			}
			this.ReleaseShowerContext();
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x0016FFE4 File Offset: 0x0016E1E4
		private bool TryAcquireShowerContext()
		{
			if (this._showerLease != null)
			{
				if (this._showerLease.IsActive)
				{
					return true;
				}
				this._showerLease = null;
			}
			if (!PlayerActivityGate.TryAcquire("Shower#" + base.GetInstanceID().ToString(), this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._showerLease, false, null))
			{
				return false;
			}
			InteractionCandidateRegistry.MarkInteract();
			ShowerInteraction.ActiveActions.Add(this);
			return true;
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x00170054 File Offset: 0x0016E254
		private void BeginShowerView()
		{
			this._showerFirstPerson = ((this._playerInRange != null) ? this._playerInRange.GetComponentInChildren<FirstPersonController>(true) : null);
			FirstPersonController showerFirstPerson = this._showerFirstPerson;
			if (showerFirstPerson == null)
			{
				return;
			}
			showerFirstPerson.BeginInteractionView(this, true, 65f, false, true, null, null, null);
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x001700B0 File Offset: 0x0016E2B0
		private void ReleaseShowerContext()
		{
			FirstPersonController showerFirstPerson = this._showerFirstPerson;
			if (showerFirstPerson != null)
			{
				showerFirstPerson.EndInteractionView(this);
			}
			this._showerFirstPerson = null;
			PlayerActivityLease showerLease = this._showerLease;
			this._showerLease = null;
			if (showerLease != null)
			{
				showerLease.Dispose();
			}
			if (!this._soapEnvironmentActive)
			{
				ShowerInteraction.ActiveActions.Remove(this);
			}
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x00170102 File Offset: 0x0016E302
		private void ShowStopHud()
		{
			if (this.configStop == null)
			{
				return;
			}
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Show(this.configStop);
			}
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x00170130 File Offset: 0x0016E330
		private void HideStopHud()
		{
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Hide();
			}
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x0017014C File Offset: 0x0016E34C
		private void SetWetEffects(bool on)
		{
			if (this._wetLingerRoutine != null)
			{
				base.StopCoroutine(this._wetLingerRoutine);
				this._wetLingerRoutine = null;
			}
			if (on)
			{
				if (this._wetEffectsActive)
				{
					return;
				}
				this._wetEffectsActive = true;
				if (this._wetController != null)
				{
					this._wetController.AcquireWet(this._wetOwner);
				}
				if (this._skinDecalController != null && this.sweatDecal != null)
				{
					if (this._decalFadeRoutine != null)
					{
						base.StopCoroutine(this._decalFadeRoutine);
					}
					this._skinDecalController.AddDecal(this.sweatDecal, null, new float?(0f));
					this._skinDecalController.SetDecalActive(this.sweatDecal, true);
					this._decalFadeRoutine = base.StartCoroutine(this.FadeDecal(0f, this.sweatDecalPeakOpacity, false));
				}
				if (this.bubbleSpawner != null)
				{
					if (this._subscribedSpawner != null)
					{
						this._subscribedSpawner.OnBubblesGone -= this.HandleBubblesGone;
					}
					this.bubbleSpawner.OnBubblesGone += this.HandleBubblesGone;
					this._subscribedSpawner = this.bubbleSpawner;
					return;
				}
			}
			else
			{
				if (!this._wetEffectsActive)
				{
					return;
				}
				this._wetEffectsActive = false;
				if (this._subscribedSpawner != null)
				{
					this._subscribedSpawner.OnBubblesGone -= this.HandleBubblesGone;
					this._subscribedSpawner = null;
				}
				if (this._wetController != null)
				{
					this._wetController.ReleaseWet(this._wetOwner);
				}
				if (this._skinDecalController != null && this.sweatDecal != null)
				{
					if (this._decalFadeRoutine != null)
					{
						base.StopCoroutine(this._decalFadeRoutine);
					}
					this._decalFadeRoutine = base.StartCoroutine(this.FadeDecal(this.sweatDecalPeakOpacity, 0f, true));
				}
			}
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00170330 File Offset: 0x0016E530
		private IEnumerator FadeDecal(float from, float to, bool removeAtEnd)
		{
			float fadeTime = Mathf.Max(0f, this.sweatDecalFadeDuration);
			if (fadeTime > 0f && this._skinDecalController != null && this.sweatDecal != null)
			{
				float elapsed = 0f;
				while (elapsed < fadeTime)
				{
					elapsed += Time.deltaTime;
					float num = Mathf.Clamp01(elapsed / fadeTime);
					this._skinDecalController.SetDecalOpacity(this.sweatDecal, Mathf.Lerp(from, to, num));
					yield return null;
				}
			}
			if (this._skinDecalController != null && this.sweatDecal != null)
			{
				this._skinDecalController.SetDecalOpacity(this.sweatDecal, to);
			}
			if (removeAtEnd && this._skinDecalController != null && this.sweatDecal != null)
			{
				this._skinDecalController.RemoveDecal(this.sweatDecal);
			}
			this._decalFadeRoutine = null;
			yield break;
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x00170354 File Offset: 0x0016E554
		private void HandleBubblesGone()
		{
			if (this._wetLingerRoutine != null)
			{
				base.StopCoroutine(this._wetLingerRoutine);
			}
			this._wetLingerRoutine = base.StartCoroutine(this.LingerThenEndWetEffects());
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x0017037C File Offset: 0x0016E57C
		private IEnumerator LingerThenEndWetEffects()
		{
			float num = Mathf.Max(0f, this.wetEffectsExtraSeconds);
			if (num > 0f)
			{
				yield return new WaitForSeconds(num);
			}
			this._wetLingerRoutine = null;
			this.SetWetEffects(false);
			yield break;
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x0017038C File Offset: 0x0016E58C
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this._candidateSqrDistance = float.MaxValue;
			if (this._isShowering)
			{
				this.StopShower();
			}
			else
			{
				this.ReleaseShowerContext();
			}
			this.ResetInteractionState();
			this._soapModeObserved = false;
			this.EndSoapModeEnvironment();
			this.HidePrompt();
			if (this._wetEffectsActive)
			{
				this.SetWetEffects(false);
			}
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x001703E8 File Offset: 0x0016E5E8
		private void StartShowerAnimation()
		{
			if (this._actionAnimator == null || this.showerTriggerPrefab == null)
			{
				Debug.LogError("[ShowerInteraction] Missing PlayerActionAnimator or showerTriggerPrefab.", this);
				return;
			}
			int num = this.PickVariant();
			this.showerTriggerPrefab.playAnimation = this.showerStatePrefix + num.ToString();
			this._actionAnimator.TriggerAction(new PlayerActionEntry
			{
				ActionId = this.actionId,
				DisplayName = "Shower",
				Duration = 0f,
				TriggerPrefab = this.showerTriggerPrefab
			});
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x00170480 File Offset: 0x0016E680
		private void StopShowerAnimation()
		{
			if (this._actionAnimator == null)
			{
				return;
			}
			this._actionAnimator.FinishCurrentAction();
			this._actionAnimator.RestoreOverrideLayer();
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x001704A8 File Offset: 0x0016E6A8
		private int PickVariant()
		{
			if (this.showerVariantCount <= 1)
			{
				this._lastVariant = 1;
				return 1;
			}
			int num = Random.Range(1, this.showerVariantCount + 1);
			if (this.avoidImmediateRepeat && num == this._lastVariant)
			{
				num = num % this.showerVariantCount + 1;
			}
			this._lastVariant = num;
			return num;
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x001704FA File Offset: 0x0016E6FA
		private void LateUpdate()
		{
			if (this._faceOverridden)
			{
				this.ApplyBaseFaceFrame();
			}
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0017050C File Offset: 0x0016E70C
		private void SetShowerAudio(bool on)
		{
			if (this.audioSource == null)
			{
				return;
			}
			float num = this.showerVolume * (this._soapEnvironmentActive ? this.soapModeAudioScale : 1f);
			if (on)
			{
				if (this.showerStartClip != null)
				{
					this.audioSource.PlayOneShot(this.showerStartClip, num);
				}
				if (this.audioSource.clip == null)
				{
					this.audioSource.clip = this.showerLoopClip;
				}
				if (this.audioSource.clip == null)
				{
					return;
				}
				this.audioSource.volume = num;
				this.audioSource.loop = true;
				if (!this.audioSource.isPlaying)
				{
					this.audioSource.Play();
					return;
				}
			}
			else
			{
				this.audioSource.Stop();
				if (this.showerStopClip != null)
				{
					this.audioSource.PlayOneShot(this.showerStopClip, num);
				}
			}
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x00170600 File Offset: 0x0016E800
		private void ApplyBaseFace(GameObject player)
		{
			this._faceAnimator = player.GetComponent<Animator>();
			this._emotionController = ShowerInteraction.FindComponentByName(player, "PlayerEmotionController");
			if (this._emotionController != null)
			{
				ShowerInteraction.InvokeNoArg(this._emotionController, "DisableEmotionLayersForAction");
				ShowerInteraction.InvokeNoArg(this._emotionController, "DisableFacialExpressionForAction");
				this._faceParamName = ShowerInteraction.GetStringMember(this._emotionController, "ExpressionParamName", "FaceExpressionIndex");
			}
			if (this._faceAnimator != null && !string.IsNullOrEmpty(this._faceParamName))
			{
				this._savedFaceIndex = this._faceAnimator.GetInteger(this._faceParamName);
				this._faceAnimator.SetInteger(this._faceParamName, this.showerFaceIndex);
			}
			this._faceOverridden = true;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x001706C5 File Offset: 0x0016E8C5
		private void ApplyBaseFaceFrame()
		{
			if (this._faceAnimator != null && !string.IsNullOrEmpty(this._faceParamName))
			{
				this._faceAnimator.SetInteger(this._faceParamName, this.showerFaceIndex);
			}
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x001706FC File Offset: 0x0016E8FC
		private void RestoreFace()
		{
			if (!this._faceOverridden)
			{
				return;
			}
			if (this._faceAnimator != null && !string.IsNullOrEmpty(this._faceParamName))
			{
				this._faceAnimator.SetInteger(this._faceParamName, this._savedFaceIndex);
			}
			if (this._emotionController != null)
			{
				ShowerInteraction.InvokeNoArg(this._emotionController, "RestoreEmotionLayers");
				ShowerInteraction.InvokeNoArg(this._emotionController, "RestoreFacialExpression");
			}
			this._faceAnimator = null;
			this._faceOverridden = false;
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x00170782 File Offset: 0x0016E982
		private IEnumerator ShowerRoutine()
		{
			if (this.bubbleDelay > 0f)
			{
				yield return new WaitForSeconds(this.bubbleDelay);
			}
			if (this.bubbleSpawner != null)
			{
				this.bubbleSpawner.ShowBubbles();
			}
			UnityEvent onBubblesAppeared = this.OnBubblesAppeared;
			if (onBubblesAppeared != null)
			{
				onBubblesAppeared.Invoke();
			}
			this._routine = null;
			yield break;
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x00170794 File Offset: 0x0016E994
		private bool ResolvePlayerComponents(GameObject player)
		{
			if (this.bubbleSpawner == null)
			{
				this.bubbleSpawner = player.GetComponentInChildren<ShowerBubbleSpawner>(true);
			}
			if (this._actionAnimator == null)
			{
				this._actionAnimator = player.GetComponentInChildren<PlayerActionAnimator>();
			}
			if (this._playerAnimator == null)
			{
				this._playerAnimator = player.GetComponent<Animator>();
			}
			if (this._wetController == null)
			{
				this._wetController = player.GetComponentInChildren<CielWetController>(true);
			}
			if (this._skinDecalController == null)
			{
				this._skinDecalController = player.GetComponentInChildren<SkinDecalController>(true);
			}
			return this.bubbleSpawner != null && this._actionAnimator != null;
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x00170844 File Offset: 0x0016EA44
		private void SetWaterEmitting(bool on)
		{
			if (this.waterParticles == null)
			{
				return;
			}
			foreach (ParticleSystem particleSystem in this.waterParticles)
			{
				if (!(particleSystem == null))
				{
					if (on)
					{
						particleSystem.Play(true);
					}
					else
					{
						particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
					}
				}
			}
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x00170890 File Offset: 0x0016EA90
		private void BeginSoapModeEnvironment()
		{
			if (this._soapEnvironmentActive)
			{
				return;
			}
			this._soapEnvironmentActive = true;
			ShowerInteraction.ActiveActions.Add(this);
			this.SetWaterEmitting(true);
			this.SetShowerAudio(true);
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x001708BC File Offset: 0x0016EABC
		private void EndSoapModeEnvironment()
		{
			if (!this._soapEnvironmentActive)
			{
				return;
			}
			this._soapEnvironmentActive = false;
			this.SetWaterEmitting(false);
			this.SetShowerAudio(false);
			this.RestorePlayerFromSoapAnchor();
			if (!this._isShowering)
			{
				ShowerInteraction.ActiveActions.Remove(this);
			}
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x001708F8 File Offset: 0x0016EAF8
		private void SnapPlayerToSoapAnchor()
		{
			if (this._playerInRange == null || this.soapModeAnchor == null)
			{
				return;
			}
			Transform transform = this._playerInRange.transform;
			if (!this._hasSoapReturnPose)
			{
				this._soapReturnPosition = transform.position;
				this._soapReturnRotation = transform.rotation;
				this._hasSoapReturnPose = true;
			}
			this.SetPlayerPose(transform, this.soapModeAnchor.position, this.soapModeAnchor.rotation);
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x00170972 File Offset: 0x0016EB72
		private void RestorePlayerFromSoapAnchor()
		{
			if (!this._hasSoapReturnPose)
			{
				return;
			}
			this._hasSoapReturnPose = false;
			if (this._playerInRange == null)
			{
				return;
			}
			this.SetPlayerPose(this._playerInRange.transform, this._soapReturnPosition, this._soapReturnRotation);
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x001709B0 File Offset: 0x0016EBB0
		private void SetPlayerPose(Transform playerTransform, Vector3 position, Quaternion rotation)
		{
			if (playerTransform == null)
			{
				return;
			}
			Rigidbody component = playerTransform.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.velocity = Vector3.zero;
				component.angularVelocity = Vector3.zero;
				component.position = position;
				component.rotation = rotation;
			}
			playerTransform.SetPositionAndRotation(position, rotation);
			Physics.SyncTransforms();
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x00170A08 File Offset: 0x0016EC08
		private void LockPlayerControl(bool locked)
		{
			if (this._playerInRange != null)
			{
				this.SetInvectorLock(this._playerInRange, locked);
			}
			if (!locked)
			{
				foreach (Behaviour behaviour in this._disabled)
				{
					if (behaviour != null)
					{
						behaviour.enabled = true;
					}
				}
				this._disabled.Clear();
				return;
			}
			this._disabled.Clear();
			if (this._playerInRange == null || this.controllersToDisable == null)
			{
				return;
			}
			foreach (string text in this.controllersToDisable)
			{
				foreach (MonoBehaviour monoBehaviour in this._playerInRange.GetComponents<MonoBehaviour>())
				{
					if (!(monoBehaviour == null) && !(monoBehaviour.GetType().Name != text) && monoBehaviour.enabled)
					{
						monoBehaviour.enabled = false;
						this._disabled.Add(monoBehaviour);
					}
				}
			}
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x00170B30 File Offset: 0x0016ED30
		private void SetInvectorLock(GameObject player, bool locked)
		{
			foreach (MonoBehaviour monoBehaviour in player.GetComponents<MonoBehaviour>())
			{
				if (!(monoBehaviour == null))
				{
					string name = monoBehaviour.GetType().Name;
					if (name == "vThirdPersonController")
					{
						ShowerInteraction.SetBoolMember(monoBehaviour, "lockMovement", locked);
						ShowerInteraction.SetBoolMember(monoBehaviour, "lockRotation", locked);
					}
					else if (name == "vThirdPersonInput")
					{
						ShowerInteraction.SetBoolMember(monoBehaviour, "lockMoveInput", locked);
					}
				}
			}
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x00170BAC File Offset: 0x0016EDAC
		private static void SetBoolMember(object obj, string name, bool value)
		{
			Type type = obj.GetType();
			PropertyInfo property = type.GetProperty(name);
			if (property != null && property.CanWrite)
			{
				property.SetValue(obj, value);
				return;
			}
			FieldInfo field = type.GetField(name);
			if (field != null)
			{
				field.SetValue(obj, value);
			}
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x00170C08 File Offset: 0x0016EE08
		private static Component FindComponentByName(GameObject go, string typeName)
		{
			foreach (Component component in go.GetComponents<Component>())
			{
				if (component != null && component.GetType().Name == typeName)
				{
					return component;
				}
			}
			return null;
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x00170C50 File Offset: 0x0016EE50
		private static bool InvokeNoArg(object obj, string method)
		{
			MethodInfo method2 = obj.GetType().GetMethod(method, Type.EmptyTypes);
			if (method2 == null)
			{
				return false;
			}
			method2.Invoke(obj, null);
			return true;
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x00170C84 File Offset: 0x0016EE84
		private static bool InvokeOneInt(object obj, string method, int arg)
		{
			MethodInfo method2 = obj.GetType().GetMethod(method, new Type[] { typeof(int) });
			if (method2 == null)
			{
				return false;
			}
			method2.Invoke(obj, new object[] { arg });
			return true;
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x00170CD4 File Offset: 0x0016EED4
		private static int GetIntMember(object obj, string name, int fallback)
		{
			Type type = obj.GetType();
			PropertyInfo property = type.GetProperty(name);
			if (property != null && property.PropertyType == typeof(int))
			{
				return (int)property.GetValue(obj);
			}
			FieldInfo field = type.GetField(name);
			if (field != null && field.FieldType == typeof(int))
			{
				return (int)field.GetValue(obj);
			}
			return fallback;
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x00170D54 File Offset: 0x0016EF54
		private static string GetStringMember(object obj, string name, string fallback)
		{
			Type type = obj.GetType();
			PropertyInfo property = type.GetProperty(name);
			if (property != null && property.PropertyType == typeof(string))
			{
				string text = (string)property.GetValue(obj);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
				return fallback;
			}
			else
			{
				FieldInfo field = type.GetField(name);
				if (!(field != null) || !(field.FieldType == typeof(string)))
				{
					return fallback;
				}
				string text2 = (string)field.GetValue(obj);
				if (!string.IsNullOrEmpty(text2))
				{
					return text2;
				}
				return fallback;
			}
		}

		// Token: 0x04003DDD RID: 15837
		private static readonly HashSet<ShowerInteraction> ActiveActions = new HashSet<ShowerInteraction>();

		// Token: 0x04003DDE RID: 15838
		[Header("Trigger")]
		[Tooltip("Tag the player GameObject must have to be detected.")]
		[SerializeField]
		private string playerTag = "Player";

		// Token: 0x04003DDF RID: 15839
		[Tooltip("Generic input (keyboard / joystick / mobile) the player presses to start/stop the shower. Same input type used across the game's interactions.")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("F", "RT", "F");

		// Token: 0x04003DE0 RID: 15840
		[Tooltip("How close (in metres) the player must be to this object to use the shower. Detection is by distance, not physics, so it never blocks movement.")]
		[SerializeField]
		private float interactRange = 1.6f;

		// Token: 0x04003DE1 RID: 15841
		[Header("Water")]
		[Tooltip("Particle systems for the running water. If left empty, all child ParticleSystems of this object are used automatically.")]
		[SerializeField]
		private ParticleSystem[] waterParticles;

		// Token: 0x04003DE2 RID: 15842
		[Header("Timing")]
		[Tooltip("Seconds after the shower starts before the soap bubbles appear. The shower itself runs until the player presses the key again.")]
		[SerializeField]
		private float bubbleDelay = 5f;

		// Token: 0x04003DE3 RID: 15843
		[Header("References")]
		[Tooltip("If left empty, this is looked up on the player when it enters the trigger.")]
		[SerializeField]
		private ShowerBubbleSpawner bubbleSpawner;

		// Token: 0x04003DE4 RID: 15844
		[Header("Shower Animation (Animator / Invector action)")]
		[Tooltip("vTriggerGenericAction prefab that plays the shower animation via the Animator (ShowerEmote.prefab: playAnimation=ShowerEntry, endActionManualy). Required.")]
		[SerializeField]
		private vTriggerGenericAction showerTriggerPrefab;

		// Token: 0x04003DE5 RID: 15845
		[Tooltip("How many shower variant states exist in the Animator (ShowerAnim_1..N).")]
		[SerializeField]
		private int showerVariantCount = 5;

		// Token: 0x04003DE6 RID: 15846
		[Tooltip("Prefix of the Animator state names for the shower variants. The code sets the trigger prefab's playAnimation to <prefix><variant> so Invector CrossFades DIRECTLY into that state (real blend, no ShowerEntry).")]
		[SerializeField]
		private string showerStatePrefix = "ShowerAnim_";

		// Token: 0x04003DE7 RID: 15847
		[Tooltip("Logical action id for the shower (passed to PlayerActionAnimator).")]
		[SerializeField]
		private string actionId = "Shower";

		// Token: 0x04003DE8 RID: 15848
		[Tooltip("Avoid replaying the same variant two showers in a row (when >1 variant).")]
		[SerializeField]
		private bool avoidImmediateRepeat = true;

		// Token: 0x04003DE9 RID: 15849
		[Header("Prompt UI")]
		[Tooltip("Shared interaction prompt (same as vehicles/dildo). If null, found in children. Shown when the player is in range.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04003DEA RID: 15850
		[Tooltip("Config shown by the prompt while the shower is OFF (objectName / key / action).")]
		[SerializeField]
		private InteractionConfigSO configStart;

		// Token: 0x04003DEB RID: 15851
		[Tooltip("Config shown by the prompt while the shower is RUNNING (e.g. action = 'Smetti').")]
		[SerializeField]
		private InteractionConfigSO configStop;

		// Token: 0x04003DEC RID: 15852
		[Header("Soap Mode (optional hold interaction)")]
		[Tooltip("When enabled, a short interaction starts the shower while holding the same input enters the soap/body-touch mode. Keep disabled on showers that should retain their original click-only behaviour.")]
		[SerializeField]
		private bool enableSoapModeOnHold;

		// Token: 0x04003DED RID: 15853
		[Tooltip("Unscaled seconds the interaction input must stay pressed to enter soap mode.")]
		[SerializeField]
		[Min(0.1f)]
		private float soapHoldDuration = 0.6f;

		// Token: 0x04003DEE RID: 15854
		[Tooltip("Prompt config with the extra HOLD F soap row. Falls back to configStart when null.")]
		[SerializeField]
		private InteractionConfigSO configSoapStart;

		// Token: 0x04003DEF RID: 15855
		[Tooltip("Optional exact standing point used when soap mode starts. The home-shower setup places this anchor on the floor inside the bathtub.")]
		[SerializeField]
		private Transform soapModeAnchor;

		// Token: 0x04003DF0 RID: 15856
		[Header("Audio")]
		[Tooltip("Looping shower water sound. Plays on a loop while the shower runs. Drop an AudioClip here to give the shower a sound.")]
		[SerializeField]
		private AudioClip showerLoopClip;

		// Token: 0x04003DF1 RID: 15857
		[Tooltip("Optional one-shot sound played once when the shower STARTS (e.g. a tap/valve squeak). Leave empty for none.")]
		[SerializeField]
		private AudioClip showerStartClip;

		// Token: 0x04003DF2 RID: 15858
		[Tooltip("Optional one-shot sound played once when the shower STOPS. Leave empty for none.")]
		[SerializeField]
		private AudioClip showerStopClip;

		// Token: 0x04003DF3 RID: 15859
		[Tooltip("AudioSource used for the looping shower sound. If null, one is created on this object.")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003DF4 RID: 15860
		[Range(0f, 1f)]
		[SerializeField]
		private float showerVolume = 0.18f;

		// Token: 0x04003DF5 RID: 15861
		[Tooltip("Scala del volume del loop acqua durante il SOAP MODE. La camera orbitale sta a ~0.3-0.8m dal soffione (molto piu' vicina della camera della doccia classica), quindi senza compensazione l'acqua suona nettamente piu' forte: questo fattore pareggia la resa percepita con la doccia senza soap. 1 = off.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float soapModeAudioScale = 0.35f;

		// Token: 0x04003DF6 RID: 15862
		[Tooltip("Distanza alla quale il loop acqua si sente a volume pieno.")]
		[SerializeField]
		[Min(0.1f)]
		private float showerMinDistance = 0.75f;

		// Token: 0x04003DF7 RID: 15863
		[Tooltip("Distanza oltre la quale il loop acqua non e' piu' udibile.")]
		[SerializeField]
		[Min(0.2f)]
		private float showerMaxDistance = 6f;

		// Token: 0x04003DF8 RID: 15864
		[Header("Face")]
		[Tooltip("Force the dedicated Shower face expression while showering (ShowerFace.anim, on the Face layer, with eyes closed), restoring the normal face afterwards.")]
		[SerializeField]
		private bool forceShowerFace = true;

		// Token: 0x04003DF9 RID: 15865
		[Tooltip("FaceExpressionIndex value that activates the Shower face in the Animator's Face layer.")]
		[SerializeField]
		private int showerFaceIndex = 7;

		// Token: 0x04003DFA RID: 15866
		[Header("Wet Effects (reflection + sweat decal)")]
		[Tooltip("Sweat skin decal activated while Ciel is wet (e.g. 'Sweat Heavy'). Assign the SkinDecal asset. Active from shower start until the bubbles are gone.")]
		[SerializeField]
		private SkinDecal sweatDecal;

		// Token: 0x04003DFB RID: 15867
		[Tooltip("Opacity the sweat decal fades up to when wet (its peak).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float sweatDecalPeakOpacity = 1f;

		// Token: 0x04003DFC RID: 15868
		[Tooltip("Seconds for the sweat decal to fade IN (0 → peak) and fade OUT (peak → 0), same gradual approach as the art-gallery tint.")]
		[SerializeField]
		private float sweatDecalFadeDuration = 5f;

		// Token: 0x04003DFD RID: 15869
		[Tooltip("Extra seconds the wet effects (reflection + sweat decal) linger AFTER the bubbles are gone, before they start fading out. The wet look outlasts the bubbles.")]
		[SerializeField]
		private float wetEffectsExtraSeconds = 20f;

		// Token: 0x04003DFE RID: 15870
		[Header("Player Control Lock")]
		[Tooltip("MonoBehaviours on the player to disable while showering. LEAVE EMPTY to keep full control (incl. camera) during the shower. NOTE: disabling vThirdPersonInput also freezes the camera — only add it if you really want the player locked in place.")]
		[SerializeField]
		private string[] controllersToDisable = new string[0];

		// Token: 0x04003DFF RID: 15871
		[Header("Events")]
		public UnityEvent OnShowerStarted;

		// Token: 0x04003E00 RID: 15872
		public UnityEvent OnBubblesAppeared;

		// Token: 0x04003E01 RID: 15873
		public UnityEvent OnShowerEnded;

		// Token: 0x04003E02 RID: 15874
		private GameObject _playerInRange;

		// Token: 0x04003E03 RID: 15875
		private Transform _player;

		// Token: 0x04003E04 RID: 15876
		private ClothingToggleController _clothing;

		// Token: 0x04003E05 RID: 15877
		private bool _isShowering;

		// Token: 0x04003E06 RID: 15878
		private FirstPersonController _showerFirstPerson;

		// Token: 0x04003E07 RID: 15879
		private bool _promptShown;

		// Token: 0x04003E08 RID: 15880
		private bool _promptShownIsShowering;

		// Token: 0x04003E09 RID: 15881
		private Coroutine _routine;

		// Token: 0x04003E0A RID: 15882
		private PlayerActivityLease _showerLease;

		// Token: 0x04003E0B RID: 15883
		private BoobTouchModeController _soapModeController;

		// Token: 0x04003E0C RID: 15884
		private bool _interactPressPending;

		// Token: 0x04003E0D RID: 15885
		private bool _suppressInteractUntilRelease;

		// Token: 0x04003E0E RID: 15886
		private bool _soapModeObserved;

		// Token: 0x04003E0F RID: 15887
		private bool _soapEnvironmentActive;

		// Token: 0x04003E10 RID: 15888
		private bool _hasSoapReturnPose;

		// Token: 0x04003E11 RID: 15889
		private Vector3 _soapReturnPosition;

		// Token: 0x04003E12 RID: 15890
		private Quaternion _soapReturnRotation;

		// Token: 0x04003E13 RID: 15891
		private float _interactPressStartedAt;

		// Token: 0x04003E14 RID: 15892
		private int _playerColliderCount;

		// Token: 0x04003E15 RID: 15893
		private float _candidateSqrDistance = float.MaxValue;

		// Token: 0x04003E16 RID: 15894
		private Collider _interactionZone;

		// Token: 0x04003E17 RID: 15895
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x04003E18 RID: 15896
		private Animator _playerAnimator;

		// Token: 0x04003E19 RID: 15897
		private int _lastVariant = -1;

		// Token: 0x04003E1A RID: 15898
		private CielWetController _wetController;

		// Token: 0x04003E1B RID: 15899
		private SkinDecalController _skinDecalController;

		// Token: 0x04003E1C RID: 15900
		private bool _wetEffectsActive;

		// Token: 0x04003E1D RID: 15901
		private ShowerBubbleSpawner _subscribedSpawner;

		// Token: 0x04003E1E RID: 15902
		private Coroutine _decalFadeRoutine;

		// Token: 0x04003E1F RID: 15903
		private Coroutine _wetLingerRoutine;

		// Token: 0x04003E20 RID: 15904
		private string _wetOwner;

		// Token: 0x04003E21 RID: 15905
		private Component _emotionController;

		// Token: 0x04003E22 RID: 15906
		private bool _faceOverridden;

		// Token: 0x04003E23 RID: 15907
		private Animator _faceAnimator;

		// Token: 0x04003E24 RID: 15908
		private int _savedFaceIndex;

		// Token: 0x04003E25 RID: 15909
		private string _faceParamName = "FaceExpressionIndex";

		// Token: 0x04003E26 RID: 15910
		private readonly List<Behaviour> _disabled = new List<Behaviour>();

		// Token: 0x04003E27 RID: 15911
		private const PlayerContextResource ShowerResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;
	}
}
