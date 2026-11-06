using System;
using System.Collections;
using System.Collections.Generic;
using Features.PhoneCompanion;
using Features.RenderingPrecision;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x0200092F RID: 2351
	public class SubwayRideDirector : MonoBehaviour
	{
		// Token: 0x06003FB5 RID: 16309 RVA: 0x0017A36C File Offset: 0x0017856C
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.SubwayRideDirector_arriveConfig)
			{
				return this.arriveConfig;
			}
			throw new InvalidOperationException("Unsupported SubwayRideDirector POI binding: " + field.ToString());
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x0017A396 File Offset: 0x00178596
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.SubwayRideDirector_arriveConfig)
			{
				this.arriveConfig = (InteractionConfigSO)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported SubwayRideDirector POI binding: " + field.ToString());
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003FB7 RID: 16311 RVA: 0x0017A3C6 File Offset: 0x001785C6
		public float FirstArrivalDelaySeconds
		{
			get
			{
				return Mathf.Max(0f, this.initialDelaySeconds);
			}
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x0017A3D8 File Offset: 0x001785D8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			SubwayRideDirector._closedStations.Clear();
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06003FB9 RID: 16313 RVA: 0x0017A3E4 File Offset: 0x001785E4
		private Transform StationKey
		{
			get
			{
				if (!(base.transform.parent != null))
				{
					return base.transform;
				}
				return base.transform.parent;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06003FBA RID: 16314 RVA: 0x0017A40C File Offset: 0x0017860C
		private bool ServiceClosedHere
		{
			get
			{
				if (RunModeService.IsLongWayHome)
				{
					return true;
				}
				PoiResidencyStateKey poiResidencyStateKey;
				if (PoiResidencyPlacement.TryGetKey(this.StationKey, "subway-closed", out poiResidencyStateKey))
				{
					return PoiResidencyLedger.ReadFlag(poiResidencyStateKey);
				}
				SubwayRideDirector._closedStations.RemoveWhere((Transform t) => t == null);
				return SubwayRideDirector._closedStations.Contains(this.StationKey);
			}
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x0017A478 File Offset: 0x00178678
		private void Awake()
		{
			SubwayAudioLib.WarmAll();
			this._trainRb = base.GetComponent<Rigidbody>();
			this._carry = base.GetComponent<SubwayPlatformCarry>();
			if (this._carry == null)
			{
				this._carry = base.gameObject.AddComponent<SubwayPlatformCarry>();
			}
			this._carry.TrainRb = this._trainRb;
			GameObject gameObject = new GameObject("SubwayRumbleAnchor");
			gameObject.transform.SetParent(base.transform, false);
			this._rumbleAnchor = gameObject.transform;
			this._rumbleSource = gameObject.AddComponent<AudioSource>();
			this._rumbleSource.clip = ((this.rumbleOverride != null) ? this.rumbleOverride : SubwayAudioLib.RumbleLoop);
			this._rumbleSource.loop = true;
			this._rumbleSource.playOnAwake = false;
			SubwayRideDirector.ConfigureSpatial(this._rumbleSource);
			this._rumbleSource.volume = 0f;
			this._oneShotSource = gameObject.AddComponent<AudioSource>();
			this._oneShotSource.playOnAwake = false;
			SubwayRideDirector.ConfigureSpatial(this._oneShotSource);
			this._occlusion = SubwayAudioOcclusion.Attach(gameObject, base.transform);
		}

		// Token: 0x06003FBC RID: 16316 RVA: 0x0017A595 File Offset: 0x00178795
		private static void ConfigureSpatial(AudioSource s)
		{
			s.spatialBlend = 1f;
			SubwayAudioOcclusion.ApplyRolloffToZero(s, 3.5f, 35f);
			s.dopplerLevel = 0f;
		}

		// Token: 0x06003FBD RID: 16317 RVA: 0x0017A5BD File Offset: 0x001787BD
		private void Start()
		{
			if (this.doorSystem != null)
			{
				this.doorSystem.SetLocked(true);
			}
			this.CaptureStationPose();
			this._started = true;
			this.StartServiceWhenZoneOperational();
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x0017A5EC File Offset: 0x001787EC
		private void InitializeFirstOperationalService()
		{
			if (this.doorSystem != null)
			{
				this.doorSystem.PrepareForOperationalUse();
			}
			this.RestoreStationPoseForCurrentZone();
			this.BuildRidePrompts();
			List<Renderer> list = new List<Renderer>();
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
			{
				if ((!(renderer.transform.parent != null) || !(renderer.transform.parent.name == "SubwayTunnelFX")) && !(renderer.GetComponentInParent<InteractablePromptUI>() != null))
				{
					list.Add(renderer);
				}
			}
			this._bodyRenderers = list.ToArray();
			this._allColliders = base.GetComponentsInChildren<Collider>(true);
			List<Light> list2 = new List<Light>();
			foreach (Light light in base.GetComponentsInChildren<Light>(true))
			{
				if (!(light == null))
				{
					if (light.GetComponentInParent<SubwayPassengers>() != null)
					{
						light.enabled = false;
					}
					else
					{
						list2.Add(light);
					}
				}
			}
			this._lights = list2.ToArray();
			this.CacheBodyExtent();
			if (this._occlusion != null)
			{
				this._occlusion.ResetForReuse();
			}
			this._operationalInitialized = true;
			if (Application.isPlaying)
			{
				base.StartCoroutine(this.ServiceLoop());
			}
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x0017A735 File Offset: 0x00178935
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
			if (!this._started)
			{
				return;
			}
			this.StartServiceWhenZoneOperational();
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x0017A757 File Offset: 0x00178957
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			this.CancelPendingStagedRestart();
			this._serviceGeneration++;
			base.StopAllCoroutines();
			this.ReleaseCarriedPlayer();
			this._activePassenger = null;
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0017A791 File Offset: 0x00178991
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			if (this._stationPoseCaptured)
			{
				this._stationPos += delta;
			}
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x0017A7AD File Offset: 0x001789AD
		private void HandleStagedZoneOperationalReady(IncrementalPooledZoneActivation stagedActivation)
		{
			if (stagedActivation == null || stagedActivation != this._pendingStagedActivation)
			{
				return;
			}
			this.CancelPendingStagedRestart();
			if (base.isActiveAndEnabled)
			{
				this.StartServiceWhenZoneOperational();
			}
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x0017A7DB File Offset: 0x001789DB
		private void HandleHeavyZoneOperationalReady(HeavyZoneStreamingProxy proxy)
		{
			if (proxy == null || proxy != this._pendingHeavyActivation)
			{
				return;
			}
			this.CancelPendingStagedRestart();
			if (base.isActiveAndEnabled)
			{
				this.StartServiceWhenZoneOperational();
			}
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x0017A80C File Offset: 0x00178A0C
		private void StartServiceWhenZoneOperational()
		{
			this.CancelPendingStagedRestart();
			HeavyZoneStreamingProxy componentInParent = base.GetComponentInParent<HeavyZoneStreamingProxy>();
			if (componentInParent != null && !componentInParent.IsOperationalReady)
			{
				this._pendingHeavyActivation = componentInParent;
				componentInParent.OperationalReady += this.HandleHeavyZoneOperationalReady;
				return;
			}
			IncrementalPooledZoneActivation componentInParent2 = base.GetComponentInParent<IncrementalPooledZoneActivation>();
			if (componentInParent2 != null && componentInParent2.HasSnapshot && !componentInParent2.IsOperationalReady)
			{
				this._pendingStagedActivation = componentInParent2;
				this._pendingStagedActivation.OperationalReady += this.HandleStagedZoneOperationalReady;
				return;
			}
			this.StartOrResumeOperationalService();
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x0017A896 File Offset: 0x00178A96
		private void StartOrResumeOperationalService()
		{
			if (!this._operationalInitialized)
			{
				this.InitializeFirstOperationalService();
				return;
			}
			this.ResumeServiceAfterPoolWake();
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x0017A8B0 File Offset: 0x00178AB0
		private void CancelPendingStagedRestart()
		{
			if (this._pendingHeavyActivation != null)
			{
				this._pendingHeavyActivation.OperationalReady -= this.HandleHeavyZoneOperationalReady;
			}
			this._pendingHeavyActivation = null;
			if (this._pendingStagedActivation != null)
			{
				this._pendingStagedActivation.OperationalReady -= this.HandleStagedZoneOperationalReady;
			}
			this._pendingStagedActivation = null;
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x0017A918 File Offset: 0x00178B18
		private void ResumeServiceAfterPoolWake()
		{
			this.RestoreStationPoseForCurrentZone();
			if (this.doorSystem != null)
			{
				this.doorSystem.PrepareForOperationalUse();
			}
			if (this._occlusion != null)
			{
				this._occlusion.ResetForReuse();
			}
			SubwayRideDirector._closedStations.Remove(this.StationKey);
			this.RestartService();
			Debug.Log("[SubwayRide] '" + base.name + "' riattivato dal pool: servizio riavviato da capo " + string.Format("alla fermata {0}.", this._stationPos));
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x0017A9A3 File Offset: 0x00178BA3
		private void CaptureStationPose()
		{
			this._stationLocalPos = base.transform.localPosition;
			this._stationLocalRotation = base.transform.localRotation;
			this._stationPoseCaptured = true;
			this.RefreshStationWorldPose();
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x0017A9D4 File Offset: 0x00178BD4
		private void RestoreStationPoseForCurrentZone()
		{
			if (this._stationPoseCaptured)
			{
				base.transform.localPosition = this._stationLocalPos;
				base.transform.localRotation = this._stationLocalRotation;
				if (this._trainRb != null)
				{
					this._trainRb.position = base.transform.position;
					this._trainRb.rotation = base.transform.rotation;
					if (!this._trainRb.isKinematic)
					{
						this._trainRb.velocity = Vector3.zero;
						this._trainRb.angularVelocity = Vector3.zero;
					}
				}
				Physics.SyncTransforms();
			}
			this.RefreshStationWorldPose();
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x0017AA80 File Offset: 0x00178C80
		private void RefreshStationWorldPose()
		{
			this._stationPos = base.transform.position;
			this._worldDir = base.transform.TransformDirection(this.localTravelDir.normalized);
			this._scrollSign = ((this.localTravelDir.x >= 0f) ? 1f : (-1f));
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x0017AAE0 File Offset: 0x00178CE0
		public void RestartService()
		{
			if (!this._started)
			{
				return;
			}
			this._serviceGeneration++;
			base.StopAllCoroutines();
			this._rumbleFade = null;
			this._rumbleMute = 0f;
			this._drivenVolume = 0f;
			if (this._rumbleSource != null)
			{
				this._rumbleSource.Stop();
				this._rumbleSource.volume = 0f;
			}
			this.ReleaseCarriedPlayer();
			this._activePassenger = null;
			this._recoveryInterrupted = false;
			this._arriveRequested = false;
			this._skipPhase = false;
			this.HideRidePrompt();
			if (this.doorSystem != null)
			{
				this.doorSystem.ResetToClosed();
			}
			this.SnapTo(this._stationPos);
			this.SetTrainPresent(false);
			if (Application.isPlaying)
			{
				base.StartCoroutine(this.ServiceLoop());
			}
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x0017ABB8 File Offset: 0x00178DB8
		private void Update()
		{
			this.UpdateRumbleAnchor();
			if (Time.unscaledTime >= this._nextPresenceIntegrityCheck)
			{
				this._nextPresenceIntegrityCheck = Time.unscaledTime + 0.25f;
				this.ReassertPresenceIfCulled();
			}
			this.UpdateRidePromptProximity();
			if (this._promptShown && InteractionCandidateRegistry.CanStartWorldInteraction && this.rideInput != null && this.rideInput.GetButtonDown())
			{
				this._arriveRequested = true;
			}
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x0017AC20 File Offset: 0x00178E20
		private void CacheBodyExtent()
		{
			bool flag = false;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			foreach (Renderer renderer in this._bodyRenderers)
			{
				if (!(renderer == null))
				{
					Bounds bounds = renderer.bounds;
					for (int j = 0; j < 8; j++)
					{
						Vector3 vector3 = new Vector3(((j & 1) == 0) ? bounds.min.x : bounds.max.x, ((j & 2) == 0) ? bounds.min.y : bounds.max.y, ((j & 4) == 0) ? bounds.min.z : bounds.max.z);
						Vector3 vector4 = base.transform.InverseTransformPoint(vector3);
						if (!flag)
						{
							vector2 = (vector = vector4);
							flag = true;
						}
						else
						{
							vector = Vector3.Min(vector, vector4);
							vector2 = Vector3.Max(vector2, vector4);
						}
					}
				}
			}
			if (!flag)
			{
				this._bodyLocalMinX = (this._bodyLocalMaxX = 0f);
				this._bodyLocalCenter = Vector3.zero;
				return;
			}
			this._bodyLocalMinX = vector.x;
			this._bodyLocalMaxX = vector2.x;
			this._bodyLocalCenter = (vector + vector2) * 0.5f;
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x0017AD74 File Offset: 0x00178F74
		private void UpdateRumbleAnchor()
		{
			if (this._rumbleAnchor == null)
			{
				return;
			}
			Camera main = Camera.main;
			Transform transform = ((main != null) ? main.transform : PlayerRegistry.Player);
			if (transform == null)
			{
				return;
			}
			float x = base.transform.InverseTransformPoint(transform.position).x;
			this._rumbleAnchor.localPosition = new Vector3(Mathf.Clamp(x, this._bodyLocalMinX, this._bodyLocalMaxX), this._bodyLocalCenter.y, this._bodyLocalCenter.z);
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x0017AE08 File Offset: 0x00179008
		private void ReassertPresenceIfCulled()
		{
			if (this._bodyRenderers == null || this._allColliders == null || this._lights == null)
			{
				return;
			}
			bool flag = false;
			int num = 0;
			while (num < this._bodyRenderers.Length && !flag)
			{
				Renderer renderer = this._bodyRenderers[num];
				if (renderer != null && renderer.enabled != this._trainPresent)
				{
					flag = true;
				}
				num++;
			}
			int num2 = 0;
			while (num2 < this._allColliders.Length && !flag)
			{
				Collider collider = this._allColliders[num2];
				if (collider != null && collider.enabled != this._trainPresent)
				{
					flag = true;
				}
				num2++;
			}
			int num3 = 0;
			while (num3 < this._lights.Length && !flag)
			{
				Light light = this._lights[num3];
				if (light != null && light.enabled != this._trainPresent)
				{
					flag = true;
				}
				num3++;
			}
			if (flag)
			{
				this.ApplyPresence(this._trainPresent);
			}
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x0017AEF4 File Offset: 0x001790F4
		private void CloseStationService()
		{
			this.RememberStationClosed();
			foreach (SubwayRideDirector subwayRideDirector in Object.FindObjectsOfType<SubwayRideDirector>(true))
			{
				if (!(subwayRideDirector == this) && !(subwayRideDirector.StationKey != this.StationKey))
				{
					subwayRideDirector.RetireImmediately();
				}
			}
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x0017AF44 File Offset: 0x00179144
		private void RememberStationClosed()
		{
			PoiResidencyStateKey poiResidencyStateKey;
			if (PoiResidencyPlacement.TryGetKey(this.StationKey, "subway-closed", out poiResidencyStateKey))
			{
				PoiResidencyLedger.WriteFlag(poiResidencyStateKey, true);
				return;
			}
			SubwayRideDirector._closedStations.Add(this.StationKey);
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x0017AF80 File Offset: 0x00179180
		private void ReopenStationAfterSuccessfulTransfer()
		{
			SubwayRideDirector._closedStations.Remove(this.StationKey);
			PoiResidencyStateKey poiResidencyStateKey;
			if (PoiResidencyPlacement.TryGetKey(this.StationKey, "subway-closed", out poiResidencyStateKey))
			{
				PoiResidencyLedger.WriteFlag(poiResidencyStateKey, false);
			}
			foreach (SubwayRideDirector subwayRideDirector in Object.FindObjectsOfType<SubwayRideDirector>(true))
			{
				if (subwayRideDirector.StationKey == this.StationKey && subwayRideDirector.isActiveAndEnabled)
				{
					subwayRideDirector.RestartService();
				}
			}
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x0017AFF4 File Offset: 0x001791F4
		private void RetireImmediately()
		{
			base.StopAllCoroutines();
			this._rumbleFade = null;
			if (this._rumbleSource != null)
			{
				this._rumbleSource.Stop();
				this._rumbleSource.volume = 0f;
			}
			this.HideRidePrompt();
			this.SetTrainPresent(false);
			Debug.Log("[SubwayRide] '" + base.name + "' ritirato: il player sta viaggiando su un altro treno di questa stazione.");
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003FD4 RID: 16340 RVA: 0x0017B05E File Offset: 0x0017925E
		private static bool IsPhoneInUse
		{
			get
			{
				return SmartphoneController.IsAnyPhoneOut;
			}
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x0017B068 File Offset: 0x00179268
		private float NextAwaySeconds()
		{
			float num = Mathf.Max(0f, Mathf.Min(this.awaySecondsMin, this.awaySecondsMax));
			float num2 = Mathf.Max(this.awaySecondsMin, this.awaySecondsMax);
			return Random.Range(num, num2);
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0017B0A8 File Offset: 0x001792A8
		public void SkipPhase()
		{
			this._skipPhase = true;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0017B0B1 File Offset: 0x001792B1
		public void RequestArrival()
		{
			this._arriveRequested = true;
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0017B0BC File Offset: 0x001792BC
		public bool InterruptForUnstuck()
		{
			Transform player = PlayerRegistry.Player;
			if (player == null || !base.isActiveAndEnabled || this._recoveryInterrupted)
			{
				return false;
			}
			bool flag = this._activePassenger == player || (this._carry != null && this._carry.PlayerRb != null && this._carry.PlayerRb.transform == player);
			if (!flag && (!this._trainPresent || (!this.PlayerAboard() && !this.PlayerInTrainVolume(base.transform.position))))
			{
				return false;
			}
			this._recoveryInterrupted = true;
			this._serviceGeneration++;
			base.StopAllCoroutines();
			this.ReleaseCarriedPlayer();
			this._activePassenger = null;
			this._rumbleFade = null;
			this._rumbleMute = (this._drivenVolume = 0f);
			if (this._rumbleSource != null)
			{
				this._rumbleSource.Stop();
				this._rumbleSource.volume = 0f;
			}
			this._arriveRequested = (this._skipPhase = false);
			this.HideRidePrompt();
			if (this.scroller != null)
			{
				this.scroller.SetSpeed(0f);
				this.scroller.Hide(0f);
			}
			if (flag)
			{
				SubwayRideGuard.End();
				SubwayCameraShaker.SetIntensity(0f);
				SubwayScreenFade.ClearImmediately();
				SubwayTravelService.CompleteTrip();
			}
			this.SnapTo(this._stationPos);
			this.SetTrainPresent(true);
			this.RememberStationClosed();
			if (this.doorSystem != null)
			{
				this.doorSystem.OpenAll();
			}
			if (Application.isPlaying && base.isActiveAndEnabled && this.doorSystem != null)
			{
				base.StartCoroutine(this.DepartTerminalWhenClear());
			}
			return true;
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x0017B287 File Offset: 0x00179487
		public bool TrainPresent
		{
			get
			{
				return this._trainPresent;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06003FDA RID: 16346 RVA: 0x0017B28F File Offset: 0x0017948F
		public bool HasActiveResidencyRide
		{
			get
			{
				return this._activePassenger != null || this._carriedPlayer != null || (this._trainPresent && this.PlayerOccupiesTerminalTrain());
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003FDB RID: 16347 RVA: 0x0017B2BF File Offset: 0x001794BF
		public bool ArrivalRequestAvailable
		{
			get
			{
				return this._promptShown;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003FDC RID: 16348 RVA: 0x0017B2C7 File Offset: 0x001794C7
		public bool ServiceClosed
		{
			get
			{
				return this.ServiceClosedHere;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003FDD RID: 16349 RVA: 0x0017B2CF File Offset: 0x001794CF
		public bool DoorsOpen
		{
			get
			{
				return this.doorSystem != null && this.doorSystem.DoorsOpen;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06003FDE RID: 16350 RVA: 0x0017B2EC File Offset: 0x001794EC
		public SubwayDoorSystem DoorSystem
		{
			get
			{
				return this.doorSystem;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003FDF RID: 16351 RVA: 0x0017B2F4 File Offset: 0x001794F4
		public Collider InteriorZone
		{
			get
			{
				return this.interiorZone;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003FE0 RID: 16352 RVA: 0x0017B2FC File Offset: 0x001794FC
		public Vector3 TravelDirection
		{
			get
			{
				return this._worldDir;
			}
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x0017B304 File Offset: 0x00179504
		public bool TryGetArrivalCabin(out Vector3 center, out Vector3 axis, out float halfLength)
		{
			center = (axis = Vector3.zero);
			halfLength = 0f;
			BoxCollider boxCollider = this.interiorZone as BoxCollider;
			if (boxCollider == null || !this._stationPoseCaptured)
			{
				return false;
			}
			center = boxCollider.transform.TransformPoint(boxCollider.center) + (this._stationPos - base.transform.position);
			bool flag = boxCollider.size.x >= boxCollider.size.z;
			axis = boxCollider.transform.TransformDirection(flag ? Vector3.right : Vector3.forward).normalized;
			Vector3 lossyScale = boxCollider.transform.lossyScale;
			halfLength = (flag ? (boxCollider.size.x * Mathf.Abs(lossyScale.x)) : (boxCollider.size.z * Mathf.Abs(lossyScale.z))) * 0.5f - 1f;
			return halfLength > 0f;
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003FE2 RID: 16354 RVA: 0x0017B411 File Offset: 0x00179611
		public AudioSource RumbleAudioSource
		{
			get
			{
				return this._rumbleSource;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003FE3 RID: 16355 RVA: 0x0017B419 File Offset: 0x00179619
		public AudioSource OneShotAudioSource
		{
			get
			{
				return this._oneShotSource;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06003FE4 RID: 16356 RVA: 0x0017B421 File Offset: 0x00179621
		public int BodyRendererCount
		{
			get
			{
				if (this._bodyRenderers == null)
				{
					return 0;
				}
				return this._bodyRenderers.Length;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06003FE5 RID: 16357 RVA: 0x0017B438 File Offset: 0x00179638
		public int EnabledBodyRendererCount
		{
			get
			{
				if (this._bodyRenderers == null)
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this._bodyRenderers.Length; i++)
				{
					if (this._bodyRenderers[i] != null && this._bodyRenderers[i].enabled)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x0017B488 File Offset: 0x00179688
		private void ShowRidePrompt()
		{
			if (this._ridePrompts.Count == 0 || this._promptShown)
			{
				return;
			}
			for (int i = 0; i < this._promptNear.Length; i++)
			{
				this._promptNear[i] = false;
			}
			foreach (InteractablePromptUI interactablePromptUI in this._ridePrompts)
			{
				if (interactablePromptUI != null)
				{
					interactablePromptUI.Hide();
				}
			}
			this._promptShown = true;
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x0017B51C File Offset: 0x0017971C
		private void HideRidePrompt()
		{
			if (!this._promptShown)
			{
				return;
			}
			for (int i = 0; i < this._promptNear.Length; i++)
			{
				this._promptNear[i] = false;
			}
			foreach (InteractablePromptUI interactablePromptUI in this._ridePrompts)
			{
				if (interactablePromptUI != null)
				{
					interactablePromptUI.Hide();
				}
			}
			this._promptShown = false;
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0017B5A4 File Offset: 0x001797A4
		private void UpdateRidePromptProximity()
		{
			if (!this._promptShown)
			{
				return;
			}
			Transform player = PlayerRegistry.Player;
			if (player == null)
			{
				return;
			}
			float num = this.promptVisibleDistance * this.promptVisibleDistance;
			for (int i = 0; i < this._ridePrompts.Count; i++)
			{
				InteractablePromptUI interactablePromptUI = this._ridePrompts[i];
				if (!(interactablePromptUI == null))
				{
					Vector3 vector = interactablePromptUI.transform.position - player.position;
					bool flag = vector.x * vector.x + vector.z * vector.z <= num;
					if (flag != this._promptNear[i])
					{
						this._promptNear[i] = flag;
						if (flag)
						{
							if (this.arriveConfig != null)
							{
								interactablePromptUI.Show(this.arriveConfig);
							}
							else
							{
								interactablePromptUI.gameObject.SetActive(true);
							}
						}
						else
						{
							interactablePromptUI.Hide();
						}
					}
				}
			}
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x0017B698 File Offset: 0x00179898
		private void BuildRidePrompts()
		{
			this._ridePrompts.Clear();
			if (this.authoredDoorPrompts != null)
			{
				foreach (InteractablePromptUI interactablePromptUI in this.authoredDoorPrompts)
				{
					if (interactablePromptUI != null)
					{
						this._ridePrompts.Add(interactablePromptUI);
					}
				}
			}
			if (this._ridePrompts.Count > 0)
			{
				this._promptNear = new bool[this._ridePrompts.Count];
				foreach (InteractablePromptUI interactablePromptUI2 in this._ridePrompts)
				{
					interactablePromptUI2.Hide();
				}
				Debug.Log(string.Format("[SubwayRide] '{0}': {1} prompt di fermata autorati in scena.", base.name, this._ridePrompts.Count));
				return;
			}
			if (this.ridePromptUI == null)
			{
				return;
			}
			this._ridePrompts.Add(this.ridePromptUI);
			List<Vector3> list = this.ResolveDoorCenters();
			if (list.Count == 0)
			{
				this._promptNear = new bool[this._ridePrompts.Count];
				Debug.LogWarning("[SubwayRide] '" + base.name + "': nessuna porta riconosciuta, resta il solo prompt autorato al centro della carrozza.", this);
				return;
			}
			Transform transform = this.ridePromptUI.transform;
			Vector3 vector = base.transform.InverseTransformPoint(transform.position);
			for (int j = 0; j < list.Count; j++)
			{
				Vector3 vector2 = base.transform.InverseTransformPoint(list[j]);
				Vector3 vector3 = new Vector3(vector2.x, vector.y, vector.z);
				if (j == 0)
				{
					transform.position = base.transform.TransformPoint(vector3);
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(transform.gameObject, transform.parent);
					gameObject.name = string.Format("{0}_Door{1}", transform.name, j);
					gameObject.transform.position = base.transform.TransformPoint(vector3);
					gameObject.transform.rotation = transform.rotation;
					gameObject.transform.localScale = transform.localScale;
					InteractablePromptUI component = gameObject.GetComponent<InteractablePromptUI>();
					if (component != null)
					{
						this._ridePrompts.Add(component);
					}
				}
			}
			this._promptNear = new bool[this._ridePrompts.Count];
			foreach (InteractablePromptUI interactablePromptUI3 in this._ridePrompts)
			{
				interactablePromptUI3.Hide();
			}
			Debug.Log(string.Format("[SubwayRide] '{0}': {1} prompt di fermata, uno per porta.", base.name, this._ridePrompts.Count));
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x0017B964 File Offset: 0x00179B64
		private List<Vector3> ResolveDoorCenters()
		{
			List<Vector3> list = new List<Vector3>();
			if (this.doorSystem == null)
			{
				return list;
			}
			Bounds bounds = default(Bounds);
			bool flag = this.interiorZone != null;
			if (flag)
			{
				bounds = this.interiorZone.bounds;
				bounds.Expand(new Vector3(1.5f, 4f, 1.5f));
			}
			List<Vector3> list2 = new List<Vector3>();
			foreach (SubwayDoorPanel subwayDoorPanel in this.doorSystem.OpeningPanels)
			{
				if (!(((subwayDoorPanel != null) ? subwayDoorPanel.panel : null) == null))
				{
					Vector3 vector = SubwayDoorSystem.DoorwayWorldCenter(subwayDoorPanel);
					if (!flag || bounds.Contains(vector))
					{
						list2.Add(vector);
					}
				}
			}
			foreach (Vector3 vector2 in list2)
			{
				bool flag2 = false;
				for (int i = 0; i < list.Count; i++)
				{
					if (Vector3.Distance(list[i], vector2) <= this.doorGroupingDistance)
					{
						list[i] = (list[i] + vector2) * 0.5f;
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					list.Add(vector2);
				}
			}
			return list;
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x0017BAE0 File Offset: 0x00179CE0
		private IEnumerator ServiceLoop()
		{
			int generation = this._serviceGeneration;
			yield return null;
			this.SetTrainPresent(false);
			Debug.Log(string.Format("[SubwayRide] service start (away) t={0:F1}", Time.time));
			yield return this.WaitSkippable(this.FirstArrivalDelaySeconds, true);
			for (;;)
			{
				float rollDist = this.emptyRollTopSpeed * this.emptyRollSeconds * 0.5f + this.emptyRollTopSpeed * this.vanishExtraSeconds;
				while (!this.PlayerInServiceCell() && !this.PlayerAboard() && !this.ServiceClosedHere)
				{
					yield return new WaitForSeconds(0.5f);
				}
				if (this.ServiceClosedHere)
				{
					break;
				}
				while (this.PlayerInTrainVolume(this._stationPos - this._worldDir * rollDist) || this.PlayerInTrainVolume(this._stationPos))
				{
					yield return new WaitForSeconds(0.5f);
				}
				this.SnapTo(this._stationPos - this._worldDir * rollDist);
				this.SetTrainPresent(true);
				this.StartRumble();
				yield return this.Roll(this.emptyRollTopSpeed, this.emptyRollTopSpeed, this.vanishExtraSeconds, null);
				yield return this.Roll(this.emptyRollTopSpeed, 0f, this.emptyRollSeconds, null);
				this.SnapTo(this._stationPos);
				this.Drive(0f, false);
				this.StopRumble();
				Debug.Log(string.Format("[SubwayRide] arrived t={0:F1} pos={1}", Time.time, base.transform.position));
				yield return new WaitForSeconds(0.5f);
				this.PlayChime();
				yield return new WaitForSeconds(0.5f);
				this.doorSystem.OpenAll();
				while (this.doorSystem.Moving)
				{
					yield return null;
				}
				yield return this.WaitSkippable(this.dwellOpenSeconds, false);
				this.PlayChime();
				yield return new WaitForSeconds(this.closeWarningDelay);
				this.doorSystem.CloseAll();
				while (this.doorSystem.Moving)
				{
					yield return null;
				}
				yield return new WaitForSeconds(0.4f);
				bool flag = this.PlayerAboard();
				Transform player = PlayerRegistry.Player;
				Debug.Log(string.Concat(new string[]
				{
					string.Format("[SubwayRide] '{0}' close check: aboard={1} ", base.name, flag),
					"player=",
					(player != null) ? player.position.ToString("F2") : "NULL",
					" zone=",
					(this.interiorZone != null) ? (this.interiorZone.bounds.center.ToString("F2") + "/" + this.interiorZone.bounds.size.ToString("F2")) : "NULL"
				}));
				if (!flag && this.PlayerInTrainVolume(this._stationPos))
				{
					float held = 0f;
					while (this.PlayerInTrainVolume(this._stationPos) && held < 8f)
					{
						held += Time.deltaTime;
						yield return null;
					}
					flag = this.PlayerAboard() || this.PlayerInTrainVolume(this._stationPos);
				}
				if (flag)
				{
					goto Block_14;
				}
				this.StartRumble();
				yield return this.Roll(0f, this.emptyRollTopSpeed, this.emptyRollSeconds, null);
				yield return this.Roll(this.emptyRollTopSpeed, this.emptyRollTopSpeed, this.vanishExtraSeconds, null);
				this.SetTrainPresent(false);
				this.SnapTo(this._stationPos);
				this.Drive(0f, false);
				this.StopRumble();
				Debug.Log(string.Format("[SubwayRide] departed empty t={0:F1}", Time.time));
				yield return this.WaitSkippable(this.NextAwaySeconds(), true);
			}
			this.SetTrainPresent(false);
			Debug.Log("[SubwayRide] '" + base.name + "' service closed — no more arrivals.");
			yield break;
			Block_14:
			yield return this.ExperiencedRide();
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			if (this._passengerTransferred)
			{
				yield break;
			}
			yield return this.DepartTerminalWhenClear();
			yield break;
			yield break;
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x0017BAEF File Offset: 0x00179CEF
		private IEnumerator DepartTerminalWhenClear()
		{
			int generation = this._serviceGeneration;
			if (!this._connectingArrival)
			{
				this.RememberStationClosed();
			}
			yield return new WaitForSeconds(0.4f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			this.PlayChime();
			yield return new WaitForSeconds(0.5f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			yield return this.WaitForTerminalClearance();
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			this.StartRumble();
			yield return this.Roll(0f, this.emptyRollTopSpeed, this.emptyRollSeconds, null);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			yield return this.Roll(this.emptyRollTopSpeed, this.emptyRollTopSpeed, this.vanishExtraSeconds, null);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			this.SetTrainPresent(false);
			this.SnapTo(this._stationPos);
			this.Drive(0f, false);
			this.StopRumble();
			yield break;
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0017BAFE File Offset: 0x00179CFE
		private bool PlayerOccupiesTerminalTrain()
		{
			return this.PlayerAboard() || this.PlayerInTrainVolume(this._stationPos);
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0017BB16 File Offset: 0x00179D16
		private IEnumerator WaitForTerminalClearance()
		{
			int generation = this._serviceGeneration;
			while (generation == this._serviceGeneration)
			{
				this.doorSystem.OpenAll();
				while (this.doorSystem.Moving)
				{
					if (generation != this._serviceGeneration)
					{
						break;
					}
					yield return null;
				}
				while (this.PlayerOccupiesTerminalTrain() && generation == this._serviceGeneration)
				{
					yield return null;
				}
				if (generation != this._serviceGeneration)
				{
					yield break;
				}
				yield return new WaitForSeconds(0.6f);
				if (generation != this._serviceGeneration)
				{
					yield break;
				}
				if (!this.PlayerOccupiesTerminalTrain())
				{
					this.PlayChime();
					yield return new WaitForSeconds(this.closeWarningDelay);
					if (generation != this._serviceGeneration)
					{
						yield break;
					}
					if (!this.PlayerOccupiesTerminalTrain())
					{
						this.doorSystem.CloseAll();
						while (this.doorSystem.Moving && !this.PlayerOccupiesTerminalTrain() && generation == this._serviceGeneration)
						{
							yield return null;
						}
						if (generation != this._serviceGeneration)
						{
							yield break;
						}
						if (!this.PlayerOccupiesTerminalTrain())
						{
							yield break;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x0017BB25 File Offset: 0x00179D25
		private IEnumerator ExperiencedRide()
		{
			this._passengerTransferred = false;
			this._connectingArrival = false;
			int generation = this._serviceGeneration;
			Transform player = PlayerRegistry.Player;
			this._activePassenger = player;
			Rigidbody playerRb = ((player != null) ? player.GetComponent<Rigidbody>() : null);
			Debug.Log(string.Format("[SubwayRide] ride START t={0:F1} pos={1}", Time.time, base.transform.position));
			SubwayRideGuard.Begin(this);
			this.StartRumble();
			this._clackDistance = this.railJointSpacing * 0.5f;
			yield return this.Roll(0f, this.realMoveTopSpeed, this.realMoveSeconds, playerRb);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			Debug.Log(string.Format("[SubwayRide] launch roll done t={0:F1} pos={1}", Time.time, base.transform.position));
			yield return SubwayScreenFade.Fade(1f, 0.35f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			this.SnapTrainKeepingPlayerSeat(this._stationPos, player, playerRb);
			this.scroller.Show(0.05f);
			this.CloseStationService();
			SubwayTravelService.BeginTrip(this._worldDir, this.travelMeters, base.transform);
			yield return new WaitForSeconds(0.6f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			yield return SubwayScreenFade.Fade(0f, 0.45f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			yield return this.SpeedRamp(this.realMoveTopSpeed, this.cruiseSpeed, this.accelSeconds, true);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			this.ShowRidePrompt();
			this._arriveRequested = false;
			this._skipPhase = false;
			while (!this._arriveRequested && !this._skipPhase && generation == this._serviceGeneration)
			{
				this.Drive(this.cruiseSpeed, true);
				yield return null;
			}
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			this._arriveRequested = false;
			this._skipPhase = false;
			this.HideRidePrompt();
			this.PlayOneShot((this.brakeOverride != null) ? this.brakeOverride : SubwayAudioLib.Brake, this.brakeVolume);
			yield return this.SpeedRamp(this.cruiseSpeed, this.realMoveTopSpeed, this.brakeSeconds, true);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			float rollDist = this.realMoveTopSpeed * this.realMoveSeconds * 0.5f;
			yield return SubwayScreenFade.Fade(1f, 0.35f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			if (SubwayTravelService.HasPendingTrip)
			{
				float deadline = Time.realtimeSinceStartup + 30f;
				SubwayRideDirector subwayRideDirector;
				Vector3 vector;
				while (!SubwayTravelService.TryGetArrival(out subwayRideDirector, out vector) && Time.realtimeSinceStartup < deadline && generation == this._serviceGeneration)
				{
					yield return null;
				}
				if (generation != this._serviceGeneration)
				{
					yield break;
				}
				if (SubwayTravelService.TryGetArrival(out subwayRideDirector, out vector))
				{
					this.ReleaseCarriedPlayer();
					this._activePassenger = null;
					this._passengerTransferred = true;
					this.scroller.Hide(0f);
					this.Drive(0f, false);
					this.StopRumble();
					this.SetTrainPresent(false);
					this.SnapTo(this._stationPos);
					subwayRideDirector.AcceptConnectingPassenger(player, vector, this);
					yield break;
				}
				Debug.LogWarning("[SubwayTravel] Arrival platform did not become ready; returning to the departure platform.");
				SubwayTravelService.CompleteTrip();
			}
			this.scroller.Hide(0.05f);
			this.SnapTrainKeepingPlayerSeat(this._stationPos - this._worldDir * rollDist, player, playerRb);
			yield return new WaitForSeconds(0.6f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			yield return SubwayScreenFade.Fade(0f, 0.45f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			yield return this.Roll(this.realMoveTopSpeed, 0f, this.realMoveSeconds, playerRb);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			this.SnapTrainKeepingPlayerSeat(this._stationPos, player, playerRb);
			this.Drive(0f, true);
			this.StopRumble();
			SubwayCameraShaker.SetIntensity(0f);
			SubwayTravelService.CompleteTrip();
			SubwayRideGuard.End();
			this._activePassenger = null;
			Debug.Log(string.Format("[SubwayRide] ride DONE t={0:F1} pos={1}", Time.time, base.transform.position));
			yield return new WaitForSeconds(0.4f);
			yield break;
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x0017BB34 File Offset: 0x00179D34
		private void AcceptConnectingPassenger(Transform player, Vector3 arrivalPosition, SubwayRideDirector departure)
		{
			this._serviceGeneration++;
			base.StopAllCoroutines();
			this.ReleaseCarriedPlayer();
			this._rumbleFade = null;
			this._activePassenger = player;
			this._recoveryInterrupted = false;
			this._connectingArrival = true;
			SubwayRideDirector._closedStations.Remove(this.StationKey);
			PoiResidencyStateKey poiResidencyStateKey;
			if (PoiResidencyPlacement.TryGetKey(this.StationKey, "subway-closed", out poiResidencyStateKey))
			{
				PoiResidencyLedger.WriteFlag(poiResidencyStateKey, false);
			}
			foreach (SubwayRideDirector subwayRideDirector in Object.FindObjectsOfType<SubwayRideDirector>(true))
			{
				if (subwayRideDirector != this && subwayRideDirector.StationKey == this.StationKey)
				{
					subwayRideDirector.RestartService();
				}
			}
			this.SnapTo(this._stationPos);
			this.SetTrainPresent(true);
			if (this.doorSystem != null)
			{
				this.doorSystem.ResetToClosed();
			}
			SubwayRideDirector.PlaceConnectingPassenger(player, arrivalPosition);
			Rigidbody rigidbody = ((player != null) ? player.GetComponent<Rigidbody>() : null);
			SubwayRideGuard.Begin(this);
			SubwayTravelService.RecordPhysicalArrival(base.transform);
			if (departure != null)
			{
				departure.ReopenStationAfterSuccessfulTransfer();
			}
			SubwayTravelService.CompleteTrip();
			base.StartCoroutine(this.ConnectingArrival(player, rigidbody));
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0017BC5C File Offset: 0x00179E5C
		internal static void PlaceConnectingPassenger(Transform player, Vector3 arrivalPosition)
		{
			Rigidbody rigidbody = ((player != null) ? player.GetComponent<Rigidbody>() : null);
			if (player != null)
			{
				if (rigidbody != null)
				{
					rigidbody.position = arrivalPosition;
					if (!rigidbody.isKinematic)
					{
						rigidbody.velocity = Vector3.zero;
						rigidbody.angularVelocity = Vector3.zero;
					}
				}
				player.position = arrivalPosition;
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0017BCBF File Offset: 0x00179EBF
		private IEnumerator ConnectingArrival(Transform player, Rigidbody rb)
		{
			int generation = this._serviceGeneration;
			float num = this.realMoveTopSpeed * this.realMoveSeconds * 0.5f;
			this.SnapTrainKeepingPlayerSeat(this._stationPos - this._worldDir * num, player, rb);
			if (this.scroller != null)
			{
				this.scroller.Hide(0f);
			}
			this.StartRumble();
			yield return new WaitForSeconds(0.6f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			yield return SubwayScreenFade.Fade(0f, 0.45f);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			yield return this.Roll(this.realMoveTopSpeed, 0f, this.realMoveSeconds, rb);
			if (generation != this._serviceGeneration)
			{
				yield break;
			}
			this.SnapTrainKeepingPlayerSeat(this._stationPos, player, rb);
			this.Drive(0f, true);
			this.StopRumble();
			SubwayCameraShaker.SetIntensity(0f);
			SubwayRideGuard.End();
			this._activePassenger = null;
			yield return this.DepartTerminalWhenClear();
			if (generation == this._serviceGeneration)
			{
				this.RestartService();
			}
			yield break;
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0017BCDC File Offset: 0x00179EDC
		private IEnumerator Roll(float fromSpeed, float toSpeed, float seconds, Rigidbody playerRb)
		{
			int generation = this._serviceGeneration;
			if (this._trainRb != null)
			{
				this._trainRb.interpolation = RigidbodyInterpolation.None;
			}
			if (playerRb != null)
			{
				this._carriedPlayer = playerRb;
				this._savedPassengerInterpolation = playerRb.interpolation;
				playerRb.interpolation = RigidbodyInterpolation.None;
			}
			this._carry.PlayerRb = playerRb;
			this._carry.Velocity = Vector3.zero;
			bool aboard = playerRb != null;
			float mt = 0f;
			while (mt < seconds)
			{
				yield return new WaitForFixedUpdate();
				if (generation != this._serviceGeneration)
				{
					yield break;
				}
				mt += Time.fixedDeltaTime;
				float num = Mathf.Lerp(fromSpeed, toSpeed, Mathf.SmoothStep(0f, 1f, mt / seconds));
				this._carry.Velocity = this._worldDir * num;
				this.Drive(num, aboard);
			}
			this.ReleaseCarriedPlayer();
			yield break;
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0017BD08 File Offset: 0x00179F08
		private void ReleaseCarriedPlayer()
		{
			if (this._carry != null)
			{
				this._carry.Velocity = Vector3.zero;
				this._carry.PlayerRb = null;
			}
			if (this._carriedPlayer != null)
			{
				this._carriedPlayer.interpolation = this._savedPassengerInterpolation;
			}
			this._carriedPlayer = null;
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0017BD68 File Offset: 0x00179F68
		private void SnapTo(Vector3 worldPos)
		{
			Vector3 vector = worldPos - base.transform.position;
			if (this._trainRb != null)
			{
				this._trainRb.position += vector;
			}
			base.transform.position += vector;
			Physics.SyncTransforms();
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x0017BDC8 File Offset: 0x00179FC8
		private void SnapTrainKeepingPlayerSeat(Vector3 targetPos, Transform player, Rigidbody playerRb)
		{
			Vector3 vector = ((player != null) ? base.transform.InverseTransformPoint(player.position) : Vector3.zero);
			Vector3 vector2 = targetPos - base.transform.position;
			if (this._trainRb != null)
			{
				this._trainRb.position += vector2;
			}
			base.transform.position += vector2;
			if (player != null)
			{
				Vector3 vector3 = base.transform.TransformPoint(vector);
				if (playerRb != null)
				{
					RigidbodyInterpolation interpolation = playerRb.interpolation;
					playerRb.interpolation = RigidbodyInterpolation.None;
					playerRb.position = vector3;
					playerRb.velocity = Vector3.zero;
					playerRb.interpolation = interpolation;
				}
				player.position = vector3;
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x0017BE98 File Offset: 0x0017A098
		private void SnapWithPlayer(Vector3 delta, Transform player, Rigidbody playerRb)
		{
			if (this._trainRb != null)
			{
				this._trainRb.position += delta;
			}
			base.transform.position += delta;
			if (player != null)
			{
				if (playerRb != null)
				{
					RigidbodyInterpolation interpolation = playerRb.interpolation;
					playerRb.interpolation = RigidbodyInterpolation.None;
					playerRb.position += delta;
					playerRb.velocity = Vector3.zero;
					playerRb.interpolation = interpolation;
				}
				player.position += delta;
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x0017BF3C File Offset: 0x0017A13C
		private bool PlayerInTrainVolume(Vector3 trainPos)
		{
			Transform player = PlayerRegistry.Player;
			if (player == null || this.interiorZone == null)
			{
				return false;
			}
			Vector3 vector = trainPos - base.transform.position;
			Bounds bounds = this.InteriorWorldBounds();
			bounds.center += vector;
			bounds.Expand(new Vector3(1.5f, 1.5f, 1.5f));
			return bounds.Contains(player.position);
		}

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06003FF9 RID: 16377 RVA: 0x0017BFBC File Offset: 0x0017A1BC
		// (remove) Token: 0x06003FFA RID: 16378 RVA: 0x0017BFF4 File Offset: 0x0017A1F4
		public event Action<bool> TrainPresenceChanged;

		// Token: 0x06003FFB RID: 16379 RVA: 0x0017C029 File Offset: 0x0017A229
		private void SetTrainPresent(bool present)
		{
			this.ApplyPresence(present);
			Action<bool> trainPresenceChanged = this.TrainPresenceChanged;
			if (trainPresenceChanged == null)
			{
				return;
			}
			trainPresenceChanged(present);
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x0017C044 File Offset: 0x0017A244
		private void ApplyPresence(bool present)
		{
			this._trainPresent = present;
			if (this._bodyRenderers != null)
			{
				foreach (Renderer renderer in this._bodyRenderers)
				{
					if (renderer != null)
					{
						renderer.enabled = present;
					}
				}
			}
			if (this._allColliders != null)
			{
				foreach (Collider collider in this._allColliders)
				{
					if (collider != null)
					{
						collider.enabled = present;
					}
				}
			}
			if (this._lights != null)
			{
				foreach (Light light in this._lights)
				{
					if (light != null)
					{
						light.enabled = present;
					}
				}
			}
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x0017C0F4 File Offset: 0x0017A2F4
		private bool PlayerInServiceCell()
		{
			Transform player = PlayerRegistry.Player;
			if (player == null)
			{
				return false;
			}
			ChunkManager instance = ChunkManager.Instance;
			if (!(instance != null))
			{
				return SubwayRideDirector.IsCellWithinServiceArea(SubwayRideDirector.FallbackCell(player.position), SubwayRideDirector.FallbackCell(this._stationPos), Vector2Int.one, this.serviceCellPadding);
			}
			Vector2Int vector2Int = instance.WorldToCell(player.position);
			ChunkInstance chunkContaining = instance.GetChunkContaining(base.transform);
			if (chunkContaining != null)
			{
				Vector2Int vector2Int2 = ((chunkContaining.chunkData != null) ? chunkContaining.chunkData.gridSize : Vector2Int.one);
				return SubwayRideDirector.IsCellWithinServiceArea(vector2Int, chunkContaining.originCell, vector2Int2, this.serviceCellPadding);
			}
			return SubwayRideDirector.IsCellWithinServiceArea(vector2Int, instance.WorldToCell(this._stationPos), Vector2Int.one, this.serviceCellPadding);
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x0017C1BC File Offset: 0x0017A3BC
		public static bool IsCellWithinServiceArea(Vector2Int playerCell, Vector2Int originCell, Vector2Int chunkSize, int padding)
		{
			padding = Mathf.Max(0, padding);
			int num = Mathf.Max(1, chunkSize.x);
			int num2 = Mathf.Max(1, chunkSize.y);
			return playerCell.x >= originCell.x - padding && playerCell.x <= originCell.x + num - 1 + padding && playerCell.y >= originCell.y - padding && playerCell.y <= originCell.y + num2 - 1 + padding;
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x0017C243 File Offset: 0x0017A443
		private static Vector2Int FallbackCell(Vector3 world)
		{
			return new Vector2Int(Mathf.FloorToInt(world.x / 60f), Mathf.FloorToInt(world.z / 60f));
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x0017C26C File Offset: 0x0017A46C
		private bool PlayerAboard()
		{
			Transform player = PlayerRegistry.Player;
			if (player == null || this.interiorZone == null)
			{
				return false;
			}
			Bounds bounds = this.InteriorWorldBounds();
			bounds.Expand(new Vector3(0.8f, 0.8f, 0.8f));
			return bounds.Contains(player.position);
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x0017C2C8 File Offset: 0x0017A4C8
		private Bounds InteriorWorldBounds()
		{
			BoxCollider boxCollider = this.interiorZone as BoxCollider;
			if (boxCollider == null)
			{
				return this.interiorZone.bounds;
			}
			Bounds bounds = new Bounds(boxCollider.transform.TransformPoint(boxCollider.center), Vector3.zero);
			Vector3 vector = boxCollider.size * 0.5f;
			for (int i = -1; i <= 1; i += 2)
			{
				for (int j = -1; j <= 1; j += 2)
				{
					for (int k = -1; k <= 1; k += 2)
					{
						bounds.Encapsulate(boxCollider.transform.TransformPoint(boxCollider.center + Vector3.Scale(vector, new Vector3((float)i, (float)j, (float)k))));
					}
				}
			}
			return bounds;
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x0017C37B File Offset: 0x0017A57B
		private IEnumerator WaitSkippable(float seconds, bool requirePlayerNear = false)
		{
			this._skipPhase = false;
			float t = 0f;
			while (t < seconds && !this._skipPhase && !this.ServiceClosedHere)
			{
				t += Time.deltaTime;
				yield return null;
			}
			this._skipPhase = false;
			if (!requirePlayerNear)
			{
				yield break;
			}
			while (!this.PlayerInServiceCell() && !this.PlayerAboard() && !this.ServiceClosedHere && !this._skipPhase)
			{
				yield return new WaitForSeconds(0.5f);
			}
			this._skipPhase = false;
			yield break;
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x0017C398 File Offset: 0x0017A598
		private IEnumerator SpeedRamp(float from, float to, float seconds, bool aboard)
		{
			float t = 0f;
			while (t < seconds)
			{
				t += Time.deltaTime;
				this.Drive(Mathf.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t / seconds)), aboard);
				yield return null;
			}
			this.Drive(to, aboard);
			yield break;
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x0017C3C4 File Offset: 0x0017A5C4
		private void Drive(float speed, bool aboard)
		{
			float num = ((this.cruiseSpeed > 0.01f) ? Mathf.Clamp01(speed / this.cruiseSpeed) : 0f);
			this.scroller.SetSpeed(speed * this._scrollSign);
			if (aboard)
			{
				SubwayCameraShaker.SetIntensity(SubwayRideDirector.IsPhoneInUse ? 0f : (this.shakeAmplitude * num));
			}
			float num2 = this.rumbleVolume * Mathf.Max(num, (speed > 0.1f) ? 0.25f : 0f);
			this._drivenVolume = Mathf.MoveTowards(this._drivenVolume, num2, 1.5f * Time.deltaTime);
			this._rumbleSource.volume = this._drivenVolume * this._rumbleMute * this.OcclusionFactor;
			this._rumbleSource.pitch = Mathf.MoveTowards(this._rumbleSource.pitch, 0.8f + 0.35f * num, 0.6f * Time.deltaTime);
			if (aboard && speed > 3f)
			{
				this._clackDistance -= speed * Time.deltaTime;
				if (this._clackDistance <= 0f)
				{
					this._clackDistance += this.railJointSpacing;
					this.PlayAtDoorway((this.clackOverride != null) ? this.clackOverride : SubwayAudioLib.RailClack, this.clackVolume * (0.8f + 0.4f * Random.value));
				}
			}
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0017C52D File Offset: 0x0017A72D
		private void PlayChime()
		{
			this.PlayAtDoorway((this.chimeOverride != null) ? this.chimeOverride : SubwayAudioLib.Chime, this.chimeVolume);
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0017C556 File Offset: 0x0017A756
		private void PlayAtDoorway(AudioClip clip, float volume)
		{
			if (this.doorSystem != null && this.doorSystem.HasOpeningPanels)
			{
				this.doorSystem.PlayAtNearestDoorway(clip, volume);
				return;
			}
			this.PlayOneShot(clip, volume);
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x0017C58C File Offset: 0x0017A78C
		private void StartRumble()
		{
			if (this._rumbleFade != null)
			{
				base.StopCoroutine(this._rumbleFade);
			}
			this._rumbleMute = 0f;
			if (!this._rumbleSource.isPlaying)
			{
				this._rumbleSource.volume = 0f;
				this._rumbleSource.Play();
			}
			this._rumbleFade = base.StartCoroutine(this.FadeRumbleMute(1f, this.rumbleFadeSeconds, false));
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x0017C5FE File Offset: 0x0017A7FE
		private void StopRumble()
		{
			if (this._rumbleFade != null)
			{
				base.StopCoroutine(this._rumbleFade);
			}
			this._rumbleFade = base.StartCoroutine(this.FadeRumbleMute(0f, this.rumbleFadeSeconds, true));
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0017C632 File Offset: 0x0017A832
		private IEnumerator FadeRumbleMute(float target, float seconds, bool stopAtEnd = false)
		{
			float from = this._rumbleMute;
			float t = 0f;
			while (t < seconds)
			{
				t += Time.deltaTime;
				this._rumbleMute = Mathf.Lerp(from, target, Mathf.SmoothStep(0f, 1f, t / seconds));
				yield return null;
			}
			this._rumbleMute = target;
			if (stopAtEnd)
			{
				this._rumbleSource.Stop();
				this._rumbleSource.volume = 0f;
			}
			this._rumbleFade = null;
			yield break;
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x0600400A RID: 16394 RVA: 0x0017C656 File Offset: 0x0017A856
		private float OcclusionFactor
		{
			get
			{
				if (!(this._occlusion != null))
				{
					return 1f;
				}
				return this._occlusion.Factor;
			}
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x0017C677 File Offset: 0x0017A877
		private void PlayOneShot(AudioClip clip, float volume)
		{
			if (clip != null)
			{
				this._oneShotSource.PlayOneShot(clip, volume * this.OcclusionFactor);
			}
		}

		// Token: 0x04003F93 RID: 16275
		[Header("Wiring (authored by Tools/Subway)")]
		[SerializeField]
		private SubwayDoorSystem doorSystem;

		// Token: 0x04003F94 RID: 16276
		[SerializeField]
		private SubwayTunnelScroller scroller;

		// Token: 0x04003F95 RID: 16277
		[Tooltip("Still trigger box covering the carriage interior; decides if the player departs with the train.")]
		[SerializeField]
		private Collider interiorZone;

		// Token: 0x04003F96 RID: 16278
		[Tooltip("Prompt world-space mostrato dentro la carrozza durante la corsa. Ne basta UNO autorato: all'avvio viene clonato sopra OGNI porta che si apre, così la richiesta di fermata sta dove il player la cerca invece che in mezzo al nulla. Quota e profondità restano quelle autorate qui, cambia solo la posizione lungo la carrozza.")]
		[SerializeField]
		private InteractablePromptUI ridePromptUI;

		// Token: 0x04003F97 RID: 16279
		[Tooltip("Prompt autorati a mano, uno per porta. Se questa lista ha almeno un elemento sono QUESTI i prompt e il codice non ne crea ne' ne sposta nessuno: le posizioni le decidi in scena. Vuota = si derivano dalle porte allo Start clonando il prompt qui sopra, che e' l'unica strada per i treni che spawnera' la mappa procedurale, dove non c'e' niente da autorare.")]
		[SerializeField]
		private InteractablePromptUI[] authoredDoorPrompts;

		// Token: 0x04003F98 RID: 16280
		[Tooltip("Distanza ORIZZONTALE (m) entro cui il prompt di una porta si accende. Orizzontale perche' il prompt sta a ~2.2 m sopra il pavimento e l'origine del player e' ai PIEDI: in 3D il solo dislivello mangerebbe quasi tutta la soglia.")]
		[SerializeField]
		[Min(0.5f)]
		private float promptVisibleDistance = 1.5f;

		// Token: 0x04003F99 RID: 16281
		[Tooltip("Distanza (m) entro cui due pannelli sono considerati la stessa porta: le portine sono a due ante che si aprono in versi opposti, e senza raggruppamento verrebbe un prompt per anta. Serve solo alla derivazione automatica.")]
		[SerializeField]
		[Min(0.1f)]
		private float doorGroupingDistance = 1.5f;

		// Token: 0x04003F9A RID: 16282
		private readonly List<InteractablePromptUI> _ridePrompts = new List<InteractablePromptUI>();

		// Token: 0x04003F9B RID: 16283
		private bool[] _promptNear = new bool[0];

		// Token: 0x04003F9C RID: 16284
		[SerializeField]
		private InteractionConfigSO arriveConfig;

		// Token: 0x04003F9D RID: 16285
		[Header("Input")]
		[Tooltip("Key the player presses during the cruise to arrive at the next station.")]
		[SerializeField]
		private GenericInput rideInput = new GenericInput("F", "Y", "F");

		// Token: 0x04003F9E RID: 16286
		[Header("Service timing")]
		[Tooltip("Attesa MINIMA sulla banchina fra una visita e l'altra. Il tempo effettivo si tira a sorte fra min e max a ogni giro: un orario prevedibile fa sembrare la metro un meccanismo, non un servizio.")]
		[SerializeField]
		private float awaySecondsMin = 18f;

		// Token: 0x04003F9F RID: 16287
		[Tooltip("Attesa MASSIMA sulla banchina. Tenerla ben sopra il minimo o la casualita' non si nota; troppo alta e la banchina diventa noiosa.")]
		[SerializeField]
		private float awaySecondsMax = 55f;

		// Token: 0x04003FA0 RID: 16288
		[Tooltip("Extra seconds before this train's very first arrival (stagger multiple trains).")]
		[SerializeField]
		private float initialDelaySeconds;

		// Token: 0x04003FA1 RID: 16289
		[Tooltip("Seconds the doors stay open at the platform.")]
		[SerializeField]
		private float dwellOpenSeconds = 12f;

		// Token: 0x04003FA2 RID: 16290
		[Tooltip("Pause between the warning chime and the doors closing.")]
		[SerializeField]
		private float closeWarningDelay = 1.3f;

		// Token: 0x04003FA3 RID: 16291
		[Tooltip("Margine in celle attorno al chunk Metro che attiva il servizio. La stazione 1x1 oltrepassa fisicamente il bordo della propria cella: con zero il player puo' essere sulla banchina ma risultare nella cella confinante.")]
		[SerializeField]
		[Range(0f, 2f)]
		private int serviceCellPadding = 1;

		// Token: 0x04003FA4 RID: 16292
		[Header("Viaggio sulla mappa")]
		[Tooltip("Distanza fisica tra i punti sicuri di imbarco e arrivo in due stazioni diverse. La posizione della casa e dei quartieri visitati resta invariata.")]
		[SerializeField]
		private float travelMeters = 500f;

		// Token: 0x04003FA5 RID: 16293
		[Header("Ride timing (player aboard)")]
		[SerializeField]
		private float accelSeconds = 5f;

		// Token: 0x04003FA6 RID: 16294
		[Tooltip("Seconds at cruise speed in the fake tunnel.")]
		[SerializeField]
		private float cruiseSeconds = 16f;

		// Token: 0x04003FA7 RID: 16295
		[SerializeField]
		private float brakeSeconds = 4f;

		// Token: 0x04003FA8 RID: 16296
		[Header("Real rolls")]
		[Tooltip("Seconds of the REAL roll at ride launch / re-entry (player aboard).")]
		[SerializeField]
		private float realMoveSeconds = 4f;

		// Token: 0x04003FA9 RID: 16297
		[Tooltip("Speed reached by the launch roll / entered by the re-entry roll (m/s).")]
		[SerializeField]
		private float realMoveTopSpeed = 3.5f;

		// Token: 0x04003FAA RID: 16298
		[Tooltip("Seconds of the visible arrival/departure roll when the train comes and goes empty.")]
		[SerializeField]
		private float emptyRollSeconds = 5f;

		// Token: 0x04003FAB RID: 16299
		[Tooltip("Top speed of the empty arrival/departure roll (m/s). Also sets how deep in the tunnel it appears/vanishes.")]
		[SerializeField]
		private float emptyRollTopSpeed = 8f;

		// Token: 0x04003FAC RID: 16300
		[Tooltip("Extra seconds of full-speed rolling INSIDE the tunnel before despawning (and after spawning), so the pop happens deep in the dark, not in sight.")]
		[SerializeField]
		private float vanishExtraSeconds = 2f;

		// Token: 0x04003FAD RID: 16301
		[Tooltip("Travel direction in the train root's LOCAL space.")]
		[SerializeField]
		private Vector3 localTravelDir = new Vector3(1f, 0f, 0f);

		// Token: 0x04003FAE RID: 16302
		[Header("Feel")]
		[Tooltip("Fake cruise speed in m/s — sets how fast the tunnel walls scroll.")]
		[SerializeField]
		private float cruiseSpeed = 16f;

		// Token: 0x04003FAF RID: 16303
		[Tooltip("Camera shake amplitude (meters) at cruise speed (only while the player is aboard).")]
		[SerializeField]
		private float shakeAmplitude = 0.022f;

		// Token: 0x04003FB0 RID: 16304
		[Tooltip("Meters between rail joints (sets the clack-clack cadence).")]
		[SerializeField]
		private float railJointSpacing = 26f;

		// Token: 0x04003FB1 RID: 16305
		[Header("Audio (procedural if left empty)")]
		[SerializeField]
		private AudioClip rumbleOverride;

		// Token: 0x04003FB2 RID: 16306
		[SerializeField]
		private AudioClip chimeOverride;

		// Token: 0x04003FB3 RID: 16307
		[SerializeField]
		private AudioClip clackOverride;

		// Token: 0x04003FB4 RID: 16308
		[SerializeField]
		private AudioClip brakeOverride;

		// Token: 0x04003FB5 RID: 16309
		[Range(0f, 1f)]
		[SerializeField]
		private float rumbleVolume = 0.55f;

		// Token: 0x04003FB6 RID: 16310
		[Range(0f, 1f)]
		[SerializeField]
		private float chimeVolume = 0.5f;

		// Token: 0x04003FB7 RID: 16311
		[Range(0f, 1f)]
		[SerializeField]
		private float clackVolume = 0.16f;

		// Token: 0x04003FB8 RID: 16312
		[Range(0f, 1f)]
		[SerializeField]
		private float brakeVolume = 0.6f;

		// Token: 0x04003FB9 RID: 16313
		[Tooltip("Seconds the rumble takes to fade in when the train starts moving and out when it stops.")]
		[SerializeField]
		private float rumbleFadeSeconds = 1.6f;

		// Token: 0x04003FBA RID: 16314
		private AudioSource _rumbleSource;

		// Token: 0x04003FBB RID: 16315
		private AudioSource _oneShotSource;

		// Token: 0x04003FBC RID: 16316
		private SubwayAudioOcclusion _occlusion;

		// Token: 0x04003FBD RID: 16317
		private Transform _rumbleAnchor;

		// Token: 0x04003FBE RID: 16318
		private float _bodyLocalMinX;

		// Token: 0x04003FBF RID: 16319
		private float _bodyLocalMaxX;

		// Token: 0x04003FC0 RID: 16320
		private Vector3 _bodyLocalCenter;

		// Token: 0x04003FC1 RID: 16321
		private Rigidbody _trainRb;

		// Token: 0x04003FC2 RID: 16322
		private SubwayPlatformCarry _carry;

		// Token: 0x04003FC3 RID: 16323
		private Renderer[] _bodyRenderers;

		// Token: 0x04003FC4 RID: 16324
		private Collider[] _allColliders;

		// Token: 0x04003FC5 RID: 16325
		private Light[] _lights;

		// Token: 0x04003FC6 RID: 16326
		private float _nextPresenceIntegrityCheck;

		// Token: 0x04003FC7 RID: 16327
		private const float PresenceIntegrityInterval = 0.25f;

		// Token: 0x04003FC8 RID: 16328
		private Vector3 _stationLocalPos;

		// Token: 0x04003FC9 RID: 16329
		private Quaternion _stationLocalRotation;

		// Token: 0x04003FCA RID: 16330
		private bool _stationPoseCaptured;

		// Token: 0x04003FCB RID: 16331
		private Vector3 _stationPos;

		// Token: 0x04003FCC RID: 16332
		private Vector3 _worldDir;

		// Token: 0x04003FCD RID: 16333
		private float _scrollSign;

		// Token: 0x04003FCE RID: 16334
		private float _clackDistance;

		// Token: 0x04003FCF RID: 16335
		private bool _skipPhase;

		// Token: 0x04003FD0 RID: 16336
		private float _rumbleMute;

		// Token: 0x04003FD1 RID: 16337
		private Coroutine _rumbleFade;

		// Token: 0x04003FD2 RID: 16338
		private float _drivenVolume;

		// Token: 0x04003FD3 RID: 16339
		private bool _arriveRequested;

		// Token: 0x04003FD4 RID: 16340
		private bool _promptShown;

		// Token: 0x04003FD5 RID: 16341
		private bool _trainPresent;

		// Token: 0x04003FD6 RID: 16342
		private bool _started;

		// Token: 0x04003FD7 RID: 16343
		private bool _operationalInitialized;

		// Token: 0x04003FD8 RID: 16344
		private IncrementalPooledZoneActivation _pendingStagedActivation;

		// Token: 0x04003FD9 RID: 16345
		private HeavyZoneStreamingProxy _pendingHeavyActivation;

		// Token: 0x04003FDA RID: 16346
		private int _serviceGeneration;

		// Token: 0x04003FDB RID: 16347
		private Transform _activePassenger;

		// Token: 0x04003FDC RID: 16348
		private Rigidbody _carriedPlayer;

		// Token: 0x04003FDD RID: 16349
		private RigidbodyInterpolation _savedPassengerInterpolation;

		// Token: 0x04003FDE RID: 16350
		private bool _recoveryInterrupted;

		// Token: 0x04003FDF RID: 16351
		private static readonly HashSet<Transform> _closedStations = new HashSet<Transform>();

		// Token: 0x04003FE0 RID: 16352
		private bool _passengerTransferred;

		// Token: 0x04003FE1 RID: 16353
		private bool _connectingArrival;
	}
}
