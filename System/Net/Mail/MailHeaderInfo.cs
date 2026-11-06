using System;
using System.Collections.Generic;

namespace System.Net.Mail
{
	// Token: 0x020007F5 RID: 2037
	internal static class MailHeaderInfo
	{
		// Token: 0x06004125 RID: 16677 RVA: 0x000DF50C File Offset: 0x000DD70C
		static MailHeaderInfo()
		{
			for (int i = 0; i < MailHeaderInfo.s_headerInfo.Length; i++)
			{
				MailHeaderInfo.s_headerDictionary.Add(MailHeaderInfo.s_headerInfo[i].NormalizedName, i);
			}
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x000DF848 File Offset: 0x000DDA48
		internal static string GetString(MailHeaderID id)
		{
			if (id == MailHeaderID.Unknown || id == (MailHeaderID)33)
			{
				return null;
			}
			return MailHeaderInfo.s_headerInfo[(int)id].NormalizedName;
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x000DF868 File Offset: 0x000DDA68
		internal static MailHeaderID GetID(string name)
		{
			int num;
			if (!MailHeaderInfo.s_headerDictionary.TryGetValue(name, out num))
			{
				return MailHeaderID.Unknown;
			}
			return (MailHeaderID)num;
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x000DF888 File Offset: 0x000DDA88
		internal static bool IsUserSettable(string name)
		{
			int num;
			return !MailHeaderInfo.s_headerDictionary.TryGetValue(name, out num) || MailHeaderInfo.s_headerInfo[num].IsUserSettable;
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x000DF8B8 File Offset: 0x000DDAB8
		internal static bool IsSingleton(string name)
		{
			int num;
			return MailHeaderInfo.s_headerDictionary.TryGetValue(name, out num) && MailHeaderInfo.s_headerInfo[num].IsSingleton;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x000DF8E8 File Offset: 0x000DDAE8
		internal static string NormalizeCase(string name)
		{
			int num;
			if (!MailHeaderInfo.s_headerDictionary.TryGetValue(name, out num))
			{
				return name;
			}
			return MailHeaderInfo.s_headerInfo[num].NormalizedName;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x000DF918 File Offset: 0x000DDB18
		internal static bool AllowsUnicode(string name)
		{
			int num;
			return !MailHeaderInfo.s_headerDictionary.TryGetValue(name, out num) || MailHeaderInfo.s_headerInfo[num].AllowsUnicode;
		}

		// Token: 0x04002715 RID: 10005
		private static readonly MailHeaderInfo.HeaderInfo[] s_headerInfo = new MailHeaderInfo.HeaderInfo[]
		{
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Bcc, "Bcc", true, false, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Cc, "Cc", true, false, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Comments, "Comments", false, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentDescription, "Content-Description", true, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentDisposition, "Content-Disposition", true, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentID, "Content-ID", true, false, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentLocation, "Content-Location", true, false, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentTransferEncoding, "Content-Transfer-Encoding", true, false, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentType, "Content-Type", true, false, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Date, "Date", true, false, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.From, "From", true, false, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Importance, "Importance", true, false, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.InReplyTo, "In-Reply-To", true, true, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Keywords, "Keywords", false, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Max, "Max", false, true, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.MessageID, "Message-ID", true, true, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.MimeVersion, "MIME-Version", true, false, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Priority, "Priority", true, false, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.References, "References", true, true, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ReplyTo, "Reply-To", true, false, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentBcc, "Resent-Bcc", false, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentCc, "Resent-Cc", false, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentDate, "Resent-Date", false, true, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentFrom, "Resent-From", false, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentMessageID, "Resent-Message-ID", false, true, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentSender, "Resent-Sender", false, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentTo, "Resent-To", false, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Sender, "Sender", true, false, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Subject, "Subject", true, false, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.To, "To", true, false, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.XPriority, "X-Priority", true, false, false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.XReceiver, "X-Receiver", false, true, true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.XSender, "X-Sender", true, true, true)
		};

		// Token: 0x04002716 RID: 10006
		private static readonly Dictionary<string, int> s_headerDictionary = new Dictionary<string, int>(33, StringComparer.OrdinalIgnoreCase);

		// Token: 0x020007F6 RID: 2038
		private readonly struct HeaderInfo
		{
			// Token: 0x0600412C RID: 16684 RVA: 0x000DF946 File Offset: 0x000DDB46
			public HeaderInfo(MailHeaderID id, string name, bool isSingleton, bool isUserSettable, bool allowsUnicode)
			{
				this.ID = id;
				this.NormalizedName = name;
				this.IsSingleton = isSingleton;
				this.IsUserSettable = isUserSettable;
				this.AllowsUnicode = allowsUnicode;
			}

			// Token: 0x04002717 RID: 10007
			public readonly string NormalizedName;

			// Token: 0x04002718 RID: 10008
			public readonly bool IsSingleton;

			// Token: 0x04002719 RID: 10009
			public readonly MailHeaderID ID;

			// Token: 0x0400271A RID: 10010
			public readonly bool IsUserSettable;

			// Token: 0x0400271B RID: 10011
			public readonly bool AllowsUnicode;
		}
	}
}
