using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Features.PhoneCompanion;
using UnityEngine;

namespace Features.CielOS.VFS
{
	// Token: 0x020013AE RID: 5038
	public sealed class CielOSPhotosSync : MonoBehaviour
	{
		// Token: 0x14000119 RID: 281
		// (add) Token: 0x06009BF3 RID: 39923 RVA: 0x003FB230 File Offset: 0x003F9430
		// (remove) Token: 0x06009BF4 RID: 39924 RVA: 0x003FB264 File Offset: 0x003F9464
		public static event Action Changed;

		// Token: 0x06009BF5 RID: 39925 RVA: 0x003FB297 File Offset: 0x003F9497
		public static CielOSPhotosSync Ensure()
		{
			if (CielOSPhotosSync._instance == null)
			{
				GameObject gameObject = new GameObject("CielOSPhotosSync");
				CielOSPhotosSync._instance = gameObject.AddComponent<CielOSPhotosSync>();
				Object.DontDestroyOnLoad(gameObject);
			}
			return CielOSPhotosSync._instance;
		}

		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x06009BF6 RID: 39926 RVA: 0x003FB2C5 File Offset: 0x003F94C5
		private static string LedgerPath
		{
			get
			{
				string text = Path.Combine(Application.persistentDataPath, "CielOS_State");
				Directory.CreateDirectory(text);
				return Path.Combine(text, "phone-media-sync.json");
			}
		}

		// Token: 0x06009BF7 RID: 39927 RVA: 0x003FB2E7 File Offset: 0x003F94E7
		private void Awake()
		{
			if (CielOSPhotosSync._instance != null && CielOSPhotosSync._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			CielOSPhotosSync._instance = this;
			this.LoadLedger();
		}

		// Token: 0x06009BF8 RID: 39928 RVA: 0x003FB31B File Offset: 0x003F951B
		private void Start()
		{
			this.SyncOnce();
			base.StartCoroutine(this.RescanLoop());
		}

		// Token: 0x06009BF9 RID: 39929 RVA: 0x003FB331 File Offset: 0x003F9531
		private IEnumerator RescanLoop()
		{
			WaitForSecondsRealtime wait = new WaitForSecondsRealtime(4f);
			for (;;)
			{
				yield return wait;
				this.SyncOnce();
			}
			yield break;
		}

		// Token: 0x06009BFA RID: 39930 RVA: 0x003FB340 File Offset: 0x003F9540
		public static bool IsEligiblePhotoFileName(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return false;
			}
			string extension = Path.GetExtension(fileName);
			if (!extension.Equals(".png", StringComparison.OrdinalIgnoreCase) && !extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) && !extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			int num;
			return !fileNameWithoutExtension.EndsWith(".censored", StringComparison.OrdinalIgnoreCase) && !PhonePhotoStorage.IsExcludedFromGallery(fileName) && !int.TryParse(fileNameWithoutExtension, out num);
		}

		// Token: 0x06009BFB RID: 39931 RVA: 0x003FB3B8 File Offset: 0x003F95B8
		public int SyncOnce()
		{
			this.EnsureLedger();
			int num = 0;
			bool flag = false;
			try
			{
				CielOSFileSystem instance = CielOSFileSystem.Instance;
				string text = instance.ToReal("/Photos");
				string text2 = instance.ToReal("/Videos");
				Directory.CreateDirectory(text);
				Directory.CreateDirectory(text2);
				string imagesFolder = PhonePhotoStorage.ImagesFolder;
				if (Directory.Exists(imagesFolder))
				{
					foreach (string text3 in Directory.GetFiles(imagesFolder, "*.*", SearchOption.TopDirectoryOnly))
					{
						string fileName = Path.GetFileName(text3);
						if (CielOSPhotosSync.IsEligiblePhotoFileName(fileName) && !this._knownPhotos.Contains(fileName))
						{
							string text4 = Path.Combine(text, fileName);
							try
							{
								if (!File.Exists(text4))
								{
									File.Copy(text3, text4, false);
								}
								this._knownPhotos.Add(fileName);
								this._ledger.photos.Add(fileName);
								flag = true;
								num++;
								CielOSMediaLibrary.AppendToOrder("/Photos", instance.ToVirtual(text4));
							}
							catch (Exception ex)
							{
								Debug.LogWarning("[CielOSPhotosSync] Photo import failed for " + text3 + ": " + ex.Message);
							}
						}
					}
				}
				string videosFolder = PhoneVideoStorage.VideosFolder;
				if (Directory.Exists(videosFolder))
				{
					foreach (string text5 in Directory.GetFiles(videosFolder, "*.avi", SearchOption.TopDirectoryOnly))
					{
						string fileName2 = Path.GetFileName(text5);
						if (!this._knownVideos.Contains(fileName2))
						{
							string text6 = Path.Combine(text2, fileName2);
							try
							{
								CielOSPhotosSync.CopyIfMissing(text5, text6);
								CielOSPhotosSync.CopyIfMissing(PhoneVideoStorage.ThumbnailPathFor(text5), Path.ChangeExtension(text6, ".jpg"));
								CielOSPhotosSync.CopyIfMissing(PhoneVideoStorage.MetadataPathFor(text5), Path.ChangeExtension(text6, ".json"));
								this._knownVideos.Add(fileName2);
								this._ledger.videos.Add(fileName2);
								flag = true;
								num++;
								CielOSMediaLibrary.AppendToOrder("/Videos", instance.ToVirtual(text6));
							}
							catch (Exception ex2)
							{
								Debug.LogWarning("[CielOSPhotosSync] Video import failed for " + text5 + ": " + ex2.Message);
							}
						}
					}
				}
			}
			catch (Exception ex3)
			{
				Debug.LogWarning("[CielOSPhotosSync] Sync pass failed: " + ex3.Message);
			}
			if (flag)
			{
				this.SaveLedger();
			}
			if (num > 0)
			{
				Action changed = CielOSPhotosSync.Changed;
				if (changed != null)
				{
					changed();
				}
			}
			return num;
		}

		// Token: 0x06009BFC RID: 39932 RVA: 0x003FB660 File Offset: 0x003F9860
		private void EnsureLedger()
		{
			if (this._ledger == null)
			{
				this.LoadLedger();
			}
		}

		// Token: 0x06009BFD RID: 39933 RVA: 0x003FB670 File Offset: 0x003F9870
		private void LoadLedger()
		{
			bool flag = false;
			bool flag2 = false;
			try
			{
				string ledgerPath = CielOSPhotosSync.LedgerPath;
				flag = File.Exists(ledgerPath);
				this._ledger = (flag ? JsonUtility.FromJson<CielOSPhotosSync.SyncLedger>(File.ReadAllText(ledgerPath)) : null);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSPhotosSync] Ledger unreadable: " + ex.Message);
			}
			if (this._ledger == null)
			{
				this._ledger = new CielOSPhotosSync.SyncLedger();
				flag2 = true;
			}
			if (this._ledger.photos == null)
			{
				this._ledger.photos = new List<string>();
			}
			if (this._ledger.videos == null)
			{
				this._ledger.videos = new List<string>();
			}
			this._knownPhotos = new HashSet<string>(this._ledger.photos, StringComparer.OrdinalIgnoreCase);
			this._knownVideos = new HashSet<string>(this._ledger.videos, StringComparer.OrdinalIgnoreCase);
			if (!flag || flag2)
			{
				this.SeedKnownMedia();
				this.SaveLedger();
			}
		}

		// Token: 0x06009BFE RID: 39934 RVA: 0x003FB768 File Offset: 0x003F9968
		private void SeedKnownMedia()
		{
			CielOSFileSystem instance = CielOSFileSystem.Instance;
			string text = instance.ToReal("/Photos");
			string[] array;
			if (Directory.Exists(text))
			{
				array = Directory.GetFiles(text, "*.*", SearchOption.AllDirectories);
				for (int i = 0; i < array.Length; i++)
				{
					string fileName = Path.GetFileName(array[i]);
					if (CielOSPhotosSync.IsEligiblePhotoFileName(fileName) && this._knownPhotos.Add(fileName))
					{
						this._ledger.photos.Add(fileName);
					}
				}
			}
			string text2 = instance.ToReal("/Videos");
			if (!Directory.Exists(text2))
			{
				return;
			}
			array = Directory.GetFiles(text2, "*.*", SearchOption.AllDirectories);
			for (int i = 0; i < array.Length; i++)
			{
				string fileName2 = Path.GetFileName(array[i]);
				if (CielOSMediaLibrary.IsVideoFileName(fileName2) && this._knownVideos.Add(fileName2))
				{
					this._ledger.videos.Add(fileName2);
				}
			}
		}

		// Token: 0x06009BFF RID: 39935 RVA: 0x003FB84C File Offset: 0x003F9A4C
		private void SaveLedger()
		{
			try
			{
				File.WriteAllText(CielOSPhotosSync.LedgerPath, JsonUtility.ToJson(this._ledger, true));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSPhotosSync] Ledger save failed: " + ex.Message);
			}
		}

		// Token: 0x06009C00 RID: 39936 RVA: 0x003FB89C File Offset: 0x003F9A9C
		private static void CopyIfMissing(string source, string destination)
		{
			if (!File.Exists(source) || File.Exists(destination))
			{
				return;
			}
			File.Copy(source, destination, false);
		}

		// Token: 0x0400A087 RID: 41095
		public const string PhotosVirtualDir = "/Photos";

		// Token: 0x0400A088 RID: 41096
		public const string VideosVirtualDir = "/Videos";

		// Token: 0x0400A089 RID: 41097
		private const string CensoredSuffix = ".censored";

		// Token: 0x0400A08A RID: 41098
		private const string LedgerFolderName = "CielOS_State";

		// Token: 0x0400A08B RID: 41099
		private const string LedgerFileName = "phone-media-sync.json";

		// Token: 0x0400A08C RID: 41100
		private const float RescanInterval = 4f;

		// Token: 0x0400A08D RID: 41101
		private static CielOSPhotosSync _instance;

		// Token: 0x0400A08E RID: 41102
		private CielOSPhotosSync.SyncLedger _ledger;

		// Token: 0x0400A08F RID: 41103
		private HashSet<string> _knownPhotos;

		// Token: 0x0400A090 RID: 41104
		private HashSet<string> _knownVideos;

		// Token: 0x020013AF RID: 5039
		[Serializable]
		private sealed class SyncLedger
		{
			// Token: 0x0400A092 RID: 41106
			public List<string> photos = new List<string>();

			// Token: 0x0400A093 RID: 41107
			public List<string> videos = new List<string>();
		}
	}
}
