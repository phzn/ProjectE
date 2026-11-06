using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Features.RenderingPrecision;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ProjectExhibitionist.Diagnostics
{
	// Token: 0x02000A0B RID: 2571
	[DefaultExecutionOrder(30000)]
	public sealed class MapVehicleStreamingBenchmark : MonoBehaviour
	{
		// Token: 0x0600493D RID: 18749 RVA: 0x001C02D8 File Offset: 0x001BE4D8
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Install()
		{
			if (Application.isEditor)
			{
				return;
			}
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (Array.IndexOf<string>(commandLineArgs, "-mapVehicleBenchmark") < 0)
			{
				return;
			}
			GameObject gameObject = new GameObject("[MapVehicleStreamingBenchmark]");
			Object.DontDestroyOnLoad(gameObject);
			MapVehicleStreamingBenchmark mapVehicleStreamingBenchmark = gameObject.AddComponent<MapVehicleStreamingBenchmark>();
			try
			{
				mapVehicleStreamingBenchmark.Configure(MapVehicleStreamingBenchmark.Configuration.Parse(commandLineArgs));
			}
			catch (Exception ex)
			{
				mapVehicleStreamingBenchmark.Fail("Configuration failed: " + ex.Message);
			}
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x001C0350 File Offset: 0x001BE550
		private void Configure(MapVehicleStreamingBenchmark.Configuration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			this._configuration = configuration;
			if (Application.isBatchMode)
			{
				throw new InvalidOperationException("A rendered standalone player is required.");
			}
			if (!string.IsNullOrWhiteSpace(configuration.routePath))
			{
				if (new FileInfo(configuration.routePath).Length > 65536L)
				{
					throw new ArgumentException("Route baseline exceeds 64KiB.");
				}
				string text = File.ReadAllText(configuration.routePath);
				this._route = JsonUtility.FromJson<MapVehicleStreamingBenchmark.RouteBaseline>(text);
				if (this._route == null)
				{
					throw new ArgumentException("Invalid route baseline JSON.");
				}
				this._route.Validate();
			}
			configuration.outputDirectory = Path.GetFullPath(configuration.outputDirectory ?? Path.Combine(Application.persistentDataPath, "PerformanceCaptures", "Vehicle_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff")));
			if (Directory.Exists(configuration.outputDirectory) && Directory.GetFileSystemEntries(configuration.outputDirectory).Length != 0)
			{
				throw new IOException("Vehicle benchmark output directory must be empty.");
			}
			Directory.CreateDirectory(configuration.outputDirectory);
			this._ownsDirectory = true;
			MapVehicleStreamingBenchmark.Protocol protocol = new MapVehicleStreamingBenchmark.Protocol();
			protocol.outputDirectory = configuration.outputDirectory;
			protocol.requestedSeconds = configuration.seconds;
			protocol.vehicleType = configuration.vehicleType.ToString();
			protocol.routeBaselineConfigured = this._route != null;
			MapVehicleStreamingBenchmark.RouteBaseline route = this._route;
			protocol.routeBaselineId = ((route != null) ? route.id : null);
			protocol.routeBaseline = this._route;
			this._report = protocol;
			this._armedAt = Time.realtimeSinceStartupAsDouble;
			this.SaveProtocol();
			Debug.Log("[MapVehicleBenchmark] Armed for " + configuration.vehicleType.ToString() + "; mount and drive normally. Capture starts after 10 seconds on the same vehicle.");
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x001C0508 File Offset: 0x001BE708
		private void Update()
		{
			if (this._finished || this._configuration == null || this._recorder != null)
			{
				return;
			}
			if (Time.realtimeSinceStartupAsDouble - this._armedAt >= 1200.0)
			{
				this.Fail("No valid mount within 20 minutes.");
				return;
			}
			Transform player = PlayerRegistry.Player;
			vVehicleController vVehicleController = ((player != null) ? player.GetComponentInParent<vVehicleController>() : null);
			if (vVehicleController == null || !vVehicleController.IsBeingRidden || vVehicleController.CurrentRider == null)
			{
				if (this._mountedAt >= 0.0)
				{
					this.Fail("Rider dismounted during warmup.");
				}
				return;
			}
			if (vVehicleController.Type != this._configuration.vehicleType)
			{
				this.Fail("Mounted the wrong vehicle type.");
				return;
			}
			if (vVehicleController.CurrentRider.transform != player)
			{
				this.Fail("Mounted rider does not match PlayerRegistry.");
				return;
			}
			if (vVehicleController.IsTransitioning)
			{
				return;
			}
			if (this._mountedAt < 0.0)
			{
				this._vehicle = vVehicleController;
				this._vehicleBody = vVehicleController.GetComponent<Rigidbody>();
				this._warmupVehicleId = vVehicleController.GetInstanceID();
				this._mountedAt = Time.realtimeSinceStartupAsDouble;
				this._report.phase = "mount_warmup";
				this.SaveProtocol();
			}
			if (vVehicleController.GetInstanceID() != this._warmupVehicleId)
			{
				this.Fail("Vehicle changed during warmup.");
				return;
			}
			if (Time.realtimeSinceStartupAsDouble - this._mountedAt < 10.0)
			{
				return;
			}
			try
			{
				this.BeginCapture();
			}
			catch (Exception ex)
			{
				this.Fail("Capture start failed: " + ex.Message);
			}
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x001C06A8 File Offset: 0x001BE8A8
		private void BeginCapture()
		{
			ChunkManager instance = ChunkManager.Instance;
			if (instance == null || !instance.HasChunkUnderPlayer)
			{
				throw new InvalidOperationException("No current streamed map chunk under the rider.");
			}
			int num;
			bool flag = instance.TryGetSessionSeed(out num);
			this._evidence = new MapVehicleStreamingBenchmark.Evidence(this._configuration.vehicleType, this.ObserveVehicle(), this._configuration.seconds, flag, num, this._route, new MapVehicleStreamingBenchmark.GraphicsSnapshot?(this.CaptureGraphics()));
			this._report = this._evidence.Report;
			this._report.outputDirectory = this._configuration.outputDirectory;
			this._report.vehicleName = this._vehicle.name;
			if (this._evidence.Invalid)
			{
				throw new InvalidOperationException(this._evidence.InvalidReason);
			}
			this._rows = new MapVehicleStreamingBenchmark.Row[(int)Math.Ceiling(this._configuration.seconds) + 4];
			this._createdFrame = Time.frameCount;
			this._recorder = RunSoakRecorder.Begin(new RunSoakRecorder.Options
			{
				outputDirectory = this._configuration.outputDirectory,
				durationSeconds = this._configuration.seconds,
				maximumFrames = 1000000,
				memoryIntervalSeconds = 1.0,
				requireMainCameraRendering = true,
				protocol = "MapVehicleStreamingBenchmark/v2; passive manual " + this._configuration.vehicleType.ToString() + "; unchanged physics/NPC settings; see vehicle_protocol.json and vehicle_samples.csv."
			});
			this._recorder.Phase = 1;
			this._recorder.RuntimeTick += this.OnRecorderTick;
			this._recorder.Stopped += this.OnRecorderStopped;
			this.SaveProtocol();
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x001C085C File Offset: 0x001BEA5C
		private void LateUpdate()
		{
			if (this._finished || this._recorder == null || !this._recorder.IsRecording || Time.frameCount <= this._createdFrame)
			{
				return;
			}
			this._pending = this.ObserveVehicle();
			this._pendingFrame = Time.frameCount;
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x001C08B4 File Offset: 0x001BEAB4
		private MapVehicleStreamingBenchmark.Observation ObserveVehicle()
		{
			MapVehicleStreamingBenchmark.Observation observation = new MapVehicleStreamingBenchmark.Observation
			{
				sourceFrame = Time.frameCount
			};
			if (this._vehicle == null || this._vehicleBody == null)
			{
				return observation;
			}
			Transform player = PlayerRegistry.Player;
			observation.vehicleId = this._vehicle.GetInstanceID();
			observation.vehicleType = this._vehicle.Type;
			observation.riderId = ((this._vehicle.CurrentRider != null) ? this._vehicle.CurrentRider.GetInstanceID() : 0);
			observation.mounted = this._vehicle.isActiveAndEnabled && this._vehicle.IsBeingRidden && !this._vehicle.IsTransitioning && this._vehicle.CurrentRider != null && this._vehicle.CurrentRider.transform == player && this._vehicle.Vehicle != null && this._vehicle.Vehicle.isInControl;
			observation.dynamicBody = !this._vehicleBody.isKinematic;
			observation.logicalPosition = WorldOrigin.ToLogical(this._vehicleBody.position);
			observation.speed = MapVehicleStreamingBenchmark.PlanarSpeed(this._vehicleBody.velocity);
			observation.controlsFresh = this._vehicle.DiagnosticInputFrame == Time.frameCount;
			observation.controlsAvailable = this._vehicle.DiagnosticInputSuppression == "none";
			observation.throttle = this._vehicle.DiagnosticRawThrottle;
			observation.steering = this._vehicle.DiagnosticRawSteering;
			observation.sprint = this._vehicle.DiagnosticRawSprint;
			observation.brake = this._vehicle.DiagnosticRawBrake;
			observation.grounded = this._vehicle.Vehicle != null && this._vehicle.Vehicle.OnGround();
			return observation;
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x001C0AB0 File Offset: 0x001BECB0
		private void OnRecorderTick(RunSoakRecorder recorder)
		{
			this._evidence.ObserveGraphics(this.CaptureGraphics());
			if (Time.frameCount > this._createdFrame + 1 && this._pendingFrame != Time.frameCount - 1)
			{
				recorder.Stop("invalid", "A completed frame lacks its vehicle observation.");
				return;
			}
			this.ConsumeCompletedObservation();
			if (this._evidence.Invalid)
			{
				recorder.Stop("invalid", this._evidence.InvalidReason);
				return;
			}
			if (recorder.Elapsed >= this._nextRow && this._pendingFrame == Time.frameCount - 1)
			{
				if (this._rowCount >= this._rows.Length)
				{
					recorder.Stop("invalid", "Vehicle sample buffer filled.");
					return;
				}
				this.CaptureRow(recorder.Elapsed, this._pending);
				this._nextRow = recorder.Elapsed + 1.0;
			}
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x001C0B90 File Offset: 0x001BED90
		private void ConsumeCompletedObservation()
		{
			if (this._pendingFrame != Time.frameCount - 1 || this._pendingFrame <= this._consumedFrame)
			{
				return;
			}
			this._consumedFrame = this._pendingFrame;
			this._evidence.Observe(this._pending, (double)Time.unscaledDeltaTime);
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x001C0BE0 File Offset: 0x001BEDE0
		private void CaptureRow(double elapsed, MapVehicleStreamingBenchmark.Observation vehicle)
		{
			ChunkManager instance = ChunkManager.Instance;
			PlayerProximityProvider instance2 = PlayerProximityProvider.Instance;
			Vector2Int vector2Int = ((instance2 != null) ? instance2.CurrentCell : new Vector2Int(int.MinValue, int.MinValue));
			MapVehicleStreamingBenchmark.Row[] rows = this._rows;
			int rowCount = this._rowCount;
			this._rowCount = rowCount + 1;
			rows[rowCount] = new MapVehicleStreamingBenchmark.Row
			{
				vehicle = vehicle,
				elapsed = elapsed,
				cellX = vector2Int.x,
				cellZ = vector2Int.y,
				hasCurrentChunk = ((instance != null && instance.HasChunkUnderPlayer) ? 1 : 0),
				liveChunks = ((instance != null) ? instance.SpawnedChunkCount : (-1)),
				pooledChunks = ((instance != null) ? instance.AvailableChunkPoolCount : (-1)),
				pooledZones = ((instance != null) ? instance.AvailableZonePoolCount : (-1)),
				retiringZones = ((instance != null) ? instance.PendingZoneRetirementCount : (-1)),
				retirementGroups = ((instance != null) ? instance.PendingZoneRetirementWorkGroups : (-1)),
				residentPayloads = ((instance != null) ? instance.ResidentHeavyPayloadCount : (-1)),
				pendingPayloads = ((instance != null) ? instance.PendingHeavyPayloadCount : (-1)),
				assetLoads = HeavyZoneStreamingProxy.ActivePayloadAssetLoadCount,
				resourceCache = HeavyZoneStreamingProxy.CompletedPayloadCacheEntryCount,
				addressableEntries = HeavyZoneAddressableResidency.EntryCount,
				addressableOwners = HeavyZoneAddressableResidency.ActiveLeaseCount
			};
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x001C0D68 File Offset: 0x001BEF68
		private void OnRecorderStopped(RunSoakRecorder recorder)
		{
			this._evidence.ObserveGraphics(this.CaptureGraphics());
			this.ConsumeCompletedObservation();
			RunSoakRecorder.Report lastReport = recorder.LastReport;
			bool flag = lastReport.outcome == "completed" && lastReport.completeFrameCoverage && lastReport.mainCameraRenderingValid && !lastReport.sceneChanged && !lastReport.settingsChanged && !lastReport.sessionSeedChanged && lastReport.applicationErrors == 0;
			this._evidence.Finish(flag, lastReport.reason);
			this._report.frameTimes = lastReport.allFrames;
			this._report.sampledRows = this._rowCount;
			this._finished = true;
			try
			{
				this.ExportRows();
				this.SaveProtocol();
			}
			catch (Exception ex)
			{
				string text = "[MapVehicleBenchmark] Export failed: ";
				Exception ex2 = ex;
				Debug.LogError(text + ((ex2 != null) ? ex2.ToString() : null));
			}
			this._rows = null;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x001C0E5C File Offset: 0x001BF05C
		private void Fail(string reason)
		{
			if (this._finished)
			{
				return;
			}
			if (this._recorder != null && this._recorder.IsRecording)
			{
				this._recorder.Stop("invalid", reason);
				return;
			}
			this._finished = true;
			if (this._report != null)
			{
				this._report.phase = "finished";
				this._report.outcome = "invalid";
				this._report.reason = reason;
			}
			if (this._ownsDirectory)
			{
				this.SaveProtocol();
			}
			Debug.LogWarning("[MapVehicleBenchmark] " + reason);
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x001C0EF8 File Offset: 0x001BF0F8
		private void OnApplicationQuit()
		{
			if (!this._finished)
			{
				this.Fail("Application exited before vehicle capture completed.");
			}
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x001C0F0D File Offset: 0x001BF10D
		private void OnDestroy()
		{
			if (!this._finished)
			{
				this.Fail("Vehicle recorder was destroyed before completion.");
			}
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x001C0F22 File Offset: 0x001BF122
		private void SaveProtocol()
		{
			if (!this._ownsDirectory || this._report == null)
			{
				return;
			}
			File.WriteAllText(Path.Combine(this._configuration.outputDirectory, "vehicle_protocol.json"), JsonUtility.ToJson(this._report, true));
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x001C0F5C File Offset: 0x001BF15C
		private void ExportRows()
		{
			using (StreamWriter streamWriter = new StreamWriter(Path.Combine(this._configuration.outputDirectory, "vehicle_samples.csv"), false, new UTF8Encoding(false)))
			{
				streamWriter.WriteLine("source_frame,elapsed_s,vehicle_id,vehicle_type,rider_id,mounted,dynamic_body,logical_x,logical_y,logical_z,planar_speed_mps,throttle,steering,sprint,brake,input_fresh,input_available,grounded,cell_x,cell_z,has_current_chunk,live_chunks,pooled_chunks,pooled_zones,retiring_zones,retirement_groups,resident_payloads,pending_payloads,asset_loads,resource_payload_cache,addressable_entries,addressable_owners");
				for (int i = 0; i < this._rowCount; i++)
				{
					MapVehicleStreamingBenchmark.Row row = this._rows[i];
					MapVehicleStreamingBenchmark.Observation vehicle = row.vehicle;
					streamWriter.WriteLine(string.Join(",", new string[]
					{
						MapVehicleStreamingBenchmark.Number((double)vehicle.sourceFrame),
						MapVehicleStreamingBenchmark.Number(row.elapsed),
						MapVehicleStreamingBenchmark.Number((double)vehicle.vehicleId),
						MapVehicleStreamingBenchmark.Number((double)vehicle.vehicleType),
						MapVehicleStreamingBenchmark.Number((double)vehicle.riderId),
						MapVehicleStreamingBenchmark.Bit(vehicle.mounted),
						MapVehicleStreamingBenchmark.Bit(vehicle.dynamicBody),
						MapVehicleStreamingBenchmark.Number((double)vehicle.logicalPosition.x),
						MapVehicleStreamingBenchmark.Number((double)vehicle.logicalPosition.y),
						MapVehicleStreamingBenchmark.Number((double)vehicle.logicalPosition.z),
						MapVehicleStreamingBenchmark.Number((double)vehicle.speed),
						MapVehicleStreamingBenchmark.Number((double)vehicle.throttle),
						MapVehicleStreamingBenchmark.Number((double)vehicle.steering),
						MapVehicleStreamingBenchmark.Bit(vehicle.sprint),
						MapVehicleStreamingBenchmark.Bit(vehicle.brake),
						MapVehicleStreamingBenchmark.Bit(vehicle.controlsFresh),
						MapVehicleStreamingBenchmark.Bit(vehicle.controlsAvailable),
						MapVehicleStreamingBenchmark.Bit(vehicle.grounded),
						MapVehicleStreamingBenchmark.Number((double)row.cellX),
						MapVehicleStreamingBenchmark.Number((double)row.cellZ),
						MapVehicleStreamingBenchmark.Number((double)row.hasCurrentChunk),
						MapVehicleStreamingBenchmark.Number((double)row.liveChunks),
						MapVehicleStreamingBenchmark.Number((double)row.pooledChunks),
						MapVehicleStreamingBenchmark.Number((double)row.pooledZones),
						MapVehicleStreamingBenchmark.Number((double)row.retiringZones),
						MapVehicleStreamingBenchmark.Number((double)row.retirementGroups),
						MapVehicleStreamingBenchmark.Number((double)row.residentPayloads),
						MapVehicleStreamingBenchmark.Number((double)row.pendingPayloads),
						MapVehicleStreamingBenchmark.Number((double)row.assetLoads),
						MapVehicleStreamingBenchmark.Number((double)row.resourceCache),
						MapVehicleStreamingBenchmark.Number((double)row.addressableEntries),
						MapVehicleStreamingBenchmark.Number((double)row.addressableOwners)
					}));
				}
			}
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x001C1200 File Offset: 0x001BF400
		private MapVehicleStreamingBenchmark.GraphicsSnapshot CaptureGraphics()
		{
			RenderPipelineAsset currentRenderPipeline = GraphicsSettings.currentRenderPipeline;
			if (!this._graphicsIdentityCaptured || currentRenderPipeline != this._graphicsAsset)
			{
				this._graphicsIdentityCaptured = true;
				this._graphicsAsset = currentRenderPipeline;
				this._graphicsAssetName = ((currentRenderPipeline != null) ? currentRenderPipeline.name : "");
				this._graphicsPipelineType = ((currentRenderPipeline != null) ? currentRenderPipeline.GetType().FullName : "BuiltIn");
			}
			UniversalRenderPipelineAsset universalRenderPipelineAsset = currentRenderPipeline as UniversalRenderPipelineAsset;
			return new MapVehicleStreamingBenchmark.GraphicsSnapshot
			{
				pipelineAssetName = this._graphicsAssetName,
				pipelineType = this._graphicsPipelineType,
				mapCatalogResource = ((ChunkManager.Instance != null) ? ChunkManager.Instance.ActiveMapCatalogResource : ""),
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

		// Token: 0x0600494D RID: 18765 RVA: 0x001C144C File Offset: 0x001BF64C
		public static float PlanarSpeed(Vector3 velocity)
		{
			return new Vector2(velocity.x, velocity.z).magnitude;
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x001BF9FC File Offset: 0x001BDBFC
		private static string Number(double value)
		{
			return value.ToString("R", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x001BFA0F File Offset: 0x001BDC0F
		private static string Bit(bool value)
		{
			if (!value)
			{
				return "0";
			}
			return "1";
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x001C1474 File Offset: 0x001BF674
		private static string Argument(string[] args, string key)
		{
			int num = Array.IndexOf<string>(args, key);
			if (num < 0)
			{
				return null;
			}
			if (num + 1 >= args.Length || args[num + 1].StartsWith("-", StringComparison.Ordinal))
			{
				throw new ArgumentException("Missing value for " + key);
			}
			return args[num + 1];
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x001BF9BB File Offset: 0x001BDBBB
		private static bool Finite(double value)
		{
			return !double.IsNaN(value) && !double.IsInfinity(value);
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x001C14BF File Offset: 0x001BF6BF
		private static bool Positive(double value)
		{
			return MapVehicleStreamingBenchmark.Finite(value) && value > 0.0;
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x001C14D7 File Offset: 0x001BF6D7
		private static bool Finite(Vector3 value)
		{
			return MapVehicleStreamingBenchmark.Finite((double)value.x) && MapVehicleStreamingBenchmark.Finite((double)value.y) && MapVehicleStreamingBenchmark.Finite((double)value.z);
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x001C1504 File Offset: 0x001BF704
		private static float PlanarDistance(Vector3 a, Vector3 b)
		{
			return new Vector2(a.x - b.x, a.z - b.z).magnitude;
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x001C1538 File Offset: 0x001BF738
		private static float SegmentDistance(Vector3 a, Vector3 b, Vector3 point)
		{
			Vector2 vector = new Vector2(a.x, a.z);
			Vector2 vector2 = new Vector2(b.x, b.z);
			Vector2 vector3 = new Vector2(point.x, point.z);
			Vector2 vector4 = vector2 - vector;
			float num = ((vector4.sqrMagnitude > 0f) ? Mathf.Clamp01(Vector2.Dot(vector3 - vector, vector4) / vector4.sqrMagnitude) : 0f);
			return Vector2.Distance(vector3, vector + vector4 * num);
		}

		// Token: 0x04004970 RID: 18800
		public const string Flag = "-mapVehicleBenchmark";

		// Token: 0x04004971 RID: 18801
		private MapVehicleStreamingBenchmark.Configuration _configuration;

		// Token: 0x04004972 RID: 18802
		private MapVehicleStreamingBenchmark.RouteBaseline _route;

		// Token: 0x04004973 RID: 18803
		private MapVehicleStreamingBenchmark.Evidence _evidence;

		// Token: 0x04004974 RID: 18804
		private MapVehicleStreamingBenchmark.Protocol _report;

		// Token: 0x04004975 RID: 18805
		private RunSoakRecorder _recorder;

		// Token: 0x04004976 RID: 18806
		private vVehicleController _vehicle;

		// Token: 0x04004977 RID: 18807
		private Rigidbody _vehicleBody;

		// Token: 0x04004978 RID: 18808
		private MapVehicleStreamingBenchmark.Row[] _rows;

		// Token: 0x04004979 RID: 18809
		private int _rowCount;

		// Token: 0x0400497A RID: 18810
		private int _createdFrame;

		// Token: 0x0400497B RID: 18811
		private int _pendingFrame = -1;

		// Token: 0x0400497C RID: 18812
		private int _consumedFrame = -1;

		// Token: 0x0400497D RID: 18813
		private int _warmupVehicleId;

		// Token: 0x0400497E RID: 18814
		private MapVehicleStreamingBenchmark.Observation _pending;

		// Token: 0x0400497F RID: 18815
		private double _armedAt;

		// Token: 0x04004980 RID: 18816
		private double _mountedAt = -1.0;

		// Token: 0x04004981 RID: 18817
		private double _nextRow;

		// Token: 0x04004982 RID: 18818
		private bool _finished;

		// Token: 0x04004983 RID: 18819
		private bool _ownsDirectory;

		// Token: 0x04004984 RID: 18820
		private RenderPipelineAsset _graphicsAsset;

		// Token: 0x04004985 RID: 18821
		private string _graphicsAssetName;

		// Token: 0x04004986 RID: 18822
		private string _graphicsPipelineType;

		// Token: 0x04004987 RID: 18823
		private bool _graphicsIdentityCaptured;

		// Token: 0x04004988 RID: 18824
		private const double MountTimeoutSeconds = 1200.0;

		// Token: 0x02000A0C RID: 2572
		[Serializable]
		public struct GraphicsSnapshot : IEquatable<MapVehicleStreamingBenchmark.GraphicsSnapshot>
		{
			// Token: 0x06004957 RID: 18775 RVA: 0x001C15EC File Offset: 0x001BF7EC
			public bool Equals(MapVehicleStreamingBenchmark.GraphicsSnapshot other)
			{
				return MapVehicleStreamingBenchmark.GraphicsSnapshot.SameText(this.pipelineType, other.pipelineType) && MapVehicleStreamingBenchmark.GraphicsSnapshot.SameText(this.pipelineAssetName, other.pipelineAssetName) && MapVehicleStreamingBenchmark.GraphicsSnapshot.SameText(this.mapCatalogResource, other.mapCatalogResource) && this.qualityLevel == other.qualityLevel && this.textureMipmapLimit == other.textureMipmapLimit && this.width == other.width && this.height == other.height && this.fullscreenMode == other.fullscreenMode && this.vSyncCount == other.vSyncCount && this.targetFrameRate == other.targetFrameRate && this.qualityAntiAliasing == other.qualityAntiAliasing && this.anisotropicFiltering == other.anisotropicFiltering && this.maximumLODLevel == other.maximumLODLevel && this.lodBias.Equals(other.lodBias) && this.streamingMipmapsActive == other.streamingMipmapsActive && this.universalPipeline == other.universalPipeline && this.renderScale.Equals(other.renderScale) && this.shadowDistance.Equals(other.shadowDistance) && this.msaaSamples == other.msaaSamples && this.shadowCascades == other.shadowCascades && this.mainLightShadowResolution == other.mainLightShadowResolution && this.additionalLightShadowResolution == other.additionalLightShadowResolution && this.mainLightShadows == other.mainLightShadows && this.additionalLightShadows == other.additionalLightShadows && this.softShadows == other.softShadows;
			}

			// Token: 0x06004958 RID: 18776 RVA: 0x001C17A7 File Offset: 0x001BF9A7
			private static bool SameText(string left, string right)
			{
				return string.Equals(left ?? string.Empty, right ?? string.Empty, StringComparison.Ordinal);
			}

			// Token: 0x06004959 RID: 18777 RVA: 0x001C17C4 File Offset: 0x001BF9C4
			public string Fingerprint()
			{
				string text;
				using (SHA256 sha = SHA256.Create())
				{
					byte[] array = sha.ComputeHash(Encoding.UTF8.GetBytes(JsonUtility.ToJson(this)));
					StringBuilder stringBuilder = new StringBuilder(64);
					for (int i = 0; i < array.Length; i++)
					{
						stringBuilder.Append(array[i].ToString("x2", CultureInfo.InvariantCulture));
					}
					text = stringBuilder.ToString();
				}
				return text;
			}

			// Token: 0x04004989 RID: 18825
			public string pipelineType;

			// Token: 0x0400498A RID: 18826
			public string pipelineAssetName;

			// Token: 0x0400498B RID: 18827
			public string mapCatalogResource;

			// Token: 0x0400498C RID: 18828
			public int qualityLevel;

			// Token: 0x0400498D RID: 18829
			public int textureMipmapLimit;

			// Token: 0x0400498E RID: 18830
			public int width;

			// Token: 0x0400498F RID: 18831
			public int height;

			// Token: 0x04004990 RID: 18832
			public int fullscreenMode;

			// Token: 0x04004991 RID: 18833
			public int vSyncCount;

			// Token: 0x04004992 RID: 18834
			public int targetFrameRate;

			// Token: 0x04004993 RID: 18835
			public int qualityAntiAliasing;

			// Token: 0x04004994 RID: 18836
			public int anisotropicFiltering;

			// Token: 0x04004995 RID: 18837
			public int maximumLODLevel;

			// Token: 0x04004996 RID: 18838
			public float lodBias;

			// Token: 0x04004997 RID: 18839
			public bool streamingMipmapsActive;

			// Token: 0x04004998 RID: 18840
			public bool universalPipeline;

			// Token: 0x04004999 RID: 18841
			public float renderScale;

			// Token: 0x0400499A RID: 18842
			public float shadowDistance;

			// Token: 0x0400499B RID: 18843
			public int msaaSamples;

			// Token: 0x0400499C RID: 18844
			public int shadowCascades;

			// Token: 0x0400499D RID: 18845
			public int mainLightShadowResolution;

			// Token: 0x0400499E RID: 18846
			public int additionalLightShadowResolution;

			// Token: 0x0400499F RID: 18847
			public bool mainLightShadows;

			// Token: 0x040049A0 RID: 18848
			public bool additionalLightShadows;

			// Token: 0x040049A1 RID: 18849
			public bool softShadows;
		}

		// Token: 0x02000A0D RID: 2573
		[Serializable]
		public sealed class RouteBaseline
		{
			// Token: 0x0600495A RID: 18778 RVA: 0x001C1854 File Offset: 0x001BFA54
			public void Validate()
			{
				if (string.IsNullOrWhiteSpace(this.id) || (this.vehicle != "bike" && this.vehicle != "scooter") || !MapVehicleStreamingBenchmark.Finite(this.startLogicalPosition) || !MapVehicleStreamingBenchmark.Finite(this.poiLogicalPosition) || !MapVehicleStreamingBenchmark.Positive((double)this.startReturnRadiusMetres) || !MapVehicleStreamingBenchmark.Positive((double)this.poiRadiusMetres) || !MapVehicleStreamingBenchmark.Positive((double)this.outboundMetres) || this.outboundMetres <= this.startReturnRadiusMetres * 2f || this.requiredRoundTrips < 1 || this.requiredRoundTrips > 100 || !MapVehicleStreamingBenchmark.Positive((double)this.minimumReferenceSpeedMetresPerSecond) || !MapVehicleStreamingBenchmark.Positive(this.requiredSustainedFastSeconds) || MapVehicleStreamingBenchmark.PlanarDistance(this.startLogicalPosition, this.poiLogicalPosition) <= this.startReturnRadiusMetres + this.poiRadiusMetres)
				{
					throw new ArgumentException("Route baseline needs an ID, finite seed-relative positions, distinct start/POI, positive radii, outbound distance, round trips and a measured speed reference.");
				}
				if (!string.IsNullOrWhiteSpace(this.graphicsFingerprint))
				{
					if (this.graphicsFingerprint.Length != 64)
					{
						throw new ArgumentException("Graphics fingerprint must be a 64-character SHA256 hex value.");
					}
					for (int i = 0; i < this.graphicsFingerprint.Length; i++)
					{
						if (!Uri.IsHexDigit(this.graphicsFingerprint[i]))
						{
							throw new ArgumentException("Graphics fingerprint must be a 64-character SHA256 hex value.");
						}
					}
				}
			}

			// Token: 0x040049A2 RID: 18850
			public string id;

			// Token: 0x040049A3 RID: 18851
			public string vehicle;

			// Token: 0x040049A4 RID: 18852
			public string graphicsFingerprint;

			// Token: 0x040049A5 RID: 18853
			public int sessionSeed;

			// Token: 0x040049A6 RID: 18854
			public Vector3 startLogicalPosition;

			// Token: 0x040049A7 RID: 18855
			public Vector3 poiLogicalPosition;

			// Token: 0x040049A8 RID: 18856
			public float startReturnRadiusMetres;

			// Token: 0x040049A9 RID: 18857
			public float poiRadiusMetres;

			// Token: 0x040049AA RID: 18858
			public float outboundMetres;

			// Token: 0x040049AB RID: 18859
			public int requiredRoundTrips;

			// Token: 0x040049AC RID: 18860
			public float minimumReferenceSpeedMetresPerSecond;

			// Token: 0x040049AD RID: 18861
			public double requiredSustainedFastSeconds;
		}

		// Token: 0x02000A0E RID: 2574
		public sealed class Configuration
		{
			// Token: 0x0600495C RID: 18780 RVA: 0x001C19AC File Offset: 0x001BFBAC
			public static MapVehicleStreamingBenchmark.Configuration Parse(string[] args)
			{
				if (args == null || Array.IndexOf<string>(args, "-mapVehicleBenchmark") < 0)
				{
					return null;
				}
				string text = MapVehicleStreamingBenchmark.Argument(args, "-mapVehicleBenchmark");
				if (text != "bike" && text != "scooter")
				{
					throw new ArgumentException("-mapVehicleBenchmark requires bike or scooter (the KickScooter/monopattino).");
				}
				if (Array.IndexOf<string>(args, "-runSoakBenchmark") >= 0 || Array.IndexOf<string>(args, "-runSoakCapture") >= 0)
				{
					throw new ArgumentException("Vehicle capture must run alone, without another soak driver/recorder.");
				}
				MapVehicleStreamingBenchmark.Configuration configuration = new MapVehicleStreamingBenchmark.Configuration
				{
					vehicleType = ((text == "bike") ? vVehicleController.VehicleType.Bicycle : vVehicleController.VehicleType.KickScooter),
					outputDirectory = MapVehicleStreamingBenchmark.Argument(args, "-mapVehicleBenchmarkOutput"),
					routePath = MapVehicleStreamingBenchmark.Argument(args, "-mapVehicleBenchmarkRoute")
				};
				string text2 = MapVehicleStreamingBenchmark.Argument(args, "-mapVehicleBenchmarkSeconds");
				if (text2 != null && !double.TryParse(text2, NumberStyles.Float, CultureInfo.InvariantCulture, out configuration.seconds))
				{
					throw new ArgumentException("Invalid vehicle benchmark duration.");
				}
				if (!MapVehicleStreamingBenchmark.Positive(configuration.seconds) || configuration.seconds < 10.0 || configuration.seconds > 3600.0)
				{
					throw new ArgumentException("Vehicle benchmark duration must be between 10 and 3600 seconds.");
				}
				return configuration;
			}

			// Token: 0x040049AE RID: 18862
			public vVehicleController.VehicleType vehicleType;

			// Token: 0x040049AF RID: 18863
			public string outputDirectory;

			// Token: 0x040049B0 RID: 18864
			public string routePath;

			// Token: 0x040049B1 RID: 18865
			public double seconds = 300.0;
		}

		// Token: 0x02000A0F RID: 2575
		public struct Observation
		{
			// Token: 0x040049B2 RID: 18866
			public int sourceFrame;

			// Token: 0x040049B3 RID: 18867
			public int vehicleId;

			// Token: 0x040049B4 RID: 18868
			public int riderId;

			// Token: 0x040049B5 RID: 18869
			public vVehicleController.VehicleType vehicleType;

			// Token: 0x040049B6 RID: 18870
			public bool mounted;

			// Token: 0x040049B7 RID: 18871
			public bool dynamicBody;

			// Token: 0x040049B8 RID: 18872
			public bool controlsFresh;

			// Token: 0x040049B9 RID: 18873
			public bool controlsAvailable;

			// Token: 0x040049BA RID: 18874
			public bool sprint;

			// Token: 0x040049BB RID: 18875
			public bool brake;

			// Token: 0x040049BC RID: 18876
			public bool grounded;

			// Token: 0x040049BD RID: 18877
			public float throttle;

			// Token: 0x040049BE RID: 18878
			public float steering;

			// Token: 0x040049BF RID: 18879
			public float speed;

			// Token: 0x040049C0 RID: 18880
			public Vector3 logicalPosition;
		}

		// Token: 0x02000A10 RID: 2576
		[Serializable]
		public sealed class Protocol
		{
			// Token: 0x040049C1 RID: 18881
			public string version = "MapVehicleStreamingBenchmark/v2";

			// Token: 0x040049C2 RID: 18882
			public string phase = "waiting_for_mount";

			// Token: 0x040049C3 RID: 18883
			public string outcome;

			// Token: 0x040049C4 RID: 18884
			public string reason;

			// Token: 0x040049C5 RID: 18885
			public string outputDirectory;

			// Token: 0x040049C6 RID: 18886
			public string vehicleName;

			// Token: 0x040049C7 RID: 18887
			public string vehicleType;

			// Token: 0x040049C8 RID: 18888
			public string routeBaselineId;

			// Token: 0x040049C9 RID: 18889
			public int vehicleInstanceId;

			// Token: 0x040049CA RID: 18890
			public int riderInstanceId;

			// Token: 0x040049CB RID: 18891
			public int sessionSeed;

			// Token: 0x040049CC RID: 18892
			public int measuredFrames;

			// Token: 0x040049CD RID: 18893
			public int roundTrips;

			// Token: 0x040049CE RID: 18894
			public int sampledRows;

			// Token: 0x040049CF RID: 18895
			public bool sessionSeedValid;

			// Token: 0x040049D0 RID: 18896
			public bool completed;

			// Token: 0x040049D1 RID: 18897
			public bool captureValid;

			// Token: 0x040049D2 RID: 18898
			public bool validForComparison;

			// Token: 0x040049D3 RID: 18899
			public bool routeBaselineConfigured;

			// Token: 0x040049D4 RID: 18900
			public bool routePassed;

			// Token: 0x040049D5 RID: 18901
			public bool referenceSpeedExposurePassed;

			// Token: 0x040049D6 RID: 18902
			public bool maximumSpeedProven;

			// Token: 0x040049D7 RID: 18903
			public bool graphicsSnapshotAvailable;

			// Token: 0x040049D8 RID: 18904
			public bool graphicsChanged;

			// Token: 0x040049D9 RID: 18905
			public bool graphicsBaselineConfigured;

			// Token: 0x040049DA RID: 18906
			public bool graphicsBaselineMatched;

			// Token: 0x040049DB RID: 18907
			public MapVehicleStreamingBenchmark.GraphicsSnapshot graphicsStart;

			// Token: 0x040049DC RID: 18908
			public MapVehicleStreamingBenchmark.GraphicsSnapshot graphicsEnd;

			// Token: 0x040049DD RID: 18909
			public string graphicsStartFingerprint;

			// Token: 0x040049DE RID: 18910
			public string graphicsEndFingerprint;

			// Token: 0x040049DF RID: 18911
			public double requestedSeconds;

			// Token: 0x040049E0 RID: 18912
			public double warmupSeconds = 10.0;

			// Token: 0x040049E1 RID: 18913
			public double elapsedSeconds;

			// Token: 0x040049E2 RID: 18914
			public double movingSeconds;

			// Token: 0x040049E3 RID: 18915
			public double fullThrottleSprintSeconds;

			// Token: 0x040049E4 RID: 18916
			public double controlsUnavailableSeconds;

			// Token: 0x040049E5 RID: 18917
			public double groundedSeconds;

			// Token: 0x040049E6 RID: 18918
			public double distanceMetres;

			// Token: 0x040049E7 RID: 18919
			public double maximumExcursionMetres;

			// Token: 0x040049E8 RID: 18920
			public double longestSustainedReferenceSpeedSeconds;

			// Token: 0x040049E9 RID: 18921
			public double peakObservedSpeedMetresPerSecond;

			// Token: 0x040049EA RID: 18922
			public Vector3 startLogicalPosition;

			// Token: 0x040049EB RID: 18923
			public Vector3 endLogicalPosition;

			// Token: 0x040049EC RID: 18924
			public MapVehicleStreamingBenchmark.RouteBaseline routeBaseline;

			// Token: 0x040049ED RID: 18925
			public RunSoakRecorder.Distribution frameTimes;

			// Token: 0x040049EE RID: 18926
			public string limitation = "Manual driving with unchanged production controls/physics. Warmup is 10 seconds after the first stable mount, outside capture. Speed is planar (XZ); sustained reference speed requires grounded contact and full throttle+sprint. Peak observed speed is not proof of the vehicle's absolute maximum. Without an explicit measured route/speed baseline and matching graphics fingerprint this is observational. validForComparison means only baseline_criteria_met. The optional baseline checks seed, start/POI geometry, repeated outbound/return legs, sustained reference speed and the recorded graphics fields. Graphics scalars are checked each recorder tick and at stop; start/end snapshots and SHA256 fingerprints are saved. Camera overrides, unrecorded pipeline settings and asset contents are outside this fingerprint. It does not prove identical traffic, steering, weather or cache warmth between captures. Frames/render receipts/process RAM are recorded by RunSoakRecorder; vehicle_samples.csv is buffered at 1Hz and written after capture. Private bytes are committed memory; working set is resident memory. Memory growth alone does not prove a leak.";
		}

		// Token: 0x02000A11 RID: 2577
		public sealed class Evidence
		{
			// Token: 0x17000C69 RID: 3177
			// (get) Token: 0x0600495F RID: 18783 RVA: 0x001C1B1F File Offset: 0x001BFD1F
			public MapVehicleStreamingBenchmark.Protocol Report { get; }

			// Token: 0x17000C6A RID: 3178
			// (get) Token: 0x06004960 RID: 18784 RVA: 0x001C1B27 File Offset: 0x001BFD27
			public bool Invalid
			{
				get
				{
					return this._invalidReason != null;
				}
			}

			// Token: 0x17000C6B RID: 3179
			// (get) Token: 0x06004961 RID: 18785 RVA: 0x001C1B32 File Offset: 0x001BFD32
			public string InvalidReason
			{
				get
				{
					return this._invalidReason;
				}
			}

			// Token: 0x06004962 RID: 18786 RVA: 0x001C1B3C File Offset: 0x001BFD3C
			public Evidence(vVehicleController.VehicleType type, MapVehicleStreamingBenchmark.Observation initial, double requestedSeconds, bool seedValid, int seed, MapVehicleStreamingBenchmark.RouteBaseline route = null, MapVehicleStreamingBenchmark.GraphicsSnapshot? graphics = null)
			{
				if (!MapVehicleStreamingBenchmark.Positive(requestedSeconds))
				{
					throw new ArgumentOutOfRangeException("requestedSeconds");
				}
				if (route != null)
				{
					route.Validate();
				}
				this._type = type;
				this._route = route;
				this._previousPosition = (this._anchor = initial.logicalPosition);
				this._previousSpeed = initial.speed;
				this.Report = new MapVehicleStreamingBenchmark.Protocol
				{
					phase = "capturing",
					requestedSeconds = requestedSeconds,
					vehicleType = type.ToString(),
					vehicleInstanceId = initial.vehicleId,
					riderInstanceId = initial.riderId,
					sessionSeedValid = seedValid,
					sessionSeed = seed,
					startLogicalPosition = initial.logicalPosition,
					endLogicalPosition = initial.logicalPosition,
					routeBaselineConfigured = (route != null),
					routeBaselineId = ((route != null) ? route.id : null),
					routeBaseline = route
				};
				this.Report.graphicsBaselineConfigured = !string.IsNullOrWhiteSpace((route != null) ? route.graphicsFingerprint : null);
				if (graphics != null)
				{
					this.Report.graphicsSnapshotAvailable = true;
					this.Report.graphicsStart = (this.Report.graphicsEnd = graphics.Value);
					this.Report.graphicsStartFingerprint = graphics.Value.Fingerprint();
					this.Report.graphicsBaselineMatched = this.Report.graphicsBaselineConfigured && string.Equals(route.graphicsFingerprint, this.Report.graphicsStartFingerprint, StringComparison.OrdinalIgnoreCase);
				}
				this.ValidateObservation(initial);
				if (route != null)
				{
					this._anchor = route.startLogicalPosition;
					string text = ((type == vVehicleController.VehicleType.Bicycle) ? "bike" : "scooter");
					if (route.vehicle != text)
					{
						this.Invalidate("Route baseline was measured with a different vehicle type.");
					}
					if (!seedValid || seed != route.sessionSeed)
					{
						this.Invalidate("Route baseline seed does not match the active run.");
					}
					if (MapVehicleStreamingBenchmark.PlanarDistance(initial.logicalPosition, this._anchor) > route.startReturnRadiusMetres)
					{
						this.Invalidate("Capture did not start within the baseline start radius.");
					}
				}
			}

			// Token: 0x06004963 RID: 18787 RVA: 0x001C1D58 File Offset: 0x001BFF58
			public void Observe(MapVehicleStreamingBenchmark.Observation value, double seconds)
			{
				if (this.Invalid)
				{
					return;
				}
				if (!MapVehicleStreamingBenchmark.Positive(seconds))
				{
					this.Invalidate("Invalid observation duration.");
					return;
				}
				this.ValidateObservation(value);
				if (this.Invalid)
				{
					return;
				}
				double num = (double)MapVehicleStreamingBenchmark.PlanarDistance(value.logicalPosition, this._previousPosition);
				double num2 = Math.Max(5.0, (double)(this._previousSpeed + value.speed) * seconds * 2.0 + 2.0);
				if (num > num2)
				{
					this.Invalidate("Position discontinuity: this capture cannot certify real vehicle travel.");
					return;
				}
				this.Report.measuredFrames++;
				this.Report.elapsedSeconds += seconds;
				this.Report.distanceMetres += num;
				this.Report.endLogicalPosition = value.logicalPosition;
				this.Report.peakObservedSpeedMetresPerSecond = Math.Max(this.Report.peakObservedSpeedMetresPerSecond, (double)value.speed);
				if (value.speed >= 0.5f)
				{
					this.Report.movingSeconds += seconds;
					this._stalledSeconds = 0.0;
				}
				else
				{
					this._stalledSeconds += seconds;
				}
				if (this._stalledSeconds >= 15.0)
				{
					this.Invalidate("Vehicle stalled/stopped for 15 continuous measured seconds.");
				}
				if (value.grounded)
				{
					this.Report.groundedSeconds += seconds;
				}
				bool flag = value.controlsFresh && value.controlsAvailable && value.throttle >= 0.95f && value.sprint && !value.brake;
				if (flag)
				{
					this.Report.fullThrottleSprintSeconds += seconds;
				}
				if (!value.controlsFresh || !value.controlsAvailable)
				{
					this.Report.controlsUnavailableSeconds += seconds;
				}
				double num3 = (double)MapVehicleStreamingBenchmark.PlanarDistance(value.logicalPosition, this._anchor);
				this.Report.maximumExcursionMetres = Math.Max(this.Report.maximumExcursionMetres, num3);
				if (this._route != null)
				{
					if (MapVehicleStreamingBenchmark.SegmentDistance(this._previousPosition, value.logicalPosition, this._route.poiLogicalPosition) <= this._route.poiRadiusMetres)
					{
						this._poiVisited = true;
					}
					if (num3 >= (double)this._route.outboundMetres)
					{
						this._outbound = true;
					}
					if (this._outbound && num3 <= (double)this._route.startReturnRadiusMetres)
					{
						if (this._poiVisited)
						{
							this.Report.roundTrips++;
						}
						this._outbound = (this._poiVisited = false);
					}
					this._fastSeconds = ((flag && value.grounded && value.speed >= this._route.minimumReferenceSpeedMetresPerSecond) ? (this._fastSeconds + seconds) : 0.0);
					this.Report.longestSustainedReferenceSpeedSeconds = Math.Max(this.Report.longestSustainedReferenceSpeedSeconds, this._fastSeconds);
					this.Report.routePassed = this.Report.roundTrips >= this._route.requiredRoundTrips;
					this.Report.referenceSpeedExposurePassed = this.Report.longestSustainedReferenceSpeedSeconds >= this._route.requiredSustainedFastSeconds;
				}
				this._previousPosition = value.logicalPosition;
				this._previousSpeed = value.speed;
			}

			// Token: 0x06004964 RID: 18788 RVA: 0x001C20B8 File Offset: 0x001C02B8
			public void ObserveGraphics(MapVehicleStreamingBenchmark.GraphicsSnapshot value)
			{
				if (!this.Report.graphicsSnapshotAvailable)
				{
					return;
				}
				this.Report.graphicsEnd = value;
				if (this.Report.graphicsStart.Equals(value))
				{
					return;
				}
				this.Report.graphicsChanged = true;
				this.Invalidate("Recorded graphics settings changed during capture.");
			}

			// Token: 0x06004965 RID: 18789 RVA: 0x001C210C File Offset: 0x001C030C
			public void Finish(bool recorderValid, string recorderReason)
			{
				if (this.Report.graphicsSnapshotAvailable)
				{
					this.Report.graphicsEndFingerprint = this.Report.graphicsEnd.Fingerprint();
				}
				this.Report.phase = "finished";
				this.Report.completed = recorderValid && !this.Invalid;
				this.Report.captureValid = this.Report.completed && this.Report.measuredFrames > 0 && this.Report.elapsedSeconds >= this.Report.requestedSeconds * 0.95;
				this.Report.validForComparison = this.Report.captureValid && this.Report.routeBaselineConfigured && this.Report.routePassed && this.Report.referenceSpeedExposurePassed && this.Report.graphicsSnapshotAvailable && this.Report.graphicsBaselineConfigured && this.Report.graphicsBaselineMatched && !this.Report.graphicsChanged;
				this.Report.outcome = ((!this.Report.captureValid) ? "invalid" : (this.Report.validForComparison ? "baseline_criteria_met" : "observational"));
				this.Report.reason = this._invalidReason ?? ((!recorderValid) ? recorderReason : ((!this.Report.captureValid) ? "Insufficient completed vehicle observations for the requested duration." : ((!this.Report.routeBaselineConfigured) ? "No route/speed baseline supplied; actual speed and streaming were measured without certifying maximum-speed workload." : ((!this.Report.graphicsBaselineConfigured) ? "Graphics baseline unknown: no explicit reference fingerprint supplied; capture remains observational." : ((!this.Report.graphicsSnapshotAvailable) ? "Graphics snapshot unavailable; capture remains observational." : ((!this.Report.graphicsBaselineMatched) ? "Recorded graphics do not match the supplied reference fingerprint; capture remains observational." : ((!this.Report.routePassed) ? "Required repeated outbound/POI/return route was not completed." : ((!this.Report.referenceSpeedExposurePassed) ? "Sustained grounded planar reference speed with full throttle+sprint was not reached." : "Configured baseline criteria met; absolute maximum speed and unrecorded conditions remain unproven."))))))));
			}

			// Token: 0x06004966 RID: 18790 RVA: 0x001C231C File Offset: 0x001C051C
			private void ValidateObservation(MapVehicleStreamingBenchmark.Observation value)
			{
				if (!value.mounted || value.vehicleId == 0 || value.riderId == 0 || !value.dynamicBody)
				{
					this.Invalidate("Expected a genuinely mounted rider and a dynamic vehicle Rigidbody.");
					return;
				}
				if (value.vehicleType != this._type)
				{
					this.Invalidate("Wrong vehicle type.");
					return;
				}
				if (value.vehicleId != this.Report.vehicleInstanceId || value.riderId != this.Report.riderInstanceId)
				{
					this.Invalidate("Vehicle or mounted rider identity changed during capture.");
					return;
				}
				if (!MapVehicleStreamingBenchmark.Finite(value.logicalPosition) || !MapVehicleStreamingBenchmark.Finite((double)value.speed) || value.speed < 0f)
				{
					this.Invalidate("Non-finite vehicle telemetry.");
				}
			}

			// Token: 0x06004967 RID: 18791 RVA: 0x001C23D4 File Offset: 0x001C05D4
			private void Invalidate(string reason)
			{
				if (this._invalidReason == null)
				{
					this._invalidReason = reason;
				}
			}

			// Token: 0x040049F0 RID: 18928
			private readonly MapVehicleStreamingBenchmark.RouteBaseline _route;

			// Token: 0x040049F1 RID: 18929
			private readonly vVehicleController.VehicleType _type;

			// Token: 0x040049F2 RID: 18930
			private Vector3 _previousPosition;

			// Token: 0x040049F3 RID: 18931
			private Vector3 _anchor;

			// Token: 0x040049F4 RID: 18932
			private float _previousSpeed;

			// Token: 0x040049F5 RID: 18933
			private double _stalledSeconds;

			// Token: 0x040049F6 RID: 18934
			private double _fastSeconds;

			// Token: 0x040049F7 RID: 18935
			private bool _outbound;

			// Token: 0x040049F8 RID: 18936
			private bool _poiVisited;

			// Token: 0x040049F9 RID: 18937
			private string _invalidReason;
		}

		// Token: 0x02000A12 RID: 2578
		private struct Row
		{
			// Token: 0x040049FA RID: 18938
			public MapVehicleStreamingBenchmark.Observation vehicle;

			// Token: 0x040049FB RID: 18939
			public double elapsed;

			// Token: 0x040049FC RID: 18940
			public int cellX;

			// Token: 0x040049FD RID: 18941
			public int cellZ;

			// Token: 0x040049FE RID: 18942
			public int hasCurrentChunk;

			// Token: 0x040049FF RID: 18943
			public int liveChunks;

			// Token: 0x04004A00 RID: 18944
			public int pooledChunks;

			// Token: 0x04004A01 RID: 18945
			public int pooledZones;

			// Token: 0x04004A02 RID: 18946
			public int retiringZones;

			// Token: 0x04004A03 RID: 18947
			public int retirementGroups;

			// Token: 0x04004A04 RID: 18948
			public int residentPayloads;

			// Token: 0x04004A05 RID: 18949
			public int pendingPayloads;

			// Token: 0x04004A06 RID: 18950
			public int assetLoads;

			// Token: 0x04004A07 RID: 18951
			public int resourceCache;

			// Token: 0x04004A08 RID: 18952
			public int addressableEntries;

			// Token: 0x04004A09 RID: 18953
			public int addressableOwners;
		}
	}
}
