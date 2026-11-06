using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200021C RID: 540
	[AddComponentMenu("More Mountains/Feedbacks/Sequencing/MMAudioSourceSequencer")]
	public class MMAudioSourceSequencer : MMSequencer
	{
		// Token: 0x06000EE5 RID: 3813 RVA: 0x00042960 File Offset: 0x00040B60
		protected override void OnBeat()
		{
			base.OnBeat();
			for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
			{
				if (this.Sequence.SequenceTracks[i].Active && this.Sequence.QuantizedSequence[i].Line[this.CurrentSequenceIndex].ID != -1 && this.AudioSources.Count > i && this.AudioSources[i] != null)
				{
					this.AudioSources[i].Play();
				}
			}
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00042A05 File Offset: 0x00040C05
		public override void PlayTrackEvent(int index)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			base.PlayTrackEvent(index);
			this.AudioSources[index].Play();
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00042A27 File Offset: 0x00040C27
		public override void EditorMaintenance()
		{
			base.EditorMaintenance();
			this.SetupSounds();
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00042A38 File Offset: 0x00040C38
		public virtual void SetupSounds()
		{
			if (this.Sequence == null)
			{
				return;
			}
			if (this.AudioSources.Count < this.Sequence.SequenceTracks.Count)
			{
				for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
				{
					if (i >= this.AudioSources.Count)
					{
						this.AudioSources.Add(null);
					}
				}
			}
			if (this.AudioSources.Count > this.Sequence.SequenceTracks.Count)
			{
				this.AudioSources.Clear();
				for (int j = 0; j < this.Sequence.SequenceTracks.Count; j++)
				{
					this.AudioSources.Add(null);
				}
			}
		}

		// Token: 0x0400117D RID: 4477
		[Tooltip("the list of audio sources to play (one per track)")]
		public List<AudioSource> AudioSources;
	}
}
