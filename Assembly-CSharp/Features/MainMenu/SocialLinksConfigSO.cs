using System;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x020010B2 RID: 4274
	[CreateAssetMenu(fileName = "SocialLinksConfig", menuName = "Main Menu/Social Links Config")]
	public class SocialLinksConfigSO : ScriptableObject
	{
		// Token: 0x06008179 RID: 33145 RVA: 0x0034E7DC File Offset: 0x0034C9DC
		public void Open(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				Debug.LogWarning("[SocialLinks] URL non configurato su '" + base.name + "'.");
				return;
			}
			Application.OpenURL(url);
		}

		// Token: 0x0600817A RID: 33146 RVA: 0x0034E807 File Offset: 0x0034CA07
		public void OpenDiscord()
		{
			this.Open(this.discordUrl);
		}

		// Token: 0x0600817B RID: 33147 RVA: 0x0034E815 File Offset: 0x0034CA15
		public void OpenX()
		{
			this.Open(this.xUrl);
		}

		// Token: 0x0600817C RID: 33148 RVA: 0x0034E823 File Offset: 0x0034CA23
		public void OpenItch()
		{
			this.Open(this.itchUrl);
		}

		// Token: 0x0600817D RID: 33149 RVA: 0x0034E831 File Offset: 0x0034CA31
		public void OpenSteam()
		{
			this.Open(this.steamUrl);
		}

		// Token: 0x040084B5 RID: 33973
		[Header("Social Links")]
		[Tooltip("Invito al server Discord.")]
		public string discordUrl = "https://discord.gg/";

		// Token: 0x040084B6 RID: 33974
		[Tooltip("Profilo X / Twitter.")]
		public string xUrl = "https://x.com/";

		// Token: 0x040084B7 RID: 33975
		[Tooltip("Pagina Itch.io del gioco.")]
		public string itchUrl = "https://itch.io/";

		// Token: 0x040084B8 RID: 33976
		[Tooltip("Pagina Steam del gioco (opzionale).")]
		public string steamUrl = "https://store.steampowered.com/";
	}
}
