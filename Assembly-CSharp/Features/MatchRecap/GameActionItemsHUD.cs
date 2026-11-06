using System;
using System.Collections;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Detection;
using Features.GameOver;
using Features.GameOver.Newspaper;
using Features.Localization;
using Features.Police;
using Features.Score;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.MatchRecap
{
	// Token: 0x02000FC2 RID: 4034
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-710)]
	public class GameActionItemsHUD : MonoBehaviour
	{
		// Token: 0x060079C4 RID: 31172 RVA: 0x0031BF42 File Offset: 0x0031A142
		private void OnDestroy()
		{
			if (this._controllerLabels != null)
			{
				Object.Destroy(this._controllerLabels);
			}
		}

		// Token: 0x060079C5 RID: 31173 RVA: 0x0031BF5D File Offset: 0x0031A15D
		private void Awake()
		{
			if (this.rootGroup == null)
			{
				this.rootGroup = base.GetComponentInParent<CanvasGroup>();
			}
			if (this.autoShowOnGameOver)
			{
				this.SetRootAlpha(0f, false);
				this._inputActive = false;
				return;
			}
			this._inputActive = true;
		}

		// Token: 0x060079C6 RID: 31174 RVA: 0x0031BF9C File Offset: 0x0031A19C
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.StabilizeLayout;
			this.TryRegisterGameOverListener();
			if (this._showRequested && this._showRoutine == null && base.isActiveAndEnabled)
			{
				this._showRoutine = base.StartCoroutine(this.ShowAfterDelay());
			}
		}

		// Token: 0x060079C7 RID: 31175 RVA: 0x0031BFEC File Offset: 0x0031A1EC
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.StabilizeLayout;
			if (this._showRoutine != null)
			{
				base.StopCoroutine(this._showRoutine);
				this._showRoutine = null;
			}
			if (this.autoShowOnGameOver)
			{
				this._inputActive = false;
			}
			if (this._layoutRoutine != null)
			{
				base.StopCoroutine(this._layoutRoutine);
				this._layoutRoutine = null;
			}
			if (this._gameOverListenerRegistered && GameOverManager.Instance != null)
			{
				GameOverManager.Instance.OnGameOver.RemoveListener(new UnityAction<GameOverReason>(this.OnGameOverTriggered));
			}
			this._gameOverListenerRegistered = false;
		}

		// Token: 0x060079C8 RID: 31176 RVA: 0x0031C086 File Offset: 0x0031A286
		private void Start()
		{
			this.TryRegisterGameOverListener();
			if (this.autoBindItemsFromChildren)
			{
				this.AutoBindItems();
			}
			this.ApplyLabels();
			this.EnsurePhotoGalleryHint();
			this.EnsurePhotoModeHint();
			this.ApplyVisibility();
		}

		// Token: 0x060079C9 RID: 31177 RVA: 0x0031C0B4 File Offset: 0x0031A2B4
		private void TryRegisterGameOverListener()
		{
			if (this._gameOverListenerRegistered)
			{
				return;
			}
			if (!this.autoShowOnGameOver)
			{
				return;
			}
			if (GameOverManager.Instance == null)
			{
				return;
			}
			GameOverManager.Instance.OnGameOver.AddListener(new UnityAction<GameOverReason>(this.OnGameOverTriggered));
			this._gameOverListenerRegistered = true;
			if (GameOverManager.Instance.IsGameOver)
			{
				this.OnGameOverTriggered(GameOverManager.Instance.CurrentReason);
			}
		}

		// Token: 0x060079CA RID: 31178 RVA: 0x0031C120 File Offset: 0x0031A320
		private void OnGameOverTriggered(GameOverReason reason)
		{
			GameOverManager instance = GameOverManager.Instance;
			if (instance != null && this._observedGameOver == instance && this._observedGameOverSequence == instance.GameOverSequenceId)
			{
				return;
			}
			this._observedGameOver = instance;
			this._observedGameOverSequence = ((instance != null) ? instance.GameOverSequenceId : (-1));
			this._hiddenForThisGameOver.Clear();
			if (reason == GameOverReason.ReturnedHome)
			{
				this.Hide();
				return;
			}
			if (reason == GameOverReason.CensorLeak || reason == GameOverReason.RankDefeat || reason == GameOverReason.LongWayHomeSpotted)
			{
				this._hiddenForThisGameOver.Add(GameActionItemsHUD.BuiltInAction.OpenNewspaper);
			}
			if (this._showRoutine != null)
			{
				base.StopCoroutine(this._showRoutine);
			}
			this._showRequested = true;
			this._showAtRealtime = ((instance != null) ? instance.StartedAtRealtime : Time.realtimeSinceStartup) + this.showDelayAfterGameOver;
			if (base.isActiveAndEnabled)
			{
				this._showRoutine = base.StartCoroutine(this.ShowAfterDelay());
			}
		}

		// Token: 0x060079CB RID: 31179 RVA: 0x0031C1FF File Offset: 0x0031A3FF
		private IEnumerator ShowAfterDelay()
		{
			float num = this._showAtRealtime - Time.realtimeSinceStartup;
			if (num > 0f)
			{
				yield return new WaitForSecondsRealtime(num);
			}
			if (this.rootGroup == null)
			{
				this._showRoutine = null;
				yield break;
			}
			this.rootGroup.interactable = true;
			this.rootGroup.blocksRaycasts = false;
			float t = 0f;
			float start = this.rootGroup.alpha;
			while (t < this.fadeInDuration)
			{
				t += Time.unscaledDeltaTime;
				this.rootGroup.alpha = Mathf.Lerp(start, 1f, (this.fadeInDuration > 0f) ? (t / this.fadeInDuration) : 1f);
				yield return null;
			}
			this.rootGroup.alpha = 1f;
			this.ApplyVisibility();
			this._inputActive = true;
			this._showRoutine = null;
			yield break;
		}

		// Token: 0x060079CC RID: 31180 RVA: 0x0031C210 File Offset: 0x0031A410
		public void Hide()
		{
			this._showRequested = false;
			if (this._showRoutine != null)
			{
				base.StopCoroutine(this._showRoutine);
				this._showRoutine = null;
			}
			if (NpcPhotoGalleryController.Instance != null)
			{
				NpcPhotoGalleryController.Instance.Hide();
			}
			this._inputActive = false;
			this.SetRootAlpha(0f, false);
		}

		// Token: 0x060079CD RID: 31181 RVA: 0x0031C269 File Offset: 0x0031A469
		private void SetRootAlpha(float alpha, bool interactable)
		{
			if (this.rootGroup == null)
			{
				return;
			}
			this.rootGroup.alpha = alpha;
			this.rootGroup.interactable = interactable;
			this.rootGroup.blocksRaycasts = interactable;
		}

		// Token: 0x060079CE RID: 31182 RVA: 0x0031C2A0 File Offset: 0x0031A4A0
		private void AutoBindItems()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				GameActionItemsHUD.ActionEntry actionEntry = this.items[i];
				if (actionEntry != null && !(actionEntry.itemUI != null))
				{
					if (i >= base.transform.childCount)
					{
						break;
					}
					Transform child = base.transform.GetChild(i);
					ActionItemUI component = child.GetComponent<ActionItemUI>();
					if (component != null)
					{
						actionEntry.itemUI = component;
						if (this.debugLogs)
						{
							Debug.Log(string.Format("[GameActionItemsHUD] Auto-bound item {0} '{1}' to '{2}'", i, actionEntry.actionLabel, child.name));
						}
					}
				}
			}
		}

		// Token: 0x060079CF RID: 31183 RVA: 0x0031C340 File Offset: 0x0031A540
		private void Update()
		{
			bool flag = MatchRecapController.Instance != null && MatchRecapController.Instance.IsShowing;
			int count = NpcPhotoSessionArchive.Count;
			bool flag2 = GameActionItemsHUD.CanShowPhotoModeHint();
			if (flag != this._lastRecapVisible || count != this._lastPhotoCount || flag2 != this._lastPhotoModeAvailable || GameActionItemsHUD.InspectionOverlayOpen != this._lastOverlayVisible)
			{
				this._lastRecapVisible = flag;
				this._lastPhotoCount = count;
				this.ApplyVisibility();
			}
			if (!this._inputActive && !this.debugAllowInputBeforeGameOver)
			{
				return;
			}
			if (FlyCamera.IsActive || FlyCameraPhotoMode.AnyOpen)
			{
				return;
			}
			if (!flag && NpcPhotoSessionArchive.Count > 0 && GameActionItemsHUD.ContextKeyDown(KeyCode.P))
			{
				this.ExecuteBuiltIn(GameActionItemsHUD.BuiltInAction.OpenPhotoGallery);
				return;
			}
			int i = 0;
			while (i < this.items.Count)
			{
				GameActionItemsHUD.ActionEntry actionEntry = this.items[i];
				if (actionEntry != null && actionEntry.trigger != KeyCode.None && this.IsEntryVisible(actionEntry, flag) && (!(actionEntry.itemUI != null) || actionEntry.itemUI.gameObject.activeInHierarchy) && GameActionItemsHUD.ContextActionDown(actionEntry))
				{
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[GameActionItemsHUD] {0} pressed → {1}", actionEntry.trigger, actionEntry.actionLabel));
					}
					if (((actionEntry.OnPressed != null) ? actionEntry.OnPressed.GetPersistentEventCount() : 0) > 0)
					{
						actionEntry.OnPressed.Invoke();
						return;
					}
					this.ExecuteBuiltIn(actionEntry.builtIn);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060079D0 RID: 31184 RVA: 0x0031C4C2 File Offset: 0x0031A6C2
		private static bool ContextKeyDown(KeyCode key)
		{
			if (GameActionItemsHUD.ContextBlocked())
			{
				return false;
			}
			if (Input.GetKeyDown(key))
			{
				return true;
			}
			if (!ControllerInput.IsActive || !ControllerInput.GetKeyDown(key))
			{
				return false;
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x060079D1 RID: 31185 RVA: 0x0031C4EE File Offset: 0x0031A6EE
		private static bool ContextBlocked()
		{
			return ControllerWorldPointer.BlocksContextInput || GameActionItemsHUD.InspectionOverlayOpen;
		}

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x060079D2 RID: 31186 RVA: 0x0031C4FE File Offset: 0x0031A6FE
		private static bool InspectionOverlayOpen
		{
			get
			{
				return (NewspaperController.Instance != null && NewspaperController.Instance.IsShowing) || (NpcPhotoGalleryController.Instance != null && NpcPhotoGalleryController.Instance.IsShowing);
			}
		}

		// Token: 0x060079D3 RID: 31187 RVA: 0x0031C534 File Offset: 0x0031A734
		private static bool ContextActionDown(GameActionItemsHUD.ActionEntry entry)
		{
			if (entry.builtIn != GameActionItemsHUD.BuiltInAction.SwitchCamera)
			{
				return GameActionItemsHUD.ContextKeyDown(entry.trigger);
			}
			if (GameActionItemsHUD.ContextBlocked())
			{
				return false;
			}
			if (Input.GetKeyDown(entry.trigger))
			{
				return true;
			}
			if (!ControllerInput.IsActive || !ControllerInput.GetKeyDown(KeyCode.V))
			{
				return false;
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x060079D4 RID: 31188 RVA: 0x0031C588 File Offset: 0x0031A788
		private bool IsEntryVisible(GameActionItemsHUD.ActionEntry entry, bool recapVisible)
		{
			bool flag = entry.builtIn == GameActionItemsHUD.BuiltInAction.HideMatchRecap;
			return !GameActionItemsHUD.InspectionOverlayOpen && (recapVisible ? flag : (!flag)) && !this._hiddenForThisGameOver.Contains(entry.builtIn);
		}

		// Token: 0x060079D5 RID: 31189 RVA: 0x0031C5C8 File Offset: 0x0031A7C8
		private void ExecuteBuiltIn(GameActionItemsHUD.BuiltInAction action)
		{
			switch (action)
			{
			case GameActionItemsHUD.BuiltInAction.ToggleMatchRecap:
				if (MatchRecapController.Instance != null)
				{
					MatchRecapController.Instance.Toggle();
					return;
				}
				break;
			case GameActionItemsHUD.BuiltInAction.OpenNewspaper:
				if (NewspaperController.Instance != null)
				{
					NewspaperController.Instance.Toggle();
					return;
				}
				if (this.debugLogs)
				{
					Debug.Log("[GameActionItemsHUD] OpenNewspaper: no NewspaperController in scene");
					return;
				}
				break;
			case GameActionItemsHUD.BuiltInAction.SwitchCamera:
			{
				DetectionCameraController detectionCameraController = ((PerpWalkController.Instance != null) ? PerpWalkController.Instance.ActiveCameraController : null);
				if (detectionCameraController != null && detectionCameraController.IsActive)
				{
					detectionCameraController.CycleToNextPreset();
					return;
				}
				break;
			}
			case GameActionItemsHUD.BuiltInAction.HideMatchRecap:
				if (MatchRecapController.Instance != null)
				{
					MatchRecapController.Instance.Hide();
					return;
				}
				break;
			case GameActionItemsHUD.BuiltInAction.ShowMatchRecap:
				if (MatchRecapController.Instance != null)
				{
					MatchRecapController.Instance.Show();
					return;
				}
				break;
			case GameActionItemsHUD.BuiltInAction.OpenPhotoGallery:
			{
				NpcPhotoGalleryController npcPhotoGalleryController = this.EnsurePhotoGalleryController();
				if (npcPhotoGalleryController != null)
				{
					npcPhotoGalleryController.Toggle();
					return;
				}
				if (this.debugLogs)
				{
					Debug.Log("[GameActionItemsHUD] OpenPhotoGallery: controller non disponibile");
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060079D6 RID: 31190 RVA: 0x0031C6D0 File Offset: 0x0031A8D0
		private void EnsurePhotoGalleryHint()
		{
			Transform transform = base.transform.Find("ActionItem_NpcPhotos");
			if (transform != null)
			{
				this._photoGalleryItemUI = transform.GetComponent<ActionItemUI>();
			}
			else if (base.transform.childCount > 0)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(base.transform.GetChild(0).gameObject, base.transform);
				gameObject.name = "ActionItem_NpcPhotos";
				gameObject.transform.SetSiblingIndex(Mathf.Min(2, base.transform.childCount - 1));
				this._photoGalleryItemUI = gameObject.GetComponent<ActionItemUI>();
			}
			if (this._photoGalleryItemUI != null)
			{
				this._photoGalleryItemUI.Setup("P", "NPC PHOTOS", null);
			}
			this.EnsurePhotoGalleryController();
		}

		// Token: 0x060079D7 RID: 31191 RVA: 0x0031C790 File Offset: 0x0031A990
		private NpcPhotoGalleryController EnsurePhotoGalleryController()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			Sprite sprite = null;
			if (MatchRecapController.Instance != null)
			{
				Image[] componentsInChildren = MatchRecapController.Instance.GetComponentsInChildren<Image>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i] != null && componentsInChildren[i].gameObject.name == "Banner")
					{
						sprite = componentsInChildren[i].sprite;
						break;
					}
				}
			}
			return NpcPhotoGalleryController.Ensure(componentInParent, null, sprite);
		}

		// Token: 0x060079D8 RID: 31192 RVA: 0x0031C804 File Offset: 0x0031AA04
		private void EnsurePhotoModeHint()
		{
			Transform transform = base.transform.Find("ActionItem_PhotoMode");
			if (transform != null)
			{
				this._photoModeItemUI = transform.GetComponent<ActionItemUI>();
			}
			else if (base.transform.childCount > 0)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(base.transform.GetChild(0).gameObject, base.transform);
				gameObject.name = "ActionItem_PhotoMode";
				gameObject.transform.SetSiblingIndex(Mathf.Min(2, base.transform.childCount - 1));
				this._photoModeItemUI = gameObject.GetComponent<ActionItemUI>();
			}
			if (this._photoModeItemUI != null)
			{
				this._photoModeItemUI.Setup("K", "PHOTO MODE", null);
			}
		}

		// Token: 0x060079D9 RID: 31193 RVA: 0x0031C8C0 File Offset: 0x0031AAC0
		private static bool CanShowPhotoModeHint()
		{
			PerpWalkController instance = PerpWalkController.Instance;
			return instance != null && instance.IsActive && FlyCamera.CanEnterPhotoMode;
		}

		// Token: 0x060079DA RID: 31194 RVA: 0x0031C8EC File Offset: 0x0031AAEC
		private void ApplyLabels()
		{
			if (this._controllerLabels == null)
			{
				this._controllerLabels = ScriptableObject.CreateInstance<InteractionConfigSO>();
			}
			foreach (GameActionItemsHUD.ActionEntry actionEntry in this.items)
			{
				if (!(((actionEntry != null) ? actionEntry.itemUI : null) == null))
				{
					if (actionEntry.builtIn == GameActionItemsHUD.BuiltInAction.SwitchCamera)
					{
						ControllerContextBindings.SetLabel(this._controllerLabels, actionEntry.keyLabel, "R3", false);
					}
					actionEntry.itemUI.Setup(actionEntry.keyLabel, actionEntry.actionLabel, this._controllerLabels);
				}
			}
		}

		// Token: 0x060079DB RID: 31195 RVA: 0x0031C9A4 File Offset: 0x0031ABA4
		private void ApplyVisibility()
		{
			bool flag = MatchRecapController.Instance != null && MatchRecapController.Instance.IsShowing;
			this._lastRecapVisible = flag;
			this._lastOverlayVisible = GameActionItemsHUD.InspectionOverlayOpen;
			this._lastPhotoCount = NpcPhotoSessionArchive.Count;
			this._lastPhotoModeAvailable = GameActionItemsHUD.CanShowPhotoModeHint();
			foreach (GameActionItemsHUD.ActionEntry actionEntry in this.items)
			{
				if (!(((actionEntry != null) ? actionEntry.itemUI : null) == null))
				{
					actionEntry.itemUI.gameObject.SetActive(this.IsEntryVisible(actionEntry, flag));
				}
			}
			if (this._photoGalleryItemUI != null)
			{
				this._photoGalleryItemUI.gameObject.SetActive(!this._lastOverlayVisible && !flag && NpcPhotoSessionArchive.Count > 0);
			}
			if (this._photoModeItemUI != null)
			{
				this._photoModeItemUI.gameObject.SetActive(!this._lastOverlayVisible && this._lastPhotoModeAvailable);
			}
			this.StabilizeLayout();
		}

		// Token: 0x060079DC RID: 31196 RVA: 0x0031CAC8 File Offset: 0x0031ACC8
		private void StabilizeLayout()
		{
			this.RebuildLayoutNow();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this._layoutRoutine != null)
			{
				base.StopCoroutine(this._layoutRoutine);
			}
			this._layoutRoutine = base.StartCoroutine(this.StabilizeLayoutOverFrames());
		}

		// Token: 0x060079DD RID: 31197 RVA: 0x0031CAFF File Offset: 0x0031ACFF
		private IEnumerator StabilizeLayoutOverFrames()
		{
			yield return null;
			this.RebuildLayoutNow();
			yield return new WaitForEndOfFrame();
			this.RebuildLayoutNow();
			this._layoutRoutine = null;
			yield break;
		}

		// Token: 0x060079DE RID: 31198 RVA: 0x0031CB10 File Offset: 0x0031AD10
		private void RebuildLayoutNow()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null || !rectTransform.gameObject.activeInHierarchy)
			{
				return;
			}
			Canvas.ForceUpdateCanvases();
			RectTransform[] componentsInChildren = rectTransform.GetComponentsInChildren<RectTransform>(true);
			for (int i = componentsInChildren.Length - 1; i >= 0; i--)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(componentsInChildren[i]);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			Canvas.ForceUpdateCanvases();
		}

		// Token: 0x060079DF RID: 31199 RVA: 0x0031CB6A File Offset: 0x0031AD6A
		public void RefreshVisibility()
		{
			this.ApplyVisibility();
		}

		// Token: 0x04007D09 RID: 32009
		[Header("Items")]
		[Tooltip("Lista degli action item esposti. Setup labels e bindings in inspector. L'ordine in scena (dall'alto verso il basso) corrisponde a questa lista.")]
		[SerializeField]
		private List<GameActionItemsHUD.ActionEntry> items = new List<GameActionItemsHUD.ActionEntry>();

		// Token: 0x04007D0A RID: 32010
		[Tooltip("Quando true, se un ActionEntry.itemUI e' null, lo script prova a binarlo automaticamente al child con lo stesso index (transform.GetChild(i)) cercando un ActionItemUI. Comodo per setup rapido senza drag&drop manuale.")]
		[SerializeField]
		private bool autoBindItemsFromChildren = true;

		// Token: 0x04007D0B RID: 32011
		[Header("Visibility")]
		[Tooltip("Quando true, l'HUD parte nascosto e si auto-attiva ascoltando GameOverManager.OnGameOver. Mettere false se l'HUD deve essere visibile durante TUTTO il gameplay (non solo post-cattura).")]
		[SerializeField]
		private bool autoShowOnGameOver = true;

		// Token: 0x04007D0C RID: 32012
		[Tooltip("Secondi di attesa dopo OnGameOver prima di mostrare l'HUD. Allinea con il MatchRecap (default 3s) per apparire insieme al pannello recap.")]
		[Min(0f)]
		[SerializeField]
		private float showDelayAfterGameOver = 3f;

		// Token: 0x04007D0D RID: 32013
		[Tooltip("CanvasGroup root da fade-in/fade-out. Se null, viene preso da GetComponentInParent.")]
		[SerializeField]
		private CanvasGroup rootGroup;

		// Token: 0x04007D0E RID: 32014
		[Tooltip("Durata del fade-in dell'HUD quando appare al game-over (s).")]
		[Min(0f)]
		[SerializeField]
		private float fadeInDuration = 0.35f;

		// Token: 0x04007D0F RID: 32015
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04007D10 RID: 32016
		[Tooltip("SOLO TEST. Quando true, gli hotkey (es. TAB → Match Recap) vengono ascoltati anche durante il gameplay, prima di qualsiasi game-over. Lasciare false in produzione: l'input parte solo quando l'HUD appare post-game-over.")]
		[SerializeField]
		private bool debugAllowInputBeforeGameOver;

		// Token: 0x04007D11 RID: 32017
		private bool _lastRecapVisible;

		// Token: 0x04007D12 RID: 32018
		private bool _lastOverlayVisible;

		// Token: 0x04007D13 RID: 32019
		private bool _lastPhotoModeAvailable;

		// Token: 0x04007D14 RID: 32020
		private int _lastPhotoCount = -1;

		// Token: 0x04007D15 RID: 32021
		private Coroutine _showRoutine;

		// Token: 0x04007D16 RID: 32022
		private bool _showRequested;

		// Token: 0x04007D17 RID: 32023
		private float _showAtRealtime;

		// Token: 0x04007D18 RID: 32024
		private GameOverManager _observedGameOver;

		// Token: 0x04007D19 RID: 32025
		private int _observedGameOverSequence = -1;

		// Token: 0x04007D1A RID: 32026
		private Coroutine _layoutRoutine;

		// Token: 0x04007D1B RID: 32027
		private ActionItemUI _photoGalleryItemUI;

		// Token: 0x04007D1C RID: 32028
		private ActionItemUI _photoModeItemUI;

		// Token: 0x04007D1D RID: 32029
		private bool _inputActive;

		// Token: 0x04007D1E RID: 32030
		private InteractionConfigSO _controllerLabels;

		// Token: 0x04007D1F RID: 32031
		private bool _gameOverListenerRegistered;

		// Token: 0x04007D20 RID: 32032
		private readonly HashSet<GameActionItemsHUD.BuiltInAction> _hiddenForThisGameOver = new HashSet<GameActionItemsHUD.BuiltInAction>();

		// Token: 0x02000FC3 RID: 4035
		public enum BuiltInAction
		{
			// Token: 0x04007D22 RID: 32034
			None,
			// Token: 0x04007D23 RID: 32035
			ToggleMatchRecap,
			// Token: 0x04007D24 RID: 32036
			OpenNewspaper,
			// Token: 0x04007D25 RID: 32037
			SwitchCamera,
			// Token: 0x04007D26 RID: 32038
			HideMatchRecap,
			// Token: 0x04007D27 RID: 32039
			ShowMatchRecap,
			// Token: 0x04007D28 RID: 32040
			OpenPhotoGallery
		}

		// Token: 0x02000FC4 RID: 4036
		[Serializable]
		public class ActionEntry
		{
			// Token: 0x04007D29 RID: 32041
			[Tooltip("Label key visualizzata (es. 'TAB', 'C', 'N'). Usata da ActionItemUI.Setup(key, ...).")]
			public string keyLabel;

			// Token: 0x04007D2A RID: 32042
			[Tooltip("Label azione visualizzata (es. 'MATCH RECAP'). Usata da ActionItemUI.Setup(..., action).")]
			public string actionLabel;

			// Token: 0x04007D2B RID: 32043
			[Tooltip("KeyCode su cui ascolta. Premuto = firma OnPressed (o l'azione builtIn se OnPressed e' vuoto).")]
			public KeyCode trigger;

			// Token: 0x04007D2C RID: 32044
			[Tooltip("Azione built-in di default. Eseguita SOLO se OnPressed non ha listener wirati in inspector. Cosi' puoi avere il comportamento di default senza configurare UnityEvent.")]
			public GameActionItemsHUD.BuiltInAction builtIn;

			// Token: 0x04007D2D RID: 32045
			[Tooltip("Evento Unity firma al keyDown. Wirare qui MatchRecapController.Toggle() ecc. Se vuoto, viene eseguita l'azione builtIn (se impostata).")]
			public UnityEvent OnPressed;

			// Token: 0x04007D2E RID: 32046
			[Tooltip("Reference all'ActionItemUI istanziato in scena per questa entry. Lasciare null se l'HUD spawna runtime via prefab.")]
			public ActionItemUI itemUI;

			// Token: 0x04007D2F RID: 32047
			[Tooltip("Se true, durante il game-over (MatchRecap aperto) questo item viene nascosto. Lascia false per gli item che devono restare sempre visibili.")]
			public bool hideDuringRecap = true;

			// Token: 0x04007D30 RID: 32048
			[Tooltip("Se true, l'item appare SOLO mentre il MatchRecap e' aperto. Tipico del chip HIDE: c'e' solo quando il pannello recap e' visibile. Mutua esclusione con hideDuringRecap.")]
			public bool showOnlyDuringRecap;
		}
	}
}
