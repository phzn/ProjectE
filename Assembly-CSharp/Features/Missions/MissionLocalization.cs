using System;
using Features.Localization;
using TMPro;
using UnityEngine;

namespace Features.Missions
{
	// Token: 0x02000F83 RID: 3971
	public static class MissionLocalization
	{
		// Token: 0x060077B3 RID: 30643 RVA: 0x0030C27A File Offset: 0x0030A47A
		public static string Text(string english)
		{
			return GameLocalization.GetSource("missions_ui", english);
		}

		// Token: 0x060077B4 RID: 30644 RVA: 0x0030C288 File Offset: 0x0030A488
		public static string Format(string english, params object[] arguments)
		{
			string text;
			GameLocalization.TryGetSourceKey("missions_ui", english, out text);
			return GameLocalization.Format(text, english, arguments);
		}

		// Token: 0x060077B5 RID: 30645 RVA: 0x0030C2AC File Offset: 0x0030A4AC
		public static LocalizedGameText Bind(TMP_Text label, string english, bool fit = false)
		{
			if (label == null)
			{
				return null;
			}
			string text;
			LocalizedGameText localizedGameText;
			if (GameLocalization.TryGetSourceKey("missions_ui", english, out text))
			{
				localizedGameText = LocalizedGameText.Bind(label, text, english, null);
			}
			else
			{
				localizedGameText = LocalizedGameText.BindFontOnly(label);
				label.text = english;
			}
			if (fit && localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			return localizedGameText;
		}

		// Token: 0x060077B6 RID: 30646 RVA: 0x0009FD2D File Offset: 0x0009DF2D
		public static void Set(TMP_Text label, string rendered)
		{
			if (label == null)
			{
				return;
			}
			LocalizedGameText.BindFontOnly(label);
			label.text = rendered;
		}

		// Token: 0x060077B7 RID: 30647 RVA: 0x0030C301 File Offset: 0x0030A501
		public static string Context(string english)
		{
			if (string.IsNullOrEmpty(english))
			{
				return english ?? string.Empty;
			}
			return GameLocalization.GetSource("poi", english);
		}

		// Token: 0x060077B8 RID: 30648 RVA: 0x0030C324 File Offset: 0x0030A524
		public static string Full(MissionState mission)
		{
			string text = ((mission != null) ? mission.description : null) ?? string.Empty;
			if (mission == null)
			{
				return text;
			}
			bool flag2;
			bool flag = MissionService.MatchesGeneratedDescription(mission, out flag2);
			if (flag && (mission.kind == MissionObjectiveKind.ClothesCollected || mission.kind == MissionObjectiveKind.ClothesWorn))
			{
				text = MissionService.BuildCurrentDescription(mission);
				if (flag2)
				{
					text += " across your account";
				}
			}
			if (GameLocalization.CurrentCode == "en")
			{
				return text;
			}
			string text2;
			if (GameLocalization.TryGetSourceKey("missions_account", text, out text2))
			{
				return GameLocalization.Get(text2, text);
			}
			if (!flag)
			{
				return text;
			}
			int num = Mathf.RoundToInt(mission.target);
			bool flag3 = MissionService.UsesRunCopy(mission.kind, mission.scope, num);
			string text3 = (flag3 ? MissionLocalizationTemplates.RunFull(mission.kind, num == 1) : MissionLocalizationTemplates.Full(mission.kind, num == 1));
			if (text3 == null)
			{
				return text;
			}
			string text4 = (flag3 ? "missions.run." : "missions.full.") + mission.kind.ToString();
			if (num == 1 && (flag3 ? MissionLocalizationTemplates.HasRunSingular(mission.kind) : MissionLocalizationTemplates.HasSingular(mission.kind)))
			{
				text4 += ".one";
			}
			string text5 = GameLocalization.Format(text4, text3, new object[]
			{
				num,
				MissionLocalization.Qualifier(mission, false),
				MissionLocalization.Qualifier(mission, true)
			});
			if (!string.IsNullOrWhiteSpace(mission.contextName))
			{
				text5 = MissionLocalization.Format("{0} in {1}", new object[]
				{
					text5,
					MissionLocalization.Context(mission.contextName)
				});
			}
			if (flag2)
			{
				text5 = MissionLocalization.Format("{0} across your account", new object[] { text5 });
			}
			return text5;
		}

		// Token: 0x060077B9 RID: 30649 RVA: 0x0030C4D8 File Offset: 0x0030A6D8
		public static string Compact(MissionState mission)
		{
			string text = RunMissionPresentation.CompactEnglish(mission);
			if (mission == null || GameLocalization.CurrentCode == "en")
			{
				return text;
			}
			bool flag = MissionService.IsStationEmote(mission);
			string text2 = (flag ? "Perform {0}" : MissionLocalizationTemplates.Hud(mission.kind));
			if (text2 == null)
			{
				return MissionLocalization.Full(mission);
			}
			return GameLocalization.Format("missions.hud." + mission.kind.ToString() + (flag ? ".station" : string.Empty), text2, new object[]
			{
				MissionLocalization.Qualifier(mission, false),
				MissionLocalization.Qualifier(mission, true)
			});
		}

		// Token: 0x060077BA RID: 30650 RVA: 0x0030C574 File Offset: 0x0030A774
		private static string Qualifier(MissionState mission, bool secondary)
		{
			string text = (secondary ? mission.secondaryQualifierName : mission.qualifierName) ?? string.Empty;
			if (mission.kind == MissionObjectiveKind.EmoteNearLandmark || (secondary && mission.kind == MissionObjectiveKind.SpecificEmoteNearLandmark))
			{
				string text2;
				if (GameLocalization.TryGetSourceKey("missions_landmarks", text, out text2))
				{
					return GameLocalization.Get(text2, text);
				}
				return MissionLocalization.Context(text);
			}
			else if (secondary && mission.kind == MissionObjectiveKind.WearableSpecialAction)
			{
				string text3;
				if (GameLocalization.TryGetSourceKey("missions_actions", text, out text3))
				{
					return GameLocalization.Get(text3, text);
				}
				return InteractionLocalization.ActionName(text);
			}
			else
			{
				MissionObjectiveKind kind = mission.kind;
				if (kind - MissionObjectiveKind.SpecificClothingWorn <= 4)
				{
					return GameLocalization.GetSource("items", text);
				}
				string text4;
				string text5;
				if (!GameLocalization.TryGetSourceKey("emotes", text, out text4) && GameLocalization.TryGetSourceKey("gameplay_prompts", text, out text5))
				{
					return GameLocalization.Get(text5, text);
				}
				return GameLocalization.GetSource("emotes", text);
			}
		}
	}
}
