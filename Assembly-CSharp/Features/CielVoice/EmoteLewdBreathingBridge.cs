using System;
using Features.Detection;
using Features.PlayerEmotes;
using UnityEngine;

namespace Features.CielVoice
{
	// Token: 0x0200136B RID: 4971
	[DisallowMultipleComponent]
	public class EmoteLewdBreathingBridge : MonoBehaviour
	{
		// Token: 0x060099F2 RID: 39410 RVA: 0x003F0B28 File Offset: 0x003EED28
		private void Awake()
		{
			if (this.emoteController == null)
			{
				this.emoteController = base.GetComponentInParent<GameplayEmoteController>();
			}
			if (this.emoteController == null)
			{
				this.emoteController = base.GetComponentInChildren<GameplayEmoteController>();
			}
			if (this.emoteController == null && base.transform.root != null)
			{
				this.emoteController = base.transform.root.GetComponentInChildren<GameplayEmoteController>(true);
			}
			if (this.breathing == null)
			{
				this.breathing = base.GetComponentInParent<CielAmbientBreathing>();
			}
			if (this.breathing == null)
			{
				this.breathing = base.GetComponentInChildren<CielAmbientBreathing>();
			}
			if (this.breathing == null && base.transform.root != null)
			{
				this.breathing = base.transform.root.GetComponentInChildren<CielAmbientBreathing>(true);
			}
		}

		// Token: 0x060099F3 RID: 39411 RVA: 0x003F0C10 File Offset: 0x003EEE10
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
				Debug.LogWarning("[EmoteLewdBreathingBridge] Nessun GameplayEmoteController trovato: il ponte non fa nulla.");
			}
		}

		// Token: 0x060099F4 RID: 39412 RVA: 0x003F0C6C File Offset: 0x003EEE6C
		private void OnDisable()
		{
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			CielAmbientBreathing cielAmbientBreathing = this.breathing;
			if (cielAmbientBreathing == null)
			{
				return;
			}
			cielAmbientBreathing.SetEmoteLewdBreathing(false);
		}

		// Token: 0x060099F5 RID: 39413 RVA: 0x003F0CC8 File Offset: 0x003EEEC8
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (emote == null || !emote.UseLewdBreathing)
			{
				return;
			}
			if (this.debugLog)
			{
				Debug.Log("[EmoteLewdBreathingBridge] Emote '" + emote.EmoteId + "' → lewd breathing ON");
			}
			CielAmbientBreathing cielAmbientBreathing = this.breathing;
			if (cielAmbientBreathing == null)
			{
				return;
			}
			cielAmbientBreathing.SetEmoteLewdBreathing(true);
		}

		// Token: 0x060099F6 RID: 39414 RVA: 0x003F0D1C File Offset: 0x003EEF1C
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			if (this.debugLog)
			{
				Debug.Log("[EmoteLewdBreathingBridge] Emote '" + ((emote != null) ? emote.EmoteId : "<null>") + "' ended → lewd breathing OFF");
			}
			CielAmbientBreathing cielAmbientBreathing = this.breathing;
			if (cielAmbientBreathing == null)
			{
				return;
			}
			cielAmbientBreathing.SetEmoteLewdBreathing(false);
		}

		// Token: 0x04009EF3 RID: 40691
		[Tooltip("GameplayEmoteController da osservare. Auto-risolto sul player (parent/children) se null.")]
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x04009EF4 RID: 40692
		[Tooltip("CielAmbientBreathing da pilotare. Auto-risolto sul player (parent/children) se null.")]
		[SerializeField]
		private CielAmbientBreathing breathing;

		// Token: 0x04009EF5 RID: 40693
		[SerializeField]
		private bool debugLog;
	}
}
