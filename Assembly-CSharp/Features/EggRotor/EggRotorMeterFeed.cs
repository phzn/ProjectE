using System;
using Features.MasturbationSystem;

namespace Features.EggRotor
{
	// Token: 0x0200121D RID: 4637
	public sealed class EggRotorMeterFeed
	{
		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x06008F65 RID: 36709 RVA: 0x003AA25D File Offset: 0x003A845D
		// (set) Token: 0x06008F66 RID: 36710 RVA: 0x003AA265 File Offset: 0x003A8465
		public bool Feeding { get; private set; }

		// Token: 0x06008F67 RID: 36711 RVA: 0x003AA26E File Offset: 0x003A846E
		public bool NotifyMaxReached()
		{
			if (!this.Feeding)
			{
				return false;
			}
			this._maxReached = true;
			return true;
		}

		// Token: 0x06008F68 RID: 36712 RVA: 0x003AA284 File Offset: 0x003A8484
		public bool Feed(OrgasmMeterController meter, float amount)
		{
			if (meter == null || amount <= 0f || float.IsInfinity(amount) || this.Feeding)
			{
				return false;
			}
			this._maxReached = false;
			this.Feeding = true;
			try
			{
				meter.AddProgress(amount);
			}
			finally
			{
				this.Feeding = false;
			}
			bool maxReached = this._maxReached;
			this._maxReached = false;
			return maxReached;
		}

		// Token: 0x04009307 RID: 37639
		private bool _maxReached;
	}
}
