using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Features.RenderingPrecision;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Diagnostics
{
	// Token: 0x02000A01 RID: 2561
	public sealed class MapPoiStreamingRoute
	{
		// Token: 0x06004904 RID: 18692 RVA: 0x001BDD7D File Offset: 0x001BBF7D
		public static bool UsesDirectBaseline(string family, bool addressable)
		{
			return !addressable && (family == "kombini" || family == "subway");
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06004905 RID: 18693 RVA: 0x001BDD9E File Offset: 0x001BBF9E
		public MapPoiStreamingRoute.Report Status { get; } = new MapPoiStreamingRoute.Report();

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06004906 RID: 18694 RVA: 0x001BDDA6 File Offset: 0x001BBFA6
		public bool Completed
		{
			get
			{
				return this.Status.completed;
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06004907 RID: 18695 RVA: 0x001BDDB3 File Offset: 0x001BBFB3
		public bool ValidForComparison
		{
			get
			{
				return this.Status.validForComparison;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06004908 RID: 18696 RVA: 0x001BDDC0 File Offset: 0x001BBFC0
		public string Phase
		{
			get
			{
				return this.Status.phase;
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06004909 RID: 18697 RVA: 0x001BDDCD File Offset: 0x001BBFCD
		public string PhaseLabel
		{
			get
			{
				return this.Phase;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x0600490A RID: 18698 RVA: 0x001BDDD5 File Offset: 0x001BBFD5
		public string Reason
		{
			get
			{
				return this.Status.reason;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x0600490B RID: 18699 RVA: 0x001BDDE2 File Offset: 0x001BBFE2
		public double ScheduledDistance
		{
			get
			{
				return this.Status.actualDistanceMetres;
			}
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x001BDDF0 File Offset: 0x001BBFF0
		public MapPoiStreamingRoute(Transform player, Rigidbody body, ChunkManager manager, MapPoiStreamingRoute.Target target, MapPoiStreamingRoute.Config config, Action<string> keypoint = null)
		{
			if (player == null || manager == null || target == null || config == null)
			{
				throw new ArgumentNullException();
			}
			config.Validate();
			MapPoiStreamingRoute.ValidateTarget(target);
			PoiResidencyCatalog.Entry entry;
			if (target.family != config.family || !PoiResidencyCatalog.TryGetByAddress(target.payloadAddress, out entry) || entry.family != config.family)
			{
				throw new InvalidOperationException("POI target identity does not match the selected catalog family.");
			}
			int num;
			if (!manager.TryGetSessionSeed(out num) || num != config.expectedSeed || MapResidencyPilotSelection.IsFamilyEnabled(config.family) != config.requireAddressablePayload)
			{
				throw new InvalidOperationException("POI route seed or pre-map pilot selection mismatch.");
			}
			this._player = player;
			this._body = body;
			this._manager = manager;
			this._config = config;
			this._keypoint = keypoint;
			this._directBaseline = MapPoiStreamingRoute.UsesDirectBaseline(config.family, config.requireAddressablePayload);
			if (target.zoneSlotIndex < 0)
			{
				throw new InvalidOperationException("Actual POI zone slot is required.");
			}
			bool flag = !config.requireAddressablePayload && (config.family == "park" || this._directBaseline);
			this._progress = new MapPoiStreamingRoute.Progress(config.roundTrips, config.requireAddressablePayload, flag);
			this._sceneHandle = SceneManager.GetActiveScene().handle;
			this.Status.startLogical = WorldOrigin.ToLogical(player.position);
			this.Status.target = target;
			this.Status.expectedSeed = num;
			this.Status.requestedRoundTrips = config.roundTrips;
			this.Status.family = config.family;
			this.Status.payloadAddress = target.payloadAddress;
			this.Status.materializationKind = (this._directBaseline ? "direct-resources-zone" : "heavy-proxy");
			this.Status.targetResourcePath = entry.resourceKey;
			this.Status.retirementPolicy = (config.requireAddressablePayload ? "destroy-clone-and-native-prefab-per-address" : (this._directBaseline ? "resources-direct-zone-pool-or-destruction" : (flag ? "resources-park-dormant-pool-or-destruction" : "resources-clone-destruction-cache-retained")));
			this.Status.requireAddressablePayload = config.requireAddressablePayload;
			this.Status.mapResidencyPilot = MapResidencyPilotSelection.Mode;
			this.Status.speedMetresPerSecond = config.speedMetresPerSecond;
			this.Status.maximumSeconds = config.maximumSeconds;
			this.Status.minimumPossibleSeconds = MapPoiStreamingRoute.MinimumSeconds(this.Status.startLogical, target, config);
			if (this.Status.minimumPossibleSeconds + 10.0 > config.maximumSeconds)
			{
				throw new InvalidOperationException("Full POI route cannot fit capture budget.");
			}
			this.Status.graphicsStart = MapPoiStreamingRoute.CaptureGraphics(manager);
			this.Status.graphicsStartFingerprint = this.Status.graphicsStart.Fingerprint();
			this._events.Add("0\t0\tstarted\tActual POI and retirement proof required; no NPC override");
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x001BE124 File Offset: 0x001BC324
		public void Tick(RunSoakRecorder recorder)
		{
			if (recorder == null || !recorder.IsRecording || this._finalized)
			{
				return;
			}
			this.Status.elapsedSeconds = recorder.Elapsed;
			if (recorder.Elapsed >= this._config.maximumSeconds)
			{
				this.Stop(recorder, "incomplete", "Capture budget ended before all requested POI trips.");
				return;
			}
			int num;
			if (this._player == null || this._manager == null || ChunkManager.Instance != this._manager || PlayerRegistry.Player != this._player || SceneManager.GetActiveScene().handle != this._sceneHandle || !this._manager.TryGetSessionSeed(out num) || num != this._config.expectedSeed || MapResidencyPilotSelection.IsFamilyEnabled(this._config.family) != this._config.requireAddressablePayload || MapResidencyPilotSelection.Mode != this.Status.mapResidencyPilot)
			{
				this.Stop(recorder, "invalid", "Player, map, scene, seed or pilot selection changed.");
				return;
			}
			if (recorder.Elapsed >= this._nextSample)
			{
				if (!this.Status.graphicsStart.Equals(MapPoiStreamingRoute.CaptureGraphics(this._manager)))
				{
					this.Status.graphicsChanged = true;
					this.Stop(recorder, "invalid", "Graphics changed.");
					return;
				}
				this.RefreshTarget();
				this.Sample(recorder);
				this._nextSample = recorder.Elapsed + 1.0;
			}
			if (this.Phase == "alignment" || this.Phase == "approach" || this.Phase == "outbound" || this.Phase == "return")
			{
				Vector3 vector = WorldOrigin.ToLogical(this._player.position);
				Vector3 vector2 = ((this.Phase == "alignment") ? this.Status.target.alignmentLogical : ((this.Phase == "outbound") ? this.Status.target.farLogical : this.Status.target.approachLogical));
				vector2.y = vector.y;
				Vector3 vector3 = MapPoiStreamingRoute.StepPosition(vector, vector2, this._config.speedMetresPerSecond, (double)Time.unscaledDeltaTime);
				Vector3 vector4 = vector3 - vector;
				this.Status.actualDistanceMetres += (double)MapPoiStreamingRoute.PlanarDistance(vector, vector3);
				Vector3 vector5 = WorldOrigin.ToUnity(vector3);
				this._player.position = vector5;
				if (this._body != null)
				{
					this._body.position = vector5;
				}
				if (vector4.sqrMagnitude > 1E-06f)
				{
					this._player.rotation = Quaternion.LookRotation(vector4, Vector3.up);
					if (this._body != null)
					{
						this._body.rotation = this._player.rotation;
					}
				}
				WorldOrigin.TryCenterAround(vector5, false);
				if (MapPoiStreamingRoute.PlanarDistance(vector3, vector2) <= 0.001f)
				{
					if (this.Phase == "alignment")
					{
						this.Change(recorder, "approach");
						return;
					}
					recorder.Phase = ((this.Phase == "outbound") ? 3 : 1);
					this.Change(recorder, (this.Phase == "outbound") ? "far_hold" : "near_hold");
				}
				return;
			}
			if (this.Phase == "near_hold")
			{
				if (this._targetProxy != null && this._targetProxy.LoadFailed)
				{
					this.Stop(recorder, "invalid", "Target " + this._config.family + " payload failed.");
					return;
				}
				if (this._directBaseline)
				{
					this.RefreshTarget();
				}
				if (this._directBaseline ? (this._targetDirectRoot == null || !this._targetDirectOperational) : (this._targetProxy == null || !this._targetProxy.IsOperationalReady))
				{
					if (recorder.Elapsed - this._stageStart > 60.0)
					{
						this.Stop(recorder, "incomplete", "Preview candidate did not become an actual operational " + this._config.family + " within60s.");
					}
					return;
				}
				this._observedProxy = this._targetProxy;
				this._observedClone = (this._directBaseline ? this._targetDirectRoot : this._targetProxy.PayloadInstance);
				this._observedAsset = (this._directBaseline ? null : this._targetProxy.NativeAddressablePrefab);
				if (!this._directBaseline && (this._targetProxy.UsesAddressablePayload != this._config.requireAddressablePayload || (this._config.requireAddressablePayload && !this._targetProxy.HasCanonicalGameplayReferences)))
				{
					this.Stop(recorder, "invalid", "Actual POI payload does not match the selected residency/canonical-binding mode.");
					return;
				}
				this._observedCloneId = ((this._observedClone != null) ? this._observedClone.GetInstanceID() : 0);
				HeavyZoneAddressableResidency.AddressSnapshot addressSnapshot = HeavyZoneAddressableResidency.ObserveAddress(this.Status.payloadAddress);
				try
				{
					this._progress.ObserveNear(this._observedCloneId, (this._observedAsset != null) ? this._observedAsset.GetInstanceID() : 0, addressSnapshot.releaseCount);
				}
				catch (Exception ex)
				{
					this.Stop(recorder, "invalid", ex.Message);
					return;
				}
				this.Status.completedRoundTrips = this._progress.CompletedRoundTrips;
				this.Status.reusedBaselineReturns = this._progress.ReusedBaselineReturns;
				if (this._directBaseline)
				{
					this.Status.directBaselineVisits.Add(new MapPoiStreamingRoute.DirectBaselineVisit
					{
						rootId = this._observedCloneId,
						zoneSlotIndex = this.Status.target.zoneSlotIndex,
						footprintOrigin = this.Status.target.footprintOrigin,
						resourcePath = this.Status.targetResourcePath,
						operational = true
					});
				}
				this.Event(recorder, "poi_operational", string.Concat(new string[]
				{
					"family=",
					this.Status.family,
					"; address=",
					this.Status.payloadAddress,
					"; proxy=",
					((this._targetProxy != null) ? this._targetProxy.GetInstanceID() : 0).ToString(),
					"; payload=",
					this._observedCloneId.ToString(),
					"; nativePrefab=",
					((this._observedAsset != null) ? this._observedAsset.GetInstanceID() : 0).ToString(),
					"; materialization=",
					this.Status.materializationKind,
					"; resourcePath=",
					this.Status.targetResourcePath,
					"; slot=",
					this.Status.target.zoneSlotIndex.ToString(),
					"; ownerX=",
					this.Status.target.footprintOrigin.x.ToString(),
					"; ownerZ=",
					this.Status.target.footprintOrigin.y.ToString()
				}));
				Action<string> keypoint = this._keypoint;
				if (keypoint != null)
				{
					keypoint((this.Status.completedRoundTrips == 0) ? "poi_near_ready" : ("poi_return_ready_" + this.Status.completedRoundTrips.ToString()));
				}
				if (this._progress.Completed)
				{
					this.Status.completed = true;
					this.Stop(recorder, "completed", "All requested actual " + this.Status.family + " retirement/return cycles completed.");
					return;
				}
				recorder.Phase = 2;
				this.Change(recorder, "outbound");
				return;
			}
			else if (this.Phase == "far_hold")
			{
				if (this._directBaseline)
				{
					this.RefreshTarget();
				}
				HeavyZoneAddressableResidency.AddressSnapshot addressSnapshot2 = HeavyZoneAddressableResidency.ObserveAddress(this.Status.payloadAddress);
				MapPoiStreamingRoute.FarObservation farObservation = new MapPoiStreamingRoute.FarObservation
				{
					holdSeconds = recorder.Elapsed - this._stageStart,
					targetPresent = (this._directBaseline ? (this._manager.GetChunkAt(this.Status.target.footprintOrigin) != null || this._targetDirectRoot != null) : (this._targetProxy != null)),
					cloneAlive = (this._observedClone != null),
					nativePrefabAlive = (this._observedAsset != null),
					cloneRetiredInactive = this.BaselineCloneRetiredInactive(),
					entries = addressSnapshot2.entries,
					owners = addressSnapshot2.owners,
					deferred = addressSnapshot2.deferred,
					releaseCount = addressSnapshot2.releaseCount,
					releaseErrors = addressSnapshot2.releaseErrors
				};
				if (this._progress.TryObserveFar(farObservation))
				{
					this.Status.retirementEvents = this._progress.Retirements;
					this.Status.pooledBaselineRetirements = this._progress.PooledBaselineRetirements;
					if (this._directBaseline)
					{
						this.Status.directBaselineRetirements.Add(new MapPoiStreamingRoute.DirectBaselineRetirement
						{
							rootId = this._observedCloneId,
							holdSeconds = farObservation.holdSeconds,
							targetPresent = farObservation.targetPresent,
							destroyed = !farObservation.cloneAlive,
							poolMemberAndDormant = farObservation.cloneRetiredInactive
						});
					}
					this.Event(recorder, "far_retired", this._config.requireAddressablePayload ? "Target clone and native prefab destroyed; target address entries/owners/deferred=0; target release advanced" : (farObservation.cloneAlive ? (this._directBaseline ? "Direct baseline clone dormant in actual zone pool; no live owner or pending retirement; Resources cache retained" : "Park baseline clone dormant outside live chunk ownership; pooled clone and Resources cache retained") : "Target clone destroyed; Resources cache intentionally retained"));
					Action<string> keypoint2 = this._keypoint;
					if (keypoint2 != null)
					{
						keypoint2((farObservation.cloneAlive ? "poi_far_pooled_" : "poi_far_released_") + this.Status.retirementEvents.ToString());
					}
					recorder.Phase = 2;
					this.Change(recorder, "return");
					return;
				}
				if (farObservation.holdSeconds > 90.0)
				{
					this.Stop(recorder, "incomplete", "Target hierarchy/native ownership did not settle within90s at far point.");
				}
			}
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x001BEBAC File Offset: 0x001BCDAC
		public void Complete(RunSoakRecorder recorder)
		{
			if (this._finalized)
			{
				return;
			}
			this._finalized = true;
			this.Status.outputDirectory = recorder.OutputDirectory;
			this.Status.elapsedSeconds = recorder.Elapsed;
			this.Status.endLogical = ((this._player != null) ? WorldOrigin.ToLogical(this._player.position) : this.Status.startLogical);
			this.Status.graphicsEnd = MapPoiStreamingRoute.CaptureGraphics(this._manager);
			this.Status.graphicsEndFingerprint = this.Status.graphicsEnd.Fingerprint();
			this.Status.graphicsChanged |= !this.Status.graphicsStart.Equals(this.Status.graphicsEnd);
			RunSoakRecorder.Report lastReport = recorder.LastReport;
			this.Status.completed &= this._progress.Completed && lastReport != null && lastReport.outcome == "completed";
			this.Status.validForComparison = this.Status.completed && !this.Status.graphicsChanged && lastReport.completeFrameCoverage && lastReport.applicationErrors == 0 && lastReport.mainCameraRenderingValid && !lastReport.sceneChanged && !lastReport.settingsChanged && !lastReport.sessionSeedChanged;
			if (!this.Status.validForComparison)
			{
				this.Status.outcome = ((lastReport != null && lastReport.outcome != "completed") ? lastReport.outcome : "invalid");
				this.Status.reason = ((lastReport != null && lastReport.outcome != "completed") ? lastReport.reason : "POI lifecycle or recorder comparison criteria failed.");
			}
			this.Status.nativePrefabIds = this._progress.NativePrefabIds.ToArray();
			this.Event(recorder, "stopped", this.Status.outcome + ": " + this.Status.reason);
			File.WriteAllLines(Path.Combine(recorder.OutputDirectory, "poi_route_events.tsv"), this._events);
			File.WriteAllLines(Path.Combine(recorder.OutputDirectory, "poi_route_samples.tsv"), this._samples);
			File.WriteAllText(Path.Combine(recorder.OutputDirectory, "poi_route_protocol.json"), JsonUtility.ToJson(this.Status, true));
		}

		// Token: 0x0600490F RID: 18703 RVA: 0x001BEE1C File Offset: 0x001BD01C
		private void RefreshTarget()
		{
			if (this._directBaseline)
			{
				this._targetProxy = null;
				if (!this._manager.TryObserveMaterializedPoi(this.Status.target.footprintOrigin, this.Status.target.zoneSlotIndex, this.Status.payloadAddress, out this._targetDirectRoot, out this._targetDirectOperational))
				{
					this._targetDirectRoot = null;
					this._targetDirectOperational = false;
					return;
				}
			}
			else
			{
				this._targetProxy = this.FindTarget();
			}
		}

		// Token: 0x06004910 RID: 18704 RVA: 0x001BEE98 File Offset: 0x001BD098
		private HeavyZoneStreamingProxy FindTarget()
		{
			foreach (HeavyZoneStreamingProxy heavyZoneStreamingProxy in Object.FindObjectsOfType<HeavyZoneStreamingProxy>(true))
			{
				PoiResidencyCatalog.Entry entry;
				if (heavyZoneStreamingProxy.gameObject.scene.IsValid() && heavyZoneStreamingProxy.gameObject.scene.isLoaded && PoiResidencyCatalog.TryGetByResourcePath(heavyZoneStreamingProxy.ResourcesPrefabPath, out entry) && !(entry.family != this._config.family) && !(entry.address != this.Status.payloadAddress))
				{
					ChunkInstance chunkContaining = this._manager.GetChunkContaining(heavyZoneStreamingProxy.transform);
					if (chunkContaining != null && chunkContaining.originCell == this.Status.target.footprintOrigin)
					{
						return heavyZoneStreamingProxy;
					}
				}
			}
			return null;
		}

		// Token: 0x06004911 RID: 18705 RVA: 0x001BEF6C File Offset: 0x001BD16C
		private bool BaselineCloneRetiredInactive()
		{
			if (!this._directBaseline)
			{
				return !this._config.requireAddressablePayload && this._config.family == "park" && this._observedClone != null && this._observedProxy != null && !this._observedProxy.NeverPool && this._observedProxy.UsesStagedPooledActivation && !this._observedProxy.gameObject.activeSelf && !this._observedClone.activeInHierarchy && !this._observedProxy.IsOperationalReady && this._observedProxy.StagedSleepWorkRemaining == 0 && this._manager.GetChunkContaining(this._observedProxy.transform) == null;
			}
			return this._observedClone != null && this._manager.IsRootPooledAndDormant(this._observedClone);
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x001BF05C File Offset: 0x001BD25C
		private void Sample(RunSoakRecorder recorder)
		{
			Vector3 vector = WorldOrigin.ToLogical(this._player.position);
			HeavyZoneAddressableResidency.AddressSnapshot addressSnapshot = HeavyZoneAddressableResidency.ObserveAddress(this.Status.payloadAddress);
			this._samples.Add(string.Join("\t", new string[]
			{
				Time.frameCount.ToString(),
				MapPoiStreamingRoute.N(recorder.Elapsed),
				this.Phase,
				this.Status.completedRoundTrips.ToString(),
				MapPoiStreamingRoute.N((double)vector.x),
				MapPoiStreamingRoute.N((double)vector.y),
				MapPoiStreamingRoute.N((double)vector.z),
				MapPoiStreamingRoute.N(this.Status.actualDistanceMetres),
				((this._targetProxy != null) ? this._targetProxy.GetInstanceID() : 0).ToString(),
				MapPoiStreamingRoute.B(this._directBaseline ? this._targetDirectOperational : (this._targetProxy != null && this._targetProxy.IsReady)),
				MapPoiStreamingRoute.B(this._directBaseline ? this._targetDirectOperational : (this._targetProxy != null && this._targetProxy.IsOperationalReady)),
				MapPoiStreamingRoute.B(this._targetProxy != null && this._targetProxy.UsesAddressablePayload),
				MapPoiStreamingRoute.B(this._observedClone != null),
				this._config.requireAddressablePayload ? MapPoiStreamingRoute.B(this._observedAsset != null) : "-1",
				this._manager.SpawnedChunkCount.ToString(),
				this._manager.AvailableChunkPoolCount.ToString(),
				this._manager.AvailableZonePoolCount.ToString(),
				this._manager.PendingZoneRetirementCount.ToString(),
				this._manager.PendingZoneRetirementWorkGroups.ToString(),
				HeavyZoneStreamingProxy.CompletedPayloadCacheEntryCount.ToString(),
				HeavyZoneAddressableResidency.EntryCount.ToString(),
				HeavyZoneAddressableResidency.ActiveLeaseCount.ToString(),
				HeavyZoneAddressableResidency.DeferredReleaseCount.ToString(),
				HeavyZoneAddressableResidency.OperationReleaseCount.ToString(),
				addressSnapshot.entries.ToString(),
				addressSnapshot.owners.ToString(),
				addressSnapshot.deferred.ToString(),
				addressSnapshot.releaseCount.ToString(),
				addressSnapshot.releaseErrors.ToString(),
				MapPoiStreamingRoute.B(this.BaselineCloneRetiredInactive()),
				((this._targetDirectRoot != null) ? this._targetDirectRoot.GetInstanceID() : 0).ToString(),
				MapPoiStreamingRoute.B(this._targetDirectOperational),
				this.Status.target.zoneSlotIndex.ToString()
			}));
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x001BF38C File Offset: 0x001BD58C
		private void Change(RunSoakRecorder recorder, string phase)
		{
			this.Status.phase = phase;
			this._stageStart = recorder.Elapsed;
			this.Event(recorder, "phase", phase);
		}

		// Token: 0x06004914 RID: 18708 RVA: 0x001BF3B4 File Offset: 0x001BD5B4
		private void Event(RunSoakRecorder recorder, string name, string details)
		{
			this._events.Add(string.Concat(new string[]
			{
				Time.frameCount.ToString(),
				"\t",
				MapPoiStreamingRoute.N(recorder.Elapsed),
				"\t",
				name,
				"\t",
				(details ?? "").Replace('\t', ' ').Replace('\n', ' ')
			}));
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x001BF430 File Offset: 0x001BD630
		private void Stop(RunSoakRecorder recorder, string outcome, string reason)
		{
			this.Status.outcome = outcome;
			this.Status.reason = reason;
			recorder.Stop(outcome, reason);
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x001BF452 File Offset: 0x001BD652
		public static Vector3 StepPosition(Vector3 current, Vector3 destination, float speed, double seconds)
		{
			return Vector3.MoveTowards(current, destination, (float)((double)speed * Math.Max(0.0, Math.Min(seconds, 0.1))));
		}

		// Token: 0x06004917 RID: 18711 RVA: 0x001BF47C File Offset: 0x001BD67C
		public static string ParseFamilyOptIn(string value)
		{
			if (value == null || PoiResidencyCatalog.IsFamily(value))
			{
				return value;
			}
			throw new ArgumentException("-runSoakPoi requires an exact POI catalog family; off/all are residency modes, not route targets.");
		}

		// Token: 0x06004918 RID: 18712 RVA: 0x001BF498 File Offset: 0x001BD698
		public static double MinimumSeconds(Vector3 start, MapPoiStreamingRoute.Target target, MapPoiStreamingRoute.Config config)
		{
			return ((double)(MapPoiStreamingRoute.PlanarDistance(start, target.alignmentLogical) + MapPoiStreamingRoute.PlanarDistance(target.alignmentLogical, target.approachLogical)) + 2.0 * (double)config.roundTrips * (double)MapPoiStreamingRoute.PlanarDistance(target.approachLogical, target.farLogical)) / (double)config.speedMetresPerSecond + 31.0 * (double)config.roundTrips;
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x001BF504 File Offset: 0x001BD704
		public static MapPoiStreamingRoute.Target CreateTarget(Vector3 start, Vector2Int origin, Vector2Int size, Vector3 center, float cellSize, int radius)
		{
			bool flag = Mathf.Abs(center.x - start.x) >= Mathf.Abs(center.z - start.z);
			int num = ((flag ? (center.x >= start.x) : (center.z >= start.z)) ? 1 : (-1));
			int num2 = ((num > 0) ? ((flag ? (origin.x + size.x) : (origin.y + size.y)) - 1 + radius + 3) : ((flag ? origin.x : origin.y) - radius - 3));
			MapPoiStreamingRoute.Target target = new MapPoiStreamingRoute.Target();
			target.footprintOrigin = origin;
			target.gridSize = size;
			target.cellSize = cellSize;
			target.runtimeDespawnRadius = radius;
			target.poiLogical = new Vector3(center.x, start.y, center.z);
			target.approachDirection = (flag ? new Vector2Int(num, 0) : new Vector2Int(0, num));
			target.alignmentLogical = (flag ? new Vector3(start.x, start.y, center.z) : new Vector3(center.x, start.y, start.z));
			target.approachLogical = (flag ? new Vector3(center.x - (float)num * ((float)size.x * cellSize * 0.5f + 5f), start.y, center.z) : new Vector3(center.x, start.y, center.z - (float)num * ((float)size.y * cellSize * 0.5f + 5f)));
			target.farLogical = (flag ? new Vector3((float)num2 * cellSize, start.y, center.z) : new Vector3(center.x, start.y, (float)num2 * cellSize));
			MapPoiStreamingRoute.ValidateTarget(target);
			return target;
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x001BF6F0 File Offset: 0x001BD8F0
		private static void ValidateTarget(MapPoiStreamingRoute.Target target)
		{
			if (target.gridSize.x < 1 || target.gridSize.y < 1 || target.cellSize <= 0f || !MapPoiStreamingRoute.Finite((double)target.cellSize) || target.runtimeDespawnRadius < 0 || !MapPoiStreamingRoute.Finite(target.alignmentLogical) || !MapPoiStreamingRoute.Finite(target.approachLogical) || !MapPoiStreamingRoute.Finite(target.farLogical) || MapPoiStreamingRoute.PlanarDistance(target.approachLogical, target.farLogical) <= 0f)
			{
				throw new ArgumentException("Invalid POI target geometry.");
			}
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x001BF788 File Offset: 0x001BD988
		private static MapVehicleStreamingBenchmark.GraphicsSnapshot CaptureGraphics(ChunkManager manager)
		{
			RenderPipelineAsset currentRenderPipeline = GraphicsSettings.currentRenderPipeline;
			UniversalRenderPipelineAsset universalRenderPipelineAsset = currentRenderPipeline as UniversalRenderPipelineAsset;
			return new MapVehicleStreamingBenchmark.GraphicsSnapshot
			{
				pipelineAssetName = ((currentRenderPipeline != null) ? currentRenderPipeline.name : ""),
				pipelineType = ((currentRenderPipeline != null) ? currentRenderPipeline.GetType().FullName : "BuiltIn"),
				mapCatalogResource = ((manager != null) ? manager.ActiveMapCatalogResource : ""),
				qualityLevel = QualitySettings.GetQualityLevel(),
				textureMipmapLimit = QualitySettings.globalTextureMipmapLimit,
				width = Screen.width,
				height = Screen.height,
				fullscreenMode = (int)Screen.fullScreenMode,
				vSyncCount = QualitySettings.vSyncCount,
				targetFrameRate = Application.targetFrameRate,
				qualityAntiAliasing = QualitySettings.antiAliasing,
				anisotropicFiltering = (int)QualitySettings.anisotropicFiltering,
				maximumLODLevel = QualitySettings.maximumLODLevel,
				lodBias = QualitySettings.lodBias,
				streamingMipmapsActive = QualitySettings.streamingMipmapsActive,
				universalPipeline = (universalRenderPipelineAsset != null),
				renderScale = ((universalRenderPipelineAsset != null) ? universalRenderPipelineAsset.renderScale : 1f),
				msaaSamples = ((universalRenderPipelineAsset != null) ? universalRenderPipelineAsset.msaaSampleCount : 0),
				shadowDistance = ((universalRenderPipelineAsset != null) ? universalRenderPipelineAsset.shadowDistance : QualitySettings.shadowDistance),
				shadowCascades = ((universalRenderPipelineAsset != null) ? universalRenderPipelineAsset.shadowCascadeCount : QualitySettings.shadowCascades),
				mainLightShadowResolution = ((universalRenderPipelineAsset != null) ? universalRenderPipelineAsset.mainLightShadowmapResolution : 0),
				additionalLightShadowResolution = ((universalRenderPipelineAsset != null) ? universalRenderPipelineAsset.additionalLightsShadowmapResolution : 0),
				mainLightShadows = (universalRenderPipelineAsset != null && universalRenderPipelineAsset.supportsMainLightShadows),
				additionalLightShadows = (universalRenderPipelineAsset != null && universalRenderPipelineAsset.supportsAdditionalLightShadows),
				softShadows = (universalRenderPipelineAsset != null && universalRenderPipelineAsset.supportsSoftShadows)
			};
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x001BF992 File Offset: 0x001BDB92
		private static float PlanarDistance(Vector3 a, Vector3 b)
		{
			return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x001BF9BB File Offset: 0x001BDBBB
		private static bool Finite(double value)
		{
			return !double.IsNaN(value) && !double.IsInfinity(value);
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x001BF9D0 File Offset: 0x001BDBD0
		private static bool Finite(Vector3 value)
		{
			return MapPoiStreamingRoute.Finite((double)value.x) && MapPoiStreamingRoute.Finite((double)value.y) && MapPoiStreamingRoute.Finite((double)value.z);
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x001BF9FC File Offset: 0x001BDBFC
		private static string N(double value)
		{
			return value.ToString("R", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x001BFA0F File Offset: 0x001BDC0F
		private static string B(bool value)
		{
			if (!value)
			{
				return "0";
			}
			return "1";
		}

		// Token: 0x040048F4 RID: 18676
		private readonly Transform _player;

		// Token: 0x040048F5 RID: 18677
		private readonly Rigidbody _body;

		// Token: 0x040048F6 RID: 18678
		private readonly ChunkManager _manager;

		// Token: 0x040048F7 RID: 18679
		private readonly MapPoiStreamingRoute.Config _config;

		// Token: 0x040048F8 RID: 18680
		private readonly Action<string> _keypoint;

		// Token: 0x040048F9 RID: 18681
		private readonly MapPoiStreamingRoute.Progress _progress;

		// Token: 0x040048FA RID: 18682
		private readonly int _sceneHandle;

		// Token: 0x040048FB RID: 18683
		private readonly List<string> _events = new List<string> { "frame\telapsed_s\tevent\tdetails" };

		// Token: 0x040048FC RID: 18684
		private readonly List<string> _samples = new List<string> { "frame\telapsed_s\tstage\tcompleted_round_trips\tlogical_x\tlogical_y\tlogical_z\tdistance_m\tpoi_proxy_id\tready\toperational\taddressable\tclone_alive\tnative_prefab_alive\tlive_chunks\tpooled_chunks\tpooled_zones\tretiring_zones\tretirement_groups\tresource_cache\taddressable_entries\taddressable_owners\tdeferred_releases\trelease_count\ttarget_address_entries\ttarget_address_owners\ttarget_address_deferred\ttarget_address_releases\ttarget_address_release_errors\tbaseline_clone_retired_inactive\tdirect_target_root_id\tdirect_target_operational\ttarget_zone_slot" };

		// Token: 0x040048FD RID: 18685
		private HeavyZoneStreamingProxy _targetProxy;

		// Token: 0x040048FE RID: 18686
		private HeavyZoneStreamingProxy _observedProxy;

		// Token: 0x040048FF RID: 18687
		private GameObject _observedClone;

		// Token: 0x04004900 RID: 18688
		private GameObject _observedAsset;

		// Token: 0x04004901 RID: 18689
		private GameObject _targetDirectRoot;

		// Token: 0x04004902 RID: 18690
		private bool _targetDirectOperational;

		// Token: 0x04004903 RID: 18691
		private int _observedCloneId;

		// Token: 0x04004904 RID: 18692
		private readonly bool _directBaseline;

		// Token: 0x04004905 RID: 18693
		private double _stageStart;

		// Token: 0x04004906 RID: 18694
		private double _nextSample;

		// Token: 0x04004907 RID: 18695
		private bool _finalized;

		// Token: 0x02000A02 RID: 2562
		[Serializable]
		public sealed class Config
		{
			// Token: 0x06004921 RID: 18721 RVA: 0x001BFA20 File Offset: 0x001BDC20
			public void Validate()
			{
				if (!PoiResidencyCatalog.IsFamily(this.family))
				{
					throw new ArgumentException("POI route requires an exact catalog family.", "family");
				}
				if (this.roundTrips < 1 || this.roundTrips > 3 || this.searchRadiusCells < 1 || this.searchRadiusCells > 64 || !MapPoiStreamingRoute.Finite((double)this.speedMetresPerSecond) || this.speedMetresPerSecond <= 0f || this.speedMetresPerSecond > 100f || !MapPoiStreamingRoute.Finite(this.maximumSeconds) || this.maximumSeconds < 60.0 || this.maximumSeconds > 3600.0)
				{
					throw new ArgumentOutOfRangeException("POI route requires trips1..3, radius1..64, speed(0,100] and budget60..3600s.");
				}
			}

			// Token: 0x04004909 RID: 18697
			public string family = "university";

			// Token: 0x0400490A RID: 18698
			public int expectedSeed = 57391;

			// Token: 0x0400490B RID: 18699
			public int roundTrips = 1;

			// Token: 0x0400490C RID: 18700
			public int searchRadiusCells = 48;

			// Token: 0x0400490D RID: 18701
			public float speedMetresPerSecond = 15.37f;

			// Token: 0x0400490E RID: 18702
			public double maximumSeconds = 240.0;

			// Token: 0x0400490F RID: 18703
			public bool requireAddressablePayload;
		}

		// Token: 0x02000A03 RID: 2563
		[Serializable]
		public sealed class Target
		{
			// Token: 0x04004910 RID: 18704
			public Vector2Int footprintOrigin;

			// Token: 0x04004911 RID: 18705
			public Vector2Int gridSize;

			// Token: 0x04004912 RID: 18706
			public Vector3 poiLogical;

			// Token: 0x04004913 RID: 18707
			public Vector3 alignmentLogical;

			// Token: 0x04004914 RID: 18708
			public Vector3 approachLogical;

			// Token: 0x04004915 RID: 18709
			public Vector3 farLogical;

			// Token: 0x04004916 RID: 18710
			public string family;

			// Token: 0x04004917 RID: 18711
			public string payloadAddress;

			// Token: 0x04004918 RID: 18712
			public Vector2Int approachDirection;

			// Token: 0x04004919 RID: 18713
			public float cellSize;

			// Token: 0x0400491A RID: 18714
			public int runtimeDespawnRadius;

			// Token: 0x0400491B RID: 18715
			public int scannedCells;

			// Token: 0x0400491C RID: 18716
			public int zoneSlotIndex = -1;

			// Token: 0x0400491D RID: 18717
			public string selectionEvidence = "bounded Classic-compatible candidate preview; actual owner/slot materialization proof required";
		}

		// Token: 0x02000A04 RID: 2564
		public sealed class Locator
		{
			// Token: 0x17000C61 RID: 3169
			// (get) Token: 0x06004924 RID: 18724 RVA: 0x001BFB40 File Offset: 0x001BDD40
			// (set) Token: 0x06004925 RID: 18725 RVA: 0x001BFB48 File Offset: 0x001BDD48
			public bool IsDone { get; private set; }

			// Token: 0x17000C62 RID: 3170
			// (get) Token: 0x06004926 RID: 18726 RVA: 0x001BFB51 File Offset: 0x001BDD51
			// (set) Token: 0x06004927 RID: 18727 RVA: 0x001BFB59 File Offset: 0x001BDD59
			public MapPoiStreamingRoute.Target Result { get; private set; }

			// Token: 0x17000C63 RID: 3171
			// (get) Token: 0x06004928 RID: 18728 RVA: 0x001BFB62 File Offset: 0x001BDD62
			// (set) Token: 0x06004929 RID: 18729 RVA: 0x001BFB6A File Offset: 0x001BDD6A
			public string Failure { get; private set; }

			// Token: 0x0600492A RID: 18730 RVA: 0x001BFB74 File Offset: 0x001BDD74
			public Locator(ChunkManager manager, Vector3 startLogical, MapPoiStreamingRoute.Config config)
			{
				if (manager == null || config == null)
				{
					throw new ArgumentNullException();
				}
				config.Validate();
				this._manager = manager;
				this._start = startLogical;
				this._config = config;
				this._started = Time.realtimeSinceStartupAsDouble;
				Vector2Int center = manager.WorldToCell(WorldOrigin.ToUnity(startLogical));
				int searchRadiusCells = config.searchRadiusCells;
				for (int i = -searchRadiusCells; i <= searchRadiusCells; i++)
				{
					for (int j = -searchRadiusCells; j <= searchRadiusCells; j++)
					{
						this._cells.Add(center + new Vector2Int(i, j));
					}
				}
				this._cells.Sort(delegate(Vector2Int a, Vector2Int b)
				{
					int num = (a - center).sqrMagnitude.CompareTo((b - center).sqrMagnitude);
					if (num != 0)
					{
						return num;
					}
					if (a.x == b.x)
					{
						return a.y.CompareTo(b.y);
					}
					return a.x.CompareTo(b.x);
				});
			}

			// Token: 0x0600492B RID: 18731 RVA: 0x001BFC44 File Offset: 0x001BDE44
			public bool Advance(int cellBudget = 128)
			{
				if (this.IsDone)
				{
					return true;
				}
				if (cellBudget < 1)
				{
					throw new ArgumentOutOfRangeException("cellBudget");
				}
				int num;
				if (this._manager == null || !this._manager.DiagnosticRuntimeChunkDespawnEnabled || !this._manager.TryGetSessionSeed(out num) || num != this._config.expectedSeed)
				{
					return this.Fail("Map/seed changed or normal runtime retirement is disabled.");
				}
				double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
				if (realtimeSinceStartupAsDouble - this._started > 60.0)
				{
					return this.Fail("Bounded " + this._config.family + " lookup exceeded60 seconds.");
				}
				int num2 = 0;
				while (this._index < this._cells.Count && num2++ < cellBudget && Time.realtimeSinceStartupAsDouble - realtimeSinceStartupAsDouble < 0.003)
				{
					List<Vector2Int> cells = this._cells;
					int index = this._index;
					this._index = index + 1;
					Vector2Int vector2Int = cells[index];
					Vector2Int vector2Int2;
					ChunkSpawnResolver.SpawnEntry spawnEntry;
					if (this._manager.TryResolveDiagnosticPoiPreview(vector2Int, PoiResidencyCatalog.KindFor(this._config.family), out vector2Int2, out spawnEntry) && this._visited.Add(vector2Int2))
					{
						ChunkData chunkData = spawnEntry.chunkData;
						if (!(chunkData == null) && chunkData.availableZones != null && spawnEntry.zoneIndices != null)
						{
							string text = null;
							int num3 = -1;
							for (int i = 0; i < spawnEntry.zoneIndices.Length; i++)
							{
								int num4 = spawnEntry.zoneIndices[i];
								if (num4 >= 0 && num4 < chunkData.availableZones.Count)
								{
									ZoneData zoneData = chunkData.availableZones[num4];
									HeavyZoneStreamingProxy heavyZoneStreamingProxy = ((zoneData != null && zoneData.zonePrefab != null) ? zoneData.zonePrefab.GetComponent<HeavyZoneStreamingProxy>() : null);
									PoiResidencyCatalog.Entry entry;
									if (heavyZoneStreamingProxy != null && PoiResidencyCatalog.TryGetByResourcePath(heavyZoneStreamingProxy.ResourcesPrefabPath, out entry) && entry.family == this._config.family)
									{
										text = entry.address;
										num3 = i;
										break;
									}
								}
							}
							if (text != null)
							{
								MapPoiStreamingRoute.Target target = MapPoiStreamingRoute.CreateTarget(this._start, vector2Int2, chunkData.gridSize, WorldOrigin.ToLogical(this._manager.CellToWorld(vector2Int2, chunkData.gridSize)), this._manager.CellSize, this._manager.DiagnosticRuntimeDespawnRadius);
								target.scannedCells = this._index;
								target.family = this._config.family;
								target.payloadAddress = text;
								target.zoneSlotIndex = num3;
								if (MapPoiStreamingRoute.MinimumSeconds(this._start, target, this._config) + 10.0 <= this._config.maximumSeconds)
								{
									this.Result = target;
									this.IsDone = true;
									return true;
								}
								this._tooFar++;
							}
						}
					}
				}
				return this._index == this._cells.Count && this.Fail((this._tooFar > 0) ? (this._config.family + " candidates exist, but no full route fits the bounded capture budget.") : ("No eligible " + this._config.family + " in bounded production preview; no POI was forced or spawned."));
			}

			// Token: 0x0600492C RID: 18732 RVA: 0x001BFF85 File Offset: 0x001BE185
			private bool Fail(string reason)
			{
				this.Failure = reason;
				this.IsDone = true;
				return true;
			}

			// Token: 0x0400491E RID: 18718
			private readonly ChunkManager _manager;

			// Token: 0x0400491F RID: 18719
			private readonly Vector3 _start;

			// Token: 0x04004920 RID: 18720
			private readonly MapPoiStreamingRoute.Config _config;

			// Token: 0x04004921 RID: 18721
			private readonly List<Vector2Int> _cells = new List<Vector2Int>();

			// Token: 0x04004922 RID: 18722
			private readonly HashSet<Vector2Int> _visited = new HashSet<Vector2Int>();

			// Token: 0x04004923 RID: 18723
			private readonly double _started;

			// Token: 0x04004924 RID: 18724
			private int _index;

			// Token: 0x04004925 RID: 18725
			private int _tooFar;
		}

		// Token: 0x02000A06 RID: 2566
		[Serializable]
		public sealed class Report
		{
			// Token: 0x0400492A RID: 18730
			public string version = "MapPoiStreamingRoute/v4";

			// Token: 0x0400492B RID: 18731
			public string phase = "alignment";

			// Token: 0x0400492C RID: 18732
			public string outcome = "running";

			// Token: 0x0400492D RID: 18733
			public string reason;

			// Token: 0x0400492E RID: 18734
			public string outputDirectory;

			// Token: 0x0400492F RID: 18735
			public string kind = "synthetic-player-translation-not-vehicle-physics";

			// Token: 0x04004930 RID: 18736
			public string mapResidencyPilot;

			// Token: 0x04004931 RID: 18737
			public string family;

			// Token: 0x04004932 RID: 18738
			public string payloadAddress;

			// Token: 0x04004933 RID: 18739
			public string retirementPolicy;

			// Token: 0x04004934 RID: 18740
			public string materializationKind;

			// Token: 0x04004935 RID: 18741
			public string targetResourcePath;

			// Token: 0x04004936 RID: 18742
			public List<MapPoiStreamingRoute.DirectBaselineVisit> directBaselineVisits = new List<MapPoiStreamingRoute.DirectBaselineVisit>();

			// Token: 0x04004937 RID: 18743
			public List<MapPoiStreamingRoute.DirectBaselineRetirement> directBaselineRetirements = new List<MapPoiStreamingRoute.DirectBaselineRetirement>();

			// Token: 0x04004938 RID: 18744
			public bool completed;

			// Token: 0x04004939 RID: 18745
			public bool validForComparison;

			// Token: 0x0400493A RID: 18746
			public bool requireAddressablePayload;

			// Token: 0x0400493B RID: 18747
			public bool graphicsChanged;

			// Token: 0x0400493C RID: 18748
			public int expectedSeed;

			// Token: 0x0400493D RID: 18749
			public int requestedRoundTrips;

			// Token: 0x0400493E RID: 18750
			public int completedRoundTrips;

			// Token: 0x0400493F RID: 18751
			public int retirementEvents;

			// Token: 0x04004940 RID: 18752
			public int pooledBaselineRetirements;

			// Token: 0x04004941 RID: 18753
			public int reusedBaselineReturns;

			// Token: 0x04004942 RID: 18754
			public double elapsedSeconds;

			// Token: 0x04004943 RID: 18755
			public double actualDistanceMetres;

			// Token: 0x04004944 RID: 18756
			public double minimumPossibleSeconds;

			// Token: 0x04004945 RID: 18757
			public double maximumSeconds;

			// Token: 0x04004946 RID: 18758
			public float speedMetresPerSecond;

			// Token: 0x04004947 RID: 18759
			public float stepDeltaClampSeconds = 0.1f;

			// Token: 0x04004948 RID: 18760
			public Vector3 startLogical;

			// Token: 0x04004949 RID: 18761
			public Vector3 endLogical;

			// Token: 0x0400494A RID: 18762
			public MapPoiStreamingRoute.Target target;

			// Token: 0x0400494B RID: 18763
			public MapVehicleStreamingBenchmark.GraphicsSnapshot graphicsStart;

			// Token: 0x0400494C RID: 18764
			public MapVehicleStreamingBenchmark.GraphicsSnapshot graphicsEnd;

			// Token: 0x0400494D RID: 18765
			public string graphicsStartFingerprint;

			// Token: 0x0400494E RID: 18766
			public string graphicsEndFingerprint;

			// Token: 0x0400494F RID: 18767
			public int[] nativePrefabIds;

			// Token: 0x04004950 RID: 18768
			public string limitation = "Synthetic translation with normal map/NPC/rendering. Does not test vehicle physics or walkability. Only actual operational selected-family materializations, observed normal retirement and return can complete the route. Resources baseline retains its normal cache; Park keeps its existing proxy pooling. Kombini/Subway off use actual direct roots and their normal zone pool. A returned direct root may be the same or another prewarmed copy; distinct root IDs do not prove runtime creation. A root active elsewhere cannot certify current pool dormancy. Heavy payload setup CSV remains required, but direct roots prewarmed before capture have no target HeavyProxy setup events. Addressables requires target clone/native destruction and fresh identities; target-address ownership gates are separate from global observations. Player memory is process committed/working set, not VRAM; growth alone is not a leak. Same seed/route/graphics does not make traffic, weather, roster or cache warmth identical. Initial go/no-go, not a long soak.";
		}

		// Token: 0x02000A07 RID: 2567
		[Serializable]
		public sealed class DirectBaselineVisit
		{
			// Token: 0x04004951 RID: 18769
			public int rootId;

			// Token: 0x04004952 RID: 18770
			public int zoneSlotIndex;

			// Token: 0x04004953 RID: 18771
			public Vector2Int footprintOrigin;

			// Token: 0x04004954 RID: 18772
			public string resourcePath;

			// Token: 0x04004955 RID: 18773
			public bool operational;
		}

		// Token: 0x02000A08 RID: 2568
		[Serializable]
		public sealed class DirectBaselineRetirement
		{
			// Token: 0x04004956 RID: 18774
			public int rootId;

			// Token: 0x04004957 RID: 18775
			public double holdSeconds;

			// Token: 0x04004958 RID: 18776
			public bool targetPresent;

			// Token: 0x04004959 RID: 18777
			public bool destroyed;

			// Token: 0x0400495A RID: 18778
			public bool poolMemberAndDormant;
		}

		// Token: 0x02000A09 RID: 2569
		public struct FarObservation
		{
			// Token: 0x0400495B RID: 18779
			public double holdSeconds;

			// Token: 0x0400495C RID: 18780
			public bool targetPresent;

			// Token: 0x0400495D RID: 18781
			public bool cloneAlive;

			// Token: 0x0400495E RID: 18782
			public bool nativePrefabAlive;

			// Token: 0x0400495F RID: 18783
			public bool cloneRetiredInactive;

			// Token: 0x04004960 RID: 18784
			public int entries;

			// Token: 0x04004961 RID: 18785
			public int owners;

			// Token: 0x04004962 RID: 18786
			public int deferred;

			// Token: 0x04004963 RID: 18787
			public int releaseCount;

			// Token: 0x04004964 RID: 18788
			public int releaseErrors;
		}

		// Token: 0x02000A0A RID: 2570
		public sealed class Progress
		{
			// Token: 0x17000C64 RID: 3172
			// (get) Token: 0x06004932 RID: 18738 RVA: 0x001C0083 File Offset: 0x001BE283
			// (set) Token: 0x06004933 RID: 18739 RVA: 0x001C008B File Offset: 0x001BE28B
			public int Retirements { get; private set; }

			// Token: 0x17000C65 RID: 3173
			// (get) Token: 0x06004934 RID: 18740 RVA: 0x001C0094 File Offset: 0x001BE294
			// (set) Token: 0x06004935 RID: 18741 RVA: 0x001C009C File Offset: 0x001BE29C
			public int PooledBaselineRetirements { get; private set; }

			// Token: 0x17000C66 RID: 3174
			// (get) Token: 0x06004936 RID: 18742 RVA: 0x001C00A5 File Offset: 0x001BE2A5
			// (set) Token: 0x06004937 RID: 18743 RVA: 0x001C00AD File Offset: 0x001BE2AD
			public int ReusedBaselineReturns { get; private set; }

			// Token: 0x17000C67 RID: 3175
			// (get) Token: 0x06004938 RID: 18744 RVA: 0x001C00B6 File Offset: 0x001BE2B6
			public int CompletedRoundTrips
			{
				get
				{
					return Math.Min(this.Retirements, Math.Max(0, this._nearVisits - 1));
				}
			}

			// Token: 0x17000C68 RID: 3176
			// (get) Token: 0x06004939 RID: 18745 RVA: 0x001C00D1 File Offset: 0x001BE2D1
			public bool Completed
			{
				get
				{
					return this.CompletedRoundTrips == this._requested;
				}
			}

			// Token: 0x0600493A RID: 18746 RVA: 0x001C00E4 File Offset: 0x001BE2E4
			public Progress(int requested, bool addressable, bool allowPooledBaseline = false)
			{
				if (requested < 1 || requested > 3)
				{
					throw new ArgumentOutOfRangeException("requested");
				}
				if (addressable && allowPooledBaseline)
				{
					throw new ArgumentException("Addressables cannot use pooled baseline evidence.");
				}
				this._requested = requested;
				this._addressable = addressable;
				this._allowPooledBaseline = allowPooledBaseline;
			}

			// Token: 0x0600493B RID: 18747 RVA: 0x001C0150 File Offset: 0x001BE350
			public void ObserveNear(int cloneId, int nativePrefabId, int releaseCount)
			{
				if (this.Completed || this._nearVisits != this.Retirements || cloneId == 0)
				{
					throw new InvalidOperationException("Expected a new actual payload clone after the preceding retirement.");
				}
				bool flag = this._clones.Contains(cloneId);
				if (flag && !this._allowPooledBaseline)
				{
					throw new InvalidOperationException("Expected a new actual payload clone after the preceding retirement.");
				}
				if (this._addressable && (nativePrefabId == 0 || !this._nativeIds.Add(nativePrefabId)))
				{
					throw new InvalidOperationException("Addressable return did not produce a new native prefab identity.");
				}
				this._clones.Add(cloneId);
				if (flag)
				{
					int reusedBaselineReturns = this.ReusedBaselineReturns;
					this.ReusedBaselineReturns = reusedBaselineReturns + 1;
				}
				this._nearVisits++;
				this._releaseAtNear = releaseCount;
				if (this._addressable)
				{
					this.NativePrefabIds.Add(nativePrefabId);
				}
			}

			// Token: 0x0600493C RID: 18748 RVA: 0x001C0210 File Offset: 0x001BE410
			public bool TryObserveFar(MapPoiStreamingRoute.FarObservation value)
			{
				if (this._nearVisits != this.Retirements + 1 || this.Retirements >= this._requested || value.holdSeconds < 31.0 || value.targetPresent || (value.cloneAlive && (!this._allowPooledBaseline || !value.cloneRetiredInactive)))
				{
					return false;
				}
				if (this._addressable && (value.nativePrefabAlive || value.entries != 0 || value.owners != 0 || value.deferred != 0 || value.releaseCount <= this._releaseAtNear || value.releaseErrors != 0))
				{
					return false;
				}
				int num;
				if (value.cloneAlive)
				{
					num = this.PooledBaselineRetirements;
					this.PooledBaselineRetirements = num + 1;
				}
				num = this.Retirements;
				this.Retirements = num + 1;
				return true;
			}

			// Token: 0x04004965 RID: 18789
			private readonly int _requested;

			// Token: 0x04004966 RID: 18790
			private readonly bool _addressable;

			// Token: 0x04004967 RID: 18791
			private readonly bool _allowPooledBaseline;

			// Token: 0x04004968 RID: 18792
			private int _nearVisits;

			// Token: 0x04004969 RID: 18793
			private int _releaseAtNear;

			// Token: 0x0400496A RID: 18794
			private readonly HashSet<int> _clones = new HashSet<int>();

			// Token: 0x0400496B RID: 18795
			private readonly HashSet<int> _nativeIds = new HashSet<int>();

			// Token: 0x0400496C RID: 18796
			public readonly List<int> NativePrefabIds = new List<int>();
		}
	}
}
