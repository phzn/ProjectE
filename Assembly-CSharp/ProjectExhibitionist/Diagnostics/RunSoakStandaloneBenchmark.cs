using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Features.GameOver;
using Features.Home;
using Features.Loading;
using Features.MainMenu;
using Features.NPC;
using Features.RenderingPrecision;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Diagnostics
{
	// Token: 0x02000A2A RID: 2602
	public sealed class RunSoakStandaloneBenchmark : MonoBehaviour
	{
		// Token: 0x060049EA RID: 18922 RVA: 0x001C6C40 File Offset: 0x001C4E40
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Install()
		{
			if (Application.isEditor)
			{
				return;
			}
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (Array.IndexOf<string>(commandLineArgs, "-runSoakBenchmark") < 0)
			{
				return;
			}
			GameObject gameObject = new GameObject("[RunSoakStandaloneBenchmark]");
			Object.DontDestroyOnLoad(gameObject);
			RunSoakStandaloneBenchmark runSoakStandaloneBenchmark = gameObject.AddComponent<RunSoakStandaloneBenchmark>();
			try
			{
				runSoakStandaloneBenchmark.Configure(commandLineArgs);
			}
			catch (Exception ex)
			{
				RunSoakStandaloneBenchmark runSoakStandaloneBenchmark2 = runSoakStandaloneBenchmark;
				string text = "Configuration failed: ";
				Exception ex2 = ex;
				runSoakStandaloneBenchmark2.Fail(text + ((ex2 != null) ? ex2.ToString() : null), 1);
			}
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x001C6CBC File Offset: 0x001C4EBC
		private void Configure(string[] args)
		{
			this._status.outputDirectory = Path.GetFullPath(RunSoakStandaloneBenchmark.Argument(args, "-runSoakOutput") ?? Path.Combine(Application.persistentDataPath, "PerformanceCaptures", "Standalone_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff")));
			if (Directory.Exists(this._status.outputDirectory) && Directory.GetFileSystemEntries(this._status.outputDirectory).Length != 0)
			{
				throw new IOException("Output directory must be empty: " + this._status.outputDirectory);
			}
			Directory.CreateDirectory(this._status.outputDirectory);
			this._outputReady = true;
			this._status.mapResidencyPilot = MapResidencyPilotSelection.Mode;
			this._status.mapResidencyCooldownSeconds = 30.0;
			this._status.batchMode = Application.isBatchMode;
			this._status.hudComparison = RunSoakHudComparison.Requested(args);
			if (this._status.hudComparison)
			{
				this._status.version = "RunSoakHudComparison/v1";
				this._status.limitation = "HUD comparison v1: stationary ABBA ABBA; A=all three gameplay HUD settings on, B=off. Every block has at least 3 seconds of completed settling frames and 12 seconds of completed measured frames. Only GameSettingsService.SetShowFaceCamera/SetShowMinimap/SetShowDistanceAndScore are invoked. These APIs save preferences, so launch only with an isolated diagnostic product/profile; original values are restored on normal cleanup. Restoration covers these three boolean values, not original key absence or the entire preference store. An OS kill can prevent restoration. Actual PlayerFaceCam render callbacks are observed, never assumed from its enabled flag. frames.csv provides CPU/GPU/frame/GC measurements; memory.csv provides OS private/working-set and Unity allocated/reserved bytes. Join hud_frames.csv to frames.csv by completed source-frame number; phase IDs 101..116 label settling/measured windows. Generic recorder phase summaries 0..4 do not apply; use hud_protocol.json windows and raw phase IDs. NPC animation, weather and gameplay remain live; ABBA ordering reduces but cannot eliminate drift. This is a short paired HUD-toggle comparison, not evidence of a long-term memory plateau or leak removal.";
			}
			if (this._status.batchMode)
			{
				throw new InvalidOperationException("RunSoakBenchmark/v2 requires a normal rendered player window; -batchmode is rejected.");
			}
			this.ConfigureOptIns(args);
			this._status.durationSeconds = RunSoakStandaloneBenchmark.Number(args, "-runSoakSeconds", (double)(this.HasPoiRoute ? 240 : 600));
			this._status.speedMetresPerSecond = RunSoakStandaloneBenchmark.Number(args, "-runSoakSpeed", this.HasPoiRoute ? 15.37 : 20.0);
			this._status.watchdogSeconds = RunSoakStandaloneBenchmark.Number(args, "-runSoakWatchdogSeconds", (double)(1800 * this.RunCount));
			this._status.warmupStableSeconds = RunSoakStandaloneBenchmark.Number(args, "-runSoakWarmupStableSeconds", 5.0);
			this._status.warmupTimeoutSeconds = RunSoakStandaloneBenchmark.Number(args, "-runSoakWarmupTimeoutSeconds", 90.0);
			string text = RunSoakStandaloneBenchmark.Argument(args, "-runSoakSeed");
			if (text != null)
			{
				this._status.requestedSeed = int.Parse(text, CultureInfo.InvariantCulture);
			}
			this._status.standingBeforeSeconds = RunSoakStandaloneBenchmark.Number(args, "-runSoakStandingBeforeSeconds", Math.Min(60.0, this._status.durationSeconds * 0.1));
			this._status.standingAfterSeconds = RunSoakStandaloneBenchmark.Number(args, "-runSoakStandingAfterSeconds", Math.Min(60.0, this._status.durationSeconds * 0.1));
			this._status.travelSeconds = this._status.durationSeconds - this._status.standingBeforeSeconds - this._status.standingAfterSeconds;
			if (this._status.durationSeconds < 30.0 || this._status.durationSeconds + this._status.warmupTimeoutSeconds + 1.0 > 86400.0 || this._status.speedMetresPerSecond <= 0.0 || this._status.speedMetresPerSecond > 100.0 || (!this.HasPoiRoute && (this._status.standingBeforeSeconds < 3.0 || this._status.standingAfterSeconds < 10.0 || this._status.travelSeconds < 10.0)) || this._status.warmupStableSeconds < 1.0 || this._status.warmupTimeoutSeconds < this._status.warmupStableSeconds || this._status.warmupTimeoutSeconds > 600.0 || this._status.watchdogSeconds < (double)this.RunCount * (this._status.durationSeconds + 2.0 * this._status.warmupTimeoutSeconds + 60.0))
			{
				throw new ArgumentOutOfRangeException("Require before>=3s, after>=10s, travel>=10s, warmup1..600s, speed(0,100], total capture<=86400s and watchdog>=runs*(measured+2*warmupTimeout+60s).");
			}
			if (this._status.hudComparison)
			{
				this._status.durationSeconds = 120.0;
				this._status.standingBeforeSeconds = (this._status.standingAfterSeconds = (this._status.travelSeconds = 0.0));
				this._status.speedMetresPerSecond = 0.0;
			}
			if (this.HasPoiRoute)
			{
				this._status.standingBeforeSeconds = (this._status.standingAfterSeconds = (this._status.travelSeconds = 0.0));
				this._status.limitation = "Standalone synthetic player translation at the configured speed cap; not real bicycle physics or proof of maximum vehicle speed. The shipped NPC configuration, rendering, streaming, retirement and residency cooldown remain active and unchanged. A read-only bounded POI preview runs before capture; the route verifies the real selected-family proxy and completes only after the requested observed retire/revisit cycles. The Park Resources baseline may reuse a fully dormant pooled clone; selected Addressables routes require target-address release, clone/native destruction and recreation. Timeouts and unavailable targets fail comparison. The same executable, seed, route coordinates, graphics and profile must be used for A/B. Release player process memory uses Windows PSAPI; unavailable profiler counters are reported as unavailable. Phase keypoints take brief memory samples during capture without full object dumps. Normal startup may write its usual preferences and saves.";
			}
			if (NPCZoneManager.Instance != null)
			{
				throw new InvalidOperationException("NPC manager already awake; stress configuration is too late.");
			}
			this._config = Resources.Load<NPCSpawnConfig>("NPCSpawnConfig");
			if (this._config == null)
			{
				throw new InvalidOperationException("NPCSpawnConfig resource is missing.");
			}
			this._status.originalNpcConfig = JsonUtility.ToJson(this._config);
			if (this._status.shippedNpcConfig)
			{
				this.RecordShippedNpcConfig();
			}
			else
			{
				this._config.hideFlags |= HideFlags.DontUnloadUnusedAsset;
				this._config.maxTotalNpc = 80;
				this._config.desiredActiveNpc = 60;
				this._config.desiredAmbientNpc = 16;
				this._config.prePoolSize = 80;
				this._config.maxSpawnsPerRebalance = 16;
				this._config.useTimeOfDayPopulation = false;
				this._config.nearRadius = 55f;
				this._config.ambientRadius = 60f;
				this._config.rebalanceInterval = 0.2f;
				this._config.retirementsPerRebalance = 8;
				this._status.configuredActiveRadius = this._config.activeRadius;
				if (this._config.activeRadius < this._config.ambientRadius)
				{
					throw new InvalidOperationException("Existing active radius must cover the stress bands; harness does not increase simulation radius.");
				}
			}
			this._status.appliedNpcConfig = JsonUtility.ToJson(this._config);
			if (this._status.shippedNpcConfig)
			{
				this._config = null;
			}
			ChunkManager.DiagnosticSessionSeedOverride = new int?(this._status.requestedSeed);
			Random.InitState(this._status.requestedSeed);
			Application.logMessageReceived += this.OnLog;
			this._status.phase = "awaiting_menu";
			this.WriteProtocol();
			this._configuredProtocol = JsonUtility.ToJson(this._status);
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x060049EC RID: 18924 RVA: 0x001C7354 File Offset: 0x001C5554
		private int RunCount
		{
			get
			{
				return Math.Max(1, this._status.requestedCycles);
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x060049ED RID: 18925 RVA: 0x001C7367 File Offset: 0x001C5567
		private bool HasPoiRoute
		{
			get
			{
				return PoiResidencyCatalog.IsFamily(this._status.poi);
			}
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x001C737C File Offset: 0x001C557C
		private void ConfigureOptIns(string[] args)
		{
			this._status.poi = MapPoiStreamingRoute.ParseFamilyOptIn(RunSoakStandaloneBenchmark.Argument(args, "-runSoakPoi"));
			string text = RunSoakStandaloneBenchmark.Argument(args, "-runSoakRoundTrips");
			if (text != null && !this.HasPoiRoute)
			{
				throw new ArgumentException("-runSoakRoundTrips requires -runSoakPoi and an exact POI family.");
			}
			this._status.requestedRoundTrips = (this.HasPoiRoute ? ((text == null) ? 1 : int.Parse(text, NumberStyles.Integer, CultureInfo.InvariantCulture)) : 0);
			if (this.HasPoiRoute && (this._status.requestedRoundTrips < 1 || this._status.requestedRoundTrips > 3))
			{
				throw new ArgumentOutOfRangeException("-runSoakRoundTrips", "Require 1..3 observed POI round trips.");
			}
			if (this.HasPoiRoute && this._status.hudComparison)
			{
				throw new ArgumentException("-runSoakPoi cannot be combined with -runSoakHudComparison.");
			}
			this._status.shippedNpcConfig = this.HasPoiRoute || Array.IndexOf<string>(args, "-runSoakShippedNpc") >= 0;
			this._status.memoryDumps = Array.IndexOf<string>(args, "-runSoakMemoryDump") >= 0;
			string text2 = RunSoakStandaloneBenchmark.Argument(args, "-runSoakCycles");
			if (text2 != null)
			{
				this._status.requestedCycles = int.Parse(text2, NumberStyles.Integer, CultureInfo.InvariantCulture);
				if (this._status.requestedCycles < 1 || this._status.requestedCycles > 50)
				{
					throw new ArgumentOutOfRangeException("-runSoakCycles", "Require 1..50 Home->Game->Home cycles.");
				}
			}
			this._status.homeSettleSeconds = RunSoakStandaloneBenchmark.Number(args, "-runSoakHomeSettleSeconds", this._status.homeSettleSeconds);
			if (this._status.homeSettleSeconds < 0.0 || this._status.homeSettleSeconds > 600.0)
			{
				throw new ArgumentOutOfRangeException("-runSoakHomeSettleSeconds", "Require 0..600 seconds at Home.");
			}
			if (this._status.hudComparison && (this._status.shippedNpcConfig || this._status.requestedCycles > 0))
			{
				throw new ArgumentException("-runSoakHudComparison cannot be combined with -runSoakShippedNpc or -runSoakCycles.");
			}
			this._status.profilerSupported = Profiler.supported;
			this._status.developmentBuild = Debug.isDebugBuild;
			if (this._status.memoryDumps && !this._status.profilerSupported)
			{
				Debug.LogWarning("[RunSoakStandalone] -runSoakMemoryDump without profiler support (non-Development player): per-object sizes are 0; object lists, counts and allocator totals are still written.");
			}
			if (this._status.shippedNpcConfig)
			{
				RunSoakStandaloneBenchmark.Protocol status = this._status;
				status.version += "+shipped-npc";
			}
			if (this.HasPoiRoute)
			{
				RunSoakStandaloneBenchmark.Protocol status2 = this._status;
				status2.version = status2.version + "+poi-route-" + this._status.poi;
			}
			if (this._status.requestedCycles > 0)
			{
				RunSoakStandaloneBenchmark.Protocol status3 = this._status;
				status3.version += "+cycles";
				RunSoakStandaloneBenchmark.Protocol status4 = this._status;
				status4.limitation += " Cycles: every Home visit, the first included, settles for homeSettleSeconds before its keypoint; each run hands back the player controls the harness froze and leaves through GameOverManager.ReturnToHome (the Try Again path, no recap awards); run_NN folders hold each run's capture and protocol.";
			}
			if (this._status.memoryDumps)
			{
				RunSoakStandaloneBenchmark.Protocol status5 = this._status;
				status5.version += "+memory-dump";
				RunSoakStandaloneBenchmark.Protocol status6 = this._status;
				status6.limitation += " Memory dumps stall the main thread at keypoints only, after the keypoint sample; use dump runs for attribution, not for A/B commit figures.";
			}
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x001C768C File Offset: 0x001C588C
		private void RecordShippedNpcConfig()
		{
			this._status.requestedNpcTarget = 0;
			this._status.populationThreshold = 0;
			this._status.configuredMaximum = this._config.maxTotalNpc;
			this._status.configuredNear = this._config.desiredActiveNpc;
			this._status.configuredAmbient = this._config.desiredAmbientNpc;
			this._status.configuredPrepool = this._config.prePoolSize;
			this._status.configuredMaxSpawnsPerRebalance = this._config.maxSpawnsPerRebalance;
			this._status.configuredRetirementsPerRebalance = this._config.retirementsPerRebalance;
			this._status.configuredNearRadius = this._config.nearRadius;
			this._status.configuredAmbientRadius = this._config.ambientRadius;
			this._status.configuredRebalanceInterval = this._config.rebalanceInterval;
			this._status.configuredTimeOfDayPopulation = this._config.useTimeOfDayPopulation;
			this._status.configuredActiveRadius = this._config.activeRadius;
			this._status.limitation = "Shipped NPCSpawnConfig: no in-memory NPC override, no NPC workload gate; the population warmups are fixed settle windows. " + this._status.limitation;
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x001C77C4 File Offset: 0x001C59C4
		private void Update()
		{
			if (this._finished)
			{
				return;
			}
			if (this._wall.Elapsed.TotalSeconds > this._status.watchdogSeconds)
			{
				this.Fail("Total wall-clock watchdog expired in phase " + this._status.phase, 3);
				return;
			}
			if (this._status.running)
			{
				return;
			}
			if (this._entryStep == 3)
			{
				try
				{
					this.WarmInitialPopulation();
				}
				catch (Exception ex)
				{
					string text = "Initial population warmup failed: ";
					Exception ex2 = ex;
					this.Fail(text + ((ex2 != null) ? ex2.ToString() : null), 1);
				}
				return;
			}
			if (Time.realtimeSinceStartupAsDouble < this._nextPoll)
			{
				return;
			}
			this._nextPoll = Time.realtimeSinceStartupAsDouble + 0.25;
			try
			{
				string name = SceneManager.GetActiveScene().name;
				if (string.IsNullOrEmpty(this._status.initialScene))
				{
					this._status.initialScene = name;
				}
				if (!(LoadingScreenController.ExistingInstance != null) || !LoadingScreenController.ExistingInstance.IsLoadingActive)
				{
					if (!(SceneTransitionManager.Instance != null) || !SceneTransitionManager.Instance.IsTransitioning)
					{
						if (this._entryStep == 0 && name == "MainMenu")
						{
							MainMenuController mainMenuController = Object.FindObjectOfType<MainMenuController>();
							MainMenuPresentationDirector mainMenuPresentationDirector = Object.FindObjectOfType<MainMenuPresentationDirector>();
							MainMenuEntranceAnimator mainMenuEntranceAnimator = Object.FindObjectOfType<MainMenuEntranceAnimator>();
							if (!(mainMenuController == null) && !(mainMenuPresentationDirector == null) && mainMenuPresentationDirector.IsPresentationReady && (!(mainMenuEntranceAnimator != null) || mainMenuEntranceAnimator.IsComplete))
							{
								this.Keypoint("main_menu_ready", true);
								this._entryStep = 1;
								this._status.phase = "awaiting_home";
								this.WriteProtocol();
								mainMenuController.OnNewGameClicked();
							}
						}
						else if (this._entryStep == 1 && name == "CielHome" && SceneBootLoader.CoreReady && GameSystemsBootstrapper.GameplayReady)
						{
							HomeManager homeManager = Object.FindObjectOfType<HomeManager>();
							if (!(homeManager == null))
							{
								if (this._status.requestedCycles <= 0 || this.SettledAtHome())
								{
									RunSoakStandaloneBenchmark.MemorySample memorySample = this.Keypoint((this._cycle == null) ? "home_ready" : "home_after_run", true);
									if (this._cycle == null || this.CompleteCycleAtHome(memorySample))
									{
										this.BeginRun();
										this._entryStep = 2;
										this._status.phase = "awaiting_gameplay";
										this.WriteProtocol();
										string text2;
										if (!homeManager.TryStartGame(null, out text2))
										{
											throw new InvalidOperationException("Home could not start the run: " + text2);
										}
									}
								}
							}
						}
						else if (this._entryStep == 2 && name == "Game - Current" && GameSystemsBootstrapper.GameplayReady)
						{
							this.BeginCapture();
						}
						else if (this._entryStep == 4)
						{
							this.ReturnHome();
						}
					}
				}
			}
			catch (Exception ex3)
			{
				string text3 = "Startup failed: ";
				Exception ex4 = ex3;
				this.Fail(text3 + ((ex4 != null) ? ex4.ToString() : null), 1);
			}
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x001C7AE4 File Offset: 0x001C5CE4
		private void BeginCapture()
		{
			this._player = PlayerRegistry.Player;
			this._zone = NPCZoneManager.Instance;
			ChunkManager instance = ChunkManager.Instance;
			if (this._player == null || this._zone == null || instance == null || !instance.IsMapReady || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0f || Time.timeScale <= 0f)
			{
				return;
			}
			if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D11)
			{
				throw new InvalidOperationException("Rendered D3D11 required; use -force-d3d11 without -nographics.");
			}
			if (!this._status.shippedNpcConfig && (this._zone.MaxNPCCount != 80 || this._zone.DesiredActiveNpc != 60 || this._zone.DesiredAmbientNpc != 16))
			{
				throw new InvalidOperationException("NPC managers did not retain the requested startup stress configuration.");
			}
			this._status.seedMatchesRequest = instance.TryGetSessionSeed(out this._status.actualSeed) && this._status.actualSeed == this._status.requestedSeed;
			if (!this._status.seedMatchesRequest)
			{
				throw new InvalidOperationException("Map seed does not match the requested benchmark seed.");
			}
			this._routePosition = (this._status.startLogicalPosition = WorldOrigin.ToLogical(this._player.position));
			Camera main = Camera.main;
			if (main != null)
			{
				this._status.cameraStartPosition = main.transform.position;
				this._status.cameraStartRotation = main.transform.rotation;
				this._status.cameraFieldOfView = main.fieldOfView;
			}
			this._input = this._player.GetComponent<vThirdPersonInput>();
			this._controller = this._player.GetComponent<vThirdPersonController>();
			this._character = this._player.GetComponent<global::CharacterController>();
			this._body = this._player.GetComponent<Rigidbody>();
			this._inputWasEnabled = this._input != null && this._input.enabled;
			this._controllerWasEnabled = this._controller != null && this._controller.enabled;
			this._characterWasEnabled = this._character != null && this._character.enabled;
			this._bodyWasKinematic = this._body != null && this._body.isKinematic;
			this._bodyUsedGravity = this._body != null && this._body.useGravity;
			if (this._input != null)
			{
				this._input.enabled = false;
			}
			if (this._controller != null)
			{
				this._controller.enabled = false;
			}
			if (this._character != null)
			{
				this._character.enabled = false;
			}
			if (this._body != null)
			{
				this._body.velocity = Vector3.zero;
				this._body.angularVelocity = Vector3.zero;
				this._body.isKinematic = true;
				this._body.useGravity = false;
			}
			if (!this._status.shippedNpcConfig)
			{
				this._zone.SetDensityMultiplier(1f);
				this._zone.SetPopulationTargets(60, 80);
			}
			this._status.effectiveNpcTargetAtStart = this._zone.EffectiveNearTarget;
			this._status.effectiveAmbientTargetAtStart = this._zone.EffectiveAmbientTarget;
			this._player.rotation = Quaternion.LookRotation(Vector3.forward);
			if (this._body != null)
			{
				this._body.rotation = this._player.rotation;
			}
			this._sceneHandle = SceneManager.GetActiveScene().handle;
			this._entryStep = 3;
			this._stableSince = -1.0;
			this._status.phase = "initial_visual_capture";
			this.WriteProtocol();
			this.Keypoint("gameplay_ready", true);
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x001C7ED4 File Offset: 0x001C60D4
		private bool GameplayContextValid()
		{
			return this._player != null && this._zone != null && ChunkManager.Instance != null && SceneManager.GetActiveScene().handle == this._sceneHandle && GameSystemsBootstrapper.GameplayReady && Time.timeScale > 0f && !NPCSimulationPause.IsPaused && NPCSimulationPause.SimulationScale > 0f;
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x001C7F48 File Offset: 0x001C6148
		private bool PopulationStabilized(double now)
		{
			if (this._zone.ActiveNPCCount < this._status.requestedNpcTarget)
			{
				this._stableSince = -1.0;
				return false;
			}
			if (this._stableSince < 0.0)
			{
				this._stableSince = now;
			}
			return now - this._stableSince >= this._status.warmupStableSeconds;
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x001C7FB0 File Offset: 0x001C61B0
		private void WarmInitialPopulation()
		{
			if (!this.GameplayContextValid())
			{
				this.Fail("Gameplay context invalid during initial population warmup.", 2);
				return;
			}
			double totalSeconds = this._wall.Elapsed.TotalSeconds;
			bool flag = this.EnsureMainCameraRendered();
			if (!this.CompletePreflightScreenshot(totalSeconds, flag))
			{
				return;
			}
			if (this._initialWarmupStarted < 0.0)
			{
				this._initialWarmupStarted = totalSeconds;
				this._stableSince = -1.0;
				this._status.phase = "initial_population_warmup";
				this.WriteProtocol();
			}
			this._status.initialPopulationWarmupSeconds = totalSeconds - this._initialWarmupStarted;
			if (this._status.initialPopulationWarmupSeconds >= this._status.warmupTimeoutSeconds)
			{
				this.Fail("Initial population did not stabilize at 60 active NPCs within its independent timeout.", 2);
				return;
			}
			if (this.PopulationStabilized(totalSeconds) && flag && totalSeconds - this._status.screenshotCompletedWallSeconds >= 5.0)
			{
				this._status.initialPopulationStable = true;
				this.StartCapture();
			}
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x001C80A4 File Offset: 0x001C62A4
		private bool CompletePreflightScreenshot(double now, bool renderingReady)
		{
			if (this._status.preflightScreenshotComplete)
			{
				return true;
			}
			if (this._status.screenshotRequestedWallSeconds < 0.0)
			{
				if (!renderingReady)
				{
					return false;
				}
				this._status.preflightScreenshotPath = Path.Combine(this._status.runOutputDirectory, "gameplay_pre_capture.png");
				if (File.Exists(this._status.preflightScreenshotPath))
				{
					throw new IOException("Refusing to overwrite preflight screenshot: " + this._status.preflightScreenshotPath);
				}
				this._status.screenshotRequestedWallSeconds = now;
				ScreenCapture.CaptureScreenshot(this._status.preflightScreenshotPath, 1);
				this.WriteProtocol();
				return false;
			}
			else
			{
				if (now - this._status.screenshotRequestedWallSeconds >= 30.0)
				{
					throw new IOException("Gameplay screenshot did not become a complete PNG within 30 seconds: " + this._status.preflightScreenshotPath);
				}
				if (now < this._nextScreenshotPoll)
				{
					return false;
				}
				this._nextScreenshotPoll = now + 0.25;
				long num;
				int num2;
				int num3;
				if (!RunSoakStandaloneBenchmark.TryReadCompletedPng(this._status.preflightScreenshotPath, out num, out num2, out num3))
				{
					return false;
				}
				this._status.preflightScreenshotBytes = num;
				this._status.preflightScreenshotWidth = num2;
				this._status.preflightScreenshotHeight = num3;
				this._status.screenshotCompletedWallSeconds = this._wall.Elapsed.TotalSeconds;
				this._status.preflightScreenshotComplete = true;
				return true;
			}
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x001C8208 File Offset: 0x001C6408
		private static bool TryReadCompletedPng(string path, out long bytes, out int width, out int height)
		{
			bytes = 0L;
			width = (height = 0);
			if (!File.Exists(path))
			{
				return false;
			}
			bool flag;
			try
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					if (fileStream.Length < 45L)
					{
						flag = false;
					}
					else
					{
						byte[] array = new byte[24];
						byte[] array2 = new byte[12];
						if (fileStream.Read(array, 0, array.Length) != array.Length)
						{
							flag = false;
						}
						else
						{
							byte[] array3 = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
							for (int i = 0; i < array3.Length; i++)
							{
								if (array[i] != array3[i])
								{
									return false;
								}
							}
							if (array[8] != 0 || array[9] != 0 || array[10] != 0 || array[11] != 13 || array[12] != 73 || array[13] != 72 || array[14] != 68 || array[15] != 82)
							{
								flag = false;
							}
							else
							{
								fileStream.Seek(-12L, SeekOrigin.End);
								if (fileStream.Read(array2, 0, array2.Length) != array2.Length)
								{
									flag = false;
								}
								else
								{
									byte[] array4 = new byte[]
									{
										0, 0, 0, 0, 73, 69, 78, 68, 174, 66,
										96, 130
									};
									for (int j = 0; j < array4.Length; j++)
									{
										if (array2[j] != array4[j])
										{
											return false;
										}
									}
									width = ((int)array[16] << 24) | ((int)array[17] << 16) | ((int)array[18] << 8) | (int)array[19];
									height = ((int)array[20] << 24) | ((int)array[21] << 16) | ((int)array[22] << 8) | (int)array[23];
									if (width <= 0 || height <= 0)
									{
										flag = false;
									}
									else
									{
										bytes = fileStream.Length;
										flag = true;
									}
								}
							}
						}
					}
				}
			}
			catch (IOException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x001C83E4 File Offset: 0x001C65E4
		private bool EnsureMainCameraRendered()
		{
			double totalSeconds = this._wall.Elapsed.TotalSeconds;
			if (this._renderLastReady < 0.0)
			{
				this._renderLastReady = totalSeconds;
			}
			Camera main = Camera.main;
			if (this._renderPreflight == null && main != null && main.cameraType == CameraType.Game)
			{
				this._renderPreflight = new RunSoakRenderGuard(main);
			}
			if (this._renderPreflight != null && this._renderPreflight.IsReady(3, Time.frameCount))
			{
				this._status.preflightRenderedMainCameraFrames = this._renderPreflight.Frames.UniqueRenderedFrames;
				this._renderLastReady = totalSeconds;
				return true;
			}
			if (this._renderPreflight != null && this._renderPreflight.CameraIdentityChanged)
			{
				throw new InvalidOperationException("The main camera changed during render/population preflight.");
			}
			if (totalSeconds - this._renderLastReady >= 15.0)
			{
				throw new InvalidOperationException("No three consecutive completed Camera.main renders within 15 seconds; a D3D11 device alone is insufficient.");
			}
			return false;
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x001C84C8 File Offset: 0x001C66C8
		private void StartCapture()
		{
			this._status.screenshotToCaptureSeconds = this._wall.Elapsed.TotalSeconds - this._status.screenshotCompletedWallSeconds;
			if (!this._status.preflightScreenshotComplete || this._status.screenshotToCaptureSeconds < 5.0)
			{
				return;
			}
			if (!this.EnsureMainCameraRendered())
			{
				return;
			}
			if (this.HasPoiRoute && this._poiRoute == null)
			{
				MapPoiStreamingRoute.Config config = new MapPoiStreamingRoute.Config
				{
					family = this._status.poi,
					expectedSeed = this._status.requestedSeed,
					roundTrips = this._status.requestedRoundTrips,
					speedMetresPerSecond = (float)this._status.speedMetresPerSecond,
					maximumSeconds = this._status.durationSeconds,
					requireAddressablePayload = MapResidencyPilotSelection.IsFamilyEnabled(this._status.poi, this._status.mapResidencyPilot)
				};
				if (this._poiLocator == null)
				{
					this._poiLocator = new MapPoiStreamingRoute.Locator(ChunkManager.Instance, this._status.startLogicalPosition, config);
				}
				this._poiLocator.Advance(128);
				if (!this._poiLocator.IsDone)
				{
					this._status.phase = "locating_" + this._status.poi + "_before_capture";
					return;
				}
				if (this._poiLocator.Result == null)
				{
					throw new InvalidOperationException(this._status.poi + " route unavailable: " + this._poiLocator.Failure);
				}
				this._poiRoute = new MapPoiStreamingRoute(this._player, this._body, ChunkManager.Instance, this._poiLocator.Result, config, delegate(string label)
				{
					this.Keypoint(label, false);
				});
				this._status.poiRoute = this._poiRoute.Status;
			}
			Camera main = Camera.main;
			this._status.cameraStartPosition = main.transform.position;
			this._status.cameraStartRotation = main.transform.rotation;
			this._status.cameraFieldOfView = main.fieldOfView;
			this._recorder = RunSoakRecorder.Begin(new RunSoakRecorder.Options
			{
				outputDirectory = this._status.runOutputDirectory,
				durationSeconds = (this._status.hudComparison ? 150.0 : (this.HasPoiRoute ? (this._status.durationSeconds + 1.0) : (this._status.durationSeconds + this._status.warmupTimeoutSeconds + 1.0))),
				npcStressThreshold = this._status.populationThreshold,
				requireMainCameraRendering = true,
				protocol = (this._status.hudComparison ? "HUD comparison v1: stationary ABBA ABBA; A=all three gameplay HUD settings on, B=off. Every block has at least 3 seconds of completed settling frames and 12 seconds of completed measured frames. Only GameSettingsService.SetShowFaceCamera/SetShowMinimap/SetShowDistanceAndScore are invoked. These APIs save preferences, so launch only with an isolated diagnostic product/profile; original values are restored on normal cleanup. Restoration covers these three boolean values, not original key absence or the entire preference store. An OS kill can prevent restoration. Actual PlayerFaceCam render callbacks are observed, never assumed from its enabled flag. frames.csv provides CPU/GPU/frame/GC measurements; memory.csv provides OS private/working-set and Unity allocated/reserved bytes. Join hud_frames.csv to frames.csv by completed source-frame number; phase IDs 101..116 label settling/measured windows. Generic recorder phase summaries 0..4 do not apply; use hud_protocol.json windows and raw phase IDs. NPC animation, weather and gameplay remain live; ABBA ordering reduces but cannot eliminate drift. This is a short paired HUD-toggle comparison, not evidence of a long-term memory plateau or leak removal." : (this.HasPoiRoute ? this._status.limitation : "RunSoakBenchmark/v2; standalone; standing, continuous logical +Z route, population settling, standing; actual NPC coverage gates"))
			});
			this._renderPreflight.Dispose();
			this._renderPreflight = null;
			this._recorder.RuntimeTick += this.Tick;
			this._recorder.Stopped += this.OnStopped;
			this._status.startupSeconds = this._wall.Elapsed.TotalSeconds;
			this._status.running = true;
			this._status.phase = "standing_before";
			if (this.HasPoiRoute)
			{
				this._status.phase = this._poiRoute.Phase;
			}
			if (this._status.hudComparison)
			{
				this._hudComparison = base.gameObject.AddComponent<RunSoakHudComparison>();
				this._hudComparison.Initialize(this._recorder, this._player);
				this._status.phase = this._hudComparison.CurrentLabel;
			}
			this.WriteProtocol();
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x001C889C File Offset: 0x001C6A9C
		private void Tick(RunSoakRecorder recorder)
		{
			if (!this.GameplayContextValid())
			{
				recorder.Stop("invalid", "Gameplay context changed or simulation paused during capture.");
				return;
			}
			double elapsed = recorder.Elapsed;
			double num = Math.Max(0.0, elapsed - this._lastTick);
			this._lastTick = elapsed;
			this._status.elapsedSeconds = elapsed;
			this._status.maximumObservedNpcs = Math.Max(this._status.maximumObservedNpcs, this._zone.CurrentNPCCount);
			this._status.maximumObservedActiveNpcs = Math.Max(this._status.maximumObservedActiveNpcs, this._zone.ActiveNPCCount);
			if (this._zone.ActiveNPCCount >= this._status.requestedNpcTarget)
			{
				this._status.secondsAtRequestedActivePopulation += num;
			}
			if (!ChunkManager.Instance.HasChunkUnderPlayer)
			{
				this._status.framesWithoutCurrentChunk++;
			}
			if (this.HasPoiRoute)
			{
				if (Time.timeScale != 1f)
				{
					recorder.Stop("invalid", "POI comparison requires unchanged normal simulation speed.");
					return;
				}
				this._poiRoute.Tick(recorder);
				this._status.actualDistanceMetres = recorder.DistanceMetres;
				this._status.scheduledTravelMetres = this._poiRoute.ScheduledDistance;
				if (recorder.IsRecording)
				{
					this._status.phase = this._poiRoute.Phase;
				}
				return;
			}
			else
			{
				if (this._status.hudComparison)
				{
					this._status.actualDistanceMetres = recorder.DistanceMetres;
					this._hudComparison.Tick(recorder);
					if (recorder.IsRecording)
					{
						this._status.phase = this._hudComparison.CurrentLabel;
					}
					return;
				}
				if (elapsed < this._status.standingBeforeSeconds)
				{
					recorder.Phase = 1;
					this._status.phase = "standing_before";
				}
				else if (elapsed < this._status.durationSeconds - this._status.standingAfterSeconds)
				{
					recorder.Phase = 2;
					this._status.phase = ((elapsed < this._holdUntil) ? "checkpoint_settling" : "synthetic_travel");
					if (elapsed >= this._holdUntil)
					{
						double num2 = this._status.speedMetresPerSecond * Math.Min(num, 0.25);
						this._routePosition.z = this._routePosition.z + (float)num2;
						this._routeDistance += num2;
						if (this._routeDistance >= this._nextCheckpoint)
						{
							this._holdUntil = elapsed + 5.0;
							this._nextCheckpoint += 1000.0;
							this._status.checkpoints++;
						}
					}
				}
				else if (!this._status.finalPopulationStable)
				{
					recorder.Phase = 4;
					this._status.phase = "final_population_warmup";
					if (this._finalWarmupStarted < 0.0)
					{
						this._finalWarmupStarted = elapsed;
						this._stableSince = -1.0;
					}
					this._status.finalPopulationWarmupSeconds = elapsed - this._finalWarmupStarted;
					if (this._status.finalPopulationWarmupSeconds >= this._status.warmupTimeoutSeconds)
					{
						recorder.Stop("invalid", "Final population did not stabilize at 60 active NPCs within its independent timeout.");
						return;
					}
					if (this.PopulationStabilized(elapsed))
					{
						this._status.finalPopulationStable = true;
						this._status.finalMeasuredStartSeconds = elapsed;
						recorder.Phase = 3;
						this._status.phase = "standing_after";
					}
				}
				else
				{
					recorder.Phase = 3;
					this._status.phase = "standing_after";
					if (elapsed - this._status.finalMeasuredStartSeconds >= this._status.standingAfterSeconds)
					{
						recorder.Stop("completed", "V2 measured phases and independent population warmups completed.");
						return;
					}
				}
				Vector3 vector = WorldOrigin.ToUnity(this._routePosition);
				this._player.SetPositionAndRotation(vector, Quaternion.LookRotation(Vector3.forward));
				if (this._body != null)
				{
					this._body.position = vector;
					this._body.rotation = this._player.rotation;
				}
				WorldOrigin.TryCenterAround(vector, false);
				this._status.actualDistanceMetres = recorder.DistanceMetres;
				this._status.scheduledTravelMetres = this._routeDistance;
				return;
			}
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x001C8CD0 File Offset: 0x001C6ED0
		private void OnStopped(RunSoakRecorder recorder)
		{
			if (this.HasPoiRoute)
			{
				this.RestorePlayerControl();
			}
			this._status.running = false;
			this._status.outcome = recorder.LastReport.outcome;
			this._status.reason = recorder.LastReport.reason;
			this._status.mainCameraRenderingValid = recorder.LastReport.mainCameraRenderingValid;
			this._status.completed = this._status.outcome == "completed";
			this._status.elapsedSeconds = recorder.LastReport.elapsedSeconds;
			this._status.actualDistanceMetres = recorder.DistanceMetres;
			this._status.endLogicalPosition = ((this._player != null) ? WorldOrigin.ToLogical(this._player.position) : this._routePosition);
			this._status.finalActiveNpcs = ((this._zone != null) ? this._zone.ActiveNPCCount : (-1));
			this._status.finalNearNpcs = ((this._zone != null) ? this._zone.CurrentNearCount : (-1));
			this._status.finalAmbientNpcs = ((this._zone != null) ? this._zone.CurrentAmbientCount : (-1));
			if (this.HasPoiRoute)
			{
				try
				{
					if (this._poiRoute == null)
					{
						throw new InvalidOperationException("POI route was not initialized.");
					}
					this._poiRoute.Complete(recorder);
					this._status.poiRoute = this._poiRoute.Status;
					if (!this._poiRoute.Completed && this._status.completed)
					{
						this._status.completed = false;
						this._status.outcome = "incomplete";
					}
					this._status.reason = this._poiRoute.Reason;
					goto IL_038D;
				}
				catch (Exception ex)
				{
					this._status.completed = false;
					this._status.outcome = "export_failed";
					RunSoakStandaloneBenchmark.Protocol status = this._status;
					string text = "POI route finalization failed: ";
					Exception ex2 = ex;
					status.reason = text + ((ex2 != null) ? ex2.ToString() : null);
					goto IL_038D;
				}
			}
			if (this._status.hudComparison && this._hudComparison != null)
			{
				try
				{
					this._hudComparison.Complete(recorder);
					this._status.heavyNpcTargetObserved = this._hudComparison.PopulationPassed;
					this._status.reason = this._hudComparison.Reason;
					goto IL_038D;
				}
				catch (Exception ex3)
				{
					this._status.completed = false;
					this._status.outcome = "export_failed";
					RunSoakStandaloneBenchmark.Protocol status2 = this._status;
					string text2 = "HUD comparison finalization failed: ";
					Exception ex4 = ex3;
					status2.reason = text2 + ((ex4 != null) ? ex4.ToString() : null);
					goto IL_038D;
				}
			}
			RunSoakRecorder.PhaseReport[] phases = recorder.LastReport.phases;
			if (phases != null && phases.Length > 3)
			{
				this._status.travelPopulationCoverage = phases[2].populationCoverage;
				this._status.finalPopulationCoverage = phases[3].populationCoverage;
				this._status.travelPopulationPassed = phases[2].HasSustainedPopulation(this._status.travelSeconds, this._status.requiredPopulationCoverage);
				this._status.finalPopulationPassed = phases[3].HasSustainedPopulation(this._status.standingAfterSeconds, this._status.requiredPopulationCoverage);
			}
			this._status.heavyNpcTargetObserved = this._status.initialPopulationStable && this._status.finalPopulationStable && this._status.travelPopulationPassed && this._status.finalPopulationPassed;
			IL_038D:
			if (!this.HasPoiRoute && !this._status.hudComparison && this._status.completed && !this._status.heavyNpcTargetObserved)
			{
				this._status.reason = string.Concat(new string[]
				{
					"Capture completed, but sustained NPC workload gate failed. Travel coverage=",
					this._status.travelPopulationCoverage.ToString("P2", CultureInfo.InvariantCulture),
					"; final coverage=",
					this._status.finalPopulationCoverage.ToString("P2", CultureInfo.InvariantCulture),
					"."
				});
			}
			this._status.validForComparison = this._status.completed && recorder.LastReport.completeFrameCoverage && this._status.preflightScreenshotComplete && this._status.screenshotToCaptureSeconds >= 5.0 && recorder.LastReport.mainCameraRenderingValid && !recorder.LastReport.settingsChanged && !recorder.LastReport.sceneChanged && !recorder.LastReport.sessionSeedChanged && recorder.LastReport.applicationErrors == 0 && this._status.startupErrors == 0 && this._status.seedMatchesRequest && (this.HasPoiRoute ? (this._poiRoute != null && this._poiRoute.Completed && this._poiRoute.ValidForComparison) : this._status.heavyNpcTargetObserved) && this._status.framesWithoutCurrentChunk == 0 && (!this._status.hudComparison || (this._hudComparison != null && this._hudComparison.ValidForComparison));
			this._status.phase = (this._status.completed ? "completed" : "stopped");
			if (this._cycle != null)
			{
				this.RecordCycleRun();
			}
			this.Keypoint("run_end", true);
			if (this._cycle == null)
			{
				this.Finish((this._failureExitCode > 0) ? this._failureExitCode : (this._status.validForComparison ? 0 : 2));
				return;
			}
			if (this._failureExitCode > 0 || !this._status.completed)
			{
				this.FinishCycles();
				return;
			}
			this._entryStep = 4;
			this._status.phase = "returning_home";
			this.WriteProtocol();
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x001C9304 File Offset: 0x001C7504
		private void BeginRun()
		{
			this._runsStarted++;
			if (this._status.requestedCycles == 0)
			{
				this._status.cycle = 1;
				this._status.runOutputDirectory = this._status.outputDirectory;
				return;
			}
			if (this._runsStarted > 1)
			{
				this.ResetPerRunState();
			}
			this._status.cycle = this._runsStarted;
			this._status.runOutputDirectory = Path.Combine(this._status.outputDirectory, "run_" + this._runsStarted.ToString("00", CultureInfo.InvariantCulture));
			if (Directory.Exists(this._status.runOutputDirectory))
			{
				throw new IOException("Run output directory already exists: " + this._status.runOutputDirectory);
			}
			Directory.CreateDirectory(this._status.runOutputDirectory);
			this._cycle = new RunSoakStandaloneBenchmark.CycleResult
			{
				cycle = this._runsStarted,
				outputDirectory = this._status.runOutputDirectory,
				runStartedWallSeconds = this._wall.Elapsed.TotalSeconds
			};
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x001C9428 File Offset: 0x001C7628
		private void ResetPerRunState()
		{
			List<RunSoakStandaloneBenchmark.CycleResult> cycles = this._status.cycles;
			int completedCycles = this._status.completedCycles;
			int keypointSamples = this._status.keypointSamples;
			int memoryDumpFiles = this._status.memoryDumpFiles;
			int memoryDumpFailures = this._status.memoryDumpFailures;
			string initialScene = this._status.initialScene;
			this._status.cycles = new List<RunSoakStandaloneBenchmark.CycleResult>();
			JsonUtility.FromJsonOverwrite(this._configuredProtocol, this._status);
			this._status.cycles = cycles;
			this._status.completedCycles = completedCycles;
			this._status.keypointSamples = keypointSamples;
			this._status.memoryDumpFiles = memoryDumpFiles;
			this._status.memoryDumpFailures = memoryDumpFailures;
			this._status.initialScene = initialScene;
			this._player = null;
			this._zone = null;
			this._body = null;
			this._input = null;
			this._controller = null;
			this._character = null;
			this._poiRoute = null;
			this._poiLocator = null;
			RunSoakRenderGuard renderPreflight = this._renderPreflight;
			if (renderPreflight != null)
			{
				renderPreflight.Dispose();
			}
			this._renderPreflight = null;
			this._renderLastReady = -1.0;
			this._lastTick = (this._holdUntil = (this._routeDistance = (this._nextScreenshotPoll = 0.0)));
			this._nextCheckpoint = 1000.0;
			this._initialWarmupStarted = (this._finalWarmupStarted = (this._stableSince = -1.0));
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x001C95B0 File Offset: 0x001C77B0
		private void RecordCycleRun()
		{
			this._cycle.outcome = this._status.outcome;
			this._cycle.reason = this._status.reason;
			this._cycle.completed = this._status.completed;
			this._cycle.validForComparison = this._status.validForComparison;
			this._cycle.heavyNpcTargetObserved = this._status.heavyNpcTargetObserved;
			this._cycle.maximumObservedActiveNpcs = this._status.maximumObservedActiveNpcs;
			this._cycle.framesWithoutCurrentChunk = this._status.framesWithoutCurrentChunk;
			this._cycle.elapsedSeconds = this._status.elapsedSeconds;
			this._cycle.actualDistanceMetres = this._status.actualDistanceMetres;
			this._cycle.captureStartedWallSeconds = this._status.startupSeconds;
			this._cycle.runEndedWallSeconds = this._wall.Elapsed.TotalSeconds;
			this._status.cycles.Add(this._cycle);
			try
			{
				File.WriteAllText(Path.Combine(this._cycle.outputDirectory, "protocol.json"), JsonUtility.ToJson(this._status, true));
			}
			catch (Exception ex)
			{
				this._failureExitCode = 1;
				string text = "[RunSoakStandalone] Run protocol export failed: ";
				Exception ex2 = ex;
				Debug.LogError(text + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x001C972C File Offset: 0x001C792C
		private bool SettledAtHome()
		{
			double totalSeconds = this._wall.Elapsed.TotalSeconds;
			if (this._homeReadySince < 0.0)
			{
				this._homeReadySince = totalSeconds;
				this._status.phase = "home_settling";
				this.WriteProtocol();
			}
			if (totalSeconds - this._homeReadySince < this._status.homeSettleSeconds)
			{
				return false;
			}
			this._homeReadySince = -1.0;
			return true;
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x001C97A4 File Offset: 0x001C79A4
		private bool CompleteCycleAtHome(RunSoakStandaloneBenchmark.MemorySample sample)
		{
			this._cycle.homeReadyWallSeconds = this._wall.Elapsed.TotalSeconds;
			this._cycle.errorsOutsideCapture = this._status.startupErrors;
			this._cycle.homePrivateBytesAfterRun = sample.committed;
			this._cycle.homeWorkingSetBytesAfterRun = sample.working;
			this._cycle.homePeakPrivateBytesAfterRun = sample.peakCommitted;
			this._status.completedCycles++;
			Debug.Log(string.Concat(new string[]
			{
				"[RunSoakStandalone] cycle ",
				this._cycle.cycle.ToString(),
				"/",
				this._status.requestedCycles.ToString(),
				" at Home after run: commit=",
				RunSoakStandaloneBenchmark.Megabytes(sample.committed),
				" MB ws=",
				RunSoakStandaloneBenchmark.Megabytes(sample.working),
				" MB peakCommit=",
				RunSoakStandaloneBenchmark.Megabytes(sample.peakCommitted),
				" MB"
			}));
			this._cycle = null;
			if (this._status.completedCycles < this._status.requestedCycles)
			{
				return true;
			}
			this.FinishCycles();
			return false;
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x001C98E8 File Offset: 0x001C7AE8
		private void FinishCycles()
		{
			int count = this._status.cycles.FindAll((RunSoakStandaloneBenchmark.CycleResult c) => c.validForComparison && c.homePrivateBytesAfterRun > 0L).Count;
			this._status.allCyclesValidForComparison = this._failureExitCode == 0 && this._status.completedCycles == this._status.requestedCycles && count == this._status.requestedCycles;
			this._status.validForComparison = this._status.allCyclesValidForComparison;
			this._status.reason = string.Concat(new string[]
			{
				this._status.completedCycles.ToString(),
				"/",
				this._status.requestedCycles.ToString(),
				" cycles reached Home after their run, ",
				count.ToString(),
				" valid for comparison. Last run: ",
				this._status.reason
			});
			this._status.phase = (this._status.allCyclesValidForComparison ? "completed" : "stopped");
			this.Finish((this._failureExitCode > 0) ? this._failureExitCode : (this._status.allCyclesValidForComparison ? 0 : 2));
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x001C9A34 File Offset: 0x001C7C34
		private void ReturnHome()
		{
			GameOverManager instance = GameOverManager.Instance;
			if (instance == null)
			{
				throw new InvalidOperationException("GameOverManager is missing; the cycle cannot return Home.");
			}
			this.RestorePlayerControl();
			this._entryStep = 1;
			this._status.phase = "awaiting_home";
			this.WriteProtocol();
			instance.ReturnToHome();
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x001C9A84 File Offset: 0x001C7C84
		private void RestorePlayerControl()
		{
			if (this._body != null)
			{
				this._body.isKinematic = this._bodyWasKinematic;
				this._body.useGravity = this._bodyUsedGravity;
				if (!this._body.isKinematic)
				{
					this._body.velocity = Vector3.zero;
					this._body.angularVelocity = Vector3.zero;
				}
			}
			if (this._character != null)
			{
				this._character.enabled = this._characterWasEnabled;
			}
			if (this._controller != null)
			{
				this._controller.enabled = this._controllerWasEnabled;
			}
			if (this._input != null)
			{
				this._input.enabled = this._inputWasEnabled;
			}
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x001C9B4C File Offset: 0x001C7D4C
		private RunSoakStandaloneBenchmark.MemorySample Keypoint(string label, bool allowObjectDump = true)
		{
			RunSoakStandaloneBenchmark.MemorySample memorySample = default(RunSoakStandaloneBenchmark.MemorySample);
			try
			{
				memorySample = RunSoakStandaloneBenchmark.SampleMemory();
				this._status.keypointSamples++;
				string text = ((this._status.memoryDumps && allowObjectDump) ? string.Concat(new string[]
				{
					"memory_dump_",
					this._status.keypointSamples.ToString("00", CultureInfo.InvariantCulture),
					"_",
					label,
					".csv"
				}) : "");
				this.AppendKeypointRow(label, memorySample, text);
				MemoryReport.Log("soak:" + label);
				if (this._status.memoryDumps && allowObjectDump)
				{
					this.WriteMemoryDump(Path.Combine(this._status.outputDirectory, text), memorySample);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[RunSoakStandalone] Keypoint '" + label + "' failed: " + ex.Message);
			}
			return memorySample;
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x001C9C48 File Offset: 0x001C7E48
		private static RunSoakStandaloneBenchmark.MemorySample SampleMemory()
		{
			RunSoakStandaloneBenchmark.MemorySample memorySample = default(RunSoakStandaloneBenchmark.MemorySample);
			RunSoakProcessMemory.TryReadWindows(out memorySample.working, out memorySample.committed, out memorySample.peakWorking, out memorySample.peakCommitted);
			MemoryReport.TryReadSystem(out memorySample.physical, out memorySample.commitLimit, out memorySample.commitAvailable);
			memorySample.allocated = Profiler.GetTotalAllocatedMemoryLong();
			memorySample.reserved = Profiler.GetTotalReservedMemoryLong();
			memorySample.unusedReserved = Profiler.GetTotalUnusedReservedMemoryLong();
			memorySample.monoHeap = Profiler.GetMonoHeapSizeLong();
			memorySample.monoUsed = Profiler.GetMonoUsedSizeLong();
			memorySample.graphicsDriver = Profiler.GetAllocatedMemoryForGraphicsDriver();
			memorySample.gcManaged = GC.GetTotalMemory(false);
			memorySample.textureCurrent = (long)Texture.currentTextureMemory;
			memorySample.textureDesired = (long)Texture.desiredTextureMemory;
			memorySample.textureTarget = (long)Texture.targetTextureMemory;
			memorySample.textureTotal = (long)Texture.totalTextureMemory;
			memorySample.textureNonStreaming = (long)Texture.nonStreamingTextureMemory;
			ChunkManager instance = ChunkManager.Instance;
			memorySample.spawnedChunks = ((instance != null) ? instance.SpawnedChunkCount : (-1));
			memorySample.pooledChunks = ((instance != null) ? instance.AvailableChunkPoolCount : (-1));
			memorySample.zonePool = ((instance != null) ? instance.AvailableZonePoolCount : (-1));
			memorySample.zoneRetirements = ((instance != null) ? instance.PendingZoneRetirementCount : (-1));
			memorySample.catalogLoaded = ((instance != null) ? (instance.IsCatalogLoaded ? 1 : 0) : (-1));
			memorySample.activePayloadLoads = HeavyZoneStreamingProxy.ActivePayloadAssetLoadCount;
			memorySample.completedPayloadCacheEntries = HeavyZoneStreamingProxy.CompletedPayloadCacheEntryCount;
			memorySample.addressableEntries = HeavyZoneAddressableResidency.EntryCount;
			memorySample.addressableOwners = HeavyZoneAddressableResidency.ActiveLeaseCount;
			memorySample.addressableDeferredReleases = HeavyZoneAddressableResidency.DeferredReleaseCount;
			memorySample.addressableUnusedEntries = HeavyZoneAddressableResidency.CompletedUnusedEntryCount;
			memorySample.addressableLoadStarts = HeavyZoneAddressableResidency.LoadStartCount;
			memorySample.addressableReleaseRequests = HeavyZoneAddressableResidency.OperationReleaseCount;
			return memorySample;
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x001C9E18 File Offset: 0x001C8018
		private void AppendKeypointRow(string label, RunSoakStandaloneBenchmark.MemorySample sample, string dumpFile)
		{
			string text = Path.Combine(this._status.outputDirectory, "memory_keypoints.csv");
			StringBuilder stringBuilder = new StringBuilder(512);
			if (!File.Exists(text))
			{
				stringBuilder.AppendLine("utc,wall_s,frame,keypoint,cycle,scene,process_working_set_bytes,process_private_bytes,process_peak_working_set_bytes,process_peak_private_bytes,system_physical_bytes,commit_limit_bytes,commit_available_bytes,unity_allocated_bytes,unity_reserved_bytes,unity_unused_reserved_bytes,mono_used_bytes,mono_reserved_bytes,gc_managed_bytes,graphics_driver_bytes,texture_current_bytes,texture_desired_bytes,texture_target_bytes,texture_total_bytes,texture_non_streaming_bytes,memory_dump,spawned_chunks,pooled_chunks,pooled_zones,pending_zone_retirements,catalog_loaded,active_payload_asset_loads,completed_payload_cache_entries,addressable_payload_entries,addressable_payload_owners,addressable_deferred_releases,addressable_unused_entries,addressable_load_starts,addressable_release_requests");
			}
			stringBuilder.Append(DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture)).Append(',').Append(this._wall.Elapsed.TotalSeconds.ToString("F3", CultureInfo.InvariantCulture))
				.Append(',')
				.Append(Time.frameCount.ToString(CultureInfo.InvariantCulture))
				.Append(',')
				.Append(label)
				.Append(',')
				.Append(this._status.cycle.ToString(CultureInfo.InvariantCulture))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Csv(SceneManager.GetActiveScene().name));
			foreach (long num in new long[]
			{
				sample.working, sample.committed, sample.peakWorking, sample.peakCommitted, sample.physical, sample.commitLimit, sample.commitAvailable, sample.allocated, sample.reserved, sample.unusedReserved,
				sample.monoUsed, sample.monoHeap, sample.gcManaged, sample.graphicsDriver, sample.textureCurrent, sample.textureDesired, sample.textureTarget, sample.textureTotal, sample.textureNonStreaming
			})
			{
				stringBuilder.Append(',').Append(RunSoakStandaloneBenchmark.Integer(num));
			}
			stringBuilder.Append(',').Append(dumpFile).Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.spawnedChunks))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.pooledChunks))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.zonePool))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.zoneRetirements))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.catalogLoaded))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.activePayloadLoads))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.completedPayloadCacheEntries))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.addressableEntries))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.addressableOwners))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.addressableDeferredReleases))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.addressableUnusedEntries))
				.Append(',')
				.Append(RunSoakStandaloneBenchmark.Integer((long)sample.addressableLoadStarts))
				.Append(',')
				.AppendLine(RunSoakStandaloneBenchmark.Integer((long)sample.addressableReleaseRequests));
			File.AppendAllText(text, stringBuilder.ToString());
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x001CA158 File Offset: 0x001C8358
		private void WriteMemoryDump(string path, RunSoakStandaloneBenchmark.MemorySample sample)
		{
			try
			{
				Object[] array = Resources.FindObjectsOfTypeAll<Object>();
				Dictionary<Type, RunSoakStandaloneBenchmark.TypeTotal> dictionary = new Dictionary<Type, RunSoakStandaloneBenchmark.TypeTotal>();
				List<RunSoakStandaloneBenchmark.ObjectSize> list = new List<RunSoakStandaloneBenchmark.ObjectSize>(array.Length);
				long num = 0L;
				foreach (Object @object in array)
				{
					if (!(@object == null))
					{
						Type type = @object.GetType();
						RunSoakStandaloneBenchmark.TypeTotal typeTotal;
						if (!dictionary.TryGetValue(type, out typeTotal))
						{
							Dictionary<Type, RunSoakStandaloneBenchmark.TypeTotal> dictionary2 = dictionary;
							Type type2 = type;
							RunSoakStandaloneBenchmark.TypeTotal typeTotal2 = new RunSoakStandaloneBenchmark.TypeTotal();
							typeTotal2.name = type.FullName;
							typeTotal = typeTotal2;
							dictionary2.Add(type2, typeTotal2);
						}
						long runtimeMemorySizeLong = Profiler.GetRuntimeMemorySizeLong(@object);
						typeTotal.count += 1L;
						typeTotal.bytes += runtimeMemorySizeLong;
						num += runtimeMemorySizeLong;
						list.Add(new RunSoakStandaloneBenchmark.ObjectSize
						{
							type = typeTotal,
							name = @object.name,
							bytes = runtimeMemorySizeLong
						});
					}
				}
				List<RunSoakStandaloneBenchmark.TypeTotal> list2 = new List<RunSoakStandaloneBenchmark.TypeTotal>(dictionary.Values);
				list2.Sort(delegate(RunSoakStandaloneBenchmark.TypeTotal a, RunSoakStandaloneBenchmark.TypeTotal b)
				{
					if (a.bytes == b.bytes)
					{
						return string.CompareOrdinal(a.name, b.name);
					}
					return b.bytes.CompareTo(a.bytes);
				});
				list.Sort(delegate(RunSoakStandaloneBenchmark.ObjectSize a, RunSoakStandaloneBenchmark.ObjectSize b)
				{
					if (a.bytes == b.bytes)
					{
						return string.CompareOrdinal(a.type.name, b.type.name);
					}
					return b.bytes.CompareTo(a.bytes);
				});
				using (StreamWriter streamWriter = new StreamWriter(path, false, new UTF8Encoding(false)))
				{
					streamWriter.WriteLine("record,type,name,count,bytes");
					RunSoakStandaloneBenchmark.WriteTotal(streamWriter, "process_private_bytes", sample.committed);
					RunSoakStandaloneBenchmark.WriteTotal(streamWriter, "process_working_set_bytes", sample.working);
					RunSoakStandaloneBenchmark.WriteTotal(streamWriter, "Profiler.GetTotalAllocatedMemoryLong", sample.allocated);
					RunSoakStandaloneBenchmark.WriteTotal(streamWriter, "Profiler.GetTotalReservedMemoryLong", sample.reserved);
					RunSoakStandaloneBenchmark.WriteTotal(streamWriter, "Profiler.GetMonoHeapSizeLong", sample.monoHeap);
					RunSoakStandaloneBenchmark.WriteTotal(streamWriter, "Profiler.GetMonoUsedSizeLong", sample.monoUsed);
					RunSoakStandaloneBenchmark.WriteTotal(streamWriter, "Profiler.GetAllocatedMemoryForGraphicsDriver", sample.graphicsDriver);
					streamWriter.WriteLine("total,objects,," + RunSoakStandaloneBenchmark.Integer((long)list.Count) + "," + RunSoakStandaloneBenchmark.Integer(num));
					foreach (RunSoakStandaloneBenchmark.TypeTotal typeTotal3 in list2)
					{
						streamWriter.WriteLine(string.Concat(new string[]
						{
							"type,",
							RunSoakStandaloneBenchmark.Csv(typeTotal3.name),
							",,",
							RunSoakStandaloneBenchmark.Integer(typeTotal3.count),
							",",
							RunSoakStandaloneBenchmark.Integer(typeTotal3.bytes)
						}));
					}
					foreach (RunSoakStandaloneBenchmark.ObjectSize objectSize in list)
					{
						streamWriter.WriteLine(string.Concat(new string[]
						{
							"object,",
							RunSoakStandaloneBenchmark.Csv(objectSize.type.name),
							",",
							RunSoakStandaloneBenchmark.Csv(objectSize.name),
							",1,",
							RunSoakStandaloneBenchmark.Integer(objectSize.bytes)
						}));
					}
				}
				this._status.memoryDumpFiles++;
			}
			catch (Exception ex)
			{
				this._status.memoryDumpFailures++;
				Debug.LogWarning("[RunSoakStandalone] Memory dump failed (" + path + "): " + ex.Message);
			}
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x001CA518 File Offset: 0x001C8718
		private static void WriteTotal(StreamWriter writer, string metric, long bytes)
		{
			writer.WriteLine("total," + metric + ",,," + RunSoakStandaloneBenchmark.Integer(bytes));
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x001CA536 File Offset: 0x001C8736
		private static string Csv(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			if (value.IndexOfAny(RunSoakStandaloneBenchmark.CsvSpecial) >= 0)
			{
				return "\"" + value.Replace("\"", "\"\"") + "\"";
			}
			return value;
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x001CA575 File Offset: 0x001C8775
		private static string Integer(long value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x001CA583 File Offset: 0x001C8783
		private static string Megabytes(long bytes)
		{
			if (bytes >= 0L)
			{
				return RunSoakStandaloneBenchmark.Integer(bytes / 1048576L);
			}
			return "-1";
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x001CA5A0 File Offset: 0x001C87A0
		private void Fail(string reason, int exitCode)
		{
			if (this._finished)
			{
				return;
			}
			this._failureExitCode = exitCode;
			if (this._recorder != null && this._recorder.IsRecording)
			{
				this._recorder.Stop("invalid", reason);
				return;
			}
			this._status.outcome = "failed";
			this._status.reason = reason;
			this._status.phase = "failed";
			this._status.validForComparison = false;
			this.Finish(exitCode);
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x001CA62C File Offset: 0x001C882C
		private void Finish(int exitCode)
		{
			if (this._finished)
			{
				return;
			}
			if (this.HasPoiRoute)
			{
				this.RestorePlayerControl();
			}
			this._finished = true;
			this._status.running = false;
			this._status.exitCode = exitCode;
			this._status.totalWallSeconds = this._wall.Elapsed.TotalSeconds;
			Application.logMessageReceived -= this.OnLog;
			RunSoakRenderGuard renderPreflight = this._renderPreflight;
			if (renderPreflight != null)
			{
				renderPreflight.Dispose();
			}
			this._renderPreflight = null;
			try
			{
				this.WriteProtocol();
			}
			catch (Exception ex)
			{
				exitCode = 1;
				string text = "[RunSoakStandalone] Protocol export failed: ";
				Exception ex2 = ex;
				Debug.LogError(text + ((ex2 != null) ? ex2.ToString() : null));
			}
			Debug.Log(string.Concat(new string[]
			{
				"[RunSoakStandalone] exit=",
				exitCode.ToString(),
				" outcome=",
				this._status.outcome,
				" reason=",
				this._status.reason
			}));
			Application.Quit(exitCode);
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x001CA744 File Offset: 0x001C8944
		private void OnLog(string message, string stack, LogType type)
		{
			if (!this._status.running && (type == LogType.Error || type == LogType.Exception || type == LogType.Assert))
			{
				this._status.startupErrors++;
			}
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x001CA771 File Offset: 0x001C8971
		private void OnDestroy()
		{
			if (this.HasPoiRoute)
			{
				this.RestorePlayerControl();
			}
			RunSoakRenderGuard renderPreflight = this._renderPreflight;
			if (renderPreflight != null)
			{
				renderPreflight.Dispose();
			}
			this._renderPreflight = null;
			Application.logMessageReceived -= this.OnLog;
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x001CA7AC File Offset: 0x001C89AC
		private void WriteProtocol()
		{
			if (this._zone != null)
			{
				this._status.currentNearNpcs = this._zone.CurrentNearCount;
				this._status.currentAmbientNpcs = this._zone.CurrentAmbientCount;
			}
			if (this._outputReady)
			{
				File.WriteAllText(Path.Combine(this._status.outputDirectory, "protocol.json"), JsonUtility.ToJson(this._status, true));
			}
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x001CA824 File Offset: 0x001C8A24
		private static string Argument(string[] args, string key)
		{
			int num = Array.IndexOf<string>(args, key);
			if (num < 0)
			{
				return null;
			}
			if (num + 1 >= args.Length)
			{
				throw new ArgumentException("Missing value for " + key);
			}
			return args[num + 1];
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x001CA860 File Offset: 0x001C8A60
		private static double Number(string[] args, string key, double fallback)
		{
			string text = RunSoakStandaloneBenchmark.Argument(args, key);
			double num = ((text == null) ? fallback : double.Parse(text, CultureInfo.InvariantCulture));
			if (double.IsNaN(num) || double.IsInfinity(num))
			{
				throw new ArgumentException("Finite value required for " + key);
			}
			return num;
		}

		// Token: 0x04004B80 RID: 19328
		private const string KeypointHeader = "utc,wall_s,frame,keypoint,cycle,scene,process_working_set_bytes,process_private_bytes,process_peak_working_set_bytes,process_peak_private_bytes,system_physical_bytes,commit_limit_bytes,commit_available_bytes,unity_allocated_bytes,unity_reserved_bytes,unity_unused_reserved_bytes,mono_used_bytes,mono_reserved_bytes,gc_managed_bytes,graphics_driver_bytes,texture_current_bytes,texture_desired_bytes,texture_target_bytes,texture_total_bytes,texture_non_streaming_bytes,memory_dump,spawned_chunks,pooled_chunks,pooled_zones,pending_zone_retirements,catalog_loaded,active_payload_asset_loads,completed_payload_cache_entries,addressable_payload_entries,addressable_payload_owners,addressable_deferred_releases,addressable_unused_entries,addressable_load_starts,addressable_release_requests";

		// Token: 0x04004B81 RID: 19329
		private RunSoakStandaloneBenchmark.Protocol _status = new RunSoakStandaloneBenchmark.Protocol();

		// Token: 0x04004B82 RID: 19330
		private NPCSpawnConfig _config;

		// Token: 0x04004B83 RID: 19331
		private RunSoakRecorder _recorder;

		// Token: 0x04004B84 RID: 19332
		private RunSoakHudComparison _hudComparison;

		// Token: 0x04004B85 RID: 19333
		private MapPoiStreamingRoute _poiRoute;

		// Token: 0x04004B86 RID: 19334
		private MapPoiStreamingRoute.Locator _poiLocator;

		// Token: 0x04004B87 RID: 19335
		private RunSoakRenderGuard _renderPreflight;

		// Token: 0x04004B88 RID: 19336
		private double _renderLastReady = -1.0;

		// Token: 0x04004B89 RID: 19337
		private NPCZoneManager _zone;

		// Token: 0x04004B8A RID: 19338
		private Transform _player;

		// Token: 0x04004B8B RID: 19339
		private Rigidbody _body;

		// Token: 0x04004B8C RID: 19340
		private vThirdPersonInput _input;

		// Token: 0x04004B8D RID: 19341
		private vThirdPersonController _controller;

		// Token: 0x04004B8E RID: 19342
		private global::CharacterController _character;

		// Token: 0x04004B8F RID: 19343
		private bool _inputWasEnabled;

		// Token: 0x04004B90 RID: 19344
		private bool _controllerWasEnabled;

		// Token: 0x04004B91 RID: 19345
		private bool _characterWasEnabled;

		// Token: 0x04004B92 RID: 19346
		private bool _bodyWasKinematic;

		// Token: 0x04004B93 RID: 19347
		private bool _bodyUsedGravity;

		// Token: 0x04004B94 RID: 19348
		private Vector3 _routePosition;

		// Token: 0x04004B95 RID: 19349
		private int _entryStep;

		// Token: 0x04004B96 RID: 19350
		private int _sceneHandle;

		// Token: 0x04004B97 RID: 19351
		private int _failureExitCode;

		// Token: 0x04004B98 RID: 19352
		private int _runsStarted;

		// Token: 0x04004B99 RID: 19353
		private bool _finished;

		// Token: 0x04004B9A RID: 19354
		private bool _outputReady;

		// Token: 0x04004B9B RID: 19355
		private double _lastTick;

		// Token: 0x04004B9C RID: 19356
		private double _holdUntil;

		// Token: 0x04004B9D RID: 19357
		private double _nextCheckpoint = 1000.0;

		// Token: 0x04004B9E RID: 19358
		private double _routeDistance;

		// Token: 0x04004B9F RID: 19359
		private double _nextPoll;

		// Token: 0x04004BA0 RID: 19360
		private double _initialWarmupStarted = -1.0;

		// Token: 0x04004BA1 RID: 19361
		private double _finalWarmupStarted = -1.0;

		// Token: 0x04004BA2 RID: 19362
		private double _stableSince = -1.0;

		// Token: 0x04004BA3 RID: 19363
		private double _nextScreenshotPoll;

		// Token: 0x04004BA4 RID: 19364
		private double _homeReadySince = -1.0;

		// Token: 0x04004BA5 RID: 19365
		private string _configuredProtocol;

		// Token: 0x04004BA6 RID: 19366
		private RunSoakStandaloneBenchmark.CycleResult _cycle;

		// Token: 0x04004BA7 RID: 19367
		private static readonly char[] CsvSpecial = new char[] { ',', '"', '\r', '\n' };

		// Token: 0x04004BA8 RID: 19368
		private readonly Stopwatch _wall = Stopwatch.StartNew();

		// Token: 0x02000A2B RID: 2603
		[Serializable]
		public sealed class Protocol
		{
			// Token: 0x04004BA9 RID: 19369
			public bool standalone = true;

			// Token: 0x04004BAA RID: 19370
			public bool running;

			// Token: 0x04004BAB RID: 19371
			public bool completed;

			// Token: 0x04004BAC RID: 19372
			public bool seedMatchesRequest;

			// Token: 0x04004BAD RID: 19373
			public bool heavyNpcTargetObserved;

			// Token: 0x04004BAE RID: 19374
			public bool validForComparison;

			// Token: 0x04004BAF RID: 19375
			public bool batchMode;

			// Token: 0x04004BB0 RID: 19376
			public bool mainCameraRenderingValid;

			// Token: 0x04004BB1 RID: 19377
			public bool hudComparison;

			// Token: 0x04004BB2 RID: 19378
			public string poi;

			// Token: 0x04004BB3 RID: 19379
			public int requestedRoundTrips;

			// Token: 0x04004BB4 RID: 19380
			public MapPoiStreamingRoute.Report poiRoute;

			// Token: 0x04004BB5 RID: 19381
			public bool shippedNpcConfig;

			// Token: 0x04004BB6 RID: 19382
			public bool memoryDumps;

			// Token: 0x04004BB7 RID: 19383
			public bool profilerSupported;

			// Token: 0x04004BB8 RID: 19384
			public bool developmentBuild;

			// Token: 0x04004BB9 RID: 19385
			public bool allCyclesValidForComparison;

			// Token: 0x04004BBA RID: 19386
			public int requestedCycles;

			// Token: 0x04004BBB RID: 19387
			public int cycle;

			// Token: 0x04004BBC RID: 19388
			public int completedCycles;

			// Token: 0x04004BBD RID: 19389
			public int keypointSamples;

			// Token: 0x04004BBE RID: 19390
			public int memoryDumpFiles;

			// Token: 0x04004BBF RID: 19391
			public int memoryDumpFailures;

			// Token: 0x04004BC0 RID: 19392
			public string runOutputDirectory;

			// Token: 0x04004BC1 RID: 19393
			public string mapResidencyPilot = "off";

			// Token: 0x04004BC2 RID: 19394
			public double mapResidencyCooldownSeconds;

			// Token: 0x04004BC3 RID: 19395
			public double homeSettleSeconds = 10.0;

			// Token: 0x04004BC4 RID: 19396
			public List<RunSoakStandaloneBenchmark.CycleResult> cycles = new List<RunSoakStandaloneBenchmark.CycleResult>();

			// Token: 0x04004BC5 RID: 19397
			public int preflightRenderedMainCameraFrames;

			// Token: 0x04004BC6 RID: 19398
			public bool preflightScreenshotComplete;

			// Token: 0x04004BC7 RID: 19399
			public string preflightScreenshotPath;

			// Token: 0x04004BC8 RID: 19400
			public long preflightScreenshotBytes;

			// Token: 0x04004BC9 RID: 19401
			public int preflightScreenshotWidth;

			// Token: 0x04004BCA RID: 19402
			public int preflightScreenshotHeight;

			// Token: 0x04004BCB RID: 19403
			public double screenshotRequestedWallSeconds = -1.0;

			// Token: 0x04004BCC RID: 19404
			public double screenshotCompletedWallSeconds = -1.0;

			// Token: 0x04004BCD RID: 19405
			public double screenshotToCaptureSeconds;

			// Token: 0x04004BCE RID: 19406
			public string version = "RunSoakBenchmark/v2";

			// Token: 0x04004BCF RID: 19407
			public string phase = "configuring";

			// Token: 0x04004BD0 RID: 19408
			public string outputDirectory;

			// Token: 0x04004BD1 RID: 19409
			public string outcome;

			// Token: 0x04004BD2 RID: 19410
			public string reason;

			// Token: 0x04004BD3 RID: 19411
			public string initialScene;

			// Token: 0x04004BD4 RID: 19412
			public string originalNpcConfig;

			// Token: 0x04004BD5 RID: 19413
			public string appliedNpcConfig;

			// Token: 0x04004BD6 RID: 19414
			public int requestedSeed = 57391;

			// Token: 0x04004BD7 RID: 19415
			public int actualSeed;

			// Token: 0x04004BD8 RID: 19416
			public int requestedNpcTarget = 60;

			// Token: 0x04004BD9 RID: 19417
			public int configuredMaximum = 80;

			// Token: 0x04004BDA RID: 19418
			public int configuredNear = 60;

			// Token: 0x04004BDB RID: 19419
			public int configuredAmbient = 16;

			// Token: 0x04004BDC RID: 19420
			public int configuredPrepool = 80;

			// Token: 0x04004BDD RID: 19421
			public int configuredMaxSpawnsPerRebalance = 16;

			// Token: 0x04004BDE RID: 19422
			public int configuredRetirementsPerRebalance = 8;

			// Token: 0x04004BDF RID: 19423
			public float configuredNearRadius = 55f;

			// Token: 0x04004BE0 RID: 19424
			public float configuredAmbientRadius = 60f;

			// Token: 0x04004BE1 RID: 19425
			public float configuredActiveRadius;

			// Token: 0x04004BE2 RID: 19426
			public float configuredRebalanceInterval = 0.2f;

			// Token: 0x04004BE3 RID: 19427
			public int populationThreshold = 54;

			// Token: 0x04004BE4 RID: 19428
			public double requiredPopulationCoverage = 0.9;

			// Token: 0x04004BE5 RID: 19429
			public double warmupStableSeconds = 5.0;

			// Token: 0x04004BE6 RID: 19430
			public double warmupTimeoutSeconds = 90.0;

			// Token: 0x04004BE7 RID: 19431
			public double initialPopulationWarmupSeconds;

			// Token: 0x04004BE8 RID: 19432
			public double finalPopulationWarmupSeconds;

			// Token: 0x04004BE9 RID: 19433
			public double finalMeasuredStartSeconds = -1.0;

			// Token: 0x04004BEA RID: 19434
			public double travelPopulationCoverage;

			// Token: 0x04004BEB RID: 19435
			public double finalPopulationCoverage;

			// Token: 0x04004BEC RID: 19436
			public bool initialPopulationStable;

			// Token: 0x04004BED RID: 19437
			public bool finalPopulationStable;

			// Token: 0x04004BEE RID: 19438
			public bool travelPopulationPassed;

			// Token: 0x04004BEF RID: 19439
			public bool finalPopulationPassed;

			// Token: 0x04004BF0 RID: 19440
			public int currentNearNpcs;

			// Token: 0x04004BF1 RID: 19441
			public int currentAmbientNpcs;

			// Token: 0x04004BF2 RID: 19442
			public int finalActiveNpcs;

			// Token: 0x04004BF3 RID: 19443
			public int finalNearNpcs;

			// Token: 0x04004BF4 RID: 19444
			public int finalAmbientNpcs;

			// Token: 0x04004BF5 RID: 19445
			public bool configuredTimeOfDayPopulation;

			// Token: 0x04004BF6 RID: 19446
			public int effectiveNpcTargetAtStart;

			// Token: 0x04004BF7 RID: 19447
			public int effectiveAmbientTargetAtStart;

			// Token: 0x04004BF8 RID: 19448
			public int maximumObservedNpcs;

			// Token: 0x04004BF9 RID: 19449
			public int maximumObservedActiveNpcs;

			// Token: 0x04004BFA RID: 19450
			public int checkpoints;

			// Token: 0x04004BFB RID: 19451
			public int framesWithoutCurrentChunk;

			// Token: 0x04004BFC RID: 19452
			public int startupErrors;

			// Token: 0x04004BFD RID: 19453
			public int exitCode;

			// Token: 0x04004BFE RID: 19454
			public double durationSeconds = 600.0;

			// Token: 0x04004BFF RID: 19455
			public double standingBeforeSeconds = 60.0;

			// Token: 0x04004C00 RID: 19456
			public double standingAfterSeconds = 60.0;

			// Token: 0x04004C01 RID: 19457
			public double travelSeconds = 480.0;

			// Token: 0x04004C02 RID: 19458
			public double speedMetresPerSecond = 20.0;

			// Token: 0x04004C03 RID: 19459
			public double checkpointHoldSeconds = 5.0;

			// Token: 0x04004C04 RID: 19460
			public double checkpointSpacingMetres = 1000.0;

			// Token: 0x04004C05 RID: 19461
			public double elapsedSeconds;

			// Token: 0x04004C06 RID: 19462
			public double actualDistanceMetres;

			// Token: 0x04004C07 RID: 19463
			public double scheduledTravelMetres;

			// Token: 0x04004C08 RID: 19464
			public double secondsAtRequestedActivePopulation;

			// Token: 0x04004C09 RID: 19465
			public double startupSeconds;

			// Token: 0x04004C0A RID: 19466
			public double totalWallSeconds;

			// Token: 0x04004C0B RID: 19467
			public double watchdogSeconds = 1800.0;

			// Token: 0x04004C0C RID: 19468
			public Vector3 startLogicalPosition;

			// Token: 0x04004C0D RID: 19469
			public Vector3 endLogicalPosition;

			// Token: 0x04004C0E RID: 19470
			public Vector3 cameraStartPosition;

			// Token: 0x04004C0F RID: 19471
			public Quaternion cameraStartRotation;

			// Token: 0x04004C10 RID: 19472
			public float cameraFieldOfView;

			// Token: 0x04004C11 RID: 19473
			public string limitation = "V2 changes the in-memory NPC stress profile. Travel uses the same numerical +Z route as v1; batchmode is rejected and the actual main camera must render three consecutive frames before capture and every measured completed frame. One gameplay PNG is saved and checked before population warmup; capture begins at least five seconds after file completion. initial population stabilization is outside capture; final population stabilization is recorded separately as phase4, before the measured final standing window. The configured measured duration excludes those two warmups. NPC gates use frame-time-weighted active population independently for travel and final standing; initial crowd cannot satisfy them. The synthetic route is not a walkability test. Actual streaming, NPC logic, animation and rendering remain active. A map seed does not make NPC random state, weather, time, saved roster or cache warmth deterministic. The harness changes only in-memory NPC stress settings and player movement, with no direct prefs/save writes. Normal GameWarmUp, menu, Home and run startup can write their usual preferences/saves/caches. Quality, resolution, VSync and FPS limits are observed, never changed by this driver. The in-process watchdog needs the main thread to run; a process supervisor must handle a total engine hang.";
		}

		// Token: 0x02000A2C RID: 2604
		[Serializable]
		public sealed class CycleResult
		{
			// Token: 0x04004C12 RID: 19474
			public int cycle;

			// Token: 0x04004C13 RID: 19475
			public int errorsOutsideCapture;

			// Token: 0x04004C14 RID: 19476
			public int framesWithoutCurrentChunk;

			// Token: 0x04004C15 RID: 19477
			public int maximumObservedActiveNpcs;

			// Token: 0x04004C16 RID: 19478
			public string outputDirectory;

			// Token: 0x04004C17 RID: 19479
			public string outcome;

			// Token: 0x04004C18 RID: 19480
			public string reason;

			// Token: 0x04004C19 RID: 19481
			public bool completed;

			// Token: 0x04004C1A RID: 19482
			public bool validForComparison;

			// Token: 0x04004C1B RID: 19483
			public bool heavyNpcTargetObserved;

			// Token: 0x04004C1C RID: 19484
			public double runStartedWallSeconds;

			// Token: 0x04004C1D RID: 19485
			public double captureStartedWallSeconds;

			// Token: 0x04004C1E RID: 19486
			public double runEndedWallSeconds;

			// Token: 0x04004C1F RID: 19487
			public double homeReadyWallSeconds;

			// Token: 0x04004C20 RID: 19488
			public double elapsedSeconds;

			// Token: 0x04004C21 RID: 19489
			public double actualDistanceMetres;

			// Token: 0x04004C22 RID: 19490
			public long homePrivateBytesAfterRun = -1L;

			// Token: 0x04004C23 RID: 19491
			public long homeWorkingSetBytesAfterRun = -1L;

			// Token: 0x04004C24 RID: 19492
			public long homePeakPrivateBytesAfterRun = -1L;
		}

		// Token: 0x02000A2D RID: 2605
		private struct MemorySample
		{
			// Token: 0x04004C25 RID: 19493
			public long working;

			// Token: 0x04004C26 RID: 19494
			public long committed;

			// Token: 0x04004C27 RID: 19495
			public long peakWorking;

			// Token: 0x04004C28 RID: 19496
			public long peakCommitted;

			// Token: 0x04004C29 RID: 19497
			public long physical;

			// Token: 0x04004C2A RID: 19498
			public long commitLimit;

			// Token: 0x04004C2B RID: 19499
			public long commitAvailable;

			// Token: 0x04004C2C RID: 19500
			public long allocated;

			// Token: 0x04004C2D RID: 19501
			public long reserved;

			// Token: 0x04004C2E RID: 19502
			public long unusedReserved;

			// Token: 0x04004C2F RID: 19503
			public long monoHeap;

			// Token: 0x04004C30 RID: 19504
			public long monoUsed;

			// Token: 0x04004C31 RID: 19505
			public long graphicsDriver;

			// Token: 0x04004C32 RID: 19506
			public long gcManaged;

			// Token: 0x04004C33 RID: 19507
			public long textureCurrent;

			// Token: 0x04004C34 RID: 19508
			public long textureDesired;

			// Token: 0x04004C35 RID: 19509
			public long textureTarget;

			// Token: 0x04004C36 RID: 19510
			public long textureTotal;

			// Token: 0x04004C37 RID: 19511
			public long textureNonStreaming;

			// Token: 0x04004C38 RID: 19512
			public int spawnedChunks;

			// Token: 0x04004C39 RID: 19513
			public int pooledChunks;

			// Token: 0x04004C3A RID: 19514
			public int zonePool;

			// Token: 0x04004C3B RID: 19515
			public int zoneRetirements;

			// Token: 0x04004C3C RID: 19516
			public int catalogLoaded;

			// Token: 0x04004C3D RID: 19517
			public int activePayloadLoads;

			// Token: 0x04004C3E RID: 19518
			public int completedPayloadCacheEntries;

			// Token: 0x04004C3F RID: 19519
			public int addressableEntries;

			// Token: 0x04004C40 RID: 19520
			public int addressableOwners;

			// Token: 0x04004C41 RID: 19521
			public int addressableDeferredReleases;

			// Token: 0x04004C42 RID: 19522
			public int addressableUnusedEntries;

			// Token: 0x04004C43 RID: 19523
			public int addressableLoadStarts;

			// Token: 0x04004C44 RID: 19524
			public int addressableReleaseRequests;
		}

		// Token: 0x02000A2E RID: 2606
		private sealed class TypeTotal
		{
			// Token: 0x04004C45 RID: 19525
			public string name;

			// Token: 0x04004C46 RID: 19526
			public long count;

			// Token: 0x04004C47 RID: 19527
			public long bytes;
		}

		// Token: 0x02000A2F RID: 2607
		private struct ObjectSize
		{
			// Token: 0x04004C48 RID: 19528
			public RunSoakStandaloneBenchmark.TypeTotal type;

			// Token: 0x04004C49 RID: 19529
			public string name;

			// Token: 0x04004C4A RID: 19530
			public long bytes;
		}
	}
}
