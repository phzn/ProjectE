using System;
using System.IO;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A64 RID: 2660
	public static class PlayerMediaExport
	{
		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x06004BBB RID: 19387 RVA: 0x001D68CB File Offset: 0x001D4ACB
		public static string RootDirectory
		{
			get
			{
				return PlayerMediaExport.ResolveRoot(Application.dataPath, Application.platform);
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x06004BBC RID: 19388 RVA: 0x001D68DC File Offset: 0x001D4ADC
		public static string LocationLabel
		{
			get
			{
				return "Media / Smartphone";
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06004BBD RID: 19389 RVA: 0x001D68E3 File Offset: 0x001D4AE3
		public static string PhotoBoothPhotosFolder
		{
			get
			{
				return PlayerMediaExport.GetSourceDirectory("PhotoBooth", "Photos");
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06004BBE RID: 19390 RVA: 0x001D68F4 File Offset: 0x001D4AF4
		public static string PcRootDirectory
		{
			get
			{
				return PlayerMediaExport.GetSourceDirectory("PC", null);
			}
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x001D6904 File Offset: 0x001D4B04
		internal static string ResolveRoot(string dataPath, RuntimePlatform platform)
		{
			if (string.IsNullOrWhiteSpace(dataPath))
			{
				throw new DirectoryNotFoundException("The game installation folder could not be located.");
			}
			string text = Path.GetDirectoryName(Path.GetFullPath(dataPath));
			if (platform == RuntimePlatform.OSXPlayer)
			{
				text = Path.GetDirectoryName(text);
			}
			return Path.Combine(text, "Media");
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x001D6948 File Offset: 0x001D4B48
		public static string GetDirectory(string category)
		{
			if (category != "Photos" && category != "Videos" && category != "Screenshots")
			{
				throw new ArgumentException("Unknown export category.", "category");
			}
			return PlayerMediaExport.GetSourceDirectory((category == "Screenshots") ? "FlyMode" : "Smartphone", (category == "Screenshots") ? "Photos" : category);
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x001D69BF File Offset: 0x001D4BBF
		public static string GetSourceDirectory(string source, string category = null)
		{
			string text = PlayerMediaExport.ResolveSourceDirectory(PlayerMediaExport.RootDirectory, source, category);
			PlayerMediaMigration.EnsureInitialized();
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x001D69DC File Offset: 0x001D4BDC
		internal static string ResolveSourceDirectory(string root, string source, string category = null)
		{
			if (source != "Smartphone" && source != "PC" && source != "PhotoBooth" && source != "FlyMode" && source != "NPC" && source != "CCTV")
			{
				throw new ArgumentException("Unknown media source.", "source");
			}
			if (category != null && category != "Photos" && category != "Videos")
			{
				throw new ArgumentException("Unknown media category.", "category");
			}
			if (category != null)
			{
				return Path.Combine(root, source, category);
			}
			return Path.Combine(root, source);
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x001D6A87 File Offset: 0x001D4C87
		public static string CopyPhoto(string source)
		{
			return PlayerMediaExport.CopyFile(source, PlayerMediaExport.GetDirectory("Photos"));
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x001D6A99 File Offset: 0x001D4C99
		public static string CopyVideo(string source)
		{
			return PlayerMediaExport.CopyFile(source, PlayerMediaExport.GetDirectory("Videos"));
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x001D6AAB File Offset: 0x001D4CAB
		public static string WritePhoto(byte[] image, string fileName)
		{
			return PlayerMediaExport.WriteBytes(image, PlayerMediaExport.GetDirectory("Photos"), fileName);
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x001D6ABE File Offset: 0x001D4CBE
		public static string WritePhotoBooth(byte[] image, string fileName)
		{
			return PlayerMediaExport.WriteBytes(image, PlayerMediaExport.PhotoBoothPhotosFolder, fileName);
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x001D6ACC File Offset: 0x001D4CCC
		public static string WriteNpcPhoto(byte[] image, string fileName)
		{
			return PlayerMediaExport.WriteBytes(image, PlayerMediaExport.GetSourceDirectory("NPC", "Photos"), fileName);
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x001D6AE4 File Offset: 0x001D4CE4
		public static string WriteCctvPhoto(byte[] image, string fileName)
		{
			return PlayerMediaExport.WriteBytes(image, PlayerMediaExport.GetSourceDirectory("CCTV", "Photos"), fileName);
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x001D6AFC File Offset: 0x001D4CFC
		public static string CopyPcPhoto(string source)
		{
			return PlayerMediaExport.CopyFile(source, PlayerMediaExport.GetSourceDirectory("PC", "Photos"));
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x001D6B13 File Offset: 0x001D4D13
		public static string CopyPcVideo(string source)
		{
			return PlayerMediaExport.CopyFile(source, PlayerMediaExport.GetSourceDirectory("PC", "Videos"));
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x001D6B2C File Offset: 0x001D4D2C
		internal static string CopyFile(string source, string directory)
		{
			if (!File.Exists(source))
			{
				throw new FileNotFoundException("The original media file is missing.", source);
			}
			if (string.Equals(Path.GetFullPath(source), Path.GetFullPath(Path.Combine(directory, Path.GetFileName(source))), StringComparison.OrdinalIgnoreCase))
			{
				return source;
			}
			return PlayerMediaExport.SaveFile(directory, Path.GetFileName(source), delegate(Stream destination)
			{
				using (FileStream fileStream = File.OpenRead(source))
				{
					fileStream.CopyTo(destination);
				}
			});
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x001D6BB4 File Offset: 0x001D4DB4
		internal static string WriteBytes(byte[] bytes, string directory, string fileName)
		{
			if (bytes == null || bytes.Length == 0)
			{
				throw new ArgumentException("The image is empty.", "bytes");
			}
			return PlayerMediaExport.SaveFile(directory, fileName, delegate(Stream destination)
			{
				destination.Write(bytes, 0, bytes.Length);
			});
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x001D6C04 File Offset: 0x001D4E04
		private static string SaveFile(string directory, string fileName, Action<Stream> write)
		{
			Directory.CreateDirectory(directory);
			fileName = Path.GetFileName(fileName);
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException("A filename is required.", "fileName");
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			string extension = Path.GetExtension(fileName);
			string text = Path.Combine(directory, ".export-" + Guid.NewGuid().ToString("N") + ".tmp");
			string text3;
			try
			{
				using (FileStream fileStream = new FileStream(text, FileMode.CreateNew, FileAccess.Write, FileShare.None))
				{
					write(fileStream);
					fileStream.Flush(true);
				}
				int num = 0;
				for (;;)
				{
					string text2 = Path.Combine(directory, (num == 0) ? fileName : string.Format("{0} ({1}){2}", fileNameWithoutExtension, num, extension));
					if (!File.Exists(text2))
					{
						try
						{
							File.Move(text, text2);
							text3 = text2;
							break;
						}
						catch (IOException obj) when (File.Exists(text2))
						{
						}
					}
					num++;
				}
			}
			finally
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			return text3;
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x001D6D34 File Offset: 0x001D4F34
		public static void OpenRootFolder()
		{
			PlayerMediaExport.OpenFolder(null);
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x001D6D3D File Offset: 0x001D4F3D
		public static void OpenPhotosFolder()
		{
			PlayerMediaExport.OpenFolder("Photos");
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x001D6D4A File Offset: 0x001D4F4A
		public static void OpenVideosFolder()
		{
			PlayerMediaExport.OpenFolder("Videos");
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x001D6D58 File Offset: 0x001D4F58
		public static bool OpenFolder(string category)
		{
			string text;
			try
			{
				text = ((category == null) ? PlayerMediaExport.RootDirectory : PlayerMediaExport.GetDirectory(category));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PlayerMediaExport] Could not open the export folder: " + ex.Message);
				return false;
			}
			return PlayerMediaExport.OpenDirectory(text);
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x001D6DAC File Offset: 0x001D4FAC
		public static bool OpenDirectory(string directory)
		{
			bool flag;
			try
			{
				Directory.CreateDirectory(directory);
				Application.OpenURL(new Uri(Path.GetFullPath(directory) + Path.DirectorySeparatorChar.ToString()).AbsoluteUri);
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PlayerMediaExport] Could not open the export folder: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x04004D74 RID: 19828
		public const string PhotosDirectory = "Photos";

		// Token: 0x04004D75 RID: 19829
		public const string VideosDirectory = "Videos";

		// Token: 0x04004D76 RID: 19830
		public const string ScreenshotsDirectory = "Screenshots";

		// Token: 0x04004D77 RID: 19831
		public const string SmartphoneDirectory = "Smartphone";

		// Token: 0x04004D78 RID: 19832
		public const string PcDirectory = "PC";

		// Token: 0x04004D79 RID: 19833
		public const string PhotoBoothDirectory = "PhotoBooth";

		// Token: 0x04004D7A RID: 19834
		public const string FlyModeDirectory = "FlyMode";

		// Token: 0x04004D7B RID: 19835
		public const string NpcDirectory = "NPC";

		// Token: 0x04004D7C RID: 19836
		public const string CctvDirectory = "CCTV";
	}
}
