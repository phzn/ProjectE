using System;
using System.Collections;
using Features.DebugTools;
using Features.Detection;
using Features.Inventory;
using Features.PlayerEmotes;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FF2 RID: 4082
	[DisallowMultipleComponent]
	public class MasturbationSpeedController : MonoBehaviour
	{
		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x06007BB3 RID: 31667 RVA: 0x00328608 File Offset: 0x00326808
		public bool IsActive
		{
			get
			{
				return this._activeEmote != null;
			}
		}

		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x06007BB4 RID: 31668 RVA: 0x00328616 File Offset: 0x00326816
		public bool ReservesModifiedScroll
		{
			get
			{
				if (!this.IsActive)
				{
					return false;
				}
				if (!ControllerInput.IsActive)
				{
					return GameplayInput.GetKey(this.speedModifierKey);
				}
				return InventoryControllerInput.SpeedModifierHeld(null);
			}
		}

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x06007BB5 RID: 31669 RVA: 0x0032863B File Offset: 0x0032683B
		public float CurrentSpeed
		{
			get
			{
				return this._currentSpeed;
			}
		}

		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x06007BB6 RID: 31670 RVA: 0x00328643 File Offset: 0x00326843
		public EmoteDefinitionSO ActiveEmote
		{
			get
			{
				return this._activeEmote;
			}
		}

		// Token: 0x06007BB7 RID: 31671 RVA: 0x0032864C File Offset: 0x0032684C
		public void ShowExternalSpeed(float speed)
		{
			if (this._activeEmote != null)
			{
				return;
			}
			if (this.speedLabel != null)
			{
				string text = (string.IsNullOrEmpty(this.speedLabelFormat) ? "x{0:F1}" : this.speedLabelFormat);
				this.speedLabel.text = string.Format(text, speed);
			}
			this.ShowSpeedWidgetWithKick();
		}

		// Token: 0x06007BB8 RID: 31672 RVA: 0x003286AE File Offset: 0x003268AE
		public void HideExternalSpeed()
		{
			if (this._activeEmote != null)
			{
				return;
			}
			this.HideSpeedWidgetSmooth();
		}

		// Token: 0x06007BB9 RID: 31673 RVA: 0x003286C5 File Offset: 0x003268C5
		private void Awake()
		{
			if (this.emoteController == null)
			{
				this.emoteController = base.GetComponent<GameplayEmoteController>();
			}
			if (this.playerInput == null)
			{
				this.playerInput = base.GetComponent<vThirdPersonInput>();
			}
			this.InitializeSpeedWidgetUI();
		}

		// Token: 0x06007BBA RID: 31674 RVA: 0x00328701 File Offset: 0x00326901
		public void BindSpeedWidget(RectTransform widget, TMP_Text label)
		{
			this.speedWidgetTransform = widget;
			this.speedLabel = label;
			this.InitializeSpeedWidgetUI();
		}

		// Token: 0x06007BBB RID: 31675 RVA: 0x00328718 File Offset: 0x00326918
		private void InitializeSpeedWidgetUI()
		{
			if (this.speedWidgetTransform == null && this.speedLabel != null)
			{
				this.speedWidgetTransform = this.speedLabel.transform as RectTransform;
			}
			if (this.speedWidgetTransform != null)
			{
				this._speedWidgetCanvasGroup = this.speedWidgetTransform.GetComponent<CanvasGroup>();
				if (this._speedWidgetCanvasGroup == null)
				{
					this._speedWidgetCanvasGroup = this.speedWidgetTransform.gameObject.AddComponent<CanvasGroup>();
				}
				this._speedWidgetCanvasGroup.alpha = 0f;
				this._speedWidgetCanvasGroup.interactable = false;
				this._speedWidgetCanvasGroup.blocksRaycasts = false;
				this._speedWidgetVisibleAnchoredX = this.speedWidgetTransform.anchoredPosition.x;
				Vector2 anchoredPosition = this.speedWidgetTransform.anchoredPosition;
				anchoredPosition.x = this._speedWidgetVisibleAnchoredX + this.speedWidgetSlideOffset;
				this.speedWidgetTransform.anchoredPosition = anchoredPosition;
			}
			if (this.speedLabel != null)
			{
				this._speedLabelBaseScale = this.speedLabel.transform.localScale;
			}
			this._speedWidgetVisible = false;
		}

		// Token: 0x06007BBC RID: 31676 RVA: 0x00328834 File Offset: 0x00326A34
		private void OnEnable()
		{
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted += this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded += this.HandleEmoteEnded;
			}
			this.TryHookMeter();
		}

		// Token: 0x06007BBD RID: 31677 RVA: 0x00328884 File Offset: 0x00326A84
		private void OnDisable()
		{
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			if (this.orgasmMeter != null)
			{
				this.orgasmMeter.OnMaxReached -= this.HandleOrgasmMaxReached;
			}
			if (this._autoEndCoroutine != null)
			{
				base.StopCoroutine(this._autoEndCoroutine);
				this._autoEndCoroutine = null;
			}
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = false;
			}
		}

		// Token: 0x06007BBE RID: 31678 RVA: 0x00328928 File Offset: 0x00326B28
		private void TryHookMeter()
		{
			if (this.orgasmMeter == null)
			{
				this.orgasmMeter = OrgasmMeterController.Instance;
			}
			if (this.orgasmMeter != null)
			{
				this.orgasmMeter.OnMaxReached -= this.HandleOrgasmMaxReached;
				this.orgasmMeter.OnMaxReached += this.HandleOrgasmMaxReached;
			}
		}

		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x06007BBF RID: 31679 RVA: 0x0032898A File Offset: 0x00326B8A
		// (set) Token: 0x06007BC0 RID: 31680 RVA: 0x00328992 File Offset: 0x00326B92
		public bool ExternalDriverActive { get; set; }

		// Token: 0x06007BC1 RID: 31681 RVA: 0x0032899C File Offset: 0x00326B9C
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (emote == null || !emote.IsMasturbationEmote)
			{
				return;
			}
			ItemUsageSignal.Set(this, true);
			if (this.ExternalDriverActive)
			{
				return;
			}
			this._activeEmote = emote;
			this._maxReachedThisEmote = false;
			this._targetSpeed = Mathf.Clamp(emote.DefaultClipSpeed, emote.MinClipSpeed, emote.MaxClipSpeed);
			this._currentSpeed = this._targetSpeed;
			this.ApplyAnimatorSpeed(this._currentSpeed);
			this.UpdateSpeedLabel();
			this._lastReportedTargetSpeed = this._targetSpeed;
			this.TryHookMeter();
			if (this.debugMode)
			{
				Debug.Log(string.Format("[MasturbationSpeed] Emote started: {0} (speed={1:F2})", emote.DisplayName, this._currentSpeed));
			}
		}

		// Token: 0x06007BC2 RID: 31682 RVA: 0x00328A50 File Offset: 0x00326C50
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			if (emote == null || !emote.IsMasturbationEmote)
			{
				return;
			}
			ItemUsageSignal.Set(this, false);
			if (this._activeEmote != null && this._activeEmote != emote)
			{
				return;
			}
			if (this.debugMode)
			{
				Debug.Log("[MasturbationSpeed] Emote ended: " + emote.DisplayName);
			}
			if (this._autoEndCoroutine != null)
			{
				base.StopCoroutine(this._autoEndCoroutine);
				this._autoEndCoroutine = null;
			}
			this.ApplyAnimatorSpeed(1f);
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = false;
			}
			this._activeEmote = null;
			this._targetSpeed = 1f;
			this._currentSpeed = 1f;
			this.HideSpeedWidgetSmooth();
			if (this.orgasmMeter != null)
			{
				if (this._maxReachedThisEmote)
				{
					this.orgasmMeter.UnlockAndCooldown();
				}
				else
				{
					this.orgasmMeter.ForceCooldown();
				}
			}
			this._maxReachedThisEmote = false;
		}

		// Token: 0x06007BC3 RID: 31683 RVA: 0x00328B48 File Offset: 0x00326D48
		private void HandleOrgasmMaxReached()
		{
			if (this._activeEmote == null)
			{
				return;
			}
			if (this._maxReachedThisEmote)
			{
				return;
			}
			this._maxReachedThisEmote = true;
			Animator animator = this.ResolveAnimator();
			if (animator == null)
			{
				if (this.debugMode)
				{
					Debug.LogWarning("[MasturbationSpeed] Cannot fire OrgasmEnding: animator not found.");
				}
				return;
			}
			if (!string.IsNullOrEmpty(this.orgasmEndingTrigger))
			{
				animator.ResetTrigger(this.orgasmEndingTrigger);
				animator.SetTrigger(this.orgasmEndingTrigger);
				if (this.debugMode)
				{
					Debug.Log("[MasturbationSpeed] OrgasmMax → trigger '" + this.orgasmEndingTrigger + "'");
				}
			}
			if (this._autoEndCoroutine != null)
			{
				base.StopCoroutine(this._autoEndCoroutine);
			}
			this._autoEndCoroutine = base.StartCoroutine(this.AutoEndAfterEndingRoutine());
		}

		// Token: 0x06007BC4 RID: 31684 RVA: 0x00328C04 File Offset: 0x00326E04
		private IEnumerator AutoEndAfterEndingRoutine()
		{
			vThirdPersonController cc = ((this.playerInput != null) ? this.playerInput.cc : null);
			yield return null;
			bool sawEndStateActive = false;
			bool timedOut = false;
			float enterDeadline = Time.unscaledTime + this.autoEndEnterTimeoutSeconds;
			while (Time.unscaledTime < enterDeadline)
			{
				if (cc != null && cc.customAction)
				{
					sawEndStateActive = true;
					break;
				}
				yield return null;
			}
			if (!sawEndStateActive)
			{
				timedOut = true;
				if (this.debugMode)
				{
					Debug.LogWarning(string.Format("[MasturbationSpeed] Phase 1 timeout after {0:0.0}s — animator never entered the ending state. Check that the ending state has the vAnimatorTag 'CustomAction'.", this.autoEndEnterTimeoutSeconds));
				}
			}
			else
			{
				float playDeadline = Time.unscaledTime + this.autoEndPlayTimeoutSeconds;
				while (Time.unscaledTime < playDeadline && !(cc == null) && cc.customAction)
				{
					yield return null;
				}
				if (cc != null && cc.customAction)
				{
					timedOut = true;
					if (this.debugMode)
					{
						Debug.LogWarning(string.Format("[MasturbationSpeed] Phase 2 timeout after {0:0.0}s — ending state never finished. Either the clip is longer than the timeout or there is no Exit route.", this.autoEndPlayTimeoutSeconds));
					}
				}
			}
			if (this.debugMode && !timedOut)
			{
				Debug.Log("[MasturbationSpeed] Ending state finished, calling EndEmote");
			}
			this._autoEndCoroutine = null;
			if (this.emoteController != null && this.emoteController.IsPlaying)
			{
				this.emoteController.EndEmote();
			}
			yield break;
		}

		// Token: 0x06007BC5 RID: 31685 RVA: 0x00328C14 File Offset: 0x00326E14
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (this._activeEmote == null)
			{
				return;
			}
			bool isActive = ControllerInput.IsActive;
			bool flag = (isActive ? InventoryControllerInput.SpeedModifierHeld(null) : GameplayInput.GetKey(this.speedModifierKey));
			int num = (isActive ? InventoryControllerInput.SpeedStep(flag, ref this._controllerSpeedDirection, ref this._controllerSpeedNextRepeat) : 0);
			if (this.playerInput != null)
			{
				this.playerInput.suppressCameraZoom = flag;
			}
			if (flag)
			{
				float num2 = (isActive ? ((float)num) : GameplayInput.GetAxisRaw("Mouse ScrollWheel"));
				if (!Mathf.Approximately(num2, 0f))
				{
					int num3 = ((num2 > 0f) ? 1 : (-1));
					float targetSpeed = this._targetSpeed;
					this._targetSpeed = Mathf.Clamp(this._targetSpeed + (float)num3 * this.speedStepPerScrollNotch, this._activeEmote.MinClipSpeed, this._activeEmote.MaxClipSpeed);
					if (!Mathf.Approximately(targetSpeed, this._targetSpeed))
					{
						this.ShowSpeedWidgetWithKick();
					}
					if (this.debugMode)
					{
						Debug.Log(string.Format("[MasturbationSpeed] Scroll → target={0:F2}", this._targetSpeed));
					}
				}
			}
			if (this._speedWidgetVisible && Time.unscaledTime - this._lastSpeedChangeTime > this.speedWidgetAutoHideAfter)
			{
				this.HideSpeedWidgetSmooth();
			}
			if (this.speedSmoothing >= 1f)
			{
				this._currentSpeed = this._targetSpeed;
			}
			else
			{
				float num4 = 1f - Mathf.Pow(1f - this.speedSmoothing, Time.deltaTime * 60f);
				this._currentSpeed = Mathf.Lerp(this._currentSpeed, this._targetSpeed, num4);
			}
			this.ApplyAnimatorSpeed(this._currentSpeed);
			this.UpdateSpeedLabel();
			if (this.orgasmMeter != null && !this._maxReachedThisEmote)
			{
				float num5 = this._activeEmote.OrgasmProgressPerSecondAtBaseSpeed * this._currentSpeed * Time.deltaTime;
				if (num5 > 0f)
				{
					this.orgasmMeter.AddProgress(num5);
				}
			}
		}

		// Token: 0x06007BC6 RID: 31686 RVA: 0x00328DFC File Offset: 0x00326FFC
		private void ApplyAnimatorSpeed(float value)
		{
			if (string.IsNullOrEmpty(this.speedMultiplierParameter))
			{
				return;
			}
			Animator animator = this.ResolveAnimator();
			if (animator == null)
			{
				return;
			}
			animator.SetFloat(this.speedMultiplierParameter, value);
		}

		// Token: 0x06007BC7 RID: 31687 RVA: 0x00328E38 File Offset: 0x00327038
		private void UpdateSpeedLabel()
		{
			if (this.speedLabel == null)
			{
				return;
			}
			string text = (string.IsNullOrEmpty(this.speedLabelFormat) ? "x{0:F1}" : this.speedLabelFormat);
			this.speedLabel.text = string.Format(text, this._currentSpeed);
		}

		// Token: 0x06007BC8 RID: 31688 RVA: 0x00328E8C File Offset: 0x0032708C
		private void ShowSpeedWidgetWithKick()
		{
			this._lastSpeedChangeTime = Time.unscaledTime;
			if (this.speedWidgetTransform == null)
			{
				Debug.LogWarning("[MasturbationSpeed] ShowSpeedWidgetWithKick called but speedWidgetTransform is null — widget binding probably failed (MasturbationSpeedWidgetLink missing in HUD prefab, or no MasturbationSpeedController in scene).");
				return;
			}
			if (!this._speedWidgetVisible)
			{
				this._speedWidgetVisible = true;
				if (this._speedWidgetCoroutine != null)
				{
					base.StopCoroutine(this._speedWidgetCoroutine);
				}
				this._speedWidgetCoroutine = base.StartCoroutine(this.SlideWidgetRoutine(true));
			}
			if (this.speedLabel != null && this.speedLabelPunchScale > 1.001f)
			{
				if (this._speedLabelPunchCoroutine != null)
				{
					base.StopCoroutine(this._speedLabelPunchCoroutine);
				}
				this._speedLabelPunchCoroutine = base.StartCoroutine(this.PunchLabelRoutine());
			}
		}

		// Token: 0x06007BC9 RID: 31689 RVA: 0x00328F34 File Offset: 0x00327134
		private void HideSpeedWidgetSmooth()
		{
			if (!this._speedWidgetVisible)
			{
				return;
			}
			this._speedWidgetVisible = false;
			if (this.speedWidgetTransform == null)
			{
				return;
			}
			if (this._speedWidgetCoroutine != null)
			{
				base.StopCoroutine(this._speedWidgetCoroutine);
			}
			this._speedWidgetCoroutine = base.StartCoroutine(this.SlideWidgetRoutine(false));
		}

		// Token: 0x06007BCA RID: 31690 RVA: 0x00328F87 File Offset: 0x00327187
		private IEnumerator SlideWidgetRoutine(bool visible)
		{
			if (this.speedWidgetTransform == null)
			{
				yield break;
			}
			float fromX = this.speedWidgetTransform.anchoredPosition.x;
			float toX = (visible ? this._speedWidgetVisibleAnchoredX : (this._speedWidgetVisibleAnchoredX + this.speedWidgetSlideOffset));
			float fromAlpha = ((this._speedWidgetCanvasGroup != null) ? this._speedWidgetCanvasGroup.alpha : (visible ? 0f : 1f));
			float toAlpha = (visible ? 1f : 0f);
			float dur = Mathf.Max(0.01f, this.speedWidgetSlideDuration);
			float t = 0f;
			while (t < dur)
			{
				t += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / dur));
				Vector2 anchoredPosition = this.speedWidgetTransform.anchoredPosition;
				anchoredPosition.x = Mathf.Lerp(fromX, toX, num);
				this.speedWidgetTransform.anchoredPosition = anchoredPosition;
				if (this._speedWidgetCanvasGroup != null)
				{
					this._speedWidgetCanvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, num);
				}
				yield return null;
			}
			Vector2 anchoredPosition2 = this.speedWidgetTransform.anchoredPosition;
			anchoredPosition2.x = toX;
			this.speedWidgetTransform.anchoredPosition = anchoredPosition2;
			if (this._speedWidgetCanvasGroup != null)
			{
				this._speedWidgetCanvasGroup.alpha = toAlpha;
			}
			this._speedWidgetCoroutine = null;
			yield break;
		}

		// Token: 0x06007BCB RID: 31691 RVA: 0x00328F9D File Offset: 0x0032719D
		private IEnumerator PunchLabelRoutine()
		{
			if (this.speedLabel == null)
			{
				yield break;
			}
			Transform labelTr = this.speedLabel.transform;
			float dur = Mathf.Max(0.05f, this.speedLabelPunchDuration);
			float t = 0f;
			while (t < dur)
			{
				t += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(t / dur);
				float num2 = ((num < 0.4f) ? (num / 0.4f) : (1f - (num - 0.4f) / 0.6f));
				float num3 = 1f + (this.speedLabelPunchScale - 1f) * num2;
				labelTr.localScale = this._speedLabelBaseScale * num3;
				yield return null;
			}
			labelTr.localScale = this._speedLabelBaseScale;
			this._speedLabelPunchCoroutine = null;
			yield break;
		}

		// Token: 0x06007BCC RID: 31692 RVA: 0x00328FAC File Offset: 0x003271AC
		private Animator ResolveAnimator()
		{
			if (this.playerInput != null && this.playerInput.cc != null)
			{
				return this.playerInput.cc.animator;
			}
			return base.GetComponentInChildren<Animator>();
		}

		// Token: 0x04007F45 RID: 32581
		[Header("Refs (auto-found)")]
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x04007F46 RID: 32582
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x04007F47 RID: 32583
		[SerializeField]
		private OrgasmMeterController orgasmMeter;

		// Token: 0x04007F48 RID: 32584
		[Header("Animator Parameters")]
		[Tooltip("Animator parameter (float) collegato come Speed Multiplier sui masturbation state nel CielAnimatorController. Lo script lo aggiorna ogni frame mentre un'emote di masturbazione e' attiva.")]
		[SerializeField]
		private string speedMultiplierParameter = "EmoteSpeedMultiplier";

		// Token: 0x04007F49 RID: 32585
		[Tooltip("Animator trigger settato quando OrgasmMeter raggiunge il max. Configurare una transizione loop-state -> ending-state che lo consuma (pattern HandstandWalk).")]
		[SerializeField]
		private string orgasmEndingTrigger = "OrgasmEnding";

		// Token: 0x04007F4A RID: 32586
		[Header("Speed Input")]
		[Tooltip("Modificatore tenuto premuto per intercettare lo ScrollWheel (es. LeftControl). Senza il modificatore, lo scroll torna a Invector come zoom camera.")]
		[SerializeField]
		private KeyCode speedModifierKey = KeyCode.LeftControl;

		// Token: 0x04007F4B RID: 32587
		[Tooltip("Quanta speed (moltiplicatore) viene aggiunta/tolta per ogni notch della scroll wheel.")]
		[Min(0.01f)]
		[SerializeField]
		private float speedStepPerScrollNotch = 0.15f;

		// Token: 0x04007F4C RID: 32588
		[Tooltip("Smoothing della transizione tra speed corrente e target (1 = istantaneo). Valori piccoli (0.1) rendono la rampa di accelerazione/decelerazione piu' organica.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float speedSmoothing = 0.2f;

		// Token: 0x04007F4D RID: 32589
		[Header("Speed Visual Feedback")]
		[Tooltip("TMP_Text opzionale che mostra la velocita corrente (es. 'x1.0', 'x1.5').")]
		[SerializeField]
		private TMP_Text speedLabel;

		// Token: 0x04007F4E RID: 32590
		[Tooltip("Formato C# numerico per la speed (es. 'x{0:F1}' → 'x1.5', 'x{0:0}x' → '2x').")]
		[SerializeField]
		private string speedLabelFormat = "x{0:F1}";

		// Token: 0x04007F4F RID: 32591
		[Tooltip("RectTransform del widget speed che viene animato (slide-in/out + scale punch). Tipicamente il container del speedLabel (es. un GameObject 'SpeedWidget' a destra della OrgasmBar). Se null, ricade su speedLabel.transform.")]
		[SerializeField]
		private RectTransform speedWidgetTransform;

		// Token: 0x04007F50 RID: 32592
		[Tooltip("Offset (in pixel UI) dal quale il widget parte durante lo slide-in. Negativo = entra da sinistra. Esempio: -40 = parte 40px a sinistra della posizione visibile (la posizione 'visibile' viene letta dal RectTransform al bind, cosi' puoi posizionare il widget liberamente nell'editor del prefab).")]
		[SerializeField]
		private float speedWidgetSlideOffset = -40f;

		// Token: 0x04007F51 RID: 32593
		[Tooltip("Durata dello slide-in (visibile) e dello slide-out (nascosto). Secondi.")]
		[Min(0.05f)]
		[SerializeField]
		private float speedWidgetSlideDuration = 0.18f;

		// Token: 0x04007F52 RID: 32594
		[Tooltip("Secondi di inattivita (nessun cambio di velocita) prima che il widget si nasconda automaticamente.")]
		[Min(0.5f)]
		[SerializeField]
		private float speedWidgetAutoHideAfter = 2f;

		// Token: 0x04007F53 RID: 32595
		[Tooltip("Scale di picco del punch animato sul label al cambio di velocita (1 = niente punch, 1.3 = 30% piu' grande al picco).")]
		[Range(1f, 2f)]
		[SerializeField]
		private float speedLabelPunchScale = 1.35f;

		// Token: 0x04007F54 RID: 32596
		[Tooltip("Durata del punch scale (rapido, secondi).")]
		[Min(0.05f)]
		[SerializeField]
		private float speedLabelPunchDuration = 0.18f;

		// Token: 0x04007F55 RID: 32597
		[Header("Auto-End on Orgasm")]
		[Tooltip("Timeout massimo per Phase 1 (animator entra nell'ending state). Tipico < 1s perche' la transizione dal loop e' rapida. Se scade, l'auto-end forza EndEmote come safety net.")]
		[Min(0.5f)]
		[SerializeField]
		private float autoEndEnterTimeoutSeconds = 3f;

		// Token: 0x04007F56 RID: 32598
		[Tooltip("Timeout massimo per Phase 2 (durata dell'ending clip + transizione fuori). Deve essere >= della durata della clip ending piu' margine. Tipicamente 10-30s per clip espressive.")]
		[Min(1f)]
		[SerializeField]
		private float autoEndPlayTimeoutSeconds = 30f;

		// Token: 0x04007F57 RID: 32599
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007F58 RID: 32600
		private EmoteDefinitionSO _activeEmote;

		// Token: 0x04007F59 RID: 32601
		private float _targetSpeed = 1f;

		// Token: 0x04007F5A RID: 32602
		private float _currentSpeed = 1f;

		// Token: 0x04007F5B RID: 32603
		private int _controllerSpeedDirection;

		// Token: 0x04007F5C RID: 32604
		private float _controllerSpeedNextRepeat;

		// Token: 0x04007F5D RID: 32605
		private bool _maxReachedThisEmote;

		// Token: 0x04007F5E RID: 32606
		private Coroutine _autoEndCoroutine;

		// Token: 0x04007F5F RID: 32607
		private Coroutine _speedWidgetCoroutine;

		// Token: 0x04007F60 RID: 32608
		private Coroutine _speedLabelPunchCoroutine;

		// Token: 0x04007F61 RID: 32609
		private bool _speedWidgetVisible;

		// Token: 0x04007F62 RID: 32610
		private float _lastSpeedChangeTime = -999f;

		// Token: 0x04007F63 RID: 32611
		private float _lastReportedTargetSpeed = -1f;

		// Token: 0x04007F64 RID: 32612
		private CanvasGroup _speedWidgetCanvasGroup;

		// Token: 0x04007F65 RID: 32613
		private Vector3 _speedLabelBaseScale = Vector3.one;

		// Token: 0x04007F66 RID: 32614
		private float _speedWidgetVisibleAnchoredX;
	}
}
