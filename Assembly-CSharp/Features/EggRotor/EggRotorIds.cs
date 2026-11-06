using System;
using Features.NippleRotor;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x02001231 RID: 4657
	public static class EggRotorIds
	{
		// Token: 0x0600909C RID: 37020 RVA: 0x003B3846 File Offset: 0x003B1A46
		public static int NextLevel(int level)
		{
			return NippleRotorIds.NextLevel(level);
		}

		// Token: 0x0600909D RID: 37021 RVA: 0x003B384E File Offset: 0x003B1A4E
		public static int ClampLevel(int level)
		{
			return NippleRotorIds.ClampLevel(level);
		}

		// Token: 0x0600909E RID: 37022 RVA: 0x003B3856 File Offset: 0x003B1A56
		public static EggRotorPlacement NextPlacement(EggRotorPlacement current, bool crotchAvailable = true, bool futanari = false)
		{
			if (futanari)
			{
				if (current != EggRotorPlacement.Glans)
				{
					return EggRotorPlacement.Glans;
				}
				return EggRotorPlacement.Swing;
			}
			else
			{
				if (!crotchAvailable)
				{
					return EggRotorPlacement.Swing;
				}
				if (current == EggRotorPlacement.Inside)
				{
					return EggRotorPlacement.Outside;
				}
				if (current != EggRotorPlacement.Outside)
				{
					return EggRotorPlacement.Inside;
				}
				return EggRotorPlacement.Swing;
			}
		}

		// Token: 0x0600909F RID: 37023 RVA: 0x003B3876 File Offset: 0x003B1A76
		public static EggRotorPlacement ClampPlacement(int placement)
		{
			if (placement != 3)
			{
				return (EggRotorPlacement)Mathf.Clamp(placement, 0, 2);
			}
			return EggRotorPlacement.Glans;
		}

		// Token: 0x060090A0 RID: 37024 RVA: 0x003B3886 File Offset: 0x003B1A86
		public static EggRotorPlacement ForMode(EggRotorPlacement placement, bool futanari, EggRotorPlacement classicCrotch = EggRotorPlacement.Outside)
		{
			placement = EggRotorIds.ClampPlacement((int)placement);
			if (futanari)
			{
				if (!EggRotorIds.UsesCrotch(placement))
				{
					return placement;
				}
				return EggRotorPlacement.Glans;
			}
			else
			{
				if (placement != EggRotorPlacement.Glans)
				{
					return placement;
				}
				if (!EggRotorIds.UsesCrotch(classicCrotch))
				{
					return EggRotorPlacement.Outside;
				}
				return classicCrotch;
			}
		}

		// Token: 0x060090A1 RID: 37025 RVA: 0x003B38B0 File Offset: 0x003B1AB0
		public static bool UsesCrotch(EggRotorPlacement placement)
		{
			return placement == EggRotorPlacement.Inside || placement == EggRotorPlacement.Outside;
		}

		// Token: 0x060090A2 RID: 37026 RVA: 0x003B38BB File Offset: 0x003B1ABB
		public static bool Stimulates(EggRotorPlacement placement)
		{
			return EggRotorIds.UsesCrotch(placement) || placement == EggRotorPlacement.Glans;
		}

		// Token: 0x040094AF RID: 38063
		public const string ItemId = "egg_rotor";

		// Token: 0x040094B0 RID: 38064
		public const string ItemName = "Egg Rotor";

		// Token: 0x040094B1 RID: 38065
		public const string ItemDescription = "A pink egg vibrator on a cable, with a remote strapped to the thigh. Wear it inside, taped outside, or let it dangle; higher speeds buzz loud enough for people nearby to hear.";

		// Token: 0x040094B2 RID: 38066
		public const string FeatureRoot = "Assets/Features/EggRotor";

		// Token: 0x040094B3 RID: 38067
		public const string ConfigResourcePath = "EggRotor/EggRotorConfig";

		// Token: 0x040094B4 RID: 38068
		public const string ItemAssetPath = "Assets/Features/EggRotor/Data/EggRotorItem.asset";

		// Token: 0x040094B5 RID: 38069
		public const string ConfigAssetPath = "Assets/Features/EggRotor/Resources/EggRotor/EggRotorConfig.asset";

		// Token: 0x040094B6 RID: 38070
		public const string RemotePrefabPath = "Assets/Features/EggRotor/Prefabs/EggRotorRemote.prefab";

		// Token: 0x040094B7 RID: 38071
		public const string EggPrefabPath = "Assets/Features/EggRotor/Prefabs/EggRotorEgg.prefab";

		// Token: 0x040094B8 RID: 38072
		public const string HandPrefabPath = "Assets/Features/EggRotor/Prefabs/EggRotorHand.prefab";

		// Token: 0x040094B9 RID: 38073
		public const string FloorPrefabPath = "Assets/Features/EggRotor/Prefabs/EggRotor_FloorItem.prefab";

		// Token: 0x040094BA RID: 38074
		public const string MeshesFolder = "Assets/Features/EggRotor/Meshes";

		// Token: 0x040094BB RID: 38075
		public const string IconPath = "Assets/Features/EggRotor/Icons/EggRotorIcon.png";

		// Token: 0x040094BC RID: 38076
		public const string SourcePrefabPath = "Assets/CLAIVE/Pura²rotor/Milltina/Milltina_Pura²rotor_R.prefab";

		// Token: 0x040094BD RID: 38077
		public const string SourceFbxPath = "Assets/CLAIVE/Pura²rotor/Milltina/_FBX/Milltina_Pura²rotor_R.fbx";

		// Token: 0x040094BE RID: 38078
		public const string BuzzName = "Buzz";

		// Token: 0x040094BF RID: 38079
		public const string CordSocketName = "CordSocket";

		// Token: 0x040094C0 RID: 38080
		public const string TapeName = "Tape";

		// Token: 0x040094C1 RID: 38081
		public const string PussyAnchorName = "PussyAnchor";

		// Token: 0x040094C2 RID: 38082
		public const string RightThighBoneName = "thigh_stretch.r";

		// Token: 0x040094C3 RID: 38083
		public const string InsertionBlendShapeName = "Kupa_V_LR";

		// Token: 0x040094C4 RID: 38084
		public const int LevelCount = 4;

		// Token: 0x040094C5 RID: 38085
		public const int PlacementCount = 3;

		// Token: 0x040094C6 RID: 38086
		public const int AllPlacementCount = 4;

		// Token: 0x040094C7 RID: 38087
		public const string PenisMeshName = "sao";

		// Token: 0x040094C8 RID: 38088
		public const string PenisTipBoneName = "Bone.019";

		// Token: 0x040094C9 RID: 38089
		public const string PenisClothName = "Sao_Physics";

		// Token: 0x040094CA RID: 38090
		public const KeyCode ControlKey = KeyCode.Y;

		// Token: 0x040094CB RID: 38091
		public const float HoldSeconds = 0.45f;

		// Token: 0x040094CC RID: 38092
		public const string TapKeyLabel = "Y";

		// Token: 0x040094CD RID: 38093
		public const string HoldKeyLabel = "Hold Y";

		// Token: 0x040094CE RID: 38094
		public const string TapControllerLabel = "LT + R3";

		// Token: 0x040094CF RID: 38095
		public const string HoldControllerLabel = "Hold LT + R3";

		// Token: 0x040094D0 RID: 38096
		public const string UnboundControllerLabel = "-";

		// Token: 0x040094D1 RID: 38097
		public static readonly string[] LevelActionNames = new string[] { "Egg: Off", "Egg: Low", "Egg: Medium", "Egg: High" };

		// Token: 0x040094D2 RID: 38098
		public static readonly string[] PlacementActionNames = new string[] { "Egg position: Inside", "Egg position: Outside", "Egg position: Swing", "Egg position: Glans" };
	}
}
