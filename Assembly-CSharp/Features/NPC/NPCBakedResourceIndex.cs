using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ED3 RID: 3795
	public static class NPCBakedResourceIndex
	{
		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x060071A0 RID: 29088 RVA: 0x002E271D File Offset: 0x002E091D
		public static IReadOnlyList<string> Names
		{
			get
			{
				return NPCBakedResourceIndex.CharacterNames;
			}
		}

		// Token: 0x060071A1 RID: 29089 RVA: 0x002E2724 File Offset: 0x002E0924
		public static CharacterObject LoadCharacter(string characterName)
		{
			NPCBakedResourceIndex.ReportSyncLoadDuringMapCatalog(characterName, false);
			if (!string.IsNullOrEmpty(characterName))
			{
				return Resources.Load<CharacterObject>(characterName);
			}
			return null;
		}

		// Token: 0x060071A2 RID: 29090 RVA: 0x002E273D File Offset: 0x002E093D
		public static GameObject LoadPrefab(string characterName)
		{
			NPCBakedResourceIndex.ReportSyncLoadDuringMapCatalog(characterName, true);
			if (!string.IsNullOrEmpty(characterName))
			{
				return Resources.Load<GameObject>(NPCBakedResourceIndex.PrefabPath(characterName));
			}
			return null;
		}

		// Token: 0x060071A3 RID: 29091 RVA: 0x002E275C File Offset: 0x002E095C
		public static Task<GameObject> LoadPrefabAsync(string characterName)
		{
			if (string.IsNullOrEmpty(characterName))
			{
				return Task.FromResult<GameObject>(null);
			}
			ResourceRequest request = Resources.LoadAsync<GameObject>(NPCBakedResourceIndex.PrefabPath(characterName));
			if (request.isDone)
			{
				return Task.FromResult<GameObject>(request.asset as GameObject);
			}
			TaskCompletionSource<GameObject> completion = new TaskCompletionSource<GameObject>();
			request.completed += delegate(AsyncOperation _)
			{
				completion.TrySetResult(request.asset as GameObject);
			};
			return completion.Task;
		}

		// Token: 0x060071A4 RID: 29092 RVA: 0x002E27DF File Offset: 0x002E09DF
		private static string PrefabPath(string characterName)
		{
			return "NPC_BakedPrefabs/" + characterName + "/" + characterName;
		}

		// Token: 0x060071A5 RID: 29093 RVA: 0x002E27F4 File Offset: 0x002E09F4
		private static void ReportSyncLoadDuringMapCatalog(string characterName, bool prefab)
		{
			try
			{
				if (!string.IsNullOrEmpty(characterName))
				{
					ChunkManager instance = ChunkManager.Instance;
					if (!(instance == null) && instance.IsReadingCatalog)
					{
						NPCBakedResourceIndex.ReportSyncLoad(prefab ? NPCBakedResourceIndex.PrefabPath(characterName) : characterName);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060071A6 RID: 29094 RVA: 0x002E284C File Offset: 0x002E0A4C
		private static bool ReportSyncLoad(string path)
		{
			if (!NPCBakedResourceIndex.SyncLoadsReportedDuringMapCatalog.Add(path))
			{
				return false;
			}
			Debug.LogError("[NPCBakedResourceIndex] Synchronous load of '" + path + "' while the map catalog is loading: it waits for the catalog read and freezes the loading bar at 32%. Load it asynchronously or after ChunkManager.IsAwaitingCatalog clears.\n" + Environment.StackTrace);
			return true;
		}

		// Token: 0x04007499 RID: 29849
		private static readonly string[] CharacterNames = new string[]
		{
			"NPC_Akio_Brightman", "NPC_Akira_Brightstar", "NPC_Akira_Brightstar_1", "NPC_Akira_Brightstar_2", "NPC_Akira_Brightstar_3", "NPC_Akira_Brightstar_4", "NPC_Daichi_Greatearth", "NPC_Daichi_Greatearth_1", "NPC_Daichi_Greatearth_2", "NPC_Daichi_Greatearth_3",
			"NPC_Daiki_Greatshine", "NPC_Daiki_Greatshine_1", "NPC_Eiji_Eternity", "NPC_Eiji_Eternity_1", "NPC_Eiji_Eternity_2", "NPC_Eiji_Eternity_3", "NPC_Eiji_Eternity_4", "NPC_Fumio_Literaryhero", "NPC_Fumio_Literaryhero_1", "NPC_Fumio_Literaryhero_2",
			"NPC_Fumio_Literaryhero_3", "NPC_Fumio_Literaryhero_4", "NPC_Fumio_Literaryhero_5", "NPC_Fumio_Literaryhero_6", "NPC_Fumio_Literaryhero_7", "NPC_Fumio_Literaryhero_8", "NPC_Haru_Springbreeze", "NPC_Haru_Springbreeze_1", "NPC_Haru_Springbreeze_2", "NPC_Haru_Springbreeze_3",
			"NPC_Haru_Springbreeze_4", "NPC_Haruto_Sunlight", "NPC_Haruto_Sunlight_1", "NPC_Haruto_Sunlight_2", "NPC_Haruto_Sunlight_3", "NPC_Haruto_Sunlight_4", "NPC_Hayato_Falcon", "NPC_Hayato_Falcon_1", "NPC_Hayato_Falcon_2", "NPC_Hayato_Falcon_3",
			"NPC_Hayato_Falcon_4", "NPC_Hideo_Excellentman", "NPC_Hideo_Excellentman_1", "NPC_Hideo_Excellentman_2", "NPC_Hiroshi_Generous", "NPC_Hiroshi_Generous_1", "NPC_Hiroshi_Generous_2", "NPC_Hiroshi_Generous_3", "NPC_Ichiro_Firstson", "NPC_Ichiro_Firstson_1",
			"NPC_Ichiro_Firstson_2", "NPC_Isamu_Courage", "NPC_Isamu_Courage_1", "NPC_Isamu_Courage_2", "NPC_Isamu_Courage_3", "NPC_Jiro_Secondson", "NPC_Jiro_Secondson_1", "NPC_Kaito_Oceanguard", "NPC_Kazuo_Harmonious", "NPC_Kazuo_Harmonious_1",
			"NPC_Kazuo_Harmonious_2", "NPC_Kazuo_Harmonious_3", "NPC_Kenji_Strongsecond", "NPC_Kenji_Strongsecond_1", "NPC_Kenji_Strongsecond_2", "NPC_Kenji_Strongsecond_3", "NPC_Kenji_Strongsecond_4", "NPC_Kenji_Strongsecond_5", "NPC_Kenta_Healthy", "NPC_Kenta_Healthy_1",
			"NPC_Kenta_Healthy_2", "NPC_Kenta_Healthy_3", "NPC_Koji_Shining", "NPC_Koji_Shining_1", "NPC_Koji_Shining_2", "NPC_Koji_Shining_3", "NPC_Koji_Shining_4", "NPC_Koji_Shining_5", "NPC_Koji_Shining_6", "NPC_Makoto_Sincere",
			"NPC_Makoto_Sincere_1", "NPC_Makoto_Sincere_2", "NPC_Makoto_Sincere_3", "NPC_Masato_Righteousman", "NPC_Masato_Righteousman_1", "NPC_Masato_Righteousman_2", "NPC_Masato_Righteousman_3", "NPC_Minoru_Truth", "NPC_Minoru_Truth_1", "NPC_Minoru_Truth_2",
			"NPC_Minoru_Truth_3", "NPC_Naoki_Honestree", "NPC_Naoki_Honestree_1", "NPC_Naoki_Honestree_2", "NPC_Naoki_Honestree_3", "NPC_Naoki_Honestree_4", "NPC_Noboru_Ascend", "NPC_Noboru_Ascend_1", "NPC_Noboru_Ascend_2", "NPC_Noboru_Ascend_3",
			"NPC_Norio_Lawman", "NPC_Osamu_Discipline", "NPC_Osamu_Discipline_1", "NPC_Osamu_Discipline_2", "NPC_Osamu_Discipline_3", "NPC_Ren_Lotus", "NPC_Ren_Lotus_1", "NPC_Ren_Lotus_2", "NPC_Ren_Lotus_3", "NPC_Riku_Landwalker",
			"NPC_Ryo_Dragon", "NPC_Ryota_Refreshing", "NPC_Ryota_Refreshing_1", "NPC_Ryota_Refreshing_2", "NPC_Ryota_Refreshing_3", "NPC_Ryota_Refreshing_4", "NPC_Ryota_Refreshing_5", "NPC_Ryota_Refreshing_6", "NPC_Satoshi_Wisdom", "NPC_Satoshi_Wisdom_1",
			"NPC_Satoshi_Wisdom_2", "NPC_Satoshi_Wisdom_3", "NPC_Shinji_Truefaith", "NPC_Shinji_Truefaith_1", "NPC_Shinji_Truefaith_2", "NPC_Shinji_Truefaith_3", "NPC_Shinji_Truefaith_4", "NPC_Shinji_Truefaith_5", "NPC_Shinji_Truefaith_6", "NPC_Shota_Soaring",
			"NPC_Shota_Soaring_1", "NPC_Shun_Speed", "NPC_Shun_Speed_1", "NPC_Shun_Speed_2", "NPC_Shun_Speed_3", "NPC_Sora_Skybound", "NPC_Sora_Skybound_1", "NPC_Sora_Skybound_2", "NPC_Taichi_Greatfirst", "NPC_Taichi_Greatfirst_1",
			"NPC_Taichi_Greatfirst_2", "NPC_Taichi_Greatfirst_3", "NPC_Taichi_Greatfirst_4", "NPC_Taichi_Greatfirst_5", "NPC_Taichi_Greatfirst_6", "NPC_Taichi_Greatfirst_7", "NPC_Taichi_Greatfirst_8", "NPC_Takashi_Prosperous", "NPC_Takashi_Prosperous_1", "NPC_Takashi_Prosperous_2",
			"NPC_Takashi_Prosperous_3", "NPC_Takashi_Prosperous_4", "NPC_Takashi_Prosperous_5", "NPC_Takashi_Prosperous_6", "NPC_Takumi_Artisan", "NPC_Takumi_Artisan_1", "NPC_Takumi_Artisan_2", "NPC_Takumi_Artisan_3", "NPC_Taro_Firstborn", "NPC_Tatsuya_Achiever",
			"NPC_Tatsuya_Achiever_1", "NPC_Tatsuya_Achiever_2", "NPC_Tomoya_Wisefriend", "NPC_Tomoya_Wisefriend_1", "NPC_Tomoya_Wisefriend_2", "NPC_Tomoya_Wisefriend_3", "NPC_Tomoya_Wisefriend_4", "NPC_Toshiro_Talented", "NPC_Toshiro_Talented_1", "NPC_Toshiro_Talented_2",
			"NPC_Toshiro_Talented_3", "NPC_Yasuo_Peaceful", "NPC_Yasuo_Peaceful_1", "NPC_Yasuo_Peaceful_2", "NPC_Yasuo_Peaceful_3", "NPC_Yoshi_Goodluck", "NPC_Yoshi_Goodluck_1", "NPC_Yoshi_Goodluck_2", "NPC_Yuji_Heroicsecond", "NPC_Yuji_Heroicsecond_1",
			"NPC_Yuji_Heroicsecond_2", "NPC_Yuki_Snowdrift", "NPC_Yuki_Snowdrift_1", "NPC_Yuki_Snowdrift_2", "NPC_Yuki_Snowdrift_3", "NPC_Yutaka_Abundant", "NPC_Yutaka_Abundant_1", "NPC_Yutaka_Abundant_2", "NPC_Yutaka_Abundant_3", "NPC_Yutaka_Abundant_4",
			"NPC_Yutaka_Abundant_5", "NPC_Yuto_Gentleheart", "NPC_Yuto_Gentleheart_1", "NPC_Yuto_Gentleheart_2", "NPC_Yuto_Gentleheart_3", "NPC_Yuto_Gentleheart_4", "NPC_Zen_Goodness", "NPC_Zen_Goodness_1", "NPC_Zen_Goodness_2", "NPC_Zen_Goodness_3",
			"Pool_Lifeguard_F_01", "Pool_Lifeguard_M_01", "Pool_Swimmer_F_01", "Pool_Swimmer_F_02", "Pool_Swimmer_F_03", "Pool_Swimmer_F_04", "Pool_Swimmer_M_01", "Pool_Swimmer_M_02", "Pool_Swimmer_M_03", "Pool_Swimmer_M_04"
		};

		// Token: 0x0400749A RID: 29850
		private static readonly HashSet<string> SyncLoadsReportedDuringMapCatalog = new HashSet<string>();
	}
}
