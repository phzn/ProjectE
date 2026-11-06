using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000076 RID: 118
	[NullableContext(1)]
	[Nullable(0)]
	public class DefaultSerializationBinder : SerializationBinder, ISerializationBinder
	{
		// Token: 0x06000647 RID: 1607 RVA: 0x0001AB64 File Offset: 0x00018D64
		public DefaultSerializationBinder()
		{
			this._typeCache = new ThreadSafeStore<StructMultiKey<string, string>, Type>(new Func<StructMultiKey<string, string>, Type>(this.GetTypeFromTypeNameKey));
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001AB84 File Offset: 0x00018D84
		private Type GetTypeFromTypeNameKey([Nullable(new byte[] { 0, 2, 1 })] StructMultiKey<string, string> typeNameKey)
		{
			string value = typeNameKey.Value1;
			string value2 = typeNameKey.Value2;
			if (value == null)
			{
				return Type.GetType(value2);
			}
			Assembly assembly = Assembly.LoadWithPartialName(value);
			if (assembly == null)
			{
				foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
				{
					if (assembly2.FullName == value || assembly2.GetName().Name == value)
					{
						assembly = assembly2;
						break;
					}
				}
			}
			if (assembly == null)
			{
				throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, value));
			}
			Type type = assembly.GetType(value2);
			if (type == null)
			{
				if (StringUtils.IndexOf(value2, '`') >= 0)
				{
					try
					{
						type = this.GetGenericTypeFromTypeName(value2, assembly);
					}
					catch (Exception ex)
					{
						throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, value2, assembly.FullName), ex);
					}
				}
				if (type == null)
				{
					throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, value2, assembly.FullName));
				}
			}
			return type;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001ACA8 File Offset: 0x00018EA8
		[return: Nullable(2)]
		private Type GetGenericTypeFromTypeName(string typeName, Assembly assembly)
		{
			Type type = null;
			int num = StringUtils.IndexOf(typeName, '[');
			if (num >= 0)
			{
				string text = typeName.Substring(0, num);
				Type type2 = assembly.GetType(text);
				if (type2 != null)
				{
					List<Type> list = new List<Type>();
					int num2 = 0;
					int num3 = 0;
					int num4 = typeName.Length - 1;
					for (int i = num + 1; i < num4; i++)
					{
						char c = typeName[i];
						if (c != '[')
						{
							if (c == ']')
							{
								num2--;
								if (num2 == 0)
								{
									StructMultiKey<string, string> structMultiKey = ReflectionUtils.SplitFullyQualifiedTypeName(typeName.Substring(num3, i - num3));
									list.Add(this.GetTypeByName(structMultiKey));
								}
							}
						}
						else
						{
							if (num2 == 0)
							{
								num3 = i + 1;
							}
							num2++;
						}
					}
					type = type2.MakeGenericType(list.ToArray());
				}
			}
			return type;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001AD74 File Offset: 0x00018F74
		private Type GetTypeByName([Nullable(new byte[] { 0, 2, 1 })] StructMultiKey<string, string> typeNameKey)
		{
			return this._typeCache.Get(typeNameKey);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001AD82 File Offset: 0x00018F82
		public override Type BindToType([Nullable(2)] string assemblyName, string typeName)
		{
			return this.GetTypeByName(new StructMultiKey<string, string>(assemblyName, typeName));
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001AD91 File Offset: 0x00018F91
		[NullableContext(2)]
		public override void BindToName([Nullable(1)] Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = serializedType.Assembly.FullName;
			typeName = serializedType.FullName;
		}

		// Token: 0x0400023A RID: 570
		internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();

		// Token: 0x0400023B RID: 571
		[Nullable(new byte[] { 1, 0, 2, 1, 1 })]
		private readonly ThreadSafeStore<StructMultiKey<string, string>, Type> _typeCache;
	}
}
