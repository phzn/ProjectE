using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x02001436 RID: 5174
	public sealed class PersonalDiaryStore
	{
		// Token: 0x170018F0 RID: 6384
		// (get) Token: 0x06009FD3 RID: 40915 RVA: 0x0041F271 File Offset: 0x0041D471
		// (set) Token: 0x06009FD4 RID: 40916 RVA: 0x0041F279 File Offset: 0x0041D479
		public string LastLoadWarning { get; private set; }

		// Token: 0x170018F1 RID: 6385
		// (get) Token: 0x06009FD5 RID: 40917 RVA: 0x0041F282 File Offset: 0x0041D482
		public int PageCount
		{
			get
			{
				return this._library.pages.Count;
			}
		}

		// Token: 0x06009FD6 RID: 40918 RVA: 0x0041F294 File Offset: 0x0041D494
		public PersonalDiaryStore.Page GetPage(int index)
		{
			if (index < 0 || index >= this._library.pages.Count)
			{
				return null;
			}
			return PersonalDiaryStore.ClonePage(this._library.pages[index]);
		}

		// Token: 0x170018F2 RID: 6386
		// (get) Token: 0x06009FD7 RID: 40919 RVA: 0x0041F2C5 File Offset: 0x0041D4C5
		public IReadOnlyList<PersonalDiaryStore.Page> Pages
		{
			get
			{
				return PersonalDiaryStore.CloneLibrary(this._library).pages;
			}
		}

		// Token: 0x06009FD8 RID: 40920 RVA: 0x0041F2D8 File Offset: 0x0041D4D8
		public PersonalDiaryStore(string rootDirectory = null)
		{
			this._root = Path.GetFullPath(rootDirectory ?? Path.Combine(GameDataPaths.RuntimeRoot, "PersonalDiary"));
			this._filePath = Path.Combine(this._root, "diary.json");
			this._backupPath = this._filePath + ".bak";
			this._imagesDirectory = Path.Combine(this._root, "Images");
		}

		// Token: 0x06009FD9 RID: 40921 RVA: 0x0041F358 File Offset: 0x0041D558
		public bool TryLoad(out string error)
		{
			error = null;
			this.LastLoadWarning = null;
			this._recoveredBackup = false;
			bool flag;
			try
			{
				if (!File.Exists(this._filePath) && !File.Exists(this._backupPath))
				{
					this._library = new PersonalDiaryStore.Library();
					this._loaded = true;
					flag = true;
				}
				else
				{
					try
					{
						this._library = PersonalDiaryStore.ReadLibrary(this._filePath);
					}
					catch (Exception ex)
					{
						if (!File.Exists(this._backupPath))
						{
							throw;
						}
						this._library = PersonalDiaryStore.ReadLibrary(this._backupPath);
						this._recoveredBackup = true;
						this.LastLoadWarning = "The diary was restored from its previous save. " + ex.Message;
					}
					this._loaded = true;
					flag = true;
				}
			}
			catch (Exception ex2)
			{
				this._loaded = false;
				error = "The personal diary could not be opened. " + ex2.Message;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06009FDA RID: 40922 RVA: 0x0041F440 File Offset: 0x0041D640
		public PersonalDiaryStore.Page CreateDraft()
		{
			string text = DateTime.UtcNow.ToString("o");
			return new PersonalDiaryStore.Page
			{
				createdUtc = text,
				modifiedUtc = text
			};
		}

		// Token: 0x06009FDB RID: 40923 RVA: 0x0041F474 File Offset: 0x0041D674
		public static PersonalDiaryStore.Page ClonePage(PersonalDiaryStore.Page page)
		{
			if (page == null)
			{
				return null;
			}
			PersonalDiaryStore.Page page2 = new PersonalDiaryStore.Page
			{
				id = page.id,
				title = page.title,
				createdUtc = page.createdUtc,
				modifiedUtc = page.modifiedUtc,
				blocks = new List<PersonalDiaryStore.Block>()
			};
			if (page.blocks == null)
			{
				return page2;
			}
			foreach (PersonalDiaryStore.Block block in page.blocks)
			{
				List<PersonalDiaryStore.Block> blocks = page2.blocks;
				PersonalDiaryStore.Block block3;
				if (block != null)
				{
					PersonalDiaryStore.Block block2 = new PersonalDiaryStore.Block();
					block2.id = block.id;
					block2.type = block.type;
					block2.text = block.text;
					block2.richText = block.richText;
					block2.alignment = block.alignment;
					block2.imageFileName = block.imageFileName;
					block2.caption = block.caption;
					block2.imageWidth = block.imageWidth;
					block2.rotation = block.rotation;
					block2.sensitiveImage = block.sensitiveImage;
					block2.freePosition = block.freePosition;
					block3 = block2;
					block2.photoPosition = block.photoPosition;
				}
				else
				{
					block3 = null;
				}
				blocks.Add(block3);
			}
			return page2;
		}

		// Token: 0x06009FDC RID: 40924 RVA: 0x0041F5C0 File Offset: 0x0041D7C0
		public bool TrySavePage(PersonalDiaryStore.Page page, out string error)
		{
			error = null;
			if (!this._loaded && !this.TryLoad(out error))
			{
				return false;
			}
			if (page == null)
			{
				error = "There is no diary page to save.";
				return false;
			}
			PersonalDiaryStore.Library library = PersonalDiaryStore.CloneLibrary(this._library);
			PersonalDiaryStore.Page saved = PersonalDiaryStore.ClonePage(page);
			int num = library.pages.FindIndex((PersonalDiaryStore.Page item) => item.id == saved.id);
			string text = DateTime.UtcNow.ToString("o");
			saved.createdUtc = ((num >= 0) ? library.pages[num].createdUtc : (string.IsNullOrEmpty(saved.createdUtc) ? text : saved.createdUtc));
			saved.modifiedUtc = text;
			if (num < 0)
			{
				library.pages.Add(saved);
			}
			else
			{
				library.pages[num] = saved;
			}
			return this.TryCommit(library, out error);
		}

		// Token: 0x06009FDD RID: 40925 RVA: 0x0041F6B8 File Offset: 0x0041D8B8
		public bool TryDeletePage(string id, out string error)
		{
			error = null;
			if (!this._loaded && !this.TryLoad(out error))
			{
				return false;
			}
			PersonalDiaryStore.Library library = PersonalDiaryStore.CloneLibrary(this._library);
			int num = library.pages.FindIndex((PersonalDiaryStore.Page page) => page.id == id);
			if (num < 0)
			{
				error = "This diary page no longer exists.";
				return false;
			}
			library.pages.RemoveAt(num);
			return this.TryCommit(library, out error);
		}

		// Token: 0x06009FDE RID: 40926 RVA: 0x0041F730 File Offset: 0x0041D930
		private bool TryCommit(PersonalDiaryStore.Library candidate, out string error)
		{
			error = null;
			string text = null;
			bool flag;
			try
			{
				PersonalDiaryStore.ValidateLibrary(candidate);
				byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(candidate, true));
				if (bytes.Length > 33554432)
				{
					throw new InvalidDataException("The notebook is too large to save.");
				}
				Directory.CreateDirectory(this._root);
				text = Path.Combine(this._root, ".writing-" + Guid.NewGuid().ToString("N"));
				PersonalDiaryStore.WriteNewFile(text, bytes);
				if (File.Exists(this._filePath))
				{
					File.Replace(text, this._filePath, this._recoveredBackup ? null : this._backupPath);
				}
				else
				{
					File.Move(text, this._filePath);
				}
				this._library = candidate;
				this._recoveredBackup = false;
				this.LastLoadWarning = null;
				this.DiscardUnusedImages();
				flag = true;
			}
			catch (Exception ex)
			{
				error = "The diary could not be saved. " + ex.Message;
				flag = false;
			}
			finally
			{
				PersonalDiaryStore.DeleteFileIfPresent(text);
			}
			return flag;
		}

		// Token: 0x06009FDF RID: 40927 RVA: 0x0041F840 File Offset: 0x0041DA40
		public bool TryImportImage(string sourcePath, out string imageFileName, out string error)
		{
			imageFileName = null;
			error = null;
			if (!this._loaded && !this.TryLoad(out error))
			{
				return false;
			}
			Texture2D texture2D = null;
			string text = null;
			bool flag;
			try
			{
				byte[] array = PersonalDiaryStore.ReadImageBytes(sourcePath);
				string text2 = PersonalDiaryStore.ValidateImageHeader(array);
				texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
				if (!texture2D.LoadImage(array, true))
				{
					throw new InvalidDataException("This image could not be decoded.");
				}
				PersonalDiaryStore.ValidateDimensions(texture2D.width, texture2D.height);
				Directory.CreateDirectory(this._imagesDirectory);
				imageFileName = Guid.NewGuid().ToString("N") + text2;
				text = this.GetImagePath(imageFileName);
				PersonalDiaryStore.WriteNewFile(text, array);
				flag = true;
			}
			catch (Exception ex)
			{
				PersonalDiaryStore.DeleteFileIfPresent(text);
				imageFileName = null;
				error = "The photo could not be added. " + ex.Message;
				flag = false;
			}
			finally
			{
				if (texture2D != null)
				{
					if (Application.isPlaying)
					{
						Object.Destroy(texture2D);
					}
					else
					{
						Object.DestroyImmediate(texture2D);
					}
				}
			}
			return flag;
		}

		// Token: 0x06009FE0 RID: 40928 RVA: 0x0041F944 File Offset: 0x0041DB44
		public string GetImagePath(string imageFileName)
		{
			if (!PersonalDiaryStore.IsManagedImageName(imageFileName))
			{
				return null;
			}
			return Path.Combine(this._imagesDirectory, imageFileName);
		}

		// Token: 0x06009FE1 RID: 40929 RVA: 0x0041F95C File Offset: 0x0041DB5C
		public bool TryLoadImageBytes(string imageFileName, out byte[] bytes, out string error)
		{
			bytes = null;
			error = null;
			bool flag;
			try
			{
				string imagePath = this.GetImagePath(imageFileName);
				if (imagePath == null)
				{
					throw new InvalidDataException("Invalid diary image name.");
				}
				bytes = PersonalDiaryStore.ReadImageBytes(imagePath);
				PersonalDiaryStore.ValidateImageHeader(bytes);
				flag = true;
			}
			catch (Exception ex)
			{
				bytes = null;
				error = "This diary photo is unavailable. " + ex.Message;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06009FE2 RID: 40930 RVA: 0x0041F9C4 File Offset: 0x0041DBC4
		public void DiscardUnusedImages()
		{
			if (!this._loaded || !Directory.Exists(this._imagesDirectory))
			{
				return;
			}
			try
			{
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				PersonalDiaryStore.AddImageNames(this._library, hashSet);
				if (File.Exists(this._backupPath))
				{
					PersonalDiaryStore.AddImageNames(PersonalDiaryStore.ReadLibrary(this._backupPath), hashSet);
				}
				foreach (string text in Directory.GetFiles(this._imagesDirectory))
				{
					string fileName = Path.GetFileName(text);
					if (PersonalDiaryStore.IsManagedImageName(fileName) && !hashSet.Contains(fileName))
					{
						PersonalDiaryStore.DeleteFileIfPresent(text);
					}
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (ArgumentException)
			{
			}
		}

		// Token: 0x06009FE3 RID: 40931 RVA: 0x0041FA90 File Offset: 0x0041DC90
		private static PersonalDiaryStore.Library ReadLibrary(string path)
		{
			FileInfo fileInfo = new FileInfo(path);
			if (!fileInfo.Exists)
			{
				throw new FileNotFoundException("The notebook save is missing.");
			}
			if (fileInfo.Length > 33554432L)
			{
				throw new InvalidDataException("The notebook save is too large.");
			}
			string text = File.ReadAllText(path, Encoding.UTF8);
			if (string.IsNullOrWhiteSpace(text) || !text.Contains("\"version\"") || !text.Contains("\"pages\""))
			{
				throw new InvalidDataException("The notebook save is incomplete.");
			}
			PersonalDiaryStore.Library library = JsonUtility.FromJson<PersonalDiaryStore.Library>(text);
			PersonalDiaryStore.ValidateLibrary(library);
			return library;
		}

		// Token: 0x06009FE4 RID: 40932 RVA: 0x0041FB14 File Offset: 0x0041DD14
		private static void ValidateLibrary(PersonalDiaryStore.Library library)
		{
			if (library == null || library.version != 1 || library.pages == null)
			{
				throw new InvalidDataException("This notebook format is not supported.");
			}
			if (library.pages.Count > 200)
			{
				throw new InvalidDataException("A notebook can contain up to " + 200.ToString() + " pages.");
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			foreach (PersonalDiaryStore.Page page in library.pages)
			{
				if (page == null || !PersonalDiaryStore.IsId(page.id) || !hashSet.Add(page.id))
				{
					throw new InvalidDataException("A diary page has an invalid or duplicate ID.");
				}
				if ((page.title ?? "").Length > 160)
				{
					throw new InvalidDataException("A page title can contain up to " + 160.ToString() + " characters.");
				}
				if (page.blocks == null || page.blocks.Count > 100)
				{
					throw new InvalidDataException("A page can contain up to " + 100.ToString() + " blocks.");
				}
				HashSet<string> hashSet2 = new HashSet<string>(StringComparer.Ordinal);
				foreach (PersonalDiaryStore.Block block in page.blocks)
				{
					if (block == null || !PersonalDiaryStore.IsId(block.id) || !hashSet2.Add(block.id) || !Enum.IsDefined(typeof(PersonalDiaryStore.BlockType), block.type))
					{
						throw new InvalidDataException("A diary block is invalid.");
					}
					string text = block.text ?? "";
					if (text.Length > (block.richText ? 384000 : 12000) || (block.caption ?? "").Length > 500)
					{
						throw new InvalidDataException("This text is too long for one diary block.");
					}
					if (block.richText)
					{
						block.text = PersonalDiaryTextFormatting.Normalize(text);
						if (PersonalDiaryTextFormatting.ToPlainText(block.text).Length > 12000 || block.text.Length > 384000)
						{
							throw new InvalidDataException("This text is too long for one diary block.");
						}
					}
					if (block.alignment < 0 || block.alignment > 2)
					{
						throw new InvalidDataException("A diary paragraph has invalid alignment.");
					}
					if (block.type == PersonalDiaryStore.BlockType.Photo && !PersonalDiaryStore.IsManagedImageName(block.imageFileName))
					{
						throw new InvalidDataException("A diary photo has an invalid image name.");
					}
					if (float.IsNaN(block.imageWidth) || float.IsInfinity(block.imageWidth) || float.IsNaN(block.rotation) || float.IsInfinity(block.rotation))
					{
						throw new InvalidDataException("A diary photo has invalid dimensions.");
					}
					if (float.IsNaN(block.photoPosition.x) || float.IsInfinity(block.photoPosition.x) || float.IsNaN(block.photoPosition.y) || float.IsInfinity(block.photoPosition.y))
					{
						throw new InvalidDataException("A diary photo has an invalid position.");
					}
					block.imageWidth = Mathf.Clamp(block.imageWidth, 160f, 1200f);
					block.rotation = Mathf.Clamp(block.rotation, -20f, 20f);
					if (block.freePosition)
					{
						block.photoPosition = new Vector2(Mathf.Clamp(block.photoPosition.x, 0f, 1560f), Mathf.Clamp(block.photoPosition.y, 0f, 100000f));
					}
				}
			}
		}

		// Token: 0x06009FE5 RID: 40933 RVA: 0x0041FF18 File Offset: 0x0041E118
		private static PersonalDiaryStore.Library CloneLibrary(PersonalDiaryStore.Library library)
		{
			PersonalDiaryStore.Library library2 = new PersonalDiaryStore.Library
			{
				version = library.version
			};
			foreach (PersonalDiaryStore.Page page in library.pages)
			{
				library2.pages.Add(PersonalDiaryStore.ClonePage(page));
			}
			return library2;
		}

		// Token: 0x06009FE6 RID: 40934 RVA: 0x0041FF88 File Offset: 0x0041E188
		private static void AddImageNames(PersonalDiaryStore.Library library, HashSet<string> names)
		{
			foreach (PersonalDiaryStore.Page page in library.pages)
			{
				foreach (PersonalDiaryStore.Block block in page.blocks)
				{
					if (block.type == PersonalDiaryStore.BlockType.Photo)
					{
						names.Add(block.imageFileName);
					}
				}
			}
		}

		// Token: 0x06009FE7 RID: 40935 RVA: 0x00420024 File Offset: 0x0041E224
		private static bool IsId(string value)
		{
			Guid guid;
			return value != null && value.Length == 32 && Guid.TryParseExact(value, "N", out guid);
		}

		// Token: 0x06009FE8 RID: 40936 RVA: 0x00420050 File Offset: 0x0041E250
		private static bool IsManagedImageName(string value)
		{
			if (string.IsNullOrEmpty(value) || value.Length > 37)
			{
				return false;
			}
			if (value.Length < 36 || !PersonalDiaryStore.IsId(value.Substring(0, 32)))
			{
				return false;
			}
			string text = value.Substring(32);
			return text == ".png" || text == ".jpg" || text == ".jpeg";
		}

		// Token: 0x06009FE9 RID: 40937 RVA: 0x004200C0 File Offset: 0x0041E2C0
		private static byte[] ReadImageBytes(string path)
		{
			byte[] array2;
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				if (fileStream.Length == 0L || fileStream.Length > 20971520L)
				{
					throw new InvalidDataException("Choose a PNG or JPEG photo smaller than 20 MB.");
				}
				byte[] array = new byte[(int)fileStream.Length];
				int num;
				for (int i = 0; i < array.Length; i += num)
				{
					num = fileStream.Read(array, i, array.Length - i);
					if (num == 0)
					{
						throw new EndOfStreamException("The photo changed while being opened.");
					}
				}
				array2 = array;
			}
			return array2;
		}

		// Token: 0x06009FEA RID: 40938 RVA: 0x00420150 File Offset: 0x0041E350
		private static string ValidateImageHeader(byte[] bytes)
		{
			if (bytes.Length >= 24 && bytes[0] == 137 && bytes[1] == 80 && bytes[2] == 78 && bytes[3] == 71 && bytes[4] == 13 && bytes[5] == 10 && bytes[6] == 26 && bytes[7] == 10 && bytes[12] == 73 && bytes[13] == 72 && bytes[14] == 68 && bytes[15] == 82)
			{
				PersonalDiaryStore.ValidateDimensions(PersonalDiaryStore.ReadInt32(bytes, 16), PersonalDiaryStore.ReadInt32(bytes, 20));
				return ".png";
			}
			if (bytes.Length >= 4 && bytes[0] == 255 && bytes[1] == 216)
			{
				int num = 2;
				while (num + 3 < bytes.Length)
				{
					if (bytes[num++] != 255)
					{
						break;
					}
					while (num < bytes.Length && bytes[num] == 255)
					{
						num++;
					}
					if (num >= bytes.Length)
					{
						break;
					}
					int num2 = (int)bytes[num++];
					if (num2 == 217 || num2 == 218)
					{
						break;
					}
					if (num2 != 1 && (num2 < 208 || num2 > 215))
					{
						if (num + 2 > bytes.Length)
						{
							break;
						}
						int num3 = ((int)bytes[num] << 8) | (int)bytes[num + 1];
						if (num3 < 2 || num + num3 > bytes.Length)
						{
							break;
						}
						if (num2 >= 192 && num2 <= 207 && num2 != 196 && num2 != 200 && num2 != 204)
						{
							if (num3 >= 8)
							{
								int num4 = ((int)bytes[num + 3] << 8) | (int)bytes[num + 4];
								PersonalDiaryStore.ValidateDimensions(((int)bytes[num + 5] << 8) | (int)bytes[num + 6], num4);
								return ".jpg";
							}
							break;
						}
						else
						{
							num += num3;
						}
					}
				}
			}
			throw new InvalidDataException("Choose a valid PNG or JPEG photo.");
		}

		// Token: 0x06009FEB RID: 40939 RVA: 0x004202F5 File Offset: 0x0041E4F5
		private static int ReadInt32(byte[] bytes, int offset)
		{
			return ((int)bytes[offset] << 24) | ((int)bytes[offset + 1] << 16) | ((int)bytes[offset + 2] << 8) | (int)bytes[offset + 3];
		}

		// Token: 0x06009FEC RID: 40940 RVA: 0x00420314 File Offset: 0x0041E514
		private static void ValidateDimensions(int width, int height)
		{
			if (width <= 0 || height <= 0 || width > 8192 || height > 8192 || (long)width * (long)height > 33554432L)
			{
				throw new InvalidDataException("Choose a photo up to 8192 pixels per side and 32 megapixels.");
			}
		}

		// Token: 0x06009FED RID: 40941 RVA: 0x00420348 File Offset: 0x0041E548
		private static void WriteNewFile(string path, byte[] bytes)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Flush(true);
			}
		}

		// Token: 0x06009FEE RID: 40942 RVA: 0x00420390 File Offset: 0x0041E590
		private static void DeleteFileIfPresent(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x0400A3AF RID: 41903
		public const int MaxPages = 200;

		// Token: 0x0400A3B0 RID: 41904
		public const int MaxBlocksPerPage = 100;

		// Token: 0x0400A3B1 RID: 41905
		public const int MaxTitleLength = 160;

		// Token: 0x0400A3B2 RID: 41906
		public const int MaxTextLength = 12000;

		// Token: 0x0400A3B3 RID: 41907
		public const int MaxRichTextLength = 384000;

		// Token: 0x0400A3B4 RID: 41908
		public const int MaxCaptionLength = 500;

		// Token: 0x0400A3B5 RID: 41909
		public const int MaxImageBytes = 20971520;

		// Token: 0x0400A3B6 RID: 41910
		public const int MaxImageDimension = 8192;

		// Token: 0x0400A3B7 RID: 41911
		public const long MaxImagePixels = 33554432L;

		// Token: 0x0400A3B8 RID: 41912
		private const int MaxSaveBytes = 33554432;

		// Token: 0x0400A3B9 RID: 41913
		private const int CurrentVersion = 1;

		// Token: 0x0400A3BA RID: 41914
		private readonly string _root;

		// Token: 0x0400A3BB RID: 41915
		private readonly string _filePath;

		// Token: 0x0400A3BC RID: 41916
		private readonly string _backupPath;

		// Token: 0x0400A3BD RID: 41917
		private readonly string _imagesDirectory;

		// Token: 0x0400A3BE RID: 41918
		private PersonalDiaryStore.Library _library = new PersonalDiaryStore.Library();

		// Token: 0x0400A3BF RID: 41919
		private bool _loaded;

		// Token: 0x0400A3C0 RID: 41920
		private bool _recoveredBackup;

		// Token: 0x02001437 RID: 5175
		public enum BlockType
		{
			// Token: 0x0400A3C3 RID: 41923
			Paragraph,
			// Token: 0x0400A3C4 RID: 41924
			Heading1,
			// Token: 0x0400A3C5 RID: 41925
			Heading2,
			// Token: 0x0400A3C6 RID: 41926
			Photo
		}

		// Token: 0x02001438 RID: 5176
		[Serializable]
		public sealed class Block
		{
			// Token: 0x0400A3C7 RID: 41927
			public string id = Guid.NewGuid().ToString("N");

			// Token: 0x0400A3C8 RID: 41928
			public PersonalDiaryStore.BlockType type;

			// Token: 0x0400A3C9 RID: 41929
			public string text = "";

			// Token: 0x0400A3CA RID: 41930
			public bool richText;

			// Token: 0x0400A3CB RID: 41931
			public int alignment;

			// Token: 0x0400A3CC RID: 41932
			public string imageFileName = "";

			// Token: 0x0400A3CD RID: 41933
			public string caption = "";

			// Token: 0x0400A3CE RID: 41934
			public float imageWidth = 560f;

			// Token: 0x0400A3CF RID: 41935
			public float rotation = -3f;

			// Token: 0x0400A3D0 RID: 41936
			public bool sensitiveImage;

			// Token: 0x0400A3D1 RID: 41937
			public bool freePosition;

			// Token: 0x0400A3D2 RID: 41938
			public Vector2 photoPosition;
		}

		// Token: 0x02001439 RID: 5177
		[Serializable]
		public sealed class Page
		{
			// Token: 0x0400A3D3 RID: 41939
			public string id = Guid.NewGuid().ToString("N");

			// Token: 0x0400A3D4 RID: 41940
			public string title = "";

			// Token: 0x0400A3D5 RID: 41941
			public string createdUtc = "";

			// Token: 0x0400A3D6 RID: 41942
			public string modifiedUtc = "";

			// Token: 0x0400A3D7 RID: 41943
			public List<PersonalDiaryStore.Block> blocks = new List<PersonalDiaryStore.Block>();
		}

		// Token: 0x0200143A RID: 5178
		[Serializable]
		private sealed class Library
		{
			// Token: 0x0400A3D8 RID: 41944
			public int version = 1;

			// Token: 0x0400A3D9 RID: 41945
			public List<PersonalDiaryStore.Page> pages = new List<PersonalDiaryStore.Page>();
		}
	}
}
