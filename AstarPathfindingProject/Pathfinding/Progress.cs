using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200002C RID: 44
	public readonly struct Progress
	{
		// Token: 0x0600025C RID: 604 RVA: 0x0000B17B File Offset: 0x0000937B
		public Progress(float progress, ScanningStage stage, int graphIndex = 0, int graphCount = 0)
		{
			this.progress = progress;
			this.stage = stage;
			this.graphIndex = graphIndex;
			this.graphCount = graphCount;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000B19A File Offset: 0x0000939A
		public Progress MapTo(float min, float max)
		{
			return new Progress(Mathf.Lerp(min, max, this.progress), this.stage, this.graphIndex, this.graphCount);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000B1C0 File Offset: 0x000093C0
		public override string ToString()
		{
			string text = this.progress.ToString("0%") + " ";
			switch (this.stage)
			{
			case ScanningStage.PreProcessingGraphs:
				text += "Pre-processing graphs";
				break;
			case ScanningStage.PreProcessingGraph:
				text = string.Concat(new string[]
				{
					text,
					"Pre-processing graph ",
					(this.graphIndex + 1).ToString(),
					" of ",
					this.graphCount.ToString()
				});
				break;
			case ScanningStage.ScanningGraph:
				text = string.Concat(new string[]
				{
					text,
					"Scanning graph ",
					(this.graphIndex + 1).ToString(),
					" of ",
					this.graphCount.ToString()
				});
				break;
			case ScanningStage.PostProcessingGraph:
				text = string.Concat(new string[]
				{
					text,
					"Post-processing graph ",
					(this.graphIndex + 1).ToString(),
					" of ",
					this.graphCount.ToString()
				});
				break;
			case ScanningStage.FinishingScans:
				text += "Finalizing graph scans";
				break;
			}
			return text;
		}

		// Token: 0x0400015D RID: 349
		public readonly float progress;

		// Token: 0x0400015E RID: 350
		internal readonly ScanningStage stage;

		// Token: 0x0400015F RID: 351
		internal readonly int graphIndex;

		// Token: 0x04000160 RID: 352
		internal readonly int graphCount;
	}
}
