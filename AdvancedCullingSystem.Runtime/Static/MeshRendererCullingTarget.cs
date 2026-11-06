using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000013 RID: 19
	public class MeshRendererCullingTarget : CullingTarget
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000031FE File Offset: 0x000013FE
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00003206 File Offset: 0x00001406
		public CullingMethod CullingMethod { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000320F File Offset: 0x0000140F
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00003217 File Offset: 0x00001417
		public bool IsOccluder { get; set; }

		// Token: 0x06000067 RID: 103 RVA: 0x00003220 File Offset: 0x00001420
		private void Awake()
		{
			this._renderer = base.GetComponent<MeshRenderer>();
			if (this.CullingMethod == CullingMethod.FullDisable)
			{
				this._makeVisAction = new Action(this.EnableRenderer);
				this._makeInvisAction = new Action(this.DisableRenderer);
				return;
			}
			this._makeVisAction = new Action(this.EnableRendererKeepShadows);
			this._makeInvisAction = new Action(this.DisableRendererKeepShadows);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000328B File Offset: 0x0000148B
		protected override void MakeInvisible()
		{
			this._makeInvisAction();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003298 File Offset: 0x00001498
		protected override void MakeVisible()
		{
			this._makeVisAction();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000032A5 File Offset: 0x000014A5
		private void EnableRenderer()
		{
			this._renderer.enabled = true;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000032B3 File Offset: 0x000014B3
		private void DisableRenderer()
		{
			this._renderer.enabled = false;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000032C1 File Offset: 0x000014C1
		private void EnableRendererKeepShadows()
		{
			this._renderer.shadowCastingMode = ShadowCastingMode.On;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000032CF File Offset: 0x000014CF
		private void DisableRendererKeepShadows()
		{
			this._renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
		}

		// Token: 0x04000028 RID: 40
		private MeshRenderer _renderer;

		// Token: 0x04000029 RID: 41
		private Action _makeVisAction;

		// Token: 0x0400002A RID: 42
		private Action _makeInvisAction;
	}
}
