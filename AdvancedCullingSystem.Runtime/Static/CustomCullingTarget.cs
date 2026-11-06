using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x0200000E RID: 14
	public class CustomCullingTarget : CullingTarget
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002FE4 File Offset: 0x000011E4
		public CustomTargetEvent OnVisible
		{
			get
			{
				return this._onVisible;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002FEC File Offset: 0x000011EC
		public CustomTargetEvent OnInvisible
		{
			get
			{
				return this._onInvisible;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002FF4 File Offset: 0x000011F4
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002FFC File Offset: 0x000011FC
		public bool IsOccluder { get; set; }

		// Token: 0x0600004C RID: 76 RVA: 0x00003005 File Offset: 0x00001205
		private void Awake()
		{
			if (this._onVisible == null)
			{
				this._onInvisible = new CustomTargetEvent();
			}
			if (this._onInvisible == null)
			{
				this._onInvisible = new CustomTargetEvent();
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000302D File Offset: 0x0000122D
		public void SetActions(CustomTargetEvent onVisible, CustomTargetEvent onInvisible)
		{
			this._onVisible = onVisible;
			this._onInvisible = onInvisible;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000303D File Offset: 0x0000123D
		protected override void MakeVisible()
		{
			this._onVisible.Invoke(this);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000304B File Offset: 0x0000124B
		protected override void MakeInvisible()
		{
			this._onInvisible.Invoke(this);
		}

		// Token: 0x0400001B RID: 27
		[SerializeField]
		private CustomTargetEvent _onVisible;

		// Token: 0x0400001C RID: 28
		[SerializeField]
		private CustomTargetEvent _onInvisible;
	}
}
