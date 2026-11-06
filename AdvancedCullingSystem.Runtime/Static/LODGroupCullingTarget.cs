using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000011 RID: 17
	public class LODGroupCullingTarget : CullingTarget
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003093 File Offset: 0x00001293
		// (set) Token: 0x06000057 RID: 87 RVA: 0x0000309B File Offset: 0x0000129B
		public CullingMethod CullingMethod { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000030A4 File Offset: 0x000012A4
		// (set) Token: 0x06000059 RID: 89 RVA: 0x000030AC File Offset: 0x000012AC
		public bool IsOccluder { get; set; }

		// Token: 0x0600005A RID: 90 RVA: 0x000030B8 File Offset: 0x000012B8
		private void Awake()
		{
			if (this.CullingMethod == CullingMethod.FullDisable)
			{
				this._makeVisibleAction = new Action(this.MakeRenderersVisible);
				this._makeInvisibleAction = new Action(this.MakeRenderersInvisible);
				return;
			}
			this._makeVisibleAction = new Action(this.MakeRenderersVisibleKeepShadows);
			this._makeInvisibleAction = new Action(this.MakeRenderersInvisibleKeepShadows);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003117 File Offset: 0x00001317
		public void SetRenderers(IEnumerable<Renderer> renderers)
		{
			this._renderers = renderers.ToArray<Renderer>();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003125 File Offset: 0x00001325
		protected override void MakeVisible()
		{
			this._makeVisibleAction();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003132 File Offset: 0x00001332
		protected override void MakeInvisible()
		{
			this._makeInvisibleAction();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003140 File Offset: 0x00001340
		private void MakeRenderersVisible()
		{
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].enabled = true;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003170 File Offset: 0x00001370
		private void MakeRenderersInvisible()
		{
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].enabled = false;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000031A0 File Offset: 0x000013A0
		private void MakeRenderersVisibleKeepShadows()
		{
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].shadowCastingMode = ShadowCastingMode.On;
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000031D0 File Offset: 0x000013D0
		private void MakeRenderersInvisibleKeepShadows()
		{
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._renderers[i].shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			}
		}

		// Token: 0x04000020 RID: 32
		[SerializeField]
		[HideInInspector]
		private Renderer[] _renderers;

		// Token: 0x04000021 RID: 33
		private Action _makeVisibleAction;

		// Token: 0x04000022 RID: 34
		private Action _makeInvisibleAction;
	}
}
