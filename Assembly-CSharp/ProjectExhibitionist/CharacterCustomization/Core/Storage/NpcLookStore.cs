using System;
using System.IO;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Storage
{
	// Token: 0x02000ADB RID: 2779
	public static class NpcLookStore
	{
		// Token: 0x06004EB2 RID: 20146 RVA: 0x001E8172 File Offset: 0x001E6372
		private static string LookPath(string presetId)
		{
			return Path.Combine(GameDataPaths.NpcLooksDirectory, NpcLookStore.SanitizeId(presetId) + ".json");
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x001E818E File Offset: 0x001E638E
		private static string MetaPath(string presetId)
		{
			return Path.Combine(GameDataPaths.NpcLooksDirectory, NpcLookStore.SanitizeId(presetId) + ".meta.json");
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x001E81AA File Offset: 0x001E63AA
		public static bool HasOverride(string presetId)
		{
			return !string.IsNullOrWhiteSpace(presetId) && File.Exists(NpcLookStore.LookPath(presetId));
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x001E81C4 File Offset: 0x001E63C4
		public static void Save(string presetId, CharacterData look, string displayName, string iconFileName)
		{
			if (string.IsNullOrWhiteSpace(presetId) || look == null)
			{
				Debug.LogWarning("[NpcLookStore] Save ignorato: presetId o look nullo.");
				return;
			}
			GameDataPaths.EnsureCustomizationDirectories();
			File.WriteAllText(NpcLookStore.LookPath(presetId), JsonUtility.ToJson(look));
			NpcLookStore.NpcLookMeta npcLookMeta = new NpcLookStore.NpcLookMeta
			{
				presetId = presetId,
				displayName = displayName,
				iconFileName = iconFileName
			};
			File.WriteAllText(NpcLookStore.MetaPath(presetId), JsonUtility.ToJson(npcLookMeta, true));
		}

		// Token: 0x06004EB6 RID: 20150 RVA: 0x001E822C File Offset: 0x001E642C
		public static CharacterData LoadLook(string presetId)
		{
			if (!NpcLookStore.HasOverride(presetId))
			{
				return null;
			}
			CharacterData characterData;
			try
			{
				characterData = JsonUtility.FromJson<CharacterData>(File.ReadAllText(NpcLookStore.LookPath(presetId)));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NpcLookStore] LoadLook fallito per '" + presetId + "': " + ex.Message);
				characterData = null;
			}
			return characterData;
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x001E8288 File Offset: 0x001E6488
		public static NpcLookStore.NpcLookMeta LoadMeta(string presetId)
		{
			string text = NpcLookStore.MetaPath(presetId);
			if (string.IsNullOrWhiteSpace(presetId) || !File.Exists(text))
			{
				return null;
			}
			NpcLookStore.NpcLookMeta npcLookMeta;
			try
			{
				npcLookMeta = JsonUtility.FromJson<NpcLookStore.NpcLookMeta>(File.ReadAllText(text));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NpcLookStore] LoadMeta fallito per '" + presetId + "': " + ex.Message);
				npcLookMeta = null;
			}
			return npcLookMeta;
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x001E82F0 File Offset: 0x001E64F0
		public static void Delete(string presetId)
		{
			if (string.IsNullOrWhiteSpace(presetId))
			{
				return;
			}
			NpcLookStore.TryDelete(NpcLookStore.LookPath(presetId));
			NpcLookStore.TryDelete(NpcLookStore.MetaPath(presetId));
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x001E8314 File Offset: 0x001E6514
		private static void TryDelete(string path)
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
				Debug.LogWarning("[NpcLookStore] Delete fallito '" + path + "': " + ex.Message);
			}
		}

		// Token: 0x06004EBA RID: 20154 RVA: 0x001E8360 File Offset: 0x001E6560
		private static string SanitizeId(string id)
		{
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				id = id.Replace(c, '_');
			}
			return id;
		}

		// Token: 0x02000ADC RID: 2780
		[Serializable]
		public class NpcLookMeta
		{
			// Token: 0x0400500C RID: 20492
			public string presetId;

			// Token: 0x0400500D RID: 20493
			public string displayName;

			// Token: 0x0400500E RID: 20494
			public string iconFileName;
		}
	}
}
