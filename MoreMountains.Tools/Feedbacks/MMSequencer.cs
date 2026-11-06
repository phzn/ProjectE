using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000224 RID: 548
	[AddComponentMenu("More Mountains/Feedbacks/Sequencing/MMSequencer")]
	public class MMSequencer : MonoBehaviour
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x00043686 File Offset: 0x00041886
		public float InternalTime
		{
			get
			{
				if (this.TimeScale != MMSequencer.TimeScales.Scaled)
				{
					return Time.unscaledTime;
				}
				return Time.time;
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0004369B File Offset: 0x0004189B
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x000436A4 File Offset: 0x000418A4
		protected virtual void Initialization()
		{
			this.Playing = false;
			if (this.MetronomeSound != null)
			{
				GameObject gameObject = new GameObject();
				SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
				gameObject.name = "BeatSoundAudioSource";
				gameObject.transform.SetParent(base.transform);
				this._beatSoundAudiosource = gameObject.AddComponent<AudioSource>();
				this._beatSoundAudiosource.clip = this.MetronomeSound;
				this._beatSoundAudiosource.loop = false;
				this._beatSoundAudiosource.playOnAwake = false;
			}
			if (this.PlayOnStart)
			{
				this.PlaySequence();
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0004373C File Offset: 0x0004193C
		public virtual void ToggleSequence()
		{
			if (this.Playing)
			{
				this.StopSequence();
				return;
			}
			this.PlaySequence();
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00043753 File Offset: 0x00041953
		public virtual void PlaySequence()
		{
			this.CurrentSequenceIndex = 0;
			this.Playing = true;
			this.LastBeatTimestamp = 0f;
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0004376E File Offset: 0x0004196E
		public virtual void StopSequence()
		{
			this.Playing = false;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00043778 File Offset: 0x00041978
		public virtual void ClearSequence()
		{
			for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
			{
				for (int j = 0; j < this.SequencerLength; j++)
				{
					this.Sequence.QuantizedSequence[i].Line[j].ID = -1;
				}
			}
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x000437D3 File Offset: 0x000419D3
		protected virtual void Update()
		{
			this.HandleBeat();
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x000437DC File Offset: 0x000419DC
		protected virtual void HandleBeat()
		{
			this.BeatThisFrame = false;
			if (!this.Playing)
			{
				return;
			}
			if (this.CurrentSequenceIndex >= this.SequencerLength)
			{
				this.StopSequence();
				return;
			}
			this._beatInterval = 60f / (float)this.BPM;
			if (this.InternalTime - this.LastBeatTimestamp >= this._beatInterval || this.LastBeatTimestamp == 0f)
			{
				this.PlayBeat();
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0004384C File Offset: 0x00041A4C
		public virtual void PlayBeat()
		{
			this.BeatThisFrame = true;
			this.LastBeatIndex = this.CurrentSequenceIndex;
			this.LastBeatTimestamp = this.InternalTime;
			this.PlayedOnce = true;
			this.PlayMetronomeSound();
			this.OnBeat();
			for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
			{
				if (this.Sequence.SequenceTracks[i].Active && this.Sequence.QuantizedSequence[i].Line[this.CurrentSequenceIndex].ID != -1 && this.TrackEvents[i] != null)
				{
					this.TrackEvents[i].Invoke();
				}
			}
			this.CurrentSequenceIndex++;
			if (this.CurrentSequenceIndex >= this.SequencerLength && this.Loop)
			{
				this.CurrentSequenceIndex = 0;
			}
			if (this.RandomSequence)
			{
				this.CurrentSequenceIndex = Random.Range(0, this.SequencerLength);
			}
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0004394B File Offset: 0x00041B4B
		protected virtual void OnBeat()
		{
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0004394D File Offset: 0x00041B4D
		public virtual void PlayTrackEvent(int index)
		{
			this.TrackEvents[index].Invoke();
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00043960 File Offset: 0x00041B60
		public virtual void ToggleActive(int trackIndex)
		{
			this.Sequence.SequenceTracks[trackIndex].Active = !this.Sequence.SequenceTracks[trackIndex].Active;
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00043994 File Offset: 0x00041B94
		public virtual void ToggleStep(int stepIndex)
		{
			bool flag = this.Sequence.QuantizedSequence[0].Line[stepIndex].ID != -1;
			for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
			{
				if (flag)
				{
					this.Sequence.QuantizedSequence[i].Line[stepIndex].ID = -1;
				}
				else
				{
					this.Sequence.QuantizedSequence[i].Line[stepIndex].ID = this.Sequence.SequenceTracks[i].ID;
				}
			}
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00043A42 File Offset: 0x00041C42
		protected virtual void PlayMetronomeSound()
		{
			if (this.MetronomeSound != null)
			{
				this._beatSoundAudiosource.volume = this.MetronomeVolume;
				this._beatSoundAudiosource.Play();
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x00043A70 File Offset: 0x00041C70
		public virtual void IncrementLength()
		{
			if (this.Sequence == null)
			{
				return;
			}
			float num = 60f / (float)this.BPM;
			this.SequencerLength++;
			this.Sequence.Length = (float)this.SequencerLength * num;
			this.LastSequencerLength = this.SequencerLength;
			for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
			{
				MMSequenceNote mmsequenceNote = new MMSequenceNote();
				mmsequenceNote.ID = -1;
				mmsequenceNote.Timestamp = (float)this.Sequence.QuantizedSequence[i].Line.Count * num;
				this.Sequence.QuantizedSequence[i].Line.Add(mmsequenceNote);
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00043B30 File Offset: 0x00041D30
		public virtual void DecrementLength()
		{
			if (this.Sequence == null)
			{
				return;
			}
			float num = 60f / (float)this.BPM;
			this.SequencerLength--;
			this.Sequence.Length = (float)this.SequencerLength * num;
			this.LastSequencerLength = this.SequencerLength;
			for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
			{
				int num2 = this.Sequence.QuantizedSequence[i].Line.Count - 1;
				this.Sequence.QuantizedSequence[i].Line.RemoveAt(num2);
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00043BE0 File Offset: 0x00041DE0
		public virtual void UpdateTimestampsToMatchNewBPM()
		{
			if (this.Sequence == null)
			{
				return;
			}
			float num = 60f / (float)this.BPM;
			this.Sequence.TargetBPM = this.BPM;
			this.Sequence.Length = (float)this.SequencerLength * num;
			this.Sequence.EndSilenceDuration = num;
			this.Sequence.Quantized = true;
			for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
			{
				for (int j = 0; j < this.SequencerLength; j++)
				{
					this.Sequence.QuantizedSequence[i].Line[j].Timestamp = (float)j * num;
				}
			}
			this.LastBPM = this.BPM;
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x00043CA4 File Offset: 0x00041EA4
		public virtual void ApplySequencerLengthToSequence()
		{
			if (this.Sequence == null)
			{
				return;
			}
			float num = 60f / (float)this.BPM;
			this.Sequence.TargetBPM = this.BPM;
			this.Sequence.Length = (float)this.SequencerLength * num;
			this.Sequence.EndSilenceDuration = num;
			this.Sequence.Quantized = true;
			if (this.LastSequencerLength != this.SequencerLength || this.LastTracksCount != this.Sequence.SequenceTracks.Count)
			{
				this.Sequence.QuantizedSequence = new List<MMSequenceList>();
				for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
				{
					this.Sequence.QuantizedSequence.Add(new MMSequenceList());
					this.Sequence.QuantizedSequence[i].Line = new List<MMSequenceNote>();
					for (int j = 0; j < this.SequencerLength; j++)
					{
						MMSequenceNote mmsequenceNote = new MMSequenceNote();
						mmsequenceNote.ID = -1;
						mmsequenceNote.Timestamp = (float)j * num;
						this.Sequence.QuantizedSequence[i].Line.Add(mmsequenceNote);
					}
				}
			}
			this.LastTracksCount = this.Sequence.SequenceTracks.Count;
			this.LastSequencerLength = this.SequencerLength;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00043DF8 File Offset: 0x00041FF8
		public virtual void EditorMaintenance()
		{
			this.SetupTrackEvents();
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00043E00 File Offset: 0x00042000
		public virtual void SetupTrackEvents()
		{
			if (this.Sequence == null)
			{
				return;
			}
			if (this.TrackEvents.Count < this.Sequence.SequenceTracks.Count)
			{
				for (int i = 0; i < this.Sequence.SequenceTracks.Count; i++)
				{
					if (i >= this.TrackEvents.Count)
					{
						this.TrackEvents.Add(new UnityEvent());
					}
				}
			}
			if (this.TrackEvents.Count > this.Sequence.SequenceTracks.Count)
			{
				this.TrackEvents.Clear();
				for (int j = 0; j < this.Sequence.SequenceTracks.Count; j++)
				{
					this.TrackEvents.Add(new UnityEvent());
				}
			}
		}

		// Token: 0x040011A0 RID: 4512
		[Header("Sequence")]
		[Tooltip("the sequence to design on or to play")]
		public MMSequence Sequence;

		// Token: 0x040011A1 RID: 4513
		[Tooltip("the intended BPM for playback and design")]
		public int BPM = 160;

		// Token: 0x040011A2 RID: 4514
		[Tooltip("the number of notes in the sequence")]
		public int SequencerLength = 8;

		// Token: 0x040011A3 RID: 4515
		[Header("Playback")]
		[Tooltip("the timescale on which this sequencer should play")]
		public MMSequencer.TimeScales TimeScale;

		// Token: 0x040011A4 RID: 4516
		[Tooltip("whether the sequence should loop or not when played back")]
		public bool Loop = true;

		// Token: 0x040011A5 RID: 4517
		[Tooltip("if this is true the sequence will play in random order")]
		public bool RandomSequence;

		// Token: 0x040011A6 RID: 4518
		[Tooltip("whether that sequencer should start playing on application start")]
		public bool PlayOnStart;

		// Token: 0x040011A7 RID: 4519
		[Header("Metronome")]
		[Tooltip("a sound to play every beat")]
		public AudioClip MetronomeSound;

		// Token: 0x040011A8 RID: 4520
		[Tooltip("the volume of the metronome sound")]
		[Range(0f, 1f)]
		public float MetronomeVolume = 0.2f;

		// Token: 0x040011A9 RID: 4521
		[Header("Events")]
		[Tooltip("a list of events to play every time an active beat is found on each track (one event per track)")]
		public List<UnityEvent> TrackEvents;

		// Token: 0x040011AA RID: 4522
		[Header("Monitor")]
		[Tooltip("true if the sequencer is playing right now")]
		[MMFReadOnly]
		public bool Playing;

		// Token: 0x040011AB RID: 4523
		[Tooltip("true if the sequencer has been played once")]
		[HideInInspector]
		public bool PlayedOnce;

		// Token: 0x040011AC RID: 4524
		[Tooltip("true if a perfect beat was found this frame")]
		[MMFReadOnly]
		public bool BeatThisFrame;

		// Token: 0x040011AD RID: 4525
		[Tooltip("the index of the last played bit (our position in the playing sequence)")]
		[MMFReadOnly]
		public int LastBeatIndex;

		// Token: 0x040011AE RID: 4526
		[HideInInspector]
		public int LastBPM = -1;

		// Token: 0x040011AF RID: 4527
		[HideInInspector]
		public int LastTracksCount = -1;

		// Token: 0x040011B0 RID: 4528
		[HideInInspector]
		public int LastSequencerLength = -1;

		// Token: 0x040011B1 RID: 4529
		[HideInInspector]
		public MMSequence LastSequence;

		// Token: 0x040011B2 RID: 4530
		[HideInInspector]
		public int CurrentSequenceIndex;

		// Token: 0x040011B3 RID: 4531
		[HideInInspector]
		public float LastBeatTimestamp;

		// Token: 0x040011B4 RID: 4532
		protected float _beatInterval;

		// Token: 0x040011B5 RID: 4533
		protected AudioSource _beatSoundAudiosource;

		// Token: 0x0200050A RID: 1290
		public enum TimeScales
		{
			// Token: 0x04002390 RID: 9104
			Scaled,
			// Token: 0x04002391 RID: 9105
			Unscaled
		}
	}
}
