using System;
using System.Collections.Generic;
using System.Globalization;
using Features.Vehicles.Spawning;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using RayznGames.VehicleSystem;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x020006F3 RID: 1779
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(31000)]
	public sealed class BicycleRideDiagnostics : MonoBehaviour
	{
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002D40 RID: 11584 RVA: 0x000F9AB6 File Offset: 0x000F7CB6
		// (set) Token: 0x06002D41 RID: 11585 RVA: 0x000F9AC0 File Offset: 0x000F7CC0
		public static bool RecordingEnabled
		{
			get
			{
				return BicycleRideDiagnostics.recordingEnabled;
			}
			set
			{
				if (BicycleRideDiagnostics.recordingEnabled == value)
				{
					return;
				}
				BicycleRideDiagnostics.recordingEnabled = value;
				Application.logMessageReceived -= BicycleRideDiagnostics.OnUnityLog;
				if (value)
				{
					Application.logMessageReceived += BicycleRideDiagnostics.OnUnityLog;
				}
				else
				{
					BicycleDiagnosticLog.Close();
				}
				if (!value || !Application.isPlaying)
				{
					return;
				}
				foreach (BicycleVehicle bicycleVehicle in Object.FindObjectsByType<BicycleVehicle>(FindObjectsInactive.Include, FindObjectsSortMode.None))
				{
					if (!(bicycleVehicle == null) && bicycleVehicle.gameObject.scene.IsValid())
					{
						vVehicleController component = bicycleVehicle.GetComponent<vVehicleController>();
						if (!(component != null) || component.Type == vVehicleController.VehicleType.Bicycle)
						{
							BicycleRideDiagnostics bicycleRideDiagnostics = bicycleVehicle.GetComponent<BicycleRideDiagnostics>();
							if (bicycleRideDiagnostics == null)
							{
								bicycleRideDiagnostics = bicycleVehicle.gameObject.AddComponent<BicycleRideDiagnostics>();
							}
							bicycleRideDiagnostics.Initialize();
							bicycleRideDiagnostics.nextSample = 0f;
							bicycleRideDiagnostics.watchUntil = Time.unscaledTime + 2f;
							if (bicycleRideDiagnostics.isActiveAndEnabled)
							{
								BicycleRideDiagnostics.Recorders.Add(bicycleRideDiagnostics);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000F9BC8 File Offset: 0x000F7DC8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetSession()
		{
			BicycleRideDiagnostics.Recorders.Clear();
			BicycleRideDiagnostics.MountsByRider.Clear();
			BicycleRideDiagnostics.LastBikeByRider.Clear();
			BicycleRideDiagnostics.BikesByRider.Clear();
			BicycleRideDiagnostics.reportingUnityError = false;
			BicycleRideDiagnostics.recordingEnabled = false;
			Application.logMessageReceived -= BicycleRideDiagnostics.OnUnityLog;
			if (BicycleRideDiagnostics.recordingEnabled)
			{
				Application.logMessageReceived += BicycleRideDiagnostics.OnUnityLog;
			}
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000F9C32 File Offset: 0x000F7E32
		public static void Attach(Component owner)
		{
			BicycleRideDiagnostics.Record(owner, "controller.awake", null, null);
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000F9C44 File Offset: 0x000F7E44
		public static void Record(Component owner, string phase, vThirdPersonController rider = null, string detail = null)
		{
			if (!BicycleRideDiagnostics.RecordingEnabled || !Application.isPlaying || owner == null)
			{
				return;
			}
			try
			{
				BicycleRideDiagnostics.RecordCore(owner, phase, rider, detail);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[BICI] Evento diagnostico non riuscito: " + ex.Message, owner);
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000F9CA0 File Offset: 0x000F7EA0
		private static void RecordCore(Component owner, string phase, vThirdPersonController rider, string detail)
		{
			vVehicleController component = owner.GetComponent<vVehicleController>();
			if (component != null && component.Type != vVehicleController.VehicleType.Bicycle)
			{
				return;
			}
			if (owner.GetComponent<BicycleVehicle>() == null)
			{
				return;
			}
			BicycleRideDiagnostics bicycleRideDiagnostics = owner.GetComponent<BicycleRideDiagnostics>();
			if (bicycleRideDiagnostics == null)
			{
				bicycleRideDiagnostics = owner.gameObject.AddComponent<BicycleRideDiagnostics>();
			}
			bicycleRideDiagnostics.Initialize();
			if (rider != null)
			{
				bicycleRideDiagnostics.BindRider(rider);
			}
			bicycleRideDiagnostics.phase = phase;
			bicycleRideDiagnostics.watchUntil = Time.unscaledTime + 5f;
			if (phase == "mount.complete" && rider != null)
			{
				bicycleRideDiagnostics.RecordRiderSequence(rider);
			}
			bicycleRideDiagnostics.Emit(phase, detail, true);
			if (phase == "mount.request")
			{
				bicycleRideDiagnostics.Configuration();
			}
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000F9D58 File Offset: 0x000F7F58
		public static void RecordIfAttached(Component owner, string phase, string detail)
		{
			if (!BicycleRideDiagnostics.RecordingEnabled || !Application.isPlaying || owner == null)
			{
				return;
			}
			BicycleRideDiagnostics component = owner.GetComponent<BicycleRideDiagnostics>();
			if (component != null)
			{
				component.Emit(phase, detail, false);
			}
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000F9D98 File Offset: 0x000F7F98
		public static void RecordSpawn(Component owner, GameObject prefab, string detail)
		{
			if (!BicycleRideDiagnostics.RecordingEnabled || !Application.isPlaying || owner == null)
			{
				return;
			}
			BicycleRideDiagnostics component = owner.GetComponent<BicycleRideDiagnostics>();
			if (component == null)
			{
				return;
			}
			component.sourcePrefab = ((prefab != null) ? prefab.name : "");
			component.Emit("spawn.complete", detail, true);
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000F9DF8 File Offset: 0x000F7FF8
		private void RecordRiderSequence(vThirdPersonController current)
		{
			int instanceID = current.GetInstanceID();
			int num;
			BicycleRideDiagnostics.MountsByRider.TryGetValue(instanceID, out num);
			this.rideNumber = (BicycleRideDiagnostics.MountsByRider[instanceID] = num + 1);
			BicycleRideDiagnostics.LastBikeByRider.TryGetValue(instanceID, out this.previousBikeId);
			BicycleRideDiagnostics.LastBikeByRider[instanceID] = this.bike.GetInstanceID();
			HashSet<int> hashSet;
			if (!BicycleRideDiagnostics.BikesByRider.TryGetValue(instanceID, out hashSet))
			{
				hashSet = (BicycleRideDiagnostics.BikesByRider[instanceID] = new HashSet<int>());
			}
			hashSet.Add(this.bike.GetInstanceID());
			this.distinctBikesUsed = hashSet.Count;
			foreach (BicycleRideDiagnostics bicycleRideDiagnostics in BicycleRideDiagnostics.Recorders)
			{
				if (!(bicycleRideDiagnostics == null) && !(bicycleRideDiagnostics == this) && !(bicycleRideDiagnostics.rider != current))
				{
					bool flag = bicycleRideDiagnostics.Mounted || (bicycleRideDiagnostics.driver != null && bicycleRideDiagnostics.driver.IsActive);
					bicycleRideDiagnostics.Emit(flag ? "ANOMALY.previous_bike_still_owns_rider" : "rider.moved_to_another_bike", "newBikeId=" + this.bike.GetInstanceID().ToString() + "; riderRideNumber=" + this.rideNumber.ToString(), true);
				}
			}
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000F9F78 File Offset: 0x000F8178
		public static void MarkProblem(string description = "Problema segnalato dal giocatore")
		{
			BicycleRideDiagnostics.RecordingEnabled = true;
			BicycleDiagnosticLog.Write(new BicycleRideDiagnostics.ProblemMarker
			{
				session = BicycleDiagnosticLog.SessionId,
				detail = description,
				frame = Time.frameCount,
				realtime = Time.unscaledTime,
				activeBikes = BicycleRideDiagnostics.Recorders.Count
			}, true);
			foreach (BicycleRideDiagnostics bicycleRideDiagnostics in BicycleRideDiagnostics.Recorders)
			{
				if (!(bicycleRideDiagnostics == null))
				{
					bicycleRideDiagnostics.Emit("USER_MARKER", description, true);
					bicycleRideDiagnostics.Configuration();
				}
			}
			BicycleDiagnosticLog.Flush();
			Debug.Log("[BICI] Segnalazione salvata. Cronologia: " + BicycleDiagnosticLog.CurrentPath);
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000FA044 File Offset: 0x000F8244
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x000FA04C File Offset: 0x000F824C
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.initialized = true;
			this.bike = base.GetComponent<BicycleVehicle>();
			this.standard = base.GetComponent<vVehicleController>();
			this.legacy = base.GetComponent<vBikeController>();
			this.body = base.GetComponent<Rigidbody>();
			this.driver = base.GetComponent<VehiclePoseDriver>();
			this.anchors = base.GetComponent<IVehicleAnchorProvider>();
			this.initializedInstanceId = base.GetInstanceID();
			this.initializedBikeId = ((this.bike != null) ? this.bike.GetInstanceID() : 0);
			this.initializedName = base.name;
			this.initializedHierarchy = BicycleRideDiagnostics.Path(base.transform);
			this.initializedScene = base.gameObject.scene.path;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000FA118 File Offset: 0x000F8318
		private void OnEnable()
		{
			if (!BicycleRideDiagnostics.RecordingEnabled || !Application.isPlaying)
			{
				return;
			}
			this.Initialize();
			BicycleRideDiagnostics.Recorders.Add(this);
			this.nextContactSample.Clear();
			this.watchUntil = Time.unscaledTime + 2f;
			this.Emit("enabled", null, false);
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000FA16F File Offset: 0x000F836F
		private void Start()
		{
			if (!BicycleRideDiagnostics.RecordingEnabled)
			{
				return;
			}
			this.Configuration();
			this.Emit("ready.after_awake", null, false);
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000FA18C File Offset: 0x000F838C
		private void OnDisable()
		{
			if (BicycleRideDiagnostics.RecordingEnabled && Application.isPlaying)
			{
				this.Emit("disabled", "activeSelf=" + base.gameObject.activeSelf.ToString(), true);
			}
			BicycleRideDiagnostics.Recorders.Remove(this);
			if (BicycleRideDiagnostics.RecordingEnabled)
			{
				BicycleDiagnosticLog.Flush();
			}
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000FA1E8 File Offset: 0x000F83E8
		private void OnDestroy()
		{
			BicycleRideDiagnostics.Recorders.Remove(this);
			if (!BicycleRideDiagnostics.RecordingEnabled || !Application.isPlaying)
			{
				return;
			}
			BicycleRideDiagnostics.LifecycleEvent lifecycleEvent = new BicycleRideDiagnostics.LifecycleEvent();
			lifecycleEvent.kind = "destroyed";
			lifecycleEvent.phase = this.phase;
			lifecycleEvent.session = BicycleDiagnosticLog.SessionId;
			lifecycleEvent.utc = DateTime.UtcNow.ToString("O");
			int num = this.sequence + 1;
			this.sequence = num;
			lifecycleEvent.sequence = num;
			lifecycleEvent.frame = Time.frameCount;
			lifecycleEvent.realtime = Time.unscaledTime;
			lifecycleEvent.instanceId = ((this.lastSafeSnapshot != null) ? this.lastSafeSnapshot.instanceId : this.initializedInstanceId);
			lifecycleEvent.bikeId = ((this.lastSafeSnapshot != null) ? this.lastSafeSnapshot.bikeId : this.initializedBikeId);
			lifecycleEvent.riderId = ((this.lastSafeSnapshot != null) ? this.lastSafeSnapshot.riderId : 0);
			lifecycleEvent.name = ((this.lastSafeSnapshot != null) ? this.lastSafeSnapshot.name : this.initializedName);
			lifecycleEvent.hierarchy = ((this.lastSafeSnapshot != null) ? this.lastSafeSnapshot.hierarchy : this.initializedHierarchy);
			lifecycleEvent.scene = ((this.lastSafeSnapshot != null) ? this.lastSafeSnapshot.scene : this.initializedScene);
			lifecycleEvent.sourcePrefab = this.sourcePrefab;
			lifecycleEvent.riderRideNumber = this.rideNumber;
			lifecycleEvent.distinctBikesUsed = this.distinctBikesUsed;
			lifecycleEvent.previousBikeId = this.previousBikeId;
			BicycleDiagnosticLog.Write(lifecycleEvent, true);
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002D50 RID: 11600 RVA: 0x000FA373 File Offset: 0x000F8573
		private bool Mounted
		{
			get
			{
				if (!(this.standard != null))
				{
					return this.legacy != null && this.legacy.IsBeingRidden;
				}
				return this.standard.IsBeingRidden;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002D51 RID: 11601 RVA: 0x000FA3AA File Offset: 0x000F85AA
		private vThirdPersonController CurrentRider
		{
			get
			{
				if (this.standard != null)
				{
					return this.standard.CurrentRider;
				}
				if (!(this.legacy != null))
				{
					return null;
				}
				return this.legacy.CurrentRider;
			}
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000FA3E4 File Offset: 0x000F85E4
		private void BindRider(vThirdPersonController next)
		{
			if (this.rider == next)
			{
				return;
			}
			this.rider = next;
			this.input = ((next != null) ? next.GetComponent<vThirdPersonInput>() : null);
			this.animator = ((next != null) ? next.GetComponent<Animator>() : null);
			this.parameterController = ((this.animator != null) ? this.animator.runtimeAnimatorController : null);
			this.rig = ((next != null) ? next.GetComponent<CharacterRigController>() : null);
			this.fp = ((next != null) ? next.GetComponent<FirstPersonController>() : null);
			this.calibration = ((next != null) ? next.GetComponent<FirstPersonCameraCalibration>() : null);
			this.parameters = ((this.animator != null && this.animator.runtimeAnimatorController != null) ? this.animator.parameters : null);
			this.head = (this.leftEye = (this.rightEye = null));
			Array.Clear(this.limbBones, 0, this.limbBones.Length);
			if (this.animator == null || !this.animator.isHuman)
			{
				return;
			}
			this.head = this.animator.GetBoneTransform(HumanBodyBones.Head);
			this.leftEye = this.animator.GetBoneTransform(HumanBodyBones.LeftEye);
			this.rightEye = this.animator.GetBoneTransform(HumanBodyBones.RightEye);
			this.limbBones[0] = this.animator.GetBoneTransform(HumanBodyBones.LeftHand);
			this.limbBones[1] = this.animator.GetBoneTransform(HumanBodyBones.RightHand);
			this.limbBones[2] = this.animator.GetBoneTransform(HumanBodyBones.LeftFoot);
			this.limbBones[3] = this.animator.GetBoneTransform(HumanBodyBones.RightFoot);
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000FA5AC File Offset: 0x000F87AC
		private void LateUpdate()
		{
			if (!BicycleRideDiagnostics.RecordingEnabled || this.bike == null)
			{
				return;
			}
			if (this.CurrentRider != null)
			{
				this.BindRider(this.CurrentRider);
			}
			bool mounted = this.Mounted;
			if (mounted != this.wasMounted)
			{
				this.wasMounted = mounted;
				this.mountedSince = Time.unscaledTime;
				this.blockedSince = (this.transitionSince = -1f);
				this.nextSample = Time.unscaledTime;
				this.watchUntil = Time.unscaledTime + 5f;
				this.Emit(mounted ? "riding.observed" : "dismounted.observed", this.phase, true);
			}
			bool flag = mounted || (this.standard != null && this.standard.IsTransitioning) || (this.driver != null && this.driver.IsActive) || Time.unscaledTime <= this.watchUntil;
			if (!flag && this.rider == null)
			{
				return;
			}
			if (Time.unscaledTime < this.nextSample)
			{
				return;
			}
			this.nextSample = Time.unscaledTime + (flag ? 0.1f : 1f);
			BicycleRideDiagnostics.Snapshot snapshot = this.CaptureSafely("sample", null);
			if (snapshot == null)
			{
				return;
			}
			BicycleDiagnosticLog.Write(snapshot, false);
			string text = string.Concat(new string[]
			{
				snapshot.mounted.ToString(),
				"|",
				snapshot.inControl.ToString(),
				"|",
				snapshot.bodyKinematic.ToString(),
				"|",
				snapshot.constraints,
				"|",
				snapshot.front.grounded.ToString(),
				"|",
				snapshot.rear.grounded.ToString(),
				"|",
				snapshot.poseState,
				"|",
				snapshot.targetPose,
				"|",
				snapshot.inputLocked.ToString(),
				"|",
				snapshot.braking.ToString(),
				"|",
				snapshot.firstPerson.ToString(),
				"|",
				snapshot.cameraState,
				"|",
				snapshot.controllerEnabled.ToString(),
				"|",
				snapshot.enabled.ToString(),
				"|",
				snapshot.inputSuppression,
				"|",
				snapshot.rawSprint.ToString(),
				"|",
				snapshot.rawBrake.ToString(),
				"|",
				snapshot.ragdolled.ToString(),
				"|",
				snapshot.focused.ToString()
			});
			if (text != this.lastState)
			{
				this.lastState = text;
				snapshot.kind = "state.changed";
				BicycleDiagnosticLog.Write(snapshot, true);
			}
			string text2 = this.DetectAnomaly(snapshot);
			if (text2 != this.lastAnomaly)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					snapshot.kind = "ANOMALY";
					snapshot.detail = text2;
					BicycleDiagnosticLog.Write(snapshot, true);
					Debug.LogWarning(BicycleRideDiagnostics.Summary(snapshot) + " | " + text2, this);
				}
				else if (!string.IsNullOrEmpty(this.lastAnomaly))
				{
					this.Emit("anomaly.cleared", this.lastAnomaly, true);
				}
				this.lastAnomaly = text2;
			}
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000FA950 File Offset: 0x000F8B50
		private string DetectAnomaly(BicycleRideDiagnostics.Snapshot sample)
		{
			bool flag = sample.mounted && sample.throttle > 0.25f && !sample.braking && !sample.inputLocked && sample.speed < 0.05f && Time.timeScale > 0f;
			if (!sample.mounted || !sample.controllerEnabled || !(sample.inputSuppression == "none") || sample.inputFrame < 0 || sample.frame - sample.inputFrame <= 3 || sample.timeScale <= 0f)
			{
				this.staleInputSince = -1f;
			}
			else if (this.staleInputSince < 0f)
			{
				this.staleInputSince = Time.unscaledTime;
			}
			if (!flag)
			{
				this.blockedSince = -1f;
			}
			if (!BicycleRideDiagnostics.Finite(sample.position) || !BicycleRideDiagnostics.Finite(sample.velocity) || !BicycleRideDiagnostics.Finite(sample.riderPosition) || !BicycleRideDiagnostics.Finite(sample.cameraPosition))
			{
				return "NON_FINITE_TRANSFORM_OR_VELOCITY";
			}
			if (sample.transitioning)
			{
				if (this.transitionSince < 0f)
				{
					this.transitionSince = Time.unscaledTime;
				}
				if (Time.unscaledTime - this.transitionSince > 2f)
				{
					return "MOUNT_OR_DISMOUNT_TRANSITION_STUCK";
				}
			}
			else
			{
				this.transitionSince = -1f;
			}
			if (!sample.mounted && !sample.transitioning && sample.poseActive)
			{
				return "UNMOUNTED_BIKE_STILL_DRIVING_RIDER_POSE";
			}
			if (!sample.mounted || Time.unscaledTime - this.mountedSince < 0.35f)
			{
				return null;
			}
			if (!sample.controllerEnabled)
			{
				return "RIDDEN_BIKE_CONTROLLER_DISABLED";
			}
			if (!sample.enabled)
			{
				return "RIDDEN_BIKE_SIMULATION_DISABLED";
			}
			if (this.staleInputSince >= 0f && Time.unscaledTime - this.staleInputSince > 1f)
			{
				return "RIDDEN_BIKE_INPUT_POLL_STALLED";
			}
			if (!sample.front.exists || !sample.rear.exists)
			{
				return "RIDDEN_BIKE_WHEEL_MISSING";
			}
			if (!sample.front.enabled || !sample.rear.enabled)
			{
				return "RIDDEN_BIKE_WHEEL_DISABLED";
			}
			if (sample.bodyKinematic)
			{
				return "RIDDEN_BIKE_IS_KINEMATIC";
			}
			if (!sample.inControl)
			{
				return "RIDDEN_BIKE_CONTROL_DISABLED";
			}
			if (!sample.hasCurrentRider)
			{
				return "RIDDEN_BIKE_HAS_NO_RIDER";
			}
			if (!sample.riderParented)
			{
				return "RIDER_NO_LONGER_PARENTED_TO_BIKE";
			}
			if (!sample.riderKinematic || sample.riderCapsuleEnabled)
			{
				return "RIDER_PHYSICS_ACTIVE_WHILE_MOUNTED";
			}
			if (sample.tilt > 55f)
			{
				return "BIKE_TIPPED_OVER";
			}
			if (sample.velocity.magnitude > 70f || sample.angularVelocity.magnitude > 25f)
			{
				return "EXTREME_PHYSICS_VELOCITY";
			}
			if (this.driver != null && !sample.poseActive)
			{
				return "MOUNTED_WITH_INACTIVE_POSE_DRIVER";
			}
			if (sample.poseActive && sample.hasSeat && sample.riderSeatError > 0.3f)
			{
				return "RIDER_DRIFT_FROM_POSE_SEAT";
			}
			if (flag)
			{
				if (this.blockedSince < 0f)
				{
					this.blockedSince = Time.unscaledTime;
				}
				if (Time.unscaledTime - this.blockedSince > 3f)
				{
					return "THROTTLE_WITHOUT_MOVEMENT_CHECK_CONTACTS";
				}
			}
			else
			{
				this.blockedSince = -1f;
			}
			return null;
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000FAC64 File Offset: 0x000F8E64
		private void OnCollisionEnter(Collision collision)
		{
			this.RecordContact("collision.enter", collision, true);
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000FAC74 File Offset: 0x000F8E74
		private void OnCollisionStay(Collision collision)
		{
			if (!BicycleRideDiagnostics.RecordingEnabled)
			{
				return;
			}
			if (collision == null || collision.collider == null || (!this.Mounted && Time.unscaledTime > this.watchUntil))
			{
				return;
			}
			int instanceID = collision.collider.GetInstanceID();
			float num;
			if (this.nextContactSample.TryGetValue(instanceID, out num) && Time.unscaledTime < num)
			{
				return;
			}
			this.nextContactSample[instanceID] = Time.unscaledTime + 1f;
			this.RecordContact("collision.stay", collision, false);
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000FACF9 File Offset: 0x000F8EF9
		private void OnCollisionExit(Collision collision)
		{
			if (!BicycleRideDiagnostics.RecordingEnabled)
			{
				return;
			}
			if (collision != null && collision.collider != null)
			{
				this.nextContactSample.Remove(collision.collider.GetInstanceID());
			}
			this.RecordContact("collision.exit", collision, false);
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000FAD38 File Offset: 0x000F8F38
		private void RecordContact(string kind, Collision collision, bool console)
		{
			if (!BicycleRideDiagnostics.RecordingEnabled)
			{
				return;
			}
			if (collision == null || (!this.Mounted && Time.unscaledTime > this.watchUntil))
			{
				return;
			}
			Vector3 vector = ((collision.contactCount > 0) ? collision.GetContact(0).point : Vector3.zero);
			Vector3 vector2 = ((collision.contactCount > 0) ? collision.GetContact(0).normal : Vector3.zero);
			this.Emit(kind, string.Concat(new string[]
			{
				"other=",
				BicycleRideDiagnostics.Path(collision.transform),
				"; layer=",
				collision.gameObject.layer.ToString(),
				"; colliderId=",
				((collision.collider != null) ? collision.collider.GetInstanceID() : 0).ToString(),
				"; contacts=",
				collision.contactCount.ToString(),
				"; relativeVelocity=",
				collision.relativeVelocity.ToString("F3"),
				"; impulse=",
				collision.impulse.ToString("F3"),
				"; point=",
				vector.ToString("F3"),
				"; normal=",
				vector2.ToString("F3")
			}), console);
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000FAEAC File Offset: 0x000F90AC
		private static void OnUnityLog(string message, string stack, LogType type)
		{
			if (!BicycleRideDiagnostics.RecordingEnabled || BicycleRideDiagnostics.reportingUnityError || message.StartsWith("[BICI]", StringComparison.Ordinal))
			{
				return;
			}
			if (type != LogType.Error && type != LogType.Exception && !message.StartsWith("[BikeCrashRagdoll]", StringComparison.Ordinal) && (type != LogType.Warning || (!message.Contains("Bike") && !message.Contains("Vehicle") && !message.Contains("parent"))))
			{
				return;
			}
			BicycleRideDiagnostics.reportingUnityError = true;
			try
			{
				foreach (BicycleRideDiagnostics bicycleRideDiagnostics in BicycleRideDiagnostics.Recorders)
				{
					if (bicycleRideDiagnostics != null && (bicycleRideDiagnostics.Mounted || bicycleRideDiagnostics.rider != null || Time.unscaledTime < bicycleRideDiagnostics.watchUntil))
					{
						bicycleRideDiagnostics.Emit("unity." + type.ToString(), message + "\n" + stack, false);
					}
				}
				BicycleDiagnosticLog.Flush();
			}
			finally
			{
				BicycleRideDiagnostics.reportingUnityError = false;
			}
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000FAFCC File Offset: 0x000F91CC
		private void Emit(string kind, string detail, bool console)
		{
			if (!BicycleRideDiagnostics.RecordingEnabled)
			{
				return;
			}
			BicycleRideDiagnostics.Snapshot snapshot = this.CaptureSafely(kind, detail);
			if (snapshot == null)
			{
				return;
			}
			BicycleDiagnosticLog.Write(snapshot, console);
			if (console)
			{
				Debug.Log(BicycleRideDiagnostics.Summary(snapshot) + (string.IsNullOrEmpty(detail) ? "" : (" | " + detail)) + " | log=" + BicycleDiagnosticLog.CurrentPath, this);
			}
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000FB030 File Offset: 0x000F9230
		private BicycleRideDiagnostics.Snapshot CaptureSafely(string kind, string detail)
		{
			BicycleRideDiagnostics.Snapshot snapshot2;
			try
			{
				BicycleRideDiagnostics.Snapshot snapshot = this.Capture(kind, detail);
				this.lastSafeSnapshot = snapshot;
				snapshot2 = snapshot;
			}
			catch (Exception ex)
			{
				if (kind != "sample" || Time.unscaledTime >= this.nextFailureSample)
				{
					this.nextFailureSample = Time.unscaledTime + 1f;
					BicycleDiagnosticLog.Write(new BicycleRideDiagnostics.CaptureFailure
					{
						session = BicycleDiagnosticLog.SessionId,
						frame = Time.frameCount,
						realtime = Time.unscaledTime,
						instanceId = base.GetInstanceID(),
						attemptedKind = kind,
						detail = detail,
						error = ex.ToString()
					}, true);
				}
				if (!this.snapshotFailed)
				{
					this.snapshotFailed = true;
					Debug.LogWarning("[BICI] Snapshot diagnostico non riuscito: " + ex.Message, this);
				}
				snapshot2 = null;
			}
			return snapshot2;
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000FB10C File Offset: 0x000F930C
		private BicycleRideDiagnostics.Snapshot Capture(string kind, string detail)
		{
			BicycleRideDiagnostics.Snapshot snapshot = new BicycleRideDiagnostics.Snapshot();
			snapshot.kind = kind;
			snapshot.detail = detail;
			snapshot.phase = this.phase;
			snapshot.session = BicycleDiagnosticLog.SessionId;
			snapshot.utc = DateTime.UtcNow.ToString("O");
			int num = this.sequence + 1;
			this.sequence = num;
			snapshot.sequence = num;
			snapshot.frame = Time.frameCount;
			snapshot.realtime = Time.unscaledTime;
			snapshot.gameTime = Time.time;
			snapshot.timeScale = Time.timeScale;
			snapshot.instanceId = base.GetInstanceID();
			snapshot.bikeId = ((this.bike != null) ? this.bike.GetInstanceID() : 0);
			snapshot.riderRideNumber = this.rideNumber;
			snapshot.distinctBikesUsed = this.distinctBikesUsed;
			snapshot.previousBikeId = this.previousBikeId;
			snapshot.name = base.name;
			snapshot.hierarchy = BicycleRideDiagnostics.Path(base.transform);
			snapshot.scene = base.gameObject.scene.path;
			snapshot.sourcePrefab = this.sourcePrefab;
			snapshot.active = base.gameObject.activeInHierarchy;
			snapshot.mounted = this.Mounted;
			snapshot.focused = Application.isFocused;
			snapshot.controllerEnabled = ((this.standard != null) ? this.standard.isActiveAndEnabled : (this.legacy != null && this.legacy.isActiveAndEnabled));
			snapshot.inputFrame = ((this.standard != null) ? this.standard.DiagnosticInputFrame : ((this.legacy != null) ? this.legacy.DiagnosticInputFrame : (-1)));
			snapshot.inputCheckFrame = ((this.standard != null) ? this.standard.DiagnosticInputCheckFrame : ((this.legacy != null) ? this.legacy.DiagnosticInputCheckFrame : (-1)));
			snapshot.rawThrottle = ((this.standard != null) ? this.standard.DiagnosticRawThrottle : ((this.legacy != null) ? this.legacy.DiagnosticRawThrottle : 0f));
			snapshot.rawSteering = ((this.standard != null) ? this.standard.DiagnosticRawSteering : ((this.legacy != null) ? this.legacy.DiagnosticRawSteering : 0f));
			snapshot.rawBrake = ((this.standard != null) ? this.standard.DiagnosticRawBrake : (this.legacy != null && this.legacy.DiagnosticRawBrake));
			snapshot.rawSprint = ((this.standard != null) ? this.standard.DiagnosticRawSprint : (this.legacy != null && this.legacy.DiagnosticRawSprint));
			snapshot.inputSuppression = ((this.standard != null) ? this.standard.DiagnosticInputSuppression : ((this.legacy != null) ? this.legacy.DiagnosticInputSuppression : "missing_controller"));
			snapshot.hasCurrentRider = this.CurrentRider != null;
			snapshot.transitioning = this.standard != null && this.standard.IsTransitioning;
			snapshot.mountStabilizationRemaining = ((this.standard != null) ? this.standard.MountStabilizationRemaining : 0f);
			snapshot.hasBothControllers = this.standard != null && this.legacy != null;
			snapshot.controller = ((this.standard != null) ? "vVehicleController" : "vBikeController");
			snapshot.position = base.transform.position;
			snapshot.euler = base.transform.eulerAngles;
			snapshot.scale = base.transform.lossyScale;
			snapshot.tilt = Vector3.Angle(base.transform.up, Vector3.up);
			snapshot.enabled = this.bike != null && this.bike.enabled;
			snapshot.inControl = this.bike != null && this.bike.isInControl;
			snapshot.throttle = ((this.bike != null) ? this.bike.verticalInput : 0f);
			snapshot.steering = ((this.bike != null) ? this.bike.horizontalInput : 0f);
			snapshot.braking = this.bike != null && this.bike.braking;
			snapshot.speed = ((this.bike != null) ? this.bike.currentSpeed : 0f);
			snapshot.motorForce = ((this.bike != null) ? this.bike.motorForce : 0f);
			snapshot.steerAngle = ((this.bike != null) ? this.bike.currentSteeringAngle : 0f);
			snapshot.leanAngle = ((this.bike != null) ? this.bike.currentLeanAngle : 0f);
			snapshot.triggerContact = this.bike != null && this.bike.controlContact != null && this.bike.controlContact.GetContact();
			snapshot.posePreset = ((this.driver != null && this.driver.PosePreset != null) ? this.driver.PosePreset.name : "MISSING");
			snapshot.poseActive = this.driver != null && this.driver.IsActive;
			snapshot.poseState = ((this.driver != null) ? this.driver.CurrentState.ToString() : "MISSING");
			snapshot.targetPose = ((this.driver != null) ? this.driver.TargetState.ToString() : "MISSING");
			snapshot.poseBlend = ((this.driver != null) ? this.driver.BlendProgress : 0f);
			snapshot.inputLocked = this.input != null && this.input.lockInput;
			snapshot.riderInputEnabled = this.input != null && this.input.isActiveAndEnabled;
			snapshot.cameraInputLocked = this.input != null && this.input.lockCameraInput;
			snapshot.forceCameraLock = vThirdPersonInput.ForceCameraLock;
			snapshot.activeResources = PlayerActivityGate.ActiveResources.ToString();
			snapshot.blockedActivities = PlayerActivityGate.BlockedMask.ToString();
			BicycleRideDiagnostics.Snapshot snapshot2 = snapshot;
			SpawnedVehicleGroundSafety component = base.GetComponent<SpawnedVehicleGroundSafety>();
			snapshot2.hasGroundSafety = component != null;
			snapshot2.groundRecoveries = ((component != null) ? component.RecoveryCount : 0);
			snapshot2.lastSafePosition = ((component != null) ? component.LastSafePosition : Vector3.zero);
			SpawnedVehicleLifetime component2 = base.GetComponent<SpawnedVehicleLifetime>();
			snapshot2.hasLifetime = component2 != null;
			snapshot2.lifetimeRemaining = ((component2 != null) ? component2.RemainingSeconds : (-1f));
			snapshot2.lifetimeExpiring = component2 != null && component2.IsExpiring;
			if (this.body != null)
			{
				snapshot2.bodyKinematic = this.body.isKinematic;
				snapshot2.sleeping = this.body.IsSleeping();
				snapshot2.velocity = this.body.velocity;
				snapshot2.angularVelocity = this.body.angularVelocity;
				snapshot2.bodyPosition = this.body.position;
				snapshot2.physicsSpeed = this.body.velocity.magnitude;
				snapshot2.bodyDetectCollisions = this.body.detectCollisions;
				snapshot2.bodyUseGravity = this.body.useGravity;
				snapshot2.constraints = this.body.constraints.ToString();
				snapshot2.collisionMode = this.body.collisionDetectionMode.ToString();
				snapshot2.interpolation = this.body.interpolation.ToString();
			}
			snapshot2.front = this.ReadWheel((this.bike != null) ? this.bike.FrontWheel : null, this.bike != null && this.bike.slipFront);
			snapshot2.rear = this.ReadWheel((this.bike != null) ? this.bike.RearWheel : null, this.bike != null && this.bike.slipRear);
			Transform transform = ((this.anchors != null) ? this.anchors.GetAnchor(VehicleAnchorType.Seat) : null);
			if (transform == null)
			{
				transform = ((this.standard != null) ? this.standard.seatPosition : ((this.legacy != null) ? this.legacy.seatPosition : null));
			}
			snapshot2.hasSeat = transform != null;
			if (transform != null)
			{
				snapshot2.seatPosition = transform.position;
			}
			if (this.rider != null)
			{
				snapshot2.riderName = this.rider.name;
				snapshot2.riderId = this.rider.GetInstanceID();
				snapshot2.riderParent = BicycleRideDiagnostics.Path(this.rider.transform.parent);
				snapshot2.riderParented = this.rider.transform.IsChildOf(base.transform);
				snapshot2.riderPosition = this.rider.transform.position;
				snapshot2.riderLocalPosition = this.rider.transform.localPosition;
				snapshot2.riderEuler = this.rider.transform.eulerAngles;
				snapshot2.movementLocked = this.rider.lockMovement;
				snapshot2.rotationLocked = this.rider.lockRotation;
				snapshot2.animationMovementLocked = this.rider.lockAnimMovement;
				snapshot2.animationRotationLocked = this.rider.lockAnimRotation;
				snapshot2.ragdolled = this.rider.ragdolled;
				Rigidbody component3 = this.rider.GetComponent<Rigidbody>();
				CapsuleCollider component4 = this.rider.GetComponent<CapsuleCollider>();
				snapshot2.riderKinematic = component3 != null && component3.isKinematic;
				snapshot2.riderCapsuleEnabled = component4 != null && component4.enabled;
				if (this.driver != null && this.driver.CurrentPose != null && transform != null)
				{
					snapshot2.expectedRiderPosition = transform.TransformPoint(this.driver.CurrentPose.seatPositionOffset);
					snapshot2.riderSeatError = Vector3.Distance(snapshot2.riderPosition, snapshot2.expectedRiderPosition);
					snapshot2.poseSeatOffset = this.driver.CurrentPose.seatPositionOffset;
					snapshot2.poseSeatRotation = this.driver.CurrentPose.seatRotationOffset;
				}
				if (this.head != null)
				{
					snapshot2.headPosition = this.head.position;
				}
				snapshot2.eyePosition = ((this.leftEye != null && this.rightEye != null) ? ((this.leftEye.position + this.rightEye.position) * 0.5f) : snapshot2.headPosition);
				snapshot2.hasEyeBones = this.leftEye != null && this.rightEye != null;
				snapshot2.headEuler = ((this.head != null) ? this.head.eulerAngles : Vector3.zero);
				snapshot2.eyesInRiderSpace = this.rider.transform.InverseTransformPoint(snapshot2.eyePosition);
				Transform transform2 = ((this.fp != null) ? this.fp.positionReferenceBone : null);
				snapshot2.hasChest = transform2 != null;
				snapshot2.chestPosition = ((transform2 != null) ? transform2.position : Vector3.zero);
				snapshot2.chestEuler = ((transform2 != null) ? transform2.eulerAngles : Vector3.zero);
			}
			if (this.animator != null && this.animator.runtimeAnimatorController != null)
			{
				if (this.parameterController != this.animator.runtimeAnimatorController)
				{
					this.parameterController = this.animator.runtimeAnimatorController;
					this.parameters = this.animator.parameters;
				}
				snapshot2.animatorEnabled = this.animator.enabled;
				snapshot2.animatorSpeed = this.animator.speed;
				snapshot2.animatorUpdateMode = this.animator.updateMode.ToString();
				snapshot2.applyRootMotion = this.animator.applyRootMotion;
				snapshot2.animatorController = this.animator.runtimeAnimatorController.name;
				snapshot2.animationLayers = new BicycleRideDiagnostics.AnimationLayer[this.animator.layerCount];
				for (int i = 0; i < snapshot2.animationLayers.Length; i++)
				{
					AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(i);
					bool flag = this.animator.IsInTransition(i);
					AnimatorStateInfo animatorStateInfo = (flag ? this.animator.GetNextAnimatorStateInfo(i) : default(AnimatorStateInfo));
					snapshot2.animationLayers[i] = new BicycleRideDiagnostics.AnimationLayer
					{
						name = this.animator.GetLayerName(i),
						hash = currentAnimatorStateInfo.fullPathHash,
						normalizedTime = currentAnimatorStateInfo.normalizedTime,
						weight = this.animator.GetLayerWeight(i),
						transitioning = flag,
						nextHash = animatorStateInfo.fullPathHash,
						nextNormalizedTime = animatorStateInfo.normalizedTime,
						clips = this.ReadClips(i, false),
						nextClips = (flag ? this.ReadClips(i, true) : Array.Empty<BicycleRideDiagnostics.ClipSnapshot>())
					};
				}
				snapshot2.animationParameters = this.ReadAnimatorParameters();
			}
			if (this.rig != null)
			{
				snapshot2.vehicleRigWeight = ((this.rig.VehicleRig != null) ? this.rig.VehicleRig.weight : (-1f));
				snapshot2.coverRigWeight = ((this.rig.CoverPoseRig != null) ? this.rig.CoverPoseRig.weight : (-1f));
				snapshot2.limbs = new BicycleRideDiagnostics.LimbSnapshot[BicycleRideDiagnostics.Limbs.Length];
				for (int j = 0; j < BicycleRideDiagnostics.Limbs.Length; j++)
				{
					Transform vehicleLimbTarget = this.rig.GetVehicleLimbTarget(BicycleRideDiagnostics.Limbs[j]);
					snapshot2.limbs[j] = new BicycleRideDiagnostics.LimbSnapshot
					{
						name = BicycleRideDiagnostics.Limbs[j].ToString(),
						weight = this.rig.GetVehicleLimbWeight(BicycleRideDiagnostics.Limbs[j]),
						hasBone = (this.limbBones[j] != null),
						hasTarget = (vehicleLimbTarget != null),
						bone = ((this.limbBones[j] != null) ? this.limbBones[j].position : Vector3.zero),
						target = ((vehicleLimbTarget != null) ? vehicleLimbTarget.position : Vector3.zero)
					};
					if (this.limbBones[j] != null && vehicleLimbTarget != null)
					{
						snapshot2.limbs[j].error = Vector3.Distance(this.limbBones[j].position, vehicleLimbTarget.position);
					}
				}
			}
			Camera main = Camera.main;
			if (main != null)
			{
				snapshot2.cameraName = main.name;
				snapshot2.cameraPosition = main.transform.position;
				snapshot2.cameraEuler = main.transform.eulerAngles;
				snapshot2.cameraFov = main.fieldOfView;
				snapshot2.cameraNear = main.nearClipPlane;
				snapshot2.eyeToCameraDistance = ((this.rider != null) ? Vector3.Distance(snapshot2.eyePosition, snapshot2.cameraPosition) : 0f);
				if (this.rider != null)
				{
					snapshot2.cameraInRiderSpace = this.rider.transform.InverseTransformPoint(snapshot2.cameraPosition);
					snapshot2.cameraFromEyesRiderAxes = this.rider.transform.InverseTransformDirection(snapshot2.cameraPosition - snapshot2.eyePosition);
					snapshot2.cameraBehindEyes = Mathf.Max(0f, -snapshot2.cameraFromEyesRiderAxes.z);
				}
			}
			snapshot2.firstPerson = this.fp != null && this.fp.IsFirstPerson;
			snapshot2.firstPersonPoseFrame = ((this.fp != null) ? this.fp.CameraPoseFrame : (-1));
			snapshot2.externalCameraAnchor = ((this.fp != null && this.fp.CameraAnchorOverride != null) ? BicycleRideDiagnostics.Path(this.fp.CameraAnchorOverride) : "");
			snapshot2.calibrationOpen = this.calibration != null && this.calibration.IsOpen;
			snapshot2.calibrationPreview = this.calibration != null && this.calibration.HasPreview;
			if (this.input != null && this.input.tpCamera != null)
			{
				snapshot2.cameraState = this.input.tpCamera.currentStateName;
				snapshot2.cameraFrozen = this.input.tpCamera.isFreezed;
				snapshot2.cameraTarget = BicycleRideDiagnostics.Path(this.input.tpCamera.mainTarget);
			}
			return snapshot2;
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000FC3A8 File Offset: 0x000FA5A8
		private BicycleRideDiagnostics.ClipSnapshot[] ReadClips(int layer, bool next)
		{
			this.clipBuffer.Clear();
			if (next)
			{
				this.animator.GetNextAnimatorClipInfo(layer, this.clipBuffer);
			}
			else
			{
				this.animator.GetCurrentAnimatorClipInfo(layer, this.clipBuffer);
			}
			BicycleRideDiagnostics.ClipSnapshot[] array = new BicycleRideDiagnostics.ClipSnapshot[this.clipBuffer.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new BicycleRideDiagnostics.ClipSnapshot
				{
					name = ((this.clipBuffer[i].clip != null) ? this.clipBuffer[i].clip.name : "MISSING"),
					weight = this.clipBuffer[i].weight
				};
			}
			return array;
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000FC470 File Offset: 0x000FA670
		private string[] ReadAnimatorParameters()
		{
			if (this.parameters == null)
			{
				return Array.Empty<string>();
			}
			List<string> list = new List<string>();
			foreach (AnimatorControllerParameter animatorControllerParameter in this.parameters)
			{
				string name = animatorControllerParameter.name;
				if (name.Contains("Vehicle") || name.Contains("Riding") || name.Contains("Cover") || name.Contains("Input") || name.Contains("Ground") || name.Contains("Stance") || name.Contains("Crouch"))
				{
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Float)
					{
						list.Add(name + "=" + this.animator.GetFloat(animatorControllerParameter.nameHash).ToString("R", CultureInfo.InvariantCulture));
					}
					else if (animatorControllerParameter.type == AnimatorControllerParameterType.Int)
					{
						list.Add(name + "=" + this.animator.GetInteger(animatorControllerParameter.nameHash).ToString());
					}
					else if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool)
					{
						list.Add(name + "=" + this.animator.GetBool(animatorControllerParameter.nameHash).ToString());
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000FC5D0 File Offset: 0x000FA7D0
		private BicycleRideDiagnostics.WheelSnapshot ReadWheel(WheelCollider wheel, bool effectSlip)
		{
			BicycleRideDiagnostics.WheelSnapshot wheelSnapshot = new BicycleRideDiagnostics.WheelSnapshot
			{
				exists = (wheel != null),
				effectSlip = effectSlip
			};
			if (wheel == null)
			{
				return wheelSnapshot;
			}
			wheelSnapshot.name = BicycleRideDiagnostics.Path(wheel.transform);
			wheelSnapshot.enabled = wheel.enabled && wheel.gameObject.activeInHierarchy;
			wheelSnapshot.layer = wheel.gameObject.layer;
			wheelSnapshot.attachedBody = ((wheel.attachedRigidbody != null) ? wheel.attachedRigidbody.GetInstanceID() : 0);
			wheelSnapshot.position = wheel.transform.position;
			wheelSnapshot.radius = wheel.radius;
			wheelSnapshot.suspension = wheel.suspensionDistance;
			wheelSnapshot.forceAppPointDistance = wheel.forceAppPointDistance;
			JointSpring suspensionSpring = wheel.suspensionSpring;
			wheelSnapshot.spring = suspensionSpring.spring;
			wheelSnapshot.damper = suspensionSpring.damper;
			wheelSnapshot.targetPosition = suspensionSpring.targetPosition;
			wheelSnapshot.sprungMass = wheel.sprungMass;
			wheelSnapshot.rpm = wheel.rpm;
			wheelSnapshot.motorTorque = wheel.motorTorque;
			wheelSnapshot.brakeTorque = wheel.brakeTorque;
			WheelHit wheelHit;
			wheelSnapshot.grounded = wheel.GetGroundHit(out wheelHit);
			if (wheelSnapshot.grounded)
			{
				wheelSnapshot.contact = BicycleRideDiagnostics.Path((wheelHit.collider != null) ? wheelHit.collider.transform : null);
				wheelSnapshot.contactPoint = wheelHit.point;
				wheelSnapshot.contactNormal = wheelHit.normal;
				wheelSnapshot.contactForce = wheelHit.force;
				wheelSnapshot.forwardSlip = wheelHit.forwardSlip;
				wheelSnapshot.sidewaysSlip = wheelHit.sidewaysSlip;
			}
			int num = Physics.RaycastNonAlloc(wheel.transform.position + Vector3.up, Vector3.down, this.supportHits, 5f, -1, QueryTriggerInteraction.Ignore);
			float num2 = float.MaxValue;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this.supportHits[i];
				if (!(raycastHit.collider == null) && !raycastHit.collider.transform.IsChildOf(base.transform) && raycastHit.distance < num2)
				{
					num2 = raycastHit.distance;
					wheelSnapshot.supportBelow = BicycleRideDiagnostics.Path(raycastHit.collider.transform);
					wheelSnapshot.supportLayer = raycastHit.collider.gameObject.layer;
					wheelSnapshot.supportPoint = raycastHit.point;
					wheelSnapshot.supportLayerIgnored = Physics.GetIgnoreLayerCollision(wheelSnapshot.layer, wheelSnapshot.supportLayer);
				}
			}
			return wheelSnapshot;
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000FC85C File Offset: 0x000FAA5C
		private void Configuration()
		{
			if (this.bike == null)
			{
				return;
			}
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>(true);
			List<string> list = new List<string>();
			foreach (Collider collider in componentsInChildren)
			{
				if (!(this.rider != null) || !collider.transform.IsChildOf(this.rider.transform))
				{
					list.Add(string.Concat(new string[]
					{
						collider.GetType().Name,
						":",
						BicycleRideDiagnostics.Path(collider.transform),
						"; enabled=",
						collider.enabled.ToString(),
						"; trigger=",
						collider.isTrigger.ToString(),
						"; layer=",
						collider.gameObject.layer.ToString(),
						"; body=",
						(collider.attachedRigidbody != null) ? collider.attachedRigidbody.name : "none"
					}));
				}
			}
			this.Emit("configuration", string.Concat(new string[]
			{
				"mass=",
				((this.body != null) ? this.body.mass : 0f).ToString(),
				"; GroundSafety=",
				(base.GetComponent<SpawnedVehicleGroundSafety>() != null).ToString(),
				"; handle=",
				BicycleRideDiagnostics.Path(this.bike.handle),
				"; colliders=[",
				string.Join(" | ", list),
				"]"
			}), false);
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x000FCA24 File Offset: 0x000FAC24
		private static string Summary(BicycleRideDiagnostics.Snapshot s)
		{
			return string.Concat(new string[]
			{
				"[BICI] ",
				s.kind,
				" ",
				s.name,
				"#",
				s.bikeId.ToString(),
				" ride=",
				s.riderRideNumber.ToString(),
				" distinctBikes=",
				s.distinctBikesUsed.ToString(),
				" speed=",
				s.speed.ToString("F2"),
				" tilt=",
				s.tilt.ToString("F1"),
				" ground=",
				s.front.grounded.ToString(),
				"/",
				s.rear.grounded.ToString(),
				" kin=",
				s.bodyKinematic.ToString(),
				" pose=",
				s.poseState,
				"->",
				s.targetPose,
				" FP=",
				s.firstPerson.ToString()
			});
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000FCB6C File Offset: 0x000FAD6C
		private static bool Finite(Vector3 v)
		{
			return !float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z) && !float.IsInfinity(v.x) && !float.IsInfinity(v.y) && !float.IsInfinity(v.z);
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x000FCBCC File Offset: 0x000FADCC
		public static string Path(Transform transform)
		{
			if (transform == null)
			{
				return "none";
			}
			string text = transform.name;
			Transform transform2 = transform.parent;
			while (transform2 != null)
			{
				text = transform2.name + "/" + text;
				transform2 = transform2.parent;
			}
			return text;
		}

		// Token: 0x04002C2F RID: 11311
		private const string EnabledSessionKey = "ProjectExhibitionist.BicycleDiagnostics.Enabled";

		// Token: 0x04002C30 RID: 11312
		private static bool recordingEnabled;

		// Token: 0x04002C31 RID: 11313
		private static readonly HashSet<BicycleRideDiagnostics> Recorders = new HashSet<BicycleRideDiagnostics>();

		// Token: 0x04002C32 RID: 11314
		private static readonly Dictionary<int, int> MountsByRider = new Dictionary<int, int>();

		// Token: 0x04002C33 RID: 11315
		private static readonly Dictionary<int, int> LastBikeByRider = new Dictionary<int, int>();

		// Token: 0x04002C34 RID: 11316
		private static readonly Dictionary<int, HashSet<int>> BikesByRider = new Dictionary<int, HashSet<int>>();

		// Token: 0x04002C35 RID: 11317
		private static bool reportingUnityError;

		// Token: 0x04002C36 RID: 11318
		private BicycleVehicle bike;

		// Token: 0x04002C37 RID: 11319
		private vVehicleController standard;

		// Token: 0x04002C38 RID: 11320
		private vBikeController legacy;

		// Token: 0x04002C39 RID: 11321
		private Rigidbody body;

		// Token: 0x04002C3A RID: 11322
		private VehiclePoseDriver driver;

		// Token: 0x04002C3B RID: 11323
		private IVehicleAnchorProvider anchors;

		// Token: 0x04002C3C RID: 11324
		private vThirdPersonController rider;

		// Token: 0x04002C3D RID: 11325
		private vThirdPersonInput input;

		// Token: 0x04002C3E RID: 11326
		private Animator animator;

		// Token: 0x04002C3F RID: 11327
		private RuntimeAnimatorController parameterController;

		// Token: 0x04002C40 RID: 11328
		private AnimatorControllerParameter[] parameters;

		// Token: 0x04002C41 RID: 11329
		private CharacterRigController rig;

		// Token: 0x04002C42 RID: 11330
		private FirstPersonController fp;

		// Token: 0x04002C43 RID: 11331
		private FirstPersonCameraCalibration calibration;

		// Token: 0x04002C44 RID: 11332
		private Transform head;

		// Token: 0x04002C45 RID: 11333
		private Transform leftEye;

		// Token: 0x04002C46 RID: 11334
		private Transform rightEye;

		// Token: 0x04002C47 RID: 11335
		private readonly Transform[] limbBones = new Transform[4];

		// Token: 0x04002C48 RID: 11336
		private static readonly LimbType[] Limbs = new LimbType[]
		{
			LimbType.LeftHand,
			LimbType.RightHand,
			LimbType.LeftFoot,
			LimbType.RightFoot
		};

		// Token: 0x04002C49 RID: 11337
		private readonly RaycastHit[] supportHits = new RaycastHit[16];

		// Token: 0x04002C4A RID: 11338
		private readonly List<AnimatorClipInfo> clipBuffer = new List<AnimatorClipInfo>(8);

		// Token: 0x04002C4B RID: 11339
		private readonly Dictionary<int, float> nextContactSample = new Dictionary<int, float>();

		// Token: 0x04002C4C RID: 11340
		private float nextSample;

		// Token: 0x04002C4D RID: 11341
		private float watchUntil;

		// Token: 0x04002C4E RID: 11342
		private float mountedSince;

		// Token: 0x04002C4F RID: 11343
		private float transitionSince = -1f;

		// Token: 0x04002C50 RID: 11344
		private float blockedSince = -1f;

		// Token: 0x04002C51 RID: 11345
		private bool wasMounted;

		// Token: 0x04002C52 RID: 11346
		private string lastState;

		// Token: 0x04002C53 RID: 11347
		private string lastAnomaly;

		// Token: 0x04002C54 RID: 11348
		private string sourcePrefab;

		// Token: 0x04002C55 RID: 11349
		private string phase;

		// Token: 0x04002C56 RID: 11350
		private bool initialized;

		// Token: 0x04002C57 RID: 11351
		private bool snapshotFailed;

		// Token: 0x04002C58 RID: 11352
		private float nextFailureSample;

		// Token: 0x04002C59 RID: 11353
		private int rideNumber;

		// Token: 0x04002C5A RID: 11354
		private int distinctBikesUsed;

		// Token: 0x04002C5B RID: 11355
		private int previousBikeId;

		// Token: 0x04002C5C RID: 11356
		private int sequence;

		// Token: 0x04002C5D RID: 11357
		private float staleInputSince = -1f;

		// Token: 0x04002C5E RID: 11358
		private BicycleRideDiagnostics.Snapshot lastSafeSnapshot;

		// Token: 0x04002C5F RID: 11359
		private int initializedInstanceId;

		// Token: 0x04002C60 RID: 11360
		private int initializedBikeId;

		// Token: 0x04002C61 RID: 11361
		private string initializedName;

		// Token: 0x04002C62 RID: 11362
		private string initializedHierarchy;

		// Token: 0x04002C63 RID: 11363
		private string initializedScene;

		// Token: 0x020006F4 RID: 1780
		[Serializable]
		private sealed class LifecycleEvent
		{
			// Token: 0x04002C64 RID: 11364
			public int schema = 2;

			// Token: 0x04002C65 RID: 11365
			public string session;

			// Token: 0x04002C66 RID: 11366
			public string kind;

			// Token: 0x04002C67 RID: 11367
			public string phase;

			// Token: 0x04002C68 RID: 11368
			public string utc;

			// Token: 0x04002C69 RID: 11369
			public string name;

			// Token: 0x04002C6A RID: 11370
			public string hierarchy;

			// Token: 0x04002C6B RID: 11371
			public string scene;

			// Token: 0x04002C6C RID: 11372
			public string sourcePrefab;

			// Token: 0x04002C6D RID: 11373
			public int sequence;

			// Token: 0x04002C6E RID: 11374
			public int frame;

			// Token: 0x04002C6F RID: 11375
			public int instanceId;

			// Token: 0x04002C70 RID: 11376
			public int bikeId;

			// Token: 0x04002C71 RID: 11377
			public int riderId;

			// Token: 0x04002C72 RID: 11378
			public int riderRideNumber;

			// Token: 0x04002C73 RID: 11379
			public int distinctBikesUsed;

			// Token: 0x04002C74 RID: 11380
			public int previousBikeId;

			// Token: 0x04002C75 RID: 11381
			public float realtime;
		}

		// Token: 0x020006F5 RID: 1781
		[Serializable]
		private sealed class ProblemMarker
		{
			// Token: 0x04002C76 RID: 11382
			public string kind = "USER_MARKER";

			// Token: 0x04002C77 RID: 11383
			public string session;

			// Token: 0x04002C78 RID: 11384
			public string detail;

			// Token: 0x04002C79 RID: 11385
			public int frame;

			// Token: 0x04002C7A RID: 11386
			public int activeBikes;

			// Token: 0x04002C7B RID: 11387
			public float realtime;
		}

		// Token: 0x020006F6 RID: 1782
		[Serializable]
		private sealed class Snapshot
		{
			// Token: 0x04002C7C RID: 11388
			public int schema = 2;

			// Token: 0x04002C7D RID: 11389
			public string utc;

			// Token: 0x04002C7E RID: 11390
			public string inputSuppression;

			// Token: 0x04002C7F RID: 11391
			public int sequence;

			// Token: 0x04002C80 RID: 11392
			public int inputFrame;

			// Token: 0x04002C81 RID: 11393
			public int inputCheckFrame;

			// Token: 0x04002C82 RID: 11394
			public float rawThrottle;

			// Token: 0x04002C83 RID: 11395
			public float rawSteering;

			// Token: 0x04002C84 RID: 11396
			public float physicsSpeed;

			// Token: 0x04002C85 RID: 11397
			public bool rawBrake;

			// Token: 0x04002C86 RID: 11398
			public bool rawSprint;

			// Token: 0x04002C87 RID: 11399
			public bool focused;

			// Token: 0x04002C88 RID: 11400
			public bool controllerEnabled;

			// Token: 0x04002C89 RID: 11401
			public bool riderInputEnabled;

			// Token: 0x04002C8A RID: 11402
			public bool cameraInputLocked;

			// Token: 0x04002C8B RID: 11403
			public bool forceCameraLock;

			// Token: 0x04002C8C RID: 11404
			public bool bodyDetectCollisions;

			// Token: 0x04002C8D RID: 11405
			public bool bodyUseGravity;

			// Token: 0x04002C8E RID: 11406
			public bool hasEyeBones;

			// Token: 0x04002C8F RID: 11407
			public bool hasChest;

			// Token: 0x04002C90 RID: 11408
			public Vector3 poseSeatOffset;

			// Token: 0x04002C91 RID: 11409
			public Vector3 poseSeatRotation;

			// Token: 0x04002C92 RID: 11410
			public Vector3 headEuler;

			// Token: 0x04002C93 RID: 11411
			public Vector3 chestPosition;

			// Token: 0x04002C94 RID: 11412
			public Vector3 chestEuler;

			// Token: 0x04002C95 RID: 11413
			public Vector3 eyesInRiderSpace;

			// Token: 0x04002C96 RID: 11414
			public Vector3 cameraInRiderSpace;

			// Token: 0x04002C97 RID: 11415
			public Vector3 cameraFromEyesRiderAxes;

			// Token: 0x04002C98 RID: 11416
			public float cameraBehindEyes;

			// Token: 0x04002C99 RID: 11417
			public string session;

			// Token: 0x04002C9A RID: 11418
			public string kind;

			// Token: 0x04002C9B RID: 11419
			public string detail;

			// Token: 0x04002C9C RID: 11420
			public string phase;

			// Token: 0x04002C9D RID: 11421
			public string name;

			// Token: 0x04002C9E RID: 11422
			public string hierarchy;

			// Token: 0x04002C9F RID: 11423
			public string scene;

			// Token: 0x04002CA0 RID: 11424
			public string sourcePrefab;

			// Token: 0x04002CA1 RID: 11425
			public string controller;

			// Token: 0x04002CA2 RID: 11426
			public int frame;

			// Token: 0x04002CA3 RID: 11427
			public int instanceId;

			// Token: 0x04002CA4 RID: 11428
			public int bikeId;

			// Token: 0x04002CA5 RID: 11429
			public int riderId;

			// Token: 0x04002CA6 RID: 11430
			public int groundRecoveries;

			// Token: 0x04002CA7 RID: 11431
			public int riderRideNumber;

			// Token: 0x04002CA8 RID: 11432
			public int distinctBikesUsed;

			// Token: 0x04002CA9 RID: 11433
			public int previousBikeId;

			// Token: 0x04002CAA RID: 11434
			public int firstPersonPoseFrame;

			// Token: 0x04002CAB RID: 11435
			public float realtime;

			// Token: 0x04002CAC RID: 11436
			public float gameTime;

			// Token: 0x04002CAD RID: 11437
			public float timeScale;

			// Token: 0x04002CAE RID: 11438
			public float tilt;

			// Token: 0x04002CAF RID: 11439
			public float throttle;

			// Token: 0x04002CB0 RID: 11440
			public float steering;

			// Token: 0x04002CB1 RID: 11441
			public float speed;

			// Token: 0x04002CB2 RID: 11442
			public float motorForce;

			// Token: 0x04002CB3 RID: 11443
			public float steerAngle;

			// Token: 0x04002CB4 RID: 11444
			public float leanAngle;

			// Token: 0x04002CB5 RID: 11445
			public float mountStabilizationRemaining;

			// Token: 0x04002CB6 RID: 11446
			public bool active;

			// Token: 0x04002CB7 RID: 11447
			public bool mounted;

			// Token: 0x04002CB8 RID: 11448
			public bool transitioning;

			// Token: 0x04002CB9 RID: 11449
			public bool hasBothControllers;

			// Token: 0x04002CBA RID: 11450
			public bool enabled;

			// Token: 0x04002CBB RID: 11451
			public bool inControl;

			// Token: 0x04002CBC RID: 11452
			public bool braking;

			// Token: 0x04002CBD RID: 11453
			public bool triggerContact;

			// Token: 0x04002CBE RID: 11454
			public bool hasCurrentRider;

			// Token: 0x04002CBF RID: 11455
			public bool hasGroundSafety;

			// Token: 0x04002CC0 RID: 11456
			public bool hasLifetime;

			// Token: 0x04002CC1 RID: 11457
			public bool lifetimeExpiring;

			// Token: 0x04002CC2 RID: 11458
			public float lifetimeRemaining;

			// Token: 0x04002CC3 RID: 11459
			public Vector3 lastSafePosition;

			// Token: 0x04002CC4 RID: 11460
			public Vector3 position;

			// Token: 0x04002CC5 RID: 11461
			public Vector3 euler;

			// Token: 0x04002CC6 RID: 11462
			public Vector3 scale;

			// Token: 0x04002CC7 RID: 11463
			public Vector3 bodyPosition;

			// Token: 0x04002CC8 RID: 11464
			public Vector3 velocity;

			// Token: 0x04002CC9 RID: 11465
			public Vector3 angularVelocity;

			// Token: 0x04002CCA RID: 11466
			public bool bodyKinematic;

			// Token: 0x04002CCB RID: 11467
			public bool sleeping;

			// Token: 0x04002CCC RID: 11468
			public string constraints;

			// Token: 0x04002CCD RID: 11469
			public string collisionMode;

			// Token: 0x04002CCE RID: 11470
			public string interpolation;

			// Token: 0x04002CCF RID: 11471
			public BicycleRideDiagnostics.WheelSnapshot front;

			// Token: 0x04002CD0 RID: 11472
			public BicycleRideDiagnostics.WheelSnapshot rear;

			// Token: 0x04002CD1 RID: 11473
			public string riderName;

			// Token: 0x04002CD2 RID: 11474
			public string riderParent;

			// Token: 0x04002CD3 RID: 11475
			public string activeResources;

			// Token: 0x04002CD4 RID: 11476
			public string blockedActivities;

			// Token: 0x04002CD5 RID: 11477
			public Vector3 riderPosition;

			// Token: 0x04002CD6 RID: 11478
			public Vector3 riderLocalPosition;

			// Token: 0x04002CD7 RID: 11479
			public Vector3 riderEuler;

			// Token: 0x04002CD8 RID: 11480
			public Vector3 headPosition;

			// Token: 0x04002CD9 RID: 11481
			public Vector3 eyePosition;

			// Token: 0x04002CDA RID: 11482
			public Vector3 seatPosition;

			// Token: 0x04002CDB RID: 11483
			public Vector3 expectedRiderPosition;

			// Token: 0x04002CDC RID: 11484
			public bool inputLocked;

			// Token: 0x04002CDD RID: 11485
			public bool movementLocked;

			// Token: 0x04002CDE RID: 11486
			public bool rotationLocked;

			// Token: 0x04002CDF RID: 11487
			public bool animationMovementLocked;

			// Token: 0x04002CE0 RID: 11488
			public bool animationRotationLocked;

			// Token: 0x04002CE1 RID: 11489
			public bool riderParented;

			// Token: 0x04002CE2 RID: 11490
			public bool riderKinematic;

			// Token: 0x04002CE3 RID: 11491
			public bool riderCapsuleEnabled;

			// Token: 0x04002CE4 RID: 11492
			public bool ragdolled;

			// Token: 0x04002CE5 RID: 11493
			public bool hasSeat;

			// Token: 0x04002CE6 RID: 11494
			public bool poseActive;

			// Token: 0x04002CE7 RID: 11495
			public string posePreset;

			// Token: 0x04002CE8 RID: 11496
			public string poseState;

			// Token: 0x04002CE9 RID: 11497
			public string targetPose;

			// Token: 0x04002CEA RID: 11498
			public float poseBlend;

			// Token: 0x04002CEB RID: 11499
			public float riderSeatError;

			// Token: 0x04002CEC RID: 11500
			public float vehicleRigWeight;

			// Token: 0x04002CED RID: 11501
			public float coverRigWeight;

			// Token: 0x04002CEE RID: 11502
			public bool animatorEnabled;

			// Token: 0x04002CEF RID: 11503
			public bool applyRootMotion;

			// Token: 0x04002CF0 RID: 11504
			public float animatorSpeed;

			// Token: 0x04002CF1 RID: 11505
			public string animatorUpdateMode;

			// Token: 0x04002CF2 RID: 11506
			public string animatorController;

			// Token: 0x04002CF3 RID: 11507
			public BicycleRideDiagnostics.AnimationLayer[] animationLayers;

			// Token: 0x04002CF4 RID: 11508
			public string[] animationParameters;

			// Token: 0x04002CF5 RID: 11509
			public BicycleRideDiagnostics.LimbSnapshot[] limbs;

			// Token: 0x04002CF6 RID: 11510
			public string cameraName;

			// Token: 0x04002CF7 RID: 11511
			public string cameraState;

			// Token: 0x04002CF8 RID: 11512
			public string cameraTarget;

			// Token: 0x04002CF9 RID: 11513
			public string externalCameraAnchor;

			// Token: 0x04002CFA RID: 11514
			public Vector3 cameraPosition;

			// Token: 0x04002CFB RID: 11515
			public Vector3 cameraEuler;

			// Token: 0x04002CFC RID: 11516
			public float cameraFov;

			// Token: 0x04002CFD RID: 11517
			public float cameraNear;

			// Token: 0x04002CFE RID: 11518
			public float eyeToCameraDistance;

			// Token: 0x04002CFF RID: 11519
			public bool firstPerson;

			// Token: 0x04002D00 RID: 11520
			public bool cameraFrozen;

			// Token: 0x04002D01 RID: 11521
			public bool calibrationOpen;

			// Token: 0x04002D02 RID: 11522
			public bool calibrationPreview;
		}

		// Token: 0x020006F7 RID: 1783
		[Serializable]
		private sealed class WheelSnapshot
		{
			// Token: 0x04002D03 RID: 11523
			public bool exists;

			// Token: 0x04002D04 RID: 11524
			public bool enabled;

			// Token: 0x04002D05 RID: 11525
			public bool grounded;

			// Token: 0x04002D06 RID: 11526
			public bool supportLayerIgnored;

			// Token: 0x04002D07 RID: 11527
			public bool effectSlip;

			// Token: 0x04002D08 RID: 11528
			public string name;

			// Token: 0x04002D09 RID: 11529
			public string contact;

			// Token: 0x04002D0A RID: 11530
			public string supportBelow;

			// Token: 0x04002D0B RID: 11531
			public int layer;

			// Token: 0x04002D0C RID: 11532
			public int attachedBody;

			// Token: 0x04002D0D RID: 11533
			public int supportLayer;

			// Token: 0x04002D0E RID: 11534
			public float radius;

			// Token: 0x04002D0F RID: 11535
			public float suspension;

			// Token: 0x04002D10 RID: 11536
			public float rpm;

			// Token: 0x04002D11 RID: 11537
			public float motorTorque;

			// Token: 0x04002D12 RID: 11538
			public float brakeTorque;

			// Token: 0x04002D13 RID: 11539
			public float contactForce;

			// Token: 0x04002D14 RID: 11540
			public float forwardSlip;

			// Token: 0x04002D15 RID: 11541
			public float sidewaysSlip;

			// Token: 0x04002D16 RID: 11542
			public float forceAppPointDistance;

			// Token: 0x04002D17 RID: 11543
			public float spring;

			// Token: 0x04002D18 RID: 11544
			public float damper;

			// Token: 0x04002D19 RID: 11545
			public float targetPosition;

			// Token: 0x04002D1A RID: 11546
			public float sprungMass;

			// Token: 0x04002D1B RID: 11547
			public Vector3 position;

			// Token: 0x04002D1C RID: 11548
			public Vector3 contactPoint;

			// Token: 0x04002D1D RID: 11549
			public Vector3 contactNormal;

			// Token: 0x04002D1E RID: 11550
			public Vector3 supportPoint;
		}

		// Token: 0x020006F8 RID: 1784
		[Serializable]
		private sealed class AnimationLayer
		{
			// Token: 0x04002D1F RID: 11551
			public string name;

			// Token: 0x04002D20 RID: 11552
			public int hash;

			// Token: 0x04002D21 RID: 11553
			public int nextHash;

			// Token: 0x04002D22 RID: 11554
			public float normalizedTime;

			// Token: 0x04002D23 RID: 11555
			public float weight;

			// Token: 0x04002D24 RID: 11556
			public float nextNormalizedTime;

			// Token: 0x04002D25 RID: 11557
			public bool transitioning;

			// Token: 0x04002D26 RID: 11558
			public BicycleRideDiagnostics.ClipSnapshot[] clips;

			// Token: 0x04002D27 RID: 11559
			public BicycleRideDiagnostics.ClipSnapshot[] nextClips;
		}

		// Token: 0x020006F9 RID: 1785
		[Serializable]
		private sealed class ClipSnapshot
		{
			// Token: 0x04002D28 RID: 11560
			public string name;

			// Token: 0x04002D29 RID: 11561
			public float weight;
		}

		// Token: 0x020006FA RID: 1786
		[Serializable]
		private sealed class CaptureFailure
		{
			// Token: 0x04002D2A RID: 11562
			public string kind = "diagnostics.capture_failed";

			// Token: 0x04002D2B RID: 11563
			public string session;

			// Token: 0x04002D2C RID: 11564
			public string attemptedKind;

			// Token: 0x04002D2D RID: 11565
			public string detail;

			// Token: 0x04002D2E RID: 11566
			public string error;

			// Token: 0x04002D2F RID: 11567
			public int frame;

			// Token: 0x04002D30 RID: 11568
			public int instanceId;

			// Token: 0x04002D31 RID: 11569
			public float realtime;
		}

		// Token: 0x020006FB RID: 1787
		[Serializable]
		private sealed class LimbSnapshot
		{
			// Token: 0x04002D32 RID: 11570
			public string name;

			// Token: 0x04002D33 RID: 11571
			public bool hasBone;

			// Token: 0x04002D34 RID: 11572
			public bool hasTarget;

			// Token: 0x04002D35 RID: 11573
			public float weight;

			// Token: 0x04002D36 RID: 11574
			public float error;

			// Token: 0x04002D37 RID: 11575
			public Vector3 bone;

			// Token: 0x04002D38 RID: 11576
			public Vector3 target;
		}
	}
}
