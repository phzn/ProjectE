using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000016 RID: 22
	[ExecuteInEditMode]
	[DefaultExecutionOrder(100)]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#The_Prefab_Manager")]
	public class GPUIPrefabManager : GPUIManagerWithPrototypeData<GPUIPrefabPrototypeData>, IGPUIInstanceTransformProvider
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00003AC8 File Offset: 0x00001CC8
		protected override void OnEnable()
		{
			this._isNullOrInstancedPredicate = new Predicate<GPUIPrefab>(this.IsPrefabNullOrInstanced);
			base.OnEnable();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003AE2 File Offset: 0x00001CE2
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003AEA File Offset: 0x00001CEA
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (!GPUIRenderingSystem.IsActive || !base.IsInitialized)
			{
				return;
			}
			this.AddRemoveInstances();
			this.StartAutoUpdateTransformJobs();
			this.AutoEnableDisablePrototype();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003B14 File Offset: 0x00001D14
		public override void Initialize()
		{
			base.Initialize();
			if (!GPUIRenderingSystem.IsActive || !base.IsInitialized)
			{
				return;
			}
			Dictionary<int, List<GPUIPrefab>> dictionary = new Dictionary<int, List<GPUIPrefab>>();
			for (int i = 0; i < this._prototypes.Length; i++)
			{
				List<GPUIPrefab> list = new List<GPUIPrefab>();
				int prefabID = this.GetPrefabID(i);
				if (dictionary.ContainsKey(prefabID))
				{
					Debug.LogError(GPUIConstants.LOG_PREFIX + "There are multiple prototypes with the same prefab ID: " + prefabID.ToString(), this);
				}
				else
				{
					dictionary.Add(prefabID, list);
					if (!this.isFindInstancesAtInitialization && this._prototypeDataArray[i].GetRegisteredInstanceCount() > 0)
					{
						foreach (GameObject gameObject in this._prototypeDataArray[i].registeredInstances.prefabInstances)
						{
							GPUIPrefab gpuiprefab;
							if (gameObject != null && gameObject.TryGetComponent<GPUIPrefab>(out gpuiprefab) && !gpuiprefab.IsInstanced)
							{
								list.Add(gpuiprefab);
								gpuiprefab._isBeingAddedToThePrefabManager = true;
							}
						}
					}
				}
			}
			if (this.isFindInstancesAtInitialization)
			{
				foreach (GPUIPrefab gpuiprefab2 in GPUIUtility.FindObjectsByType<GPUIPrefab>())
				{
					if (!gpuiprefab2.IsInstanced)
					{
						int prefabID2 = gpuiprefab2.GetPrefabID();
						List<GPUIPrefab> list2;
						if (prefabID2 != 0 && dictionary.TryGetValue(prefabID2, out list2) && !gpuiprefab2._isBeingAddedToThePrefabManager)
						{
							list2.Add(gpuiprefab2);
							gpuiprefab2._isBeingAddedToThePrefabManager = true;
						}
					}
				}
			}
			for (int k = 0; k < this._prototypes.Length; k++)
			{
				GPUIPrototype gpuiprototype = this._prototypes[k];
				GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[k];
				if (gpuiprototype.isEnabled && gpuiprefabPrototypeData.IsInitialized)
				{
					if (gpuiprototype.prefabObject == null)
					{
						string log_PREFIX = GPUIConstants.LOG_PREFIX;
						string text = "Prefab object reference is not set on the prototype: ";
						GPUIPrototype gpuiprototype2 = gpuiprototype;
						Debug.LogError(log_PREFIX + text + ((gpuiprototype2 != null) ? gpuiprototype2.ToString() : null), this);
					}
					else
					{
						List<GPUIPrefab> list3 = dictionary[this.GetPrefabID(k)];
						if (list3.Count > 0)
						{
							this.AddPrefabInstances(list3, k);
						}
					}
				}
			}
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.ApplyTransformBufferChanges));
			GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
			instance2.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance2.OnPreCull, new Action<GPUICameraData>(GPUIMaterialVariationDataProvider.Instance.UpdateVariationBuffers));
			GPUIRenderingSystem instance3 = GPUIRenderingSystem.Instance;
			instance3.OnPreCull = (Action<GPUICameraData>)Delegate.Combine(instance3.OnPreCull, new Action<GPUICameraData>(this.ApplyTransformBufferChanges));
			GPUIRenderingSystem instance4 = GPUIRenderingSystem.Instance;
			instance4.OnPreCull = (Action<GPUICameraData>)Delegate.Combine(instance4.OnPreCull, new Action<GPUICameraData>(GPUIMaterialVariationDataProvider.Instance.UpdateVariationBuffers));
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003DAC File Offset: 0x00001FAC
		public override void Dispose()
		{
			base.Dispose();
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
				instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.ApplyTransformBufferChanges));
				GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
				instance2.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance2.OnPreCull, new Action<GPUICameraData>(GPUIMaterialVariationDataProvider.Instance.UpdateVariationBuffers));
			}
			if (this._jobHandles.IsCreated)
			{
				this._jobHandles.Dispose();
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003E2E File Offset: 0x0000202E
		private bool IsPrefabNullOrInstanced(GPUIPrefab p)
		{
			return p == null || p.IsInstanced;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003E44 File Offset: 0x00002044
		protected override void OnPrototypeEnabled(int prototypeIndex)
		{
			base.OnPrototypeEnabled(prototypeIndex);
			List<GPUIPrefab> list = new List<GPUIPrefab>();
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			if (!this.isFindInstancesAtInitialization && gpuiprefabPrototypeData.registeredInstances != null)
			{
				if (gpuiprefabPrototypeData.registeredInstances.prefabInstances != null)
				{
					foreach (GameObject gameObject in gpuiprefabPrototypeData.registeredInstances.prefabInstances)
					{
						GPUIPrefab gpuiprefab;
						if (gameObject != null && gameObject.TryGetComponent<GPUIPrefab>(out gpuiprefab) && !gpuiprefab.IsInstanced)
						{
							list.Add(gpuiprefab);
							gpuiprefab._isBeingAddedToThePrefabManager = true;
						}
					}
				}
				if (gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances != null)
				{
					foreach (GPUIPrefab gpuiprefab2 in gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances)
					{
						if (!gpuiprefab2.IsInstanced && gpuiprefab2._isBeingAddedToThePrefabManager)
						{
							list.Add(gpuiprefab2);
						}
					}
					gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances.Clear();
				}
			}
			else
			{
				GPUIPrefab[] array = GPUIUtility.FindObjectsByType<GPUIPrefab>();
				int prefabID = this.GetPrefabID(prototypeIndex);
				foreach (GPUIPrefab gpuiprefab3 in array)
				{
					if (!gpuiprefab3.IsInstanced && prefabID == gpuiprefab3.GetPrefabID())
					{
						list.Add(gpuiprefab3);
						gpuiprefab3._isBeingAddedToThePrefabManager = true;
					}
				}
			}
			if (list.Count > 0)
			{
				this.AddPrefabInstances(list, prototypeIndex);
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003FB4 File Offset: 0x000021B4
		protected override void DisposeRenderer(int prototypeIndex)
		{
			if (this._prototypeDataArray == null)
			{
				return;
			}
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			if (gpuiprefabPrototypeData == null || !gpuiprefabPrototypeData.IsInitialized)
			{
				return;
			}
			if (this._prototypes[prototypeIndex].prefabObject != null && gpuiprefabPrototypeData.instanceTransforms != null)
			{
				GPUIPrefabPrototypeData gpuiprefabPrototypeData2 = gpuiprefabPrototypeData;
				if (gpuiprefabPrototypeData2.registeredInstances == null)
				{
					gpuiprefabPrototypeData2.registeredInstances = new GPUIPrefabPrototypeData.GPUIPrefabInstances();
				}
				if (!this.isFindInstancesAtInitialization)
				{
					if (gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances == null)
					{
						gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances = new List<GPUIPrefab>();
					}
					else
					{
						gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances.Clear();
					}
				}
				for (int i = 0; i < gpuiprefabPrototypeData.instanceTransforms.Length; i++)
				{
					Transform transform = gpuiprefabPrototypeData.instanceTransforms[i];
					GPUIPrefab gpuiprefab;
					if (transform != null && transform.TryGetComponent<GPUIPrefab>(out gpuiprefab))
					{
						this.ClearInstancingData(gpuiprefab, this.isEnableDefaultRenderingWhenDisabled);
						if (!this.isFindInstancesAtInitialization)
						{
							gpuiprefab._isBeingAddedToThePrefabManager = true;
							gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances.Add(gpuiprefab);
						}
					}
				}
				if (!this.isFindInstancesAtInitialization)
				{
					foreach (GPUIPrefab gpuiprefab2 in gpuiprefabPrototypeData.instancesToAdd)
					{
						if (!(gpuiprefab2 == null))
						{
							gpuiprefab2._isBeingAddedToThePrefabManager = true;
							gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances.Add(gpuiprefab2);
						}
					}
				}
			}
			base.DisposeRenderer(prototypeIndex);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004124 File Offset: 0x00002324
		protected override void ClearNullPrototypes()
		{
			base.ClearNullPrototypes();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000412C File Offset: 0x0000232C
		public override void RemovePrototypeAtIndex(int index)
		{
			base.RemovePrototypeAtIndex(index);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004135 File Offset: 0x00002335
		public override bool CanAddObjectAsPrototype(Object obj)
		{
			return base.CanAddObjectAsPrototype(obj);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004144 File Offset: 0x00002344
		private void StartAutoUpdateTransformJobs()
		{
			if (!this._jobHandles.IsCreated)
			{
				this._jobHandles = new NativeArray<JobHandle>(this._prototypes.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
			else if (this._jobHandles.Length != this._prototypes.Length)
			{
				this._jobHandles.Dispose();
				this._jobHandles = new NativeArray<JobHandle>(this._prototypes.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
			for (int i = 0; i < this._prototypeDataArray.Length; i++)
			{
				GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[i];
				if (gpuiprefabPrototypeData.IsInitialized && (this._requireTransformUpdate || gpuiprefabPrototypeData.isAutoUpdateTransformData) && gpuiprefabPrototypeData.HasMatrixArray() && gpuiprefabPrototypeData.instanceTransforms != null && gpuiprefabPrototypeData.instanceCount > 0)
				{
					gpuiprefabPrototypeData.UpdateTransformAccessArray();
					gpuiprefabPrototypeData.autoUpdateTransformsJob.instanceCount = gpuiprefabPrototypeData.instanceCount;
					gpuiprefabPrototypeData.autoUpdateTransformsJob.zeroMatrix = GPUIPrefabManager.ZERO_MATRIX;
					gpuiprefabPrototypeData.autoUpdateTransformsJob.p_matrixArray = gpuiprefabPrototypeData.GetMatrixArrayUnsafePtr();
					gpuiprefabPrototypeData.autoUpdateTransformsJob.p_isModifiedArray = gpuiprefabPrototypeData.isModifiedArray.GetUnsafePtr<int>();
					this._jobHandles[i] = gpuiprefabPrototypeData.autoUpdateTransformsJob.ScheduleReadOnly(gpuiprefabPrototypeData.transformAccessArray, 32, default(JobHandle));
					gpuiprefabPrototypeData.isMatrixArrayModified = true;
					if (gpuiprefabPrototypeData._isAutoUpdateTransformJobsStarted)
					{
						gpuiprefabPrototypeData.SetAllMatricesModified();
					}
					else
					{
						gpuiprefabPrototypeData._isAutoUpdateTransformJobsStarted = true;
					}
				}
			}
			this._requireTransformUpdate = false;
			this._dependentJob = JobHandle.CombineDependencies(this._jobHandles);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000042B8 File Offset: 0x000024B8
		private unsafe void ApplyTransformBufferChanges()
		{
			this._dependentJob.Complete();
			for (int i = 0; i < this._prototypeDataArray.Length; i++)
			{
				GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[i];
				if (gpuiprefabPrototypeData.IsInitialized)
				{
					if (gpuiprefabPrototypeData.isMatrixArrayModified && gpuiprefabPrototypeData.HasMatrixArray())
					{
						int num = gpuiprefabPrototypeData.minModifiedIndex;
						int num2 = gpuiprefabPrototypeData.maxModifiedIndex;
						bool flag = num == 0 && num2 >= gpuiprefabPrototypeData.instanceCount - 1;
						if (gpuiprefabPrototypeData._isAutoUpdateTransformJobsStarted && gpuiprefabPrototypeData.instanceCount > 0 && !flag)
						{
							void* unsafePtr = gpuiprefabPrototypeData.isModifiedArray.GetUnsafePtr<int>();
							for (int j = 0; j < gpuiprefabPrototypeData.instanceCount; j++)
							{
								if (UnsafeUtility.ReadArrayElementWithStride<int>(unsafePtr, j, 4) != 0)
								{
									if (num > j)
									{
										num = j;
									}
									if (num2 < j)
									{
										num2 = j;
									}
								}
							}
						}
						num = Mathf.Max(0, num);
						num2 = Mathf.Min(gpuiprefabPrototypeData.instanceCount - 1, num2);
						flag = num == 0 && num2 == gpuiprefabPrototypeData.instanceCount - 1;
						int num3 = this._runtimeRenderKeys[i];
						GPUIRenderingSystem.SetBufferSize(num3, gpuiprefabPrototypeData.GetMatrixLength(), !flag || num > num2);
						if (num <= num2)
						{
							GPUIRenderingSystem.SetTransformBufferData<Matrix4x4>(num3, gpuiprefabPrototypeData.GetTransformationMatrixArray(), num, num, num2 - num + 1, false);
						}
						GPUIRenderingSystem.SetInstanceCount(num3, gpuiprefabPrototypeData.instanceCount);
						gpuiprefabPrototypeData.isMatrixArrayModified = false;
						gpuiprefabPrototypeData.minModifiedIndex = int.MaxValue;
						gpuiprefabPrototypeData.maxModifiedIndex = -1;
					}
					gpuiprefabPrototypeData._isAutoUpdateTransformJobsStarted = false;
					if (gpuiprefabPrototypeData.hasOptionalRenderers && gpuiprefabPrototypeData.isOptionalRendererStatusModified && gpuiprefabPrototypeData.optionalRendererStatusData.IsCreated)
					{
						GPUIRenderingSystem.Instance.SetOptionalRendererStatusData(this._runtimeRenderKeys[i], gpuiprefabPrototypeData.optionalRendererStatusData);
						gpuiprefabPrototypeData.isOptionalRendererStatusModified = false;
					}
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000445B File Offset: 0x0000265B
		private void ApplyTransformBufferChanges(GPUICameraData cameraData)
		{
			this.ApplyTransformBufferChanges();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004464 File Offset: 0x00002664
		private void AutoEnableDisablePrototype()
		{
			if (this.isFindInstancesAtInitialization)
			{
				return;
			}
			int frameCount = Time.frameCount;
			for (int i = 0; i < this._prototypeDataArray.Length; i++)
			{
				GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[i];
				if (gpuiprefabPrototypeData.isAutoEnableDisablePrototype && frameCount - gpuiprefabPrototypeData._lastEnabledStatusChangeFrame >= 3)
				{
					GPUIPrototype gpuiprototype = this._prototypes[i];
					int num = (gpuiprototype.isEnabled ? gpuiprefabPrototypeData.GetInstanceCount() : this.GetRuntimeCachedInstanceCount(i));
					if (num >= gpuiprefabPrototypeData.autoEnableDisableMinMaxCounts.y && !gpuiprototype.isEnabled)
					{
						this.OnPrototypeEnabledStatusChanged(i, true);
						gpuiprefabPrototypeData._lastEnabledStatusChangeFrame = frameCount;
					}
					else if (num < gpuiprefabPrototypeData.autoEnableDisableMinMaxCounts.x && gpuiprototype.isEnabled)
					{
						this.OnPrototypeEnabledStatusChanged(i, false);
						gpuiprefabPrototypeData._lastEnabledStatusChangeFrame = frameCount;
					}
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004528 File Offset: 0x00002728
		private void AddRemoveInstances()
		{
			this._dependentJob.Complete();
			int num = 0;
			if (GPUIPrefabManager._instancesToAdd != null && GPUIPrefabManager._instancesToAdd.Count > 0)
			{
				GPUIPrefabManager._instancesToAdd.RemoveAll(this._isNullOrInstancedPredicate);
				num = GPUIPrefabManager._instancesToAdd.Count;
			}
			for (int i = 0; i < this._prototypeDataArray.Length; i++)
			{
				GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[i];
				if (!gpuiprefabPrototypeData.IsInitialized)
				{
					if (!this.isFindInstancesAtInitialization && num > 0)
					{
						GPUIPrefabPrototypeData gpuiprefabPrototypeData2 = gpuiprefabPrototypeData;
						if (gpuiprefabPrototypeData2.registeredInstances == null)
						{
							gpuiprefabPrototypeData2.registeredInstances = new GPUIPrefabPrototypeData.GPUIPrefabInstances();
						}
						for (int j = num - 1; j >= 0; j--)
						{
							GPUIPrefab gpuiprefab = GPUIPrefabManager._instancesToAdd[j];
							if (gpuiprefab._isBeingAddedToThePrefabManager && gpuiprefab.GetPrefabID() == gpuiprefabPrototypeData.prefabID)
							{
								gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances.Add(gpuiprefab);
								GPUIPrefabManager._instancesToAdd.RemoveAtSwapBack<GPUIPrefab>(j);
								num--;
							}
						}
					}
				}
				else
				{
					gpuiprefabPrototypeData.instancesToAdd.RemoveAll(this._isNullOrInstancedPredicate);
					if (num > 0)
					{
						for (int k = num - 1; k >= 0; k--)
						{
							GPUIPrefab gpuiprefab2 = GPUIPrefabManager._instancesToAdd[k];
							if (gpuiprefab2.GetPrefabID() == gpuiprefabPrototypeData.prefabID)
							{
								gpuiprefabPrototypeData.instancesToAdd.Add(gpuiprefab2);
								GPUIPrefabManager._instancesToAdd.RemoveAtSwapBack<GPUIPrefab>(k);
								num--;
							}
						}
					}
					int count = gpuiprefabPrototypeData.indexesToRemove.Count;
					int count2 = gpuiprefabPrototypeData.instancesToAdd.Count;
					int num2 = gpuiprefabPrototypeData.instanceCount - 1;
					if (count > 0)
					{
						if (num2 >= 0 && gpuiprefabPrototypeData.instanceTransforms != null)
						{
							int num3 = gpuiprefabPrototypeData.instanceTransforms.Length;
							foreach (int num4 in gpuiprefabPrototypeData.indexesToRemove)
							{
								if (num4 >= 0 && num4 <= num2 && num4 < num3)
								{
									Transform transform = gpuiprefabPrototypeData.instanceTransforms[num4];
									GPUIPrefab gpuiprefab3;
									if (transform != null && transform.TryGetComponent<GPUIPrefab>(out gpuiprefab3))
									{
										this.ClearInstancingData(gpuiprefab3, false);
									}
									if (num2 != num4)
									{
										gpuiprefabPrototypeData.SetMatrix(num4, gpuiprefabPrototypeData.GetMatrix(num2));
										Transform transform2 = gpuiprefabPrototypeData.instanceTransforms[num2];
										if (transform2)
										{
											GPUIPrefab component = transform2.GetComponent<GPUIPrefab>();
											gpuiprefabPrototypeData.instanceTransforms[num4] = transform2;
											component.SetBufferIndex(num4);
											if (gpuiprefabPrototypeData.hasOptionalRenderers)
											{
												component.OnOptionalRendererStatusChanged();
												if (gpuiprefabPrototypeData._optionalRendererCount > 32)
												{
													component.OnOptionalRendererStatusExtraChanged();
												}
											}
										}
									}
									gpuiprefabPrototypeData.SetMatrix(num2, GPUIPrefabManager.ZERO_MATRIX);
									num2--;
								}
							}
						}
						gpuiprefabPrototypeData.indexesToRemove.Clear();
						gpuiprefabPrototypeData.instanceCount = num2 + 1;
						gpuiprefabPrototypeData.isMatrixArrayModified = true;
						gpuiprefabPrototypeData.isTransformReferencesModified = true;
						gpuiprefabPrototypeData.isOptionalRendererStatusModified = true;
						if (count2 == 0 && gpuiprefabPrototypeData.instanceCount < gpuiprefabPrototypeData.GetMatrixLength() - 256)
						{
							this.ResizeArrays(i, (gpuiprefabPrototypeData.instanceCount / 128 + 1) * 128);
						}
					}
					if (count2 > 0)
					{
						int num5 = gpuiprefabPrototypeData.instanceCount + count2;
						if (num5 > gpuiprefabPrototypeData.GetMatrixLength())
						{
							this.ResizeArrays(i, (num5 / 128 + 1) * 128);
						}
						for (int l = 0; l < count2; l++)
						{
							GPUIPrefab gpuiprefab4 = gpuiprefabPrototypeData.instancesToAdd[l];
							if (!gpuiprefab4.IsInstanced && gpuiprefab4.enabled)
							{
								num2++;
								this.SetupPrefabInstanceForInstancing(gpuiprefab4, i, num2);
							}
						}
						gpuiprefabPrototypeData.instancesToAdd.Clear();
						gpuiprefabPrototypeData.instanceCount = num2 + 1;
						gpuiprefabPrototypeData.isMatrixArrayModified = true;
						gpuiprefabPrototypeData.isTransformReferencesModified = true;
						gpuiprefabPrototypeData.isOptionalRendererStatusModified = true;
					}
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000048CC File Offset: 0x00002ACC
		public void UpdateTransformData()
		{
			for (int i = 0; i < this._prototypeDataArray.Length; i++)
			{
				GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[i];
				if (gpuiprefabPrototypeData.IsInitialized)
				{
					gpuiprefabPrototypeData.SetAllMatricesModified();
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004904 File Offset: 0x00002B04
		public void UpdateTransformData(int prototypeIndex)
		{
			if (prototypeIndex < 0 || prototypeIndex >= this._prototypeDataArray.Length)
			{
				Debug.Log(string.Concat(new string[]
				{
					GPUIConstants.LOG_PREFIX,
					"Invalid prototype index: ",
					prototypeIndex.ToString(),
					". Current number of prototypes: ",
					this._prototypeDataArray.Length.ToString()
				}), this);
				return;
			}
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			if (!gpuiprefabPrototypeData.IsInitialized)
			{
				return;
			}
			gpuiprefabPrototypeData.SetAllMatricesModified();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000497F File Offset: 0x00002B7F
		protected virtual void ClearInstancingData(GPUIPrefab gpuiPrefab, bool enableRenderers)
		{
			gpuiPrefab.ClearInstancingData(enableRenderers);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004988 File Offset: 0x00002B88
		public int GetPrototypeIndex(GPUIPrefab gpuiPrefab)
		{
			return this.GetPrototypeIndexWithPrefabID(gpuiPrefab.GetPrefabID());
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004998 File Offset: 0x00002B98
		public int GetPrototypeIndexWithPrefabID(int prefabID)
		{
			if (prefabID != 0)
			{
				if (base.IsInitialized)
				{
					for (int i = 0; i < this._prototypes.Length; i++)
					{
						if (this._prototypeDataArray[i].prefabID == prefabID)
						{
							return i;
						}
					}
				}
				else
				{
					for (int j = 0; j < this._prototypes.Length; j++)
					{
						if (this._prototypes[j].prefabObject.GetComponent<GPUIPrefab>().GetPrefabID() == prefabID)
						{
							return j;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004A08 File Offset: 0x00002C08
		public bool TryGetPrefabPrototypeDataWithPrefabID(int prefabID, out GPUIPrefabPrototypeData prototypeData)
		{
			prototypeData = null;
			if (prefabID == 0)
			{
				return false;
			}
			for (int i = 0; i < this._prototypeDataArray.Length; i++)
			{
				if (this._prototypeDataArray[i].prefabID == prefabID)
				{
					prototypeData = this._prototypeDataArray[i];
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004A50 File Offset: 0x00002C50
		public static void AddPrefabInstance(GPUIPrefab gpuiPrefab)
		{
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				return;
			}
			if (gpuiPrefab == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not add prefab instance! Given prefab is null.");
				return;
			}
			if (gpuiPrefab.IsInstanced || gpuiPrefab._isBeingAddedToThePrefabManager)
			{
				return;
			}
			if (gpuiPrefab.GetPrefabID() == 0)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not add prefab instance. Unknown prefab ID! Make sure the prefab ID is not overridden or use the AddPrefabInstance(GPUIPrefab gpuiPrefab, int prototypeIndex) method with the prototypeIndex parameter.", gpuiPrefab.gameObject);
				return;
			}
			if (GPUIPrefabManager._instancesToAdd == null)
			{
				GPUIPrefabManager._instancesToAdd = new List<GPUIPrefab>();
			}
			GPUIPrefabManager._instancesToAdd.Add(gpuiPrefab);
			gpuiPrefab._isBeingAddedToThePrefabManager = true;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004ADC File Offset: 0x00002CDC
		public static void AddPrefabInstances(IEnumerable<GPUIPrefab> gpuiPrefabs)
		{
			if (gpuiPrefabs == null)
			{
				return;
			}
			if (GPUIPrefabManager._instancesToAdd == null)
			{
				GPUIPrefabManager._instancesToAdd = new List<GPUIPrefab>();
			}
			foreach (GPUIPrefab gpuiprefab in gpuiPrefabs)
			{
				gpuiprefab._isBeingAddedToThePrefabManager = true;
			}
			GPUIPrefabManager._instancesToAdd.AddRange(gpuiPrefabs);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004B44 File Offset: 0x00002D44
		public bool AddPrefabInstances(IEnumerable<GPUIPrefab> instances, int prototypeIndex)
		{
			if (prototypeIndex < 0 || prototypeIndex > this._prototypeDataArray.Length)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Invalid prototype index: " + prototypeIndex.ToString(), base.gameObject);
				return false;
			}
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			if (gpuiprefabPrototypeData == null || !gpuiprefabPrototypeData.IsInitialized)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find runtime data at index: " + prototypeIndex.ToString(), base.gameObject);
				return false;
			}
			foreach (GPUIPrefab gpuiprefab in instances)
			{
				gpuiprefab._isBeingAddedToThePrefabManager = true;
			}
			gpuiprefabPrototypeData.instancesToAdd.AddRange(instances);
			return true;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004C00 File Offset: 0x00002E00
		public bool AddPrefabInstance(GameObject go, int prototypeIndex)
		{
			if (prototypeIndex < 0 || prototypeIndex > this._prototypeDataArray.Length)
			{
				Debug.LogError(string.Concat(new string[]
				{
					GPUIConstants.LOG_PREFIX,
					"Can not find prototype at index ",
					prototypeIndex.ToString(),
					" for: ",
					(go != null) ? go.ToString() : null
				}), go);
				return false;
			}
			return this.AddPrefabInstance(go.AddOrGetComponent<GPUIPrefab>(false), prototypeIndex);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004C70 File Offset: 0x00002E70
		public void AddPrefabInstances(IEnumerable<GameObject> instances, int prototypeIndex)
		{
			if (prototypeIndex < 0 || prototypeIndex > this._prototypeDataArray.Length)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find prototype at index " + prototypeIndex.ToString() + ".");
				return;
			}
			foreach (GameObject gameObject in instances)
			{
				this.AddPrefabInstance(gameObject.AddOrGetComponent<GPUIPrefab>(false), prototypeIndex);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004CF0 File Offset: 0x00002EF0
		public bool AddPrefabInstance(GPUIPrefab gpuiPrefab, int prototypeIndex = -1)
		{
			if (gpuiPrefab == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not add prefab instance! Given prefab is null.");
				return false;
			}
			if (gpuiPrefab.IsInstanced || gpuiPrefab._isBeingAddedToThePrefabManager)
			{
				return true;
			}
			if (prototypeIndex < 0 || prototypeIndex > this._prototypeDataArray.Length)
			{
				prototypeIndex = this.GetPrototypeIndex(gpuiPrefab);
				if (prototypeIndex < 0)
				{
					Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find prototype for: " + ((gpuiPrefab != null) ? gpuiPrefab.ToString() : null), gpuiPrefab);
					return false;
				}
			}
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			if (gpuiprefabPrototypeData == null || !gpuiprefabPrototypeData.IsInitialized)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find runtime data for: " + ((gpuiPrefab != null) ? gpuiPrefab.ToString() : null), gpuiPrefab);
				return false;
			}
			gpuiprefabPrototypeData.instancesToAdd.Add(gpuiPrefab);
			gpuiPrefab._isBeingAddedToThePrefabManager = true;
			return true;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004DBC File Offset: 0x00002FBC
		public int AddPrefabInstanceImmediate(GPUIPrefab gpuiPrefab, int prototypeIndex = -1)
		{
			if (gpuiPrefab == null)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not add prefab instance! Given prefab is null.");
				return -1;
			}
			if (gpuiPrefab.IsInstanced)
			{
				return gpuiPrefab.bufferIndex;
			}
			if (prototypeIndex < 0)
			{
				prototypeIndex = this.GetPrototypeIndex(gpuiPrefab);
				if (prototypeIndex < 0)
				{
					Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find prototype for: " + ((gpuiPrefab != null) ? gpuiPrefab.ToString() : null), gpuiPrefab);
					return -1;
				}
			}
			this._dependentJob.Complete();
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			int instanceCount = gpuiprefabPrototypeData.instanceCount;
			this.ResizeArrays(prototypeIndex, instanceCount + 1);
			this.SetupPrefabInstanceForInstancing(gpuiPrefab, prototypeIndex, instanceCount);
			gpuiprefabPrototypeData.isTransformReferencesModified = true;
			gpuiprefabPrototypeData.isOptionalRendererStatusModified = true;
			gpuiprefabPrototypeData.isMatrixArrayModified = true;
			gpuiprefabPrototypeData.instanceCount++;
			return instanceCount;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004E7C File Offset: 0x0000307C
		public bool RemovePrefabInstance(GPUIPrefab gpuiPrefab, int prototypeIndex = -1)
		{
			GPUIPrefabPrototypeData gpuiprefabPrototypeData;
			if (!gpuiPrefab.IsInstanced)
			{
				if (gpuiPrefab._isBeingAddedToThePrefabManager)
				{
					gpuiPrefab._isBeingAddedToThePrefabManager = false;
					if (prototypeIndex < 0)
					{
						prototypeIndex = this.GetPrototypeIndex(gpuiPrefab);
					}
					gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
					int num = gpuiprefabPrototypeData.instancesToAdd.IndexOf(gpuiPrefab);
					if (num >= 0)
					{
						gpuiprefabPrototypeData.instancesToAdd.RemoveAt(num);
						return true;
					}
					if (GPUIPrefabManager._instancesToAdd != null)
					{
						num = GPUIPrefabManager._instancesToAdd.IndexOf(gpuiPrefab);
						if (num >= 0)
						{
							GPUIPrefabManager._instancesToAdd.RemoveAt(num);
							return true;
						}
					}
				}
				return true;
			}
			if (prototypeIndex < 0)
			{
				prototypeIndex = this.GetPrototypeIndex(gpuiPrefab);
				if (prototypeIndex < 0)
				{
					Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find prototype for: " + ((gpuiPrefab != null) ? gpuiPrefab.ToString() : null), gpuiPrefab);
					return false;
				}
			}
			if (this._prototypeDataArray == null || this._prototypeDataArray.Length <= prototypeIndex)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find runtime data at index: " + prototypeIndex.ToString(), gpuiPrefab);
				return false;
			}
			gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			int bufferIndex = gpuiPrefab.bufferIndex;
			if (bufferIndex < 0 || bufferIndex >= gpuiprefabPrototypeData.instanceCount)
			{
				gpuiPrefab.ClearInstancingData(false);
				return true;
			}
			gpuiprefabPrototypeData.indexesToRemove.Add(bufferIndex);
			return true;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004F98 File Offset: 0x00003198
		public void UpdateTransformData(GPUIPrefab gpuiPrefab)
		{
			int num;
			if (gpuiPrefab.IsInstanced)
			{
				num = base.GetPrototypeIndex(gpuiPrefab.renderKey);
			}
			else
			{
				num = this.GetPrototypeIndex(gpuiPrefab);
			}
			if (num < 0)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find prototype for: " + ((gpuiPrefab != null) ? gpuiPrefab.ToString() : null), gpuiPrefab);
				return;
			}
			Transform cachedTransform = gpuiPrefab.CachedTransform;
			if (this.UpdateTransformData(num, gpuiPrefab.bufferIndex, cachedTransform.localToWorldMatrix))
			{
				cachedTransform.hasChanged = false;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005010 File Offset: 0x00003210
		public bool UpdateTransformData(int prototypeIndex, int bufferIndex, Matrix4x4 matrix)
		{
			if (this._prototypeDataArray.Length <= prototypeIndex)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not find runtime data at index: " + prototypeIndex.ToString());
				return false;
			}
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			if (gpuiprefabPrototypeData.isAutoUpdateTransformData)
			{
				return true;
			}
			this._dependentJob.Complete();
			if (gpuiprefabPrototypeData.HasMatrixArray() && gpuiprefabPrototypeData.GetMatrixLength() > bufferIndex)
			{
				gpuiprefabPrototypeData.isMatrixArrayModified = true;
				gpuiprefabPrototypeData.SetMatrix(bufferIndex, matrix);
				return true;
			}
			return false;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00005088 File Offset: 0x00003288
		public virtual void SetupPrefabInstanceForInstancing(GPUIPrefab gpuiPrefab, int prototypeIndex, int bufferIndex)
		{
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			Transform cachedTransform = gpuiPrefab.CachedTransform;
			if (!gpuiprefabPrototypeData.isAutoUpdateTransformData)
			{
				gpuiprefabPrototypeData.SetMatrix(bufferIndex, cachedTransform.localToWorldMatrix);
			}
			gpuiprefabPrototypeData.instanceTransforms[bufferIndex] = cachedTransform;
			cachedTransform.hasChanged = false;
			gpuiPrefab.SetInstancingData(this, gpuiprefabPrototypeData.prefabID, this._runtimeRenderKeys[prototypeIndex], bufferIndex);
			if (gpuiprefabPrototypeData.hasOptionalRenderers)
			{
				gpuiPrefab.OnOptionalRendererStatusChanged();
				if (gpuiprefabPrototypeData._optionalRendererCount > 32)
				{
					gpuiPrefab.OnOptionalRendererStatusExtraChanged();
				}
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005100 File Offset: 0x00003300
		protected virtual void ResizeArrays(int prototypeIndex, int newSize)
		{
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			gpuiprefabPrototypeData.ResizeMatrixArray(newSize);
			gpuiprefabPrototypeData.isMatrixArrayModified = true;
			if (gpuiprefabPrototypeData.instanceTransforms == null)
			{
				gpuiprefabPrototypeData.instanceTransforms = new Transform[newSize];
			}
			else
			{
				Array.Resize<Transform>(ref gpuiprefabPrototypeData.instanceTransforms, newSize);
			}
			gpuiprefabPrototypeData.isTransformReferencesModified = true;
			if (gpuiprefabPrototypeData.hasOptionalRenderers)
			{
				(ref gpuiprefabPrototypeData.optionalRendererStatusData).ResizeNativeArray<uint>(newSize * ((gpuiprefabPrototypeData._optionalRendererCount - 1) / 32 + 1), Allocator.Persistent);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00005170 File Offset: 0x00003370
		public int GetPrefabID(int prototypeIndex)
		{
			return GPUIPrefabManager.GetPrefabID(this._prototypes[prototypeIndex]);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005180 File Offset: 0x00003380
		internal static int GetPrefabID(GPUIPrototype prototype)
		{
			GPUIPrefab gpuiprefab;
			if (prototype.prefabObject.TryGetComponent<GPUIPrefab>(out gpuiprefab))
			{
				return gpuiprefab.GetPrefabID();
			}
			return prototype.GetKey();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000051AC File Offset: 0x000033AC
		public override int GetRegisteredInstanceCount(int prototypeIndex)
		{
			if (!Application.isPlaying && !this.isFindInstancesAtInitialization && this._prototypeDataArray != null && this._prototypeDataArray.Length > prototypeIndex && this._prototypeDataArray[prototypeIndex] != null && this._prototypeDataArray[prototypeIndex].registeredInstances != null && this._prototypeDataArray[prototypeIndex].registeredInstances.prefabInstances != null)
			{
				return this._prototypeDataArray[prototypeIndex].registeredInstances.prefabInstances.Length;
			}
			return base.GetRegisteredInstanceCount(prototypeIndex);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005224 File Offset: 0x00003424
		public virtual void SetPrefabInstanceRenderersEnabled(GPUIPrefab prefabInstance, bool enabled)
		{
			prefabInstance.SetRenderersEnabled(enabled);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000522D File Offset: 0x0000342D
		protected override void OnUpdatePerInstanceLightProbes(int prototypeIndex)
		{
			this._prototypeDataArray[prototypeIndex].SetAllMatricesModified();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000523C File Offset: 0x0000343C
		public override List<string> GetShaderKeywords(int prototypeIndex)
		{
			if (this._prototypes[prototypeIndex].prefabObject.HasComponent<GPUIMaterialVariationInstance>())
			{
				return GPUIPrefabManager.MATERIAL_VARIATION_SHADER_KEYWORDS;
			}
			return base.GetShaderKeywords(prototypeIndex);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000525F File Offset: 0x0000345F
		public int GetPrefabID(GameObject prefabObject)
		{
			return prefabObject.GetComponent<GPUIPrefab>().GetPrefabID();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000526C File Offset: 0x0000346C
		public NativeArray<Matrix4x4> GetTransformMatrix(int prefabID)
		{
			this._dependentJob.Complete();
			int prototypeIndexWithPrefabID = this.GetPrototypeIndexWithPrefabID(prefabID);
			if (prototypeIndexWithPrefabID < 0)
			{
				return default(NativeArray<Matrix4x4>);
			}
			return base.GetPrototypeData(prototypeIndexWithPrefabID).GetTransformationMatrixArray();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000052A8 File Offset: 0x000034A8
		public void SetTransformMatrixModified(int prefabID)
		{
			int prototypeIndexWithPrefabID = this.GetPrototypeIndexWithPrefabID(prefabID);
			if (prototypeIndexWithPrefabID < 0)
			{
				return;
			}
			base.GetPrototypeData(prototypeIndexWithPrefabID).SetAllMatricesModified();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000052D0 File Offset: 0x000034D0
		public TransformAccessArray GetTransformAccessArray(int prefabID)
		{
			int prototypeIndexWithPrefabID = this.GetPrototypeIndexWithPrefabID(prefabID);
			if (prototypeIndexWithPrefabID < 0)
			{
				return default(TransformAccessArray);
			}
			GPUIPrefabPrototypeData prototypeData = base.GetPrototypeData(prototypeIndexWithPrefabID);
			prototypeData.UpdateTransformAccessArray();
			return prototypeData.transformAccessArray;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005308 File Offset: 0x00003508
		public Transform[] GetInstanceTransforms(int prefabID)
		{
			int prototypeIndexWithPrefabID = this.GetPrototypeIndexWithPrefabID(prefabID);
			if (prototypeIndexWithPrefabID < 0)
			{
				return null;
			}
			return base.GetPrototypeData(prototypeIndexWithPrefabID).instanceTransforms;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005330 File Offset: 0x00003530
		public Transform GetInstanceTransform(int prefabID, int bufferIndex)
		{
			int prototypeIndexWithPrefabID = this.GetPrototypeIndexWithPrefabID(prefabID);
			if (prototypeIndexWithPrefabID < 0)
			{
				return null;
			}
			GPUIPrefabPrototypeData prototypeData = base.GetPrototypeData(prototypeIndexWithPrefabID);
			if (prototypeData.instanceTransforms.Length <= bufferIndex)
			{
				return null;
			}
			return prototypeData.instanceTransforms[bufferIndex];
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005368 File Offset: 0x00003568
		public Transform GetInstanceTransformWithRenderKey(int renderKey, int bufferIndex)
		{
			int prototypeIndex = base.GetPrototypeIndex(renderKey);
			if (prototypeIndex < 0)
			{
				return null;
			}
			GPUIPrefabPrototypeData prototypeData = base.GetPrototypeData(prototypeIndex);
			if (prototypeData.instanceTransforms.Length <= bufferIndex)
			{
				return null;
			}
			return prototypeData.instanceTransforms[bufferIndex];
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000053A0 File Offset: 0x000035A0
		public int GetInstanceCount(int prefabID)
		{
			int prototypeIndexWithPrefabID = this.GetPrototypeIndexWithPrefabID(prefabID);
			if (prototypeIndexWithPrefabID < 0)
			{
				return 0;
			}
			return base.GetPrototypeData(prototypeIndexWithPrefabID).instanceCount;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000053C7 File Offset: 0x000035C7
		public void RequireTransformUpdate()
		{
			this._requireTransformUpdate = true;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000053D0 File Offset: 0x000035D0
		public int GetRuntimeCachedInstanceCount(int prototypeIndex)
		{
			if (this.isFindInstancesAtInitialization || prototypeIndex < 0 || this._prototypeDataArray.Length <= prototypeIndex)
			{
				return 0;
			}
			GPUIPrefabPrototypeData gpuiprefabPrototypeData = this._prototypeDataArray[prototypeIndex];
			if (gpuiprefabPrototypeData.registeredInstances == null || gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances == null)
			{
				return 0;
			}
			gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances.RemoveAll(this._isNullOrInstancedPredicate);
			return gpuiprefabPrototypeData.registeredInstances.runtimeCachedInstances.Count;
		}

		// Token: 0x04000069 RID: 105
		[SerializeField]
		public bool isFindInstancesAtInitialization = true;

		// Token: 0x0400006A RID: 106
		private const int BUFFER_SIZE_INCREMENT = 128;

		// Token: 0x0400006B RID: 107
		private const int TRANSFORM_UPDATE_JOB_BATCH_SIZE = 32;

		// Token: 0x0400006C RID: 108
		private static List<GPUIPrefab> _instancesToAdd;

		// Token: 0x0400006D RID: 109
		private Predicate<GPUIPrefab> _isNullOrInstancedPredicate;

		// Token: 0x0400006E RID: 110
		private NativeArray<JobHandle> _jobHandles;

		// Token: 0x0400006F RID: 111
		private List<int> _prefabIDCheckList;

		// Token: 0x04000070 RID: 112
		private static readonly List<string> MATERIAL_VARIATION_SHADER_KEYWORDS = new List<string> { GPUIPrefabConstants.Kw_GPUI_MATERIAL_VARIATION };

		// Token: 0x04000071 RID: 113
		private static readonly Matrix4x4 ZERO_MATRIX = Matrix4x4.zero;

		// Token: 0x04000072 RID: 114
		[NonSerialized]
		private bool _requireTransformUpdate;

		// Token: 0x04000073 RID: 115
		private const int _noFramesToWaitBetweenStatusChange = 3;
	}
}
