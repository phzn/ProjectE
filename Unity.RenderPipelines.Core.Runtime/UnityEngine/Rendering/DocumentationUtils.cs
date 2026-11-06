using System;
using System.Linq;
using System.Reflection;

namespace UnityEngine.Rendering
{
	// Token: 0x02000081 RID: 129
	public static class DocumentationUtils
	{
		// Token: 0x06000409 RID: 1033 RVA: 0x0001187C File Offset: 0x0000FA7C
		public static string GetHelpURL<TEnum>(TEnum mask = default(TEnum)) where TEnum : struct, IConvertible
		{
			HelpURLAttribute helpURLAttribute = (HelpURLAttribute)mask.GetType().GetCustomAttributes(typeof(HelpURLAttribute), false).FirstOrDefault<object>();
			if (helpURLAttribute != null)
			{
				return string.Format("{0}#{1}", helpURLAttribute.URL, mask);
			}
			return string.Empty;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x000118D0 File Offset: 0x0000FAD0
		public static bool TryGetHelpURL(Type type, out string url)
		{
			HelpURLAttribute customAttribute = type.GetCustomAttribute<HelpURLAttribute>(false);
			url = ((customAttribute != null) ? customAttribute.URL : null);
			return customAttribute != null;
		}
	}
}
