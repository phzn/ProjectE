using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x0200000D RID: 13
	public abstract class CullingTarget : MonoBehaviour
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002F80 File Offset: 0x00001180
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002F88 File Offset: 0x00001188
		public Bounds Bounds { get; set; }

		// Token: 0x06000043 RID: 67 RVA: 0x00002F91 File Offset: 0x00001191
		private void LateUpdate()
		{
			if (this._isVisible)
			{
				if (!this._makedVisible)
				{
					this.MakeVisible();
					this._makedVisible = true;
				}
				this._isVisible = false;
				return;
			}
			this.MakeInvisible();
			this._makedVisible = false;
			base.enabled = false;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002FCC File Offset: 0x000011CC
		public void SetVisible()
		{
			if (!this._isVisible)
			{
				base.enabled = true;
				this._isVisible = true;
			}
		}

		// Token: 0x06000045 RID: 69
		protected abstract void MakeVisible();

		// Token: 0x06000046 RID: 70
		protected abstract void MakeInvisible();

		// Token: 0x04000018 RID: 24
		private bool _isVisible;

		// Token: 0x04000019 RID: 25
		private bool _makedVisible;
	}
}
