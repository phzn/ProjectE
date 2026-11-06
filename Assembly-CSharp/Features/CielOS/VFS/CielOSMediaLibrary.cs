using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Features.CielOS.VFS
{
	// Token: 0x020013A5 RID: 5029
	public static class CielOSMediaLibrary
	{
		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x06009BC8 RID: 39880 RVA: 0x003FA5E7 File Offset: 0x003F87E7
		private static string StatePath
		{
			get
			{
				string text = Path.Combine(Application.persistentDataPath, "CielOS_State");
				Directory.CreateDirectory(text);
				return Path.Combine(text, "media-order.json");
			}
		}

		// Token: 0x06009BC9 RID: 39881 RVA: 0x003FA60C File Offset: 0x003F880C
		public static bool IsVideoFileName(string fileName)
		{
			string extension = Path.GetExtension(fileName ?? string.Empty);
			return extension.Equals(".avi", StringComparison.OrdinalIgnoreCase) || extension.Equals(".mp4", StringComparison.OrdinalIgnoreCase) || extension.Equals(".mov", StringComparison.OrdinalIgnoreCase) || extension.Equals(".m4v", StringComparison.OrdinalIgnoreCase) || extension.Equals(".webm", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06009BCA RID: 39882 RVA: 0x003FA670 File Offset: 0x003F8870
		public static List<CielOSFileSystem.Entry> ApplyOrder(string virtualDirectory, IEnumerable<CielOSFileSystem.Entry> source)
		{
			List<CielOSFileSystem.Entry> list = ((source != null) ? new List<CielOSFileSystem.Entry>(source) : new List<CielOSFileSystem.Entry>());
			CielOSMediaLibrary.DirectoryOrder directoryOrder = CielOSMediaLibrary.FindRecord(virtualDirectory, false);
			Dictionary<string, int> positions = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			if (directoryOrder != null)
			{
				for (int i = 0; i < directoryOrder.fileNames.Count; i++)
				{
					if (!positions.ContainsKey(directoryOrder.fileNames[i]))
					{
						positions.Add(directoryOrder.fileNames[i], i);
					}
				}
			}
			list.Sort(delegate(CielOSFileSystem.Entry a, CielOSFileSystem.Entry b)
			{
				if (a.isDirectory != b.isDirectory)
				{
					if (!a.isDirectory)
					{
						return 1;
					}
					return -1;
				}
				else
				{
					if (a.isDirectory)
					{
						return StringComparer.OrdinalIgnoreCase.Compare(a.name, b.name);
					}
					int num;
					bool flag = positions.TryGetValue(a.name, out num);
					int num2;
					bool flag2 = positions.TryGetValue(b.name, out num2);
					if (flag && flag2)
					{
						return num.CompareTo(num2);
					}
					if (flag != flag2)
					{
						if (!flag)
						{
							return 1;
						}
						return -1;
					}
					else
					{
						DateTime dateTime = CielOSMediaLibrary.SafeWriteTime(a.virtualPath);
						int num3 = CielOSMediaLibrary.SafeWriteTime(b.virtualPath).CompareTo(dateTime);
						if (num3 == 0)
						{
							return StringComparer.OrdinalIgnoreCase.Compare(a.name, b.name);
						}
						return num3;
					}
				}
			});
			return list;
		}

		// Token: 0x06009BCB RID: 39883 RVA: 0x003FA70C File Offset: 0x003F890C
		public static void SaveOrder(string virtualDirectory, IEnumerable<string> orderedVirtualPaths)
		{
			CielOSMediaLibrary.DirectoryOrder directoryOrder = CielOSMediaLibrary.FindRecord(virtualDirectory, true);
			directoryOrder.fileNames.Clear();
			if (orderedVirtualPaths != null)
			{
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (string text in orderedVirtualPaths)
				{
					string fileName = Path.GetFileName((text ?? string.Empty).TrimEnd(new char[] { '/', '\\' }));
					if (!string.IsNullOrEmpty(fileName) && hashSet.Add(fileName))
					{
						directoryOrder.fileNames.Add(fileName);
					}
				}
			}
			CielOSMediaLibrary.SaveDatabase();
		}

		// Token: 0x06009BCC RID: 39884 RVA: 0x003FA7B4 File Offset: 0x003F89B4
		public static void AppendToOrder(string virtualDirectory, string virtualPath)
		{
			string name = Path.GetFileName((virtualPath ?? string.Empty).TrimEnd(new char[] { '/', '\\' }));
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			CielOSMediaLibrary.DirectoryOrder directoryOrder = CielOSMediaLibrary.FindRecord(virtualDirectory, true);
			if (directoryOrder.fileNames.Exists((string n) => n.Equals(name, StringComparison.OrdinalIgnoreCase)))
			{
				return;
			}
			directoryOrder.fileNames.Add(name);
			CielOSMediaLibrary.SaveDatabase();
		}

		// Token: 0x06009BCD RID: 39885 RVA: 0x003FA838 File Offset: 0x003F8A38
		public static bool MoveMedia(string sourceVirtualPath, string destinationVirtualDirectory, bool isVideo, out string movedVirtualPath)
		{
			movedVirtualPath = null;
			CielOSFileSystem instance = CielOSFileSystem.Instance;
			string text = instance.ToReal(sourceVirtualPath);
			string text2 = instance.ToReal(destinationVirtualDirectory);
			if (!File.Exists(text) || !Directory.Exists(text2))
			{
				return false;
			}
			string text3 = CielOSMediaLibrary.ParentVirtual(sourceVirtualPath);
			if (text3.Equals(CielOSMediaLibrary.NormalizeVirtual(destinationVirtualDirectory), StringComparison.OrdinalIgnoreCase))
			{
				movedVirtualPath = sourceVirtualPath;
				return true;
			}
			string text4 = CielOSMediaLibrary.UniqueDestination(text2, Path.GetFileName(text), isVideo);
			if (!CielOSMediaLibrary.MoveOperations(CielOSMediaLibrary.BuildBundleOperations(text, text4, isVideo)))
			{
				return false;
			}
			movedVirtualPath = instance.ToVirtual(text4);
			CielOSMediaLibrary.RemoveFromOrder(text3, Path.GetFileName(text));
			CielOSMediaLibrary.AppendToOrder(destinationVirtualDirectory, movedVirtualPath);
			return true;
		}

		// Token: 0x06009BCE RID: 39886 RVA: 0x003FA8CC File Offset: 0x003F8ACC
		public static bool DeleteMedia(string virtualPath, bool isVideo)
		{
			string text = CielOSFileSystem.Instance.ToReal(virtualPath);
			if (!File.Exists(text))
			{
				return false;
			}
			bool flag;
			try
			{
				foreach (string text2 in CielOSMediaLibrary.BundlePaths(text, isVideo))
				{
					if (File.Exists(text2))
					{
						File.Delete(text2);
					}
				}
				CielOSMediaLibrary.RemoveFromOrder(CielOSMediaLibrary.ParentVirtual(virtualPath), Path.GetFileName(text));
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSMediaLibrary] Delete failed: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06009BCF RID: 39887 RVA: 0x003FA978 File Offset: 0x003F8B78
		public static bool RenameMedia(string virtualPath, string requestedName, bool isVideo, out string renamedVirtualPath)
		{
			renamedVirtualPath = null;
			if (string.IsNullOrWhiteSpace(requestedName))
			{
				return false;
			}
			CielOSFileSystem instance = CielOSFileSystem.Instance;
			string text = instance.ToReal(virtualPath);
			if (!File.Exists(text))
			{
				return false;
			}
			string extension = Path.GetExtension(text);
			string text2 = CielOSMediaLibrary.Sanitize(requestedName);
			string text3 = Path.GetFileNameWithoutExtension(text2).Trim().TrimEnd('.');
			if (string.IsNullOrWhiteSpace(text3))
			{
				return false;
			}
			text2 = text3 + extension;
			string text4 = Path.Combine(Path.GetDirectoryName(text) ?? string.Empty, text2);
			if (text4.Equals(text, StringComparison.OrdinalIgnoreCase))
			{
				renamedVirtualPath = virtualPath;
				return true;
			}
			if (File.Exists(text4))
			{
				return false;
			}
			if (!CielOSMediaLibrary.MoveOperations(CielOSMediaLibrary.BuildBundleOperations(text, text4, isVideo)))
			{
				return false;
			}
			renamedVirtualPath = instance.ToVirtual(text4);
			CielOSMediaLibrary.ReplaceInOrder(CielOSMediaLibrary.ParentVirtual(virtualPath), Path.GetFileName(text), Path.GetFileName(text4));
			return true;
		}

		// Token: 0x06009BD0 RID: 39888 RVA: 0x003FAA47 File Offset: 0x003F8C47
		public static string VideoThumbnailRealPath(string videoVirtualPath)
		{
			return Path.ChangeExtension(CielOSFileSystem.Instance.ToReal(videoVirtualPath), ".jpg");
		}

		// Token: 0x06009BD1 RID: 39889 RVA: 0x003FAA60 File Offset: 0x003F8C60
		private static List<KeyValuePair<string, string>> BuildBundleOperations(string sourcePrimary, string destinationPrimary, bool isVideo)
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>(sourcePrimary, destinationPrimary)
			};
			if (!isVideo)
			{
				return list;
			}
			foreach (string text in new string[] { ".jpg", ".json" })
			{
				string text2 = Path.ChangeExtension(sourcePrimary, text);
				if (File.Exists(text2))
				{
					list.Add(new KeyValuePair<string, string>(text2, Path.ChangeExtension(destinationPrimary, text)));
				}
			}
			return list;
		}

		// Token: 0x06009BD2 RID: 39890 RVA: 0x003FAAD5 File Offset: 0x003F8CD5
		private static IEnumerable<string> BundlePaths(string primary, bool isVideo)
		{
			yield return primary;
			if (!isVideo)
			{
				yield break;
			}
			yield return Path.ChangeExtension(primary, ".jpg");
			yield return Path.ChangeExtension(primary, ".json");
			yield break;
		}

		// Token: 0x06009BD3 RID: 39891 RVA: 0x003FAAEC File Offset: 0x003F8CEC
		private static bool MoveOperations(List<KeyValuePair<string, string>> operations)
		{
			foreach (KeyValuePair<string, string> keyValuePair in operations)
			{
				if (File.Exists(keyValuePair.Value))
				{
					return false;
				}
			}
			int i = 0;
			bool flag;
			try
			{
				while (i < operations.Count)
				{
					File.Move(operations[i].Key, operations[i].Value);
					i++;
				}
				flag = true;
			}
			catch (Exception ex)
			{
				for (int j = i - 1; j >= 0; j--)
				{
					try
					{
						if (File.Exists(operations[j].Value) && !File.Exists(operations[j].Key))
						{
							File.Move(operations[j].Value, operations[j].Key);
						}
					}
					catch
					{
					}
				}
				Debug.LogWarning("[CielOSMediaLibrary] Move failed: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06009BD4 RID: 39892 RVA: 0x003FAC2C File Offset: 0x003F8E2C
		private static string UniqueDestination(string directory, string fileName, bool isVideo)
		{
			string text = Path.Combine(directory, fileName);
			if (!CielOSMediaLibrary.DestinationBundleExists(text, isVideo))
			{
				return text;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			string extension = Path.GetExtension(fileName);
			for (int i = 2; i < 10000; i++)
			{
				text = Path.Combine(directory, string.Format("{0} ({1}){2}", fileNameWithoutExtension, i, extension));
				if (!CielOSMediaLibrary.DestinationBundleExists(text, isVideo))
				{
					return text;
				}
			}
			return Path.Combine(directory, string.Format("{0} ({1:N}){2}", fileNameWithoutExtension, Guid.NewGuid(), extension));
		}

		// Token: 0x06009BD5 RID: 39893 RVA: 0x003FACAA File Offset: 0x003F8EAA
		private static bool DestinationBundleExists(string primary, bool isVideo)
		{
			return File.Exists(primary) || (isVideo && (File.Exists(Path.ChangeExtension(primary, ".jpg")) || File.Exists(Path.ChangeExtension(primary, ".json"))));
		}

		// Token: 0x06009BD6 RID: 39894 RVA: 0x003FACE0 File Offset: 0x003F8EE0
		private static void RemoveFromOrder(string directory, string fileName)
		{
			CielOSMediaLibrary.DirectoryOrder directoryOrder = CielOSMediaLibrary.FindRecord(directory, false);
			if (directoryOrder == null)
			{
				return;
			}
			if (directoryOrder.fileNames.RemoveAll((string n) => n.Equals(fileName, StringComparison.OrdinalIgnoreCase)) > 0)
			{
				CielOSMediaLibrary.SaveDatabase();
			}
		}

		// Token: 0x06009BD7 RID: 39895 RVA: 0x003FAD28 File Offset: 0x003F8F28
		private static void ReplaceInOrder(string directory, string oldName, string newName)
		{
			CielOSMediaLibrary.DirectoryOrder directoryOrder = CielOSMediaLibrary.FindRecord(directory, true);
			int num = directoryOrder.fileNames.FindIndex((string n) => n.Equals(oldName, StringComparison.OrdinalIgnoreCase));
			if (num >= 0)
			{
				directoryOrder.fileNames[num] = newName;
			}
			else
			{
				directoryOrder.fileNames.Add(newName);
			}
			CielOSMediaLibrary.SaveDatabase();
		}

		// Token: 0x06009BD8 RID: 39896 RVA: 0x003FAD88 File Offset: 0x003F8F88
		private static CielOSMediaLibrary.DirectoryOrder FindRecord(string virtualDirectory, bool create)
		{
			CielOSMediaLibrary.EnsureDatabase();
			string normalized = CielOSMediaLibrary.NormalizeVirtual(virtualDirectory);
			CielOSMediaLibrary.DirectoryOrder directoryOrder = CielOSMediaLibrary._database.directories.FirstOrDefault<CielOSMediaLibrary.DirectoryOrder>((CielOSMediaLibrary.DirectoryOrder d) => d != null && string.Equals(d.virtualDirectory, normalized, StringComparison.OrdinalIgnoreCase));
			if (directoryOrder == null && create)
			{
				directoryOrder = new CielOSMediaLibrary.DirectoryOrder
				{
					virtualDirectory = normalized
				};
				CielOSMediaLibrary._database.directories.Add(directoryOrder);
			}
			return directoryOrder;
		}

		// Token: 0x06009BD9 RID: 39897 RVA: 0x003FADF4 File Offset: 0x003F8FF4
		private static void EnsureDatabase()
		{
			if (CielOSMediaLibrary._database != null)
			{
				return;
			}
			try
			{
				string statePath = CielOSMediaLibrary.StatePath;
				CielOSMediaLibrary._database = (File.Exists(statePath) ? JsonUtility.FromJson<CielOSMediaLibrary.OrderDatabase>(File.ReadAllText(statePath)) : null);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSMediaLibrary] Order database unreadable: " + ex.Message);
			}
			if (CielOSMediaLibrary._database == null)
			{
				CielOSMediaLibrary._database = new CielOSMediaLibrary.OrderDatabase();
			}
			if (CielOSMediaLibrary._database.directories == null)
			{
				CielOSMediaLibrary._database.directories = new List<CielOSMediaLibrary.DirectoryOrder>();
			}
		}

		// Token: 0x06009BDA RID: 39898 RVA: 0x003FAE84 File Offset: 0x003F9084
		private static void SaveDatabase()
		{
			CielOSMediaLibrary.EnsureDatabase();
			try
			{
				File.WriteAllText(CielOSMediaLibrary.StatePath, JsonUtility.ToJson(CielOSMediaLibrary._database, true));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSMediaLibrary] Order database save failed: " + ex.Message);
			}
		}

		// Token: 0x06009BDB RID: 39899 RVA: 0x003FAED8 File Offset: 0x003F90D8
		private static DateTime SafeWriteTime(string virtualPath)
		{
			DateTime dateTime;
			try
			{
				dateTime = File.GetLastWriteTimeUtc(CielOSFileSystem.Instance.ToReal(virtualPath));
			}
			catch
			{
				dateTime = DateTime.MinValue;
			}
			return dateTime;
		}

		// Token: 0x06009BDC RID: 39900 RVA: 0x003FAF14 File Offset: 0x003F9114
		private static string ParentVirtual(string virtualPath)
		{
			string text = CielOSMediaLibrary.NormalizeVirtual(virtualPath).TrimEnd('/');
			int num = text.LastIndexOf('/');
			if (num > 0)
			{
				return text.Substring(0, num);
			}
			return "/";
		}

		// Token: 0x06009BDD RID: 39901 RVA: 0x003FAF4C File Offset: 0x003F914C
		private static string NormalizeVirtual(string path)
		{
			string text = (path ?? "/").Replace('\\', '/').Trim();
			if (!text.StartsWith("/"))
			{
				text = "/" + text;
			}
			if (text.Length <= 1)
			{
				return text;
			}
			return text.TrimEnd('/');
		}

		// Token: 0x06009BDE RID: 39902 RVA: 0x003FAFA0 File Offset: 0x003F91A0
		private static string Sanitize(string name)
		{
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				name = name.Replace(c, '_');
			}
			return name.Trim();
		}

		// Token: 0x0400A075 RID: 41077
		private const string StateFolderName = "CielOS_State";

		// Token: 0x0400A076 RID: 41078
		private const string StateFileName = "media-order.json";

		// Token: 0x0400A077 RID: 41079
		private static CielOSMediaLibrary.OrderDatabase _database;

		// Token: 0x020013A6 RID: 5030
		[Serializable]
		private sealed class DirectoryOrder
		{
			// Token: 0x0400A078 RID: 41080
			public string virtualDirectory;

			// Token: 0x0400A079 RID: 41081
			public List<string> fileNames = new List<string>();
		}

		// Token: 0x020013A7 RID: 5031
		[Serializable]
		private sealed class OrderDatabase
		{
			// Token: 0x0400A07A RID: 41082
			public List<CielOSMediaLibrary.DirectoryOrder> directories = new List<CielOSMediaLibrary.DirectoryOrder>();
		}
	}
}
