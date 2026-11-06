using System;
using System.Collections;
using Features.Loading;
using Michsky.MUIP;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x02001058 RID: 4184
	[DefaultExecutionOrder(11000)]
	public class MainMenuController : MonoBehaviour
	{
		// Token: 0x06007EF1 RID: 32497 RVA: 0x0033EBBA File Offset: 0x0033CDBA
		private void OnEnable()
		{
			this.RestoreMenuCursor();
		}

		// Token: 0x06007EF2 RID: 32498 RVA: 0x0033EBBA File Offset: 0x0033CDBA
		private void LateUpdate()
		{
			this.RestoreMenuCursor();
		}

		// Token: 0x06007EF3 RID: 32499 RVA: 0x0033EBC2 File Offset: 0x0033CDC2
		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				this.RestoreMenuCursor();
			}
		}

		// Token: 0x06007EF4 RID: 32500 RVA: 0x0033EBD0 File Offset: 0x0033CDD0
		private void RestoreMenuCursor()
		{
			if (Application.isPlaying && !Application.isFocused)
			{
				return;
			}
			if (!base.isActiveAndEnabled || this._loadingHome || SceneManager.GetActiveScene() != base.gameObject.scene)
			{
				return;
			}
			Scene sceneByName = SceneManager.GetSceneByName("GameWarmUp");
			if (sceneByName.IsValid() && sceneByName.isLoaded)
			{
				return;
			}
			if (Cursor.lockState != CursorLockMode.None)
			{
				Cursor.lockState = CursorLockMode.None;
			}
			bool flag = !ControllerInput.IsActive;
			if (Cursor.visible != flag)
			{
				Cursor.visible = flag;
			}
		}

		// Token: 0x06007EF5 RID: 32501 RVA: 0x0033EC56 File Offset: 0x0033CE56
		private void Start()
		{
			this.RefreshVersionLabel();
			this.EnsureMenuPresentationHelpers();
			if (this.playIdleEmotes)
			{
				this.StartIdleEmotes();
			}
		}

		// Token: 0x06007EF6 RID: 32502 RVA: 0x0033EC74 File Offset: 0x0033CE74
		private void RefreshVersionLabel()
		{
			foreach (GameObject gameObject in base.gameObject.scene.GetRootGameObjects())
			{
				foreach (Text text in gameObject.GetComponentsInChildren<Text>(true))
				{
					if (text.name == "VERSION")
					{
						text.text = "v" + Application.version;
					}
				}
				foreach (TMP_Text tmp_Text in gameObject.GetComponentsInChildren<TMP_Text>(true))
				{
					if (tmp_Text.name == "VERSION")
					{
						tmp_Text.text = "v" + Application.version;
					}
				}
			}
		}

		// Token: 0x06007EF7 RID: 32503 RVA: 0x0033ED48 File Offset: 0x0033CF48
		private void EnsureMenuPresentationHelpers()
		{
			MainMenuCharacterLightRig mainMenuCharacterLightRig = base.GetComponent<MainMenuCharacterLightRig>();
			if (mainMenuCharacterLightRig == null)
			{
				mainMenuCharacterLightRig = base.gameObject.AddComponent<MainMenuCharacterLightRig>();
			}
			mainMenuCharacterLightRig.Initialize(this.characterAnimator);
			MainMenuWorkshopMenu mainMenuWorkshopMenu = base.GetComponent<MainMenuWorkshopMenu>();
			if (mainMenuWorkshopMenu == null)
			{
				mainMenuWorkshopMenu = base.gameObject.AddComponent<MainMenuWorkshopMenu>();
			}
			mainMenuWorkshopMenu.BuildIfNeeded();
			MainMenuExitMenu mainMenuExitMenu = base.GetComponent<MainMenuExitMenu>();
			if (mainMenuExitMenu == null)
			{
				mainMenuExitMenu = base.gameObject.AddComponent<MainMenuExitMenu>();
			}
			mainMenuExitMenu.BuildIfNeeded();
		}

		// Token: 0x06007EF8 RID: 32504 RVA: 0x0033EDC1 File Offset: 0x0033CFC1
		private void OnDisable()
		{
			this.StopIdleEmotes();
		}

		// Token: 0x06007EF9 RID: 32505 RVA: 0x0033EDCC File Offset: 0x0033CFCC
		public void StartIdleEmotes()
		{
			if (this._idleEmoteCoroutine != null || this._creditsPresentation)
			{
				return;
			}
			if (this.characterAnimator == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this.characterAnimator = gameObject.GetComponentInChildren<Animator>();
				}
			}
			if (this.characterAnimator == null)
			{
				Debug.LogWarning("[MainMenu] Idle emotes disabilitate: nessun Animator del personaggio trovato.");
				return;
			}
			if (this.emoteCount <= 0)
			{
				if (this.emoteDebugLogs)
				{
					Debug.Log("[MainMenu] Idle emotes: emoteCount <= 0, niente da riprodurre.");
				}
				return;
			}
			this._idleEmoteCoroutine = base.StartCoroutine(this.IdleEmoteRoutine());
		}

		// Token: 0x06007EFA RID: 32506 RVA: 0x0033EE5F File Offset: 0x0033D05F
		public void StopIdleEmotes()
		{
			if (this._idleEmoteCoroutine != null)
			{
				base.StopCoroutine(this._idleEmoteCoroutine);
				this._idleEmoteCoroutine = null;
			}
		}

		// Token: 0x06007EFB RID: 32507 RVA: 0x0033EE7C File Offset: 0x0033D07C
		private IEnumerator IdleEmoteRoutine()
		{
			if (this.initialDelay > 0f)
			{
				yield return new WaitForSeconds(this.initialDelay);
			}
			for (;;)
			{
				int num = this.PickNextEmoteIndex();
				this.PlayEmote(num);
				yield return this.WaitForEmoteToFinish();
				float num2 = Random.Range(this.minInterval, Mathf.Max(this.minInterval, this.maxInterval));
				yield return new WaitForSeconds(num2);
			}
			yield break;
		}

		// Token: 0x06007EFC RID: 32508 RVA: 0x0033EE8B File Offset: 0x0033D08B
		private IEnumerator WaitForEmoteToFinish()
		{
			if (this.characterAnimator == null)
			{
				yield break;
			}
			float elapsed = 0f;
			while (this.IsInMenuIdle(0) && elapsed < 1.5f)
			{
				elapsed += Time.deltaTime;
				yield return null;
			}
			if (this.IsInMenuIdle(0))
			{
				if (this.emoteDebugLogs)
				{
					Debug.Log("[MainMenu] Idle emote non partita entro il timeout (stato senza clip?).");
				}
				yield break;
			}
			elapsed = 0f;
			while (!this.IsInMenuIdle(0) && elapsed < 30f)
			{
				elapsed += Time.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06007EFD RID: 32509 RVA: 0x0033EE9C File Offset: 0x0033D09C
		private bool IsInMenuIdle(int layer)
		{
			return this.characterAnimator.GetCurrentAnimatorStateInfo(layer).shortNameHash == MainMenuController.MenuIdleStateHash || (this.characterAnimator.IsInTransition(layer) && this.characterAnimator.GetNextAnimatorStateInfo(layer).shortNameHash == MainMenuController.MenuIdleStateHash);
		}

		// Token: 0x06007EFE RID: 32510 RVA: 0x0033EEF4 File Offset: 0x0033D0F4
		private int PickNextEmoteIndex()
		{
			if (this.emoteCount <= 1)
			{
				this._lastEmoteIndex = 0;
				return 0;
			}
			int num;
			do
			{
				num = Random.Range(0, this.emoteCount);
			}
			while (num == this._lastEmoteIndex);
			this._lastEmoteIndex = num;
			return num;
		}

		// Token: 0x06007EFF RID: 32511 RVA: 0x0033EF34 File Offset: 0x0033D134
		public void PlayEmote(int index)
		{
			if (this.characterAnimator == null || this._creditsPresentation)
			{
				return;
			}
			this.characterAnimator.SetInteger(MainMenuController.MenuEmoteIndexHash, index);
			this.characterAnimator.SetTrigger(MainMenuController.MenuEmoteTriggerHash);
			if (this.emoteDebugLogs)
			{
				Debug.Log(string.Format("[MainMenu] Idle emote -> MenuEmote_{0}", index));
			}
		}

		// Token: 0x06007F00 RID: 32512 RVA: 0x0033EF98 File Offset: 0x0033D198
		public void EnterCreditsPresentation()
		{
			if (this._creditsPresentation)
			{
				return;
			}
			this._creditsPresentation = true;
			this.StopIdleEmotes();
			this.ResolveCharacterAnimator();
			if (this.characterAnimator != null)
			{
				this.characterAnimator.ResetTrigger(MainMenuController.MenuEmoteTriggerHash);
				this.characterAnimator.CrossFade("Base Layer.MenuCreditsWave", 0.18f, 0, 0f);
			}
		}

		// Token: 0x06007F01 RID: 32513 RVA: 0x0033EFFC File Offset: 0x0033D1FC
		public void ExitCreditsPresentation()
		{
			if (!this._creditsPresentation)
			{
				return;
			}
			this._creditsPresentation = false;
			if (this.characterAnimator != null)
			{
				this.characterAnimator.ResetTrigger(MainMenuController.MenuEmoteTriggerHash);
				this.characterAnimator.CrossFade("Base Layer.MenuIdle", 0.18f, 0, 0f);
			}
			if (this.playIdleEmotes && base.isActiveAndEnabled)
			{
				this.StartIdleEmotes();
			}
		}

		// Token: 0x06007F02 RID: 32514 RVA: 0x0033F068 File Offset: 0x0033D268
		private void ResolveCharacterAnimator()
		{
			if (this.characterAnimator != null)
			{
				return;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				this.characterAnimator = gameObject.GetComponentInChildren<Animator>();
			}
		}

		// Token: 0x06007F03 RID: 32515 RVA: 0x0033F0A4 File Offset: 0x0033D2A4
		public void OnNewGameClicked()
		{
			if (this._loadingHome)
			{
				return;
			}
			if (!Application.CanStreamedLevelBeLoaded(this.newGameSceneName))
			{
				Debug.LogError("[MainMenu] Scene '" + this.newGameSceneName + "' is not available in Build Settings.");
				return;
			}
			LoadingScreenController instance = LoadingScreenController.Instance;
			if (instance == null)
			{
				Debug.LogError("[MainMenu] LoadingScreenController could not be created.");
				return;
			}
			this._loadingHome = true;
			this.StopIdleEmotes();
			instance.LoadSceneAsync(this.newGameSceneName);
		}

		// Token: 0x06007F04 RID: 32516 RVA: 0x0033F115 File Offset: 0x0033D315
		public void OnDiscordClicked()
		{
			if (this.socialLinks != null)
			{
				this.socialLinks.OpenDiscord();
				return;
			}
			Debug.LogWarning("[MainMenu] SocialLinks config non assegnato.");
		}

		// Token: 0x06007F05 RID: 32517 RVA: 0x0033F13B File Offset: 0x0033D33B
		public void OnTwitterClicked()
		{
			if (this.socialLinks != null)
			{
				this.socialLinks.OpenX();
				return;
			}
			Debug.LogWarning("[MainMenu] SocialLinks config non assegnato.");
		}

		// Token: 0x06007F06 RID: 32518 RVA: 0x0033F161 File Offset: 0x0033D361
		public void OnShopClicked()
		{
			if (this.socialLinks != null)
			{
				this.socialLinks.OpenItch();
				return;
			}
			Debug.LogWarning("[MainMenu] SocialLinks config non assegnato.");
		}

		// Token: 0x06007F07 RID: 32519 RVA: 0x0033F187 File Offset: 0x0033D387
		public void OnItchClicked()
		{
			this.OnShopClicked();
		}

		// Token: 0x06007F08 RID: 32520 RVA: 0x0033F18F File Offset: 0x0033D38F
		public void OnSteamClicked()
		{
			if (this.socialLinks != null)
			{
				this.socialLinks.OpenSteam();
				return;
			}
			Debug.LogWarning("[MainMenu] SocialLinks config non assegnato.");
		}

		// Token: 0x06007F09 RID: 32521 RVA: 0x0033F1B8 File Offset: 0x0033D3B8
		public void OnWIPClicked()
		{
			if (this.wipTooltipContent != null)
			{
				this.wipTooltipContent.description = this.wipDescription;
				this.wipTooltipContent.delay = 0f;
				this.wipTooltipContent.ProcessEnter();
				if (this._hideTooltipCoroutine != null)
				{
					base.StopCoroutine(this._hideTooltipCoroutine);
				}
				this._hideTooltipCoroutine = base.StartCoroutine(this.HideTooltipRoutine());
				return;
			}
			Debug.LogWarning("WIP Tooltip Content is not assigned!");
		}

		// Token: 0x06007F0A RID: 32522 RVA: 0x0033F230 File Offset: 0x0033D430
		private IEnumerator HideTooltipRoutine()
		{
			yield return new WaitForSeconds(this.tooltipDuration);
			if (this.wipTooltipContent != null)
			{
				this.wipTooltipContent.ProcessExit();
			}
			yield break;
		}

		// Token: 0x0400827D RID: 33405
		[Header("Scene Management")]
		[SerializeField]
		private string newGameSceneName = "Game";

		// Token: 0x0400827E RID: 33406
		[Header("Social Links")]
		[Tooltip("Punto unico di configurazione dei link social (Discord, X, Itch, Steam). Crea l'asset da Create > Main Menu > Social Links Config.")]
		[SerializeField]
		private SocialLinksConfigSO socialLinks;

		// Token: 0x0400827F RID: 33407
		[Header("WIP Feedback")]
		[SerializeField]
		private TooltipManager tooltipManager;

		// Token: 0x04008280 RID: 33408
		[SerializeField]
		private TooltipContent wipTooltipContent;

		// Token: 0x04008281 RID: 33409
		[SerializeField]
		private string wipDescription = "This feature is currently under development.";

		// Token: 0x04008282 RID: 33410
		[SerializeField]
		private float tooltipDuration = 2f;

		// Token: 0x04008283 RID: 33411
		[Header("Character Idle Emotes")]
		[Tooltip("Animator del personaggio in vetrina nel menu. Se null viene cercato a runtime via tag 'Player'.")]
		[SerializeField]
		private Animator characterAnimator;

		// Token: 0x04008284 RID: 33412
		[Tooltip("Quante idle-emote sono configurate nel controller (stati MenuEmote_0 .. MenuEmote_{N-1}). Deve combaciare col numero di stati MenuEmote_i creati nell'Animator.")]
		[SerializeField]
		private int emoteCount = 4;

		// Token: 0x04008285 RID: 33413
		[Tooltip("Se true, le idle-emote casuali partono automaticamente all'avvio del menu.")]
		[SerializeField]
		private bool playIdleEmotes = true;

		// Token: 0x04008286 RID: 33414
		[Tooltip("Intervallo minimo (secondi) tra una idle-emote e la successiva.")]
		[SerializeField]
		private float minInterval = 4f;

		// Token: 0x04008287 RID: 33415
		[Tooltip("Intervallo massimo (secondi) tra una idle-emote e la successiva.")]
		[SerializeField]
		private float maxInterval = 9f;

		// Token: 0x04008288 RID: 33416
		[Tooltip("Attesa iniziale (secondi) prima della prima idle-emote, così il personaggio resta un attimo in MenuIdle.")]
		[SerializeField]
		private float initialDelay = 3f;

		// Token: 0x04008289 RID: 33417
		[SerializeField]
		private bool emoteDebugLogs;

		// Token: 0x0400828A RID: 33418
		private static readonly int MenuEmoteIndexHash = Animator.StringToHash("MenuEmoteIndex");

		// Token: 0x0400828B RID: 33419
		private static readonly int MenuEmoteTriggerHash = Animator.StringToHash("MenuEmoteTrigger");

		// Token: 0x0400828C RID: 33420
		private static readonly int MenuIdleStateHash = Animator.StringToHash("MenuIdle");

		// Token: 0x0400828D RID: 33421
		private const string MenuIdleStatePath = "Base Layer.MenuIdle";

		// Token: 0x0400828E RID: 33422
		private const string MenuCreditsWaveStatePath = "Base Layer.MenuCreditsWave";

		// Token: 0x0400828F RID: 33423
		private int _lastEmoteIndex = -1;

		// Token: 0x04008290 RID: 33424
		private Coroutine _idleEmoteCoroutine;

		// Token: 0x04008291 RID: 33425
		private bool _creditsPresentation;

		// Token: 0x04008292 RID: 33426
		private Coroutine _hideTooltipCoroutine;

		// Token: 0x04008293 RID: 33427
		private bool _loadingHome;

		// Token: 0x04008294 RID: 33428
		private const float EmoteStartTimeout = 1.5f;

		// Token: 0x04008295 RID: 33429
		private const float EmoteMaxDuration = 30f;
	}
}
