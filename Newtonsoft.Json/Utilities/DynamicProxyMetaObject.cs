using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000052 RID: 82
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DynamicProxyMetaObject<[Nullable(2)] T> : DynamicMetaObject
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x0001445E File Offset: 0x0001265E
		internal DynamicProxyMetaObject(Expression expression, T value, DynamicProxy<T> proxy)
			: base(expression, BindingRestrictions.Empty, value)
		{
			this._proxy = proxy;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00014479 File Offset: 0x00012679
		private bool IsOverridden(string method)
		{
			return ReflectionUtils.IsMethodOverridden(this._proxy.GetType(), typeof(DynamicProxy<T>), method);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00014498 File Offset: 0x00012698
		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			if (!this.IsOverridden("TryGetMember"))
			{
				return base.BindGetMember(binder);
			}
			return this.CallMethodWithResult("TryGetMember", binder, DynamicProxyMetaObject<T>.NoArgs, ([Nullable(2)] DynamicMetaObject e) => binder.FallbackGetMember(this, e), null);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000144F8 File Offset: 0x000126F8
		public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
		{
			if (!this.IsOverridden("TrySetMember"))
			{
				return base.BindSetMember(binder, value);
			}
			return this.CallMethodReturnLast("TrySetMember", binder, DynamicProxyMetaObject<T>.GetArgs(new DynamicMetaObject[] { value }), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackSetMember(this, value, e));
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00014574 File Offset: 0x00012774
		public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
		{
			if (!this.IsOverridden("TryDeleteMember"))
			{
				return base.BindDeleteMember(binder);
			}
			return this.CallMethodNoResult("TryDeleteMember", binder, DynamicProxyMetaObject<T>.NoArgs, ([Nullable(2)] DynamicMetaObject e) => binder.FallbackDeleteMember(this, e));
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000145D4 File Offset: 0x000127D4
		public override DynamicMetaObject BindConvert(ConvertBinder binder)
		{
			if (!this.IsOverridden("TryConvert"))
			{
				return base.BindConvert(binder);
			}
			return this.CallMethodWithResult("TryConvert", binder, DynamicProxyMetaObject<T>.NoArgs, ([Nullable(2)] DynamicMetaObject e) => binder.FallbackConvert(this, e), null);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00014634 File Offset: 0x00012834
		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryInvokeMember"))
			{
				return base.BindInvokeMember(binder, args);
			}
			DynamicProxyMetaObject<T>.Fallback fallback = ([Nullable(2)] DynamicMetaObject e) => binder.FallbackInvokeMember(this, args, e);
			return this.BuildCallMethodWithResult("TryInvokeMember", binder, DynamicProxyMetaObject<T>.GetArgArray(args), this.BuildCallMethodWithResult("TryGetMember", new DynamicProxyMetaObject<T>.GetBinderAdapter(binder), DynamicProxyMetaObject<T>.NoArgs, fallback(null), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackInvoke(e, args, null)), null);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000146D4 File Offset: 0x000128D4
		public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryCreateInstance"))
			{
				return base.BindCreateInstance(binder, args);
			}
			return this.CallMethodWithResult("TryCreateInstance", binder, DynamicProxyMetaObject<T>.GetArgArray(args), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackCreateInstance(this, args, e), null);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00014748 File Offset: 0x00012948
		public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryInvoke"))
			{
				return base.BindInvoke(binder, args);
			}
			return this.CallMethodWithResult("TryInvoke", binder, DynamicProxyMetaObject<T>.GetArgArray(args), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackInvoke(this, args, e), null);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000147BC File Offset: 0x000129BC
		public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
		{
			if (!this.IsOverridden("TryBinaryOperation"))
			{
				return base.BindBinaryOperation(binder, arg);
			}
			return this.CallMethodWithResult("TryBinaryOperation", binder, DynamicProxyMetaObject<T>.GetArgs(new DynamicMetaObject[] { arg }), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackBinaryOperation(this, arg, e), null);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00014838 File Offset: 0x00012A38
		public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
		{
			if (!this.IsOverridden("TryUnaryOperation"))
			{
				return base.BindUnaryOperation(binder);
			}
			return this.CallMethodWithResult("TryUnaryOperation", binder, DynamicProxyMetaObject<T>.NoArgs, ([Nullable(2)] DynamicMetaObject e) => binder.FallbackUnaryOperation(this, e), null);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00014898 File Offset: 0x00012A98
		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!this.IsOverridden("TryGetIndex"))
			{
				return base.BindGetIndex(binder, indexes);
			}
			return this.CallMethodWithResult("TryGetIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackGetIndex(this, indexes, e), null);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001490C File Offset: 0x00012B0C
		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			if (!this.IsOverridden("TrySetIndex"))
			{
				return base.BindSetIndex(binder, indexes, value);
			}
			return this.CallMethodReturnLast("TrySetIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes, value), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackSetIndex(this, indexes, value, e));
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00014990 File Offset: 0x00012B90
		public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!this.IsOverridden("TryDeleteIndex"))
			{
				return base.BindDeleteIndex(binder, indexes);
			}
			return this.CallMethodNoResult("TryDeleteIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), ([Nullable(2)] DynamicMetaObject e) => binder.FallbackDeleteIndex(this, indexes, e));
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00014A00 File Offset: 0x00012C00
		private static Expression[] NoArgs
		{
			get
			{
				return CollectionUtils.ArrayEmpty<Expression>();
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00014A07 File Offset: 0x00012C07
		private static IEnumerable<Expression> GetArgs(params DynamicMetaObject[] args)
		{
			return args.Select<DynamicMetaObject, Expression>(delegate(DynamicMetaObject arg)
			{
				Expression expression = arg.Expression;
				if (!expression.Type.IsValueType())
				{
					return expression;
				}
				return Expression.Convert(expression, typeof(object));
			});
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00014A30 File Offset: 0x00012C30
		private static Expression[] GetArgArray(DynamicMetaObject[] args)
		{
			return new NewArrayExpression[] { Expression.NewArrayInit(typeof(object), DynamicProxyMetaObject<T>.GetArgs(args)) };
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00014A60 File Offset: 0x00012C60
		private static Expression[] GetArgArray(DynamicMetaObject[] args, DynamicMetaObject value)
		{
			Expression expression = value.Expression;
			return new Expression[]
			{
				Expression.NewArrayInit(typeof(object), DynamicProxyMetaObject<T>.GetArgs(args)),
				expression.Type.IsValueType() ? Expression.Convert(expression, typeof(object)) : expression
			};
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00014AB8 File Offset: 0x00012CB8
		private static ConstantExpression Constant(DynamicMetaObjectBinder binder)
		{
			Type type = binder.GetType();
			while (!type.IsVisible())
			{
				type = type.BaseType();
			}
			return Expression.Constant(binder, type);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00014AE4 File Offset: 0x00012CE4
		private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, IEnumerable<Expression> args, [Nullable(new byte[] { 1, 0 })] DynamicProxyMetaObject<T>.Fallback fallback, [Nullable(new byte[] { 2, 0 })] DynamicProxyMetaObject<T>.Fallback fallbackInvoke = null)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			return this.BuildCallMethodWithResult(methodName, binder, args, dynamicMetaObject, fallbackInvoke);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00014B08 File Offset: 0x00012D08
		private DynamicMetaObject BuildCallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, IEnumerable<Expression> args, DynamicMetaObject fallbackResult, [Nullable(new byte[] { 2, 0 })] DynamicProxyMetaObject<T>.Fallback fallbackInvoke)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(DynamicProxyMetaObject<T>.Constant(binder));
			list.AddRange<Expression>(args);
			list.Add(parameterExpression);
			DynamicMetaObject dynamicMetaObject = new DynamicMetaObject(parameterExpression, BindingRestrictions.Empty);
			if (binder.ReturnType != typeof(object))
			{
				dynamicMetaObject = new DynamicMetaObject(Expression.Convert(dynamicMetaObject.Expression, binder.ReturnType), dynamicMetaObject.Restrictions);
			}
			if (fallbackInvoke != null)
			{
				dynamicMetaObject = fallbackInvoke(dynamicMetaObject);
			}
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[] { parameterExpression }, new Expression[] { Expression.Condition(Expression.Call(Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), dynamicMetaObject.Expression, fallbackResult.Expression, binder.ReturnType) }), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions).Merge(fallbackResult.Restrictions));
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00014C24 File Offset: 0x00012E24
		private DynamicMetaObject CallMethodReturnLast(string methodName, DynamicMetaObjectBinder binder, IEnumerable<Expression> args, [Nullable(new byte[] { 1, 0 })] DynamicProxyMetaObject<T>.Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(DynamicProxyMetaObject<T>.Constant(binder));
			list.AddRange<Expression>(args);
			list[list.Count - 1] = Expression.Assign(parameterExpression, list[list.Count - 1]);
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[] { parameterExpression }, new Expression[] { Expression.Condition(Expression.Call(Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), parameterExpression, dynamicMetaObject.Expression, typeof(object)) }), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00014D08 File Offset: 0x00012F08
		private DynamicMetaObject CallMethodNoResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, [Nullable(new byte[] { 1, 0 })] DynamicProxyMetaObject<T>.Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(DynamicProxyMetaObject<T>.Constant(binder));
			list.AddRange<Expression>(args);
			return new DynamicMetaObject(Expression.Condition(Expression.Call(Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), Expression.Empty(), dynamicMetaObject.Expression, typeof(void)), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00014DA3 File Offset: 0x00012FA3
		private BindingRestrictions GetRestrictions()
		{
			if (base.Value != null || !base.HasValue)
			{
				return BindingRestrictions.GetTypeRestriction(base.Expression, base.LimitType);
			}
			return BindingRestrictions.GetInstanceRestriction(base.Expression, null);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00014DD3 File Offset: 0x00012FD3
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this._proxy.GetDynamicMemberNames((T)((object)base.Value));
		}

		// Token: 0x040001D9 RID: 473
		private readonly DynamicProxy<T> _proxy;

		// Token: 0x0200016B RID: 363
		// (Invoke) Token: 0x06000E97 RID: 3735
		[NullableContext(0)]
		private delegate DynamicMetaObject Fallback([Nullable(2)] DynamicMetaObject errorSuggestion);

		// Token: 0x0200016C RID: 364
		[Nullable(0)]
		private sealed class GetBinderAdapter : GetMemberBinder
		{
			// Token: 0x06000E9A RID: 3738 RVA: 0x00040D00 File Offset: 0x0003EF00
			internal GetBinderAdapter(InvokeMemberBinder binder)
				: base(binder.Name, binder.IgnoreCase)
			{
			}

			// Token: 0x06000E9B RID: 3739 RVA: 0x00040D14 File Offset: 0x0003EF14
			public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, [Nullable(2)] DynamicMetaObject errorSuggestion)
			{
				throw new NotSupportedException();
			}
		}
	}
}
