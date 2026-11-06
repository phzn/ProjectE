using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.UIElements
{
	// Token: 0x020003D6 RID: 982
	internal class UxmlObjectFactoryRegistry
	{
		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x00079AC8 File Offset: 0x00077CC8
		internal static Dictionary<string, List<IBaseUxmlObjectFactory>> factories
		{
			get
			{
				bool flag = UxmlObjectFactoryRegistry.s_Factories == null;
				if (flag)
				{
					UxmlObjectFactoryRegistry.s_Factories = new Dictionary<string, List<IBaseUxmlObjectFactory>>();
					UxmlObjectFactoryRegistry.RegisterEngineFactories();
					UxmlObjectFactoryRegistry.RegisterUserFactories();
				}
				return UxmlObjectFactoryRegistry.s_Factories;
			}
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x00079B04 File Offset: 0x00077D04
		protected static void RegisterFactory(IBaseUxmlObjectFactory factory)
		{
			List<IBaseUxmlObjectFactory> list;
			bool flag = UxmlObjectFactoryRegistry.factories.TryGetValue(factory.uxmlQualifiedName, out list);
			if (flag)
			{
				foreach (IBaseUxmlObjectFactory baseUxmlObjectFactory in list)
				{
					bool flag2 = baseUxmlObjectFactory.GetType() == factory.GetType();
					if (flag2)
					{
						throw new ArgumentException("A factory for the type " + factory.GetType().FullName + " was already registered");
					}
				}
				list.Add(factory);
			}
			else
			{
				list = new List<IBaseUxmlObjectFactory> { factory };
				UxmlObjectFactoryRegistry.factories.Add(factory.uxmlQualifiedName, list);
			}
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x00079BCC File Offset: 0x00077DCC
		internal static bool TryGetFactories(string fullTypeName, out List<IBaseUxmlObjectFactory> factoryList)
		{
			return UxmlObjectFactoryRegistry.factories.TryGetValue(fullTypeName, out factoryList);
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x00079BEC File Offset: 0x00077DEC
		private static void RegisterEngineFactories()
		{
			IBaseUxmlObjectFactory[] array = new IBaseUxmlObjectFactory[]
			{
				new Columns.UxmlObjectFactory<Columns>(),
				new Column.UxmlObjectFactory<Column>(),
				new SortColumnDescriptions.UxmlObjectFactory<SortColumnDescriptions>(),
				new SortColumnDescription.UxmlObjectFactory<SortColumnDescription>()
			};
			foreach (IBaseUxmlObjectFactory baseUxmlObjectFactory in array)
			{
				UxmlObjectFactoryRegistry.RegisterFactory(baseUxmlObjectFactory);
			}
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x00079C40 File Offset: 0x00077E40
		private static void RegisterUserFactories()
		{
			HashSet<string> hashSet = new HashSet<string>(ScriptingRuntime.GetAllUserAssemblies());
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				bool flag = !hashSet.Contains(assembly.GetName().Name + ".dll") || assembly.GetName().Name == "UnityEngine.UIElementsModule";
				if (!flag)
				{
					Type[] types = assembly.GetTypes();
					foreach (Type type in types)
					{
						bool flag2 = !typeof(IBaseUxmlObjectFactory).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract || type.IsGenericType;
						if (!flag2)
						{
							IBaseUxmlObjectFactory baseUxmlObjectFactory = (IBaseUxmlObjectFactory)Activator.CreateInstance(type);
							UxmlObjectFactoryRegistry.RegisterFactory(baseUxmlObjectFactory);
						}
					}
				}
			}
		}

		// Token: 0x04000D46 RID: 3398
		private static Dictionary<string, List<IBaseUxmlObjectFactory>> s_Factories;
	}
}
