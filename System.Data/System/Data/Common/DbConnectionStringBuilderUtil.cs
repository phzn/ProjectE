using System;
using System.Data.SqlClient;
using System.Reflection;

namespace System.Data.Common
{
	// Token: 0x020003CB RID: 971
	internal static class DbConnectionStringBuilderUtil
	{
		// Token: 0x06002F0D RID: 12045 RVA: 0x000C9E78 File Offset: 0x000C8078
		internal static bool ConvertToBoolean(object value)
		{
			string text = value as string;
			if (text == null)
			{
				bool flag;
				try
				{
					flag = Convert.ToBoolean(value);
				}
				catch (InvalidCastException ex)
				{
					throw ADP.ConvertFailed(value.GetType(), typeof(bool), ex);
				}
				return flag;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "true") || StringComparer.OrdinalIgnoreCase.Equals(text, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "false") || StringComparer.OrdinalIgnoreCase.Equals(text, "no"))
			{
				return false;
			}
			string text2 = text.Trim();
			return StringComparer.OrdinalIgnoreCase.Equals(text2, "true") || StringComparer.OrdinalIgnoreCase.Equals(text2, "yes") || (!StringComparer.OrdinalIgnoreCase.Equals(text2, "false") && !StringComparer.OrdinalIgnoreCase.Equals(text2, "no") && bool.Parse(text));
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000C9F6C File Offset: 0x000C816C
		internal static bool ConvertToIntegratedSecurity(object value)
		{
			string text = value as string;
			if (text == null)
			{
				bool flag;
				try
				{
					flag = Convert.ToBoolean(value);
				}
				catch (InvalidCastException ex)
				{
					throw ADP.ConvertFailed(value.GetType(), typeof(bool), ex);
				}
				return flag;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "sspi") || StringComparer.OrdinalIgnoreCase.Equals(text, "true") || StringComparer.OrdinalIgnoreCase.Equals(text, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "false") || StringComparer.OrdinalIgnoreCase.Equals(text, "no"))
			{
				return false;
			}
			string text2 = text.Trim();
			return StringComparer.OrdinalIgnoreCase.Equals(text2, "sspi") || StringComparer.OrdinalIgnoreCase.Equals(text2, "true") || StringComparer.OrdinalIgnoreCase.Equals(text2, "yes") || (!StringComparer.OrdinalIgnoreCase.Equals(text2, "false") && !StringComparer.OrdinalIgnoreCase.Equals(text2, "no") && bool.Parse(text));
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000CA084 File Offset: 0x000C8284
		internal static int ConvertToInt32(object value)
		{
			int num;
			try
			{
				num = Convert.ToInt32(value);
			}
			catch (InvalidCastException ex)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(int), ex);
			}
			return num;
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000CA0C4 File Offset: 0x000C82C4
		internal static string ConvertToString(object value)
		{
			string text;
			try
			{
				text = Convert.ToString(value);
			}
			catch (InvalidCastException ex)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(string), ex);
			}
			return text;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000CA104 File Offset: 0x000C8304
		internal static bool TryConvertToApplicationIntent(string value, out ApplicationIntent result)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(value, "ReadOnly"))
			{
				result = ApplicationIntent.ReadOnly;
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(value, "ReadWrite"))
			{
				result = ApplicationIntent.ReadWrite;
				return true;
			}
			result = ApplicationIntent.ReadWrite;
			return false;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000CA138 File Offset: 0x000C8338
		internal static bool IsValidApplicationIntentValue(ApplicationIntent value)
		{
			return value == ApplicationIntent.ReadOnly || value == ApplicationIntent.ReadWrite;
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000CA144 File Offset: 0x000C8344
		internal static string ApplicationIntentToString(ApplicationIntent value)
		{
			if (value == ApplicationIntent.ReadOnly)
			{
				return "ReadOnly";
			}
			return "ReadWrite";
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000CA158 File Offset: 0x000C8358
		internal static ApplicationIntent ConvertToApplicationIntent(string keyword, object value)
		{
			string text = value as string;
			if (text != null)
			{
				ApplicationIntent applicationIntent;
				if (DbConnectionStringBuilderUtil.TryConvertToApplicationIntent(text, out applicationIntent))
				{
					return applicationIntent;
				}
				text = text.Trim();
				if (DbConnectionStringBuilderUtil.TryConvertToApplicationIntent(text, out applicationIntent))
				{
					return applicationIntent;
				}
				throw ADP.InvalidConnectionOptionValue(keyword);
			}
			else
			{
				ApplicationIntent applicationIntent2;
				if (value is ApplicationIntent)
				{
					applicationIntent2 = (ApplicationIntent)value;
				}
				else
				{
					if (value.GetType().GetTypeInfo().IsEnum)
					{
						throw ADP.ConvertFailed(value.GetType(), typeof(ApplicationIntent), null);
					}
					try
					{
						applicationIntent2 = (ApplicationIntent)Enum.ToObject(typeof(ApplicationIntent), value);
					}
					catch (ArgumentException ex)
					{
						throw ADP.ConvertFailed(value.GetType(), typeof(ApplicationIntent), ex);
					}
				}
				if (DbConnectionStringBuilderUtil.IsValidApplicationIntentValue(applicationIntent2))
				{
					return applicationIntent2;
				}
				throw ADP.InvalidEnumerationValue(typeof(ApplicationIntent), (int)applicationIntent2);
			}
		}

		// Token: 0x04001C06 RID: 7174
		private const string ApplicationIntentReadWriteString = "ReadWrite";

		// Token: 0x04001C07 RID: 7175
		private const string ApplicationIntentReadOnlyString = "ReadOnly";
	}
}
