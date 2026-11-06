using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000D1 RID: 209
	[ExcludeFromPreset]
	public abstract class ScriptableRendererFeature : ScriptableObject, IDisposable
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00027BF2 File Offset: 0x00025DF2
		public bool isActive
		{
			get
			{
				return this.m_Active;
			}
		}

		// Token: 0x06000694 RID: 1684
		public abstract void Create();

		// Token: 0x06000695 RID: 1685 RVA: 0x00027BFA File Offset: 0x00025DFA
		public virtual void OnCameraPreCull(ScriptableRenderer renderer, in CameraData cameraData)
		{
		}

		// Token: 0x06000696 RID: 1686
		public abstract void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData);

		// Token: 0x06000697 RID: 1687 RVA: 0x00027BFC File Offset: 0x00025DFC
		public virtual void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
		{
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00027BFE File Offset: 0x00025DFE
		private void OnEnable()
		{
			this.Create();
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00027C06 File Offset: 0x00025E06
		private void OnValidate()
		{
			this.Create();
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00027C0E File Offset: 0x00025E0E
		internal virtual bool SupportsNativeRenderPass()
		{
			return false;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00027C11 File Offset: 0x00025E11
		internal virtual bool RequireRenderingLayers(bool isDeferred, bool needsGBufferAccurateNormals, out RenderingLayerUtils.Event atEvent, out RenderingLayerUtils.MaskSize maskSize)
		{
			atEvent = RenderingLayerUtils.Event.DepthNormalPrePass;
			maskSize = RenderingLayerUtils.MaskSize.Bits8;
			return false;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00027C1B File Offset: 0x00025E1B
		public void SetActive(bool active)
		{
			this.m_Active = active;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00027C24 File Offset: 0x00025E24
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00027C33 File Offset: 0x00025E33
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x04000540 RID: 1344
		[SerializeField]
		[HideInInspector]
		private bool m_Active = true;
	}
}
