using System;
using System.IO;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001110 RID: 4368
	public static class CollarIdPortraitCache
	{
		// Token: 0x0600849C RID: 33948 RVA: 0x00360A54 File Offset: 0x0035EC54
		public static string BuildKey(CustomizationPresetData preset)
		{
			if (((preset != null) ? preset.stateData : null) == null)
			{
				return null;
			}
			return Hash128.Compute(string.Concat(new string[]
			{
				"Collar ID portrait v4 skinning sync|",
				preset.presetId,
				"|",
				preset.isSystemPreset.ToString(),
				"|",
				JsonUtility.ToJson(preset.stateData)
			})).ToString();
		}

		// Token: 0x0600849D RID: 33949 RVA: 0x00360ACC File Offset: 0x0035ECCC
		public static Texture2D Load(string path, string key)
		{
			if (string.IsNullOrEmpty(key) || !File.Exists(path))
			{
				return null;
			}
			Texture2D texture2D = null;
			try
			{
				using (FileStream fileStream = File.OpenRead(path))
				{
					if (fileStream.Length > 8388736L)
					{
						return null;
					}
					using (BinaryReader binaryReader = new BinaryReader(fileStream))
					{
						if (binaryReader.ReadInt32() != 1128875057 || binaryReader.ReadInt32() != 32)
						{
							return null;
						}
						if (!string.Equals(new string(binaryReader.ReadChars(32)), key, StringComparison.Ordinal))
						{
							return null;
						}
						int num = binaryReader.ReadInt32();
						if (num <= 0 || num > 8388608 || fileStream.Length - fileStream.Position != (long)num)
						{
							return null;
						}
						byte[] array = binaryReader.ReadBytes(num);
						if (array.Length < 8 || array[0] != 137 || array[1] != 80 || array[2] != 78 || array[3] != 71 || array[4] != 13 || array[5] != 10 || array[6] != 26 || array[7] != 10)
						{
							return null;
						}
						texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false)
						{
							name = "Collar ID portrait (cached)",
							wrapMode = TextureWrapMode.Clamp
						};
						if (texture2D.LoadImage(array, true))
						{
							return texture2D;
						}
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException)
			{
			}
			if (texture2D != null)
			{
				CollarIdPortraitCache.Release(texture2D);
			}
			return null;
		}

		// Token: 0x0600849E RID: 33950 RVA: 0x00360CB0 File Offset: 0x0035EEB0
		public static void Save(string path, string key, byte[] png)
		{
			if (key == null || key.Length != 32 || png == null || png.Length == 0 || png.Length > 8388608)
			{
				throw new ArgumentException("Invalid collar portrait cache payload.");
			}
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			string text = path + ".tmp";
			try
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(File.Create(text)))
				{
					binaryWriter.Write(1128875057);
					binaryWriter.Write(32);
					binaryWriter.Write(key.ToCharArray());
					binaryWriter.Write(png.Length);
					binaryWriter.Write(png);
				}
				if (File.Exists(path))
				{
					File.Replace(text, path, null);
				}
				else
				{
					File.Move(text, path);
				}
			}
			finally
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
		}

		// Token: 0x0600849F RID: 33951 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void Release(Object asset)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(asset);
				return;
			}
			Object.DestroyImmediate(asset);
		}

		// Token: 0x04008762 RID: 34658
		private const int Magic = 1128875057;

		// Token: 0x04008763 RID: 34659
		private const int MaximumPngBytes = 8388608;
	}
}
