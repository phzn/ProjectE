using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000064 RID: 100
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class ReflectionDelegateFactory
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x00016D14 File Offset: 0x00014F14
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public Func<T, object> CreateGet<[Nullable(2)] T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				if (propertyInfo.PropertyType.IsByRef)
				{
					throw new InvalidOperationException("Could not create getter for {0}. ByRef return values are not supported.".FormatWith(CultureInfo.InvariantCulture, propertyInfo));
				}
				return this.CreateGet<T>(propertyInfo);
			}
			else
			{
				FieldInfo fieldInfo = memberInfo as FieldInfo;
				if (fieldInfo != null)
				{
					return this.CreateGet<T>(fieldInfo);
				}
				throw new Exception("Could not create getter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00016D80 File Offset: 0x00014F80
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public Action<T, object> CreateSet<[Nullable(2)] T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return this.CreateSet<T>(propertyInfo);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return this.CreateSet<T>(fieldInfo);
			}
			throw new Exception("Could not create setter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
		}

		// Token: 0x0600056B RID: 1387
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public abstract MethodCall<T, object> CreateMethodCall<[Nullable(2)] T>(MethodBase method);

		// Token: 0x0600056C RID: 1388
		public abstract ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method);

		// Token: 0x0600056D RID: 1389
		public abstract Func<T> CreateDefaultConstructor<[Nullable(2)] T>(Type type);

		// Token: 0x0600056E RID: 1390
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public abstract Func<T, object> CreateGet<[Nullable(2)] T>(PropertyInfo propertyInfo);

		// Token: 0x0600056F RID: 1391
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public abstract Func<T, object> CreateGet<[Nullable(2)] T>(FieldInfo fieldInfo);

		// Token: 0x06000570 RID: 1392
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public abstract Action<T, object> CreateSet<[Nullable(2)] T>(FieldInfo fieldInfo);

		// Token: 0x06000571 RID: 1393
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public abstract Action<T, object> CreateSet<[Nullable(2)] T>(PropertyInfo propertyInfo);
	}
}
