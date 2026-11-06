using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000036 RID: 54
	public class GPUITransformBufferData : IGPUIDisposable, IDisposable
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A4AF File Offset: 0x000086AF
		public GPUIRenderSourceGroup RenderSourceGroup
		{
			get
			{
				return this._renderSourceGroup;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000A4B7 File Offset: 0x000086B7
		public Dictionary<int, GPUIShaderBuffer>.ValueCollection TransformBufferValues
		{
			get
			{
				if (this._transformBufferDict != null)
				{
					return this._transformBufferDict.Values;
				}
				return null;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000A4CE File Offset: 0x000086CE
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000A4D6 File Offset: 0x000086D6
		public GraphicsBuffer PreviousFrameTransformBuffer { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A4DF File Offset: 0x000086DF
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000A4E8 File Offset: 0x000086E8
		public bool HasPreviousFrameTransformBuffer
		{
			get
			{
				return this._hasPreviousFrameTransformBuffer;
			}
			private set
			{
				if (this._hasPreviousFrameTransformBuffer != value)
				{
					this._hasPreviousFrameTransformBuffer = value;
					if (!GPUIShaderBindings.Instance.stripObjectMotionVectorVariants)
					{
						if (value)
						{
							if (this._renderSourceGroup.AddShaderKeyword("GPUI_OBJECT_MOTION_VECTOR_ON"))
							{
								this._renderSourceGroup.RemoveReplacementMaterials();
								return;
							}
						}
						else if (this._renderSourceGroup.RemoveShaderKeyword("GPUI_OBJECT_MOTION_VECTOR_ON"))
						{
							this._renderSourceGroup.RemoveReplacementMaterials();
							return;
						}
					}
					else if (value)
					{
						Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not generate Per Object Motion Vector data. Disabled by the Editor Settings.");
						this._hasPreviousFrameTransformBuffer = false;
					}
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000A56E File Offset: 0x0000876E
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000A578 File Offset: 0x00008778
		public bool HasPerInstanceLightProbes
		{
			get
			{
				return this._hasPerInstanceLightProbes;
			}
			private set
			{
				if (this._hasPerInstanceLightProbes != value)
				{
					this._hasPerInstanceLightProbes = value;
					if (!GPUIShaderBindings.Instance.stripPerInstanceLightProbeVariants)
					{
						if (value)
						{
							if (this._renderSourceGroup.AddShaderKeyword("GPUI_PER_INSTANCE_LIGHTPROBES_ON"))
							{
								this._renderSourceGroup.RemoveReplacementMaterials();
								return;
							}
						}
						else if (this._renderSourceGroup.RemoveShaderKeyword("GPUI_PER_INSTANCE_LIGHTPROBES_ON"))
						{
							this._renderSourceGroup.RemoveReplacementMaterials();
							return;
						}
					}
					else if (value)
					{
						Debug.LogError(GPUIConstants.LOG_PREFIX + "Can not generate Per Instance Light Probe data. Disabled by the Editor Settings.");
						this._hasPerInstanceLightProbes = false;
					}
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000A5FE File Offset: 0x000087FE
		public bool IsCameraBasedBuffer
		{
			get
			{
				return this._renderSourceGroup.TransformBufferType == GPUITransformBufferType.CameraBased;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000A60E File Offset: 0x0000880E
		public bool IsGeneratePerInstanceLightProbes
		{
			get
			{
				return this._isAllowPerInstanceLightProbes && this._renderSourceGroup.Profile.lightProbeSetting == GPUILightProbeSetting.PerInstance;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000A62D File Offset: 0x0000882D
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000A635 File Offset: 0x00008835
		public bool HasInstancingBounds { get; internal set; }

		// Token: 0x06000204 RID: 516 RVA: 0x0000A640 File Offset: 0x00008840
		public GPUITransformBufferData(GPUIRenderSourceGroup renderSourceGroup)
		{
			this._renderSourceGroup = renderSourceGroup;
			if (Application.isPlaying && !this.IsCameraBasedBuffer && !GPUIRuntimeSettings.Instance.DisablePreviousFrameTransformBuffer && !GPUIRuntimeSettings.Instance.IsBuiltInRP && renderSourceGroup.LODGroupData != null && renderSourceGroup.Profile != null && renderSourceGroup.Profile.enablePerObjectMotionVectors && !GPUIShaderBindings.Instance.stripObjectMotionVectorVariants && renderSourceGroup.LODGroupData.HasObjectMotion())
			{
				this.HasPreviousFrameTransformBuffer = true;
				this._previousFrameBufferFrameNo = -1;
			}
			this._isAllowPerInstanceLightProbes = Application.isPlaying && !this.IsCameraBasedBuffer && !GPUIRuntimeSettings.Instance.DisablePerInstanceLightProbesBuffer && !GPUIRuntimeSettings.IsAdaptiveProbeVolumesEnabled() && !GPUIShaderBindings.Instance.stripPerInstanceLightProbeVariants;
			this.HasInstancingBounds = false;
			this._instancingBoundsIndex = -1;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A71E File Offset: 0x0000891E
		public void ReleaseBuffers()
		{
			this.ReleaseTransformBuffers();
			this.ReleaseInstanceDataBuffers();
			this.ReleaseOptionalRendererBuffers();
			this.ReleaseLightProbeBuffers();
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A738 File Offset: 0x00008938
		internal void ReleaseTransformBuffers()
		{
			if (this._transformBufferDict != null)
			{
				foreach (GPUIShaderBuffer gpuishaderBuffer in this._transformBufferDict.Values)
				{
					if (gpuishaderBuffer != null)
					{
						gpuishaderBuffer.Dispose();
					}
				}
				this._transformBufferDict = null;
			}
			if (this.PreviousFrameTransformBuffer != null)
			{
				this.PreviousFrameTransformBuffer.Dispose();
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A7B4 File Offset: 0x000089B4
		internal void ReleaseInstanceDataBuffers()
		{
			if (this._instanceDataBufferDict != null)
			{
				foreach (GPUIShaderBuffer gpuishaderBuffer in this._instanceDataBufferDict.Values)
				{
					if (gpuishaderBuffer != null)
					{
						gpuishaderBuffer.Dispose();
					}
				}
				this._instanceDataBufferDict = null;
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000A820 File Offset: 0x00008A20
		internal void ReleaseOptionalRendererBuffers()
		{
			if (this._optionalRendererStatusBuffer != null)
			{
				this._optionalRendererStatusBuffer.Dispose();
				this._optionalRendererStatusBuffer = null;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000A83C File Offset: 0x00008A3C
		internal void ReleaseLightProbeBuffers()
		{
			this.HasPerInstanceLightProbes = false;
			if (this._perInstanceLightProbesBuffer != null)
			{
				this._perInstanceLightProbesBuffer.Dispose();
				this._perInstanceLightProbesBuffer = null;
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000A860 File Offset: 0x00008A60
		internal void ReleaseInstanceDataBuffers(GPUICameraData cameraData)
		{
			int instanceIDGPUI = cameraData.ActiveCamera.GetInstanceIDGPUI();
			GPUIShaderBuffer gpuishaderBuffer;
			if (this._instanceDataBufferDict != null && this._instanceDataBufferDict.TryGetValue(instanceIDGPUI, out gpuishaderBuffer))
			{
				if (gpuishaderBuffer != null)
				{
					gpuishaderBuffer.Dispose();
				}
				this._instanceDataBufferDict.Remove(instanceIDGPUI);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000A8A7 File Offset: 0x00008AA7
		public void Dispose()
		{
			this.ReleaseBuffers();
			this._transformBufferDict = null;
			this._instanceDataBufferDict = null;
			this.HasInstancingBounds = false;
			this._instancingBoundsIndex = -1;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000A8CC File Offset: 0x00008ACC
		internal void Dispose(GPUICameraData cameraData)
		{
			if (this.IsCameraBasedBuffer && cameraData != null && cameraData.ActiveCamera != null)
			{
				int instanceIDGPUI = cameraData.ActiveCamera.GetInstanceIDGPUI();
				GPUIShaderBuffer gpuishaderBuffer;
				if (this._transformBufferDict != null && this._transformBufferDict.TryGetValue(instanceIDGPUI, out gpuishaderBuffer))
				{
					gpuishaderBuffer.Dispose();
					this._transformBufferDict.Remove(instanceIDGPUI);
				}
				GPUIShaderBuffer gpuishaderBuffer2;
				if (this._instanceDataBufferDict != null && this._instanceDataBufferDict.TryGetValue(instanceIDGPUI, out gpuishaderBuffer2))
				{
					gpuishaderBuffer2.Dispose();
					this._instanceDataBufferDict.Remove(instanceIDGPUI);
				}
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000A954 File Offset: 0x00008B54
		internal void ResizeTransformBuffer(bool isCopyPreviousData)
		{
			if (this.IsCameraBasedBuffer)
			{
				this.Dispose();
				return;
			}
			if (this._transformBufferDict == null)
			{
				this._transformBufferDict = new Dictionary<int, GPUIShaderBuffer>();
			}
			GPUIShaderBuffer gpuishaderBuffer;
			this._transformBufferDict.TryGetValue(0, out gpuishaderBuffer);
			bool flag = gpuishaderBuffer != null;
			int num = (flag ? gpuishaderBuffer.BufferSize : 0);
			if (!flag || num != this._renderSourceGroup.BufferSize)
			{
				GPUIShaderBuffer gpuishaderBuffer2 = this.CreateTransformBuffer();
				this._transformBufferDict[0] = gpuishaderBuffer2;
				if (gpuishaderBuffer != null)
				{
					if (isCopyPreviousData)
					{
						gpuishaderBuffer2.Buffer.SetData(gpuishaderBuffer.Buffer, 0, 0, Math.Min(this._renderSourceGroup.BufferSize, num));
					}
					gpuishaderBuffer.Dispose();
				}
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A9F8 File Offset: 0x00008BF8
		internal bool ResizeTransformBuffer(out GPUIShaderBuffer previousTransformBuffer, out GPUIShaderBuffer transformBuffer)
		{
			previousTransformBuffer = null;
			transformBuffer = null;
			if (this.IsCameraBasedBuffer)
			{
				this.Dispose();
				return false;
			}
			if (this._transformBufferDict == null)
			{
				this._transformBufferDict = new Dictionary<int, GPUIShaderBuffer>();
			}
			this._transformBufferDict.TryGetValue(0, out previousTransformBuffer);
			bool flag = previousTransformBuffer != null;
			int num = (flag ? previousTransformBuffer.BufferSize : 0);
			if (!flag || num != this._renderSourceGroup.BufferSize)
			{
				transformBuffer = this.CreateTransformBuffer();
				this._transformBufferDict[0] = transformBuffer;
				return true;
			}
			return false;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000AA78 File Offset: 0x00008C78
		public void CalculateInterpolatedLightAndOcclusionProbes<[IsUnmanaged] T>(NativeArray<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct, ValueType
		{
			if (this.IsGeneratePerInstanceLightProbes)
			{
				this.HasPerInstanceLightProbes = true;
				Vector3 vector = this._renderSourceGroup.Profile.lightProbePositionOffset;
				vector += this._renderSourceGroup.LODGroupData.bounds.center;
				GPUIRenderingSystem.Instance.CalculateInterpolatedLightAndOcclusionProbes(this, matrices.GetUnsafeReadOnlyPtr<T>(), managedBufferStartIndex, graphicsBufferStartIndex, count, this._renderSourceGroup.BufferSize, vector);
				return;
			}
			if (this.HasPerInstanceLightProbes)
			{
				this.ReleaseLightProbeBuffers();
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000AAF4 File Offset: 0x00008CF4
		public unsafe void CalculateInterpolatedLightAndOcclusionProbes<[IsUnmanaged] T>(T[] matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct, ValueType
		{
			if (this.IsGeneratePerInstanceLightProbes)
			{
				this.HasPerInstanceLightProbes = true;
				Vector3 vector = this._renderSourceGroup.Profile.lightProbePositionOffset;
				vector += this._renderSourceGroup.LODGroupData.bounds.center;
				fixed (T[] array = matrices)
				{
					void* ptr;
					if (matrices == null || array.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = (void*)(&array[0]);
					}
					GPUIRenderingSystem.Instance.CalculateInterpolatedLightAndOcclusionProbes(this, ptr, managedBufferStartIndex, graphicsBufferStartIndex, count, this._renderSourceGroup.BufferSize, vector);
				}
				return;
			}
			if (this.HasPerInstanceLightProbes)
			{
				this.ReleaseLightProbeBuffers();
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000AB84 File Offset: 0x00008D84
		public unsafe void CalculateInterpolatedLightAndOcclusionProbes<[IsUnmanaged] T>(List<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct, ValueType
		{
			if (this.IsGeneratePerInstanceLightProbes)
			{
				this.HasPerInstanceLightProbes = true;
				Vector3 vector = this._renderSourceGroup.Profile.lightProbePositionOffset;
				vector += this._renderSourceGroup.LODGroupData.bounds.center;
				T[] array;
				void* ptr;
				if ((array = GPUIUtility.GetListInternalArray<T>(matrices)) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = (void*)(&array[0]);
				}
				GPUIRenderingSystem.Instance.CalculateInterpolatedLightAndOcclusionProbes(this, ptr, managedBufferStartIndex, graphicsBufferStartIndex, count, this._renderSourceGroup.BufferSize, vector);
				array = null;
				return;
			}
			if (this.HasPerInstanceLightProbes)
			{
				this.ReleaseLightProbeBuffers();
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000AC18 File Offset: 0x00008E18
		internal void SetTransformBufferData<[IsUnmanaged] T>(NativeArray<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer) where T : struct, ValueType
		{
			this.GetTransformBuffer().Buffer.SetData<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, count);
			if (isOverwritePreviousFrameBuffer && this.HasPreviousFrameTransformBuffer && this.PreviousFrameTransformBuffer != null && graphicsBufferStartIndex < this.PreviousFrameTransformBuffer.count)
			{
				this.PreviousFrameTransformBuffer.SetData<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, Math.Min(count, this.PreviousFrameTransformBuffer.count - graphicsBufferStartIndex));
			}
			this.CalculateInterpolatedLightAndOcclusionProbes<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, count);
			this.OnTransformDataModified();
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000AC90 File Offset: 0x00008E90
		internal void SetTransformBufferData<[IsUnmanaged] T>(T[] matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer) where T : struct, ValueType
		{
			this.GetTransformBuffer().Buffer.SetData(matrices, managedBufferStartIndex, graphicsBufferStartIndex, count);
			if (isOverwritePreviousFrameBuffer && this.HasPreviousFrameTransformBuffer && this.PreviousFrameTransformBuffer != null && graphicsBufferStartIndex < this.PreviousFrameTransformBuffer.count)
			{
				this.PreviousFrameTransformBuffer.SetData(matrices, managedBufferStartIndex, graphicsBufferStartIndex, Math.Min(count, this.PreviousFrameTransformBuffer.count - graphicsBufferStartIndex));
			}
			this.CalculateInterpolatedLightAndOcclusionProbes<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, count);
			this.OnTransformDataModified();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000AD08 File Offset: 0x00008F08
		internal void SetTransformBufferData<[IsUnmanaged] T>(List<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer) where T : struct, ValueType
		{
			this.GetTransformBuffer().Buffer.SetData<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, count);
			if (isOverwritePreviousFrameBuffer && this.HasPreviousFrameTransformBuffer && this.PreviousFrameTransformBuffer != null && graphicsBufferStartIndex < this.PreviousFrameTransformBuffer.count)
			{
				this.PreviousFrameTransformBuffer.SetData<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, Math.Min(count, this.PreviousFrameTransformBuffer.count - graphicsBufferStartIndex));
			}
			this.CalculateInterpolatedLightAndOcclusionProbes<T>(matrices, managedBufferStartIndex, graphicsBufferStartIndex, count);
			this.OnTransformDataModified();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000AD80 File Offset: 0x00008F80
		internal void RemoveIndexes(int startIndex, int count)
		{
			if (this.IsCameraBasedBuffer)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "RemoveIndexes method can not be used with Camera Based transform buffers.");
				return;
			}
			GPUIShaderBuffer gpuishaderBuffer = this.CreateTransformBuffer();
			GPUIShaderBuffer gpuishaderBuffer2;
			if (this._transformBufferDict.TryGetValue(0, out gpuishaderBuffer2))
			{
				gpuishaderBuffer.Buffer.SetData(gpuishaderBuffer2.Buffer, 0, 0, startIndex);
				gpuishaderBuffer.Buffer.SetData(gpuishaderBuffer2.Buffer, startIndex + count, startIndex, this._renderSourceGroup.BufferSize - startIndex);
				gpuishaderBuffer2.Dispose();
			}
			this._transformBufferDict[0] = gpuishaderBuffer;
			this.OnTransformDataModified();
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000AE10 File Offset: 0x00009010
		private GPUIShaderBuffer CreateTransformBuffer()
		{
			this.ReleaseInstanceDataBuffers();
			return new GPUIShaderBuffer(this._renderSourceGroup.BufferSize, 64);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000AE2A File Offset: 0x0000902A
		private GPUIShaderBuffer CreateInstanceDataBuffer(int instanceDataBufferSize)
		{
			this.resetCrossFadeDataFrame = Time.frameCount;
			GPUIShaderBuffer gpuishaderBuffer = new GPUIShaderBuffer(instanceDataBufferSize, 16);
			gpuishaderBuffer.Buffer.SetData(this._renderSourceGroup._shaderCommandParamsArray, 0, this._shaderCommandParamsStartIndex, this._renderSourceGroup._shaderCommandParamsArray.Length);
			return gpuishaderBuffer;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000AE6C File Offset: 0x0000906C
		public GPUIShaderBuffer GetTransformBuffer()
		{
			if (this.IsCameraBasedBuffer)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "GetTransformBuffer method can not be used with Camera Based transform buffers.");
				return null;
			}
			GPUIShaderBuffer gpuishaderBuffer;
			if (this._transformBufferDict == null)
			{
				this._transformBufferDict = new Dictionary<int, GPUIShaderBuffer>();
			}
			else if (this._transformBufferDict.TryGetValue(0, out gpuishaderBuffer) && gpuishaderBuffer != null)
			{
				return gpuishaderBuffer;
			}
			GPUIShaderBuffer gpuishaderBuffer2 = this.CreateTransformBuffer();
			this._transformBufferDict[0] = gpuishaderBuffer2;
			return gpuishaderBuffer2;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000AED8 File Offset: 0x000090D8
		public GPUIShaderBuffer GetTransformBuffer(GPUICameraData cameraData)
		{
			if (!this.IsCameraBasedBuffer || cameraData == null)
			{
				return this.GetTransformBuffer();
			}
			int instanceIDGPUI = cameraData.ActiveCamera.GetInstanceIDGPUI();
			if (this._transformBufferDict == null)
			{
				this._transformBufferDict = new Dictionary<int, GPUIShaderBuffer>();
			}
			GPUIShaderBuffer gpuishaderBuffer;
			if (!this._transformBufferDict.TryGetValue(instanceIDGPUI, out gpuishaderBuffer) || gpuishaderBuffer == null)
			{
				gpuishaderBuffer = this.CreateTransformBuffer();
				this._transformBufferDict[instanceIDGPUI] = gpuishaderBuffer;
			}
			return gpuishaderBuffer;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000AF40 File Offset: 0x00009140
		public GPUIShaderBuffer GetInstanceDataBuffer(GPUICameraData cameraData)
		{
			int instanceIDGPUI = cameraData.ActiveCamera.GetInstanceIDGPUI();
			if (this._instanceDataBufferDict == null)
			{
				this._instanceDataBufferDict = new Dictionary<int, GPUIShaderBuffer>();
			}
			GPUILODGroupData lodgroupData = this._renderSourceGroup.LODGroupData;
			GPUIProfile profile = this._renderSourceGroup.Profile;
			int num = this._renderSourceGroup.BufferSize * ((lodgroupData.Length + lodgroupData.optionalRendererCount) * (profile.isShadowCasting ? 2 : 1) + ((profile.isLODCrossFade && profile.isAnimateCrossFade && !this.IsCameraBasedBuffer) ? 1 : 0));
			this._shaderCommandParamsStartIndex = num;
			num += this._renderSourceGroup._shaderCommandParamsArray.Length;
			GPUIShaderBuffer gpuishaderBuffer;
			if (!this._instanceDataBufferDict.TryGetValue(instanceIDGPUI, out gpuishaderBuffer) || gpuishaderBuffer == null || gpuishaderBuffer.BufferSize != num)
			{
				if (gpuishaderBuffer != null)
				{
					gpuishaderBuffer.Dispose();
				}
				gpuishaderBuffer = this.CreateInstanceDataBuffer(num);
				this._instanceDataBufferDict[instanceIDGPUI] = gpuishaderBuffer;
				this._renderSourceGroup._requireShaderCommandParamsUpdate = false;
			}
			if (this._renderSourceGroup._requireShaderCommandParamsUpdate)
			{
				this._renderSourceGroup._requireShaderCommandParamsUpdate = false;
				gpuishaderBuffer.Buffer.SetData(this._renderSourceGroup._shaderCommandParamsArray, 0, this._shaderCommandParamsStartIndex, this._renderSourceGroup._shaderCommandParamsArray.Length);
			}
			return gpuishaderBuffer;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000B074 File Offset: 0x00009274
		public void SetMPBBuffers(MaterialPropertyBlock mpb, GPUICameraData cameraData)
		{
			GPUIShaderBuffer transformBuffer = this.GetTransformBuffer(cameraData);
			GPUIShaderBuffer instanceDataBuffer = this.GetInstanceDataBuffer(cameraData);
			if (GPUIRuntimeSettings.Instance.DisableShaderBuffers)
			{
				mpb.SetTexture(GPUIConstants.PROP_gpuiTransformBufferTexture, transformBuffer.Texture);
				mpb.SetTexture(GPUIConstants.PROP_gpuiInstanceDataBufferTexture, instanceDataBuffer.Texture);
			}
			else
			{
				mpb.SetBuffer(GPUIConstants.PROP_gpuiTransformBuffer, transformBuffer.Buffer);
				mpb.SetBuffer(GPUIConstants.PROP_gpuiInstanceDataBuffer, instanceDataBuffer.Buffer);
				if (this.HasPreviousFrameTransformBuffer)
				{
					bool flag = this.PreviousFrameTransformBuffer != null;
					mpb.SetInt(GPUIConstants.PROP_hasPreviousFrameTransformBuffer, flag ? 1 : 0);
					if (flag)
					{
						int count = transformBuffer.Buffer.count;
						if (this.PreviousFrameTransformBuffer.count < count)
						{
							int count2 = this.PreviousFrameTransformBuffer.count;
							GraphicsBuffer graphicsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, 64);
							graphicsBuffer.SetData(this.PreviousFrameTransformBuffer, 0, 0, count2);
							graphicsBuffer.SetData(transformBuffer.Buffer, count2, count2, count - count2);
							this.PreviousFrameTransformBuffer.Release();
							this.PreviousFrameTransformBuffer = graphicsBuffer;
						}
						mpb.SetBuffer(GPUIConstants.PROP_gpuiPreviousFrameTransformBuffer, this.PreviousFrameTransformBuffer);
					}
					else
					{
						mpb.SetBuffer(GPUIConstants.PROP_gpuiPreviousFrameTransformBuffer, GPUIRenderingSystem.Instance.DummyGraphicsBuffer);
					}
				}
				if (this.HasPerInstanceLightProbes)
				{
					if (this._renderSourceGroup.Profile.lightProbeSetting != GPUILightProbeSetting.PerInstance)
					{
						this.ReleaseLightProbeBuffers();
					}
					else
					{
						bool flag2 = this._perInstanceLightProbesBuffer != null;
						mpb.SetInt(GPUIConstants.PROP_hasPerInstanceLightProbes, flag2 ? 1 : 0);
						if (flag2)
						{
							mpb.SetBuffer(GPUIConstants.PROP_gpuiPerInstanceLightProbesBuffer, this._perInstanceLightProbesBuffer);
						}
						else
						{
							mpb.SetBuffer(GPUIConstants.PROP_gpuiPerInstanceLightProbesBuffer, GPUIRenderingSystem.Instance.DummyGraphicsBuffer);
						}
					}
				}
			}
			mpb.SetFloat(GPUIConstants.PROP_transformBufferSize, (float)this._renderSourceGroup.BufferSize);
			mpb.SetFloat(GPUIConstants.PROP_instanceDataBufferSize, (float)instanceDataBuffer.BufferSize);
			mpb.SetFloat(GPUIConstants.PROP_maxTextureSize, (float)GPUIConstants.TEXTURE_MAX_SIZE);
			mpb.SetInt(GPUIConstants.PROP_commandParamsStartIndex, this._shaderCommandParamsStartIndex);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000B25C File Offset: 0x0000945C
		internal void UpdateData(int frameNo)
		{
			GPUIShaderBuffer gpuishaderBuffer;
			if (this.HasPreviousFrameTransformBuffer && this._renderSourceGroup.BufferSize > 0 && frameNo > this._previousFrameBufferFrameNo && this._transformBufferDict.TryGetValue(0, out gpuishaderBuffer) && gpuishaderBuffer.Buffer != null)
			{
				this._previousFrameBufferFrameNo = frameNo;
				int count = gpuishaderBuffer.Buffer.count;
				if (this.PreviousFrameTransformBuffer == null)
				{
					this.PreviousFrameTransformBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, 64);
				}
				else if (this.PreviousFrameTransformBuffer.count != count)
				{
					this.PreviousFrameTransformBuffer.Release();
					this.PreviousFrameTransformBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, 64);
				}
				this.PreviousFrameTransformBuffer.SetData(gpuishaderBuffer.Buffer, 0, 0, count);
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000B313 File Offset: 0x00009513
		public void OnTransformDataModified()
		{
			this._transformDataModified = true;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000B31C File Offset: 0x0000951C
		public void RequireInstancingBoundsUpdate()
		{
			this._requiresInstancingBoundsUpdate = true;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000B328 File Offset: 0x00009528
		public void ApplyTransformDataUpdates()
		{
			if (!this._transformDataModified)
			{
				if (this._requiresInstancingBoundsUpdate)
				{
					this.CalculateInstancingBounds();
				}
				return;
			}
			this._transformDataModified = false;
			if (this._transformBufferDict == null)
			{
				return;
			}
			foreach (GPUIShaderBuffer gpuishaderBuffer in this._transformBufferDict.Values)
			{
				if (gpuishaderBuffer != null)
				{
					gpuishaderBuffer.OnDataModified();
				}
			}
			GPUIRenderingSystem.OnBufferDataModifiedCallback onBufferDataModified = GPUIRenderingSystem.OnBufferDataModified;
			if (onBufferDataModified != null)
			{
				onBufferDataModified(this);
			}
			this.CalculateInstancingBounds();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000B3C4 File Offset: 0x000095C4
		public void ResetPreviousFrameBuffer()
		{
			this.UpdateData(this._previousFrameBufferFrameNo + 1);
			this._previousFrameBufferFrameNo = -1;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000B3DC File Offset: 0x000095DC
		public void SetOptionalRendererStatusBufferData(NativeArray<uint> optionalRendererStatusData, int bufferStartIndex)
		{
			int num = (this.RenderSourceGroup.LODGroupData.optionalRendererCount - 1) / 32 + 1;
			int num2 = this._renderSourceGroup.BufferSize * num;
			bufferStartIndex *= num;
			if (this._optionalRendererStatusBuffer != null && this._optionalRendererStatusBuffer.count != num2)
			{
				this._optionalRendererStatusBuffer.Dispose();
				this._optionalRendererStatusBuffer = null;
			}
			if (this._optionalRendererStatusBuffer == null)
			{
				this._optionalRendererStatusBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num2, 4);
				this._optionalRendererStatusBuffer.ClearBufferData();
			}
			this._optionalRendererStatusBuffer.SetData<uint>(optionalRendererStatusData, 0, bufferStartIndex, Mathf.Min(optionalRendererStatusData.Length, num2 - bufferStartIndex));
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000B47C File Offset: 0x0000967C
		public GraphicsBuffer GetOptionalRendererStatusBuffer()
		{
			if (this._optionalRendererStatusBuffer == null)
			{
				return null;
			}
			int num = this._renderSourceGroup.BufferSize * ((this.RenderSourceGroup.LODGroupData.optionalRendererCount - 1) / 32 + 1);
			if (this._optionalRendererStatusBuffer.count != num)
			{
				GraphicsBuffer optionalRendererStatusBuffer = this._optionalRendererStatusBuffer;
				this._optionalRendererStatusBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, num, 4);
				this._optionalRendererStatusBuffer.ClearBufferData();
				this._optionalRendererStatusBuffer.SetData(optionalRendererStatusBuffer, 0, 0, Mathf.Min(optionalRendererStatusBuffer.count, num));
				optionalRendererStatusBuffer.Dispose();
			}
			return this._optionalRendererStatusBuffer;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000B50C File Offset: 0x0000970C
		private bool CalculateInstancingBounds()
		{
			this.HasInstancingBounds = false;
			if (this.IsCameraBasedBuffer || this._renderSourceGroup.Profile == null || !this._renderSourceGroup.Profile.isCalculateInstancingBounds || GPUIRuntimeSettings.Instance.DisableInstancingBoundsCalculation)
			{
				return false;
			}
			GPUIShaderBuffer transformBuffer = this.GetTransformBuffer();
			int bufferSize = this._renderSourceGroup.BufferSize;
			if (transformBuffer == null || transformBuffer.Buffer == null || bufferSize <= 0)
			{
				return false;
			}
			GPUILODGroupData lodgroupData = this._renderSourceGroup.LODGroupData;
			if (lodgroupData == null)
			{
				return false;
			}
			GPUIDataBuffer<uint> instancingBoundsMinMaxBuffer = GPUIRenderingSystem.Instance._instancingBoundsMinMaxBuffer;
			if (instancingBoundsMinMaxBuffer.IsDataRequested() || GPUIRenderingSystem.Instance._requireInstancingBoundsDataRead)
			{
				this._requiresInstancingBoundsUpdate = true;
				return false;
			}
			this._requiresInstancingBoundsUpdate = false;
			if (this._instancingBoundsIndex < 0 || instancingBoundsMinMaxBuffer.Length < this._instancingBoundsIndex + 6)
			{
				this._instancingBoundsIndex = instancingBoundsMinMaxBuffer.Length;
				instancingBoundsMinMaxBuffer.Resize(this._instancingBoundsIndex + 6);
				instancingBoundsMinMaxBuffer.UpdateBufferData(false);
			}
			ComputeShader cs_CalculateInstancingBounds = GPUIConstants.CS_CalculateInstancingBounds;
			int num = 0;
			cs_CalculateInstancingBounds.SetBuffer(num, GPUIConstants.PROP_gpuiTransformBuffer, transformBuffer.Buffer);
			cs_CalculateInstancingBounds.SetBuffer<uint>(num, GPUIConstants.PROP_gpuiBoundsMinMax, instancingBoundsMinMaxBuffer);
			cs_CalculateInstancingBounds.SetInt(GPUIConstants.PROP_bufferSize, bufferSize);
			cs_CalculateInstancingBounds.SetInt(GPUIConstants.PROP_startIndex, this._instancingBoundsIndex);
			cs_CalculateInstancingBounds.SetVector(GPUIConstants.PROP_boundsCenter, lodgroupData.bounds.center);
			cs_CalculateInstancingBounds.SetVector(GPUIConstants.PROP_boundsExtents, lodgroupData.bounds.extents);
			cs_CalculateInstancingBounds.DispatchX(num, bufferSize);
			GPUIRenderingSystem.Instance._requireInstancingBoundsDataRead = true;
			return true;
		}

		// Token: 0x04000153 RID: 339
		private GPUIRenderSourceGroup _renderSourceGroup;

		// Token: 0x04000154 RID: 340
		private Dictionary<int, GPUIShaderBuffer> _transformBufferDict;

		// Token: 0x04000156 RID: 342
		private bool _hasPreviousFrameTransformBuffer;

		// Token: 0x04000157 RID: 343
		private int _previousFrameBufferFrameNo;

		// Token: 0x04000158 RID: 344
		internal GraphicsBuffer _perInstanceLightProbesBuffer;

		// Token: 0x04000159 RID: 345
		private bool _hasPerInstanceLightProbes;

		// Token: 0x0400015A RID: 346
		private bool _isAllowPerInstanceLightProbes;

		// Token: 0x0400015B RID: 347
		private Dictionary<int, GPUIShaderBuffer> _instanceDataBufferDict;

		// Token: 0x0400015C RID: 348
		public int resetCrossFadeDataFrame;

		// Token: 0x0400015D RID: 349
		private GraphicsBuffer _optionalRendererStatusBuffer;

		// Token: 0x0400015E RID: 350
		private int _shaderCommandParamsStartIndex;

		// Token: 0x0400015F RID: 351
		private bool _transformDataModified;

		// Token: 0x04000160 RID: 352
		private bool _requiresInstancingBoundsUpdate;

		// Token: 0x04000161 RID: 353
		internal int _instancingBoundsIndex = -1;

		// Token: 0x04000162 RID: 354
		internal Bounds _instancingBounds;
	}
}
