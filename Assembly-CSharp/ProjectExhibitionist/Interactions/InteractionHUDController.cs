using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x02000915 RID: 2325
	public class InteractionHUDController : MonoBehaviour
	{
		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003EEB RID: 16107 RVA: 0x00175A8C File Offset: 0x00173C8C
		// (set) Token: 0x06003EEC RID: 16108 RVA: 0x00175A93 File Offset: 0x00173C93
		public static InteractionHUDController Instance { get; private set; }

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003EED RID: 16109 RVA: 0x00175A9B File Offset: 0x00173C9B
		public bool IsShowing
		{
			get
			{
				return this.actionsContainer != null && this.actionsContainer.activeInHierarchy;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003EEE RID: 16110 RVA: 0x00175AB8 File Offset: 0x00173CB8
		public InteractionConfigSO CurrentConfig
		{
			get
			{
				return this._currentConfig;
			}
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x00175AC0 File Offset: 0x00173CC0
		public bool IsActionVisible(InteractionConfigSO config, string key)
		{
			foreach (UsableAction usableAction in this._visibleActions)
			{
				InteractionConfigSO interactionConfigSO;
				if (usableAction != null && string.Equals(usableAction.keyDisplayLabel, key, StringComparison.OrdinalIgnoreCase) && this._actionSources.TryGetValue(usableAction, out interactionConfigSO) && interactionConfigSO == config)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x00175B40 File Offset: 0x00173D40
		public void ShowSupplemental(Object owner, InteractionConfigSO config)
		{
			if (owner == null)
			{
				return;
			}
			for (int i = 0; i < this._supplementalConfigs.Count; i++)
			{
				if (!(this._supplementalConfigs[i].owner != owner))
				{
					this._supplementalConfigs[i] = new InteractionHUDController.SupplementalConfig
					{
						owner = owner,
						config = config
					};
					this.RefreshDisplay(false);
					return;
				}
			}
			this._supplementalConfigs.Add(new InteractionHUDController.SupplementalConfig
			{
				owner = owner,
				config = config
			});
			this.RefreshDisplay(false);
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x00175BE0 File Offset: 0x00173DE0
		public void HideSupplemental(Object owner)
		{
			for (int i = this._supplementalConfigs.Count - 1; i >= 0; i--)
			{
				if (this._supplementalConfigs[i].owner == owner || this._supplementalConfigs[i].owner == null)
				{
					this._supplementalConfigs.RemoveAt(i);
				}
			}
			this.RefreshDisplay(false);
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x00175C4A File Offset: 0x00173E4A
		private void Awake()
		{
			if (InteractionHUDController.Instance == null)
			{
				InteractionHUDController.Instance = this;
				this.Hide();
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x00175C73 File Offset: 0x00173E73
		private void OnDestroy()
		{
			if (InteractionHUDController.Instance == this)
			{
				InteractionHUDController.Instance = null;
			}
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x00175C88 File Offset: 0x00173E88
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.StabilizeLayout;
			this.StabilizeLayout();
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x00175CA1 File Offset: 0x00173EA1
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.StabilizeLayout;
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x00175CB4 File Offset: 0x00173EB4
		public void Show(InteractionConfigSO config)
		{
			this.Show(config, false);
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x00175CC0 File Offset: 0x00173EC0
		public void Show(InteractionConfigSO config, bool forceRefresh)
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null && instance.isActiveAndEnabled && instance.IsEventActive && !instance.AllowsInteractionControls(config))
			{
				if (!instance.AllowsInteractionControls(this._currentConfig))
				{
					this.Hide();
				}
				return;
			}
			if (config == null || config.usableActions.Count == 0)
			{
				this.Hide();
				return;
			}
			this._currentConfig = config;
			this.RefreshDisplay(forceRefresh);
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x00175D38 File Offset: 0x00173F38
		private void RefreshDisplay(bool forceRefresh)
		{
			this._visibleActions.Clear();
			this._actionSources.Clear();
			this.AppendVisibleActions(this._currentConfig, false);
			for (int i = this._supplementalConfigs.Count - 1; i >= 0; i--)
			{
				if (this._supplementalConfigs[i].owner == null)
				{
					this._supplementalConfigs.RemoveAt(i);
				}
				else
				{
					this.AppendVisibleActions(this._supplementalConfigs[i].config, true);
				}
			}
			if (!forceRefresh && this.HasSameDisplayedContent())
			{
				if (this.actionsContainer != null)
				{
					this.actionsContainer.SetActive(this._visibleActions.Count > 0);
				}
				return;
			}
			this.ClearItems();
			this.SnapshotDisplayedContent();
			if (this.actionsContainer != null)
			{
				this.actionsContainer.SetActive(this._visibleActions.Count > 0);
			}
			foreach (UsableAction usableAction in this._visibleActions)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.actionItemPrefab, this.actionsParent);
				ActionItemUI component = gameObject.GetComponent<ActionItemUI>();
				if (component != null)
				{
					InteractionConfigSO interactionConfigSO;
					this._actionSources.TryGetValue(usableAction, out interactionConfigSO);
					component.Setup(usableAction.keyDisplayLabel, usableAction.actionName, interactionConfigSO);
				}
				this.spawnedItems.Add(gameObject);
			}
			this.StabilizeLayout();
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x00175EC0 File Offset: 0x001740C0
		private void AppendVisibleActions(InteractionConfigSO config, bool supplemental)
		{
			if (config == null)
			{
				return;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null && instance.isActiveAndEnabled && instance.IsEventActive && !instance.AllowsInteractionControls(config))
			{
				return;
			}
			foreach (UsableAction usableAction in config.usableActions)
			{
				if (usableAction != null)
				{
					bool flag = false;
					if (supplemental && !string.IsNullOrEmpty(usableAction.keyDisplayLabel))
					{
						for (int i = 0; i < this._visibleActions.Count; i++)
						{
							if (string.Equals(this._visibleActions[i].keyDisplayLabel, usableAction.keyDisplayLabel, StringComparison.OrdinalIgnoreCase))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						this._visibleActions.Add(usableAction);
						this._actionSources[usableAction] = config;
					}
				}
			}
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x00175FB0 File Offset: 0x001741B0
		public void Hide()
		{
			if (this._layoutRoutine != null)
			{
				base.StopCoroutine(this._layoutRoutine);
				this._layoutRoutine = null;
			}
			this._currentConfig = null;
			this.RefreshDisplay(false);
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x00175FDB File Offset: 0x001741DB
		public bool HideIfCurrent(InteractionConfigSO config)
		{
			if (config == null || this._currentConfig != config)
			{
				return false;
			}
			this.Hide();
			return true;
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x00176000 File Offset: 0x00174200
		private void ClearItems()
		{
			foreach (GameObject gameObject in this.spawnedItems)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(false);
					if (Application.isPlaying)
					{
						Object.Destroy(gameObject);
					}
					else
					{
						Object.DestroyImmediate(gameObject);
					}
				}
			}
			this.spawnedItems.Clear();
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x0017607C File Offset: 0x0017427C
		private bool HasSameDisplayedContent()
		{
			List<UsableAction> visibleActions = this._visibleActions;
			if (this._displayedActionNames.Count != visibleActions.Count || this._displayedKeyLabels.Count != visibleActions.Count || this.spawnedItems.Count != visibleActions.Count)
			{
				return false;
			}
			for (int i = 0; i < visibleActions.Count; i++)
			{
				UsableAction usableAction = visibleActions[i];
				if (usableAction == null || this.spawnedItems[i] == null || !string.Equals(this._displayedActionNames[i], usableAction.actionName, StringComparison.Ordinal) || !string.Equals(this._displayedKeyLabels[i], usableAction.keyDisplayLabel, StringComparison.Ordinal))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x00176134 File Offset: 0x00174334
		private void SnapshotDisplayedContent()
		{
			this._displayedActionNames.Clear();
			this._displayedKeyLabels.Clear();
			List<UsableAction> visibleActions = this._visibleActions;
			for (int i = 0; i < visibleActions.Count; i++)
			{
				UsableAction usableAction = visibleActions[i];
				this._displayedActionNames.Add((usableAction != null) ? usableAction.actionName : null);
				this._displayedKeyLabels.Add((usableAction != null) ? usableAction.keyDisplayLabel : null);
			}
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x001761A5 File Offset: 0x001743A5
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

		// Token: 0x06003F00 RID: 16128 RVA: 0x001761DC File Offset: 0x001743DC
		private IEnumerator StabilizeLayoutOverFrames()
		{
			yield return null;
			this.RebuildLayoutNow();
			yield return new WaitForEndOfFrame();
			this.RebuildLayoutNow();
			this._layoutRoutine = null;
			yield break;
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x001761EC File Offset: 0x001743EC
		private void RebuildLayoutNow()
		{
			RectTransform rectTransform = this.actionsParent as RectTransform;
			if (rectTransform == null || this.actionsContainer == null || !this.actionsContainer.activeInHierarchy)
			{
				return;
			}
			rectTransform.GetComponentsInChildren<TextMeshProUGUI>(true, this._layoutLabels);
			for (int i = 0; i < this._layoutLabels.Count; i++)
			{
				TextMeshProUGUI textMeshProUGUI = this._layoutLabels[i];
				if (textMeshProUGUI != null && textMeshProUGUI.font != null && textMeshProUGUI.fontSharedMaterial != null)
				{
					textMeshProUGUI.ForceMeshUpdate(true, true);
				}
			}
			Canvas.ForceUpdateCanvases();
			rectTransform.GetComponentsInChildren<RectTransform>(true, this._layoutRects);
			for (int j = this._layoutRects.Count - 1; j >= 0; j--)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this._layoutRects[j]);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			RectTransform rectTransform2 = this.actionsContainer.transform as RectTransform;
			if (rectTransform2 != null && rectTransform2 != rectTransform)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform2);
			}
			Canvas.ForceUpdateCanvases();
		}

		// Token: 0x04003EE7 RID: 16103
		[Header("HUD Container")]
		[SerializeField]
		private GameObject actionsContainer;

		// Token: 0x04003EE8 RID: 16104
		[SerializeField]
		private Transform actionsParent;

		// Token: 0x04003EE9 RID: 16105
		[SerializeField]
		private GameObject actionItemPrefab;

		// Token: 0x04003EEA RID: 16106
		private List<GameObject> spawnedItems = new List<GameObject>();

		// Token: 0x04003EEB RID: 16107
		private InteractionConfigSO _currentConfig;

		// Token: 0x04003EEC RID: 16108
		private readonly List<string> _displayedActionNames = new List<string>();

		// Token: 0x04003EED RID: 16109
		private readonly List<string> _displayedKeyLabels = new List<string>();

		// Token: 0x04003EEE RID: 16110
		private readonly List<TextMeshProUGUI> _layoutLabels = new List<TextMeshProUGUI>();

		// Token: 0x04003EEF RID: 16111
		private readonly List<RectTransform> _layoutRects = new List<RectTransform>();

		// Token: 0x04003EF0 RID: 16112
		private Coroutine _layoutRoutine;

		// Token: 0x04003EF1 RID: 16113
		private readonly List<InteractionHUDController.SupplementalConfig> _supplementalConfigs = new List<InteractionHUDController.SupplementalConfig>();

		// Token: 0x04003EF2 RID: 16114
		private readonly List<UsableAction> _visibleActions = new List<UsableAction>();

		// Token: 0x04003EF3 RID: 16115
		private readonly Dictionary<UsableAction, InteractionConfigSO> _actionSources = new Dictionary<UsableAction, InteractionConfigSO>();

		// Token: 0x02000916 RID: 2326
		private struct SupplementalConfig
		{
			// Token: 0x04003EF4 RID: 16116
			public Object owner;

			// Token: 0x04003EF5 RID: 16117
			public InteractionConfigSO config;
		}
	}
}
