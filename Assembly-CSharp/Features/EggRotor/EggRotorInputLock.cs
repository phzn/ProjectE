using System;

namespace Features.EggRotor
{
	// Token: 0x0200121C RID: 4636
	public sealed class EggRotorInputLock
	{
		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x06008F5A RID: 36698 RVA: 0x003AA0F8 File Offset: 0x003A82F8
		// (set) Token: 0x06008F5B RID: 36699 RVA: 0x003AA100 File Offset: 0x003A8300
		public bool Owned { get; private set; }

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x06008F5C RID: 36700 RVA: 0x003AA109 File Offset: 0x003A8309
		// (set) Token: 0x06008F5D RID: 36701 RVA: 0x003AA111 File Offset: 0x003A8311
		public bool RepairPending { get; private set; }

		// Token: 0x06008F5E RID: 36702 RVA: 0x003AA11C File Offset: 0x003A831C
		public bool TryTake(bool locked)
		{
			if (locked || this.Owned)
			{
				return false;
			}
			this.Owned = true;
			this.RepairPending = false;
			this._stableSeconds = (this._pendingSeconds = 0f);
			return true;
		}

		// Token: 0x06008F5F RID: 36703 RVA: 0x003AA159 File Offset: 0x003A8359
		public void Observe(bool locked)
		{
			if (locked)
			{
				return;
			}
			this.Owned = false;
			this.RepairPending = false;
		}

		// Token: 0x06008F60 RID: 36704 RVA: 0x003AA16D File Offset: 0x003A836D
		public bool Release(bool locked)
		{
			bool flag = this.Owned && locked;
			this.Owned = false;
			this.RepairPending = false;
			return flag;
		}

		// Token: 0x06008F61 RID: 36705 RVA: 0x003AA188 File Offset: 0x003A8388
		public void Orphan(bool locked)
		{
			this.RepairPending = this.Owned && locked;
			this.Owned = false;
			this._stableSeconds = (this._pendingSeconds = 0f);
		}

		// Token: 0x06008F62 RID: 36706 RVA: 0x003AA1BE File Offset: 0x003A83BE
		public void Forget()
		{
			this.Owned = false;
			this.RepairPending = false;
		}

		// Token: 0x06008F63 RID: 36707 RVA: 0x003AA1D0 File Offset: 0x003A83D0
		public bool TickRepair(float deltaTime, bool locked, bool othersMayOwn, float stableSeconds, float timeoutSeconds)
		{
			if (!this.RepairPending)
			{
				return false;
			}
			if (!locked)
			{
				this.RepairPending = false;
				return false;
			}
			if (deltaTime <= 0f || float.IsInfinity(deltaTime))
			{
				deltaTime = 0f;
			}
			if (othersMayOwn)
			{
				this._stableSeconds = 0f;
				return false;
			}
			this._pendingSeconds += deltaTime;
			if (this._pendingSeconds >= timeoutSeconds)
			{
				this.RepairPending = false;
				return false;
			}
			this._stableSeconds += deltaTime;
			if (this._stableSeconds < stableSeconds)
			{
				return false;
			}
			this.RepairPending = false;
			return true;
		}

		// Token: 0x04009303 RID: 37635
		private float _stableSeconds;

		// Token: 0x04009304 RID: 37636
		private float _pendingSeconds;
	}
}
