using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Linq.Expressions.Compiler;
using System.Reflection;
using System.Threading;

namespace System.Runtime.CompilerServices
{
	/// <summary>Dynamic site type.</summary>
	/// <typeparam name="T">The delegate type.</typeparam>
	// Token: 0x020002DC RID: 732
	public class CallSite<T> : CallSite where T : class
	{
		/// <summary>The update delegate. Called when the dynamic site experiences cache miss.</summary>
		/// <returns>The update delegate.</returns>
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600163F RID: 5695 RVA: 0x0004AEF9 File Offset: 0x000490F9
		public T Update
		{
			get
			{
				if (this._match)
				{
					return CallSite<T>.s_cachedNoMatch;
				}
				return CallSite<T>.s_cachedUpdate;
			}
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0004AF10 File Offset: 0x00049110
		private CallSite(CallSiteBinder binder)
			: base(binder)
		{
			this.Target = this.GetUpdateDelegate();
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0004AF25 File Offset: 0x00049125
		private CallSite()
			: base(null)
		{
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0004AF2E File Offset: 0x0004912E
		internal CallSite<T> CreateMatchMaker()
		{
			return new CallSite<T>();
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0004AF38 File Offset: 0x00049138
		internal CallSite GetMatchmaker()
		{
			CallSite callSite = this._cachedMatchmaker;
			if (callSite != null)
			{
				callSite = Interlocked.Exchange<CallSite>(ref this._cachedMatchmaker, null);
			}
			CallSite callSite2;
			if ((callSite2 = callSite) == null)
			{
				(callSite2 = new CallSite<T>())._match = true;
			}
			return callSite2;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0004AF6D File Offset: 0x0004916D
		internal void ReleaseMatchmaker(CallSite matchMaker)
		{
			if (this.Rules != null)
			{
				this._cachedMatchmaker = matchMaker;
			}
		}

		/// <summary>Creates an instance of the dynamic call site, initialized with the binder responsible for the runtime binding of the dynamic operations at this call site.</summary>
		/// <param name="binder">The binder responsible for the runtime binding of the dynamic operations at this call site.</param>
		/// <returns>The new instance of dynamic call site.</returns>
		// Token: 0x06001645 RID: 5701 RVA: 0x0004AF7E File Offset: 0x0004917E
		public static CallSite<T> Create(CallSiteBinder binder)
		{
			if (!typeof(T).IsSubclassOf(typeof(MulticastDelegate)))
			{
				throw Error.TypeMustBeDerivedFromSystemDelegate();
			}
			ContractUtils.RequiresNotNull(binder, "binder");
			return new CallSite<T>(binder);
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x0004AFB2 File Offset: 0x000491B2
		private T GetUpdateDelegate()
		{
			return this.GetUpdateDelegate(ref CallSite<T>.s_cachedUpdate);
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0004AFBF File Offset: 0x000491BF
		private T GetUpdateDelegate(ref T addr)
		{
			if (addr == null)
			{
				addr = this.MakeUpdateDelegate();
			}
			return addr;
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0004AFE0 File Offset: 0x000491E0
		private void ClearRuleCache()
		{
			base.Binder.GetRuleCache<T>();
			Dictionary<Type, object> cache = base.Binder.Cache;
			if (cache != null)
			{
				Dictionary<Type, object> dictionary = cache;
				lock (dictionary)
				{
					cache.Clear();
				}
			}
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0004B038 File Offset: 0x00049238
		internal void AddRule(T newRule)
		{
			T[] rules = this.Rules;
			if (rules == null)
			{
				this.Rules = new T[] { newRule };
				return;
			}
			T[] array;
			if (rules.Length < 9)
			{
				array = new T[rules.Length + 1];
				Array.Copy(rules, 0, array, 1, rules.Length);
			}
			else
			{
				array = new T[10];
				Array.Copy(rules, 0, array, 1, 9);
			}
			array[0] = newRule;
			this.Rules = array;
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x0004B0A8 File Offset: 0x000492A8
		internal void MoveRule(int i)
		{
			if (i > 1)
			{
				T[] rules = this.Rules;
				T t = rules[i];
				rules[i] = rules[i - 1];
				rules[i - 1] = rules[i - 2];
				rules[i - 2] = t;
			}
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0004B0F4 File Offset: 0x000492F4
		internal T MakeUpdateDelegate()
		{
			Type typeFromHandle = typeof(T);
			MethodInfo invokeMethod = typeFromHandle.GetInvokeMethod();
			Type[] array;
			if (typeFromHandle.IsGenericType && CallSite<T>.IsSimpleSignature(invokeMethod, out array))
			{
				MethodInfo methodInfo = null;
				MethodInfo methodInfo2 = null;
				if (invokeMethod.ReturnType == typeof(void))
				{
					if (typeFromHandle == DelegateHelpers.GetActionType(array.AddFirst(typeof(CallSite))))
					{
						methodInfo = typeof(UpdateDelegates).GetMethod("UpdateAndExecuteVoid" + array.Length.ToString(), BindingFlags.Static | BindingFlags.NonPublic);
						methodInfo2 = typeof(UpdateDelegates).GetMethod("NoMatchVoid" + array.Length.ToString(), BindingFlags.Static | BindingFlags.NonPublic);
					}
				}
				else if (typeFromHandle == DelegateHelpers.GetFuncType(array.AddFirst(typeof(CallSite))))
				{
					methodInfo = typeof(UpdateDelegates).GetMethod("UpdateAndExecute" + (array.Length - 1).ToString(), BindingFlags.Static | BindingFlags.NonPublic);
					methodInfo2 = typeof(UpdateDelegates).GetMethod("NoMatch" + (array.Length - 1).ToString(), BindingFlags.Static | BindingFlags.NonPublic);
				}
				if (methodInfo != null)
				{
					CallSite<T>.s_cachedNoMatch = (T)((object)methodInfo2.MakeGenericMethod(array).CreateDelegate(typeFromHandle));
					return (T)((object)methodInfo.MakeGenericMethod(array).CreateDelegate(typeFromHandle));
				}
			}
			CallSite<T>.s_cachedNoMatch = this.CreateCustomNoMatchDelegate(invokeMethod);
			return this.CreateCustomUpdateDelegate(invokeMethod);
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x0004B278 File Offset: 0x00049478
		private static bool IsSimpleSignature(MethodInfo invoke, out Type[] sig)
		{
			ParameterInfo[] parametersCached = invoke.GetParametersCached();
			ContractUtils.Requires(parametersCached.Length != 0 && parametersCached[0].ParameterType == typeof(CallSite), "T");
			Type[] array = new Type[(invoke.ReturnType != typeof(void)) ? parametersCached.Length : (parametersCached.Length - 1)];
			bool flag = true;
			for (int i = 1; i < parametersCached.Length; i++)
			{
				ParameterInfo parameterInfo = parametersCached[i];
				if (parameterInfo.IsByRefParameter())
				{
					flag = false;
				}
				array[i - 1] = parameterInfo.ParameterType;
			}
			if (invoke.ReturnType != typeof(void))
			{
				array[array.Length - 1] = invoke.ReturnType;
			}
			sig = array;
			return flag;
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x0004B330 File Offset: 0x00049530
		private T CreateCustomUpdateDelegate(MethodInfo invoke)
		{
			Type returnType = invoke.GetReturnType();
			bool flag = returnType == typeof(void);
			global::System.Collections.Generic.ArrayBuilder<Expression> arrayBuilder = new global::System.Collections.Generic.ArrayBuilder<Expression>(13);
			global::System.Collections.Generic.ArrayBuilder<ParameterExpression> arrayBuilder2 = new global::System.Collections.Generic.ArrayBuilder<ParameterExpression>(8 + (flag ? 0 : 1));
			ParameterExpression[] array = Array.ConvertAll<ParameterInfo, ParameterExpression>(invoke.GetParametersCached(), (ParameterInfo p) => Expression.Parameter(p.ParameterType, p.Name));
			LabelTarget labelTarget = Expression.Label(returnType);
			Type[] array2 = new Type[] { typeof(T) };
			ParameterExpression parameterExpression = array[0];
			ParameterExpression[] array3 = array.RemoveFirst<ParameterExpression>();
			ParameterExpression parameterExpression2 = Expression.Variable(typeof(CallSite<T>), "this");
			arrayBuilder2.UncheckedAdd(parameterExpression2);
			arrayBuilder.UncheckedAdd(Expression.Assign(parameterExpression2, Expression.Convert(parameterExpression, parameterExpression2.Type)));
			ParameterExpression parameterExpression3 = Expression.Variable(typeof(T[]), "applicable");
			arrayBuilder2.UncheckedAdd(parameterExpression3);
			ParameterExpression parameterExpression4 = Expression.Variable(typeof(T), "rule");
			arrayBuilder2.UncheckedAdd(parameterExpression4);
			ParameterExpression parameterExpression5 = Expression.Variable(typeof(T), "originalRule");
			arrayBuilder2.UncheckedAdd(parameterExpression5);
			Expression expression = Expression.Field(parameterExpression2, "Target");
			arrayBuilder.UncheckedAdd(Expression.Assign(parameterExpression5, expression));
			ParameterExpression parameterExpression6 = null;
			if (!flag)
			{
				arrayBuilder2.UncheckedAdd(parameterExpression6 = Expression.Variable(labelTarget.Type, "result"));
			}
			ParameterExpression parameterExpression7 = Expression.Variable(typeof(int), "count");
			arrayBuilder2.UncheckedAdd(parameterExpression7);
			ParameterExpression parameterExpression8 = Expression.Variable(typeof(int), "index");
			arrayBuilder2.UncheckedAdd(parameterExpression8);
			arrayBuilder.UncheckedAdd(Expression.Assign(parameterExpression, Expression.Call(CachedReflectionInfo.CallSiteOps_CreateMatchmaker.MakeGenericMethod(array2), parameterExpression2)));
			Expression expression2 = Expression.Call(CachedReflectionInfo.CallSiteOps_GetMatch, parameterExpression);
			Expression expression3 = Expression.Call(CachedReflectionInfo.CallSiteOps_ClearMatch, parameterExpression);
			Expression expression4 = parameterExpression4;
			Expression[] array4 = array;
			Expression expression5 = Expression.Invoke(expression4, new TrueReadOnlyCollection<Expression>(array4));
			Expression expression6 = Expression.Call(CachedReflectionInfo.CallSiteOps_UpdateRules.MakeGenericMethod(array2), parameterExpression2, parameterExpression8);
			Expression expression7;
			if (flag)
			{
				expression7 = Expression.Block(expression5, Expression.IfThen(expression2, Expression.Block(expression6, Expression.Return(labelTarget))));
			}
			else
			{
				expression7 = Expression.Block(Expression.Assign(parameterExpression6, expression5), Expression.IfThen(expression2, Expression.Block(expression6, Expression.Return(labelTarget, parameterExpression6))));
			}
			Expression expression8 = Expression.Assign(parameterExpression4, Expression.ArrayAccess(parameterExpression3, new TrueReadOnlyCollection<Expression>(new Expression[] { parameterExpression8 })));
			Expression expression9 = expression8;
			LabelTarget labelTarget2 = Expression.Label();
			Expression expression10 = Expression.IfThen(Expression.Equal(parameterExpression8, parameterExpression7), Expression.Break(labelTarget2));
			Expression expression11 = Expression.PreIncrementAssign(parameterExpression8);
			arrayBuilder.UncheckedAdd(Expression.IfThen(Expression.NotEqual(Expression.Assign(parameterExpression3, Expression.Call(CachedReflectionInfo.CallSiteOps_GetRules.MakeGenericMethod(array2), parameterExpression2)), Expression.Constant(null, parameterExpression3.Type)), Expression.Block(Expression.Assign(parameterExpression7, Expression.ArrayLength(parameterExpression3)), Expression.Assign(parameterExpression8, Utils.Constant(0)), Expression.Loop(Expression.Block(expression10, expression9, Expression.IfThen(Expression.NotEqual(Expression.Convert(parameterExpression4, typeof(object)), Expression.Convert(parameterExpression5, typeof(object))), Expression.Block(Expression.Assign(expression, parameterExpression4), expression7, expression3)), expression11), labelTarget2, null))));
			ParameterExpression parameterExpression9 = Expression.Variable(typeof(RuleCache<T>), "cache");
			arrayBuilder2.UncheckedAdd(parameterExpression9);
			arrayBuilder.UncheckedAdd(Expression.Assign(parameterExpression9, Expression.Call(CachedReflectionInfo.CallSiteOps_GetRuleCache.MakeGenericMethod(array2), parameterExpression2)));
			arrayBuilder.UncheckedAdd(Expression.Assign(parameterExpression3, Expression.Call(CachedReflectionInfo.CallSiteOps_GetCachedRules.MakeGenericMethod(array2), parameterExpression9)));
			if (flag)
			{
				expression7 = Expression.Block(expression5, Expression.IfThen(expression2, Expression.Return(labelTarget)));
			}
			else
			{
				expression7 = Expression.Block(Expression.Assign(parameterExpression6, expression5), Expression.IfThen(expression2, Expression.Return(labelTarget, parameterExpression6)));
			}
			Expression expression12 = Expression.TryFinally(expression7, Expression.IfThen(expression2, Expression.Block(Expression.Call(CachedReflectionInfo.CallSiteOps_AddRule.MakeGenericMethod(array2), parameterExpression2, parameterExpression4), Expression.Call(CachedReflectionInfo.CallSiteOps_MoveRule.MakeGenericMethod(array2), parameterExpression9, parameterExpression4, parameterExpression8))));
			expression9 = Expression.Assign(expression, expression8);
			arrayBuilder.UncheckedAdd(Expression.Assign(parameterExpression8, Utils.Constant(0)));
			arrayBuilder.UncheckedAdd(Expression.Assign(parameterExpression7, Expression.ArrayLength(parameterExpression3)));
			arrayBuilder.UncheckedAdd(Expression.Loop(Expression.Block(expression10, expression9, expression12, expression3, expression11), labelTarget2, null));
			arrayBuilder.UncheckedAdd(Expression.Assign(parameterExpression4, Expression.Constant(null, parameterExpression4.Type)));
			ParameterExpression parameterExpression10 = Expression.Variable(typeof(object[]), "args");
			Expression[] array5 = Array.ConvertAll<ParameterExpression, Expression>(array3, (ParameterExpression p) => CallSite<T>.Convert(p, typeof(object)));
			arrayBuilder2.UncheckedAdd(parameterExpression10);
			arrayBuilder.UncheckedAdd(Expression.Assign(parameterExpression10, Expression.NewArrayInit(typeof(object), new TrueReadOnlyCollection<Expression>(array5))));
			Expression expression13 = Expression.Assign(expression, parameterExpression5);
			expression9 = Expression.Assign(expression, Expression.Assign(parameterExpression4, Expression.Call(CachedReflectionInfo.CallSiteOps_Bind.MakeGenericMethod(array2), Expression.Property(parameterExpression2, "Binder"), parameterExpression2, parameterExpression10)));
			expression12 = Expression.TryFinally(expression7, Expression.IfThen(expression2, Expression.Call(CachedReflectionInfo.CallSiteOps_AddRule.MakeGenericMethod(array2), parameterExpression2, parameterExpression4)));
			arrayBuilder.UncheckedAdd(Expression.Loop(Expression.Block(expression13, expression9, expression12, expression3), null, null));
			arrayBuilder.UncheckedAdd(Expression.Default(labelTarget.Type));
			return Expression.Lambda<T>(Expression.Label(labelTarget, Expression.Block(arrayBuilder2.ToReadOnly<ParameterExpression>(), arrayBuilder.ToReadOnly<Expression>())), "CallSite.Target", true, new TrueReadOnlyCollection<ParameterExpression>(array)).Compile();
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x0004B8F0 File Offset: 0x00049AF0
		private T CreateCustomNoMatchDelegate(MethodInfo invoke)
		{
			ParameterExpression[] array = Array.ConvertAll<ParameterInfo, ParameterExpression>(invoke.GetParametersCached(), (ParameterInfo p) => Expression.Parameter(p.ParameterType, p.Name));
			return Expression.Lambda<T>(Expression.Block(Expression.Call(typeof(CallSiteOps).GetMethod("SetNotMatched"), array[0]), Expression.Default(invoke.GetReturnType())), new TrueReadOnlyCollection<ParameterExpression>(array)).Compile();
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0004B964 File Offset: 0x00049B64
		private static Expression Convert(Expression arg, Type type)
		{
			if (TypeUtils.AreReferenceAssignable(type, arg.Type))
			{
				return arg;
			}
			return Expression.Convert(arg, type);
		}

		/// <summary>The Level 0 cache - a delegate specialized based on the site history.</summary>
		// Token: 0x04000B44 RID: 2884
		public T Target;

		// Token: 0x04000B45 RID: 2885
		internal T[] Rules;

		// Token: 0x04000B46 RID: 2886
		internal CallSite _cachedMatchmaker;

		// Token: 0x04000B47 RID: 2887
		private static T s_cachedUpdate;

		// Token: 0x04000B48 RID: 2888
		private static volatile T s_cachedNoMatch;

		// Token: 0x04000B49 RID: 2889
		private const int MaxRules = 10;
	}
}
