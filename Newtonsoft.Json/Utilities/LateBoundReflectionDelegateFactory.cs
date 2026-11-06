using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005F RID: 95
	[NullableContext(1)]
	[Nullable(0)]
	internal class LateBoundReflectionDelegateFactory : ReflectionDelegateFactory
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x000166E7 File Offset: 0x000148E7
		internal static ReflectionDelegateFactory Instance
		{
			get
			{
				return LateBoundReflectionDelegateFactory._instance;
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000166F0 File Offset: 0x000148F0
		public override ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			ConstructorInfo c = method as ConstructorInfo;
			if (c != null)
			{
				return ([Nullable(new byte[] { 1, 2 })] object[] a) => c.Invoke(a);
			}
			return ([Nullable(new byte[] { 1, 2 })] object[] a) => method.Invoke(null, a);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001674C File Offset: 0x0001494C
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override MethodCall<T, object> CreateMethodCall<[Nullable(2)] T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			ConstructorInfo c = method as ConstructorInfo;
			if (c != null)
			{
				return (T o, [Nullable(new byte[] { 1, 2 })] object[] a) => c.Invoke(a);
			}
			return (T o, [Nullable(new byte[] { 1, 2 })] object[] a) => method.Invoke(o, a);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x000167A8 File Offset: 0x000149A8
		public override Func<T> CreateDefaultConstructor<[Nullable(2)] T>(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsValueType())
			{
				return () => (T)((object)Activator.CreateInstance(type));
			}
			ConstructorInfo constructorInfo = ReflectionUtils.GetDefaultConstructor(type, true);
			if (constructorInfo == null)
			{
				throw new InvalidOperationException("Unable to find default constructor for " + type.FullName);
			}
			return () => (T)((object)constructorInfo.Invoke(null));
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00016833 File Offset: 0x00014A33
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override Func<T, object> CreateGet<[Nullable(2)] T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return (T o) => propertyInfo.GetValue(o, null);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001685C File Offset: 0x00014A5C
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override Func<T, object> CreateGet<[Nullable(2)] T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return (T o) => fieldInfo.GetValue(o);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00016885 File Offset: 0x00014A85
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override Action<T, object> CreateSet<[Nullable(2)] T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return delegate(T o, [Nullable(2)] object v)
			{
				fieldInfo.SetValue(o, v);
			};
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x000168AE File Offset: 0x00014AAE
		[return: Nullable(new byte[] { 1, 1, 2 })]
		public override Action<T, object> CreateSet<[Nullable(2)] T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return delegate(T o, [Nullable(2)] object v)
			{
				propertyInfo.SetValue(o, v, null);
			};
		}

		// Token: 0x04000214 RID: 532
		private static readonly LateBoundReflectionDelegateFactory _instance = new LateBoundReflectionDelegateFactory();
	}
}
