using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Features.Workshop
{
	// Token: 0x02000B4D RID: 2893
	[Serializable]
	public sealed class WorkshopModManifest
	{
		// Token: 0x0600520F RID: 21007 RVA: 0x001FC6E4 File Offset: 0x001FA8E4
		public static bool TryRead(string folder, out WorkshopModManifest manifest, out string assemblyPath, out string error)
		{
			manifest = null;
			assemblyPath = null;
			error = string.Empty;
			bool flag;
			try
			{
				if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
				{
					throw new InvalidDataException("The installed content folder is missing.");
				}
				string text = WorkshopModManifest.ResolveContentPath(folder, "pe-mod.json");
				if (!File.Exists(text) || new FileInfo(text).Length > 65536L)
				{
					throw new InvalidDataException("A pe-mod.json manifest smaller than 64 KiB is required.");
				}
				manifest = JsonUtility.FromJson<WorkshopModManifest>(File.ReadAllText(text));
				if (manifest == null || manifest.schemaVersion != 1 || manifest.apiVersion != 1)
				{
					throw new InvalidDataException("Unsupported manifest or mod API version.");
				}
				if (string.IsNullOrWhiteSpace(manifest.name) || string.IsNullOrWhiteSpace(manifest.version) || string.IsNullOrWhiteSpace(manifest.entryType))
				{
					throw new InvalidDataException("The manifest requires name, version and entryType.");
				}
				assemblyPath = WorkshopModManifest.ResolveContentPath(folder, manifest.entryAssembly);
				if (!string.Equals(Path.GetExtension(assemblyPath), ".dll", StringComparison.OrdinalIgnoreCase) || !File.Exists(assemblyPath))
				{
					throw new InvalidDataException("entryAssembly must point to an existing managed .dll in the package.");
				}
				HashSet<ulong> hashSet = new HashSet<ulong>();
				string[] array = manifest.requiredWorkshopItems ?? Array.Empty<string>();
				for (int i = 0; i < array.Length; i++)
				{
					ulong num;
					if (!ulong.TryParse(array[i], out num) || num == 0UL || !hashSet.Add(num))
					{
						throw new InvalidDataException("requiredWorkshopItems must contain unique, non-zero Workshop IDs.");
					}
				}
				flag = true;
			}
			catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is ArgumentException || ex is UnauthorizedAccessException || ex is NotSupportedException)
			{
				manifest = null;
				assemblyPath = null;
				error = ex.Message;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x001FC8B0 File Offset: 0x001FAAB0
		public static string ResolveContentPath(string folder, string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath) || Path.IsPathRooted(relativePath) || relativePath.IndexOf(':') >= 0)
			{
				throw new InvalidDataException("Mod files must use relative paths inside the package.");
			}
			string[] array = relativePath.Replace('\\', '/').Split('/', StringSplitOptions.None);
			foreach (string text in array)
			{
				if (text == ".." || text == "." || string.IsNullOrWhiteSpace(text))
				{
					throw new InvalidDataException("Mod file paths cannot traverse directories.");
				}
			}
			string text2 = Path.GetFullPath(folder).TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			if ((File.GetAttributes(text2) & FileAttributes.ReparsePoint) != (FileAttributes)0)
			{
				throw new InvalidDataException("Linked mod folders are not supported.");
			}
			string text3 = text2;
			foreach (string text4 in array)
			{
				text3 = Path.Combine(text3, text4);
				if ((File.Exists(text3) || Directory.Exists(text3)) && (File.GetAttributes(text3) & FileAttributes.ReparsePoint) != (FileAttributes)0)
				{
					throw new InvalidDataException("Linked mod files are not supported.");
				}
			}
			return Path.GetFullPath(text3);
		}

		// Token: 0x040052A9 RID: 21161
		public const string FileName = "pe-mod.json";

		// Token: 0x040052AA RID: 21162
		public int schemaVersion;

		// Token: 0x040052AB RID: 21163
		public int apiVersion;

		// Token: 0x040052AC RID: 21164
		public string name;

		// Token: 0x040052AD RID: 21165
		public string version;

		// Token: 0x040052AE RID: 21166
		public string gameVersion;

		// Token: 0x040052AF RID: 21167
		public string entryAssembly;

		// Token: 0x040052B0 RID: 21168
		public string entryType;

		// Token: 0x040052B1 RID: 21169
		public string[] requiredWorkshopItems;
	}
}
