using System;
using System.Collections.Generic;
using System.IO;
using Features.Inventory.Placement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x0200097F RID: 2431
	[DisallowMultipleComponent]
	public sealed class PhotoBoothHomePlacedPhotoPersistence : MonoBehaviour, IPlacedItemPersistence
	{
		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x060042FF RID: 17151 RVA: 0x00190C1F File Offset: 0x0018EE1F
		private static string StorePath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "PhotoBoothHomePlacements.json");
			}
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x00190C30 File Offset: 0x0018EE30
		public static bool ContainsArchiveId(string archiveId)
		{
			if (string.IsNullOrEmpty(archiveId))
			{
				return false;
			}
			PhotoBoothHomePlacedPhotoPersistence.EnsureLoaded();
			for (int i = 0; i < PhotoBoothHomePlacedPhotoPersistence._store.placements.Count; i++)
			{
				if (string.Equals(PhotoBoothHomePlacedPhotoPersistence._store.placements[i].archiveId, archiveId, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x00190C88 File Offset: 0x0018EE88
		public static void RegisterNew(GameObject instance, PhotoBoothPhotoItemSO item)
		{
			if (instance == null || item == null || SceneManager.GetActiveScene().name != "CielHome" || string.IsNullOrEmpty(item.SelectedArchiveId))
			{
				return;
			}
			PhotoBoothHomePlacedPhotoPersistence.EnsureLoaded();
			string text = Guid.NewGuid().ToString("N");
			PhotoBoothPrint photoBoothPrint = instance.GetComponent<PhotoBoothPrint>();
			if (photoBoothPrint == null)
			{
				photoBoothPrint = instance.GetComponentInChildren<PhotoBoothPrint>(true);
			}
			Transform transform = ((photoBoothPrint != null) ? photoBoothPrint.PivotTransform : instance.transform);
			PhotoBoothHomePlacedPhotoPersistence._store.placements.Add(new PhotoBoothHomePlacedPhotoPersistence.PlacementRecord
			{
				placementId = text,
				archiveId = item.SelectedArchiveId,
				sequence = item.SelectedSequence,
				position = transform.position,
				rotation = transform.rotation
			});
			PhotoBoothHomePlacedPhotoPersistence.Save();
			PhotoBoothHomePlacedPhotoPersistence photoBoothHomePlacedPhotoPersistence = instance.GetComponent<PhotoBoothHomePlacedPhotoPersistence>();
			if (photoBoothHomePlacedPhotoPersistence == null)
			{
				photoBoothHomePlacedPhotoPersistence = instance.AddComponent<PhotoBoothHomePlacedPhotoPersistence>();
			}
			photoBoothHomePlacedPhotoPersistence.Initialize(text);
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x00190D88 File Offset: 0x0018EF88
		public static void RestoreAll(PhotoBoothHomePhotoStack source)
		{
			if (!PhotoBoothHomePlacedPhotoPersistence._featureUnlocked || source == null || source.PrintVisualPrefab == null || SceneManager.GetActiveScene().name != "CielHome")
			{
				return;
			}
			PhotoBoothHomePlacedPhotoPersistence.EnsureLoaded();
			IReadOnlyList<string> archiveIds = PhotoBoothPhotoArchive.GetArchiveIds();
			HashSet<string> available = new HashSet<string>(archiveIds, StringComparer.OrdinalIgnoreCase);
			if (PhotoBoothHomePlacedPhotoPersistence._store.placements.RemoveAll((PhotoBoothHomePlacedPhotoPersistence.PlacementRecord record) => record == null || string.IsNullOrEmpty(record.placementId) || string.IsNullOrEmpty(record.archiveId) || !available.Contains(record.archiveId)) > 0)
			{
				PhotoBoothHomePlacedPhotoPersistence.Save();
			}
			for (int i = 0; i < PhotoBoothHomePlacedPhotoPersistence._store.placements.Count; i++)
			{
				PhotoBoothHomePlacedPhotoPersistence.PlacementRecord placementRecord = PhotoBoothHomePlacedPhotoPersistence._store.placements[i];
				if (!(PhotoBoothHomePlacedPhotoPersistence.FindActive(placementRecord.placementId) != null))
				{
					PhotoBoothPhotoItemSO photoBoothPhotoItemSO = source.CreateArchiveItem(placementRecord.archiveId, placementRecord.sequence);
					if (photoBoothPhotoItemSO == null || !photoBoothPhotoItemSO.PreloadSelected())
					{
						if (photoBoothPhotoItemSO != null)
						{
							photoBoothPhotoItemSO.DisposeRuntimeAssets();
						}
					}
					else
					{
						GameObject gameObject = Object.Instantiate<GameObject>(source.PrintVisualPrefab, placementRecord.position, placementRecord.rotation);
						gameObject.name = "Placed_Photo Booth Photo_Wall (Persistent)";
						if (!photoBoothPhotoItemSO.ConfigureRestoredWorldInstance(gameObject))
						{
							Object.Destroy(gameObject);
							photoBoothPhotoItemSO.DisposeRuntimeAssets();
						}
						else
						{
							PlacedDildoInteractable placedDildoInteractable = gameObject.GetComponent<PlacedDildoInteractable>();
							if (placedDildoInteractable == null)
							{
								placedDildoInteractable = gameObject.AddComponent<PlacedDildoInteractable>();
							}
							placedDildoInteractable.Initialize(null, PlacementSurfaceType.Wall, null, null, photoBoothPhotoItemSO, null, source.PlayerTransform);
							gameObject.AddComponent<PhotoBoothHomePlacedPhotoPersistence>().Initialize(placementRecord.placementId);
						}
					}
				}
			}
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x00190F20 File Offset: 0x0018F120
		public static void SetFeatureUnlocked(bool unlocked)
		{
			PhotoBoothHomePlacedPhotoPersistence._featureUnlocked = unlocked;
			for (int i = PhotoBoothHomePlacedPhotoPersistence.Active.Count - 1; i >= 0; i--)
			{
				if (PhotoBoothHomePlacedPhotoPersistence.Active[i] == null)
				{
					PhotoBoothHomePlacedPhotoPersistence.Active.RemoveAt(i);
				}
				else
				{
					PhotoBoothHomePlacedPhotoPersistence.Active[i].ApplyVisibility(unlocked);
				}
			}
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x00190F7B File Offset: 0x0018F17B
		public void OnPlacedItemPickedUp()
		{
			if (string.IsNullOrEmpty(this._placementId))
			{
				return;
			}
			PhotoBoothHomePlacedPhotoPersistence.EnsureLoaded();
			if (PhotoBoothHomePlacedPhotoPersistence._store.placements.RemoveAll((PhotoBoothHomePlacedPhotoPersistence.PlacementRecord record) => record != null && record.placementId == this._placementId) > 0)
			{
				PhotoBoothHomePlacedPhotoPersistence.Save();
			}
			this._placementId = null;
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x00190FBA File Offset: 0x0018F1BA
		private void Initialize(string placementId)
		{
			this._placementId = placementId;
			this.CacheDefaults();
			this.ApplyVisibility(PhotoBoothHomePlacedPhotoPersistence._featureUnlocked);
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x00190FD4 File Offset: 0x0018F1D4
		private void Awake()
		{
			this.CacheDefaults();
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x00190FDC File Offset: 0x0018F1DC
		private void OnEnable()
		{
			if (!PhotoBoothHomePlacedPhotoPersistence.Active.Contains(this))
			{
				PhotoBoothHomePlacedPhotoPersistence.Active.Add(this);
			}
			this.ApplyVisibility(PhotoBoothHomePlacedPhotoPersistence._featureUnlocked);
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x00191001 File Offset: 0x0018F201
		private void OnDisable()
		{
			PhotoBoothHomePlacedPhotoPersistence.Active.Remove(this);
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x00191010 File Offset: 0x0018F210
		private void CacheDefaults()
		{
			if (this._renderers != null)
			{
				return;
			}
			this._renderers = base.GetComponentsInParent<Renderer>(true);
			this._colliders = base.GetComponentsInParent<Collider>(true);
			this._rendererDefaults = new bool[this._renderers.Length];
			this._colliderDefaults = new bool[this._colliders.Length];
			for (int i = 0; i < this._renderers.Length; i++)
			{
				this._rendererDefaults[i] = this._renderers[i] != null && this._renderers[i].enabled;
			}
			for (int j = 0; j < this._colliders.Length; j++)
			{
				this._colliderDefaults[j] = this._colliders[j] != null && this._colliders[j].enabled;
			}
			this._interactable = base.GetComponent<PlacedDildoInteractable>();
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x001910E8 File Offset: 0x0018F2E8
		private void ApplyVisibility(bool visible)
		{
			this.CacheDefaults();
			for (int i = 0; i < this._renderers.Length; i++)
			{
				if (this._renderers[i] != null)
				{
					this._renderers[i].enabled = visible && this._rendererDefaults[i];
				}
			}
			for (int j = 0; j < this._colliders.Length; j++)
			{
				if (this._colliders[j] != null)
				{
					this._colliders[j].enabled = visible && this._colliderDefaults[j];
				}
			}
			if (this._interactable != null)
			{
				this._interactable.enabled = visible;
			}
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x00191194 File Offset: 0x0018F394
		private static PhotoBoothHomePlacedPhotoPersistence FindActive(string placementId)
		{
			for (int i = PhotoBoothHomePlacedPhotoPersistence.Active.Count - 1; i >= 0; i--)
			{
				if (PhotoBoothHomePlacedPhotoPersistence.Active[i] == null)
				{
					PhotoBoothHomePlacedPhotoPersistence.Active.RemoveAt(i);
				}
				else if (PhotoBoothHomePlacedPhotoPersistence.Active[i]._placementId == placementId)
				{
					return PhotoBoothHomePlacedPhotoPersistence.Active[i];
				}
			}
			return null;
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x00191200 File Offset: 0x0018F400
		private static void EnsureLoaded()
		{
			if (PhotoBoothHomePlacedPhotoPersistence._store != null)
			{
				return;
			}
			PhotoBoothHomePlacedPhotoPersistence._store = new PhotoBoothHomePlacedPhotoPersistence.PlacementStore();
			try
			{
				if (File.Exists(PhotoBoothHomePlacedPhotoPersistence.StorePath))
				{
					PhotoBoothHomePlacedPhotoPersistence.PlacementStore placementStore = JsonUtility.FromJson<PhotoBoothHomePlacedPhotoPersistence.PlacementStore>(File.ReadAllText(PhotoBoothHomePlacedPhotoPersistence.StorePath));
					if (placementStore != null && placementStore.placements != null)
					{
						PhotoBoothHomePlacedPhotoPersistence._store = placementStore;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PhotoBoothHomePersistence] Cannot load placements: " + ex.Message);
			}
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x00191278 File Offset: 0x0018F478
		private static void Save()
		{
			PhotoBoothHomePlacedPhotoPersistence.EnsureLoaded();
			try
			{
				File.WriteAllText(PhotoBoothHomePlacedPhotoPersistence.StorePath, JsonUtility.ToJson(PhotoBoothHomePlacedPhotoPersistence._store, true));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PhotoBoothHomePersistence] Cannot save placements: " + ex.Message);
			}
		}

		// Token: 0x04004309 RID: 17161
		private const string HomeSceneName = "CielHome";

		// Token: 0x0400430A RID: 17162
		private const string FileName = "PhotoBoothHomePlacements.json";

		// Token: 0x0400430B RID: 17163
		private static readonly List<PhotoBoothHomePlacedPhotoPersistence> Active = new List<PhotoBoothHomePlacedPhotoPersistence>(32);

		// Token: 0x0400430C RID: 17164
		private static PhotoBoothHomePlacedPhotoPersistence.PlacementStore _store;

		// Token: 0x0400430D RID: 17165
		private static bool _featureUnlocked = true;

		// Token: 0x0400430E RID: 17166
		private string _placementId;

		// Token: 0x0400430F RID: 17167
		private Renderer[] _renderers;

		// Token: 0x04004310 RID: 17168
		private Collider[] _colliders;

		// Token: 0x04004311 RID: 17169
		private bool[] _rendererDefaults;

		// Token: 0x04004312 RID: 17170
		private bool[] _colliderDefaults;

		// Token: 0x04004313 RID: 17171
		private PlacedDildoInteractable _interactable;

		// Token: 0x02000980 RID: 2432
		[Serializable]
		private sealed class PlacementRecord
		{
			// Token: 0x04004314 RID: 17172
			public string placementId;

			// Token: 0x04004315 RID: 17173
			public string archiveId;

			// Token: 0x04004316 RID: 17174
			public int sequence;

			// Token: 0x04004317 RID: 17175
			public Vector3 position;

			// Token: 0x04004318 RID: 17176
			public Quaternion rotation;
		}

		// Token: 0x02000981 RID: 2433
		[Serializable]
		private sealed class PlacementStore
		{
			// Token: 0x04004319 RID: 17177
			public List<PhotoBoothHomePlacedPhotoPersistence.PlacementRecord> placements = new List<PhotoBoothHomePlacedPhotoPersistence.PlacementRecord>();
		}
	}
}
