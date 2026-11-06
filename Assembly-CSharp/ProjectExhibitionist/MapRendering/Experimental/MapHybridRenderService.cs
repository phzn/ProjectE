using System;
using System.Collections.Generic;
using Features.RenderingPrecision;
using GPUInstancerPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectExhibitionist.MapRendering.Experimental
{
	// Token: 0x020009BD RID: 2493
	[DefaultExecutionOrder(32000)]
	internal sealed class MapHybridRenderService : MonoBehaviour, IGPUILightProbeDataProvider
	{
		// Token: 0x0600462A RID: 17962 RVA: 0x001A7EFA File Offset: 0x001A60FA
		internal static void ResetStatic()
		{
			MapHybridRenderService.instance = null;
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x001A7F02 File Offset: 0x001A6102
		internal static MapHybridRenderService Get()
		{
			if (MapHybridRenderService.instance != null)
			{
				return MapHybridRenderService.instance;
			}
			GameObject gameObject = new GameObject("Map Hybrid Render Pilot (runtime)");
			gameObject.hideFlags = HideFlags.DontSave;
			Object.DontDestroyOnLoad(gameObject);
			MapHybridRenderService.instance = gameObject.AddComponent<MapHybridRenderService>();
			return MapHybridRenderService.instance;
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x001A7F40 File Offset: 0x001A6140
		private void Awake()
		{
			MapHybridRenderService.instance = this;
			WorldOrigin.Shifted += this.OnOriginShifted;
			this.profile = ScriptableObject.CreateInstance<GPUIProfile>();
			this.profile.name = "Map hybrid pilot runtime profile";
			this.profile.hideFlags = HideFlags.DontSave;
			this.profile.isDistanceCulling = (this.profile.isOcclusionCulling = false);
			this.profile.isShadowDistanceCulling = (this.profile.isShadowOcclusionCulling = (this.profile.isShadowFrustumCulling = false));
			this.profile.isFrustumCulling = true;
			this.profile.isLODCrossFade = false;
			this.profile.lightProbeSetting = GPUILightProbeSetting.Single;
			this.BindManager();
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x001A7FF8 File Offset: 0x001A61F8
		internal void Add(MapHybridRenderPayload payload)
		{
			if (this.quitting)
			{
				return;
			}
			MapHybridRenderService.State state;
			if (!this.states.TryGetValue(payload, out state))
			{
				state = new MapHybridRenderService.State
				{
					payload = payload,
					sentinels = (payload.visibilitySentinels ?? new Renderer[0])
				};
				state.sentinelVisible = new bool[state.sentinels.Length];
				this.states.Add(payload, state);
				try
				{
					if (payload.entries == null || payload.entries.Length == 0)
					{
						throw new InvalidOperationException("Empty payload");
					}
					foreach (MapHybridRenderPayload.Entry entry in payload.entries)
					{
						MapHybridRenderService.Validate(entry);
						int num = Array.IndexOf<Renderer>(state.sentinels, entry.sentinel);
						if (num < 0)
						{
							throw new InvalidOperationException("Entry sentinel is absent from visibilitySentinels");
						}
						MapHybridRenderService.Key key = new MapHybridRenderService.Key(entry);
						MapHybridRenderService.Group group;
						if (!this.groups.TryGetValue(key, out group))
						{
							group = this.Register(entry);
							this.groups.Add(key, group);
						}
						group.slots.Add(new MapHybridRenderService.Slot
						{
							state = state,
							entry = entry,
							sentinelIndex = num
						});
						if (entry.reflectionProbeUsage != ReflectionProbeUsage.Off)
						{
							MapHybridRenderPayload.ReflectionProbeEntries++;
						}
					}
				}
				catch (Exception ex)
				{
					this.Fail(payload.name + ": " + ex.Message);
				}
			}
			state.active = true;
			state.ready = false;
			state.owner = null;
			state.matrix = payload.transform.localToWorldMatrix;
			state.staged = payload.GetComponentInParent<IncrementalPooledZoneActivation>();
			state.heavy = payload.GetComponentInParent<HeavyZoneStreamingProxy>();
			if (payload.nativeFallbackRoot != null)
			{
				payload.nativeFallbackRoot.SetActive(MapHybridRenderPayload.HasFailure);
			}
			if (payload.nativeResidualRoot != null)
			{
				payload.nativeResidualRoot.SetActive(!MapHybridRenderPayload.HasFailure);
			}
			this.dirty = (this.pollNow = true);
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x001A81F8 File Offset: 0x001A63F8
		private static void Validate(MapHybridRenderPayload.Entry entry)
		{
			if (entry == null || entry.mesh == null || entry.materials == null || entry.materials.Length == 0 || entry.sentinel == null)
			{
				throw new InvalidOperationException("Missing mesh/material/sentinel descriptor");
			}
			MeshFilter component = entry.sentinel.GetComponent<MeshFilter>();
			if (component != null && component.sharedMesh != null)
			{
				MapHybridRenderPayload.DuplicateNativeRendererCount++;
				throw new InvalidOperationException("Native sentinel still contains geometry; duplicate rendering prevented");
			}
			foreach (Material material in entry.materials)
			{
				if (material == null || material.shader == null || !material.shader.isSupported)
				{
					throw new InvalidOperationException("Missing or unsupported source material");
				}
			}
			if (entry.lightProbeUsage != LightProbeUsage.Off && entry.lightProbeUsage != LightProbeUsage.BlendProbes)
			{
				throw new InvalidOperationException("Custom/proxy light probes are outside this pilot");
			}
			if (entry.lightProbeUsage != LightProbeUsage.Off && LightmapSettings.lightProbes != null && LightmapSettings.lightProbes.count > 0)
			{
				throw new InvalidOperationException("Local light probes would change original combined-cell sampling; capture ineligible");
			}
			if (entry.reflectionProbeUsage != ReflectionProbeUsage.BlendProbes && entry.reflectionProbeUsage != ReflectionProbeUsage.Off)
			{
				throw new InvalidOperationException("Non-default reflection probe usage is outside this pilot");
			}
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x001A8328 File Offset: 0x001A6528
		private MapHybridRenderService.Group Register(MapHybridRenderPayload.Entry entry)
		{
			MapHybridRenderService.Group group;
			using (MapHybridRenderService.RegisterMarker.Auto())
			{
				GPUILODGroupData gpuilodgroupData = ScriptableObject.CreateInstance<GPUILODGroupData>();
				gpuilodgroupData.name = "Map hybrid " + entry.mesh.name;
				gpuilodgroupData.hideFlags = HideFlags.DontSave;
				gpuilodgroupData.AddLOD(-1f, 0f);
				gpuilodgroupData.AddRenderer(0, entry.mesh, (Material[])entry.materials.Clone(), Matrix4x4.identity, entry.layer, entry.shadows, entry.receiveShadows, MotionVectorGenerationMode.Camera, false, false, entry.renderingLayerMask, entry.lightProbeUsage, -1, Matrix4x4.identity);
				GPUIPrototype gpuiprototype = new GPUIPrototype(gpuilodgroupData, this.profile);
				int num;
				if (!GPUICoreAPI.RegisterRenderer(this, gpuiprototype, out num))
				{
					Object.Destroy(gpuilodgroupData);
					throw new InvalidOperationException("GPUI registration rejected " + entry.mesh.name);
				}
				GPUICoreAPI.SetInstanceCount(num, 0);
				foreach (Material material in entry.materials)
				{
					Material material2;
					if (!GPUIRenderingSystem.Instance.MaterialProvider.TryGetReplacementMaterial(material, null, null, out material2) || material2 == null || material2.shader == null || !material2.shader.isSupported)
					{
						GPUICoreAPI.DisposeRenderer(num);
						Object.Destroy(gpuilodgroupData);
						throw new InvalidOperationException("GPUI shader binding unavailable for " + material.name + "; prepare bindings before capture");
					}
				}
				MapHybridRenderPayload.RegisteredGroups++;
				group = new MapHybridRenderService.Group
				{
					data = gpuilodgroupData,
					rendererKey = num
				};
			}
			return group;
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x001A84E4 File Offset: 0x001A66E4
		internal static void SetInactive(MapHybridRenderPayload payload)
		{
			MapHybridRenderService.State state;
			if (MapHybridRenderService.instance == null || !MapHybridRenderService.instance.states.TryGetValue(payload, out state))
			{
				return;
			}
			state.active = (state.visible = (state.ready = false));
			MapHybridRenderService.instance.dirty = true;
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x001A8538 File Offset: 0x001A6738
		internal static void Remove(MapHybridRenderPayload payload)
		{
			MapHybridRenderService.State state;
			if (MapHybridRenderService.instance == null || !MapHybridRenderService.instance.states.TryGetValue(payload, out state))
			{
				return;
			}
			Predicate<MapHybridRenderService.Slot> <>9__0;
			foreach (MapHybridRenderService.Group group in MapHybridRenderService.instance.groups.Values)
			{
				List<MapHybridRenderService.Slot> slots = group.slots;
				Predicate<MapHybridRenderService.Slot> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (MapHybridRenderService.Slot slot) => slot.state == state);
				}
				slots.RemoveAll(predicate);
			}
			MapHybridRenderService.instance.states.Remove(payload);
			MapHybridRenderService.instance.dirty = true;
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x001A8600 File Offset: 0x001A6800
		private void BindManager()
		{
			if (this.manager == ChunkManager.Instance)
			{
				return;
			}
			this.UnbindManager();
			this.manager = ChunkManager.Instance;
			foreach (MapHybridRenderService.State state in this.states.Values)
			{
				state.owner = null;
				state.ready = false;
			}
			if (this.manager == null)
			{
				return;
			}
			this.manager.OnChunkActivated += this.OnChunkChanged;
			this.manager.OnChunkDeactivated += this.OnChunkChanged;
			this.manager.OnChunkRemoved += this.OnChunkChanged;
			this.manager.OnChunkZonesReady += this.OnZonesReady;
			this.manager.OnContentRootReady += this.OnContentReady;
			this.pollNow = true;
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x001A870C File Offset: 0x001A690C
		private void UnbindManager()
		{
			if (this.manager == null)
			{
				return;
			}
			this.manager.OnChunkActivated -= this.OnChunkChanged;
			this.manager.OnChunkDeactivated -= this.OnChunkChanged;
			this.manager.OnChunkRemoved -= this.OnChunkChanged;
			this.manager.OnChunkZonesReady -= this.OnZonesReady;
			this.manager.OnContentRootReady -= this.OnContentReady;
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x001A879B File Offset: 0x001A699B
		private void OnChunkChanged(ChunkInstance chunk)
		{
			this.pollNow = true;
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x001A87A4 File Offset: 0x001A69A4
		private void OnZonesReady(ChunkInstance chunk)
		{
			foreach (MapHybridRenderService.State state in this.states.Values)
			{
				if (state.active && state.payload != null && chunk.chunkRoot != null && state.payload.transform.IsChildOf(chunk.chunkRoot.transform))
				{
					state.owner = chunk;
					state.ready = true;
				}
			}
			this.pollNow = true;
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x001A884C File Offset: 0x001A6A4C
		private void OnContentReady(GameObject root)
		{
			if (root == null)
			{
				return;
			}
			foreach (MapHybridRenderService.State state in this.states.Values)
			{
				if (state.active && state.payload != null && state.payload.transform.IsChildOf(root.transform))
				{
					state.ready = true;
				}
			}
			this.pollNow = true;
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x001A88E4 File Offset: 0x001A6AE4
		private void LateUpdate()
		{
			if (MapHybridRenderPayload.HasFailure || this.quitting)
			{
				return;
			}
			using (MapHybridRenderService.SyncMarker.Auto())
			{
				bool flag = this.pollNow || Time.unscaledTime >= this.nextPoll;
				if (flag)
				{
					this.BindManager();
					this.nextPoll = Time.unscaledTime + 0.1f;
					this.pollNow = false;
				}
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				foreach (MapHybridRenderService.State state in this.states.Values)
				{
					if (state.active && !(state.payload == null))
					{
						num++;
						num3 += state.payload.entries.Length;
						Matrix4x4 localToWorldMatrix = state.payload.transform.localToWorldMatrix;
						if (state.matrix != localToWorldMatrix)
						{
							state.matrix = localToWorldMatrix;
							this.dirty = true;
						}
						if (flag && (state.owner == null || state.owner.chunkRoot == null || !state.payload.transform.IsChildOf(state.owner.chunkRoot.transform)))
						{
							state.owner = ((this.manager != null) ? this.manager.GetChunkContaining(state.payload.transform) : null);
						}
						object obj = this.manager != null && this.manager.IsChunkPresentationVisible(state.owner);
						bool flag2 = (state.staged == null || !state.staged.HasSnapshot || state.staged.IsOperationalReady) && (state.heavy == null || state.heavy.IsOperationalReady);
						if (!state.ready && flag2 && state.owner != null && this.manager != null && this.manager.IsInitialGenerationComplete && !this.manager.HasPendingZoneContent(state.owner))
						{
							state.ready = true;
						}
						object obj2 = obj;
						bool flag3 = (obj2 & flag2) != null && state.ready;
						if (obj2 != null && !flag3)
						{
							num2++;
						}
						if (state.visible != flag3)
						{
							state.visible = flag3;
							this.dirty = true;
							MapHybridRenderPayload.VisibilityChanges++;
						}
						for (int i = 0; i < state.sentinels.Length; i++)
						{
							Renderer renderer = state.sentinels[i];
							bool flag4 = renderer != null && renderer.enabled && renderer.gameObject.activeInHierarchy && !renderer.forceRenderingOff;
							if (flag4 != state.sentinelVisible[i])
							{
								state.sentinelVisible[i] = flag4;
								this.dirty = true;
							}
						}
					}
				}
				MapHybridRenderPayload.ActivePayloads = num;
				MapHybridRenderPayload.ExpectedSourceInstances = num3;
				MapHybridRenderPayload.PendingVisiblePayloads = num2;
				if (this.dirty)
				{
					this.Upload();
				}
			}
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x001A8C4C File Offset: 0x001A6E4C
		private void Upload()
		{
			if (MapHybridRenderPayload.HasFailure)
			{
				return;
			}
			using (MapHybridRenderService.UploadMarker.Auto())
			{
				try
				{
					int num = 0;
					int num2 = 0;
					foreach (MapHybridRenderService.Group group in this.groups.Values)
					{
						int num3 = 0;
						foreach (MapHybridRenderService.Slot slot in group.slots)
						{
							if (slot.state.active && slot.state.payload != null)
							{
								num2++;
							}
							if (MapHybridRenderService.IsVisible(slot))
							{
								num3++;
							}
						}
						int num4 = Mathf.NextPowerOfTwo(Math.Max(1, group.slots.Count));
						if (group.capacity < num4)
						{
							if (!GPUICoreAPI.SetBufferSize(group.rendererKey, num4))
							{
								throw new InvalidOperationException("GPUI buffer reservation rejected");
							}
							group.capacity = num4;
							group.matrices = new Matrix4x4[num4];
						}
						int num5 = 0;
						foreach (MapHybridRenderService.Slot slot2 in group.slots)
						{
							if (MapHybridRenderService.IsVisible(slot2))
							{
								group.matrices[num5++] = slot2.state.matrix * slot2.entry.localMatrix;
							}
						}
						if (num3 > 0 && !GPUICoreAPI.SetTransformBufferData(group.rendererKey, group.matrices, 0, 0, num3, true))
						{
							throw new InvalidOperationException("GPUI matrix upload rejected");
						}
						if (!GPUICoreAPI.SetInstanceCount(group.rendererKey, num3))
						{
							throw new InvalidOperationException("GPUI instance count rejected");
						}
						num += num3;
					}
					MapHybridRenderPayload.ActiveInstances = num;
					MapHybridRenderPayload.RegisteredSourceInstances = num2;
					MapHybridRenderPayload.UploadCount++;
					this.dirty = false;
				}
				catch (Exception ex)
				{
					this.Fail(ex.Message);
				}
			}
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x001A8EE4 File Offset: 0x001A70E4
		private static bool IsVisible(MapHybridRenderService.Slot slot)
		{
			return slot.state.active && slot.state.visible && slot.state.payload != null && slot.state.sentinelVisible[slot.sentinelIndex];
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x001A8F34 File Offset: 0x001A7134
		private void OnOriginShifted(Vector3 delta)
		{
			MapHybridRenderPayload.OriginShiftCount++;
			using (Dictionary<MapHybridRenderService.Key, MapHybridRenderService.Group>.ValueCollection.Enumerator enumerator = this.groups.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GPUITransformBufferData gpuitransformBufferData;
					int num;
					int num2;
					if (GPUICoreAPI.TryGetTransformBufferData(enumerator.Current.rendererKey, out gpuitransformBufferData, out num, out num2, false) && gpuitransformBufferData.TransformBufferValues != null)
					{
						foreach (GPUIShaderBuffer gpuishaderBuffer in gpuitransformBufferData.TransformBufferValues)
						{
							gpuishaderBuffer.CompleteAsyncRequests();
						}
					}
				}
			}
			foreach (MapHybridRenderService.State state in this.states.Values)
			{
				if (state.payload != null)
				{
					state.matrix = state.payload.transform.localToWorldMatrix;
				}
			}
			this.dirty = true;
			this.Upload();
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x001A9064 File Offset: 0x001A7264
		public void OnLightProbesUpdated()
		{
			if (LightmapSettings.lightProbes != null && LightmapSettings.lightProbes.count > 0)
			{
				this.Fail("Local light probes appeared after registration; combined-cell probe equivalence is unverified");
				return;
			}
			this.dirty = true;
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x001A9094 File Offset: 0x001A7294
		private void Fail(string reason)
		{
			if (MapHybridRenderPayload.HasFailure)
			{
				return;
			}
			MapHybridRenderPayload.HasFailure = true;
			MapHybridRenderPayload.FailureCount++;
			MapHybridRenderPayload.LastError = reason;
			foreach (MapHybridRenderService.Group group in this.groups.Values)
			{
				GPUICoreAPI.SetInstanceCount(group.rendererKey, 0);
			}
			foreach (MapHybridRenderService.State state in this.states.Values)
			{
				if (state.payload != null)
				{
					if (state.payload.nativeResidualRoot != null)
					{
						state.payload.nativeResidualRoot.SetActive(false);
					}
					if (state.payload.nativeFallbackRoot != null)
					{
						state.payload.nativeFallbackRoot.SetActive(true);
					}
				}
			}
			MapHybridRenderPayload.ActiveInstances = 0;
			Debug.LogError("[MapHybridRender] CAPTURE INVALID: " + reason, this);
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x001A91C0 File Offset: 0x001A73C0
		private void OnApplicationQuit()
		{
			this.quitting = true;
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x001A91CC File Offset: 0x001A73CC
		private void OnDestroy()
		{
			this.quitting = true;
			this.UnbindManager();
			WorldOrigin.Shifted -= this.OnOriginShifted;
			foreach (MapHybridRenderService.Group group in this.groups.Values)
			{
				GPUICoreAPI.DisposeRenderer(group.rendererKey);
				if (group.data != null)
				{
					Object.Destroy(group.data);
				}
			}
			if (this.profile != null)
			{
				Object.Destroy(this.profile);
			}
			this.states.Clear();
			this.groups.Clear();
			MapHybridRenderPayload.RegisteredGroups = (MapHybridRenderPayload.ActiveInstances = (MapHybridRenderPayload.ActivePayloads = 0));
			if (MapHybridRenderService.instance == this)
			{
				MapHybridRenderService.instance = null;
			}
		}

		// Token: 0x040045BC RID: 17852
		private static MapHybridRenderService instance;

		// Token: 0x040045BD RID: 17853
		private static readonly ProfilerMarker SyncMarker = new ProfilerMarker("MapHybridRender.Sync");

		// Token: 0x040045BE RID: 17854
		private static readonly ProfilerMarker UploadMarker = new ProfilerMarker("MapHybridRender.Upload");

		// Token: 0x040045BF RID: 17855
		private static readonly ProfilerMarker RegisterMarker = new ProfilerMarker("MapHybridRender.Register");

		// Token: 0x040045C0 RID: 17856
		private readonly Dictionary<MapHybridRenderPayload, MapHybridRenderService.State> states = new Dictionary<MapHybridRenderPayload, MapHybridRenderService.State>();

		// Token: 0x040045C1 RID: 17857
		private readonly Dictionary<MapHybridRenderService.Key, MapHybridRenderService.Group> groups = new Dictionary<MapHybridRenderService.Key, MapHybridRenderService.Group>();

		// Token: 0x040045C2 RID: 17858
		private ChunkManager manager;

		// Token: 0x040045C3 RID: 17859
		private GPUIProfile profile;

		// Token: 0x040045C4 RID: 17860
		private bool dirty;

		// Token: 0x040045C5 RID: 17861
		private bool pollNow;

		// Token: 0x040045C6 RID: 17862
		private bool quitting;

		// Token: 0x040045C7 RID: 17863
		private float nextPoll;

		// Token: 0x020009BE RID: 2494
		private sealed class State
		{
			// Token: 0x040045C8 RID: 17864
			public MapHybridRenderPayload payload;

			// Token: 0x040045C9 RID: 17865
			public ChunkInstance owner;

			// Token: 0x040045CA RID: 17866
			public bool active;

			// Token: 0x040045CB RID: 17867
			public bool ready;

			// Token: 0x040045CC RID: 17868
			public bool visible;

			// Token: 0x040045CD RID: 17869
			public Matrix4x4 matrix;

			// Token: 0x040045CE RID: 17870
			public Renderer[] sentinels;

			// Token: 0x040045CF RID: 17871
			public bool[] sentinelVisible;

			// Token: 0x040045D0 RID: 17872
			public IncrementalPooledZoneActivation staged;

			// Token: 0x040045D1 RID: 17873
			public HeavyZoneStreamingProxy heavy;
		}

		// Token: 0x020009BF RID: 2495
		private sealed class Slot
		{
			// Token: 0x040045D2 RID: 17874
			public MapHybridRenderService.State state;

			// Token: 0x040045D3 RID: 17875
			public MapHybridRenderPayload.Entry entry;

			// Token: 0x040045D4 RID: 17876
			public int sentinelIndex;
		}

		// Token: 0x020009C0 RID: 2496
		private sealed class Group
		{
			// Token: 0x040045D5 RID: 17877
			public int rendererKey;

			// Token: 0x040045D6 RID: 17878
			public int capacity;

			// Token: 0x040045D7 RID: 17879
			public GPUILODGroupData data;

			// Token: 0x040045D8 RID: 17880
			public readonly List<MapHybridRenderService.Slot> slots = new List<MapHybridRenderService.Slot>();

			// Token: 0x040045D9 RID: 17881
			public Matrix4x4[] matrices = new Matrix4x4[0];
		}

		// Token: 0x020009C1 RID: 2497
		private sealed class Key : IEquatable<MapHybridRenderService.Key>
		{
			// Token: 0x06004644 RID: 17988 RVA: 0x001A9320 File Offset: 0x001A7520
			public Key(MapHybridRenderPayload.Entry entry)
			{
				this.e = entry;
				this.hash = this.e.mesh.GetInstanceID();
				foreach (Material material in this.e.materials)
				{
					this.hash = this.hash * 31 + material.GetInstanceID();
				}
				this.hash = this.hash * 31 + this.e.layer;
				this.hash = (int)(this.hash * 31 + this.e.shadows);
				this.hash = this.hash * 31 + (int)this.e.renderingLayerMask;
				this.hash = this.hash * 31 + (this.e.receiveShadows ? 1 : 0);
				this.hash = (int)(this.hash * 31 + this.e.lightProbeUsage);
				this.hash = (int)(this.hash * 31 + this.e.reflectionProbeUsage);
			}

			// Token: 0x06004645 RID: 17989 RVA: 0x001A942C File Offset: 0x001A762C
			public bool Equals(MapHybridRenderService.Key other)
			{
				if (other == null)
				{
					return false;
				}
				MapHybridRenderPayload.Entry entry = other.e;
				if (this.e.mesh != entry.mesh || this.e.layer != entry.layer || this.e.shadows != entry.shadows || this.e.receiveShadows != entry.receiveShadows || this.e.renderingLayerMask != entry.renderingLayerMask || this.e.lightProbeUsage != entry.lightProbeUsage || this.e.reflectionProbeUsage != entry.reflectionProbeUsage || this.e.materials.Length != entry.materials.Length)
				{
					return false;
				}
				for (int i = 0; i < this.e.materials.Length; i++)
				{
					if (this.e.materials[i] != entry.materials[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06004646 RID: 17990 RVA: 0x001A9522 File Offset: 0x001A7722
			public override bool Equals(object obj)
			{
				return this.Equals(obj as MapHybridRenderService.Key);
			}

			// Token: 0x06004647 RID: 17991 RVA: 0x001A9530 File Offset: 0x001A7730
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x040045DA RID: 17882
			private readonly MapHybridRenderPayload.Entry e;

			// Token: 0x040045DB RID: 17883
			private readonly int hash;
		}
	}
}
