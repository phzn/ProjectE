using System;
using System.Collections;
using BoZo.CharacterCreator.MC;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AF0 RID: 2800
	public class CustomizationHubController : MonoBehaviour
	{
		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06004F21 RID: 20257 RVA: 0x001EAAD3 File Offset: 0x001E8CD3
		public CustomizationTargetType SelectedTabTarget
		{
			get
			{
				return this._selectedTabTarget;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06004F22 RID: 20258 RVA: 0x001EAADB File Offset: 0x001E8CDB
		public CustomizationHubPresetListController PresetListController
		{
			get
			{
				return this.presetListController;
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06004F23 RID: 20259 RVA: 0x001EAAE3 File Offset: 0x001E8CE3
		public bool HasMainCharacterTabRoot
		{
			get
			{
				return this.mainCharacterTabRoot != null;
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06004F24 RID: 20260 RVA: 0x001EAAF1 File Offset: 0x001E8CF1
		public bool HasNpcTabRoot
		{
			get
			{
				return this.npcTabRoot != null;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06004F25 RID: 20261 RVA: 0x001EAAFF File Offset: 0x001E8CFF
		public bool HasHubRoot
		{
			get
			{
				return this.hubRoot != null;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06004F26 RID: 20262 RVA: 0x001EAB0D File Offset: 0x001E8D0D
		public bool HasEditorViewRouter
		{
			get
			{
				return this.editorViewRouter != null;
			}
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x001EAB1C File Offset: 0x001E8D1C
		private void Start()
		{
			this._started = true;
			if (this.hubClothingToggleButton != null)
			{
				this.hubClothingToggleButton.onClick.RemoveAllListeners();
				this.hubClothingToggleButton.onClick.AddListener(new UnityAction(this.ToggleHubClothing));
				if (this.mcCustomizationController == null)
				{
					this.mcCustomizationController = Object.FindObjectOfType<MCCustomizationController>(true);
				}
				if (this.mcCustomizationController != null)
				{
					this.mcCustomizationController.BindClothingToggleVisual(this.hubClothingToggleButton);
				}
			}
			if (this.sceneBootstrapper == null)
			{
				return;
			}
			CustomizationSessionContext initialContext = this.sceneBootstrapper.InitialContext;
			bool flag = initialContext != null && initialContext.openInHub;
			if (initialContext != null && !string.IsNullOrWhiteSpace(initialContext.sourceScene))
			{
				this._sourceScene = initialContext.sourceScene;
			}
			if (!flag && initialContext != null && initialContext.targetType != CustomizationTargetType.None)
			{
				this._selectedTabTarget = initialContext.targetType;
				this.ApplyTabVisualState(this._selectedTabTarget);
				this.SetHubVisible(false);
				if (this.editorViewRouter != null)
				{
					this.editorViewRouter.ShowForTarget(this._selectedTabTarget);
				}
				return;
			}
			this._selectedTabTarget = ((initialContext != null && initialContext.targetType > CustomizationTargetType.None) ? initialContext.targetType : ((this.defaultHubTab == CustomizationHubController.HubTab.MainCharacter) ? CustomizationTargetType.MainCharacter : CustomizationTargetType.Npc));
			this.ApplyTabVisualState(this._selectedTabTarget);
			this.SetHubVisible(true);
			if (this.editorViewRouter != null)
			{
				this.editorViewRouter.HideEditors();
			}
			this.LoadPresetsForCurrentTab(null);
			this.MarkPresetsLoaded(this._selectedTabTarget);
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x001EAC9E File Offset: 0x001E8E9E
		private void OnEnable()
		{
			if (!this._started || !this._refreshHubOnNextEnable)
			{
				return;
			}
			this._refreshHubOnNextEnable = false;
			base.StartCoroutine(this.RefreshHubAfterTakeoverOpen());
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x001EACC5 File Offset: 0x001E8EC5
		public void SelectMainCharacterTab()
		{
			this.SelectTabWithTransition(CustomizationTargetType.MainCharacter);
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x001EACCE File Offset: 0x001E8ECE
		public void SelectNpcTab()
		{
			this.SelectTabWithTransition(CustomizationTargetType.Npc);
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x001EACD7 File Offset: 0x001E8ED7
		public void ToggleHubClothing()
		{
			if (this._selectedTabTarget != CustomizationTargetType.MainCharacter)
			{
				return;
			}
			if (this.mcCustomizationController == null)
			{
				this.mcCustomizationController = Object.FindObjectOfType<MCCustomizationController>(true);
			}
			if (this.mcCustomizationController != null)
			{
				this.mcCustomizationController.ToggleNakedMC();
			}
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x001EAD16 File Offset: 0x001E8F16
		public void OpenSelectedTabCustomization()
		{
			if (this.presetListController != null && this.presetListController.SelectedPreset != null)
			{
				this.presetListController.EnterCustomizationForSelectedPreset();
				return;
			}
			if (this._selectedTabTarget == CustomizationTargetType.MainCharacter)
			{
				this.OpenMainCharacterCustomization();
				return;
			}
			this.OpenNpcCustomization();
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x001EAD58 File Offset: 0x001E8F58
		public void OpenMainCharacterCustomization()
		{
			string text = ((this.presetListController != null && this.presetListController.SystemPresetConfig != null) ? this.presetListController.SystemPresetConfig.MainCharacterId : "Ciel");
			this.OpenMainCharacterCustomization(text, true, string.Empty, false);
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x001EADAC File Offset: 0x001E8FAC
		public void OpenMainCharacterCustomization(string characterId, bool isDefaultCharacter, string presetId, bool skipPresetAutoApply = false)
		{
			CustomizationSessionContext customizationSessionContext = CustomizationSessionContext.Create(CustomizationTargetType.MainCharacter, characterId, isDefaultCharacter, this._sourceScene);
			customizationSessionContext.presetId = presetId;
			customizationSessionContext.skipPresetAutoApply = skipPresetAutoApply;
			this.SelectTab(CustomizationTargetType.MainCharacter);
			this.TryEnterCustomization(customizationSessionContext);
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x001EADE5 File Offset: 0x001E8FE5
		public void OpenNpcCustomization()
		{
			if (this.presetListController != null && this.presetListController.SelectedPreset != null)
			{
				this.presetListController.EnterCustomizationForSelectedPreset();
				return;
			}
			Debug.LogWarning("[HubController] No NPC preset selected to open.");
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x001EAE18 File Offset: 0x001E9018
		public void OpenNpcCustomization(string npcCharacterId)
		{
			this.OpenNpcCustomization(npcCharacterId, false, string.Empty, false);
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x001EAE28 File Offset: 0x001E9028
		public void OpenNpcCustomization(string npcCharacterId, bool isDefaultCharacter, string presetId, bool skipPresetAutoApply = false)
		{
			CustomizationSessionContext customizationSessionContext = CustomizationSessionContext.Create(CustomizationTargetType.Npc, npcCharacterId, isDefaultCharacter, this._sourceScene);
			customizationSessionContext.presetId = presetId;
			customizationSessionContext.skipPresetAutoApply = skipPresetAutoApply;
			this.SelectTab(CustomizationTargetType.Npc);
			this.TryEnterCustomization(customizationSessionContext);
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x001EAE64 File Offset: 0x001E9064
		public void BackToHub()
		{
			if (this.sceneBootstrapper == null)
			{
				return;
			}
			string text = ((this.presetListController != null && this.presetListController.SelectedPreset != null) ? this.presetListController.SelectedPreset.presetId : null);
			this.sceneBootstrapper.CloseActiveSession();
			if (this.editorViewRouter != null)
			{
				this.editorViewRouter.HideEditors();
			}
			this.SetHubVisible(true);
			this.LoadPresetsForCurrentTab(text);
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x001EAEE4 File Offset: 0x001E90E4
		public void PrepareForTakeoverClose()
		{
			if (this._tabTransition != null)
			{
				base.StopCoroutine(this._tabTransition);
				this._tabTransition = null;
			}
			CustomizationBusyOverlay busyOverlay = this._busyOverlay;
			if (busyOverlay != null)
			{
				busyOverlay.Hide();
			}
			MCCustomizationController componentInChildren = base.GetComponentInChildren<MCCustomizationController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.ResetTransientEditorState();
			}
			if (this.sceneBootstrapper != null)
			{
				this.sceneBootstrapper.CloseActiveSession();
			}
			if (this.editorViewRouter != null)
			{
				this.editorViewRouter.HideEditors();
			}
			this._selectedTabTarget = ((this.defaultHubTab == CustomizationHubController.HubTab.MainCharacter) ? CustomizationTargetType.MainCharacter : CustomizationTargetType.Npc);
			this.ApplyTabVisualState(this._selectedTabTarget);
			this.SetHubVisible(true);
			this._refreshHubOnNextEnable = true;
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x001EAF92 File Offset: 0x001E9192
		private void SetHubVisible(bool visible)
		{
			if (this.hubRoot != null)
			{
				this.hubRoot.SetActive(visible);
			}
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x001EAFAE File Offset: 0x001E91AE
		private void SelectTab(CustomizationTargetType targetType)
		{
			bool flag = this._selectedTabTarget != targetType;
			this._selectedTabTarget = targetType;
			this.ApplyTabVisualState(targetType);
			if (flag)
			{
				this.LoadPresetsForCurrentTab(null);
			}
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x001EAFD3 File Offset: 0x001E91D3
		private void SelectTabWithTransition(CustomizationTargetType targetType)
		{
			if (this._selectedTabTarget == targetType && this._tabTransition == null)
			{
				return;
			}
			if (this._tabTransition != null)
			{
				base.StopCoroutine(this._tabTransition);
			}
			this._tabTransition = base.StartCoroutine(this.SwitchTabRoutine(targetType));
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x001EB00E File Offset: 0x001E920E
		private IEnumerator SwitchTabRoutine(CustomizationTargetType targetType)
		{
			bool firstLoad = !this.HavePresetsLoaded(targetType);
			float minimumVisibleUntil = 0f;
			if (firstLoad)
			{
				this._busyOverlay = CustomizationBusyOverlay.GetOrCreate((this.hubRoot != null) ? this.hubRoot.transform : base.transform);
				CustomizationBusyOverlay busyOverlay = this._busyOverlay;
				if (busyOverlay != null)
				{
					busyOverlay.Show((targetType == CustomizationTargetType.Npc) ? "LOADING CHARACTERS" : "LOADING CIEL");
				}
				minimumVisibleUntil = Time.unscaledTime + 0.3f;
				yield return null;
			}
			this._selectedTabTarget = targetType;
			this.ApplyTabVisualState(targetType);
			this.LoadPresetsForCurrentTab(null);
			float timeoutAt = Time.unscaledTime + 8f;
			while (this.presetListController != null && this.presetListController.IsPreviewLoading && Time.unscaledTime < timeoutAt)
			{
				yield return null;
			}
			this.MarkPresetsLoaded(targetType);
			while (firstLoad && Time.unscaledTime < minimumVisibleUntil)
			{
				yield return null;
			}
			if (firstLoad)
			{
				yield return null;
				CustomizationBusyOverlay busyOverlay2 = this._busyOverlay;
				if (busyOverlay2 != null)
				{
					busyOverlay2.Hide();
				}
			}
			this._tabTransition = null;
			yield break;
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x001EB024 File Offset: 0x001E9224
		private IEnumerator RefreshHubAfterTakeoverOpen()
		{
			yield return null;
			this.SetHubVisible(true);
			this.ApplyTabVisualState(this._selectedTabTarget);
			this.LoadPresetsForCurrentTab(null);
			yield break;
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x001EB033 File Offset: 0x001E9233
		private void LoadPresetsForCurrentTab(string preferredPresetId = null)
		{
			if (this.presetListController == null)
			{
				return;
			}
			this.presetListController.LoadPresetsForTarget(this._selectedTabTarget, null, preferredPresetId);
		}

		// Token: 0x06004F3A RID: 20282 RVA: 0x001EB057 File Offset: 0x001E9257
		private bool HavePresetsLoaded(CustomizationTargetType targetType)
		{
			if (targetType != CustomizationTargetType.Npc)
			{
				return this._mainCharacterPresetsLoaded;
			}
			return this._npcPresetsLoaded;
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x001EB06A File Offset: 0x001E926A
		private void MarkPresetsLoaded(CustomizationTargetType targetType)
		{
			if (targetType == CustomizationTargetType.Npc)
			{
				this._npcPresetsLoaded = true;
				return;
			}
			if (targetType == CustomizationTargetType.MainCharacter)
			{
				this._mainCharacterPresetsLoaded = true;
			}
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x001EB084 File Offset: 0x001E9284
		private void ApplyTabVisualState(CustomizationTargetType targetType)
		{
			if (this.mainCharacterTabRoot != null)
			{
				this.mainCharacterTabRoot.SetActive(targetType == CustomizationTargetType.MainCharacter);
			}
			if (this.npcTabRoot != null)
			{
				this.npcTabRoot.SetActive(targetType == CustomizationTargetType.Npc);
			}
			if (this.hubClothingToggleRoot != null)
			{
				this.hubClothingToggleRoot.SetActive(targetType == CustomizationTargetType.MainCharacter);
			}
			this.ApplyTabButtonSelectedState(targetType);
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x001EB0F0 File Offset: 0x001E92F0
		private void ApplyTabButtonSelectedState(CustomizationTargetType targetType)
		{
			bool flag = targetType == CustomizationTargetType.MainCharacter;
			CustomizationHubController.SetTabGOState(this.mcTabNormalGO, this.mcTabSelectedGO, flag);
			CustomizationHubController.SetTabGOState(this.npcTabNormalGO, this.npcTabSelectedGO, !flag);
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x001EB129 File Offset: 0x001E9329
		private static void SetTabGOState(GameObject normalGO, GameObject selectedGO, bool selected)
		{
			if (normalGO != null)
			{
				normalGO.SetActive(!selected);
			}
			if (selectedGO != null)
			{
				selectedGO.SetActive(selected);
			}
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x001EB150 File Offset: 0x001E9350
		private void TryEnterCustomization(CustomizationSessionContext context)
		{
			if (this.verboseNavigationLogs)
			{
				Debug.Log(string.Format("[HubController] TryEnterCustomization target={0} characterId='{1}' presetId='{2}' skipPresetAutoApply={3} hasBootstrapper={4} hasEditorRouter={5}", new object[]
				{
					context.targetType,
					context.characterId,
					context.presetId,
					context.skipPresetAutoApply,
					this.sceneBootstrapper != null,
					this.editorViewRouter != null
				}));
			}
			if (this.sceneBootstrapper == null || !this.sceneBootstrapper.Bootstrap(context))
			{
				Debug.LogWarning(string.Format("[HubController] Bootstrap failed (bootstrapperNull={0}) target={1} characterId='{2}' presetId='{3}' skipPresetAutoApply={4}", new object[]
				{
					this.sceneBootstrapper == null,
					context.targetType,
					context.characterId,
					context.presetId,
					context.skipPresetAutoApply
				}));
				return;
			}
			if (this.verboseNavigationLogs)
			{
				Debug.Log("[HubController] Bootstrap succeeded.");
			}
			if (this.editorViewRouter != null)
			{
				if (this.verboseNavigationLogs)
				{
					Debug.Log(string.Format("[HubController] Calling editorViewRouter.ShowForTarget({0})", context.targetType));
				}
				this.editorViewRouter.ShowForTarget(context.targetType);
			}
			else
			{
				Debug.LogWarning("[HubController] editorViewRouter is NULL — cannot show editor!");
			}
			this.SetHubVisible(false);
			if (this.verboseNavigationLogs)
			{
				Debug.Log("[HubController] Hub hidden. Editor should now be visible.");
			}
		}

		// Token: 0x04005048 RID: 20552
		[Header("References")]
		[SerializeField]
		private CustomizationSceneBootstrapper sceneBootstrapper;

		// Token: 0x04005049 RID: 20553
		[SerializeField]
		private CustomizationEditorViewRouter editorViewRouter;

		// Token: 0x0400504A RID: 20554
		[SerializeField]
		private CustomizationHubPresetListController presetListController;

		// Token: 0x0400504B RID: 20555
		[Header("Hub UI")]
		[SerializeField]
		private GameObject hubRoot;

		// Token: 0x0400504C RID: 20556
		[SerializeField]
		private GameObject mainCharacterTabRoot;

		// Token: 0x0400504D RID: 20557
		[SerializeField]
		private GameObject npcTabRoot;

		// Token: 0x0400504E RID: 20558
		[SerializeField]
		private CustomizationHubController.HubTab defaultHubTab;

		// Token: 0x0400504F RID: 20559
		[Header("Tab Button State GOs")]
		[Tooltip("The 'Normal' child GO of the Main Character tab button.")]
		[SerializeField]
		private GameObject mcTabNormalGO;

		// Token: 0x04005050 RID: 20560
		[Tooltip("The 'Selected' child GO of the Main Character tab button.")]
		[SerializeField]
		private GameObject mcTabSelectedGO;

		// Token: 0x04005051 RID: 20561
		[Tooltip("The 'Normal' child GO of the NPC tab button.")]
		[SerializeField]
		private GameObject npcTabNormalGO;

		// Token: 0x04005052 RID: 20562
		[Tooltip("The 'Selected' child GO of the NPC tab button.")]
		[SerializeField]
		private GameObject npcTabSelectedGO;

		// Token: 0x04005053 RID: 20563
		[Header("Hub Clothing Toggle (solo MC)")]
		[Tooltip("Il GameObject ClothingTrigger clonato nell'Hub: visibile SOLO quando la tab è Main Character (l'area preview dell'Hub è condivisa MC/NPC, ma il toggle vestiti ha senso solo per il MC).")]
		[SerializeField]
		private GameObject hubClothingToggleRoot;

		// Token: 0x04005054 RID: 20564
		[Tooltip("Il Button del toggle vestiti nell'Hub. L'onClick viene agganciato A RUNTIME in Start a ToggleHubClothing(), che inoltra al MCCustomizationController del preview.")]
		[SerializeField]
		private Button hubClothingToggleButton;

		// Token: 0x04005055 RID: 20565
		[Tooltip("Controller MC che possiede ToggleNakedMC (relay verso MCStyleCustomizationService). Se nullo, risolto via FindObjectOfType.")]
		[SerializeField]
		private MCCustomizationController mcCustomizationController;

		// Token: 0x04005056 RID: 20566
		[Header("Debug")]
		[SerializeField]
		private bool verboseNavigationLogs;

		// Token: 0x04005057 RID: 20567
		private string _sourceScene;

		// Token: 0x04005058 RID: 20568
		private CustomizationTargetType _selectedTabTarget = CustomizationTargetType.MainCharacter;

		// Token: 0x04005059 RID: 20569
		private Coroutine _tabTransition;

		// Token: 0x0400505A RID: 20570
		private CustomizationBusyOverlay _busyOverlay;

		// Token: 0x0400505B RID: 20571
		private bool _started;

		// Token: 0x0400505C RID: 20572
		private bool _refreshHubOnNextEnable;

		// Token: 0x0400505D RID: 20573
		private bool _mainCharacterPresetsLoaded;

		// Token: 0x0400505E RID: 20574
		private bool _npcPresetsLoaded;

		// Token: 0x02000AF1 RID: 2801
		private enum HubTab
		{
			// Token: 0x04005060 RID: 20576
			MainCharacter,
			// Token: 0x04005061 RID: 20577
			Npc
		}
	}
}
