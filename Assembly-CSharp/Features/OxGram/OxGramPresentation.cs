using System;
using Features.Localization;

namespace Features.OxGram
{
	// Token: 0x02000EAF RID: 3759
	internal static class OxGramPresentation
	{
		// Token: 0x060070CF RID: 28879 RVA: 0x002DC68C File Offset: 0x002DA88C
		internal static string MessageBody(OxGramMessage message)
		{
			if (message == null)
			{
				return string.Empty;
			}
			if (message.sender == OxGramSender.Me || GameLocalization.CurrentCode == "en")
			{
				return message.text;
			}
			if (message.reactionTemplate == null)
			{
				return GameLocalization.GetSource("phone_story", message.text);
			}
			if (string.IsNullOrWhiteSpace(message.reactionTemplate))
			{
				string text2;
				string text3;
				string text = (OxGramUser.TryNormalizeDisplayName(message.reactionAlias, out text2, out text3) ? text2 : "anonymous");
				return GameLocalization.FormatSource("phone_story", "{0}, I saw your photo.", new object[] { "@" + text });
			}
			return OxGramReactionPool.FormatReply(GameLocalization.GetSource("phone_story", message.reactionTemplate), message.reactionAlias);
		}

		// Token: 0x060070D0 RID: 28880 RVA: 0x002DC744 File Offset: 0x002DA944
		internal static string Preview(OxGramMessage message)
		{
			if (message == null)
			{
				return GameLocalization.GetSource("phone_story", "Be the first to say something.");
			}
			string text = ((message.sender == OxGramSender.Me) ? GameLocalization.GetSource("phone_story", "You") : message.authorName);
			if (message.kind != OxGramMessageKind.Photo)
			{
				return text + ": " + OxGramPresentation.MessageBody(message);
			}
			return GameLocalization.FormatSource("phone_story", "{0} shared a photo", new object[] { text });
		}
	}
}
