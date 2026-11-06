using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000225 RID: 549
	[AddComponentMenu("More Mountains/Feedbacks/Sequencing/MMSoundSequencer")]
	public class MMSoundSequencer : MMSequencer
	{
		// Token: 0x06000F1E RID: 3870 RVA: 0x00043F14 File Offset: 0x00042114
		protected override void Initialization()
		{
			base.Initialization();
			this._audioSources = new List<AudioSource>();
			foreach (AudioClip audioClip in this.Sounds)
			{
				GameObject gameObject = new GameObject();
				SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
				gameObject.name = "AudioSource - " + audioClip.name;
				gameObject.transform.SetParent(base.transform);
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				audioSource.loop = false;
				audioSource.playOnAwake = false;
				audioSource.clip = audioClip;
				audioSource.volume = 1f;
				audioSource.pitch = 1f;
				this._audioSources.Add(audioSource);
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00043FF4 File Offset: 0x000421F4
		protected override void OnBeat()
		{
			base.OnBeat();
			for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
			{
				if (this.Sequence.SequenceTracks[i].Active && this.Sequence.QuantizedSequence[i].Line[this.CurrentSequenceIndex].ID != -1 && this._audioSources.Count > i && this._audioSources[i] != null)
				{
					this._audioSources[i].Play();
				}
			}
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00044099 File Offset: 0x00042299
		public override void PlayTrackEvent(int index)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			base.PlayTrackEvent(index);
			this._audioSources[index].Play();
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x000440BB File Offset: 0x000422BB
		public override void EditorMaintenance()
		{
			base.EditorMaintenance();
			this.SetupSounds();
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x000440CC File Offset: 0x000422CC
		public virtual void SetupSounds()
		{
			if (this.Sequence == null)
			{
				return;
			}
			if (this.Sounds.Count < this.Sequence.SequenceTracks.Count)
			{
				for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
				{
					if (i >= this.Sounds.Count)
					{
						this.Sounds.Add(null);
					}
				}
			}
			if (this.Sounds.Count > this.Sequence.SequenceTracks.Count)
			{
				this.Sounds.Clear();
				for (int j = 0; j < this.Sequence.SequenceTracks.Count; j++)
				{
					this.Sounds.Add(null);
				}
			}
		}

		// Token: 0x040011B6 RID: 4534
		[Tooltip("the list of audio clips to play (one per track)")]
		public List<AudioClip> Sounds;

		// Token: 0x040011B7 RID: 4535
		protected List<AudioSource> _audioSources;
	}
}
