using System;
using System.Collections;
using Features.Detection;
using UnityEngine;
using UnityEngine.Events;

namespace Features.CielVoice
{
	// Token: 0x02001353 RID: 4947
	[DisallowMultipleComponent]
	public class CielDetectionVoiceListener : MonoBehaviour
	{
		// Token: 0x06009924 RID: 39204 RVA: 0x003ECD04 File Offset: 0x003EAF04
		private void OnEnable()
		{
			this._hookupRoutine = base.StartCoroutine(this.HookupWhenReady());
		}

		// Token: 0x06009925 RID: 39205 RVA: 0x003ECD18 File Offset: 0x003EAF18
		private void OnDisable()
		{
			if (this._hookupRoutine != null)
			{
				base.StopCoroutine(this._hookupRoutine);
				this._hookupRoutine = null;
			}
			this.CancelEncounterPresentation();
			this.Unsubscribe();
		}

		// Token: 0x06009926 RID: 39206 RVA: 0x003ECD41 File Offset: 0x003EAF41
		private IEnumerator HookupWhenReady()
		{
			float elapsed = 0f;
			while (ExhibitionEventDirector.Instance == null)
			{
				if (this.directorHookupTimeout > 0f && elapsed >= this.directorHookupTimeout)
				{
					if (this.debugLog)
					{
						Debug.LogWarning("[CielDetectionVoiceListener] Director non trovato entro il timeout — nessun aggancio.");
					}
					yield break;
				}
				elapsed += Time.unscaledDeltaTime;
				yield return null;
			}
			this.Subscribe(ExhibitionEventDirector.Instance);
			this._hookupRoutine = null;
			yield break;
		}

		// Token: 0x06009927 RID: 39207 RVA: 0x003ECD50 File Offset: 0x003EAF50
		private void Subscribe(ExhibitionEventDirector director)
		{
			if (director == null || this._director == director)
			{
				return;
			}
			this.Unsubscribe();
			this._director = director;
			this._director.OnPlayerReactionStarted += this.HandlePlayerReactionStarted;
			this._director.OnPlayerCategoryChosen += this.HandlePlayerCategoryChosen;
			UnityEvent onEventSkipped = this._director.OnEventSkipped;
			if (onEventSkipped != null)
			{
				onEventSkipped.AddListener(new UnityAction(this.CancelEncounterPresentation));
			}
			if (this.debugLog)
			{
				Debug.Log("[CielDetectionVoiceListener] Agganciato a OnPlayerReactionStarted + OnPlayerCategoryChosen.");
			}
		}

		// Token: 0x06009928 RID: 39208 RVA: 0x003ECDE4 File Offset: 0x003EAFE4
		private void Unsubscribe()
		{
			if (this._director != null)
			{
				this._director.OnPlayerReactionStarted -= this.HandlePlayerReactionStarted;
				this._director.OnPlayerCategoryChosen -= this.HandlePlayerCategoryChosen;
				UnityEvent onEventSkipped = this._director.OnEventSkipped;
				if (onEventSkipped != null)
				{
					onEventSkipped.RemoveListener(new UnityAction(this.CancelEncounterPresentation));
				}
				this._director = null;
			}
		}

		// Token: 0x06009929 RID: 39209 RVA: 0x003ECE58 File Offset: 0x003EB058
		private void HandlePlayerReactionStarted(DetectionActorType actorType)
		{
			if (this.voiceSet == null)
			{
				return;
			}
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance == null)
			{
				if (this.debugLog)
				{
					Debug.LogWarning("[CielDetectionVoiceListener] CielVoiceController.Instance null — reaction non gestita.");
				}
				return;
			}
			CielReactionMood cielReactionMood = this.ResolveMood();
			CielVoiceSO cielVoiceSO = this.voiceSet.ResolveVoice(actorType, cielReactionMood);
			if (cielVoiceSO == null)
			{
				if (this.debugLog)
				{
					Debug.Log(string.Format("[CielDetectionVoiceListener] Nessuna voce per actor={0}, mood={1} — fallback al Director.", actorType, cielReactionMood));
				}
				return;
			}
			this._director.MarkReactionHandled();
			this.PlayOwnedVoice(instance, cielVoiceSO);
			string subtitle = cielVoiceSO.Subtitle;
			if (!string.IsNullOrWhiteSpace(subtitle) && SimpleDialogueManager.Instance != null)
			{
				this._reactionGatePassed = false;
				this._director.RegisterReactionHold(() => this._reactionGatePassed);
				DialogueLine dialogueLine = new DialogueLine
				{
					speakerName = (this.cielSpeakerName ?? ""),
					text = subtitle,
					localizationScope = "voice_subtitle"
				};
				this.StartOwnedVoiceDialogue(dialogueLine, delegate
				{
					this._reactionGatePassed = true;
				});
			}
			if (this.debugLog)
			{
				Debug.Log(string.Format("[CielDetectionVoiceListener] Reaction: actor={0}, mood={1}, voice='{2}', hasText={3}.", new object[]
				{
					actorType,
					cielReactionMood,
					cielVoiceSO.ResolvedId,
					!string.IsNullOrWhiteSpace(subtitle)
				}));
			}
		}

		// Token: 0x0600992A RID: 39210 RVA: 0x003ECFB0 File Offset: 0x003EB1B0
		private void HandlePlayerCategoryChosen(ChoiceCategory category, DetectionActorType actorType)
		{
			if (this._director != null && this._director.IsGropingChoiceSelected)
			{
				if (this.debugLog)
				{
					Debug.Log("[CielDetectionVoiceListener] Wanna touch: voce generica Expose saltata.");
				}
				return;
			}
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance == null)
			{
				if (this.debugLog)
				{
					Debug.LogWarning("[CielDetectionVoiceListener] CielVoiceController.Instance null — voce scelta non gestita.");
				}
				return;
			}
			CielVoiceSO cielVoiceSO;
			switch (category)
			{
			case ChoiceCategory.Shame:
				cielVoiceSO = this.shameChoiceVoice;
				break;
			case ChoiceCategory.Exhibit:
				cielVoiceSO = this.exposeChoiceVoice;
				break;
			case ChoiceCategory.Police:
				cielVoiceSO = this.shameChoiceVoice;
				break;
			default:
				cielVoiceSO = this.shameChoiceVoice;
				break;
			}
			CielVoiceSO cielVoiceSO2 = cielVoiceSO;
			if (cielVoiceSO2 == null)
			{
				if (this.debugLog)
				{
					Debug.Log(string.Format("[CielDetectionVoiceListener] Nessuna voce scelta configurata per category={0}.", category));
				}
				return;
			}
			this.PlayOwnedVoice(instance, cielVoiceSO2);
			string subtitle = cielVoiceSO2.Subtitle;
			if (!string.IsNullOrWhiteSpace(subtitle) && SimpleDialogueManager.Instance != null)
			{
				DialogueLine dialogueLine = new DialogueLine
				{
					speakerName = (this.cielSpeakerName ?? ""),
					text = subtitle,
					localizationScope = "voice_subtitle"
				};
				this.StartOwnedVoiceDialogue(dialogueLine, null);
			}
			if (this.debugLog)
			{
				Debug.Log(string.Format("[CielDetectionVoiceListener] Voce scelta: category={0}, actor={1}, voice='{2}'.", category, actorType, cielVoiceSO2.ResolvedId));
			}
		}

		// Token: 0x0600992B RID: 39211 RVA: 0x003ED0F0 File Offset: 0x003EB2F0
		private void StartOwnedVoiceDialogue(DialogueLine line, Action onComplete)
		{
			this._voiceDialogueManager = SimpleDialogueManager.Instance;
			object token = new object();
			this._voiceDialogueToken = token;
			this._voiceDialogueManager.StartOwnedDialogue(token, new DialogueLine[] { line }, delegate
			{
				if (this._voiceDialogueToken != token)
				{
					return;
				}
				this._voiceDialogueToken = null;
				this._voiceDialogueManager = null;
				Action onComplete2 = onComplete;
				if (onComplete2 == null)
				{
					return;
				}
				onComplete2();
			});
		}

		// Token: 0x0600992C RID: 39212 RVA: 0x003ED15A File Offset: 0x003EB35A
		private void PlayOwnedVoice(CielVoiceController controller, CielVoiceSO voice)
		{
			if (!controller.Play(voice, false))
			{
				return;
			}
			this.ReleaseVoiceOwnership();
			this._ownedVoiceController = controller;
			this._ownedVoice = voice;
			controller.OnVoiceEnded += this.HandleOwnedVoiceEnded;
		}

		// Token: 0x0600992D RID: 39213 RVA: 0x003ED18D File Offset: 0x003EB38D
		private void HandleOwnedVoiceEnded(CielVoiceSO voice)
		{
			if (voice == this._ownedVoice)
			{
				this.ReleaseVoiceOwnership();
			}
		}

		// Token: 0x0600992E RID: 39214 RVA: 0x003ED1A3 File Offset: 0x003EB3A3
		private void ReleaseVoiceOwnership()
		{
			if (this._ownedVoiceController != null)
			{
				this._ownedVoiceController.OnVoiceEnded -= this.HandleOwnedVoiceEnded;
			}
			this._ownedVoiceController = null;
			this._ownedVoice = null;
		}

		// Token: 0x0600992F RID: 39215 RVA: 0x003ED1D8 File Offset: 0x003EB3D8
		private void CancelEncounterPresentation()
		{
			this._reactionGatePassed = true;
			if (this._voiceDialogueManager != null)
			{
				this._voiceDialogueManager.TryEndDialogue(this._voiceDialogueToken, false);
			}
			this._voiceDialogueManager = null;
			this._voiceDialogueToken = null;
			CielVoiceController ownedVoiceController = this._ownedVoiceController;
			CielVoiceSO ownedVoice = this._ownedVoice;
			this.ReleaseVoiceOwnership();
			if (ownedVoiceController != null && ownedVoice != null && ownedVoiceController.CurrentVoice == ownedVoice)
			{
				ownedVoiceController.Stop();
			}
		}

		// Token: 0x06009930 RID: 39216 RVA: 0x003ED258 File Offset: 0x003EB458
		private CielReactionMood ResolveMood()
		{
			if (PlayerExposureState.IsFlashActive)
			{
				return CielReactionMood.Exhibition;
			}
			float num = 0f;
			float num2 = 0f;
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance != null)
			{
				num = instance.ShameLevel;
				num2 = instance.ExhibitionLevel;
			}
			return this.voiceSet.ResolveMood(num, num2);
		}

		// Token: 0x04009E38 RID: 40504
		[Header("Data")]
		[Tooltip("Set di reazioni vocali per-attore (Police/Reporter/Referee/Generic) × stato emotivo. Se null o senza voce per l'attore/stato corrente, il listener NON gestisce la reaction e il Director usa il suo fallback interno.")]
		[SerializeField]
		private CielDetectionVoiceSet voiceSet;

		// Token: 0x04009E39 RID: 40505
		[Tooltip("Nome mostrato come speaker nel pannello dialogo classico per le battute di Ciel durante la detection. Vuoto = nessun nome.")]
		[SerializeField]
		private string cielSpeakerName = "Ciel";

		// Token: 0x04009E3A RID: 40506
		[Header("Voce della SCELTA (Shame / Expose)")]
		[Tooltip("Battuta quando il player sceglie SHAME durante la detection (es. EncounterSouns2). Il sottotitolo usa il pannello CielVoice con auto-fade finale. Usata ANCHE per la Polizia, dove la scelta e' forzata sulle azioni speciali (categoria Police).")]
		[SerializeField]
		private CielVoiceSO shameChoiceVoice;

		// Token: 0x04009E3B RID: 40507
		[Tooltip("Battuta quando il player sceglie EXPOSE durante la detection (es. HornyCaught). Il sottotitolo usa il pannello CielVoice con auto-fade finale.")]
		[SerializeField]
		private CielVoiceSO exposeChoiceVoice;

		// Token: 0x04009E3C RID: 40508
		[Header("Hookup")]
		[Tooltip("Secondi di retry per agganciare l'ExhibitionEventDirector.Instance se non e' ancora pronto all'OnEnable (il Director vive in scena, questo componente nel prefab persistente).")]
		[SerializeField]
		private float directorHookupTimeout = 10f;

		// Token: 0x04009E3D RID: 40509
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04009E3E RID: 40510
		private ExhibitionEventDirector _director;

		// Token: 0x04009E3F RID: 40511
		private Coroutine _hookupRoutine;

		// Token: 0x04009E40 RID: 40512
		private SimpleDialogueManager _voiceDialogueManager;

		// Token: 0x04009E41 RID: 40513
		private object _voiceDialogueToken;

		// Token: 0x04009E42 RID: 40514
		private CielVoiceController _ownedVoiceController;

		// Token: 0x04009E43 RID: 40515
		private CielVoiceSO _ownedVoice;

		// Token: 0x04009E44 RID: 40516
		private bool _reactionGatePassed = true;
	}
}
