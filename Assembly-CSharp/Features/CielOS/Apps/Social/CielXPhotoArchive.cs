using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Features.OxGram;
using Features.PhoneCompanion;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.CielOS.Apps.Social
{
	// Token: 0x02001456 RID: 5206
	public sealed class CielXPhotoArchive
	{
		// Token: 0x0600A0AB RID: 41131 RVA: 0x00427F3A File Offset: 0x0042613A
		public CielXPhotoArchive(string archiveDirectory, params string[] sourceRoots)
		{
			this._directory = archiveDirectory;
			this._sourceRoots = sourceRoots ?? Array.Empty<string>();
		}

		// Token: 0x0600A0AC RID: 41132 RVA: 0x00427F5C File Offset: 0x0042615C
		public static CielXPhotoArchive CreateDefault()
		{
			string persistentDataPath = Application.persistentDataPath;
			return new CielXPhotoArchive(Path.Combine(persistentDataPath, "CielOS_State", "PeoplePhotos"), new string[]
			{
				PhonePhotoStorage.ImagesFolder,
				PlayerMediaExport.PcRootDirectory,
				Path.Combine(new string[] { persistentDataPath, "CompanionToolkit", "Common", "Data", "Media", "Images" }),
				Path.Combine(persistentDataPath, "CielOS_VFS")
			});
		}

		// Token: 0x0600A0AD RID: 41133 RVA: 0x00427FE4 File Offset: 0x004261E4
		public static string NormalizeName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			string text = Path.GetFileName(name.Replace('\\', '/'));
			string extension = Path.GetExtension(text);
			if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
			{
				text = Path.GetFileNameWithoutExtension(text);
			}
			if (text.EndsWith(".censored", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(0, text.Length - ".censored".Length);
			}
			if (!string.IsNullOrEmpty(text) && text.IndexOfAny(Path.GetInvalidFileNameChars()) < 0)
			{
				return text;
			}
			return null;
		}

		// Token: 0x0600A0AE RID: 41134 RVA: 0x00428083 File Offset: 0x00426283
		public Sprite LoadPublished(CielXPost post)
		{
			return CielXPhotoArchive.LoadSprite(this.ArchivePath(post), CielXPhotoArchive.NormalizeName((post != null) ? post.photoName : null));
		}

		// Token: 0x0600A0AF RID: 41135 RVA: 0x004280A4 File Offset: 0x004262A4
		public void SavePublished(CielXPost post, Sprite photo)
		{
			string text = this.ArchivePath(post);
			if (text == null || photo == null || photo.texture == null || OxGramGallery.IsPrivacyCover(photo))
			{
				return;
			}
			string text2 = null;
			try
			{
				Sprite sprite = this.LoadPublished(post);
				if (sprite != null)
				{
					CielXPhotoArchive.DestroyPhotoObject(sprite.texture);
					CielXPhotoArchive.DestroyPhotoObject(sprite);
				}
				else
				{
					byte[] array = photo.texture.EncodeToPNG();
					if (array != null && array.Length != 0)
					{
						Directory.CreateDirectory(this._directory);
						text2 = Path.Combine(this._directory, ".writing-" + Guid.NewGuid().ToString("N"));
						File.WriteAllBytes(text2, array);
						if (File.Exists(text))
						{
							File.Replace(text2, text, null);
						}
						else
						{
							File.Move(text2, text);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[People] Could not preserve the published photo: " + ex.Message);
			}
			finally
			{
				try
				{
					if (text2 != null && File.Exists(text2))
					{
						File.Delete(text2);
					}
				}
				catch (IOException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
		}

		// Token: 0x0600A0B0 RID: 41136 RVA: 0x004281E0 File Offset: 0x004263E0
		public void DeletePublished(CielXPost post)
		{
			string text = this.ArchivePath(post);
			if (text == null)
			{
				return;
			}
			try
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x0600A0B1 RID: 41137 RVA: 0x0042822C File Offset: 0x0042642C
		public Sprite LoadSource(string photoName, bool faceVisible)
		{
			string text = CielXPhotoArchive.NormalizeName(photoName);
			if (text == null)
			{
				return null;
			}
			bool flag;
			byte[] array = this.FindUniqueSourceBytes(text, out flag);
			if (flag)
			{
				return null;
			}
			if (!faceVisible)
			{
				bool flag2;
				byte[] array2 = this.FindUniqueSourceBytes(text + ".censored", out flag2);
				if (flag2)
				{
					return null;
				}
				Sprite sprite = CielXPhotoArchive.DecodeSprite(array2, text);
				if (sprite != null)
				{
					return sprite;
				}
			}
			Sprite sprite2 = CielXPhotoArchive.DecodeSprite(array, text);
			if (sprite2 == null || faceVisible)
			{
				return sprite2;
			}
			Sprite sprite3 = OxGramGallery.ProtectedFallbackOf(sprite2);
			CielXPhotoArchive.DestroyPhotoObject(sprite2.texture);
			CielXPhotoArchive.DestroyPhotoObject(sprite2);
			return sprite3;
		}

		// Token: 0x0600A0B2 RID: 41138 RVA: 0x004282BC File Offset: 0x004264BC
		private string ArchivePath(CielXPost post)
		{
			if (post == null || !post.HasPhoto || string.IsNullOrEmpty(post.id))
			{
				return null;
			}
			string text;
			using (SHA256 sha = SHA256.Create())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(post.id + (post.faceVisible ? ":visible" : ":protected"));
				text = Path.Combine(this._directory, BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "") + ".png");
			}
			return text;
		}

		// Token: 0x0600A0B3 RID: 41139 RVA: 0x00428364 File Offset: 0x00426564
		private byte[] FindUniqueSourceBytes(string stem, out bool ambiguous)
		{
			ambiguous = false;
			byte[] array = null;
			byte[] array2 = null;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string text in this._sourceRoots)
			{
				if (!string.IsNullOrEmpty(text))
				{
					Stack<string> stack = new Stack<string>();
					stack.Push(text);
					while (stack.Count > 0)
					{
						string text2 = stack.Pop();
						try
						{
							if (Directory.Exists(text2) && (File.GetAttributes(text2) & FileAttributes.ReparsePoint) == (FileAttributes)0 && hashSet.Add(Path.GetFullPath(text2)))
							{
								foreach (string text3 in Directory.GetFiles(text2))
								{
									string extension = Path.GetExtension(text3);
									if ((extension.Equals(".png", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) && Path.GetFileNameWithoutExtension(text3).Equals(stem, StringComparison.OrdinalIgnoreCase) && (File.GetAttributes(text3) & FileAttributes.ReparsePoint) == (FileAttributes)0)
									{
										byte[] array4 = File.ReadAllBytes(text3);
										byte[] array5;
										using (SHA256 sha = SHA256.Create())
										{
											array5 = sha.ComputeHash(array4);
										}
										if (array2 != null && !CielXPhotoArchive.EqualHash(array2, array5))
										{
											ambiguous = true;
											return null;
										}
										array = array4;
										array2 = array5;
									}
								}
								foreach (string text4 in Directory.GetDirectories(text2))
								{
									stack.Push(text4);
								}
							}
						}
						catch (IOException)
						{
						}
						catch (UnauthorizedAccessException)
						{
						}
					}
				}
			}
			return array;
		}

		// Token: 0x0600A0B4 RID: 41140 RVA: 0x00428554 File Offset: 0x00426754
		private static bool EqualHash(byte[] first, byte[] second)
		{
			if (first.Length != second.Length)
			{
				return false;
			}
			for (int i = 0; i < first.Length; i++)
			{
				if (first[i] != second[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A0B5 RID: 41141 RVA: 0x00428584 File Offset: 0x00426784
		private static Sprite LoadSprite(string path, string name)
		{
			if (path == null || !File.Exists(path))
			{
				return null;
			}
			Sprite sprite;
			try
			{
				sprite = CielXPhotoArchive.DecodeSprite(File.ReadAllBytes(path), name);
			}
			catch (IOException)
			{
				sprite = null;
			}
			catch (UnauthorizedAccessException)
			{
				sprite = null;
			}
			return sprite;
		}

		// Token: 0x0600A0B6 RID: 41142 RVA: 0x004285D8 File Offset: 0x004267D8
		private static Sprite DecodeSprite(byte[] bytes, string name)
		{
			if (bytes == null || bytes.Length == 0)
			{
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
			try
			{
				if (texture2D.LoadImage(bytes))
				{
					Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.one * 0.5f, 100f);
					sprite.name = name;
					return sprite;
				}
			}
			catch (Exception)
			{
			}
			CielXPhotoArchive.DestroyPhotoObject(texture2D);
			return null;
		}

		// Token: 0x0600A0B7 RID: 41143 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyPhotoObject(Object value)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x0400A478 RID: 42104
		private readonly string _directory;

		// Token: 0x0400A479 RID: 42105
		private readonly string[] _sourceRoots;
	}
}
