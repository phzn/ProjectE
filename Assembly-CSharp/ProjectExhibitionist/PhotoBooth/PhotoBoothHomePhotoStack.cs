using System;
using System.Collections;
using System.Collections.Generic;
using Features.Inventory;
using Features.Localization;
using Features.Rank;
using Features.TrashcanLoot;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.UI;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x0200097C RID: 2428
	[DisallowMultipleComponent]
	public sealed class PhotoBoothHomePhotoStack : MonoBehaviour, IInteractable, IInteractionCandidate
	{
		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x060042CE RID: 17102 RVA: 0x0018F902 File Offset: 0x0018DB02
		public InteractionConfigSO Config
		{
			get
			{
				if (!(this._runtimeConfig != null))
				{
					return this.interactionConfig;
				}
				return this._runtimeConfig;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x060042CF RID: 17103 RVA: 0x0018F91F File Offset: 0x0018DB1F
		public bool IsBeingUsed
		{
			get
			{
				return this._collecting;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x060042D0 RID: 17104 RVA: 0x0018F927 File Offset: 0x0018DB27
		public float CandidateSqrDistance
		{
			get
			{
				return this._candidateSqrDistance;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x060042D1 RID: 17105 RVA: 0x0018F92F File Offset: 0x0018DB2F
		public bool IsCandidateEligible
		{
			get
			{
				return this._unlocked && this._inRange && !this._collecting && this.HasMissingPhotos();
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x060042D2 RID: 17106 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x060042D3 RID: 17107 RVA: 0x0018F951 File Offset: 0x0018DB51
		public bool IsUnlocked
		{
			get
			{
				return this._unlocked;
			}
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x0018F95C File Offset: 0x0018DB5C
		private void Awake()
		{
			if (this.visualRoot == null)
			{
				GameObject gameObject = new GameObject("PhotoStack_Visuals");
				gameObject.transform.SetParent(base.transform, false);
				this.visualRoot = gameObject.transform;
			}
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			this.BuildRuntimeConfig();
			this.SetPromptVisible(false);
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x0018F9C8 File Offset: 0x0018DBC8
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
			PhotoBoothPhotoArchive.Changed += this.RefreshStack;
			RankProgressionService.Changed += this.HandleRankChanged;
			this.ApplyRankUnlock(this._started);
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x0018F9FE File Offset: 0x0018DBFE
		private void Start()
		{
			this._started = true;
			this.ApplyRankUnlock(false);
			if (!this._unlocked)
			{
				return;
			}
			this.ResolvePlayer();
			this.RefreshStack();
			PhotoBoothHomePlacedPhotoPersistence.RestoreAll(this);
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x0018FA2C File Offset: 0x0018DC2C
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			PhotoBoothPhotoArchive.Changed -= this.RefreshStack;
			RankProgressionService.Changed -= this.HandleRankChanged;
			if (this._refreshRoutine != null)
			{
				base.StopCoroutine(this._refreshRoutine);
				this._refreshRoutine = null;
			}
			this.ResetHold();
			this._collecting = false;
			this.SetPromptVisible(false);
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x0018FA90 File Offset: 0x0018DC90
		private void OnDestroy()
		{
			this.ClearDisplay();
			if (this._runtimeConfig != null)
			{
				Object.Destroy(this._runtimeConfig);
			}
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x0018FAB4 File Offset: 0x0018DCB4
		private void Update()
		{
			if (!this._unlocked)
			{
				this._candidateSqrDistance = float.MaxValue;
				this._inRange = false;
				this.ResetHold();
				this.SetPromptVisible(false);
				return;
			}
			if (this._player == null || this._inventory == null || this._printHolder == null)
			{
				this.ResolvePlayer();
			}
			if (this._player == null)
			{
				this._candidateSqrDistance = float.MaxValue;
				this._inRange = false;
				this.ResetHold();
				this.SetPromptVisible(false);
				return;
			}
			this._candidateSqrDistance = Vector3.ProjectOnPlane(this._player.transform.position - base.transform.position, Vector3.up).sqrMagnitude;
			this._inRange = this._candidateSqrDistance <= this.interactionRadius * this.interactionRadius;
			bool flag = this.IsCandidateEligible && InteractionCandidateRegistry.IsWinner(this) && PlayerActivityGate.Allows(PlayerActivity.Interact);
			this.SetPromptVisible(flag);
			if (!flag)
			{
				this.ResetHold();
				return;
			}
			if (this.interactInput.GetButtonDown())
			{
				this._holding = true;
				this._holdTime = 0f;
				this.ShowHoldProgress();
			}
			if (this._holding && this.interactInput.GetButton())
			{
				this._holdTime += Time.deltaTime;
				if (this._holdProgressUI != null)
				{
					this._holdProgressUI.SetProgress(Mathf.Clamp01(this._holdTime / this.holdDuration));
				}
				if (this._holdTime >= this.holdDuration)
				{
					this.ResetHold();
					this.OnInteract(this._player.gameObject);
					return;
				}
			}
			else if (this._holding)
			{
				this.ResetHold();
			}
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x0018FC74 File Offset: 0x0018DE74
		public bool CanInteract(GameObject player)
		{
			if (this._player == null || this._inventory == null || this._printHolder == null)
			{
				this.ResolvePlayer();
			}
			return this._unlocked && !this._collecting && this._inventory != null && this._printHolder != null && this.printVisualPrefab != null && this.HasMissingPhotos();
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x0018FCF4 File Offset: 0x0018DEF4
		public void OnInteract(GameObject player)
		{
			if (!this.CanInteract(player))
			{
				return;
			}
			InteractionCandidateRegistry.MarkInteract();
			this._collecting = true;
			this.SetPromptVisible(false);
			IReadOnlyList<string> archiveIds = PhotoBoothPhotoArchive.GetArchiveIds();
			List<string> list = new List<string>(archiveIds.Count);
			List<int> list2 = new List<int>(archiveIds.Count);
			bool flag = this.InventoryHasPhotoAlbum();
			for (int i = 0; i < archiveIds.Count; i++)
			{
				string text = archiveIds[i];
				if (!this.InventoryContains(text) && !PhotoBoothHomePlacedPhotoPersistence.ContainsArchiveId(text))
				{
					list.Add(text);
					list2.Add(i + 1);
				}
			}
			PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto = null;
			for (int j = this._displayPhotos.Count - 1; j >= 0; j--)
			{
				PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto2 = this._displayPhotos[j];
				if (loadedPhoto2 != null && list.Contains(loadedPhoto2.archiveId))
				{
					loadedPhoto = loadedPhoto2;
					break;
				}
			}
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = PhotoBoothPhotoItemSO.CreateRuntimeArchiveAlbum(this.printVisualPrefab, this.inventoryWorldScale, Quaternion.Euler(this.inventoryWorldRestEuler), list, list2, loadedPhoto, this._prebuiltTopIconTexture);
			if (loadedPhoto != null && loadedPhoto.portraitTexture == null)
			{
				this._prebuiltTopIconTexture = null;
			}
			int num = ((photoBoothPhotoItemSO != null) ? photoBoothPhotoItemSO.PhotoCount : 0);
			bool flag2 = photoBoothPhotoItemSO != null && (flag || photoBoothPhotoItemSO.PreloadSelected());
			if (photoBoothPhotoItemSO != null && (!flag2 || !this._inventory.AddItem(photoBoothPhotoItemSO)))
			{
				photoBoothPhotoItemSO.DisposeRuntimeAssets();
				num = 0;
			}
			if (num > 0)
			{
				base.StartCoroutine(this.FinishCollection(num));
				return;
			}
			if (loadedPhoto != null)
			{
				this.RefreshStack();
				this._collecting = false;
				return;
			}
			this._collecting = false;
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x0018FE97 File Offset: 0x0018E097
		private IEnumerator FinishCollection(int added)
		{
			yield return null;
			this.RefreshStack();
			this._collecting = false;
			if (added == 1)
			{
				ToastService.Show("1 Photo Booth print added to your inventory.", "Photo Booth", null, -1f, "photobooth_ui");
			}
			else
			{
				ToastService.ShowFormat("photobooth_ui", "{0} Photo Booth prints added to your inventory.", new object[] { added }, "Photo Booth", null, -1f);
			}
			yield break;
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x0018FEAD File Offset: 0x0018E0AD
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.OnInteract(player);
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x0018FEB8 File Offset: 0x0018E0B8
		public void RefreshStack()
		{
			if (this._refreshRoutine != null)
			{
				base.StopCoroutine(this._refreshRoutine);
				this._refreshRoutine = null;
			}
			this.ClearDisplay();
			if (!this._unlocked)
			{
				this._archiveCount = 0;
				this._archiveIds.Clear();
				this.BuildRuntimeConfig();
				return;
			}
			IReadOnlyList<string> archiveIds = PhotoBoothPhotoArchive.GetArchiveIds();
			this._archiveIds.Clear();
			for (int i = 0; i < archiveIds.Count; i++)
			{
				this._archiveIds.Add(archiveIds[i]);
			}
			List<string> list = new List<string>(this._archiveIds.Count);
			for (int j = 0; j < this._archiveIds.Count; j++)
			{
				if (!this.InventoryContains(this._archiveIds[j]) && !PhotoBoothHomePlacedPhotoPersistence.ContainsArchiveId(this._archiveIds[j]))
				{
					list.Add(this._archiveIds[j]);
				}
			}
			this._archiveCount = list.Count;
			this.BuildRuntimeConfig();
			if (this.printVisualPrefab == null || this.visualRoot == null || archiveIds.Count == 0)
			{
				return;
			}
			if (list.Count == 0)
			{
				return;
			}
			this._refreshRoutine = base.StartCoroutine(this.BuildDisplayStack(list));
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x0018FFEE File Offset: 0x0018E1EE
		private void HandleRankChanged()
		{
			this.ApplyRankUnlock(true);
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x0018FFF8 File Offset: 0x0018E1F8
		private void ApplyRankUnlock(bool refreshIfUnlocked)
		{
			bool flag = this.IsRankUnlocked();
			bool flag2 = flag != this._unlocked;
			this._unlocked = flag;
			PhotoBoothHomePlacedPhotoPersistence.SetFeatureUnlocked(flag);
			if (this.visualRoot != null && this.visualRoot.gameObject.activeSelf != flag)
			{
				this.visualRoot.gameObject.SetActive(flag);
			}
			if (!flag)
			{
				if (this._refreshRoutine != null)
				{
					base.StopCoroutine(this._refreshRoutine);
					this._refreshRoutine = null;
				}
				this.ClearDisplay();
				this.ResetHold();
				this.SetPromptVisible(false);
				this._candidateSqrDistance = float.MaxValue;
				this._inRange = false;
				return;
			}
			if (this._started && (flag2 || refreshIfUnlocked))
			{
				this.ResolvePlayer();
				this.RefreshStack();
				PhotoBoothHomePlacedPhotoPersistence.RestoreAll(this);
			}
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x001900BC File Offset: 0x0018E2BC
		private bool IsRankUnlocked()
		{
			if (RankProgressionService.IsAdminRankActive)
			{
				return true;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (config == null || config.ranksInOrder == null || string.IsNullOrEmpty(this.minimumRankId))
			{
				return false;
			}
			for (int i = 0; i < config.ranksInOrder.Count; i++)
			{
				RankDefinitionSO rankDefinitionSO = config.ranksInOrder[i];
				if (!(rankDefinitionSO == null) && !(rankDefinitionSO.rankId != this.minimumRankId))
				{
					return RankProgressionService.RankIndex >= i;
				}
			}
			return false;
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x00190144 File Offset: 0x0018E344
		private IEnumerator BuildDisplayStack(List<string> archiveIds)
		{
			int first = ((this.maximumVisiblePrints <= 0) ? 0 : Mathf.Max(0, archiveIds.Count - this.maximumVisiblePrints));
			int displayIndex = 0;
			int i = first;
			while (i < archiveIds.Count)
			{
				bool flag = i == archiveIds.Count - 1;
				PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto = (flag ? PhotoBoothPhotoArchive.LoadInventory(archiveIds[i], i + 1, false) : PhotoBoothPhotoArchive.LoadDisplay(archiveIds[i], i + 1, 512, true, true));
				if (loadedPhoto != null)
				{
					if (flag)
					{
						this._prebuiltTopIconTexture = PhotoBoothPhotoItemSO.BuildRuntimeIconTexture(loadedPhoto.portraitTexture, "Home Photo Stack");
					}
					GameObject gameObject = Object.Instantiate<GameObject>(this.printVisualPrefab, this.visualRoot);
					gameObject.name = "Photo_" + loadedPhoto.sequence.ToString("000");
					gameObject.SetActive(true);
					PhotoBoothPrint photoBoothPrint = gameObject.GetComponent<PhotoBoothPrint>();
					if (photoBoothPrint == null)
					{
						photoBoothPrint = gameObject.AddComponent<PhotoBoothPrint>();
					}
					photoBoothPrint.InitializeDisplayVisual(loadedPhoto.portraitTexture, loadedPhoto.printTexture, this.displayWorldScale, loadedPhoto.printNeeds180Rotation);
					Transform pivotTransform = photoBoothPrint.PivotTransform;
					pivotTransform.SetParent(this.visualRoot, true);
					float num = (float)displayIndex - (float)(archiveIds.Count - first - 1) * 0.5f;
					float num2 = ((displayIndex % 2 == 0) ? (-1f) : 1f);
					pivotTransform.localPosition = new Vector3(num2 * this.layerSlide * 0.45f, (float)displayIndex * this.layerHeight, num * this.layerSlide);
					pivotTransform.localRotation = Quaternion.Euler(0f, num2 * this.layerYaw, 0f);
					this._displayPivots.Add(pivotTransform.gameObject);
					this._displayPhotos.Add(loadedPhoto);
					if ((displayIndex + 1) % 4 == 0)
					{
						yield return null;
					}
				}
				int num3 = i;
				i = num3 + 1;
				num3 = displayIndex;
				displayIndex = num3 + 1;
			}
			this._refreshRoutine = null;
			yield break;
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x0019015C File Offset: 0x0018E35C
		private void ResolvePlayer()
		{
			PlayerInventoryManager playerInventoryManager = PlayerInventoryManager.Instance;
			vThirdPersonController vThirdPersonController = ((playerInventoryManager != null) ? playerInventoryManager.GetComponentInParent<vThirdPersonController>() : null);
			if (vThirdPersonController == null)
			{
				vThirdPersonController = Object.FindObjectOfType<vThirdPersonController>();
			}
			if (vThirdPersonController == null)
			{
				this._player = null;
				this._inventory = null;
				this._printHolder = null;
				return;
			}
			if (playerInventoryManager == null)
			{
				playerInventoryManager = vThirdPersonController.GetComponentInChildren<PlayerInventoryManager>(true);
			}
			this._player = vThirdPersonController;
			this._inventory = playerInventoryManager;
			this._printHolder = PhotoBoothPrintHolder.GetOrCreate(this._player);
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x001901E0 File Offset: 0x0018E3E0
		private bool HasMissingPhotos()
		{
			if (this._archiveCount <= 0 || this._inventory == null)
			{
				return false;
			}
			for (int i = 0; i < this._archiveIds.Count; i++)
			{
				if (!this.InventoryContains(this._archiveIds[i]) && !PhotoBoothHomePlacedPhotoPersistence.ContainsArchiveId(this._archiveIds[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x060042E5 RID: 17125 RVA: 0x00190246 File Offset: 0x0018E446
		internal GameObject PrintVisualPrefab
		{
			get
			{
				return this.printVisualPrefab;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x060042E6 RID: 17126 RVA: 0x0019024E File Offset: 0x0018E44E
		internal Transform PlayerTransform
		{
			get
			{
				if (!(this._player != null))
				{
					return null;
				}
				return this._player.transform;
			}
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x0019026B File Offset: 0x0018E46B
		internal PhotoBoothPhotoItemSO CreateArchiveItem(string archiveId, int sequence)
		{
			if (this.printVisualPrefab == null || string.IsNullOrEmpty(archiveId))
			{
				return null;
			}
			return PhotoBoothPhotoItemSO.CreateRuntimeArchiveReference(this.printVisualPrefab, this.inventoryWorldScale, Quaternion.Euler(this.inventoryWorldRestEuler), sequence, archiveId);
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x001902A4 File Offset: 0x0018E4A4
		private bool InventoryContains(string archiveId)
		{
			if (this._inventory == null || string.IsNullOrEmpty(archiveId))
			{
				return false;
			}
			for (int i = 0; i < this._inventory.MaxItemSlots; i++)
			{
				PhotoBoothPhotoItemSO photoBoothPhotoItemSO = this._inventory.GetItemAt(i) as PhotoBoothPhotoItemSO;
				if (photoBoothPhotoItemSO != null && photoBoothPhotoItemSO.ContainsArchiveId(archiveId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x00190300 File Offset: 0x0018E500
		private bool InventoryHasPhotoAlbum()
		{
			return this.FindPhotoAlbum() != null;
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x00190310 File Offset: 0x0018E510
		private PhotoBoothPhotoItemSO FindPhotoAlbum()
		{
			if (this._inventory == null)
			{
				return null;
			}
			for (int i = 0; i < this._inventory.MaxItemSlots; i++)
			{
				PhotoBoothPhotoItemSO photoBoothPhotoItemSO = this._inventory.GetItemAt(i) as PhotoBoothPhotoItemSO;
				if (photoBoothPhotoItemSO != null)
				{
					return photoBoothPhotoItemSO;
				}
			}
			return null;
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x0019035C File Offset: 0x0018E55C
		private void BuildRuntimeConfig()
		{
			if (this.interactionConfig == null)
			{
				return;
			}
			if (this._runtimeConfig == null)
			{
				this._runtimeConfig = Object.Instantiate<InteractionConfigSO>(this.interactionConfig);
				this._runtimeConfig.name = "Photo Booth Home Stack (Runtime)";
				this._runtimeConfig.hideFlags = HideFlags.DontSave;
			}
			this._runtimeConfig.objectName = ((this._archiveCount > 0) ? ("PHOTO BOOTH PHOTOS  x" + this._archiveCount.ToString()) : "PHOTO BOOTH PHOTOS");
			this._runtimeConfig.mainActionName = "(HOLD) TAKE PHOTOS";
			this._runtimeConfig.mainKeyDisplayLabel = "E";
			if (this._promptVisible && this.promptUI != null)
			{
				this.promptUI.Show(this._runtimeConfig, new Func<string>(this.LocalizedObjectName));
			}
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x00190438 File Offset: 0x0018E638
		private string LocalizedObjectName()
		{
			return ((this._archiveCount > 0) ? GameLocalization.FormatSource("interaction_objects", "PHOTO BOOTH PHOTOS  x{0}", new object[] { this._archiveCount }) : GameLocalization.GetSource("interaction_objects", "PHOTO BOOTH PHOTOS")).ToUpper();
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x00190488 File Offset: 0x0018E688
		private void SetPromptVisible(bool visible)
		{
			if (this.promptUI == null)
			{
				return;
			}
			if (this._promptVisible == visible && this.promptUI.gameObject.activeSelf == visible)
			{
				return;
			}
			this._promptVisible = visible;
			if (visible)
			{
				this.promptUI.gameObject.SetActive(true);
				this.promptUI.Show(this.Config, new Func<string>(this.LocalizedObjectName));
				return;
			}
			this.promptUI.gameObject.SetActive(false);
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x0019050B File Offset: 0x0018E70B
		private void ResetHold()
		{
			this._holding = false;
			this._holdTime = 0f;
			if (this._holdProgressUI != null)
			{
				this._holdProgressUI.Hide();
			}
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x00190538 File Offset: 0x0018E738
		private void ShowHoldProgress()
		{
			if (this._holdProgressUI == null)
			{
				if (this.holdProgressUiPrefab != null)
				{
					this._holdProgressUI = Object.Instantiate<TrashcanLootProgressUI>(this.holdProgressUiPrefab, base.transform);
				}
				else
				{
					this._holdProgressUI = TrashcanLootProgressUI.Create(base.transform, Vector3.zero);
				}
				Bounds bounds = default(Bounds);
				bool flag = false;
				foreach (Renderer renderer in (this.visualRoot != null) ? this.visualRoot.GetComponentsInChildren<Renderer>(true) : base.GetComponentsInChildren<Renderer>(true))
				{
					if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
					{
						if (!flag)
						{
							bounds = renderer.bounds;
							flag = true;
						}
						else
						{
							bounds.Encapsulate(renderer.bounds);
						}
					}
				}
				this._holdProgressUI.transform.position = (flag ? (bounds.center + Vector3.up * Mathf.Max(0.18f, bounds.extents.y + 0.12f)) : (base.transform.position + Vector3.up * 0.24f));
				PhotoBoothHomePhotoStack.SetWorldScale(this._holdProgressUI.transform, 0.0021f);
				this._holdProgressUI.gameObject.SetActive(false);
			}
			this._holdProgressUI.SetRunningText("PICKING UP");
			this._holdProgressUI.Show(1);
			this._holdProgressUI.SetProgress(0f);
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x001906BC File Offset: 0x0018E8BC
		private static void SetWorldScale(Transform target, float worldScale)
		{
			Vector3 vector = ((target.parent != null) ? target.parent.lossyScale : Vector3.one);
			target.localScale = new Vector3(worldScale / Mathf.Max(0.0001f, Mathf.Abs(vector.x)), worldScale / Mathf.Max(0.0001f, Mathf.Abs(vector.y)), worldScale / Mathf.Max(0.0001f, Mathf.Abs(vector.z)));
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x0019073C File Offset: 0x0018E93C
		private void ClearDisplay()
		{
			for (int i = 0; i < this._displayPivots.Count; i++)
			{
				if (this._displayPivots[i] != null)
				{
					Object.Destroy(this._displayPivots[i]);
				}
			}
			this._displayPivots.Clear();
			for (int j = 0; j < this._displayPhotos.Count; j++)
			{
				PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto = this._displayPhotos[j];
				if (loadedPhoto != null)
				{
					loadedPhoto.DestroyTextures();
				}
			}
			this._displayPhotos.Clear();
			if (this._prebuiltTopIconTexture != null)
			{
				Object.Destroy(this._prebuiltTopIconTexture);
			}
			this._prebuiltTopIconTexture = null;
		}

		// Token: 0x040042DB RID: 17115
		[Header("Interaction")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x040042DC RID: 17116
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x040042DD RID: 17117
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x040042DE RID: 17118
		[SerializeField]
		[Min(0.25f)]
		private float interactionRadius = 1.25f;

		// Token: 0x040042DF RID: 17119
		[SerializeField]
		[Min(0.1f)]
		private float holdDuration = 0.55f;

		// Token: 0x040042E0 RID: 17120
		[SerializeField]
		private TrashcanLootProgressUI holdProgressUiPrefab;

		// Token: 0x040042E1 RID: 17121
		[Header("Rank Unlock")]
		[Tooltip("Rank minimo che rende visibili e interagibili le foto sulla scrivania.")]
		[SerializeField]
		private string minimumRankId = "hornyslut_declared";

		// Token: 0x040042E2 RID: 17122
		[Header("Print Visual")]
		[SerializeField]
		private GameObject printVisualPrefab;

		// Token: 0x040042E3 RID: 17123
		[SerializeField]
		private Transform visualRoot;

		// Token: 0x040042E4 RID: 17124
		[SerializeField]
		private Vector3 displayWorldScale = new Vector3(1.5f, 1.5f, 1.5f);

		// Token: 0x040042E5 RID: 17125
		[SerializeField]
		private Vector3 inventoryWorldScale = new Vector3(1.5f, 1.5f, 1.5f);

		// Token: 0x040042E6 RID: 17126
		[SerializeField]
		private Vector3 inventoryWorldRestEuler;

		// Token: 0x040042E7 RID: 17127
		[Tooltip("Maximum desk sheets. Zero displays the complete persistent archive.")]
		[SerializeField]
		[Min(0f)]
		private int maximumVisiblePrints;

		// Token: 0x040042E8 RID: 17128
		[SerializeField]
		[Min(1E-05f)]
		private float layerHeight = 0.00016f;

		// Token: 0x040042E9 RID: 17129
		[SerializeField]
		[Min(0f)]
		private float layerSlide = 5E-05f;

		// Token: 0x040042EA RID: 17130
		[SerializeField]
		[Range(0f, 8f)]
		private float layerYaw = 0.5f;

		// Token: 0x040042EB RID: 17131
		private readonly List<GameObject> _displayPivots = new List<GameObject>(24);

		// Token: 0x040042EC RID: 17132
		private readonly List<PhotoBoothPhotoArchive.LoadedPhoto> _displayPhotos = new List<PhotoBoothPhotoArchive.LoadedPhoto>(24);

		// Token: 0x040042ED RID: 17133
		private readonly List<string> _archiveIds = new List<string>(32);

		// Token: 0x040042EE RID: 17134
		private Texture2D _prebuiltTopIconTexture;

		// Token: 0x040042EF RID: 17135
		private vThirdPersonController _player;

		// Token: 0x040042F0 RID: 17136
		private PlayerInventoryManager _inventory;

		// Token: 0x040042F1 RID: 17137
		private PhotoBoothPrintHolder _printHolder;

		// Token: 0x040042F2 RID: 17138
		private InteractionConfigSO _runtimeConfig;

		// Token: 0x040042F3 RID: 17139
		private float _candidateSqrDistance = float.MaxValue;

		// Token: 0x040042F4 RID: 17140
		private bool _inRange;

		// Token: 0x040042F5 RID: 17141
		private bool _promptVisible;

		// Token: 0x040042F6 RID: 17142
		private bool _holding;

		// Token: 0x040042F7 RID: 17143
		private bool _collecting;

		// Token: 0x040042F8 RID: 17144
		private float _holdTime;

		// Token: 0x040042F9 RID: 17145
		private int _archiveCount;

		// Token: 0x040042FA RID: 17146
		private Coroutine _refreshRoutine;

		// Token: 0x040042FB RID: 17147
		private bool _started;

		// Token: 0x040042FC RID: 17148
		private bool _unlocked;

		// Token: 0x040042FD RID: 17149
		private TrashcanLootProgressUI _holdProgressUI;
	}
}
