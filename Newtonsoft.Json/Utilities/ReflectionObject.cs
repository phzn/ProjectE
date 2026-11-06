using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000066 RID: 102
	[NullableContext(1)]
	[Nullable(0)]
	internal class ReflectionObject
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00016E09 File Offset: 0x00015009
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> Creator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00016E11 File Offset: 0x00015011
		public IDictionary<string, ReflectionMember> Members { get; }

		// Token: 0x0600057C RID: 1404 RVA: 0x00016E19 File Offset: 0x00015019
		private ReflectionObject([Nullable(new byte[] { 2, 1 })] ObjectConstructor<object> creator)
		{
			this.Members = new Dictionary<string, ReflectionMember>();
			this.Creator = creator;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00016E33 File Offset: 0x00015033
		[return: Nullable(2)]
		public object GetValue(object target, string member)
		{
			return this.Members[member].Getter(target);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00016E4C File Offset: 0x0001504C
		public void SetValue(object target, string member, [Nullable(2)] object value)
		{
			this.Members[member].Setter(target, value);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00016E66 File Offset: 0x00015066
		public Type GetType(string member)
		{
			return this.Members[member].MemberType;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00016E79 File Offset: 0x00015079
		public static ReflectionObject Create(Type t, params string[] memberNames)
		{
			return ReflectionObject.Create(t, null, memberNames);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00016E84 File Offset: 0x00015084
		public static ReflectionObject Create(Type t, [Nullable(2)] MethodBase creator, params string[] memberNames)
		{
			ReflectionDelegateFactory reflectionDelegateFactory = JsonTypeReflector.ReflectionDelegateFactory;
			ObjectConstructor<object> objectConstructor = null;
			if (creator != null)
			{
				objectConstructor = reflectionDelegateFactory.CreateParameterizedConstructor(creator);
			}
			else if (ReflectionUtils.HasDefaultConstructor(t, false))
			{
				Func<object> ctor = reflectionDelegateFactory.CreateDefaultConstructor<object>(t);
				objectConstructor = ([Nullable(new byte[] { 1, 2 })] object[] args) => ctor();
			}
			ReflectionObject reflectionObject = new ReflectionObject(objectConstructor);
			int i = 0;
			while (i < memberNames.Length)
			{
				string text = memberNames[i];
				MemberInfo[] member = t.GetMember(text, BindingFlags.Instance | BindingFlags.Public);
				if (member.Length != 1)
				{
					throw new ArgumentException("Expected a single member with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
				}
				MemberInfo memberInfo = member.Single<MemberInfo>();
				ReflectionMember reflectionMember = new ReflectionMember();
				MemberTypes memberTypes = memberInfo.MemberType();
				if (memberTypes == MemberTypes.Field)
				{
					goto IL_00AA;
				}
				if (memberTypes != MemberTypes.Method)
				{
					if (memberTypes == MemberTypes.Property)
					{
						goto IL_00AA;
					}
					throw new ArgumentException("Unexpected member type '{0}' for member '{1}'.".FormatWith(CultureInfo.InvariantCulture, memberInfo.MemberType(), memberInfo.Name));
				}
				else
				{
					MethodInfo methodInfo = (MethodInfo)memberInfo;
					if (methodInfo.IsPublic)
					{
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (parameters.Length == 0 && methodInfo.ReturnType != typeof(void))
						{
							MethodCall<object, object> call2 = reflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
							reflectionMember.Getter = (object target) => call2(target, Array.Empty<object>());
						}
						else if (parameters.Length == 1 && methodInfo.ReturnType == typeof(void))
						{
							MethodCall<object, object> call = reflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
							reflectionMember.Setter = delegate(object target, [Nullable(2)] object arg)
							{
								call(target, new object[] { arg });
							};
						}
					}
				}
				IL_01BF:
				reflectionMember.MemberType = ReflectionUtils.GetMemberUnderlyingType(memberInfo);
				reflectionObject.Members[text] = reflectionMember;
				i++;
				continue;
				IL_00AA:
				if (ReflectionUtils.CanReadMemberValue(memberInfo, false))
				{
					reflectionMember.Getter = reflectionDelegateFactory.CreateGet<object>(memberInfo);
				}
				if (ReflectionUtils.CanSetMemberValue(memberInfo, false, false))
				{
					reflectionMember.Setter = reflectionDelegateFactory.CreateSet<object>(memberInfo);
					goto IL_01BF;
				}
				goto IL_01BF;
			}
			return reflectionObject;
		}
	}
}
