using System;
using Features.Localization;
using Features.Rank;
using Features.RunModes;
using ProjectExhibitionist.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E31 RID: 3633
	public static class PhoneAppAccessPolicy
	{
		// Token: 0x06006C5C RID: 27740 RVA: 0x002B580C File Offset: 0x002B3A0C
		public static PhoneAppAccess GetAccess(string appId)
		{
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			return PhoneAppAccessPolicy.GetAccess(appId, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive, RunModeService.CurrentMode);
		}

		// Token: 0x06006C5D RID: 27741 RVA: 0x002B5830 File Offset: 0x002B3A30
		public static PhoneAppAccess GetAccess(string appId, string rankId, bool admin, RunModeKind mode)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(appId);
			if (num <= 1481252212U)
			{
				if (num <= 1005578654U)
				{
					if (num <= 874369225U)
					{
						if (num != 469332941U)
						{
							if (num != 874369225U)
							{
								goto IL_025A;
							}
							if (!(appId == "Call"))
							{
								goto IL_025A;
							}
							return PhoneAppAccess.Unavailable;
						}
						else
						{
							if (!(appId == "OxGram"))
							{
								goto IL_025A;
							}
							return PhoneAppAccess.Available;
						}
					}
					else if (num != 912196998U)
					{
						if (num != 1005578654U)
						{
							goto IL_025A;
						}
						if (!(appId == "Missions"))
						{
							goto IL_025A;
						}
						return PhoneAppAccess.Available;
					}
					else if (!(appId == "Maps"))
					{
						goto IL_025A;
					}
				}
				else if (num <= 1151856721U)
				{
					if (num != 1107743617U)
					{
						if (num != 1151856721U)
						{
							goto IL_025A;
						}
						if (!(appId == "Map"))
						{
							goto IL_025A;
						}
					}
					else
					{
						if (!(appId == "Banking"))
						{
							goto IL_025A;
						}
						return PhoneAppAccess.Unavailable;
					}
				}
				else if (num != 1461820269U)
				{
					if (num != 1481252212U)
					{
						goto IL_025A;
					}
					if (!(appId == "Music"))
					{
						goto IL_025A;
					}
					return PhoneAppAccess.Available;
				}
				else
				{
					if (!(appId == "Calculator"))
					{
						goto IL_025A;
					}
					return PhoneAppAccess.Available;
				}
				if (mode != RunModeKind.LongWayHome)
				{
					return PhoneAppAccess.LongWayHomeOnly;
				}
				return PhoneAppAccess.Available;
			}
			else if (num <= 3366850832U)
			{
				if (num <= 2920208772U)
				{
					if (num != 2556511663U)
					{
						if (num != 2920208772U)
						{
							goto IL_025A;
						}
						if (!(appId == "Message"))
						{
							goto IL_025A;
						}
						return PhoneAppAccess.Unavailable;
					}
					else if (!(appId == "Health"))
					{
						goto IL_025A;
					}
				}
				else if (num != 3263580745U)
				{
					if (num != 3366850832U)
					{
						goto IL_025A;
					}
					if (!(appId == "Contacts"))
					{
						goto IL_025A;
					}
					return PhoneAppAccess.Unavailable;
				}
				else if (!(appId == "MusicPlayer"))
				{
					goto IL_025A;
				}
			}
			else if (num <= 3660099683U)
			{
				if (num != 3618983133U)
				{
					if (num != 3660099683U)
					{
						goto IL_025A;
					}
					if (!(appId == "CielGram"))
					{
						goto IL_025A;
					}
				}
				else if (!(appId == "Gallery"))
				{
					goto IL_025A;
				}
			}
			else if (num != 3880549230U)
			{
				if (num != 3948127682U)
				{
					if (num != 4167114709U)
					{
						goto IL_025A;
					}
					if (!(appId == "Messages"))
					{
						goto IL_025A;
					}
					return PhoneAppAccess.Unavailable;
				}
				else if (!(appId == "Timer"))
				{
					goto IL_025A;
				}
			}
			else if (!(appId == "Camera"))
			{
				goto IL_025A;
			}
			return PhoneAppAccess.Available;
			IL_025A:
			if (!admin && !(rankId == "hornyslut_declared"))
			{
				return PhoneAppAccess.RankLocked;
			}
			return PhoneAppAccess.Available;
		}

		// Token: 0x06006C5E RID: 27742 RVA: 0x002B5AAC File Offset: 0x002B3CAC
		public static string GetMessage(PhoneAppAccess access)
		{
			switch (access)
			{
			case PhoneAppAccess.RankLocked:
				return GameLocalization.FormatSource("phone_story", "Reach Certified Exhibitionist to unlock this app.", Array.Empty<object>());
			case PhoneAppAccess.Unavailable:
				return GameLocalization.FormatSource("phone_story", "This app isn't available.", Array.Empty<object>());
			case PhoneAppAccess.LongWayHomeOnly:
				return GameLocalization.FormatSource("phone_story", "Maps is only available in Long Way Home.", Array.Empty<object>());
			default:
				return string.Empty;
			}
		}

		// Token: 0x06006C5F RID: 27743 RVA: 0x002B5B14 File Offset: 0x002B3D14
		public static bool TryOpen(string appId)
		{
			PhoneAppAccess access = PhoneAppAccessPolicy.GetAccess(appId);
			if (access == PhoneAppAccess.Available)
			{
				return true;
			}
			ToastService.ShowComputed(() => PhoneAppAccessPolicy.GetMessage(access), "Phone", -1f, "phone_story", null);
			return false;
		}
	}
}
