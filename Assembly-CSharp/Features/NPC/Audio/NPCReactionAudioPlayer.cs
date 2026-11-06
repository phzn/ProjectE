using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using Features.Detection;
using Features.NPC.StateMachine;
using UnityEngine;

namespace Features.NPC.Audio
{
	// Token: 0x02000F4D RID: 3917
	[RequireComponent(typeof(AudioSource))]
	public class NPCReactionAudioPlayer : MonoBehaviour
	{
		// Token: 0x06007559 RID: 30041 RVA: 0x002F8BE9 File Offset: 0x002F6DE9
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.NPCReactionAudioPlayer_audioProfile)
			{
				return this.audioProfile;
			}
			throw new InvalidOperationException("Unsupported NPCReactionAudioPlayer POI binding: " + field.ToString());
		}

		// Token: 0x0600755A RID: 30042 RVA: 0x002F8C13 File Offset: 0x002F6E13
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.NPCReactionAudioPlayer_audioProfile)
			{
				this.audioProfile = (NPCReactionAudioProfileSO)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported NPCReactionAudioPlayer POI binding: " + field.ToString());
		}

		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x0600755B RID: 30043 RVA: 0x002F8C43 File Offset: 0x002F6E43
		// (set) Token: 0x0600755C RID: 30044 RVA: 0x002F8C4B File Offset: 0x002F6E4B
		public float LastStumbleAudioTime { get; private set; } = float.NegativeInfinity;

		// Token: 0x0600755D RID: 30045 RVA: 0x002F8C54 File Offset: 0x002F6E54
		private void Awake()
		{
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponent<AudioSource>();
			}
			if (this.dialogueHolder == null)
			{
				this.dialogueHolder = base.GetComponent<NPCDialogueHolder>();
			}
			this._fsm = base.GetComponent<NPCStateMachine>();
			if (this.audioSource != null)
			{
				this.audioSource.playOnAwake = false;
				this.audioSource.spatialBlend = 1f;
				this.audioSource.rolloffMode = AudioRolloffMode.Linear;
				this.audioSource.minDistance = 1f;
				this.audioSource.maxDistance = 20f;
			}
		}

		// Token: 0x0600755E RID: 30046 RVA: 0x002F8CF7 File Offset: 0x002F6EF7
		private void OnEnable()
		{
			if (this._fsm != null)
			{
				this._fsm.OnStateChanged += this.OnStateChanged;
			}
		}

		// Token: 0x0600755F RID: 30047 RVA: 0x002F8D1E File Offset: 0x002F6F1E
		private void OnDisable()
		{
			if (this._fsm != null)
			{
				this._fsm.OnStateChanged -= this.OnStateChanged;
			}
		}

		// Token: 0x06007560 RID: 30048 RVA: 0x002F8D45 File Offset: 0x002F6F45
		private void Start()
		{
			if (this.dialogueHolder != null)
			{
				this._cachedGender = this.dialogueHolder.Gender;
			}
		}

		// Token: 0x06007561 RID: 30049 RVA: 0x002F8D68 File Offset: 0x002F6F68
		private void OnStateChanged(NPCStateType oldState, NPCStateType newState, string owner)
		{
			if (this.audioProfile == null || this.audioSource == null)
			{
				return;
			}
			if (newState == NPCStateType.Idle || newState == NPCStateType.Wandering)
			{
				this._stumblePlayedThisDetection = false;
				return;
			}
			if (Time.time - this._lastSoundTime < this.minDelayBetweenSounds)
			{
				return;
			}
			if (!this.allowOverlap && this.audioSource.isPlaying)
			{
				return;
			}
			WeightedAudioClip weightedAudioClip = null;
			switch (newState)
			{
			case NPCStateType.StopAndStare:
				weightedAudioClip = this.GetOrCacheClip(newState, () => this.audioProfile.GetStopAndStareAudio(this._cachedGender));
				break;
			case NPCStateType.Searching:
				if (this._stumblePlayedThisDetection)
				{
					if (NPCDebug.Verbose)
					{
						Debug.Log("[NPCReactionAudio] " + base.gameObject.name + ": Skipping Searching audio (Stumble already played)");
					}
					return;
				}
				weightedAudioClip = this.GetOrCacheClip(newState, () => this.audioProfile.GetSearchingAudio(this._cachedGender));
				break;
			case NPCStateType.Confusion:
				weightedAudioClip = this.GetOrCacheClip(newState, () => this.audioProfile.GetConfusionAudio(this._cachedGender));
				break;
			default:
				if (newState == NPCStateType.Stumbling)
				{
					weightedAudioClip = this.GetOrCacheClip(newState, () => this.audioProfile.GetStumbleAudio(this._cachedGender));
					if (weightedAudioClip != null && weightedAudioClip.clip != null)
					{
						this._stumblePlayedThisDetection = true;
					}
				}
				break;
			}
			if (weightedAudioClip != null && weightedAudioClip.clip != null)
			{
				this.PlayReactionAudio(weightedAudioClip);
				if (newState == NPCStateType.Stumbling)
				{
					this.LastStumbleAudioTime = Time.time;
				}
			}
		}

		// Token: 0x06007562 RID: 30050 RVA: 0x002F8EB8 File Offset: 0x002F70B8
		private WeightedAudioClip GetOrCacheClip(NPCStateType state, Func<WeightedAudioClip> fetchClip)
		{
			WeightedAudioClip weightedAudioClip;
			if (this._cachedClipsPerState.TryGetValue(state, out weightedAudioClip))
			{
				return weightedAudioClip;
			}
			WeightedAudioClip weightedAudioClip2 = fetchClip();
			if (weightedAudioClip2 != null)
			{
				this._cachedClipsPerState[state] = weightedAudioClip2;
			}
			return weightedAudioClip2;
		}

		// Token: 0x06007563 RID: 30051 RVA: 0x002F8EF0 File Offset: 0x002F70F0
		private void PlayReactionAudio(WeightedAudioClip audioClip)
		{
			if (audioClip == null || audioClip.clip == null)
			{
				return;
			}
			float num = audioClip.volume * this.audioProfile.baseVolume;
			float pitchVariation = this.audioProfile.pitchVariation;
			this.audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
			this.audioSource.PlayOneShot(audioClip.clip, num);
			this._lastSoundTime = Time.time;
			if (NPCDebug.Verbose)
			{
				Debug.Log(string.Format("[NPCReactionAudio] {0}: Playing {1} (gender: {2})", base.gameObject.name, audioClip.clip.name, this._cachedGender));
			}
		}

		// Token: 0x06007564 RID: 30052 RVA: 0x002F8F9C File Offset: 0x002F719C
		public void PlayStopAndStareAudio()
		{
			if (this.audioProfile == null)
			{
				return;
			}
			WeightedAudioClip stopAndStareAudio = this.audioProfile.GetStopAndStareAudio(this._cachedGender);
			if (stopAndStareAudio != null)
			{
				this.PlayReactionAudio(stopAndStareAudio);
			}
		}

		// Token: 0x06007565 RID: 30053 RVA: 0x002F8FD4 File Offset: 0x002F71D4
		public void PlayStumbleAudio()
		{
			if (this.audioProfile == null)
			{
				return;
			}
			WeightedAudioClip stumbleAudio = this.audioProfile.GetStumbleAudio(this._cachedGender);
			if (stumbleAudio != null && stumbleAudio.clip != null)
			{
				this._stumblePlayedThisDetection = true;
				this.PlayReactionAudio(stumbleAudio);
				this.LastStumbleAudioTime = Time.time;
			}
		}

		// Token: 0x06007566 RID: 30054 RVA: 0x002F902C File Offset: 0x002F722C
		public void PlaySearchingAudio()
		{
			if (this.audioProfile == null)
			{
				return;
			}
			WeightedAudioClip searchingAudio = this.audioProfile.GetSearchingAudio(this._cachedGender);
			if (searchingAudio != null)
			{
				this.PlayReactionAudio(searchingAudio);
			}
		}

		// Token: 0x06007567 RID: 30055 RVA: 0x002F9064 File Offset: 0x002F7264
		public void PlayConfusionAudio()
		{
			if (this.audioProfile == null)
			{
				return;
			}
			WeightedAudioClip confusionAudio = this.audioProfile.GetConfusionAudio(this._cachedGender);
			if (confusionAudio != null)
			{
				this.PlayReactionAudio(confusionAudio);
			}
		}

		// Token: 0x06007568 RID: 30056 RVA: 0x002F909C File Offset: 0x002F729C
		public void UpdateGender(Gender gender)
		{
			this._cachedGender = gender;
		}

		// Token: 0x06007569 RID: 30057 RVA: 0x002F90A5 File Offset: 0x002F72A5
		public void SetAudioProfile(NPCReactionAudioProfileSO profile)
		{
			this.audioProfile = profile;
		}

		// Token: 0x04007838 RID: 30776
		[Header("Audio Profile")]
		[Tooltip("Profile containing reaction audio clips")]
		[SerializeField]
		private NPCReactionAudioProfileSO audioProfile;

		// Token: 0x04007839 RID: 30777
		[Header("References")]
		[Tooltip("AudioSource for playing reaction sounds (auto-assigned if null)")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400783A RID: 30778
		[Tooltip("Reference to NPCDialogueHolder to get gender info")]
		[SerializeField]
		private NPCDialogueHolder dialogueHolder;

		// Token: 0x0400783B RID: 30779
		[Header("Settings")]
		[Tooltip("Minimum delay between reaction sounds to prevent spam")]
		[SerializeField]
		private float minDelayBetweenSounds = 0.5f;

		// Token: 0x0400783C RID: 30780
		[Tooltip("If true, will play audio even if another sound is playing")]
		[SerializeField]
		private bool allowOverlap;

		// Token: 0x0400783D RID: 30781
		private NPCStateMachine _fsm;

		// Token: 0x0400783E RID: 30782
		private float _lastSoundTime;

		// Token: 0x0400783F RID: 30783
		private Gender _cachedGender;

		// Token: 0x04007840 RID: 30784
		private bool _stumblePlayedThisDetection;

		// Token: 0x04007841 RID: 30785
		private Dictionary<NPCStateType, WeightedAudioClip> _cachedClipsPerState = new Dictionary<NPCStateType, WeightedAudioClip>();
	}
}
