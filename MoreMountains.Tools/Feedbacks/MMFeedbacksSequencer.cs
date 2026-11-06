using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200021D RID: 541
	[AddComponentMenu("More Mountains/Feedbacks/Sequencing/MMFeedbacksSequencer")]
	public class MMFeedbacksSequencer : MMSequencer
	{
		// Token: 0x06000EEA RID: 3818 RVA: 0x00042B00 File Offset: 0x00040D00
		protected override void OnBeat()
		{
			base.OnBeat();
			for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
			{
				if (this.Sequence.SequenceTracks[i].Active && this.Sequence.QuantizedSequence[i].Line[this.CurrentSequenceIndex].ID != -1 && this.Feedbacks.Count > i && this.Feedbacks[i] != null)
				{
					this.Feedbacks[i].PlayFeedbacks();
				}
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00042BA5 File Offset: 0x00040DA5
		public override void PlayTrackEvent(int index)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			base.PlayTrackEvent(index);
			this.Feedbacks[index].PlayFeedbacks();
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00042BC7 File Offset: 0x00040DC7
		public override void EditorMaintenance()
		{
			base.EditorMaintenance();
			this.SetupFeedbacks();
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00042BD8 File Offset: 0x00040DD8
		public virtual void SetupFeedbacks()
		{
			if (this.Sequence == null)
			{
				return;
			}
			if (this.Feedbacks.Count < this.Sequence.SequenceTracks.Count)
			{
				for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
				{
					if (i >= this.Feedbacks.Count)
					{
						this.Feedbacks.Add(null);
					}
				}
			}
			if (this.Feedbacks.Count > this.Sequence.SequenceTracks.Count)
			{
				this.Feedbacks.Clear();
				for (int j = 0; j < this.Sequence.SequenceTracks.Count; j++)
				{
					this.Feedbacks.Add(null);
				}
			}
		}

		// Token: 0x0400117E RID: 4478
		[Tooltip("the list of audio clips to play (one per track)")]
		public List<MMFeedbacks> Feedbacks;
	}
}
