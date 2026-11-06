using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000CB RID: 203
	internal static class RenderingLayerUtils
	{
		// Token: 0x0600063C RID: 1596 RVA: 0x000261B7 File Offset: 0x000243B7
		public static void CombineRendererEvents(bool isDeferred, int msaaSampleCount, RenderingLayerUtils.Event rendererEvent, ref RenderingLayerUtils.Event combinedEvent)
		{
			if (msaaSampleCount > 1 && !isDeferred)
			{
				combinedEvent = RenderingLayerUtils.Event.DepthNormalPrePass;
				return;
			}
			combinedEvent = RenderingLayerUtils.Combine(combinedEvent, rendererEvent);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000261D0 File Offset: 0x000243D0
		public static bool RequireRenderingLayers(UniversalRendererData universalRendererData, int msaaSampleCount, out RenderingLayerUtils.Event combinedEvent, out RenderingLayerUtils.MaskSize combinedMaskSize)
		{
			combinedEvent = RenderingLayerUtils.Event.Opaque;
			combinedMaskSize = RenderingLayerUtils.MaskSize.Bits8;
			bool flag = universalRendererData.renderingMode == RenderingMode.Deferred;
			bool flag2 = false;
			foreach (ScriptableRendererFeature scriptableRendererFeature in universalRendererData.rendererFeatures)
			{
				if (scriptableRendererFeature.isActive)
				{
					RenderingLayerUtils.Event @event;
					RenderingLayerUtils.MaskSize maskSize;
					flag2 |= scriptableRendererFeature.RequireRenderingLayers(flag, universalRendererData.accurateGbufferNormals, out @event, out maskSize);
					combinedEvent = RenderingLayerUtils.Combine(combinedEvent, @event);
					combinedMaskSize = RenderingLayerUtils.Combine(combinedMaskSize, maskSize);
				}
			}
			if (msaaSampleCount > 1 && combinedEvent == RenderingLayerUtils.Event.Opaque && !flag)
			{
				combinedEvent = RenderingLayerUtils.Event.DepthNormalPrePass;
			}
			if (UniversalRenderPipelineGlobalSettings.instance)
			{
				RenderingLayerUtils.MaskSize maskSize2 = RenderingLayerUtils.GetMaskSize(UniversalRenderPipelineGlobalSettings.instance.renderingLayerMaskNames.Length);
				combinedMaskSize = RenderingLayerUtils.Combine(combinedMaskSize, maskSize2);
			}
			return flag2;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0002629C File Offset: 0x0002449C
		public static bool RequireRenderingLayers(UniversalRenderer universalRenderer, List<ScriptableRendererFeature> rendererFeatures, int msaaSampleCount, out RenderingLayerUtils.Event combinedEvent, out RenderingLayerUtils.MaskSize combinedMaskSize)
		{
			combinedEvent = RenderingLayerUtils.Event.Opaque;
			combinedMaskSize = RenderingLayerUtils.MaskSize.Bits8;
			bool flag = universalRenderer.renderingModeActual == RenderingMode.Deferred;
			bool flag2 = false;
			foreach (ScriptableRendererFeature scriptableRendererFeature in rendererFeatures)
			{
				if (scriptableRendererFeature.isActive)
				{
					RenderingLayerUtils.Event @event;
					RenderingLayerUtils.MaskSize maskSize;
					flag2 |= scriptableRendererFeature.RequireRenderingLayers(flag, universalRenderer.accurateGbufferNormals, out @event, out maskSize);
					combinedEvent = RenderingLayerUtils.Combine(combinedEvent, @event);
					combinedMaskSize = RenderingLayerUtils.Combine(combinedMaskSize, maskSize);
				}
			}
			if (msaaSampleCount > 1 && combinedEvent == RenderingLayerUtils.Event.Opaque)
			{
				combinedEvent = RenderingLayerUtils.Event.DepthNormalPrePass;
			}
			if (UniversalRenderPipelineGlobalSettings.instance)
			{
				RenderingLayerUtils.MaskSize maskSize2 = RenderingLayerUtils.GetMaskSize(UniversalRenderPipelineGlobalSettings.instance.renderingLayerMaskNames.Length);
				combinedMaskSize = RenderingLayerUtils.Combine(combinedMaskSize, maskSize2);
			}
			return flag2;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00026364 File Offset: 0x00024564
		public static void SetupProperties(CommandBuffer cmd, RenderingLayerUtils.MaskSize maskSize)
		{
			int bits = RenderingLayerUtils.GetBits(maskSize);
			uint num = ((bits != 32) ? ((1U << bits) - 1U) : uint.MaxValue);
			float num2 = math.rcp(num);
			cmd.SetGlobalInt(ShaderPropertyId.renderingLayerMaxInt, (int)num);
			cmd.SetGlobalFloat(ShaderPropertyId.renderingLayerRcpMaxInt, num2);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000263AA File Offset: 0x000245AA
		public static GraphicsFormat GetFormat(RenderingLayerUtils.MaskSize maskSize)
		{
			switch (maskSize)
			{
			case RenderingLayerUtils.MaskSize.Bits8:
				return GraphicsFormat.R8_UNorm;
			case RenderingLayerUtils.MaskSize.Bits16:
				return GraphicsFormat.R16_UNorm;
			case RenderingLayerUtils.MaskSize.Bits24:
			case RenderingLayerUtils.MaskSize.Bits32:
				return GraphicsFormat.R32_SFloat;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000263D1 File Offset: 0x000245D1
		public static uint ToValidRenderingLayers(uint renderingLayers)
		{
			if (UniversalRenderPipelineGlobalSettings.instance)
			{
				return UniversalRenderPipelineGlobalSettings.instance.validRenderingLayers & renderingLayers;
			}
			return renderingLayers;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000263F0 File Offset: 0x000245F0
		private static RenderingLayerUtils.MaskSize GetMaskSize(int bits)
		{
			switch ((bits + 7) / 8)
			{
			case 0:
				return RenderingLayerUtils.MaskSize.Bits8;
			case 1:
				return RenderingLayerUtils.MaskSize.Bits8;
			case 2:
				return RenderingLayerUtils.MaskSize.Bits16;
			case 3:
				return RenderingLayerUtils.MaskSize.Bits24;
			case 4:
				return RenderingLayerUtils.MaskSize.Bits32;
			default:
				return RenderingLayerUtils.MaskSize.Bits32;
			}
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002642A File Offset: 0x0002462A
		private static int GetBits(RenderingLayerUtils.MaskSize maskSize)
		{
			switch (maskSize)
			{
			case RenderingLayerUtils.MaskSize.Bits8:
				return 8;
			case RenderingLayerUtils.MaskSize.Bits16:
				return 16;
			case RenderingLayerUtils.MaskSize.Bits24:
				return 24;
			case RenderingLayerUtils.MaskSize.Bits32:
				return 32;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00026454 File Offset: 0x00024654
		private static RenderingLayerUtils.Event Combine(RenderingLayerUtils.Event a, RenderingLayerUtils.Event b)
		{
			return (RenderingLayerUtils.Event)Mathf.Min((int)a, (int)b);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0002645D File Offset: 0x0002465D
		private static RenderingLayerUtils.MaskSize Combine(RenderingLayerUtils.MaskSize a, RenderingLayerUtils.MaskSize b)
		{
			return (RenderingLayerUtils.MaskSize)Mathf.Max((int)a, (int)b);
		}

		// Token: 0x020001B5 RID: 437
		public enum Event
		{
			// Token: 0x04000B79 RID: 2937
			DepthNormalPrePass,
			// Token: 0x04000B7A RID: 2938
			Opaque
		}

		// Token: 0x020001B6 RID: 438
		public enum MaskSize
		{
			// Token: 0x04000B7C RID: 2940
			Bits8,
			// Token: 0x04000B7D RID: 2941
			Bits16,
			// Token: 0x04000B7E RID: 2942
			Bits24,
			// Token: 0x04000B7F RID: 2943
			Bits32
		}
	}
}
