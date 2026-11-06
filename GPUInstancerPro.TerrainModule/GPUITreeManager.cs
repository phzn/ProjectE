using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

namespace GPUInstancerPro.TerrainModule
{
	// Token: 0x02000016 RID: 22
	[ExecuteInEditMode]
	[DefaultExecutionOrder(200)]
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#The_Tree_Manager")]
	public class GPUITreeManager : GPUITerrainManager<GPUITreePrototypeData>
	{
		// Token: 0x06000128 RID: 296 RVA: 0x00008404 File Offset: 0x00006604
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
				if (terrain != null && terrain.TreePrototypes != null && terrain.TreePrototypes.Length != 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.errorCode = -502;
				return false;
			}
			return true;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00008468 File Offset: 0x00006668
		public override void Initialize()
		{
			base.Initialize();
			int num = this._prototypes.Length;
			this._treeInstanceCounts = new int[num];
			this._treeTransformBuffers = new GPUITransformBufferData[num];
			this._treeTransformBufferStartIndexes = new int[num];
			this._counterDataBuffer = new GPUIDataBuffer<uint>("Tree Counter Buffer", num, GraphicsBuffer.Target.Structured);
			this._batchedTreeTerrainData = new List<GPUITreeManager.GPUITreeTerrainData>();
			this._batchedTerrains = new List<GPUITerrain>();
			this._unbatchedTerrains = new List<GPUITerrain>();
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.UpdateTreeMatrices));
			GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
			instance2.OnPreCull = (Action<GPUICameraData>)Delegate.Combine(instance2.OnPreCull, new Action<GPUICameraData>(this.UpdateTreeMatrices));
			if (GPUITerrain._terrainsSearchingForTreeManager != null)
			{
				base.AddTerrains(GPUITerrain._terrainsSearchingForTreeManager);
				GPUITerrain._terrainsSearchingForTreeManager.Clear();
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008544 File Offset: 0x00006744
		public override void Dispose()
		{
			base.Dispose();
			this._treeInstanceCounts = null;
			this._treeTransformBuffers = null;
			this._treeTransformBufferStartIndexes = null;
			this._batchedTreeTerrainData = null;
			this._batchedTerrains = null;
			this._unbatchedTerrains = null;
			this._batchedPrototypeIndexes = null;
			if (this._counterDataBuffer != null)
			{
				this._counterDataBuffer.Dispose();
				this._counterDataBuffer = null;
			}
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
				instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.UpdateTreeMatrices));
			}
			if (this._treeDataBuffer != null)
			{
				this._treeDataBuffer.Dispose();
				this._treeDataBuffer = null;
			}
			if (this._batchedTreeTerrainDataBuffer != null)
			{
				this._batchedTreeTerrainDataBuffer.Dispose();
				this._batchedTreeTerrainDataBuffer = null;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008603 File Offset: 0x00006803
		private void UpdateTreeMatrices(GPUICameraData cameraData)
		{
			this.UpdateTreeMatrices();
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000860C File Offset: 0x0000680C
		private void UpdateTreeMatrices()
		{
			if (!GPUIRenderingSystem.IsActive || !base.IsInitialized)
			{
				if (this._treeDataBuffer != null)
				{
					this._treeDataBuffer.Dispose();
					this._treeDataBuffer = null;
				}
				return;
			}
			bool flag = true;
			if (!this._requireUpdate)
			{
				base.CheckPendingTerrainTransformChanges();
				if (!this._requireUpdate)
				{
					if (!this._isPersistentTreeDataBuffer && this._treeDataBuffer != null && Time.frameCount - this._lastTreeDataBufferUpdateFrame > GPUITreeManager.TREE_DATA_BUFFER_DISPOSE_FRAME_COUNT)
					{
						this._treeDataBuffer.Dispose();
						this._treeDataBuffer = null;
					}
					return;
				}
				flag = false;
			}
			this._hasPendingTerrainTransformChanges = false;
			this._requireUpdate = false;
			int num = this._prototypes.Length;
			if (num == 0)
			{
				return;
			}
			if (this._treeInstanceCounts.Length != num)
			{
				this._treeInstanceCounts = new int[num];
			}
			int activeTerrainCount = base.GetActiveTerrainCount();
			int num2 = num + activeTerrainCount * num;
			if (this._counterDataBuffer.Length < num2)
			{
				this._counterDataBuffer.Resize(num2);
			}
			for (int i = 0; i < num; i++)
			{
				this._counterDataBuffer[i] = 0U;
			}
			int num3 = 0;
			int num4 = 0;
			this._batchedTreeTerrainData.Clear();
			this._batchedTerrains.Clear();
			this._unbatchedTerrains.Clear();
			if (this._batchedPrototypeIndexes == null || this._batchedPrototypeIndexes.Length != num)
			{
				this._batchedPrototypeIndexes = new uint[num];
			}
			bool flag2 = false;
			int num5 = num;
			foreach (GPUITerrain gpuiterrain in base.GetActiveTerrainValues())
			{
				if (this.IsRenderTerrainTrees(gpuiterrain))
				{
					int[] terrainPrototypeIndexes = this.GetTerrainPrototypeIndexes(gpuiterrain);
					if (terrainPrototypeIndexes != null)
					{
						TreeInstance[] treeInstances = gpuiterrain.GetTreeInstances(this._reloadTreeInstances);
						int num6 = treeInstances.Length;
						if (num6 != 0)
						{
							num3 = Mathf.Max(num3, num6);
							for (int j = 0; j < num6; j++)
							{
								int prototypeIndex = treeInstances[j].prototypeIndex;
								if (prototypeIndex < terrainPrototypeIndexes.Length && prototypeIndex >= 0)
								{
									int num7 = terrainPrototypeIndexes[prototypeIndex];
									if (num7 >= 0 && num7 < num)
									{
										this._treeInstanceCounts[num7]++;
									}
								}
							}
							if (activeTerrainCount > 2 && !this._enableTreeInstanceColors && gpuiterrain.terrainHolesSampleMode != GPUITerrain.GPUITerrainHolesSampleMode.Runtime)
							{
								Matrix4x4 matrix4x = GPUIConstants.IDENTITY_Matrix4x4;
								if (gpuiterrain.HasRotationSupport())
								{
									matrix4x = gpuiterrain.GetRotationMatrix();
									if (!flag2)
									{
										flag2 = !matrix4x.EqualsMatrix4x4(GPUIConstants.IDENTITY_Matrix4x4);
									}
								}
								this._batchedTreeTerrainData.Add(new GPUITreeManager.GPUITreeTerrainData
								{
									terrainSize = gpuiterrain.GetSize(),
									terrainPosition = gpuiterrain.GetPosition(),
									treeDataStartIndex = num4,
									treeCount = num6,
									gpuiTransformOffset = matrix4x
								});
								num4 += num6;
								uint num8 = 0U;
								while ((ulong)num8 < (ulong)((long)num))
								{
									this._batchedPrototypeIndexes[(int)num8] = uint.MaxValue;
									uint num9 = 0U;
									while ((ulong)num9 < (ulong)((long)terrainPrototypeIndexes.Length))
									{
										if ((long)terrainPrototypeIndexes[(int)num9] == (long)((ulong)num8))
										{
											this._batchedPrototypeIndexes[(int)num8] = num9;
											break;
										}
										num9 += 1U;
									}
									num8 += 1U;
								}
								this._counterDataBuffer.SetData(num5, 0, num, this._batchedPrototypeIndexes);
								num5 += num;
								this._batchedTerrains.Add(gpuiterrain);
							}
							else
							{
								flag = true;
								this._unbatchedTerrains.Add(gpuiterrain);
							}
						}
					}
				}
			}
			num3 = Mathf.Max(num3, num4);
			this._reloadTreeInstances = false;
			if (this._treeTransformBuffers.Length != num)
			{
				this._treeTransformBuffers = new GPUITransformBufferData[num];
			}
			if (this._treeTransformBufferStartIndexes.Length != num)
			{
				this._treeTransformBufferStartIndexes = new int[num];
			}
			for (int k = 0; k < num; k++)
			{
				if (!this._prototypes[k].isEnabled || this._runtimeRenderKeys[k] == 0)
				{
					this._treeTransformBuffers[k] = null;
				}
				else
				{
					int num10 = this._treeInstanceCounts[k];
					GPUIRenderingSystem.SetBufferSize(this._runtimeRenderKeys[k], num10, false);
					GPUIRenderingSystem.SetInstanceCount(this._runtimeRenderKeys[k], num10);
					GraphicsBuffer treeInstanceDataBuffer = this._prototypeDataArray[k]._treeInstanceDataBuffer;
					if (treeInstanceDataBuffer != null)
					{
						treeInstanceDataBuffer.Release();
					}
					if (num10 > 0)
					{
						if (this._enableTreeInstanceColors)
						{
							this._prototypeDataArray[k]._treeInstanceDataBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num10, 16);
						}
						int num11;
						if (GPUIRenderingSystem.TryGetTransformBufferData(this._runtimeRenderKeys[k], out this._treeTransformBuffers[k], out this._treeTransformBufferStartIndexes[k], out num11, true))
						{
							GPUIShaderBuffer transformBuffer = this._treeTransformBuffers[k].GetTransformBuffer();
							if (transformBuffer != null)
							{
								transformBuffer.CompleteAsyncRequests();
							}
						}
						else
						{
							string log_PREFIX = GPUIConstants.LOG_PREFIX;
							string text = "Tree Manager can not find transform buffer for prototype: ";
							GPUIPrototype gpuiprototype = this._prototypes[k];
							Debug.LogError(log_PREFIX + text + ((gpuiprototype != null) ? gpuiprototype.ToString() : null));
						}
					}
					else
					{
						this._treeTransformBuffers[k] = null;
					}
				}
			}
			ComputeShader cs_TerrainTreeGenerator = GPUITerrainConstants.CS_TerrainTreeGenerator;
			if (this._enableTreeInstanceColors)
			{
				cs_TerrainTreeGenerator.EnableKeyword(GPUITerrainConstants.Kw_GPUI_TREE_INSTANCE_COLOR);
				for (int l = 0; l < num; l++)
				{
					GPUIRenderSourceGroup gpuirenderSourceGroup;
					if (this._treeInstanceCounts[l] > 0 && GPUIRenderingSystem.TryGetRenderSourceGroup(this._runtimeRenderKeys[l], out gpuirenderSourceGroup))
					{
						gpuirenderSourceGroup.AddMaterialPropertyOverride(GPUITerrainConstants.PROP_gpuiTreeInstanceDataBuffer, this._prototypeDataArray[l]._treeInstanceDataBuffer, -1, -1, true);
					}
				}
				cs_TerrainTreeGenerator.SetBool(GPUIConstants.PROP_isLinearSpace, QualitySettings.activeColorSpace == ColorSpace.Linear);
			}
			else
			{
				cs_TerrainTreeGenerator.DisableKeyword(GPUITerrainConstants.Kw_GPUI_TREE_INSTANCE_COLOR);
			}
			if (num3 > 0)
			{
				if (this._treeDataBuffer != null && this._treeDataBuffer.count < num3)
				{
					this._treeDataBuffer.Dispose();
					this._treeDataBuffer = null;
				}
				if (this._treeDataBuffer == null)
				{
					this._treeDataBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num3, Marshal.SizeOf(typeof(TreeInstance)));
					flag = true;
				}
				if (GPUITreeManager.TERRAIN_TREE_PROTOTYPE_INDEXES == null)
				{
					GPUITreeManager.TERRAIN_TREE_PROTOTYPE_INDEXES = new List<int>();
				}
				this._counterDataBuffer.UpdateBufferData(false);
				if (this._batchedTreeTerrainData.Count > 0)
				{
					ComputeShader cs_TerrainTreeGeneratorBatched = GPUITerrainConstants.CS_TerrainTreeGeneratorBatched;
					if (flag2)
					{
						cs_TerrainTreeGeneratorBatched.EnableKeyword("GPUI_TRANSFORM_OFFSET");
					}
					else
					{
						cs_TerrainTreeGeneratorBatched.DisableKeyword("GPUI_TRANSFORM_OFFSET");
					}
					if (flag)
					{
						for (int m = 0; m < this._batchedTerrains.Count; m++)
						{
							GPUITreeManager.GPUITreeTerrainData gpuitreeTerrainData = this._batchedTreeTerrainData[m];
							this._treeDataBuffer.SetData(this._batchedTerrains[m].GetTreeInstances(false), 0, gpuitreeTerrainData.treeDataStartIndex, gpuitreeTerrainData.treeCount);
						}
					}
					int count = this._batchedTreeTerrainData.Count;
					if (this._batchedTreeTerrainDataBuffer != null && this._batchedTreeTerrainDataBuffer.count < count)
					{
						this._batchedTreeTerrainDataBuffer.Dispose();
						this._batchedTreeTerrainDataBuffer = null;
					}
					if (this._batchedTreeTerrainDataBuffer == null)
					{
						this._batchedTreeTerrainDataBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, Marshal.SizeOf(typeof(GPUITreeManager.GPUITreeTerrainData)));
					}
					this._batchedTreeTerrainDataBuffer.SetData<GPUITreeManager.GPUITreeTerrainData>(this._batchedTreeTerrainData);
					for (int n = 0; n < num; n++)
					{
						GPUITransformBufferData gpuitransformBufferData = this._treeTransformBuffers[n];
						if (gpuitransformBufferData != null)
						{
							GPUIShaderBuffer transformBuffer2 = gpuitransformBufferData.GetTransformBuffer();
							if (transformBuffer2 != null && transformBuffer2.Buffer != null)
							{
								int num12 = this._treeTransformBufferStartIndexes[n];
								GPUIPrototype gpuiprototype2 = this._prototypes[n];
								GPUITreePrototypeData gpuitreePrototypeData = this._prototypeDataArray[n];
								cs_TerrainTreeGeneratorBatched.SetBuffer(0, GPUIConstants.PROP_gpuiTransformBuffer, transformBuffer2.Buffer);
								cs_TerrainTreeGeneratorBatched.SetBuffer<uint>(0, GPUIConstants.PROP_counterBuffer, this._counterDataBuffer);
								cs_TerrainTreeGeneratorBatched.SetBuffer(0, GPUITerrainConstants.PROP_treeData, this._treeDataBuffer);
								cs_TerrainTreeGeneratorBatched.SetBuffer(0, GPUITerrainConstants.PROP_terrainDataBuffer, this._batchedTreeTerrainDataBuffer);
								cs_TerrainTreeGeneratorBatched.SetInt(GPUIConstants.PROP_bufferSize, num4);
								cs_TerrainTreeGeneratorBatched.SetInt(GPUITerrainConstants.PROP_terrainDataCount, count);
								cs_TerrainTreeGeneratorBatched.SetInt(GPUIConstants.PROP_transformBufferStartIndex, num12);
								cs_TerrainTreeGeneratorBatched.SetInt(GPUIConstants.PROP_prototypeIndex, n);
								cs_TerrainTreeGeneratorBatched.SetInt(GPUIConstants.PROP_prototypeCount, num);
								cs_TerrainTreeGeneratorBatched.SetVector(GPUITerrainConstants.PROP_prefabScale, gpuitreePrototypeData.isApplyPrefabScale ? gpuiprototype2.prefabObject.transform.localScale : Vector3.one);
								cs_TerrainTreeGeneratorBatched.SetBool(GPUITerrainConstants.PROP_applyRotation, gpuitreePrototypeData.isApplyRotation);
								cs_TerrainTreeGeneratorBatched.SetBool(GPUITerrainConstants.PROP_applyHeight, gpuitreePrototypeData.isApplyHeight);
								cs_TerrainTreeGeneratorBatched.DispatchXY(0, num4, count);
								gpuitransformBufferData.OnTransformDataModified();
								gpuitransformBufferData.ResetPreviousFrameBuffer();
							}
						}
					}
				}
				foreach (GPUITerrain gpuiterrain2 in this._unbatchedTerrains)
				{
					TreeInstance[] treeInstances2 = gpuiterrain2.GetTreeInstances(false);
					int num13 = treeInstances2.Length;
					this._treeDataBuffer.SetData(treeInstances2);
					Vector3 size = gpuiterrain2.GetSize();
					Vector3 position = gpuiterrain2.GetPosition();
					bool flag3 = gpuiterrain2.terrainHolesSampleMode == GPUITerrain.GPUITerrainHolesSampleMode.Runtime;
					Texture holesTexture = gpuiterrain2.GetHolesTexture();
					bool flag4 = false;
					Matrix4x4 matrix4x2 = GPUIConstants.IDENTITY_Matrix4x4;
					if (gpuiterrain2.HasRotationSupport())
					{
						matrix4x2 = gpuiterrain2.GetRotationMatrix();
						flag4 = !matrix4x2.EqualsMatrix4x4(GPUIConstants.IDENTITY_Matrix4x4);
					}
					for (int num14 = 0; num14 < num; num14++)
					{
						GPUITransformBufferData gpuitransformBufferData2 = this._treeTransformBuffers[num14];
						if (gpuitransformBufferData2 != null)
						{
							GPUIShaderBuffer transformBuffer3 = gpuitransformBufferData2.GetTransformBuffer();
							if (transformBuffer3 != null && transformBuffer3.Buffer != null)
							{
								int num15 = this._treeTransformBufferStartIndexes[num14];
								GPUIPrototype gpuiprototype3 = this._prototypes[num14];
								GPUITreePrototypeData gpuitreePrototypeData2 = this._prototypeDataArray[num14];
								gpuiterrain2.GetTerrainTreePrototypeIndexes(num14, ref GPUITreeManager.TERRAIN_TREE_PROTOTYPE_INDEXES);
								foreach (int num16 in GPUITreeManager.TERRAIN_TREE_PROTOTYPE_INDEXES)
								{
									cs_TerrainTreeGenerator.SetBuffer(0, GPUIConstants.PROP_gpuiTransformBuffer, transformBuffer3.Buffer);
									cs_TerrainTreeGenerator.SetBuffer(0, GPUITerrainConstants.PROP_treeData, this._treeDataBuffer);
									cs_TerrainTreeGenerator.SetBuffer<uint>(0, GPUIConstants.PROP_counterBuffer, this._counterDataBuffer);
									if (this._enableTreeInstanceColors)
									{
										cs_TerrainTreeGenerator.SetBuffer(0, GPUITerrainConstants.PROP_gpuiTreeInstanceDataBuffer, gpuitreePrototypeData2._treeInstanceDataBuffer);
									}
									cs_TerrainTreeGenerator.SetInt(GPUIConstants.PROP_bufferSize, num13);
									cs_TerrainTreeGenerator.SetInt(GPUIConstants.PROP_transformBufferStartIndex, num15);
									cs_TerrainTreeGenerator.SetInt(GPUIConstants.PROP_prototypeIndex, num14);
									cs_TerrainTreeGenerator.SetInt(GPUITerrainConstants.PROP_terrainPrototypeIndex, num16);
									cs_TerrainTreeGenerator.SetVector(GPUITerrainConstants.PROP_terrainSize, size);
									cs_TerrainTreeGenerator.SetVector(GPUITerrainConstants.PROP_terrainPosition, position);
									cs_TerrainTreeGenerator.SetVector(GPUITerrainConstants.PROP_prefabScale, gpuitreePrototypeData2.isApplyPrefabScale ? gpuiprototype3.prefabObject.transform.localScale : Vector3.one);
									cs_TerrainTreeGenerator.SetBool(GPUITerrainConstants.PROP_applyRotation, gpuitreePrototypeData2.isApplyRotation);
									cs_TerrainTreeGenerator.SetBool(GPUITerrainConstants.PROP_applyHeight, gpuitreePrototypeData2.isApplyHeight);
									if (flag3 && holesTexture != null)
									{
										cs_TerrainTreeGenerator.EnableKeyword(GPUITerrainConstants.Kw_GPUI_TERRAIN_HOLES);
										cs_TerrainTreeGenerator.SetTexture(0, GPUITerrainConstants.PROP_terrainHoleTexture, holesTexture);
									}
									else
									{
										cs_TerrainTreeGenerator.DisableKeyword(GPUITerrainConstants.Kw_GPUI_TERRAIN_HOLES);
									}
									if (flag4)
									{
										cs_TerrainTreeGenerator.EnableKeyword("GPUI_TRANSFORM_OFFSET");
										cs_TerrainTreeGenerator.SetMatrix(GPUIConstants.PROP_gpuiTransformOffset, matrix4x2);
									}
									else
									{
										cs_TerrainTreeGenerator.DisableKeyword("GPUI_TRANSFORM_OFFSET");
									}
									cs_TerrainTreeGenerator.DispatchX(0, num13);
								}
								gpuitransformBufferData2.OnTransformDataModified();
								gpuitransformBufferData2.ResetPreviousFrameBuffer();
							}
						}
					}
				}
				this._lastTreeDataBufferUpdateFrame = Time.frameCount;
			}
			for (int num17 = 0; num17 < this._treeInstanceCounts.Length; num17++)
			{
				this._treeInstanceCounts[num17] = 0;
			}
			base.OnLightProbesUpdated();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000916C File Offset: 0x0000736C
		private bool IsRenderTerrainTrees(GPUITerrain gpuiTerrain)
		{
			return gpuiTerrain != null && gpuiTerrain.isActiveAndEnabled;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00009180 File Offset: 0x00007380
		protected override void OnUpdatePerInstanceLightProbes(int prototypeIndex)
		{
			GPUITransformBufferData transformBufferData;
			int bufferStartIndex;
			int bufferSize;
			if (GPUIRenderingSystem.TryGetTransformBufferData(this._runtimeRenderKeys[prototypeIndex], out transformBufferData, out bufferStartIndex, out bufferSize, false))
			{
				GPUIShaderBuffer transformBuffer = transformBufferData.GetTransformBuffer();
				if (transformBuffer == null)
				{
					return;
				}
				transformBuffer.CompleteAsyncRequests();
				transformBuffer.AsyncRequestIntoNativeArray(delegate(NativeArray<Matrix4x4> matrices)
				{
					transformBufferData.CalculateInterpolatedLightAndOcclusionProbes<Matrix4x4>(matrices, 0, bufferStartIndex, bufferSize);
					matrices.Dispose();
				});
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000091E0 File Offset: 0x000073E0
		protected override bool AddMissingPrototypesFromTerrain(GPUITerrain gpuiTerrain)
		{
			bool flag = false;
			TreePrototype[] treePrototypes = gpuiTerrain.TreePrototypes;
			int[] terrainPrototypeIndexes = this.GetTerrainPrototypeIndexes(gpuiTerrain);
			for (int i = 0; i < terrainPrototypeIndexes.Length; i++)
			{
				flag |= terrainPrototypeIndexes[i] < 0 && this.AddTreePrototype(treePrototypes[i]) >= 0;
			}
			return flag;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00009228 File Offset: 0x00007428
		protected override void SetGPUITerrainManager(GPUITerrain gpuiTerrain)
		{
			gpuiTerrain.SetTreeManager(this);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009231 File Offset: 0x00007431
		protected override void RemoveGPUITerrainManager(GPUITerrain gpuiTerrain)
		{
			if (gpuiTerrain.TreeManager == this)
			{
				gpuiTerrain.RemoveTreeManager();
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00009248 File Offset: 0x00007448
		internal int DetermineTreePrototypeIndex(TreePrototype treePrototype)
		{
			if (this._prototypes != null)
			{
				for (int i = 0; i < this._prototypes.Length; i++)
				{
					GPUIPrototype gpuiprototype = this._prototypes[i];
					if (treePrototype.prefab == gpuiprototype.prefabObject)
					{
						return i;
					}
				}
			}
			if (this._isAutoAddPrototypesBasedOnTerrains)
			{
				this._isTerrainsModified = true;
			}
			return -1;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000929E File Offset: 0x0000749E
		protected override void DeterminePrototypeIndexes(GPUITerrain gpuiTerrain)
		{
			gpuiTerrain.DetermineTreePrototypeIndexes(this);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000092A7 File Offset: 0x000074A7
		protected override int[] GetTerrainPrototypeIndexes(GPUITerrain gpuiTerrain)
		{
			if (gpuiTerrain.TreePrototypes == null)
			{
				gpuiTerrain.LoadTerrainData();
			}
			if (gpuiTerrain.TreePrototypes != null && (gpuiTerrain.TreePrototypeIndexes == null || gpuiTerrain.TreePrototypes.Length != gpuiTerrain.TreePrototypeIndexes.Length))
			{
				this.DeterminePrototypeIndexes(gpuiTerrain);
			}
			return gpuiTerrain.TreePrototypeIndexes;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000092E8 File Offset: 0x000074E8
		public int AddTreePrototype(TreePrototype treePrototype)
		{
			if (treePrototype == null || treePrototype.prefab == null)
			{
				return -1;
			}
			if (this._prototypes != null)
			{
				for (int i = 0; i < this._prototypes.Length; i++)
				{
					if (this._prototypes[i] != null && this._prototypes[i].prefabObject == treePrototype.prefab)
					{
						return i;
					}
				}
			}
			GPUITreePrototypeData gpuitreePrototypeData = new GPUITreePrototypeData(treePrototype);
			int num = 0;
			if (this._prototypeDataArray == null)
			{
				this._prototypeDataArray = new GPUITreePrototypeData[1];
			}
			else
			{
				num = this._prototypeDataArray.Length;
				Array.Resize<GPUITreePrototypeData>(ref this._prototypeDataArray, num + 1);
			}
			this._prototypeDataArray[num] = gpuitreePrototypeData;
			GPUIPrototype gpuiprototype = new GPUIPrototype(treePrototype.prefab, this.GetDefaultProfile());
			if (this._autoGenerateBillboards && (!treePrototype.prefab.HasComponent<LODGroup>() || treePrototype.prefab.HasComponentInChildren<BillboardRenderer>()))
			{
				gpuiprototype.isGenerateBillboard = true;
			}
			int num2 = this.AddPrototype(gpuiprototype);
			this.OnNewPrototypeDataCreated(num);
			return num2;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000093D4 File Offset: 0x000075D4
		public void RemoveTreePrototypeAtIndex(int index, bool removeFromTerrain)
		{
			if (removeFromTerrain)
			{
				int terrainCount = base.GetTerrainCount();
				for (int i = 0; i < terrainCount; i++)
				{
					GPUITerrain terrain = base.GetTerrain(i);
					if (terrain != null)
					{
						terrain.RemoveTreePrototypeAtIndex(index);
					}
				}
			}
			this.RemovePrototypeAtIndex(index);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00009418 File Offset: 0x00007618
		public void AddPrototypeToTerrains(GameObject pickerGameObject, int overwriteIndex)
		{
			int terrainCount = base.GetTerrainCount();
			for (int i = 0; i < terrainCount; i++)
			{
				GPUITerrain terrain = base.GetTerrain(i);
				if (terrain != null)
				{
					terrain.AddTreePrototypeToTerrain(pickerGameObject, overwriteIndex);
				}
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00009451 File Offset: 0x00007651
		public override void RequireUpdate()
		{
			this._requireUpdate = true;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000945A File Offset: 0x0000765A
		public void RequireUpdate(bool reloadTreeInstances)
		{
			this._reloadTreeInstances = reloadTreeInstances;
			this.RequireUpdate();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009469 File Offset: 0x00007669
		public override GPUIProfile GetDefaultProfile()
		{
			if (this.defaultProfile != null)
			{
				return this.defaultProfile;
			}
			return GPUITerrainConstants.DefaultTreeProfile;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009485 File Offset: 0x00007685
		public override List<string> GetShaderKeywords(int prototypeIndex)
		{
			if (this._enableTreeInstanceColors)
			{
				return GPUITreeManager.TREE_INSTANCE_COLORS_SHADER_KEYWORDS;
			}
			return base.GetShaderKeywords(prototypeIndex);
		}

		// Token: 0x040000CA RID: 202
		[SerializeField]
		internal bool _enableTreeInstanceColors;

		// Token: 0x040000CB RID: 203
		[SerializeField]
		internal bool _autoGenerateBillboards = true;

		// Token: 0x040000CC RID: 204
		[SerializeField]
		internal bool _isPersistentTreeDataBuffer;

		// Token: 0x040000CD RID: 205
		[NonSerialized]
		private bool _requireUpdate;

		// Token: 0x040000CE RID: 206
		[NonSerialized]
		private int[] _treeInstanceCounts;

		// Token: 0x040000CF RID: 207
		[NonSerialized]
		private GPUITransformBufferData[] _treeTransformBuffers;

		// Token: 0x040000D0 RID: 208
		[NonSerialized]
		private int[] _treeTransformBufferStartIndexes;

		// Token: 0x040000D1 RID: 209
		[NonSerialized]
		private GPUIDataBuffer<uint> _counterDataBuffer;

		// Token: 0x040000D2 RID: 210
		[NonSerialized]
		private bool _reloadTreeInstances;

		// Token: 0x040000D3 RID: 211
		[NonSerialized]
		private GraphicsBuffer _treeDataBuffer;

		// Token: 0x040000D4 RID: 212
		[NonSerialized]
		private int _lastTreeDataBufferUpdateFrame;

		// Token: 0x040000D5 RID: 213
		[NonSerialized]
		private List<GPUITreeManager.GPUITreeTerrainData> _batchedTreeTerrainData;

		// Token: 0x040000D6 RID: 214
		[NonSerialized]
		private GraphicsBuffer _batchedTreeTerrainDataBuffer;

		// Token: 0x040000D7 RID: 215
		[NonSerialized]
		private List<GPUITerrain> _batchedTerrains;

		// Token: 0x040000D8 RID: 216
		[NonSerialized]
		private List<GPUITerrain> _unbatchedTerrains;

		// Token: 0x040000D9 RID: 217
		[NonSerialized]
		private uint[] _batchedPrototypeIndexes;

		// Token: 0x040000DA RID: 218
		private const int ERROR_CODE_ADDITION = 500;

		// Token: 0x040000DB RID: 219
		private static readonly List<string> TREE_INSTANCE_COLORS_SHADER_KEYWORDS = new List<string> { GPUITerrainConstants.Kw_GPUI_TREE_INSTANCE_COLOR };

		// Token: 0x040000DC RID: 220
		private static List<int> TERRAIN_TREE_PROTOTYPE_INDEXES = new List<int>();

		// Token: 0x040000DD RID: 221
		private static readonly int TREE_DATA_BUFFER_DISPOSE_FRAME_COUNT = 100;

		// Token: 0x02000017 RID: 23
		public struct GPUITreeTerrainData
		{
			// Token: 0x040000DE RID: 222
			public Vector3 terrainSize;

			// Token: 0x040000DF RID: 223
			public Vector3 terrainPosition;

			// Token: 0x040000E0 RID: 224
			public int treeDataStartIndex;

			// Token: 0x040000E1 RID: 225
			public int treeCount;

			// Token: 0x040000E2 RID: 226
			public Matrix4x4 gpuiTransformOffset;
		}
	}
}
