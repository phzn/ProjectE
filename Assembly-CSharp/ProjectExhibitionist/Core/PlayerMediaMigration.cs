using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A67 RID: 2663
	internal static class PlayerMediaMigration
	{
		// Token: 0x06004BD7 RID: 19415 RVA: 0x001D6E67 File Offset: 0x001D5067
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			PlayerMediaMigration._initialized = false;
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x001D6E70 File Offset: 0x001D5070
		internal static void EnsureInitialized()
		{
			if (PlayerMediaMigration._initialized)
			{
				return;
			}
			PlayerMediaMigration._initialized = true;
			try
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string text = (string.IsNullOrEmpty(folderPath) ? null : Path.Combine(folderPath, "JoystickyLabs", Application.productName));
				PlayerMediaMigration.Migrate(Application.persistentDataPath, text, PlayerMediaExport.RootDirectory);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[PlayerMediaMigration] Legacy media migration will retry next launch. New media can still be saved: " + ex.Message);
			}
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x001D6EE8 File Offset: 0x001D50E8
		internal static void Migrate(string persistentRoot, string legacyExports, string mediaRoot)
		{
			PlayerMediaMigration.MigrationPass migrationPass = new PlayerMediaMigration.MigrationPass();
			try
			{
				string text = PlayerMediaMigration.ResolveStateDirectory(persistentRoot, mediaRoot);
				Directory.CreateDirectory(text);
				string text2 = Path.Combine(text, ".media-library-v1");
				string text3 = Path.Combine(text, "g");
				Directory.CreateDirectory(text3);
				PlayerMediaMigration.RelocateLegacyDirectory(Path.Combine(mediaRoot, ".media-migration-v1"), text3);
				PlayerMediaMigration.RelocateLegacyFile(Path.Combine(mediaRoot, ".media-library-v1"), text2);
				if (File.Exists(text2))
				{
					return;
				}
				Directory.CreateDirectory(mediaRoot);
				string text4 = Path.Combine(new string[] { persistentRoot, "CompanionToolkit", "Common", "Data", "Media" });
				PlayerMediaMigration.CopyTree(Path.Combine(text4, "Images"), null, text3, migrationPass, (string key) => Path.Combine(mediaRoot, key.StartsWith("PHOTOBOOTH_", StringComparison.OrdinalIgnoreCase) ? "PhotoBooth" : "Smartphone", "Photos"));
				PlayerMediaMigration.CopyTree(Path.Combine(text4, "Videos"), Path.Combine(mediaRoot, "Smartphone", "Videos"), text3, migrationPass, null);
				PlayerMediaMigration.CopyTree(Path.Combine(persistentRoot, "CielOS_VFS"), Path.Combine(mediaRoot, "PC"), text3, migrationPass, null);
				if (!string.IsNullOrEmpty(legacyExports))
				{
					PlayerMediaMigration.CopyTree(Path.Combine(legacyExports, "Screenshots"), Path.Combine(mediaRoot, "FlyMode", "Photos"), text3, migrationPass, null);
					PlayerMediaMigration.CopyTree(Path.Combine(legacyExports, "Photos"), Path.Combine(mediaRoot, "PC", "Exports", "Photos"), text3, migrationPass, null);
					PlayerMediaMigration.CopyTree(Path.Combine(legacyExports, "Videos"), Path.Combine(mediaRoot, "PC", "Exports", "Videos"), text3, migrationPass, null);
				}
				if (migrationPass.Failures == 0)
				{
					PlayerMediaMigration.WriteMarker(text2, "Existing media copied. Original libraries were retained.");
				}
			}
			catch (Exception ex)
			{
				migrationPass.Failed(mediaRoot, ex);
			}
			if (migrationPass.Failures > 0)
			{
				Debug.LogWarning("[PlayerMediaMigration] " + migrationPass.Failures.ToString() + " legacy media group/folder(s) could not be copied; retrying next launch. New media can still be saved. " + migrationPass.FirstFailure);
			}
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x001D7114 File Offset: 0x001D5314
		internal static string ResolveStateDirectory(string persistentRoot, string mediaRoot)
		{
			string text = Path.GetFullPath(mediaRoot).TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			if (Path.DirectorySeparatorChar == '\\')
			{
				text = text.ToUpperInvariant();
			}
			string text2;
			using (SHA256 sha = SHA256.Create())
			{
				text2 = Path.Combine(persistentRoot, "MediaState", "v1", BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(text)), 0, 16).Replace("-", ""));
			}
			return text2;
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x001D71B0 File Offset: 0x001D53B0
		private static void RelocateLegacyDirectory(string source, string destination)
		{
			if (!Directory.Exists(source))
			{
				return;
			}
			if ((File.GetAttributes(source) & FileAttributes.ReparsePoint) != (FileAttributes)0)
			{
				throw new IOException("The old media journal is a link: " + source);
			}
			Directory.CreateDirectory(destination);
			foreach (string text in Directory.GetFiles(source))
			{
				PlayerMediaMigration.RelocateLegacyFile(text, Path.Combine(destination, Path.GetFileName(text)));
			}
			foreach (string text2 in Directory.GetDirectories(source))
			{
				PlayerMediaMigration.RelocateLegacyDirectory(text2, Path.Combine(destination, Path.GetFileName(text2)));
			}
			Directory.Delete(source);
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x001D7248 File Offset: 0x001D5448
		private static void RelocateLegacyFile(string source, string destination)
		{
			if (!File.Exists(source))
			{
				return;
			}
			if ((File.GetAttributes(source) & FileAttributes.ReparsePoint) != (FileAttributes)0)
			{
				throw new IOException("The old media journal contains a link: " + source);
			}
			if (!File.Exists(destination))
			{
				string text = Path.Combine(Path.GetDirectoryName(destination), ".relocating-" + Guid.NewGuid().ToString("N"));
				try
				{
					File.Copy(source, text, false);
					if (!PlayerMediaMigration.SameBytes(source, text))
					{
						throw new IOException("Media migration receipt changed during relocation: " + source);
					}
					File.Move(text, destination);
				}
				finally
				{
					PlayerMediaMigration.DeleteTemporary(text);
				}
			}
			if (!PlayerMediaMigration.SameBytes(source, destination))
			{
				throw new IOException("Media migration receipts differ: " + source);
			}
			File.Delete(source);
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x001D7310 File Offset: 0x001D5510
		private static void CopyTree(string source, string destination, string journal, PlayerMediaMigration.MigrationPass pass, Func<string, string> destinationForGroup = null)
		{
			try
			{
				if (Directory.Exists(source) && (File.GetAttributes(source) & FileAttributes.ReparsePoint) == (FileAttributes)0)
				{
					SortedDictionary<string, List<string>> sortedDictionary = new SortedDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
					foreach (string text in Directory.GetFiles(source))
					{
						string text2 = PlayerMediaMigration.GroupKey(text);
						List<string> list;
						if (!sortedDictionary.TryGetValue(text2, out list))
						{
							sortedDictionary.Add(text2, list = new List<string>());
						}
						list.Add(text);
					}
					foreach (KeyValuePair<string, List<string>> keyValuePair in sortedDictionary)
					{
						try
						{
							string text3 = ((destinationForGroup != null) ? destinationForGroup(keyValuePair.Key) : destination);
							PlayerMediaMigration.CopyGroup(source, keyValuePair.Key, keyValuePair.Value, text3, journal);
						}
						catch (Exception ex)
						{
							pass.Failed(Path.Combine(source, keyValuePair.Key), ex);
						}
					}
					string[] array = Directory.GetDirectories(source);
					for (int i = 0; i < array.Length; i++)
					{
						string text4 = array[i];
						string name = Path.GetFileName(text4);
						Func<string, string> func = ((destinationForGroup == null) ? null : new Func<string, string>((string key) => Path.Combine(destinationForGroup(key), name)));
						PlayerMediaMigration.CopyTree(text4, (destination != null) ? Path.Combine(destination, name) : null, journal, pass, func);
					}
				}
			}
			catch (Exception ex2)
			{
				pass.Failed(source, ex2);
			}
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x001D74E8 File Offset: 0x001D56E8
		private static string GroupKey(string path)
		{
			string text = Path.GetFileName(path);
			if (text.EndsWith(".recording", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(0, text.Length - ".recording".Length);
			}
			text = Path.GetFileNameWithoutExtension(text);
			if (text.EndsWith(".censored", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(0, text.Length - ".censored".Length);
			}
			return text;
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x001D7554 File Offset: 0x001D5754
		private static void CopyGroup(string sourceDirectory, string key, List<string> files, string destination, string journal)
		{
			string text = Path.GetFullPath(sourceDirectory) + "\n" + key;
			string text2;
			using (SHA256 sha = SHA256.Create())
			{
				text2 = Path.Combine(journal, BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "") + ".done");
			}
			if (File.Exists(text2))
			{
				return;
			}
			using (List<string>.Enumerator enumerator = files.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((File.GetAttributes(enumerator.Current) & FileAttributes.ReparsePoint) != (FileAttributes)0)
					{
						return;
					}
				}
			}
			string text3 = PlayerMediaMigration.ResolveGroupDestination(files, destination);
			Directory.CreateDirectory(text3);
			string text4 = Path.Combine(journal, ".staging-" + Guid.NewGuid().ToString("N"));
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			try
			{
				Directory.CreateDirectory(text4);
				foreach (string text5 in files)
				{
					string text6 = Path.Combine(text4, list.Count.ToString());
					list.Add(text6);
					File.Copy(text5, text6, false);
				}
				for (int i = 0; i < list.Count; i++)
				{
					string text7 = list[i];
					string text8 = Path.Combine(text3, Path.GetFileName(files[i]));
					if (File.Exists(text8))
					{
						if (!PlayerMediaMigration.SameBytes(text7, text8))
						{
							throw new IOException("Media changed during migration: " + text8);
						}
					}
					else
					{
						using (FileStream fileStream = File.OpenRead(text7))
						{
							using (FileStream fileStream2 = new FileStream(text8, FileMode.CreateNew, FileAccess.Write, FileShare.None))
							{
								list2.Add(text8);
								fileStream.CopyTo(fileStream2);
								fileStream2.Flush(true);
							}
						}
					}
				}
				PlayerMediaMigration.WriteMarker(text2, text3);
			}
			catch
			{
				foreach (string text9 in list2)
				{
					PlayerMediaMigration.DeleteTemporary(text9);
				}
				throw;
			}
			finally
			{
				foreach (string text10 in list)
				{
					PlayerMediaMigration.DeleteTemporary(text10);
				}
				try
				{
					if (Directory.Exists(text4))
					{
						Directory.Delete(text4);
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

		// Token: 0x06004BE0 RID: 19424 RVA: 0x001D78E0 File Offset: 0x001D5AE0
		private static string ResolveGroupDestination(List<string> files, string directory)
		{
			int num = 0;
			string text;
			for (;;)
			{
				text = ((num == 0) ? directory : Path.Combine(directory, (num == 1) ? "Recovered" : ("Recovered " + num.ToString())));
				bool flag = false;
				foreach (string text2 in files)
				{
					string text3 = Path.Combine(text, Path.GetFileName(text2));
					if (Directory.Exists(text3) || (File.Exists(text3) && !PlayerMediaMigration.SameBytes(text2, text3)))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				num++;
			}
			return text;
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x001D7994 File Offset: 0x001D5B94
		private static void WriteMarker(string destination, string value)
		{
			string text = Path.Combine(Path.GetDirectoryName(destination), ".writing-" + Guid.NewGuid().ToString("N"));
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(value);
				using (FileStream fileStream = new FileStream(text, FileMode.CreateNew, FileAccess.Write, FileShare.None))
				{
					fileStream.Write(bytes, 0, bytes.Length);
					fileStream.Flush(true);
				}
				File.Move(text, destination);
			}
			finally
			{
				PlayerMediaMigration.DeleteTemporary(text);
			}
		}

		// Token: 0x06004BE2 RID: 19426 RVA: 0x001D7A2C File Offset: 0x001D5C2C
		private static void DeleteTemporary(string file)
		{
			try
			{
				if (File.Exists(file))
				{
					File.Delete(file);
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x001D7A6C File Offset: 0x001D5C6C
		private static bool SameBytes(string first, string second)
		{
			if (new FileInfo(first).Length != new FileInfo(second).Length)
			{
				return false;
			}
			using (FileStream fileStream = File.OpenRead(first))
			{
				using (FileStream fileStream2 = File.OpenRead(second))
				{
					byte[] array = new byte[65536];
					byte[] array2 = new byte[array.Length];
					int num;
					while ((num = fileStream.Read(array, 0, array.Length)) > 0)
					{
						int num2;
						for (int i = 0; i < num; i += num2)
						{
							num2 = fileStream2.Read(array2, i, num - i);
							if (num2 == 0)
							{
								return false;
							}
						}
						for (int j = 0; j < num; j++)
						{
							if (array[j] != array2[j])
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04004D7F RID: 19839
		private const string Marker = ".media-library-v1";

		// Token: 0x04004D80 RID: 19840
		private const string JournalDirectory = ".media-migration-v1";

		// Token: 0x04004D81 RID: 19841
		private const string PrivateJournalDirectory = "g";

		// Token: 0x04004D82 RID: 19842
		private static bool _initialized;

		// Token: 0x02000A68 RID: 2664
		private sealed class MigrationPass
		{
			// Token: 0x06004BE4 RID: 19428 RVA: 0x001D7B4C File Offset: 0x001D5D4C
			public void Failed(string path, Exception exception)
			{
				this.Failures++;
				if (this.FirstFailure == null)
				{
					this.FirstFailure = path + ": " + exception.Message;
				}
			}

			// Token: 0x04004D83 RID: 19843
			public int Failures;

			// Token: 0x04004D84 RID: 19844
			public string FirstFailure;
		}
	}
}
