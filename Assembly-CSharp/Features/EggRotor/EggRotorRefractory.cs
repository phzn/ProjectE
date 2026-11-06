using System;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x0200121B RID: 4635
	public sealed class EggRotorRefractory
	{
		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x06008F52 RID: 36690 RVA: 0x003AA03E File Offset: 0x003A823E
		// (set) Token: 0x06008F53 RID: 36691 RVA: 0x003AA046 File Offset: 0x003A8246
		public bool Active { get; private set; }

		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x06008F54 RID: 36692 RVA: 0x003AA04F File Offset: 0x003A824F
		// (set) Token: 0x06008F55 RID: 36693 RVA: 0x003AA057 File Offset: 0x003A8257
		public float Remaining { get; private set; }

		// Token: 0x06008F56 RID: 36694 RVA: 0x003AA060 File Offset: 0x003A8260
		public void Begin(float seconds)
		{
			this.Active = true;
			this.Remaining = ((seconds > 0f && !float.IsInfinity(seconds)) ? seconds : 0f);
		}

		// Token: 0x06008F57 RID: 36695 RVA: 0x003AA087 File Offset: 0x003A8287
		public void Clear()
		{
			this.Active = false;
			this.Remaining = 0f;
		}

		// Token: 0x06008F58 RID: 36696 RVA: 0x003AA09C File Offset: 0x003A829C
		public bool Tick(float deltaTime, float meterValue, float resumeBelow)
		{
			if (!this.Active)
			{
				return false;
			}
			if (deltaTime > 0f && !float.IsInfinity(deltaTime))
			{
				this.Remaining = Mathf.Max(0f, this.Remaining - deltaTime);
			}
			if (this.Remaining > 0f)
			{
				return true;
			}
			if (meterValue > resumeBelow)
			{
				return true;
			}
			this.Active = false;
			return false;
		}
	}
}
