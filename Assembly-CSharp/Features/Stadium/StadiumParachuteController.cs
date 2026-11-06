using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Flash;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.MainMenu;
using Features.Missions;
using Features.PhoneCompanion;
using Invector;
using Invector.vCamera;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Features.Stadium
{
	// Token: 0x02000C43 RID: 3139
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(120)]
	public sealed class StadiumParachuteController : MonoBehaviour
	{
		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06005A89 RID: 23177 RVA: 0x00236F21 File Offset: 0x00235121
		// (set) Token: 0x06005A8A RID: 23178 RVA: 0x00236F29 File Offset: 0x00235129
		public bool IsFlying { get; private set; }

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06005A8B RID: 23179 RVA: 0x00236F32 File Offset: 0x00235132
		// (set) Token: 0x06005A8C RID: 23180 RVA: 0x00236F3A File Offset: 0x0023513A
		public bool IsLanding { get; private set; }

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06005A8D RID: 23181 RVA: 0x00236F43 File Offset: 0x00235143
		// (set) Token: 0x06005A8E RID: 23182 RVA: 0x00236F4B File Offset: 0x0023514B
		public float FlightTime { get; private set; }

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06005A8F RID: 23183 RVA: 0x00236F54 File Offset: 0x00235154
		// (set) Token: 0x06005A90 RID: 23184 RVA: 0x00236F5C File Offset: 0x0023515C
		public StadiumParachuteZone Zone { get; private set; }

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06005A91 RID: 23185 RVA: 0x00236F65 File Offset: 0x00235165
		public StadiumParachuteProfile Profile
		{
			get
			{
				if (!(this.Zone != null))
				{
					return null;
				}
				return this.Zone.profile;
			}
		}

		// Token: 0x06005A92 RID: 23186 RVA: 0x00236F84 File Offset: 0x00235184
		internal bool IsDepartureContextAvailable()
		{
			return !GameplayInputGate.BlocksGameplayInput && !TextInputFocus.BlocksGameplayInput && PlayerActivityGate.CanStartWorldInteraction && Time.timeScale > 0f && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI) && (this._inventory == null || this._inventory.ActiveClothIndex < 0) && !ClothActionGate.IsBusy && !SmartphoneController.IsAnyPhoneOut && WallCoverActivityLock.ActiveController == null;
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06005A93 RID: 23187 RVA: 0x00236FF4 File Offset: 0x002351F4
		private bool CanPrepareDeparture
		{
			get
			{
				return this.IsEquipped && this.Profile != null && this.Profile.animationController != null && this.Zone.canopyPrefab != null && this._motor != null && this._motor.enabled && !this._motor.isDead && !this._motor.ragdolled && !this._motor.customAction && this._input != null && this._input.enabled && !this._input.lockInput && this._body != null && !this._body.isKinematic && this._inventory != null && Time.time >= this._nextDeploy && this.IsDepartureContextAvailable();
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06005A94 RID: 23188 RVA: 0x002370F7 File Offset: 0x002352F7
		public bool CanApproachLaunchPoint
		{
			get
			{
				return !this.IsFlying && !this.IsLanding && this.CanPrepareDeparture;
			}
		}

		// Token: 0x06005A95 RID: 23189 RVA: 0x00237114 File Offset: 0x00235314
		private void Awake()
		{
			this._motor = base.GetComponent<vThirdPersonController>();
			this._input = base.GetComponent<vThirdPersonInput>();
			this._body = base.GetComponent<Rigidbody>();
			this._animator = ((this._motor != null) ? this._motor.animator : base.GetComponentInChildren<Animator>());
			this._inventory = base.GetComponentInChildren<PlayerInventoryManager>(true);
			this._wearables = base.GetComponentInChildren<WearableUseController>(true);
			this._firstPerson = base.GetComponent<FirstPersonController>();
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x00237194 File Offset: 0x00235394
		public void Bind(StadiumParachuteZone zone)
		{
			if (this.Zone != zone)
			{
				this.EndFlight(false);
				this._departure.Reset();
			}
			this.Zone = zone;
			if (zone != null)
			{
				ControllerContextBindings.SetLabel(zone.flightHud, "A / D", "LS", false);
				ControllerContextBindings.SetLabel(zone.flightHud, "S", "LS Down", false);
				ControllerContextBindings.SetLabel(zone.flightHud, "W", "LS Up", false);
				ControllerContextBindings.SetLabel(zone.flightHud, "V", "RS", false);
			}
			base.enabled = true;
		}

		// Token: 0x06005A97 RID: 23191 RVA: 0x00237230 File Offset: 0x00235430
		public void Unbind(StadiumParachuteZone zone)
		{
			if (this.Zone != zone)
			{
				return;
			}
			this.EndFlight(false);
			this.HideHud();
			this.Zone = null;
			base.enabled = false;
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06005A98 RID: 23192 RVA: 0x0023725C File Offset: 0x0023545C
		public bool IsEquipped
		{
			get
			{
				if (this.Zone != null)
				{
					WearableItemSO wearableItemSO = this.Zone.item as WearableItemSO;
					if (wearableItemSO != null && this._wearables != null)
					{
						return this._wearables.IsWorn(wearableItemSO.itemId, true);
					}
				}
				return false;
			}
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x002372B0 File Offset: 0x002354B0
		public bool CanDeploy()
		{
			if (this.IsFlying || this.IsLanding || !this.CanPrepareDeparture || !this.Zone.Contains(base.transform.position) || this._motor.isGrounded || this._body.velocity.y >= -0.35f || !this._departure.CanDepart(base.transform.position, Time.time))
			{
				return false;
			}
			Vector3 vector;
			float num = this.GroundClearance(30f, out vector);
			return num > this.Profile.minimumClearance && base.transform.position.y - num < this._departure.Point.transform.position.y - this.Profile.departureDrop;
		}

		// Token: 0x06005A9A RID: 23194 RVA: 0x00237388 File Offset: 0x00235588
		private void Update()
		{
			if (!this.IsFlying && !this.IsLanding)
			{
				bool canPrepareDeparture = this.CanPrepareDeparture;
				Vector3 vector;
				bool flag = canPrepareDeparture && this._motor.isGrounded && this.GroundClearance(0.5f, out vector) < 0.2f;
				this._departure.Observe(this.Zone, base.transform.position, flag, canPrepareDeparture, Time.time);
				this.PresentHud(null);
				this.TryDeploy();
				return;
			}
			if (this.Zone == null || !this.IsEquipped || !this.Zone.Contains(base.transform.position) || this._motor == null || this._motor.isDead || this._motor.ragdolled || this._lease == null || !this._lease.IsActive || this._body.isKinematic)
			{
				this.EndFlight(false);
				return;
			}
			this.PresentHud(this.Zone.flightHud);
			if (Time.timeScale <= 0f)
			{
				return;
			}
			bool flag2 = !GameplayInputGate.BlocksGameplayInput && !TextInputFocus.BlocksGameplayInput && !FlyCamera.IsActive;
			this._targetTurn = ((flag2 && !this.IsLanding) ? GameplayInput.GetAxisRaw("Horizontal") : 0f);
			this._targetVertical = ((flag2 && !this.IsLanding) ? GameplayInput.GetAxisRaw("Vertical") : 0f);
			if (flag2 && GameplayInput.GetKeyDown(KeyCode.V) && this._firstPerson != null)
			{
				this._firstPerson.ToggleFirstPerson();
			}
			if (this.IsLanding)
			{
				this._landingTime += Time.deltaTime;
				if (this._landingTime >= this.Profile.landingDuration)
				{
					this.EndFlight(true);
				}
			}
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x0023755C File Offset: 0x0023575C
		public bool TryDeploy()
		{
			if (!this.CanDeploy() || !this.BeginMotion())
			{
				return false;
			}
			if (this._departure.Point.gate != null)
			{
				this._departure.Point.gate.NotifyDeparture();
			}
			this._departure.Reset();
			this.IsFlying = true;
			this._body.useGravity = false;
			this.FlightTime = 0f;
			this._heading = base.transform.eulerAngles.y;
			this._speed = Mathf.Clamp(Vector3.ProjectOnPlane(this._body.velocity, Vector3.up).magnitude, this.Profile.brakeSpeed, this.Profile.forwardSpeed);
			this._sink = Mathf.Clamp(-this._body.velocity.y, 0.35f, 5f);
			this._turn = (this._brake = (this._verticalInput = (this._targetTurn = (this._targetVertical = (this._turnRate = 0f)))));
			if (this.Zone.canopyPrefab != null)
			{
				this._visual = Object.Instantiate<StadiumParachuteVisual>(this.Zone.canopyPrefab, base.transform);
				this._visual.transform.localPosition = Vector3.zero;
				this._visual.transform.localRotation = Quaternion.identity;
				this._visual.Bind(this._animator, this.Profile);
				this._visual.Pose(0f, 0f, 0f);
			}
			this.BuildPose();
			this.BeginFlightCamera();
			if (this._flightAnimation.IsValid<AnimatorControllerPlayable>())
			{
				this._flightAnimation.Play("Open", 0, 0f);
				this._flightAnimation.SetSpeed(1.0);
			}
			this._audio = base.gameObject.AddComponent<AudioSource>();
			this._audio.playOnAwake = false;
			this._audio.spatialBlend = 0f;
			this._audio.volume = 0f;
			GameAudioRouter.Route(this._audio, GameAudioRouter.Channel.Sfx);
			if (this.Zone.windSound != null)
			{
				this._audio.clip = this.Zone.windSound;
				this._audio.loop = true;
				this._audio.Play();
			}
			this._deploymentAudio = base.gameObject.AddComponent<AudioSource>();
			this._deploymentAudio.playOnAwake = false;
			this._deploymentAudio.spatialBlend = 0f;
			GameAudioRouter.Route(this._deploymentAudio, GameAudioRouter.Channel.Sfx);
			if (this.Zone.openingSound != null)
			{
				this._deploymentAudio.PlayOneShot(this.Zone.openingSound, this.Profile.openingVolume);
			}
			MissionGameplaySignals.NotifyFirstFlight();
			return true;
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x0023784C File Offset: 0x00235A4C
		private bool BeginMotion()
		{
			if (!PlayerActivityGate.TryAcquire("StadiumParachute", this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose, out this._lease, false, null))
			{
				return false;
			}
			this._motorEnabled = this._motor.enabled;
			this._inputEnabled = this._input.enabled;
			this._gravity = this._body.useGravity;
			this._drag = this._body.drag;
			this._collisionMode = this._body.collisionDetectionMode;
			this._rootMotion = this._animator != null && this._animator.applyRootMotion;
			this._finishedLanding = false;
			this._suspendedItem = this._inventory.GetActiveItem();
			this._suspendedSlot = ((this._suspendedItem != null) ? this._inventory.ActiveSlotIndex : (-1));
			if (this._suspendedItem != null)
			{
				this._inventory.SuspendActiveItemImmediate();
			}
			this._inventory.AcquireSlotInputLock(this);
			this._input.enabled = false;
			this._motor.enabled = false;
			this._body.useGravity = false;
			this._body.drag = 0f;
			this._body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			if (this._animator != null)
			{
				this._animator.applyRootMotion = false;
			}
			return true;
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x002379AC File Offset: 0x00235BAC
		private void BeginFlightCamera()
		{
			if (this._firstPerson != null)
			{
				this._firstPerson.BeginInteractionView(this, false, this.Profile.lookYawRange, false, true, base.transform, new Vector3?(this.Profile.eyeOffset), new Vector2?(this.Profile.lookPitchRange));
				this._firstPerson.OnExitFirstPerson.AddListener(new UnityAction(this.OnExitFirstPerson));
			}
			this._camera = this._input.tpCamera;
			if (this._camera == null || this._camera.CameraStateList == null || this._camera.lerpState == null)
			{
				return;
			}
			this._originalCameraList = this._camera.CameraStateList;
			this._previousCameraState = ((this._firstPerson != null && this._firstPerson.IsFirstPerson) ? this._firstPerson.ReturnCameraState : this._camera.currentStateName);
			if (string.IsNullOrEmpty(this._previousCameraState))
			{
				this._previousCameraState = "Default";
			}
			this._flightCameraList = ScriptableObject.CreateInstance<vThirdPersonCameraListData>();
			this._flightCameraList.tpCameraStates = new List<vThirdPersonCameraState>(this._originalCameraList.tpCameraStates);
			vThirdPersonCameraState vThirdPersonCameraState;
			if ((vThirdPersonCameraState = this._originalCameraList.tpCameraStates.Find((vThirdPersonCameraState s) => s.Name == this._previousCameraState)) == null)
			{
				vThirdPersonCameraState = this._originalCameraList.tpCameraStates.Find((vThirdPersonCameraState s) => s.Name == "Default");
			}
			vThirdPersonCameraState vThirdPersonCameraState2 = vThirdPersonCameraState;
			if (vThirdPersonCameraState2 == null)
			{
				vThirdPersonCameraState2 = new vThirdPersonCameraState("Default")
				{
					forward = -1f
				};
			}
			vThirdPersonCameraState vThirdPersonCameraState3 = StadiumParachuteController.CreateFlightCameraState(vThirdPersonCameraState2, this.Profile);
			this._flightCameraList.tpCameraStates.Add(vThirdPersonCameraState3);
			this._camera.CameraStateList = this._flightCameraList;
			if (this._firstPerson == null || !this._firstPerson.IsFirstPerson)
			{
				this._camera.ChangeState(vThirdPersonCameraState3.Name, true);
			}
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x00237BB0 File Offset: 0x00235DB0
		internal static vThirdPersonCameraState CreateFlightCameraState(vThirdPersonCameraState source, StadiumParachuteProfile profile)
		{
			vThirdPersonCameraState vThirdPersonCameraState = JsonUtility.FromJson<vThirdPersonCameraState>(JsonUtility.ToJson(source));
			vThirdPersonCameraState.Name = "StadiumParachute";
			vThirdPersonCameraState.minDistance = Mathf.Max(0.5f, profile.cameraMinDistance);
			vThirdPersonCameraState.maxDistance = Mathf.Max(vThirdPersonCameraState.minDistance, profile.cameraMaxDistance);
			vThirdPersonCameraState.defaultDistance = Mathf.Clamp(profile.cameraDistance, vThirdPersonCameraState.minDistance, vThirdPersonCameraState.maxDistance);
			vThirdPersonCameraState.height = profile.cameraHeight;
			vThirdPersonCameraState.useZoom = true;
			vThirdPersonCameraState.fov = profile.cameraFov;
			return vThirdPersonCameraState;
		}

		// Token: 0x06005A9F RID: 23199 RVA: 0x00237C3E File Offset: 0x00235E3E
		private void OnExitFirstPerson()
		{
			if ((this.IsFlying || this.IsLanding) && this._camera != null && this._flightCameraList != null)
			{
				this._camera.ChangeState("StadiumParachute", true);
			}
		}

		// Token: 0x06005AA0 RID: 23200 RVA: 0x00237C80 File Offset: 0x00235E80
		private void RestoreCamera()
		{
			if (this._firstPerson != null)
			{
				this._firstPerson.OnExitFirstPerson.RemoveListener(new UnityAction(this.OnExitFirstPerson));
				this._firstPerson.EndInteractionView(this);
				this._firstPerson.ReplaceInteractionReturnCamera("StadiumParachute", this._previousCameraState ?? "Default");
			}
			if (this._camera != null && this._originalCameraList != null && this._camera.CameraStateList == this._flightCameraList)
			{
				this._camera.CameraStateList = this._originalCameraList;
				if (this._firstPerson == null || !this._firstPerson.IsFirstPerson)
				{
					this._camera.ChangeState(this._previousCameraState, true);
				}
			}
			if (this._flightCameraList != null)
			{
				Object.Destroy(this._flightCameraList);
			}
			this._flightCameraList = null;
			this._originalCameraList = null;
			this._camera = null;
		}

		// Token: 0x06005AA1 RID: 23201 RVA: 0x00237D84 File Offset: 0x00235F84
		private void BuildPose()
		{
			if (this._animator == null || this.Profile.animationController == null)
			{
				return;
			}
			this._graph = PlayableGraph.Create("Stadium parachute pose");
			this._flightAnimation = AnimatorControllerPlayable.Create(this._graph, this.Profile.animationController);
			this._poseMixer = AnimationMixerPlayable.Create(this._graph, 2);
			if (this._animator.runtimeAnimatorController != null)
			{
				AnimatorControllerPlayable animatorControllerPlayable = AnimatorControllerPlayable.Create(this._graph, this._animator.runtimeAnimatorController);
				this._graph.Connect<AnimatorControllerPlayable, AnimationMixerPlayable>(animatorControllerPlayable, 0, this._poseMixer, 0);
				this._poseMixer.SetInputWeight(0, 1f);
			}
			this._graph.Connect<AnimatorControllerPlayable, AnimationMixerPlayable>(this._flightAnimation, 0, this._poseMixer, 1);
			AnimationPlayableOutput.Create(this._graph, "Parachute", this._animator).SetSourcePlayable(this._poseMixer);
			this._graph.Play();
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x00237E88 File Offset: 0x00236088
		private void FixedUpdate()
		{
			if (!this.IsFlying || this._body == null || this._body.isKinematic)
			{
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.FlightTime += fixedDeltaTime;
			float num = 1f - Mathf.Exp(-fixedDeltaTime / Mathf.Max(0.02f, this.Profile.inputSmoothing));
			this._turn = Mathf.Lerp(this._turn, this._targetTurn, num);
			this._verticalInput = Mathf.Lerp(this._verticalInput, this._targetVertical, num);
			this._brake = Mathf.Clamp01(-this._verticalInput);
			Vector3 vector;
			float num2 = this.GroundClearance(6f, out vector);
			if (this.FlightTime > 0.2f && num2 < 0.13f && vector.y > 0.55f)
			{
				this.BeginLanding();
				return;
			}
			float num3 = Mathf.Clamp01((this.Profile.flareHeight - num2) / Mathf.Max(0.1f, this.Profile.flareHeight - 0.2f));
			float num4 = Mathf.Max(this._brake, num3);
			float num5 = this._turn * Mathf.Lerp(this.Profile.turnSpeed, this.Profile.brakingTurnSpeed, num4);
			this._turnRate = Mathf.MoveTowards(this._turnRate, num5, this.Profile.turnAcceleration * fixedDeltaTime);
			this._heading += this._turnRate * fixedDeltaTime;
			Vector2 vector2 = this.Profile.SpeedForInput(this._verticalInput);
			vector2 = Vector2.Lerp(vector2, new Vector2(this.Profile.brakeSpeed, this.Profile.brakeSink), num3);
			this._speed = Mathf.MoveTowards(this._speed, vector2.x, this.Profile.acceleration * fixedDeltaTime);
			this._sink = Mathf.MoveTowards(this._sink, vector2.y, this.Profile.verticalAcceleration * fixedDeltaTime);
			Quaternion quaternion = Quaternion.Euler(0f, this._heading, 0f);
			this._body.MoveRotation(quaternion);
			this._body.velocity = quaternion * Vector3.forward * this._speed + Vector3.down * this._sink;
			this._motor.heightReached = base.transform.position.y;
			this._motor.verticalVelocity = -this._sink;
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x00238110 File Offset: 0x00236310
		public float GroundClearance(float distance, out Vector3 normal)
		{
			float num = float.PositiveInfinity;
			normal = Vector3.up;
			int num2 = Physics.SphereCastNonAlloc(base.transform.position + Vector3.up * 0.45f, 0.18f, Vector3.down, this._hits, distance + 0.45f, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num2; i++)
			{
				RaycastHit raycastHit = this._hits[i];
				if (!raycastHit.transform.IsChildOf(base.transform) && raycastHit.normal.y >= 0.4f)
				{
					float num3 = Mathf.Max(0f, raycastHit.distance - 0.27f);
					if (num3 < num)
					{
						num = num3;
						normal = raycastHit.normal;
					}
				}
			}
			return num;
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x002381DC File Offset: 0x002363DC
		private void LateUpdate()
		{
			if (!this.IsFlying && !this.IsLanding)
			{
				return;
			}
			if (this._input != null && !GameplayInputGate.BlocksGameplayInput && !TextInputFocus.BlocksGameplayInput && !FlyCamera.IsActive && Time.timeScale > 0f)
			{
				this._input.CameraInput();
			}
			float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(this.FlightTime / this.Profile.openingDuration));
			if (this._visual != null && this.IsFlying)
			{
				this._visual.Pose(num, this._turn, this._brake);
			}
			if (this._flightAnimation.IsValid<AnimatorControllerPlayable>())
			{
				this._flightAnimation.SetFloat("InputHorizontal", this._turn);
				this._flightAnimation.SetFloat("InputVertical", this._verticalInput);
			}
			float num2 = (this.IsLanding ? (1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(this.Profile.landingDuration * 0.55f, this.Profile.landingDuration, this._landingTime))) : num);
			if (this._poseMixer.IsValid<AnimationMixerPlayable>())
			{
				this._poseMixer.SetInputWeight(0, 1f - num2);
				this._poseMixer.SetInputWeight(1, num2);
			}
			if (this._audio != null)
			{
				float num3 = this.Profile.windVolume * (this.IsLanding ? num2 : Mathf.Lerp(0.65f, 1f, this._speed / this.Profile.forwardSpeed));
				this._audio.volume = Mathf.MoveTowards(this._audio.volume, num3, Time.deltaTime * this.Profile.windVolume / 0.35f);
			}
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x002383B4 File Offset: 0x002365B4
		private void OnCollisionEnter(Collision collision)
		{
			if (!this.IsFlying || this.FlightTime < 0.15f)
			{
				return;
			}
			for (int i = 0; i < collision.contactCount; i++)
			{
				if (collision.GetContact(i).normal.y > 0.55f)
				{
					this.BeginLanding();
					return;
				}
			}
			this._speed = Mathf.Min(this._speed, 1f);
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x00238420 File Offset: 0x00236620
		private void BeginLanding()
		{
			if (!this.IsFlying)
			{
				return;
			}
			this.IsFlying = false;
			this.IsLanding = true;
			this._landingTime = 0f;
			this._finishedLanding = true;
			this._body.useGravity = this._gravity;
			this._body.velocity = Vector3.zero;
			if (this._flightAnimation.IsValid<AnimatorControllerPlayable>())
			{
				this._flightAnimation.CrossFadeInFixedTime("Land", 0.12f);
			}
			if (this._visual != null)
			{
				this._visual.Retire();
			}
			if (this.Zone.landingSound != null && this._deploymentAudio != null)
			{
				this._deploymentAudio.PlayOneShot(this.Zone.landingSound, 0.45f);
			}
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x002384F0 File Offset: 0x002366F0
		public void EndFlight(bool landed = false)
		{
			this._departure.Reset();
			if (!this.IsFlying && !this.IsLanding)
			{
				return;
			}
			FlyCamera.StopForActivityLease(this._lease);
			landed |= this._finishedLanding;
			this.IsFlying = false;
			this.IsLanding = false;
			this._nextDeploy = Time.time + 0.75f;
			if (this._graph.IsValid())
			{
				this._graph.Destroy();
			}
			if (this._visual != null)
			{
				this._visual.gameObject.SetActive(false);
				Object.Destroy(this._visual.gameObject);
				this._visual = null;
			}
			if (this._audio != null)
			{
				this._audio.Stop();
				Object.Destroy(this._audio);
			}
			if (this._deploymentAudio != null)
			{
				this._deploymentAudio.Stop();
				Object.Destroy(this._deploymentAudio);
			}
			if (this._animator != null)
			{
				this._animator.applyRootMotion = this._rootMotion;
			}
			this.RestoreCamera();
			if (this._body != null)
			{
				this._body.useGravity = this._gravity;
				this._body.drag = this._drag;
				this._body.collisionDetectionMode = this._collisionMode;
				if (!this._body.isKinematic && landed)
				{
					this._body.velocity = Vector3.zero;
				}
			}
			if (this._motor != null)
			{
				this._motor.heightReached = base.transform.position.y;
				this._motor.verticalVelocity = 0f;
				this._motor.isJumping = false;
				this._motor.input = (this._motor.moveDirection = Vector3.zero);
				this._motor.enabled = this._motorEnabled;
			}
			if (this._input != null)
			{
				this._input.enabled = this._inputEnabled;
			}
			if (this._inventory != null)
			{
				this._inventory.ReleaseSlotInputLock(this);
			}
			PlayerActivityLease lease = this._lease;
			if (lease != null)
			{
				lease.Dispose();
			}
			this._lease = null;
			this.RestoreHeldItem();
			this.HideHud();
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x0023873C File Offset: 0x0023693C
		private void RestoreHeldItem()
		{
			InventoryItemSO suspendedItem = this._suspendedItem;
			int suspendedSlot = this._suspendedSlot;
			this._suspendedItem = null;
			this._suspendedSlot = -1;
			if (suspendedItem == null || this._inventory == null || this._inventory.IsSlotInputLocked || (this._motor != null && this._motor.isDead) || this._inventory.ActiveSlotIndex != suspendedSlot || this._inventory.GetActiveSlotItem() != suspendedItem || PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem))
			{
				return;
			}
			this._inventory.ResumeActiveItem();
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x002387DC File Offset: 0x002369DC
		private void PresentHud(InteractionConfigSO config)
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (this._hud != instance)
			{
				this.HideHud();
				this._hud = instance;
			}
			if (this._hud == null)
			{
				return;
			}
			if (config == null)
			{
				this._hud.HideSupplemental(this);
				return;
			}
			this._hud.ShowSupplemental(this, config);
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x0023883C File Offset: 0x00236A3C
		private void HideHud()
		{
			if (this._hud != null)
			{
				this._hud.HideSupplemental(this);
			}
			this._hud = null;
		}

		// Token: 0x06005AAB RID: 23211 RVA: 0x0023885F File Offset: 0x00236A5F
		private void OnEnable()
		{
			if (this._wearables != null)
			{
				this._wearables.OnItemUnworn += this.HandleEquipmentRemoved;
			}
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x00238886 File Offset: 0x00236A86
		private void HandleEquipmentRemoved(WearableItemSO item)
		{
			if (this.Zone == null || item != this.Zone.item)
			{
				return;
			}
			this.EndFlight(false);
			this.HideHud();
			this._departure.Reset();
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x002388C2 File Offset: 0x00236AC2
		private void OnDisable()
		{
			if (this._wearables != null)
			{
				this._wearables.OnItemUnworn -= this.HandleEquipmentRemoved;
			}
			this.EndFlight(false);
			this.HideHud();
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x002388F6 File Offset: 0x00236AF6
		private void OnDestroy()
		{
			this.EndFlight(false);
			this.HideHud();
		}

		// Token: 0x04005B7F RID: 23423
		private vThirdPersonController _motor;

		// Token: 0x04005B80 RID: 23424
		private vThirdPersonInput _input;

		// Token: 0x04005B81 RID: 23425
		private Rigidbody _body;

		// Token: 0x04005B82 RID: 23426
		private Animator _animator;

		// Token: 0x04005B83 RID: 23427
		private PlayerInventoryManager _inventory;

		// Token: 0x04005B84 RID: 23428
		private WearableUseController _wearables;

		// Token: 0x04005B85 RID: 23429
		private InventoryItemSO _suspendedItem;

		// Token: 0x04005B86 RID: 23430
		private int _suspendedSlot = -1;

		// Token: 0x04005B87 RID: 23431
		private StadiumParachuteVisual _visual;

		// Token: 0x04005B88 RID: 23432
		private PlayerActivityLease _lease;

		// Token: 0x04005B89 RID: 23433
		private InteractionHUDController _hud;

		// Token: 0x04005B8A RID: 23434
		private AudioSource _audio;

		// Token: 0x04005B8B RID: 23435
		private AudioSource _deploymentAudio;

		// Token: 0x04005B8C RID: 23436
		private PlayableGraph _graph;

		// Token: 0x04005B8D RID: 23437
		private AnimationMixerPlayable _poseMixer;

		// Token: 0x04005B8E RID: 23438
		private AnimatorControllerPlayable _flightAnimation;

		// Token: 0x04005B8F RID: 23439
		private FirstPersonController _firstPerson;

		// Token: 0x04005B90 RID: 23440
		private bool _motorEnabled;

		// Token: 0x04005B91 RID: 23441
		private bool _inputEnabled;

		// Token: 0x04005B92 RID: 23442
		private bool _gravity;

		// Token: 0x04005B93 RID: 23443
		private bool _rootMotion;

		// Token: 0x04005B94 RID: 23444
		private float _drag;

		// Token: 0x04005B95 RID: 23445
		private float _nextDeploy;

		// Token: 0x04005B96 RID: 23446
		private float _heading;

		// Token: 0x04005B97 RID: 23447
		private float _speed;

		// Token: 0x04005B98 RID: 23448
		private float _sink;

		// Token: 0x04005B99 RID: 23449
		private float _turn;

		// Token: 0x04005B9A RID: 23450
		private float _brake;

		// Token: 0x04005B9B RID: 23451
		private float _verticalInput;

		// Token: 0x04005B9C RID: 23452
		private float _targetTurn;

		// Token: 0x04005B9D RID: 23453
		private float _targetVertical;

		// Token: 0x04005B9E RID: 23454
		private float _turnRate;

		// Token: 0x04005B9F RID: 23455
		private float _landingTime;

		// Token: 0x04005BA0 RID: 23456
		private bool _finishedLanding;

		// Token: 0x04005BA1 RID: 23457
		private CollisionDetectionMode _collisionMode;

		// Token: 0x04005BA2 RID: 23458
		private vThirdPersonCamera _camera;

		// Token: 0x04005BA3 RID: 23459
		private vThirdPersonCameraListData _originalCameraList;

		// Token: 0x04005BA4 RID: 23460
		private vThirdPersonCameraListData _flightCameraList;

		// Token: 0x04005BA5 RID: 23461
		private string _previousCameraState;

		// Token: 0x04005BA6 RID: 23462
		private readonly RaycastHit[] _hits = new RaycastHit[32];

		// Token: 0x04005BA7 RID: 23463
		private readonly StadiumParachuteDeparture _departure = new StadiumParachuteDeparture();
	}
}
