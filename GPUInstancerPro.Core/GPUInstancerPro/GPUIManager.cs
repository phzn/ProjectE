using System;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

namespace GPUInstancerPro
{
	// Token: 0x02000008 RID: 8
	public abstract class GPUIManager : MonoBehaviour, IGPUIDisposable, IDisposable, IGPUILightProbeDataProvider
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000242E File Offset: 0x0000062E
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002436 File Offset: 0x00000636
		public bool IsInitialized { get; private set; }

		// Token: 0x06000023 RID: 35 RVA: 0x0000243F File Offset: 0x0000063F
		protected virtual void Awake()
		{
			if (Application.isPlaying && this._disablePlayModeRendering)
			{
				Object.Destroy(this);
				return;
			}
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002466 File Offset: 0x00000666
		protected virtual void Start()
		{
			this.CheckPrototypeChanges();
			if (Application.isPlaying && this.isDontDestroyOnLoad)
			{
				Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002488 File Offset: 0x00000688
		protected virtual void OnEnable()
		{
			if (Application.isPlaying && this._disablePlayModeRendering)
			{
				Object.Destroy(this);
				this.Dispose();
				return;
			}
			if (!this.IsInitialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000024B4 File Offset: 0x000006B4
		protected virtual void OnDisable()
		{
			this.Dispose();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000024BC File Offset: 0x000006BC
		protected virtual void Update()
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000024BC File Offset: 0x000006BC
		protected virtual void LateUpdate()
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000024BE File Offset: 0x000006BE
		public virtual bool IsValid(bool logError)
		{
			this.errorCode = 0;
			this.errorFixAction = null;
			if (this._disablePlayModeRendering)
			{
				this.errorCode = -106;
				this.errorFixAction = delegate
				{
					this._disablePlayModeRendering = false;
				};
			}
			return true;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000024F4 File Offset: 0x000006F4
		public virtual void Initialize()
		{
			this.Dispose();
			this.CheckPrototypeChanges();
			this.IsValid(Application.isPlaying);
			GPUIRenderingSystem.AddActiveManager(this);
			for (int i = 0; i < this._prototypes.Length; i++)
			{
				this.RegisterRenderer(i);
			}
			this.IsInitialized = true;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void ReleaseBuffers()
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002544 File Offset: 0x00000744
		public virtual void Dispose()
		{
			this._dependentJob.Complete();
			if (!this.IsInitialized)
			{
				return;
			}
			this.IsInitialized = false;
			this.ReleaseBuffers();
			GPUIRenderingSystem.RemoveActiveManager(this);
			if (this._runtimeRenderKeys != null)
			{
				for (int i = 0; i < this._runtimeRenderKeys.Length; i++)
				{
					this.DisposeRenderer(i);
				}
				this._runtimeRenderKeys = null;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000025A4 File Offset: 0x000007A4
		public virtual void OnPrototypeEnabledStatusChanged(int prototypeIndex, bool isEnabled)
		{
			if (prototypeIndex < 0 || this._prototypes.Length <= prototypeIndex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					GPUIConstants.LOG_PREFIX,
					"Given prototype index is out of range: ",
					prototypeIndex.ToString(),
					". Max index: ",
					(this._prototypes.Length - 1).ToString()
				}));
				return;
			}
			this._prototypes[prototypeIndex].isEnabled = isEnabled;
			if (!this.IsInitialized)
			{
				return;
			}
			if (isEnabled && this._runtimeRenderKeys[prototypeIndex] == 0)
			{
				this.OnPrototypeEnabled(prototypeIndex);
				return;
			}
			if (!isEnabled && this._runtimeRenderKeys[prototypeIndex] != 0)
			{
				this.OnPrototypeDisabled(prototypeIndex);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002648 File Offset: 0x00000848
		protected virtual void OnPrototypeEnabled(int prototypeIndex)
		{
			this.RegisterRenderer(prototypeIndex);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002652 File Offset: 0x00000852
		protected virtual void OnPrototypeDisabled(int prototypeIndex)
		{
			this.DisposeRenderer(prototypeIndex);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000265C File Offset: 0x0000085C
		protected virtual bool RegisterRenderer(int prototypeIndex)
		{
			GPUIPrototype gpuiprototype = this._prototypes[prototypeIndex];
			if (gpuiprototype == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Prototype at index: " + prototypeIndex.ToString() + " is null.", this);
				return false;
			}
			if (!gpuiprototype.IsValid(true) || !gpuiprototype.isEnabled)
			{
				return false;
			}
			int num;
			if (GPUIRenderingSystem.RegisterRenderer(this, gpuiprototype, out num, this.GetRendererGroupID(prototypeIndex), this.GetTransformBufferType(prototypeIndex), this.GetShaderKeywords(prototypeIndex)))
			{
				this._runtimeRenderKeys[prototypeIndex] = num;
				return true;
			}
			return false;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000026D8 File Offset: 0x000008D8
		protected virtual void DisposeRenderer(int prototypeIndex)
		{
			if (this._runtimeRenderKeys == null || this._runtimeRenderKeys.Length <= prototypeIndex)
			{
				return;
			}
			int num = this._runtimeRenderKeys[prototypeIndex];
			if (num != 0)
			{
				GPUIRenderingSystem.DisposeRenderer(num);
			}
			this._runtimeRenderKeys[prototypeIndex] = 0;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002714 File Offset: 0x00000914
		internal void OnRenderSourceDisposed(int runtimeRenderKey)
		{
			int prototypeIndex = this.GetPrototypeIndex(runtimeRenderKey);
			if (prototypeIndex >= 0 && this._runtimeRenderKeys[prototypeIndex] != 0)
			{
				this._runtimeRenderKeys[prototypeIndex] = 0;
				this.DisposeRenderer(prototypeIndex);
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002748 File Offset: 0x00000948
		protected virtual void DisposeAllRenderers()
		{
			if (this._runtimeRenderKeys == null)
			{
				return;
			}
			for (int i = 0; i < this._runtimeRenderKeys.Length; i++)
			{
				int num = this._runtimeRenderKeys[i];
				if (num != 0)
				{
					GPUIRenderingSystem.DisposeRenderer(num);
				}
				this._runtimeRenderKeys[i] = 0;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000278C File Offset: 0x0000098C
		public int GetRenderKey(int prototypeIndex)
		{
			if (!this.IsInitialized || this._runtimeRenderKeys == null || this._runtimeRenderKeys.Length < prototypeIndex || prototypeIndex < 0)
			{
				return 0;
			}
			return this._runtimeRenderKeys[prototypeIndex];
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000027B8 File Offset: 0x000009B8
		public int GetPrototypeIndex(int renderKey)
		{
			if (this._runtimeRenderKeys != null)
			{
				for (int i = 0; i < this._runtimeRenderKeys.Length; i++)
				{
					if (this._runtimeRenderKeys[i] == renderKey)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000027F0 File Offset: 0x000009F0
		public int GetPrototypeIndex(GameObject prefabObject)
		{
			if (prefabObject == null)
			{
				return -1;
			}
			if (this._prototypes != null)
			{
				for (int i = 0; i < this._prototypes.Length; i++)
				{
					if (this._prototypes[i].prototypeType == GPUIPrototypeType.Prefab && this._prototypes[i].prefabObject == prefabObject)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000284C File Offset: 0x00000A4C
		public int GetPrototypeIndex(GPUILODGroupData lgd)
		{
			if (lgd == null)
			{
				return -1;
			}
			if (this._prototypes != null)
			{
				for (int i = 0; i < this._prototypes.Length; i++)
				{
					if (this._prototypes[i].prototypeType == GPUIPrototypeType.LODGroupData && this._prototypes[i].gpuiLODGroupData == lgd)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000028A7 File Offset: 0x00000AA7
		public virtual GPUIProfile GetDefaultProfile()
		{
			if (this.defaultProfile != null)
			{
				return this.defaultProfile;
			}
			return GPUIProfile.DefaultProfile;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000028C4 File Offset: 0x00000AC4
		public virtual GPUIProfile GetDefaultProfile(GameObject prefab)
		{
			GPUIProfile defaultGPUSkinningProfile = this.GetDefaultProfile();
			if (prefab.HasComponent<GPUISkinningBase>() && GPUIProfile.defaultGPUSkinningProfile != null)
			{
				defaultGPUSkinningProfile = GPUIProfile.defaultGPUSkinningProfile;
			}
			return defaultGPUSkinningProfile;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000028F4 File Offset: 0x00000AF4
		public virtual int GetRendererGroupID(int prototypeIndex)
		{
			return 0;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000028F4 File Offset: 0x00000AF4
		public virtual GPUITransformBufferType GetTransformBufferType(int prototypeIndex)
		{
			return GPUITransformBufferType.Default;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000028F7 File Offset: 0x00000AF7
		public virtual List<string> GetShaderKeywords(int prototypeIndex)
		{
			return null;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000028FC File Offset: 0x00000AFC
		public void OnLightProbesUpdated()
		{
			if (GPUIRenderingSystem.WillUpdateLightProbes() || this._runtimeRenderKeys == null)
			{
				return;
			}
			for (int i = 0; i < this._runtimeRenderKeys.Length; i++)
			{
				GPUITransformBufferData gpuitransformBufferData;
				int num;
				int num2;
				if (GPUIRenderingSystem.TryGetTransformBufferData(this._runtimeRenderKeys[i], out gpuitransformBufferData, out num, out num2, false) && gpuitransformBufferData.IsGeneratePerInstanceLightProbes)
				{
					this.OnUpdatePerInstanceLightProbes(i);
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000024BC File Offset: 0x000006BC
		protected virtual void OnUpdatePerInstanceLightProbes(int prototypeIndex)
		{
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002951 File Offset: 0x00000B51
		public virtual void CheckPrototypeChanges()
		{
			this.ClearNullPrototypes();
			this.SynchronizeData();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002960 File Offset: 0x00000B60
		protected virtual bool ValidatePrototype(int prototypeIndex)
		{
			GPUIPrototype gpuiprototype = this._prototypes[prototypeIndex];
			return gpuiprototype.IsValid(Application.isPlaying) && (gpuiprototype.prototypeType != GPUIPrototypeType.Prefab || GPUIManager.ValidatePrefabPrototype(gpuiprototype));
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002998 File Offset: 0x00000B98
		public static bool ValidatePrefabPrototype(GPUIPrototype prototype)
		{
			LODGroup lodgroup;
			if (prototype.prefabObject.TryGetComponent<LODGroup>(out lodgroup))
			{
				LOD[] lods = lodgroup.GetLODs();
				for (int i = 0; i < lods.Length; i++)
				{
					bool flag = false;
					if (lods[i].renderers != null)
					{
						foreach (Renderer renderer in lods[i].renderers)
						{
							if (renderer != null)
							{
								if (renderer is BillboardRenderer)
								{
									flag = true;
								}
								else
								{
									MeshRenderer meshRenderer = renderer as MeshRenderer;
									if (meshRenderer != null)
									{
										flag = true;
										MeshFilter meshFilter;
										if (!meshRenderer.TryGetComponent<MeshFilter>(out meshFilter) || meshFilter.sharedMesh == null)
										{
											prototype.errorCode = 2005;
											return false;
										}
									}
									else
									{
										SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
										if (skinnedMeshRenderer == null)
										{
											goto IL_0126;
										}
										flag = true;
										if (skinnedMeshRenderer.sharedMesh == null)
										{
											prototype.errorCode = 2005;
											return false;
										}
									}
									Material[] sharedMaterials = renderer.sharedMaterials;
									if (sharedMaterials.Contains(null))
									{
										prototype.errorCode = 2004;
										return false;
									}
									Material[] array = sharedMaterials;
									for (int k = 0; k < array.Length; k++)
									{
										if (array[k].shader == null)
										{
											prototype.errorCode = 2006;
											return false;
										}
									}
								}
							}
							IL_0126:;
						}
					}
					if (!flag)
					{
						prototype.errorCode = 2002;
						return false;
					}
				}
			}
			else
			{
				if (prototype.prefabObject.GetComponentInChildren<LODGroup>(true) != null)
				{
					prototype.errorCode = 2001;
					return false;
				}
				MeshRenderer[] componentsInChildren = prototype.prefabObject.GetComponentsInChildren<MeshRenderer>();
				SkinnedMeshRenderer[] componentsInChildren2 = prototype.prefabObject.GetComponentsInChildren<SkinnedMeshRenderer>();
				if ((componentsInChildren == null || componentsInChildren.Length == 0) && (componentsInChildren2 == null || componentsInChildren2.Length == 0))
				{
					prototype.errorCode = 2003;
					return false;
				}
				foreach (MeshRenderer meshRenderer2 in componentsInChildren)
				{
					if (meshRenderer2.sharedMaterials == null || meshRenderer2.sharedMaterials.Contains(null))
					{
						prototype.errorCode = 2004;
						return false;
					}
					MeshFilter meshFilter2;
					if (!meshRenderer2.TryGetComponent<MeshFilter>(out meshFilter2) || meshFilter2.sharedMesh == null)
					{
						prototype.errorCode = 2005;
						return false;
					}
					Material[] array = meshRenderer2.sharedMaterials;
					for (int k = 0; k < array.Length; k++)
					{
						if (array[k].shader == null)
						{
							prototype.errorCode = 2006;
							return false;
						}
					}
				}
				foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in componentsInChildren2)
				{
					if (skinnedMeshRenderer2.sharedMaterials == null || skinnedMeshRenderer2.sharedMaterials.Contains(null))
					{
						prototype.errorCode = 2004;
						return false;
					}
					if (skinnedMeshRenderer2.sharedMesh == null)
					{
						prototype.errorCode = 2005;
						return false;
					}
					Material[] array = skinnedMeshRenderer2.sharedMaterials;
					for (int k = 0; k < array.Length; k++)
					{
						if (array[k].shader == null)
						{
							prototype.errorCode = 2006;
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002CA4 File Offset: 0x00000EA4
		protected virtual void SynchronizeData()
		{
			int num = this._prototypes.Length;
			if (this._runtimeRenderKeys == null)
			{
				this._runtimeRenderKeys = new int[num];
				return;
			}
			if (this._runtimeRenderKeys.Length != num)
			{
				Array.Resize<int>(ref this._runtimeRenderKeys, num);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002CE8 File Offset: 0x00000EE8
		protected virtual void ClearNullPrototypes()
		{
			if (this._prototypes == null)
			{
				this._prototypes = new GPUIPrototype[0];
			}
			int num = this._prototypes.Length;
			for (int i = 0; i < num; i++)
			{
				GPUIPrototype gpuiprototype = this._prototypes[i];
				if (gpuiprototype.prototypeType == GPUIPrototypeType.Prefab && gpuiprototype.prefabObject == null)
				{
					this.RemovePrototypeAtIndex(i);
					return;
				}
			}
			if (this._runtimeRenderKeys == null)
			{
				this._runtimeRenderKeys = new int[num];
			}
			bool flag = true;
			for (int j = 0; j < this._prototypes.Length; j++)
			{
				if (this._prototypes[j] == null)
				{
					this.RemovePrototypeAtIndex(j);
					return;
				}
				flag &= this.ValidatePrototype(j);
			}
			if (!flag && Application.isPlaying && !this._loggedPrototypeValidationError)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "There are errors with the prototype setup, please check the prototypes on " + GPUIUtility.CamelToTitleCase(base.GetType().Name) + " for further information.", base.gameObject);
				this._loggedPrototypeValidationError = true;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002DDC File Offset: 0x00000FDC
		public virtual int AddPrototype(GPUIPrototype prototype)
		{
			if (!prototype.IsValid(true))
			{
				return -1;
			}
			if (this._prototypes != null && this._prototypes.Contains(prototype))
			{
				return -1;
			}
			int num = 0;
			if (this._prototypes == null)
			{
				this._prototypes = new GPUIPrototype[1];
			}
			else
			{
				num = this._prototypes.Length;
				Array.Resize<GPUIPrototype>(ref this._prototypes, num + 1);
			}
			this._prototypes[num] = prototype;
			this.CheckPrototypeChanges();
			prototype.GenerateBillboard(false);
			if (this.IsInitialized)
			{
				this.RegisterRenderer(num);
			}
			return num;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002E62 File Offset: 0x00001062
		public int AddPrototype(GameObject prefab)
		{
			return this.AddPrototype(new GPUIPrototype(prefab, this.GetDefaultProfile(prefab)));
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002E77 File Offset: 0x00001077
		public int AddPrototype(GameObject prefab, GPUIProfile profile)
		{
			return this.AddPrototype(new GPUIPrototype(prefab, profile));
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002E86 File Offset: 0x00001086
		public virtual void RemovePrototypeAtIndex(int index)
		{
			if (this.IsInitialized)
			{
				this.DisposeRenderer(index);
				this._runtimeRenderKeys = this._runtimeRenderKeys.RemoveAtAndReturn<int>(index);
			}
			this._prototypes = this._prototypes.RemoveAtAndReturn<GPUIPrototype>(index);
			this.CheckPrototypeChanges();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002EC1 File Offset: 0x000010C1
		public virtual void RemoveAllPrototypes()
		{
			if (this.IsInitialized)
			{
				this.DisposeAllRenderers();
				this._runtimeRenderKeys = new int[0];
			}
			this._prototypes = new GPUIPrototype[0];
			this.CheckPrototypeChanges();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002EF0 File Offset: 0x000010F0
		public virtual bool CanAddObjectAsPrototype(Object obj)
		{
			GameObject gameObject = obj as GameObject;
			if (gameObject != null)
			{
				Transform transform = gameObject.transform;
				Stack<Transform> stack = new Stack<Transform>();
				stack.Push(transform);
				while (stack.Count > 0)
				{
					Transform transform2 = stack.Pop();
					if (!(transform2.GetComponent<GPUIPrefabDefinition>() != null))
					{
						if (transform2 != transform && transform2.GetComponent<LODGroup>() != null)
						{
							Debug.LogWarning(GPUIConstants.LOG_PREFIX + "'" + gameObject.name + "' cannot be added as prototype because it contains an LODGroup in its children.", gameObject);
							return false;
						}
						for (int i = 0; i < transform2.childCount; i++)
						{
							stack.Push(transform2.GetChild(i));
						}
					}
				}
				for (int j = 0; j < this._prototypes.Length; j++)
				{
					if (this._prototypes[j].prefabObject == gameObject)
					{
						return false;
					}
				}
				return true;
			}
			GPUILODGroupData gpuilodgroupData = obj as GPUILODGroupData;
			if (gpuilodgroupData != null)
			{
				for (int k = 0; k < this._prototypes.Length; k++)
				{
					if (this._prototypes[k].gpuiLODGroupData == gpuilodgroupData)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000300A File Offset: 0x0000120A
		public int GetPrototypeCount()
		{
			if (this._prototypes == null)
			{
				return 0;
			}
			return this._prototypes.Length;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000301E File Offset: 0x0000121E
		public GPUIPrototype GetPrototype(int index)
		{
			if (this._prototypes == null || this._prototypes.Length <= index)
			{
				return null;
			}
			return this._prototypes[index];
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000028F7 File Offset: 0x00000AF7
		public virtual GPUIPrototypeData GetPrototypeDataGeneric(int prototypeIndex)
		{
			return null;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void OnPrototypePropertiesModified()
		{
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003040 File Offset: 0x00001240
		public virtual int GetRegisteredInstanceCount(int prototypeIndex)
		{
			if (!this.IsInitialized || prototypeIndex < 0 || this._runtimeRenderKeys == null || prototypeIndex >= this._runtimeRenderKeys.Length)
			{
				return 0;
			}
			GPUIRenderSource gpuirenderSource;
			if (GPUIRenderingSystem.TryGetRenderSource(this._runtimeRenderKeys[prototypeIndex], out gpuirenderSource))
			{
				return gpuirenderSource.instanceCount;
			}
			return 0;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003087 File Offset: 0x00001287
		public virtual void AddDependentJob(JobHandle jobHandle)
		{
			this._dependentJob = JobHandle.CombineDependencies(this._dependentJob, jobHandle);
		}

		// Token: 0x0400000B RID: 11
		[SerializeField]
		public bool isDontDestroyOnLoad;

		// Token: 0x0400000C RID: 12
		[SerializeField]
		protected GPUIPrototype[] _prototypes;

		// Token: 0x0400000D RID: 13
		[SerializeField]
		public GPUIProfile defaultProfile;

		// Token: 0x0400000E RID: 14
		[SerializeField]
		public bool isEnableDefaultRenderingWhenDisabled = true;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private bool _disablePlayModeRendering;

		// Token: 0x04000010 RID: 16
		[NonSerialized]
		protected int[] _runtimeRenderKeys;

		// Token: 0x04000012 RID: 18
		[NonSerialized]
		protected JobHandle _dependentJob;

		// Token: 0x04000013 RID: 19
		[NonSerialized]
		private bool _loggedPrototypeValidationError;

		// Token: 0x04000014 RID: 20
		[NonSerialized]
		public int errorCode;

		// Token: 0x04000015 RID: 21
		[NonSerialized]
		public UnityAction errorFixAction;

		// Token: 0x04000016 RID: 22
		[NonSerialized]
		private static readonly Type[] _prefabRendererTypes = new Type[]
		{
			typeof(MeshRenderer),
			typeof(BillboardRenderer)
		};

		// Token: 0x04000017 RID: 23
		private const int ERROR_CODE_ADDITION = 200;
	}
}
