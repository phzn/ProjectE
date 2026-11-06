using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pathfinding.Drawing
{
	// Token: 0x02000048 RID: 72
	[ExecuteAlways]
	[AddComponentMenu("")]
	public class DrawingManager : MonoBehaviour
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000AE37 File Offset: 0x00009037
		public static DrawingManager instance
		{
			get
			{
				if (DrawingManager._instance == null)
				{
					DrawingManager.Init();
				}
				return DrawingManager._instance;
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000AE50 File Offset: 0x00009050
		public static void Init()
		{
			if (DrawingManager._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("RetainedGizmos")
			{
				hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset)
			};
			DrawingManager._instance = gameObject.AddComponent<DrawingManager>();
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			if (Application.isBatchMode)
			{
				DrawingManager.ignoreAllDrawing = true;
				DrawingManager.gizmoDrawers.Clear();
				DrawingManager.pendingGizmoDrawers.Clear();
				DrawingManager.gizmoDrawerIndices.Clear();
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000AEC1 File Offset: 0x000090C1
		private void RefreshRenderPipelineMode()
		{
			if (((RenderPipelineManager.currentPipeline != null) ? RenderPipelineManager.currentPipeline.GetType() : null) == typeof(UniversalRenderPipeline))
			{
				this.detectedRenderPipeline = DetectedRenderPipeline.URP;
				return;
			}
			this.detectedRenderPipeline = DetectedRenderPipeline.BuiltInOrCustom;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000AEF8 File Offset: 0x000090F8
		private void OnEnable()
		{
			if (DrawingManager._instance == null)
			{
				DrawingManager._instance = this;
			}
			if (DrawingManager._instance != this)
			{
				return;
			}
			this.actuallyEnabled = true;
			if (this.gizmos == null)
			{
				this.gizmos = new DrawingData();
			}
			this.gizmos.frameRedrawScope = new RedrawScope(this.gizmos);
			Draw.builder = this.gizmos.GetBuiltInBuilder(false);
			Draw.ingame_builder = this.gizmos.GetBuiltInBuilder(true);
			this.commandBuffer = new CommandBuffer();
			this.commandBuffer.name = "ALINE Gizmos";
			this.detectedRenderPipeline = DetectedRenderPipeline.BuiltInOrCustom;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.PostRender));
			RenderPipelineManager.beginContextRendering += this.BeginContextRendering;
			RenderPipelineManager.beginCameraRendering += this.BeginCameraRendering;
			RenderPipelineManager.endCameraRendering += this.EndCameraRendering;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000AFED File Offset: 0x000091ED
		private void BeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			this.RefreshRenderPipelineMode();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000AFED File Offset: 0x000091ED
		private void BeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
		{
			this.RefreshRenderPipelineMode();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000AFF8 File Offset: 0x000091F8
		private void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (this.detectedRenderPipeline == DetectedRenderPipeline.URP)
			{
				UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
				if (universalAdditionalCameraData != null)
				{
					ScriptableRenderer scriptableRenderer = universalAdditionalCameraData.scriptableRenderer;
					if (this.renderPassFeature == null)
					{
						this.renderPassFeature = ScriptableObject.CreateInstance<AlineURPRenderPassFeature>();
					}
					this.renderPassFeature.AddRenderPasses(scriptableRenderer);
				}
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000B04C File Offset: 0x0000924C
		private void OnDisable()
		{
			if (!this.actuallyEnabled)
			{
				return;
			}
			this.actuallyEnabled = false;
			this.commandBuffer.Dispose();
			this.commandBuffer = null;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.PostRender));
			RenderPipelineManager.beginContextRendering -= this.BeginContextRendering;
			RenderPipelineManager.beginCameraRendering -= this.BeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= this.EndCameraRendering;
			if (this.gizmos != null)
			{
				Draw.builder.DiscardAndDisposeInternal();
				Draw.ingame_builder.DiscardAndDisposeInternal();
				this.gizmos.ClearData();
			}
			if (this.renderPassFeature != null)
			{
				Object.DestroyImmediate(this.renderPassFeature);
				this.renderPassFeature = null;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000B115 File Offset: 0x00009315
		private void OnEditorUpdate()
		{
			this.framePassed = true;
			this.CleanupIfNoCameraRendered();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B124 File Offset: 0x00009324
		private void Update()
		{
			if (this.actuallyEnabled)
			{
				this.CleanupIfNoCameraRendered();
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000B134 File Offset: 0x00009334
		private void CleanupIfNoCameraRendered()
		{
			if (Time.frameCount > this.lastFrameCount + 1)
			{
				this.CheckFrameTicking();
				this.gizmos.PostRenderCleanup();
			}
			if (Time.realtimeSinceStartup - this.lastFrameTime > 10f)
			{
				Draw.builder.DiscardAndDisposeInternal();
				Draw.ingame_builder.DiscardAndDisposeInternal();
				Draw.builder = this.gizmos.GetBuiltInBuilder(false);
				Draw.ingame_builder = this.gizmos.GetBuiltInBuilder(true);
				this.lastFrameTime = Time.realtimeSinceStartup;
				DrawingManager.AddPendingGizmoDrawers();
				DrawingManager.RemoveDestroyedGizmoDrawers();
			}
			if (Time.frameCount - this.lastFilterFrame > 5)
			{
				this.lastFilterFrame = Time.frameCount;
				DrawingManager.AddPendingGizmoDrawers();
				DrawingManager.RemoveDestroyedGizmoDrawers();
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000B1E4 File Offset: 0x000093E4
		internal void ExecuteCustomRenderPass(ScriptableRenderContext context, Camera camera)
		{
			this.commandBuffer.Clear();
			this.SubmitFrame(camera, new DrawingData.CommandBufferWrapper
			{
				cmd = this.commandBuffer
			}, true);
			context.ExecuteCommandBuffer(this.commandBuffer);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B227 File Offset: 0x00009427
		internal void ExecuteCustomRenderGraphPass(DrawingData.CommandBufferWrapper cmd, Camera camera)
		{
			this.SubmitFrame(camera, cmd, true);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000B232 File Offset: 0x00009432
		private void EndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (this.detectedRenderPipeline == DetectedRenderPipeline.BuiltInOrCustom)
			{
				this.ExecuteCustomRenderPass(context, camera);
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000B244 File Offset: 0x00009444
		private void PostRender(Camera camera)
		{
			this.commandBuffer.Clear();
			this.SubmitFrame(camera, new DrawingData.CommandBufferWrapper
			{
				cmd = this.commandBuffer
			}, false);
			Graphics.ExecuteCommandBuffer(this.commandBuffer);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000B288 File Offset: 0x00009488
		private void CheckFrameTicking()
		{
			if (Time.frameCount != this.lastFrameCount)
			{
				this.framePassed = true;
				this.lastFrameCount = Time.frameCount;
				this.lastFrameTime = Time.realtimeSinceStartup;
				this.previousFrameRedrawScope = this.gizmos.frameRedrawScope;
				this.gizmos.frameRedrawScope = new RedrawScope(this.gizmos);
				Draw.builder.DisposeInternal();
				Draw.ingame_builder.DisposeInternal();
				Draw.builder = this.gizmos.GetBuiltInBuilder(false);
				Draw.ingame_builder = this.gizmos.GetBuiltInBuilder(true);
			}
			else if (this.framePassed && Application.isPlaying)
			{
				this.previousFrameRedrawScope.Draw();
			}
			if (this.framePassed)
			{
				this.gizmos.TickFramePreRender();
				this.framePassed = false;
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000B354 File Offset: 0x00009554
		internal void SubmitFrame(Camera camera, DrawingData.CommandBufferWrapper cmd, bool usingRenderPipeline)
		{
			bool flag = false;
			bool flag2 = DrawingManager.allowRenderToRenderTextures || DrawingManager.drawToAllCameras || camera.targetTexture == null || flag;
			this.CheckFrameTicking();
			this.Submit(camera, cmd, usingRenderPipeline, flag2);
			this.gizmos.PostRenderCleanup();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000B39E File Offset: 0x0000959E
		private bool ShouldDrawGizmos(Object obj)
		{
			return true;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000B3A4 File Offset: 0x000095A4
		private static void AddPendingGizmoDrawers()
		{
			for (int i = 0; i < DrawingManager.pendingGizmoDrawers.Count; i++)
			{
				ValueTuple<Type, IDrawGizmos> valueTuple = DrawingManager.pendingGizmoDrawers[i];
				Type item = valueTuple.Item1;
				IDrawGizmos item2 = valueTuple.Item2;
				int gizmoDrawerIndex = DrawingManager.GetGizmoDrawerIndex(item);
				if (gizmoDrawerIndex != -1)
				{
					MonoBehaviour monoBehaviour = item2 as MonoBehaviour;
					if (monoBehaviour == null || monoBehaviour)
					{
						DrawingManager.gizmoDrawers[gizmoDrawerIndex].drawers.Add(item2);
					}
				}
			}
			DrawingManager.pendingGizmoDrawers.Clear();
			if (DrawingManager.pendingGizmoDrawers.Capacity > 1024)
			{
				DrawingManager.pendingGizmoDrawers.Capacity = 4;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000B43C File Offset: 0x0000963C
		private static void RemoveDestroyedGizmoDrawers()
		{
			for (int i = 0; i < DrawingManager.gizmoDrawers.Count; i++)
			{
				DrawingManager.GizmoDrawerGroup gizmoDrawerGroup = DrawingManager.gizmoDrawers[i];
				int num = 0;
				for (int j = 0; j < gizmoDrawerGroup.drawers.Count; j++)
				{
					IDrawGizmos drawGizmos = gizmoDrawerGroup.drawers[j];
					if (drawGizmos as MonoBehaviour || (!(drawGizmos is MonoBehaviour) && drawGizmos.Exists))
					{
						gizmoDrawerGroup.drawers[num] = drawGizmos;
						num++;
					}
				}
				gizmoDrawerGroup.drawers.RemoveRange(num, gizmoDrawerGroup.drawers.Count - num);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000B4E4 File Offset: 0x000096E4
		public static bool ShouldDrawGizmos(Type type)
		{
			int gizmoDrawerIndex = DrawingManager.GetGizmoDrawerIndex(type);
			return gizmoDrawerIndex != -1 && DrawingManager.gizmoDrawers[gizmoDrawerIndex].enabled;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000B510 File Offset: 0x00009710
		private void Submit(Camera camera, DrawingData.CommandBufferWrapper cmd, bool usingRenderPipeline, bool allowCameraDefault)
		{
			bool flag = false;
			Draw.builder.DisposeInternal();
			Draw.ingame_builder.DisposeInternal();
			this.gizmos.Render(camera, flag, cmd, allowCameraDefault);
			Draw.builder = this.gizmos.GetBuiltInBuilder(false);
			Draw.ingame_builder = this.gizmos.GetBuiltInBuilder(true);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000B565 File Offset: 0x00009765
		public static void Register(IDrawGizmos item)
		{
			DrawingManager.Register(item, item.GetType());
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000B573 File Offset: 0x00009773
		public static void Register(IDrawGizmos item, Type overrideType)
		{
			if (!DrawingManager.ignoreAllDrawing)
			{
				DrawingManager.pendingGizmoDrawers.Add(new ValueTuple<Type, IDrawGizmos>(overrideType, item));
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000B590 File Offset: 0x00009790
		private static int GetGizmoDrawerIndex(Type tp)
		{
			int num;
			if (!DrawingManager.gizmoDrawerIndices.TryGetValue(tp, out num))
			{
				BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				MethodInfo methodInfo;
				if ((methodInfo = tp.GetMethod("DrawGizmos", bindingFlags)) == null)
				{
					methodInfo = tp.GetMethod("Pathfinding.Drawing.IDrawGizmos.DrawGizmos", bindingFlags) ?? tp.GetMethod("Drawing.IDrawGizmos.DrawGizmos", bindingFlags);
				}
				MethodInfo methodInfo2 = methodInfo;
				if (methodInfo2 == null)
				{
					throw new Exception("Could not find the DrawGizmos method in type " + tp.Name);
				}
				if (methodInfo2.DeclaringType != typeof(MonoBehaviourGizmos))
				{
					num = (DrawingManager.gizmoDrawerIndices[tp] = DrawingManager.gizmoDrawers.Count);
					DrawingManager.gizmoDrawers.Add(new DrawingManager.GizmoDrawerGroup
					{
						type = tp,
						enabled = true,
						drawers = new List<IDrawGizmos>(),
						profilerMarker = new ProfilerMarker(ProfilerCategory.Render, "Gizmos for " + tp.Name)
					});
				}
				else
				{
					num = (DrawingManager.gizmoDrawerIndices[tp] = -1);
				}
			}
			return num;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000B68F File Offset: 0x0000988F
		public static CommandBuilder GetBuilder(bool renderInGame = false)
		{
			return DrawingManager.instance.gizmos.GetBuilder(renderInGame);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000B6A1 File Offset: 0x000098A1
		public static CommandBuilder GetBuilder(RedrawScope redrawScope, bool renderInGame = false)
		{
			return DrawingManager.instance.gizmos.GetBuilder(redrawScope, renderInGame);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000B6B4 File Offset: 0x000098B4
		public static CommandBuilder GetBuilder(DrawingData.Hasher hasher, RedrawScope redrawScope = default(RedrawScope), bool renderInGame = false)
		{
			return DrawingManager.instance.gizmos.GetBuilder(hasher, redrawScope, renderInGame);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000B6C8 File Offset: 0x000098C8
		public static bool TryDrawHasher(DrawingData.Hasher hasher, RedrawScope redrawScope = default(RedrawScope))
		{
			return DrawingManager.instance.gizmos.Draw(hasher, redrawScope);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000B6DC File Offset: 0x000098DC
		public static RedrawScope GetRedrawScope(GameObject associatedGameObject = null)
		{
			RedrawScope redrawScope = new RedrawScope(DrawingManager.instance.gizmos);
			redrawScope.DrawUntilDispose(associatedGameObject);
			return redrawScope;
		}

		// Token: 0x04000108 RID: 264
		public DrawingData gizmos;

		// Token: 0x04000109 RID: 265
		private static List<DrawingManager.GizmoDrawerGroup> gizmoDrawers = new List<DrawingManager.GizmoDrawerGroup>();

		// Token: 0x0400010A RID: 266
		private static List<ValueTuple<Type, IDrawGizmos>> pendingGizmoDrawers = new List<ValueTuple<Type, IDrawGizmos>>();

		// Token: 0x0400010B RID: 267
		private static Dictionary<Type, int> gizmoDrawerIndices = new Dictionary<Type, int>();

		// Token: 0x0400010C RID: 268
		private static bool ignoreAllDrawing;

		// Token: 0x0400010D RID: 269
		private static DrawingManager _instance;

		// Token: 0x0400010E RID: 270
		private bool framePassed;

		// Token: 0x0400010F RID: 271
		private int lastFrameCount = int.MinValue;

		// Token: 0x04000110 RID: 272
		private float lastFrameTime = float.NegativeInfinity;

		// Token: 0x04000111 RID: 273
		private int lastFilterFrame;

		// Token: 0x04000112 RID: 274
		[SerializeField]
		private bool actuallyEnabled;

		// Token: 0x04000113 RID: 275
		private RedrawScope previousFrameRedrawScope;

		// Token: 0x04000114 RID: 276
		public static bool allowRenderToRenderTextures = false;

		// Token: 0x04000115 RID: 277
		public static bool drawToAllCameras = false;

		// Token: 0x04000116 RID: 278
		public static float lineWidthMultiplier = 1f;

		// Token: 0x04000117 RID: 279
		private CommandBuffer commandBuffer;

		// Token: 0x04000118 RID: 280
		[NonSerialized]
		private DetectedRenderPipeline detectedRenderPipeline;

		// Token: 0x04000119 RID: 281
		private HashSet<ScriptableRenderer> scriptableRenderersWithPass = new HashSet<ScriptableRenderer>();

		// Token: 0x0400011A RID: 282
		private AlineURPRenderPassFeature renderPassFeature;

		// Token: 0x0400011B RID: 283
		private static readonly ProfilerMarker MarkerALINE = new ProfilerMarker("ALINE");

		// Token: 0x0400011C RID: 284
		private static readonly ProfilerMarker MarkerCommandBuffer = new ProfilerMarker("Executing command buffer");

		// Token: 0x0400011D RID: 285
		private static readonly ProfilerMarker MarkerFrameTick = new ProfilerMarker("Frame Tick");

		// Token: 0x0400011E RID: 286
		private static readonly ProfilerMarker MarkerFilterDestroyedObjects = new ProfilerMarker("Filter destroyed objects");

		// Token: 0x0400011F RID: 287
		internal static readonly ProfilerMarker MarkerRefreshSelectionCache = new ProfilerMarker("Refresh Selection Cache");

		// Token: 0x04000120 RID: 288
		private static readonly ProfilerMarker MarkerGizmosAllowed = new ProfilerMarker("GizmosAllowed");

		// Token: 0x04000121 RID: 289
		private static readonly ProfilerMarker MarkerDrawGizmos = new ProfilerMarker("DrawGizmos");

		// Token: 0x04000122 RID: 290
		private static readonly ProfilerMarker MarkerSubmitGizmos = new ProfilerMarker("Submit Gizmos");

		// Token: 0x04000123 RID: 291
		private const float NO_DRAWING_TIMEOUT_SECS = 10f;

		// Token: 0x02000049 RID: 73
		private struct GizmoDrawerGroup
		{
			// Token: 0x04000124 RID: 292
			public Type type;

			// Token: 0x04000125 RID: 293
			public ProfilerMarker profilerMarker;

			// Token: 0x04000126 RID: 294
			public List<IDrawGizmos> drawers;

			// Token: 0x04000127 RID: 295
			public bool enabled;
		}
	}
}
