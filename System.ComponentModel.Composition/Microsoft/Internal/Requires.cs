using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Internal
{
	// Token: 0x02000011 RID: 17
	internal static class Requires
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00002DBB File Offset: 0x00000FBB
		[DebuggerStepThrough]
		public static void NotNull<T>(T value, string parameterName) where T : class
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002DCC File Offset: 0x00000FCC
		[DebuggerStepThrough]
		public static void NotNullOrEmpty(string value, string parameterName)
		{
			Requires.NotNull<string>(value, parameterName);
			if (value.Length == 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ArgumentException_EmptyString, parameterName), parameterName);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002DF4 File Offset: 0x00000FF4
		[DebuggerStepThrough]
		public static void NotNullOrNullElements<T>(IEnumerable<T> values, string parameterName) where T : class
		{
			Requires.NotNull<IEnumerable<T>>(values, parameterName);
			Requires.NotNullElements<T>(values, parameterName);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002E04 File Offset: 0x00001004
		[DebuggerStepThrough]
		public static void NullOrNotNullElements<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> values, string parameterName) where TKey : class where TValue : class
		{
			Requires.NotNullElements<TKey, TValue>(values, parameterName);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002E0D File Offset: 0x0000100D
		[DebuggerStepThrough]
		public static void NullOrNotNullElements<T>(IEnumerable<T> values, string parameterName) where T : class
		{
			Requires.NotNullElements<T>(values, parameterName);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002E16 File Offset: 0x00001016
		private static void NotNullElements<T>(IEnumerable<T> values, string parameterName) where T : class
		{
			if (values != null)
			{
				if (!Contract.ForAll<T>(values, (T value) => value != null))
				{
					throw ExceptionBuilder.CreateContainsNullElement(parameterName);
				}
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E49 File Offset: 0x00001049
		private static void NotNullElements<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> values, string parameterName) where TKey : class where TValue : class
		{
			if (values != null)
			{
				if (!Contract.ForAll<KeyValuePair<TKey, TValue>>(values, (KeyValuePair<TKey, TValue> keyValue) => keyValue.Key != null && keyValue.Value != null))
				{
					throw ExceptionBuilder.CreateContainsNullElement(parameterName);
				}
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002E7C File Offset: 0x0000107C
		[DebuggerStepThrough]
		public static void IsInMembertypeSet(MemberTypes value, string parameterName, MemberTypes enumFlagSet)
		{
			if ((value & enumFlagSet) != value || (value & (value - 1)) != (MemberTypes)0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ArgumentOutOfRange_InvalidEnumInSet, parameterName, value, enumFlagSet.ToString()), parameterName);
			}
		}
	}
}
