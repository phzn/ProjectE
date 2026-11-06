using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DA_Assets.Extensions;
using DA_Assets.Networking;

namespace DA_Assets.FCU
{
	// Token: 0x02000027 RID: 39
	public static class OtherExtensions
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00003208 File Offset: 0x00001408
		public static bool TryParseSpriteName(this string spriteName, out float scale, out BigInteger hash)
		{
			bool flag3;
			try
			{
				if (spriteName.IsEmpty())
				{
					throw new Exception("Sprite name is empty.");
				}
				char c = ' ';
				List<string> list = Path.GetFileNameWithoutExtension(spriteName).Split(c, StringSplitOptions.None).ToList<string>();
				if (list.Count < 2)
				{
					throw new Exception("nameParts.Count < 2: " + spriteName);
				}
				string text = list[list.Count<string>() - 1];
				bool flag = list[list.Count<string>() - 2].Replace("x", "").TryParseWithDot(out scale);
				bool flag2 = BigInteger.TryParse(text, out hash);
				if (!flag)
				{
					throw new Exception("Cant parse scale from name: " + spriteName);
				}
				if (!flag2)
				{
					throw new Exception("Cant parse hash from name: " + spriteName);
				}
				flag3 = true;
			}
			catch
			{
				scale = 1f;
				hash = -1;
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000032E4 File Offset: 0x000014E4
		public static async Task WriteLog(this DARequest request, UnityHttpClient webRequest)
		{
			FileInfo[] files = new DirectoryInfo(FcuConfig.LogPath).GetFiles("*.*");
			if (files.Length >= FcuConfig.LogFilesLimit)
			{
				foreach (FileInfo fileInfo in files)
				{
					try
					{
						fileInfo.Delete();
					}
					catch
					{
					}
				}
			}
			string text = DateTime.Now.ToString(FcuConfig.DateTimeFormat1) + "_" + FcuConfig.WebLogFileName;
			string text2 = Path.Combine(FcuConfig.LogPath, text);
			string text3 = webRequest.downloadHandler.text;
			JFResult jfresult = DAFormatter.Format<string>(text3);
			string text4;
			if (jfresult.IsValid)
			{
				text4 = jfresult.Json;
			}
			else
			{
				text4 = text3;
			}
			text4 = string.Concat(new string[] { request.Query, "\n", webRequest.error, "\n", text4 });
			File.WriteAllText(text2, text4);
			await Task.Yield();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000332F File Offset: 0x0000152F
		public static bool IsProjectEmpty(this SelectableFObject sf)
		{
			return sf == null || sf.Id.IsEmpty();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003346 File Offset: 0x00001546
		public static bool IsScrollContent(this string objectName)
		{
			if (objectName.IsEmpty())
			{
				return false;
			}
			objectName = objectName.ToLower();
			objectName = Regex.Replace(objectName, "[^a-z]", "");
			return objectName == "content";
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003377 File Offset: 0x00001577
		public static bool IsScrollViewport(this string objectName)
		{
			if (objectName.IsEmpty())
			{
				return false;
			}
			objectName = objectName.ToLower();
			objectName = Regex.Replace(objectName, "[^a-z]", "");
			return objectName == "viewport";
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000033A8 File Offset: 0x000015A8
		public static bool IsInputTextArea(this string objectName)
		{
			if (objectName.IsEmpty())
			{
				return false;
			}
			objectName = objectName.ToLower();
			objectName = Regex.Replace(objectName, "[^a-z]", "");
			return objectName == "textarea";
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000033D9 File Offset: 0x000015D9
		public static bool IsCheckmark(this string objectName)
		{
			if (objectName.IsEmpty())
			{
				return false;
			}
			objectName = objectName.ToLower();
			objectName = Regex.Replace(objectName, "[^a-z]", "");
			return objectName == "checkmark";
		}
	}
}
