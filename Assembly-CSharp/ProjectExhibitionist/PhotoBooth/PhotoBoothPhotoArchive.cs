using System;
using System.Collections.Generic;
using System.IO;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x02000989 RID: 2441
	public static class PhotoBoothPhotoArchive
	{
		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06004359 RID: 17241 RVA: 0x00192FFC File Offset: 0x001911FC
		// (remove) Token: 0x0600435A RID: 17242 RVA: 0x00193030 File Offset: 0x00191230
		public static event Action Changed;

		// Token: 0x0600435B RID: 17243 RVA: 0x00193063 File Offset: 0x00191263
		public static string ArchiveIdFromPath(string path)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				return Path.GetFileName(path);
			}
			return string.Empty;
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x0019307C File Offset: 0x0019127C
		public static IReadOnlyList<string> GetArchiveIds()
		{
			List<string> list = new List<string>();
			try
			{
				string[] files = Directory.GetFiles(PlayerMediaExport.PhotoBoothPhotosFolder, "PHOTOBOOTH_*.png", SearchOption.TopDirectoryOnly);
				Array.Sort<string>(files, StringComparer.OrdinalIgnoreCase);
				for (int i = 0; i < files.Length; i++)
				{
					list.Add(Path.GetFileName(files[i]));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PhotoBoothArchive] Cannot enumerate photos: " + ex.Message);
			}
			return list;
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x001930F4 File Offset: 0x001912F4
		public static PhotoBoothPhotoArchive.LoadedPhoto Load(string archiveId, int sequence)
		{
			string text;
			if (!PhotoBoothPhotoArchive.TryResolveArchivePath(archiveId, out text) || !File.Exists(text))
			{
				return null;
			}
			PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto;
			try
			{
				byte[] array = File.ReadAllBytes(text);
				Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGB24, false, false)
				{
					name = Path.GetFileName(text),
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Bilinear
				};
				if (!texture2D.LoadImage(array, false))
				{
					Object.Destroy(texture2D);
					loadedPhoto = null;
				}
				else
				{
					Texture2D texture2D2 = PhotoBoothPhotoArchive.CreatePrintTexture(texture2D);
					if (texture2D2 == null)
					{
						Object.Destroy(texture2D);
						loadedPhoto = null;
					}
					else
					{
						texture2D2.name = Path.GetFileNameWithoutExtension(text) + "_PRINT";
						loadedPhoto = new PhotoBoothPhotoArchive.LoadedPhoto
						{
							archiveId = Path.GetFileName(text),
							path = text,
							portraitTexture = texture2D,
							printTexture = texture2D2,
							sequence = sequence
						};
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PhotoBoothArchive] Cannot load '" + archiveId + "': " + ex.Message);
				loadedPhoto = null;
			}
			return loadedPhoto;
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x001931F4 File Offset: 0x001913F4
		public static PhotoBoothPhotoArchive.LoadedPhoto LoadDisplay(string archiveId, int sequence, int maximumDimension = 512, bool rotate180 = true, bool markNonReadable = false)
		{
			string text;
			if (!PhotoBoothPhotoArchive.TryResolveArchivePath(archiveId, out text) || !File.Exists(text))
			{
				return null;
			}
			Texture2D texture2D = null;
			PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto;
			try
			{
				byte[] array = File.ReadAllBytes(text);
				texture2D = new Texture2D(2, 2, TextureFormat.RGB24, false, false)
				{
					name = Path.GetFileName(text) + " (Display Source)",
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Bilinear
				};
				if (!texture2D.LoadImage(array, false))
				{
					Object.DestroyImmediate(texture2D);
					loadedPhoto = null;
				}
				else
				{
					Texture2D texture2D2 = PhotoBoothPhotoArchive.CreateScaledTexture(texture2D, Mathf.Max(64, maximumDimension), rotate180, markNonReadable);
					Object.DestroyImmediate(texture2D);
					texture2D = null;
					if (texture2D2 == null)
					{
						loadedPhoto = null;
					}
					else
					{
						texture2D2.name = Path.GetFileNameWithoutExtension(text) + "_DESK";
						loadedPhoto = new PhotoBoothPhotoArchive.LoadedPhoto
						{
							archiveId = Path.GetFileName(text),
							path = text,
							portraitTexture = texture2D2,
							printTexture = texture2D2,
							printNeeds180Rotation = false,
							sequence = sequence
						};
					}
				}
			}
			catch (Exception ex)
			{
				if (texture2D != null)
				{
					Object.DestroyImmediate(texture2D);
				}
				Debug.LogWarning("[PhotoBoothArchive] Cannot load display copy '" + archiveId + "': " + ex.Message);
				loadedPhoto = null;
			}
			return loadedPhoto;
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x00193324 File Offset: 0x00191524
		public static PhotoBoothPhotoArchive.LoadedPhoto LoadInventory(string archiveId, int sequence, bool markNonReadable = false)
		{
			string text;
			if (!PhotoBoothPhotoArchive.TryResolveArchivePath(archiveId, out text) || !File.Exists(text))
			{
				return null;
			}
			Texture2D texture2D = null;
			PhotoBoothPhotoArchive.LoadedPhoto loadedPhoto;
			try
			{
				texture2D = new Texture2D(2, 2, TextureFormat.RGB24, false, false)
				{
					name = Path.GetFileName(text) + " (Inventory Source)",
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Bilinear
				};
				if (!texture2D.LoadImage(File.ReadAllBytes(text), markNonReadable))
				{
					Object.DestroyImmediate(texture2D);
					loadedPhoto = null;
				}
				else
				{
					texture2D.name = Path.GetFileNameWithoutExtension(text) + "_ALBUM_FULL";
					loadedPhoto = new PhotoBoothPhotoArchive.LoadedPhoto
					{
						archiveId = Path.GetFileName(text),
						path = text,
						portraitTexture = texture2D,
						printTexture = texture2D,
						printNeeds180Rotation = true,
						sequence = sequence
					};
				}
			}
			catch (Exception ex)
			{
				if (texture2D != null)
				{
					Object.DestroyImmediate(texture2D);
				}
				Debug.LogWarning("[PhotoBoothArchive] Cannot load album page '" + archiveId + "': " + ex.Message);
				loadedPhoto = null;
			}
			return loadedPhoto;
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x0019341C File Offset: 0x0019161C
		public static bool Delete(string archiveId)
		{
			string text;
			if (!PhotoBoothPhotoArchive.TryResolveArchivePath(archiveId, out text) || !File.Exists(text))
			{
				return false;
			}
			bool flag;
			try
			{
				File.Delete(text);
				Action changed = PhotoBoothPhotoArchive.Changed;
				if (changed != null)
				{
					changed();
				}
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PhotoBoothArchive] Cannot delete '" + archiveId + "': " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x0019348C File Offset: 0x0019168C
		public static void NotifyChanged()
		{
			Action changed = PhotoBoothPhotoArchive.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x001934A0 File Offset: 0x001916A0
		public static Texture2D CreatePrintTexture(Texture2D portrait)
		{
			if (portrait == null)
			{
				return null;
			}
			Color32[] pixels = portrait.GetPixels32();
			Color32[] array = new Color32[pixels.Length];
			int num = pixels.Length - 1;
			for (int i = 0; i < pixels.Length; i++)
			{
				array[num - i] = pixels[i];
			}
			Texture2D texture2D = new Texture2D(portrait.width, portrait.height, TextureFormat.RGB24, false, false);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.SetPixels32(array);
			texture2D.Apply(false, false);
			return texture2D;
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x00193520 File Offset: 0x00191720
		private static Texture2D CreateScaledTexture(Texture2D portrait, int maximumDimension, bool rotate180, bool markNonReadable)
		{
			if (portrait == null || portrait.width <= 0 || portrait.height <= 0)
			{
				return null;
			}
			float num = Mathf.Min(1f, (float)maximumDimension / (float)Mathf.Max(portrait.width, portrait.height));
			int num2 = Mathf.Max(1, Mathf.RoundToInt((float)portrait.width * num));
			int num3 = Mathf.Max(1, Mathf.RoundToInt((float)portrait.height * num));
			Color32[] pixels = portrait.GetPixels32();
			Color32[] array = new Color32[num2 * num3];
			for (int i = 0; i < num3; i++)
			{
				int num4 = Mathf.Clamp(Mathf.FloorToInt(((float)i + 0.5f) * (float)portrait.height / (float)num3), 0, portrait.height - 1);
				int num5 = (rotate180 ? (portrait.height - 1 - num4) : num4);
				int num6 = i * num2;
				int num7 = num5 * portrait.width;
				for (int j = 0; j < num2; j++)
				{
					int num8 = Mathf.Clamp(Mathf.FloorToInt(((float)j + 0.5f) * (float)portrait.width / (float)num2), 0, portrait.width - 1);
					int num9 = (rotate180 ? (portrait.width - 1 - num8) : num8);
					array[num6 + j] = pixels[num7 + num9];
				}
			}
			Texture2D texture2D = new Texture2D(num2, num3, TextureFormat.RGB24, false, false);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.SetPixels32(array);
			texture2D.Apply(false, markNonReadable);
			return texture2D;
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x00193694 File Offset: 0x00191894
		private static bool TryResolveArchivePath(string archiveId, out string path)
		{
			path = null;
			if (string.IsNullOrWhiteSpace(archiveId))
			{
				return false;
			}
			string fileName = Path.GetFileName(archiveId);
			if (!fileName.StartsWith("PHOTOBOOTH_", StringComparison.OrdinalIgnoreCase) || !fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			string text = Path.GetFullPath(PlayerMediaExport.PhotoBoothPhotosFolder).TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			}) + Path.DirectorySeparatorChar.ToString();
			string fullPath = Path.GetFullPath(Path.Combine(text, fileName));
			if (!fullPath.StartsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			path = fullPath;
			return true;
		}

		// Token: 0x04004354 RID: 17236
		public const string FilePrefix = "PHOTOBOOTH_";

		// Token: 0x04004355 RID: 17237
		public const string SearchPattern = "PHOTOBOOTH_*.png";

		// Token: 0x0200098A RID: 2442
		public sealed class LoadedPhoto
		{
			// Token: 0x06004365 RID: 17253 RVA: 0x00193728 File Offset: 0x00191928
			public void DestroyTextures()
			{
				if (this.portraitTexture != null)
				{
					Object.Destroy(this.portraitTexture);
				}
				if (this.printTexture != null && this.printTexture != this.portraitTexture)
				{
					Object.Destroy(this.printTexture);
				}
				this.portraitTexture = null;
				this.printTexture = null;
			}

			// Token: 0x04004357 RID: 17239
			public string archiveId;

			// Token: 0x04004358 RID: 17240
			public string path;

			// Token: 0x04004359 RID: 17241
			public Texture2D portraitTexture;

			// Token: 0x0400435A RID: 17242
			public Texture2D printTexture;

			// Token: 0x0400435B RID: 17243
			public bool printNeeds180Rotation;

			// Token: 0x0400435C RID: 17244
			public int sequence;
		}
	}
}
