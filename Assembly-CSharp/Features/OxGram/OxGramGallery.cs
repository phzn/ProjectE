using System;
using System.Collections.Generic;
using System.IO;
using Features.PhoneCompanion;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace Features.OxGram
{
	// Token: 0x02000EA0 RID: 3744
	public static class OxGramGallery
	{
		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06007076 RID: 28790 RVA: 0x002DA766 File Offset: 0x002D8966
		public static List<Sprite> Photos
		{
			get
			{
				List<Sprite> list;
				if ((list = OxGramGallery._photos) == null)
				{
					list = (OxGramGallery._photos = OxGramGallery.Load());
				}
				return list;
			}
		}

		// Token: 0x06007077 RID: 28791 RVA: 0x002DA77C File Offset: 0x002D897C
		public static List<Sprite> Reload()
		{
			return OxGramGallery._photos = OxGramGallery.Load();
		}

		// Token: 0x06007078 RID: 28792 RVA: 0x002DA78C File Offset: 0x002D898C
		public static void DecodeWhileOpen(GameObject picker)
		{
			if (picker == null)
			{
				return;
			}
			OxGramGallery._pickers.RemoveAll((GameObject open) => open == null);
			if (!OxGramGallery._pickers.Contains(picker))
			{
				OxGramGallery._pickers.Add(picker);
			}
			if (!OxGramGallery._decoding && Application.isPlaying)
			{
				OxGramGallery.DecodeNext();
			}
		}

		// Token: 0x06007079 RID: 28793 RVA: 0x002DA7F8 File Offset: 0x002D89F8
		public static Sprite OriginalOf(Sprite photo)
		{
			if (photo == null)
			{
				return null;
			}
			OxGramGallery.GalleryEntry galleryEntry = OxGramGallery.EntryOf(photo);
			if (galleryEntry == null)
			{
				return photo;
			}
			if (OxGramGallery._originalEntry == galleryEntry && OxGramGallery._original != null)
			{
				return OxGramGallery._original;
			}
			Sprite sprite = null;
			try
			{
				byte[] array = File.ReadAllBytes(galleryEntry.path);
				Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
				if (texture2D.LoadImage(array))
				{
					sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
					sprite.name = photo.name;
				}
				else
				{
					OxGramGallery.DestroyGeneratedObject(texture2D);
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			if (sprite == null)
			{
				return photo;
			}
			OxGramGallery._originalEntry = galleryEntry;
			OxGramGallery._original = sprite;
			return sprite;
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x0600707A RID: 28794 RVA: 0x002DA8E0 File Offset: 0x002D8AE0
		private static string GalleryPath
		{
			get
			{
				return PhonePhotoStorage.ImagesFolder;
			}
		}

		// Token: 0x0600707B RID: 28795 RVA: 0x002DA8E7 File Offset: 0x002D8AE7
		private static List<Sprite> Load()
		{
			return OxGramGallery.Load(OxGramGallery.GalleryPath);
		}

		// Token: 0x0600707C RID: 28796 RVA: 0x002DA8F4 File Offset: 0x002D8AF4
		private static List<Sprite> Load(string dir)
		{
			List<Sprite> list = new List<Sprite>();
			List<OxGramGallery.GalleryEntry> list2 = new List<OxGramGallery.GalleryEntry>();
			int num = ++OxGramGallery._generation;
			if (Directory.Exists(dir))
			{
				List<FileInfo> list3 = new List<FileInfo>(new DirectoryInfo(dir).GetFiles("*.png"));
				list3.Sort(delegate(FileInfo a, FileInfo b)
				{
					int num3 = b.LastWriteTimeUtc.CompareTo(a.LastWriteTimeUtc);
					if (num3 == 0)
					{
						return StringComparer.Ordinal.Compare(a.Name, b.Name);
					}
					return num3;
				});
				foreach (FileInfo fileInfo in list3)
				{
					string fullName = fileInfo.FullName;
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullName);
					int num2;
					if (!int.TryParse(fileNameWithoutExtension, out num2) && !PhonePhotoStorage.IsExcludedFromGallery(fullName) && !fileNameWithoutExtension.EndsWith(".censored", StringComparison.OrdinalIgnoreCase))
					{
						OxGramGallery.GalleryEntry galleryEntry = OxGramGallery.EntryFor(fileInfo, fileNameWithoutExtension);
						if (galleryEntry != null)
						{
							galleryEntry.generation = num;
							list2.Add(galleryEntry);
							if (!galleryEntry.failed)
							{
								list.Add(galleryEntry.thumbnail);
							}
						}
					}
				}
			}
			OxGramGallery.ReleaseUnlisted(num);
			OxGramGallery.QueueDecoding(list2);
			return list;
		}

		// Token: 0x0600707D RID: 28797 RVA: 0x002DAA14 File Offset: 0x002D8C14
		private static OxGramGallery.GalleryEntry EntryFor(FileInfo info, string name)
		{
			OxGramGallery.GalleryEntry galleryEntry;
			if (OxGramGallery._entries.TryGetValue(info.FullName, out galleryEntry))
			{
				if (galleryEntry.thumbnail != null && galleryEntry.texture != null && galleryEntry.lastWriteUtc == info.LastWriteTimeUtc && galleryEntry.length == info.Length)
				{
					return galleryEntry;
				}
				OxGramGallery.ReleaseEntry(galleryEntry);
			}
			galleryEntry = OxGramGallery.CreateEntry(info, name);
			if (galleryEntry != null)
			{
				OxGramGallery._entries[info.FullName] = galleryEntry;
			}
			return galleryEntry;
		}

		// Token: 0x0600707E RID: 28798 RVA: 0x002DAA98 File Offset: 0x002D8C98
		private static OxGramGallery.GalleryEntry CreateEntry(FileInfo info, string name)
		{
			Texture2D texture2D = null;
			int width;
			int height;
			if (!OxGramGallery.TryReadPngSize(info.FullName, out width, out height) || !Application.isPlaying)
			{
				texture2D = OxGramGallery.DecodeFile(info.FullName);
				if (texture2D == null)
				{
					return null;
				}
				width = texture2D.width;
				height = texture2D.height;
			}
			float num = Mathf.Min(1f, 384f / (float)Mathf.Max(width, height));
			int num2 = Mathf.Max(1, Mathf.RoundToInt((float)width * num));
			int num3 = Mathf.Max(1, Mathf.RoundToInt((float)height * num));
			Texture2D texture2D2 = new Texture2D(num2, num3, TextureFormat.RGBA32, false)
			{
				wrapMode = TextureWrapMode.Clamp
			};
			texture2D2.SetPixels32(OxGramGallery.ClearPixels(num2 * num3));
			texture2D2.Apply(false, true);
			Sprite sprite = Sprite.Create(texture2D2, new Rect(0f, 0f, (float)num2, (float)num3), new Vector2(0.5f, 0.5f), 100f);
			sprite.name = name;
			OxGramGallery.GalleryEntry galleryEntry = new OxGramGallery.GalleryEntry
			{
				path = info.FullName,
				lastWriteUtc = info.LastWriteTimeUtc,
				length = info.Length,
				width = width,
				height = height,
				thumbnail = sprite,
				texture = texture2D2
			};
			if (texture2D != null)
			{
				galleryEntry.decoded = OxGramGallery.CopyScaled(texture2D, texture2D2);
				OxGramGallery.DestroyGeneratedObject(texture2D);
			}
			return galleryEntry;
		}

		// Token: 0x0600707F RID: 28799 RVA: 0x002DABF0 File Offset: 0x002D8DF0
		private static OxGramGallery.GalleryEntry EntryOf(Sprite photo)
		{
			foreach (OxGramGallery.GalleryEntry galleryEntry in OxGramGallery._entries.Values)
			{
				if (galleryEntry.thumbnail == photo)
				{
					return galleryEntry;
				}
			}
			return null;
		}

		// Token: 0x06007080 RID: 28800 RVA: 0x002DAC58 File Offset: 0x002D8E58
		private static bool IsLive(OxGramGallery.GalleryEntry entry)
		{
			OxGramGallery.GalleryEntry galleryEntry;
			return entry.thumbnail != null && entry.texture != null && OxGramGallery._entries.TryGetValue(entry.path, out galleryEntry) && galleryEntry == entry;
		}

		// Token: 0x06007081 RID: 28801 RVA: 0x002DAC9C File Offset: 0x002D8E9C
		private static void ReleaseEntry(OxGramGallery.GalleryEntry entry)
		{
			OxGramGallery.GalleryEntry galleryEntry;
			if (OxGramGallery._entries.TryGetValue(entry.path, out galleryEntry) && galleryEntry == entry)
			{
				OxGramGallery._entries.Remove(entry.path);
			}
			if (OxGramGallery._originalEntry == entry)
			{
				OxGramGallery._originalEntry = null;
				OxGramGallery._original = null;
			}
			OxGramGallery.DestroyGeneratedObject(entry.thumbnail);
			OxGramGallery.DestroyGeneratedObject(entry.texture);
			entry.thumbnail = null;
			entry.texture = null;
		}

		// Token: 0x06007082 RID: 28802 RVA: 0x002DAD0C File Offset: 0x002D8F0C
		private static void ReleaseUnlisted(int generation)
		{
			List<OxGramGallery.GalleryEntry> list = new List<OxGramGallery.GalleryEntry>();
			foreach (OxGramGallery.GalleryEntry galleryEntry in OxGramGallery._entries.Values)
			{
				if (galleryEntry.generation != generation)
				{
					list.Add(galleryEntry);
				}
			}
			foreach (OxGramGallery.GalleryEntry galleryEntry2 in list)
			{
				OxGramGallery.ReleaseEntry(galleryEntry2);
			}
		}

		// Token: 0x06007083 RID: 28803 RVA: 0x002DADAC File Offset: 0x002D8FAC
		private static void QueueDecoding(List<OxGramGallery.GalleryEntry> entries)
		{
			OxGramGallery._decodeQueue.Clear();
			foreach (OxGramGallery.GalleryEntry galleryEntry in entries)
			{
				if (!galleryEntry.decoded && !galleryEntry.failed)
				{
					OxGramGallery._decodeQueue.Enqueue(galleryEntry);
				}
			}
			if (!OxGramGallery._decoding && Application.isPlaying)
			{
				OxGramGallery.DecodeNext();
			}
		}

		// Token: 0x06007084 RID: 28804 RVA: 0x002DAE2C File Offset: 0x002D902C
		private static void DecodeNext()
		{
			if (!OxGramGallery.IsPickerOpen())
			{
				return;
			}
			while (OxGramGallery._decodeQueue.Count > 0)
			{
				OxGramGallery.GalleryEntry entry = OxGramGallery._decodeQueue.Dequeue();
				if (!entry.decoded && !entry.failed && OxGramGallery.IsLive(entry))
				{
					UnityWebRequest request;
					try
					{
						request = UnityWebRequestTexture.GetTexture(new Uri(entry.path).AbsoluteUri, true);
					}
					catch (UriFormatException)
					{
						entry.failed = true;
						continue;
					}
					OxGramGallery._decoding = true;
					request.SendWebRequest().completed += delegate(AsyncOperation _)
					{
						try
						{
							OxGramGallery.Fill(entry, (request.result == UnityWebRequest.Result.Success) ? DownloadHandlerTexture.GetContent(request) : null);
						}
						finally
						{
							request.Dispose();
							OxGramGallery._decoding = false;
							if (Application.isPlaying)
							{
								OxGramGallery.DecodeNext();
							}
						}
					};
					return;
				}
			}
		}

		// Token: 0x06007085 RID: 28805 RVA: 0x002DAEF8 File Offset: 0x002D90F8
		private static bool IsPickerOpen()
		{
			foreach (GameObject gameObject in OxGramGallery._pickers)
			{
				if (gameObject != null && gameObject.activeInHierarchy)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007086 RID: 28806 RVA: 0x002DAF5C File Offset: 0x002D915C
		private static void Fill(OxGramGallery.GalleryEntry entry, Texture2D photo)
		{
			try
			{
				if (photo == null || photo.width != entry.width || photo.height != entry.height)
				{
					entry.failed = true;
				}
				else if (OxGramGallery.IsLive(entry))
				{
					entry.decoded = OxGramGallery.CopyScaled(photo, entry.texture);
				}
			}
			finally
			{
				OxGramGallery.DestroyGeneratedObject(photo);
			}
		}

		// Token: 0x06007087 RID: 28807 RVA: 0x002DAFCC File Offset: 0x002D91CC
		private static Texture2D DecodeFile(string file)
		{
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
			try
			{
				if (texture2D.LoadImage(File.ReadAllBytes(file), true))
				{
					return texture2D;
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			OxGramGallery.DestroyGeneratedObject(texture2D);
			return null;
		}

		// Token: 0x06007088 RID: 28808 RVA: 0x002DB024 File Offset: 0x002D9224
		private static bool TryReadPngSize(string file, out int width, out int height)
		{
			width = (height = 0);
			byte[] array = new byte[24];
			try
			{
				using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					if (fileStream.Read(array, 0, array.Length) != array.Length)
					{
						return false;
					}
				}
			}
			catch (IOException)
			{
				return false;
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}
			if (array[0] != 137 || array[1] != 80 || array[2] != 78 || array[3] != 71 || array[12] != 73 || array[13] != 72 || array[14] != 68 || array[15] != 82)
			{
				return false;
			}
			width = ((int)array[16] << 24) | ((int)array[17] << 16) | ((int)array[18] << 8) | (int)array[19];
			height = ((int)array[20] << 24) | ((int)array[21] << 16) | ((int)array[22] << 8) | (int)array[23];
			return width > 0 && height > 0;
		}

		// Token: 0x06007089 RID: 28809 RVA: 0x002DB134 File Offset: 0x002D9334
		private static Color32[] ClearPixels(int count)
		{
			if (OxGramGallery._clearPixels == null || OxGramGallery._clearPixels.Length != count)
			{
				OxGramGallery._clearPixels = new Color32[count];
			}
			return OxGramGallery._clearPixels;
		}

		// Token: 0x0600708A RID: 28810 RVA: 0x002DB158 File Offset: 0x002D9358
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

		// Token: 0x0600708B RID: 28811 RVA: 0x002DB1D0 File Offset: 0x002D93D0
		public static Sprite CensoredSiblingOf(Sprite photo, bool requireCensored = false)
		{
			if (photo == null)
			{
				return null;
			}
			string text = photo.name;
			try
			{
				text = Path.GetFileNameWithoutExtension(photo.name);
			}
			catch
			{
			}
			if (string.IsNullOrEmpty(text))
			{
				if (!requireCensored)
				{
					return photo;
				}
				return OxGramGallery.PrivacyCoverOf(photo, string.Empty);
			}
			else
			{
				Sprite sprite;
				if (OxGramGallery._censoredCache.TryGetValue(text, out sprite) && sprite != null)
				{
					return sprite;
				}
				string text2 = Path.Combine(OxGramGallery.GalleryPath, text + ".censored.png");
				if (File.Exists(text2))
				{
					try
					{
						byte[] array = File.ReadAllBytes(text2);
						Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
						if (texture2D.LoadImage(array))
						{
							Sprite sprite2 = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
							sprite2.name = text;
							OxGramGallery._censoredCache[text] = sprite2;
							return sprite2;
						}
						OxGramGallery.DestroyGeneratedObject(texture2D);
					}
					catch
					{
					}
				}
				if (!requireCensored)
				{
					return photo;
				}
				return OxGramGallery.PrivacyCoverOf(photo, text);
			}
		}

		// Token: 0x0600708C RID: 28812 RVA: 0x002DB2FC File Offset: 0x002D94FC
		public static Sprite ProtectedFallbackOf(Sprite photo)
		{
			if (!(photo == null))
			{
				return OxGramGallery.PrivacyCoverOf(photo, photo.name);
			}
			return null;
		}

		// Token: 0x0600708D RID: 28813 RVA: 0x002DB318 File Offset: 0x002D9518
		private static Sprite PrivacyCoverOf(Sprite photo, string baseName)
		{
			string text = string.Concat(new string[]
			{
				baseName,
				":",
				photo.rect.width.ToString(),
				"x",
				photo.rect.height.ToString()
			});
			Sprite sprite;
			if (OxGramGallery._privacyCoverCache.TryGetValue(text, out sprite) && sprite != null)
			{
				return sprite;
			}
			float num = 128f / Mathf.Max(1f, Mathf.Max(photo.rect.width, photo.rect.height));
			int num2 = Mathf.Max(1, Mathf.RoundToInt(photo.rect.width * num));
			int num3 = Mathf.Max(1, Mathf.RoundToInt(photo.rect.height * num));
			Texture2D texture2D = new Texture2D(num2, num3, TextureFormat.RGBA32, false);
			Color32[] array = new Color32[num2 * num3];
			float num4 = (float)Mathf.Min(num2, num3) * 0.55f;
			for (int i = 0; i < num3; i++)
			{
				Color color = Color.Lerp(new Color32(24, 30, 49, byte.MaxValue), new Color32(49, 55, 81, byte.MaxValue), (float)i / (float)Mathf.Max(1, num3 - 1));
				for (int j = 0; j < num2; j++)
				{
					float num5 = ((float)j - (float)num2 * 0.5f) / Mathf.Max(1f, num4);
					float num6 = ((float)i - (float)num3 * 0.5f) / Mathf.Max(1f, num4);
					bool flag = Mathf.Abs(num5) < 0.28f && num6 > -0.3f && num6 < 0.1f;
					float magnitude = new Vector2(num5, num6 - 0.13f).magnitude;
					bool flag2 = num6 >= 0.1f && magnitude > 0.15f && magnitude < 0.23f;
					bool flag3 = Mathf.Abs(num5) < 0.025f && num6 > -0.18f && num6 < -0.05f;
					array[i * num2 + j] = (((flag || flag2) && !flag3) ? new Color32(190, 180, 239, byte.MaxValue) : color);
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			Sprite sprite2 = Sprite.Create(texture2D, new Rect(0f, 0f, (float)num2, (float)num3), new Vector2(0.5f, 0.5f), 100f);
			sprite2.name = baseName;
			OxGramGallery._privacyCoverCache[text] = sprite2;
			return sprite2;
		}

		// Token: 0x0600708E RID: 28814 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyGeneratedObject(Object generated)
		{
			if (generated == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(generated);
				return;
			}
			Object.DestroyImmediate(generated);
		}

		// Token: 0x0600708F RID: 28815 RVA: 0x002DB5E1 File Offset: 0x002D97E1
		public static bool IsPrivacyCover(Sprite photo)
		{
			return photo != null && OxGramGallery._privacyCoverCache.ContainsValue(photo);
		}

		// Token: 0x06007090 RID: 28816 RVA: 0x002DB5FC File Offset: 0x002D97FC
		public static bool HasCensoredSibling(Sprite photo)
		{
			if (photo == null)
			{
				return false;
			}
			string text;
			try
			{
				text = Path.GetFileNameWithoutExtension(photo.name);
			}
			catch
			{
				text = photo.name;
			}
			return !string.IsNullOrEmpty(text) && File.Exists(Path.Combine(OxGramGallery.GalleryPath, text + ".censored.png"));
		}

		// Token: 0x0400732F RID: 29487
		private static List<Sprite> _photos;

		// Token: 0x04007330 RID: 29488
		public const string CensoredSuffix = ".censored";

		// Token: 0x04007331 RID: 29489
		public const int ThumbnailMaxSize = 384;

		// Token: 0x04007332 RID: 29490
		private static readonly Dictionary<string, OxGramGallery.GalleryEntry> _entries = new Dictionary<string, OxGramGallery.GalleryEntry>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04007333 RID: 29491
		private static readonly Queue<OxGramGallery.GalleryEntry> _decodeQueue = new Queue<OxGramGallery.GalleryEntry>();

		// Token: 0x04007334 RID: 29492
		private static readonly List<GameObject> _pickers = new List<GameObject>();

		// Token: 0x04007335 RID: 29493
		private static int _generation;

		// Token: 0x04007336 RID: 29494
		private static bool _decoding;

		// Token: 0x04007337 RID: 29495
		private static Color32[] _clearPixels;

		// Token: 0x04007338 RID: 29496
		private static OxGramGallery.GalleryEntry _originalEntry;

		// Token: 0x04007339 RID: 29497
		private static Sprite _original;

		// Token: 0x0400733A RID: 29498
		private static readonly Dictionary<string, Sprite> _censoredCache = new Dictionary<string, Sprite>();

		// Token: 0x0400733B RID: 29499
		private static readonly Dictionary<string, Sprite> _privacyCoverCache = new Dictionary<string, Sprite>();

		// Token: 0x02000EA1 RID: 3745
		private sealed class GalleryEntry
		{
			// Token: 0x0400733C RID: 29500
			public string path;

			// Token: 0x0400733D RID: 29501
			public DateTime lastWriteUtc;

			// Token: 0x0400733E RID: 29502
			public long length;

			// Token: 0x0400733F RID: 29503
			public int width;

			// Token: 0x04007340 RID: 29504
			public int height;

			// Token: 0x04007341 RID: 29505
			public int generation;

			// Token: 0x04007342 RID: 29506
			public Sprite thumbnail;

			// Token: 0x04007343 RID: 29507
			public Texture2D texture;

			// Token: 0x04007344 RID: 29508
			public bool decoded;

			// Token: 0x04007345 RID: 29509
			public bool failed;
		}
	}
}
