using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200007D RID: 125
	[Obsolete("ForwardRenderer has been deprecated (UnityUpgradable) -> UniversalRenderer", true)]
	public sealed class ForwardRenderer : ScriptableRenderer
	{
		// Token: 0x06000490 RID: 1168 RVA: 0x00019C13 File Offset: 0x00017E13
		public ForwardRenderer(ForwardRendererData data)
			: base(data)
		{
			throw new NotSupportedException(ForwardRenderer.k_ErrorMessage);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00019C26 File Offset: 0x00017E26
		public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			throw new NotSupportedException(ForwardRenderer.k_ErrorMessage);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00019C32 File Offset: 0x00017E32
		public override void SetupLights(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			throw new NotSupportedException(ForwardRenderer.k_ErrorMessage);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00019C3E File Offset: 0x00017E3E
		public override void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData)
		{
			throw new NotSupportedException(ForwardRenderer.k_ErrorMessage);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00019C4A File Offset: 0x00017E4A
		public override void FinishRendering(CommandBuffer cmd)
		{
			throw new NotSupportedException(ForwardRenderer.k_ErrorMessage);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00019C56 File Offset: 0x00017E56
		internal override void SwapColorBuffer(CommandBuffer cmd)
		{
			throw new NotSupportedException(ForwardRenderer.k_ErrorMessage);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00019C62 File Offset: 0x00017E62
		internal override RTHandle GetCameraColorFrontBuffer(CommandBuffer cmd)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000367 RID: 871
		private static readonly string k_ErrorMessage = "ForwardRenderer has been deprecated. Use UniversalRenderer instead";
	}
}
