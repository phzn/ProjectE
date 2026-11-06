using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.SkinDecals.UI
{
	// Token: 0x02000942 RID: 2370
	public class DecalCustomizationPanelController : MonoBehaviour
	{
		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06004084 RID: 16516 RVA: 0x0017F360 File Offset: 0x0017D560
		// (remove) Token: 0x06004085 RID: 16517 RVA: 0x0017F398 File Offset: 0x0017D598
		public event Action OnDecalStateChanged;

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06004086 RID: 16518 RVA: 0x0017F3CD File Offset: 0x0017D5CD
		public SkinDecalController DecalController
		{
			get
			{
				return this.skinDecalController;
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06004087 RID: 16519 RVA: 0x0017F3D5 File Offset: 0x0017D5D5
		public IReadOnlyList<SkinDecal> Catalog
		{
			get
			{
				return this.decalCatalog;
			}
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x0017F3DD File Offset: 0x0017D5DD
		private void OnEnable()
		{
			if (this.skinDecalController != null)
			{
				this.skinDecalController.OnDecalsChanged += this.RefreshCardStates;
			}
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x0017F404 File Offset: 0x0017D604
		private void OnDisable()
		{
			if (this.skinDecalController != null)
			{
				this.skinDecalController.OnDecalsChanged -= this.RefreshCardStates;
			}
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x0017F42B File Offset: 0x0017D62B
		public void Show()
		{
			if (this.panelRoot != null)
			{
				this.panelRoot.SetActive(true);
			}
			this.RebuildCards();
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x0017F44D File Offset: 0x0017D64D
		public void Hide()
		{
			if (this.panelRoot != null)
			{
				this.panelRoot.SetActive(false);
			}
			this.ClearCards();
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x0600408C RID: 16524 RVA: 0x0017F46F File Offset: 0x0017D66F
		public bool IsActive
		{
			get
			{
				return this.panelRoot != null && this.panelRoot.activeSelf;
			}
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x0017F48C File Offset: 0x0017D68C
		private void RebuildCards()
		{
			this.ClearCards();
			if (this.cardPrefab == null || this.cardContainer == null || this.skinDecalController == null)
			{
				return;
			}
			for (int i = 0; i < this.decalCatalog.Count; i++)
			{
				SkinDecal skinDecal = this.decalCatalog[i];
				if (!(skinDecal == null) && !skinDecal.gameplayOnly)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.cardPrefab, this.cardContainer);
					gameObject.name = "DecalCard_" + skinDecal.name;
					DecalCardUI decalCardUI = gameObject.GetComponent<DecalCardUI>();
					if (decalCardUI == null)
					{
						decalCardUI = gameObject.AddComponent<DecalCardUI>();
					}
					bool flag = this.skinDecalController.IsDecalActive(skinDecal);
					decalCardUI.Setup(skinDecal, flag, new Action<SkinDecal, bool>(this.OnCardToggled));
					this._spawnedCards.Add(decalCardUI);
				}
			}
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x0017F574 File Offset: 0x0017D774
		private void ClearCards()
		{
			for (int i = this._spawnedCards.Count - 1; i >= 0; i--)
			{
				if (this._spawnedCards[i] != null && this._spawnedCards[i].gameObject != null)
				{
					Object.Destroy(this._spawnedCards[i].gameObject);
				}
			}
			this._spawnedCards.Clear();
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x0017F5E8 File Offset: 0x0017D7E8
		private void RefreshCardStates()
		{
			for (int i = 0; i < this._spawnedCards.Count; i++)
			{
				DecalCardUI decalCardUI = this._spawnedCards[i];
				if (!(decalCardUI == null) && !(decalCardUI.Decal == null))
				{
					decalCardUI.SetActiveState(this.skinDecalController.IsDecalActive(decalCardUI.Decal));
				}
			}
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x0017F648 File Offset: 0x0017D848
		private void OnCardToggled(SkinDecal decal, bool newState)
		{
			if (this.skinDecalController == null || decal == null || decal.gameplayOnly)
			{
				return;
			}
			if (newState)
			{
				if (!this.skinDecalController.HasDecal(decal))
				{
					this.skinDecalController.AddDecal(decal, null, null);
				}
				else
				{
					this.skinDecalController.SetDecalActive(decal, true);
				}
			}
			else
			{
				this.skinDecalController.SetDecalActive(decal, false);
			}
			Action onDecalStateChanged = this.OnDecalStateChanged;
			if (onDecalStateChanged == null)
			{
				return;
			}
			onDecalStateChanged();
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x0017F6D2 File Offset: 0x0017D8D2
		public SkinDecalSaveData CaptureSaveData()
		{
			if (this.skinDecalController == null)
			{
				return new SkinDecalSaveData();
			}
			return this.skinDecalController.GetSaveData(true);
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x0017F6F4 File Offset: 0x0017D8F4
		public void LoadSaveData(SkinDecalSaveData data)
		{
			if (this.skinDecalController == null || data == null)
			{
				return;
			}
			this.skinDecalController.LoadSaveData(data, this.decalCatalog);
			this.RefreshCardStates();
		}

		// Token: 0x0400405B RID: 16475
		[Header("References")]
		[Tooltip("Root GameObject toggled on/off when the panel is shown/hidden.")]
		[SerializeField]
		private GameObject panelRoot;

		// Token: 0x0400405C RID: 16476
		[Tooltip("Container (with GridLayoutGroup) where decal cards are spawned.")]
		[SerializeField]
		private Transform cardContainer;

		// Token: 0x0400405D RID: 16477
		[Tooltip("Prefab for a single decal card. Must have DecalCardUI component.")]
		[SerializeField]
		private GameObject cardPrefab;

		// Token: 0x0400405E RID: 16478
		[Header("Data")]
		[Tooltip("The SkinDecalController that manages runtime decal state.")]
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x0400405F RID: 16479
		[Tooltip("Full catalog of available decals the player can toggle.")]
		[SerializeField]
		private List<SkinDecal> decalCatalog = new List<SkinDecal>();

		// Token: 0x04004060 RID: 16480
		private readonly List<DecalCardUI> _spawnedCards = new List<DecalCardUI>();
	}
}
