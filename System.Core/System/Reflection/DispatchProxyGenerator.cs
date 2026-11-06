using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x02000066 RID: 102
	internal static class DispatchProxyGenerator
	{
		// Token: 0x0600022A RID: 554 RVA: 0x00006288 File Offset: 0x00004488
		internal static object CreateProxyInstance(Type baseType, Type interfaceType)
		{
			return Activator.CreateInstance(DispatchProxyGenerator.GetProxyType(baseType, interfaceType), new object[]
			{
				new Action<object[]>(DispatchProxyGenerator.Invoke)
			});
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000062AC File Offset: 0x000044AC
		private static Type GetProxyType(Type baseType, Type interfaceType)
		{
			Dictionary<Type, Dictionary<Type, Type>> dictionary = DispatchProxyGenerator.s_baseTypeAndInterfaceToGeneratedProxyType;
			Type type2;
			lock (dictionary)
			{
				Dictionary<Type, Type> dictionary2 = null;
				if (!DispatchProxyGenerator.s_baseTypeAndInterfaceToGeneratedProxyType.TryGetValue(baseType, out dictionary2))
				{
					dictionary2 = new Dictionary<Type, Type>();
					DispatchProxyGenerator.s_baseTypeAndInterfaceToGeneratedProxyType[baseType] = dictionary2;
				}
				Type type = null;
				if (!dictionary2.TryGetValue(interfaceType, out type))
				{
					type = DispatchProxyGenerator.GenerateProxyType(baseType, interfaceType);
					dictionary2[interfaceType] = type;
				}
				type2 = type;
			}
			return type2;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000632C File Offset: 0x0000452C
		private static Type GenerateProxyType(Type baseType, Type interfaceType)
		{
			TypeInfo typeInfo = baseType.GetTypeInfo();
			if (!interfaceType.GetTypeInfo().IsInterface)
			{
				throw new ArgumentException(global::SR.Format("The type '{0}' must be an interface, not a class.", interfaceType.FullName), "T");
			}
			if (typeInfo.IsSealed)
			{
				throw new ArgumentException(global::SR.Format("The base type '{0}' cannot be sealed.", typeInfo.FullName), "TProxy");
			}
			if (typeInfo.IsAbstract)
			{
				throw new ArgumentException(global::SR.Format("The base type '{0}' cannot be abstract.", baseType.FullName), "TProxy");
			}
			if (!typeInfo.DeclaredConstructors.Any<ConstructorInfo>((ConstructorInfo c) => c.IsPublic && c.GetParameters().Length == 0))
			{
				throw new ArgumentException(global::SR.Format("The base type '{0}' must have a public parameterless constructor.", baseType.FullName), "TProxy");
			}
			DispatchProxyGenerator.ProxyBuilder proxyBuilder = DispatchProxyGenerator.s_proxyAssembly.CreateProxy("generatedProxy", baseType);
			foreach (Type type in interfaceType.GetTypeInfo().ImplementedInterfaces)
			{
				proxyBuilder.AddInterfaceImpl(type);
			}
			proxyBuilder.AddInterfaceImpl(interfaceType);
			return proxyBuilder.CreateType();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00006458 File Offset: 0x00004658
		private static void Invoke(object[] args)
		{
			DispatchProxyGenerator.PackedArgs packedArgs = new DispatchProxyGenerator.PackedArgs(args);
			MethodBase methodBase = DispatchProxyGenerator.s_proxyAssembly.ResolveMethodToken(packedArgs.DeclaringType, packedArgs.MethodToken);
			if (methodBase.IsGenericMethodDefinition)
			{
				methodBase = ((MethodInfo)methodBase).MakeGenericMethod(packedArgs.GenericTypes);
			}
			try
			{
				object obj = DispatchProxyGenerator.s_dispatchProxyInvokeMethod.Invoke(packedArgs.DispatchProxy, new object[] { methodBase, packedArgs.Args });
				packedArgs.ReturnValue = obj;
			}
			catch (TargetInvocationException ex)
			{
				ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
			}
		}

		// Token: 0x04000371 RID: 881
		private const int InvokeActionFieldAndCtorParameterIndex = 0;

		// Token: 0x04000372 RID: 882
		private static readonly Dictionary<Type, Dictionary<Type, Type>> s_baseTypeAndInterfaceToGeneratedProxyType = new Dictionary<Type, Dictionary<Type, Type>>();

		// Token: 0x04000373 RID: 883
		private static readonly DispatchProxyGenerator.ProxyAssembly s_proxyAssembly = new DispatchProxyGenerator.ProxyAssembly();

		// Token: 0x04000374 RID: 884
		private static readonly MethodInfo s_dispatchProxyInvokeMethod = typeof(DispatchProxy).GetTypeInfo().GetDeclaredMethod("Invoke");

		// Token: 0x02000067 RID: 103
		private class PackedArgs
		{
			// Token: 0x0600022F RID: 559 RVA: 0x00006520 File Offset: 0x00004720
			internal PackedArgs()
				: this(new object[DispatchProxyGenerator.PackedArgs.PackedTypes.Length])
			{
			}

			// Token: 0x06000230 RID: 560 RVA: 0x00006534 File Offset: 0x00004734
			internal PackedArgs(object[] args)
			{
				this._args = args;
			}

			// Token: 0x17000074 RID: 116
			// (get) Token: 0x06000231 RID: 561 RVA: 0x00006543 File Offset: 0x00004743
			internal DispatchProxy DispatchProxy
			{
				get
				{
					return (DispatchProxy)this._args[0];
				}
			}

			// Token: 0x17000075 RID: 117
			// (get) Token: 0x06000232 RID: 562 RVA: 0x00006552 File Offset: 0x00004752
			internal Type DeclaringType
			{
				get
				{
					return (Type)this._args[1];
				}
			}

			// Token: 0x17000076 RID: 118
			// (get) Token: 0x06000233 RID: 563 RVA: 0x00006561 File Offset: 0x00004761
			internal int MethodToken
			{
				get
				{
					return (int)this._args[2];
				}
			}

			// Token: 0x17000077 RID: 119
			// (get) Token: 0x06000234 RID: 564 RVA: 0x00006570 File Offset: 0x00004770
			internal object[] Args
			{
				get
				{
					return (object[])this._args[3];
				}
			}

			// Token: 0x17000078 RID: 120
			// (get) Token: 0x06000235 RID: 565 RVA: 0x0000657F File Offset: 0x0000477F
			internal Type[] GenericTypes
			{
				get
				{
					return (Type[])this._args[4];
				}
			}

			// Token: 0x17000079 RID: 121
			// (set) Token: 0x06000236 RID: 566 RVA: 0x0000658E File Offset: 0x0000478E
			internal object ReturnValue
			{
				set
				{
					this._args[5] = value;
				}
			}

			// Token: 0x04000375 RID: 885
			internal const int DispatchProxyPosition = 0;

			// Token: 0x04000376 RID: 886
			internal const int DeclaringTypePosition = 1;

			// Token: 0x04000377 RID: 887
			internal const int MethodTokenPosition = 2;

			// Token: 0x04000378 RID: 888
			internal const int ArgsPosition = 3;

			// Token: 0x04000379 RID: 889
			internal const int GenericTypesPosition = 4;

			// Token: 0x0400037A RID: 890
			internal const int ReturnValuePosition = 5;

			// Token: 0x0400037B RID: 891
			internal static readonly Type[] PackedTypes = new Type[]
			{
				typeof(object),
				typeof(Type),
				typeof(int),
				typeof(object[]),
				typeof(Type[]),
				typeof(object)
			};

			// Token: 0x0400037C RID: 892
			private object[] _args;
		}

		// Token: 0x02000068 RID: 104
		private class ProxyAssembly
		{
			// Token: 0x06000238 RID: 568 RVA: 0x00006604 File Offset: 0x00004804
			public ProxyAssembly()
			{
				this._ab = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("ProxyBuilder"), AssemblyBuilderAccess.Run);
				this._mb = this._ab.DefineDynamicModule("testmod");
			}

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x06000239 RID: 569 RVA: 0x00006664 File Offset: 0x00004864
			internal ConstructorInfo IgnoresAccessChecksAttributeConstructor
			{
				get
				{
					if (this._ignoresAccessChecksToAttributeConstructor == null)
					{
						TypeInfo typeInfo = this.GenerateTypeInfoOfIgnoresAccessChecksToAttribute();
						this._ignoresAccessChecksToAttributeConstructor = typeInfo.DeclaredConstructors.Single<ConstructorInfo>();
					}
					return this._ignoresAccessChecksToAttributeConstructor;
				}
			}

			// Token: 0x0600023A RID: 570 RVA: 0x000066A0 File Offset: 0x000048A0
			public DispatchProxyGenerator.ProxyBuilder CreateProxy(string name, Type proxyBaseType)
			{
				int num = Interlocked.Increment(ref this._typeId);
				TypeBuilder typeBuilder = this._mb.DefineType(name + "_" + num.ToString(), TypeAttributes.Public, proxyBaseType);
				return new DispatchProxyGenerator.ProxyBuilder(this, typeBuilder, proxyBaseType);
			}

			// Token: 0x0600023B RID: 571 RVA: 0x000066E4 File Offset: 0x000048E4
			private TypeInfo GenerateTypeInfoOfIgnoresAccessChecksToAttribute()
			{
				TypeBuilder typeBuilder = this._mb.DefineType("System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute", TypeAttributes.Public, typeof(Attribute));
				FieldBuilder fieldBuilder = typeBuilder.DefineField("assemblyName", typeof(string), FieldAttributes.Private);
				ILGenerator ilgenerator = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[] { fieldBuilder.FieldType }).GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldarg, 1);
				ilgenerator.Emit(OpCodes.Stfld, fieldBuilder);
				ilgenerator.Emit(OpCodes.Ret);
				typeBuilder.DefineProperty("AssemblyName", PropertyAttributes.None, CallingConventions.HasThis, typeof(string), null);
				ILGenerator ilgenerator2 = typeBuilder.DefineMethod("get_AssemblyName", MethodAttributes.Public, CallingConventions.HasThis, typeof(string), null).GetILGenerator();
				ilgenerator2.Emit(OpCodes.Ldarg_0);
				ilgenerator2.Emit(OpCodes.Ldfld, fieldBuilder);
				ilgenerator2.Emit(OpCodes.Ret);
				TypeInfo typeInfo = typeof(AttributeUsageAttribute).GetTypeInfo();
				ConstructorInfo constructorInfo = typeInfo.DeclaredConstructors.Single<ConstructorInfo>((ConstructorInfo c) => c.GetParameters().Count<ParameterInfo>() == 1 && c.GetParameters()[0].ParameterType == typeof(AttributeTargets));
				PropertyInfo propertyInfo = typeInfo.DeclaredProperties.Single<PropertyInfo>((PropertyInfo f) => string.Equals(f.Name, "AllowMultiple"));
				CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(constructorInfo, new object[] { AttributeTargets.Assembly }, new PropertyInfo[] { propertyInfo }, new object[] { true });
				typeBuilder.SetCustomAttribute(customAttributeBuilder);
				return typeBuilder.CreateTypeInfo();
			}

			// Token: 0x0600023C RID: 572 RVA: 0x0000686C File Offset: 0x00004A6C
			internal void GenerateInstanceOfIgnoresAccessChecksToAttribute(string assemblyName)
			{
				CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(this.IgnoresAccessChecksAttributeConstructor, new object[] { assemblyName });
				this._ab.SetCustomAttribute(customAttributeBuilder);
			}

			// Token: 0x0600023D RID: 573 RVA: 0x0000689C File Offset: 0x00004A9C
			internal void EnsureTypeIsVisible(Type type)
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				if (!typeInfo.IsVisible)
				{
					string name = typeInfo.Assembly.GetName().Name;
					if (!this._ignoresAccessAssemblyNames.Contains(name))
					{
						this.GenerateInstanceOfIgnoresAccessChecksToAttribute(name);
						this._ignoresAccessAssemblyNames.Add(name);
					}
				}
			}

			// Token: 0x0600023E RID: 574 RVA: 0x000068EC File Offset: 0x00004AEC
			internal void GetTokenForMethod(MethodBase method, out Type type, out int token)
			{
				type = method.DeclaringType;
				token = 0;
				if (!this._methodToToken.TryGetValue(method, out token))
				{
					this._methodsByToken.Add(method);
					token = this._methodsByToken.Count - 1;
					this._methodToToken[method] = token;
				}
			}

			// Token: 0x0600023F RID: 575 RVA: 0x0000693C File Offset: 0x00004B3C
			internal MethodBase ResolveMethodToken(Type type, int token)
			{
				return this._methodsByToken[token];
			}

			// Token: 0x0400037D RID: 893
			private AssemblyBuilder _ab;

			// Token: 0x0400037E RID: 894
			private ModuleBuilder _mb;

			// Token: 0x0400037F RID: 895
			private int _typeId;

			// Token: 0x04000380 RID: 896
			private Dictionary<MethodBase, int> _methodToToken = new Dictionary<MethodBase, int>();

			// Token: 0x04000381 RID: 897
			private List<MethodBase> _methodsByToken = new List<MethodBase>();

			// Token: 0x04000382 RID: 898
			private HashSet<string> _ignoresAccessAssemblyNames = new HashSet<string>();

			// Token: 0x04000383 RID: 899
			private ConstructorInfo _ignoresAccessChecksToAttributeConstructor;
		}

		// Token: 0x0200006A RID: 106
		private class ProxyBuilder
		{
			// Token: 0x06000244 RID: 580 RVA: 0x00006998 File Offset: 0x00004B98
			internal ProxyBuilder(DispatchProxyGenerator.ProxyAssembly assembly, TypeBuilder tb, Type proxyBaseType)
			{
				this._assembly = assembly;
				this._tb = tb;
				this._proxyBaseType = proxyBaseType;
				this._fields = new List<FieldBuilder>();
				this._fields.Add(tb.DefineField("invoke", typeof(Action<object[]>), FieldAttributes.Private));
			}

			// Token: 0x06000245 RID: 581 RVA: 0x000069EC File Offset: 0x00004BEC
			private void Complete()
			{
				Type[] array = new Type[this._fields.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._fields[i].FieldType;
				}
				ILGenerator ilgenerator = this._tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, array).GetILGenerator();
				ConstructorInfo constructorInfo = this._proxyBaseType.GetTypeInfo().DeclaredConstructors.SingleOrDefault<ConstructorInfo>((ConstructorInfo c) => c.IsPublic && c.GetParameters().Length == 0);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Call, constructorInfo);
				for (int j = 0; j < array.Length; j++)
				{
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ilgenerator.Emit(OpCodes.Ldarg, j + 1);
					ilgenerator.Emit(OpCodes.Stfld, this._fields[j]);
				}
				ilgenerator.Emit(OpCodes.Ret);
			}

			// Token: 0x06000246 RID: 582 RVA: 0x00006ADC File Offset: 0x00004CDC
			internal Type CreateType()
			{
				this.Complete();
				return this._tb.CreateTypeInfo().AsType();
			}

			// Token: 0x06000247 RID: 583 RVA: 0x00006AF4 File Offset: 0x00004CF4
			internal void AddInterfaceImpl(Type iface)
			{
				this._assembly.EnsureTypeIsVisible(iface);
				this._tb.AddInterfaceImplementation(iface);
				Dictionary<MethodInfo, DispatchProxyGenerator.ProxyBuilder.PropertyAccessorInfo> dictionary = new Dictionary<MethodInfo, DispatchProxyGenerator.ProxyBuilder.PropertyAccessorInfo>(DispatchProxyGenerator.ProxyBuilder.MethodInfoEqualityComparer.Instance);
				foreach (PropertyInfo propertyInfo in iface.GetRuntimeProperties())
				{
					DispatchProxyGenerator.ProxyBuilder.PropertyAccessorInfo propertyAccessorInfo = new DispatchProxyGenerator.ProxyBuilder.PropertyAccessorInfo(propertyInfo.GetMethod, propertyInfo.SetMethod);
					if (propertyInfo.GetMethod != null)
					{
						dictionary[propertyInfo.GetMethod] = propertyAccessorInfo;
					}
					if (propertyInfo.SetMethod != null)
					{
						dictionary[propertyInfo.SetMethod] = propertyAccessorInfo;
					}
				}
				Dictionary<MethodInfo, DispatchProxyGenerator.ProxyBuilder.EventAccessorInfo> dictionary2 = new Dictionary<MethodInfo, DispatchProxyGenerator.ProxyBuilder.EventAccessorInfo>(DispatchProxyGenerator.ProxyBuilder.MethodInfoEqualityComparer.Instance);
				foreach (EventInfo eventInfo in iface.GetRuntimeEvents())
				{
					DispatchProxyGenerator.ProxyBuilder.EventAccessorInfo eventAccessorInfo = new DispatchProxyGenerator.ProxyBuilder.EventAccessorInfo(eventInfo.AddMethod, eventInfo.RemoveMethod, eventInfo.RaiseMethod);
					if (eventInfo.AddMethod != null)
					{
						dictionary2[eventInfo.AddMethod] = eventAccessorInfo;
					}
					if (eventInfo.RemoveMethod != null)
					{
						dictionary2[eventInfo.RemoveMethod] = eventAccessorInfo;
					}
					if (eventInfo.RaiseMethod != null)
					{
						dictionary2[eventInfo.RaiseMethod] = eventAccessorInfo;
					}
				}
				foreach (MethodInfo methodInfo in iface.GetRuntimeMethods())
				{
					MethodBuilder methodBuilder = this.AddMethodImpl(methodInfo);
					DispatchProxyGenerator.ProxyBuilder.PropertyAccessorInfo propertyAccessorInfo2;
					if (dictionary.TryGetValue(methodInfo, out propertyAccessorInfo2))
					{
						if (DispatchProxyGenerator.ProxyBuilder.MethodInfoEqualityComparer.Instance.Equals(propertyAccessorInfo2.InterfaceGetMethod, methodInfo))
						{
							propertyAccessorInfo2.GetMethodBuilder = methodBuilder;
						}
						else
						{
							propertyAccessorInfo2.SetMethodBuilder = methodBuilder;
						}
					}
					DispatchProxyGenerator.ProxyBuilder.EventAccessorInfo eventAccessorInfo2;
					if (dictionary2.TryGetValue(methodInfo, out eventAccessorInfo2))
					{
						if (DispatchProxyGenerator.ProxyBuilder.MethodInfoEqualityComparer.Instance.Equals(eventAccessorInfo2.InterfaceAddMethod, methodInfo))
						{
							eventAccessorInfo2.AddMethodBuilder = methodBuilder;
						}
						else if (DispatchProxyGenerator.ProxyBuilder.MethodInfoEqualityComparer.Instance.Equals(eventAccessorInfo2.InterfaceRemoveMethod, methodInfo))
						{
							eventAccessorInfo2.RemoveMethodBuilder = methodBuilder;
						}
						else
						{
							eventAccessorInfo2.RaiseMethodBuilder = methodBuilder;
						}
					}
				}
				foreach (PropertyInfo propertyInfo2 in iface.GetRuntimeProperties())
				{
					DispatchProxyGenerator.ProxyBuilder.PropertyAccessorInfo propertyAccessorInfo3 = dictionary[propertyInfo2.GetMethod ?? propertyInfo2.SetMethod];
					PropertyBuilder propertyBuilder = this._tb.DefineProperty(propertyInfo2.Name, propertyInfo2.Attributes, propertyInfo2.PropertyType, (from p in propertyInfo2.GetIndexParameters()
						select p.ParameterType).ToArray<Type>());
					if (propertyAccessorInfo3.GetMethodBuilder != null)
					{
						propertyBuilder.SetGetMethod(propertyAccessorInfo3.GetMethodBuilder);
					}
					if (propertyAccessorInfo3.SetMethodBuilder != null)
					{
						propertyBuilder.SetSetMethod(propertyAccessorInfo3.SetMethodBuilder);
					}
				}
				foreach (EventInfo eventInfo2 in iface.GetRuntimeEvents())
				{
					DispatchProxyGenerator.ProxyBuilder.EventAccessorInfo eventAccessorInfo3 = dictionary2[eventInfo2.AddMethod ?? eventInfo2.RemoveMethod];
					EventBuilder eventBuilder = this._tb.DefineEvent(eventInfo2.Name, eventInfo2.Attributes, eventInfo2.EventHandlerType);
					if (eventAccessorInfo3.AddMethodBuilder != null)
					{
						eventBuilder.SetAddOnMethod(eventAccessorInfo3.AddMethodBuilder);
					}
					if (eventAccessorInfo3.RemoveMethodBuilder != null)
					{
						eventBuilder.SetRemoveOnMethod(eventAccessorInfo3.RemoveMethodBuilder);
					}
					if (eventAccessorInfo3.RaiseMethodBuilder != null)
					{
						eventBuilder.SetRaiseMethod(eventAccessorInfo3.RaiseMethodBuilder);
					}
				}
			}

			// Token: 0x06000248 RID: 584 RVA: 0x00006EF4 File Offset: 0x000050F4
			private MethodBuilder AddMethodImpl(MethodInfo mi)
			{
				ParameterInfo[] parameters = mi.GetParameters();
				Type[] array = DispatchProxyGenerator.ProxyBuilder.ParamTypes(parameters, false);
				MethodBuilder methodBuilder = this._tb.DefineMethod(mi.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, mi.ReturnType, array);
				if (mi.ContainsGenericParameters)
				{
					Type[] genericArguments = mi.GetGenericArguments();
					string[] array2 = new string[genericArguments.Length];
					for (int i = 0; i < genericArguments.Length; i++)
					{
						array2[i] = genericArguments[i].Name;
					}
					GenericTypeParameterBuilder[] array3 = methodBuilder.DefineGenericParameters(array2);
					for (int j = 0; j < array3.Length; j++)
					{
						array3[j].SetGenericParameterAttributes(genericArguments[j].GetTypeInfo().GenericParameterAttributes);
					}
				}
				ILGenerator ilgenerator = methodBuilder.GetILGenerator();
				DispatchProxyGenerator.ProxyBuilder.ParametersArray parametersArray = new DispatchProxyGenerator.ProxyBuilder.ParametersArray(ilgenerator, array);
				ilgenerator.Emit(OpCodes.Nop);
				DispatchProxyGenerator.ProxyBuilder.GenericArray<object> genericArray = new DispatchProxyGenerator.ProxyBuilder.GenericArray<object>(ilgenerator, DispatchProxyGenerator.ProxyBuilder.ParamTypes(parameters, true).Length);
				for (int k = 0; k < parameters.Length; k++)
				{
					if (!parameters[k].IsOut)
					{
						genericArray.BeginSet(k);
						parametersArray.Get(k);
						genericArray.EndSet(parameters[k].ParameterType);
					}
				}
				DispatchProxyGenerator.ProxyBuilder.GenericArray<object> genericArray2 = new DispatchProxyGenerator.ProxyBuilder.GenericArray<object>(ilgenerator, DispatchProxyGenerator.PackedArgs.PackedTypes.Length);
				genericArray2.BeginSet(0);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				genericArray2.EndSet(typeof(DispatchProxy));
				MethodInfo runtimeMethod = typeof(Type).GetRuntimeMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) });
				Type type;
				int num;
				this._assembly.GetTokenForMethod(mi, out type, out num);
				genericArray2.BeginSet(1);
				ilgenerator.Emit(OpCodes.Ldtoken, type);
				ilgenerator.Emit(OpCodes.Call, runtimeMethod);
				genericArray2.EndSet(typeof(object));
				genericArray2.BeginSet(2);
				ilgenerator.Emit(OpCodes.Ldc_I4, num);
				genericArray2.EndSet(typeof(int));
				genericArray2.BeginSet(3);
				genericArray.Load();
				genericArray2.EndSet(typeof(object[]));
				if (mi.ContainsGenericParameters)
				{
					genericArray2.BeginSet(4);
					Type[] genericArguments2 = mi.GetGenericArguments();
					DispatchProxyGenerator.ProxyBuilder.GenericArray<Type> genericArray3 = new DispatchProxyGenerator.ProxyBuilder.GenericArray<Type>(ilgenerator, genericArguments2.Length);
					for (int l = 0; l < genericArguments2.Length; l++)
					{
						genericArray3.BeginSet(l);
						ilgenerator.Emit(OpCodes.Ldtoken, genericArguments2[l]);
						ilgenerator.Emit(OpCodes.Call, runtimeMethod);
						genericArray3.EndSet(typeof(Type));
					}
					genericArray3.Load();
					genericArray2.EndSet(typeof(Type[]));
				}
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, this._fields[0]);
				genericArray2.Load();
				ilgenerator.Emit(OpCodes.Call, DispatchProxyGenerator.ProxyBuilder.s_delegateInvoke);
				for (int m = 0; m < parameters.Length; m++)
				{
					if (parameters[m].ParameterType.IsByRef)
					{
						parametersArray.BeginSet(m);
						genericArray.Get(m);
						parametersArray.EndSet(m, typeof(object));
					}
				}
				if (mi.ReturnType != typeof(void))
				{
					genericArray2.Get(5);
					DispatchProxyGenerator.ProxyBuilder.Convert(ilgenerator, typeof(object), mi.ReturnType, false);
				}
				ilgenerator.Emit(OpCodes.Ret);
				this._tb.DefineMethodOverride(methodBuilder, mi);
				return methodBuilder;
			}

			// Token: 0x06000249 RID: 585 RVA: 0x00007244 File Offset: 0x00005444
			private static Type[] ParamTypes(ParameterInfo[] parms, bool noByRef)
			{
				Type[] array = new Type[parms.Length];
				for (int i = 0; i < parms.Length; i++)
				{
					array[i] = parms[i].ParameterType;
					if (noByRef && array[i].IsByRef)
					{
						array[i] = array[i].GetElementType();
					}
				}
				return array;
			}

			// Token: 0x0600024A RID: 586 RVA: 0x0000728C File Offset: 0x0000548C
			private static int GetTypeCode(Type type)
			{
				if (type == null)
				{
					return 0;
				}
				if (type == typeof(bool))
				{
					return 3;
				}
				if (type == typeof(char))
				{
					return 4;
				}
				if (type == typeof(sbyte))
				{
					return 5;
				}
				if (type == typeof(byte))
				{
					return 6;
				}
				if (type == typeof(short))
				{
					return 7;
				}
				if (type == typeof(ushort))
				{
					return 8;
				}
				if (type == typeof(int))
				{
					return 9;
				}
				if (type == typeof(uint))
				{
					return 10;
				}
				if (type == typeof(long))
				{
					return 11;
				}
				if (type == typeof(ulong))
				{
					return 12;
				}
				if (type == typeof(float))
				{
					return 13;
				}
				if (type == typeof(double))
				{
					return 14;
				}
				if (type == typeof(decimal))
				{
					return 15;
				}
				if (type == typeof(DateTime))
				{
					return 16;
				}
				if (type == typeof(string))
				{
					return 18;
				}
				if (type.GetTypeInfo().IsEnum)
				{
					return DispatchProxyGenerator.ProxyBuilder.GetTypeCode(Enum.GetUnderlyingType(type));
				}
				return 1;
			}

			// Token: 0x0600024B RID: 587 RVA: 0x000073F4 File Offset: 0x000055F4
			private static void Convert(ILGenerator il, Type source, Type target, bool isAddress)
			{
				if (target == source)
				{
					return;
				}
				TypeInfo typeInfo = source.GetTypeInfo();
				TypeInfo typeInfo2 = target.GetTypeInfo();
				if (source.IsByRef)
				{
					Type elementType = source.GetElementType();
					DispatchProxyGenerator.ProxyBuilder.Ldind(il, elementType);
					DispatchProxyGenerator.ProxyBuilder.Convert(il, elementType, target, isAddress);
					return;
				}
				if (typeInfo2.IsValueType)
				{
					if (typeInfo.IsValueType)
					{
						OpCode opCode = DispatchProxyGenerator.ProxyBuilder.s_convOpCodes[DispatchProxyGenerator.ProxyBuilder.GetTypeCode(target)];
						il.Emit(opCode);
						return;
					}
					il.Emit(OpCodes.Unbox, target);
					if (!isAddress)
					{
						DispatchProxyGenerator.ProxyBuilder.Ldind(il, target);
						return;
					}
				}
				else if (typeInfo2.IsAssignableFrom(typeInfo))
				{
					if (typeInfo.IsValueType || source.IsGenericParameter)
					{
						if (isAddress)
						{
							DispatchProxyGenerator.ProxyBuilder.Ldind(il, source);
						}
						il.Emit(OpCodes.Box, source);
						return;
					}
				}
				else
				{
					if (target.IsGenericParameter)
					{
						il.Emit(OpCodes.Unbox_Any, target);
						return;
					}
					il.Emit(OpCodes.Castclass, target);
				}
			}

			// Token: 0x0600024C RID: 588 RVA: 0x000074CC File Offset: 0x000056CC
			private static void Ldind(ILGenerator il, Type type)
			{
				OpCode opCode = DispatchProxyGenerator.ProxyBuilder.s_ldindOpCodes[DispatchProxyGenerator.ProxyBuilder.GetTypeCode(type)];
				if (!opCode.Equals(OpCodes.Nop))
				{
					il.Emit(opCode);
					return;
				}
				il.Emit(OpCodes.Ldobj, type);
			}

			// Token: 0x0600024D RID: 589 RVA: 0x0000750C File Offset: 0x0000570C
			private static void Stind(ILGenerator il, Type type)
			{
				OpCode opCode = DispatchProxyGenerator.ProxyBuilder.s_stindOpCodes[DispatchProxyGenerator.ProxyBuilder.GetTypeCode(type)];
				if (!opCode.Equals(OpCodes.Nop))
				{
					il.Emit(opCode);
					return;
				}
				il.Emit(OpCodes.Stobj, type);
			}

			// Token: 0x04000387 RID: 903
			private static readonly MethodInfo s_delegateInvoke = typeof(Action<object[]>).GetTypeInfo().GetDeclaredMethod("Invoke");

			// Token: 0x04000388 RID: 904
			private DispatchProxyGenerator.ProxyAssembly _assembly;

			// Token: 0x04000389 RID: 905
			private TypeBuilder _tb;

			// Token: 0x0400038A RID: 906
			private Type _proxyBaseType;

			// Token: 0x0400038B RID: 907
			private List<FieldBuilder> _fields;

			// Token: 0x0400038C RID: 908
			private static OpCode[] s_convOpCodes = new OpCode[]
			{
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Conv_I1,
				OpCodes.Conv_I2,
				OpCodes.Conv_I1,
				OpCodes.Conv_U1,
				OpCodes.Conv_I2,
				OpCodes.Conv_U2,
				OpCodes.Conv_I4,
				OpCodes.Conv_U4,
				OpCodes.Conv_I8,
				OpCodes.Conv_U8,
				OpCodes.Conv_R4,
				OpCodes.Conv_R8,
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Nop
			};

			// Token: 0x0400038D RID: 909
			private static OpCode[] s_ldindOpCodes = new OpCode[]
			{
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Ldind_I1,
				OpCodes.Ldind_I2,
				OpCodes.Ldind_I1,
				OpCodes.Ldind_U1,
				OpCodes.Ldind_I2,
				OpCodes.Ldind_U2,
				OpCodes.Ldind_I4,
				OpCodes.Ldind_U4,
				OpCodes.Ldind_I8,
				OpCodes.Ldind_I8,
				OpCodes.Ldind_R4,
				OpCodes.Ldind_R8,
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Ldind_Ref
			};

			// Token: 0x0400038E RID: 910
			private static OpCode[] s_stindOpCodes = new OpCode[]
			{
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Stind_I1,
				OpCodes.Stind_I2,
				OpCodes.Stind_I1,
				OpCodes.Stind_I1,
				OpCodes.Stind_I2,
				OpCodes.Stind_I2,
				OpCodes.Stind_I4,
				OpCodes.Stind_I4,
				OpCodes.Stind_I8,
				OpCodes.Stind_I8,
				OpCodes.Stind_R4,
				OpCodes.Stind_R8,
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Nop,
				OpCodes.Stind_Ref
			};

			// Token: 0x0200006B RID: 107
			private class ParametersArray
			{
				// Token: 0x0600024F RID: 591 RVA: 0x00007865 File Offset: 0x00005A65
				internal ParametersArray(ILGenerator il, Type[] paramTypes)
				{
					this._il = il;
					this._paramTypes = paramTypes;
				}

				// Token: 0x06000250 RID: 592 RVA: 0x0000787B File Offset: 0x00005A7B
				internal void Get(int i)
				{
					this._il.Emit(OpCodes.Ldarg, i + 1);
				}

				// Token: 0x06000251 RID: 593 RVA: 0x0000787B File Offset: 0x00005A7B
				internal void BeginSet(int i)
				{
					this._il.Emit(OpCodes.Ldarg, i + 1);
				}

				// Token: 0x06000252 RID: 594 RVA: 0x00007890 File Offset: 0x00005A90
				internal void EndSet(int i, Type stackType)
				{
					Type elementType = this._paramTypes[i].GetElementType();
					DispatchProxyGenerator.ProxyBuilder.Convert(this._il, stackType, elementType, false);
					DispatchProxyGenerator.ProxyBuilder.Stind(this._il, elementType);
				}

				// Token: 0x0400038F RID: 911
				private ILGenerator _il;

				// Token: 0x04000390 RID: 912
				private Type[] _paramTypes;
			}

			// Token: 0x0200006C RID: 108
			private class GenericArray<T>
			{
				// Token: 0x06000253 RID: 595 RVA: 0x000078C8 File Offset: 0x00005AC8
				internal GenericArray(ILGenerator il, int len)
				{
					this._il = il;
					this._lb = il.DeclareLocal(typeof(T[]));
					il.Emit(OpCodes.Ldc_I4, len);
					il.Emit(OpCodes.Newarr, typeof(T));
					il.Emit(OpCodes.Stloc, this._lb);
				}

				// Token: 0x06000254 RID: 596 RVA: 0x0000792A File Offset: 0x00005B2A
				internal void Load()
				{
					this._il.Emit(OpCodes.Ldloc, this._lb);
				}

				// Token: 0x06000255 RID: 597 RVA: 0x00007942 File Offset: 0x00005B42
				internal void Get(int i)
				{
					this._il.Emit(OpCodes.Ldloc, this._lb);
					this._il.Emit(OpCodes.Ldc_I4, i);
					this._il.Emit(OpCodes.Ldelem_Ref);
				}

				// Token: 0x06000256 RID: 598 RVA: 0x0000797B File Offset: 0x00005B7B
				internal void BeginSet(int i)
				{
					this._il.Emit(OpCodes.Ldloc, this._lb);
					this._il.Emit(OpCodes.Ldc_I4, i);
				}

				// Token: 0x06000257 RID: 599 RVA: 0x000079A4 File Offset: 0x00005BA4
				internal void EndSet(Type stackType)
				{
					DispatchProxyGenerator.ProxyBuilder.Convert(this._il, stackType, typeof(T), false);
					this._il.Emit(OpCodes.Stelem_Ref);
				}

				// Token: 0x04000391 RID: 913
				private ILGenerator _il;

				// Token: 0x04000392 RID: 914
				private LocalBuilder _lb;
			}

			// Token: 0x0200006D RID: 109
			private sealed class PropertyAccessorInfo
			{
				// Token: 0x1700007B RID: 123
				// (get) Token: 0x06000258 RID: 600 RVA: 0x000079CD File Offset: 0x00005BCD
				public MethodInfo InterfaceGetMethod { get; }

				// Token: 0x1700007C RID: 124
				// (get) Token: 0x06000259 RID: 601 RVA: 0x000079D5 File Offset: 0x00005BD5
				public MethodInfo InterfaceSetMethod { get; }

				// Token: 0x1700007D RID: 125
				// (get) Token: 0x0600025A RID: 602 RVA: 0x000079DD File Offset: 0x00005BDD
				// (set) Token: 0x0600025B RID: 603 RVA: 0x000079E5 File Offset: 0x00005BE5
				public MethodBuilder GetMethodBuilder { get; set; }

				// Token: 0x1700007E RID: 126
				// (get) Token: 0x0600025C RID: 604 RVA: 0x000079EE File Offset: 0x00005BEE
				// (set) Token: 0x0600025D RID: 605 RVA: 0x000079F6 File Offset: 0x00005BF6
				public MethodBuilder SetMethodBuilder { get; set; }

				// Token: 0x0600025E RID: 606 RVA: 0x000079FF File Offset: 0x00005BFF
				public PropertyAccessorInfo(MethodInfo interfaceGetMethod, MethodInfo interfaceSetMethod)
				{
					this.InterfaceGetMethod = interfaceGetMethod;
					this.InterfaceSetMethod = interfaceSetMethod;
				}
			}

			// Token: 0x0200006E RID: 110
			private sealed class EventAccessorInfo
			{
				// Token: 0x1700007F RID: 127
				// (get) Token: 0x0600025F RID: 607 RVA: 0x00007A15 File Offset: 0x00005C15
				public MethodInfo InterfaceAddMethod { get; }

				// Token: 0x17000080 RID: 128
				// (get) Token: 0x06000260 RID: 608 RVA: 0x00007A1D File Offset: 0x00005C1D
				public MethodInfo InterfaceRemoveMethod { get; }

				// Token: 0x17000081 RID: 129
				// (get) Token: 0x06000261 RID: 609 RVA: 0x00007A25 File Offset: 0x00005C25
				public MethodInfo InterfaceRaiseMethod { get; }

				// Token: 0x17000082 RID: 130
				// (get) Token: 0x06000262 RID: 610 RVA: 0x00007A2D File Offset: 0x00005C2D
				// (set) Token: 0x06000263 RID: 611 RVA: 0x00007A35 File Offset: 0x00005C35
				public MethodBuilder AddMethodBuilder { get; set; }

				// Token: 0x17000083 RID: 131
				// (get) Token: 0x06000264 RID: 612 RVA: 0x00007A3E File Offset: 0x00005C3E
				// (set) Token: 0x06000265 RID: 613 RVA: 0x00007A46 File Offset: 0x00005C46
				public MethodBuilder RemoveMethodBuilder { get; set; }

				// Token: 0x17000084 RID: 132
				// (get) Token: 0x06000266 RID: 614 RVA: 0x00007A4F File Offset: 0x00005C4F
				// (set) Token: 0x06000267 RID: 615 RVA: 0x00007A57 File Offset: 0x00005C57
				public MethodBuilder RaiseMethodBuilder { get; set; }

				// Token: 0x06000268 RID: 616 RVA: 0x00007A60 File Offset: 0x00005C60
				public EventAccessorInfo(MethodInfo interfaceAddMethod, MethodInfo interfaceRemoveMethod, MethodInfo interfaceRaiseMethod)
				{
					this.InterfaceAddMethod = interfaceAddMethod;
					this.InterfaceRemoveMethod = interfaceRemoveMethod;
					this.InterfaceRaiseMethod = interfaceRaiseMethod;
				}
			}

			// Token: 0x0200006F RID: 111
			private sealed class MethodInfoEqualityComparer : EqualityComparer<MethodInfo>
			{
				// Token: 0x06000269 RID: 617 RVA: 0x00007A7D File Offset: 0x00005C7D
				private MethodInfoEqualityComparer()
				{
				}

				// Token: 0x0600026A RID: 618 RVA: 0x00007A88 File Offset: 0x00005C88
				public sealed override bool Equals(MethodInfo left, MethodInfo right)
				{
					if (left == right)
					{
						return true;
					}
					if (left == null)
					{
						return right == null;
					}
					if (right == null)
					{
						return false;
					}
					if (!object.Equals(left.DeclaringType, right.DeclaringType))
					{
						return false;
					}
					if (!object.Equals(left.ReturnType, right.ReturnType))
					{
						return false;
					}
					if (left.CallingConvention != right.CallingConvention)
					{
						return false;
					}
					if (left.IsStatic != right.IsStatic)
					{
						return false;
					}
					if (left.Name != right.Name)
					{
						return false;
					}
					Type[] genericArguments = left.GetGenericArguments();
					Type[] genericArguments2 = right.GetGenericArguments();
					if (genericArguments.Length != genericArguments2.Length)
					{
						return false;
					}
					for (int i = 0; i < genericArguments.Length; i++)
					{
						if (!object.Equals(genericArguments[i], genericArguments2[i]))
						{
							return false;
						}
					}
					ParameterInfo[] parameters = left.GetParameters();
					ParameterInfo[] parameters2 = right.GetParameters();
					if (parameters.Length != parameters2.Length)
					{
						return false;
					}
					for (int j = 0; j < parameters.Length; j++)
					{
						if (!object.Equals(parameters[j].ParameterType, parameters2[j].ParameterType))
						{
							return false;
						}
					}
					return true;
				}

				// Token: 0x0600026B RID: 619 RVA: 0x00007B98 File Offset: 0x00005D98
				public sealed override int GetHashCode(MethodInfo obj)
				{
					if (obj == null)
					{
						return 0;
					}
					int num = obj.DeclaringType.GetHashCode();
					num ^= obj.Name.GetHashCode();
					foreach (ParameterInfo parameterInfo in obj.GetParameters())
					{
						num ^= parameterInfo.ParameterType.GetHashCode();
					}
					return num;
				}

				// Token: 0x0400039D RID: 925
				public static readonly DispatchProxyGenerator.ProxyBuilder.MethodInfoEqualityComparer Instance = new DispatchProxyGenerator.ProxyBuilder.MethodInfoEqualityComparer();
			}
		}
	}
}
