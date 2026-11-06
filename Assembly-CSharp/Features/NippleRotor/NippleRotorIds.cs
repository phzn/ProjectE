using System;
using UnityEngine;

namespace Features.NippleRotor
{
	// Token: 0x02000F5D RID: 3933
	public static class NippleRotorIds
	{
		// Token: 0x06007676 RID: 30326 RVA: 0x00301FB0 File Offset: 0x003001B0
		public static int NextLevel(int level)
		{
			if (level <= 0)
			{
				return 1;
			}
			if (level < 3)
			{
				return level + 1;
			}
			return 0;
		}

		// Token: 0x06007677 RID: 30327 RVA: 0x00301FC1 File Offset: 0x003001C1
		public static int ClampLevel(int level)
		{
			return Mathf.Clamp(level, 0, 3);
		}

		// Token: 0x0400799D RID: 31133
		public const string ItemId = "nipple_rotor";

		// Token: 0x0400799E RID: 31134
		public const string FeatureRoot = "Assets/Features/NippleRotor";

		// Token: 0x0400799F RID: 31135
		public const string ConfigResourcePath = "NippleRotor/NippleRotorConfig";

		// Token: 0x040079A0 RID: 31136
		public const string ItemAssetPath = "Assets/Features/NippleRotor/Data/NippleRotorItem.asset";

		// Token: 0x040079A1 RID: 31137
		public const string ConfigAssetPath = "Assets/Features/NippleRotor/Resources/NippleRotor/NippleRotorConfig.asset";

		// Token: 0x040079A2 RID: 31138
		public const string CupsPrefabPath = "Assets/Features/NippleRotor/Prefabs/NippleRotorCups.prefab";

		// Token: 0x040079A3 RID: 31139
		public const string RemotePrefabPath = "Assets/Features/NippleRotor/Prefabs/NippleRotorRemote.prefab";

		// Token: 0x040079A4 RID: 31140
		public const string HandPrefabPath = "Assets/Features/NippleRotor/Prefabs/NippleRotorHand.prefab";

		// Token: 0x040079A5 RID: 31141
		public const string FloorPrefabPath = "Assets/Features/NippleRotor/Prefabs/NippleRotor_FloorItem.prefab";

		// Token: 0x040079A6 RID: 31142
		public const string SourcePrefabPath = "Assets/SakuraTunaShop/ピンクローター/胸ローター.prefab";

		// Token: 0x040079A7 RID: 31143
		public const string LeftAnchorName = "LeftAnchor";

		// Token: 0x040079A8 RID: 31144
		public const string RightAnchorName = "RightAnchor";

		// Token: 0x040079A9 RID: 31145
		public const string BuzzName = "Buzz";

		// Token: 0x040079AA RID: 31146
		public const string CordSocketName = "CordSocket";

		// Token: 0x040079AB RID: 31147
		public const int LevelCount = 4;

		// Token: 0x040079AC RID: 31148
		public const KeyCode CycleKey = KeyCode.L;

		// Token: 0x040079AD RID: 31149
		public const string CycleKeyLabel = "L";

		// Token: 0x040079AE RID: 31150
		public const string CycleControllerLabel = "LT + R3";

		// Token: 0x040079AF RID: 31151
		public static readonly string[] LevelActionNames = new string[] { "Vibration: Off", "Vibration: Low", "Vibration: Medium", "Vibration: High" };
	}
}
