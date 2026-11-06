using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Rank;

namespace Features.NipplePiercings
{
	// Token: 0x02000F69 RID: 3945
	public static class NipplePiercingRankPolicy
	{
		// Token: 0x060076E4 RID: 30436 RVA: 0x003057C8 File Offset: 0x003039C8
		public static string GetRequiredRankId(MCStylePresetSO preset)
		{
			string text = ((preset != null) ? preset.presetId : null);
			if (string.IsNullOrEmpty(text) || !text.StartsWith("nipple_piercing_", StringComparison.Ordinal))
			{
				return null;
			}
			string text2 = text.Substring("nipple_piercing_".Length);
			if (text2.EndsWith("_Left", StringComparison.Ordinal))
			{
				text2 = text2.Substring(0, text2.Length - 5);
			}
			if (text2.EndsWith("_Right", StringComparison.Ordinal))
			{
				text2 = text2.Substring(0, text2.Length - 6);
			}
			text2 = text2.Replace("_Single_", "_");
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num > 1132387910U)
			{
				if (num <= 2184180604U)
				{
					if (num != 1791095585U)
					{
						if (num != 2184180604U)
						{
							goto IL_01DB;
						}
						if (!(text2 == "CaptiveBeadRings"))
						{
							goto IL_01DB;
						}
						goto IL_01C3;
					}
					else if (!(text2 == "ONB_NippleRing_OMNK_Silver"))
					{
						goto IL_01DB;
					}
				}
				else if (num != 2405111541U)
				{
					if (num != 2510665142U)
					{
						if (num != 3877353632U)
						{
							goto IL_01DB;
						}
						if (!(text2 == "HeartWithArrow"))
						{
							goto IL_01DB;
						}
						goto IL_01C9;
					}
					else if (!(text2 == "ONB_NippleRing_OMNK_Gold"))
					{
						goto IL_01DB;
					}
				}
				else
				{
					if (!(text2 == "NipplePiercingWithChain"))
					{
						goto IL_01DB;
					}
					goto IL_01CF;
				}
				return "hornyslut";
			}
			if (num <= 492126127U)
			{
				if (num != 256058732U)
				{
					if (num != 492126127U)
					{
						goto IL_01DB;
					}
					if (!(text2 == "ShieldPiercing"))
					{
						goto IL_01DB;
					}
					goto IL_01C9;
				}
				else
				{
					if (!(text2 == "NipplePiercingCross"))
					{
						goto IL_01DB;
					}
					goto IL_01CF;
				}
			}
			else if (num != 577643153U)
			{
				if (num != 936384807U)
				{
					if (num != 1132387910U)
					{
						goto IL_01DB;
					}
					if (!(text2 == "JeweledPiercing"))
					{
						goto IL_01DB;
					}
					goto IL_01C9;
				}
				else if (!(text2 == "NippleClickers"))
				{
					goto IL_01DB;
				}
			}
			else if (!(text2 == "StraightBarbell"))
			{
				goto IL_01DB;
			}
			IL_01C3:
			return "curious";
			IL_01C9:
			return "aroused";
			IL_01CF:
			return "horny";
			IL_01DB:
			return "hornyslut_declared";
		}

		// Token: 0x060076E5 RID: 30437 RVA: 0x003059B8 File Offset: 0x00303BB8
		public static bool IsPresetAvailable(MCStylePresetSO preset)
		{
			if (preset == null)
			{
				return false;
			}
			string requiredRankId = NipplePiercingRankPolicy.GetRequiredRankId(preset);
			if (requiredRankId == null || RankProgressionService.IsAdminRankActive)
			{
				return true;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			List<RankDefinitionSO> list = ((config != null) ? config.ranksInOrder : null);
			if (list == null)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null && string.Equals(list[i].rankId, requiredRankId, StringComparison.Ordinal))
				{
					return RankProgressionService.RankIndex >= i;
				}
			}
			return false;
		}

		// Token: 0x04007A2B RID: 31275
		public const string PartId = "Accessory.NipplePiercing";

		// Token: 0x04007A2C RID: 31276
		private const string PresetPrefix = "nipple_piercing_";

		// Token: 0x04007A2D RID: 31277
		private const string FinalRankId = "hornyslut_declared";
	}
}
