using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200021E RID: 542
	[AddComponentMenu("More Mountains/Feedbacks/Sequencing/MMInputSequenceRecorder")]
	[ExecuteAlways]
	public class MMInputSequenceRecorder : MonoBehaviour
	{
		// Token: 0x06000EEF RID: 3823 RVA: 0x00042C9D File Offset: 0x00040E9D
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00042CA5 File Offset: 0x00040EA5
		public virtual void Initialization()
		{
			this.Recording = false;
			this._note = new MMSequenceNote();
			if (this.SequenceScriptableObject == null)
			{
				Debug.LogError(base.name + " this input based sequencer needs a bound scriptable object to function, please create one and bind it in the inspector.");
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00042CDC File Offset: 0x00040EDC
		protected virtual void Start()
		{
			if (this.StartRecordingOnGameStart)
			{
				this.StartRecording();
			}
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00042CEC File Offset: 0x00040EEC
		public virtual void StartRecording()
		{
			this.Recording = true;
			if (!this.AdditiveRecording)
			{
				this.SequenceScriptableObject.OriginalSequence.Line.Clear();
			}
			this._recordingStartedAt = Time.realtimeSinceStartup;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00042D1D File Offset: 0x00040F1D
		public virtual void StopRecording()
		{
			this.Recording = false;
			this.SequenceScriptableObject.QuantizeOriginalSequence();
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00042D31 File Offset: 0x00040F31
		protected virtual void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.DetectStartAndEnd();
			this.DetectRecording();
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00042D47 File Offset: 0x00040F47
		protected virtual void DetectStartAndEnd()
		{
			if (!this.Recording)
			{
				if (Input.GetKeyDown(this.StartRecordingHotkey))
				{
					this.StartRecording();
					return;
				}
			}
			else if (Input.GetKeyDown(this.StartRecordingHotkey))
			{
				this.StopRecording();
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00042D78 File Offset: 0x00040F78
		protected virtual void DetectRecording()
		{
			if (this.Recording && this.SequenceScriptableObject != null)
			{
				foreach (MMSequenceTrack mmsequenceTrack in this.SequenceScriptableObject.SequenceTracks)
				{
					if (Input.GetKeyDown(mmsequenceTrack.Key))
					{
						this.AddNoteToTrack(mmsequenceTrack);
					}
				}
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00042DF4 File Offset: 0x00040FF4
		public virtual void AddNoteToTrack(MMSequenceTrack track)
		{
			if (this.SequenceScriptableObject.OriginalSequence.Line.Count == 0 && this.RemoveInitialSilence)
			{
				this._recordingStartedAt = Time.realtimeSinceStartup;
			}
			this._note = new MMSequenceNote();
			this._note.ID = track.ID;
			this._note.Timestamp = Time.realtimeSinceStartup + this.RecordingStartOffset - this._recordingStartedAt;
			this.SequenceScriptableObject.OriginalSequence.Line.Add(this._note);
		}

		// Token: 0x0400117F RID: 4479
		[Header("Target")]
		[Tooltip("the target scriptable object to write to")]
		public MMSequence SequenceScriptableObject;

		// Token: 0x04001180 RID: 4480
		[Header("Recording")]
		[MMFReadOnly]
		[Tooltip("whether this recorder is recording right now or not")]
		public bool Recording;

		// Token: 0x04001181 RID: 4481
		[Tooltip("whether any silence between the start of the recording and the first press should be removed or not")]
		public bool RemoveInitialSilence = true;

		// Token: 0x04001182 RID: 4482
		[Tooltip("whether this recording should write on top of existing entries or not")]
		public bool AdditiveRecording;

		// Token: 0x04001183 RID: 4483
		[Tooltip("whether this recorder should start recording when entering play mode")]
		public bool StartRecordingOnGameStart;

		// Token: 0x04001184 RID: 4484
		[Tooltip("the offset to apply to entries")]
		public float RecordingStartOffset;

		// Token: 0x04001185 RID: 4485
		[Header("Recorder Keys")]
		[Tooltip("the key binding for recording start")]
		public KeyCode StartRecordingHotkey = KeyCode.Home;

		// Token: 0x04001186 RID: 4486
		[Tooltip("the key binding for recording stop")]
		public KeyCode StopRecordingHotkey = KeyCode.End;

		// Token: 0x04001187 RID: 4487
		protected MMSequenceNote _note;

		// Token: 0x04001188 RID: 4488
		protected float _recordingStartedAt;
	}
}
