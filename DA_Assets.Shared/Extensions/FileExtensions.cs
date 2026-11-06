using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DA_Assets.Extensions
{
	// Token: 0x0200002C RID: 44
	public static class FileExtensions
	{
		// Token: 0x06000093 RID: 147 RVA: 0x00004179 File Offset: 0x00002379
		public static void OpenFolderInOS(this string path)
		{
			Application.OpenURL("file://" + path);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000418C File Offset: 0x0000238C
		public static string GetPathRelativeToProjectDirectory(this string path)
		{
			char c = (path.Contains('/') ? '/' : '\\');
			return string.Join(c.ToString(), path.Split(new char[] { c }).SkipWhile<string>((string s) => s.ToLower() != "assets").ToArray<string>());
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000041F0 File Offset: 0x000023F0
		public static void ReplaceInFile(string filePath, string searchText, string replaceText)
		{
			StreamReader streamReader = new StreamReader(filePath);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			text = Regex.Replace(text, searchText, replaceText);
			StreamWriter streamWriter = new StreamWriter(filePath);
			streamWriter.Write(text);
			streamWriter.Close();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000422A File Offset: 0x0000242A
		public static void CreateFolderIfNotExists(this string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000423C File Offset: 0x0000243C
		public static bool TryReadAllText(this string path, out string text)
		{
			bool flag;
			try
			{
				text = File.ReadAllText(path);
				flag = true;
			}
			catch
			{
				text = null;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004270 File Offset: 0x00002470
		public static string GetFullAssetPath(this string assetPath)
		{
			if (assetPath.IsEmpty())
			{
				return "";
			}
			string text = assetPath.Substring("Assets".Length + 1);
			return Path.Combine(Application.dataPath, text);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000042A9 File Offset: 0x000024A9
		public static bool IsPathInsideAssetsPath(this string path)
		{
			return path.IndexOf(Application.dataPath, StringComparison.InvariantCultureIgnoreCase) != -1;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000042BD File Offset: 0x000024BD
		public static string ToRelativePath(this string absolutePath)
		{
			if (absolutePath.StartsWith(Application.dataPath))
			{
				return "Assets" + absolutePath.Substring(Application.dataPath.Length);
			}
			return absolutePath;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000042E8 File Offset: 0x000024E8
		public static string RemoveInvalidCharsFromFileName(this string fileName, char repl = '_')
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return fileName;
			}
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				fileName = fileName.Replace(c, repl);
			}
			return fileName;
		}
	}
}
