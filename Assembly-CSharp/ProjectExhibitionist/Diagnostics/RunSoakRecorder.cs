using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Features.RenderingPrecision;
using ProjectExhibitionist.Core.SceneManagement;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Diagnostics
{
	// Token: 0x02000A20 RID: 2592
	[DefaultExecutionOrder(-31000)]
	public sealed class RunSoakRecorder : MonoBehaviour
	{
		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x0600499C RID: 18844 RVA: 0x001C4108 File Offset: 0x001C2308
		// (set) Token: 0x0600499D RID: 18845 RVA: 0x001C410F File Offset: 0x001C230F
		public static RunSoakRecorder Instance { get; private set; }

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x0600499E RID: 18846 RVA: 0x001C4117 File Offset: 0x001C2317
		// (set) Token: 0x0600499F RID: 18847 RVA: 0x001C411F File Offset: 0x001C231F
		public bool IsRecording { get; private set; }

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x060049A0 RID: 18848 RVA: 0x001C4128 File Offset: 0x001C2328
		public string OutputDirectory
		{
			get
			{
				if (this._options == null)
				{
					return null;
				}
				return this._options.outputDirectory;
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x060049A1 RID: 18849 RVA: 0x001C413F File Offset: 0x001C233F
		public double Elapsed
		{
			get
			{
				if (!this.IsRecording)
				{
					return this._elapsed;
				}
				return Time.realtimeSinceStartupAsDouble - this._start;
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x060049A2 RID: 18850 RVA: 0x001C415C File Offset: 0x001C235C
		public double DistanceMetres
		{
			get
			{
				return this._distance;
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x060049A3 RID: 18851 RVA: 0x001C4164 File Offset: 0x001C2364
		public int FrameCount
		{
			get
			{
				return this._frameCount;
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x060049A4 RID: 18852 RVA: 0x001C416C File Offset: 0x001C236C
		// (set) Token: 0x060049A5 RID: 18853 RVA: 0x001C4174 File Offset: 0x001C2374
		public int Phase { get; set; } = 1;

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060049A6 RID: 18854 RVA: 0x001C417D File Offset: 0x001C237D
		// (set) Token: 0x060049A7 RID: 18855 RVA: 0x001C4185 File Offset: 0x001C2385
		public RunSoakRecorder.Report LastReport { get; private set; }

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x060049A8 RID: 18856 RVA: 0x001C4190 File Offset: 0x001C2390
		// (remove) Token: 0x060049A9 RID: 18857 RVA: 0x001C41C8 File Offset: 0x001C23C8
		public event Action<RunSoakRecorder> RuntimeTick;

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x060049AA RID: 18858 RVA: 0x001C4200 File Offset: 0x001C2400
		// (remove) Token: 0x060049AB RID: 18859 RVA: 0x001C4238 File Offset: 0x001C2438
		public event Action<RunSoakRecorder> Stopped;

		// Token: 0x060049AC RID: 18860 RVA: 0x001C426D File Offset: 0x001C246D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			RunSoakRecorder.Instance = null;
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x001C4278 File Offset: 0x001C2478
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InstallFromCommandLine()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (Array.IndexOf<string>(commandLineArgs, "-runSoakBenchmark") >= 0)
			{
				return;
			}
			if (Array.IndexOf<string>(commandLineArgs, "-runSoakCapture") < 0 || RunSoakRecorder.Instance != null)
			{
				return;
			}
			RunSoakRecorder.Options options = new RunSoakRecorder.Options
			{
				outputDirectory = (RunSoakRecorder.Argument(commandLineArgs, "-runSoakOutput") ?? Path.Combine(Application.persistentDataPath, "PerformanceCaptures", "Soak_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff")))
			};
			string text = RunSoakRecorder.Argument(commandLineArgs, "-runSoakSeconds");
			double num;
			if (text != null && double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
			{
				options.durationSeconds = num;
			}
			string text2 = RunSoakRecorder.Argument(commandLineArgs, "-runSoakMaxFrames");
			int num2;
			if (text2 != null && int.TryParse(text2, out num2))
			{
				options.maximumFrames = num2;
			}
			try
			{
				RunSoakRecorder.Begin(options);
			}
			catch (Exception ex)
			{
				Debug.LogError("[RunSoak] Could not start: " + ex.Message);
			}
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x001C4380 File Offset: 0x001C2580
		private static string Argument(string[] args, string key)
		{
			int num = Array.IndexOf<string>(args, key);
			if (num < 0 || num + 1 >= args.Length)
			{
				return null;
			}
			return args[num + 1];
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x001C43A8 File Offset: 0x001C25A8
		public static RunSoakRecorder Begin(RunSoakRecorder.Options options)
		{
			if (!Application.isPlaying)
			{
				throw new InvalidOperationException("Soak recording requires Play Mode/a running player.");
			}
			if (RunSoakRecorder.Instance != null && RunSoakRecorder.Instance.IsRecording)
			{
				throw new InvalidOperationException("A soak capture is already running.");
			}
			if (RunSoakRecorder.Instance != null)
			{
				Object.Destroy(RunSoakRecorder.Instance.gameObject);
			}
			if (options == null || string.IsNullOrWhiteSpace(options.outputDirectory) || double.IsNaN(options.durationSeconds) || double.IsInfinity(options.durationSeconds) || options.durationSeconds <= 0.0 || options.durationSeconds > 86400.0 || options.maximumFrames < 2 || options.maximumFrames > 3000000 || options.npcStressThreshold < 0 || double.IsNaN(options.memoryIntervalSeconds) || double.IsInfinity(options.memoryIntervalSeconds) || options.memoryIntervalSeconds < 0.5 || options.memoryIntervalSeconds > 60.0)
			{
				throw new ArgumentException("Provide output, duration (0,86400], frame cap [2,3000000], memory interval [0.5,60].");
			}
			string fullPath = Path.GetFullPath(options.outputDirectory);
			Directory.CreateDirectory(fullPath);
			if (File.Exists(Path.Combine(fullPath, "frames.csv")) || File.Exists(Path.Combine(fullPath, "summary.json")))
			{
				throw new IOException("Capture directory already contains a capture: " + fullPath);
			}
			GameObject gameObject = new GameObject("[RunSoakRecorder]");
			Object.DontDestroyOnLoad(gameObject);
			RunSoakRecorder runSoakRecorder = gameObject.AddComponent<RunSoakRecorder>();
			RunSoakRecorder runSoakRecorder2;
			try
			{
				runSoakRecorder.Initialize(new RunSoakRecorder.Options
				{
					outputDirectory = fullPath,
					durationSeconds = options.durationSeconds,
					maximumFrames = options.maximumFrames,
					memoryIntervalSeconds = options.memoryIntervalSeconds,
					npcStressThreshold = options.npcStressThreshold,
					protocol = options.protocol,
					requireMainCameraRendering = options.requireMainCameraRendering
				});
				RunSoakRecorder.Instance = runSoakRecorder;
				runSoakRecorder2 = runSoakRecorder;
			}
			catch
			{
				Object.Destroy(gameObject);
				throw;
			}
			return runSoakRecorder2;
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x001C45A0 File Offset: 0x001C27A0
		private void Initialize(RunSoakRecorder.Options options)
		{
			this._options = options;
			this._frames = new RunSoakRecorder.Frame[options.maximumFrames];
			this._memory = new RunSoakRecorder.Memory[(int)Math.Ceiling(options.durationSeconds / options.memoryIntervalSeconds) + 4];
			this._heavyPayloadSetups = new RunSoakRecorder.HeavyPayloadSetup[2048];
			this._gc = RunSoakRecorder.StartCounter(ProfilerCategory.Memory, "GC Allocated In Frame");
			this._cpu = RunSoakRecorder.StartCounter(ProfilerCategory.Render, "CPU Main Thread Frame Time");
			this._gpu = RunSoakRecorder.StartCounter(ProfilerCategory.Render, "GPU Frame Time");
			try
			{
				this._process = Process.GetCurrentProcess();
			}
			catch
			{
				this._process = null;
			}
			this._spawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
			this._sceneHandle = SceneManager.GetActiveScene().handle;
			this._width = Screen.width;
			this._height = Screen.height;
			this._quality = QualitySettings.GetQualityLevel();
			this._vSync = QualitySettings.vSyncCount;
			this._targetFps = Application.targetFrameRate;
			if (options.requireMainCameraRendering)
			{
				Camera main = Camera.main;
				if (main != null && main.cameraType == CameraType.Game)
				{
					this._renderGuard = new RunSoakRenderGuard(main);
				}
			}
			this.LastReport = new RunSoakRecorder.Report
			{
				mainCameraRenderingRequired = options.requireMainCameraRendering,
				outputDirectory = options.outputDirectory,
				protocol = options.protocol,
				utcStarted = DateTime.UtcNow.ToString("O"),
				unity = Application.unityVersion,
				scene = SceneManager.GetActiveScene().name,
				cpu = SystemInfo.processorType,
				gpu = SystemInfo.graphicsDeviceName,
				graphicsApi = SystemInfo.graphicsDeviceType.ToString(),
				quality = QualitySettings.names[this._quality],
				qualityIndex = this._quality,
				width = this._width,
				height = this._height,
				vSync = this._vSync,
				targetFrameRate = this._targetFps,
				editor = Application.isEditor,
				developmentBuild = Debug.isDebugBuild,
				ramMiB = SystemInfo.systemMemorySize,
				graphicsMemoryMiB = SystemInfo.graphicsMemorySize,
				renderPipeline = ((GraphicsSettings.currentRenderPipeline != null) ? GraphicsSettings.currentRenderPipeline.name : "Built-in"),
				maximumFrames = options.maximumFrames,
				npcStressThreshold = options.npcStressThreshold,
				recorderBufferBytes = (long)Marshal.SizeOf<RunSoakRecorder.Frame>() * (long)this._frames.Length + (long)Marshal.SizeOf<RunSoakRecorder.Memory>() * (long)this._memory.Length + (long)Marshal.SizeOf<RunSoakRecorder.HeavyPayloadSetup>() * (long)this._heavyPayloadSetups.Length
			};
			if (ChunkManager.Instance != null)
			{
				this.LastReport.sessionSeedValid = ChunkManager.Instance.TryGetSessionSeed(out this.LastReport.sessionSeed);
			}
			this._start = Time.realtimeSinceStartupAsDouble;
			this._frameTimeline = new RunSoakFrameTimeline(Time.frameCount);
			this.IsRecording = true;
			this._heavyPayloadSetupInitialCompleted = HeavyZoneStreamingProxy.CompletedColdPayloadSetupCount;
			HeavyZoneStreamingProxy.ColdPayloadSetupMeasured += this.OnColdPayloadSetup;
			Application.logMessageReceived += this.OnLog;
			this.CapturePosition();
			this._startLogical = this._lastLogical;
			this.CaptureMemory();
			this._nextMemory = options.memoryIntervalSeconds;
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x001C4900 File Offset: 0x001C2B00
		private void OnColdPayloadSetup(HeavyZoneStreamingProxy.ColdPayloadSetupSample sample)
		{
			if (!this.IsRecording || this._finishing)
			{
				return;
			}
			if (this._heavyPayloadSetupCount >= this._heavyPayloadSetups.Length)
			{
				this._heavyPayloadSetupDropped++;
				return;
			}
			RunSoakRecorder.HeavyPayloadSetup[] heavyPayloadSetups = this._heavyPayloadSetups;
			int heavyPayloadSetupCount = this._heavyPayloadSetupCount;
			this._heavyPayloadSetupCount = heavyPayloadSetupCount + 1;
			heavyPayloadSetups[heavyPayloadSetupCount] = new RunSoakRecorder.HeavyPayloadSetup
			{
				sample = sample,
				phase = this.Phase
			};
		}

		// Token: 0x060049B2 RID: 18866 RVA: 0x001C4978 File Offset: 0x001C2B78
		private static ProfilerRecorder StartCounter(ProfilerCategory category, string name)
		{
			ProfilerRecorder profilerRecorder;
			try
			{
				profilerRecorder = ProfilerRecorder.StartNew(category, name, 1, ProfilerRecorderOptions.Default);
			}
			catch
			{
				profilerRecorder = default(ProfilerRecorder);
			}
			return profilerRecorder;
		}

		// Token: 0x060049B3 RID: 18867 RVA: 0x001C49B0 File Offset: 0x001C2BB0
		private static long ReadCounter(ProfilerRecorder recorder)
		{
			if (!recorder.Valid || recorder.Count <= 0)
			{
				return -1L;
			}
			return recorder.LastValue;
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x001C49D0 File Offset: 0x001C2BD0
		private void Update()
		{
			if (!this.IsRecording)
			{
				return;
			}
			string text = this.CaptureCompletedFrame();
			if (text != null)
			{
				this.Stop("invalid", text);
				return;
			}
			try
			{
				Action<RunSoakRecorder> runtimeTick = this.RuntimeTick;
				if (runtimeTick != null)
				{
					runtimeTick(this);
				}
			}
			catch (Exception ex)
			{
				string text2 = "invalid";
				string text3 = "Route controller exception: ";
				Exception ex2 = ex;
				this.Stop(text2, text3 + ((ex2 != null) ? ex2.ToString() : null));
			}
			if (this.IsRecording && this.Elapsed >= this._options.durationSeconds)
			{
				this.Stop("completed", "Configured duration reached.");
			}
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x001C4A74 File Offset: 0x001C2C74
		private void LateUpdate()
		{
			if (!this.IsRecording)
			{
				return;
			}
			this.CapturePosition();
			ChunkManager instance = ChunkManager.Instance;
			bool flag = instance != null && instance.DeferredStreamingBudgetFrame == Time.frameCount;
			this._frameTimeline.CaptureLateState(Time.frameCount, this.Phase, (NPCZoneManager.Instance != null) ? NPCZoneManager.Instance.ActiveNPCCount : (-1), (float)this._distance, flag ? (instance.DeferredStreamingBudgetActiveLastFrame ? 1 : 0) : (-1), flag ? instance.DeferredStreamingBudgetMsLastFrame : (-1.0), flag ? instance.DeferredStreamingElapsedMsLastFrame : (-1.0), flag ? instance.DeferredStreamingBudgetDeferralsLastFrame : (-1));
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x001C4B30 File Offset: 0x001C2D30
		private string CaptureCompletedFrame()
		{
			RunSoakFrameTimeline.State state;
			if (!this._frameTimeline.TryTakeCompleted(Time.frameCount, out state))
			{
				this._missedFrames = this._frameTimeline.MissingFrames;
				if (!this._frameTimeline.Invalid)
				{
					return null;
				}
				return "Source-frame state was missing or stale; no phase/duration relabeling is permitted.";
			}
			else
			{
				if (this._frameCount == this._frames.Length)
				{
					return "Frame capacity exhausted; no silent sample dropping.";
				}
				if (this._options.requireMainCameraRendering)
				{
					if (this._renderGuard != null)
					{
						this._renderGuard.CheckPreviousFrame(Time.frameCount);
					}
					else
					{
						this._framesWithoutRenderGuard++;
					}
				}
				double elapsed = this.Elapsed;
				long num = RunSoakRecorder.ReadCounter(this._cpu);
				long num2 = RunSoakRecorder.ReadCounter(this._gpu);
				float num3 = Time.unscaledDeltaTime * 1000f;
				if (this._frameCount == 0)
				{
					this.LastReport.firstMeasuredSourceFrame = state.sourceFrame;
					this.LastReport.initialUnmeasuredSeconds = Math.Max(0.0, elapsed - (double)num3 / 1000.0);
				}
				this.LastReport.lastMeasuredSourceFrame = state.sourceFrame;
				RunSoakRecorder.Frame[] frames = this._frames;
				int frameCount = this._frameCount;
				this._frameCount = frameCount + 1;
				frames[frameCount] = new RunSoakRecorder.Frame
				{
					number = state.sourceFrame,
					phase = state.phase,
					elapsed = elapsed,
					activeNpcs = state.activeNpcs,
					milliseconds = num3,
					distance = state.distance,
					deferredStreamingBudgetActive = state.deferredStreamingBudgetActive,
					deferredStreamingBudgetMs = state.deferredStreamingBudgetMs,
					deferredStreamingElapsedMs = state.deferredStreamingElapsedMs,
					deferredStreamingBudgetDeferrals = state.deferredStreamingBudgetDeferrals,
					gcBytes = RunSoakRecorder.ReadCounter(this._gc),
					cpuMs = ((num <= 0L) ? (-1f) : ((float)num / 1000000f)),
					gpuMs = ((num2 <= 0L) ? (-1f) : ((float)num2 / 1000000f))
				};
				if (elapsed >= this._nextMemory)
				{
					this.CaptureMemory();
					this._nextMemory = elapsed + this._options.memoryIntervalSeconds;
				}
				return RunSoakRenderGuard.CaptureFailure(this._options.requireMainCameraRendering, this._renderGuard != null && this._renderGuard.CameraIdentityChanged, this._framesWithoutRenderGuard, (this._renderGuard != null) ? this._renderGuard.Frames.ConsecutiveMissingFrames : 0);
			}
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x001C4D94 File Offset: 0x001C2F94
		private void CapturePosition()
		{
			Transform player = PlayerRegistry.Player;
			if (player == null)
			{
				this._hasPreviousPlayer = false;
				return;
			}
			Vector3 vector = WorldOrigin.ToLogical(player.position);
			if (this._hasPreviousPlayer)
			{
				this._distance += RunSoakRecorder.PlanarDistance(this._previousLogical, vector);
			}
			else if (this._frameCount == 0)
			{
				this._startLogical = vector;
			}
			this._previousLogical = (this._lastLogical = vector);
			this._hasPreviousPlayer = true;
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x001C4E10 File Offset: 0x001C3010
		public static double PlanarDistance(Vector3 a, Vector3 b)
		{
			double num = (double)b.x - (double)a.x;
			double num2 = (double)b.z - (double)a.z;
			return Math.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x060049B9 RID: 18873 RVA: 0x001C4E48 File Offset: 0x001C3048
		private void CaptureMemory()
		{
			if (this._memoryCount >= this._memory.Length)
			{
				return;
			}
			long timestamp = Stopwatch.GetTimestamp();
			long num2;
			long num3;
			int num = RunSoakProcessMemory.Read(this._process, out num2, out num3);
			if (num2 > 0L && num3 > 0L)
			{
				this.LastReport.processMemoryValidSamples++;
			}
			else
			{
				this.LastReport.processMemoryUnavailableSamples++;
			}
			if (num == 1)
			{
				this.LastReport.nativeProcessMemorySamples++;
			}
			ChunkManager instance = ChunkManager.Instance;
			NPCZoneManager instance2 = NPCZoneManager.Instance;
			if (this._spawner == null && instance2 != null)
			{
				this._spawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
			}
			int num4 = -1;
			if (instance != null)
			{
				num4 = 0;
				foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in instance.SpawnedChunks)
				{
					if (keyValuePair.Value != null && keyValuePair.Value.chunkRoot != null && keyValuePair.Value.chunkRoot.activeInHierarchy)
					{
						num4++;
					}
				}
				int num5;
				if (!this.LastReport.sessionSeedValid)
				{
					this.LastReport.sessionSeedValid = instance.TryGetSessionSeed(out this.LastReport.sessionSeed);
				}
				else if (instance.TryGetSessionSeed(out num5) && num5 != this.LastReport.sessionSeed)
				{
					this.LastReport.sessionSeedChanged = true;
				}
			}
			this._sceneChanged |= SceneManager.GetActiveScene().handle != this._sceneHandle;
			this._settingsChanged |= Screen.width != this._width || Screen.height != this._height || QualitySettings.GetQualityLevel() != this._quality || QualitySettings.vSyncCount != this._vSync || Application.targetFrameRate != this._targetFps;
			RunSoakRecorder.Memory memory = new RunSoakRecorder.Memory
			{
				elapsed = this.Elapsed,
				phase = this.Phase,
				distance = (float)this._distance,
				logicalX = this._lastLogical.x,
				logicalY = this._lastLogical.y,
				logicalZ = this._lastLogical.z,
				hasPlayer = (PlayerRegistry.Player != null),
				ready = GameSystemsBootstrapper.GameplayReady,
				workingSet = num2,
				privateBytes = num3,
				processMemoryBackend = num,
				allocated = Profiler.GetTotalAllocatedMemoryLong(),
				reserved = Profiler.GetTotalReservedMemoryLong(),
				monoUsed = Profiler.GetMonoUsedSizeLong(),
				monoReserved = Profiler.GetMonoHeapSizeLong(),
				gcManaged = GC.GetTotalMemory(false),
				chunks = ((instance != null) ? instance.SpawnedChunks.Count : (-1)),
				visibleChunks = num4,
				presentationChunks = ((instance != null) ? instance.PresentationVisibleChunkCount : (-1)),
				zonePool = ((instance != null) ? instance.AvailableZonePoolCount : (-1)),
				chunkInstanceMap = RunSoakRecorder.CollectionCount(RunSoakRecorder.ChunkInstanceMap, instance),
				zoneInstanceMap = RunSoakRecorder.CollectionCount(RunSoakRecorder.ZoneInstanceMap, instance),
				chunkIdMap = RunSoakRecorder.CollectionCount(RunSoakRecorder.ChunkIdMap, instance),
				npcChunkIdMap = RunSoakRecorder.CollectionCount(RunSoakRecorder.NpcChunkIdMap, instance2),
				spawnPointMap = RunSoakRecorder.CollectionCount(RunSoakRecorder.SpawnPointMap, instance2),
				resolverCells = ((instance != null) ? instance.ResolverCachedCellCount : (-1)),
				committedPlans = ((instance != null) ? instance.CommittedSpawnPlanCount : (-1)),
				zoneRetirements = ((instance != null) ? instance.PendingZoneRetirementCount : (-1)),
				retirementGroups = ((instance != null) ? instance.PendingZoneRetirementWorkGroups : (-1)),
				zoneInstantiates = ((instance != null) ? instance.TotalZoneInstantiates : (-1)),
				coldZoneInstantiates = ((instance != null) ? instance.RuntimeZoneColdInstantiates : (-1)),
				npcs = ((instance2 != null) ? instance2.CurrentNPCCount : (-1)),
				activeNpcs = ((instance2 != null) ? instance2.ActiveNPCCount : (-1)),
				nearNpcs = ((instance2 != null) ? instance2.CurrentNearCount : (-1)),
				ambientNpcs = ((instance2 != null) ? instance2.CurrentAmbientCount : (-1)),
				ambientTarget = ((instance2 != null) ? instance2.EffectiveAmbientTarget : (-1)),
				npcTarget = ((instance2 != null) ? instance2.EffectiveNearTarget : (-1)),
				pendingNpcs = ((instance2 != null) ? instance2.PendingSpawns : (-1)),
				populationHour = ((instance2 != null) ? instance2.CurrentPopulationHour : (-1f)),
				populationMultiplier = ((instance2 != null) ? instance2.TimeOfDayPopulationMultiplier : (-1f)),
				spawnerActive = ((this._spawner != null) ? this._spawner.ActiveNPCCount : (-1)),
				spawnerPool = ((this._spawner != null) ? this._spawner.PooledNPCCount : (-1)),
				residentNpcs = RunSoakRecorder.CollectionCount(RunSoakRecorder.Residents, this._spawner),
				fadingNpcs = RunSoakRecorder.CollectionCount(RunSoakRecorder.Fading, this._spawner),
				characterMap = RunSoakRecorder.CollectionCount(RunSoakRecorder.CharacterMap, this._spawner),
				chunkSpawnPoints = RunSoakRecorder.CollectionCount(RunSoakRecorder.ChunkSpawnPoints, instance2),
				deadResidents = -1,
				inactiveUnpooledResidents = -1,
				deadSpawnPoints = -1,
				gc0 = GC.CollectionCount(0),
				gc1 = GC.CollectionCount(1),
				gc2 = GC.CollectionCount(2)
			};
			if (this._spawner != null)
			{
				FieldInfo residents = RunSoakRecorder.Residents;
				HashSet<GameObject> hashSet = ((residents != null) ? residents.GetValue(this._spawner) : null) as HashSet<GameObject>;
				if (hashSet != null)
				{
					FieldInfo genericPool = RunSoakRecorder.GenericPool;
					Queue<GameObject> queue = ((genericPool != null) ? genericPool.GetValue(this._spawner) : null) as Queue<GameObject>;
					if (queue != null)
					{
						memory.deadResidents = (memory.inactiveUnpooledResidents = 0);
						foreach (GameObject gameObject in hashSet)
						{
							if (gameObject == null)
							{
								memory.deadResidents++;
							}
							else if (!gameObject.activeInHierarchy && !queue.Contains(gameObject))
							{
								memory.inactiveUnpooledResidents++;
							}
						}
					}
				}
			}
			if (instance2 != null)
			{
				FieldInfo spawnPointMap = RunSoakRecorder.SpawnPointMap;
				Dictionary<NPCSpawnPoint, ChunkInstance> dictionary = ((spawnPointMap != null) ? spawnPointMap.GetValue(instance2) : null) as Dictionary<NPCSpawnPoint, ChunkInstance>;
				if (dictionary != null)
				{
					memory.deadSpawnPoints = 0;
					foreach (KeyValuePair<NPCSpawnPoint, ChunkInstance> keyValuePair2 in dictionary)
					{
						if (keyValuePair2.Key == null)
						{
							memory.deadSpawnPoints++;
						}
					}
				}
			}
			memory.probeMs = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
			RunSoakRecorder.Memory[] memory2 = this._memory;
			int memoryCount = this._memoryCount;
			this._memoryCount = memoryCount + 1;
			memory2[memoryCount] = memory;
		}

		// Token: 0x060049BA RID: 18874 RVA: 0x001C5604 File Offset: 0x001C3804
		private static int CollectionCount(FieldInfo field, object owner)
		{
			if (field == null || owner == null)
			{
				return -1;
			}
			int num;
			try
			{
				object value = field.GetValue(owner);
				ICollection collection = value as ICollection;
				if (collection != null)
				{
					num = collection.Count;
				}
				else
				{
					ICollection<GameObject> collection2 = value as ICollection<GameObject>;
					num = ((collection2 != null) ? collection2.Count : (-1));
				}
			}
			catch
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x001C5668 File Offset: 0x001C3868
		public void Stop(string outcome = "cancelled", string reason = "Stopped by caller.")
		{
			if (!this.IsRecording || this._finishing)
			{
				return;
			}
			this._finishing = true;
			string text = this.CaptureCompletedFrame();
			if (text != null)
			{
				outcome = "invalid";
				reason = text;
			}
			this._elapsed = this.Elapsed;
			this.CapturePosition();
			this.CaptureMemory();
			this.IsRecording = false;
			HeavyZoneStreamingProxy.ColdPayloadSetupMeasured -= this.OnColdPayloadSetup;
			Application.logMessageReceived -= this.OnLog;
			this._gc.Dispose();
			this._cpu.Dispose();
			this._gpu.Dispose();
			Process process = this._process;
			if (process != null)
			{
				process.Dispose();
			}
			this._process = null;
			this.LastReport.renderedMainCameraName = ((this._renderGuard != null) ? this._renderGuard.CameraName : null);
			this.LastReport.renderedMainCameraInstanceId = ((this._renderGuard != null) ? this._renderGuard.CameraInstanceId : 0);
			this.LastReport.renderedMainCameraFrames = ((this._renderGuard != null) ? this._renderGuard.Frames.UniqueRenderedFrames : 0);
			this.LastReport.renderCheckedFrames = ((this._renderGuard != null) ? this._renderGuard.Frames.CheckedFrames : this._framesWithoutRenderGuard);
			this.LastReport.framesWithoutMainCameraRender = this._framesWithoutRenderGuard + ((this._renderGuard != null) ? this._renderGuard.Frames.MissingFrames : 0);
			this.LastReport.mainCameraRenderingValid = this._renderGuard != null && this._renderGuard.AllCheckedFramesRendered && this._framesWithoutRenderGuard == 0 && this.LastReport.renderCheckedFrames == this._frameCount;
			this.LastReport.mainCameraIdentityChanged = this._renderGuard != null && this._renderGuard.CameraIdentityChanged;
			RunSoakRenderGuard renderGuard = this._renderGuard;
			if (renderGuard != null)
			{
				renderGuard.Dispose();
			}
			this._renderGuard = null;
			if (outcome == "completed" && this._options.requireMainCameraRendering && !this.LastReport.mainCameraRenderingValid)
			{
				outcome = "invalid";
				reason = "A measured frame lacks an unchanged actual Camera.main render receipt.";
			}
			this.LastReport.outcome = outcome;
			this.LastReport.reason = reason;
			this.LastReport.elapsedSeconds = this._elapsed;
			this.LastReport.frameSamples = this._frameCount;
			this.LastReport.memorySamples = this._memoryCount;
			this.LastReport.heavyPayloadSetupSamples = this._heavyPayloadSetupCount;
			this.LastReport.heavyPayloadSetupSamplesDropped = this._heavyPayloadSetupDropped;
			this.LastReport.heavyPayloadSetupsCompleted = HeavyZoneStreamingProxy.CompletedColdPayloadSetupCount - this._heavyPayloadSetupInitialCompleted;
			this.LastReport.missedFrames = this._missedFrames;
			this.LastReport.completeFrameCoverage = this._missedFrames == 0 && outcome == "completed" && this._frameCount > 0;
			this.LastReport.applicationErrors = this._errors;
			this.LastReport.sceneChanged = this._sceneChanged;
			this.LastReport.settingsChanged = this._settingsChanged;
			this.LastReport.distanceMetres = this._distance;
			this.LastReport.startLogicalPosition = this._startLogical;
			this.LastReport.endLogicalPosition = this._lastLogical;
			try
			{
				this.Export();
			}
			catch (Exception ex)
			{
				this.LastReport.outcome = "export_failed";
				this.LastReport.reason = ex.ToString();
				string text2 = "[RunSoak] Export failed: ";
				Exception ex2 = ex;
				Debug.LogError(text2 + ((ex2 != null) ? ex2.ToString() : null));
			}
			finally
			{
				try
				{
					Action<RunSoakRecorder> stopped = this.Stopped;
					if (stopped != null)
					{
						stopped(this);
					}
				}
				finally
				{
					this.RuntimeTick = null;
					this.Stopped = null;
					this._frames = null;
					this._memory = null;
					this._heavyPayloadSetups = null;
					this._finishing = false;
				}
			}
		}

		// Token: 0x060049BC RID: 18876 RVA: 0x001C5A64 File Offset: 0x001C3C64
		private void Export()
		{
			float[] array = new float[this._frameCount];
			for (int i = 0; i < this._frameCount; i++)
			{
				array[i] = this._frames[i].milliseconds;
			}
			this.LastReport.allFrames = RunSoakRecorder.Distribution.Calculate(array, this._frameCount);
			this.LastReport.phases = new RunSoakRecorder.PhaseReport[5];
			string[] array2 = new string[] { "warmup", "standing_before_or_manual", "synthetic_travel", "standing_after", "final_population_warmup" };
			for (int j = 0; j < this.LastReport.phases.Length; j++)
			{
				int num = 0;
				RunSoakRecorder.PhaseReport phaseReport = new RunSoakRecorder.PhaseReport
				{
					phase = j,
					name = array2[j]
				};
				for (int k = 0; k < this._frameCount; k++)
				{
					if (this._frames[k].phase == j)
					{
						array[num++] = this._frames[k].milliseconds;
						phaseReport.AddPopulationSample(this._frames[k].activeNpcs, this._frames[k].milliseconds, this._options.npcStressThreshold);
					}
				}
				if (phaseReport.minimumActiveNpcs == 2147483647)
				{
					phaseReport.minimumActiveNpcs = -1;
				}
				phaseReport.frameTime = RunSoakRecorder.Distribution.Calculate(array, num);
				this.LastReport.phases[j] = phaseReport;
			}
			using (StreamWriter streamWriter = new StreamWriter(Path.Combine(this.OutputDirectory, "frames.csv"), false, new UTF8Encoding(false), 65536))
			{
				streamWriter.WriteLine("frame,elapsed_s,phase,frame_ms,distance_m,cpu_main_ms,gpu_ms,gc_allocated_bytes,active_npcs,deferred_streaming_budget_active,deferred_streaming_budget_ms,deferred_streaming_elapsed_ms,deferred_streaming_budget_deferrals,deferred_streaming_overshoot_ms");
				for (int l = 0; l < this._frameCount; l++)
				{
					RunSoakRecorder.Frame frame = this._frames[l];
					streamWriter.WriteLine(string.Join(",", new string[]
					{
						frame.number.ToString(CultureInfo.InvariantCulture),
						RunSoakRecorder.F(frame.elapsed),
						frame.phase.ToString(),
						RunSoakRecorder.F((double)frame.milliseconds),
						RunSoakRecorder.F((double)frame.distance),
						RunSoakRecorder.Optional((double)frame.cpuMs),
						RunSoakRecorder.Optional((double)frame.gpuMs),
						RunSoakRecorder.Optional(frame.gcBytes),
						frame.activeNpcs.ToString(),
						frame.deferredStreamingBudgetActive.ToString(),
						RunSoakRecorder.Optional(frame.deferredStreamingBudgetMs),
						RunSoakRecorder.Optional(frame.deferredStreamingElapsedMs),
						frame.deferredStreamingBudgetDeferrals.ToString(),
						(frame.deferredStreamingBudgetActive == 1) ? RunSoakRecorder.F(Math.Max(0.0, frame.deferredStreamingElapsedMs - frame.deferredStreamingBudgetMs)) : ""
					}));
				}
			}
			using (StreamWriter streamWriter2 = new StreamWriter(Path.Combine(this.OutputDirectory, "memory.csv"), false, new UTF8Encoding(false), 65536))
			{
				streamWriter2.WriteLine("elapsed_s,phase,distance_m,logical_x,logical_y,logical_z,has_player,gameplay_ready,process_working_set_bytes,process_private_bytes,process_memory_backend,unity_allocated_bytes,unity_reserved_bytes,mono_used_bytes,mono_reserved_bytes,gc_managed_bytes,chunks,visible_chunk_roots,presentation_visible_chunks,zone_pool,chunk_id_map,npc_chunk_id_map,spawn_point_map,chunk_instance_prefab_map,zone_instance_prefab_map,resolver_cached_cells,committed_spawn_plans,pending_zone_retirements,pending_retirement_groups,total_zone_instantiates,runtime_cold_zone_instantiates,npcs,active_npcs,effective_near_target,pending_spawns,population_hour,population_multiplier,primary_spawner_active,primary_spawner_pool,resident_npcs,dead_residents,inactive_unpooled_residents,fading_npcs,character_instance_map,chunk_spawn_point_map,dead_spawn_points,gc_gen0,gc_gen1,gc_gen2,probe_ms,near_npcs,ambient_npcs,effective_ambient_target");
				for (int m = 0; m < this._memoryCount; m++)
				{
					RunSoakRecorder.Memory memory = this._memory[m];
					streamWriter2.WriteLine(string.Join(",", new string[]
					{
						RunSoakRecorder.F(memory.elapsed),
						memory.phase.ToString(),
						RunSoakRecorder.F((double)memory.distance),
						RunSoakRecorder.F((double)memory.logicalX),
						RunSoakRecorder.F((double)memory.logicalY),
						RunSoakRecorder.F((double)memory.logicalZ),
						memory.hasPlayer ? "1" : "0",
						memory.ready ? "1" : "0",
						RunSoakRecorder.Optional(memory.workingSet),
						RunSoakRecorder.Optional(memory.privateBytes),
						memory.processMemoryBackend.ToString(),
						RunSoakRecorder.Optional(memory.allocated),
						RunSoakRecorder.Optional(memory.reserved),
						RunSoakRecorder.Optional(memory.monoUsed),
						RunSoakRecorder.Optional(memory.monoReserved),
						RunSoakRecorder.Optional(memory.gcManaged),
						memory.chunks.ToString(),
						memory.visibleChunks.ToString(),
						memory.presentationChunks.ToString(),
						memory.zonePool.ToString(),
						memory.chunkIdMap.ToString(),
						memory.npcChunkIdMap.ToString(),
						memory.spawnPointMap.ToString(),
						memory.chunkInstanceMap.ToString(),
						memory.zoneInstanceMap.ToString(),
						memory.resolverCells.ToString(),
						memory.committedPlans.ToString(),
						memory.zoneRetirements.ToString(),
						memory.retirementGroups.ToString(),
						memory.zoneInstantiates.ToString(),
						memory.coldZoneInstantiates.ToString(),
						memory.npcs.ToString(),
						memory.activeNpcs.ToString(),
						memory.npcTarget.ToString(),
						memory.pendingNpcs.ToString(),
						RunSoakRecorder.Optional((double)memory.populationHour),
						RunSoakRecorder.Optional((double)memory.populationMultiplier),
						memory.spawnerActive.ToString(),
						memory.spawnerPool.ToString(),
						memory.residentNpcs.ToString(),
						memory.deadResidents.ToString(),
						memory.inactiveUnpooledResidents.ToString(),
						memory.fadingNpcs.ToString(),
						memory.characterMap.ToString(),
						memory.chunkSpawnPoints.ToString(),
						memory.deadSpawnPoints.ToString(),
						memory.gc0.ToString(),
						memory.gc1.ToString(),
						memory.gc2.ToString(),
						RunSoakRecorder.F(memory.probeMs),
						memory.nearNpcs.ToString(),
						memory.ambientNpcs.ToString(),
						memory.ambientTarget.ToString()
					}));
				}
			}
			using (StreamWriter streamWriter3 = new StreamWriter(Path.Combine(this.OutputDirectory, "heavy_payload_setups.csv"), false, new UTF8Encoding(false), 65536))
			{
				streamWriter3.WriteLine("sequence,frame,elapsed_start_s,phase,proxy_instance_id,resource_path,clone_ms,setup_ms,total_sync_ms");
				for (int n = 0; n < this._heavyPayloadSetupCount; n++)
				{
					RunSoakRecorder.HeavyPayloadSetup heavyPayloadSetup = this._heavyPayloadSetups[n];
					HeavyZoneStreamingProxy.ColdPayloadSetupSample sample = heavyPayloadSetup.sample;
					streamWriter3.WriteLine(string.Join(",", new string[]
					{
						sample.sequence.ToString(CultureInfo.InvariantCulture),
						sample.frame.ToString(CultureInfo.InvariantCulture),
						RunSoakRecorder.F(sample.startedRealtime - this._start),
						heavyPayloadSetup.phase.ToString(CultureInfo.InvariantCulture),
						sample.proxyInstanceId.ToString(CultureInfo.InvariantCulture),
						RunSoakRecorder.CsvText(sample.resourcePath),
						RunSoakRecorder.F(sample.cloneMs),
						RunSoakRecorder.F(sample.setupMs),
						RunSoakRecorder.F(sample.cloneMs + sample.setupMs)
					}));
				}
			}
			File.WriteAllText(Path.Combine(this.OutputDirectory, "summary.json"), JsonUtility.ToJson(this.LastReport, true));
		}

		// Token: 0x060049BD RID: 18877 RVA: 0x001C62E4 File Offset: 0x001C44E4
		private static string F(double value)
		{
			return value.ToString("0.######", CultureInfo.InvariantCulture);
		}

		// Token: 0x060049BE RID: 18878 RVA: 0x001C62F7 File Offset: 0x001C44F7
		private static string CsvText(string value)
		{
			return "\"" + (value ?? "").Replace("\"", "\"\"") + "\"";
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x001C6321 File Offset: 0x001C4521
		private static string Optional(double value)
		{
			if (value >= 0.0)
			{
				return RunSoakRecorder.F(value);
			}
			return "";
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x001C633B File Offset: 0x001C453B
		private static string Optional(long value)
		{
			if (value >= 0L)
			{
				return value.ToString(CultureInfo.InvariantCulture);
			}
			return "";
		}

		// Token: 0x060049C1 RID: 18881 RVA: 0x001C6354 File Offset: 0x001C4554
		private void OnLog(string condition, string trace, LogType type)
		{
			if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
			{
				this._errors++;
			}
		}

		// Token: 0x060049C2 RID: 18882 RVA: 0x001C636F File Offset: 0x001C456F
		private void OnApplicationQuit()
		{
			this.Stop("interrupted", "Application quit before configured end.");
		}

		// Token: 0x060049C3 RID: 18883 RVA: 0x001C6381 File Offset: 0x001C4581
		private void OnDisable()
		{
			this.Stop("interrupted", "Recorder disabled or Play Mode stopped.");
		}

		// Token: 0x060049C4 RID: 18884 RVA: 0x001C6393 File Offset: 0x001C4593
		private void OnDestroy()
		{
			HeavyZoneStreamingProxy.ColdPayloadSetupMeasured -= this.OnColdPayloadSetup;
			RunSoakRenderGuard renderGuard = this._renderGuard;
			if (renderGuard != null)
			{
				renderGuard.Dispose();
			}
			this._renderGuard = null;
			if (RunSoakRecorder.Instance == this)
			{
				RunSoakRecorder.Instance = null;
			}
		}

		// Token: 0x04004AA0 RID: 19104
		private const int MaximumHeavyPayloadSetupSamples = 2048;

		// Token: 0x04004AA7 RID: 19111
		private RunSoakRecorder.Options _options;

		// Token: 0x04004AA8 RID: 19112
		private RunSoakRecorder.Frame[] _frames;

		// Token: 0x04004AA9 RID: 19113
		private RunSoakRecorder.Memory[] _memory;

		// Token: 0x04004AAA RID: 19114
		private RunSoakRecorder.HeavyPayloadSetup[] _heavyPayloadSetups;

		// Token: 0x04004AAB RID: 19115
		private int _heavyPayloadSetupCount;

		// Token: 0x04004AAC RID: 19116
		private int _heavyPayloadSetupDropped;

		// Token: 0x04004AAD RID: 19117
		private long _heavyPayloadSetupInitialCompleted;

		// Token: 0x04004AAE RID: 19118
		private int _frameCount;

		// Token: 0x04004AAF RID: 19119
		private int _memoryCount;

		// Token: 0x04004AB0 RID: 19120
		private int _missedFrames;

		// Token: 0x04004AB1 RID: 19121
		private int _sceneHandle;

		// Token: 0x04004AB2 RID: 19122
		private int _errors;

		// Token: 0x04004AB3 RID: 19123
		private int _width;

		// Token: 0x04004AB4 RID: 19124
		private int _height;

		// Token: 0x04004AB5 RID: 19125
		private int _quality;

		// Token: 0x04004AB6 RID: 19126
		private int _vSync;

		// Token: 0x04004AB7 RID: 19127
		private int _targetFps;

		// Token: 0x04004AB8 RID: 19128
		private double _start;

		// Token: 0x04004AB9 RID: 19129
		private double _elapsed;

		// Token: 0x04004ABA RID: 19130
		private double _nextMemory;

		// Token: 0x04004ABB RID: 19131
		private double _distance;

		// Token: 0x04004ABC RID: 19132
		private Vector3 _previousLogical;

		// Token: 0x04004ABD RID: 19133
		private Vector3 _startLogical;

		// Token: 0x04004ABE RID: 19134
		private Vector3 _lastLogical;

		// Token: 0x04004ABF RID: 19135
		private bool _hasPreviousPlayer;

		// Token: 0x04004AC0 RID: 19136
		private bool _sceneChanged;

		// Token: 0x04004AC1 RID: 19137
		private bool _settingsChanged;

		// Token: 0x04004AC2 RID: 19138
		private bool _finishing;

		// Token: 0x04004AC3 RID: 19139
		private Process _process;

		// Token: 0x04004AC4 RID: 19140
		private NPCRuntimeSpawner _spawner;

		// Token: 0x04004AC5 RID: 19141
		private ProfilerRecorder _gc;

		// Token: 0x04004AC6 RID: 19142
		private ProfilerRecorder _cpu;

		// Token: 0x04004AC7 RID: 19143
		private ProfilerRecorder _gpu;

		// Token: 0x04004AC8 RID: 19144
		private RunSoakRenderGuard _renderGuard;

		// Token: 0x04004AC9 RID: 19145
		private int _framesWithoutRenderGuard;

		// Token: 0x04004ACA RID: 19146
		private RunSoakFrameTimeline _frameTimeline;

		// Token: 0x04004ACB RID: 19147
		private static readonly FieldInfo ChunkIdMap = typeof(ChunkManager).GetField("_chunkIdMap", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004ACC RID: 19148
		private static readonly FieldInfo NpcChunkIdMap = typeof(NPCZoneManager).GetField("_chunkIdMap", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004ACD RID: 19149
		private static readonly FieldInfo SpawnPointMap = typeof(NPCZoneManager).GetField("_spawnPointToChunk", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004ACE RID: 19150
		private static readonly FieldInfo ChunkSpawnPoints = typeof(NPCZoneManager).GetField("_chunkSpawnPoints", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004ACF RID: 19151
		private static readonly FieldInfo Residents = typeof(NPCRuntimeSpawner).GetField("_residentNpcs", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004AD0 RID: 19152
		private static readonly FieldInfo Fading = typeof(NPCRuntimeSpawner).GetField("_fadingNpcs", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004AD1 RID: 19153
		private static readonly FieldInfo CharacterMap = typeof(NPCRuntimeSpawner).GetField("_characterByInstance", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004AD2 RID: 19154
		private static readonly FieldInfo GenericPool = typeof(NPCRuntimeSpawner).GetField("_genericPool", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004AD3 RID: 19155
		private static readonly FieldInfo ChunkInstanceMap = typeof(ChunkManager).GetField("_instanceToPrefabMap", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004AD4 RID: 19156
		private static readonly FieldInfo ZoneInstanceMap = typeof(ChunkManager).GetField("_zoneInstanceToPrefabMap", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x02000A21 RID: 2593
		[Serializable]
		public sealed class Options
		{
			// Token: 0x04004AD5 RID: 19157
			public string outputDirectory;

			// Token: 0x04004AD6 RID: 19158
			public double durationSeconds = 600.0;

			// Token: 0x04004AD7 RID: 19159
			public double memoryIntervalSeconds = 1.0;

			// Token: 0x04004AD8 RID: 19160
			public int maximumFrames = 1000000;

			// Token: 0x04004AD9 RID: 19161
			public int npcStressThreshold;

			// Token: 0x04004ADA RID: 19162
			public bool requireMainCameraRendering;

			// Token: 0x04004ADB RID: 19163
			public string protocol = "Manual gameplay; no automated route";
		}

		// Token: 0x02000A22 RID: 2594
		[Serializable]
		public sealed class Distribution
		{
			// Token: 0x060049C8 RID: 18888 RVA: 0x001C6538 File Offset: 0x001C4738
			public static RunSoakRecorder.Distribution Calculate(float[] values, int count)
			{
				if (values == null || count < 0 || count > values.Length)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				RunSoakRecorder.Distribution distribution = new RunSoakRecorder.Distribution();
				float[] array = new float[count];
				double num = 0.0;
				for (int i = 0; i < count; i++)
				{
					float num2 = values[i];
					if (!float.IsNaN(num2) && !float.IsInfinity(num2) && num2 > 0f)
					{
						float[] array2 = array;
						RunSoakRecorder.Distribution distribution2 = distribution;
						int num3 = distribution2.samples;
						distribution2.samples = num3 + 1;
						array2[num3] = num2;
						num += (double)num2;
						if (num2 > 16.666666f)
						{
							distribution.over16_67Ms++;
						}
						if (num2 > 33.333332f)
						{
							distribution.over33_33Ms++;
						}
						if (num2 > 50f)
						{
							distribution.over50Ms++;
						}
						if (num2 > 100f)
						{
							distribution.over100Ms++;
						}
					}
				}
				if (distribution.samples == 0)
				{
					return distribution;
				}
				Array.Sort<float>(array, 0, distribution.samples);
				distribution.meanMs = num / (double)distribution.samples;
				distribution.averageFps = 1000.0 / distribution.meanMs;
				distribution.medianMs = (double)array[(int)Math.Ceiling((double)distribution.samples * 0.5) - 1];
				distribution.p95Ms = (double)array[(int)Math.Ceiling((double)distribution.samples * 0.95) - 1];
				distribution.p99Ms = (double)array[(int)Math.Ceiling((double)distribution.samples * 0.99) - 1];
				distribution.maximumMs = (double)array[distribution.samples - 1];
				return distribution;
			}

			// Token: 0x04004ADC RID: 19164
			public int samples;

			// Token: 0x04004ADD RID: 19165
			public double meanMs;

			// Token: 0x04004ADE RID: 19166
			public double medianMs;

			// Token: 0x04004ADF RID: 19167
			public double p95Ms;

			// Token: 0x04004AE0 RID: 19168
			public double p99Ms;

			// Token: 0x04004AE1 RID: 19169
			public double maximumMs;

			// Token: 0x04004AE2 RID: 19170
			public double averageFps;

			// Token: 0x04004AE3 RID: 19171
			public int over16_67Ms;

			// Token: 0x04004AE4 RID: 19172
			public int over33_33Ms;

			// Token: 0x04004AE5 RID: 19173
			public int over50Ms;

			// Token: 0x04004AE6 RID: 19174
			public int over100Ms;
		}

		// Token: 0x02000A23 RID: 2595
		[Serializable]
		public sealed class PhaseReport
		{
			// Token: 0x060049CA RID: 18890 RVA: 0x001C66DC File Offset: 0x001C48DC
			public void AddPopulationSample(int active, float frameMilliseconds, int threshold)
			{
				if (float.IsNaN(frameMilliseconds) || float.IsInfinity(frameMilliseconds) || frameMilliseconds <= 0f)
				{
					return;
				}
				double num = (double)frameMilliseconds / 1000.0;
				this.populationFrames++;
				this.populationSeconds += num;
				if (active < 0)
				{
					this.populationUnavailableFrames++;
				}
				else
				{
					this.minimumActiveNpcs = Math.Min(this.minimumActiveNpcs, active);
					this.maximumActiveNpcs = Math.Max(this.maximumActiveNpcs, active);
					this.activeNpcSeconds += (double)active * num;
					if (active >= threshold)
					{
						this.secondsAtPopulationThreshold += num;
					}
				}
				this.populationCoverage = this.secondsAtPopulationThreshold / this.populationSeconds;
				this.meanActiveNpcs = this.activeNpcSeconds / this.populationSeconds;
			}

			// Token: 0x060049CB RID: 18891 RVA: 0x001C67AC File Offset: 0x001C49AC
			public bool HasSustainedPopulation(double expectedSeconds, double requiredCoverage)
			{
				return expectedSeconds > 0.0 && requiredCoverage > 0.0 && requiredCoverage <= 1.0 && this.populationUnavailableFrames == 0 && this.populationFrames > 0 && this.populationSeconds >= expectedSeconds * 0.95 && this.populationCoverage >= requiredCoverage && this.secondsAtPopulationThreshold + 1E-06 >= expectedSeconds * requiredCoverage;
			}

			// Token: 0x04004AE7 RID: 19175
			public int phase;

			// Token: 0x04004AE8 RID: 19176
			public string name;

			// Token: 0x04004AE9 RID: 19177
			public RunSoakRecorder.Distribution frameTime;

			// Token: 0x04004AEA RID: 19178
			public int populationFrames;

			// Token: 0x04004AEB RID: 19179
			public int populationUnavailableFrames;

			// Token: 0x04004AEC RID: 19180
			public int minimumActiveNpcs = int.MaxValue;

			// Token: 0x04004AED RID: 19181
			public int maximumActiveNpcs;

			// Token: 0x04004AEE RID: 19182
			public double populationSeconds;

			// Token: 0x04004AEF RID: 19183
			public double secondsAtPopulationThreshold;

			// Token: 0x04004AF0 RID: 19184
			public double activeNpcSeconds;

			// Token: 0x04004AF1 RID: 19185
			public double populationCoverage;

			// Token: 0x04004AF2 RID: 19186
			public double meanActiveNpcs;
		}

		// Token: 0x02000A24 RID: 2596
		[Serializable]
		public sealed class Report
		{
			// Token: 0x04004AF3 RID: 19187
			public string recorderVersion = "RunSoakRecorder/v4-heavy-payload-setup";

			// Token: 0x04004AF4 RID: 19188
			public bool mainCameraRenderingRequired;

			// Token: 0x04004AF5 RID: 19189
			public bool mainCameraRenderingValid;

			// Token: 0x04004AF6 RID: 19190
			public bool mainCameraIdentityChanged;

			// Token: 0x04004AF7 RID: 19191
			public string renderedMainCameraName;

			// Token: 0x04004AF8 RID: 19192
			public int renderedMainCameraInstanceId;

			// Token: 0x04004AF9 RID: 19193
			public int renderedMainCameraFrames;

			// Token: 0x04004AFA RID: 19194
			public int renderCheckedFrames;

			// Token: 0x04004AFB RID: 19195
			public int framesWithoutMainCameraRender;

			// Token: 0x04004AFC RID: 19196
			public int firstMeasuredSourceFrame = -1;

			// Token: 0x04004AFD RID: 19197
			public int lastMeasuredSourceFrame = -1;

			// Token: 0x04004AFE RID: 19198
			public double initialUnmeasuredSeconds;

			// Token: 0x04004AFF RID: 19199
			public string frameTimingNote = "Each row describes the completed source frame: its phase/NPC/distance snapshot was captured in LateUpdate, then its unscaled duration and render receipt were consumed in the next Update before route mutation or Stop. The partial Begin frame is excluded. Population is a time-weighted end-of-source-frame snapshot, not within-frame birth/death integration.";

			// Token: 0x04004B00 RID: 19200
			public string outcome;

			// Token: 0x04004B01 RID: 19201
			public string reason;

			// Token: 0x04004B02 RID: 19202
			public string outputDirectory;

			// Token: 0x04004B03 RID: 19203
			public string protocol;

			// Token: 0x04004B04 RID: 19204
			public string utcStarted;

			// Token: 0x04004B05 RID: 19205
			public string unity;

			// Token: 0x04004B06 RID: 19206
			public string scene;

			// Token: 0x04004B07 RID: 19207
			public string cpu;

			// Token: 0x04004B08 RID: 19208
			public string gpu;

			// Token: 0x04004B09 RID: 19209
			public string graphicsApi;

			// Token: 0x04004B0A RID: 19210
			public string quality;

			// Token: 0x04004B0B RID: 19211
			public string renderPipeline;

			// Token: 0x04004B0C RID: 19212
			public bool editor;

			// Token: 0x04004B0D RID: 19213
			public bool developmentBuild;

			// Token: 0x04004B0E RID: 19214
			public bool completeFrameCoverage;

			// Token: 0x04004B0F RID: 19215
			public bool sceneChanged;

			// Token: 0x04004B10 RID: 19216
			public bool settingsChanged;

			// Token: 0x04004B11 RID: 19217
			public bool sessionSeedChanged;

			// Token: 0x04004B12 RID: 19218
			public int width;

			// Token: 0x04004B13 RID: 19219
			public int height;

			// Token: 0x04004B14 RID: 19220
			public int vSync;

			// Token: 0x04004B15 RID: 19221
			public int targetFrameRate;

			// Token: 0x04004B16 RID: 19222
			public int qualityIndex;

			// Token: 0x04004B17 RID: 19223
			public int ramMiB;

			// Token: 0x04004B18 RID: 19224
			public int graphicsMemoryMiB;

			// Token: 0x04004B19 RID: 19225
			public int frameSamples;

			// Token: 0x04004B1A RID: 19226
			public int memorySamples;

			// Token: 0x04004B1B RID: 19227
			public int missedFrames;

			// Token: 0x04004B1C RID: 19228
			public int applicationErrors;

			// Token: 0x04004B1D RID: 19229
			public int maximumFrames;

			// Token: 0x04004B1E RID: 19230
			public int npcStressThreshold;

			// Token: 0x04004B1F RID: 19231
			public int processMemoryValidSamples;

			// Token: 0x04004B20 RID: 19232
			public int processMemoryUnavailableSamples;

			// Token: 0x04004B21 RID: 19233
			public int nativeProcessMemorySamples;

			// Token: 0x04004B22 RID: 19234
			public int heavyPayloadSetupSamples;

			// Token: 0x04004B23 RID: 19235
			public int heavyPayloadSetupSamplesDropped;

			// Token: 0x04004B24 RID: 19236
			public long heavyPayloadSetupsCompleted;

			// Token: 0x04004B25 RID: 19237
			public string heavyPayloadSetupNote = "heavy_payload_setups.csv records synchronous clone/setup completions only, not Resources IO, staged snapshot/wake, readiness or pooled reuse. The Resources prefab may already be cached. clone_ms is atomic Instantiate including inactive-gate creation for staged payloads; setup_ms includes naming/pose, terrain alignment, renderer layers/camera masks, LOD tuning and terrain clamps, with no yield between them. Join frame to frames.csv only when that completed source frame exists (the partial Begin/Stop frame may not). Sequence is the session-wide completion counter; dropped rows are explicit when the fixed diagnostic buffer fills.";

			// Token: 0x04004B26 RID: 19238
			public long recorderBufferBytes;

			// Token: 0x04004B27 RID: 19239
			public double elapsedSeconds;

			// Token: 0x04004B28 RID: 19240
			public double distanceMetres;

			// Token: 0x04004B29 RID: 19241
			public Vector3 startLogicalPosition;

			// Token: 0x04004B2A RID: 19242
			public Vector3 endLogicalPosition;

			// Token: 0x04004B2B RID: 19243
			public bool sessionSeedValid;

			// Token: 0x04004B2C RID: 19244
			public int sessionSeed;

			// Token: 0x04004B2D RID: 19245
			public RunSoakRecorder.Distribution allFrames;

			// Token: 0x04004B2E RID: 19246
			public RunSoakRecorder.PhaseReport[] phases;

			// Token: 0x04004B2F RID: 19247
			public string counterNotes = "Empty CSV cells and -1 integer values mean unavailable, never zero. Memory is whole process (Editor includes Editor overhead); total allocated/reserved are Unity allocator statistics, not OS RAM. Process memory backend: 1 = Windows GetProcessMemoryInfo, 2 = positive managed Process counters, 0 = unavailable. Private bytes are private committed memory (not resident RAM); working set is resident RAM including shared pages. Zero CPU/GPU time and zero process-memory counters are treated as unavailable; zero GC bytes is valid. NPC spawner counts cover the first civilian spawner found; NPCZoneManager counts cover its managed population. CPU/GPU/GC profiler counters may be unavailable in release; frame wall times and process memory remain independently measured. Periodic memory sampling overhead is included in frame time and reported separately. Buffers are preallocated before the first measured frame; summary sorting/export occurs only after capture stops. Deferred-streaming columns are source-frame snapshots of post-spawn initialization and deferred zone spawning only; they exclude other streaming work and seeded pool maintenance. A budget deferral counts a queue visit stopped by the shared quota, not an item. Budget active = 0 means the gameplay guard was bypassed; -1/empty means no matching source-frame sample. Overshoot is elapsed beyond the shared quota while active; atomic operations cannot be preempted. Distance is accumulated planar logical player movement, including intentional repositioning; origin shifts contribute zero. Memory growth alone does not establish a leak; compare settled equivalent populations/cache states across repeat runs.";
		}

		// Token: 0x02000A25 RID: 2597
		private struct Frame
		{
			// Token: 0x04004B30 RID: 19248
			public int number;

			// Token: 0x04004B31 RID: 19249
			public int phase;

			// Token: 0x04004B32 RID: 19250
			public int activeNpcs;

			// Token: 0x04004B33 RID: 19251
			public double elapsed;

			// Token: 0x04004B34 RID: 19252
			public float milliseconds;

			// Token: 0x04004B35 RID: 19253
			public float distance;

			// Token: 0x04004B36 RID: 19254
			public float cpuMs;

			// Token: 0x04004B37 RID: 19255
			public float gpuMs;

			// Token: 0x04004B38 RID: 19256
			public long gcBytes;

			// Token: 0x04004B39 RID: 19257
			public int deferredStreamingBudgetActive;

			// Token: 0x04004B3A RID: 19258
			public int deferredStreamingBudgetDeferrals;

			// Token: 0x04004B3B RID: 19259
			public double deferredStreamingBudgetMs;

			// Token: 0x04004B3C RID: 19260
			public double deferredStreamingElapsedMs;
		}

		// Token: 0x02000A26 RID: 2598
		private struct Memory
		{
			// Token: 0x04004B3D RID: 19261
			public double elapsed;

			// Token: 0x04004B3E RID: 19262
			public double probeMs;

			// Token: 0x04004B3F RID: 19263
			public float distance;

			// Token: 0x04004B40 RID: 19264
			public float logicalX;

			// Token: 0x04004B41 RID: 19265
			public float logicalY;

			// Token: 0x04004B42 RID: 19266
			public float logicalZ;

			// Token: 0x04004B43 RID: 19267
			public long workingSet;

			// Token: 0x04004B44 RID: 19268
			public long privateBytes;

			// Token: 0x04004B45 RID: 19269
			public long allocated;

			// Token: 0x04004B46 RID: 19270
			public long reserved;

			// Token: 0x04004B47 RID: 19271
			public long monoUsed;

			// Token: 0x04004B48 RID: 19272
			public long monoReserved;

			// Token: 0x04004B49 RID: 19273
			public long gcManaged;

			// Token: 0x04004B4A RID: 19274
			public int processMemoryBackend;

			// Token: 0x04004B4B RID: 19275
			public int phase;

			// Token: 0x04004B4C RID: 19276
			public int chunks;

			// Token: 0x04004B4D RID: 19277
			public int visibleChunks;

			// Token: 0x04004B4E RID: 19278
			public int presentationChunks;

			// Token: 0x04004B4F RID: 19279
			public int zonePool;

			// Token: 0x04004B50 RID: 19280
			public int chunkIdMap;

			// Token: 0x04004B51 RID: 19281
			public int npcChunkIdMap;

			// Token: 0x04004B52 RID: 19282
			public int spawnPointMap;

			// Token: 0x04004B53 RID: 19283
			public int chunkInstanceMap;

			// Token: 0x04004B54 RID: 19284
			public int zoneInstanceMap;

			// Token: 0x04004B55 RID: 19285
			public float populationHour;

			// Token: 0x04004B56 RID: 19286
			public float populationMultiplier;

			// Token: 0x04004B57 RID: 19287
			public int resolverCells;

			// Token: 0x04004B58 RID: 19288
			public int committedPlans;

			// Token: 0x04004B59 RID: 19289
			public int zoneRetirements;

			// Token: 0x04004B5A RID: 19290
			public int retirementGroups;

			// Token: 0x04004B5B RID: 19291
			public int zoneInstantiates;

			// Token: 0x04004B5C RID: 19292
			public int coldZoneInstantiates;

			// Token: 0x04004B5D RID: 19293
			public int npcs;

			// Token: 0x04004B5E RID: 19294
			public int activeNpcs;

			// Token: 0x04004B5F RID: 19295
			public int nearNpcs;

			// Token: 0x04004B60 RID: 19296
			public int ambientNpcs;

			// Token: 0x04004B61 RID: 19297
			public int npcTarget;

			// Token: 0x04004B62 RID: 19298
			public int ambientTarget;

			// Token: 0x04004B63 RID: 19299
			public int pendingNpcs;

			// Token: 0x04004B64 RID: 19300
			public int spawnerActive;

			// Token: 0x04004B65 RID: 19301
			public int spawnerPool;

			// Token: 0x04004B66 RID: 19302
			public int gc0;

			// Token: 0x04004B67 RID: 19303
			public int gc1;

			// Token: 0x04004B68 RID: 19304
			public int gc2;

			// Token: 0x04004B69 RID: 19305
			public int residentNpcs;

			// Token: 0x04004B6A RID: 19306
			public int deadResidents;

			// Token: 0x04004B6B RID: 19307
			public int inactiveUnpooledResidents;

			// Token: 0x04004B6C RID: 19308
			public int fadingNpcs;

			// Token: 0x04004B6D RID: 19309
			public int characterMap;

			// Token: 0x04004B6E RID: 19310
			public int chunkSpawnPoints;

			// Token: 0x04004B6F RID: 19311
			public int deadSpawnPoints;

			// Token: 0x04004B70 RID: 19312
			public bool hasPlayer;

			// Token: 0x04004B71 RID: 19313
			public bool ready;
		}

		// Token: 0x02000A27 RID: 2599
		private struct HeavyPayloadSetup
		{
			// Token: 0x04004B72 RID: 19314
			public HeavyZoneStreamingProxy.ColdPayloadSetupSample sample;

			// Token: 0x04004B73 RID: 19315
			public int phase;
		}
	}
}
