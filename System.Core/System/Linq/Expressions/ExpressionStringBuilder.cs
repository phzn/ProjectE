using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Linq.Expressions
{
	// Token: 0x02000258 RID: 600
	internal sealed class ExpressionStringBuilder : ExpressionVisitor
	{
		// Token: 0x06001151 RID: 4433 RVA: 0x00038DBA File Offset: 0x00036FBA
		private ExpressionStringBuilder()
		{
			this._out = new StringBuilder();
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00038DCD File Offset: 0x00036FCD
		public override string ToString()
		{
			return this._out.ToString();
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00038DDA File Offset: 0x00036FDA
		private int GetLabelId(LabelTarget label)
		{
			return this.GetId(label);
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00038DDA File Offset: 0x00036FDA
		private int GetParamId(ParameterExpression p)
		{
			return this.GetId(p);
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00038DE4 File Offset: 0x00036FE4
		private int GetId(object o)
		{
			if (this._ids == null)
			{
				this._ids = new Dictionary<object, int>();
			}
			int count;
			if (!this._ids.TryGetValue(o, out count))
			{
				count = this._ids.Count;
				this._ids.Add(o, count);
			}
			return count;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00038E2E File Offset: 0x0003702E
		private void Out(string s)
		{
			this._out.Append(s);
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00038E3D File Offset: 0x0003703D
		private void Out(char c)
		{
			this._out.Append(c);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00038E4C File Offset: 0x0003704C
		internal static string ExpressionToString(Expression node)
		{
			ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
			expressionStringBuilder.Visit(node);
			return expressionStringBuilder.ToString();
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00038E60 File Offset: 0x00037060
		internal static string CatchBlockToString(CatchBlock node)
		{
			ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
			expressionStringBuilder.VisitCatchBlock(node);
			return expressionStringBuilder.ToString();
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00038E74 File Offset: 0x00037074
		internal static string SwitchCaseToString(SwitchCase node)
		{
			ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
			expressionStringBuilder.VisitSwitchCase(node);
			return expressionStringBuilder.ToString();
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00038E88 File Offset: 0x00037088
		internal static string MemberBindingToString(MemberBinding node)
		{
			ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
			expressionStringBuilder.VisitMemberBinding(node);
			return expressionStringBuilder.ToString();
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00038E9C File Offset: 0x0003709C
		internal static string ElementInitBindingToString(ElementInit node)
		{
			ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
			expressionStringBuilder.VisitElementInit(node);
			return expressionStringBuilder.ToString();
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00038EB0 File Offset: 0x000370B0
		private void VisitExpressions<T>(char open, ReadOnlyCollection<T> expressions, char close) where T : Expression
		{
			this.VisitExpressions<T>(open, expressions, close, ", ");
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00038EC0 File Offset: 0x000370C0
		private void VisitExpressions<T>(char open, ReadOnlyCollection<T> expressions, char close, string seperator) where T : Expression
		{
			this.Out(open);
			if (expressions != null)
			{
				bool flag = true;
				foreach (T t in expressions)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						this.Out(seperator);
					}
					this.Visit(t);
				}
			}
			this.Out(close);
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00038F30 File Offset: 0x00037130
		protected internal override Expression VisitBinary(BinaryExpression node)
		{
			if (node.NodeType == ExpressionType.ArrayIndex)
			{
				this.Visit(node.Left);
				this.Out('[');
				this.Visit(node.Right);
				this.Out(']');
			}
			else
			{
				ExpressionType nodeType = node.NodeType;
				string text;
				switch (nodeType)
				{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
					text = "+";
					goto IL_02CE;
				case ExpressionType.And:
					text = (ExpressionStringBuilder.IsBool(node) ? "And" : "&");
					goto IL_02CE;
				case ExpressionType.AndAlso:
					text = "AndAlso";
					goto IL_02CE;
				case ExpressionType.ArrayLength:
				case ExpressionType.ArrayIndex:
				case ExpressionType.Call:
				case ExpressionType.Conditional:
				case ExpressionType.Constant:
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
				case ExpressionType.Invoke:
				case ExpressionType.Lambda:
				case ExpressionType.ListInit:
				case ExpressionType.MemberAccess:
				case ExpressionType.MemberInit:
				case ExpressionType.Negate:
				case ExpressionType.UnaryPlus:
				case ExpressionType.NegateChecked:
				case ExpressionType.New:
				case ExpressionType.NewArrayInit:
				case ExpressionType.NewArrayBounds:
				case ExpressionType.Not:
				case ExpressionType.Parameter:
				case ExpressionType.Quote:
				case ExpressionType.TypeAs:
				case ExpressionType.TypeIs:
					break;
				case ExpressionType.Coalesce:
					text = "??";
					goto IL_02CE;
				case ExpressionType.Divide:
					text = "/";
					goto IL_02CE;
				case ExpressionType.Equal:
					text = "==";
					goto IL_02CE;
				case ExpressionType.ExclusiveOr:
					text = "^";
					goto IL_02CE;
				case ExpressionType.GreaterThan:
					text = ">";
					goto IL_02CE;
				case ExpressionType.GreaterThanOrEqual:
					text = ">=";
					goto IL_02CE;
				case ExpressionType.LeftShift:
					text = "<<";
					goto IL_02CE;
				case ExpressionType.LessThan:
					text = "<";
					goto IL_02CE;
				case ExpressionType.LessThanOrEqual:
					text = "<=";
					goto IL_02CE;
				case ExpressionType.Modulo:
					text = "%";
					goto IL_02CE;
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
					text = "*";
					goto IL_02CE;
				case ExpressionType.NotEqual:
					text = "!=";
					goto IL_02CE;
				case ExpressionType.Or:
					text = (ExpressionStringBuilder.IsBool(node) ? "Or" : "|");
					goto IL_02CE;
				case ExpressionType.OrElse:
					text = "OrElse";
					goto IL_02CE;
				case ExpressionType.Power:
					text = "**";
					goto IL_02CE;
				case ExpressionType.RightShift:
					text = ">>";
					goto IL_02CE;
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
					text = "-";
					goto IL_02CE;
				case ExpressionType.Assign:
					text = "=";
					goto IL_02CE;
				default:
					switch (nodeType)
					{
					case ExpressionType.AddAssign:
					case ExpressionType.AddAssignChecked:
						text = "+=";
						goto IL_02CE;
					case ExpressionType.AndAssign:
						text = (ExpressionStringBuilder.IsBool(node) ? "&&=" : "&=");
						goto IL_02CE;
					case ExpressionType.DivideAssign:
						text = "/=";
						goto IL_02CE;
					case ExpressionType.ExclusiveOrAssign:
						text = "^=";
						goto IL_02CE;
					case ExpressionType.LeftShiftAssign:
						text = "<<=";
						goto IL_02CE;
					case ExpressionType.ModuloAssign:
						text = "%=";
						goto IL_02CE;
					case ExpressionType.MultiplyAssign:
					case ExpressionType.MultiplyAssignChecked:
						text = "*=";
						goto IL_02CE;
					case ExpressionType.OrAssign:
						text = (ExpressionStringBuilder.IsBool(node) ? "||=" : "|=");
						goto IL_02CE;
					case ExpressionType.PowerAssign:
						text = "**=";
						goto IL_02CE;
					case ExpressionType.RightShiftAssign:
						text = ">>=";
						goto IL_02CE;
					case ExpressionType.SubtractAssign:
					case ExpressionType.SubtractAssignChecked:
						text = "-=";
						goto IL_02CE;
					}
					break;
				}
				throw new InvalidOperationException();
				IL_02CE:
				this.Out('(');
				this.Visit(node.Left);
				this.Out(' ');
				this.Out(text);
				this.Out(' ');
				this.Visit(node.Right);
				this.Out(')');
			}
			return node;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00039250 File Offset: 0x00037450
		protected internal override Expression VisitParameter(ParameterExpression node)
		{
			if (node.IsByRef)
			{
				this.Out("ref ");
			}
			string name = node.Name;
			if (string.IsNullOrEmpty(name))
			{
				this.Out("Param_" + this.GetParamId(node).ToString());
			}
			else
			{
				this.Out(name);
			}
			return node;
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x000392A8 File Offset: 0x000374A8
		protected internal override Expression VisitLambda<T>(Expression<T> node)
		{
			if (node.ParameterCount == 1)
			{
				this.Visit(node.GetParameter(0));
			}
			else
			{
				this.Out('(');
				string text = ", ";
				int i = 0;
				int parameterCount = node.ParameterCount;
				while (i < parameterCount)
				{
					if (i > 0)
					{
						this.Out(text);
					}
					this.Visit(node.GetParameter(i));
					i++;
				}
				this.Out(')');
			}
			this.Out(" => ");
			this.Visit(node.Body);
			return node;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0003932C File Offset: 0x0003752C
		protected internal override Expression VisitListInit(ListInitExpression node)
		{
			this.Visit(node.NewExpression);
			this.Out(" {");
			int i = 0;
			int count = node.Initializers.Count;
			while (i < count)
			{
				if (i > 0)
				{
					this.Out(", ");
				}
				this.VisitElementInit(node.Initializers[i]);
				i++;
			}
			this.Out('}');
			return node;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00039394 File Offset: 0x00037594
		protected internal override Expression VisitConditional(ConditionalExpression node)
		{
			this.Out("IIF(");
			this.Visit(node.Test);
			this.Out(", ");
			this.Visit(node.IfTrue);
			this.Out(", ");
			this.Visit(node.IfFalse);
			this.Out(')');
			return node;
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x000393F4 File Offset: 0x000375F4
		protected internal override Expression VisitConstant(ConstantExpression node)
		{
			if (node.Value != null)
			{
				string text = node.Value.ToString();
				if (node.Value is string)
				{
					this.Out('"');
					this.Out(text);
					this.Out('"');
				}
				else if (text == node.Value.GetType().ToString())
				{
					this.Out("value(");
					this.Out(text);
					this.Out(')');
				}
				else
				{
					this.Out(text);
				}
			}
			else
			{
				this.Out("null");
			}
			return node;
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00039484 File Offset: 0x00037684
		protected internal override Expression VisitDebugInfo(DebugInfoExpression node)
		{
			string text = string.Format(CultureInfo.CurrentCulture, "<DebugInfo({0}: {1}, {2}, {3}, {4})>", new object[]
			{
				node.Document.FileName,
				node.StartLine,
				node.StartColumn,
				node.EndLine,
				node.EndColumn
			});
			this.Out(text);
			return node;
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000394F5 File Offset: 0x000376F5
		protected internal override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
		{
			this.VisitExpressions<ParameterExpression>('(', node.Variables, ')');
			return node;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00039508 File Offset: 0x00037708
		private void OutMember(Expression instance, MemberInfo member)
		{
			if (instance != null)
			{
				this.Visit(instance);
			}
			else
			{
				this.Out(member.DeclaringType.Name);
			}
			this.Out('.');
			this.Out(member.Name);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0003953C File Offset: 0x0003773C
		protected internal override Expression VisitMember(MemberExpression node)
		{
			this.OutMember(node.Expression, node.Member);
			return node;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00039554 File Offset: 0x00037754
		protected internal override Expression VisitMemberInit(MemberInitExpression node)
		{
			if (node.NewExpression.ArgumentCount == 0 && node.NewExpression.Type.Name.Contains("<"))
			{
				this.Out("new");
			}
			else
			{
				this.Visit(node.NewExpression);
			}
			this.Out(" {");
			int i = 0;
			int count = node.Bindings.Count;
			while (i < count)
			{
				MemberBinding memberBinding = node.Bindings[i];
				if (i > 0)
				{
					this.Out(", ");
				}
				this.VisitMemberBinding(memberBinding);
				i++;
			}
			this.Out('}');
			return node;
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000395F4 File Offset: 0x000377F4
		protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
		{
			this.Out(assignment.Member.Name);
			this.Out(" = ");
			this.Visit(assignment.Expression);
			return assignment;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00039620 File Offset: 0x00037820
		protected override MemberListBinding VisitMemberListBinding(MemberListBinding binding)
		{
			this.Out(binding.Member.Name);
			this.Out(" = {");
			int i = 0;
			int count = binding.Initializers.Count;
			while (i < count)
			{
				if (i > 0)
				{
					this.Out(", ");
				}
				this.VisitElementInit(binding.Initializers[i]);
				i++;
			}
			this.Out('}');
			return binding;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0003968C File Offset: 0x0003788C
		protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
		{
			this.Out(binding.Member.Name);
			this.Out(" = {");
			int i = 0;
			int count = binding.Bindings.Count;
			while (i < count)
			{
				if (i > 0)
				{
					this.Out(", ");
				}
				this.VisitMemberBinding(binding.Bindings[i]);
				i++;
			}
			this.Out('}');
			return binding;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x000396F8 File Offset: 0x000378F8
		protected override ElementInit VisitElementInit(ElementInit initializer)
		{
			this.Out(initializer.AddMethod.ToString());
			string text = ", ";
			this.Out('(');
			int i = 0;
			int argumentCount = initializer.ArgumentCount;
			while (i < argumentCount)
			{
				if (i > 0)
				{
					this.Out(text);
				}
				this.Visit(initializer.GetArgument(i));
				i++;
			}
			this.Out(')');
			return initializer;
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0003975C File Offset: 0x0003795C
		protected internal override Expression VisitInvocation(InvocationExpression node)
		{
			this.Out("Invoke(");
			this.Visit(node.Expression);
			string text = ", ";
			int i = 0;
			int argumentCount = node.ArgumentCount;
			while (i < argumentCount)
			{
				this.Out(text);
				this.Visit(node.GetArgument(i));
				i++;
			}
			this.Out(')');
			return node;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x000397B8 File Offset: 0x000379B8
		protected internal override Expression VisitMethodCall(MethodCallExpression node)
		{
			int num = 0;
			Expression expression = node.Object;
			if (node.Method.GetCustomAttribute(typeof(ExtensionAttribute)) != null)
			{
				num = 1;
				expression = node.GetArgument(0);
			}
			if (expression != null)
			{
				this.Visit(expression);
				this.Out('.');
			}
			this.Out(node.Method.Name);
			this.Out('(');
			int i = num;
			int argumentCount = node.ArgumentCount;
			while (i < argumentCount)
			{
				if (i > num)
				{
					this.Out(", ");
				}
				this.Visit(node.GetArgument(i));
				i++;
			}
			this.Out(')');
			return node;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00039854 File Offset: 0x00037A54
		protected internal override Expression VisitNewArray(NewArrayExpression node)
		{
			ExpressionType nodeType = node.NodeType;
			if (nodeType != ExpressionType.NewArrayInit)
			{
				if (nodeType == ExpressionType.NewArrayBounds)
				{
					this.Out("new ");
					this.Out(node.Type.ToString());
					this.VisitExpressions<Expression>('(', node.Expressions, ')');
				}
			}
			else
			{
				this.Out("new [] ");
				this.VisitExpressions<Expression>('{', node.Expressions, '}');
			}
			return node;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x000398BC File Offset: 0x00037ABC
		protected internal override Expression VisitNew(NewExpression node)
		{
			this.Out("new ");
			this.Out(node.Type.Name);
			this.Out('(');
			ReadOnlyCollection<MemberInfo> members = node.Members;
			for (int i = 0; i < node.ArgumentCount; i++)
			{
				if (i > 0)
				{
					this.Out(", ");
				}
				if (members != null)
				{
					string name = members[i].Name;
					this.Out(name);
					this.Out(" = ");
				}
				this.Visit(node.GetArgument(i));
			}
			this.Out(')');
			return node;
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00039950 File Offset: 0x00037B50
		protected internal override Expression VisitTypeBinary(TypeBinaryExpression node)
		{
			this.Out('(');
			this.Visit(node.Expression);
			ExpressionType nodeType = node.NodeType;
			if (nodeType != ExpressionType.TypeIs)
			{
				if (nodeType == ExpressionType.TypeEqual)
				{
					this.Out(" TypeEqual ");
				}
			}
			else
			{
				this.Out(" Is ");
			}
			this.Out(node.TypeOperand.Name);
			this.Out(')');
			return node;
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x000399B8 File Offset: 0x00037BB8
		protected internal override Expression VisitUnary(UnaryExpression node)
		{
			ExpressionType expressionType = node.NodeType;
			if (expressionType <= ExpressionType.Quote)
			{
				if (expressionType <= ExpressionType.Convert)
				{
					if (expressionType == ExpressionType.ArrayLength)
					{
						this.Out("ArrayLength(");
						goto IL_019E;
					}
					if (expressionType == ExpressionType.Convert)
					{
						this.Out("Convert(");
						goto IL_019E;
					}
				}
				else
				{
					if (expressionType == ExpressionType.ConvertChecked)
					{
						this.Out("ConvertChecked(");
						goto IL_019E;
					}
					switch (expressionType)
					{
					case ExpressionType.Negate:
					case ExpressionType.NegateChecked:
						this.Out('-');
						goto IL_019E;
					case ExpressionType.UnaryPlus:
						this.Out('+');
						goto IL_019E;
					case ExpressionType.New:
					case ExpressionType.NewArrayInit:
					case ExpressionType.NewArrayBounds:
						break;
					case ExpressionType.Not:
						this.Out("Not(");
						goto IL_019E;
					default:
						if (expressionType == ExpressionType.Quote)
						{
							goto IL_019E;
						}
						break;
					}
				}
			}
			else if (expressionType <= ExpressionType.Increment)
			{
				if (expressionType == ExpressionType.TypeAs)
				{
					this.Out('(');
					goto IL_019E;
				}
				if (expressionType == ExpressionType.Decrement)
				{
					this.Out("Decrement(");
					goto IL_019E;
				}
				if (expressionType == ExpressionType.Increment)
				{
					this.Out("Increment(");
					goto IL_019E;
				}
			}
			else
			{
				if (expressionType == ExpressionType.Throw)
				{
					this.Out("throw(");
					goto IL_019E;
				}
				if (expressionType == ExpressionType.Unbox)
				{
					this.Out("Unbox(");
					goto IL_019E;
				}
				switch (expressionType)
				{
				case ExpressionType.PreIncrementAssign:
					this.Out("++");
					goto IL_019E;
				case ExpressionType.PreDecrementAssign:
					this.Out("--");
					goto IL_019E;
				case ExpressionType.PostIncrementAssign:
				case ExpressionType.PostDecrementAssign:
					goto IL_019E;
				case ExpressionType.OnesComplement:
					this.Out("~(");
					goto IL_019E;
				case ExpressionType.IsTrue:
					this.Out("IsTrue(");
					goto IL_019E;
				case ExpressionType.IsFalse:
					this.Out("IsFalse(");
					goto IL_019E;
				}
			}
			throw new InvalidOperationException();
			IL_019E:
			this.Visit(node.Operand);
			expressionType = node.NodeType;
			if (expressionType <= ExpressionType.NegateChecked)
			{
				if (expressionType - ExpressionType.Convert <= 1)
				{
					this.Out(", ");
					this.Out(node.Type.Name);
					this.Out(')');
					return node;
				}
				if (expressionType - ExpressionType.Negate <= 2)
				{
					return node;
				}
			}
			else
			{
				if (expressionType == ExpressionType.Quote)
				{
					return node;
				}
				if (expressionType == ExpressionType.TypeAs)
				{
					this.Out(" As ");
					this.Out(node.Type.Name);
					this.Out(')');
					return node;
				}
				switch (expressionType)
				{
				case ExpressionType.PreIncrementAssign:
				case ExpressionType.PreDecrementAssign:
					return node;
				case ExpressionType.PostIncrementAssign:
					this.Out("++");
					return node;
				case ExpressionType.PostDecrementAssign:
					this.Out("--");
					return node;
				}
			}
			this.Out(')');
			return node;
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00039C2C File Offset: 0x00037E2C
		protected internal override Expression VisitBlock(BlockExpression node)
		{
			this.Out('{');
			foreach (ParameterExpression parameterExpression in node.Variables)
			{
				this.Out("var ");
				this.Visit(parameterExpression);
				this.Out(';');
			}
			this.Out(" ... }");
			return node;
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00039CA4 File Offset: 0x00037EA4
		protected internal override Expression VisitDefault(DefaultExpression node)
		{
			this.Out("default(");
			this.Out(node.Type.Name);
			this.Out(')');
			return node;
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00039CCB File Offset: 0x00037ECB
		protected internal override Expression VisitLabel(LabelExpression node)
		{
			this.Out("{ ... } ");
			this.DumpLabel(node.Target);
			this.Out(':');
			return node;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00039CF0 File Offset: 0x00037EF0
		protected internal override Expression VisitGoto(GotoExpression node)
		{
			string text;
			switch (node.Kind)
			{
			case GotoExpressionKind.Goto:
				text = "goto";
				break;
			case GotoExpressionKind.Return:
				text = "return";
				break;
			case GotoExpressionKind.Break:
				text = "break";
				break;
			case GotoExpressionKind.Continue:
				text = "continue";
				break;
			default:
				throw new InvalidOperationException();
			}
			this.Out(text);
			this.Out(' ');
			this.DumpLabel(node.Target);
			if (node.Value != null)
			{
				this.Out(" (");
				this.Visit(node.Value);
				this.Out(")");
			}
			return node;
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00039D89 File Offset: 0x00037F89
		protected internal override Expression VisitLoop(LoopExpression node)
		{
			this.Out("loop { ... }");
			return node;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00039D97 File Offset: 0x00037F97
		protected override SwitchCase VisitSwitchCase(SwitchCase node)
		{
			this.Out("case ");
			this.VisitExpressions<Expression>('(', node.TestValues, ')');
			this.Out(": ...");
			return node;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00039DC0 File Offset: 0x00037FC0
		protected internal override Expression VisitSwitch(SwitchExpression node)
		{
			this.Out("switch ");
			this.Out('(');
			this.Visit(node.SwitchValue);
			this.Out(") { ... }");
			return node;
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00039DF0 File Offset: 0x00037FF0
		protected override CatchBlock VisitCatchBlock(CatchBlock node)
		{
			this.Out("catch (");
			this.Out(node.Test.Name);
			ParameterExpression variable = node.Variable;
			if (!string.IsNullOrEmpty((variable != null) ? variable.Name : null))
			{
				this.Out(' ');
				this.Out(node.Variable.Name);
			}
			this.Out(") { ... }");
			return node;
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00039E57 File Offset: 0x00038057
		protected internal override Expression VisitTry(TryExpression node)
		{
			this.Out("try { ... }");
			return node;
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00039E68 File Offset: 0x00038068
		protected internal override Expression VisitIndex(IndexExpression node)
		{
			if (node.Object != null)
			{
				this.Visit(node.Object);
			}
			else
			{
				this.Out(node.Indexer.DeclaringType.Name);
			}
			if (node.Indexer != null)
			{
				this.Out('.');
				this.Out(node.Indexer.Name);
			}
			this.Out('[');
			int i = 0;
			int argumentCount = node.ArgumentCount;
			while (i < argumentCount)
			{
				if (i > 0)
				{
					this.Out(", ");
				}
				this.Visit(node.GetArgument(i));
				i++;
			}
			this.Out(']');
			return node;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00039F0C File Offset: 0x0003810C
		protected internal override Expression VisitExtension(Expression node)
		{
			MethodInfo method = node.GetType().GetMethod("ToString", Type.EmptyTypes);
			if (method.DeclaringType != typeof(Expression) && !method.IsStatic)
			{
				this.Out(node.ToString());
				return node;
			}
			this.Out('[');
			this.Out((node.NodeType == ExpressionType.Extension) ? node.GetType().FullName : node.NodeType.ToString());
			this.Out(']');
			return node;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00039FA0 File Offset: 0x000381A0
		private void DumpLabel(LabelTarget target)
		{
			if (!string.IsNullOrEmpty(target.Name))
			{
				this.Out(target.Name);
				return;
			}
			this.Out("UnnamedLabel_" + this.GetLabelId(target).ToString());
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00039FE6 File Offset: 0x000381E6
		private static bool IsBool(Expression node)
		{
			return node.Type == typeof(bool) || node.Type == typeof(bool?);
		}

		// Token: 0x04000993 RID: 2451
		private readonly StringBuilder _out;

		// Token: 0x04000994 RID: 2452
		private Dictionary<object, int> _ids;
	}
}
