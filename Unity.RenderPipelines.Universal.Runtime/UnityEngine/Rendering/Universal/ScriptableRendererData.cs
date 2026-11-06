using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000D0 RID: 208
	public abstract class ScriptableRendererData : ScriptableObject
	{
		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x00027AF1 File Offset: 0x00025CF1
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00027AF9 File Offset: 0x00025CF9
		internal bool isInvalidated { get; set; }

		// Token: 0x06000689 RID: 1673
		protected abstract ScriptableRenderer Create();

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x00027B02 File Offset: 0x00025D02
		public List<ScriptableRendererFeature> rendererFeatures
		{
			get
			{
				return this.m_RendererFeatures;
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00027B0A File Offset: 0x00025D0A
		public new void SetDirty()
		{
			this.isInvalidated = true;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00027B13 File Offset: 0x00025D13
		internal ScriptableRenderer InternalCreateRenderer()
		{
			this.isInvalidated = false;
			return this.Create();
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00027B22 File Offset: 0x00025D22
		protected virtual void OnValidate()
		{
			this.SetDirty();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00027B2A File Offset: 0x00025D2A
		protected virtual void OnEnable()
		{
			this.SetDirty();
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x00027B32 File Offset: 0x00025D32
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x00027B3A File Offset: 0x00025D3A
		public bool useNativeRenderPass
		{
			get
			{
				return this.m_UseNativeRenderPass;
			}
			set
			{
				this.SetDirty();
				this.m_UseNativeRenderPass = value;
			}
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00027B4C File Offset: 0x00025D4C
		internal bool TryGetRendererFeature<T>(out T rendererFeature) where T : ScriptableRendererFeature
		{
			foreach (ScriptableRendererFeature scriptableRendererFeature in this.rendererFeatures)
			{
				if (scriptableRendererFeature.GetType() == typeof(T))
				{
					rendererFeature = scriptableRendererFeature as T;
					return true;
				}
			}
			rendererFeature = default(T);
			return false;
		}

		// Token: 0x0400053C RID: 1340
		public ScriptableRendererData.DebugShaderResources debugShaders;

		// Token: 0x0400053D RID: 1341
		[SerializeField]
		internal List<ScriptableRendererFeature> m_RendererFeatures = new List<ScriptableRendererFeature>(10);

		// Token: 0x0400053E RID: 1342
		[SerializeField]
		internal List<long> m_RendererFeatureMap = new List<long>(10);

		// Token: 0x0400053F RID: 1343
		[SerializeField]
		private bool m_UseNativeRenderPass;

		// Token: 0x020001B7 RID: 439
		[ReloadGroup]
		[Serializable]
		public sealed class DebugShaderResources
		{
			// Token: 0x04000B80 RID: 2944
			[Reload("Shaders/Debug/DebugReplacement.shader", ReloadAttribute.Package.Root)]
			public Shader debugReplacementPS;

			// Token: 0x04000B81 RID: 2945
			[Reload("Shaders/Debug/HDRDebugView.shader", ReloadAttribute.Package.Root)]
			public Shader hdrDebugViewPS;
		}
	}
}
