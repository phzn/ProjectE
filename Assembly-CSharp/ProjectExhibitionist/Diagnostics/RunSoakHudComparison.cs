using System;
using System.Globalization;
using System.IO;
using System.Text;
using ExhibitionistDemo.UI;
using Features.Detection;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectExhibitionist.Diagnostics
{
	// Token: 0x02000A17 RID: 2583
	public sealed class RunSoakHudComparison : MonoBehaviour
	{
		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06004975 RID: 18805 RVA: 0x001C28E6 File Offset: 0x001C0AE6
		public bool ValidForComparison
		{
			get
			{
				return this._report.validForComparison;
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06004976 RID: 18806 RVA: 0x001C28F3 File Offset: 0x001C0AF3
		public bool PopulationPassed
		{
			get
			{
				return this._report.sustainedPopulationPassed && this._report.comparablePopulationPassed;
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06004977 RID: 18807 RVA: 0x001C290F File Offset: 0x001C0B0F
		public string Reason
		{
			get
			{
				return this._report.reason;
			}
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06004978 RID: 18808 RVA: 0x001C291C File Offset: 0x001C0B1C
		public string CurrentLabel
		{
			get
			{
				if (this._report.windows == null || this._stage >= 16)
				{
					return "hud_comparison_complete";
				}
				return this._report.windows[this._stage].label;
			}
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x001C2952 File Offset: 0x001C0B52
		public static bool Requested(string[] args)
		{
			return Array.IndexOf<string>(args, "-runSoakHudComparison") >= 0;
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x001C2965 File Offset: 0x001C0B65
		public static bool HudOnForBlock(int block)
		{
			return block % 4 == 0 || block % 4 == 3;
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x001C2974 File Offset: 0x001C0B74
		public static RunSoakHudComparison.Window MakeWindow(int stage)
		{
			return new RunSoakHudComparison.Window
			{
				phase = 101 + stage,
				block = stage / 2,
				hudOn = RunSoakHudComparison.HudOnForBlock(stage / 2),
				measured = ((stage & 1) != 0),
				requestedSeconds = (((stage & 1) == 0) ? 3.0 : 12.0),
				label = string.Concat(new string[]
				{
					"block",
					(stage / 2 + 1).ToString(),
					"_hud_",
					RunSoakHudComparison.HudOnForBlock(stage / 2) ? "on_" : "off_",
					((stage & 1) == 0) ? "settling" : "measured"
				})
			};
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x001C2A34 File Offset: 0x001C0C34
		public void Initialize(RunSoakRecorder recorder, Transform player)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (Application.isEditor || !RunSoakHudComparison.Requested(commandLineArgs) || Array.IndexOf<string>(commandLineArgs, "-runSoakBenchmark") < 0)
			{
				throw new InvalidOperationException("Requires a standalone player with -runSoakBenchmark -runSoakHudComparison.");
			}
			this._recorder = recorder;
			this._player = player;
			this._report.outputDirectory = recorder.OutputDirectory;
			this._report.productName = Application.productName;
			this._report.companyName = Application.companyName;
			this._report.persistentDataPath = Application.persistentDataPath;
			this._report.windows = new RunSoakHudComparison.Window[16];
			for (int i = 0; i < 16; i++)
			{
				this._report.windows[i] = RunSoakHudComparison.MakeWindow(i);
			}
			this._mainCamera = Camera.main;
			if (this._mainCamera == null || this._player == null)
			{
				throw new InvalidOperationException("Missing gameplay player/main camera.");
			}
			foreach (PlayerFaceCam playerFaceCam in Object.FindObjectsOfType<PlayerFaceCam>(true))
			{
				if (!(playerFaceCam.target == null) && playerFaceCam.target.IsChildOf(this._player))
				{
					if (this._faceCamera != null)
					{
						throw new InvalidOperationException("More than one player-owned PlayerFaceCam; ambiguous probe.");
					}
					this._faceCamera = playerFaceCam.GetComponent<Camera>();
				}
			}
			if (this._faceCamera == null)
			{
				throw new InvalidOperationException("No player-owned PlayerFaceCam found.");
			}
			this._report.faceCameraName = this._faceCamera.name;
			this._report.faceCameraInstanceId = this._faceCamera.GetInstanceID();
			this._report.mainCameraName = this._mainCamera.name;
			this._report.mainCameraInstanceId = this._mainCamera.GetInstanceID();
			this._report.playerStartPosition = this._player.position;
			this._report.mainCameraStartPosition = this._mainCamera.transform.position;
			this._report.mainCameraStartRotation = this._mainCamera.transform.rotation;
			this._report.mainCameraStartFieldOfView = this._mainCamera.fieldOfView;
			this._report.originalFaceCamera = GameSettingsService.ShowFaceCamera;
			this._report.originalMinimap = GameSettingsService.ShowMinimap;
			this._report.originalDistanceAndScore = GameSettingsService.ShowDistanceAndScore;
			this._settingsSaved = true;
			this._rows = new RunSoakHudComparison.Row[250000];
			this._timeline = new RunSoakFrameTimeline(Time.frameCount);
			RenderPipelineManager.endCameraRendering += this.OnSrpRendered;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.OnBuiltinRendered));
			GameSettingsService.SettingsChanged += this.ObserveSettings;
			this._subscribed = true;
			this.ObserveSettings();
			this._initialized = true;
			this.EnterStage();
			File.WriteAllText(Path.Combine(this._report.outputDirectory, "hud_protocol.json"), JsonUtility.ToJson(this._report, true));
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x001C2D34 File Offset: 0x001C0F34
		private void EnterStage()
		{
			RunSoakHudComparison.Window window = this._report.windows[this._stage];
			if (!window.measured)
			{
				if (GameSettingsService.ShowFaceCamera != window.hudOn)
				{
					GameSettingsService.SetShowFaceCamera(window.hudOn);
				}
				if (GameSettingsService.ShowMinimap != window.hudOn)
				{
					GameSettingsService.SetShowMinimap(window.hudOn);
				}
				if (GameSettingsService.ShowDistanceAndScore != window.hudOn)
				{
					GameSettingsService.SetShowDistanceAndScore(window.hudOn);
				}
			}
			this._recorder.Phase = window.phase;
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x001C2DB5 File Offset: 0x001C0FB5
		private void ObserveSettings()
		{
			this._settingsBits = (GameSettingsService.ShowFaceCamera ? 1 : 0) | (GameSettingsService.ShowMinimap ? 2 : 0) | (GameSettingsService.ShowDistanceAndScore ? 4 : 0);
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x001C2DE0 File Offset: 0x001C0FE0
		private void OnSrpRendered(ScriptableRenderContext context, Camera camera)
		{
			this.ObserveRender(camera);
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x001C2DE9 File Offset: 0x001C0FE9
		private void OnBuiltinRendered(Camera camera)
		{
			if (GraphicsSettings.currentRenderPipeline == null)
			{
				this.ObserveRender(camera);
			}
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x001C2E00 File Offset: 0x001C1000
		private void ObserveRender(Camera camera)
		{
			if (!this._initialized || this._finished || !this._recorder.IsRecording)
			{
				return;
			}
			if (camera == this._faceCamera)
			{
				this._receipts.FaceRendered(Time.frameCount);
			}
			if (camera == this._mainCamera)
			{
				this._receipts.MainRendered(Time.frameCount, camera.transform.position, camera.transform.rotation, camera.fieldOfView);
			}
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x001C2E84 File Offset: 0x001C1084
		private void LateUpdate()
		{
			if (!this._initialized || this._finished || !this._recorder.IsRecording)
			{
				return;
			}
			int frameCount = Time.frameCount;
			this._contexts[frameCount & 7] = new RunSoakHudComparison.Context
			{
				frame = frameCount,
				chunks = ((ChunkManager.Instance != null) ? ChunkManager.Instance.SpawnedChunkCount : (-1)),
				settingsBits = this._settingsBits,
				faceCameraEnabled = (this._faceCamera != null && this._faceCamera.isActiveAndEnabled),
				playerDrift = ((this._player != null) ? Vector3.Distance(this._player.position, this._report.playerStartPosition) : (-1f))
			};
			this._timeline.CaptureLateState(frameCount, this._recorder.Phase, (NPCZoneManager.Instance != null) ? NPCZoneManager.Instance.ActiveNPCCount : (-1), 0f, -1, -1.0, -1.0, -1);
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x001C2FA8 File Offset: 0x001C11A8
		public void Tick(RunSoakRecorder recorder)
		{
			if (!this._initialized || this._finished)
			{
				return;
			}
			if (!this.CaptureCompletedFrame())
			{
				recorder.Stop("invalid", "HUD comparison frame metadata was missing/stale or capacity exhausted.");
				return;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null && instance.IsEventActive)
			{
				recorder.Stop("invalid", "Detection sequence started during stationary HUD comparison.");
				return;
			}
			if (this._mainCamera == null || Camera.main != this._mainCamera || this._faceCamera == null)
			{
				this._report.cameraIdentityChanged = true;
				recorder.Stop("invalid", "HUD comparison camera identity changed.");
				return;
			}
			if (this._report.windows[this._stage].DurationReached)
			{
				this._stage++;
				if (this._stage == 16)
				{
					recorder.Stop("completed", "Stationary HUD ABBA ABBA completed with separate settling windows.");
					return;
				}
				this.EnterStage();
			}
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x001C309C File Offset: 0x001C129C
		private bool CaptureCompletedFrame()
		{
			if (this._timeline == null)
			{
				return false;
			}
			RunSoakFrameTimeline.State state;
			if (!this._timeline.TryTakeCompleted(Time.frameCount, out state))
			{
				return !this._timeline.Invalid;
			}
			if (this._count == 250000)
			{
				return false;
			}
			RunSoakHudComparison.Context context = this._contexts[state.sourceFrame & 7];
			if (context.frame != state.sourceFrame || state.phase < 101 || state.phase >= 117)
			{
				return false;
			}
			RunSoakHudComparison.ReceiptLedger.Receipt receipt = this._receipts.ForFrame(state.sourceFrame);
			RunSoakHudComparison.Row row = new RunSoakHudComparison.Row
			{
				frame = state.sourceFrame,
				phase = state.phase,
				activeNpcs = state.activeNpcs,
				chunks = context.chunks,
				settingsBits = context.settingsBits,
				faceCameraEnabled = context.faceCameraEnabled,
				faceCalls = receipt.faceCalls,
				mainCalls = receipt.mainCalls,
				playerDrift = context.playerDrift,
				elapsed = this._recorder.Elapsed,
				milliseconds = Time.unscaledDeltaTime * 1000f,
				cameraPositionDrift = ((receipt.mainCalls > 0) ? Vector3.Distance(receipt.mainPosition, this._report.mainCameraStartPosition) : (-1f)),
				cameraAngleDrift = ((receipt.mainCalls > 0) ? Quaternion.Angle(receipt.mainRotation, this._report.mainCameraStartRotation) : (-1f)),
				fieldOfViewDrift = ((receipt.mainCalls > 0) ? Mathf.Abs(receipt.mainFov - this._report.mainCameraStartFieldOfView) : (-1f))
			};
			RunSoakHudComparison.Row[] rows = this._rows;
			int count = this._count;
			this._count = count + 1;
			rows[count] = row;
			RunSoakHudComparison.Window window = this._report.windows[state.phase - 101];
			window.frames++;
			window.AddDuration(row.milliseconds);
			if (window.firstElapsed < 0.0)
			{
				window.firstElapsed = row.elapsed;
			}
			window.lastElapsed = row.elapsed;
			window.faceRenderCalls += row.faceCalls;
			if (row.faceCalls > 0)
			{
				window.framesWithFaceRender++;
			}
			if (row.mainCalls > 0)
			{
				window.mainRenderFrames++;
			}
			window.npcSeconds += (double)((float)Math.Max(0, row.activeNpcs) * row.milliseconds) / 1000.0;
			if (row.activeNpcs >= 54)
			{
				window.stressSeconds += (double)row.milliseconds / 1000.0;
			}
			window.minimumActiveNpcs = Math.Min(window.minimumActiveNpcs, row.activeNpcs);
			window.maximumActiveNpcs = Math.Max(window.maximumActiveNpcs, row.activeNpcs);
			window.minimumChunks = Math.Min(window.minimumChunks, row.chunks);
			window.maximumChunks = Math.Max(window.maximumChunks, row.chunks);
			if (row.settingsBits != (window.hudOn ? 7 : 0))
			{
				window.settingMismatchFrames++;
			}
			window.maximumPlayerDrift = Mathf.Max(window.maximumPlayerDrift, row.playerDrift);
			window.maximumCameraPositionDrift = Mathf.Max(window.maximumCameraPositionDrift, row.cameraPositionDrift);
			window.maximumCameraAngleDrift = Mathf.Max(window.maximumCameraAngleDrift, row.cameraAngleDrift);
			window.maximumFieldOfViewDrift = Mathf.Max(window.maximumFieldOfViewDrift, row.fieldOfViewDrift);
			return true;
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x001C3464 File Offset: 0x001C1664
		public void Complete(RunSoakRecorder recorder)
		{
			if (this._finished)
			{
				return;
			}
			bool flag = this._initialized && this.CaptureCompletedFrame() && this._count == recorder.FrameCount;
			this._finished = true;
			this._report.lastStage = this._stage;
			this._report.rows = this._count;
			this._report.frameAlignmentValid = flag;
			this._report.completed = this._stage == 16 && recorder.LastReport.outcome == "completed";
			this._report.outcome = recorder.LastReport.outcome;
			this._report.reason = recorder.LastReport.reason;
			try
			{
				bool flag2 = this._report.windows != null;
				bool flag3 = true;
				bool flag4 = true;
				bool flag5 = true;
				int num = int.MaxValue;
				int num2 = -1;
				double num3 = 0.0;
				double num4 = 0.0;
				double num5 = 0.0;
				double num6 = 0.0;
				float[] array = new float[this._count];
				if (this._report.windows != null)
				{
					foreach (RunSoakHudComparison.Window window in this._report.windows)
					{
						int num7 = 0;
						for (int j = 0; j < this._count; j++)
						{
							if (this._rows[j].phase == window.phase)
							{
								array[num7++] = this._rows[j].milliseconds;
							}
						}
						window.frameTime = RunSoakRecorder.Distribution.Calculate(array, num7);
						window.meanActiveNpcs = ((window.seconds > 0.0) ? (window.npcSeconds / window.seconds) : (-1.0));
						window.stressCoverage = ((window.seconds > 0.0) ? (window.stressSeconds / window.seconds) : 0.0);
						if (window.minimumActiveNpcs == 2147483647)
						{
							window.minimumActiveNpcs = -1;
						}
						if (window.minimumChunks == 2147483647)
						{
							window.minimumChunks = -1;
						}
						flag2 &= window.DurationReached && window.frames > 0;
						if (window.measured)
						{
							flag5 &= window.VisibleFaceRenderedEveryFrame;
							flag4 &= window.stressCoverage >= 0.9 && window.minimumActiveNpcs >= 0;
							flag3 &= window.settingMismatchFrames == 0 && window.mainRenderFrames == window.frames && window.maximumPlayerDrift <= 0.1f && window.maximumCameraPositionDrift <= 0.1f && window.maximumCameraAngleDrift <= 1f && window.maximumFieldOfViewDrift <= 0.01f && window.minimumChunks >= 0;
							num = Math.Min(num, window.minimumChunks);
							num2 = Math.Max(num2, window.maximumChunks);
							if (window.hudOn)
							{
								num3 += window.seconds;
								num5 += window.npcSeconds;
							}
							else
							{
								num4 += window.seconds;
								num6 += window.npcSeconds;
							}
						}
					}
				}
				this._report.sustainedPopulationPassed = flag4 && flag2;
				this._report.onMeanNpcs = ((num3 > 0.0) ? (num5 / num3) : (-1.0));
				this._report.offMeanNpcs = ((num4 > 0.0) ? (num6 / num4) : (-1.0));
				this._report.relativePopulationDifference = Math.Abs(this._report.onMeanNpcs - this._report.offMeanNpcs) / Math.Max(1.0, Math.Max(this._report.onMeanNpcs, this._report.offMeanNpcs));
				if (this._report.windows != null)
				{
					for (int k = 0; k < 4; k++)
					{
						double meanActiveNpcs = this._report.windows[k * 4 + 1].meanActiveNpcs;
						double meanActiveNpcs2 = this._report.windows[k * 4 + 3].meanActiveNpcs;
						this._report.maximumAdjacentPairPopulationDifference = Math.Max(this._report.maximumAdjacentPairPopulationDifference, Math.Abs(meanActiveNpcs - meanActiveNpcs2) / Math.Max(1.0, Math.Max(meanActiveNpcs, meanActiveNpcs2)));
					}
				}
				this._report.comparablePopulationPassed = num3 > 0.0 && num4 > 0.0 && this._report.relativePopulationDifference <= 0.02 && this._report.maximumAdjacentPairPopulationDifference <= 0.02;
				this._report.stableViewAndChunksPassed = flag3 && num == num2;
				this._report.faceCameraOnRenderingPassed = flag5 && flag2;
				this._report.validForComparison = this._report.completed && flag && flag2 && this.PopulationPassed && recorder.LastReport.completeFrameCoverage && recorder.LastReport.mainCameraRenderingValid && !recorder.LastReport.settingsChanged && !recorder.LastReport.sceneChanged && !recorder.LastReport.sessionSeedChanged && recorder.LastReport.applicationErrors == 0 && this._report.stableViewAndChunksPassed && this._report.faceCameraOnRenderingPassed && !this._report.cameraIdentityChanged;
				if (this._report.completed && !this._report.validForComparison)
				{
					this._report.reason = "HUD windows completed but metadata, duration, load or stable-view/chunk comparison gate failed; inspect hud_protocol.json.";
				}
			}
			finally
			{
				this.RestoreSettingsAndUnsubscribe();
			}
			this._report.validForComparison &= this._report.settingsRestored;
			this.ExportRows();
			File.WriteAllText(Path.Combine(this._report.outputDirectory, "hud_protocol.json"), JsonUtility.ToJson(this._report, true));
			this._rows = null;
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x001C3AC0 File Offset: 0x001C1CC0
		private void RestoreSettingsAndUnsubscribe()
		{
			if (this._settingsSaved)
			{
				this._settingsSaved = false;
				bool flag = true;
				try
				{
					GameSettingsService.SetShowFaceCamera(this._report.originalFaceCamera);
				}
				catch
				{
					flag = false;
				}
				try
				{
					GameSettingsService.SetShowMinimap(this._report.originalMinimap);
				}
				catch
				{
					flag = false;
				}
				try
				{
					GameSettingsService.SetShowDistanceAndScore(this._report.originalDistanceAndScore);
				}
				catch
				{
					flag = false;
				}
				this._report.settingsRestored = flag;
			}
			if (!this._subscribed)
			{
				return;
			}
			RenderPipelineManager.endCameraRendering -= this.OnSrpRendered;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.OnBuiltinRendered));
			GameSettingsService.SettingsChanged -= this.ObserveSettings;
			this._subscribed = false;
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x001C3BAC File Offset: 0x001C1DAC
		private void ExportRows()
		{
			using (StreamWriter streamWriter = new StreamWriter(Path.Combine(this._report.outputDirectory, "hud_frames.csv"), false, new UTF8Encoding(false), 65536))
			{
				streamWriter.WriteLine("frame,elapsed_s,phase,frame_ms,active_npcs,chunks,settings_bits,face_camera_enabled,face_render_calls,main_render_calls,player_drift_m,main_position_drift_m,main_angle_drift_deg,main_fov_drift_deg");
				for (int i = 0; i < this._count; i++)
				{
					RunSoakHudComparison.Row row = this._rows[i];
					streamWriter.WriteLine(string.Join(",", new object[]
					{
						row.frame,
						RunSoakHudComparison.F(row.elapsed),
						row.phase,
						RunSoakHudComparison.F((double)row.milliseconds),
						row.activeNpcs,
						row.chunks,
						row.settingsBits,
						row.faceCameraEnabled ? 1 : 0,
						row.faceCalls,
						row.mainCalls,
						RunSoakHudComparison.F((double)row.playerDrift),
						RunSoakHudComparison.F((double)row.cameraPositionDrift),
						RunSoakHudComparison.F((double)row.cameraAngleDrift),
						RunSoakHudComparison.F((double)row.fieldOfViewDrift)
					}));
				}
			}
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x001BF9FC File Offset: 0x001BDBFC
		private static string F(double number)
		{
			return number.ToString("R", CultureInfo.InvariantCulture);
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x001C3D24 File Offset: 0x001C1F24
		private void OnDestroy()
		{
			this.RestoreSettingsAndUnsubscribe();
		}

		// Token: 0x04004A22 RID: 18978
		public const int WindowCount = 16;

		// Token: 0x04004A23 RID: 18979
		public const double SettleSeconds = 3.0;

		// Token: 0x04004A24 RID: 18980
		public const double MeasureSeconds = 12.0;

		// Token: 0x04004A25 RID: 18981
		public const double CaptureBudgetSeconds = 150.0;

		// Token: 0x04004A26 RID: 18982
		public const double MaxRelativePopulationDifference = 0.02;

		// Token: 0x04004A27 RID: 18983
		private const int MaximumFrames = 250000;

		// Token: 0x04004A28 RID: 18984
		public const string Limitation = "HUD comparison v1: stationary ABBA ABBA; A=all three gameplay HUD settings on, B=off. Every block has at least 3 seconds of completed settling frames and 12 seconds of completed measured frames. Only GameSettingsService.SetShowFaceCamera/SetShowMinimap/SetShowDistanceAndScore are invoked. These APIs save preferences, so launch only with an isolated diagnostic product/profile; original values are restored on normal cleanup. Restoration covers these three boolean values, not original key absence or the entire preference store. An OS kill can prevent restoration. Actual PlayerFaceCam render callbacks are observed, never assumed from its enabled flag. frames.csv provides CPU/GPU/frame/GC measurements; memory.csv provides OS private/working-set and Unity allocated/reserved bytes. Join hud_frames.csv to frames.csv by completed source-frame number; phase IDs 101..116 label settling/measured windows. Generic recorder phase summaries 0..4 do not apply; use hud_protocol.json windows and raw phase IDs. NPC animation, weather and gameplay remain live; ABBA ordering reduces but cannot eliminate drift. This is a short paired HUD-toggle comparison, not evidence of a long-term memory plateau or leak removal.";

		// Token: 0x04004A29 RID: 18985
		private readonly RunSoakHudComparison.Report _report = new RunSoakHudComparison.Report();

		// Token: 0x04004A2A RID: 18986
		private readonly RunSoakHudComparison.ReceiptLedger _receipts = new RunSoakHudComparison.ReceiptLedger();

		// Token: 0x04004A2B RID: 18987
		private readonly RunSoakHudComparison.Context[] _contexts = new RunSoakHudComparison.Context[8];

		// Token: 0x04004A2C RID: 18988
		private RunSoakRecorder _recorder;

		// Token: 0x04004A2D RID: 18989
		private RunSoakFrameTimeline _timeline;

		// Token: 0x04004A2E RID: 18990
		private Transform _player;

		// Token: 0x04004A2F RID: 18991
		private Camera _faceCamera;

		// Token: 0x04004A30 RID: 18992
		private Camera _mainCamera;

		// Token: 0x04004A31 RID: 18993
		private RunSoakHudComparison.Row[] _rows;

		// Token: 0x04004A32 RID: 18994
		private int _count;

		// Token: 0x04004A33 RID: 18995
		private int _stage;

		// Token: 0x04004A34 RID: 18996
		private int _settingsBits;

		// Token: 0x04004A35 RID: 18997
		private bool _initialized;

		// Token: 0x04004A36 RID: 18998
		private bool _finished;

		// Token: 0x04004A37 RID: 18999
		private bool _subscribed;

		// Token: 0x04004A38 RID: 19000
		private bool _settingsSaved;

		// Token: 0x02000A18 RID: 2584
		[Serializable]
		public sealed class Window
		{
			// Token: 0x0600498B RID: 18827 RVA: 0x001C3D56 File Offset: 0x001C1F56
			public void AddDuration(float frameMilliseconds)
			{
				this.seconds += (double)frameMilliseconds / 1000.0;
			}

			// Token: 0x17000C72 RID: 3186
			// (get) Token: 0x0600498C RID: 18828 RVA: 0x001C3D71 File Offset: 0x001C1F71
			public bool DurationReached
			{
				get
				{
					return this.seconds + 1E-06 >= this.requestedSeconds;
				}
			}

			// Token: 0x17000C73 RID: 3187
			// (get) Token: 0x0600498D RID: 18829 RVA: 0x001C3D8E File Offset: 0x001C1F8E
			public bool VisibleFaceRenderedEveryFrame
			{
				get
				{
					return !this.measured || !this.hudOn || (this.frames > 0 && this.framesWithFaceRender == this.frames);
				}
			}

			// Token: 0x04004A39 RID: 19001
			public int phase;

			// Token: 0x04004A3A RID: 19002
			public int block;

			// Token: 0x04004A3B RID: 19003
			public int frames;

			// Token: 0x04004A3C RID: 19004
			public int faceRenderCalls;

			// Token: 0x04004A3D RID: 19005
			public int framesWithFaceRender;

			// Token: 0x04004A3E RID: 19006
			public int mainRenderFrames;

			// Token: 0x04004A3F RID: 19007
			public bool hudOn;

			// Token: 0x04004A40 RID: 19008
			public bool measured;

			// Token: 0x04004A41 RID: 19009
			public string label;

			// Token: 0x04004A42 RID: 19010
			public double requestedSeconds;

			// Token: 0x04004A43 RID: 19011
			public double seconds;

			// Token: 0x04004A44 RID: 19012
			public double firstElapsed = -1.0;

			// Token: 0x04004A45 RID: 19013
			public double lastElapsed;

			// Token: 0x04004A46 RID: 19014
			public double npcSeconds;

			// Token: 0x04004A47 RID: 19015
			public double stressSeconds;

			// Token: 0x04004A48 RID: 19016
			public double meanActiveNpcs;

			// Token: 0x04004A49 RID: 19017
			public double stressCoverage;

			// Token: 0x04004A4A RID: 19018
			public int minimumActiveNpcs = int.MaxValue;

			// Token: 0x04004A4B RID: 19019
			public int maximumActiveNpcs;

			// Token: 0x04004A4C RID: 19020
			public int minimumChunks = int.MaxValue;

			// Token: 0x04004A4D RID: 19021
			public int maximumChunks;

			// Token: 0x04004A4E RID: 19022
			public int settingMismatchFrames;

			// Token: 0x04004A4F RID: 19023
			public float maximumPlayerDrift;

			// Token: 0x04004A50 RID: 19024
			public float maximumCameraPositionDrift;

			// Token: 0x04004A51 RID: 19025
			public float maximumCameraAngleDrift;

			// Token: 0x04004A52 RID: 19026
			public float maximumFieldOfViewDrift;

			// Token: 0x04004A53 RID: 19027
			public RunSoakRecorder.Distribution frameTime;
		}

		// Token: 0x02000A19 RID: 2585
		[Serializable]
		public sealed class Report
		{
			// Token: 0x04004A54 RID: 19028
			public string version = "RunSoakHudComparison/v1";

			// Token: 0x04004A55 RID: 19029
			public string limitation = "HUD comparison v1: stationary ABBA ABBA; A=all three gameplay HUD settings on, B=off. Every block has at least 3 seconds of completed settling frames and 12 seconds of completed measured frames. Only GameSettingsService.SetShowFaceCamera/SetShowMinimap/SetShowDistanceAndScore are invoked. These APIs save preferences, so launch only with an isolated diagnostic product/profile; original values are restored on normal cleanup. Restoration covers these three boolean values, not original key absence or the entire preference store. An OS kill can prevent restoration. Actual PlayerFaceCam render callbacks are observed, never assumed from its enabled flag. frames.csv provides CPU/GPU/frame/GC measurements; memory.csv provides OS private/working-set and Unity allocated/reserved bytes. Join hud_frames.csv to frames.csv by completed source-frame number; phase IDs 101..116 label settling/measured windows. Generic recorder phase summaries 0..4 do not apply; use hud_protocol.json windows and raw phase IDs. NPC animation, weather and gameplay remain live; ABBA ordering reduces but cannot eliminate drift. This is a short paired HUD-toggle comparison, not evidence of a long-term memory plateau or leak removal.";

			// Token: 0x04004A56 RID: 19030
			public string outcome;

			// Token: 0x04004A57 RID: 19031
			public string reason;

			// Token: 0x04004A58 RID: 19032
			public string outputDirectory;

			// Token: 0x04004A59 RID: 19033
			public string productName;

			// Token: 0x04004A5A RID: 19034
			public string companyName;

			// Token: 0x04004A5B RID: 19035
			public string persistentDataPath;

			// Token: 0x04004A5C RID: 19036
			public bool completed;

			// Token: 0x04004A5D RID: 19037
			public bool validForComparison;

			// Token: 0x04004A5E RID: 19038
			public bool settingsRestored;

			// Token: 0x04004A5F RID: 19039
			public bool frameAlignmentValid;

			// Token: 0x04004A60 RID: 19040
			public bool cameraIdentityChanged;

			// Token: 0x04004A61 RID: 19041
			public bool originalFaceCamera;

			// Token: 0x04004A62 RID: 19042
			public bool originalMinimap;

			// Token: 0x04004A63 RID: 19043
			public bool originalDistanceAndScore;

			// Token: 0x04004A64 RID: 19044
			public bool sustainedPopulationPassed;

			// Token: 0x04004A65 RID: 19045
			public bool comparablePopulationPassed;

			// Token: 0x04004A66 RID: 19046
			public bool stableViewAndChunksPassed;

			// Token: 0x04004A67 RID: 19047
			public bool faceCameraOnRenderingPassed;

			// Token: 0x04004A68 RID: 19048
			public string faceCameraName;

			// Token: 0x04004A69 RID: 19049
			public string mainCameraName;

			// Token: 0x04004A6A RID: 19050
			public int faceCameraInstanceId;

			// Token: 0x04004A6B RID: 19051
			public int mainCameraInstanceId;

			// Token: 0x04004A6C RID: 19052
			public int rows;

			// Token: 0x04004A6D RID: 19053
			public int maximumFrames = 250000;

			// Token: 0x04004A6E RID: 19054
			public int lastStage;

			// Token: 0x04004A6F RID: 19055
			public Vector3 playerStartPosition;

			// Token: 0x04004A70 RID: 19056
			public Vector3 mainCameraStartPosition;

			// Token: 0x04004A71 RID: 19057
			public Quaternion mainCameraStartRotation;

			// Token: 0x04004A72 RID: 19058
			public float mainCameraStartFieldOfView;

			// Token: 0x04004A73 RID: 19059
			public double onMeanNpcs;

			// Token: 0x04004A74 RID: 19060
			public double offMeanNpcs;

			// Token: 0x04004A75 RID: 19061
			public double relativePopulationDifference;

			// Token: 0x04004A76 RID: 19062
			public double maximumAdjacentPairPopulationDifference;

			// Token: 0x04004A77 RID: 19063
			public RunSoakHudComparison.Window[] windows;
		}

		// Token: 0x02000A1A RID: 2586
		public sealed class ReceiptLedger
		{
			// Token: 0x06004990 RID: 18832 RVA: 0x001C3E14 File Offset: 0x001C2014
			public void FaceRendered(int frame)
			{
				int num = frame & 7;
				if (this._frames[num].frame != frame)
				{
					this._frames[num] = new RunSoakHudComparison.ReceiptLedger.Receipt
					{
						frame = frame
					};
				}
				RunSoakHudComparison.ReceiptLedger.Receipt[] frames = this._frames;
				int num2 = num;
				frames[num2].faceCalls = frames[num2].faceCalls + 1;
			}

			// Token: 0x06004991 RID: 18833 RVA: 0x001C3E6C File Offset: 0x001C206C
			public void MainRendered(int frame, Vector3 position, Quaternion rotation, float fov)
			{
				int num = frame & 7;
				if (this._frames[num].frame != frame)
				{
					this._frames[num] = new RunSoakHudComparison.ReceiptLedger.Receipt
					{
						frame = frame
					};
				}
				RunSoakHudComparison.ReceiptLedger.Receipt[] frames = this._frames;
				int num2 = num;
				frames[num2].mainCalls = frames[num2].mainCalls + 1;
				this._frames[num].mainPosition = position;
				this._frames[num].mainRotation = rotation;
				this._frames[num].mainFov = fov;
			}

			// Token: 0x06004992 RID: 18834 RVA: 0x001C3EFC File Offset: 0x001C20FC
			public RunSoakHudComparison.ReceiptLedger.Receipt ForFrame(int frame)
			{
				if (this._frames[frame & 7].frame != frame)
				{
					return new RunSoakHudComparison.ReceiptLedger.Receipt
					{
						frame = frame
					};
				}
				return this._frames[frame & 7];
			}

			// Token: 0x04004A78 RID: 19064
			private readonly RunSoakHudComparison.ReceiptLedger.Receipt[] _frames = new RunSoakHudComparison.ReceiptLedger.Receipt[8];

			// Token: 0x02000A1B RID: 2587
			public struct Receipt
			{
				// Token: 0x04004A79 RID: 19065
				public int frame;

				// Token: 0x04004A7A RID: 19066
				public int faceCalls;

				// Token: 0x04004A7B RID: 19067
				public int mainCalls;

				// Token: 0x04004A7C RID: 19068
				public Vector3 mainPosition;

				// Token: 0x04004A7D RID: 19069
				public Quaternion mainRotation;

				// Token: 0x04004A7E RID: 19070
				public float mainFov;
			}
		}

		// Token: 0x02000A1C RID: 2588
		private struct Context
		{
			// Token: 0x04004A7F RID: 19071
			public int frame;

			// Token: 0x04004A80 RID: 19072
			public int chunks;

			// Token: 0x04004A81 RID: 19073
			public int settingsBits;

			// Token: 0x04004A82 RID: 19074
			public bool faceCameraEnabled;

			// Token: 0x04004A83 RID: 19075
			public float playerDrift;
		}

		// Token: 0x02000A1D RID: 2589
		private struct Row
		{
			// Token: 0x04004A84 RID: 19076
			public int frame;

			// Token: 0x04004A85 RID: 19077
			public int phase;

			// Token: 0x04004A86 RID: 19078
			public int activeNpcs;

			// Token: 0x04004A87 RID: 19079
			public int chunks;

			// Token: 0x04004A88 RID: 19080
			public int settingsBits;

			// Token: 0x04004A89 RID: 19081
			public int faceCalls;

			// Token: 0x04004A8A RID: 19082
			public int mainCalls;

			// Token: 0x04004A8B RID: 19083
			public bool faceCameraEnabled;

			// Token: 0x04004A8C RID: 19084
			public double elapsed;

			// Token: 0x04004A8D RID: 19085
			public float milliseconds;

			// Token: 0x04004A8E RID: 19086
			public float playerDrift;

			// Token: 0x04004A8F RID: 19087
			public float cameraPositionDrift;

			// Token: 0x04004A90 RID: 19088
			public float cameraAngleDrift;

			// Token: 0x04004A91 RID: 19089
			public float fieldOfViewDrift;
		}
	}
}
