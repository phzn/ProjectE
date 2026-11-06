using System;

namespace Features.CielOS.UI
{
	// Token: 0x020013B2 RID: 5042
	public static class CielOSAppIcons
	{
		// Token: 0x06009C0A RID: 39946 RVA: 0x003FBA50 File Offset: 0x003F9C50
		public static CielOSIconId ForApp(string appId)
		{
			string text = (appId ?? string.Empty).ToLowerInvariant();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 2521789228U)
			{
				if (num <= 910182291U)
				{
					if (num != 563467765U)
					{
						if (num != 905097718U)
						{
							if (num == 910182291U)
							{
								if (text == "weather")
								{
									return CielOSIconId.Weather;
								}
							}
						}
						else if (text == "customization")
						{
							return CielOSIconId.CustomizeCharacter;
						}
					}
					else if (text == "start_run")
					{
						return CielOSIconId.RunModes;
					}
				}
				else if (num <= 1689844019U)
				{
					if (num != 1055342736U)
					{
						if (num == 1689844019U)
						{
							if (text == "paint")
							{
								return CielOSIconId.Paint;
							}
						}
					}
					else if (text == "files")
					{
						return CielOSIconId.Files;
					}
				}
				else if (num != 1745255176U)
				{
					if (num == 2521789228U)
					{
						if (text == "leaderboard")
						{
							return CielOSIconId.Leaderboard;
						}
					}
				}
				else if (text == "settings")
				{
					return CielOSIconId.Settings;
				}
			}
			else if (num <= 2778257224U)
			{
				if (num != 2582907569U)
				{
					if (num != 2677821396U)
					{
						if (num == 2778257224U)
						{
							if (text == "cielx")
							{
								return CielOSIconId.CielX;
							}
						}
					}
					else if (text == "music")
					{
						return CielOSIconId.Music;
					}
				}
				else if (text == "terminal")
				{
					return CielOSIconId.Terminal;
				}
			}
			else if (num <= 3018300594U)
			{
				if (num != 2952127067U)
				{
					if (num == 3018300594U)
					{
						if (text == "story")
						{
							return CielOSIconId.Notebook;
						}
					}
				}
				else if (text == "snake")
				{
					return CielOSIconId.Snake;
				}
			}
			else if (num != 3481235046U)
			{
				if (num == 3767043248U)
				{
					if (text == "photos")
					{
						return CielOSIconId.Photos;
					}
				}
			}
			else if (text == "maps")
			{
				return CielOSIconId.Maps;
			}
			return CielOSIconId.Folder;
		}

		// Token: 0x06009C0B RID: 39947 RVA: 0x003FBC76 File Offset: 0x003F9E76
		public static string LogoResource(CielOSIconId id)
		{
			if (id == CielOSIconId.CielX)
			{
				return "CielX_Logo";
			}
			return null;
		}

		// Token: 0x06009C0C RID: 39948 RVA: 0x003FBC84 File Offset: 0x003F9E84
		public static string CategoryForApp(string appId)
		{
			string text = (appId ?? string.Empty).ToLowerInvariant();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 2521789228U)
			{
				if (num <= 1055342736U)
				{
					if (num != 910182291U)
					{
						if (num != 1055342736U)
						{
							goto IL_01A9;
						}
						if (!(text == "files"))
						{
							goto IL_01A9;
						}
					}
					else
					{
						if (!(text == "weather"))
						{
							goto IL_01A9;
						}
						return "Utilities";
					}
				}
				else if (num != 1689844019U)
				{
					if (num != 1745255176U)
					{
						if (num != 2521789228U)
						{
							goto IL_01A9;
						}
						if (!(text == "leaderboard"))
						{
							goto IL_01A9;
						}
						return "Games";
					}
					else
					{
						if (!(text == "settings"))
						{
							goto IL_01A9;
						}
						return "System";
					}
				}
				else
				{
					if (!(text == "paint"))
					{
						goto IL_01A9;
					}
					goto IL_0179;
				}
			}
			else if (num <= 2778257224U)
			{
				if (num != 2582907569U)
				{
					if (num != 2677821396U)
					{
						if (num != 2778257224U)
						{
							goto IL_01A9;
						}
						if (!(text == "cielx"))
						{
							goto IL_01A9;
						}
						return "Social";
					}
					else
					{
						if (!(text == "music"))
						{
							goto IL_01A9;
						}
						goto IL_0179;
					}
				}
				else if (!(text == "terminal"))
				{
					goto IL_01A9;
				}
			}
			else if (num != 2952127067U)
			{
				if (num != 3018300594U)
				{
					if (num != 3481235046U)
					{
						goto IL_01A9;
					}
					if (!(text == "maps"))
					{
						goto IL_01A9;
					}
					return "Utilities";
				}
				else
				{
					if (!(text == "story"))
					{
						goto IL_01A9;
					}
					return "Media";
				}
			}
			else
			{
				if (!(text == "snake"))
				{
					goto IL_01A9;
				}
				return "Games";
			}
			return "Tools";
			IL_0179:
			return "Media";
			IL_01A9:
			return "Other";
		}
	}
}
