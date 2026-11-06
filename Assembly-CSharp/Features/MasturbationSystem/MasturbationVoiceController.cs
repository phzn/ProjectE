using System;
using Features.CielVoice;
using Features.Detection;
using Features.PlayerEmotes;
using UnityEngine;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FF8 RID: 4088
	[DisallowMultipleComponent]
	public class MasturbationVoiceController : MonoBehaviour
	{
		// Token: 0x06007BF4 RID: 31732 RVA: 0x00329DF8 File Offset: 0x00327FF8
		private static AnimationCurve BuildEqualPowerInCurve()
		{
			AnimationCurve animationCurve = new AnimationCurve();
			for (int i = 0; i <= 8; i++)
			{
				float num = (float)i / 8f;
				animationCurve.AddKey(num, Mathf.Sqrt(num));
			}
			for (int j = 0; j < animationCurve.length; j++)
			{
				animationCurve.SmoothTangents(j, 0f);
			}
			return animationCurve;
		}

		// Token: 0x06007BF5 RID: 31733 RVA: 0x00329E4C File Offset: 0x0032804C
		private static AnimationCurve BuildEqualPowerOutCurve()
		{
			AnimationCurve animationCurve = new AnimationCurve();
			for (int i = 0; i <= 8; i++)
			{
				float num = (float)i / 8f;
				animationCurve.AddKey(num, Mathf.Sqrt(1f - num));
			}
			for (int j = 0; j < animationCurve.length; j++)
			{
				animationCurve.SmoothTangents(j, 0f);
			}
			return animationCurve;
		}

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06007BF6 RID: 31734 RVA: 0x00329EA8 File Offset: 0x003280A8
		// (remove) Token: 0x06007BF7 RID: 31735 RVA: 0x00329EDC File Offset: 0x003280DC
		public static event Action<bool> OnMasturbationVoiceActiveChanged;

		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x06007BF8 RID: 31736 RVA: 0x00329F0F File Offset: 0x0032810F
		public static bool IsMasturbationVoiceActive
		{
			get
			{
				return MasturbationVoiceController._isMasturbationVoiceActive;
			}
		}

		// Token: 0x06007BF9 RID: 31737 RVA: 0x00329F18 File Offset: 0x00328118
		private void Awake()
		{
			if (this.emoteController == null)
			{
				this.emoteController = base.GetComponent<GameplayEmoteController>();
			}
			if (this.propHandler == null)
			{
				this.propHandler = base.GetComponent<EmotePropHandler>();
			}
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponent<AudioSource>();
			}
			if (this.audioSourceB == null && this.audioSource != null)
			{
				this.audioSourceB = this.audioSource.gameObject.AddComponent<AudioSource>();
				this.audioSourceB.playOnAwake = false;
				this.audioSourceB.spatialBlend = this.audioSource.spatialBlend;
				this.audioSourceB.outputAudioMixerGroup = this.audioSource.outputAudioMixerGroup;
			}
			if (this.audioSource != null)
			{
				this.audioSource.loop = false;
				this.audioSource.playOnAwake = false;
				this.audioSource.volume = 0f;
			}
			if (this.audioSourceB != null)
			{
				this.audioSourceB.loop = false;
				this.audioSourceB.playOnAwake = false;
				this.audioSourceB.volume = 0f;
			}
			if (this.endingSource == null && this.audioSource != null)
			{
				this.endingSource = this.audioSource.gameObject.AddComponent<AudioSource>();
				this.endingSource.playOnAwake = false;
				this.endingSource.spatialBlend = this.audioSource.spatialBlend;
				this.endingSource.outputAudioMixerGroup = this.audioSource.outputAudioMixerGroup;
			}
			if (this.endingSource != null)
			{
				this.endingSource.loop = this.endingLoop;
				this.endingSource.playOnAwake = false;
				this.endingSource.volume = 0f;
			}
			MasturbationVoiceController.PreloadAudioClips(this.baseLoopClips);
			MasturbationVoiceController.PreloadAudioClips(this.endingClips);
		}

		// Token: 0x06007BFA RID: 31738 RVA: 0x0032A104 File Offset: 0x00328304
		private static void PreloadAudioClips(AudioClip[] clips)
		{
			if (clips == null)
			{
				return;
			}
			foreach (AudioClip audioClip in clips)
			{
				if (audioClip != null && audioClip.loadState == AudioDataLoadState.Unloaded)
				{
					audioClip.LoadAudioData();
				}
			}
		}

		// Token: 0x06007BFB RID: 31739 RVA: 0x0032A140 File Offset: 0x00328340
		private void OnEnable()
		{
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted += this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded += this.HandleEmoteEnded;
			}
			if (this.propHandler != null)
			{
				this.propHandler.OnPropActivated += this.HandlePropActivated;
				this.propHandler.OnPropDeactivated += this.HandlePropDeactivated;
			}
			this.TrySubscribeMeter();
		}

		// Token: 0x06007BFC RID: 31740 RVA: 0x0032A1CC File Offset: 0x003283CC
		private void TrySubscribeMeter()
		{
			if (this._meterSubscribed)
			{
				return;
			}
			OrgasmMeterController instance = OrgasmMeterController.Instance;
			if (instance != null)
			{
				instance.OnMaxReached += this.HandleOrgasmMaxReached;
				this._meterSubscribed = true;
			}
		}

		// Token: 0x06007BFD RID: 31741 RVA: 0x0032A20C File Offset: 0x0032840C
		private void OnDisable()
		{
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			if (this.propHandler != null)
			{
				this.propHandler.OnPropActivated -= this.HandlePropActivated;
				this.propHandler.OnPropDeactivated -= this.HandlePropDeactivated;
			}
			if (this._meterSubscribed && OrgasmMeterController.Instance != null)
			{
				OrgasmMeterController.Instance.OnMaxReached -= this.HandleOrgasmMaxReached;
			}
			this._meterSubscribed = false;
			CrossfadeLoopPlayer baseLoop = this._baseLoop;
			if (baseLoop != null)
			{
				baseLoop.Stop();
			}
			if (this.audioSource != null && this.audioSource.isPlaying)
			{
				this.audioSource.Stop();
			}
			if (this.audioSourceB != null && this.audioSourceB.isPlaying)
			{
				this.audioSourceB.Stop();
			}
			if (this.endingSource != null && this.endingSource.isPlaying)
			{
				this.endingSource.Stop();
			}
			MasturbationVoiceController.SetVoiceActive(false);
			this._state = MasturbationVoiceController.State.Idle;
			this._fadeWeight = 0f;
			this._fadeElapsed = 0f;
			this._endingState = MasturbationVoiceController.EndingState.Idle;
			this._endingElapsed = 0f;
			this._endingWeight = 0f;
			this._baseDuckWeight = 1f;
			this._activeEndingLabel = null;
			this._isMasturbationEmoteActive = false;
			this._externalSessionOwner = null;
		}

		// Token: 0x06007BFE RID: 31742 RVA: 0x0032A3A6 File Offset: 0x003285A6
		public bool TryBeginExternalSession(Object owner)
		{
			if (owner == null || !base.isActiveAndEnabled || this._isMasturbationEmoteActive)
			{
				return false;
			}
			if (!this.BeginVoiceSession())
			{
				return false;
			}
			this._externalSessionOwner = owner;
			return true;
		}

		// Token: 0x06007BFF RID: 31743 RVA: 0x0032A3D5 File Offset: 0x003285D5
		public void PlayExternalEnding(Object owner)
		{
			if (owner != null && this._externalSessionOwner == owner && this._isMasturbationEmoteActive)
			{
				this.StartEnding("Orgasm");
			}
		}

		// Token: 0x06007C00 RID: 31744 RVA: 0x0032A401 File Offset: 0x00328601
		public void EndExternalSession(Object owner)
		{
			if (owner == null || this._externalSessionOwner != owner)
			{
				return;
			}
			this._externalSessionOwner = null;
			this.EndVoiceSession();
		}

		// Token: 0x06007C01 RID: 31745 RVA: 0x0032A428 File Offset: 0x00328628
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (emote == null || !emote.IsMasturbationEmote)
			{
				return;
			}
			if (this._externalSessionOwner != null)
			{
				return;
			}
			this.BeginVoiceSession();
		}

		// Token: 0x06007C02 RID: 31746 RVA: 0x0032A454 File Offset: 0x00328654
		private bool BeginVoiceSession()
		{
			if (this.audioSource == null || this.audioSourceB == null)
			{
				return false;
			}
			this._currentBaseClip = MasturbationVoiceController.PickRandomClip(this.baseLoopClips, null);
			if (this._currentBaseClip == null)
			{
				if (this.debugLog)
				{
					Debug.LogWarning("[MasturbationVoice] Nessuna baseLoopClip (ToyOrgasm) assegnata, skip.");
				}
				return false;
			}
			this.TrySubscribeMeter();
			if (this._baseLoop == null)
			{
				this._baseLoop = new CrossfadeLoopPlayer(this.audioSource, this.audioSourceB, this._currentBaseClip, this.loopCrossfade, 0f, 0f);
			}
			else
			{
				this._baseLoop.SetClip(this._currentBaseClip);
			}
			this._isMasturbationEmoteActive = true;
			this._state = MasturbationVoiceController.State.FadingIn;
			this._fadeElapsed = 0f;
			this._fadeWeight = 0f;
			this._baseDuckWeight = 1f;
			this._endingState = MasturbationVoiceController.EndingState.Idle;
			this._endingElapsed = 0f;
			this._endingWeight = 0f;
			this._activeEndingLabel = null;
			if (this.endingSource != null)
			{
				this.endingSource.Stop();
				this.endingSource.volume = 0f;
			}
			this._baseLoop.Stop();
			this._baseLoop.Play();
			MasturbationVoiceController.SetVoiceActive(true);
			if (this.debugLog)
			{
				Debug.Log(string.Format("[MasturbationVoice] Fade-in start ({0}s) clip='{1}'.", this.fadeInDuration, this._currentBaseClip.name));
			}
			return true;
		}

		// Token: 0x06007C03 RID: 31747 RVA: 0x0032A5C7 File Offset: 0x003287C7
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			if (emote == null || !emote.IsMasturbationEmote)
			{
				return;
			}
			if (this._externalSessionOwner != null)
			{
				return;
			}
			this.EndVoiceSession();
		}

		// Token: 0x06007C04 RID: 31748 RVA: 0x0032A5F0 File Offset: 0x003287F0
		private void EndVoiceSession()
		{
			this._isMasturbationEmoteActive = false;
			if (this._endingState == MasturbationVoiceController.EndingState.FadingIn || this._endingState == MasturbationVoiceController.EndingState.Sustaining)
			{
				this._endingFadeOutStartWeight = this._endingWeight;
				this._endingState = MasturbationVoiceController.EndingState.FadingOut;
				this._endingElapsed = 0f;
				if (this.debugLog)
				{
					Debug.Log(string.Format("[MasturbationVoice] Emote ended -> ending fade-out ({0}s) from weight={1:F2}.", this.endingFadeOutDuration, this._endingWeight));
				}
			}
			if (this._state == MasturbationVoiceController.State.Idle)
			{
				return;
			}
			this._state = MasturbationVoiceController.State.FadingOut;
			this._fadeElapsed = 0f;
			if (this.debugLog)
			{
				Debug.Log(string.Format("[MasturbationVoice] Fade-out start ({0}s) from weight={1:F2}.", this.fadeOutDuration, this._fadeWeight));
			}
		}

		// Token: 0x06007C05 RID: 31749 RVA: 0x0032A6A9 File Offset: 0x003288A9
		private void HandleOrgasmMaxReached()
		{
			if (!this._isMasturbationEmoteActive)
			{
				return;
			}
			this.StartEnding("Orgasm");
		}

		// Token: 0x06007C06 RID: 31750 RVA: 0x0032A6BF File Offset: 0x003288BF
		private void HandlePropActivated(string label)
		{
			if (!this._isMasturbationEmoteActive)
			{
				return;
			}
			if (!this.IsEndingLabel(label))
			{
				return;
			}
			this.StartEnding(label);
		}

		// Token: 0x06007C07 RID: 31751 RVA: 0x0032A6DC File Offset: 0x003288DC
		private void StartEnding(string label)
		{
			if (this.endingSource == null)
			{
				return;
			}
			if (this.endingClips == null || this.endingClips.Length == 0)
			{
				return;
			}
			if (this._endingState != MasturbationVoiceController.EndingState.Idle && string.Equals(this._activeEndingLabel, label, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			AudioClip audioClip = MasturbationVoiceController.PickRandomClip(this.endingClips, this._currentEndingClip);
			if (audioClip == null)
			{
				return;
			}
			this._currentEndingClip = audioClip;
			this.endingSource.clip = audioClip;
			this.endingSource.loop = this.endingLoop;
			this.endingSource.volume = 0f;
			this.endingSource.Play();
			this._endingState = MasturbationVoiceController.EndingState.FadingIn;
			this._endingElapsed = 0f;
			this._endingWeight = 0f;
			this._activeEndingLabel = label;
			if (this.debugLog)
			{
				Debug.Log(string.Format("[MasturbationVoice] Orgasm ending start ('{0}') clip='{1}' fadeIn={2}s.", label, audioClip.name, this.endingFadeInDuration));
			}
		}

		// Token: 0x06007C08 RID: 31752 RVA: 0x0032A7CC File Offset: 0x003289CC
		private void HandlePropDeactivated(string label)
		{
			if (this._endingState == MasturbationVoiceController.EndingState.Idle)
			{
				return;
			}
			if (!string.Equals(this._activeEndingLabel, label, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			if (this._endingState == MasturbationVoiceController.EndingState.FadingOut)
			{
				return;
			}
			this._endingFadeOutStartWeight = this._endingWeight;
			this._endingState = MasturbationVoiceController.EndingState.FadingOut;
			this._endingElapsed = 0f;
			if (this.debugLog)
			{
				Debug.Log(string.Format("[MasturbationVoice] Ending fade-out for prop '{0}' ({1}s) from weight={2:F2}.", label, this.endingFadeOutDuration, this._endingWeight));
			}
		}

		// Token: 0x06007C09 RID: 31753 RVA: 0x0032A848 File Offset: 0x00328A48
		private bool IsEndingLabel(string label)
		{
			if (this.endingPropLabels == null || this.endingPropLabels.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.endingPropLabels.Length; i++)
			{
				if (!string.IsNullOrEmpty(this.endingPropLabels[i]) && string.Equals(this.endingPropLabels[i], label, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C0A RID: 31754 RVA: 0x0032A89E File Offset: 0x00328A9E
		private static void SetVoiceActive(bool active)
		{
			if (MasturbationVoiceController._isMasturbationVoiceActive == active)
			{
				return;
			}
			MasturbationVoiceController._isMasturbationVoiceActive = active;
			Action<bool> onMasturbationVoiceActiveChanged = MasturbationVoiceController.OnMasturbationVoiceActiveChanged;
			if (onMasturbationVoiceActiveChanged == null)
			{
				return;
			}
			onMasturbationVoiceActiveChanged(active);
		}

		// Token: 0x06007C0B RID: 31755 RVA: 0x0032A8C0 File Offset: 0x00328AC0
		private static AudioClip PickRandomClip(AudioClip[] clips, AudioClip exclude = null)
		{
			if (clips == null || clips.Length == 0)
			{
				return null;
			}
			int num = clips.Length;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (clips[i] != null && clips[i] != exclude)
				{
					num2++;
				}
			}
			if (num2 == 0)
			{
				exclude = null;
				for (int j = 0; j < num; j++)
				{
					if (clips[j] != null)
					{
						num2++;
					}
				}
				if (num2 == 0)
				{
					return null;
				}
			}
			int num3 = Random.Range(0, num2);
			int num4 = 0;
			for (int k = 0; k < num; k++)
			{
				if (!(clips[k] == null) && !(clips[k] == exclude))
				{
					if (num4 == num3)
					{
						return clips[k];
					}
					num4++;
				}
			}
			return null;
		}

		// Token: 0x06007C0C RID: 31756 RVA: 0x0032A973 File Offset: 0x00328B73
		private void Update()
		{
			this.TickVoice(Time.deltaTime);
		}

		// Token: 0x06007C0D RID: 31757 RVA: 0x0032A980 File Offset: 0x00328B80
		private void TickVoice(float dt)
		{
			if (this._isMasturbationEmoteActive)
			{
				this.TrySubscribeMeter();
			}
			if (this.audioSource == null)
			{
				return;
			}
			if (this._state == MasturbationVoiceController.State.Idle && this._endingState == MasturbationVoiceController.EndingState.Idle)
			{
				return;
			}
			switch (this._state)
			{
			case MasturbationVoiceController.State.FadingIn:
				if (this.fadeInDuration <= 0f)
				{
					this._fadeWeight = 1f;
					this._state = MasturbationVoiceController.State.Sustaining;
				}
				else
				{
					this._fadeElapsed += dt;
					float num = Mathf.Clamp01(this._fadeElapsed / this.fadeInDuration);
					this._fadeWeight = Mathf.Clamp01(this.fadeInCurve.Evaluate(num));
					if (num >= 1f)
					{
						this._state = MasturbationVoiceController.State.Sustaining;
					}
				}
				break;
			case MasturbationVoiceController.State.Sustaining:
				this._fadeWeight = 1f;
				break;
			case MasturbationVoiceController.State.FadingOut:
				if (this.fadeOutDuration <= 0f)
				{
					this._fadeWeight = 0f;
				}
				else
				{
					this._fadeElapsed += dt;
					float num2 = Mathf.Clamp01(this._fadeElapsed / this.fadeOutDuration);
					this._fadeWeight = Mathf.Clamp01(this.fadeOutCurve.Evaluate(num2));
				}
				if (this._fadeWeight <= 0.001f)
				{
					CrossfadeLoopPlayer baseLoop = this._baseLoop;
					if (baseLoop != null)
					{
						baseLoop.Stop();
					}
					this._state = MasturbationVoiceController.State.Idle;
					this._fadeWeight = 0f;
					this._fadeElapsed = 0f;
					if (this.debugLog)
					{
						Debug.Log("[MasturbationVoice] Fade-out complete, base loop stopped.");
					}
					if (this._endingState == MasturbationVoiceController.EndingState.Idle)
					{
						MasturbationVoiceController.SetVoiceActive(false);
						return;
					}
				}
				break;
			}
			float num3 = ((OrgasmMeterController.Instance != null) ? OrgasmMeterController.Instance.Value : 0f);
			float num4 = Mathf.Lerp(this.volumeAtZero, this.volumeAtMax, num3);
			float num5 = Mathf.Lerp(this.pitchAtZero, this.pitchAtMax, num3);
			switch (this._endingState)
			{
			case MasturbationVoiceController.EndingState.Idle:
				if (this._endingWeight > 0f)
				{
					this._endingWeight = 0f;
				}
				if (this._externalSessionOwner != null && this._isMasturbationEmoteActive)
				{
					this._baseDuckWeight = Mathf.MoveTowards(this._baseDuckWeight, 1f, dt / Mathf.Max(0.02f, this.endingFadeOutDuration));
				}
				break;
			case MasturbationVoiceController.EndingState.FadingIn:
				if (this.endingFadeInDuration <= 0f)
				{
					this._endingWeight = 1f;
					this._baseDuckWeight = Mathf.Clamp01(this.baseDuckCurve.Evaluate(1f));
					this._endingState = MasturbationVoiceController.EndingState.Sustaining;
				}
				else
				{
					this._endingElapsed += dt;
					float num6 = Mathf.Clamp01(this._endingElapsed / this.endingFadeInDuration);
					this._endingWeight = Mathf.Clamp01(this.endingFadeInCurve.Evaluate(num6));
					this._baseDuckWeight = Mathf.Clamp01(this.baseDuckCurve.Evaluate(num6));
					if (num6 >= 1f)
					{
						this._endingState = MasturbationVoiceController.EndingState.Sustaining;
					}
				}
				break;
			case MasturbationVoiceController.EndingState.Sustaining:
				this._endingWeight = 1f;
				this._baseDuckWeight = Mathf.Clamp01(this.baseDuckCurve.Evaluate(1f));
				if (!this.endingLoop && this.endingSource != null && !this.endingSource.isPlaying)
				{
					this._endingState = MasturbationVoiceController.EndingState.Idle;
					this._endingWeight = 0f;
					this._endingElapsed = 0f;
					this._activeEndingLabel = null;
					if (this.debugLog)
					{
						Debug.Log("[MasturbationVoice] Ending clip finished naturally.");
					}
				}
				break;
			case MasturbationVoiceController.EndingState.FadingOut:
				if (this.endingFadeOutDuration <= 0f)
				{
					this._endingWeight = 0f;
					this._baseDuckWeight = Mathf.Clamp01(this.baseRecoverCurve.Evaluate(1f));
				}
				else
				{
					this._endingElapsed += dt;
					float num7 = Mathf.Clamp01(this._endingElapsed / this.endingFadeOutDuration);
					this._endingWeight = Mathf.Clamp01(this.endingFadeOutCurve.Evaluate(num7)) * this._endingFadeOutStartWeight;
					this._baseDuckWeight = Mathf.Clamp01(this.baseRecoverCurve.Evaluate(num7));
				}
				if (this._endingWeight <= 0.001f)
				{
					if (this.endingSource != null)
					{
						this.endingSource.volume = 0f;
						this.endingSource.Stop();
					}
					this._endingState = MasturbationVoiceController.EndingState.Idle;
					this._endingWeight = 0f;
					this._endingElapsed = 0f;
					this._activeEndingLabel = null;
					if (this.debugLog)
					{
						Debug.Log("[MasturbationVoice] Ending fade-out complete.");
					}
				}
				break;
			}
			if (this.endingSource != null && this._endingState != MasturbationVoiceController.EndingState.Idle)
			{
				float num8 = this.endingVolume * Mathf.Lerp(this.volumeAtZero, this.volumeAtMax, num3);
				this.endingSource.volume = num8 * this._endingWeight;
				this.endingSource.pitch = num5;
			}
			if (this._baseLoop != null)
			{
				float num9 = num4 * this._fadeWeight * this._baseDuckWeight;
				this._baseLoop.Tick(num9);
				if (this.audioSource != null)
				{
					this.audioSource.pitch = num5;
				}
				if (this.audioSourceB != null)
				{
					this.audioSourceB.pitch = num5;
				}
			}
			MasturbationVoiceController.SetVoiceActive(this._state != MasturbationVoiceController.State.Idle || this._endingState > MasturbationVoiceController.EndingState.Idle);
		}

		// Token: 0x04007F9F RID: 32671
		[Header("References")]
		[Tooltip("GameplayEmoteController da osservare. Auto-resolved dal GameObject corrente se null.")]
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x04007FA0 RID: 32672
		[Tooltip("EmotePropHandler da osservare per sync degli ending (Cum/Squirt). Auto-resolved dal GameObject corrente se null.")]
		[SerializeField]
		private EmotePropHandler propHandler;

		// Token: 0x04007FA1 RID: 32673
		[Tooltip("AudioSource A per il loop base (crossfade). PlayOnAwake=false. Il loop e' gestito via CrossfadeLoopPlayer (2 source incrociati) per una giuntura pulita senza click/gap.")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04007FA2 RID: 32674
		[Tooltip("AudioSource B per il loop base (crossfade). PlayOnAwake=false. Se null viene creato a runtime accanto al source A.")]
		[SerializeField]
		private AudioSource audioSourceB;

		// Token: 0x04007FA3 RID: 32675
		[Tooltip("AudioSource dedicato agli ending (Orgasm). Loop=false, PlayOnAwake=false. Se null viene aggiunto a runtime sullo stesso GameObject del source base.")]
		[SerializeField]
		private AudioSource endingSource;

		// Token: 0x04007FA4 RID: 32676
		[Tooltip("Clip candidate per il loop base (ToyOrgasm): ne viene pescata UNA a caso ogni volta che parte una masturbation emote, e loopata (crossfade) per tutta la durata.")]
		[SerializeField]
		private AudioClip[] baseLoopClips;

		// Token: 0x04007FA5 RID: 32677
		[Tooltip("Durata (s) del crossfade al giro del loop base. Piu' lungo = giuntura piu' morbida.")]
		[Min(0.02f)]
		[SerializeField]
		private float loopCrossfade = 0.3f;

		// Token: 0x04007FA6 RID: 32678
		[Header("Volume")]
		[Tooltip("Volume target a OrgasmMeter.Value = 0 (inizio emote, eccitazione bassa).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float volumeAtZero = 0.15f;

		// Token: 0x04007FA7 RID: 32679
		[Tooltip("Volume target a OrgasmMeter.Value = 1 (climax).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float volumeAtMax = 1f;

		// Token: 0x04007FA8 RID: 32680
		[Header("Pitch")]
		[Tooltip("Pitch target a OrgasmMeter.Value = 0.")]
		[Range(0.5f, 2f)]
		[SerializeField]
		private float pitchAtZero = 0.95f;

		// Token: 0x04007FA9 RID: 32681
		[Tooltip("Pitch target a OrgasmMeter.Value = 1.")]
		[Range(0.5f, 2f)]
		[SerializeField]
		private float pitchAtMax = 1.1f;

		// Token: 0x04007FAA RID: 32682
		[Header("Fade In/Out")]
		[Tooltip("Durata in secondi del fade-in all'inizio dell'emote.")]
		[Min(0f)]
		[SerializeField]
		private float fadeInDuration = 0.8f;

		// Token: 0x04007FAB RID: 32683
		[Tooltip("Durata in secondi del fade-out alla fine dell'emote.")]
		[Min(0f)]
		[SerializeField]
		private float fadeOutDuration = 1.2f;

		// Token: 0x04007FAC RID: 32684
		[Tooltip("Curva di easing per il fade-in (X=tempo normalizzato 0..1, Y=peso 0..1).")]
		[SerializeField]
		private AnimationCurve fadeInCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04007FAD RID: 32685
		[Tooltip("Curva di easing per il fade-out (X=tempo normalizzato 0..1, Y=peso 0..1 dove 1=full volume, 0=muto).")]
		[SerializeField]
		private AnimationCurve fadeOutCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

		// Token: 0x04007FAE RID: 32686
		[Header("Ending Clips (Orgasm)")]
		[Tooltip("Lista di clip di ORGASM (Orgasm1..4): ne viene scelta UNA a caso quando l'OrgasmMeter raggiunge il massimo (OnMaxReached). Sostituisce il moan base con un crossfade.")]
		[SerializeField]
		private AudioClip[] endingClips;

		// Token: 0x04007FAF RID: 32687
		[Tooltip("Se true, l'orgasm clip viene messa in loop finche' resta attiva. Se false (consigliato per orgasm vocals), suona una volta sola fino al termine naturale.")]
		[SerializeField]
		private bool endingLoop;

		// Token: 0x04007FB0 RID: 32688
		[Tooltip("[LEGACY/opzionale] Label dei prop (EmotePropHandler) che possono ANCHE far partire l'ending. Ora l'orgasm parte principalmente su OrgasmMeter.OnMaxReached; lasciare vuoto per triggerare SOLO al meter pieno.")]
		[SerializeField]
		private string[] endingPropLabels = new string[0];

		// Token: 0x04007FB1 RID: 32689
		[Tooltip("Volume target dell'ending clip a peso pieno (prima della modulazione globale).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float endingVolume = 1f;

		// Token: 0x04007FB2 RID: 32690
		[Tooltip("Durata del crossfade base->ending all'attivazione del prop (secondi). Il base sfuma e l'ending sale in parallelo su questa durata. Tenerlo abbastanza lungo (0.6-1.2s) aiuta a evitare un blending innaturale.")]
		[Min(0f)]
		[SerializeField]
		private float endingFadeInDuration = 0.8f;

		// Token: 0x04007FB3 RID: 32691
		[Tooltip("Durata del fade-out dell'ending quando il prop si disattiva o la clip finisce.")]
		[Min(0f)]
		[SerializeField]
		private float endingFadeOutDuration = 1f;

		// Token: 0x04007FB4 RID: 32692
		[Tooltip("Curva di salita per il volume dell'ending clip (0..1 nel tempo del fade-in). Default sqrt-like (equal-power) per un crossfade naturale che non 'buca' al centro.")]
		[SerializeField]
		private AnimationCurve endingFadeInCurve = MasturbationVoiceController.BuildEqualPowerInCurve();

		// Token: 0x04007FB5 RID: 32693
		[Tooltip("Curva di discesa per il volume dell'ending clip quando il prop si disattiva (1..0). Default equal-power complementare al fade-in.")]
		[SerializeField]
		private AnimationCurve endingFadeOutCurve = MasturbationVoiceController.BuildEqualPowerOutCurve();

		// Token: 0x04007FB6 RID: 32694
		[Tooltip("Curva applicata al base loop durante il fade-in dell'ending (1..0). Equal-power complementare al fade-in dell'ending: cosi' la somma delle due 'potenze' resta costante.")]
		[SerializeField]
		private AnimationCurve baseDuckCurve = MasturbationVoiceController.BuildEqualPowerOutCurve();

		// Token: 0x04007FB7 RID: 32695
		[Tooltip("Curva di risalita del base quando l'ending fa fade-out (0..1). Tipicamente piatta a 0: quando l'ending finisce di solito anche l'emote sta terminando.")]
		[SerializeField]
		private AnimationCurve baseRecoverCurve = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x04007FB8 RID: 32696
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04007FBA RID: 32698
		private static bool _isMasturbationVoiceActive;

		// Token: 0x04007FBB RID: 32699
		private MasturbationVoiceController.State _state;

		// Token: 0x04007FBC RID: 32700
		private float _fadeElapsed;

		// Token: 0x04007FBD RID: 32701
		private float _fadeWeight;

		// Token: 0x04007FBE RID: 32702
		private MasturbationVoiceController.EndingState _endingState;

		// Token: 0x04007FBF RID: 32703
		private float _endingElapsed;

		// Token: 0x04007FC0 RID: 32704
		private float _endingWeight;

		// Token: 0x04007FC1 RID: 32705
		private float _baseDuckWeight = 1f;

		// Token: 0x04007FC2 RID: 32706
		private string _activeEndingLabel;

		// Token: 0x04007FC3 RID: 32707
		private AudioClip _currentEndingClip;

		// Token: 0x04007FC4 RID: 32708
		private float _endingFadeOutStartWeight = 1f;

		// Token: 0x04007FC5 RID: 32709
		private bool _isMasturbationEmoteActive;

		// Token: 0x04007FC6 RID: 32710
		private Object _externalSessionOwner;

		// Token: 0x04007FC7 RID: 32711
		private CrossfadeLoopPlayer _baseLoop;

		// Token: 0x04007FC8 RID: 32712
		private AudioClip _currentBaseClip;

		// Token: 0x04007FC9 RID: 32713
		private bool _meterSubscribed;

		// Token: 0x02000FF9 RID: 4089
		private enum State
		{
			// Token: 0x04007FCB RID: 32715
			Idle,
			// Token: 0x04007FCC RID: 32716
			FadingIn,
			// Token: 0x04007FCD RID: 32717
			Sustaining,
			// Token: 0x04007FCE RID: 32718
			FadingOut
		}

		// Token: 0x02000FFA RID: 4090
		private enum EndingState
		{
			// Token: 0x04007FD0 RID: 32720
			Idle,
			// Token: 0x04007FD1 RID: 32721
			FadingIn,
			// Token: 0x04007FD2 RID: 32722
			Sustaining,
			// Token: 0x04007FD3 RID: 32723
			FadingOut
		}
	}
}
