using System;
using System.IO;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.Detection.Customization
{
	// Token: 0x0200131D RID: 4893
	public static class DialogueCustomizationStore
	{
		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x06009763 RID: 38755 RVA: 0x003E12E3 File Offset: 0x003DF4E3
		private static string FilePath
		{
			get
			{
				return Path.Combine(GameDataPaths.CustomizationRoot, "dialogues.json");
			}
		}

		// Token: 0x06009764 RID: 38756 RVA: 0x003E12F4 File Offset: 0x003DF4F4
		public static bool Exists()
		{
			return File.Exists(DialogueCustomizationStore.FilePath);
		}

		// Token: 0x06009765 RID: 38757 RVA: 0x003E1300 File Offset: 0x003DF500
		public static DialogueCustomizationFile Load()
		{
			if (!File.Exists(DialogueCustomizationStore.FilePath))
			{
				return new DialogueCustomizationFile();
			}
			DialogueCustomizationFile dialogueCustomizationFile;
			try
			{
				dialogueCustomizationFile = JsonUtility.FromJson<DialogueCustomizationFile>(File.ReadAllText(DialogueCustomizationStore.FilePath)) ?? new DialogueCustomizationFile();
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[DialogueCustomizationStore] Load fallito: " + ex.Message + ". Uso un delta vuoto.");
				dialogueCustomizationFile = new DialogueCustomizationFile();
			}
			return dialogueCustomizationFile;
		}

		// Token: 0x06009766 RID: 38758 RVA: 0x003E1370 File Offset: 0x003DF570
		public static void Save(DialogueCustomizationFile data)
		{
			if (data == null)
			{
				Debug.LogWarning("[DialogueCustomizationStore] Save ignorato: data nullo.");
				return;
			}
			try
			{
				GameDataPaths.EnsureCustomizationDirectories();
				File.WriteAllText(DialogueCustomizationStore.FilePath, JsonUtility.ToJson(data, true));
			}
			catch (Exception ex)
			{
				Debug.LogError("[DialogueCustomizationStore] Save fallito: " + ex.Message);
			}
		}

		// Token: 0x06009767 RID: 38759 RVA: 0x003E13CC File Offset: 0x003DF5CC
		public static void DeleteAll()
		{
			try
			{
				if (File.Exists(DialogueCustomizationStore.FilePath))
				{
					File.Delete(DialogueCustomizationStore.FilePath);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[DialogueCustomizationStore] DeleteAll fallito: " + ex.Message);
			}
		}

		// Token: 0x04009CC7 RID: 40135
		private const string FileName = "dialogues.json";
	}
}
