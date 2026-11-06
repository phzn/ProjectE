using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200007C RID: 124
	public abstract class ScriptableRenderPass
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x00019410 File Offset: 0x00017610
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual void FrameCleanup(CommandBuffer cmd)
		{
			this.OnCameraCleanup(cmd);
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00019419 File Offset: 0x00017619
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x00019421 File Offset: 0x00017621
		public RenderPassEvent renderPassEvent { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0001942A File Offset: 0x0001762A
		[Obsolete("Use colorAttachmentHandles")]
		public RenderTargetIdentifier[] colorAttachments
		{
			get
			{
				return this.m_ColorAttachmentIds;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00019432 File Offset: 0x00017632
		[Obsolete("Use colorAttachmentHandle")]
		public RenderTargetIdentifier colorAttachment
		{
			get
			{
				return this.m_ColorAttachmentIds[0];
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00019440 File Offset: 0x00017640
		[Obsolete("Use depthAttachmentHandle")]
		public RenderTargetIdentifier depthAttachment
		{
			get
			{
				if (!this.m_UsesRTHandles)
				{
					return this.m_DepthAttachmentId;
				}
				return new RenderTargetIdentifier(this.m_DepthAttachment.nameID, 0, CubemapFace.Unknown, -1);
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00019464 File Offset: 0x00017664
		public RTHandle[] colorAttachmentHandles
		{
			get
			{
				return this.m_ColorAttachments;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0001946C File Offset: 0x0001766C
		public RTHandle colorAttachmentHandle
		{
			get
			{
				return this.m_ColorAttachments[0];
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00019476 File Offset: 0x00017676
		public RTHandle depthAttachmentHandle
		{
			get
			{
				return this.m_DepthAttachment;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0001947E File Offset: 0x0001767E
		public RenderBufferStoreAction[] colorStoreActions
		{
			get
			{
				return this.m_ColorStoreActions;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00019486 File Offset: 0x00017686
		public RenderBufferStoreAction depthStoreAction
		{
			get
			{
				return this.m_DepthStoreAction;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0001948E File Offset: 0x0001768E
		internal bool[] overriddenColorStoreActions
		{
			get
			{
				return this.m_OverriddenColorStoreActions;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00019496 File Offset: 0x00017696
		internal bool overriddenDepthStoreAction
		{
			get
			{
				return this.m_OverriddenDepthStoreAction;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0001949E File Offset: 0x0001769E
		public ScriptableRenderPassInput input
		{
			get
			{
				return this.m_Input;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x000194A6 File Offset: 0x000176A6
		public ClearFlag clearFlag
		{
			get
			{
				return this.m_ClearFlag;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x000194AE File Offset: 0x000176AE
		public Color clearColor
		{
			get
			{
				return this.m_ClearColor;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x000194B6 File Offset: 0x000176B6
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x000194BE File Offset: 0x000176BE
		protected internal ProfilingSampler profilingSampler { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x000194C7 File Offset: 0x000176C7
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x000194CF File Offset: 0x000176CF
		internal bool overrideCameraTarget { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x000194D8 File Offset: 0x000176D8
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x000194E0 File Offset: 0x000176E0
		internal bool isBlitRenderPass { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x000194E9 File Offset: 0x000176E9
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x000194F1 File Offset: 0x000176F1
		internal bool useNativeRenderPass { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000194FA File Offset: 0x000176FA
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00019502 File Offset: 0x00017702
		internal int renderPassQueueIndex { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0001950B File Offset: 0x0001770B
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x00019513 File Offset: 0x00017713
		internal GraphicsFormat[] renderTargetFormat { get; set; }

		// Token: 0x0600046A RID: 1130 RVA: 0x0001951C File Offset: 0x0001771C
		internal static DebugHandler GetActiveDebugHandler(ref RenderingData renderingData)
		{
			DebugHandler debugHandler = renderingData.cameraData.renderer.DebugHandler;
			if (debugHandler != null && debugHandler.IsActiveForCamera(ref renderingData.cameraData))
			{
				return debugHandler;
			}
			return null;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00019550 File Offset: 0x00017750
		public ScriptableRenderPass()
		{
			this.m_UsesRTHandles = true;
			this.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
			RTHandle[] array = new RTHandle[8];
			array[0] = ScriptableRenderPass.k_CameraTarget;
			this.m_ColorAttachments = array;
			this.m_InputAttachments = new RTHandle[8];
			this.m_InputAttachmentIsTransient = new bool[8];
			this.m_DepthAttachment = ScriptableRenderPass.k_CameraTarget;
			this.m_ColorStoreActions = new RenderBufferStoreAction[8];
			this.m_DepthStoreAction = RenderBufferStoreAction.Store;
			this.m_OverriddenColorStoreActions = new bool[8];
			this.m_OverriddenDepthStoreAction = false;
			this.m_DepthAttachment = ScriptableRenderPass.k_CameraTarget;
			this.m_DepthAttachmentId = this.m_DepthAttachment.nameID;
			this.m_ColorAttachmentIds = new RenderTargetIdentifier[]
			{
				ScriptableRenderPass.k_CameraTarget.nameID,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			this.m_ClearFlag = ClearFlag.None;
			this.m_ClearColor = Color.black;
			this.overrideCameraTarget = false;
			this.isBlitRenderPass = false;
			this.profilingSampler = new ProfilingSampler("Unnamed_ScriptableRenderPass");
			this.useNativeRenderPass = true;
			this.renderPassQueueIndex = -1;
			this.renderTargetFormat = new GraphicsFormat[8];
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000196EB File Offset: 0x000178EB
		public void ConfigureInput(ScriptableRenderPassInput passInput)
		{
			this.m_Input = passInput;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000196F4 File Offset: 0x000178F4
		public void ConfigureColorStoreAction(RenderBufferStoreAction storeAction, uint attachmentIndex = 0U)
		{
			this.m_ColorStoreActions[(int)attachmentIndex] = storeAction;
			this.m_OverriddenColorStoreActions[(int)attachmentIndex] = true;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00019708 File Offset: 0x00017908
		public void ConfigureColorStoreActions(RenderBufferStoreAction[] storeActions)
		{
			int num = Math.Min(storeActions.Length, this.m_ColorStoreActions.Length);
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				this.m_ColorStoreActions[(int)num2] = storeActions[(int)num2];
				this.m_OverriddenColorStoreActions[(int)num2] = true;
				num2 += 1U;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00019748 File Offset: 0x00017948
		public void ConfigureDepthStoreAction(RenderBufferStoreAction storeAction)
		{
			this.m_DepthStoreAction = storeAction;
			this.m_OverriddenDepthStoreAction = true;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00019758 File Offset: 0x00017958
		internal void ConfigureInputAttachments(RTHandle input, bool isTransient = false)
		{
			this.m_InputAttachments[0] = input;
			this.m_InputAttachmentIsTransient[0] = isTransient;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001976C File Offset: 0x0001796C
		internal void ConfigureInputAttachments(RTHandle[] inputs)
		{
			this.m_InputAttachments = inputs;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00019775 File Offset: 0x00017975
		internal void ConfigureInputAttachments(RTHandle[] inputs, bool[] isTransient)
		{
			this.ConfigureInputAttachments(inputs);
			this.m_InputAttachmentIsTransient = isTransient;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00019785 File Offset: 0x00017985
		internal void SetInputAttachmentTransient(int idx, bool isTransient)
		{
			this.m_InputAttachmentIsTransient[idx] = isTransient;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00019790 File Offset: 0x00017990
		internal bool IsInputAttachmentTransient(int idx)
		{
			return this.m_InputAttachmentIsTransient[idx];
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001979C File Offset: 0x0001799C
		public void ResetTarget()
		{
			this.overrideCameraTarget = false;
			this.m_UsesRTHandles = true;
			this.m_DepthAttachmentId = -1;
			this.m_DepthAttachment = null;
			this.m_ColorAttachments[0] = null;
			this.m_ColorAttachmentIds[0] = -1;
			for (int i = 1; i < this.m_ColorAttachments.Length; i++)
			{
				this.m_ColorAttachments[i] = null;
				this.m_ColorAttachmentIds[i] = 0;
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00019813 File Offset: 0x00017A13
		[Obsolete("Use RTHandles for colorAttachment and depthAttachment")]
		public void ConfigureTarget(RenderTargetIdentifier colorAttachment, RenderTargetIdentifier depthAttachment)
		{
			this.m_DepthAttachmentId = depthAttachment;
			this.m_DepthAttachment = null;
			this.ConfigureTarget(colorAttachment);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001982C File Offset: 0x00017A2C
		public void ConfigureTarget(RTHandle colorAttachment, RTHandle depthAttachment)
		{
			this.overrideCameraTarget = true;
			this.m_ColorAttachments[0] = colorAttachment;
			this.m_DepthAttachment = depthAttachment;
			this.m_DepthAttachmentId = this.m_DepthAttachment.nameID;
			for (int i = 1; i < this.m_ColorAttachments.Length; i++)
			{
				this.m_ColorAttachments[i] = null;
				this.m_ColorAttachmentIds[i] = 0;
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00019890 File Offset: 0x00017A90
		[Obsolete("Use RTHandles for colorAttachments and depthAttachment")]
		public void ConfigureTarget(RenderTargetIdentifier[] colorAttachments, RenderTargetIdentifier depthAttachment)
		{
			this.m_UsesRTHandles = false;
			this.overrideCameraTarget = true;
			uint validColorBufferCount = RenderingUtils.GetValidColorBufferCount(colorAttachments);
			if ((ulong)validColorBufferCount > (ulong)((long)SystemInfo.supportedRenderTargetCount))
			{
				Debug.LogError("Trying to set " + validColorBufferCount.ToString() + " renderTargets, which is more than the maximum supported:" + SystemInfo.supportedRenderTargetCount.ToString());
			}
			this.m_ColorAttachmentIds = colorAttachments;
			this.m_DepthAttachmentId = depthAttachment;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000198F4 File Offset: 0x00017AF4
		public void ConfigureTarget(RTHandle[] colorAttachments, RTHandle depthAttachment)
		{
			this.m_UsesRTHandles = true;
			this.overrideCameraTarget = true;
			uint validColorBufferCount = RenderingUtils.GetValidColorBufferCount(colorAttachments);
			if ((ulong)validColorBufferCount > (ulong)((long)SystemInfo.supportedRenderTargetCount))
			{
				Debug.LogError("Trying to set " + validColorBufferCount.ToString() + " renderTargets, which is more than the maximum supported:" + SystemInfo.supportedRenderTargetCount.ToString());
			}
			this.m_ColorAttachments = colorAttachments;
			if (this.m_ColorAttachmentIds.Length != this.m_ColorAttachments.Length)
			{
				this.m_ColorAttachmentIds = new RenderTargetIdentifier[this.m_ColorAttachments.Length];
			}
			for (int i = 0; i < this.m_ColorAttachmentIds.Length; i++)
			{
				this.m_ColorAttachmentIds[i] = new RenderTargetIdentifier(colorAttachments[i].nameID, 0, CubemapFace.Unknown, -1);
			}
			this.m_DepthAttachmentId = depthAttachment.nameID;
			this.m_DepthAttachment = depthAttachment;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000199B8 File Offset: 0x00017BB8
		internal void ConfigureTarget(RTHandle[] colorAttachments, RTHandle depthAttachment, GraphicsFormat[] formats)
		{
			this.ConfigureTarget(colorAttachments, depthAttachment);
			for (int i = 0; i < formats.Length; i++)
			{
				this.renderTargetFormat[i] = formats[i];
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000199E8 File Offset: 0x00017BE8
		[Obsolete("Use RTHandle for colorAttachment")]
		public void ConfigureTarget(RenderTargetIdentifier colorAttachment)
		{
			this.m_UsesRTHandles = false;
			this.overrideCameraTarget = true;
			this.m_ColorAttachmentIds[0] = colorAttachment;
			for (int i = 1; i < this.m_ColorAttachmentIds.Length; i++)
			{
				this.m_ColorAttachmentIds[i] = 0;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00019A35 File Offset: 0x00017C35
		public void ConfigureTarget(RTHandle colorAttachment)
		{
			this.m_UsesRTHandles = true;
			this.m_ColorAttachmentIds[0] = new RenderTargetIdentifier(colorAttachment.nameID, 0, CubemapFace.Unknown, -1);
			this.ConfigureTarget(colorAttachment, ScriptableRenderPass.k_CameraTarget);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00019A64 File Offset: 0x00017C64
		[Obsolete("Use RTHandles for colorAttachments")]
		public void ConfigureTarget(RenderTargetIdentifier[] colorAttachments)
		{
			this.ConfigureTarget(colorAttachments, ScriptableRenderPass.k_CameraTarget.nameID);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00019A77 File Offset: 0x00017C77
		public void ConfigureTarget(RTHandle[] colorAttachments)
		{
			this.ConfigureTarget(colorAttachments, ScriptableRenderPass.k_CameraTarget);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00019A85 File Offset: 0x00017C85
		public void ConfigureClear(ClearFlag clearFlag, Color clearColor)
		{
			this.m_ClearFlag = clearFlag;
			this.m_ClearColor = clearColor;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00019A95 File Offset: 0x00017C95
		public virtual void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00019A97 File Offset: 0x00017C97
		public virtual void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00019A99 File Offset: 0x00017C99
		public virtual void OnCameraCleanup(CommandBuffer cmd)
		{
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00019A9B File Offset: 0x00017C9B
		public virtual void OnFinishCameraStackRendering(CommandBuffer cmd)
		{
		}

		// Token: 0x06000484 RID: 1156
		public abstract void Execute(ScriptableRenderContext context, ref RenderingData renderingData);

		// Token: 0x06000485 RID: 1157 RVA: 0x00019A9D File Offset: 0x00017C9D
		internal virtual void RecordRenderGraph(RenderGraph renderGraph, ref RenderingData renderingData)
		{
			Debug.LogWarning("RecordRenderGraph is not implemented, the pass " + this.ToString() + " won't be recorded in the current RenderGraph.");
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00019AB9 File Offset: 0x00017CB9
		[Obsolete("Use RTHandles for source and destination")]
		public void Blit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material = null, int passIndex = 0)
		{
			ScriptableRenderer.SetRenderTarget(cmd, destination, BuiltinRenderTextureType.CameraTarget, this.clearFlag, this.clearColor);
			cmd.Blit(source, destination, material, passIndex);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00019AE0 File Offset: 0x00017CE0
		public void Blit(CommandBuffer cmd, RTHandle source, RTHandle destination, Material material = null, int passIndex = 0)
		{
			if (material == null)
			{
				Blitter.BlitCameraTexture(cmd, source, destination, 0f, source.rt.filterMode == FilterMode.Bilinear);
				return;
			}
			Blitter.BlitCameraTexture(cmd, source, destination, material, passIndex);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00019B14 File Offset: 0x00017D14
		public void Blit(CommandBuffer cmd, ref RenderingData data, Material material, int passIndex = 0)
		{
			ScriptableRenderer renderer = data.cameraData.renderer;
			this.Blit(cmd, renderer.cameraColorTargetHandle, renderer.GetCameraColorFrontBuffer(cmd), material, passIndex);
			renderer.SwapColorBuffer(cmd);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00019B4C File Offset: 0x00017D4C
		public void Blit(CommandBuffer cmd, ref RenderingData data, RTHandle source, Material material, int passIndex = 0)
		{
			ScriptableRenderer renderer = data.cameraData.renderer;
			this.Blit(cmd, source, renderer.cameraColorTargetHandle, material, passIndex);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00019B77 File Offset: 0x00017D77
		public DrawingSettings CreateDrawingSettings(ShaderTagId shaderTagId, ref RenderingData renderingData, SortingCriteria sortingCriteria)
		{
			return RenderingUtils.CreateDrawingSettings(shaderTagId, ref renderingData, sortingCriteria);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00019B81 File Offset: 0x00017D81
		public DrawingSettings CreateDrawingSettings(List<ShaderTagId> shaderTagIdList, ref RenderingData renderingData, SortingCriteria sortingCriteria)
		{
			return RenderingUtils.CreateDrawingSettings(shaderTagIdList, ref renderingData, sortingCriteria);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00019B8B File Offset: 0x00017D8B
		public static bool operator <(ScriptableRenderPass lhs, ScriptableRenderPass rhs)
		{
			return lhs.renderPassEvent < rhs.renderPassEvent;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00019B9B File Offset: 0x00017D9B
		public static bool operator >(ScriptableRenderPass lhs, ScriptableRenderPass rhs)
		{
			return lhs.renderPassEvent > rhs.renderPassEvent;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00019BAC File Offset: 0x00017DAC
		internal static int GetRenderPassEventRange(RenderPassEvent renderPassEvent)
		{
			int num = RenderPassEventsEnumValues.values.Length;
			int num2 = 0;
			int num3 = 0;
			while (num3 < num && RenderPassEventsEnumValues.values[num2] != (int)renderPassEvent)
			{
				num2++;
				num3++;
			}
			if (num2 >= num)
			{
				Debug.LogError("GetRenderPassEventRange: invalid renderPassEvent value cannot be found in the RenderPassEvent enumeration");
				return 0;
			}
			if (num2 + 1 >= num)
			{
				return 50;
			}
			return RenderPassEventsEnumValues.values[num2 + 1] - (int)renderPassEvent;
		}

		// Token: 0x0400034F RID: 847
		public static RTHandle k_CameraTarget = RTHandles.Alloc(BuiltinRenderTextureType.CameraTarget);

		// Token: 0x04000351 RID: 849
		private RenderBufferStoreAction[] m_ColorStoreActions = new RenderBufferStoreAction[1];

		// Token: 0x04000352 RID: 850
		private RenderBufferStoreAction m_DepthStoreAction;

		// Token: 0x04000353 RID: 851
		private bool[] m_OverriddenColorStoreActions = new bool[1];

		// Token: 0x04000354 RID: 852
		private bool m_OverriddenDepthStoreAction;

		// Token: 0x0400035A RID: 858
		internal NativeArray<int> m_ColorAttachmentIndices;

		// Token: 0x0400035B RID: 859
		internal NativeArray<int> m_InputAttachmentIndices;

		// Token: 0x0400035D RID: 861
		internal bool m_UsesRTHandles;

		// Token: 0x0400035E RID: 862
		private RTHandle[] m_ColorAttachments;

		// Token: 0x0400035F RID: 863
		private RenderTargetIdentifier[] m_ColorAttachmentIds;

		// Token: 0x04000360 RID: 864
		internal RTHandle[] m_InputAttachments = new RTHandle[8];

		// Token: 0x04000361 RID: 865
		internal bool[] m_InputAttachmentIsTransient = new bool[8];

		// Token: 0x04000362 RID: 866
		private RTHandle m_DepthAttachment;

		// Token: 0x04000363 RID: 867
		private RenderTargetIdentifier m_DepthAttachmentId;

		// Token: 0x04000364 RID: 868
		private ScriptableRenderPassInput m_Input;

		// Token: 0x04000365 RID: 869
		private ClearFlag m_ClearFlag;

		// Token: 0x04000366 RID: 870
		private Color m_ClearColor = Color.black;
	}
}
