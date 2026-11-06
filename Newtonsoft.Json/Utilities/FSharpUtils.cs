using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005A RID: 90
	[NullableContext(1)]
	[Nullable(0)]
	internal class FSharpUtils
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x00015648 File Offset: 0x00013848
		private FSharpUtils(Assembly fsharpCoreAssembly)
		{
			this.FSharpCoreAssembly = fsharpCoreAssembly;
			Type type = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, "IsUnion", BindingFlags.Static | BindingFlags.Public);
			this.IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodInfo methodWithNonPublicFallback2 = FSharpUtils.GetMethodWithNonPublicFallback(type, "GetUnionCases", BindingFlags.Static | BindingFlags.Public);
			this.GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback2);
			Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
			this.PreComputeUnionTagReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionTagReader");
			this.PreComputeUnionReader = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionReader");
			this.PreComputeUnionConstructor = FSharpUtils.CreateFSharpFuncCall(type2, "PreComputeUnionConstructor");
			Type type3 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
			this.GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Name"));
			this.GetUnionCaseInfoTag = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Tag"));
			this.GetUnionCaseInfoDeclaringType = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("DeclaringType"));
			this.GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(type3.GetMethod("GetFields"));
			Type type4 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule");
			this._ofSeq = type4.GetMethod("OfSeq");
			this._mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00015791 File Offset: 0x00013991
		public static FSharpUtils Instance
		{
			get
			{
				return FSharpUtils._instance;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00015798 File Offset: 0x00013998
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x000157A0 File Offset: 0x000139A0
		public Assembly FSharpCoreAssembly { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x000157A9 File Offset: 0x000139A9
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x000157B1 File Offset: 0x000139B1
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> IsUnion
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x000157BA File Offset: 0x000139BA
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x000157C2 File Offset: 0x000139C2
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> GetUnionCases
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x000157CB File Offset: 0x000139CB
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x000157D3 File Offset: 0x000139D3
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> PreComputeUnionTagReader
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x000157DC File Offset: 0x000139DC
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x000157E4 File Offset: 0x000139E4
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> PreComputeUnionReader
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x000157ED File Offset: 0x000139ED
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x000157F5 File Offset: 0x000139F5
		[Nullable(new byte[] { 1, 2, 1 })]
		public MethodCall<object, object> PreComputeUnionConstructor
		{
			[return: Nullable(new byte[] { 1, 2, 1 })]
			get;
			[param: Nullable(new byte[] { 1, 2, 1 })]
			private set;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x000157FE File Offset: 0x000139FE
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x00015806 File Offset: 0x00013A06
		public Func<object, object> GetUnionCaseInfoDeclaringType { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001580F File Offset: 0x00013A0F
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00015817 File Offset: 0x00013A17
		public Func<object, object> GetUnionCaseInfoName { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00015820 File Offset: 0x00013A20
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00015828 File Offset: 0x00013A28
		public Func<object, object> GetUnionCaseInfoTag { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00015831 File Offset: 0x00013A31
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00015839 File Offset: 0x00013A39
		[Nullable(new byte[] { 1, 1, 2 })]
		public MethodCall<object, object> GetUnionCaseInfoFields
		{
			[return: Nullable(new byte[] { 1, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 1, 1, 2 })]
			private set;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00015844 File Offset: 0x00013A44
		public static void EnsureInitialized(Assembly fsharpCoreAssembly)
		{
			if (FSharpUtils._instance == null)
			{
				object @lock = FSharpUtils.Lock;
				lock (@lock)
				{
					if (FSharpUtils._instance == null)
					{
						FSharpUtils._instance = new FSharpUtils(fsharpCoreAssembly);
					}
				}
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00015898 File Offset: 0x00013A98
		private static MethodInfo GetMethodWithNonPublicFallback(Type type, string methodName, BindingFlags bindingFlags)
		{
			MethodInfo methodInfo = type.GetMethod(methodName, bindingFlags);
			if (methodInfo == null && (bindingFlags & BindingFlags.NonPublic) != BindingFlags.NonPublic)
			{
				methodInfo = type.GetMethod(methodName, bindingFlags | BindingFlags.NonPublic);
			}
			return methodInfo;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000158CC File Offset: 0x00013ACC
		[return: Nullable(new byte[] { 1, 2, 1 })]
		private static MethodCall<object, object> CreateFSharpFuncCall(Type type, string methodName)
		{
			MethodInfo methodWithNonPublicFallback = FSharpUtils.GetMethodWithNonPublicFallback(type, methodName, BindingFlags.Static | BindingFlags.Public);
			MethodInfo method = methodWithNonPublicFallback.ReturnType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
			MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodWithNonPublicFallback);
			MethodCall<object, object> invoke = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return ([Nullable(2)] object target, [Nullable(new byte[] { 1, 2 })] object[] args) => new FSharpFunction(call(target, args), invoke);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00015928 File Offset: 0x00013B28
		public ObjectConstructor<object> CreateSeq(Type t)
		{
			MethodInfo methodInfo = this._ofSeq.MakeGenericMethod(new Type[] { t });
			return JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(methodInfo);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00015956 File Offset: 0x00013B56
		public ObjectConstructor<object> CreateMap(Type keyType, Type valueType)
		{
			return (ObjectConstructor<object>)typeof(FSharpUtils).GetMethod("BuildMapCreator").MakeGenericMethod(new Type[] { keyType, valueType }).Invoke(this, null);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001598C File Offset: 0x00013B8C
		[NullableContext(2)]
		[return: Nullable(1)]
		public ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
		{
			ConstructorInfo constructor = this._mapType.MakeGenericType(new Type[]
			{
				typeof(TKey),
				typeof(TValue)
			}).GetConstructor(new Type[] { typeof(IEnumerable<Tuple<TKey, TValue>>) });
			ObjectConstructor<object> ctorDelegate = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			return delegate([Nullable(new byte[] { 1, 2 })] object[] args)
			{
				IEnumerable<Tuple<TKey, TValue>> enumerable = ((IEnumerable<KeyValuePair<TKey, TValue>>)args[0]).Select<KeyValuePair<TKey, TValue>, Tuple<TKey, TValue>>((KeyValuePair<TKey, TValue> kv) => new Tuple<TKey, TValue>(kv.Key, kv.Value));
				return ctorDelegate(new object[] { enumerable });
			};
		}

		// Token: 0x040001E7 RID: 487
		private static readonly object Lock = new object();

		// Token: 0x040001E8 RID: 488
		[Nullable(2)]
		private static FSharpUtils _instance;

		// Token: 0x040001E9 RID: 489
		private MethodInfo _ofSeq;

		// Token: 0x040001EA RID: 490
		private Type _mapType;

		// Token: 0x040001F5 RID: 501
		public const string FSharpSetTypeName = "FSharpSet`1";

		// Token: 0x040001F6 RID: 502
		public const string FSharpListTypeName = "FSharpList`1";

		// Token: 0x040001F7 RID: 503
		public const string FSharpMapTypeName = "FSharpMap`2";
	}
}
