using System;
using System.IO;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E4A RID: 3658
	public static class PhoneVideoStorage
	{
		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x06006D4C RID: 27980 RVA: 0x002BD7E6 File Offset: 0x002BB9E6
		public static string VideosFolder
		{
			get
			{
				return PhonePhotoStorage.VideosFolder;
			}
		}

		// Token: 0x06006D4D RID: 27981 RVA: 0x002BD7F0 File Offset: 0x002BB9F0
		public static string CreateVideoPath()
		{
			string text = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
			return Path.Combine(PhoneVideoStorage.VideosFolder, "VID_" + text + ".avi");
		}

		// Token: 0x06006D4E RID: 27982 RVA: 0x002BD82A File Offset: 0x002BBA2A
		public static string ThumbnailPathFor(string videoPath)
		{
			return Path.ChangeExtension(videoPath, ".jpg");
		}

		// Token: 0x06006D4F RID: 27983 RVA: 0x002BD837 File Offset: 0x002BBA37
		public static string MetadataPathFor(string videoPath)
		{
			return Path.ChangeExtension(videoPath, ".json");
		}

		// Token: 0x06006D50 RID: 27984 RVA: 0x002BD844 File Offset: 0x002BBA44
		public static void SaveMetadata(string videoPath, PhoneVideoMetadata metadata)
		{
			if (metadata == null || string.IsNullOrEmpty(videoPath))
			{
				return;
			}
			metadata.fileName = Path.GetFileName(videoPath);
			File.WriteAllText(PhoneVideoStorage.MetadataPathFor(videoPath), JsonUtility.ToJson(metadata, true));
		}

		// Token: 0x06006D51 RID: 27985 RVA: 0x002BD870 File Offset: 0x002BBA70
		public static PhoneVideoMetadata LoadMetadata(string videoPath)
		{
			if (string.IsNullOrEmpty(videoPath))
			{
				return null;
			}
			string text = PhoneVideoStorage.MetadataPathFor(videoPath);
			if (File.Exists(text))
			{
				try
				{
					PhoneVideoMetadata phoneVideoMetadata = JsonUtility.FromJson<PhoneVideoMetadata>(File.ReadAllText(text));
					if (phoneVideoMetadata != null)
					{
						return phoneVideoMetadata;
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("[PhoneVideoStorage] Metadata unreadable for " + videoPath + ": " + ex.Message);
				}
			}
			return PhoneVideoStorage.ReadAviMetadata(videoPath);
		}

		// Token: 0x06006D52 RID: 27986 RVA: 0x002BD8E4 File Offset: 0x002BBAE4
		public static void DeleteVideo(string videoPath)
		{
			if (string.IsNullOrEmpty(videoPath))
			{
				return;
			}
			PhoneVideoStorage.DeleteIfPresent(videoPath);
			PhoneVideoStorage.DeleteIfPresent(PhoneVideoStorage.ThumbnailPathFor(videoPath));
			PhoneVideoStorage.DeleteIfPresent(PhoneVideoStorage.MetadataPathFor(videoPath));
			PhoneVideoStorage.DeleteIfPresent(videoPath + ".recording");
		}

		// Token: 0x06006D53 RID: 27987 RVA: 0x002BD91C File Offset: 0x002BBB1C
		private static void DeleteIfPresent(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PhoneVideoStorage] Cannot delete " + path + ": " + ex.Message);
			}
		}

		// Token: 0x06006D54 RID: 27988 RVA: 0x002BD968 File Offset: 0x002BBB68
		private static PhoneVideoMetadata ReadAviMetadata(string videoPath)
		{
			if (!File.Exists(videoPath))
			{
				return null;
			}
			try
			{
				using (FileStream fileStream = File.OpenRead(videoPath))
				{
					using (BinaryReader binaryReader = new BinaryReader(fileStream))
					{
						long num = Math.Min(fileStream.Length, 262144L);
						while (fileStream.Position + 8L <= num)
						{
							uint num2 = binaryReader.ReadUInt32();
							uint num3 = binaryReader.ReadUInt32();
							if (num2 == PhoneVideoStorage.FourCc("avih") && num3 >= 40U)
							{
								uint num4 = binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								uint num5 = binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								int num6 = binaryReader.ReadInt32();
								int num7 = binaryReader.ReadInt32();
								float num8 = ((num4 > 0U) ? (1000000f / num4) : 0f);
								return new PhoneVideoMetadata
								{
									fileName = Path.GetFileName(videoPath),
									createdUtc = File.GetCreationTimeUtc(videoPath).ToString("o"),
									durationSeconds = ((num8 > 0f) ? (num5 / num8) : 0f),
									width = num6,
									height = num7,
									frameRate = Mathf.RoundToInt(num8),
									hasAudio = true
								};
							}
							fileStream.Position = Math.Min(fileStream.Position - 7L, num);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PhoneVideoStorage] AVI header unreadable for " + videoPath + ": " + ex.Message);
			}
			return null;
		}

		// Token: 0x06006D55 RID: 27989 RVA: 0x002BDB4C File Offset: 0x002BBD4C
		private static uint FourCc(string value)
		{
			return (uint)((int)((byte)value[0]) | ((int)((byte)value[1]) << 8) | ((int)((byte)value[2]) << 16) | ((int)((byte)value[3]) << 24));
		}
	}
}
