using System;
using System.IO;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200084F RID: 2127
	public static class GameDataPaths
	{
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600385A RID: 14426 RVA: 0x001498E9 File Offset: 0x00147AE9
		public static string GameDataRoot
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "ProjectExhibitionist", "GameData");
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x0600385B RID: 14427 RVA: 0x001498FF File Offset: 0x00147AFF
		public static string CharactersRoot
		{
			get
			{
				return Path.Combine(GameDataPaths.GameDataRoot, "Characters");
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x0600385C RID: 14428 RVA: 0x00149910 File Offset: 0x00147B10
		public static string CharacterProfilesDirectory
		{
			get
			{
				return Path.Combine(GameDataPaths.CharactersRoot, "Profiles");
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x0600385D RID: 14429 RVA: 0x00149921 File Offset: 0x00147B21
		public static string CharacterProfileIconsDirectory
		{
			get
			{
				return Path.Combine(GameDataPaths.CharactersRoot, "Icons");
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x0600385E RID: 14430 RVA: 0x00149932 File Offset: 0x00147B32
		public static string CharacterSavesDirectory
		{
			get
			{
				return Path.Combine(GameDataPaths.CharactersRoot, "Saves");
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600385F RID: 14431 RVA: 0x00149943 File Offset: 0x00147B43
		public static string CharacterSaveIconsDirectory
		{
			get
			{
				return Path.Combine(GameDataPaths.CharacterSavesDirectory, "Icons");
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06003860 RID: 14432 RVA: 0x00149954 File Offset: 0x00147B54
		public static string CustomizationRoot
		{
			get
			{
				return Path.Combine(GameDataPaths.GameDataRoot, "Customization");
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06003861 RID: 14433 RVA: 0x00149965 File Offset: 0x00147B65
		public static string CustomizationPresetIconsDirectory
		{
			get
			{
				return Path.Combine(GameDataPaths.CustomizationRoot, "PresetIcons");
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06003862 RID: 14434 RVA: 0x00149976 File Offset: 0x00147B76
		public static string NpcLooksDirectory
		{
			get
			{
				return Path.Combine(GameDataPaths.CustomizationRoot, "NpcLooks");
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06003863 RID: 14435 RVA: 0x00149987 File Offset: 0x00147B87
		public static string RuntimeRoot
		{
			get
			{
				return Path.Combine(GameDataPaths.GameDataRoot, "Runtime");
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06003864 RID: 14436 RVA: 0x00149998 File Offset: 0x00147B98
		public static string CharacterStateFilePath
		{
			get
			{
				return Path.Combine(GameDataPaths.RuntimeRoot, "character_state.json");
			}
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x001499A9 File Offset: 0x00147BA9
		public static void EnsureCharacterProfileDirectories()
		{
			GameDataPaths.EnsureDirectory(GameDataPaths.CharacterProfilesDirectory);
			GameDataPaths.EnsureDirectory(GameDataPaths.CharacterProfileIconsDirectory);
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x001499BF File Offset: 0x00147BBF
		public static void EnsureCharacterSaveDirectories()
		{
			GameDataPaths.EnsureDirectory(GameDataPaths.CharacterSavesDirectory);
			GameDataPaths.EnsureDirectory(GameDataPaths.CharacterSaveIconsDirectory);
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x001499D5 File Offset: 0x00147BD5
		public static void EnsureCustomizationDirectories()
		{
			GameDataPaths.EnsureDirectory(GameDataPaths.CustomizationRoot);
			GameDataPaths.EnsureDirectory(GameDataPaths.CustomizationPresetIconsDirectory);
			GameDataPaths.EnsureDirectory(GameDataPaths.NpcLooksDirectory);
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x001499F5 File Offset: 0x00147BF5
		public static void EnsureRuntimeDirectories()
		{
			GameDataPaths.EnsureDirectory(GameDataPaths.RuntimeRoot);
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x00149A01 File Offset: 0x00147C01
		public static string GetCustomizationFilePath(string fileName)
		{
			GameDataPaths.EnsureCustomizationDirectories();
			return Path.Combine(GameDataPaths.CustomizationRoot, fileName);
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x00149A13 File Offset: 0x00147C13
		private static void EnsureDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
	}
}
