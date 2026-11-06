using System;
using System.Linq;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001BC RID: 444
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class FeedbackPathAttribute : Attribute
	{
		// Token: 0x06000A95 RID: 2709 RVA: 0x0002FA6D File Offset: 0x0002DC6D
		public FeedbackPathAttribute(string path)
		{
			this.Path = path;
			this.Name = path.Split('/', StringSplitOptions.None).Last<string>();
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0002FA90 File Offset: 0x0002DC90
		public static string GetFeedbackDefaultName(Type type)
		{
			FeedbackPathAttribute feedbackPathAttribute = type.GetCustomAttributes(false).OfType<FeedbackPathAttribute>().FirstOrDefault<FeedbackPathAttribute>();
			if (feedbackPathAttribute == null)
			{
				return type.Name;
			}
			return feedbackPathAttribute.Name;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0002FAC0 File Offset: 0x0002DCC0
		public static string GetFeedbackDefaultPath(Type type)
		{
			FeedbackPathAttribute feedbackPathAttribute = type.GetCustomAttributes(false).OfType<FeedbackPathAttribute>().FirstOrDefault<FeedbackPathAttribute>();
			if (feedbackPathAttribute == null)
			{
				return null;
			}
			return feedbackPathAttribute.Path;
		}

		// Token: 0x04000C96 RID: 3222
		public string Path;

		// Token: 0x04000C97 RID: 3223
		public string Name;
	}
}
