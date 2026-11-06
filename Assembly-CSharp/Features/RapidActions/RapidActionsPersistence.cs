using System;
using UnityEngine;

namespace Features.RapidActions
{
	// Token: 0x02000D96 RID: 3478
	public static class RapidActionsPersistence
	{
		// Token: 0x0600666A RID: 26218 RVA: 0x0028F593 File Offset: 0x0028D793
		private static string FutanariSuffix(bool futanari)
		{
			if (!futanari)
			{
				return ".Std";
			}
			return ".Futa";
		}

		// Token: 0x0600666B RID: 26219 RVA: 0x0028F5A3 File Offset: 0x0028D7A3
		public static int LoadArmsPoseIndex(bool exhibit, int defaultValue)
		{
			return PlayerPrefs.GetInt(exhibit ? "RapidActions.Arms.Confident" : "RapidActions.Arms.Embarrassed", defaultValue);
		}

		// Token: 0x0600666C RID: 26220 RVA: 0x0028F5BA File Offset: 0x0028D7BA
		public static void SaveArmsPoseIndex(bool exhibit, int index)
		{
			PlayerPrefs.SetInt(exhibit ? "RapidActions.Arms.Confident" : "RapidActions.Arms.Embarrassed", index);
			PlayerPrefs.Save();
		}

		// Token: 0x0600666D RID: 26221 RVA: 0x0028F5D6 File Offset: 0x0028D7D6
		public static bool HasUnifiedArmsPose()
		{
			return PlayerPrefs.HasKey("RapidActions.Arms.Unified.Index");
		}

		// Token: 0x0600666E RID: 26222 RVA: 0x0028F5E2 File Offset: 0x0028D7E2
		public static void LoadUnifiedArmsPose(out bool exhibit, out int index)
		{
			exhibit = PlayerPrefs.GetInt("RapidActions.Arms.Unified.Exhibit", 0) != 0;
			index = PlayerPrefs.GetInt("RapidActions.Arms.Unified.Index", 0);
		}

		// Token: 0x0600666F RID: 26223 RVA: 0x0028F601 File Offset: 0x0028D801
		public static void SaveUnifiedArmsPose(bool exhibit, int index)
		{
			PlayerPrefs.SetInt("RapidActions.Arms.Unified.Exhibit", exhibit ? 1 : 0);
			PlayerPrefs.SetInt("RapidActions.Arms.Unified.Index", index);
			PlayerPrefs.Save();
		}

		// Token: 0x06006670 RID: 26224 RVA: 0x0028F624 File Offset: 0x0028D824
		public static bool HasEmoteSlot(int slotIndex, bool futanari)
		{
			return PlayerPrefs.HasKey("RapidActions.EmoteSlot." + slotIndex.ToString() + RapidActionsPersistence.FutanariSuffix(futanari));
		}

		// Token: 0x06006671 RID: 26225 RVA: 0x0028F642 File Offset: 0x0028D842
		public static string LoadEmoteSlot(int slotIndex, bool futanari, string defaultEmoteId)
		{
			return PlayerPrefs.GetString("RapidActions.EmoteSlot." + slotIndex.ToString() + RapidActionsPersistence.FutanariSuffix(futanari), defaultEmoteId ?? string.Empty);
		}

		// Token: 0x06006672 RID: 26226 RVA: 0x0028F66A File Offset: 0x0028D86A
		public static void SaveEmoteSlot(int slotIndex, bool futanari, string emoteId)
		{
			PlayerPrefs.SetString("RapidActions.EmoteSlot." + slotIndex.ToString() + RapidActionsPersistence.FutanariSuffix(futanari), emoteId ?? string.Empty);
			PlayerPrefs.Save();
		}

		// Token: 0x06006673 RID: 26227 RVA: 0x0028F697 File Offset: 0x0028D897
		public static bool HasCrouchEmoteId(bool futanari)
		{
			return PlayerPrefs.HasKey("RapidActions.Crouch.Emote" + RapidActionsPersistence.FutanariSuffix(futanari));
		}

		// Token: 0x06006674 RID: 26228 RVA: 0x0028F6AE File Offset: 0x0028D8AE
		public static string LoadCrouchEmoteId(bool futanari, string defaultEmoteId)
		{
			return PlayerPrefs.GetString("RapidActions.Crouch.Emote" + RapidActionsPersistence.FutanariSuffix(futanari), defaultEmoteId ?? string.Empty);
		}

		// Token: 0x06006675 RID: 26229 RVA: 0x0028F6CF File Offset: 0x0028D8CF
		public static void SaveCrouchEmoteId(bool futanari, string emoteId)
		{
			PlayerPrefs.SetString("RapidActions.Crouch.Emote" + RapidActionsPersistence.FutanariSuffix(futanari), emoteId ?? string.Empty);
			PlayerPrefs.Save();
		}

		// Token: 0x040068F8 RID: 26872
		private const string KeyArmsConfident = "RapidActions.Arms.Confident";

		// Token: 0x040068F9 RID: 26873
		private const string KeyArmsEmbarrassed = "RapidActions.Arms.Embarrassed";

		// Token: 0x040068FA RID: 26874
		private const string KeyArmsUnifiedExhibit = "RapidActions.Arms.Unified.Exhibit";

		// Token: 0x040068FB RID: 26875
		private const string KeyArmsUnifiedIndex = "RapidActions.Arms.Unified.Index";

		// Token: 0x040068FC RID: 26876
		private const string KeyEmoteSlotPrefix = "RapidActions.EmoteSlot.";

		// Token: 0x040068FD RID: 26877
		private const string KeyCrouchEmote = "RapidActions.Crouch.Emote";
	}
}
