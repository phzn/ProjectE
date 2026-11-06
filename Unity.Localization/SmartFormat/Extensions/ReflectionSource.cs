using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x02000075 RID: 117
	[Serializable]
	public class ReflectionSource : ISource
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000BEC9 File Offset: 0x0000A0C9
		[TupleElementNames(new string[] { null, null, "field", "method" })]
		private Dictionary<ValueTuple<Type, string>, ValueTuple<FieldInfo, MethodInfo>> TypeCache
		{
			[return: TupleElementNames(new string[] { null, null, "field", "method" })]
			get
			{
				if (this.m_TypeCache == null)
				{
					this.m_TypeCache = new Dictionary<ValueTuple<Type, string>, ValueTuple<FieldInfo, MethodInfo>>();
				}
				return this.m_TypeCache;
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000BEE4 File Offset: 0x0000A0E4
		public ReflectionSource(SmartFormatter formatter)
		{
			formatter.Parser.AddAlphanumericSelectors();
			formatter.Parser.AddAdditionalSelectorChars("_");
			formatter.Parser.AddOperators(".");
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000BF18 File Offset: 0x0000A118
		public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
		{
			object currentValue = selectorInfo.CurrentValue;
			string selector = selectorInfo.SelectorText;
			if (currentValue == null)
			{
				return false;
			}
			Type type = currentValue.GetType();
			ValueTuple<FieldInfo, MethodInfo> valueTuple;
			if (!this.TypeCache.TryGetValue(new ValueTuple<Type, string>(type, selector), out valueTuple))
			{
				foreach (MemberInfo memberInfo in from m in type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
					where string.Equals(m.Name, selector, selectorInfo.FormatDetails.Settings.GetCaseSensitivityComparison())
					select m)
				{
					MemberTypes memberType = memberInfo.MemberType;
					if (memberType == MemberTypes.Field)
					{
						FieldInfo fieldInfo = (FieldInfo)memberInfo;
						selectorInfo.Result = fieldInfo.GetValue(currentValue);
						this.TypeCache[new ValueTuple<Type, string>(type, selector)] = new ValueTuple<FieldInfo, MethodInfo>(fieldInfo, null);
						return true;
					}
					if (memberType == MemberTypes.Method || memberType == MemberTypes.Property)
					{
						MethodInfo methodInfo;
						if (memberInfo.MemberType == MemberTypes.Property)
						{
							PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
							if (!propertyInfo.CanRead)
							{
								continue;
							}
							methodInfo = propertyInfo.GetGetMethod();
						}
						else
						{
							methodInfo = (MethodInfo)memberInfo;
						}
						if (!(methodInfo == null) && methodInfo.GetParameters().Length == 0 && !(methodInfo.ReturnType == typeof(void)))
						{
							this.TypeCache[new ValueTuple<Type, string>(type, selector)] = new ValueTuple<FieldInfo, MethodInfo>(null, methodInfo);
							selectorInfo.Result = methodInfo.Invoke(currentValue, ReflectionSource.k_Empty);
							return true;
						}
					}
				}
				this.TypeCache[new ValueTuple<Type, string>(type, selector)] = new ValueTuple<FieldInfo, MethodInfo>(null, null);
				return false;
			}
			if (valueTuple.Item1 != null)
			{
				selectorInfo.Result = valueTuple.Item1.GetValue(currentValue);
				return true;
			}
			if (valueTuple.Item2 != null)
			{
				selectorInfo.Result = valueTuple.Item2.Invoke(currentValue, ReflectionSource.k_Empty);
				return true;
			}
			return false;
		}

		// Token: 0x040000C9 RID: 201
		private static readonly object[] k_Empty = new object[0];

		// Token: 0x040000CA RID: 202
		[TupleElementNames(new string[] { null, null, "field", "method" })]
		private Dictionary<ValueTuple<Type, string>, ValueTuple<FieldInfo, MethodInfo>> m_TypeCache;
	}
}
