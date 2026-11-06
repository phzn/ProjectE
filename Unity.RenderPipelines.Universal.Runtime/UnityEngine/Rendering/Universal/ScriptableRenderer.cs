using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Unity.Collections;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.VFX;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000080 RID: 128
	public abstract class ScriptableRenderer : IDisposable
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00019C75 File Offset: 0x00017E75
		[Obsolete("cameraDepth has been renamed to cameraDepthTarget. (UnityUpgradable) -> cameraDepthTarget")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public RenderTargetIdentifier cameraDepth
		{
			get
			{
				return this.m_CameraDepthTarget.nameID;
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00019C84 File Offset: 0x00017E84
		internal void ResetNativeRenderPassFrameData()
		{
			if (this.m_MergeableRenderPassesMapArrays == null)
			{
				this.m_MergeableRenderPassesMapArrays = new int[10][];
			}
			for (int i = 0; i < 10; i++)
			{
				if (this.m_MergeableRenderPassesMapArrays[i] == null)
				{
					this.m_MergeableRenderPassesMapArrays[i] = new int[20];
				}
				for (int j = 0; j < 20; j++)
				{
					this.m_MergeableRenderPassesMapArrays[i][j] = -1;
				}
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00019CE4 File Offset: 0x00017EE4
		internal void SetupNativeRenderPassFrameData(ref CameraData cameraData, bool isRenderPassEnabled)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.setupFrameData))
			{
				int count = this.m_ActiveRenderPassQueue.Count;
				this.m_MergeableRenderPassesMap.Clear();
				this.m_RenderPassesAttachmentCount.Clear();
				uint num = 0U;
				for (int i = 0; i < this.m_ActiveRenderPassQueue.Count; i++)
				{
					ScriptableRenderPass scriptableRenderPass = this.m_ActiveRenderPassQueue[i];
					scriptableRenderPass.renderPassQueueIndex = i;
					if (this.IsRenderPassEnabled(scriptableRenderPass))
					{
						ScriptableRenderer.RenderPassDescriptor renderPassDescriptor = this.InitializeRenderPassDescriptor(ref cameraData, scriptableRenderPass);
						Hash128 hash = ScriptableRenderer.CreateRenderPassHash(renderPassDescriptor, num);
						this.m_PassIndexToPassHash[i] = hash;
						if (!this.m_MergeableRenderPassesMap.ContainsKey(hash))
						{
							this.m_MergeableRenderPassesMap.Add(hash, this.m_MergeableRenderPassesMapArrays[this.m_MergeableRenderPassesMap.Count]);
							this.m_RenderPassesAttachmentCount.Add(hash, 0);
						}
						else if (this.m_MergeableRenderPassesMap[hash][ScriptableRenderer.GetValidPassIndexCount(this.m_MergeableRenderPassesMap[hash]) - 1] != i - 1)
						{
							num += 1U;
							hash = ScriptableRenderer.CreateRenderPassHash(renderPassDescriptor, num);
							this.m_PassIndexToPassHash[i] = hash;
							this.m_MergeableRenderPassesMap.Add(hash, this.m_MergeableRenderPassesMapArrays[this.m_MergeableRenderPassesMap.Count]);
							this.m_RenderPassesAttachmentCount.Add(hash, 0);
						}
						this.m_MergeableRenderPassesMap[hash][ScriptableRenderer.GetValidPassIndexCount(this.m_MergeableRenderPassesMap[hash])] = i;
					}
				}
				for (int j = 0; j < this.m_ActiveRenderPassQueue.Count; j++)
				{
					this.m_ActiveRenderPassQueue[j].m_ColorAttachmentIndices = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.ClearMemory);
					this.m_ActiveRenderPassQueue[j].m_InputAttachmentIndices = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.ClearMemory);
				}
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00019ECC File Offset: 0x000180CC
		internal void UpdateFinalStoreActions(int[] currentMergeablePasses, ref CameraData cameraData)
		{
			for (int i = 0; i < this.m_FinalColorStoreAction.Length; i++)
			{
				this.m_FinalColorStoreAction[i] = RenderBufferStoreAction.Store;
			}
			this.m_FinalDepthStoreAction = RenderBufferStoreAction.Store;
			foreach (int num in currentMergeablePasses)
			{
				if (!ScriptableRenderer.m_UseOptimizedStoreActions || num == -1)
				{
					break;
				}
				ScriptableRenderPass scriptableRenderPass = this.m_ActiveRenderPassQueue[num];
				int num2 = (scriptableRenderPass.overrideCameraTarget ? ScriptableRenderer.GetFirstAllocatedRTHandle(scriptableRenderPass).rt.descriptor.msaaSamples : ((cameraData.targetTexture != null) ? cameraData.targetTexture.descriptor.msaaSamples : cameraData.cameraTargetDescriptor.msaaSamples));
				bool flag = cameraData.renderer != null && cameraData.renderer.supportedRenderingFeatures.msaa;
				if (!cameraData.camera.allowMSAA || !flag)
				{
					num2 = 1;
				}
				for (int k = 0; k < this.m_FinalColorStoreAction.Length; k++)
				{
					if (this.m_FinalColorStoreAction[k] == RenderBufferStoreAction.Store || this.m_FinalColorStoreAction[k] == RenderBufferStoreAction.StoreAndResolve || scriptableRenderPass.overriddenColorStoreActions[k])
					{
						this.m_FinalColorStoreAction[k] = scriptableRenderPass.colorStoreActions[k];
					}
					if (num2 > 1)
					{
						if (this.m_FinalColorStoreAction[k] == RenderBufferStoreAction.Store)
						{
							this.m_FinalColorStoreAction[k] = RenderBufferStoreAction.StoreAndResolve;
						}
						else if (this.m_FinalColorStoreAction[k] == RenderBufferStoreAction.DontCare)
						{
							this.m_FinalColorStoreAction[k] = RenderBufferStoreAction.Resolve;
						}
					}
				}
				if (this.m_FinalDepthStoreAction == RenderBufferStoreAction.Store || (this.m_FinalDepthStoreAction == RenderBufferStoreAction.StoreAndResolve && scriptableRenderPass.depthStoreAction == RenderBufferStoreAction.Resolve) || scriptableRenderPass.overriddenDepthStoreAction)
				{
					this.m_FinalDepthStoreAction = scriptableRenderPass.depthStoreAction;
				}
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001A06C File Offset: 0x0001826C
		internal void SetNativeRenderPassMRTAttachmentList(ScriptableRenderPass renderPass, ref CameraData cameraData, bool needCustomCameraColorClear, ClearFlag cameraClearFlag)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.setMRTAttachmentsList))
			{
				int renderPassQueueIndex = renderPass.renderPassQueueIndex;
				Hash128 hash = this.m_PassIndexToPassHash[renderPassQueueIndex];
				int[] array = this.m_MergeableRenderPassesMap[hash];
				if (array.First<int>() == renderPassQueueIndex)
				{
					this.m_RenderPassesAttachmentCount[hash] = 0;
					this.UpdateFinalStoreActions(array, ref cameraData);
					int num = 0;
					bool flag = false;
					foreach (int num2 in array)
					{
						if (num2 == -1)
						{
							break;
						}
						ScriptableRenderPass scriptableRenderPass = this.m_ActiveRenderPassQueue[num2];
						for (int j = 0; j < scriptableRenderPass.m_ColorAttachmentIndices.Length; j++)
						{
							scriptableRenderPass.m_ColorAttachmentIndices[j] = -1;
						}
						for (int k = 0; k < scriptableRenderPass.m_InputAttachmentIndices.Length; k++)
						{
							scriptableRenderPass.m_InputAttachmentIndices[k] = -1;
						}
						uint validColorBufferCount = RenderingUtils.GetValidColorBufferCount(scriptableRenderPass.colorAttachmentHandles);
						int num3 = 0;
						while ((long)num3 < (long)((ulong)validColorBufferCount))
						{
							AttachmentDescriptor attachmentDescriptor = new AttachmentDescriptor((scriptableRenderPass.renderTargetFormat[num3] != GraphicsFormat.None) ? scriptableRenderPass.renderTargetFormat[num3] : UniversalRenderPipeline.MakeRenderTextureGraphicsFormat(cameraData.isHdrEnabled, cameraData.hdrColorBufferPrecision, Graphics.preserveFramebufferAlpha));
							RTHandle rthandle = (scriptableRenderPass.overrideCameraTarget ? scriptableRenderPass.colorAttachmentHandles[num3] : this.m_CameraColorTarget.handle);
							int num4 = ScriptableRenderer.FindAttachmentDescriptorIndexInList(rthandle.nameID, this.m_ActiveColorAttachmentDescriptors);
							if (ScriptableRenderer.m_UseOptimizedStoreActions)
							{
								attachmentDescriptor.storeAction = this.m_FinalColorStoreAction[num3];
							}
							if (num4 == -1)
							{
								this.m_ActiveColorAttachmentDescriptors[num] = attachmentDescriptor;
								bool flag2 = (scriptableRenderPass.clearFlag & ClearFlag.Color) > ClearFlag.None;
								this.m_ActiveColorAttachmentDescriptors[num].ConfigureTarget(rthandle.nameID, !flag2, true);
								if (scriptableRenderPass.colorAttachmentHandles[num3] == this.m_CameraColorTarget.handle && needCustomCameraColorClear && (cameraClearFlag & ClearFlag.Color) != ClearFlag.None)
								{
									this.m_ActiveColorAttachmentDescriptors[num].ConfigureClear(cameraData.backgroundColor, 1f, 0U);
								}
								else if (flag2)
								{
									this.m_ActiveColorAttachmentDescriptors[num].ConfigureClear(CoreUtils.ConvertSRGBToActiveColorSpace(scriptableRenderPass.clearColor), 1f, 0U);
								}
								scriptableRenderPass.m_ColorAttachmentIndices[num3] = num;
								num++;
								Dictionary<Hash128, int> renderPassesAttachmentCount = this.m_RenderPassesAttachmentCount;
								Hash128 hash2 = hash;
								int num5 = renderPassesAttachmentCount[hash2];
								renderPassesAttachmentCount[hash2] = num5 + 1;
							}
							else
							{
								scriptableRenderPass.m_ColorAttachmentIndices[num3] = num4;
							}
							num3++;
						}
						if (ScriptableRenderer.PassHasInputAttachments(scriptableRenderPass))
						{
							flag = true;
							this.SetupInputAttachmentIndices(scriptableRenderPass);
						}
						this.m_ActiveDepthAttachmentDescriptor = new AttachmentDescriptor(SystemInfo.GetGraphicsFormat(DefaultFormat.DepthStencil));
						bool flag3 = (cameraClearFlag & ClearFlag.DepthStencil) > ClearFlag.None;
						this.m_ActiveDepthAttachmentDescriptor.ConfigureTarget(scriptableRenderPass.overrideCameraTarget ? scriptableRenderPass.depthAttachmentHandle.nameID : this.m_CameraDepthTarget.nameID, !flag3, true);
						if (flag3)
						{
							this.m_ActiveDepthAttachmentDescriptor.ConfigureClear(Color.black, 1f, 0U);
						}
						if (ScriptableRenderer.m_UseOptimizedStoreActions)
						{
							this.m_ActiveDepthAttachmentDescriptor.storeAction = this.m_FinalDepthStoreAction;
						}
					}
					if (flag)
					{
						this.SetupTransientInputAttachments(this.m_RenderPassesAttachmentCount[hash]);
					}
				}
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001A3D0 File Offset: 0x000185D0
		private bool IsDepthOnlyRenderTexture(RenderTexture t)
		{
			return t.graphicsFormat == GraphicsFormat.None;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001A3E0 File Offset: 0x000185E0
		internal void SetNativeRenderPassAttachmentList(ScriptableRenderPass renderPass, ref CameraData cameraData, RTHandle passColorAttachment, RTHandle passDepthAttachment, ClearFlag finalClearFlag, Color finalClearColor)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.setAttachmentList))
			{
				int renderPassQueueIndex = renderPass.renderPassQueueIndex;
				Hash128 hash = this.m_PassIndexToPassHash[renderPassQueueIndex];
				int[] array = this.m_MergeableRenderPassesMap[hash];
				if (array.First<int>() == renderPassQueueIndex)
				{
					this.m_RenderPassesAttachmentCount[hash] = 0;
					this.UpdateFinalStoreActions(array, ref cameraData);
					int num = 0;
					foreach (int num2 in array)
					{
						if (num2 == -1)
						{
							break;
						}
						ScriptableRenderPass scriptableRenderPass = this.m_ActiveRenderPassQueue[num2];
						for (int j = 0; j < scriptableRenderPass.m_ColorAttachmentIndices.Length; j++)
						{
							scriptableRenderPass.m_ColorAttachmentIndices[j] = -1;
						}
						bool flag = cameraData.targetTexture != null;
						bool flag2 = (scriptableRenderPass.colorAttachmentHandle.rt != null && this.IsDepthOnlyRenderTexture(scriptableRenderPass.colorAttachmentHandle.rt)) || (flag && this.IsDepthOnlyRenderTexture(cameraData.targetTexture));
						AttachmentDescriptor attachmentDescriptor;
						int num3;
						RenderTargetIdentifier renderTargetIdentifier;
						if (new RenderTargetIdentifier(passColorAttachment.nameID, 0, CubemapFace.Unknown, 0) != BuiltinRenderTextureType.CameraTarget)
						{
							attachmentDescriptor = new AttachmentDescriptor(flag2 ? passColorAttachment.rt.descriptor.depthStencilFormat : passColorAttachment.rt.descriptor.graphicsFormat);
							num3 = passColorAttachment.rt.descriptor.msaaSamples;
							renderTargetIdentifier = passColorAttachment.nameID;
						}
						else
						{
							attachmentDescriptor = new AttachmentDescriptor((scriptableRenderPass.renderTargetFormat[0] != GraphicsFormat.None) ? scriptableRenderPass.renderTargetFormat[0] : UniversalRenderPipeline.MakeRenderTextureGraphicsFormat(cameraData.isHdrEnabled, cameraData.hdrColorBufferPrecision, Graphics.preserveFramebufferAlpha));
							num3 = cameraData.cameraTargetDescriptor.msaaSamples;
							renderTargetIdentifier = (flag ? new RenderTargetIdentifier(cameraData.targetTexture) : BuiltinRenderTextureType.CameraTarget);
						}
						attachmentDescriptor.ConfigureTarget(renderTargetIdentifier, (finalClearFlag & ClearFlag.Color) == ClearFlag.None, true);
						if (ScriptableRenderer.PassHasInputAttachments(scriptableRenderPass))
						{
							this.SetupInputAttachmentIndices(scriptableRenderPass);
						}
						this.m_ActiveDepthAttachmentDescriptor = new AttachmentDescriptor(SystemInfo.GetGraphicsFormat(DefaultFormat.DepthStencil));
						this.m_ActiveDepthAttachmentDescriptor.ConfigureTarget((passDepthAttachment.nameID != BuiltinRenderTextureType.CameraTarget) ? passDepthAttachment.nameID : (flag ? new RenderTargetIdentifier(cameraData.targetTexture.depthBuffer, 0, CubemapFace.Unknown, 0) : BuiltinRenderTextureType.Depth), (finalClearFlag & ClearFlag.Depth) == ClearFlag.None, true);
						if (finalClearFlag != ClearFlag.None)
						{
							if (cameraData.renderType != CameraRenderType.Overlay || (flag2 && (finalClearFlag & ClearFlag.Color) != ClearFlag.None))
							{
								attachmentDescriptor.ConfigureClear(finalClearColor, 1f, 0U);
							}
							if ((finalClearFlag & ClearFlag.Depth) != ClearFlag.None)
							{
								this.m_ActiveDepthAttachmentDescriptor.ConfigureClear(Color.black, 1f, 0U);
							}
						}
						if (num3 > 1)
						{
							attachmentDescriptor.ConfigureResolveTarget(renderTargetIdentifier);
							if (RenderingUtils.MultisampleDepthResolveSupported())
							{
								this.m_ActiveDepthAttachmentDescriptor.ConfigureResolveTarget(this.m_ActiveDepthAttachmentDescriptor.loadStoreTarget);
							}
						}
						if (ScriptableRenderer.m_UseOptimizedStoreActions)
						{
							attachmentDescriptor.storeAction = this.m_FinalColorStoreAction[0];
							this.m_ActiveDepthAttachmentDescriptor.storeAction = this.m_FinalDepthStoreAction;
						}
						int num4 = ScriptableRenderer.FindAttachmentDescriptorIndexInList(num, attachmentDescriptor, this.m_ActiveColorAttachmentDescriptors);
						if (num4 == -1)
						{
							scriptableRenderPass.m_ColorAttachmentIndices[0] = num;
							this.m_ActiveColorAttachmentDescriptors[num] = attachmentDescriptor;
							num++;
							Dictionary<Hash128, int> renderPassesAttachmentCount = this.m_RenderPassesAttachmentCount;
							Hash128 hash2 = hash;
							int num5 = renderPassesAttachmentCount[hash2];
							renderPassesAttachmentCount[hash2] = num5 + 1;
						}
						else
						{
							scriptableRenderPass.m_ColorAttachmentIndices[0] = num4;
						}
					}
				}
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001A76C File Offset: 0x0001896C
		internal void ExecuteNativeRenderPass(ScriptableRenderContext context, ScriptableRenderPass renderPass, ref CameraData cameraData, ref RenderingData renderingData)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.execute))
			{
				int renderPassQueueIndex = renderPass.renderPassQueueIndex;
				Hash128 hash = this.m_PassIndexToPassHash[renderPassQueueIndex];
				int[] array = this.m_MergeableRenderPassesMap[hash];
				int num = this.m_RenderPassesAttachmentCount[hash];
				bool flag = (renderPass.colorAttachmentHandle.rt != null && this.IsDepthOnlyRenderTexture(renderPass.colorAttachmentHandle.rt)) || (cameraData.targetTexture != null && this.IsDepthOnlyRenderTexture(cameraData.targetTexture));
				bool flag2 = flag || !renderPass.overrideCameraTarget || (renderPass.overrideCameraTarget && renderPass.depthAttachmentHandle.nameID != BuiltinRenderTextureType.CameraTarget);
				NativeArray<AttachmentDescriptor> nativeArray = new NativeArray<AttachmentDescriptor>((flag2 && !flag) ? (num + 1) : 1, Allocator.Temp, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < num; i++)
				{
					nativeArray[i] = this.m_ActiveColorAttachmentDescriptors[i];
				}
				if (flag2 && !flag)
				{
					nativeArray[num] = this.m_ActiveDepthAttachmentDescriptor;
				}
				ScriptableRenderer.RenderPassDescriptor renderPassDescriptor = this.InitializeRenderPassDescriptor(ref cameraData, renderPass);
				int validPassIndexCount = ScriptableRenderer.GetValidPassIndexCount(array);
				uint subPassAttachmentIndicesCount = ScriptableRenderer.GetSubPassAttachmentIndicesCount(renderPass);
				NativeArray<int> nativeArray2 = new NativeArray<int>((int)((!flag) ? subPassAttachmentIndicesCount : 0U), Allocator.Temp, NativeArrayOptions.ClearMemory);
				if (!flag)
				{
					int num2 = 0;
					while ((long)num2 < (long)((ulong)subPassAttachmentIndicesCount))
					{
						nativeArray2[num2] = renderPass.m_ColorAttachmentIndices[num2];
						num2++;
					}
				}
				if (validPassIndexCount == 1 || array[0] == renderPassQueueIndex)
				{
					if (ScriptableRenderer.PassHasInputAttachments(renderPass))
					{
						Debug.LogWarning("First pass in a RenderPass should not have input attachments.");
					}
					context.BeginRenderPass(renderPassDescriptor.w, renderPassDescriptor.h, Math.Max(renderPassDescriptor.samples, 1), nativeArray, flag2 ? ((!flag) ? num : 0) : (-1));
					nativeArray.Dispose();
					context.BeginSubPass(nativeArray2, false);
					this.m_LastBeginSubpassPassIndex = renderPassQueueIndex;
				}
				else if (!ScriptableRenderer.AreAttachmentIndicesCompatible(this.m_ActiveRenderPassQueue[this.m_LastBeginSubpassPassIndex], this.m_ActiveRenderPassQueue[renderPassQueueIndex]))
				{
					context.EndSubPass();
					if (ScriptableRenderer.PassHasInputAttachments(this.m_ActiveRenderPassQueue[renderPassQueueIndex]))
					{
						context.BeginSubPass(nativeArray2, this.m_ActiveRenderPassQueue[renderPassQueueIndex].m_InputAttachmentIndices, false);
					}
					else
					{
						context.BeginSubPass(nativeArray2, false);
					}
					this.m_LastBeginSubpassPassIndex = renderPassQueueIndex;
				}
				else if (ScriptableRenderer.PassHasInputAttachments(this.m_ActiveRenderPassQueue[renderPassQueueIndex]))
				{
					context.EndSubPass();
					context.BeginSubPass(nativeArray2, this.m_ActiveRenderPassQueue[renderPassQueueIndex].m_InputAttachmentIndices, false);
					this.m_LastBeginSubpassPassIndex = renderPassQueueIndex;
				}
				nativeArray2.Dispose();
				renderPass.Execute(context, ref renderingData);
				context.ExecuteCommandBuffer(renderingData.commandBuffer);
				renderingData.commandBuffer.Clear();
				if (validPassIndexCount == 1 || array[validPassIndexCount - 1] == renderPassQueueIndex)
				{
					context.EndSubPass();
					context.EndRenderPass();
					this.m_LastBeginSubpassPassIndex = 0;
				}
				for (int j = 0; j < this.m_ActiveColorAttachmentDescriptors.Length; j++)
				{
					this.m_ActiveColorAttachmentDescriptors[j] = RenderingUtils.emptyAttachment;
					this.m_IsActiveColorAttachmentTransient[j] = false;
				}
				this.m_ActiveDepthAttachmentDescriptor = RenderingUtils.emptyAttachment;
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001AAB0 File Offset: 0x00018CB0
		internal void SetupInputAttachmentIndices(ScriptableRenderPass pass)
		{
			int validInputAttachmentCount = ScriptableRenderer.GetValidInputAttachmentCount(pass);
			pass.m_InputAttachmentIndices = new NativeArray<int>(validInputAttachmentCount, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < validInputAttachmentCount; i++)
			{
				pass.m_InputAttachmentIndices[i] = ScriptableRenderer.FindAttachmentDescriptorIndexInList(pass.m_InputAttachments[i], this.m_ActiveColorAttachmentDescriptors);
				if (pass.m_InputAttachmentIndices[i] == -1)
				{
					Debug.LogWarning("RenderPass Input attachment not found in the current RenderPass");
				}
				else if (!this.m_IsActiveColorAttachmentTransient[pass.m_InputAttachmentIndices[i]])
				{
					this.m_IsActiveColorAttachmentTransient[pass.m_InputAttachmentIndices[i]] = pass.IsInputAttachmentTransient(i);
				}
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001AB4C File Offset: 0x00018D4C
		internal void SetupTransientInputAttachments(int attachmentCount)
		{
			for (int i = 0; i < attachmentCount; i++)
			{
				if (this.m_IsActiveColorAttachmentTransient[i])
				{
					this.m_ActiveColorAttachmentDescriptors[i].loadAction = RenderBufferLoadAction.DontCare;
					this.m_ActiveColorAttachmentDescriptors[i].storeAction = RenderBufferStoreAction.DontCare;
					this.m_ActiveColorAttachmentDescriptors[i].loadStoreTarget = BuiltinRenderTextureType.None;
				}
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001ABAC File Offset: 0x00018DAC
		internal static uint GetSubPassAttachmentIndicesCount(ScriptableRenderPass pass)
		{
			uint num = 0U;
			using (NativeArray<int>.Enumerator enumerator = pass.m_ColorAttachmentIndices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current >= 0)
					{
						num += 1U;
					}
				}
			}
			return num;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001AC04 File Offset: 0x00018E04
		internal static bool AreAttachmentIndicesCompatible(ScriptableRenderPass lastSubPass, ScriptableRenderPass currentSubPass)
		{
			uint subPassAttachmentIndicesCount = ScriptableRenderer.GetSubPassAttachmentIndicesCount(lastSubPass);
			uint subPassAttachmentIndicesCount2 = ScriptableRenderer.GetSubPassAttachmentIndicesCount(currentSubPass);
			if (subPassAttachmentIndicesCount2 > subPassAttachmentIndicesCount)
			{
				return false;
			}
			uint num = 0U;
			int num2 = 0;
			while ((long)num2 < (long)((ulong)subPassAttachmentIndicesCount2))
			{
				int num3 = 0;
				while ((long)num3 < (long)((ulong)subPassAttachmentIndicesCount))
				{
					if (currentSubPass.m_ColorAttachmentIndices[num2] == lastSubPass.m_ColorAttachmentIndices[num3])
					{
						num += 1U;
					}
					num3++;
				}
				num2++;
			}
			return num == subPassAttachmentIndicesCount2;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001AC6C File Offset: 0x00018E6C
		internal static uint GetValidColorAttachmentCount(AttachmentDescriptor[] colorAttachments)
		{
			uint num = 0U;
			if (colorAttachments != null)
			{
				for (int i = 0; i < colorAttachments.Length; i++)
				{
					if (colorAttachments[i] != RenderingUtils.emptyAttachment)
					{
						num += 1U;
					}
				}
			}
			return num;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001ACA8 File Offset: 0x00018EA8
		internal static int GetValidInputAttachmentCount(ScriptableRenderPass renderPass)
		{
			int num = renderPass.m_InputAttachments.Length;
			if (num != 8)
			{
				return num;
			}
			for (int i = 0; i < num; i++)
			{
				if (renderPass.m_InputAttachments[i] == null)
				{
					return i;
				}
			}
			return num;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001ACE0 File Offset: 0x00018EE0
		internal static int FindAttachmentDescriptorIndexInList(int attachmentIdx, AttachmentDescriptor attachmentDescriptor, AttachmentDescriptor[] attachmentDescriptors)
		{
			int num = -1;
			for (int i = 0; i <= attachmentIdx; i++)
			{
				AttachmentDescriptor attachmentDescriptor2 = attachmentDescriptors[i];
				if (attachmentDescriptor2.loadStoreTarget == attachmentDescriptor.loadStoreTarget && attachmentDescriptor2.graphicsFormat == attachmentDescriptor.graphicsFormat)
				{
					num = i;
					break;
				}
			}
			return num;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001AD30 File Offset: 0x00018F30
		internal static int FindAttachmentDescriptorIndexInList(RenderTargetIdentifier target, AttachmentDescriptor[] attachmentDescriptors)
		{
			for (int i = 0; i < attachmentDescriptors.Length; i++)
			{
				AttachmentDescriptor attachmentDescriptor = attachmentDescriptors[i];
				if (attachmentDescriptor.loadStoreTarget == target)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001AD68 File Offset: 0x00018F68
		internal static int GetValidPassIndexCount(int[] array)
		{
			if (array == null)
			{
				return 0;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == -1)
				{
					return i;
				}
			}
			return array.Length - 1;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001AD98 File Offset: 0x00018F98
		internal static RTHandle GetFirstAllocatedRTHandle(ScriptableRenderPass pass)
		{
			for (int i = 0; i < pass.colorAttachmentHandles.Length; i++)
			{
				if (pass.colorAttachmentHandles[i].rt != null)
				{
					return pass.colorAttachmentHandles[i];
				}
			}
			return pass.colorAttachmentHandles[0];
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001ADDE File Offset: 0x00018FDE
		internal static bool PassHasInputAttachments(ScriptableRenderPass renderPass)
		{
			return renderPass.m_InputAttachments.Length != 8 || renderPass.m_InputAttachments[0] != null;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001ADF8 File Offset: 0x00018FF8
		internal static Hash128 CreateRenderPassHash(int width, int height, int depthID, int sample, uint hashIndex)
		{
			return new Hash128((uint)((width << 4) + height), (uint)depthID, (uint)sample, hashIndex);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001AE08 File Offset: 0x00019008
		internal static Hash128 CreateRenderPassHash(ScriptableRenderer.RenderPassDescriptor desc, uint hashIndex)
		{
			return ScriptableRenderer.CreateRenderPassHash(desc.w, desc.h, desc.depthID, desc.samples, hashIndex);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001AE28 File Offset: 0x00019028
		internal static void GetRenderTextureDescriptor(ref CameraData cameraData, ScriptableRenderPass renderPass, out RenderTextureDescriptor targetRT)
		{
			if (!renderPass.overrideCameraTarget || (renderPass.colorAttachmentHandle.rt == null && renderPass.depthAttachmentHandle.rt == null))
			{
				targetRT = cameraData.cameraTargetDescriptor;
				if (cameraData.targetTexture != null)
				{
					targetRT.width = cameraData.scaledWidth;
					targetRT.height = cameraData.scaledHeight;
					return;
				}
			}
			else
			{
				RTHandle firstAllocatedRTHandle = ScriptableRenderer.GetFirstAllocatedRTHandle(renderPass);
				targetRT = ((firstAllocatedRTHandle.rt != null) ? firstAllocatedRTHandle.rt.descriptor : renderPass.depthAttachmentHandle.rt.descriptor);
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001AED0 File Offset: 0x000190D0
		private ScriptableRenderer.RenderPassDescriptor InitializeRenderPassDescriptor(ref CameraData cameraData, ScriptableRenderPass renderPass)
		{
			RenderTextureDescriptor renderTextureDescriptor;
			ScriptableRenderer.GetRenderTextureDescriptor(ref cameraData, renderPass, out renderTextureDescriptor);
			RTHandle rthandle = (renderPass.overrideCameraTarget ? renderPass.depthAttachmentHandle : this.cameraDepthTargetHandle);
			int num = ((renderTextureDescriptor.graphicsFormat == GraphicsFormat.None && renderTextureDescriptor.depthStencilFormat != GraphicsFormat.None) ? renderPass.colorAttachmentHandle.GetHashCode() : rthandle.GetHashCode());
			return new ScriptableRenderer.RenderPassDescriptor(renderTextureDescriptor.width, renderTextureDescriptor.height, renderTextureDescriptor.msaaSamples, num);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001AF3E File Offset: 0x0001913E
		public virtual int SupportedCameraStackingTypes()
		{
			return 0;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001AF41 File Offset: 0x00019141
		public bool SupportsCameraStackingType(CameraRenderType cameraRenderType)
		{
			return (this.SupportedCameraStackingTypes() & (1 << (int)cameraRenderType)) != 0;
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001AF53 File Offset: 0x00019153
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x0001AF5B File Offset: 0x0001915B
		protected ProfilingSampler profilingExecute { get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001AF64 File Offset: 0x00019164
		internal DebugHandler DebugHandler { get; }

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001AF6C File Offset: 0x0001916C
		public static void SetCameraMatrices(CommandBuffer cmd, ref CameraData cameraData, bool setInverseMatrices)
		{
			ScriptableRenderer.SetCameraMatrices(cmd, ref cameraData, setInverseMatrices, cameraData.IsCameraProjectionMatrixFlipped());
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001AF7C File Offset: 0x0001917C
		internal static void SetCameraMatrices(CommandBuffer cmd, ref CameraData cameraData, bool setInverseMatrices, bool isTargetFlipped)
		{
			if (cameraData.xr.enabled)
			{
				cameraData.PushBuiltinShaderConstantsXR(cmd, false);
				XRSystemUniversal.MarkShaderProperties(cmd, cameraData.xrUniversal, false);
				return;
			}
			Matrix4x4 viewMatrix = cameraData.GetViewMatrix(0);
			Matrix4x4 projectionMatrix = cameraData.GetProjectionMatrix(0);
			cmd.SetViewProjectionMatrices(viewMatrix, projectionMatrix);
			if (setInverseMatrices)
			{
				Matrix4x4 gpuprojectionMatrix = cameraData.GetGPUProjectionMatrix(isTargetFlipped, 0);
				gpuprojectionMatrix * viewMatrix;
				Matrix4x4 matrix4x = Matrix4x4.Inverse(viewMatrix);
				Matrix4x4 matrix4x2 = Matrix4x4.Inverse(gpuprojectionMatrix);
				Matrix4x4 matrix4x3 = matrix4x * matrix4x2;
				Matrix4x4 matrix4x4 = Matrix4x4.Scale(new Vector3(1f, 1f, -1f)) * viewMatrix;
				Matrix4x4 inverse = matrix4x4.inverse;
				cmd.SetGlobalMatrix(ShaderPropertyId.worldToCameraMatrix, matrix4x4);
				cmd.SetGlobalMatrix(ShaderPropertyId.cameraToWorldMatrix, inverse);
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseViewMatrix, matrix4x);
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseProjectionMatrix, matrix4x2);
				cmd.SetGlobalMatrix(ShaderPropertyId.inverseViewAndProjectionMatrix, matrix4x3);
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001B059 File Offset: 0x00019259
		private void SetPerCameraShaderVariables(CommandBuffer cmd, ref CameraData cameraData)
		{
			this.SetPerCameraShaderVariables(cmd, ref cameraData, cameraData.IsCameraProjectionMatrixFlipped());
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001B06C File Offset: 0x0001926C
		private void SetPerCameraShaderVariables(CommandBuffer cmd, ref CameraData cameraData, bool isTargetFlipped)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.setPerCameraShaderVariables))
			{
				Camera camera = cameraData.camera;
				float num = (float)cameraData.cameraTargetDescriptor.width;
				float num2 = (float)cameraData.cameraTargetDescriptor.height;
				float num3 = (float)camera.pixelWidth;
				float num4 = (float)camera.pixelHeight;
				if (cameraData.xr.enabled)
				{
					num3 = (float)cameraData.cameraTargetDescriptor.width;
					num4 = (float)cameraData.cameraTargetDescriptor.height;
					this.useRenderPassEnabled = false;
				}
				if (camera.allowDynamicResolution)
				{
					num *= ScalableBufferManager.widthScaleFactor;
					num2 *= ScalableBufferManager.heightScaleFactor;
				}
				float nearClipPlane = camera.nearClipPlane;
				float farClipPlane = camera.farClipPlane;
				float num5 = (Mathf.Approximately(nearClipPlane, 0f) ? 0f : (1f / nearClipPlane));
				float num6 = (Mathf.Approximately(farClipPlane, 0f) ? 0f : (1f / farClipPlane));
				float num7 = (camera.orthographic ? 1f : 0f);
				float num8 = 1f - farClipPlane * num5;
				float num9 = farClipPlane * num5;
				Vector4 vector = new Vector4(num8, num9, num8 * num6, num9 * num6);
				if (SystemInfo.usesReversedZBuffer)
				{
					vector.y += vector.x;
					vector.x = -vector.x;
					vector.w += vector.z;
					vector.z = -vector.z;
				}
				float num10 = (isTargetFlipped ? (-1f) : 1f);
				Vector4 vector2 = new Vector4(num10, nearClipPlane, farClipPlane, 1f * num6);
				cmd.SetGlobalVector(ShaderPropertyId.projectionParams, vector2);
				Vector4 vector3 = new Vector4(camera.orthographicSize * cameraData.aspectRatio, camera.orthographicSize, 0f, num7);
				cmd.SetGlobalVector(ShaderPropertyId.worldSpaceCameraPos, cameraData.worldSpaceCameraPos);
				cmd.SetGlobalVector(ShaderPropertyId.screenParams, new Vector4(num3, num4, 1f + 1f / num3, 1f + 1f / num4));
				cmd.SetGlobalVector(ShaderPropertyId.scaledScreenParams, new Vector4(num, num2, 1f + 1f / num, 1f + 1f / num2));
				cmd.SetGlobalVector(ShaderPropertyId.zBufferParams, vector);
				cmd.SetGlobalVector(ShaderPropertyId.orthoParams, vector3);
				cmd.SetGlobalVector(ShaderPropertyId.screenSize, new Vector4(num, num2, 1f / num, 1f / num2));
				CoreUtils.SetKeyword(cmd, "SCREEN_COORD_OVERRIDE", cameraData.useScreenCoordOverride);
				cmd.SetGlobalVector(ShaderPropertyId.screenSizeOverride, cameraData.screenSizeOverride);
				cmd.SetGlobalVector(ShaderPropertyId.screenCoordScaleBias, cameraData.screenCoordScaleBias);
				float num11 = Math.Min((float)(-(float)Math.Log((double)(num3 / num), 2.0)), 0f);
				float num12 = Math.Min(cameraData.taaSettings.mipBias, 0f);
				num11 = Math.Min(num11, num12);
				cmd.SetGlobalVector(ShaderPropertyId.globalMipBias, new Vector2(num11, Mathf.Pow(2f, num11)));
				ScriptableRenderer.SetCameraMatrices(cmd, ref cameraData, true, isTargetFlipped);
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001B3A8 File Offset: 0x000195A8
		private void SetPerCameraBillboardProperties(CommandBuffer cmd, ref CameraData cameraData)
		{
			Matrix4x4 viewMatrix = cameraData.GetViewMatrix(0);
			Vector3 worldSpaceCameraPos = cameraData.worldSpaceCameraPos;
			CoreUtils.SetKeyword(cmd, "BILLBOARD_FACE_CAMERA_POS", QualitySettings.billboardsFaceCameraPosition);
			Vector3 vector;
			Vector3 vector2;
			float num;
			ScriptableRenderer.CalculateBillboardProperties(in viewMatrix, out vector, out vector2, out num);
			cmd.SetGlobalVector(ShaderPropertyId.billboardNormal, new Vector4(vector2.x, vector2.y, vector2.z, 0f));
			cmd.SetGlobalVector(ShaderPropertyId.billboardTangent, new Vector4(vector.x, vector.y, vector.z, 0f));
			cmd.SetGlobalVector(ShaderPropertyId.billboardCameraParams, new Vector4(worldSpaceCameraPos.x, worldSpaceCameraPos.y, worldSpaceCameraPos.z, num));
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001B454 File Offset: 0x00019654
		private static void CalculateBillboardProperties(in Matrix4x4 worldToCameraMatrix, out Vector3 billboardTangent, out Vector3 billboardNormal, out float cameraXZAngle)
		{
			Matrix4x4 matrix4x = worldToCameraMatrix;
			matrix4x = matrix4x.transpose;
			Vector3 vector = new Vector3(matrix4x.m00, matrix4x.m10, matrix4x.m20);
			Vector3 vector2 = new Vector3(matrix4x.m01, matrix4x.m11, matrix4x.m21);
			Vector3 vector3 = new Vector3(matrix4x.m02, matrix4x.m12, matrix4x.m22);
			Vector3 up = Vector3.up;
			Vector3 vector4 = Vector3.Cross(vector3, up);
			billboardTangent = ((!Mathf.Approximately(vector4.sqrMagnitude, 0f)) ? vector4.normalized : vector);
			billboardNormal = Vector3.Cross(up, billboardTangent);
			billboardNormal = ((!Mathf.Approximately(billboardNormal.sqrMagnitude, 0f)) ? billboardNormal.normalized : vector2);
			Vector3 vector5 = new Vector3(0f, 0f, 1f);
			float num = vector5.x * billboardTangent.z - vector5.z * billboardTangent.x;
			float num2 = vector5.x * billboardTangent.x + vector5.z * billboardTangent.z;
			cameraXZAngle = Mathf.Atan2(num, num2);
			if (cameraXZAngle < 0f)
			{
				cameraXZAngle += 6.2831855f;
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001B58F File Offset: 0x0001978F
		private void SetPerCameraClippingPlaneProperties(CommandBuffer cmd, ref CameraData cameraData)
		{
			this.SetPerCameraClippingPlaneProperties(cmd, in cameraData, cameraData.IsCameraProjectionMatrixFlipped());
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001B5A0 File Offset: 0x000197A0
		private void SetPerCameraClippingPlaneProperties(CommandBuffer cmd, in CameraData cameraData, bool isTargetFlipped)
		{
			CameraData cameraData2 = cameraData;
			Matrix4x4 gpuprojectionMatrix = cameraData2.GetGPUProjectionMatrix(isTargetFlipped, 0);
			cameraData2 = cameraData;
			Matrix4x4 viewMatrix = cameraData2.GetViewMatrix(0);
			Matrix4x4 matrix4x = CoreMatrixUtils.MultiplyProjectionMatrix(gpuprojectionMatrix, viewMatrix, cameraData.camera.orthographic);
			Plane[] array = ScriptableRenderer.s_Planes;
			GeometryUtility.CalculateFrustumPlanes(matrix4x, array);
			Vector4[] array2 = ScriptableRenderer.s_VectorPlanes;
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = new Vector4(array[i].normal.x, array[i].normal.y, array[i].normal.z, array[i].distance);
			}
			cmd.SetGlobalVectorArray(ShaderPropertyId.cameraWorldClipPlanes, array2);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001B660 File Offset: 0x00019860
		private static void SetShaderTimeValues(CommandBuffer cmd, float time, float deltaTime, float smoothDeltaTime)
		{
			float num = time / 8f;
			float num2 = time / 4f;
			float num3 = time / 2f;
			Vector4 vector = time * new Vector4(0.05f, 1f, 2f, 3f);
			Vector4 vector2 = new Vector4(Mathf.Sin(num), Mathf.Sin(num2), Mathf.Sin(num3), Mathf.Sin(time));
			Vector4 vector3 = new Vector4(Mathf.Cos(num), Mathf.Cos(num2), Mathf.Cos(num3), Mathf.Cos(time));
			Vector4 vector4 = new Vector4(deltaTime, 1f / deltaTime, smoothDeltaTime, 1f / smoothDeltaTime);
			Vector4 vector5 = new Vector4(time, Mathf.Sin(time), Mathf.Cos(time), 0f);
			cmd.SetGlobalVector(ShaderPropertyId.time, vector);
			cmd.SetGlobalVector(ShaderPropertyId.sinTime, vector2);
			cmd.SetGlobalVector(ShaderPropertyId.cosTime, vector3);
			cmd.SetGlobalVector(ShaderPropertyId.deltaTime, vector4);
			cmd.SetGlobalVector(ShaderPropertyId.timeParameters, vector5);
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001B753 File Offset: 0x00019953
		[Obsolete("Use cameraColorTargetHandle")]
		public RenderTargetIdentifier cameraColorTarget
		{
			get
			{
				if (!this.m_IsPipelineExecuting)
				{
					Debug.LogError("You can only call cameraColorTarget inside the scope of a ScriptableRenderPass. Otherwise the pipeline camera target texture might have not been created or might have already been disposed.");
					return BuiltinRenderTextureType.None;
				}
				return this.m_CameraColorTarget.nameID;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x0001B779 File Offset: 0x00019979
		public RTHandle cameraColorTargetHandle
		{
			get
			{
				if (!this.m_IsPipelineExecuting)
				{
					Debug.LogError("You can only call cameraColorTarget inside the scope of a ScriptableRenderPass. Otherwise the pipeline camera target texture might have not been created or might have already been disposed.");
					return null;
				}
				return this.m_CameraColorTarget.handle;
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001B79A File Offset: 0x0001999A
		internal virtual RTHandle GetCameraColorFrontBuffer(CommandBuffer cmd)
		{
			return null;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001B79D File Offset: 0x0001999D
		internal virtual RTHandle GetCameraColorBackBuffer(CommandBuffer cmd)
		{
			return null;
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0001B7A0 File Offset: 0x000199A0
		[Obsolete("Use cameraDepthTargetHandle")]
		public RenderTargetIdentifier cameraDepthTarget
		{
			get
			{
				if (!this.m_IsPipelineExecuting)
				{
					Debug.LogError("You can only call cameraDepthTarget inside the scope of a ScriptableRenderPass. Otherwise the pipeline camera target texture might have not been created or might have already been disposed.");
					return BuiltinRenderTextureType.None;
				}
				return this.m_CameraDepthTarget.nameID;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x0001B7C6 File Offset: 0x000199C6
		public RTHandle cameraDepthTargetHandle
		{
			get
			{
				if (!this.m_IsPipelineExecuting)
				{
					Debug.LogError("You can only call cameraDepthTarget inside the scope of a ScriptableRenderPass. Otherwise the pipeline camera target texture might have not been created or might have already been disposed.");
					return null;
				}
				return this.m_CameraDepthTarget.handle;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0001B7E7 File Offset: 0x000199E7
		protected List<ScriptableRendererFeature> rendererFeatures
		{
			get
			{
				return this.m_RendererFeatures;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x0001B7EF File Offset: 0x000199EF
		protected List<ScriptableRenderPass> activeRenderPassQueue
		{
			get
			{
				return this.m_ActiveRenderPassQueue;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0001B7F7 File Offset: 0x000199F7
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x0001B7FF File Offset: 0x000199FF
		public ScriptableRenderer.RenderingFeatures supportedRenderingFeatures { get; set; } = new ScriptableRenderer.RenderingFeatures();

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0001B808 File Offset: 0x00019A08
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x0001B810 File Offset: 0x00019A10
		public GraphicsDeviceType[] unsupportedGraphicsDeviceTypes { get; set; } = new GraphicsDeviceType[0];

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001B81C File Offset: 0x00019A1C
		internal static void ConfigureActiveTarget(RenderTargetIdentifier colorAttachment, RenderTargetIdentifier depthAttachment)
		{
			ScriptableRenderer.m_ActiveColorAttachments[0] = colorAttachment;
			for (int i = 1; i < ScriptableRenderer.m_ActiveColorAttachments.Length; i++)
			{
				ScriptableRenderer.m_ActiveColorAttachments[i] = 0;
			}
			ScriptableRenderer.m_ActiveDepthAttachment = depthAttachment;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0001B85E File Offset: 0x00019A5E
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x0001B866 File Offset: 0x00019A66
		internal bool useDepthPriming { get; set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0001B86F File Offset: 0x00019A6F
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x0001B877 File Offset: 0x00019A77
		internal bool stripShadowsOffVariants { get; set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x0001B880 File Offset: 0x00019A80
		// (set) Token: 0x060004CF RID: 1231 RVA: 0x0001B888 File Offset: 0x00019A88
		internal bool stripAdditionalLightOffVariants { get; set; }

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001B894 File Offset: 0x00019A94
		public ScriptableRenderer(ScriptableRendererData data)
		{
			this.profilingExecute = new ProfilingSampler("ScriptableRenderer.Execute: " + data.name);
			foreach (ScriptableRendererFeature scriptableRendererFeature in data.rendererFeatures)
			{
				if (!(scriptableRendererFeature == null))
				{
					scriptableRendererFeature.Create();
					this.m_RendererFeatures.Add(scriptableRendererFeature);
				}
			}
			this.ResetNativeRenderPassFrameData();
			this.useRenderPassEnabled = data.useNativeRenderPass && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D12;
			this.Clear(CameraRenderType.Base);
			this.m_ActiveRenderPassQueue.Clear();
			if (UniversalRenderPipeline.asset)
			{
				this.m_StoreActionsOptimizationSetting = UniversalRenderPipeline.asset.storeActionsOptimization;
			}
			ScriptableRenderer.m_UseOptimizedStoreActions = this.m_StoreActionsOptimizationSetting != StoreActionsOptimization.Store;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001BA78 File Offset: 0x00019C78
		public void Dispose()
		{
			for (int i = 0; i < this.m_RendererFeatures.Count; i++)
			{
				if (!(this.rendererFeatures[i] == null))
				{
					this.rendererFeatures[i].Dispose();
				}
			}
			this.Dispose(true);
			this.hasReleasedRTs = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001BAD4 File Offset: 0x00019CD4
		protected virtual void Dispose(bool disposing)
		{
			DebugHandler debugHandler = this.DebugHandler;
			if (debugHandler == null)
			{
				return;
			}
			debugHandler.Dispose();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001BAE6 File Offset: 0x00019CE6
		internal virtual void ReleaseRenderTargets()
		{
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001BAE8 File Offset: 0x00019CE8
		[Obsolete("Use RTHandles for colorTarget and depthTarget")]
		public void ConfigureCameraTarget(RenderTargetIdentifier colorTarget, RenderTargetIdentifier depthTarget)
		{
			this.m_CameraColorTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				fallback = colorTarget
			};
			this.m_CameraDepthTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				fallback = depthTarget
			};
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001BB24 File Offset: 0x00019D24
		public void ConfigureCameraTarget(RTHandle colorTarget, RTHandle depthTarget)
		{
			this.m_CameraColorTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				handle = colorTarget
			};
			this.m_CameraDepthTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				handle = depthTarget
			};
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001BB60 File Offset: 0x00019D60
		[Obsolete("Use RTHandles for colorTarget, depthTarget and resolveTarget")]
		internal void ConfigureCameraTarget(RenderTargetIdentifier colorTarget, RenderTargetIdentifier depthTarget, RenderTargetIdentifier resolveTarget)
		{
			this.m_CameraColorTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				fallback = colorTarget
			};
			this.m_CameraDepthTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				fallback = depthTarget
			};
			this.m_CameraResolveTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				fallback = resolveTarget
			};
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001BBB4 File Offset: 0x00019DB4
		internal void ConfigureCameraTarget(RTHandle colorTarget, RTHandle depthTarget, RTHandle resolveTarget)
		{
			this.m_CameraColorTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				handle = colorTarget
			};
			this.m_CameraDepthTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				handle = depthTarget
			};
			this.m_CameraResolveTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				handle = resolveTarget
			};
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001BC08 File Offset: 0x00019E08
		[Obsolete("Use RTHandle for colorTarget")]
		internal void ConfigureCameraColorTarget(RenderTargetIdentifier colorTarget)
		{
			this.m_CameraColorTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				fallback = colorTarget
			};
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001BC2C File Offset: 0x00019E2C
		internal void ConfigureCameraColorTarget(RTHandle colorTarget)
		{
			this.m_CameraColorTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				handle = colorTarget
			};
		}

		// Token: 0x060004DA RID: 1242
		public abstract void Setup(ScriptableRenderContext context, ref RenderingData renderingData);

		// Token: 0x060004DB RID: 1243 RVA: 0x0001BC50 File Offset: 0x00019E50
		public virtual void SetupLights(ScriptableRenderContext context, ref RenderingData renderingData)
		{
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001BC52 File Offset: 0x00019E52
		public virtual void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData)
		{
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001BC54 File Offset: 0x00019E54
		public virtual void FinishRendering(CommandBuffer cmd)
		{
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001BC56 File Offset: 0x00019E56
		internal virtual void OnRecordRenderGraph(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData)
		{
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001BC58 File Offset: 0x00019E58
		private void InitRenderGraphFrame(RenderGraph renderGraph, ref RenderingData renderingData)
		{
			ScriptableRenderer.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<ScriptableRenderer.PassData>("InitFrame", out passData, ScriptableRenderer.Profiling.setupFrameData))
			{
				passData.renderingData = renderingData;
				passData.renderer = this;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<ScriptableRenderer.PassData>(delegate(ScriptableRenderer.PassData data, RenderGraphContext rgContext)
				{
					CommandBuffer cmd = rgContext.cmd;
					float time = Time.time;
					float deltaTime = Time.deltaTime;
					float smoothDeltaTime = Time.smoothDeltaTime;
					ScriptableRenderer.ClearRenderingState(cmd);
					ScriptableRenderer.SetShaderTimeValues(cmd, time, deltaTime, smoothDeltaTime);
					data.renderer.SetupLights(rgContext.renderContext, ref data.renderingData);
				});
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001BCDC File Offset: 0x00019EDC
		internal void ProcessVFXCameraCommand(RenderGraph renderGraph, ref RenderingData renderingData)
		{
			ScriptableRenderer.VFXProcessCameraPassData vfxprocessCameraPassData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<ScriptableRenderer.VFXProcessCameraPassData>("ProcessVFXCameraCommand", out vfxprocessCameraPassData, ScriptableRenderer.Profiling.vfxProcessCamera))
			{
				vfxprocessCameraPassData.camera = renderingData.cameraData.camera;
				vfxprocessCameraPassData.cullResults = renderingData.cullResults;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<ScriptableRenderer.VFXProcessCameraPassData>(delegate(ScriptableRenderer.VFXProcessCameraPassData data, RenderGraphContext context)
				{
					VFXManager.ProcessCameraCommand(data.camera, context.cmd, default(VFXCameraXRSettings), data.cullResults);
				});
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001BD68 File Offset: 0x00019F68
		internal void SetupRenderGraphCameraProperties(RenderGraph renderGraph, ref RenderingData renderingData, bool isTargetBackbuffer)
		{
			ScriptableRenderer.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<ScriptableRenderer.PassData>("SetupCameraProperties", out passData, ScriptableRenderer.Profiling.setupCamera))
			{
				passData.renderer = this;
				passData.renderingData = renderingData;
				passData.cameraData = renderingData.cameraData;
				passData.isTargetBackbuffer = isTargetBackbuffer;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<ScriptableRenderer.PassData>(delegate(ScriptableRenderer.PassData data, RenderGraphContext context)
				{
					if (data.cameraData.renderType == CameraRenderType.Base)
					{
						context.renderContext.SetupCameraProperties(data.cameraData.camera, false);
						data.renderer.SetPerCameraShaderVariables(context.cmd, ref data.cameraData, !data.isTargetBackbuffer);
					}
					else
					{
						data.renderer.SetPerCameraShaderVariables(context.cmd, ref data.cameraData, !data.isTargetBackbuffer);
						data.renderer.SetPerCameraClippingPlaneProperties(context.cmd, in data.cameraData, !data.isTargetBackbuffer);
						data.renderer.SetPerCameraBillboardProperties(context.cmd, ref data.cameraData);
					}
					float time = Time.time;
					float deltaTime = Time.deltaTime;
					float smoothDeltaTime = Time.smoothDeltaTime;
					ScriptableRenderer.SetShaderTimeValues(context.cmd, time, deltaTime, smoothDeltaTime);
				});
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001BDFC File Offset: 0x00019FFC
		internal void DrawRenderGraphGizmos(RenderGraph renderGraph, TextureHandle color, TextureHandle depth, GizmoSubset gizmoSubset, ref RenderingData renderingData)
		{
			ScriptableRenderer.DrawGizmosPassData drawGizmosPassData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<ScriptableRenderer.DrawGizmosPassData>("Draw Gizmos Pass", out drawGizmosPassData, ScriptableRenderer.Profiling.drawGizmos))
			{
				renderGraphBuilder.UseColorBuffer(in color, 0);
				renderGraphBuilder.UseDepthBuffer(in depth, DepthAccess.Read);
				drawGizmosPassData.renderingData = renderingData;
				drawGizmosPassData.renderer = this;
				drawGizmosPassData.gizmoSubset = gizmoSubset;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<ScriptableRenderer.DrawGizmosPassData>(delegate(ScriptableRenderer.DrawGizmosPassData data, RenderGraphContext rgContext)
				{
					RenderingData renderingData2 = data.renderingData;
				});
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001BE9C File Offset: 0x0001A09C
		private void BeginRenderGraphXRRendering(RenderGraph renderGraph, ref RenderingData renderingData)
		{
			ScriptableRenderer.BeginXRPassData beginXRPassData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<ScriptableRenderer.BeginXRPassData>("BeginXRRendering", out beginXRPassData, ScriptableRenderer.Profiling.beginXRRendering))
			{
				beginXRPassData.renderingData = renderingData;
				beginXRPassData.cameraData = renderingData.cameraData;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<ScriptableRenderer.BeginXRPassData>(delegate(ScriptableRenderer.BeginXRPassData data, RenderGraphContext context)
				{
					CameraData cameraData = data.cameraData;
					CommandBuffer commandBuffer = data.renderingData.commandBuffer;
					if (cameraData.xr.enabled)
					{
						if (cameraData.xrUniversal.isLateLatchEnabled)
						{
							cameraData.xrUniversal.canMarkLateLatch = true;
						}
						cameraData.xr.StartSinglePass(commandBuffer);
						context.renderContext.ExecuteCommandBuffer(commandBuffer);
						commandBuffer.Clear();
					}
				});
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001BF24 File Offset: 0x0001A124
		private void EndRenderGraphXRRendering(RenderGraph renderGraph, ref RenderingData renderingData)
		{
			ScriptableRenderer.EndXRPassData endXRPassData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<ScriptableRenderer.EndXRPassData>("EndXRRendering", out endXRPassData, ScriptableRenderer.Profiling.endXRRendering))
			{
				endXRPassData.renderingData = renderingData;
				endXRPassData.cameraData = renderingData.cameraData;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<ScriptableRenderer.EndXRPassData>(delegate(ScriptableRenderer.EndXRPassData data, RenderGraphContext context)
				{
					CameraData cameraData = data.cameraData;
					CommandBuffer commandBuffer = data.renderingData.commandBuffer;
					if (cameraData.xr.enabled)
					{
						cameraData.xr.StopSinglePass(commandBuffer);
						context.renderContext.ExecuteCommandBuffer(commandBuffer);
						commandBuffer.Clear();
					}
				});
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001BFAC File Offset: 0x0001A1AC
		internal void RecordRenderGraph(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.sortRenderPasses))
			{
				ScriptableRenderer.SortStable(this.m_ActiveRenderPassQueue);
			}
			this.InitRenderGraphFrame(renderGraph, ref renderingData);
			this.BeginRenderGraphXRRendering(renderGraph, ref renderingData);
			this.OnRecordRenderGraph(renderGraph, context, ref renderingData);
			this.EndRenderGraphXRRendering(renderGraph, ref renderingData);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001C014 File Offset: 0x0001A214
		internal void FinishRenderGraphRendering(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.OnFinishRenderGraphRendering(context, ref renderingData);
			this.InternalFinishRendering(renderingData.cameraData.resolveFinalTarget, renderingData);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001C035 File Offset: 0x0001A235
		internal virtual void OnFinishRenderGraphRendering(ScriptableRenderContext context, ref RenderingData renderingData)
		{
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001C038 File Offset: 0x0001A238
		internal void RecordCustomRenderGraphPasses(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData, RenderPassEvent injectionPoint)
		{
			int renderPassEventRange = ScriptableRenderPass.GetRenderPassEventRange(injectionPoint);
			int num = (int)(injectionPoint + renderPassEventRange);
			foreach (ScriptableRenderPass scriptableRenderPass in this.m_ActiveRenderPassQueue)
			{
				if (scriptableRenderPass.renderPassEvent >= injectionPoint && scriptableRenderPass.renderPassEvent < (RenderPassEvent)num)
				{
					scriptableRenderPass.RecordRenderGraph(renderGraph, ref renderingData);
				}
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001C0AC File Offset: 0x0001A2AC
		internal void SetPerCameraProperties(ScriptableRenderContext context, ref CameraData cameraData, Camera camera, CommandBuffer cmd)
		{
			if (cameraData.renderType == CameraRenderType.Base)
			{
				context.SetupCameraProperties(camera, false);
				this.SetPerCameraShaderVariables(cmd, ref cameraData);
				return;
			}
			this.SetPerCameraShaderVariables(cmd, ref cameraData);
			this.SetPerCameraClippingPlaneProperties(cmd, ref cameraData);
			this.SetPerCameraBillboardProperties(cmd, ref cameraData);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001C0E4 File Offset: 0x0001A2E4
		public void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			bool flag = DebugDisplaySettings<UniversalRenderPipelineDebugDisplaySettings>.Instance.renderingSettings.sceneOverrideMode == DebugSceneOverrideMode.None;
			this.hasReleasedRTs = false;
			this.m_IsPipelineExecuting = true;
			ref CameraData ptr = ref renderingData.cameraData;
			Camera camera = ptr.camera;
			if (this.rendererFeatures.Count != 0 && !renderingData.cameraData.isPreviewCamera)
			{
				this.SetupRenderPasses(in renderingData);
			}
			CommandBuffer commandBuffer = CommandBufferPool.Get();
			CommandBuffer commandBuffer2 = (renderingData.cameraData.xr.enabled ? null : commandBuffer);
			using (new ProfilingScope(commandBuffer2, this.profilingExecute))
			{
				this.InternalStartRendering(context, ref renderingData);
				float time = Time.time;
				float deltaTime = Time.deltaTime;
				float smoothDeltaTime = Time.smoothDeltaTime;
				ScriptableRenderer.ClearRenderingState(commandBuffer);
				ScriptableRenderer.SetShaderTimeValues(commandBuffer, time, deltaTime, smoothDeltaTime);
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				using (new ProfilingScope(null, ScriptableRenderer.Profiling.sortRenderPasses))
				{
					ScriptableRenderer.SortStable(this.m_ActiveRenderPassQueue);
				}
				using (new ProfilingScope(null, ScriptableRenderer.Profiling.RenderPass.configure))
				{
					foreach (ScriptableRenderPass scriptableRenderPass in this.activeRenderPassQueue)
					{
						scriptableRenderPass.Configure(commandBuffer, ptr.cameraTargetDescriptor);
					}
					context.ExecuteCommandBuffer(commandBuffer);
					commandBuffer.Clear();
				}
				this.SetupNativeRenderPassFrameData(ref ptr, this.useRenderPassEnabled);
				ScriptableRenderer.RenderBlocks renderBlocks = new ScriptableRenderer.RenderBlocks(this.m_ActiveRenderPassQueue);
				try
				{
					using (new ProfilingScope(null, ScriptableRenderer.Profiling.setupLights))
					{
						this.SetupLights(context, ref renderingData);
					}
					using (new ProfilingScope(null, ScriptableRenderer.Profiling.setupCamera))
					{
						this.SetPerCameraProperties(context, ref ptr, camera, commandBuffer);
						VFXManager.ProcessCameraCommand(camera, commandBuffer, default(VFXCameraXRSettings), renderingData.cullResults);
						context.ExecuteCommandBuffer(commandBuffer);
						commandBuffer.Clear();
					}
					if (renderBlocks.GetLength(ScriptableRenderer.RenderPassBlock.BeforeRendering) > 0)
					{
						using (new ProfilingScope(null, ScriptableRenderer.Profiling.RenderBlock.beforeRendering))
						{
							this.ExecuteBlock(ScriptableRenderer.RenderPassBlock.BeforeRendering, in renderBlocks, context, ref renderingData, false);
						}
					}
					using (new ProfilingScope(null, ScriptableRenderer.Profiling.setupCamera))
					{
						this.SetPerCameraProperties(context, ref ptr, camera, commandBuffer);
						ScriptableRenderer.SetShaderTimeValues(commandBuffer, time, deltaTime, smoothDeltaTime);
					}
					context.ExecuteCommandBuffer(commandBuffer);
					commandBuffer.Clear();
					this.BeginXRRendering(commandBuffer, context, ref renderingData.cameraData);
					if (renderBlocks.GetLength(ScriptableRenderer.RenderPassBlock.MainRenderingOpaque) > 0)
					{
						using (new ProfilingScope(null, ScriptableRenderer.Profiling.RenderBlock.mainRenderingOpaque))
						{
							this.ExecuteBlock(ScriptableRenderer.RenderPassBlock.MainRenderingOpaque, in renderBlocks, context, ref renderingData, false);
						}
					}
					if (renderBlocks.GetLength(ScriptableRenderer.RenderPassBlock.MainRenderingTransparent) > 0)
					{
						using (new ProfilingScope(null, ScriptableRenderer.Profiling.RenderBlock.mainRenderingTransparent))
						{
							this.ExecuteBlock(ScriptableRenderer.RenderPassBlock.MainRenderingTransparent, in renderBlocks, context, ref renderingData, false);
						}
					}
					if (ptr.xr.enabled)
					{
						ptr.xrUniversal.canMarkLateLatch = false;
					}
					if (renderBlocks.GetLength(ScriptableRenderer.RenderPassBlock.AfterRendering) > 0)
					{
						using (new ProfilingScope(null, ScriptableRenderer.Profiling.RenderBlock.afterRendering))
						{
							this.ExecuteBlock(ScriptableRenderer.RenderPassBlock.AfterRendering, in renderBlocks, context, ref renderingData, false);
						}
					}
					this.EndXRRendering(commandBuffer, context, ref renderingData.cameraData);
					this.InternalFinishRendering(context, ptr.resolveFinalTarget, renderingData);
					for (int i = 0; i < this.m_ActiveRenderPassQueue.Count; i++)
					{
						this.m_ActiveRenderPassQueue[i].m_ColorAttachmentIndices.Dispose();
						this.m_ActiveRenderPassQueue[i].m_InputAttachmentIndices.Dispose();
					}
				}
				finally
				{
					((IDisposable)renderBlocks).Dispose();
				}
			}
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001C5DC File Offset: 0x0001A7DC
		public void EnqueuePass(ScriptableRenderPass pass)
		{
			this.m_ActiveRenderPassQueue.Add(pass);
			if (this.disableNativeRenderPassInFeatures)
			{
				pass.useNativeRenderPass = false;
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001C5FC File Offset: 0x0001A7FC
		protected static ClearFlag GetCameraClearFlag(ref CameraData cameraData)
		{
			CameraClearFlags clearFlags = cameraData.camera.clearFlags;
			if (cameraData.renderType == CameraRenderType.Overlay)
			{
				if (!cameraData.clearDepth)
				{
					return ClearFlag.None;
				}
				return ClearFlag.DepthStencil;
			}
			else
			{
				DebugHandler debugHandler = cameraData.renderer.DebugHandler;
				if (debugHandler != null && debugHandler.IsActiveForCamera(ref cameraData) && debugHandler.IsScreenClearNeeded)
				{
					return ClearFlag.All;
				}
				if (clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null && cameraData.postProcessEnabled && cameraData.xr.enabled)
				{
					return ClearFlag.All;
				}
				if ((clearFlags != CameraClearFlags.Skybox || !(RenderSettings.skybox != null)) && clearFlags != CameraClearFlags.Nothing)
				{
					return ClearFlag.All;
				}
				if (cameraData.cameraTargetDescriptor.msaaSamples > 1)
				{
					cameraData.camera.backgroundColor = Color.black;
					return ClearFlag.All;
				}
				return ClearFlag.DepthStencil;
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001C6AC File Offset: 0x0001A8AC
		internal void OnPreCullRenderPasses(in CameraData cameraData)
		{
			for (int i = 0; i < this.rendererFeatures.Count; i++)
			{
				if (this.rendererFeatures[i].isActive)
				{
					this.rendererFeatures[i].OnCameraPreCull(this, in cameraData);
				}
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001C6F8 File Offset: 0x0001A8F8
		internal void AddRenderPasses(ref RenderingData renderingData)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.addRenderPasses))
			{
				for (int i = 0; i < this.rendererFeatures.Count; i++)
				{
					if (this.rendererFeatures[i].isActive)
					{
						if (!this.rendererFeatures[i].SupportsNativeRenderPass())
						{
							this.disableNativeRenderPassInFeatures = true;
						}
						this.rendererFeatures[i].AddRenderPasses(this, ref renderingData);
						this.disableNativeRenderPassInFeatures = false;
					}
				}
				int count = this.activeRenderPassQueue.Count;
				for (int j = count - 1; j >= 0; j--)
				{
					if (this.activeRenderPassQueue[j] == null)
					{
						this.activeRenderPassQueue.RemoveAt(j);
					}
				}
				if (count > 0 && this.m_StoreActionsOptimizationSetting == StoreActionsOptimization.Auto)
				{
					ScriptableRenderer.m_UseOptimizedStoreActions = false;
				}
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001C7D8 File Offset: 0x0001A9D8
		protected void SetupRenderPasses(in RenderingData renderingData)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.setupRenderPasses))
			{
				for (int i = 0; i < this.rendererFeatures.Count; i++)
				{
					if (this.rendererFeatures[i].isActive)
					{
						this.rendererFeatures[i].SetupRenderPasses(this, in renderingData);
					}
				}
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001C850 File Offset: 0x0001AA50
		private static void ClearRenderingState(CommandBuffer cmd)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.clearRenderingState))
			{
				cmd.DisableShaderKeyword("_MAIN_LIGHT_SHADOWS");
				cmd.DisableShaderKeyword("_MAIN_LIGHT_SHADOWS_CASCADE");
				cmd.DisableShaderKeyword("_ADDITIONAL_LIGHTS_VERTEX");
				cmd.DisableShaderKeyword("_ADDITIONAL_LIGHTS");
				cmd.DisableShaderKeyword("_FORWARD_PLUS");
				cmd.DisableShaderKeyword("_ADDITIONAL_LIGHT_SHADOWS");
				cmd.DisableShaderKeyword("_REFLECTION_PROBE_BLENDING");
				cmd.DisableShaderKeyword("_REFLECTION_PROBE_BOX_PROJECTION");
				cmd.DisableShaderKeyword("_SHADOWS_SOFT");
				cmd.DisableShaderKeyword("_SHADOWS_SOFT_LOW");
				cmd.DisableShaderKeyword("_SHADOWS_SOFT_MEDIUM");
				cmd.DisableShaderKeyword("_SHADOWS_SOFT_HIGH");
				cmd.DisableShaderKeyword("_MIXED_LIGHTING_SUBTRACTIVE");
				cmd.DisableShaderKeyword("LIGHTMAP_SHADOW_MIXING");
				cmd.DisableShaderKeyword("SHADOWS_SHADOWMASK");
				cmd.DisableShaderKeyword("_LINEAR_TO_SRGB_CONVERSION");
				cmd.DisableShaderKeyword("_LIGHT_LAYERS");
				cmd.SetGlobalVector(ScreenSpaceAmbientOcclusion.ScreenSpaceAmbientOcclusionPass.s_AmbientOcclusionParamID, Vector4.zero);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001C958 File Offset: 0x0001AB58
		internal void Clear(CameraRenderType cameraType)
		{
			ScriptableRenderer.m_ActiveColorAttachments[0] = BuiltinRenderTextureType.CameraTarget;
			for (int i = 1; i < ScriptableRenderer.m_ActiveColorAttachments.Length; i++)
			{
				ScriptableRenderer.m_ActiveColorAttachments[i] = 0;
			}
			ScriptableRenderer.m_ActiveDepthAttachment = BuiltinRenderTextureType.CameraTarget;
			this.m_FirstTimeCameraColorTargetIsBound = cameraType == CameraRenderType.Base;
			this.m_FirstTimeCameraDepthTargetIsBound = true;
			this.m_CameraColorTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				fallback = BuiltinRenderTextureType.CameraTarget
			};
			this.m_CameraDepthTarget = new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
			{
				fallback = BuiltinRenderTextureType.CameraTarget
			};
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001C9F0 File Offset: 0x0001ABF0
		private void ExecuteBlock(int blockIndex, in ScriptableRenderer.RenderBlocks renderBlocks, ScriptableRenderContext context, ref RenderingData renderingData, bool submit = false)
		{
			ScriptableRenderer.RenderBlocks renderBlocks2 = renderBlocks;
			foreach (int num in renderBlocks2.GetRange(blockIndex))
			{
				ScriptableRenderPass scriptableRenderPass = this.m_ActiveRenderPassQueue[num];
				this.ExecuteRenderPass(context, scriptableRenderPass, ref renderingData);
			}
			if (submit)
			{
				context.Submit();
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001CA6C File Offset: 0x0001AC6C
		private bool IsRenderPassEnabled(ScriptableRenderPass renderPass)
		{
			return renderPass.useNativeRenderPass && renderPass.m_UsesRTHandles && this.useRenderPassEnabled;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001CA88 File Offset: 0x0001AC88
		private void ExecuteRenderPass(ScriptableRenderContext context, ScriptableRenderPass renderPass, ref RenderingData renderingData)
		{
			using (new ProfilingScope(null, renderPass.profilingSampler))
			{
				ref CameraData ptr = ref renderingData.cameraData;
				CommandBuffer commandBuffer = renderingData.commandBuffer;
				if (ptr.xr.supportsFoveatedRendering && renderPass.renderPassEvent >= RenderPassEvent.BeforeRenderingPrePasses && renderPass.renderPassEvent < RenderPassEvent.BeforeRenderingPostProcessing)
				{
					commandBuffer.SetFoveatedRenderingMode(FoveatedRenderingMode.Enabled);
				}
				using (new ProfilingScope(null, ScriptableRenderer.Profiling.RenderPass.setRenderPassAttachments))
				{
					this.SetRenderPassAttachments(commandBuffer, renderPass, ref ptr);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				if (this.IsRenderPassEnabled(renderPass) && ptr.isRenderPassSupportedCamera)
				{
					this.ExecuteNativeRenderPass(context, renderPass, ref ptr, ref renderingData);
				}
				else
				{
					renderPass.Execute(context, ref renderingData);
					context.ExecuteCommandBuffer(commandBuffer);
					commandBuffer.Clear();
				}
				if (ptr.xr.enabled)
				{
					if (ptr.xr.supportsFoveatedRendering)
					{
						commandBuffer.SetFoveatedRenderingMode(FoveatedRenderingMode.Disabled);
					}
					XRSystemUniversal.UnmarkShaderProperties(commandBuffer, ptr.xrUniversal);
					context.ExecuteCommandBuffer(commandBuffer);
					commandBuffer.Clear();
				}
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001CBAC File Offset: 0x0001ADAC
		private void SetRenderPassAttachments(CommandBuffer cmd, ScriptableRenderPass renderPass, ref CameraData cameraData)
		{
			ClearFlag cameraClearFlag = ScriptableRenderer.GetCameraClearFlag(ref cameraData);
			if (RenderingUtils.GetValidColorBufferCount(renderPass.colorAttachments) == 0U)
			{
				return;
			}
			if (RenderingUtils.IsMRT(renderPass.colorAttachments))
			{
				bool flag = false;
				bool flag2 = false;
				int num = RenderingUtils.IndexOf(renderPass.colorAttachments, this.m_CameraColorTarget.nameID);
				if (num != -1 && this.m_FirstTimeCameraColorTargetIsBound)
				{
					this.m_FirstTimeCameraColorTargetIsBound = false;
					flag = (cameraClearFlag & ClearFlag.Color) != (renderPass.clearFlag & ClearFlag.Color) || cameraData.backgroundColor != renderPass.clearColor;
				}
				RenderTargetIdentifier nameID = this.m_CameraDepthTarget.nameID;
				if (cameraData.xr.enabled)
				{
					nameID = new RenderTargetIdentifier(nameID, 0, CubemapFace.Unknown, -1);
				}
				if (renderPass.depthAttachment == nameID && this.m_FirstTimeCameraDepthTargetIsBound)
				{
					this.m_FirstTimeCameraDepthTargetIsBound = false;
					flag2 = (cameraClearFlag & ClearFlag.DepthStencil) != (renderPass.clearFlag & ClearFlag.DepthStencil);
				}
				if (flag)
				{
					if ((cameraClearFlag & ClearFlag.Color) != ClearFlag.None && (!this.IsRenderPassEnabled(renderPass) || !cameraData.isRenderPassSupportedCamera))
					{
						ScriptableRenderer.SetRenderTarget(cmd, renderPass.colorAttachments[num], renderPass.depthAttachment, ClearFlag.Color, cameraData.backgroundColor);
					}
					if ((renderPass.clearFlag & ClearFlag.Color) != ClearFlag.None)
					{
						uint num2 = RenderingUtils.CountDistinct(renderPass.colorAttachments, this.m_CameraColorTarget.nameID);
						RenderTargetIdentifier[] array = ScriptableRenderer.m_TrimmedColorAttachmentCopies[(int)num2];
						int num3 = 0;
						for (int i = 0; i < renderPass.colorAttachments.Length; i++)
						{
							if (renderPass.colorAttachments[i] != this.m_CameraColorTarget.nameID && renderPass.colorAttachments[i] != 0)
							{
								array[num3] = renderPass.colorAttachments[i];
								num3++;
							}
						}
						if ((long)num3 != (long)((ulong)num2))
						{
							Debug.LogError("writeIndex and otherTargetsCount values differed. writeIndex:" + num3.ToString() + " otherTargetsCount:" + num2.ToString());
						}
						if (!this.IsRenderPassEnabled(renderPass) || !cameraData.isRenderPassSupportedCamera)
						{
							ScriptableRenderer.SetRenderTarget(cmd, array, this.m_CameraDepthTarget.nameID, ClearFlag.Color, renderPass.clearColor);
						}
					}
				}
				ClearFlag clearFlag = ClearFlag.None;
				clearFlag |= (flag2 ? (cameraClearFlag & ClearFlag.DepthStencil) : (renderPass.clearFlag & ClearFlag.DepthStencil));
				clearFlag |= (flag ? (this.IsRenderPassEnabled(renderPass) ? (cameraClearFlag & ClearFlag.Color) : ClearFlag.None) : (renderPass.clearFlag & ClearFlag.Color));
				if (this.IsRenderPassEnabled(renderPass) && cameraData.isRenderPassSupportedCamera)
				{
					this.SetNativeRenderPassMRTAttachmentList(renderPass, ref cameraData, flag, clearFlag);
				}
				if (!RenderingUtils.SequenceEqual(renderPass.colorAttachments, ScriptableRenderer.m_ActiveColorAttachments) || renderPass.depthAttachment != ScriptableRenderer.m_ActiveDepthAttachment || clearFlag != ClearFlag.None)
				{
					int num4 = RenderingUtils.LastValid(renderPass.colorAttachments);
					if (num4 >= 0)
					{
						int num5 = num4 + 1;
						RenderTargetIdentifier[] array2 = ScriptableRenderer.m_TrimmedColorAttachmentCopies[num5];
						for (int j = 0; j < num5; j++)
						{
							array2[j] = renderPass.colorAttachments[j];
						}
						if (!this.IsRenderPassEnabled(renderPass) || !cameraData.isRenderPassSupportedCamera)
						{
							if (renderPass.m_UsesRTHandles)
							{
								RTHandle rthandle = this.m_CameraDepthTarget.handle;
								if (renderPass.overrideCameraTarget)
								{
									rthandle = renderPass.depthAttachmentHandle;
								}
								else
								{
									this.m_FirstTimeCameraDepthTargetIsBound = false;
								}
								ScriptableRenderer.SetRenderTarget(cmd, array2, rthandle, clearFlag, renderPass.clearColor);
							}
							else
							{
								RenderTargetIdentifier renderTargetIdentifier = this.m_CameraDepthTarget.nameID;
								if (renderPass.overrideCameraTarget)
								{
									renderTargetIdentifier = renderPass.depthAttachment;
								}
								else
								{
									this.m_FirstTimeCameraDepthTargetIsBound = false;
								}
								ScriptableRenderer.SetRenderTarget(cmd, array2, renderTargetIdentifier, clearFlag, renderPass.clearColor);
							}
						}
						if (cameraData.xr.enabled)
						{
							bool flag3 = RenderingUtils.IndexOf(renderPass.colorAttachments, cameraData.xr.renderTarget) == -1;
							cameraData.PushBuiltinShaderConstantsXR(cmd, flag3);
							XRSystemUniversal.MarkShaderProperties(cmd, cameraData.xrUniversal, flag3);
							return;
						}
					}
				}
			}
			else
			{
				ScriptableRenderer.RTHandleRenderTargetIdentifierCompat rthandleRenderTargetIdentifierCompat = (renderPass.m_UsesRTHandles ? new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
				{
					handle = renderPass.colorAttachmentHandle
				} : new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
				{
					fallback = renderPass.colorAttachment
				});
				ScriptableRenderer.RTHandleRenderTargetIdentifierCompat rthandleRenderTargetIdentifierCompat2 = (renderPass.m_UsesRTHandles ? new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
				{
					handle = renderPass.depthAttachmentHandle
				} : new ScriptableRenderer.RTHandleRenderTargetIdentifierCompat
				{
					fallback = renderPass.depthAttachment
				});
				if (!renderPass.overrideCameraTarget)
				{
					if (renderPass.renderPassEvent < RenderPassEvent.BeforeRenderingPrePasses)
					{
						return;
					}
					rthandleRenderTargetIdentifierCompat = this.m_CameraColorTarget;
					rthandleRenderTargetIdentifierCompat2 = this.m_CameraDepthTarget;
				}
				ClearFlag clearFlag2 = ClearFlag.None;
				Color color;
				if (rthandleRenderTargetIdentifierCompat.nameID == this.m_CameraColorTarget.nameID && this.m_FirstTimeCameraColorTargetIsBound)
				{
					this.m_FirstTimeCameraColorTargetIsBound = false;
					clearFlag2 |= cameraClearFlag & ClearFlag.Color;
					if (SystemInfo.usesLoadStoreActions && new RenderTargetIdentifier(rthandleRenderTargetIdentifierCompat.nameID, 0, CubemapFace.Unknown, 0) != BuiltinRenderTextureType.CameraTarget)
					{
						clearFlag2 |= renderPass.clearFlag;
					}
					color = cameraData.backgroundColor;
					if (this.m_FirstTimeCameraDepthTargetIsBound)
					{
						this.m_FirstTimeCameraDepthTargetIsBound = false;
						clearFlag2 |= cameraClearFlag & ClearFlag.DepthStencil;
					}
				}
				else
				{
					clearFlag2 |= renderPass.clearFlag & ClearFlag.Color;
					color = renderPass.clearColor;
				}
				if (new RenderTargetIdentifier(this.m_CameraDepthTarget.nameID, 0, CubemapFace.Unknown, 0) != BuiltinRenderTextureType.CameraTarget && (rthandleRenderTargetIdentifierCompat2.nameID == this.m_CameraDepthTarget.nameID || rthandleRenderTargetIdentifierCompat.nameID == this.m_CameraDepthTarget.nameID) && this.m_FirstTimeCameraDepthTargetIsBound)
				{
					this.m_FirstTimeCameraDepthTargetIsBound = false;
					clearFlag2 |= cameraClearFlag & ClearFlag.DepthStencil;
				}
				else
				{
					clearFlag2 |= renderPass.clearFlag & ClearFlag.DepthStencil;
				}
				if (this.DebugHandler != null && this.DebugHandler.IsActiveForCamera(ref cameraData))
				{
					this.DebugHandler.TryGetScreenClearColor(ref color);
				}
				if (this.IsRenderPassEnabled(renderPass) && cameraData.isRenderPassSupportedCamera && renderPass.m_UsesRTHandles)
				{
					this.SetNativeRenderPassAttachmentList(renderPass, ref cameraData, rthandleRenderTargetIdentifierCompat.handle, rthandleRenderTargetIdentifierCompat2.handle, clearFlag2, color);
					return;
				}
				bool flag4 = false;
				for (int k = 0; k < ScriptableRenderer.m_ActiveColorAttachments.Length; k++)
				{
					if (renderPass.colorAttachments[k] != ScriptableRenderer.m_ActiveColorAttachments[k])
					{
						flag4 = true;
						break;
					}
				}
				if (flag4 || rthandleRenderTargetIdentifierCompat.nameID != ScriptableRenderer.m_ActiveColorAttachments[0] || rthandleRenderTargetIdentifierCompat2.nameID != ScriptableRenderer.m_ActiveDepthAttachment || clearFlag2 != ClearFlag.None || renderPass.colorStoreActions[0] != ScriptableRenderer.m_ActiveColorStoreActions[0] || renderPass.depthStoreAction != ScriptableRenderer.m_ActiveDepthStoreAction)
				{
					if (rthandleRenderTargetIdentifierCompat.useRTHandle && rthandleRenderTargetIdentifierCompat2.useRTHandle)
					{
						ScriptableRenderer.SetRenderTarget(cmd, rthandleRenderTargetIdentifierCompat.handle, rthandleRenderTargetIdentifierCompat2.handle, clearFlag2, color, renderPass.colorStoreActions[0], renderPass.depthStoreAction);
					}
					else
					{
						ScriptableRenderer.SetRenderTarget(cmd, rthandleRenderTargetIdentifierCompat.fallback, rthandleRenderTargetIdentifierCompat2.fallback, clearFlag2, color, renderPass.colorStoreActions[0], renderPass.depthStoreAction);
					}
					if (cameraData.xr.enabled)
					{
						bool flag5 = rthandleRenderTargetIdentifierCompat.nameID != cameraData.xr.renderTarget;
						cameraData.PushBuiltinShaderConstantsXR(cmd, flag5);
						XRSystemUniversal.MarkShaderProperties(cmd, cameraData.xrUniversal, flag5);
					}
				}
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001D27C File Offset: 0x0001B47C
		private void BeginXRRendering(CommandBuffer cmd, ScriptableRenderContext context, ref CameraData cameraData)
		{
			if (cameraData.xr.enabled)
			{
				if (cameraData.xrUniversal.isLateLatchEnabled)
				{
					cameraData.xrUniversal.canMarkLateLatch = true;
				}
				cameraData.xr.StartSinglePass(cmd);
				if (cameraData.xr.supportsFoveatedRendering)
				{
					cmd.ConfigureFoveatedRendering(cameraData.xr.foveatedRenderingInfo);
					if (XRSystem.foveatedRenderingCaps.HasFlag(FoveatedRenderingCaps.NonUniformRaster))
					{
						cmd.EnableShaderKeyword("_FOVEATED_RENDERING_NON_UNIFORM_RASTER");
					}
				}
				context.ExecuteCommandBuffer(cmd);
				cmd.Clear();
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001D30C File Offset: 0x0001B50C
		private void EndXRRendering(CommandBuffer cmd, ScriptableRenderContext context, ref CameraData cameraData)
		{
			if (cameraData.xr.enabled)
			{
				cameraData.xr.StopSinglePass(cmd);
				if (XRSystem.foveatedRenderingCaps != FoveatedRenderingCaps.None)
				{
					if (XRSystem.foveatedRenderingCaps.HasFlag(FoveatedRenderingCaps.NonUniformRaster))
					{
						cmd.DisableShaderKeyword("_FOVEATED_RENDERING_NON_UNIFORM_RASTER");
					}
					cmd.ConfigureFoveatedRendering(IntPtr.Zero);
				}
				context.ExecuteCommandBuffer(cmd);
				cmd.Clear();
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001D374 File Offset: 0x0001B574
		[Obsolete("Use RTHandles for colorAttachment and depthAttachment")]
		internal static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier colorAttachment, RenderTargetIdentifier depthAttachment, ClearFlag clearFlag, Color clearColor)
		{
			ScriptableRenderer.m_ActiveColorAttachments[0] = colorAttachment;
			for (int i = 1; i < ScriptableRenderer.m_ActiveColorAttachments.Length; i++)
			{
				ScriptableRenderer.m_ActiveColorAttachments[i] = 0;
			}
			ScriptableRenderer.m_ActiveColorStoreActions[0] = RenderBufferStoreAction.Store;
			ScriptableRenderer.m_ActiveDepthStoreAction = RenderBufferStoreAction.Store;
			for (int j = 1; j < ScriptableRenderer.m_ActiveColorStoreActions.Length; j++)
			{
				ScriptableRenderer.m_ActiveColorStoreActions[j] = RenderBufferStoreAction.Store;
			}
			ScriptableRenderer.m_ActiveDepthAttachment = depthAttachment;
			RenderBufferLoadAction renderBufferLoadAction = (((clearFlag & ClearFlag.Color) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
			RenderBufferLoadAction renderBufferLoadAction2 = (((clearFlag & ClearFlag.Depth) != ClearFlag.None || (clearFlag & ClearFlag.Stencil) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
			ScriptableRenderer.SetRenderTarget(cmd, colorAttachment, renderBufferLoadAction, RenderBufferStoreAction.Store, depthAttachment, renderBufferLoadAction2, RenderBufferStoreAction.Store, clearFlag, clearColor);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001D408 File Offset: 0x0001B608
		internal static void SetRenderTarget(CommandBuffer cmd, RTHandle colorAttachment, RTHandle depthAttachment, ClearFlag clearFlag, Color clearColor)
		{
			ScriptableRenderer.m_ActiveColorAttachments[0] = colorAttachment;
			for (int i = 1; i < ScriptableRenderer.m_ActiveColorAttachments.Length; i++)
			{
				ScriptableRenderer.m_ActiveColorAttachments[i] = 0;
			}
			ScriptableRenderer.m_ActiveColorStoreActions[0] = RenderBufferStoreAction.Store;
			ScriptableRenderer.m_ActiveDepthStoreAction = RenderBufferStoreAction.Store;
			for (int j = 1; j < ScriptableRenderer.m_ActiveColorStoreActions.Length; j++)
			{
				ScriptableRenderer.m_ActiveColorStoreActions[j] = RenderBufferStoreAction.Store;
			}
			ScriptableRenderer.m_ActiveDepthAttachment = depthAttachment;
			RenderBufferLoadAction renderBufferLoadAction = (((clearFlag & ClearFlag.Color) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
			RenderBufferLoadAction renderBufferLoadAction2 = (((clearFlag & ClearFlag.Depth) != ClearFlag.None || (clearFlag & ClearFlag.Stencil) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
			if (colorAttachment.rt == null && depthAttachment.rt == null && depthAttachment.nameID == ScriptableRenderer.k_CameraTarget.nameID)
			{
				ScriptableRenderer.SetRenderTarget(cmd, colorAttachment, renderBufferLoadAction, RenderBufferStoreAction.Store, colorAttachment, renderBufferLoadAction2, RenderBufferStoreAction.Store, clearFlag, clearColor);
				return;
			}
			ScriptableRenderer.SetRenderTarget(cmd, colorAttachment, renderBufferLoadAction, RenderBufferStoreAction.Store, depthAttachment, renderBufferLoadAction2, RenderBufferStoreAction.Store, clearFlag, clearColor);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001D4E8 File Offset: 0x0001B6E8
		[Obsolete("Use RTHandles for colorAttachment and depthAttachment")]
		internal static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier colorAttachment, RenderTargetIdentifier depthAttachment, ClearFlag clearFlag, Color clearColor, RenderBufferStoreAction colorStoreAction, RenderBufferStoreAction depthStoreAction)
		{
			ScriptableRenderer.m_ActiveColorAttachments[0] = colorAttachment;
			for (int i = 1; i < ScriptableRenderer.m_ActiveColorAttachments.Length; i++)
			{
				ScriptableRenderer.m_ActiveColorAttachments[i] = 0;
			}
			ScriptableRenderer.m_ActiveColorStoreActions[0] = colorStoreAction;
			ScriptableRenderer.m_ActiveDepthStoreAction = depthStoreAction;
			for (int j = 1; j < ScriptableRenderer.m_ActiveColorStoreActions.Length; j++)
			{
				ScriptableRenderer.m_ActiveColorStoreActions[j] = RenderBufferStoreAction.Store;
			}
			ScriptableRenderer.m_ActiveDepthAttachment = depthAttachment;
			RenderBufferLoadAction renderBufferLoadAction = (((clearFlag & ClearFlag.Color) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
			RenderBufferLoadAction renderBufferLoadAction2 = (((clearFlag & ClearFlag.Depth) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
			if (!ScriptableRenderer.m_UseOptimizedStoreActions)
			{
				if (colorStoreAction != RenderBufferStoreAction.StoreAndResolve)
				{
					colorStoreAction = RenderBufferStoreAction.Store;
				}
				if (depthStoreAction != RenderBufferStoreAction.StoreAndResolve)
				{
					depthStoreAction = RenderBufferStoreAction.Store;
				}
			}
			ScriptableRenderer.SetRenderTarget(cmd, colorAttachment, renderBufferLoadAction, colorStoreAction, depthAttachment, renderBufferLoadAction2, depthStoreAction, clearFlag, clearColor);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001D590 File Offset: 0x0001B790
		internal static void SetRenderTarget(CommandBuffer cmd, RTHandle colorAttachment, RTHandle depthAttachment, ClearFlag clearFlag, Color clearColor, RenderBufferStoreAction colorStoreAction, RenderBufferStoreAction depthStoreAction)
		{
			ScriptableRenderer.m_ActiveColorAttachments[0] = colorAttachment;
			for (int i = 1; i < ScriptableRenderer.m_ActiveColorAttachments.Length; i++)
			{
				ScriptableRenderer.m_ActiveColorAttachments[i] = 0;
			}
			ScriptableRenderer.m_ActiveColorStoreActions[0] = colorStoreAction;
			ScriptableRenderer.m_ActiveDepthStoreAction = depthStoreAction;
			for (int j = 1; j < ScriptableRenderer.m_ActiveColorStoreActions.Length; j++)
			{
				ScriptableRenderer.m_ActiveColorStoreActions[j] = RenderBufferStoreAction.Store;
			}
			ScriptableRenderer.m_ActiveDepthAttachment = depthAttachment;
			RenderBufferLoadAction renderBufferLoadAction = (((clearFlag & ClearFlag.Color) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
			RenderBufferLoadAction renderBufferLoadAction2 = (((clearFlag & ClearFlag.Depth) != ClearFlag.None) ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
			if (!ScriptableRenderer.m_UseOptimizedStoreActions)
			{
				if (colorStoreAction != RenderBufferStoreAction.StoreAndResolve)
				{
					colorStoreAction = RenderBufferStoreAction.Store;
				}
				if (depthStoreAction != RenderBufferStoreAction.StoreAndResolve)
				{
					depthStoreAction = RenderBufferStoreAction.Store;
				}
			}
			ScriptableRenderer.SetRenderTarget(cmd, colorAttachment, renderBufferLoadAction, colorStoreAction, depthAttachment, renderBufferLoadAction2, depthStoreAction, clearFlag, clearColor);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001D642 File Offset: 0x0001B842
		[Obsolete("Use RTHandle for colorAttachment")]
		private static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier colorAttachment, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, ClearFlag clearFlags, Color clearColor)
		{
			CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, clearFlags, clearColor);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001D654 File Offset: 0x0001B854
		private static void SetRenderTarget(CommandBuffer cmd, RTHandle colorAttachment, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, ClearFlag clearFlags, Color clearColor)
		{
			CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, clearFlags, clearColor, 0, CubemapFace.Unknown, -1);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001D674 File Offset: 0x0001B874
		[Obsolete("Use RTHandles for colorAttachment and depthAttachment")]
		private static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier colorAttachment, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderTargetIdentifier depthAttachment, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, ClearFlag clearFlags, Color clearColor)
		{
			if (depthAttachment == BuiltinRenderTextureType.CameraTarget)
			{
				CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, depthLoadAction, depthStoreAction, clearFlags, clearColor);
				return;
			}
			CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, depthAttachment, depthLoadAction, depthStoreAction, clearFlags, clearColor);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001D6B8 File Offset: 0x0001B8B8
		private static void SetRenderTarget(CommandBuffer cmd, RTHandle colorAttachment, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RTHandle depthAttachment, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, ClearFlag clearFlags, Color clearColor)
		{
			if (depthAttachment.nameID == BuiltinRenderTextureType.CameraTarget)
			{
				CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, colorAttachment, depthLoadAction, depthStoreAction, clearFlags, clearColor, 0, CubemapFace.Unknown, -1);
				return;
			}
			CoreUtils.SetRenderTarget(cmd, colorAttachment, colorLoadAction, colorStoreAction, depthAttachment, depthLoadAction, depthStoreAction, clearFlags, clearColor, 0, CubemapFace.Unknown, -1);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001D705 File Offset: 0x0001B905
		[Obsolete("Use RTHandles for colorAttachments and depthAttachment")]
		private static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier[] colorAttachments, RenderTargetIdentifier depthAttachment, ClearFlag clearFlag, Color clearColor)
		{
			ScriptableRenderer.m_ActiveColorAttachments = colorAttachments;
			ScriptableRenderer.m_ActiveDepthAttachment = depthAttachment;
			CoreUtils.SetRenderTarget(cmd, colorAttachments, depthAttachment, clearFlag, clearColor);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001D71E File Offset: 0x0001B91E
		[Obsolete("Use RTHandle for colorAttachments")]
		private static void SetRenderTarget(CommandBuffer cmd, RenderTargetIdentifier[] colorAttachments, RTHandle depthAttachment, ClearFlag clearFlag, Color clearColor)
		{
			ScriptableRenderer.m_ActiveColorAttachments = colorAttachments;
			ScriptableRenderer.m_ActiveDepthAttachment = depthAttachment.nameID;
			CoreUtils.SetRenderTarget(cmd, ScriptableRenderer.m_ActiveColorAttachments, depthAttachment, clearFlag, clearColor);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001D740 File Offset: 0x0001B940
		private static void SetRenderTarget(CommandBuffer cmd, RTHandle[] colorAttachments, RTHandle depthAttachment, ClearFlag clearFlag, Color clearColor)
		{
			if (ScriptableRenderer.m_ActiveColorAttachments.Length != colorAttachments.Length)
			{
				ScriptableRenderer.m_ActiveColorAttachments = new RenderTargetIdentifier[colorAttachments.Length];
			}
			for (int i = 0; i < colorAttachments.Length; i++)
			{
				ScriptableRenderer.m_ActiveColorAttachments[i] = colorAttachments[i].nameID;
			}
			ScriptableRenderer.m_ActiveDepthAttachment = depthAttachment.nameID;
			CoreUtils.SetRenderTarget(cmd, ScriptableRenderer.m_ActiveColorAttachments, depthAttachment, clearFlag, clearColor);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001D7A1 File Offset: 0x0001B9A1
		internal virtual void SwapColorBuffer(CommandBuffer cmd)
		{
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001D7A3 File Offset: 0x0001B9A3
		internal virtual void EnableSwapBufferMSAA(bool enable)
		{
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001D7A5 File Offset: 0x0001B9A5
		[Conditional("UNITY_EDITOR")]
		private void DrawGizmos(ScriptableRenderContext context, Camera camera, GizmoSubset gizmoSubset, ref RenderingData renderingData)
		{
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001D7A7 File Offset: 0x0001B9A7
		[Conditional("UNITY_EDITOR")]
		private void DrawWireOverlay(ScriptableRenderContext context, Camera camera)
		{
			context.DrawWireOverlay(camera);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001D7B4 File Offset: 0x0001B9B4
		private void InternalStartRendering(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.internalStartRendering))
			{
				for (int i = 0; i < this.m_ActiveRenderPassQueue.Count; i++)
				{
					this.m_ActiveRenderPassQueue[i].OnCameraSetup(renderingData.commandBuffer, ref renderingData);
				}
			}
			context.ExecuteCommandBuffer(renderingData.commandBuffer);
			renderingData.commandBuffer.Clear();
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001D834 File Offset: 0x0001BA34
		private void InternalFinishRendering(bool resolveFinalTarget, RenderingData renderingData)
		{
			using (new ProfilingScope(null, ScriptableRenderer.Profiling.internalFinishRendering))
			{
				for (int i = 0; i < this.m_ActiveRenderPassQueue.Count; i++)
				{
					this.m_ActiveRenderPassQueue[i].FrameCleanup(renderingData.commandBuffer);
				}
				if (resolveFinalTarget)
				{
					for (int j = 0; j < this.m_ActiveRenderPassQueue.Count; j++)
					{
						this.m_ActiveRenderPassQueue[j].OnFinishCameraStackRendering(renderingData.commandBuffer);
					}
					this.FinishRendering(renderingData.commandBuffer);
					this.m_IsPipelineExecuting = false;
				}
				this.m_ActiveRenderPassQueue.Clear();
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001D8EC File Offset: 0x0001BAEC
		private void InternalFinishRendering(ScriptableRenderContext context, bool resolveFinalTarget, RenderingData renderingData)
		{
			this.InternalFinishRendering(resolveFinalTarget, renderingData);
			this.ResetNativeRenderPassFrameData();
			context.ExecuteCommandBuffer(renderingData.commandBuffer);
			renderingData.commandBuffer.Clear();
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001D914 File Offset: 0x0001BB14
		internal static void SortStable(List<ScriptableRenderPass> list)
		{
			for (int i = 1; i < list.Count; i++)
			{
				ScriptableRenderPass scriptableRenderPass = list[i];
				int num = i - 1;
				while (num >= 0 && scriptableRenderPass < list[num])
				{
					list[num + 1] = list[num];
					num--;
				}
				list[num + 1] = scriptableRenderPass;
			}
		}

		// Token: 0x04000370 RID: 880
		private const int kRenderPassMapSize = 10;

		// Token: 0x04000371 RID: 881
		private const int kRenderPassMaxCount = 20;

		// Token: 0x04000372 RID: 882
		private int m_LastBeginSubpassPassIndex;

		// Token: 0x04000373 RID: 883
		private Dictionary<Hash128, int[]> m_MergeableRenderPassesMap = new Dictionary<Hash128, int[]>(10);

		// Token: 0x04000374 RID: 884
		private int[][] m_MergeableRenderPassesMapArrays;

		// Token: 0x04000375 RID: 885
		private Hash128[] m_PassIndexToPassHash = new Hash128[20];

		// Token: 0x04000376 RID: 886
		private Dictionary<Hash128, int> m_RenderPassesAttachmentCount = new Dictionary<Hash128, int>(10);

		// Token: 0x04000377 RID: 887
		private AttachmentDescriptor[] m_ActiveColorAttachmentDescriptors = new AttachmentDescriptor[]
		{
			RenderingUtils.emptyAttachment,
			RenderingUtils.emptyAttachment,
			RenderingUtils.emptyAttachment,
			RenderingUtils.emptyAttachment,
			RenderingUtils.emptyAttachment,
			RenderingUtils.emptyAttachment,
			RenderingUtils.emptyAttachment,
			RenderingUtils.emptyAttachment
		};

		// Token: 0x04000378 RID: 888
		private AttachmentDescriptor m_ActiveDepthAttachmentDescriptor;

		// Token: 0x04000379 RID: 889
		private bool[] m_IsActiveColorAttachmentTransient = new bool[8];

		// Token: 0x0400037A RID: 890
		internal RenderBufferStoreAction[] m_FinalColorStoreAction = new RenderBufferStoreAction[8];

		// Token: 0x0400037B RID: 891
		internal RenderBufferStoreAction m_FinalDepthStoreAction;

		// Token: 0x0400037D RID: 893
		internal bool hasReleasedRTs = true;

		// Token: 0x0400037F RID: 895
		internal static ScriptableRenderer current = null;

		// Token: 0x04000382 RID: 898
		private StoreActionsOptimization m_StoreActionsOptimizationSetting;

		// Token: 0x04000383 RID: 899
		private static bool m_UseOptimizedStoreActions = false;

		// Token: 0x04000384 RID: 900
		private const int k_RenderPassBlockCount = 4;

		// Token: 0x04000385 RID: 901
		protected static readonly RTHandle k_CameraTarget = RTHandles.Alloc(BuiltinRenderTextureType.CameraTarget);

		// Token: 0x04000386 RID: 902
		private List<ScriptableRenderPass> m_ActiveRenderPassQueue = new List<ScriptableRenderPass>(32);

		// Token: 0x04000387 RID: 903
		private List<ScriptableRendererFeature> m_RendererFeatures = new List<ScriptableRendererFeature>(10);

		// Token: 0x04000388 RID: 904
		private ScriptableRenderer.RTHandleRenderTargetIdentifierCompat m_CameraColorTarget;

		// Token: 0x04000389 RID: 905
		private ScriptableRenderer.RTHandleRenderTargetIdentifierCompat m_CameraDepthTarget;

		// Token: 0x0400038A RID: 906
		private ScriptableRenderer.RTHandleRenderTargetIdentifierCompat m_CameraResolveTarget;

		// Token: 0x0400038B RID: 907
		private bool m_FirstTimeCameraColorTargetIsBound = true;

		// Token: 0x0400038C RID: 908
		private bool m_FirstTimeCameraDepthTargetIsBound = true;

		// Token: 0x0400038D RID: 909
		private bool m_IsPipelineExecuting;

		// Token: 0x0400038E RID: 910
		internal bool disableNativeRenderPassInFeatures;

		// Token: 0x0400038F RID: 911
		internal bool useRenderPassEnabled;

		// Token: 0x04000390 RID: 912
		private static RenderTargetIdentifier[] m_ActiveColorAttachments = new RenderTargetIdentifier[] { 0, 0, 0, 0, 0, 0, 0, 0 };

		// Token: 0x04000391 RID: 913
		private static RenderTargetIdentifier m_ActiveDepthAttachment;

		// Token: 0x04000392 RID: 914
		private static RenderBufferStoreAction[] m_ActiveColorStoreActions = new RenderBufferStoreAction[8];

		// Token: 0x04000393 RID: 915
		private static RenderBufferStoreAction m_ActiveDepthStoreAction = RenderBufferStoreAction.Store;

		// Token: 0x04000394 RID: 916
		private static RenderTargetIdentifier[][] m_TrimmedColorAttachmentCopies = new RenderTargetIdentifier[][]
		{
			new RenderTargetIdentifier[0],
			new RenderTargetIdentifier[] { 0 },
			new RenderTargetIdentifier[] { 0, 0 },
			new RenderTargetIdentifier[] { 0, 0, 0 },
			new RenderTargetIdentifier[] { 0, 0, 0, 0 },
			new RenderTargetIdentifier[] { 0, 0, 0, 0, 0 },
			new RenderTargetIdentifier[] { 0, 0, 0, 0, 0, 0 },
			new RenderTargetIdentifier[] { 0, 0, 0, 0, 0, 0, 0 },
			new RenderTargetIdentifier[] { 0, 0, 0, 0, 0, 0, 0, 0 }
		};

		// Token: 0x04000395 RID: 917
		private static Plane[] s_Planes = new Plane[6];

		// Token: 0x04000396 RID: 918
		private static Vector4[] s_VectorPlanes = new Vector4[6];

		// Token: 0x02000182 RID: 386
		private static class Profiling
		{
			// Token: 0x04000A3B RID: 2619
			public static readonly ProfilingSampler setMRTAttachmentsList = new ProfilingSampler("NativeRenderPass SetNativeRenderPassMRTAttachmentList");

			// Token: 0x04000A3C RID: 2620
			public static readonly ProfilingSampler setAttachmentList = new ProfilingSampler("NativeRenderPass SetNativeRenderPassAttachmentList");

			// Token: 0x04000A3D RID: 2621
			public static readonly ProfilingSampler execute = new ProfilingSampler("NativeRenderPass ExecuteNativeRenderPass");

			// Token: 0x04000A3E RID: 2622
			public static readonly ProfilingSampler setupFrameData = new ProfilingSampler("NativeRenderPass SetupNativeRenderPassFrameData");

			// Token: 0x04000A3F RID: 2623
			private const string k_Name = "ScriptableRenderer";

			// Token: 0x04000A40 RID: 2624
			public static readonly ProfilingSampler setPerCameraShaderVariables = new ProfilingSampler("ScriptableRenderer.SetPerCameraShaderVariables");

			// Token: 0x04000A41 RID: 2625
			public static readonly ProfilingSampler sortRenderPasses = new ProfilingSampler("Sort Render Passes");

			// Token: 0x04000A42 RID: 2626
			public static readonly ProfilingSampler setupLights = new ProfilingSampler("ScriptableRenderer.SetupLights");

			// Token: 0x04000A43 RID: 2627
			public static readonly ProfilingSampler setupCamera = new ProfilingSampler("Setup Camera Parameters");

			// Token: 0x04000A44 RID: 2628
			public static readonly ProfilingSampler vfxProcessCamera = new ProfilingSampler("VFX Process Camera");

			// Token: 0x04000A45 RID: 2629
			public static readonly ProfilingSampler addRenderPasses = new ProfilingSampler("ScriptableRenderer.AddRenderPasses");

			// Token: 0x04000A46 RID: 2630
			public static readonly ProfilingSampler setupRenderPasses = new ProfilingSampler("ScriptableRenderer.SetupRenderPasses");

			// Token: 0x04000A47 RID: 2631
			public static readonly ProfilingSampler clearRenderingState = new ProfilingSampler("ScriptableRenderer.ClearRenderingState");

			// Token: 0x04000A48 RID: 2632
			public static readonly ProfilingSampler internalStartRendering = new ProfilingSampler("ScriptableRenderer.InternalStartRendering");

			// Token: 0x04000A49 RID: 2633
			public static readonly ProfilingSampler internalFinishRendering = new ProfilingSampler("ScriptableRenderer.InternalFinishRendering");

			// Token: 0x04000A4A RID: 2634
			public static readonly ProfilingSampler drawGizmos = new ProfilingSampler("DrawGizmos");

			// Token: 0x04000A4B RID: 2635
			internal static readonly ProfilingSampler beginXRRendering = new ProfilingSampler("Begin XR Rendering");

			// Token: 0x04000A4C RID: 2636
			internal static readonly ProfilingSampler endXRRendering = new ProfilingSampler("End XR Rendering");

			// Token: 0x0200022B RID: 555
			public static class RenderBlock
			{
				// Token: 0x04000D2B RID: 3371
				private const string k_Name = "RenderPassBlock";

				// Token: 0x04000D2C RID: 3372
				public static readonly ProfilingSampler beforeRendering = new ProfilingSampler("RenderPassBlock.BeforeRendering");

				// Token: 0x04000D2D RID: 3373
				public static readonly ProfilingSampler mainRenderingOpaque = new ProfilingSampler("RenderPassBlock.MainRenderingOpaque");

				// Token: 0x04000D2E RID: 3374
				public static readonly ProfilingSampler mainRenderingTransparent = new ProfilingSampler("RenderPassBlock.MainRenderingTransparent");

				// Token: 0x04000D2F RID: 3375
				public static readonly ProfilingSampler afterRendering = new ProfilingSampler("RenderPassBlock.AfterRendering");
			}

			// Token: 0x0200022C RID: 556
			public static class RenderPass
			{
				// Token: 0x04000D30 RID: 3376
				private const string k_Name = "ScriptableRenderPass";

				// Token: 0x04000D31 RID: 3377
				public static readonly ProfilingSampler configure = new ProfilingSampler("ScriptableRenderPass.Configure");

				// Token: 0x04000D32 RID: 3378
				public static readonly ProfilingSampler setRenderPassAttachments = new ProfilingSampler("ScriptableRenderPass.SetRenderPassAttachments");
			}
		}

		// Token: 0x02000183 RID: 387
		internal struct RenderPassDescriptor
		{
			// Token: 0x06000AA7 RID: 2727 RVA: 0x00049118 File Offset: 0x00047318
			internal RenderPassDescriptor(int width, int height, int sampleCount, int rtID)
			{
				this.w = width;
				this.h = height;
				this.samples = sampleCount;
				this.depthID = rtID;
			}

			// Token: 0x04000A4D RID: 2637
			internal int w;

			// Token: 0x04000A4E RID: 2638
			internal int h;

			// Token: 0x04000A4F RID: 2639
			internal int samples;

			// Token: 0x04000A50 RID: 2640
			internal int depthID;
		}

		// Token: 0x02000184 RID: 388
		public class RenderingFeatures
		{
			// Token: 0x17000248 RID: 584
			// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x00049137 File Offset: 0x00047337
			// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x0004913F File Offset: 0x0004733F
			[Obsolete("cameraStacking has been deprecated use SupportedCameraRenderTypes() in ScriptableRenderer instead.", false)]
			public bool cameraStacking { get; set; }

			// Token: 0x17000249 RID: 585
			// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00049148 File Offset: 0x00047348
			// (set) Token: 0x06000AAB RID: 2731 RVA: 0x00049150 File Offset: 0x00047350
			public bool msaa { get; set; } = true;
		}

		// Token: 0x02000185 RID: 389
		private static class RenderPassBlock
		{
			// Token: 0x04000A53 RID: 2643
			public static readonly int BeforeRendering = 0;

			// Token: 0x04000A54 RID: 2644
			public static readonly int MainRenderingOpaque = 1;

			// Token: 0x04000A55 RID: 2645
			public static readonly int MainRenderingTransparent = 2;

			// Token: 0x04000A56 RID: 2646
			public static readonly int AfterRendering = 3;
		}

		// Token: 0x02000186 RID: 390
		internal struct RTHandleRenderTargetIdentifierCompat
		{
			// Token: 0x1700024A RID: 586
			// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00049182 File Offset: 0x00047382
			public bool useRTHandle
			{
				get
				{
					return this.handle != null;
				}
			}

			// Token: 0x1700024B RID: 587
			// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0004918D File Offset: 0x0004738D
			public RenderTargetIdentifier nameID
			{
				get
				{
					if (!this.useRTHandle)
					{
						return this.fallback;
					}
					return new RenderTargetIdentifier(this.handle.nameID, 0, CubemapFace.Unknown, -1);
				}
			}

			// Token: 0x04000A57 RID: 2647
			public RTHandle handle;

			// Token: 0x04000A58 RID: 2648
			public RenderTargetIdentifier fallback;
		}

		// Token: 0x02000187 RID: 391
		private class VFXProcessCameraPassData
		{
			// Token: 0x04000A59 RID: 2649
			internal CullingResults cullResults;

			// Token: 0x04000A5A RID: 2650
			internal Camera camera;
		}

		// Token: 0x02000188 RID: 392
		private class DrawGizmosPassData
		{
			// Token: 0x04000A5B RID: 2651
			public RenderingData renderingData;

			// Token: 0x04000A5C RID: 2652
			public ScriptableRenderer renderer;

			// Token: 0x04000A5D RID: 2653
			public GizmoSubset gizmoSubset;
		}

		// Token: 0x02000189 RID: 393
		private class BeginXRPassData
		{
			// Token: 0x04000A5E RID: 2654
			public RenderingData renderingData;

			// Token: 0x04000A5F RID: 2655
			public CameraData cameraData;
		}

		// Token: 0x0200018A RID: 394
		private class EndXRPassData
		{
			// Token: 0x04000A60 RID: 2656
			public RenderingData renderingData;

			// Token: 0x04000A61 RID: 2657
			public CameraData cameraData;
		}

		// Token: 0x0200018B RID: 395
		private class PassData
		{
			// Token: 0x04000A62 RID: 2658
			internal RenderingData renderingData;

			// Token: 0x04000A63 RID: 2659
			internal ScriptableRenderer renderer;

			// Token: 0x04000A64 RID: 2660
			internal CameraData cameraData;

			// Token: 0x04000A65 RID: 2661
			internal bool isTargetBackbuffer;
		}

		// Token: 0x0200018C RID: 396
		internal struct RenderBlocks : IDisposable
		{
			// Token: 0x06000AB5 RID: 2741 RVA: 0x000491DC File Offset: 0x000473DC
			public RenderBlocks(List<ScriptableRenderPass> activeRenderPassQueue)
			{
				this.m_BlockEventLimits = new NativeArray<RenderPassEvent>(4, Allocator.Temp, NativeArrayOptions.ClearMemory);
				this.m_BlockRanges = new NativeArray<int>(this.m_BlockEventLimits.Length + 1, Allocator.Temp, NativeArrayOptions.ClearMemory);
				this.m_BlockRangeLengths = new NativeArray<int>(this.m_BlockRanges.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
				this.m_BlockEventLimits[ScriptableRenderer.RenderPassBlock.BeforeRendering] = RenderPassEvent.BeforeRenderingPrePasses;
				this.m_BlockEventLimits[ScriptableRenderer.RenderPassBlock.MainRenderingOpaque] = RenderPassEvent.AfterRenderingOpaques;
				this.m_BlockEventLimits[ScriptableRenderer.RenderPassBlock.MainRenderingTransparent] = RenderPassEvent.AfterRenderingPostProcessing;
				this.m_BlockEventLimits[ScriptableRenderer.RenderPassBlock.AfterRendering] = (RenderPassEvent)2147483647;
				this.FillBlockRanges(activeRenderPassQueue);
				this.m_BlockEventLimits.Dispose();
				for (int i = 0; i < this.m_BlockRanges.Length - 1; i++)
				{
					this.m_BlockRangeLengths[i] = this.m_BlockRanges[i + 1] - this.m_BlockRanges[i];
				}
			}

			// Token: 0x06000AB6 RID: 2742 RVA: 0x000492CE File Offset: 0x000474CE
			public void Dispose()
			{
				this.m_BlockRangeLengths.Dispose();
				this.m_BlockRanges.Dispose();
			}

			// Token: 0x06000AB7 RID: 2743 RVA: 0x000492E8 File Offset: 0x000474E8
			private void FillBlockRanges(List<ScriptableRenderPass> activeRenderPassQueue)
			{
				int num = 0;
				int num2 = 0;
				this.m_BlockRanges[num++] = 0;
				for (int i = 0; i < this.m_BlockEventLimits.Length - 1; i++)
				{
					while (num2 < activeRenderPassQueue.Count && activeRenderPassQueue[num2].renderPassEvent < this.m_BlockEventLimits[i])
					{
						num2++;
					}
					this.m_BlockRanges[num++] = num2;
				}
				this.m_BlockRanges[num] = activeRenderPassQueue.Count;
			}

			// Token: 0x06000AB8 RID: 2744 RVA: 0x0004936C File Offset: 0x0004756C
			public int GetLength(int index)
			{
				return this.m_BlockRangeLengths[index];
			}

			// Token: 0x06000AB9 RID: 2745 RVA: 0x0004937A File Offset: 0x0004757A
			public ScriptableRenderer.RenderBlocks.BlockRange GetRange(int index)
			{
				return new ScriptableRenderer.RenderBlocks.BlockRange(this.m_BlockRanges[index], this.m_BlockRanges[index + 1]);
			}

			// Token: 0x04000A66 RID: 2662
			private NativeArray<RenderPassEvent> m_BlockEventLimits;

			// Token: 0x04000A67 RID: 2663
			private NativeArray<int> m_BlockRanges;

			// Token: 0x04000A68 RID: 2664
			private NativeArray<int> m_BlockRangeLengths;

			// Token: 0x0200022D RID: 557
			public struct BlockRange : IDisposable
			{
				// Token: 0x06000C47 RID: 3143 RVA: 0x0004D4AA File Offset: 0x0004B6AA
				public BlockRange(int begin, int end)
				{
					this.m_Current = ((begin < end) ? begin : end);
					this.m_End = ((end >= begin) ? end : begin);
					this.m_Current--;
				}

				// Token: 0x06000C48 RID: 3144 RVA: 0x0004D4D6 File Offset: 0x0004B6D6
				public ScriptableRenderer.RenderBlocks.BlockRange GetEnumerator()
				{
					return this;
				}

				// Token: 0x06000C49 RID: 3145 RVA: 0x0004D4E0 File Offset: 0x0004B6E0
				public bool MoveNext()
				{
					int num = this.m_Current + 1;
					this.m_Current = num;
					return num < this.m_End;
				}

				// Token: 0x1700026D RID: 621
				// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0004D506 File Offset: 0x0004B706
				public int Current
				{
					get
					{
						return this.m_Current;
					}
				}

				// Token: 0x06000C4B RID: 3147 RVA: 0x0004D50E File Offset: 0x0004B70E
				public void Dispose()
				{
				}

				// Token: 0x04000D33 RID: 3379
				private int m_Current;

				// Token: 0x04000D34 RID: 3380
				private int m_End;
			}
		}
	}
}
