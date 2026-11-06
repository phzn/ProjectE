using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000029 RID: 41
	public class GPUIRenderSourceGroup : IGPUIDisposable, IDisposable
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000152 RID: 338 RVA: 0x000080B7 File Offset: 0x000062B7
		// (set) Token: 0x06000153 RID: 339 RVA: 0x000080BF File Offset: 0x000062BF
		public int Key { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000080C8 File Offset: 0x000062C8
		// (set) Token: 0x06000155 RID: 341 RVA: 0x000080D0 File Offset: 0x000062D0
		public int GroupID { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000080D9 File Offset: 0x000062D9
		// (set) Token: 0x06000157 RID: 343 RVA: 0x000080E1 File Offset: 0x000062E1
		public int PrototypeKey { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000080EA File Offset: 0x000062EA
		// (set) Token: 0x06000159 RID: 345 RVA: 0x000080F2 File Offset: 0x000062F2
		public GPUIProfile Profile { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000080FB File Offset: 0x000062FB
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00008103 File Offset: 0x00006303
		public List<GPUIRenderSource> RenderSources { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000810C File Offset: 0x0000630C
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00008114 File Offset: 0x00006314
		public string Name { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000811D File Offset: 0x0000631D
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00008125 File Offset: 0x00006325
		public int BufferSize { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000812E File Offset: 0x0000632E
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00008136 File Offset: 0x00006336
		public int InstanceCount { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000813F File Offset: 0x0000633F
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00008147 File Offset: 0x00006347
		public GPUITransformBufferData TransformBufferData { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00008150 File Offset: 0x00006350
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00008158 File Offset: 0x00006358
		public GPUITransformBufferType TransformBufferType { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00008161 File Offset: 0x00006361
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00008169 File Offset: 0x00006369
		public List<string> ShaderKeywords { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00008172 File Offset: 0x00006372
		public GPUILODGroupData LODGroupData
		{
			get
			{
				if (this._lodGroupData == null)
				{
					GPUIRenderingSystem.Instance.LODGroupDataProvider.TryGetData(this.PrototypeKey, out this._lodGroupData);
				}
				return this._lodGroupData;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000081A4 File Offset: 0x000063A4
		public GPUIPrototype Prototype
		{
			get
			{
				GPUILODGroupData lodgroupData = this.LODGroupData;
				if (this.LODGroupData == null)
				{
					return null;
				}
				return lodgroupData.prototype;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000081D0 File Offset: 0x000063D0
		public GPUIRenderSourceGroup(int prototypeKey, GPUIProfile profile, int groupID = 0, GPUITransformBufferType transformBufferType = GPUITransformBufferType.Default, List<string> shaderKeywords = null, GPUILODGroupData lodGroupData = null)
		{
			this.PrototypeKey = prototypeKey;
			this.Profile = profile;
			this.GroupID = groupID;
			this.RenderSources = new List<GPUIRenderSource>();
			this.TransformBufferType = transformBufferType;
			this._lodGroupData = lodGroupData;
			this.ShaderKeywords = new List<string>();
			this.AddShaderKeywords(shaderKeywords);
			this.Key = GPUIRenderSourceGroup.GetKey(prototypeKey, profile, groupID, this.ShaderKeywords);
			if (this.LODGroupData != null)
			{
				this.Name = this._lodGroupData.ToString();
			}
			else
			{
				this.Name = "KEY[" + this.Key.ToString() + "]";
			}
			this._shaderCommandParams = new List<GPUIShaderCommandParams>();
			this._shaderCommandOptionalParams = new Dictionary<int, Vector2>();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008294 File Offset: 0x00006494
		internal void UpdateCommandBuffer(GPUICameraData cameraData)
		{
			if (this.LODGroupData == null)
			{
				return;
			}
			int length = this._lodGroupData.Length;
			GPUIVisibilityData gpuivisibilityData = new GPUIVisibilityData
			{
				additional = 3U
			};
			GPUIProfile profile = this.Profile;
			int length2;
			if (!cameraData.TryGetVisibilityBufferIndex(this, out length2))
			{
				length2 = cameraData._visibilityBuffer.Length;
				cameraData._visibilityBufferIndexes[this.Key] = length2;
				for (int i = 0; i < length * 2; i++)
				{
					cameraData._visibilityBuffer.Add(gpuivisibilityData);
				}
				if (this._lodGroupData.optionalRendererCount > 0)
				{
					for (int j = 0; j < this._lodGroupData.optionalRendererCount * 2; j++)
					{
						cameraData._visibilityBuffer.Add(gpuivisibilityData);
					}
				}
			}
			this._shaderCommandParams.Clear();
			int num = 0;
			for (int k = 0; k < 2; k++)
			{
				for (int l = 0; l < length; l++)
				{
					int num2 = length2 + length * k + l;
					GPUIVisibilityData gpuivisibilityData2 = cameraData._visibilityBuffer[num2];
					GPUILODData gpuiloddata = this._lodGroupData[l];
					if (gpuivisibilityData2.additional > 1U)
					{
						uint length3 = (uint)cameraData._commandBuffer.Length;
						List<GraphicsBuffer.IndirectDrawIndexedArgs> commandBufferArgs = gpuiloddata.GetCommandBufferArgs(profile);
						cameraData._commandBuffer.Add(commandBufferArgs);
						gpuivisibilityData2.commandStartIndex = length3;
						gpuivisibilityData2.commandCount = (uint)commandBufferArgs.Count;
						gpuivisibilityData2.additional = (uint)k;
					}
					gpuiloddata.LoadShaderCommandParams(this._shaderCommandParams, num, l);
					if (k == 0 || profile.isShadowCasting)
					{
						num++;
					}
					cameraData._visibilityBuffer[num2] = gpuivisibilityData2;
				}
			}
			if (this._lodGroupData.optionalRendererCount > 0)
			{
				if (length != 1)
				{
					Debug.LogError(GPUIConstants.LOG_PREFIX + "Optional renderers require lodCount == 1.");
				}
				GPUILODData gpuiloddata2 = this._lodGroupData[0];
				for (int m = 0; m < this._lodGroupData.optionalRendererCount; m++)
				{
					List<GraphicsBuffer.IndirectDrawIndexedArgs> optionalRendererCommandBufferArgs = gpuiloddata2.GetOptionalRendererCommandBufferArgs(m + 1, profile);
					for (int n = 0; n < 2; n++)
					{
						int num3 = length2 + 2 + n + m * 2;
						GPUIVisibilityData gpuivisibilityData3 = cameraData._visibilityBuffer[num3];
						if (gpuivisibilityData3.additional > 1U)
						{
							uint length4 = (uint)cameraData._commandBuffer.Length;
							cameraData._commandBuffer.Add(optionalRendererCommandBufferArgs);
							gpuivisibilityData3.commandStartIndex = length4;
							gpuivisibilityData3.commandCount = (uint)optionalRendererCommandBufferArgs.Count;
							gpuivisibilityData3.additional = (uint)n;
						}
						gpuiloddata2.LoadShaderCommandParamsForOptionalRenderers(this._shaderCommandParams, num, m + 1);
						if (n == 0 || profile.isShadowCasting)
						{
							num++;
						}
						cameraData._visibilityBuffer[num3] = gpuivisibilityData3;
					}
				}
			}
			this.CreateShaderCommandParamsArray();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000854C File Offset: 0x0000674C
		private void CreateShaderCommandParamsArray()
		{
			int num = 5;
			this._shaderCommandParamsArray = new Vector4[this._shaderCommandParams.Count * num];
			Matrix4x4 identity_Matrix4x = GPUIConstants.IDENTITY_Matrix4x4;
			for (int i = 0; i < this._shaderCommandParams.Count; i++)
			{
				GPUIShaderCommandParams gpuishaderCommandParams = this._shaderCommandParams[i];
				Vector2 zero;
				if (!this._shaderCommandOptionalParams.TryGetValue(gpuishaderCommandParams.key, out zero))
				{
					zero = Vector2.zero;
				}
				this._shaderCommandParamsArray[i * num] = new Vector4((float)gpuishaderCommandParams.instanceDataBufferShiftMultiplier, (!gpuishaderCommandParams.transformOffset.EqualsMatrix4x4(identity_Matrix4x)) ? 1f : 0f, zero.x, zero.y);
				this._shaderCommandParamsArray[i * num + 1] = gpuishaderCommandParams.transformOffset.GetRow(0);
				this._shaderCommandParamsArray[i * num + 2] = gpuishaderCommandParams.transformOffset.GetRow(1);
				this._shaderCommandParamsArray[i * num + 3] = gpuishaderCommandParams.transformOffset.GetRow(2);
				this._shaderCommandParamsArray[i * num + 4] = gpuishaderCommandParams.transformOffset.GetRow(3);
			}
			this._requireShaderCommandParamsUpdate = true;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000867C File Offset: 0x0000687C
		internal void SetBufferSize(GPUIRenderSource renderSource, int renderSourceBufferSize, bool isCopyPreviousData)
		{
			if (renderSource.bufferSize == renderSourceBufferSize)
			{
				return;
			}
			int bufferSize = renderSource.bufferSize;
			renderSource.bufferSize = renderSourceBufferSize;
			if (renderSource.instanceCount > renderSourceBufferSize)
			{
				renderSource.instanceCount = renderSourceBufferSize;
				this.UpdateInstanceCount();
			}
			renderSource.bufferStartIndex = 0;
			this.BufferSize = 0;
			if (this.RenderSources.Count > 1)
			{
				foreach (GPUIRenderSource gpuirenderSource in this.RenderSources)
				{
					gpuirenderSource.bufferStartIndex = this.BufferSize;
					this.BufferSize += gpuirenderSource.bufferSize;
				}
				GPUIShaderBuffer gpuishaderBuffer = null;
				GPUIShaderBuffer gpuishaderBuffer2 = null;
				if (this.TransformBufferData == null)
				{
					this.TransformBufferData = new GPUITransformBufferData(this);
					isCopyPreviousData = false;
				}
				else
				{
					isCopyPreviousData |= this.TransformBufferData.ResizeTransformBuffer(out gpuishaderBuffer2, out gpuishaderBuffer);
				}
				if (isCopyPreviousData)
				{
					this.CopyTransformBufferData(gpuishaderBuffer2, gpuishaderBuffer, 0, 0, renderSource.bufferStartIndex);
					this.CopyTransformBufferData(gpuishaderBuffer2, gpuishaderBuffer, renderSource.bufferStartIndex + bufferSize, renderSource.bufferStartIndex + renderSource.bufferSize, this.BufferSize - renderSource.bufferStartIndex - renderSource.bufferSize);
				}
				if (gpuishaderBuffer2 != null)
				{
					gpuishaderBuffer2.Dispose();
				}
			}
			else
			{
				this.BufferSize = renderSource.bufferSize;
				if (this.TransformBufferData == null)
				{
					this.TransformBufferData = new GPUITransformBufferData(this);
				}
				else
				{
					this.TransformBufferData.ResizeTransformBuffer(isCopyPreviousData);
				}
			}
			if (this.BufferSize == 0)
			{
				this.ReleaseBuffers();
			}
			else
			{
				GPUIRenderingSystem.Instance.UpdateCommandBuffers(this);
			}
			GPUIRenderingSystem.Instance.OnRenderSourceGroupBufferSizeChanged(this);
			GPUIRenderingSystem.Instance.OnRenderSourceBufferSizeChanged(renderSource, bufferSize);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008818 File Offset: 0x00006A18
		internal void CopyTransformBufferData(GPUIShaderBuffer managedBuffer, GPUIShaderBuffer transformBuffer, int managedBufferStartIndex, int graphicsBufferStartIndex, int count)
		{
			if (managedBuffer == null || transformBuffer == null || this.TransformBufferData.IsCameraBasedBuffer || count <= 0)
			{
				return;
			}
			transformBuffer.Buffer.SetData(managedBuffer.Buffer, managedBufferStartIndex, graphicsBufferStartIndex, count);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008849 File Offset: 0x00006A49
		internal void SetInstanceCount(GPUIRenderSource renderSource, int renderSourceInstanceCount)
		{
			renderSource.instanceCount = renderSourceInstanceCount;
			this.UpdateInstanceCount();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008858 File Offset: 0x00006A58
		private void UpdateInstanceCount()
		{
			this.InstanceCount = 0;
			foreach (GPUIRenderSource gpuirenderSource in this.RenderSources)
			{
				this.InstanceCount += gpuirenderSource.instanceCount;
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000088C0 File Offset: 0x00006AC0
		internal void SetTransformBufferData<[IsUnmanaged] T>(GPUIRenderSource renderSource, NativeArray<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer) where T : struct, ValueType
		{
			if (count <= 0)
			{
				return;
			}
			int num = graphicsBufferStartIndex + count;
			bool flag = graphicsBufferStartIndex != 0 || count < this.InstanceCount;
			if (renderSource.bufferSize < num)
			{
				this.SetBufferSize(renderSource, num, flag);
			}
			this.TransformBufferData.SetTransformBufferData<T>(matrices, managedBufferStartIndex, renderSource.bufferStartIndex + graphicsBufferStartIndex, count, isOverwritePreviousFrameBuffer);
			if (renderSource.instanceCount < count)
			{
				this.SetInstanceCount(renderSource, count);
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000892C File Offset: 0x00006B2C
		internal void SetTransformBufferData<[IsUnmanaged] T>(GPUIRenderSource renderSource, T[] matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer) where T : struct, ValueType
		{
			if (count <= 0)
			{
				return;
			}
			int num = graphicsBufferStartIndex + count;
			bool flag = graphicsBufferStartIndex != 0 || count < this.InstanceCount;
			if (renderSource.bufferSize < num)
			{
				this.SetBufferSize(renderSource, num, flag);
			}
			this.TransformBufferData.SetTransformBufferData<T>(matrices, managedBufferStartIndex, renderSource.bufferStartIndex + graphicsBufferStartIndex, count, isOverwritePreviousFrameBuffer);
			if (renderSource.instanceCount < count)
			{
				this.SetInstanceCount(renderSource, count);
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008998 File Offset: 0x00006B98
		internal void SetTransformBufferData<[IsUnmanaged] T>(GPUIRenderSource renderSource, List<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer) where T : struct, ValueType
		{
			if (count <= 0)
			{
				return;
			}
			int num = graphicsBufferStartIndex + count;
			bool flag = graphicsBufferStartIndex != 0 || count < this.InstanceCount;
			if (renderSource.bufferSize < num)
			{
				this.SetBufferSize(renderSource, num, flag);
			}
			this.TransformBufferData.SetTransformBufferData<T>(matrices, managedBufferStartIndex, renderSource.bufferStartIndex + graphicsBufferStartIndex, count, isOverwritePreviousFrameBuffer);
			if (renderSource.instanceCount < count)
			{
				this.SetInstanceCount(renderSource, count);
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008A02 File Offset: 0x00006C02
		internal void UpdateTransformBufferData(int frameNo)
		{
			GPUITransformBufferData transformBufferData = this.TransformBufferData;
			if (transformBufferData == null)
			{
				return;
			}
			transformBufferData.UpdateData(frameNo);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008A18 File Offset: 0x00006C18
		private void RemoveRenderSource(GPUIRenderSource renderSource)
		{
			int num = this.RenderSources.IndexOf(renderSource);
			if (num < 0)
			{
				return;
			}
			this.RenderSources.RemoveAt(num);
			if (renderSource.bufferSize == 0)
			{
				return;
			}
			this.BufferSize = 0;
			foreach (GPUIRenderSource gpuirenderSource in this.RenderSources)
			{
				gpuirenderSource.bufferStartIndex = this.BufferSize;
				this.BufferSize += gpuirenderSource.bufferSize;
			}
			this.UpdateInstanceCount();
			this.TransformBufferData.RemoveIndexes(renderSource.bufferStartIndex, renderSource.bufferSize);
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem.Instance.UpdateCommandBuffers(this);
			}
			GPUIRenderingSystem.Instance.OnRenderSourceGroupBufferSizeChanged(this);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00008AEC File Offset: 0x00006CEC
		internal void Dispose(GPUIRenderSource renderSource)
		{
			if (this.RenderSources == null)
			{
				return;
			}
			if (!this.RenderSources.Contains(renderSource))
			{
				return;
			}
			if (this.RenderSources.Count == 1)
			{
				this.Dispose();
				return;
			}
			this.RemoveRenderSource(renderSource);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008B24 File Offset: 0x00006D24
		public void Dispose()
		{
			this.ReleaseBuffers();
			this.BufferSize = 0;
			if (this.RenderSources != null)
			{
				foreach (GPUIRenderSource gpuirenderSource in this.RenderSources)
				{
					if (gpuirenderSource != null)
					{
						gpuirenderSource.DisposeRenderSource();
					}
				}
			}
			this.RenderSources = null;
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem.Instance.RenderSourceGroupProvider.Remove(this.Key);
			}
			if (this._dependentDisposables != null)
			{
				foreach (IGPUIDisposable igpuidisposable in this._dependentDisposables)
				{
					if (igpuidisposable != null)
					{
						igpuidisposable.Dispose();
					}
				}
				this._dependentDisposables = null;
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008C08 File Offset: 0x00006E08
		public void ReleaseBuffers()
		{
			if (this.TransformBufferData != null)
			{
				this.TransformBufferData.Dispose();
				this.TransformBufferData = null;
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008C24 File Offset: 0x00006E24
		internal bool AddRenderSource(GPUIRenderSource renderSource)
		{
			if (this.RenderSources.Exists((GPUIRenderSource rs) => rs.Key == renderSource.Key))
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					GPUIConstants.LOG_PREFIX,
					"Renderer already registered for: ",
					this.Name,
					" with Key:",
					renderSource.Key.ToString()
				}), renderSource.source);
				return false;
			}
			this.RenderSources.Add(renderSource);
			return true;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008CBD File Offset: 0x00006EBD
		internal void AddDependentDisposable(IGPUIDisposable gpuiDisposable)
		{
			if (this._dependentDisposables == null)
			{
				this._dependentDisposables = new List<IGPUIDisposable>();
			}
			if (!this._dependentDisposables.Contains(gpuiDisposable))
			{
				this._dependentDisposables.Add(gpuiDisposable);
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008CEC File Offset: 0x00006EEC
		private void CreateMaterialPropertyBlock()
		{
			if (this._mpb == null)
			{
				this._mpb = new MaterialPropertyBlock();
				this.ResetMaterialPropertyBlock();
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008D07 File Offset: 0x00006F07
		public MaterialPropertyBlock GetMaterialPropertyBlock()
		{
			this.CreateMaterialPropertyBlock();
			return this._mpb;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00008D18 File Offset: 0x00006F18
		private void ResetMaterialPropertyBlock()
		{
			this._mpb.Clear();
			this._mpb.SetVector(GPUIConstants.PROP_unity_LODFade, new Vector4(1f, 16f, 0f, 0f));
			if (this._materialPropertyOverrides != null)
			{
				this._materialPropertyOverrides.ApplyDirectOverrides(this._mpb);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00008D72 File Offset: 0x00006F72
		internal MaterialPropertyBlock GetMaterialPropertyBlock(GPUILODGroupData lgd)
		{
			this.CreateMaterialPropertyBlock();
			if (Application.isPlaying && lgd.requiresTreeProxy)
			{
				GPUIRenderingSystem.Instance.TreeProxyProvider.GetMaterialPropertyBlock(lgd, this._mpb);
			}
			return this._mpb;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00008DA5 File Offset: 0x00006FA5
		internal void ApplyMaterialPropertyOverrides(MaterialPropertyBlock mpb, int lodIndex, int rendererIndex)
		{
			if (this._materialPropertyOverrides != null)
			{
				this._materialPropertyOverrides.ApplyOverrides(mpb, lodIndex, rendererIndex);
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008DBD File Offset: 0x00006FBD
		public void AddMaterialPropertyOverride(string propertyName, object value, int lodIndex = -1, int rendererIndex = -1, bool isPersistent = false)
		{
			this.AddMaterialPropertyOverride(Shader.PropertyToID(propertyName), value, lodIndex, rendererIndex, isPersistent);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008DD4 File Offset: 0x00006FD4
		public void AddMaterialPropertyOverride(int nameID, object value, int lodIndex = -1, int rendererIndex = -1, bool isPersistent = false)
		{
			bool flag = false;
			GPUILODGroupData lodgroupData = this.LODGroupData;
			if (isPersistent && lodgroupData != null && !lodgroupData.requiresTreeProxy && lodIndex < 0 && rendererIndex < 0)
			{
				this.CreateMaterialPropertyBlock();
				this._mpb.SetValue(nameID, value);
				flag = true;
			}
			if (this._materialPropertyOverrides == null)
			{
				this._materialPropertyOverrides = new GPUIMaterialPropertyOverrides();
			}
			this._materialPropertyOverrides.AddOverride(lodIndex, rendererIndex, nameID, value, isPersistent, flag);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008E43 File Offset: 0x00007043
		public void RemoveMaterialPropertyOverrides(string propertyName)
		{
			this.RemoveMaterialPropertyOverrides(Shader.PropertyToID(propertyName));
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008E51 File Offset: 0x00007051
		public void RemoveMaterialPropertyOverrides(int nameID)
		{
			this.ResetMaterialPropertyBlock();
			if (this._materialPropertyOverrides != null)
			{
				this._materialPropertyOverrides.RemoveMaterialPropertyOverrides(nameID);
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008E6D File Offset: 0x0000706D
		public void ClearMaterialPropertyOverrides()
		{
			this.ResetMaterialPropertyBlock();
			if (this._materialPropertyOverrides != null)
			{
				this._materialPropertyOverrides.ClearOverrides();
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008E88 File Offset: 0x00007088
		public bool AddShaderKeyword(string keyword)
		{
			if (string.IsNullOrEmpty(keyword))
			{
				return false;
			}
			if (!this.ShaderKeywords.Contains(keyword))
			{
				this.ShaderKeywords.Add(keyword);
				return true;
			}
			return false;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008EB1 File Offset: 0x000070B1
		public bool RemoveShaderKeyword(string keyword)
		{
			return !string.IsNullOrEmpty(keyword) && this.ShaderKeywords.Remove(keyword);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008ECC File Offset: 0x000070CC
		public void RemoveReplacementMaterials()
		{
			GPUILODGroupData lodgroupData = this.LODGroupData;
			if (lodgroupData == null)
			{
				return;
			}
			lodgroupData.RemoveReplacementMaterials();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008EF0 File Offset: 0x000070F0
		private void AddShaderKeywords(IEnumerable<string> keywords)
		{
			if (keywords == null)
			{
				return;
			}
			foreach (string text in keywords)
			{
				this.AddShaderKeyword(text);
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008F40 File Offset: 0x00007140
		public void SetCommandShaderOptionalParams(int lodNo, int rendererNo, Vector2 optionalParams)
		{
			int num = lodNo + 10 * rendererNo;
			this._shaderCommandOptionalParams[num] = optionalParams;
			this.CreateShaderCommandParamsArray();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008F68 File Offset: 0x00007168
		public static int GetKey(int prototypeKey, GPUIProfile profile, int groupID, List<string> shaderKeywords)
		{
			if (shaderKeywords == null || shaderKeywords.Count == 0)
			{
				return GPUIUtility.GenerateHash(new int[]
				{
					prototypeKey,
					profile.GetInstanceIDGPUI(),
					groupID
				});
			}
			shaderKeywords.Sort();
			return GPUIUtility.GenerateHash(new int[]
			{
				prototypeKey,
				profile.GetInstanceIDGPUI(),
				groupID,
				string.Concat(shaderKeywords).GetHashCode()
			});
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008FCD File Offset: 0x000071CD
		public int GetRenderSourceKey(Object source)
		{
			return GPUIUtility.GenerateHash(new int[]
			{
				source.GetInstanceIDGPUI(),
				this.Key
			});
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008FEC File Offset: 0x000071EC
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00008FF4 File Offset: 0x000071F4
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00008FFC File Offset: 0x000071FC
		public bool IsLODColorDebuggingEnabled { get; private set; }

		// Token: 0x0600018F RID: 399 RVA: 0x00009008 File Offset: 0x00007208
		public void SetLODColorDebuggingEnabled(bool enabled, string colorPropertyName = null)
		{
			if (enabled)
			{
				this.IsLODColorDebuggingEnabled = true;
				for (int i = 0; i < GPUIRenderSourceGroup.materialColors.Length; i++)
				{
					if (!string.IsNullOrEmpty(colorPropertyName))
					{
						this.AddMaterialPropertyOverride(colorPropertyName, GPUIRenderSourceGroup.materialColors[i], i, -1, false);
					}
					else
					{
						for (int j = 0; j < GPUIRenderSourceGroup._colorPropertyNames.Length; j++)
						{
							this.AddMaterialPropertyOverride(GPUIRenderSourceGroup._colorPropertyNames[j], GPUIRenderSourceGroup.materialColors[i], i, -1, false);
						}
					}
				}
				return;
			}
			this.IsLODColorDebuggingEnabled = false;
			if (!string.IsNullOrEmpty(colorPropertyName))
			{
				this.RemoveMaterialPropertyOverrides(colorPropertyName);
				return;
			}
			for (int k = 0; k < GPUIRenderSourceGroup._colorPropertyNames.Length; k++)
			{
				this.RemoveMaterialPropertyOverrides(GPUIRenderSourceGroup._colorPropertyNames[k]);
			}
		}

		// Token: 0x040000F6 RID: 246
		private MaterialPropertyBlock _mpb;

		// Token: 0x040000F7 RID: 247
		private GPUIMaterialPropertyOverrides _materialPropertyOverrides;

		// Token: 0x040000F8 RID: 248
		private List<IGPUIDisposable> _dependentDisposables;

		// Token: 0x040000F9 RID: 249
		private List<GPUIShaderCommandParams> _shaderCommandParams;

		// Token: 0x040000FA RID: 250
		internal Vector4[] _shaderCommandParamsArray;

		// Token: 0x040000FB RID: 251
		private Dictionary<int, Vector2> _shaderCommandOptionalParams;

		// Token: 0x040000FC RID: 252
		internal bool _requireShaderCommandParamsUpdate;

		// Token: 0x040000FD RID: 253
		private GPUILODGroupData _lodGroupData;

		// Token: 0x040000FF RID: 255
		private static readonly Color[] materialColors = new Color[]
		{
			new Color(1f, 0f, 0f, 1f),
			new Color(0f, 0f, 1f, 1f),
			new Color(1f, 1f, 0f, 1f),
			new Color(1f, 0.5f, 0f, 1f),
			new Color(0f, 1f, 1f, 1f),
			new Color(0.5f, 0f, 1f, 1f),
			new Color(1f, 0f, 1f, 1f),
			new Color(0f, 1f, 0f, 1f)
		};

		// Token: 0x04000100 RID: 256
		private static readonly string[] _colorPropertyNames = new string[] { "_Color", "_BaseColor", "_HealthyColor", "_DryColor" };
	}
}
