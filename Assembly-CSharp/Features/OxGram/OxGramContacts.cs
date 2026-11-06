using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Features.OxGram
{
	// Token: 0x02000E9E RID: 3742
	public static class OxGramContacts
	{
		// Token: 0x0600706F RID: 28783 RVA: 0x002DA380 File Offset: 0x002D8580
		public static OxGramContacts.Contact Find(string id)
		{
			foreach (OxGramContacts.Contact contact in OxGramContacts.All)
			{
				if (string.Equals(contact.id, id, StringComparison.Ordinal))
				{
					return contact;
				}
			}
			return null;
		}

		// Token: 0x06007070 RID: 28784 RVA: 0x002DA3DC File Offset: 0x002D85DC
		public static OxGramChat GetOrCreate(List<OxGramChat> chats, OxGramContacts.Contact contact)
		{
			if (chats == null || contact == null)
			{
				return null;
			}
			foreach (OxGramChat oxGramChat in chats)
			{
				if (!oxGramChat.isGroup && oxGramChat.contactId == contact.id)
				{
					return oxGramChat;
				}
			}
			OxGramChat oxGramChat2 = OxGramContacts.Create(contact);
			chats.Insert(0, oxGramChat2);
			return oxGramChat2;
		}

		// Token: 0x06007071 RID: 28785 RVA: 0x002DA45C File Offset: 0x002D865C
		public static OxGramChat Create(OxGramContacts.Contact contact)
		{
			return new OxGramChat
			{
				id = "dm:" + contact.id,
				contactId = contact.id,
				title = contact.name,
				isGroup = false,
				subtitle = "Direct chat",
				members = new List<string> { contact.name },
				ambientLines = new List<string> { "How's your day going?", "I'm here if you feel like chatting.", "Seen anything interesting today?" },
				messages = new List<OxGramMessage> { OxGramMessage.Text(OxGramSender.Other, contact.name, "Hey! Good to see you here.", DateTime.Now.ToString("HH:mm")) }
			};
		}

		// Token: 0x06007072 RID: 28786 RVA: 0x002DA528 File Offset: 0x002D8728
		public static bool TryNormalizeName(string value, out string name, out string error)
		{
			name = string.Empty;
			error = "Enter a contact name.";
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			try
			{
				name = value.Normalize(NormalizationForm.FormKC).Trim();
			}
			catch (ArgumentException)
			{
				error = "Use a name without special formatting.";
				return false;
			}
			if (name.Length > 24)
			{
				error = "Keep the contact name to 24 characters or fewer.";
				return false;
			}
			foreach (char c in name)
			{
				UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
				if (char.IsControl(c) || (char.IsWhiteSpace(c) && c != ' ') || unicodeCategory == UnicodeCategory.Format || c == '<' || c == '>' || c == '{' || c == '}')
				{
					error = "Use a name without tags, line breaks or special formatting.";
					return false;
				}
			}
			error = string.Empty;
			return true;
		}

		// Token: 0x06007073 RID: 28787 RVA: 0x002DA5F4 File Offset: 0x002D87F4
		public static bool TryRename(OxGramChat chat, string value, out string error)
		{
			error = "Only direct chats can be renamed.";
			if (chat == null || chat.isGroup)
			{
				return false;
			}
			string text;
			if (!OxGramContacts.TryNormalizeName(value, out text, out error))
			{
				return false;
			}
			chat.title = text;
			chat.members = new List<string> { text };
			foreach (OxGramMessage oxGramMessage in chat.messages)
			{
				if (oxGramMessage != null && oxGramMessage.sender == OxGramSender.Other)
				{
					oxGramMessage.authorName = text;
				}
			}
			return true;
		}

		// Token: 0x0400732A RID: 29482
		public const int MaxNameLength = 24;

		// Token: 0x0400732B RID: 29483
		public static readonly IReadOnlyList<OxGramContacts.Contact> All = new OxGramContacts.Contact[]
		{
			new OxGramContacts.Contact("emma", "Emma", "Oxpath CS — Year 2"),
			new OxGramContacts.Contact("liam", "Liam", "Oxpath CS — Year 2"),
			new OxGramContacts.Contact("sofia", "Sofia", "Oxpath CS — Year 2"),
			new OxGramContacts.Contact("kenji", "Kenji", "Tokyo"),
			new OxGramContacts.Contact("yuki", "Yuki", "Tokyo"),
			new OxGramContacts.Contact("nova", "nova", "Night Owls"),
			new OxGramContacts.Contact("ghost", "ghost", "Night Owls")
		};

		// Token: 0x02000E9F RID: 3743
		public sealed class Contact
		{
			// Token: 0x06007075 RID: 28789 RVA: 0x002DA749 File Offset: 0x002D8949
			public Contact(string id, string name, string group)
			{
				this.id = id;
				this.name = name;
				this.group = group;
			}

			// Token: 0x0400732C RID: 29484
			public readonly string id;

			// Token: 0x0400732D RID: 29485
			public readonly string name;

			// Token: 0x0400732E RID: 29486
			public readonly string group;
		}
	}
}
