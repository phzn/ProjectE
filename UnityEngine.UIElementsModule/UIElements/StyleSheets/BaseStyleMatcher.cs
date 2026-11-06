using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UIElements.StyleSheets.Syntax;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x020004A0 RID: 1184
	internal abstract class BaseStyleMatcher
	{
		// Token: 0x060024D5 RID: 9429
		protected abstract bool MatchKeyword(string keyword);

		// Token: 0x060024D6 RID: 9430
		protected abstract bool MatchNumber();

		// Token: 0x060024D7 RID: 9431
		protected abstract bool MatchInteger();

		// Token: 0x060024D8 RID: 9432
		protected abstract bool MatchLength();

		// Token: 0x060024D9 RID: 9433
		protected abstract bool MatchPercentage();

		// Token: 0x060024DA RID: 9434
		protected abstract bool MatchColor();

		// Token: 0x060024DB RID: 9435
		protected abstract bool MatchResource();

		// Token: 0x060024DC RID: 9436
		protected abstract bool MatchUrl();

		// Token: 0x060024DD RID: 9437
		protected abstract bool MatchTime();

		// Token: 0x060024DE RID: 9438
		protected abstract bool MatchAngle();

		// Token: 0x060024DF RID: 9439
		protected abstract bool MatchCustomIdent();

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x060024E0 RID: 9440
		public abstract int valueCount { get; }

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x060024E1 RID: 9441
		public abstract bool isCurrentVariable { get; }

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x060024E2 RID: 9442
		public abstract bool isCurrentComma { get; }

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x060024E3 RID: 9443 RVA: 0x0009B65B File Offset: 0x0009985B
		public bool hasCurrent
		{
			get
			{
				return this.m_CurrentContext.valueIndex < this.valueCount;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x060024E4 RID: 9444 RVA: 0x0009B670 File Offset: 0x00099870
		// (set) Token: 0x060024E5 RID: 9445 RVA: 0x0009B67D File Offset: 0x0009987D
		public int currentIndex
		{
			get
			{
				return this.m_CurrentContext.valueIndex;
			}
			set
			{
				this.m_CurrentContext.valueIndex = value;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x060024E6 RID: 9446 RVA: 0x0009B68B File Offset: 0x0009988B
		// (set) Token: 0x060024E7 RID: 9447 RVA: 0x0009B698 File Offset: 0x00099898
		public int matchedVariableCount
		{
			get
			{
				return this.m_CurrentContext.matchedVariableCount;
			}
			set
			{
				this.m_CurrentContext.matchedVariableCount = value;
			}
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x0009B6A6 File Offset: 0x000998A6
		protected void Initialize()
		{
			this.m_CurrentContext = default(BaseStyleMatcher.MatchContext);
			this.m_ContextStack.Clear();
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x0009B6C4 File Offset: 0x000998C4
		public void MoveNext()
		{
			bool flag = this.currentIndex + 1 <= this.valueCount;
			if (flag)
			{
				int currentIndex = this.currentIndex;
				this.currentIndex = currentIndex + 1;
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x0009B6FC File Offset: 0x000998FC
		public void SaveContext()
		{
			this.m_ContextStack.Push(this.m_CurrentContext);
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x0009B711 File Offset: 0x00099911
		public void RestoreContext()
		{
			this.m_CurrentContext = this.m_ContextStack.Pop();
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x0009B725 File Offset: 0x00099925
		public void DropContext()
		{
			this.m_ContextStack.Pop();
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x0009B734 File Offset: 0x00099934
		protected bool Match(Expression exp)
		{
			bool flag = exp.multiplier.type == ExpressionMultiplierType.None;
			bool flag2;
			if (flag)
			{
				flag2 = this.MatchExpression(exp);
			}
			else
			{
				Debug.Assert(exp.multiplier.type != ExpressionMultiplierType.GroupAtLeastOne, "'!' multiplier in syntax expression is not supported");
				flag2 = this.MatchExpressionWithMultiplier(exp);
			}
			return flag2;
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x0009B790 File Offset: 0x00099990
		private bool MatchExpression(Expression exp)
		{
			bool flag = false;
			bool flag2 = exp.type == ExpressionType.Combinator;
			if (flag2)
			{
				flag = this.MatchCombinator(exp);
			}
			else
			{
				bool isCurrentVariable = this.isCurrentVariable;
				if (isCurrentVariable)
				{
					flag = true;
					int matchedVariableCount = this.matchedVariableCount;
					this.matchedVariableCount = matchedVariableCount + 1;
				}
				else
				{
					bool flag3 = exp.type == ExpressionType.Data;
					if (flag3)
					{
						flag = this.MatchDataType(exp);
					}
					else
					{
						bool flag4 = exp.type == ExpressionType.Keyword;
						if (flag4)
						{
							flag = this.MatchKeyword(exp.keyword);
						}
					}
				}
				bool flag5 = flag;
				if (flag5)
				{
					this.MoveNext();
				}
			}
			bool flag6 = !flag && !this.hasCurrent && this.matchedVariableCount > 0;
			if (flag6)
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x0009B84C File Offset: 0x00099A4C
		private bool MatchExpressionWithMultiplier(Expression exp)
		{
			bool flag = exp.multiplier.type == ExpressionMultiplierType.OneOrMoreComma;
			bool flag2 = true;
			int min = exp.multiplier.min;
			int max = exp.multiplier.max;
			int num = 0;
			int num2 = 0;
			while (flag2 && this.hasCurrent && num2 < max)
			{
				flag2 = this.MatchExpression(exp);
				bool flag3 = flag2;
				if (flag3)
				{
					num++;
					bool flag4 = flag;
					if (flag4)
					{
						bool flag5 = !this.isCurrentComma;
						if (flag5)
						{
							break;
						}
						this.MoveNext();
					}
				}
				num2++;
			}
			flag2 = num >= min && num <= max;
			bool flag6 = !flag2 && num <= max && this.matchedVariableCount > 0;
			if (flag6)
			{
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x0009B918 File Offset: 0x00099B18
		private bool MatchGroup(Expression exp)
		{
			Debug.Assert(exp.subExpressions.Length == 1, "Group has invalid number of sub expressions");
			Expression expression = exp.subExpressions[0];
			return this.Match(expression);
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x0009B950 File Offset: 0x00099B50
		private bool MatchCombinator(Expression exp)
		{
			this.SaveContext();
			bool flag = false;
			switch (exp.combinator)
			{
			case ExpressionCombinator.Or:
				flag = this.MatchOr(exp);
				break;
			case ExpressionCombinator.OrOr:
				flag = this.MatchOrOr(exp);
				break;
			case ExpressionCombinator.AndAnd:
				flag = this.MatchAndAnd(exp);
				break;
			case ExpressionCombinator.Juxtaposition:
				flag = this.MatchJuxtaposition(exp);
				break;
			case ExpressionCombinator.Group:
				flag = this.MatchGroup(exp);
				break;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.DropContext();
			}
			else
			{
				this.RestoreContext();
			}
			return flag;
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x0009B9E0 File Offset: 0x00099BE0
		private bool MatchOr(Expression exp)
		{
			BaseStyleMatcher.MatchContext matchContext = default(BaseStyleMatcher.MatchContext);
			int num = 0;
			for (int i = 0; i < exp.subExpressions.Length; i++)
			{
				this.SaveContext();
				int currentIndex = this.currentIndex;
				bool flag = this.Match(exp.subExpressions[i]);
				int num2 = this.currentIndex - currentIndex;
				bool flag2 = flag && num2 > num;
				if (flag2)
				{
					num = num2;
					matchContext = this.m_CurrentContext;
				}
				this.RestoreContext();
			}
			bool flag3 = num > 0;
			bool flag4;
			if (flag3)
			{
				this.m_CurrentContext = matchContext;
				flag4 = true;
			}
			else
			{
				flag4 = false;
			}
			return flag4;
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x0009BA80 File Offset: 0x00099C80
		private bool MatchOrOr(Expression exp)
		{
			int num = this.MatchMany(exp);
			return num > 0;
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x0009BAA0 File Offset: 0x00099CA0
		private bool MatchAndAnd(Expression exp)
		{
			int num = this.MatchMany(exp);
			int num2 = exp.subExpressions.Length;
			return num == num2;
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x0009BAC8 File Offset: 0x00099CC8
		private unsafe int MatchMany(Expression exp)
		{
			BaseStyleMatcher.MatchContext matchContext = default(BaseStyleMatcher.MatchContext);
			int num = 0;
			int num2 = -1;
			int num3 = exp.subExpressions.Length;
			int* ptr;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)num3) * 4];
			}
			do
			{
				this.SaveContext();
				num2++;
				for (int i = 0; i < num3; i++)
				{
					int num4 = ((num2 > 0) ? ((num2 + i) % num3) : i);
					ptr[i] = num4;
				}
				int num5 = this.MatchManyByOrder(exp, ptr);
				bool flag = num5 > num;
				if (flag)
				{
					num = num5;
					matchContext = this.m_CurrentContext;
				}
				this.RestoreContext();
			}
			while (num < num3 && num2 < num3);
			bool flag2 = num > 0;
			if (flag2)
			{
				this.m_CurrentContext = matchContext;
			}
			return num;
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x0009BB88 File Offset: 0x00099D88
		private unsafe int MatchManyByOrder(Expression exp, int* matchOrder)
		{
			int num = exp.subExpressions.Length;
			int* ptr;
			int num2;
			int num3;
			int num4;
			checked
			{
				ptr = stackalloc int[unchecked((UIntPtr)num) * 4];
				num2 = 0;
				num3 = 0;
				num4 = 0;
			}
			while (num4 < num && num2 + num3 < num)
			{
				int num5 = matchOrder[num4];
				bool flag = false;
				for (int i = 0; i < num2; i++)
				{
					bool flag2 = ptr[i] == num5;
					if (flag2)
					{
						flag = true;
						break;
					}
				}
				bool flag3 = false;
				bool flag4 = !flag;
				if (flag4)
				{
					flag3 = this.Match(exp.subExpressions[num5]);
				}
				bool flag5 = flag3;
				if (flag5)
				{
					bool flag6 = num3 == this.matchedVariableCount;
					if (flag6)
					{
						ptr[num2] = num5;
						num2++;
					}
					else
					{
						num3 = this.matchedVariableCount;
					}
					num4 = 0;
				}
				else
				{
					num4++;
				}
			}
			return num2 + num3;
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x0009BC74 File Offset: 0x00099E74
		private bool MatchJuxtaposition(Expression exp)
		{
			bool flag = true;
			int num = 0;
			while (flag && num < exp.subExpressions.Length)
			{
				flag = this.Match(exp.subExpressions[num]);
				num++;
			}
			return flag;
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x0009BCB8 File Offset: 0x00099EB8
		private bool MatchDataType(Expression exp)
		{
			bool flag = false;
			bool hasCurrent = this.hasCurrent;
			if (hasCurrent)
			{
				switch (exp.dataType)
				{
				case DataType.Number:
					flag = this.MatchNumber();
					break;
				case DataType.Integer:
					flag = this.MatchInteger();
					break;
				case DataType.Length:
					flag = this.MatchLength();
					break;
				case DataType.Percentage:
					flag = this.MatchPercentage();
					break;
				case DataType.Color:
					flag = this.MatchColor();
					break;
				case DataType.Resource:
					flag = this.MatchResource();
					break;
				case DataType.Url:
					flag = this.MatchUrl();
					break;
				case DataType.Time:
					flag = this.MatchTime();
					break;
				case DataType.Angle:
					flag = this.MatchAngle();
					break;
				case DataType.CustomIdent:
					flag = this.MatchCustomIdent();
					break;
				}
			}
			return flag;
		}

		// Token: 0x040011C5 RID: 4549
		protected static readonly Regex s_CustomIdentRegex = new Regex("^-?[_a-z][_a-z0-9-]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		// Token: 0x040011C6 RID: 4550
		private Stack<BaseStyleMatcher.MatchContext> m_ContextStack = new Stack<BaseStyleMatcher.MatchContext>();

		// Token: 0x040011C7 RID: 4551
		private BaseStyleMatcher.MatchContext m_CurrentContext;

		// Token: 0x020004A1 RID: 1185
		private struct MatchContext
		{
			// Token: 0x040011C8 RID: 4552
			public int valueIndex;

			// Token: 0x040011C9 RID: 4553
			public int matchedVariableCount;
		}
	}
}
