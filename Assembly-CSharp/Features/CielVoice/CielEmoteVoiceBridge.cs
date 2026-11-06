using System;
using System.Collections.Generic;
using Features.Detection;
using Features.PlayerEmotes;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.CielVoice
{
	// Token: 0x02001359 RID: 4953
	[DisallowMultipleComponent]
	public class CielEmoteVoiceBridge : MonoBehaviour
	{
		// Token: 0x06009943 RID: 39235 RVA: 0x003ED544 File Offset: 0x003EB744
		private void Awake()
		{
			if (this.emoteController == null)
			{
				vThirdPersonInput vThirdPersonInput = Object.FindObjectOfType<vThirdPersonInput>();
				if (vThirdPersonInput != null)
				{
					this.emoteController = vThirdPersonInput.GetComponent<GameplayEmoteController>();
				}
			}
			this.RebuildMap();
		}

		// Token: 0x06009944 RID: 39236 RVA: 0x003ED580 File Offset: 0x003EB780
		private void RebuildMap()
		{
			this._map.Clear();
			if (this.entries == null)
			{
				return;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				CielEmoteVoiceBridge.EmoteVoiceEntry emoteVoiceEntry = this.entries[i];
				if (emoteVoiceEntry != null && !string.IsNullOrWhiteSpace(emoteVoiceEntry.EmoteId) && !(emoteVoiceEntry.Voice == null))
				{
					this._map[emoteVoiceEntry.EmoteId.Trim()] = emoteVoiceEntry;
				}
			}
		}

		// Token: 0x06009945 RID: 39237 RVA: 0x003ED5FC File Offset: 0x003EB7FC
		private void OnEnable()
		{
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted += this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded += this.HandleEmoteEnded;
				return;
			}
			if (this.debugLog)
			{
				Debug.LogWarning("[CielEmoteVoiceBridge] emoteController non trovato: nessuna voce sugli emote.");
			}
		}

		// Token: 0x06009946 RID: 39238 RVA: 0x003ED658 File Offset: 0x003EB858
		private void OnDisable()
		{
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			this._activeEntry = null;
		}

		// Token: 0x06009947 RID: 39239 RVA: 0x003ED6A8 File Offset: 0x003EB8A8
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (emote == null)
			{
				return;
			}
			string emoteId = emote.EmoteId;
			if (string.IsNullOrWhiteSpace(emoteId))
			{
				return;
			}
			CielEmoteVoiceBridge.EmoteVoiceEntry emoteVoiceEntry;
			if (!this._map.TryGetValue(emoteId.Trim(), out emoteVoiceEntry) || emoteVoiceEntry.Voice == null)
			{
				return;
			}
			CielVoiceController cielVoiceController = ((this.voiceController != null) ? this.voiceController : CielVoiceController.Instance);
			if (cielVoiceController == null)
			{
				if (this.debugLog)
				{
					Debug.LogWarning("[CielEmoteVoiceBridge] Nessun CielVoiceController disponibile.");
				}
				return;
			}
			if (cielVoiceController.Play(emoteVoiceEntry.Voice))
			{
				this._activeEntry = emoteVoiceEntry;
				if (this.debugLog)
				{
					Debug.Log(string.Concat(new string[]
					{
						"[CielEmoteVoiceBridge] Emote '",
						emoteId,
						"' -> voce '",
						emoteVoiceEntry.Voice.ResolvedId,
						"'."
					}));
				}
			}
		}

		// Token: 0x06009948 RID: 39240 RVA: 0x003ED784 File Offset: 0x003EB984
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			if (this._activeEntry == null)
			{
				return;
			}
			if (this._activeEntry.StopOnEmoteEnd)
			{
				CielVoiceController cielVoiceController = ((this.voiceController != null) ? this.voiceController : CielVoiceController.Instance);
				if (cielVoiceController != null && cielVoiceController.CurrentVoice == this._activeEntry.Voice)
				{
					cielVoiceController.Stop();
				}
			}
			this._activeEntry = null;
		}

		// Token: 0x04009E56 RID: 40534
		[Header("References")]
		[Tooltip("GameplayEmoteController da osservare. Auto-risolto sul player (vThirdPersonInput) se null.")]
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x04009E57 RID: 40535
		[Tooltip("CielVoiceController target. Se null usa CielVoiceController.Instance.")]
		[SerializeField]
		private CielVoiceController voiceController;

		// Token: 0x04009E58 RID: 40536
		[Header("Mapping")]
		[Tooltip("Associazioni EmoteId -> battuta vocale. Gli emote non presenti in lista non hanno voce.")]
		[SerializeField]
		private List<CielEmoteVoiceBridge.EmoteVoiceEntry> entries = new List<CielEmoteVoiceBridge.EmoteVoiceEntry>();

		// Token: 0x04009E59 RID: 40537
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04009E5A RID: 40538
		private readonly Dictionary<string, CielEmoteVoiceBridge.EmoteVoiceEntry> _map = new Dictionary<string, CielEmoteVoiceBridge.EmoteVoiceEntry>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04009E5B RID: 40539
		private CielEmoteVoiceBridge.EmoteVoiceEntry _activeEntry;

		// Token: 0x0200135A RID: 4954
		[Serializable]
		public class EmoteVoiceEntry
		{
			// Token: 0x04009E5C RID: 40540
			[Tooltip("EmoteId (campo Identity di EmoteDefinitionSO) a cui associare la voce. Case-insensitive. Puoi mappare piu' emote alla stessa battuta con piu' entry.")]
			public string EmoteId;

			// Token: 0x04009E5D RID: 40541
			[Tooltip("Battuta vocale da suonare quando quell'emote parte.")]
			public CielVoiceSO Voice;

			// Token: 0x04009E5E RID: 40542
			[Tooltip("Se true, la voce viene fermata quando l'emote termina. Utile per versi in loop o lunghi che non devono sopravvivere alla fine dell'emote (es. il verso della pee). Se false, la clip finisce naturalmente anche dopo la fine dell'emote.")]
			public bool StopOnEmoteEnd = true;
		}
	}
}
