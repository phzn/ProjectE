using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x02000013 RID: 19
	[ExecuteInEditMode]
	[DefaultExecutionOrder(200)]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#The_Detail_Manager")]
	public class GPUIDetailManager : GPUITerrainManager<GPUIDetailPrototypeData>
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00006AB8 File Offset: 0x00004CB8
		protected override void Update()
		{
			base.Update();
			if (this._detailUpdateDataDict == null)
			{
				return;
			}
			foreach (GPUIDetailManager.GPUIDetailUpdateData gpuidetailUpdateData in this._detailUpdateDataDict.Values)
			{
				if (!gpuidetailUpdateData.IsDataRequested())
				{
					if (gpuidetailUpdateData.requireReadback)
					{
						gpuidetailUpdateData.requireReadback = false;
						gpuidetailUpdateData.AsyncDataRequest(this._processCounterDataCallback, false);
					}
					else if (gpuidetailUpdateData.processReadback)
					{
						gpuidetailUpdateData.processReadback = false;
						gpuidetailUpdateData.ProcessGPUReadback(this);
					}
				}
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006B58 File Offset: 0x00004D58
		public override bool IsValid(bool logError = true)
		{
			if (!base.IsValid(logError))
			{
				return false;
			}
			bool flag = false;
			int terrainCount = base.GetTerrainCount();
			for (int i = 0; i < terrainCount; i++)
			{
				GPUITerrain terrain = base.GetTerrain(i);
				if (terrain != null && terrain.DetailPrototypes != null && terrain.DetailPrototypes.Length != 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.errorCode = -402;
				return false;
			}
			return true;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006BBC File Offset: 0x00004DBC
		public override void Initialize()
		{
			base.Initialize();
			this._sizeAndIndexes = new int[4];
			this._detailUpdateDataDict = new Dictionary<int, GPUIDetailManager.GPUIDetailUpdateData>();
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.UpdateDetailMatrices));
			GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
			instance2.OnCommandBufferModified = (Action)Delegate.Remove(instance2.OnCommandBufferModified, new Action(this.RequireUpdate));
			GPUIRenderingSystem instance3 = GPUIRenderingSystem.Instance;
			instance3.OnPreCull = (Action<GPUICameraData>)Delegate.Combine(instance3.OnPreCull, new Action<GPUICameraData>(this.UpdateDetailMatrices));
			GPUIRenderingSystem instance4 = GPUIRenderingSystem.Instance;
			instance4.OnCommandBufferModified = (Action)Delegate.Combine(instance4.OnCommandBufferModified, new Action(this.RequireUpdate));
			this._processCounterDataCallback = new Action<GPUIDataBuffer<uint>>(this.ProcessCounterData);
			this.RequireUpdate();
			if (GPUITerrain._terrainsSearchingForDetailManager != null)
			{
				base.AddTerrains(GPUITerrain._terrainsSearchingForDetailManager);
				GPUITerrain._terrainsSearchingForDetailManager.Clear();
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006CB4 File Offset: 0x00004EB4
		protected override bool RegisterRenderer(int prototypeIndex)
		{
			if (base.RegisterRenderer(prototypeIndex))
			{
				GPUIDetailPrototypeData gpuidetailPrototypeData = this._prototypeDataArray[prototypeIndex];
				int num = Mathf.Max(gpuidetailPrototypeData.initialBufferSize, 1);
				GPUIRenderingSystem.SetBufferSize(this._runtimeRenderKeys[prototypeIndex], num, false);
				if (this.disableAsyncDetailDataRequest)
				{
					GPUIRenderingSystem.SetInstanceCount(this._runtimeRenderKeys[prototypeIndex], num);
				}
				gpuidetailPrototypeData._bounds = GPUIRenderingSystem.Instance.LODGroupDataProvider.GetOrCreateLODGroupData(this._prototypes[prototypeIndex]).bounds;
				if (this._detailUpdateDataDict != null)
				{
					foreach (GPUIDetailManager.GPUIDetailUpdateData gpuidetailUpdateData in this._detailUpdateDataDict.Values)
					{
						if (prototypeIndex >= gpuidetailUpdateData.Length)
						{
							gpuidetailUpdateData.Resize(prototypeIndex + 1);
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006D8C File Offset: 0x00004F8C
		public override void Dispose()
		{
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
				instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.UpdateDetailMatrices));
				GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
				instance2.OnCommandBufferModified = (Action)Delegate.Remove(instance2.OnCommandBufferModified, new Action(this.RequireUpdate));
			}
			base.Dispose();
			if (this._detailUpdateDataDict != null)
			{
				foreach (KeyValuePair<int, GPUIDetailManager.GPUIDetailUpdateData> keyValuePair in this._detailUpdateDataDict)
				{
					keyValuePair.Value.Dispose();
				}
			}
			this._detailUpdateDataDict = null;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006E50 File Offset: 0x00005050
		private void UpdateDetailMatrices(GPUICameraData cameraData)
		{
			if (!base.IsInitialized)
			{
				return;
			}
			int num = this._prototypes.Length;
			if (num == 0)
			{
				return;
			}
			base.CheckPendingTerrainTransformChanges();
			Dictionary<int, GPUITerrain>.ValueCollection activeTerrainValues = base.GetActiveTerrainValues();
			if (base.GetActiveTerrainCount() == 0 && this._runtimeRenderKeys != null)
			{
				for (int i = 0; i < this._runtimeRenderKeys.Length; i++)
				{
					int num2 = this._runtimeRenderKeys[i];
					if (num2 != 0)
					{
						int num3 = Mathf.Max(base.GetPrototypeData(i).initialBufferSize, 1);
						GPUIRenderingSystem.SetBufferSize(num2, num3, false);
						if (this.disableAsyncDetailDataRequest)
						{
							GPUIRenderingSystem.SetInstanceCount(num2, num3);
						}
					}
				}
			}
			ComputeShader cs_VegetationGenerator = GPUITerrainConstants.CS_VegetationGenerator;
			if (this._detailUpdateDataDict == null)
			{
				this._detailUpdateDataDict = new Dictionary<int, GPUIDetailManager.GPUIDetailUpdateData>();
			}
			Vector3 cameraPosition = cameraData.GetCameraPosition();
			int instanceIDGPUI = cameraData.ActiveCamera.GetInstanceIDGPUI();
			GPUIDetailManager.GPUIDetailUpdateData gpuidetailUpdateData;
			if (!this._detailUpdateDataDict.TryGetValue(instanceIDGPUI, out gpuidetailUpdateData))
			{
				gpuidetailUpdateData = new GPUIDetailManager.GPUIDetailUpdateData(cameraData, "GPUIDetailCounterBuffer", num, GraphicsBuffer.Target.Structured);
				this._detailUpdateDataDict[instanceIDGPUI] = gpuidetailUpdateData;
			}
			if (gpuidetailUpdateData.Length < num)
			{
				gpuidetailUpdateData.Resize(num);
			}
			if (!this._forceImmediateUpdate && gpuidetailUpdateData.IsDataRequested())
			{
				return;
			}
			if (this._requireUpdateFrame > gpuidetailUpdateData.lastUpdateFrame || this.detailUpdateDistance <= 0f || Vector3.Distance(gpuidetailUpdateData.position, cameraPosition) >= this.detailUpdateDistance)
			{
				if (this._forceImmediateUpdate)
				{
					this._forceImmediateUpdate = false;
					if (this.waitForAsyncDetailDataRequestCompletion || !Application.isPlaying)
					{
						gpuidetailUpdateData.WaitForReadbackCompletion();
						if (gpuidetailUpdateData.processReadback)
						{
							gpuidetailUpdateData.processReadback = false;
							gpuidetailUpdateData.ProcessGPUReadback(this);
						}
					}
				}
				if (!gpuidetailUpdateData.UpdateBufferData(false))
				{
					this._sizeAndIndexes[0] = num;
					cs_VegetationGenerator.SetBuffer(1, GPUITerrainConstants.PROP_detailCounterBuffer, gpuidetailUpdateData.Buffer);
					cs_VegetationGenerator.SetInts(GPUIConstants.PROP_sizeAndIndexes, this._sizeAndIndexes);
					cs_VegetationGenerator.DispatchX(1, num);
				}
				bool flag = true;
				for (int j = 0; j < num; j++)
				{
					if (this._prototypes[j].isEnabled)
					{
						GPUIRenderSourceGroup gpuirenderSourceGroup;
						GPUIShaderBuffer gpuishaderBuffer;
						if (GPUIRenderingSystem.TryGetRenderSourceGroup(this._runtimeRenderKeys[j], out gpuirenderSourceGroup) && cameraData.TryGetShaderBuffer(this._runtimeRenderKeys[j], out gpuishaderBuffer))
						{
							GraphicsBuffer buffer = gpuishaderBuffer.Buffer;
							if (buffer == null)
							{
								flag = false;
							}
							else
							{
								this._sizeAndIndexes[0] = buffer.count;
								this._sizeAndIndexes[1] = j;
								GPUIDetailPrototypeData gpuidetailPrototypeData = this._prototypeDataArray[j];
								foreach (GPUITerrain gpuiterrain in activeTerrainValues)
								{
									if (!(gpuiterrain == null) && gpuiterrain.isActiveAndEnabled)
									{
										if (!gpuiterrain.IsDetailDensityTexturesLoaded || this._reloadTerrainDetailTextures)
										{
											gpuiterrain.CreateDetailTextures();
										}
										gpuiterrain.GenerateVegetation(gpuidetailPrototypeData, buffer, gpuidetailUpdateData, cameraPosition, this.detailObjectDistance, this.healthyDryNoiseTexture, this._sizeAndIndexes, this.isCheckTerrainBounds);
									}
								}
								cs_VegetationGenerator.SetBuffer(2, GPUIConstants.PROP_gpuiTransformBuffer, buffer);
								cs_VegetationGenerator.SetBuffer(2, GPUITerrainConstants.PROP_detailCounterBuffer, gpuidetailUpdateData.Buffer);
								cs_VegetationGenerator.SetInts(GPUIConstants.PROP_sizeAndIndexes, this._sizeAndIndexes);
								cs_VegetationGenerator.DispatchX(2, buffer.count);
								gpuirenderSourceGroup.TransformBufferData.OnTransformDataModified();
							}
						}
						else
						{
							flag = false;
						}
					}
				}
				this._reloadTerrainDetailTextures = false;
				if (!this.disableAsyncDetailDataRequest)
				{
					if (gpuidetailUpdateData.IsDataRequested())
					{
						this._requireUpdateFrame = Time.frameCount + 1;
					}
					else
					{
						gpuidetailUpdateData.requireReadback = true;
					}
				}
				if (flag)
				{
					gpuidetailUpdateData.position = cameraPosition;
					gpuidetailUpdateData.lastUpdateFrame = Time.frameCount;
				}
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000071D0 File Offset: 0x000053D0
		private void ProcessCounterData(GPUIDataBuffer<uint> buffer)
		{
			GPUIDetailManager.GPUIDetailUpdateData gpuidetailUpdateData = buffer as GPUIDetailManager.GPUIDetailUpdateData;
			if (gpuidetailUpdateData != null)
			{
				gpuidetailUpdateData.processReadback = true;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000071F0 File Offset: 0x000053F0
		public void ExecuteProceduralDetails(GPUITerrain gpuiTerrain)
		{
			int prototypeCount = base.GetPrototypeCount();
			int[] terrainPrototypeIndexes = this.GetTerrainPrototypeIndexes(gpuiTerrain);
			if (terrainPrototypeIndexes == null)
			{
				return;
			}
			for (int i = 0; i < prototypeCount; i++)
			{
				GPUIDetailPrototypeData gpuidetailPrototypeData = this._prototypeDataArray[i];
				if (gpuidetailPrototypeData.proceduralDensityData != null)
				{
					for (int j = 0; j < terrainPrototypeIndexes.Length; j++)
					{
						if (terrainPrototypeIndexes[j] % 1000 == i)
						{
							gpuidetailPrototypeData.proceduralDensityData.Execute(gpuiTerrain, j);
						}
					}
				}
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00007260 File Offset: 0x00005460
		protected override bool AddMissingPrototypesFromTerrain(GPUITerrain gpuiTerrain)
		{
			bool flag = false;
			DetailPrototype[] detailPrototypes = gpuiTerrain.DetailPrototypes;
			int[] terrainPrototypeIndexes = this.GetTerrainPrototypeIndexes(gpuiTerrain);
			for (int i = 0; i < terrainPrototypeIndexes.Length; i++)
			{
				if (terrainPrototypeIndexes[i] < 0)
				{
					this.AddDetailPrototype(detailPrototypes[i]);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000072A0 File Offset: 0x000054A0
		protected override void BeginDeterminePrototypeIndexes()
		{
			int terrainCount = base.GetTerrainCount();
			int prototypeCount = base.GetPrototypeCount();
			for (int i = terrainCount - 1; i >= 0; i--)
			{
				GPUITerrain terrain = base.GetTerrain(i);
				if (!(terrain == null) && terrain.DetailPrototypeIndexes != null && terrain.DetailPrototypes != null)
				{
					for (int j = 0; j < prototypeCount; j++)
					{
						int firstTerrainDetailPrototypeIndex = terrain.GetFirstTerrainDetailPrototypeIndex(j);
						if (firstTerrainDetailPrototypeIndex >= 0 && terrain.DetailPrototypes.Length > firstTerrainDetailPrototypeIndex)
						{
							DetailPrototype detailPrototype = terrain.DetailPrototypes[firstTerrainDetailPrototypeIndex];
							if (this._prototypeDataArray[j].IsMatchingPrefabAndTexture(detailPrototype, this._prototypes[j], false))
							{
								this._prototypeDataArray[j].ReadFromDetailPrototypeData(terrain.DetailPrototypes[firstTerrainDetailPrototypeIndex], terrain.DetailPrototypeIndexes[firstTerrainDetailPrototypeIndex] / 1000, this, j);
							}
						}
					}
				}
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007364 File Offset: 0x00005564
		internal int DetermineDetailPrototypeIndex(DetailPrototype detailPrototype)
		{
			if (this._prototypes != null)
			{
				for (int i = 0; i < this._prototypes.Length; i++)
				{
					GPUIPrototype gpuiprototype = this._prototypes[i];
					GPUIDetailPrototypeData gpuidetailPrototypeData = this._prototypeDataArray[i];
					if (gpuidetailPrototypeData.IsMatchingPrefabAndTexture(detailPrototype, gpuiprototype, true))
					{
						int subSettingCount = gpuidetailPrototypeData.GetSubSettingCount();
						for (int j = 0; j < subSettingCount; j++)
						{
							if (gpuidetailPrototypeData.HasSameSettingsWith(detailPrototype, j))
							{
								return i + j * 1000;
							}
						}
						gpuidetailPrototypeData.ReadFromDetailPrototypeData(detailPrototype, subSettingCount, this, i);
						if (base.IsInitialized)
						{
							gpuidetailPrototypeData.SetParameterBufferData();
						}
						return i + subSettingCount * 1000;
					}
				}
			}
			if (this._isAutoAddPrototypesBasedOnTerrains)
			{
				this._isTerrainsModified = true;
			}
			return -1;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00007408 File Offset: 0x00005608
		protected override void SetGPUITerrainManager(GPUITerrain gpuiTerrain)
		{
			gpuiTerrain.SetDetailManager(this);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007411 File Offset: 0x00005611
		protected override void RemoveGPUITerrainManager(GPUITerrain gpuiTerrain)
		{
			if (gpuiTerrain.DetailManager == this)
			{
				gpuiTerrain.RemoveDetailManager();
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007427 File Offset: 0x00005627
		protected override void OnNewPrototypeDataCreated(int prototypeIndex)
		{
			base.OnNewPrototypeDataCreated(prototypeIndex);
			if (this._prototypeDataArray[prototypeIndex].detailTexture != null)
			{
				this._prototypes[prototypeIndex].name = this._prototypeDataArray[prototypeIndex].detailTexture.name;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007464 File Offset: 0x00005664
		public override void CheckPrototypeChanges()
		{
			base.CheckPrototypeChanges();
			for (int i = 0; i < base.GetPrototypeCount(); i++)
			{
				GPUIPrototype gpuiprototype = this._prototypes[i];
				if (gpuiprototype.prototypeType == GPUIPrototypeType.MeshAndMaterial)
				{
					if (gpuiprototype.prototypeMesh == null)
					{
						gpuiprototype.prototypeMesh = GPUITerrainConstants.DefaultDetailMesh;
					}
					if (gpuiprototype.prototypeMaterials == null || gpuiprototype.prototypeMaterials.Length == 0 || gpuiprototype.prototypeMaterials[0] == null)
					{
						gpuiprototype.prototypeMaterials = new Material[] { GPUITerrainConstants.DefaultDetailMaterial };
					}
					if (this._prototypeDataArray[i].mpbDescription == null)
					{
						this._prototypeDataArray[i].mpbDescription = GPUITerrainConstants.DefaultDetailMaterialDescription;
					}
				}
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007515 File Offset: 0x00005715
		protected override void DeterminePrototypeIndexes(GPUITerrain gpuiTerrain)
		{
			gpuiTerrain.DetermineDetailPrototypeIndexes(this);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000751E File Offset: 0x0000571E
		protected override int[] GetTerrainPrototypeIndexes(GPUITerrain gpuiTerrain)
		{
			if (gpuiTerrain.DetailPrototypes == null)
			{
				gpuiTerrain.LoadTerrain();
			}
			if (gpuiTerrain.DetailPrototypeIndexes == null || (gpuiTerrain.DetailPrototypes != null && gpuiTerrain.DetailPrototypes.Length != gpuiTerrain.DetailPrototypeIndexes.Length))
			{
				this.DeterminePrototypeIndexes(gpuiTerrain);
			}
			return gpuiTerrain.DetailPrototypeIndexes;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007560 File Offset: 0x00005760
		public int AddDetailPrototype(DetailPrototype detailPrototype)
		{
			int num = -1;
			int num2 = 0;
			if (this._prototypes != null && this._prototypeDataArray != null)
			{
				for (int i = 0; i < this._prototypes.Length; i++)
				{
					GPUIPrototype gpuiprototype = this._prototypes[i];
					GPUIDetailPrototypeData gpuidetailPrototypeData = this._prototypeDataArray[i];
					if (gpuidetailPrototypeData.IsMatchingPrefabAndTexture(detailPrototype, gpuiprototype, true))
					{
						num = i;
						num2 = gpuidetailPrototypeData.GetSubSettingCount();
						for (int j = 0; j < num2; j++)
						{
							if (gpuidetailPrototypeData.HasSameSettingsWith(detailPrototype, j))
							{
								num2 = j;
								break;
							}
						}
						break;
					}
				}
			}
			if (num < 0)
			{
				if (detailPrototype.prototype != null)
				{
					num = this.AddPrototype(new GPUIPrototype(detailPrototype.prototype.GetPrefabRoot(), this.GetDefaultProfile()));
				}
				else
				{
					num = this.AddPrototype(new GPUIPrototype(GPUITerrainConstants.DefaultDetailMesh, new Material[] { GPUITerrainConstants.DefaultDetailMaterial }, this.GetTexturePrototypeProfile()));
				}
			}
			if (num < 0)
			{
				if (detailPrototype.prototype != null)
				{
					string log_PREFIX = GPUIConstants.LOG_PREFIX;
					string text = "Failed adding a new Detail prototype: ";
					GameObject prototype = detailPrototype.prototype;
					Debug.LogError(log_PREFIX + text + ((prototype != null) ? prototype.ToString() : null), detailPrototype.prototype);
				}
				else
				{
					string log_PREFIX2 = GPUIConstants.LOG_PREFIX;
					string text2 = "Failed adding a new Detail prototype: ";
					Texture2D prototypeTexture = detailPrototype.prototypeTexture;
					Debug.LogError(log_PREFIX2 + text2 + ((prototypeTexture != null) ? prototypeTexture.ToString() : null), detailPrototype.prototypeTexture);
				}
				return -1;
			}
			this._prototypeDataArray[num].ReadFromDetailPrototypeData(detailPrototype, num2, this, num);
			this.OnNewPrototypeDataCreated(num);
			return num;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000076C0 File Offset: 0x000058C0
		public override void OnPrototypePropertiesModified()
		{
			base.OnPrototypePropertiesModified();
			if (!base.IsInitialized)
			{
				return;
			}
			if (this.healthyDryNoiseTexture == null)
			{
				this.healthyDryNoiseTexture = GPUITerrainConstants.DefaultHealthyDryNoiseTexture;
			}
			for (int i = 0; i < this._prototypes.Length; i++)
			{
				GPUIRenderSourceGroup gpuirenderSourceGroup;
				if (this._runtimeRenderKeys[i] != 0 && this._prototypeDataArray[i].detailTexture != null && GPUIRenderingSystem.TryGetRenderSourceGroup(this._runtimeRenderKeys[i], out gpuirenderSourceGroup))
				{
					this._prototypeDataArray[i].SetMPBValues(this, i, gpuirenderSourceGroup);
				}
			}
			for (int j = 0; j < this._prototypeDataArray.Length; j++)
			{
				this._prototypeDataArray[j].SetParameterBufferData();
			}
			this.RequireUpdate();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007770 File Offset: 0x00005970
		public void RemoveDetailPrototypeAtIndex(int index, bool removeFromTerrain)
		{
			if (removeFromTerrain)
			{
				int terrainCount = base.GetTerrainCount();
				for (int i = 0; i < terrainCount; i++)
				{
					GPUITerrain terrain = base.GetTerrain(i);
					if (terrain != null)
					{
						terrain.RemoveDetailPrototypeAtIndex(index);
					}
				}
			}
			this.RemovePrototypeAtIndex(index);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000077B2 File Offset: 0x000059B2
		public override bool CanAddObjectAsPrototype(Object obj)
		{
			return base.CanAddObjectAsPrototype(obj) || obj is Texture2D;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000077CC File Offset: 0x000059CC
		public void AddPrototypeToTerrains(Object pickerObject, int overwriteIndex)
		{
			int terrainCount = base.GetTerrainCount();
			int num = Random.Range(100, 100000);
			for (int i = 0; i < terrainCount; i++)
			{
				GPUITerrain terrain = base.GetTerrain(i);
				if (terrain != null)
				{
					terrain.AddDetailPrototypeToTerrain(pickerObject, overwriteIndex, num);
				}
			}
			this.CheckPrototypeChanges();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000781C File Offset: 0x00005A1C
		protected override void OnFirstTerrainAdded(Terrain terrain)
		{
			base.OnFirstTerrainAdded(terrain);
			if (this.detailObjectDistance == 250f)
			{
				float num = terrain.detailObjectDistance;
				if (num > 0f)
				{
					this.detailObjectDistance = num;
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007853 File Offset: 0x00005A53
		public override void RequireUpdate()
		{
			this._requireUpdateFrame = Time.frameCount;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007860 File Offset: 0x00005A60
		public void RequireUpdate(bool forceImmediateUpdate, bool reloadTerrainDetailTextures = false)
		{
			if (forceImmediateUpdate)
			{
				this._forceImmediateUpdate = true;
			}
			if (reloadTerrainDetailTextures)
			{
				this._reloadTerrainDetailTextures = true;
			}
			this.RequireUpdate();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000787C File Offset: 0x00005A7C
		public override GPUIProfile GetDefaultProfile()
		{
			if (this.defaultProfile != null)
			{
				return this.defaultProfile;
			}
			return GPUITerrainConstants.DefaultDetailPrefabProfile;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007898 File Offset: 0x00005A98
		public GPUIProfile GetTexturePrototypeProfile()
		{
			if (this.defaultDetailTextureProfile != null)
			{
				return this.defaultDetailTextureProfile;
			}
			return GPUITerrainConstants.DefaultDetailTextureProfile;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000078B4 File Offset: 0x00005AB4
		public override int GetRendererGroupID(int prototypeIndex)
		{
			return GPUIUtility.GenerateHash(new int[]
			{
				this.GetInstanceIDGPUI(),
				prototypeIndex
			});
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003B7A File Offset: 0x00001D7A
		public override GPUITransformBufferType GetTransformBufferType(int prototypeIndex)
		{
			return GPUITransformBufferType.CameraBased;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000078CE File Offset: 0x00005ACE
		public Bounds GetPrototypeBounds(int prototypeIndex)
		{
			return this._prototypeDataArray[prototypeIndex]._bounds;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000078E0 File Offset: 0x00005AE0
		public void SetDistanceDensityReduction(bool enabled)
		{
			if (this._prototypeDataArray != null)
			{
				for (int i = 0; i < this._prototypeDataArray.Length; i++)
				{
					this._prototypeDataArray[i].isUseDensityReduction = enabled;
				}
				this.RequireUpdate();
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000791C File Offset: 0x00005B1C
		public void SetDetailObjectDistance(float distance)
		{
			this.detailObjectDistance = distance;
			this.RequireUpdate();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000792C File Offset: 0x00005B2C
		public bool IsReadTerrainDetails(int prototypeIndex)
		{
			GPUIDetailPrototypeData prototypeData = base.GetPrototypeData(prototypeIndex);
			return prototypeData == null || !(prototypeData.proceduralDensityData != null) || prototypeData.proceduralDensityData.isReadTerrainDetails;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00007964 File Offset: 0x00005B64
		public bool HasProceduralDensity()
		{
			int prototypeCount = base.GetPrototypeCount();
			for (int i = 0; i < prototypeCount; i++)
			{
				if (this._prototypeDataArray[i].proceduralDensityData != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040000B0 RID: 176
		[SerializeField]
		public GPUIProfile defaultDetailTextureProfile;

		// Token: 0x040000B1 RID: 177
		[SerializeField]
		public float detailObjectDistance = 250f;

		// Token: 0x040000B2 RID: 178
		[SerializeField]
		public Vector4 windVector = new Vector2(0.4f, 0.8f);

		// Token: 0x040000B3 RID: 179
		[SerializeField]
		public Texture2D healthyDryNoiseTexture;

		// Token: 0x040000B4 RID: 180
		[SerializeField]
		[Range(0f, 100f)]
		public float detailUpdateDistance = 1f;

		// Token: 0x040000B5 RID: 181
		[SerializeField]
		public bool disableAsyncDetailDataRequest;

		// Token: 0x040000B6 RID: 182
		[SerializeField]
		public bool waitForAsyncDetailDataRequestCompletion;

		// Token: 0x040000B7 RID: 183
		[SerializeField]
		public bool isCheckTerrainBounds = true;

		// Token: 0x040000B8 RID: 184
		[NonSerialized]
		private int _requireUpdateFrame;

		// Token: 0x040000B9 RID: 185
		[NonSerialized]
		private Dictionary<int, GPUIDetailManager.GPUIDetailUpdateData> _detailUpdateDataDict;

		// Token: 0x040000BA RID: 186
		[NonSerialized]
		private Action<GPUIDataBuffer<uint>> _processCounterDataCallback;

		// Token: 0x040000BB RID: 187
		[NonSerialized]
		private bool _forceImmediateUpdate;

		// Token: 0x040000BC RID: 188
		[NonSerialized]
		private int[] _sizeAndIndexes;

		// Token: 0x040000BD RID: 189
		[NonSerialized]
		private bool _reloadTerrainDetailTextures;

		// Token: 0x040000BE RID: 190
		private const int ERROR_CODE_ADDITION = 400;

		// Token: 0x040000BF RID: 191
		public const int DETAIL_SUB_SETTING_DIVIDER = 1000;

		// Token: 0x02000014 RID: 20
		internal class GPUIDetailUpdateData : GPUIDataBuffer<uint>
		{
			// Token: 0x06000106 RID: 262 RVA: 0x000079DB File Offset: 0x00005BDB
			public GPUIDetailUpdateData(GPUICameraData cameraData, string name, int length, GraphicsBuffer.Target target = GraphicsBuffer.Target.Structured)
				: base(name, length, target)
			{
				this.cameraData = cameraData;
				this.position = Vector3.negativeInfinity;
				this.lastUpdateFrame = 0;
			}

			// Token: 0x06000107 RID: 263 RVA: 0x00007A00 File Offset: 0x00005C00
			public void ProcessGPUReadback(GPUIDetailManager detailManager)
			{
				if (detailManager._runtimeRenderKeys == null)
				{
					return;
				}
				NativeArray<uint> requestedData = base.GetRequestedData();
				if (!requestedData.IsCreated)
				{
					return;
				}
				bool flag = GPUIRenderingSystem.Instance.CameraDataProvider.Count > 1 || !Application.isPlaying;
				int length = requestedData.Length;
				int prototypeCount = detailManager.GetPrototypeCount();
				int num = 0;
				while (num < prototypeCount && num < length)
				{
					int num2 = detailManager._runtimeRenderKeys[num];
					if (num2 != 0)
					{
						GPUIRenderSourceGroup gpuirenderSourceGroup;
						if (GPUIRenderingSystem.TryGetRenderSourceGroup(num2, out gpuirenderSourceGroup))
						{
							int num3 = (int)requestedData[num];
							GPUIDetailPrototypeData gpuidetailPrototypeData = detailManager._prototypeDataArray[num];
							if (num3 > 0 && num3 != gpuirenderSourceGroup.InstanceCount)
							{
								int num4 = Mathf.Max(Mathf.CeilToInt((float)num3 * gpuidetailPrototypeData.detailExtraBufferSizePercentage), 1024);
								if (num3 > gpuirenderSourceGroup.BufferSize)
								{
									GPUIRenderingSystem.SetBufferSize(num2, num3 + num4, false);
									if (!flag)
									{
										GPUIRenderingSystem.SetInstanceCount(num2, num3);
									}
									detailManager.RequireUpdate();
								}
								else if (!flag && gpuidetailPrototypeData.detailBufferSizePercentageDifferenceForReduction > 0f)
								{
									int num5 = Mathf.CeilToInt((float)num3 * gpuidetailPrototypeData.detailBufferSizePercentageDifferenceForReduction) + num4;
									if (gpuirenderSourceGroup.BufferSize - num3 > num5)
									{
										GPUIRenderingSystem.SetBufferSize(num2, num3 + num4, false);
										GPUIRenderingSystem.SetInstanceCount(num2, num3);
										detailManager.RequireUpdate();
									}
									else
									{
										GPUIRenderingSystem.SetInstanceCount(num2, num3);
									}
								}
								else if (num3 > gpuirenderSourceGroup.InstanceCount)
								{
									GPUIRenderingSystem.SetInstanceCount(num2, num3);
								}
							}
						}
						else
						{
							Debug.LogWarning(GPUIConstants.LOG_PREFIX + "Can not find renderer with key: " + detailManager._runtimeRenderKeys[num].ToString());
						}
					}
					num++;
				}
			}

			// Token: 0x040000C0 RID: 192
			public GPUICameraData cameraData;

			// Token: 0x040000C1 RID: 193
			public Vector3 position;

			// Token: 0x040000C2 RID: 194
			public int lastUpdateFrame;

			// Token: 0x040000C3 RID: 195
			public bool requireReadback;

			// Token: 0x040000C4 RID: 196
			public bool processReadback;
		}
	}
}
