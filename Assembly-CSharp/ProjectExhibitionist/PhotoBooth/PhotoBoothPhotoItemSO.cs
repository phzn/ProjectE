using System;
using System.Collections.Generic;
using Features.Inventory;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x0200098B RID: 2443
	public sealed class PhotoBoothPhotoItemSO : InventoryItemSO, IInventoryInstanceVisualProvider, IInventoryStackMergeable, IInventoryWorldInstanceProvider, IInventoryPlacementPoseProvider, IInventoryPlacementScaleProvider, IInventoryCenteredPlacementPreviewProvider, IInventoryPlacementPlayerVisibilityProvider, IInventoryPlacementSilhouetteProvider, IInventoryPlacementHeightProvider, IInventorySurfaceConformProvider, IInventoryWholeStackWorldProvider, IInventoryPlacementAreaProvider
	{
		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06004367 RID: 17255 RVA: 0x00193788 File Offset: 0x00191988
		public int PhotoCount
		{
			get
			{
				return this._photos.Count;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06004368 RID: 17256 RVA: 0x00193795 File Offset: 0x00191995
		public int InventoryStackQuantity
		{
			get
			{
				return Mathf.Max(1, this.PhotoCount);
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06004369 RID: 17257 RVA: 0x001937A3 File Offset: 0x001919A3
		public int SelectedIndex
		{
			get
			{
				return Mathf.Clamp(this._selectedIndex, 0, Mathf.Max(0, this._photos.Count - 1));
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x0600436A RID: 17258 RVA: 0x001937C4 File Offset: 0x001919C4
		public int SelectedDisplayNumber
		{
			get
			{
				if (this.PhotoCount <= 0)
				{
					return 0;
				}
				return this.SelectedIndex + 1;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x0600436B RID: 17259 RVA: 0x001937D9 File Offset: 0x001919D9
		public string SelectedArchiveId
		{
			get
			{
				if (this.Current == null)
				{
					return string.Empty;
				}
				return this.Current.archiveId;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x0600436C RID: 17260 RVA: 0x001937F4 File Offset: 0x001919F4
		public int SelectedSequence
		{
			get
			{
				if (this.Current == null)
				{
					return 0;
				}
				return this.Current.sequence;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x0600436D RID: 17261 RVA: 0x0019380B File Offset: 0x00191A0B
		public Texture2D PortraitTexture
		{
			get
			{
				this.EnsureCurrentLoaded();
				if (this.Current == null)
				{
					return null;
				}
				return this.Current.portraitTexture;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x0600436E RID: 17262 RVA: 0x00193829 File Offset: 0x00191A29
		public Texture2D PrintTexture
		{
			get
			{
				this.EnsureCurrentLoaded();
				if (this.Current == null)
				{
					return null;
				}
				return this.Current.printTexture;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x0600436F RID: 17263 RVA: 0x00193847 File Offset: 0x00191A47
		public bool PrintTextureRequires180Rotation
		{
			get
			{
				this.EnsureCurrentLoaded();
				return this.Current != null && this.Current.printTextureRequires180Rotation;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06004370 RID: 17264 RVA: 0x00193865 File Offset: 0x00191A65
		public Vector3 PlacementPreviewWorldScale
		{
			get
			{
				return this._visualWorldScale;
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06004371 RID: 17265 RVA: 0x0002AE8C File Offset: 0x0002908C
		public bool CenterPlacementPreviewOnVisual
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06004372 RID: 17266 RVA: 0x0002AE8C File Offset: 0x0002908C
		public bool HidePlayerVisualsDuringPlacement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06004373 RID: 17267 RVA: 0x0019386D File Offset: 0x00191A6D
		public Vector2 PlacementSilhouetteWorldSize
		{
			get
			{
				return new Vector2(0.1f * Mathf.Abs(this._visualWorldScale.z), 0.208863f * Mathf.Abs(this._visualWorldScale.x));
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06004374 RID: 17268 RVA: 0x0002AE8C File Offset: 0x0002908C
		public bool UseSurfaceHitHeight
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06004375 RID: 17269 RVA: 0x001938A0 File Offset: 0x00191AA0
		public float MinimumHeightAboveFloor
		{
			get
			{
				return Mathf.Max(0.06f, this.PlacementSilhouetteWorldSize.y * 0.52f);
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06004376 RID: 17270 RVA: 0x001938BD File Offset: 0x00191ABD
		public float MaximumHeightAboveFloor
		{
			get
			{
				return 4.5f;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06004377 RID: 17271 RVA: 0x001938C4 File Offset: 0x00191AC4
		private PhotoBoothPhotoItemSO.PhotoEntry Current
		{
			get
			{
				if (this._photos.Count <= 0)
				{
					return null;
				}
				return this._photos[this.SelectedIndex];
			}
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x001938E8 File Offset: 0x00191AE8
		public static PhotoBoothPhotoItemSO CreateRuntime(Texture2D portraitTexture, Texture2D printTexture, GameObject visualPrefab, Vector3 visualWorldScale, Quaternion worldRestRotation, int sequence, string archiveId = null)
		{
			if (portraitTexture == null || printTexture == null || visualPrefab == null)
			{
				return null;
			}
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = PhotoBoothPhotoItemSO.CreateConfiguredItem(visualPrefab, visualWorldScale, worldRestRotation, sequence);
			photoBoothPhotoItemSO._photos.Add(new PhotoBoothPhotoItemSO.PhotoEntry(portraitTexture, printTexture, sequence, archiveId, false));
			photoBoothPhotoItemSO._ownsRuntimeAssets = true;
			photoBoothPhotoItemSO.RebuildIcon();
			return photoBoothPhotoItemSO;
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x00193941 File Offset: 0x00191B41
		public static PhotoBoothPhotoItemSO CreateRuntimeArchiveReference(GameObject visualPrefab, Vector3 visualWorldScale, Quaternion worldRestRotation, int sequence, string archiveId)
		{
			if (visualPrefab == null || string.IsNullOrEmpty(archiveId))
			{
				return null;
			}
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = PhotoBoothPhotoItemSO.CreateConfiguredItem(visualPrefab, visualWorldScale, worldRestRotation, sequence);
			photoBoothPhotoItemSO._photos.Add(new PhotoBoothPhotoItemSO.PhotoEntry(null, null, sequence, archiveId, true));
			photoBoothPhotoItemSO._ownsRuntimeAssets = true;
			return photoBoothPhotoItemSO;
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x00193980 File Offset: 0x00191B80
		public static PhotoBoothPhotoItemSO CreateRuntimeArchiveAlbum(GameObject visualPrefab, Vector3 visualWorldScale, Quaternion worldRestRotation, IReadOnlyList<string> archiveIds, IReadOnlyList<int> sequences, PhotoBoothPhotoArchive.LoadedPhoto preloadedSelection = null, Texture2D prebuiltIconTexture = null)
		{
			if (visualPrefab == null || archiveIds == null || sequences == null || archiveIds.Count == 0 || archiveIds.Count != sequences.Count)
			{
				return null;
			}
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = PhotoBoothPhotoItemSO.CreateConfiguredItem(visualPrefab, visualWorldScale, worldRestRotation, sequences[0]);
			for (int i = 0; i < archiveIds.Count; i++)
			{
				if (!string.IsNullOrEmpty(archiveIds[i]))
				{
					PhotoBoothPhotoItemSO.PhotoEntry photoEntry = new PhotoBoothPhotoItemSO.PhotoEntry(null, null, sequences[i], archiveIds[i], true);
					if (preloadedSelection != null && string.Equals(preloadedSelection.archiveId, archiveIds[i], StringComparison.OrdinalIgnoreCase) && preloadedSelection.portraitTexture != null && preloadedSelection.printTexture != null)
					{
						photoEntry.portraitTexture = preloadedSelection.portraitTexture;
						photoEntry.printTexture = preloadedSelection.printTexture;
						photoEntry.printTextureRequires180Rotation = preloadedSelection.printNeeds180Rotation;
						photoBoothPhotoItemSO._selectedIndex = photoBoothPhotoItemSO._photos.Count;
						preloadedSelection.portraitTexture = null;
						preloadedSelection.printTexture = null;
					}
					photoBoothPhotoItemSO._photos.Add(photoEntry);
				}
			}
			if (photoBoothPhotoItemSO._photos.Count == 0)
			{
				Object.Destroy(photoBoothPhotoItemSO);
				return null;
			}
			photoBoothPhotoItemSO._ownsRuntimeAssets = true;
			if (prebuiltIconTexture != null && preloadedSelection != null && preloadedSelection.portraitTexture == null)
			{
				photoBoothPhotoItemSO.AdoptRuntimeIcon(prebuiltIconTexture);
			}
			return photoBoothPhotoItemSO;
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x00193AD5 File Offset: 0x00191CD5
		public bool PreloadSelected()
		{
			if (!this.EnsureCurrentLoaded())
			{
				return false;
			}
			if (this._runtimeIcon == null)
			{
				this.RebuildIcon();
			}
			this.MarkCurrentArchiveTextureNonReadable();
			return true;
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x00193AFC File Offset: 0x00191CFC
		private static PhotoBoothPhotoItemSO CreateConfiguredItem(GameObject visualPrefab, Vector3 visualWorldScale, Quaternion worldRestRotation, int sequence)
		{
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = ScriptableObject.CreateInstance<PhotoBoothPhotoItemSO>();
			photoBoothPhotoItemSO.name = "PhotoBooth Album " + sequence.ToString("00");
			photoBoothPhotoItemSO.hideFlags = HideFlags.DontSave;
			photoBoothPhotoItemSO.category = InventoryCategory.Item;
			photoBoothPhotoItemSO.rarity = ItemRarity.Common;
			photoBoothPhotoItemSO.maxRollableRarity = ItemRarity.Common;
			photoBoothPhotoItemSO.itemName = "Photo Booth Photos";
			photoBoothPhotoItemSO.itemId = "photo_booth_photo";
			photoBoothPhotoItemSO.description = "Your Photo Booth prints. Hold RMB to aim at a wall, then click LMB to hang the selected photo. Switch photos with R.";
			photoBoothPhotoItemSO.isStackable = true;
			photoBoothPhotoItemSO.maxStackSize = 999;
			visualPrefab = photoBoothPhotoItemSO.RetainVisualTemplate(visualPrefab);
			photoBoothPhotoItemSO.handPrefab = visualPrefab;
			photoBoothPhotoItemSO.floorPrefab = visualPrefab;
			photoBoothPhotoItemSO.wallPrefab = visualPrefab;
			photoBoothPhotoItemSO.attachBone = HumanBodyBones.RightHand;
			photoBoothPhotoItemSO.handPositionOffset = Vector3.zero;
			photoBoothPhotoItemSO.handRotationOffset = Vector3.zero;
			photoBoothPhotoItemSO.holdAnimationParameter = "HoldingPhotoPrint";
			photoBoothPhotoItemSO.holdLayerWeight = 0f;
			photoBoothPhotoItemSO.secondaryHoldLayerName = "PosterHold";
			photoBoothPhotoItemSO.secondaryHoldLayerWeight = 1f;
			photoBoothPhotoItemSO.isPlaceable = true;
			photoBoothPhotoItemSO.canBePickedUpAfterPlacement = true;
			photoBoothPhotoItemSO.supportsPlacedActions = false;
			photoBoothPhotoItemSO.hasDurability = false;
			photoBoothPhotoItemSO._visualPrefab = visualPrefab;
			photoBoothPhotoItemSO._visualWorldScale = visualWorldScale;
			photoBoothPhotoItemSO._worldRestRotation = worldRestRotation;
			return photoBoothPhotoItemSO;
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x00193C14 File Offset: 0x00191E14
		private GameObject RetainVisualTemplate(GameObject source)
		{
			if (!source.scene.IsValid())
			{
				return source;
			}
			this._ownedVisualTemplate = Object.Instantiate<GameObject>(source);
			this._ownedVisualTemplate.name = "PhotoBooth Inventory Print Template";
			this._ownedVisualTemplate.SetActive(false);
			this._ownedVisualTemplate.transform.SetParent(null, false);
			this._ownedVisualTemplate.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(this._ownedVisualTemplate);
			}
			return this._ownedVisualTemplate;
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x00193C9F File Offset: 0x00191E9F
		private void OnDestroy()
		{
			if (this._ownedVisualTemplate == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._ownedVisualTemplate);
			}
			else
			{
				Object.DestroyImmediate(this._ownedVisualTemplate);
			}
			this._ownedVisualTemplate = null;
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x00193CD8 File Offset: 0x00191ED8
		public bool SelectRelative(int delta)
		{
			if (this._photos.Count < 2 || delta == 0)
			{
				return false;
			}
			int num = this.WrapIndex(this._selectedIndex + delta);
			if (!this.EnsureEntryLoaded(this._photos[num], false))
			{
				return false;
			}
			this._selectedIndex = num;
			return true;
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x00193D26 File Offset: 0x00191F26
		public bool PreloadRelative(int delta)
		{
			return this._photos.Count != 0 && this.EnsureEntryLoaded(this._photos[this.WrapIndex(this._selectedIndex + delta)], true);
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x00193D58 File Offset: 0x00191F58
		public void TrimArchiveTextureCache(int keepBehind, int keepAhead)
		{
			if (this._photos.Count == 0)
			{
				return;
			}
			keepBehind = Mathf.Max(0, keepBehind);
			keepAhead = Mathf.Max(0, keepAhead);
			for (int i = 0; i < this._photos.Count; i++)
			{
				int num = (i - this.SelectedIndex + this._photos.Count) % this._photos.Count;
				int num2 = (this.SelectedIndex - i + this._photos.Count) % this._photos.Count;
				if (num > keepAhead && num2 > keepBehind)
				{
					this.ReleaseArchiveTextures(this._photos[i]);
				}
			}
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x00193DF8 File Offset: 0x00191FF8
		public bool ContainsArchiveId(string archiveId)
		{
			if (string.IsNullOrEmpty(archiveId))
			{
				return false;
			}
			for (int i = 0; i < this._photos.Count; i++)
			{
				if (string.Equals(this._photos[i].archiveId, archiveId, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x00193E44 File Offset: 0x00192044
		public bool DestroySelectedPhoto()
		{
			if (this._photos.Count == 0)
			{
				return false;
			}
			int selectedIndex = this.SelectedIndex;
			PhotoBoothPhotoItemSO.PhotoEntry photoEntry = this._photos[selectedIndex];
			this._photos.RemoveAt(selectedIndex);
			if (photoEntry != null && !string.IsNullOrEmpty(photoEntry.archiveId))
			{
				PhotoBoothPhotoArchive.Delete(photoEntry.archiveId);
			}
			if (this._ownsRuntimeAssets && photoEntry != null)
			{
				if (photoEntry.portraitTexture != null)
				{
					Object.Destroy(photoEntry.portraitTexture);
				}
				if (photoEntry.printTexture != null && photoEntry.printTexture != photoEntry.portraitTexture)
				{
					Object.Destroy(photoEntry.printTexture);
				}
			}
			this._selectedIndex = ((this._photos.Count > 0) ? Mathf.Clamp(selectedIndex, 0, this._photos.Count - 1) : 0);
			this.RebuildIcon();
			return true;
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x00193F1E File Offset: 0x0019211E
		public void ConfigureHandInstance(GameObject instance)
		{
			this.ConfigureVisual(instance, true, false);
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x00193F29 File Offset: 0x00192129
		public void ConfigureWorldInstance(GameObject instance)
		{
			this.ConfigureVisual(instance, false, false);
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x00193F34 File Offset: 0x00192134
		private void ConfigureVisual(GameObject instance, bool held, bool preserveSpawnPose)
		{
			if (instance == null || this.PrintTexture == null)
			{
				return;
			}
			Vector3 position = instance.transform.position;
			Quaternion rotation = instance.transform.rotation;
			PhotoBoothPrint photoBoothPrint = instance.GetComponent<PhotoBoothPrint>();
			if (photoBoothPrint == null)
			{
				photoBoothPrint = instance.AddComponent<PhotoBoothPrint>();
			}
			photoBoothPrint.InitializeInventoryVisual(this, this._visualWorldScale);
			if (held)
			{
				return;
			}
			Quaternion quaternion = (preserveSpawnPose ? rotation : this._worldRestRotation);
			Vector3 vector = (preserveSpawnPose ? position : photoBoothPrint.VisualCenter);
			photoBoothPrint.SetVisualWorldPose(vector, quaternion);
			photoBoothPrint.ConfigureAsWorldObject();
			photoBoothPrint.enabled = false;
			instance.SetActive(true);
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x00193FD4 File Offset: 0x001921D4
		public bool CanMergeWith(InventoryItemSO incoming)
		{
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = incoming as PhotoBoothPhotoItemSO;
			return photoBoothPhotoItemSO != null && photoBoothPhotoItemSO != this && photoBoothPhotoItemSO.PhotoCount > 0 && this.PhotoCount + photoBoothPhotoItemSO.PhotoCount <= this.maxStackSize;
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x00194018 File Offset: 0x00192218
		public bool MergeFrom(InventoryItemSO incoming)
		{
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = incoming as PhotoBoothPhotoItemSO;
			if (photoBoothPhotoItemSO == null || !this.CanMergeWith(photoBoothPhotoItemSO))
			{
				return false;
			}
			List<PhotoBoothPhotoItemSO.PhotoEntry> list = new List<PhotoBoothPhotoItemSO.PhotoEntry>(photoBoothPhotoItemSO._photos);
			photoBoothPhotoItemSO._photos.Clear();
			photoBoothPhotoItemSO._ownsRuntimeAssets = false;
			photoBoothPhotoItemSO.DestroyIcon();
			this._photos.AddRange(list);
			Object.Destroy(photoBoothPhotoItemSO);
			if (this._runtimeIcon == null)
			{
				this.RebuildIcon();
			}
			return true;
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x00194088 File Offset: 0x00192288
		public InventoryItemSO PrepareWholeStackWorldInstance(GameObject instance)
		{
			if (instance == null || this.PhotoCount == 0 || !this.EnsureCurrentLoaded())
			{
				return null;
			}
			this.ConfigureVisual(instance, false, true);
			PhotoBoothPrint photoBoothPrint = instance.GetComponent<PhotoBoothPrint>();
			if (photoBoothPrint == null)
			{
				photoBoothPrint = instance.GetComponentInChildren<PhotoBoothPrint>(true);
			}
			if (photoBoothPrint == null)
			{
				return null;
			}
			PhotoBoothDroppedPhotoStackVisual photoBoothDroppedPhotoStackVisual = instance.GetComponent<PhotoBoothDroppedPhotoStackVisual>();
			if (photoBoothDroppedPhotoStackVisual == null)
			{
				photoBoothDroppedPhotoStackVisual = instance.AddComponent<PhotoBoothDroppedPhotoStackVisual>();
			}
			Transform pivotTransform = photoBoothPrint.PivotTransform;
			int num = 0;
			for (int i = 0; i < this._photos.Count; i++)
			{
				if (i != this.SelectedIndex)
				{
					PhotoBoothPhotoItemSO.PhotoEntry photoEntry = this._photos[i];
					PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto = null;
					Texture2D texture2D = photoEntry.portraitTexture;
					Texture2D texture2D2 = photoEntry.printTexture;
					bool flag = photoEntry.printTextureRequires180Rotation;
					if (!string.IsNullOrEmpty(photoEntry.archiveId))
					{
						loadedPhoto = PhotoBoothPhotoArchive.LoadDisplay(photoEntry.archiveId, photoEntry.sequence, 384, true, true);
						if (loadedPhoto != null)
						{
							texture2D = loadedPhoto.portraitTexture;
							texture2D2 = loadedPhoto.printTexture;
							flag = loadedPhoto.printNeeds180Rotation;
						}
					}
					if (!(texture2D2 == null))
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this._visualPrefab, pivotTransform);
						gameObject.name = "PhotoStack_Sheet_" + photoEntry.sequence.ToString("000");
						PhotoBoothPrint photoBoothPrint2 = gameObject.GetComponent<PhotoBoothPrint>();
						if (photoBoothPrint2 == null)
						{
							photoBoothPrint2 = gameObject.AddComponent<PhotoBoothPrint>();
						}
						photoBoothPrint2.InitializeDisplayVisual(texture2D, texture2D2, this._visualWorldScale, flag);
						Transform pivotTransform2 = photoBoothPrint2.PivotTransform;
						pivotTransform2.SetParent(pivotTransform, true);
						num++;
						float num2 = ((num % 2 == 0) ? (-1f) : 1f);
						pivotTransform2.localPosition = new Vector3(num2 * 8E-05f, (float)(-(float)num) * 0.00016f, (float)num * 4E-05f);
						pivotTransform2.localRotation = Quaternion.Euler(0f, num2 * 0.65f, 0f);
						Collider[] componentsInChildren = pivotTransform2.GetComponentsInChildren<Collider>(true);
						for (int j = 0; j < componentsInChildren.Length; j++)
						{
							componentsInChildren[j].enabled = false;
						}
						photoBoothDroppedPhotoStackVisual.TrackDisplay(loadedPhoto);
					}
				}
			}
			instance.name = "Photo Booth Photos x" + this.PhotoCount.ToString();
			return this;
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x001942C0 File Offset: 0x001924C0
		internal bool ConfigureRestoredWorldInstance(GameObject instance)
		{
			if (instance == null || this.PhotoCount != 1 || !this.EnsureCurrentLoaded())
			{
				return false;
			}
			this.ConfigureVisual(instance, false, true);
			return true;
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x001942E8 File Offset: 0x001924E8
		public InventoryItemSO PrepareWorldInstance(GameObject instance)
		{
			if (!this.EnsureCurrentLoaded())
			{
				return null;
			}
			PhotoBoothPhotoItemSO.PhotoEntry photoEntry = this.Current;
			if (photoEntry == null || instance == null)
			{
				return null;
			}
			this.ClearPreparedWorldItem();
			this._preparedEntry = photoEntry;
			this._preparedWorldItem = PhotoBoothPhotoItemSO.CreateConfiguredItem(this._visualPrefab, this._visualWorldScale, this._worldRestRotation, photoEntry.sequence);
			this._preparedWorldItem._photos.Add(photoEntry);
			this._preparedWorldItem._ownsRuntimeAssets = false;
			this._preparedWorldItem.RebuildIcon();
			this._preparedWorldItem.ConfigureVisual(instance, false, true);
			return this._preparedWorldItem;
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x00194380 File Offset: 0x00192580
		public Vector3 AdjustPlacementPosition(Vector3 surfacePosition, Vector3 surfaceNormal, bool wall)
		{
			return surfacePosition + surfaceNormal.normalized * 0.0015f;
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0019439C File Offset: 0x0019259C
		public Quaternion AdjustPlacementRotation(Quaternion surfaceRotation, bool wall)
		{
			if (!wall)
			{
				return surfaceRotation;
			}
			return surfaceRotation * Quaternion.AngleAxis(90f, Vector3.right) * Quaternion.AngleAxis(90f, Vector3.up) * Quaternion.AngleAxis(180f, Vector3.up);
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x001943EC File Offset: 0x001925EC
		public void ConformWorldInstanceToSurface(GameObject instance, Vector3 surfacePosition, Vector3 surfaceNormal, bool wall)
		{
			if (instance == null)
			{
				return;
			}
			PhotoBoothPrint photoBoothPrint = instance.GetComponent<PhotoBoothPrint>();
			if (photoBoothPrint == null)
			{
				photoBoothPrint = instance.GetComponentInChildren<PhotoBoothPrint>(true);
			}
			if (photoBoothPrint != null)
			{
				photoBoothPrint.SnapVisibleGeometryToSurface(surfacePosition, surfaceNormal, 0.0015f);
			}
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0002AE8C File Offset: 0x0002908C
		public bool IsPlacementAreaValid(Vector3 surfacePosition, Vector3 surfaceNormal, bool wall, Transform player)
		{
			return true;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0019442C File Offset: 0x0019262C
		public void CommitWorldInstance()
		{
			if (this._preparedWorldItem == null || this._preparedEntry == null)
			{
				return;
			}
			int num = this._photos.IndexOf(this._preparedEntry);
			if (num >= 0)
			{
				this._photos.RemoveAt(num);
				this._preparedWorldItem._ownsRuntimeAssets = true;
				this._selectedIndex = ((this._photos.Count > 0) ? Mathf.Clamp(num, 0, this._photos.Count - 1) : 0);
				this.RebuildIcon();
			}
			this._preparedWorldItem = null;
			this._preparedEntry = null;
			if (this._photos.Count == 0)
			{
				this._ownsRuntimeAssets = false;
				Object.Destroy(this);
			}
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x001944D8 File Offset: 0x001926D8
		public void DisposeRuntimeAssets()
		{
			this.ClearPreparedWorldItem();
			if (!this._ownsRuntimeAssets)
			{
				return;
			}
			this._ownsRuntimeAssets = false;
			this.DestroyIcon();
			for (int i = 0; i < this._photos.Count; i++)
			{
				PhotoBoothPhotoItemSO.PhotoEntry photoEntry = this._photos[i];
				if (photoEntry.portraitTexture != null)
				{
					Object.Destroy(photoEntry.portraitTexture);
				}
				if (photoEntry.printTexture != null && photoEntry.printTexture != photoEntry.portraitTexture)
				{
					Object.Destroy(photoEntry.printTexture);
				}
			}
			this._photos.Clear();
			Object.Destroy(this);
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0019457A File Offset: 0x0019277A
		public void AbandonWithoutDestroyingTextures()
		{
			this._ownsRuntimeAssets = false;
			this.DestroyIcon();
			this._photos.Clear();
			Object.Destroy(this);
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x0019459C File Offset: 0x0019279C
		private void RebuildIcon()
		{
			this.DestroyIcon();
			Texture2D texture2D = this.PortraitTexture;
			if (texture2D == null)
			{
				return;
			}
			PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto = null;
			if (!texture2D.isReadable && this.Current != null && !string.IsNullOrEmpty(this.Current.archiveId))
			{
				loadedPhoto = PhotoBoothPhotoArchive.LoadDisplay(this.Current.archiveId, this.Current.sequence, 384, false, false);
				texture2D = ((loadedPhoto != null) ? loadedPhoto.portraitTexture : null);
			}
			if (texture2D == null)
			{
				if (loadedPhoto != null)
				{
					loadedPhoto.DestroyTextures();
				}
				return;
			}
			this._runtimeIconTexture = PhotoBoothPhotoItemSO.BuildRuntimeIconTexture(texture2D, base.name);
			if (loadedPhoto != null)
			{
				loadedPhoto.DestroyTextures();
			}
			if (this._runtimeIconTexture == null)
			{
				return;
			}
			this.CreateRuntimeIconSprite();
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x00194658 File Offset: 0x00192858
		internal static Texture2D BuildRuntimeIconTexture(Texture2D portrait, string ownerName)
		{
			if (portrait == null || !portrait.isReadable)
			{
				return null;
			}
			Texture2D texture2D = new Texture2D(192, 192, TextureFormat.RGBA32, false)
			{
				name = ownerName + " Icon Texture",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				hideFlags = HideFlags.DontSave
			};
			Color32[] array = new Color32[36864];
			float num = (float)portrait.width / (float)Mathf.Max(1, portrait.height);
			int num2 = Mathf.RoundToInt(115.200005f);
			int num3 = Mathf.Max(1, Mathf.RoundToInt((float)num2 * num));
			int num4 = Mathf.RoundToInt(115.200005f);
			if (num3 > num4)
			{
				num3 = num4;
				num2 = Mathf.Max(1, Mathf.RoundToInt((float)num3 / Mathf.Max(0.001f, num)));
			}
			int num5 = (192 - num3) / 2;
			int num6 = (192 - num2) / 2;
			Color32[] pixels = portrait.GetPixels32();
			int width = portrait.width;
			int height = portrait.height;
			for (int i = 0; i < num2; i++)
			{
				int num7 = Mathf.Clamp(Mathf.FloorToInt(((float)i + 0.5f) * (float)height / (float)num2), 0, height - 1) * width;
				int num8 = (num6 + i) * 192 + num5;
				for (int j = 0; j < num3; j++)
				{
					int num9 = Mathf.Clamp(Mathf.FloorToInt(((float)j + 0.5f) * (float)width / (float)num3), 0, width - 1);
					array[num8 + j] = pixels[num7 + num9];
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x001947F2 File Offset: 0x001929F2
		private void AdoptRuntimeIcon(Texture2D iconTexture)
		{
			if (iconTexture == null)
			{
				return;
			}
			this.DestroyIcon();
			this._runtimeIconTexture = iconTexture;
			this.CreateRuntimeIconSprite();
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x00194814 File Offset: 0x00192A14
		private void CreateRuntimeIconSprite()
		{
			if (this._runtimeIconTexture == null)
			{
				return;
			}
			this._runtimeIcon = Sprite.Create(this._runtimeIconTexture, new Rect(0f, 0f, (float)this._runtimeIconTexture.width, (float)this._runtimeIconTexture.height), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
			this._runtimeIcon.name = base.name + " Icon";
			this.icon = this._runtimeIcon;
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x001948A8 File Offset: 0x00192AA8
		private bool EnsureCurrentLoaded()
		{
			PhotoBoothPhotoItemSO.PhotoEntry photoEntry = this.Current;
			return this.EnsureEntryLoaded(photoEntry, false);
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x001948C4 File Offset: 0x00192AC4
		private bool EnsureEntryLoaded(PhotoBoothPhotoItemSO.PhotoEntry entry, bool markNonReadable)
		{
			if (entry == null)
			{
				return false;
			}
			if (entry.portraitTexture != null && entry.printTexture != null)
			{
				return true;
			}
			if (string.IsNullOrEmpty(entry.archiveId))
			{
				return false;
			}
			PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto = PhotoBoothPhotoArchive.LoadInventory(entry.archiveId, entry.sequence, markNonReadable);
			if (loadedPhoto == null)
			{
				return false;
			}
			entry.portraitTexture = loadedPhoto.portraitTexture;
			entry.printTexture = loadedPhoto.printTexture;
			entry.printTextureRequires180Rotation = loadedPhoto.printNeeds180Rotation;
			loadedPhoto.portraitTexture = null;
			loadedPhoto.printTexture = null;
			return true;
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x00194950 File Offset: 0x00192B50
		private int WrapIndex(int index)
		{
			int count = this._photos.Count;
			if (count <= 0)
			{
				return 0;
			}
			index %= count;
			if (index >= 0)
			{
				return index;
			}
			return index + count;
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x00194980 File Offset: 0x00192B80
		private void MarkCurrentArchiveTextureNonReadable()
		{
			PhotoBoothPhotoItemSO.PhotoEntry photoEntry = this.Current;
			if (photoEntry == null || string.IsNullOrEmpty(photoEntry.archiveId) || photoEntry.portraitTexture == null || photoEntry.portraitTexture != photoEntry.printTexture || !photoEntry.portraitTexture.isReadable)
			{
				return;
			}
			photoEntry.portraitTexture.Apply(false, true);
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x001949E0 File Offset: 0x00192BE0
		private void ReleaseArchiveTextures(PhotoBoothPhotoItemSO.PhotoEntry entry)
		{
			if (!this._ownsRuntimeAssets || entry == null || string.IsNullOrEmpty(entry.archiveId))
			{
				return;
			}
			if (entry.portraitTexture != null)
			{
				Object.Destroy(entry.portraitTexture);
			}
			if (entry.printTexture != null && entry.printTexture != entry.portraitTexture)
			{
				Object.Destroy(entry.printTexture);
			}
			entry.portraitTexture = null;
			entry.printTexture = null;
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x00194A5C File Offset: 0x00192C5C
		private void DestroyIcon()
		{
			if (this._runtimeIcon != null)
			{
				Object.Destroy(this._runtimeIcon);
			}
			if (this._runtimeIconTexture != null)
			{
				Object.Destroy(this._runtimeIconTexture);
			}
			this._runtimeIcon = null;
			this._runtimeIconTexture = null;
			this.icon = null;
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x00194AB0 File Offset: 0x00192CB0
		private void ClearPreparedWorldItem()
		{
			if (this._preparedWorldItem != null)
			{
				this._preparedWorldItem.AbandonWithoutDestroyingTextures();
			}
			this._preparedWorldItem = null;
			this._preparedEntry = null;
		}

		// Token: 0x0400435D RID: 17245
		private readonly List<PhotoBoothPhotoItemSO.PhotoEntry> _photos = new List<PhotoBoothPhotoItemSO.PhotoEntry>(8);

		// Token: 0x0400435E RID: 17246
		private Sprite _runtimeIcon;

		// Token: 0x0400435F RID: 17247
		private Texture2D _runtimeIconTexture;

		// Token: 0x04004360 RID: 17248
		private Vector3 _visualWorldScale = Vector3.one;

		// Token: 0x04004361 RID: 17249
		private Quaternion _worldRestRotation = Quaternion.identity;

		// Token: 0x04004362 RID: 17250
		private GameObject _visualPrefab;

		// Token: 0x04004363 RID: 17251
		private GameObject _ownedVisualTemplate;

		// Token: 0x04004364 RID: 17252
		private bool _ownsRuntimeAssets;

		// Token: 0x04004365 RID: 17253
		private int _selectedIndex;

		// Token: 0x04004366 RID: 17254
		private PhotoBoothPhotoItemSO _preparedWorldItem;

		// Token: 0x04004367 RID: 17255
		private PhotoBoothPhotoItemSO.PhotoEntry _preparedEntry;

		// Token: 0x0200098C RID: 2444
		private sealed class PhotoEntry
		{
			// Token: 0x0600439F RID: 17311 RVA: 0x00194B03 File Offset: 0x00192D03
			public PhotoEntry(Texture2D portrait, Texture2D print, int photoSequence, string persistentArchiveId, bool requires180Rotation)
			{
				this.portraitTexture = portrait;
				this.printTexture = print;
				this.printTextureRequires180Rotation = requires180Rotation;
				this.sequence = photoSequence;
				this.archiveId = persistentArchiveId;
			}

			// Token: 0x04004368 RID: 17256
			public Texture2D portraitTexture;

			// Token: 0x04004369 RID: 17257
			public Texture2D printTexture;

			// Token: 0x0400436A RID: 17258
			public bool printTextureRequires180Rotation;

			// Token: 0x0400436B RID: 17259
			public int sequence;

			// Token: 0x0400436C RID: 17260
			public string archiveId;
		}
	}
}
