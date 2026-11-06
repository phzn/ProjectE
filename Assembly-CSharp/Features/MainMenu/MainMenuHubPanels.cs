using System;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x02001063 RID: 4195
	public sealed class MainMenuHubPanels : MonoBehaviour
	{
		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x06007F54 RID: 32596 RVA: 0x00340BB5 File Offset: 0x0033EDB5
		public bool IsPanelOpen
		{
			get
			{
				return MainMenuHubPanels.IsActive(this.collectionPanel) || MainMenuHubPanels.IsActive(this.settingsPanel) || MainMenuHubPanels.IsActive(this.creditsPanel) || MainMenuHubPanels.IsActive(this._workshopPanel);
			}
		}

		// Token: 0x06007F55 RID: 32597 RVA: 0x00340BEB File Offset: 0x0033EDEB
		private void Awake()
		{
			this.ClosePanelsImmediate();
			GameSettingsService.ApplyAudio();
		}

		// Token: 0x06007F56 RID: 32598 RVA: 0x00340BF8 File Offset: 0x0033EDF8
		private void Update()
		{
			if (!this.IsPanelOpen || !EscapeInputCoordinator.TryConsume(this))
			{
				return;
			}
			if (MainMenuHubPanels.IsActive(this.settingsPanel))
			{
				MainMenuSettingsPanel component = this.settingsPanel.GetComponent<MainMenuSettingsPanel>();
				if (component != null && component.TryHandleBack())
				{
					return;
				}
			}
			if (MainMenuHubPanels.IsActive(this.collectionPanel))
			{
				MainMenuCollectionPanel component2 = this.collectionPanel.GetComponent<MainMenuCollectionPanel>();
				if (component2 != null && component2.TryHandleBack())
				{
					return;
				}
			}
			this.ClosePanels();
		}

		// Token: 0x06007F57 RID: 32599 RVA: 0x00340C74 File Offset: 0x0033EE74
		public void OpenCollection()
		{
			this.ShowOnly(this.collectionPanel);
			MainMenuCollectionPanel mainMenuCollectionPanel = ((this.collectionPanel != null) ? this.collectionPanel.GetComponent<MainMenuCollectionPanel>() : null);
			if (mainMenuCollectionPanel != null)
			{
				mainMenuCollectionPanel.Refresh();
			}
		}

		// Token: 0x06007F58 RID: 32600 RVA: 0x00340CBC File Offset: 0x0033EEBC
		public void OpenSettings()
		{
			this.ShowOnly(this.settingsPanel);
			MainMenuSettingsPanel mainMenuSettingsPanel = ((this.settingsPanel != null) ? this.settingsPanel.GetComponent<MainMenuSettingsPanel>() : null);
			if (mainMenuSettingsPanel != null)
			{
				mainMenuSettingsPanel.RefreshFromCurrentSettings();
			}
		}

		// Token: 0x06007F59 RID: 32601 RVA: 0x00340D04 File Offset: 0x0033EF04
		public void OpenCredits()
		{
			this.ShowOnly(this.creditsPanel);
			MainMenuController mainMenuController = Object.FindObjectOfType<MainMenuController>();
			if (mainMenuController != null)
			{
				mainMenuController.EnterCreditsPresentation();
			}
		}

		// Token: 0x06007F5A RID: 32602 RVA: 0x00340D32 File Offset: 0x0033EF32
		public void RegisterWorkshop(MainMenuWorkshopPanel panel)
		{
			this._workshopPanel = ((panel != null) ? panel.gameObject : null);
		}

		// Token: 0x06007F5B RID: 32603 RVA: 0x00340D4C File Offset: 0x0033EF4C
		public void OpenWorkshop()
		{
			if (this._workshopPanel == null)
			{
				return;
			}
			this.ShowOnly(this._workshopPanel);
			this._workshopPanel.transform.SetAsLastSibling();
		}

		// Token: 0x06007F5C RID: 32604 RVA: 0x00340D7C File Offset: 0x0033EF7C
		public void ClosePanels()
		{
			MainMenuHubPanels.ExitCreditsPresentation();
			MainMenuHubPanels.CloseAnimated(this.collectionPanel);
			MainMenuHubPanels.CloseAnimated(this.settingsPanel);
			MainMenuHubPanels.CloseAnimated(this.creditsPanel);
			MainMenuHubPanels.CloseAnimated(this._workshopPanel);
			this.SetMenuHidden(false);
			this.SetCollectionCharacterHidden(false);
		}

		// Token: 0x06007F5D RID: 32605 RVA: 0x00340DC8 File Offset: 0x0033EFC8
		private void ClosePanelsImmediate()
		{
			MainMenuHubPanels.ExitCreditsPresentation();
			MainMenuHubPanels.SetActive(this.collectionPanel, false);
			MainMenuHubPanels.SetActive(this.settingsPanel, false);
			MainMenuHubPanels.SetActive(this.creditsPanel, false);
			MainMenuHubPanels.SetActive(this._workshopPanel, false);
			this.SetMenuHidden(false);
			this.SetCollectionCharacterHidden(false);
		}

		// Token: 0x06007F5E RID: 32606 RVA: 0x00340E18 File Offset: 0x0033F018
		private void ShowOnly(GameObject target)
		{
			if (target != this.creditsPanel)
			{
				MainMenuHubPanels.ExitCreditsPresentation();
			}
			MainMenuHubPanels.SetActive(this.collectionPanel, target == this.collectionPanel);
			MainMenuHubPanels.SetActive(this.settingsPanel, target == this.settingsPanel);
			MainMenuHubPanels.SetActive(this.creditsPanel, target == this.creditsPanel);
			MainMenuHubPanels.SetActive(this._workshopPanel, target == this._workshopPanel);
			this.SetMenuHidden(target != null);
			this.SetCollectionCharacterHidden(target == this.collectionPanel || target == this._workshopPanel);
		}

		// Token: 0x06007F5F RID: 32607 RVA: 0x00340EC4 File Offset: 0x0033F0C4
		private static void ExitCreditsPresentation()
		{
			MainMenuController mainMenuController = Object.FindObjectOfType<MainMenuController>();
			if (mainMenuController != null)
			{
				mainMenuController.ExitCreditsPresentation();
			}
		}

		// Token: 0x06007F60 RID: 32608 RVA: 0x00340EE8 File Offset: 0x0033F0E8
		private void SetMenuHidden(bool hidden)
		{
			if (this.hiddenWhileOpen == null)
			{
				return;
			}
			GameObject[] array = this.hiddenWhileOpen;
			for (int i = 0; i < array.Length; i++)
			{
				MainMenuHubPanels.SetActive(array[i], !hidden);
			}
		}

		// Token: 0x06007F61 RID: 32609 RVA: 0x00340F20 File Offset: 0x0033F120
		private void SetCollectionCharacterHidden(bool hidden)
		{
			if (this._collectionCharacterHidden == hidden)
			{
				return;
			}
			if (!hidden)
			{
				if (this._collectionCharacterRenderers != null)
				{
					for (int i = 0; i < this._collectionCharacterRenderers.Length; i++)
					{
						if (this._collectionCharacterRenderers[i] != null)
						{
							this._collectionCharacterRenderers[i].enabled = this._collectionCharacterRendererStates != null && i < this._collectionCharacterRendererStates.Length && this._collectionCharacterRendererStates[i];
						}
					}
				}
				this._collectionCharacterRenderers = null;
				this._collectionCharacterRendererStates = null;
				this._collectionCharacterHidden = false;
				return;
			}
			GameObject gameObject = null;
			foreach (GameObject gameObject2 in GameObject.FindGameObjectsWithTag("Player"))
			{
				if (gameObject2 != null && gameObject2.GetComponent<Animator>() != null)
				{
					gameObject = gameObject2;
					break;
				}
			}
			GameObject[] array;
			if (gameObject == null && array.Length != 0)
			{
				gameObject = array[0];
			}
			if (gameObject == null)
			{
				return;
			}
			this._collectionCharacterRenderers = gameObject.GetComponentsInChildren<Renderer>(true);
			this._collectionCharacterRendererStates = new bool[this._collectionCharacterRenderers.Length];
			for (int k = 0; k < this._collectionCharacterRenderers.Length; k++)
			{
				Renderer renderer = this._collectionCharacterRenderers[k];
				if (!(renderer == null))
				{
					this._collectionCharacterRendererStates[k] = renderer.enabled;
					renderer.enabled = false;
				}
			}
			this._collectionCharacterHidden = true;
		}

		// Token: 0x06007F62 RID: 32610 RVA: 0x00341074 File Offset: 0x0033F274
		private static void CloseAnimated(GameObject target)
		{
			if (target == null || !target.activeSelf)
			{
				return;
			}
			MainMenuPanelIntroAnimator component = target.GetComponent<MainMenuPanelIntroAnimator>();
			if (component != null)
			{
				component.CloseAndDeactivate();
				return;
			}
			target.SetActive(false);
		}

		// Token: 0x06007F63 RID: 32611 RVA: 0x003410B1 File Offset: 0x0033F2B1
		private static bool IsActive(GameObject target)
		{
			return target != null && target.activeSelf;
		}

		// Token: 0x06007F64 RID: 32612 RVA: 0x003410C4 File Offset: 0x0033F2C4
		private static void SetActive(GameObject target, bool active)
		{
			if (target != null && target.activeSelf != active)
			{
				target.SetActive(active);
			}
		}

		// Token: 0x040082D3 RID: 33491
		[SerializeField]
		private GameObject collectionPanel;

		// Token: 0x040082D4 RID: 33492
		[SerializeField]
		private GameObject settingsPanel;

		// Token: 0x040082D5 RID: 33493
		[SerializeField]
		private GameObject creditsPanel;

		// Token: 0x040082D6 RID: 33494
		private GameObject _workshopPanel;

		// Token: 0x040082D7 RID: 33495
		[Tooltip("Elementi del menu (logo, colonna bottoni, socials, versione) nascosti mentre un pannello overlay è aperto, per evitare il ghosting dietro l'overlay.")]
		[SerializeField]
		private GameObject[] hiddenWhileOpen;

		// Token: 0x040082D8 RID: 33496
		private Renderer[] _collectionCharacterRenderers;

		// Token: 0x040082D9 RID: 33497
		private bool[] _collectionCharacterRendererStates;

		// Token: 0x040082DA RID: 33498
		private bool _collectionCharacterHidden;
	}
}
