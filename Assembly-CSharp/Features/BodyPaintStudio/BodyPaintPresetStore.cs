using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.BodyPaintStudio
{
	// Token: 0x02001516 RID: 5398
	public static class BodyPaintPresetStore
	{
		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x0600A6C7 RID: 42695 RVA: 0x0045B95B File Offset: 0x00459B5B
		public static string DefaultRoot
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "BodyPaintPresets");
			}
		}

		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x0600A6C8 RID: 42696 RVA: 0x0045B96C File Offset: 0x00459B6C
		public static string ExportRoot
		{
			get
			{
				return Path.Combine(Path.GetDirectoryName(PlayerMediaExport.RootDirectory), "Bodypaints");
			}
		}

		// Token: 0x0600A6C9 RID: 42697 RVA: 0x0045B984 File Offset: 0x00459B84
		public static List<BodyPaintPresetInfo> ListPresets(string root = null)
		{
			string text = BodyPaintPresetStore.ResolveRoot(root);
			List<BodyPaintPresetInfo> list = new List<BodyPaintPresetInfo>();
			if (!Directory.Exists(text))
			{
				return list;
			}
			foreach (string text2 in Directory.GetDirectories(text))
			{
				string fileName = Path.GetFileName(text2);
				if (BodyPaintPresetStore.IsValidId(fileName))
				{
					try
					{
						BodyPaintPresetStore.EnsureOrdinaryDirectory(text2);
						BodyPaintPresetInfo bodyPaintPresetInfo = BodyPaintPresetStore.ReadInfo(text2, fileName);
						BodyPaintPresetStore.CheckImageHeader(Path.Combine(text2, "body.png"), bodyPaintPresetInfo.bodyWidth, bodyPaintPresetInfo.bodyHeight, true);
						BodyPaintPresetStore.CheckImageHeader(Path.Combine(text2, "head.png"), bodyPaintPresetInfo.headWidth, bodyPaintPresetInfo.headHeight, true);
						list.Add(bodyPaintPresetInfo);
					}
					catch (Exception ex) when (BodyPaintPresetStore.IsFileError(ex))
					{
						Debug.LogWarning("[BodyPaintPresets] Skipped an unreadable preset " + fileName + ": " + ex.Message);
					}
				}
			}
			list.Sort((BodyPaintPresetInfo a, BodyPaintPresetInfo b) => string.CompareOrdinal(b.createdUtc, a.createdUtc));
			return list;
		}

		// Token: 0x0600A6CA RID: 42698 RVA: 0x0045BAAC File Offset: 0x00459CAC
		public static BodyPaintPresetInfo Save(string name, byte[] bodyPng, byte[] headPng, byte[] previewPng, string root = null)
		{
			BodyPaintPresetInfo bodyPaintPresetInfo = BodyPaintPresetStore.CreateInfo(name, bodyPng, headPng, previewPng);
			string text = BodyPaintPresetStore.ResolveRoot(root);
			string text2 = BodyPaintPresetStore.PresetDirectory(bodyPaintPresetInfo.id, text);
			BodyPaintPresetStore.CommitDirectory(text, text2, bodyPaintPresetInfo, bodyPng, headPng, previewPng, false);
			return bodyPaintPresetInfo;
		}

		// Token: 0x0600A6CB RID: 42699 RVA: 0x0045BAE8 File Offset: 0x00459CE8
		public static BodyPaintPresetFiles Load(string id, string root = null)
		{
			string text = BodyPaintPresetStore.PresetDirectory(id, BodyPaintPresetStore.ResolveRoot(root));
			BodyPaintPresetStore.EnsureOrdinaryDirectory(text);
			BodyPaintPresetInfo bodyPaintPresetInfo = BodyPaintPresetStore.ReadInfo(text, id);
			byte[] array = BodyPaintPresetStore.ReadPng(Path.Combine(text, "body.png"), true);
			byte[] array2 = BodyPaintPresetStore.ReadPng(Path.Combine(text, "head.png"), true);
			BodyPaintPresetStore.EnsureDimensions(array, bodyPaintPresetInfo.bodyWidth, bodyPaintPresetInfo.bodyHeight);
			BodyPaintPresetStore.EnsureDimensions(array2, bodyPaintPresetInfo.headWidth, bodyPaintPresetInfo.headHeight);
			BodyPaintPresetStore.CheckHash(array, bodyPaintPresetInfo.bodySha256);
			BodyPaintPresetStore.CheckHash(array2, bodyPaintPresetInfo.headSha256);
			return new BodyPaintPresetFiles
			{
				Info = bodyPaintPresetInfo,
				BodyPng = array,
				HeadPng = array2,
				PreviewPng = BodyPaintPresetStore.ReadPreview(text, bodyPaintPresetInfo)
			};
		}

		// Token: 0x0600A6CC RID: 42700 RVA: 0x0045BB98 File Offset: 0x00459D98
		public static BodyPaintPresetFiles ReadImportFolder(string folder, int bodyWidth, int bodyHeight, int headWidth, int headHeight)
		{
			BodyPaintPresetFiles bodyPaintPresetFiles;
			try
			{
				if (string.IsNullOrWhiteSpace(folder))
				{
					throw new BodyPaintImportException(BodyPaintImportError.UnreadableFolder, "Choose a folder containing body.png and head.png.", null);
				}
				string fullPath = Path.GetFullPath(folder);
				if (!Directory.Exists(fullPath))
				{
					throw new BodyPaintImportException(BodyPaintImportError.UnreadableFolder, "The selected bodypaint folder could not be opened.", null);
				}
				byte[] array = BodyPaintPresetStore.ReadImportPng(Path.Combine(fullPath, "body.png"));
				byte[] array2 = BodyPaintPresetStore.ReadImportPng(Path.Combine(fullPath, "head.png"));
				if ((ulong)BodyPaintPresetStore.ReadUInt32(array, 16) != (ulong)((long)bodyWidth) || (ulong)BodyPaintPresetStore.ReadUInt32(array, 20) != (ulong)((long)bodyHeight) || (ulong)BodyPaintPresetStore.ReadUInt32(array2, 16) != (ulong)((long)headWidth) || (ulong)BodyPaintPresetStore.ReadUInt32(array2, 20) != (ulong)((long)headHeight))
				{
					throw new BodyPaintImportException(BodyPaintImportError.IncompatibleSize, "These images use different body or head dimensions. Choose an export from the custom bodypaint pen.", null);
				}
				BodyPaintPresetStore.ValidateDecodedImport(array, bodyWidth, bodyHeight);
				BodyPaintPresetStore.ValidateDecodedImport(array2, headWidth, headHeight);
				bodyPaintPresetFiles = new BodyPaintPresetFiles
				{
					BodyPng = array,
					HeadPng = array2
				};
			}
			catch (BodyPaintImportException)
			{
				throw;
			}
			catch (FileNotFoundException ex)
			{
				throw new BodyPaintImportException(BodyPaintImportError.MissingFiles, "The selected folder must contain both body.png and head.png.", ex);
			}
			catch (InvalidDataException ex2)
			{
				throw new BodyPaintImportException(BodyPaintImportError.InvalidImages, "One of the paint images is damaged or is not an 8-bit RGBA PNG.", ex2);
			}
			catch (Exception ex3) when (BodyPaintPresetStore.IsFileError(ex3))
			{
				throw new BodyPaintImportException(BodyPaintImportError.UnreadableFolder, "The selected bodypaint folder could not be read.", ex3);
			}
			return bodyPaintPresetFiles;
		}

		// Token: 0x0600A6CD RID: 42701 RVA: 0x0045BCE0 File Offset: 0x00459EE0
		private static byte[] ReadImportPng(string path)
		{
			byte[] array;
			using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				if (fileStream.Length < 57L || fileStream.Length > 83886080L)
				{
					throw new InvalidDataException("The paint image is missing, incomplete, or too large.");
				}
				array = new byte[(int)fileStream.Length];
				int num;
				for (int i = 0; i < array.Length; i += num)
				{
					num = fileStream.Read(array, i, array.Length - i);
					if (num == 0)
					{
						throw new InvalidDataException("A paint image is incomplete.");
					}
				}
				if (fileStream.ReadByte() != -1)
				{
					throw new InvalidDataException("A paint image changed while it was being read.");
				}
			}
			int num2;
			int num3;
			BodyPaintPresetStore.ValidatePng(array, true, out num2, out num3);
			BodyPaintPresetStore.ValidatePngChecksums(array);
			return array;
		}

		// Token: 0x0600A6CE RID: 42702 RVA: 0x0045BD94 File Offset: 0x00459F94
		private static void ValidateDecodedImport(byte[] png, int width, int height)
		{
			Texture2D texture2D = null;
			try
			{
				texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
				if (!texture2D.LoadImage(png, true) || texture2D.width != width || texture2D.height != height)
				{
					throw new InvalidDataException("A paint image could not be decoded.");
				}
			}
			catch (UnityException ex)
			{
				throw new InvalidDataException("A paint image could not be decoded.", ex);
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
		}

		// Token: 0x0600A6CF RID: 42703 RVA: 0x0045BE20 File Offset: 0x0045A020
		private static void ValidatePngChecksums(byte[] png)
		{
			int num3;
			for (int i = 8; i < png.Length; i = num3 + 4)
			{
				int num = (int)BodyPaintPresetStore.ReadUInt32(png, i);
				uint num2 = uint.MaxValue;
				num3 = i + 8 + num;
				for (int j = i + 4; j < num3; j++)
				{
					num2 = BodyPaintPresetStore.PngCrcTable[(int)((num2 ^ (uint)png[j]) & 255U)] ^ (num2 >> 8);
				}
				if ((num2 ^ 4294967295U) != BodyPaintPresetStore.ReadUInt32(png, num3))
				{
					throw new InvalidDataException("A paint image has a damaged PNG chunk.");
				}
			}
		}

		// Token: 0x0600A6D0 RID: 42704 RVA: 0x0045BE8C File Offset: 0x0045A08C
		private static uint[] CreatePngCrcTable()
		{
			uint[] array = new uint[256];
			uint num = 0U;
			while ((ulong)num < (ulong)((long)array.Length))
			{
				uint num2 = num;
				for (int i = 0; i < 8; i++)
				{
					num2 = (((num2 & 1U) != 0U) ? (3988292384U ^ (num2 >> 1)) : (num2 >> 1));
				}
				array[(int)num] = num2;
				num += 1U;
			}
			return array;
		}

		// Token: 0x0600A6D1 RID: 42705 RVA: 0x0045BEDB File Offset: 0x0045A0DB
		public static byte[] LoadPreview(string id, string root = null)
		{
			string text = BodyPaintPresetStore.PresetDirectory(id, BodyPaintPresetStore.ResolveRoot(root));
			BodyPaintPresetStore.EnsureOrdinaryDirectory(text);
			return BodyPaintPresetStore.ReadPreview(text, BodyPaintPresetStore.ReadInfo(text, id));
		}

		// Token: 0x0600A6D2 RID: 42706 RVA: 0x0045BEFC File Offset: 0x0045A0FC
		public static void Delete(string id, string root = null)
		{
			string text = BodyPaintPresetStore.PresetDirectory(id, BodyPaintPresetStore.ResolveRoot(root));
			if (!Directory.Exists(text))
			{
				return;
			}
			BodyPaintPresetStore.DeleteOwnedDirectory(text);
		}

		// Token: 0x0600A6D3 RID: 42707 RVA: 0x0045BF25 File Offset: 0x0045A125
		public static string ExportToGameFolder(string name, byte[] bodyPng, byte[] headPng)
		{
			return BodyPaintPresetStore.ExportToDirectory(name, bodyPng, headPng, BodyPaintPresetStore.ExportRoot);
		}

		// Token: 0x0600A6D4 RID: 42708 RVA: 0x0045BF34 File Offset: 0x0045A134
		public static string ExportToDirectory(string name, byte[] bodyPng, byte[] headPng, string exportRoot)
		{
			BodyPaintPresetInfo bodyPaintPresetInfo = BodyPaintPresetStore.CreateInfo(name, bodyPng, headPng, null);
			string text = BodyPaintPresetStore.ResolveRoot(exportRoot);
			string text2 = string.Concat(new string[]
			{
				"Bodypaint_",
				BodyPaintPresetStore.SafeFileName(bodyPaintPresetInfo.name),
				"_",
				DateTime.UtcNow.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture),
				"_",
				bodyPaintPresetInfo.id.Substring(0, 8)
			});
			string text3 = BodyPaintPresetStore.ConfinedChild(text, text2);
			BodyPaintPresetStore.CommitDirectory(text, text3, bodyPaintPresetInfo, bodyPng, headPng, null, true);
			return text3;
		}

		// Token: 0x0600A6D5 RID: 42709 RVA: 0x0045BFC4 File Offset: 0x0045A1C4
		private static BodyPaintPresetInfo CreateInfo(string name, byte[] body, byte[] head, byte[] preview)
		{
			int num;
			int num2;
			BodyPaintPresetStore.ValidatePng(body, true, out num, out num2);
			int num3;
			int num4;
			BodyPaintPresetStore.ValidatePng(head, true, out num3, out num4);
			if (preview != null)
			{
				int num5;
				int num6;
				BodyPaintPresetStore.ValidatePng(preview, false, out num5, out num6);
			}
			return new BodyPaintPresetInfo
			{
				schemaVersion = 1,
				uvLayout = "ciel-custom-paint-uv-v1",
				id = Guid.NewGuid().ToString("N"),
				name = BodyPaintPresetStore.NormalizeName(name),
				createdUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture),
				bodyWidth = num,
				bodyHeight = num2,
				headWidth = num3,
				headHeight = num4,
				bodySha256 = BodyPaintPresetStore.Hash(body),
				headSha256 = BodyPaintPresetStore.Hash(head),
				previewSha256 = ((preview == null) ? null : BodyPaintPresetStore.Hash(preview))
			};
		}

		// Token: 0x0600A6D6 RID: 42710 RVA: 0x0045C098 File Offset: 0x0045A298
		private static void CommitDirectory(string library, string destination, BodyPaintPresetInfo info, byte[] body, byte[] head, byte[] preview, bool texturesOnly)
		{
			Directory.CreateDirectory(library);
			string text = BodyPaintPresetStore.ConfinedChild(library, ".pending-" + Guid.NewGuid().ToString("N"));
			try
			{
				Directory.CreateDirectory(text);
				File.WriteAllBytes(Path.Combine(text, "body.png"), body);
				File.WriteAllBytes(Path.Combine(text, "head.png"), head);
				if (!texturesOnly)
				{
					if (preview != null)
					{
						File.WriteAllBytes(Path.Combine(text, "preview.png"), preview);
					}
					File.WriteAllText(Path.Combine(text, "preset.json"), JsonUtility.ToJson(info, true), new UTF8Encoding(false));
				}
				Directory.Move(text, destination);
			}
			catch (Exception ex) when (BodyPaintPresetStore.IsFileError(ex))
			{
				if (Directory.Exists(text))
				{
					try
					{
						BodyPaintPresetStore.DeleteOwnedDirectory(text);
					}
					catch (Exception ex2) when (BodyPaintPresetStore.IsFileError(ex2))
					{
						Debug.LogWarning("[BodyPaintPresets] Could not remove an incomplete save: " + ex2.Message);
					}
				}
				throw new IOException("The bodypaint could not be saved. Check that the destination is available and has free space.", ex);
			}
		}

		// Token: 0x0600A6D7 RID: 42711 RVA: 0x0045C1BC File Offset: 0x0045A3BC
		private static BodyPaintPresetInfo ReadInfo(string directory, string expectedId)
		{
			string text = Path.Combine(directory, "preset.json");
			BodyPaintPresetStore.CheckFileSize(text, 16384);
			BodyPaintPresetInfo bodyPaintPresetInfo;
			try
			{
				bodyPaintPresetInfo = JsonUtility.FromJson<BodyPaintPresetInfo>(File.ReadAllText(text, Encoding.UTF8));
			}
			catch (ArgumentException ex)
			{
				throw new InvalidDataException("The preset information is damaged.", ex);
			}
			if (bodyPaintPresetInfo == null || bodyPaintPresetInfo.schemaVersion != 1 || bodyPaintPresetInfo.uvLayout != "ciel-custom-paint-uv-v1")
			{
				throw new InvalidDataException("This preset uses an unsupported bodypaint format.");
			}
			DateTime dateTime;
			if (bodyPaintPresetInfo.id != expectedId || !BodyPaintPresetStore.IsValidId(bodyPaintPresetInfo.id) || string.IsNullOrWhiteSpace(bodyPaintPresetInfo.name) || bodyPaintPresetInfo.name.Length > 64 || bodyPaintPresetInfo.name != BodyPaintPresetStore.NormalizeName(bodyPaintPresetInfo.name) || !DateTime.TryParseExact(bodyPaintPresetInfo.createdUtc, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dateTime) || dateTime.Kind != DateTimeKind.Utc || !BodyPaintPresetStore.IsValidHash(bodyPaintPresetInfo.bodySha256) || !BodyPaintPresetStore.IsValidHash(bodyPaintPresetInfo.headSha256) || (!string.IsNullOrEmpty(bodyPaintPresetInfo.previewSha256) && !BodyPaintPresetStore.IsValidHash(bodyPaintPresetInfo.previewSha256)))
			{
				throw new InvalidDataException("The preset information is damaged.");
			}
			BodyPaintPresetStore.ValidateDimensions(bodyPaintPresetInfo.bodyWidth, bodyPaintPresetInfo.bodyHeight);
			BodyPaintPresetStore.ValidateDimensions(bodyPaintPresetInfo.headWidth, bodyPaintPresetInfo.headHeight);
			return bodyPaintPresetInfo;
		}

		// Token: 0x0600A6D8 RID: 42712 RVA: 0x0045C31C File Offset: 0x0045A51C
		private static byte[] ReadPreview(string directory, BodyPaintPresetInfo info)
		{
			if (string.IsNullOrEmpty(info.previewSha256))
			{
				return null;
			}
			byte[] array2;
			try
			{
				byte[] array = BodyPaintPresetStore.ReadPng(Path.Combine(directory, "preview.png"), false);
				BodyPaintPresetStore.CheckHash(array, info.previewSha256);
				array2 = array;
			}
			catch (Exception ex) when (BodyPaintPresetStore.IsFileError(ex))
			{
				Debug.LogWarning("[BodyPaintPresets] Preview unavailable: " + ex.Message);
				array2 = null;
			}
			return array2;
		}

		// Token: 0x0600A6D9 RID: 42713 RVA: 0x0045C39C File Offset: 0x0045A59C
		private static byte[] ReadPng(string path, bool needsAlpha)
		{
			BodyPaintPresetStore.CheckFileSize(path, 83886080);
			byte[] array = File.ReadAllBytes(path);
			int num;
			int num2;
			BodyPaintPresetStore.ValidatePng(array, needsAlpha, out num, out num2);
			return array;
		}

		// Token: 0x0600A6DA RID: 42714 RVA: 0x0045C3C8 File Offset: 0x0045A5C8
		private static void CheckImageHeader(string path, int expectedWidth, int expectedHeight, bool needsAlpha)
		{
			BodyPaintPresetStore.CheckFileSize(path, 83886080);
			byte[] array = new byte[33];
			using (FileStream fileStream = File.OpenRead(path))
			{
				int num;
				for (int i = 0; i < array.Length; i += num)
				{
					num = fileStream.Read(array, i, array.Length - i);
					if (num == 0)
					{
						throw new InvalidDataException("A paint texture is incomplete.");
					}
				}
			}
			int num2;
			int num3;
			BodyPaintPresetStore.ValidatePngHeader(array, needsAlpha, out num2, out num3);
			if (num2 != expectedWidth || num3 != expectedHeight)
			{
				throw new InvalidDataException("A paint texture no longer matches this preset.");
			}
		}

		// Token: 0x0600A6DB RID: 42715 RVA: 0x0045C45C File Offset: 0x0045A65C
		private static void ValidatePng(byte[] bytes, bool needsAlpha, out int width, out int height)
		{
			if (bytes == null || bytes.Length < 57 || bytes.Length > 83886080)
			{
				throw new InvalidDataException("The paint image is missing, incomplete, or too large.");
			}
			BodyPaintPresetStore.ValidatePngHeader(bytes, needsAlpha, out width, out height);
			bool flag = false;
			int i = 33;
			while (i <= bytes.Length - 12)
			{
				uint num = BodyPaintPresetStore.ReadUInt32(bytes, i);
				if (num > (uint)(bytes.Length - i - 12))
				{
					throw new InvalidDataException("A paint image is incomplete.");
				}
				bool flag2 = BodyPaintPresetStore.IsChunk(bytes, i + 4, 'I', 'D', 'A', 'T');
				bool flag3 = BodyPaintPresetStore.IsChunk(bytes, i + 4, 'I', 'E', 'N', 'D');
				if (BodyPaintPresetStore.IsChunk(bytes, i + 4, 'I', 'H', 'D', 'R'))
				{
					throw new InvalidDataException("A paint image contains an invalid header.");
				}
				flag |= flag2 && num > 0U;
				i += (int)(num + 12U);
				if (flag3)
				{
					if (num != 0U || i != bytes.Length || !flag)
					{
						throw new InvalidDataException("A paint image is incomplete.");
					}
					return;
				}
			}
			throw new InvalidDataException("A paint image is incomplete.");
		}

		// Token: 0x0600A6DC RID: 42716 RVA: 0x0045C540 File Offset: 0x0045A740
		private static void ValidatePngHeader(byte[] bytes, bool needsAlpha, out int width, out int height)
		{
			if (bytes == null || bytes.Length < 33)
			{
				throw new InvalidDataException("A paint image has no valid PNG header.");
			}
			for (int i = 0; i < BodyPaintPresetStore.PngSignature.Length; i++)
			{
				if (bytes[i] != BodyPaintPresetStore.PngSignature[i])
				{
					throw new InvalidDataException("Paint images must be PNG files.");
				}
			}
			if (BodyPaintPresetStore.ReadUInt32(bytes, 8) != 13U || !BodyPaintPresetStore.IsChunk(bytes, 12, 'I', 'H', 'D', 'R'))
			{
				throw new InvalidDataException("A paint image has no valid PNG header.");
			}
			uint num = BodyPaintPresetStore.ReadUInt32(bytes, 16);
			uint num2 = BodyPaintPresetStore.ReadUInt32(bytes, 20);
			if (num > 4096U || num2 > 4096U || num == 0U || num2 == 0U)
			{
				throw new InvalidDataException("Paint textures must be between 1 and 4096 pixels on each side.");
			}
			width = (int)num;
			height = (int)num2;
			if (bytes[24] != 8 || (bytes[25] != 6 && (needsAlpha || bytes[25] != 2)) || bytes[26] != 0 || bytes[27] != 0 || bytes[28] > 1)
			{
				throw new InvalidDataException(needsAlpha ? "Paint textures must be 8-bit RGBA PNG images." : "The preview must be an 8-bit RGB or RGBA PNG image.");
			}
		}

		// Token: 0x0600A6DD RID: 42717 RVA: 0x0045C62C File Offset: 0x0045A82C
		private static void EnsureDimensions(byte[] bytes, int width, int height)
		{
			if (BodyPaintPresetStore.ReadUInt32(bytes, 16) != (uint)width || BodyPaintPresetStore.ReadUInt32(bytes, 20) != (uint)height)
			{
				throw new InvalidDataException("A paint texture no longer matches this preset.");
			}
		}

		// Token: 0x0600A6DE RID: 42718 RVA: 0x0045C64F File Offset: 0x0045A84F
		private static void ValidateDimensions(int width, int height)
		{
			if (width < 1 || height < 1 || width > 4096 || height > 4096)
			{
				throw new InvalidDataException("The preset texture dimensions are invalid.");
			}
		}

		// Token: 0x0600A6DF RID: 42719 RVA: 0x004202F5 File Offset: 0x0041E4F5
		private static uint ReadUInt32(byte[] bytes, int offset)
		{
			return (uint)(((int)bytes[offset] << 24) | ((int)bytes[offset + 1] << 16) | ((int)bytes[offset + 2] << 8) | (int)bytes[offset + 3]);
		}

		// Token: 0x0600A6E0 RID: 42720 RVA: 0x0045C674 File Offset: 0x0045A874
		private static bool IsChunk(byte[] bytes, int offset, char a, char b, char c, char d)
		{
			return (char)bytes[offset] == a && (char)bytes[offset + 1] == b && (char)bytes[offset + 2] == c && (char)bytes[offset + 3] == d;
		}

		// Token: 0x0600A6E1 RID: 42721 RVA: 0x0045C698 File Offset: 0x0045A898
		private static string NormalizeName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in name ?? string.Empty)
			{
				if (!char.IsControl(c))
				{
					stringBuilder.Append(c);
				}
			}
			string text2 = stringBuilder.ToString().Trim();
			if (text2.Length > 64)
			{
				text2 = text2.Substring(0, 64).TrimEnd();
			}
			if (text2.Length > 0 && char.IsHighSurrogate(text2[text2.Length - 1]))
			{
				text2 = text2.Substring(0, text2.Length - 1);
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				return text2;
			}
			return "My Bodypaint";
		}

		// Token: 0x0600A6E2 RID: 42722 RVA: 0x0045C744 File Offset: 0x0045A944
		private static string SafeFileName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in name)
			{
				bool flag = char.IsControl(c) || "<>:\"/\\|?*".IndexOf(c) >= 0;
				stringBuilder.Append(flag ? '_' : c);
			}
			string text = stringBuilder.ToString().Trim().TrimEnd(new char[] { '.', ' ' });
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return "My_Bodypaint";
		}

		// Token: 0x0600A6E3 RID: 42723 RVA: 0x0045C7D4 File Offset: 0x0045A9D4
		private static string ResolveRoot(string root)
		{
			string text = root ?? BodyPaintPresetStore.DefaultRoot;
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentException("The preset destination is not valid.");
			}
			string fullPath = Path.GetFullPath(text);
			string pathRoot = Path.GetPathRoot(fullPath);
			if (!string.Equals(fullPath, pathRoot, StringComparison.OrdinalIgnoreCase))
			{
				return fullPath.TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar,
					Path.AltDirectorySeparatorChar
				});
			}
			return pathRoot;
		}

		// Token: 0x0600A6E4 RID: 42724 RVA: 0x0045C833 File Offset: 0x0045AA33
		private static string PresetDirectory(string id, string library)
		{
			if (!BodyPaintPresetStore.IsValidId(id))
			{
				throw new ArgumentException("That preset ID is not valid.", "id");
			}
			return BodyPaintPresetStore.ConfinedChild(library, id);
		}

		// Token: 0x0600A6E5 RID: 42725 RVA: 0x0045C854 File Offset: 0x0045AA54
		private static string ConfinedChild(string library, string child)
		{
			string fullPath = Path.GetFullPath(Path.Combine(library, child));
			if (!string.Equals(Path.GetDirectoryName(fullPath), library, StringComparison.OrdinalIgnoreCase))
			{
				throw new IOException("The preset destination is outside its library.");
			}
			return fullPath;
		}

		// Token: 0x0600A6E6 RID: 42726 RVA: 0x0045C87C File Offset: 0x0045AA7C
		private static bool IsValidId(string id)
		{
			if (id == null || id.Length != 32)
			{
				return false;
			}
			foreach (char c in id)
			{
				if ((c < '0' || c > '9') && (c < 'a' || c > 'f'))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A6E7 RID: 42727 RVA: 0x0045C8CC File Offset: 0x0045AACC
		private static void CheckFileSize(string path, int maximum)
		{
			FileInfo fileInfo = new FileInfo(path);
			if (!fileInfo.Exists)
			{
				throw new FileNotFoundException("A file belonging to this preset is missing.");
			}
			if ((fileInfo.Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0 || fileInfo.Length < 1L || fileInfo.Length > (long)maximum)
			{
				throw new InvalidDataException("A file belonging to this preset is invalid or too large.");
			}
		}

		// Token: 0x0600A6E8 RID: 42728 RVA: 0x0045C920 File Offset: 0x0045AB20
		private static void EnsureOrdinaryDirectory(string directory)
		{
			if (!Directory.Exists(directory))
			{
				throw new DirectoryNotFoundException("This bodypaint preset could not be found.");
			}
			if ((File.GetAttributes(directory) & FileAttributes.ReparsePoint) != (FileAttributes)0)
			{
				throw new IOException("Linked preset folders are not supported.");
			}
		}

		// Token: 0x0600A6E9 RID: 42729 RVA: 0x0045C94E File Offset: 0x0045AB4E
		private static void DeleteOwnedDirectory(string directory)
		{
			BodyPaintPresetStore.EnsureOrdinaryDirectory(directory);
			if (Directory.GetDirectories(directory).Length != 0)
			{
				throw new IOException("This preset contains unexpected folders and could not be deleted.");
			}
			Directory.Delete(directory, true);
		}

		// Token: 0x0600A6EA RID: 42730 RVA: 0x0045C974 File Offset: 0x0045AB74
		private static string Hash(byte[] bytes)
		{
			string text;
			using (SHA256 sha = SHA256.Create())
			{
				byte[] array = sha.ComputeHash(bytes);
				StringBuilder stringBuilder = new StringBuilder(64);
				foreach (byte b in array)
				{
					stringBuilder.Append(b.ToString("x2", CultureInfo.InvariantCulture));
				}
				text = stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x0600A6EB RID: 42731 RVA: 0x0045C9E8 File Offset: 0x0045ABE8
		private static bool IsValidHash(string hash)
		{
			if (hash == null || hash.Length != 64)
			{
				return false;
			}
			foreach (char c in hash)
			{
				if ((c < '0' || c > '9') && (c < 'a' || c > 'f'))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A6EC RID: 42732 RVA: 0x0045CA36 File Offset: 0x0045AC36
		private static void CheckHash(byte[] bytes, string expected)
		{
			if (!string.Equals(BodyPaintPresetStore.Hash(bytes), expected, StringComparison.Ordinal))
			{
				throw new InvalidDataException("A paint image has changed or is damaged. This preset was not loaded.");
			}
		}

		// Token: 0x0600A6ED RID: 42733 RVA: 0x0045CA52 File Offset: 0x0045AC52
		private static bool IsFileError(Exception exception)
		{
			return exception is IOException || exception is InvalidDataException || exception is UnauthorizedAccessException || exception is ArgumentException || exception is NotSupportedException || exception is SecurityException;
		}

		// Token: 0x0400ABB6 RID: 43958
		public const int CurrentSchemaVersion = 1;

		// Token: 0x0400ABB7 RID: 43959
		public const string CurrentUvLayout = "ciel-custom-paint-uv-v1";

		// Token: 0x0400ABB8 RID: 43960
		public const int MaximumNameLength = 64;

		// Token: 0x0400ABB9 RID: 43961
		public const int MaximumTextureSize = 4096;

		// Token: 0x0400ABBA RID: 43962
		private const int MaximumPngBytes = 83886080;

		// Token: 0x0400ABBB RID: 43963
		private const int MaximumMetadataBytes = 16384;

		// Token: 0x0400ABBC RID: 43964
		private static readonly byte[] PngSignature = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

		// Token: 0x0400ABBD RID: 43965
		private static readonly uint[] PngCrcTable = BodyPaintPresetStore.CreatePngCrcTable();
	}
}
