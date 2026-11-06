using System;
using System.Collections.Generic;
using System.IO;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.CielOS.VFS
{
	// Token: 0x020013A3 RID: 5027
	public class CielOSFileSystem : MonoBehaviour
	{
		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x06009BB5 RID: 39861 RVA: 0x003F9E9B File Offset: 0x003F809B
		public static CielOSFileSystem Instance
		{
			get
			{
				if (CielOSFileSystem._instance == null)
				{
					GameObject gameObject = new GameObject("CielOSFileSystem");
					CielOSFileSystem._instance = gameObject.AddComponent<CielOSFileSystem>();
					Object.DontDestroyOnLoad(gameObject);
				}
				return CielOSFileSystem._instance;
			}
		}

		// Token: 0x170018CB RID: 6347
		// (get) Token: 0x06009BB6 RID: 39862 RVA: 0x003F9EC9 File Offset: 0x003F80C9
		public string RootPath
		{
			get
			{
				this.EnsureInitialized();
				return this._root;
			}
		}

		// Token: 0x06009BB7 RID: 39863 RVA: 0x003F9ED7 File Offset: 0x003F80D7
		private void Awake()
		{
			if (CielOSFileSystem._instance != null && CielOSFileSystem._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			CielOSFileSystem._instance = this;
			this.EnsureInitialized();
		}

		// Token: 0x06009BB8 RID: 39864 RVA: 0x003F9F0C File Offset: 0x003F810C
		private void EnsureInitialized()
		{
			if (!string.IsNullOrEmpty(this._root))
			{
				return;
			}
			this._root = PlayerMediaExport.PcRootDirectory;
			Directory.CreateDirectory(this._root);
			foreach (string text in CielOSFileSystem.DefaultFolders)
			{
				Directory.CreateDirectory(Path.Combine(this._root, text));
			}
		}

		// Token: 0x06009BB9 RID: 39865 RVA: 0x003F9F68 File Offset: 0x003F8168
		public string ToReal(string virtualPath)
		{
			this.EnsureInitialized();
			string text = (virtualPath ?? "/").Replace('\\', '/').Trim();
			if (!text.StartsWith("/"))
			{
				text = "/" + text;
			}
			string fullPath = Path.GetFullPath(Path.Combine(this._root, text.TrimStart('/')));
			string fullPath2 = Path.GetFullPath(this._root);
			string text2 = fullPath2.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			}) + Path.DirectorySeparatorChar.ToString();
			if (!fullPath.Equals(fullPath2, StringComparison.OrdinalIgnoreCase) && !fullPath.StartsWith(text2, StringComparison.OrdinalIgnoreCase))
			{
				return fullPath2;
			}
			return fullPath;
		}

		// Token: 0x06009BBA RID: 39866 RVA: 0x003FA018 File Offset: 0x003F8218
		public string ToVirtual(string realPath)
		{
			this.EnsureInitialized();
			string fullPath = Path.GetFullPath(this._root);
			string fullPath2 = Path.GetFullPath(realPath);
			string text = fullPath.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			}) + Path.DirectorySeparatorChar.ToString();
			if (!fullPath2.Equals(fullPath, StringComparison.OrdinalIgnoreCase) && !fullPath2.StartsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				return "/";
			}
			string text2 = fullPath2.Substring(fullPath.Length).Replace('\\', '/');
			if (!text2.StartsWith("/"))
			{
				text2 = "/" + text2;
			}
			if (text2.Length != 0)
			{
				return text2;
			}
			return "/";
		}

		// Token: 0x06009BBB RID: 39867 RVA: 0x003FA0C4 File Offset: 0x003F82C4
		public List<CielOSFileSystem.Entry> List(string virtualDir)
		{
			List<CielOSFileSystem.Entry> list = new List<CielOSFileSystem.Entry>();
			string text = this.ToReal(virtualDir);
			if (!Directory.Exists(text))
			{
				return list;
			}
			foreach (string text2 in Directory.GetDirectories(text))
			{
				list.Add(new CielOSFileSystem.Entry
				{
					virtualPath = this.ToVirtual(text2),
					name = Path.GetFileName(text2),
					isDirectory = true,
					sizeBytes = 0L
				});
			}
			foreach (string text3 in Directory.GetFiles(text))
			{
				if (!text3.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
				{
					FileInfo fileInfo = new FileInfo(text3);
					list.Add(new CielOSFileSystem.Entry
					{
						virtualPath = this.ToVirtual(text3),
						name = Path.GetFileName(text3),
						isDirectory = false,
						sizeBytes = fileInfo.Length
					});
				}
			}
			return list;
		}

		// Token: 0x06009BBC RID: 39868 RVA: 0x003FA1B8 File Offset: 0x003F83B8
		public bool IsProtected(string virtualPath)
		{
			string text = (virtualPath ?? "/").Replace('\\', '/').TrimEnd('/');
			if (string.IsNullOrEmpty(text))
			{
				return true;
			}
			foreach (string text2 in CielOSFileSystem.DefaultFolders)
			{
				if (text.Equals("/" + text2, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009BBD RID: 39869 RVA: 0x003FA21C File Offset: 0x003F841C
		public bool CreateFolder(string virtualDir, string folderName)
		{
			if (string.IsNullOrWhiteSpace(folderName))
			{
				return false;
			}
			string text = Path.Combine(this.ToReal(virtualDir), CielOSFileSystem.Sanitize(folderName));
			bool flag;
			try
			{
				Directory.CreateDirectory(text);
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSFileSystem] CreateFolder failed: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06009BBE RID: 39870 RVA: 0x003FA27C File Offset: 0x003F847C
		public bool Delete(string virtualPath)
		{
			if (this.IsProtected(virtualPath))
			{
				Debug.LogWarning("[CielOSFileSystem] Refusing to delete protected path '" + virtualPath + "'.");
				return false;
			}
			string text = this.ToReal(virtualPath);
			bool flag;
			try
			{
				if (Directory.Exists(text))
				{
					Directory.Delete(text, true);
				}
				else
				{
					if (!File.Exists(text))
					{
						return false;
					}
					File.Delete(text);
				}
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSFileSystem] Delete failed: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06009BBF RID: 39871 RVA: 0x003FA308 File Offset: 0x003F8508
		public bool Move(string virtualSource, string virtualDestDir)
		{
			if (this.IsProtected(virtualSource))
			{
				Debug.LogWarning("[CielOSFileSystem] Refusing to move protected path '" + virtualSource + "'.");
				return false;
			}
			string text = this.ToReal(virtualSource);
			string text2 = this.ToReal(virtualDestDir);
			bool flag;
			try
			{
				string text3 = Path.Combine(text2, Path.GetFileName(text.TrimEnd(new char[] { '/', '\\' })));
				if (Directory.Exists(text))
				{
					Directory.Move(text, text3);
				}
				else
				{
					if (!File.Exists(text))
					{
						return false;
					}
					File.Move(text, text3);
				}
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSFileSystem] Move failed: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06009BC0 RID: 39872 RVA: 0x003FA3C0 File Offset: 0x003F85C0
		public bool Rename(string virtualPath, string newName)
		{
			if (this.IsProtected(virtualPath))
			{
				Debug.LogWarning("[CielOSFileSystem] Refusing to rename protected path '" + virtualPath + "'.");
				return false;
			}
			if (string.IsNullOrWhiteSpace(newName))
			{
				return false;
			}
			string text = this.ToReal(virtualPath);
			bool flag;
			try
			{
				string text2 = Path.Combine(Path.GetDirectoryName(text), CielOSFileSystem.Sanitize(newName));
				if (Directory.Exists(text))
				{
					Directory.Move(text, text2);
				}
				else
				{
					if (!File.Exists(text))
					{
						return false;
					}
					File.Move(text, text2);
				}
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSFileSystem] Rename failed: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06009BC1 RID: 39873 RVA: 0x003FA468 File Offset: 0x003F8668
		public string ImportExternalFile(string externalRealPath, string virtualDestDir)
		{
			string text3;
			try
			{
				string text = this.ToReal(virtualDestDir);
				Directory.CreateDirectory(text);
				string text2 = Path.Combine(text, Path.GetFileName(externalRealPath));
				File.Copy(externalRealPath, text2, true);
				text3 = this.ToVirtual(text2);
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSFileSystem] Import failed: " + ex.Message);
				text3 = null;
			}
			return text3;
		}

		// Token: 0x06009BC2 RID: 39874 RVA: 0x003FA4CC File Offset: 0x003F86CC
		public bool WriteBytes(string virtualPath, byte[] data)
		{
			bool flag;
			try
			{
				string text = this.ToReal(virtualPath);
				Directory.CreateDirectory(Path.GetDirectoryName(text));
				File.WriteAllBytes(text, data);
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[CielOSFileSystem] WriteBytes failed: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06009BC3 RID: 39875 RVA: 0x003FA524 File Offset: 0x003F8724
		public byte[] ReadBytes(string virtualPath)
		{
			string text = this.ToReal(virtualPath);
			if (!File.Exists(text))
			{
				return null;
			}
			return File.ReadAllBytes(text);
		}

		// Token: 0x06009BC4 RID: 39876 RVA: 0x003FA54C File Offset: 0x003F874C
		public bool Exists(string virtualPath)
		{
			string text = this.ToReal(virtualPath);
			return File.Exists(text) || Directory.Exists(text);
		}

		// Token: 0x06009BC5 RID: 39877 RVA: 0x003FA574 File Offset: 0x003F8774
		private static string Sanitize(string name)
		{
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				name = name.Replace(c, '_');
			}
			return name.Trim();
		}

		// Token: 0x0400A06E RID: 41070
		private static CielOSFileSystem _instance;

		// Token: 0x0400A06F RID: 41071
		private static readonly string[] DefaultFolders = new string[] { "Photos", "Videos", "Pictures", "Music", "Documents", "Drawings" };

		// Token: 0x0400A070 RID: 41072
		private string _root;

		// Token: 0x020013A4 RID: 5028
		public struct Entry
		{
			// Token: 0x0400A071 RID: 41073
			public string virtualPath;

			// Token: 0x0400A072 RID: 41074
			public string name;

			// Token: 0x0400A073 RID: 41075
			public bool isDirectory;

			// Token: 0x0400A074 RID: 41076
			public long sizeBytes;
		}
	}
}
