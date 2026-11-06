using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200012C RID: 300
	public class MMRadioSignalAudioAnalyzer : MMRadioSignal
	{
		// Token: 0x060006C6 RID: 1734 RVA: 0x00021CDA File Offset: 0x0001FEDA
		protected override void Shake()
		{
			base.Shake();
			this.CurrentLevel = this.TargetAnalyzer.Beats[this.BeatID].CurrentValue * this.GlobalMultiplier;
		}

		// Token: 0x0400086D RID: 2157
		[Header("Audio Analyzer")]
		public MMAudioAnalyzer TargetAnalyzer;

		// Token: 0x0400086E RID: 2158
		public int BeatID;
	}
}
