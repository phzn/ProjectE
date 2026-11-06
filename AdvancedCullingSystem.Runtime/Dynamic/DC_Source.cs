using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000043 RID: 67
	public abstract class DC_Source : MonoBehaviour, IHitable
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00007B72 File Offset: 0x00005D72
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00007B7A File Offset: 0x00005D7A
		public float Lifetime
		{
			get
			{
				return this._lifetime;
			}
			set
			{
				this._lifetime = Mathf.Max(0.01f, value);
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007B90 File Offset: 0x00005D90
		private void Update()
		{
			try
			{
				this._currentTime += Time.deltaTime;
				if (this._currentTime > this._lifetime)
				{
					this.OnTimeout();
					base.enabled = false;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message + ex.StackTrace);
				base.enabled = false;
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007BFC File Offset: 0x00005DFC
		public void OnHit()
		{
			try
			{
				base.enabled = true;
				this._currentTime = 0f;
				this.OnHitInternal();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message + ex.StackTrace);
				base.enabled = false;
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007C54 File Offset: 0x00005E54
		public void Enable()
		{
			this.OnTimeout();
			base.enabled = true;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007C63 File Offset: 0x00005E63
		public void Disable()
		{
			this.OnHitInternal();
			base.enabled = false;
		}

		// Token: 0x06000193 RID: 403
		public abstract void SetCullingTarget(ICullingTarget target);

		// Token: 0x06000194 RID: 404
		public abstract void RemoveCullingTarget(ICullingTarget target);

		// Token: 0x06000195 RID: 405
		protected abstract void OnHitInternal();

		// Token: 0x06000196 RID: 406
		protected abstract void OnTimeout();

		// Token: 0x04000119 RID: 281
		private float _lifetime;

		// Token: 0x0400011A RID: 282
		private float _currentTime;
	}
}
