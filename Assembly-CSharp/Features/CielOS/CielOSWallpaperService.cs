using System;
using System.Collections.Generic;
using System.IO;
using Features.CensorBars;
using Features.CielOS.VFS;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x02001395 RID: 5013
	public class CielOSWallpaperService : MonoBehaviour
	{
		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x06009B1F RID: 39711 RVA: 0x003F722B File Offset: 0x003F542B
		// (set) Token: 0x06009B20 RID: 39712 RVA: 0x003F7232 File Offset: 0x003F5432
		public static CielOSWallpaperService Instance { get; private set; }

		// Token: 0x14000114 RID: 276
		// (add) Token: 0x06009B21 RID: 39713 RVA: 0x003F723C File Offset: 0x003F543C
		// (remove) Token: 0x06009B22 RID: 39714 RVA: 0x003F7270 File Offset: 0x003F5470
		public static event Action ScreensaverChanged;

		// Token: 0x06009B23 RID: 39715 RVA: 0x003F72A3 File Offset: 0x003F54A3
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticEvents()
		{
			CielOSWallpaperService.ScreensaverChanged = null;
		}

		// Token: 0x170018B8 RID: 6328
		// (get) Token: 0x06009B24 RID: 39716 RVA: 0x003F72AB File Offset: 0x003F54AB
		private static string WallpaperCachePath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "CielOS_State", "wallpaper-image.cache");
			}
		}

		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x06009B25 RID: 39717 RVA: 0x003F72C1 File Offset: 0x003F54C1
		public string ScreensaverPath
		{
			get
			{
				return this.GetScreensaverPath(0);
			}
		}

		// Token: 0x06009B26 RID: 39718 RVA: 0x003F72CA File Offset: 0x003F54CA
		private void Awake()
		{
			if (CielOSWallpaperService.Instance != null && CielOSWallpaperService.Instance != this)
			{
				Object.Destroy(this);
				return;
			}
			CielOSWallpaperService.Instance = this;
		}

		// Token: 0x06009B27 RID: 39719 RVA: 0x003F72F3 File Offset: 0x003F54F3
		private void OnDestroy()
		{
			if (CielOSWallpaperService.Instance == this)
			{
				CielOSWallpaperService.Instance = null;
			}
			this.ReleaseLoadedWallpaper();
		}

		// Token: 0x06009B28 RID: 39720 RVA: 0x003F730E File Offset: 0x003F550E
		private void Start()
		{
			this.RestoreWallpaper();
		}

		// Token: 0x06009B29 RID: 39721 RVA: 0x003F7318 File Offset: 0x003F5518
		public bool RestoreWallpaper()
		{
			string @string = PlayerPrefs.GetString("CielOS.Wallpaper.Path", string.Empty);
			if (string.IsNullOrEmpty(@string))
			{
				this.ReleaseLoadedWallpaper();
				this.ApplyFallbackIfNeeded();
				return false;
			}
			if (this._loadedWallpaperSprite != null && string.Equals(this._loadedWallpaperPath, @string, StringComparison.OrdinalIgnoreCase))
			{
				this.ApplySprite(this._loadedWallpaperSprite);
				return true;
			}
			if (string.Equals(PlayerPrefs.GetString("CielOS.Wallpaper.CacheSource", string.Empty), @string, StringComparison.OrdinalIgnoreCase))
			{
				byte[] array = CielOSWallpaperService.ReadPersistentCopy();
				if (array != null && this.ApplyWallpaperBytes(array, @string))
				{
					return true;
				}
				CielOSWallpaperService.DeletePersistentCopy();
				PlayerPrefs.DeleteKey("CielOS.Wallpaper.CacheSource");
				PlayerPrefs.Save();
			}
			byte[] array2 = CielOSWallpaperService.ReadVfsBytes(@string);
			if (array2 == null)
			{
				Debug.LogWarning("[CielOSWallpaper] Saved image no longer exists: '" + @string + "'.", this);
				return false;
			}
			if (!this.ApplyWallpaperBytes(array2, @string))
			{
				return false;
			}
			CielOSWallpaperService.PersistWallpaperCopy(array2, @string);
			PlayerPrefs.Save();
			return true;
		}

		// Token: 0x06009B2A RID: 39722 RVA: 0x003F73F4 File Offset: 0x003F55F4
		public bool ApplyWallpaper(string virtualPath, bool persist = true)
		{
			byte[] array = CielOSWallpaperService.ReadVfsBytes(virtualPath);
			if (array == null || !this.ApplyWallpaperBytes(array, virtualPath))
			{
				return false;
			}
			if (persist)
			{
				PlayerPrefs.SetString("CielOS.Wallpaper.Path", virtualPath);
				CielOSWallpaperService.PersistWallpaperCopy(array, virtualPath);
				PlayerPrefs.Save();
			}
			return true;
		}

		// Token: 0x06009B2B RID: 39723 RVA: 0x003F7432 File Offset: 0x003F5632
		public void SetScreensaver(string virtualPath)
		{
			this.SetScreensaver(virtualPath, CielOSScreensaverTarget.All);
		}

		// Token: 0x06009B2C RID: 39724 RVA: 0x003F7440 File Offset: 0x003F5640
		public bool SetScreensaver(string virtualPath, CielOSScreensaverTarget target)
		{
			byte[] array = CielOSWallpaperService.ReadVfsBytes(virtualPath);
			if (array == null)
			{
				return false;
			}
			CielOSWallpaperService.EnsureLegacyScreensaverMigrated();
			if (target < CielOSScreensaverTarget.Display1 || target > CielOSScreensaverTarget.All)
			{
				target = CielOSScreensaverTarget.All;
			}
			int num = (int)((target == CielOSScreensaverTarget.All) ? CielOSScreensaverTarget.Display1 : target);
			int num2 = ((target == CielOSScreensaverTarget.All) ? 2 : num);
			for (int i = num; i <= num2; i++)
			{
				PlayerPrefs.SetString(CielOSWallpaperService.GetScreensaverDisplayKey(i), virtualPath ?? string.Empty);
				CielOSWallpaperService.PersistScreensaverCopy(array, virtualPath, i);
			}
			if (target == CielOSScreensaverTarget.All)
			{
				PlayerPrefs.SetString("CielOS.Screensaver.Path", virtualPath ?? string.Empty);
			}
			PlayerPrefs.Save();
			Action screensaverChanged = CielOSWallpaperService.ScreensaverChanged;
			if (screensaverChanged != null)
			{
				screensaverChanged();
			}
			return true;
		}

		// Token: 0x06009B2D RID: 39725 RVA: 0x003F74D4 File Offset: 0x003F56D4
		public string GetScreensaverPath(int displayIndex)
		{
			displayIndex = Mathf.Clamp(displayIndex, 0, 2);
			string screensaverDisplayKey = CielOSWallpaperService.GetScreensaverDisplayKey(displayIndex);
			if (!PlayerPrefs.HasKey(screensaverDisplayKey))
			{
				return PlayerPrefs.GetString("CielOS.Screensaver.Path", string.Empty);
			}
			return PlayerPrefs.GetString(screensaverDisplayKey, string.Empty);
		}

		// Token: 0x06009B2E RID: 39726 RVA: 0x003F7518 File Offset: 0x003F5718
		public Sprite LoadScreensaverSprite(int displayIndex)
		{
			displayIndex = Mathf.Clamp(displayIndex, 0, 2);
			string screensaverPath = this.GetScreensaverPath(displayIndex);
			if (string.IsNullOrEmpty(screensaverPath))
			{
				return null;
			}
			if (string.Equals(PlayerPrefs.GetString("CielOS.Screensaver.CacheSource." + displayIndex.ToString(), string.Empty), screensaverPath, StringComparison.OrdinalIgnoreCase))
			{
				byte[] array = CielOSWallpaperService.ReadPersistentCopy(CielOSWallpaperService.GetScreensaverCachePath(displayIndex));
				Sprite sprite = ((array == null) ? null : CielOSWallpaperService.CreateRuntimeSprite(array));
				if (sprite != null)
				{
					return sprite;
				}
			}
			byte[] array2 = CielOSWallpaperService.ReadVfsBytes(screensaverPath);
			if (array2 == null)
			{
				return null;
			}
			CielOSWallpaperService.PersistScreensaverCopy(array2, screensaverPath, displayIndex);
			PlayerPrefs.Save();
			return CielOSWallpaperService.CreateRuntimeSprite(array2);
		}

		// Token: 0x06009B2F RID: 39727 RVA: 0x003F75A8 File Offset: 0x003F57A8
		public static void ShowScreensaverTargetMenu(Vector2 screenPosition, string virtualPath)
		{
			if (CielOSWallpaperService.Instance == null || CielOSContextMenu.Instance == null)
			{
				return;
			}
			CielOSContextMenu.Instance.Show(screenPosition, new List<CielOSContextMenu.Item>
			{
				new CielOSContextMenu.Item("Screen 1", delegate
				{
					CielOSWallpaperService.Instance.SetScreensaver(virtualPath, CielOSScreensaverTarget.Display1);
				}, true, false, "cielos_shell"),
				new CielOSContextMenu.Item("Screen 2", delegate
				{
					CielOSWallpaperService.Instance.SetScreensaver(virtualPath, CielOSScreensaverTarget.Display2);
				}, true, false, "cielos_shell"),
				new CielOSContextMenu.Item("Screen 3", delegate
				{
					CielOSWallpaperService.Instance.SetScreensaver(virtualPath, CielOSScreensaverTarget.Display3);
				}, true, false, "cielos_shell"),
				new CielOSContextMenu.Item("All screens", delegate
				{
					CielOSWallpaperService.Instance.SetScreensaver(virtualPath, CielOSScreensaverTarget.All);
				}, true, false, "cielos_shell")
			});
		}

		// Token: 0x06009B30 RID: 39728 RVA: 0x003F767C File Offset: 0x003F587C
		private void ApplySprite(Sprite sprite)
		{
			if (this.wallpaperImage == null || sprite == null)
			{
				return;
			}
			this.wallpaperImage.sprite = sprite;
			StreamerCensorImage.Bind(this.wallpaperImage, false);
			this.wallpaperImage.color = Color.white;
			this.wallpaperImage.preserveAspect = false;
			this.wallpaperImage.type = Image.Type.Simple;
		}

		// Token: 0x06009B31 RID: 39729 RVA: 0x003F76E4 File Offset: 0x003F58E4
		private bool ApplyWallpaperBytes(byte[] bytes, string sourcePath)
		{
			if (this.wallpaperImage == null)
			{
				return false;
			}
			Sprite sprite = CielOSWallpaperService.CreateRuntimeSprite(bytes);
			if (sprite == null)
			{
				return false;
			}
			Sprite loadedWallpaperSprite = this._loadedWallpaperSprite;
			Texture2D loadedWallpaperTexture = this._loadedWallpaperTexture;
			this._loadedWallpaperPath = sourcePath;
			this._loadedWallpaperSprite = sprite;
			this._loadedWallpaperTexture = sprite.texture;
			this.ApplySprite(sprite);
			CielOSWallpaperService.DestroyRuntimeResources(loadedWallpaperSprite, loadedWallpaperTexture);
			return true;
		}

		// Token: 0x06009B32 RID: 39730 RVA: 0x003F7748 File Offset: 0x003F5948
		private void ReleaseLoadedWallpaper()
		{
			Sprite loadedWallpaperSprite = this._loadedWallpaperSprite;
			Texture2D loadedWallpaperTexture = this._loadedWallpaperTexture;
			this._loadedWallpaperPath = null;
			this._loadedWallpaperSprite = null;
			this._loadedWallpaperTexture = null;
			if (this.wallpaperImage != null && this.wallpaperImage.sprite == loadedWallpaperSprite)
			{
				this.wallpaperImage.sprite = null;
			}
			CielOSWallpaperService.DestroyRuntimeResources(loadedWallpaperSprite, loadedWallpaperTexture);
		}

		// Token: 0x06009B33 RID: 39731 RVA: 0x003F77AC File Offset: 0x003F59AC
		private void ApplyFallbackIfNeeded()
		{
			if (this.wallpaperImage == null || this.wallpaperImage.GetComponent<CielOSGradient>() != null)
			{
				return;
			}
			this.wallpaperImage.sprite = null;
			this.wallpaperImage.color = this.emptyColor;
		}

		// Token: 0x06009B34 RID: 39732 RVA: 0x003F77F8 File Offset: 0x003F59F8
		public static Sprite LoadSprite(string virtualPath)
		{
			byte[] array = CielOSWallpaperService.ReadVfsBytes(virtualPath);
			if (array != null)
			{
				return CielOSWallpaperService.CreateRuntimeSprite(array);
			}
			return null;
		}

		// Token: 0x06009B35 RID: 39733 RVA: 0x003F7818 File Offset: 0x003F5A18
		public static Sprite LoadThumbnail(string virtualPath, int shortSide)
		{
			byte[] array = CielOSWallpaperService.ReadVfsBytes(virtualPath);
			if (array != null)
			{
				return CielOSWallpaperService.CreateThumbnail(array, shortSide);
			}
			return null;
		}

		// Token: 0x06009B36 RID: 39734 RVA: 0x003F7838 File Offset: 0x003F5A38
		private static byte[] ReadVfsBytes(string virtualPath)
		{
			if (string.IsNullOrWhiteSpace(virtualPath))
			{
				return null;
			}
			byte[] array;
			try
			{
				array = CielOSFileSystem.Instance.ReadBytes(virtualPath);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSWallpaper] Could not read '" + virtualPath + "': " + ex.Message);
				array = null;
			}
			return array;
		}

		// Token: 0x06009B37 RID: 39735 RVA: 0x003F7890 File Offset: 0x003F5A90
		private static Sprite CreateRuntimeSprite(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
			Sprite sprite;
			try
			{
				if (!texture2D.LoadImage(bytes))
				{
					CielOSWallpaperService.DestroyRuntimeObject(texture2D);
					sprite = null;
				}
				else
				{
					sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
				}
			}
			catch (Exception ex)
			{
				CielOSWallpaperService.DestroyRuntimeObject(texture2D);
				Debug.LogWarning("[CielOSWallpaper] Invalid image data: " + ex.Message);
				sprite = null;
			}
			return sprite;
		}

		// Token: 0x06009B38 RID: 39736 RVA: 0x003F7930 File Offset: 0x003F5B30
		public static Sprite CreateThumbnail(byte[] bytes, int shortSide)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
			Texture2D texture2D2 = null;
			Sprite sprite;
			try
			{
				if (!texture2D.LoadImage(bytes, true))
				{
					CielOSWallpaperService.DestroyRuntimeObject(texture2D);
					sprite = null;
				}
				else
				{
					texture2D.wrapMode = TextureWrapMode.Clamp;
					float num = Mathf.Min(1f, (float)shortSide / (float)Mathf.Max(1, Mathf.Min(texture2D.width, texture2D.height)));
					int num2 = Mathf.Max(1, Mathf.RoundToInt((float)texture2D.width * num));
					int num3 = Mathf.Max(1, Mathf.RoundToInt((float)texture2D.height * num));
					if (num2 < texture2D.width || num3 < texture2D.height)
					{
						texture2D2 = new Texture2D(num2, num3, TextureFormat.RGBA32, false)
						{
							wrapMode = TextureWrapMode.Clamp
						};
						texture2D2.Apply(false, true);
						if (CielOSWallpaperService.CopyScaled(texture2D, texture2D2))
						{
							Object.DestroyImmediate(texture2D);
							texture2D = texture2D2;
						}
						else
						{
							CielOSWallpaperService.DestroyRuntimeObject(texture2D2);
						}
						texture2D2 = null;
					}
					sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
				}
			}
			catch (Exception ex)
			{
				CielOSWallpaperService.DestroyRuntimeObject(texture2D2);
				CielOSWallpaperService.DestroyRuntimeObject(texture2D);
				Debug.LogWarning("[CielOSWallpaper] Invalid image data: " + ex.Message);
				sprite = null;
			}
			return sprite;
		}

		// Token: 0x06009B39 RID: 39737 RVA: 0x003F7A80 File Offset: 0x003F5C80
		private static bool CopyScaled(Texture source, Texture2D target)
		{
			if ((SystemInfo.copyTextureSupport & CopyTextureSupport.RTToTexture) == CopyTextureSupport.None)
			{
				return false;
			}
			RenderTexture active = RenderTexture.active;
			RenderTexture temporary = RenderTexture.GetTemporary(new RenderTextureDescriptor(target.width, target.height, RenderTextureFormat.ARGB32, 0)
			{
				graphicsFormat = target.graphicsFormat
			});
			bool flag;
			try
			{
				Graphics.Blit(source, temporary);
				Graphics.CopyTexture(temporary, target);
				flag = true;
			}
			finally
			{
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
			}
			return flag;
		}

		// Token: 0x06009B3A RID: 39738 RVA: 0x003F7AF8 File Offset: 0x003F5CF8
		private static byte[] ReadPersistentCopy()
		{
			return CielOSWallpaperService.ReadPersistentCopy(CielOSWallpaperService.WallpaperCachePath);
		}

		// Token: 0x06009B3B RID: 39739 RVA: 0x003F7B04 File Offset: 0x003F5D04
		private static byte[] ReadPersistentCopy(string path)
		{
			byte[] array;
			try
			{
				array = (File.Exists(path) ? File.ReadAllBytes(path) : null);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSWallpaper] Could not read persistent copy: " + ex.Message);
				array = null;
			}
			return array;
		}

		// Token: 0x06009B3C RID: 39740 RVA: 0x003F7B50 File Offset: 0x003F5D50
		private static void PersistWallpaperCopy(byte[] bytes, string sourcePath)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(CielOSWallpaperService.WallpaperCachePath);
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllBytes(CielOSWallpaperService.WallpaperCachePath, bytes);
				PlayerPrefs.SetString("CielOS.Wallpaper.CacheSource", sourcePath ?? string.Empty);
			}
			catch (Exception ex)
			{
				PlayerPrefs.DeleteKey("CielOS.Wallpaper.CacheSource");
				Debug.LogWarning("[CielOSWallpaper] Could not persist wallpaper copy: " + ex.Message);
			}
		}

		// Token: 0x06009B3D RID: 39741 RVA: 0x003F7BCC File Offset: 0x003F5DCC
		private static string GetScreensaverDisplayKey(int displayIndex)
		{
			return "CielOS.Screensaver.Display." + Mathf.Clamp(displayIndex, 0, 2).ToString();
		}

		// Token: 0x06009B3E RID: 39742 RVA: 0x003F7BF4 File Offset: 0x003F5DF4
		private static void EnsureLegacyScreensaverMigrated()
		{
			string @string = PlayerPrefs.GetString("CielOS.Screensaver.Path", string.Empty);
			for (int i = 0; i < 3; i++)
			{
				string screensaverDisplayKey = CielOSWallpaperService.GetScreensaverDisplayKey(i);
				if (!PlayerPrefs.HasKey(screensaverDisplayKey))
				{
					PlayerPrefs.SetString(screensaverDisplayKey, @string);
				}
			}
		}

		// Token: 0x06009B3F RID: 39743 RVA: 0x003F7C34 File Offset: 0x003F5E34
		private static string GetScreensaverCachePath(int displayIndex)
		{
			return Path.Combine(Application.persistentDataPath, "CielOS_State", "screensaver-display-" + Mathf.Clamp(displayIndex, 0, 2).ToString() + ".cache");
		}

		// Token: 0x06009B40 RID: 39744 RVA: 0x003F7C70 File Offset: 0x003F5E70
		private static void PersistScreensaverCopy(byte[] bytes, string sourcePath, int displayIndex)
		{
			try
			{
				string screensaverCachePath = CielOSWallpaperService.GetScreensaverCachePath(displayIndex);
				string directoryName = Path.GetDirectoryName(screensaverCachePath);
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllBytes(screensaverCachePath, bytes);
				PlayerPrefs.SetString("CielOS.Screensaver.CacheSource." + Mathf.Clamp(displayIndex, 0, 2).ToString(), sourcePath ?? string.Empty);
			}
			catch (Exception ex)
			{
				PlayerPrefs.DeleteKey("CielOS.Screensaver.CacheSource." + Mathf.Clamp(displayIndex, 0, 2).ToString());
				Debug.LogWarning(string.Format("[CielOSWallpaper] Could not persist display {0}: {1}", displayIndex + 1, ex.Message));
			}
		}

		// Token: 0x06009B41 RID: 39745 RVA: 0x003F7D1C File Offset: 0x003F5F1C
		private static void DeletePersistentCopy()
		{
			try
			{
				if (File.Exists(CielOSWallpaperService.WallpaperCachePath))
				{
					File.Delete(CielOSWallpaperService.WallpaperCachePath);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSWallpaper] Could not discard invalid copy: " + ex.Message);
			}
		}

		// Token: 0x06009B42 RID: 39746 RVA: 0x003F7D6C File Offset: 0x003F5F6C
		private static void DestroyRuntimeResources(Sprite sprite, Texture2D texture)
		{
			CielOSWallpaperService.DestroyRuntimeObject(sprite);
			CielOSWallpaperService.DestroyRuntimeObject(texture);
		}

		// Token: 0x06009B43 RID: 39747 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyRuntimeObject(Object target)
		{
			if (target == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x06009B44 RID: 39748 RVA: 0x003F7D7C File Offset: 0x003F5F7C
		public static bool IsImage(string name)
		{
			string text = name.ToLowerInvariant();
			return text.EndsWith(".png") || text.EndsWith(".jpg") || text.EndsWith(".jpeg");
		}

		// Token: 0x0400A026 RID: 40998
		[Tooltip("Desktop wallpaper Image to apply the chosen picture to.")]
		[SerializeField]
		private Image wallpaperImage;

		// Token: 0x0400A027 RID: 40999
		[Tooltip("Fallback colour shown when no wallpaper is set.")]
		[SerializeField]
		private Color emptyColor = new Color(0.1f, 0.07f, 0.14f, 1f);

		// Token: 0x0400A028 RID: 41000
		private const string WallpaperKey = "CielOS.Wallpaper.Path";

		// Token: 0x0400A029 RID: 41001
		private const string WallpaperCacheSourceKey = "CielOS.Wallpaper.CacheSource";

		// Token: 0x0400A02A RID: 41002
		private const string ScreensaverKey = "CielOS.Screensaver.Path";

		// Token: 0x0400A02B RID: 41003
		private const string ScreensaverDisplayKeyPrefix = "CielOS.Screensaver.Display.";

		// Token: 0x0400A02C RID: 41004
		private const string ScreensaverCacheSourceKeyPrefix = "CielOS.Screensaver.CacheSource.";

		// Token: 0x0400A02D RID: 41005
		private const string StateDirectoryName = "CielOS_State";

		// Token: 0x0400A02E RID: 41006
		private const string WallpaperCacheFileName = "wallpaper-image.cache";

		// Token: 0x0400A02F RID: 41007
		private const string ScreensaverCacheFilePrefix = "screensaver-display-";

		// Token: 0x0400A031 RID: 41009
		private string _loadedWallpaperPath;

		// Token: 0x0400A032 RID: 41010
		private Sprite _loadedWallpaperSprite;

		// Token: 0x0400A033 RID: 41011
		private Texture2D _loadedWallpaperTexture;
	}
}
