using System;
using System.Linq;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001BD RID: 445
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class FeedbackHelpAttribute : Attribute
	{
		// Token: 0x06000A98 RID: 2712 RVA: 0x0002FAEA File Offset: 0x0002DCEA
		public FeedbackHelpAttribute(string helpText)
		{
			this.HelpText = helpText;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002FAFC File Offset: 0x0002DCFC
		public static string GetFeedbackHelpText(Type type)
		{
			FeedbackHelpAttribute feedbackHelpAttribute = type.GetCustomAttributes(false).OfType<FeedbackHelpAttribute>().FirstOrDefault<FeedbackHelpAttribute>();
			if (feedbackHelpAttribute == null)
			{
				return "";
			}
			return feedbackHelpAttribute.HelpText;
		}

		// Token: 0x04000C98 RID: 3224
		public string HelpText;
	}
}
