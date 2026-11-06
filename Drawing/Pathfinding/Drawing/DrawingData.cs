using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Pathfinding.Drawing.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pathfinding.Drawing
{
	// Token: 0x0200002C RID: 44
	public class DrawingData
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00008609 File Offset: 0x00006809
		private int adjustedSceneModeVersion
		{
			get
			{
				return this.sceneModeVersion + (Application.isPlaying ? 1000 : 0);
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008621 File Offset: 0x00006821
		internal int GetNextDrawOrderIndex()
		{
			this.currentDrawOrderIndex++;
			return this.currentDrawOrderIndex;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00008637 File Offset: 0x00006837
		internal void PoolMesh(Mesh mesh)
		{
			this.stagingCachedMeshes.Add(mesh);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008645 File Offset: 0x00006845
		private void SortPooledMeshes()
		{
			this.cachedMeshes.Sort((Mesh a, Mesh b) => b.vertexCount - a.vertexCount);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008674 File Offset: 0x00006874
		internal Mesh GetMesh(int desiredVertexCount)
		{
			if (this.cachedMeshes.Count > 0)
			{
				int num = 0;
				int i = this.cachedMeshes.Count;
				while (i > num + 1)
				{
					int num2 = (num + i) / 2;
					if (this.cachedMeshes[num2].vertexCount < desiredVertexCount)
					{
						i = num2;
					}
					else
					{
						num = num2;
					}
				}
				Mesh mesh = this.cachedMeshes[num];
				if (num == 0)
				{
					this.lastTimeLargestCachedMeshWasUsed = this.version;
				}
				this.cachedMeshes.RemoveAt(num);
				return mesh;
			}
			Mesh mesh2 = new Mesh();
			mesh2.hideFlags = HideFlags.DontSave;
			mesh2.MarkDynamic();
			return mesh2;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008700 File Offset: 0x00006900
		internal void LoadFontDataIfNecessary()
		{
			if (this.fontData.material == null)
			{
				SDFFont sdffont = DefaultFonts.LoadDefaultFont();
				this.fontData.Dispose();
				this.fontData = new SDFLookupData(sdffont);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000873D File Offset: 0x0000693D
		private static float CurrentTime
		{
			get
			{
				if (!Application.isPlaying)
				{
					return Time.realtimeSinceStartup;
				}
				return Time.time;
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008751 File Offset: 0x00006951
		private unsafe static void UpdateTime()
		{
			*SharedDrawingData.BurstTime.Data = DrawingData.CurrentTime;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008764 File Offset: 0x00006964
		public CommandBuilder GetBuilder(bool renderInGame = false)
		{
			DrawingData.UpdateTime();
			return new CommandBuilder(this, DrawingData.Hasher.NotSupplied, this.frameRedrawScope, default(RedrawScope), !renderInGame, false, this.adjustedSceneModeVersion);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000879C File Offset: 0x0000699C
		internal CommandBuilder GetBuiltInBuilder(bool renderInGame = false)
		{
			DrawingData.UpdateTime();
			return new CommandBuilder(this, DrawingData.Hasher.NotSupplied, this.frameRedrawScope, default(RedrawScope), !renderInGame, true, this.adjustedSceneModeVersion);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000087D3 File Offset: 0x000069D3
		public CommandBuilder GetBuilder(RedrawScope redrawScope, bool renderInGame = false)
		{
			DrawingData.UpdateTime();
			return new CommandBuilder(this, DrawingData.Hasher.NotSupplied, this.frameRedrawScope, redrawScope, !renderInGame, false, this.adjustedSceneModeVersion);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000087F7 File Offset: 0x000069F7
		public CommandBuilder GetBuilder(DrawingData.Hasher hasher, RedrawScope redrawScope = default(RedrawScope), bool renderInGame = false)
		{
			if (!hasher.Equals(DrawingData.Hasher.NotSupplied))
			{
				this.DiscardData(hasher);
			}
			DrawingData.UpdateTime();
			return new CommandBuilder(this, hasher, this.frameRedrawScope, redrawScope, !renderInGame, false, this.adjustedSceneModeVersion);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000882C File Offset: 0x00006A2C
		public DrawingSettings.Settings settingsRef
		{
			get
			{
				if (this.settings == null)
				{
					this.settings = DrawingSettings.DefaultSettings;
				}
				return this.settings;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00008847 File Offset: 0x00006A47
		// (set) Token: 0x060001CF RID: 463 RVA: 0x0000884F File Offset: 0x00006A4F
		public int version { get; private set; } = 1;

		// Token: 0x060001D0 RID: 464 RVA: 0x00008858 File Offset: 0x00006A58
		public GameObject GetAssociatedGameObject(RedrawScope scope)
		{
			GameObject gameObject;
			if (this.persistentRedrawScopes.TryGetValue(scope.id, out gameObject))
			{
				return gameObject;
			}
			return null;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000887D File Offset: 0x00006A7D
		private void DiscardData(DrawingData.Hasher hasher)
		{
			this.processedData.ReleaseAllWithHash(this, hasher);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000888C File Offset: 0x00006A8C
		internal void OnChangingPlayMode()
		{
			this.sceneModeVersion++;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000889C File Offset: 0x00006A9C
		public bool Draw(DrawingData.Hasher hasher)
		{
			if (hasher.Equals(DrawingData.Hasher.NotSupplied))
			{
				throw new ArgumentException("Invalid hash value");
			}
			return this.processedData.SetVersion(hasher, this.version);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000088CC File Offset: 0x00006ACC
		public bool Draw(DrawingData.Hasher hasher, RedrawScope scope)
		{
			if (hasher.Equals(DrawingData.Hasher.NotSupplied))
			{
				throw new ArgumentException("Invalid hash value");
			}
			if (scope.isValid)
			{
				this.processedData.SetCustomScope(hasher, scope);
			}
			return this.processedData.SetVersion(hasher, this.version);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000891B File Offset: 0x00006B1B
		internal void Draw(RedrawScope scope)
		{
			if (scope.isValid)
			{
				this.processedData.SetVersion(scope, this.version);
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008939 File Offset: 0x00006B39
		internal void DrawUntilDisposed(RedrawScope scope, GameObject associatedGameObject)
		{
			if (scope.isValid)
			{
				this.Draw(scope);
				this.persistentRedrawScopes.Add(scope.id, associatedGameObject);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000895D File Offset: 0x00006B5D
		internal void DisposeRedrawScope(RedrawScope scope)
		{
			if (scope.isValid)
			{
				this.processedData.SetVersion(scope, -1);
				this.persistentRedrawScopes.Remove(scope.id);
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00008988 File Offset: 0x00006B88
		private void RefreshRedrawScopes()
		{
			foreach (KeyValuePair<int, GameObject> keyValuePair in this.persistentRedrawScopes)
			{
				this.processedData.SetVersion(new RedrawScope(this, keyValuePair.Key), this.version);
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000089F4 File Offset: 0x00006BF4
		private void CleanupOldCameras()
		{
			foreach (KeyValuePair<Camera, DrawingData.Range> keyValuePair in this.cameraVersions)
			{
				if (keyValuePair.Value.end < this.lastTickVersion - 10)
				{
					this.cameraVersions.Remove(keyValuePair.Key);
					break;
				}
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008A6C File Offset: 0x00006C6C
		public void DisposeCommandBuildersWithJobDependencies()
		{
			this.data.DisposeCommandBuildersWithJobDependencies(this);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00008A7C File Offset: 0x00006C7C
		public void TickFramePreRender()
		{
			this.DisposeCommandBuildersWithJobDependencies();
			this.processedData.FilterOldPersistentCommands(this.version, this.lastTickVersion, DrawingData.CurrentTime, this.adjustedSceneModeVersion);
			this.CleanupOldCameras();
			this.RefreshRedrawScopes();
			this.processedData.ReleaseDataOlderThan(this, this.lastTickVersion2 + 1);
			this.lastTickVersion2 = this.lastTickVersion;
			this.lastTickVersion = this.version;
			this.currentDrawOrderIndex = 0;
			this.cachedMeshes.AddRange(this.stagingCachedMeshes);
			this.stagingCachedMeshes.Clear();
			this.SortPooledMeshes();
			if (this.version - this.lastTimeLargestCachedMeshWasUsed > 60 && this.cachedMeshes.Count > 0)
			{
				Object.DestroyImmediate(this.cachedMeshes[0]);
				this.cachedMeshes.RemoveAt(0);
				this.lastTimeLargestCachedMeshWasUsed = this.version;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00008B5C File Offset: 0x00006D5C
		public void PostRenderCleanup()
		{
			this.data.ReleaseAllUnused();
			int version = this.version;
			this.version = version + 1;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008B84 File Offset: 0x00006D84
		private static int MeshCompareByDrawingOrder(DrawingData.RenderedMeshWithType a, DrawingData.RenderedMeshWithType b)
		{
			int num = (int)(a.type & DrawingData.MeshType.BaseType);
			int num2 = (int)(b.type & DrawingData.MeshType.BaseType);
			if (num == num2)
			{
				return a.drawingOrderIndex - b.drawingOrderIndex;
			}
			return num - num2;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00008BB8 File Offset: 0x00006DB8
		private int totalMemoryUsage
		{
			get
			{
				return this.data.memoryUsage + this.processedData.memoryUsage;
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008BD4 File Offset: 0x00006DD4
		private void LoadMaterials()
		{
			if (this.surfaceMaterial == null)
			{
				this.surfaceMaterial = Resources.Load<Material>("aline_surface");
			}
			if (this.lineMaterial == null)
			{
				this.lineMaterial = Resources.Load<Material>("aline_outline");
			}
			if (this.fontData.material == null)
			{
				SDFFont sdffont = DefaultFonts.LoadDefaultFont();
				this.fontData.Dispose();
				this.fontData = new SDFLookupData(sdffont);
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008C50 File Offset: 0x00006E50
		public DrawingData()
		{
			this.gizmosHandle = GCHandle.Alloc(this, GCHandleType.Weak);
			this.LoadMaterials();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008CCB File Offset: 0x00006ECB
		private static int CeilLog2(int x)
		{
			return (int)math.ceil(math.log2((float)x));
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008CDC File Offset: 0x00006EDC
		public void Render(Camera cam, bool allowGizmos, DrawingData.CommandBufferWrapper commandBuffer, bool allowCameraDefault)
		{
			if (this.processedData.isEmpty)
			{
				return;
			}
			this.LoadMaterials();
			if (this.surfaceMaterial == null || this.lineMaterial == null)
			{
				return;
			}
			DrawingData.Range range;
			if (!this.cameraVersions.TryGetValue(cam, out range))
			{
				range = new DrawingData.Range
				{
					start = int.MinValue,
					end = int.MinValue
				};
			}
			if (range.end > this.lastTickVersion)
			{
				range.end = this.version + 1;
			}
			else
			{
				range = new DrawingData.Range
				{
					start = range.end,
					end = this.version + 1
				};
			}
			range.start = Mathf.Max(range.start, this.lastTickVersion2 + 1);
			DrawingSettings.Settings settingsRef = this.settingsRef;
			if (!GL.wireframe)
			{
				this.processedData.SubmitMeshes(this, cam, range.start, allowGizmos, allowCameraDefault);
				this.meshes.Clear();
				this.processedData.CollectMeshes(range.start, this.meshes, cam, allowGizmos, allowCameraDefault);
				this.processedData.PoolDynamicMeshes(this);
				if (this.meshes.Count > 0)
				{
					this.meshes.Sort(DrawingData.meshSorter);
					Plane[] array = this.frustrumPlanes;
					GeometryUtility.CalculateFrustumPlanes(cam, array);
					int num = Shader.PropertyToID("_Color");
					int num2 = Shader.PropertyToID("_FadeColor");
					Color color = new Color(1f, 1f, 1f, settingsRef.solidOpacity);
					Color color2 = new Color(1f, 1f, 1f, settingsRef.solidOpacityBehindObjects);
					Color color3 = new Color(1f, 1f, 1f, settingsRef.lineOpacity);
					Color color4 = new Color(1f, 1f, 1f, settingsRef.lineOpacityBehindObjects);
					Color color5 = new Color(1f, 1f, 1f, settingsRef.textOpacity);
					Color color6 = new Color(1f, 1f, 1f, settingsRef.textOpacityBehindObjects);
					int i = 0;
					while (i < this.meshes.Count)
					{
						int num3 = i + 1;
						DrawingData.MeshType meshType = this.meshes[i].type & DrawingData.MeshType.BaseType;
						while (num3 < this.meshes.Count && (this.meshes[num3].type & DrawingData.MeshType.BaseType) == meshType)
						{
							num3++;
						}
						this.customMaterialProperties.Clear();
						Material material;
						switch (meshType)
						{
						case DrawingData.MeshType.Solid:
							material = this.surfaceMaterial;
							this.customMaterialProperties.SetColor(num, color);
							this.customMaterialProperties.SetColor(num2, color2);
							break;
						case DrawingData.MeshType.Lines:
							material = this.lineMaterial;
							this.customMaterialProperties.SetColor(num, color3);
							this.customMaterialProperties.SetColor(num2, color4);
							break;
						case DrawingData.MeshType.Solid | DrawingData.MeshType.Lines:
							goto IL_0300;
						case DrawingData.MeshType.Text:
							material = this.fontData.material;
							this.customMaterialProperties.SetColor(num, color5);
							this.customMaterialProperties.SetColor(num2, color6);
							break;
						default:
							goto IL_0300;
						}
						for (int j = 0; j < material.passCount; j++)
						{
							for (int k = i; k < num3; k++)
							{
								DrawingData.RenderedMeshWithType renderedMeshWithType = this.meshes[k];
								if ((renderedMeshWithType.type & DrawingData.MeshType.Custom) != (DrawingData.MeshType)0)
								{
									if (GeometryUtility.TestPlanesAABB(array, DrawingData.TransformBoundingBox(renderedMeshWithType.matrix, renderedMeshWithType.mesh.bounds)))
									{
										this.customMaterialProperties.SetColor(num, color * renderedMeshWithType.color);
										commandBuffer.DrawMesh(renderedMeshWithType.mesh, renderedMeshWithType.matrix, material, 0, j, this.customMaterialProperties);
										this.customMaterialProperties.SetColor(num, color);
									}
								}
								else if (GeometryUtility.TestPlanesAABB(array, renderedMeshWithType.mesh.bounds))
								{
									commandBuffer.DrawMesh(renderedMeshWithType.mesh, Matrix4x4.identity, material, 0, j, this.customMaterialProperties);
								}
							}
						}
						i = num3;
						continue;
						IL_0300:
						throw new InvalidOperationException("Invalid mesh type");
					}
					this.meshes.Clear();
				}
			}
			this.cameraVersions[cam] = range;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009110 File Offset: 0x00007310
		private static Bounds TransformBoundingBox(Matrix4x4 matrix, Bounds bounds)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			Bounds bounds2 = new Bounds(matrix.MultiplyPoint(min), Vector3.zero);
			bounds2.Encapsulate(matrix.MultiplyPoint(new Vector3(min.x, min.y, max.z)));
			bounds2.Encapsulate(matrix.MultiplyPoint(new Vector3(min.x, max.y, min.z)));
			bounds2.Encapsulate(matrix.MultiplyPoint(new Vector3(min.x, max.y, max.z)));
			bounds2.Encapsulate(matrix.MultiplyPoint(new Vector3(max.x, min.y, min.z)));
			bounds2.Encapsulate(matrix.MultiplyPoint(new Vector3(max.x, min.y, max.z)));
			bounds2.Encapsulate(matrix.MultiplyPoint(new Vector3(max.x, max.y, min.z)));
			bounds2.Encapsulate(matrix.MultiplyPoint(new Vector3(max.x, max.y, max.z)));
			return bounds2;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009248 File Offset: 0x00007448
		public void ClearData()
		{
			this.gizmosHandle.Free();
			this.data.Dispose();
			this.processedData.Dispose(this);
			for (int i = 0; i < this.cachedMeshes.Count; i++)
			{
				Object.DestroyImmediate(this.cachedMeshes[i]);
			}
			this.cachedMeshes.Clear();
			this.fontData.Dispose();
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009368 File Offset: 0x00007568
		public static void Initialize$BuilderData_AnyBuffersWrittenTo_000001EA$BurstDirectCall()
		{
			DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.Initialize();
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000936F File Offset: 0x0000756F
		public static void Initialize$BuilderData_ResetAllBuffers_000001EB$BurstDirectCall()
		{
			DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.Initialize();
		}

		// Token: 0x04000087 RID: 135
		internal DrawingData.BuilderDataContainer data;

		// Token: 0x04000088 RID: 136
		internal DrawingData.ProcessedBuilderDataContainer processedData;

		// Token: 0x04000089 RID: 137
		private List<DrawingData.RenderedMeshWithType> meshes = new List<DrawingData.RenderedMeshWithType>();

		// Token: 0x0400008A RID: 138
		private List<Mesh> cachedMeshes = new List<Mesh>();

		// Token: 0x0400008B RID: 139
		private List<Mesh> stagingCachedMeshes = new List<Mesh>();

		// Token: 0x0400008C RID: 140
		private int lastTimeLargestCachedMeshWasUsed;

		// Token: 0x0400008D RID: 141
		internal SDFLookupData fontData;

		// Token: 0x0400008E RID: 142
		private int currentDrawOrderIndex;

		// Token: 0x0400008F RID: 143
		internal int sceneModeVersion;

		// Token: 0x04000090 RID: 144
		public Material surfaceMaterial;

		// Token: 0x04000091 RID: 145
		public Material lineMaterial;

		// Token: 0x04000092 RID: 146
		public Material textMaterial;

		// Token: 0x04000093 RID: 147
		public DrawingSettings.Settings settings;

		// Token: 0x04000095 RID: 149
		private int lastTickVersion;

		// Token: 0x04000096 RID: 150
		private int lastTickVersion2;

		// Token: 0x04000097 RID: 151
		private Dictionary<int, GameObject> persistentRedrawScopes = new Dictionary<int, GameObject>();

		// Token: 0x04000098 RID: 152
		internal GCHandle gizmosHandle;

		// Token: 0x04000099 RID: 153
		public RedrawScope frameRedrawScope;

		// Token: 0x0400009A RID: 154
		private Dictionary<Camera, DrawingData.Range> cameraVersions = new Dictionary<Camera, DrawingData.Range>();

		// Token: 0x0400009B RID: 155
		internal static readonly ProfilerMarker MarkerScheduleJobs = new ProfilerMarker("ScheduleJobs");

		// Token: 0x0400009C RID: 156
		internal static readonly ProfilerMarker MarkerAwaitUserDependencies = new ProfilerMarker("Await user dependencies");

		// Token: 0x0400009D RID: 157
		internal static readonly ProfilerMarker MarkerSchedule = new ProfilerMarker("Schedule");

		// Token: 0x0400009E RID: 158
		internal static readonly ProfilerMarker MarkerBuild = new ProfilerMarker("Build");

		// Token: 0x0400009F RID: 159
		internal static readonly ProfilerMarker MarkerPool = new ProfilerMarker("Pool");

		// Token: 0x040000A0 RID: 160
		internal static readonly ProfilerMarker MarkerRelease = new ProfilerMarker("Release");

		// Token: 0x040000A1 RID: 161
		internal static readonly ProfilerMarker MarkerBuildMeshes = new ProfilerMarker("Build Meshes");

		// Token: 0x040000A2 RID: 162
		internal static readonly ProfilerMarker MarkerCollectMeshes = new ProfilerMarker("Collect Meshes");

		// Token: 0x040000A3 RID: 163
		internal static readonly ProfilerMarker MarkerSortMeshes = new ProfilerMarker("Sort Meshes");

		// Token: 0x040000A4 RID: 164
		internal static readonly ProfilerMarker LeakTracking = new ProfilerMarker("RedrawScope Leak Tracking");

		// Token: 0x040000A5 RID: 165
		private static readonly Comparison<DrawingData.RenderedMeshWithType> meshSorter = new Comparison<DrawingData.RenderedMeshWithType>(DrawingData.MeshCompareByDrawingOrder);

		// Token: 0x040000A6 RID: 166
		private Plane[] frustrumPlanes = new Plane[6];

		// Token: 0x040000A7 RID: 167
		private MaterialPropertyBlock customMaterialProperties = new MaterialPropertyBlock();

		// Token: 0x0200002D RID: 45
		public struct Hasher : IEquatable<DrawingData.Hasher>
		{
			// Token: 0x1700000F RID: 15
			// (get) Token: 0x060001E8 RID: 488 RVA: 0x00009378 File Offset: 0x00007578
			public static DrawingData.Hasher NotSupplied
			{
				get
				{
					return new DrawingData.Hasher
					{
						hash = ulong.MaxValue
					};
				}
			}

			// Token: 0x060001E9 RID: 489 RVA: 0x00009398 File Offset: 0x00007598
			[Obsolete("Use the constructor instead")]
			public static DrawingData.Hasher Create<T>(T init)
			{
				DrawingData.Hasher hasher = default(DrawingData.Hasher);
				hasher.Add<T>(init);
				return hasher;
			}

			// Token: 0x060001EA RID: 490 RVA: 0x000093B6 File Offset: 0x000075B6
			public void Add<T>(T hash)
			{
				this.hash = (1572869UL * this.hash) ^ (ulong)((long)hash.GetHashCode() + 12289L);
			}

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x060001EB RID: 491 RVA: 0x000093E1 File Offset: 0x000075E1
			public readonly ulong Hash
			{
				get
				{
					return this.hash;
				}
			}

			// Token: 0x060001EC RID: 492 RVA: 0x000093E9 File Offset: 0x000075E9
			public override int GetHashCode()
			{
				return (int)this.hash;
			}

			// Token: 0x060001ED RID: 493 RVA: 0x000093F2 File Offset: 0x000075F2
			public bool Equals(DrawingData.Hasher other)
			{
				return this.hash == other.hash;
			}

			// Token: 0x040000A8 RID: 168
			private ulong hash;
		}

		// Token: 0x0200002E RID: 46
		internal struct ProcessedBuilderData
		{
			// Token: 0x17000011 RID: 17
			// (get) Token: 0x060001EE RID: 494 RVA: 0x00009402 File Offset: 0x00007602
			public bool isValid
			{
				get
				{
					return this.type > DrawingData.ProcessedBuilderData.Type.Invalid;
				}
			}

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x060001EF RID: 495 RVA: 0x0000940D File Offset: 0x0000760D
			public unsafe UnsafeAppendBuffer* splitterOutputPtr
			{
				get
				{
					return &((DrawingData.ProcessedBuilderData.MeshBuffers*)this.temporaryMeshBuffers.GetUnsafePtr<DrawingData.ProcessedBuilderData.MeshBuffers>())->splitterOutput;
				}
			}

			// Token: 0x060001F0 RID: 496 RVA: 0x00009420 File Offset: 0x00007620
			public void Init(DrawingData.ProcessedBuilderData.Type type, DrawingData.BuilderData.Meta meta)
			{
				this.submitted = false;
				this.type = type;
				this.meta = meta;
				if (this.meshes == null)
				{
					this.meshes = new List<DrawingData.MeshWithType>();
				}
				if (!this.temporaryMeshBuffers.IsCreated)
				{
					this.temporaryMeshBuffers = new NativeArray<DrawingData.ProcessedBuilderData.MeshBuffers>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					this.temporaryMeshBuffers[0] = new DrawingData.ProcessedBuilderData.MeshBuffers(Allocator.Persistent);
				}
			}

			// Token: 0x060001F1 RID: 497 RVA: 0x00009484 File Offset: 0x00007684
			public unsafe void SetSplitterJob(DrawingData gizmos, JobHandle splitterJob)
			{
				this.splitterJob = splitterJob;
				if (this.type == DrawingData.ProcessedBuilderData.Type.Static)
				{
					GeometryBuilder.CameraInfo cameraInfo = new GeometryBuilder.CameraInfo(null);
					this.buildJob = GeometryBuilder.Build(gizmos, (DrawingData.ProcessedBuilderData.MeshBuffers*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<DrawingData.ProcessedBuilderData.MeshBuffers>(this.temporaryMeshBuffers), ref cameraInfo, splitterJob);
					DrawingData.ProcessedBuilderData.SubmittedJobs++;
					if (DrawingData.ProcessedBuilderData.SubmittedJobs % 8 == 0)
					{
						JobHandle.ScheduleBatchedJobs();
					}
				}
			}

			// Token: 0x060001F2 RID: 498 RVA: 0x000094E0 File Offset: 0x000076E0
			public unsafe void SchedulePersistFilter(int version, int lastTickVersion, float time, int sceneModeVersion)
			{
				if (this.type != DrawingData.ProcessedBuilderData.Type.Persistent)
				{
					throw new InvalidOperationException();
				}
				if (this.meta.sceneModeVersion != sceneModeVersion)
				{
					this.meta.version = -1;
					return;
				}
				if (this.meta.version < lastTickVersion || this.submitted)
				{
					this.splitterJob.Complete();
					this.meta.version = version;
					if (this.temporaryMeshBuffers[0].splitterOutput.Length == 0)
					{
						this.meta.version = -1;
						return;
					}
					this.buildJob.Complete();
					this.splitterJob = new PersistentFilterJob
					{
						buffer = &((DrawingData.ProcessedBuilderData.MeshBuffers*)this.temporaryMeshBuffers.GetUnsafePtr<DrawingData.ProcessedBuilderData.MeshBuffers>())->splitterOutput,
						time = time
					}.Schedule(this.splitterJob);
				}
			}

			// Token: 0x060001F3 RID: 499 RVA: 0x000095B2 File Offset: 0x000077B2
			public bool IsValidForCamera(Camera camera, bool allowGizmos, bool allowCameraDefault)
			{
				if (!allowGizmos && this.meta.isGizmos)
				{
					return false;
				}
				if (this.meta.cameraTargets != null)
				{
					return this.meta.cameraTargets.Contains(camera);
				}
				return allowCameraDefault;
			}

			// Token: 0x060001F4 RID: 500 RVA: 0x000095E6 File Offset: 0x000077E6
			public unsafe void Schedule(DrawingData gizmos, ref GeometryBuilder.CameraInfo cameraInfo)
			{
				if (this.type != DrawingData.ProcessedBuilderData.Type.Static)
				{
					this.buildJob = GeometryBuilder.Build(gizmos, (DrawingData.ProcessedBuilderData.MeshBuffers*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<DrawingData.ProcessedBuilderData.MeshBuffers>(this.temporaryMeshBuffers), ref cameraInfo, this.splitterJob);
				}
			}

			// Token: 0x060001F5 RID: 501 RVA: 0x0000960F File Offset: 0x0000780F
			public unsafe void BuildMeshes(DrawingData gizmos)
			{
				if (this.type == DrawingData.ProcessedBuilderData.Type.Static && this.submitted)
				{
					return;
				}
				this.buildJob.Complete();
				GeometryBuilder.BuildMesh(gizmos, this.meshes, (DrawingData.ProcessedBuilderData.MeshBuffers*)this.temporaryMeshBuffers.GetUnsafePtr<DrawingData.ProcessedBuilderData.MeshBuffers>());
				this.submitted = true;
			}

			// Token: 0x060001F6 RID: 502 RVA: 0x0000964C File Offset: 0x0000784C
			public unsafe void CollectMeshes(List<DrawingData.RenderedMeshWithType> meshes)
			{
				List<DrawingData.MeshWithType> list = this.meshes;
				int num = 0;
				UnsafeAppendBuffer capturedState = this.temporaryMeshBuffers[0].capturedState;
				int num2 = capturedState.Length / UnsafeUtility.SizeOf<DrawingData.ProcessedBuilderData.CapturedState>();
				for (int i = 0; i < list.Count; i++)
				{
					Color color;
					Matrix4x4 matrix4x;
					int num3;
					if ((list[i].type & DrawingData.MeshType.Custom) != (DrawingData.MeshType)0)
					{
						DrawingData.ProcessedBuilderData.CapturedState capturedState2 = *(DrawingData.ProcessedBuilderData.CapturedState*)(capturedState.Ptr + (IntPtr)num * (IntPtr)sizeof(DrawingData.ProcessedBuilderData.CapturedState));
						color = capturedState2.color;
						matrix4x = capturedState2.matrix;
						num++;
						num3 = this.meta.drawOrderIndex + 1;
					}
					else
					{
						color = Color.white;
						matrix4x = Matrix4x4.identity;
						num3 = this.meta.drawOrderIndex;
					}
					meshes.Add(new DrawingData.RenderedMeshWithType
					{
						mesh = list[i].mesh,
						type = list[i].type,
						drawingOrderIndex = num3,
						color = color,
						matrix = matrix4x
					});
				}
			}

			// Token: 0x060001F7 RID: 503 RVA: 0x00009750 File Offset: 0x00007950
			private void PoolMeshes(DrawingData gizmos, bool includeCustom)
			{
				if (!this.isValid)
				{
					throw new InvalidOperationException();
				}
				int num = 0;
				for (int i = 0; i < this.meshes.Count; i++)
				{
					if ((this.meshes[i].type & DrawingData.MeshType.Custom) == (DrawingData.MeshType)0 || (includeCustom && (this.meshes[i].type & DrawingData.MeshType.Pool) != (DrawingData.MeshType)0))
					{
						gizmos.PoolMesh(this.meshes[i].mesh);
					}
					else
					{
						this.meshes[num] = this.meshes[i];
						num++;
					}
				}
				this.meshes.RemoveRange(num, this.meshes.Count - num);
			}

			// Token: 0x060001F8 RID: 504 RVA: 0x000097FF File Offset: 0x000079FF
			public void PoolDynamicMeshes(DrawingData gizmos)
			{
				if (this.type == DrawingData.ProcessedBuilderData.Type.Static && this.submitted)
				{
					return;
				}
				this.PoolMeshes(gizmos, false);
			}

			// Token: 0x060001F9 RID: 505 RVA: 0x0000981C File Offset: 0x00007A1C
			public void Release(DrawingData gizmos)
			{
				if (!this.isValid)
				{
					throw new InvalidOperationException();
				}
				this.PoolMeshes(gizmos, true);
				this.meshes.Clear();
				this.type = DrawingData.ProcessedBuilderData.Type.Invalid;
				this.splitterJob.Complete();
				this.buildJob.Complete();
				DrawingData.ProcessedBuilderData.MeshBuffers meshBuffers = this.temporaryMeshBuffers[0];
				meshBuffers.DisposeIfLarge();
				this.temporaryMeshBuffers[0] = meshBuffers;
			}

			// Token: 0x060001FA RID: 506 RVA: 0x00009888 File Offset: 0x00007A88
			public void Dispose()
			{
				if (this.isValid)
				{
					throw new InvalidOperationException();
				}
				this.splitterJob.Complete();
				this.buildJob.Complete();
				if (this.temporaryMeshBuffers.IsCreated)
				{
					this.temporaryMeshBuffers[0].Dispose();
					this.temporaryMeshBuffers.Dispose();
				}
			}

			// Token: 0x040000A9 RID: 169
			public DrawingData.ProcessedBuilderData.Type type;

			// Token: 0x040000AA RID: 170
			public DrawingData.BuilderData.Meta meta;

			// Token: 0x040000AB RID: 171
			private bool submitted;

			// Token: 0x040000AC RID: 172
			public NativeArray<DrawingData.ProcessedBuilderData.MeshBuffers> temporaryMeshBuffers;

			// Token: 0x040000AD RID: 173
			private JobHandle buildJob;

			// Token: 0x040000AE RID: 174
			private JobHandle splitterJob;

			// Token: 0x040000AF RID: 175
			public List<DrawingData.MeshWithType> meshes;

			// Token: 0x040000B0 RID: 176
			private static int SubmittedJobs;

			// Token: 0x0200002F RID: 47
			public enum Type
			{
				// Token: 0x040000B2 RID: 178
				Invalid,
				// Token: 0x040000B3 RID: 179
				Static,
				// Token: 0x040000B4 RID: 180
				Dynamic,
				// Token: 0x040000B5 RID: 181
				Persistent
			}

			// Token: 0x02000030 RID: 48
			public struct CapturedState
			{
				// Token: 0x040000B6 RID: 182
				public Matrix4x4 matrix;

				// Token: 0x040000B7 RID: 183
				public Color color;
			}

			// Token: 0x02000031 RID: 49
			public struct MeshBuffers
			{
				// Token: 0x060001FB RID: 507 RVA: 0x000098E8 File Offset: 0x00007AE8
				public MeshBuffers(Allocator allocator)
				{
					this.splitterOutput = new UnsafeAppendBuffer(0, 4, allocator);
					this.vertices = new UnsafeAppendBuffer(0, 4, allocator);
					this.triangles = new UnsafeAppendBuffer(0, 4, allocator);
					this.solidVertices = new UnsafeAppendBuffer(0, 4, allocator);
					this.solidTriangles = new UnsafeAppendBuffer(0, 4, allocator);
					this.textVertices = new UnsafeAppendBuffer(0, 4, allocator);
					this.textTriangles = new UnsafeAppendBuffer(0, 4, allocator);
					this.capturedState = new UnsafeAppendBuffer(0, 4, allocator);
					this.bounds = default(Bounds);
				}

				// Token: 0x060001FC RID: 508 RVA: 0x0000999C File Offset: 0x00007B9C
				public void Dispose()
				{
					this.splitterOutput.Dispose();
					this.vertices.Dispose();
					this.triangles.Dispose();
					this.solidVertices.Dispose();
					this.solidTriangles.Dispose();
					this.textVertices.Dispose();
					this.textTriangles.Dispose();
					this.capturedState.Dispose();
				}

				// Token: 0x060001FD RID: 509 RVA: 0x00009A04 File Offset: 0x00007C04
				private static void DisposeIfLarge(ref UnsafeAppendBuffer ls)
				{
					if (ls.Length * 3 < ls.Capacity && ls.Capacity > 1024)
					{
						AllocatorManager.AllocatorHandle allocator = ls.Allocator;
						ls.Dispose();
						ls = new UnsafeAppendBuffer(0, 4, allocator);
					}
				}

				// Token: 0x060001FE RID: 510 RVA: 0x00009A4C File Offset: 0x00007C4C
				public void DisposeIfLarge()
				{
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.splitterOutput);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.vertices);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.triangles);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.solidVertices);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.solidTriangles);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.textVertices);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.textTriangles);
					DrawingData.ProcessedBuilderData.MeshBuffers.DisposeIfLarge(ref this.capturedState);
				}

				// Token: 0x040000B8 RID: 184
				public UnsafeAppendBuffer splitterOutput;

				// Token: 0x040000B9 RID: 185
				public UnsafeAppendBuffer vertices;

				// Token: 0x040000BA RID: 186
				public UnsafeAppendBuffer triangles;

				// Token: 0x040000BB RID: 187
				public UnsafeAppendBuffer solidVertices;

				// Token: 0x040000BC RID: 188
				public UnsafeAppendBuffer solidTriangles;

				// Token: 0x040000BD RID: 189
				public UnsafeAppendBuffer textVertices;

				// Token: 0x040000BE RID: 190
				public UnsafeAppendBuffer textTriangles;

				// Token: 0x040000BF RID: 191
				public UnsafeAppendBuffer capturedState;

				// Token: 0x040000C0 RID: 192
				public Bounds bounds;
			}
		}

		// Token: 0x02000032 RID: 50
		internal struct SubmittedMesh
		{
			// Token: 0x040000C1 RID: 193
			public Mesh mesh;

			// Token: 0x040000C2 RID: 194
			public bool temporary;
		}

		// Token: 0x02000033 RID: 51
		[BurstCompile]
		internal struct BuilderData : IDisposable
		{
			// Token: 0x17000013 RID: 19
			// (get) Token: 0x060001FF RID: 511 RVA: 0x00009AB1 File Offset: 0x00007CB1
			// (set) Token: 0x06000200 RID: 512 RVA: 0x00009AB9 File Offset: 0x00007CB9
			public DrawingData.BuilderData.State state { readonly get; private set; }

			// Token: 0x06000201 RID: 513 RVA: 0x00009AC2 File Offset: 0x00007CC2
			public void Reserve(int dataIndex, bool isBuiltInCommandBuilder)
			{
				if (this.state != DrawingData.BuilderData.State.Free)
				{
					throw new InvalidOperationException();
				}
				this.state = DrawingData.BuilderData.State.Reserved;
				this.packedMeta = new DrawingData.BuilderData.BitPackedMeta(dataIndex, DrawingData.BuilderData.UniqueIDCounter++ & 32767, isBuiltInCommandBuilder);
			}

			// Token: 0x06000202 RID: 514 RVA: 0x00009AFC File Offset: 0x00007CFC
			public void Init(DrawingData.Hasher hasher, RedrawScope frameRedrawScope, RedrawScope customRedrawScope, bool isGizmos, int drawOrderIndex, int sceneModeVersion)
			{
				if (this.state != DrawingData.BuilderData.State.Reserved)
				{
					throw new InvalidOperationException();
				}
				this.meta = new DrawingData.BuilderData.Meta
				{
					hasher = hasher,
					redrawScope1 = frameRedrawScope,
					redrawScope2 = customRedrawScope,
					isGizmos = isGizmos,
					version = 0,
					drawOrderIndex = drawOrderIndex,
					sceneModeVersion = sceneModeVersion,
					cameraTargets = null
				};
				if (this.meshes == null)
				{
					this.meshes = new List<DrawingData.SubmittedMesh>();
				}
				if (!this.commandBuffers.IsCreated)
				{
					this.commandBuffers = new NativeArray<UnsafeAppendBuffer>(JobsUtility.ThreadIndexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					for (int i = 0; i < this.commandBuffers.Length; i++)
					{
						this.commandBuffers[i] = new UnsafeAppendBuffer(0, 4, Allocator.Persistent);
					}
				}
				this.state = DrawingData.BuilderData.State.Initialized;
			}

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000203 RID: 515 RVA: 0x00009BD2 File Offset: 0x00007DD2
			public unsafe UnsafeAppendBuffer* bufferPtr
			{
				get
				{
					return (UnsafeAppendBuffer*)this.commandBuffers.GetUnsafePtr<UnsafeAppendBuffer>();
				}
			}

			// Token: 0x06000204 RID: 516 RVA: 0x00009BDF File Offset: 0x00007DDF
			[BurstCompile]
			[MonoPInvokeCallback(typeof(DrawingData.BuilderData.AnyBuffersWrittenToDelegate))]
			private unsafe static bool AnyBuffersWrittenTo(UnsafeAppendBuffer* buffers, int numBuffers)
			{
				return DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.Invoke(buffers, numBuffers);
			}

			// Token: 0x06000205 RID: 517 RVA: 0x00009BE8 File Offset: 0x00007DE8
			[BurstCompile]
			[MonoPInvokeCallback(typeof(DrawingData.BuilderData.AnyBuffersWrittenToDelegate))]
			private unsafe static void ResetAllBuffers(UnsafeAppendBuffer* buffers, int numBuffers)
			{
				DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.Invoke(buffers, numBuffers);
			}

			// Token: 0x06000206 RID: 518 RVA: 0x00009BF1 File Offset: 0x00007DF1
			public void SubmitWithDependency(GCHandle gcHandle, JobHandle dependency, AllowedDelay allowedDelay)
			{
				this.state = DrawingData.BuilderData.State.WaitingForUserDefinedJob;
				this.disposeDependency = dependency;
				this.disposeDependencyDelay = allowedDelay;
				this.disposeGCHandle = gcHandle;
			}

			// Token: 0x06000207 RID: 519 RVA: 0x00009C10 File Offset: 0x00007E10
			public unsafe void Submit(DrawingData gizmos)
			{
				if (this.state != DrawingData.BuilderData.State.Initialized)
				{
					throw new InvalidOperationException();
				}
				if (this.meta.isGizmos)
				{
					this.Release();
					return;
				}
				if (this.meshes.Count == 0 && !DrawingData.BuilderData.AnyBuffersWrittenToInvoke((UnsafeAppendBuffer*)this.commandBuffers.GetUnsafeReadOnlyPtr<UnsafeAppendBuffer>(), this.commandBuffers.Length))
				{
					this.Release();
					return;
				}
				this.meta.version = gizmos.version;
				DrawingData.BuilderData.Meta meta = this.meta;
				meta.drawOrderIndex = this.meta.drawOrderIndex * 3;
				int num = gizmos.processedData.Reserve(DrawingData.ProcessedBuilderData.Type.Static, meta);
				meta.drawOrderIndex = this.meta.drawOrderIndex * 3 + 1;
				int num2 = gizmos.processedData.Reserve(DrawingData.ProcessedBuilderData.Type.Dynamic, meta);
				meta.drawOrderIndex = this.meta.drawOrderIndex + 1000000;
				int num3 = gizmos.processedData.Reserve(DrawingData.ProcessedBuilderData.Type.Persistent, meta);
				this.splitterJob = new StreamSplitter
				{
					inputBuffers = this.commandBuffers,
					staticBuffer = gizmos.processedData.Get(num).splitterOutputPtr,
					dynamicBuffer = gizmos.processedData.Get(num2).splitterOutputPtr,
					persistentBuffer = gizmos.processedData.Get(num3).splitterOutputPtr
				}.Schedule(default(JobHandle));
				gizmos.processedData.Get(num).SetSplitterJob(gizmos, this.splitterJob);
				gizmos.processedData.Get(num2).SetSplitterJob(gizmos, this.splitterJob);
				gizmos.processedData.Get(num3).SetSplitterJob(gizmos, this.splitterJob);
				if (this.meshes.Count > 0)
				{
					List<DrawingData.MeshWithType> list = gizmos.processedData.Get(num2).meshes;
					for (int i = 0; i < this.meshes.Count; i++)
					{
						list.Add(new DrawingData.MeshWithType
						{
							mesh = this.meshes[i].mesh,
							type = (DrawingData.MeshType.Solid | DrawingData.MeshType.Custom | (this.meshes[i].temporary ? DrawingData.MeshType.Pool : ((DrawingData.MeshType)0)))
						});
					}
					this.meshes.Clear();
				}
				this.state = DrawingData.BuilderData.State.WaitingForSplitter;
			}

			// Token: 0x06000208 RID: 520 RVA: 0x00009E54 File Offset: 0x00008054
			public void CheckJobDependency(DrawingData gizmos, bool allowBlocking)
			{
				if (this.state == DrawingData.BuilderData.State.WaitingForUserDefinedJob && (this.disposeDependency.IsCompleted || (allowBlocking && this.disposeDependencyDelay == AllowedDelay.EndOfFrame)))
				{
					this.disposeDependency.Complete();
					this.disposeDependency = default(JobHandle);
					this.disposeGCHandle.Free();
					this.state = DrawingData.BuilderData.State.Initialized;
					this.Submit(gizmos);
				}
			}

			// Token: 0x06000209 RID: 521 RVA: 0x00009EB2 File Offset: 0x000080B2
			public void Release()
			{
				if (this.state == DrawingData.BuilderData.State.Free)
				{
					throw new InvalidOperationException();
				}
				this.state = DrawingData.BuilderData.State.Free;
				this.ClearData();
			}

			// Token: 0x0600020A RID: 522 RVA: 0x00009ED0 File Offset: 0x000080D0
			private unsafe void ClearData()
			{
				this.disposeDependency.Complete();
				this.splitterJob.Complete();
				this.meta = default(DrawingData.BuilderData.Meta);
				this.disposeDependency = default(JobHandle);
				this.preventDispose = false;
				this.meshes.Clear();
				DrawingData.BuilderData.ResetAllBuffers((UnsafeAppendBuffer*)this.commandBuffers.GetUnsafePtr<UnsafeAppendBuffer>(), this.commandBuffers.Length);
			}

			// Token: 0x0600020B RID: 523 RVA: 0x00009F38 File Offset: 0x00008138
			public void Dispose()
			{
				if (this.state == DrawingData.BuilderData.State.WaitingForUserDefinedJob)
				{
					this.disposeDependency.Complete();
					this.disposeGCHandle.Free();
					this.state = DrawingData.BuilderData.State.WaitingForSplitter;
				}
				if (this.state == DrawingData.BuilderData.State.Reserved || this.state == DrawingData.BuilderData.State.Initialized || this.state == DrawingData.BuilderData.State.WaitingForUserDefinedJob)
				{
					Debug.LogError("Drawing data is being destroyed, but a drawing instance is still active. Are you sure you have called Dispose on all drawing instances? This will cause a memory leak!");
					return;
				}
				this.splitterJob.Complete();
				if (this.commandBuffers.IsCreated)
				{
					for (int i = 0; i < this.commandBuffers.Length; i++)
					{
						this.commandBuffers[i].Dispose();
					}
					this.commandBuffers.Dispose();
				}
			}

			// Token: 0x0600020D RID: 525 RVA: 0x0000A030 File Offset: 0x00008230
			[BurstCompile]
			[MonoPInvokeCallback(typeof(DrawingData.BuilderData.AnyBuffersWrittenToDelegate))]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static bool AnyBuffersWrittenTo$BurstManaged(UnsafeAppendBuffer* buffers, int numBuffers)
			{
				bool flag = false;
				for (int i = 0; i < numBuffers; i++)
				{
					flag |= buffers[i].Length > 0;
				}
				return flag;
			}

			// Token: 0x0600020E RID: 526 RVA: 0x0000A064 File Offset: 0x00008264
			[BurstCompile]
			[MonoPInvokeCallback(typeof(DrawingData.BuilderData.AnyBuffersWrittenToDelegate))]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void ResetAllBuffers$BurstManaged(UnsafeAppendBuffer* buffers, int numBuffers)
			{
				for (int i = 0; i < numBuffers; i++)
				{
					buffers[i].Reset();
				}
			}

			// Token: 0x040000C3 RID: 195
			public DrawingData.BuilderData.BitPackedMeta packedMeta;

			// Token: 0x040000C4 RID: 196
			public List<DrawingData.SubmittedMesh> meshes;

			// Token: 0x040000C5 RID: 197
			public NativeArray<UnsafeAppendBuffer> commandBuffers;

			// Token: 0x040000C7 RID: 199
			public bool preventDispose;

			// Token: 0x040000C8 RID: 200
			private JobHandle splitterJob;

			// Token: 0x040000C9 RID: 201
			private JobHandle disposeDependency;

			// Token: 0x040000CA RID: 202
			private AllowedDelay disposeDependencyDelay;

			// Token: 0x040000CB RID: 203
			private GCHandle disposeGCHandle;

			// Token: 0x040000CC RID: 204
			public DrawingData.BuilderData.Meta meta;

			// Token: 0x040000CD RID: 205
			private static int UniqueIDCounter = 0;

			// Token: 0x040000CE RID: 206
			private static readonly DrawingData.BuilderData.AnyBuffersWrittenToDelegate AnyBuffersWrittenToInvoke = BurstCompiler.CompileFunctionPointer<DrawingData.BuilderData.AnyBuffersWrittenToDelegate>(new DrawingData.BuilderData.AnyBuffersWrittenToDelegate(DrawingData.BuilderData.AnyBuffersWrittenTo)).Invoke;

			// Token: 0x040000CF RID: 207
			private static readonly DrawingData.BuilderData.ResetAllBuffersToDelegate ResetAllBuffersToInvoke = BurstCompiler.CompileFunctionPointer<DrawingData.BuilderData.ResetAllBuffersToDelegate>(new DrawingData.BuilderData.ResetAllBuffersToDelegate(DrawingData.BuilderData.ResetAllBuffers)).Invoke;

			// Token: 0x02000034 RID: 52
			public enum State
			{
				// Token: 0x040000D1 RID: 209
				Free,
				// Token: 0x040000D2 RID: 210
				Reserved,
				// Token: 0x040000D3 RID: 211
				Initialized,
				// Token: 0x040000D4 RID: 212
				WaitingForSplitter,
				// Token: 0x040000D5 RID: 213
				WaitingForUserDefinedJob
			}

			// Token: 0x02000035 RID: 53
			public struct Meta
			{
				// Token: 0x040000D6 RID: 214
				public DrawingData.Hasher hasher;

				// Token: 0x040000D7 RID: 215
				public RedrawScope redrawScope1;

				// Token: 0x040000D8 RID: 216
				public RedrawScope redrawScope2;

				// Token: 0x040000D9 RID: 217
				public int version;

				// Token: 0x040000DA RID: 218
				public bool isGizmos;

				// Token: 0x040000DB RID: 219
				public int sceneModeVersion;

				// Token: 0x040000DC RID: 220
				public int drawOrderIndex;

				// Token: 0x040000DD RID: 221
				public Camera[] cameraTargets;
			}

			// Token: 0x02000036 RID: 54
			public struct BitPackedMeta
			{
				// Token: 0x0600020F RID: 527 RVA: 0x0000A08D File Offset: 0x0000828D
				public BitPackedMeta(int dataIndex, int uniqueID, bool isBuiltInCommandBuilder)
				{
					if (dataIndex > 65535)
					{
						throw new Exception("Too many command builders active. Are some command builders not being disposed?");
					}
					this.flags = (uint)(dataIndex | (uniqueID << 17) | (isBuiltInCommandBuilder ? 65536 : 0));
				}

				// Token: 0x17000015 RID: 21
				// (get) Token: 0x06000210 RID: 528 RVA: 0x0000A0BA File Offset: 0x000082BA
				public int dataIndex
				{
					get
					{
						return (int)(this.flags & 65535U);
					}
				}

				// Token: 0x17000016 RID: 22
				// (get) Token: 0x06000211 RID: 529 RVA: 0x0000A0C8 File Offset: 0x000082C8
				public int uniqueID
				{
					get
					{
						return (int)(this.flags >> 17);
					}
				}

				// Token: 0x17000017 RID: 23
				// (get) Token: 0x06000212 RID: 530 RVA: 0x0000A0D3 File Offset: 0x000082D3
				public bool isBuiltInCommandBuilder
				{
					get
					{
						return (this.flags & 65536U) > 0U;
					}
				}

				// Token: 0x06000213 RID: 531 RVA: 0x0000A0E4 File Offset: 0x000082E4
				public static bool operator ==(DrawingData.BuilderData.BitPackedMeta lhs, DrawingData.BuilderData.BitPackedMeta rhs)
				{
					return lhs.flags == rhs.flags;
				}

				// Token: 0x06000214 RID: 532 RVA: 0x0000A0F4 File Offset: 0x000082F4
				public static bool operator !=(DrawingData.BuilderData.BitPackedMeta lhs, DrawingData.BuilderData.BitPackedMeta rhs)
				{
					return lhs.flags != rhs.flags;
				}

				// Token: 0x06000215 RID: 533 RVA: 0x0000A108 File Offset: 0x00008308
				public override bool Equals(object obj)
				{
					if (obj is DrawingData.BuilderData.BitPackedMeta)
					{
						DrawingData.BuilderData.BitPackedMeta bitPackedMeta = (DrawingData.BuilderData.BitPackedMeta)obj;
						return this.flags == bitPackedMeta.flags;
					}
					return false;
				}

				// Token: 0x06000216 RID: 534 RVA: 0x0000A134 File Offset: 0x00008334
				public override int GetHashCode()
				{
					return (int)this.flags;
				}

				// Token: 0x040000DE RID: 222
				private uint flags;

				// Token: 0x040000DF RID: 223
				private const int UniqueIDBitshift = 17;

				// Token: 0x040000E0 RID: 224
				private const int IsBuiltInFlagIndex = 16;

				// Token: 0x040000E1 RID: 225
				private const int IndexMask = 65535;

				// Token: 0x040000E2 RID: 226
				private const int MaxDataIndex = 65535;

				// Token: 0x040000E3 RID: 227
				public const int UniqueIdMask = 32767;
			}

			// Token: 0x02000037 RID: 55
			// (Invoke) Token: 0x06000218 RID: 536
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private unsafe delegate bool AnyBuffersWrittenToDelegate(UnsafeAppendBuffer* buffers, int numBuffers);

			// Token: 0x02000038 RID: 56
			// (Invoke) Token: 0x0600021C RID: 540
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private unsafe delegate void ResetAllBuffersToDelegate(UnsafeAppendBuffer* buffers, int numBuffers);

			// Token: 0x02000039 RID: 57
			// (Invoke) Token: 0x06000220 RID: 544
			internal unsafe delegate bool AnyBuffersWrittenTo_000001EA$PostfixBurstDelegate(UnsafeAppendBuffer* buffers, int numBuffers);

			// Token: 0x0200003A RID: 58
			internal static class AnyBuffersWrittenTo_000001EA$BurstDirectCall
			{
				// Token: 0x06000223 RID: 547 RVA: 0x0000A13C File Offset: 0x0000833C
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.Pointer == 0)
					{
						DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.DeferredCompilation, methodof(DrawingData.BuilderData.AnyBuffersWrittenTo$BurstManaged(UnsafeAppendBuffer*, int)).MethodHandle, typeof(DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.Pointer;
				}

				// Token: 0x06000224 RID: 548 RVA: 0x0000A168 File Offset: 0x00008368
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x06000225 RID: 549 RVA: 0x0000A180 File Offset: 0x00008380
				public unsafe static void Constructor()
				{
					DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(DrawingData.BuilderData.AnyBuffersWrittenTo(UnsafeAppendBuffer*, int)).MethodHandle);
				}

				// Token: 0x06000226 RID: 550 RVA: 0x00002104 File Offset: 0x00000304
				public static void Initialize()
				{
				}

				// Token: 0x06000227 RID: 551 RVA: 0x0000A191 File Offset: 0x00008391
				// Note: this type is marked as 'beforefieldinit'.
				static AnyBuffersWrittenTo_000001EA$BurstDirectCall()
				{
					DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.Constructor();
				}

				// Token: 0x06000228 RID: 552 RVA: 0x0000A198 File Offset: 0x00008398
				public unsafe static bool Invoke(UnsafeAppendBuffer* buffers, int numBuffers)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = DrawingData.BuilderData.AnyBuffersWrittenTo_000001EA$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							return calli(System.Boolean(Unity.Collections.LowLevel.Unsafe.UnsafeAppendBuffer*,System.Int32), buffers, numBuffers, functionPointer);
						}
					}
					return DrawingData.BuilderData.AnyBuffersWrittenTo$BurstManaged(buffers, numBuffers);
				}

				// Token: 0x040000E4 RID: 228
				private static IntPtr Pointer;

				// Token: 0x040000E5 RID: 229
				private static IntPtr DeferredCompilation;
			}

			// Token: 0x0200003B RID: 59
			// (Invoke) Token: 0x0600022A RID: 554
			internal unsafe delegate void ResetAllBuffers_000001EB$PostfixBurstDelegate(UnsafeAppendBuffer* buffers, int numBuffers);

			// Token: 0x0200003C RID: 60
			internal static class ResetAllBuffers_000001EB$BurstDirectCall
			{
				// Token: 0x0600022D RID: 557 RVA: 0x0000A1CB File Offset: 0x000083CB
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.Pointer == 0)
					{
						DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.DeferredCompilation, methodof(DrawingData.BuilderData.ResetAllBuffers$BurstManaged(UnsafeAppendBuffer*, int)).MethodHandle, typeof(DrawingData.BuilderData.ResetAllBuffers_000001EB$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.Pointer;
				}

				// Token: 0x0600022E RID: 558 RVA: 0x0000A1F8 File Offset: 0x000083F8
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x0600022F RID: 559 RVA: 0x0000A210 File Offset: 0x00008410
				public unsafe static void Constructor()
				{
					DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(DrawingData.BuilderData.ResetAllBuffers(UnsafeAppendBuffer*, int)).MethodHandle);
				}

				// Token: 0x06000230 RID: 560 RVA: 0x00002104 File Offset: 0x00000304
				public static void Initialize()
				{
				}

				// Token: 0x06000231 RID: 561 RVA: 0x0000A221 File Offset: 0x00008421
				// Note: this type is marked as 'beforefieldinit'.
				static ResetAllBuffers_000001EB$BurstDirectCall()
				{
					DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.Constructor();
				}

				// Token: 0x06000232 RID: 562 RVA: 0x0000A228 File Offset: 0x00008428
				public unsafe static void Invoke(UnsafeAppendBuffer* buffers, int numBuffers)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = DrawingData.BuilderData.ResetAllBuffers_000001EB$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Unity.Collections.LowLevel.Unsafe.UnsafeAppendBuffer*,System.Int32), buffers, numBuffers, functionPointer);
							return;
						}
					}
					DrawingData.BuilderData.ResetAllBuffers$BurstManaged(buffers, numBuffers);
				}

				// Token: 0x040000E6 RID: 230
				private static IntPtr Pointer;

				// Token: 0x040000E7 RID: 231
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x0200003D RID: 61
		internal struct BuilderDataContainer : IDisposable
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x06000233 RID: 563 RVA: 0x0000A25C File Offset: 0x0000845C
			public unsafe int memoryUsage
			{
				get
				{
					int num = 0;
					if (this.data != null)
					{
						for (int i = 0; i < this.data.Length; i++)
						{
							NativeArray<UnsafeAppendBuffer> commandBuffers = this.data[i].commandBuffers;
							for (int j = 0; j < commandBuffers.Length; j++)
							{
								num += commandBuffers[j].Capacity;
							}
							num += this.data[i].commandBuffers.Length * sizeof(UnsafeAppendBuffer);
						}
					}
					return num;
				}
			}

			// Token: 0x06000234 RID: 564 RVA: 0x0000A2DC File Offset: 0x000084DC
			public DrawingData.BuilderData.BitPackedMeta Reserve(bool isBuiltInCommandBuilder)
			{
				if (this.data == null)
				{
					this.data = new DrawingData.BuilderData[1];
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].state == DrawingData.BuilderData.State.Free)
					{
						this.data[i].Reserve(i, isBuiltInCommandBuilder);
						return this.data[i].packedMeta;
					}
				}
				DrawingData.BuilderData[] array = new DrawingData.BuilderData[this.data.Length * 2];
				this.data.CopyTo(array, 0);
				this.data = array;
				return this.Reserve(isBuiltInCommandBuilder);
			}

			// Token: 0x06000235 RID: 565 RVA: 0x0000A373 File Offset: 0x00008573
			public void Release(DrawingData.BuilderData.BitPackedMeta meta)
			{
				this.data[meta.dataIndex].Release();
			}

			// Token: 0x06000236 RID: 566 RVA: 0x0000A38C File Offset: 0x0000858C
			public bool StillExists(DrawingData.BuilderData.BitPackedMeta meta)
			{
				int dataIndex = meta.dataIndex;
				return this.data != null && dataIndex < this.data.Length && this.data[dataIndex].packedMeta == meta;
			}

			// Token: 0x06000237 RID: 567 RVA: 0x0000A3D0 File Offset: 0x000085D0
			public ref DrawingData.BuilderData Get(DrawingData.BuilderData.BitPackedMeta meta)
			{
				int dataIndex = meta.dataIndex;
				if (this.data[dataIndex].state == DrawingData.BuilderData.State.Free)
				{
					throw new ArgumentException("Data is not reserved");
				}
				if (this.data[dataIndex].packedMeta != meta)
				{
					throw new ArgumentException("This command builder has already been disposed");
				}
				return ref this.data[dataIndex];
			}

			// Token: 0x06000238 RID: 568 RVA: 0x0000A434 File Offset: 0x00008634
			public void DisposeCommandBuildersWithJobDependencies(DrawingData gizmos)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					this.data[i].CheckJobDependency(gizmos, false);
				}
				for (int j = 0; j < this.data.Length; j++)
				{
					this.data[j].CheckJobDependency(gizmos, true);
				}
			}

			// Token: 0x06000239 RID: 569 RVA: 0x0000A498 File Offset: 0x00008698
			public void ReleaseAllUnused()
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].state == DrawingData.BuilderData.State.WaitingForSplitter)
					{
						this.data[i].Release();
					}
				}
			}

			// Token: 0x0600023A RID: 570 RVA: 0x0000A4E8 File Offset: 0x000086E8
			public void Dispose()
			{
				if (this.data != null)
				{
					for (int i = 0; i < this.data.Length; i++)
					{
						this.data[i].Dispose();
					}
				}
				this.data = null;
			}

			// Token: 0x040000E8 RID: 232
			private DrawingData.BuilderData[] data;
		}

		// Token: 0x0200003E RID: 62
		internal struct ProcessedBuilderDataContainer
		{
			// Token: 0x17000019 RID: 25
			// (get) Token: 0x0600023B RID: 571 RVA: 0x0000A528 File Offset: 0x00008728
			public bool isEmpty
			{
				get
				{
					return this.data == null || this.freeSlots.Count == this.data.Length;
				}
			}

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x0600023C RID: 572 RVA: 0x0000A54C File Offset: 0x0000874C
			public int memoryUsage
			{
				get
				{
					int num = 0;
					if (this.data != null)
					{
						for (int i = 0; i < this.data.Length; i++)
						{
							NativeArray<DrawingData.ProcessedBuilderData.MeshBuffers> temporaryMeshBuffers = this.data[i].temporaryMeshBuffers;
							for (int j = 0; j < temporaryMeshBuffers.Length; j++)
							{
								int num2 = 0;
								num2 += temporaryMeshBuffers[j].textVertices.Capacity;
								num2 += temporaryMeshBuffers[j].textTriangles.Capacity;
								num2 += temporaryMeshBuffers[j].solidVertices.Capacity;
								num2 += temporaryMeshBuffers[j].solidTriangles.Capacity;
								num2 += temporaryMeshBuffers[j].vertices.Capacity;
								num2 += temporaryMeshBuffers[j].triangles.Capacity;
								num2 += temporaryMeshBuffers[j].capturedState.Capacity;
								num2 += temporaryMeshBuffers[j].splitterOutput.Capacity;
								num += num2;
								Debug.Log(string.Concat(new string[]
								{
									i.ToString(),
									":",
									j.ToString(),
									" ",
									num2.ToString()
								}));
							}
						}
					}
					return num;
				}
			}

			// Token: 0x0600023D RID: 573 RVA: 0x0000A6A8 File Offset: 0x000088A8
			public int Reserve(DrawingData.ProcessedBuilderData.Type type, DrawingData.BuilderData.Meta meta)
			{
				if (this.data == null)
				{
					this.data = new DrawingData.ProcessedBuilderData[0];
					this.freeSlots = new Stack<int>();
					this.freeLists = new Stack<List<int>>();
					this.hash2index = new Dictionary<ulong, List<int>>();
				}
				if (this.freeSlots.Count == 0)
				{
					DrawingData.ProcessedBuilderData[] array = new DrawingData.ProcessedBuilderData[math.max(4, this.data.Length * 2)];
					this.data.CopyTo(array, 0);
					for (int i = this.data.Length; i < array.Length; i++)
					{
						this.freeSlots.Push(i);
					}
					this.data = array;
				}
				int num = this.freeSlots.Pop();
				this.data[num].Init(type, meta);
				if (!meta.hasher.Equals(DrawingData.Hasher.NotSupplied))
				{
					List<int> list;
					if (!this.hash2index.TryGetValue(meta.hasher.Hash, out list))
					{
						if (this.freeLists.Count == 0)
						{
							this.freeLists.Push(new List<int>());
						}
						list = (this.hash2index[meta.hasher.Hash] = this.freeLists.Pop());
					}
					list.Add(num);
				}
				return num;
			}

			// Token: 0x0600023E RID: 574 RVA: 0x0000A7DC File Offset: 0x000089DC
			public ref DrawingData.ProcessedBuilderData Get(int index)
			{
				if (!this.data[index].isValid)
				{
					throw new ArgumentException();
				}
				return ref this.data[index];
			}

			// Token: 0x0600023F RID: 575 RVA: 0x0000A804 File Offset: 0x00008A04
			private void Release(DrawingData gizmos, int i)
			{
				ulong hash = this.data[i].meta.hasher.Hash;
				List<int> list;
				if (!this.data[i].meta.hasher.Equals(DrawingData.Hasher.NotSupplied) && this.hash2index.TryGetValue(hash, out list))
				{
					list.Remove(i);
					if (list.Count == 0)
					{
						this.freeLists.Push(list);
						this.hash2index.Remove(hash);
					}
				}
				this.data[i].Release(gizmos);
				this.freeSlots.Push(i);
			}

			// Token: 0x06000240 RID: 576 RVA: 0x0000A8A8 File Offset: 0x00008AA8
			public void SubmitMeshes(DrawingData gizmos, Camera camera, int versionThreshold, bool allowGizmos, bool allowCameraDefault)
			{
				if (this.data == null)
				{
					return;
				}
				GeometryBuilder.CameraInfo cameraInfo = new GeometryBuilder.CameraInfo(camera);
				int num = 0;
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].meta.version >= versionThreshold && this.data[i].IsValidForCamera(camera, allowGizmos, allowCameraDefault))
					{
						num++;
						this.data[i].Schedule(gizmos, ref cameraInfo);
					}
				}
				JobHandle.ScheduleBatchedJobs();
				for (int j = 0; j < this.data.Length; j++)
				{
					if (this.data[j].isValid && this.data[j].meta.version >= versionThreshold && this.data[j].IsValidForCamera(camera, allowGizmos, allowCameraDefault))
					{
						this.data[j].BuildMeshes(gizmos);
					}
				}
			}

			// Token: 0x06000241 RID: 577 RVA: 0x0000A9A8 File Offset: 0x00008BA8
			public void PoolDynamicMeshes(DrawingData gizmos)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid)
					{
						this.data[i].PoolDynamicMeshes(gizmos);
					}
				}
			}

			// Token: 0x06000242 RID: 578 RVA: 0x0000A9F8 File Offset: 0x00008BF8
			public void CollectMeshes(int versionThreshold, List<DrawingData.RenderedMeshWithType> meshes, Camera camera, bool allowGizmos, bool allowCameraDefault)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].meta.version >= versionThreshold && this.data[i].IsValidForCamera(camera, allowGizmos, allowCameraDefault))
					{
						this.data[i].CollectMeshes(meshes);
					}
				}
			}

			// Token: 0x06000243 RID: 579 RVA: 0x0000AA78 File Offset: 0x00008C78
			public void FilterOldPersistentCommands(int version, int lastTickVersion, float time, int sceneModeVersion)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].type == DrawingData.ProcessedBuilderData.Type.Persistent)
					{
						this.data[i].SchedulePersistFilter(version, lastTickVersion, time, sceneModeVersion);
					}
				}
			}

			// Token: 0x06000244 RID: 580 RVA: 0x0000AAE0 File Offset: 0x00008CE0
			public bool SetVersion(DrawingData.Hasher hasher, int version)
			{
				if (this.data == null)
				{
					return false;
				}
				List<int> list;
				if (this.hash2index.TryGetValue(hasher.Hash, out list))
				{
					for (int i = 0; i < list.Count; i++)
					{
						int num = list[i];
						this.data[num].meta.version = version;
					}
					return true;
				}
				return false;
			}

			// Token: 0x06000245 RID: 581 RVA: 0x0000AB40 File Offset: 0x00008D40
			public bool SetVersion(RedrawScope scope, int version)
			{
				if (this.data == null)
				{
					return false;
				}
				bool flag = false;
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && (this.data[i].meta.redrawScope1.id == scope.id || this.data[i].meta.redrawScope2.id == scope.id))
					{
						this.data[i].meta.version = version;
						flag = true;
					}
				}
				return flag;
			}

			// Token: 0x06000246 RID: 582 RVA: 0x0000ABE4 File Offset: 0x00008DE4
			public bool SetCustomScope(DrawingData.Hasher hasher, RedrawScope scope)
			{
				if (this.data == null)
				{
					return false;
				}
				List<int> list;
				if (this.hash2index.TryGetValue(hasher.Hash, out list))
				{
					for (int i = 0; i < list.Count; i++)
					{
						int num = list[i];
						this.data[num].meta.redrawScope2 = scope;
					}
					return true;
				}
				return false;
			}

			// Token: 0x06000247 RID: 583 RVA: 0x0000AC44 File Offset: 0x00008E44
			public void ReleaseDataOlderThan(DrawingData gizmos, int version)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].meta.version < version)
					{
						this.Release(gizmos, i);
					}
				}
			}

			// Token: 0x06000248 RID: 584 RVA: 0x0000ACA4 File Offset: 0x00008EA4
			public void ReleaseAllWithHash(DrawingData gizmos, DrawingData.Hasher hasher)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid && this.data[i].meta.hasher.Hash == hasher.Hash)
					{
						this.Release(gizmos, i);
					}
				}
			}

			// Token: 0x06000249 RID: 585 RVA: 0x0000AD0C File Offset: 0x00008F0C
			public void Dispose(DrawingData gizmos)
			{
				if (this.data == null)
				{
					return;
				}
				for (int i = 0; i < this.data.Length; i++)
				{
					if (this.data[i].isValid)
					{
						this.Release(gizmos, i);
					}
					this.data[i].Dispose();
				}
				this.data = null;
			}

			// Token: 0x040000E9 RID: 233
			private DrawingData.ProcessedBuilderData[] data;

			// Token: 0x040000EA RID: 234
			private Dictionary<ulong, List<int>> hash2index;

			// Token: 0x040000EB RID: 235
			private Stack<int> freeSlots;

			// Token: 0x040000EC RID: 236
			private Stack<List<int>> freeLists;
		}

		// Token: 0x0200003F RID: 63
		[Flags]
		internal enum MeshType
		{
			// Token: 0x040000EE RID: 238
			Solid = 1,
			// Token: 0x040000EF RID: 239
			Lines = 2,
			// Token: 0x040000F0 RID: 240
			Text = 4,
			// Token: 0x040000F1 RID: 241
			Custom = 8,
			// Token: 0x040000F2 RID: 242
			Pool = 16,
			// Token: 0x040000F3 RID: 243
			BaseType = 7
		}

		// Token: 0x02000040 RID: 64
		internal struct MeshWithType
		{
			// Token: 0x040000F4 RID: 244
			public Mesh mesh;

			// Token: 0x040000F5 RID: 245
			public DrawingData.MeshType type;
		}

		// Token: 0x02000041 RID: 65
		internal struct RenderedMeshWithType
		{
			// Token: 0x040000F6 RID: 246
			public Mesh mesh;

			// Token: 0x040000F7 RID: 247
			public DrawingData.MeshType type;

			// Token: 0x040000F8 RID: 248
			public int drawingOrderIndex;

			// Token: 0x040000F9 RID: 249
			public Color color;

			// Token: 0x040000FA RID: 250
			public Matrix4x4 matrix;
		}

		// Token: 0x02000042 RID: 66
		private struct Range
		{
			// Token: 0x040000FB RID: 251
			public int start;

			// Token: 0x040000FC RID: 252
			public int end;
		}

		// Token: 0x02000043 RID: 67
		public struct CommandBufferWrapper
		{
			// Token: 0x0600024A RID: 586 RVA: 0x0000AD68 File Offset: 0x00008F68
			public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties)
			{
				if (this.cmd != null)
				{
					this.cmd.DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, properties);
				}
			}

			// Token: 0x040000FD RID: 253
			public CommandBuffer cmd;
		}
	}
}
