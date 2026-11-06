using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.StyleSheets.Syntax
{
	// Token: 0x020004AE RID: 1198
	internal class StyleSyntaxParser
	{
		// Token: 0x0600252E RID: 9518 RVA: 0x0009CF3C File Offset: 0x0009B13C
		public Expression Parse(string syntax)
		{
			bool flag = string.IsNullOrEmpty(syntax);
			Expression expression;
			if (flag)
			{
				expression = null;
			}
			else
			{
				Expression expression2 = null;
				bool flag2 = !this.m_ParsedExpressionCache.TryGetValue(syntax, out expression2);
				if (flag2)
				{
					StyleSyntaxTokenizer styleSyntaxTokenizer = new StyleSyntaxTokenizer();
					styleSyntaxTokenizer.Tokenize(syntax);
					try
					{
						expression2 = this.ParseExpression(styleSyntaxTokenizer);
					}
					catch (Exception ex)
					{
						Debug.LogException(ex);
					}
					this.m_ParsedExpressionCache[syntax] = expression2;
				}
				expression = expression2;
			}
			return expression;
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x0009CFC0 File Offset: 0x0009B1C0
		private Expression ParseExpression(StyleSyntaxTokenizer tokenizer)
		{
			StyleSyntaxToken styleSyntaxToken = tokenizer.current;
			while (!StyleSyntaxParser.IsExpressionEnd(styleSyntaxToken))
			{
				bool flag = styleSyntaxToken.type == StyleSyntaxTokenType.String || styleSyntaxToken.type == StyleSyntaxTokenType.LessThan;
				Expression expression;
				if (flag)
				{
					expression = this.ParseTerm(tokenizer);
				}
				else
				{
					bool flag2 = styleSyntaxToken.type == StyleSyntaxTokenType.OpenBracket;
					if (!flag2)
					{
						throw new Exception(string.Format("Unexpected token '{0}' in expression", styleSyntaxToken.type));
					}
					expression = this.ParseGroup(tokenizer);
				}
				this.m_ExpressionStack.Push(expression);
				ExpressionCombinator expressionCombinator = this.ParseCombinatorType(tokenizer);
				bool flag3 = expressionCombinator > ExpressionCombinator.None;
				if (flag3)
				{
					bool flag4 = this.m_CombinatorStack.Count > 0;
					if (flag4)
					{
						ExpressionCombinator expressionCombinator2 = this.m_CombinatorStack.Peek();
						int num = (int)expressionCombinator2;
						int num2 = (int)expressionCombinator;
						while (num > num2 && expressionCombinator2 != ExpressionCombinator.Group)
						{
							this.ProcessCombinatorStack();
							expressionCombinator2 = ((this.m_CombinatorStack.Count > 0) ? this.m_CombinatorStack.Peek() : ExpressionCombinator.None);
							num = (int)expressionCombinator2;
						}
					}
					this.m_CombinatorStack.Push(expressionCombinator);
				}
				styleSyntaxToken = tokenizer.current;
			}
			while (this.m_CombinatorStack.Count > 0)
			{
				ExpressionCombinator expressionCombinator3 = this.m_CombinatorStack.Peek();
				bool flag5 = expressionCombinator3 == ExpressionCombinator.Group;
				if (flag5)
				{
					this.m_CombinatorStack.Pop();
					break;
				}
				this.ProcessCombinatorStack();
			}
			return this.m_ExpressionStack.Pop();
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x0009D14C File Offset: 0x0009B34C
		private void ProcessCombinatorStack()
		{
			ExpressionCombinator expressionCombinator = this.m_CombinatorStack.Pop();
			Expression expression = this.m_ExpressionStack.Pop();
			Expression expression2 = this.m_ExpressionStack.Pop();
			this.m_ProcessExpressionList.Clear();
			this.m_ProcessExpressionList.Add(expression2);
			this.m_ProcessExpressionList.Add(expression);
			while (this.m_CombinatorStack.Count > 0 && expressionCombinator == this.m_CombinatorStack.Peek())
			{
				Expression expression3 = this.m_ExpressionStack.Pop();
				this.m_ProcessExpressionList.Insert(0, expression3);
				this.m_CombinatorStack.Pop();
			}
			Expression expression4 = new Expression(ExpressionType.Combinator);
			expression4.combinator = expressionCombinator;
			expression4.subExpressions = this.m_ProcessExpressionList.ToArray();
			this.m_ExpressionStack.Push(expression4);
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x0009D224 File Offset: 0x0009B424
		private Expression ParseTerm(StyleSyntaxTokenizer tokenizer)
		{
			StyleSyntaxToken current = tokenizer.current;
			bool flag = current.type == StyleSyntaxTokenType.LessThan;
			Expression expression;
			if (flag)
			{
				expression = this.ParseDataType(tokenizer);
			}
			else
			{
				bool flag2 = current.type == StyleSyntaxTokenType.String;
				if (!flag2)
				{
					throw new Exception(string.Format("Unexpected token '{0}' in expression. Expected term token", current.type));
				}
				expression = new Expression(ExpressionType.Keyword);
				expression.keyword = current.text.ToLower();
				tokenizer.MoveNext();
			}
			this.ParseMultiplier(tokenizer, ref expression.multiplier);
			return expression;
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x0009D2B8 File Offset: 0x0009B4B8
		private ExpressionCombinator ParseCombinatorType(StyleSyntaxTokenizer tokenizer)
		{
			ExpressionCombinator expressionCombinator = ExpressionCombinator.None;
			StyleSyntaxToken styleSyntaxToken = tokenizer.current;
			while (!StyleSyntaxParser.IsExpressionEnd(styleSyntaxToken) && expressionCombinator == ExpressionCombinator.None)
			{
				StyleSyntaxToken styleSyntaxToken2 = tokenizer.PeekNext();
				switch (styleSyntaxToken.type)
				{
				case StyleSyntaxTokenType.Space:
				{
					bool flag = !StyleSyntaxParser.IsCombinator(styleSyntaxToken2) && styleSyntaxToken2.type != StyleSyntaxTokenType.CloseBracket;
					if (flag)
					{
						expressionCombinator = ExpressionCombinator.Juxtaposition;
					}
					break;
				}
				case StyleSyntaxTokenType.SingleBar:
					expressionCombinator = ExpressionCombinator.Or;
					break;
				case StyleSyntaxTokenType.DoubleBar:
					expressionCombinator = ExpressionCombinator.OrOr;
					break;
				case StyleSyntaxTokenType.DoubleAmpersand:
					expressionCombinator = ExpressionCombinator.AndAnd;
					break;
				default:
					throw new Exception(string.Format("Unexpected token '{0}' in expression. Expected combinator token", styleSyntaxToken.type));
				}
				styleSyntaxToken = tokenizer.MoveNext();
			}
			StyleSyntaxParser.EatSpace(tokenizer);
			return expressionCombinator;
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x0009D37C File Offset: 0x0009B57C
		private Expression ParseGroup(StyleSyntaxTokenizer tokenizer)
		{
			StyleSyntaxToken styleSyntaxToken = tokenizer.current;
			bool flag = styleSyntaxToken.type != StyleSyntaxTokenType.OpenBracket;
			if (flag)
			{
				throw new Exception(string.Format("Unexpected token '{0}' in group expression. Expected '[' token", styleSyntaxToken.type));
			}
			this.m_CombinatorStack.Push(ExpressionCombinator.Group);
			tokenizer.MoveNext();
			StyleSyntaxParser.EatSpace(tokenizer);
			Expression expression = this.ParseExpression(tokenizer);
			styleSyntaxToken = tokenizer.current;
			bool flag2 = styleSyntaxToken.type != StyleSyntaxTokenType.CloseBracket;
			if (flag2)
			{
				throw new Exception(string.Format("Unexpected token '{0}' in group expression. Expected ']' token", styleSyntaxToken.type));
			}
			tokenizer.MoveNext();
			Expression expression2 = new Expression(ExpressionType.Combinator);
			expression2.combinator = ExpressionCombinator.Group;
			expression2.subExpressions = new Expression[] { expression };
			this.ParseMultiplier(tokenizer, ref expression2.multiplier);
			return expression2;
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x0009D450 File Offset: 0x0009B650
		private Expression ParseDataType(StyleSyntaxTokenizer tokenizer)
		{
			StyleSyntaxToken styleSyntaxToken = tokenizer.current;
			bool flag = styleSyntaxToken.type != StyleSyntaxTokenType.LessThan;
			if (flag)
			{
				throw new Exception(string.Format("Unexpected token '{0}' in data type expression. Expected '<' token", styleSyntaxToken.type));
			}
			styleSyntaxToken = tokenizer.MoveNext();
			StyleSyntaxTokenType type = styleSyntaxToken.type;
			StyleSyntaxTokenType styleSyntaxTokenType = type;
			Expression expression;
			if (styleSyntaxTokenType != StyleSyntaxTokenType.String)
			{
				if (styleSyntaxTokenType != StyleSyntaxTokenType.SingleQuote)
				{
					throw new Exception(string.Format("Unexpected token '{0}' in data type expression", styleSyntaxToken.type));
				}
				expression = this.ParseProperty(tokenizer);
			}
			else
			{
				string text;
				bool flag2 = StylePropertyCache.TryGetNonTerminalValue(styleSyntaxToken.text, out text);
				if (flag2)
				{
					expression = this.ParseNonTerminalValue(text);
				}
				else
				{
					DataType dataType = DataType.None;
					try
					{
						object obj = Enum.Parse(typeof(DataType), styleSyntaxToken.text.Replace("-", ""), true);
						bool flag3 = obj != null;
						if (flag3)
						{
							dataType = (DataType)obj;
						}
					}
					catch (Exception)
					{
						throw new Exception("Unknown data type '" + styleSyntaxToken.text + "'");
					}
					expression = new Expression(ExpressionType.Data);
					expression.dataType = dataType;
				}
				tokenizer.MoveNext();
			}
			styleSyntaxToken = tokenizer.current;
			bool flag4 = styleSyntaxToken.type != StyleSyntaxTokenType.GreaterThan;
			if (flag4)
			{
				throw new Exception(string.Format("Unexpected token '{0}' in data type expression. Expected '>' token", styleSyntaxToken.type));
			}
			tokenizer.MoveNext();
			return expression;
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x0009D5CC File Offset: 0x0009B7CC
		private Expression ParseNonTerminalValue(string syntax)
		{
			Expression expression = null;
			bool flag = !this.m_ParsedExpressionCache.TryGetValue(syntax, out expression);
			if (flag)
			{
				this.m_CombinatorStack.Push(ExpressionCombinator.Group);
				expression = this.Parse(syntax);
			}
			return new Expression(ExpressionType.Combinator)
			{
				combinator = ExpressionCombinator.Group,
				subExpressions = new Expression[] { expression }
			};
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x0009D62C File Offset: 0x0009B82C
		private Expression ParseProperty(StyleSyntaxTokenizer tokenizer)
		{
			Expression expression = null;
			StyleSyntaxToken styleSyntaxToken = tokenizer.current;
			bool flag = styleSyntaxToken.type != StyleSyntaxTokenType.SingleQuote;
			if (flag)
			{
				throw new Exception(string.Format("Unexpected token '{0}' in property expression. Expected ''' token", styleSyntaxToken.type));
			}
			styleSyntaxToken = tokenizer.MoveNext();
			bool flag2 = styleSyntaxToken.type != StyleSyntaxTokenType.String;
			if (flag2)
			{
				throw new Exception(string.Format("Unexpected token '{0}' in property expression. Expected 'string' token", styleSyntaxToken.type));
			}
			string text = styleSyntaxToken.text;
			string text2;
			bool flag3 = !StylePropertyCache.TryGetSyntax(text, out text2);
			if (flag3)
			{
				throw new Exception("Unknown property '" + text + "' <''> expression.");
			}
			bool flag4 = !this.m_ParsedExpressionCache.TryGetValue(text2, out expression);
			if (flag4)
			{
				this.m_CombinatorStack.Push(ExpressionCombinator.Group);
				expression = this.Parse(text2);
			}
			styleSyntaxToken = tokenizer.MoveNext();
			bool flag5 = styleSyntaxToken.type != StyleSyntaxTokenType.SingleQuote;
			if (flag5)
			{
				throw new Exception(string.Format("Unexpected token '{0}' in property expression. Expected ''' token", styleSyntaxToken.type));
			}
			styleSyntaxToken = tokenizer.MoveNext();
			bool flag6 = styleSyntaxToken.type != StyleSyntaxTokenType.GreaterThan;
			if (flag6)
			{
				throw new Exception(string.Format("Unexpected token '{0}' in property expression. Expected '>' token", styleSyntaxToken.type));
			}
			return new Expression(ExpressionType.Combinator)
			{
				combinator = ExpressionCombinator.Group,
				subExpressions = new Expression[] { expression }
			};
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x0009D794 File Offset: 0x0009B994
		private void ParseMultiplier(StyleSyntaxTokenizer tokenizer, ref ExpressionMultiplier multiplier)
		{
			StyleSyntaxToken styleSyntaxToken = tokenizer.current;
			bool flag = StyleSyntaxParser.IsMultiplier(styleSyntaxToken);
			if (flag)
			{
				switch (styleSyntaxToken.type)
				{
				case StyleSyntaxTokenType.Asterisk:
					multiplier.type = ExpressionMultiplierType.ZeroOrMore;
					goto IL_00A1;
				case StyleSyntaxTokenType.Plus:
					multiplier.type = ExpressionMultiplierType.OneOrMore;
					goto IL_00A1;
				case StyleSyntaxTokenType.QuestionMark:
					multiplier.type = ExpressionMultiplierType.ZeroOrOne;
					goto IL_00A1;
				case StyleSyntaxTokenType.HashMark:
					multiplier.type = ExpressionMultiplierType.OneOrMoreComma;
					goto IL_00A1;
				case StyleSyntaxTokenType.ExclamationPoint:
					multiplier.type = ExpressionMultiplierType.GroupAtLeastOne;
					goto IL_00A1;
				case StyleSyntaxTokenType.OpenBrace:
					multiplier.type = ExpressionMultiplierType.Ranges;
					goto IL_00A1;
				}
				throw new Exception(string.Format("Unexpected token '{0}' in expression. Expected multiplier token", styleSyntaxToken.type));
				IL_00A1:
				styleSyntaxToken = tokenizer.MoveNext();
			}
			bool flag2 = multiplier.type == ExpressionMultiplierType.Ranges;
			if (flag2)
			{
				this.ParseRanges(tokenizer, out multiplier.min, out multiplier.max);
			}
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x0009D870 File Offset: 0x0009BA70
		private void ParseRanges(StyleSyntaxTokenizer tokenizer, out int min, out int max)
		{
			min = -1;
			max = -1;
			StyleSyntaxToken styleSyntaxToken = tokenizer.current;
			bool flag = false;
			while (styleSyntaxToken.type != StyleSyntaxTokenType.CloseBrace)
			{
				StyleSyntaxTokenType type = styleSyntaxToken.type;
				StyleSyntaxTokenType styleSyntaxTokenType = type;
				if (styleSyntaxTokenType != StyleSyntaxTokenType.Number)
				{
					if (styleSyntaxTokenType != StyleSyntaxTokenType.Comma)
					{
						throw new Exception(string.Format("Unexpected token '{0}' in expression. Expected ranges token", styleSyntaxToken.type));
					}
					flag = true;
				}
				else
				{
					bool flag2 = !flag;
					if (flag2)
					{
						min = styleSyntaxToken.number;
					}
					else
					{
						max = styleSyntaxToken.number;
					}
				}
				styleSyntaxToken = tokenizer.MoveNext();
			}
			tokenizer.MoveNext();
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x0009D908 File Offset: 0x0009BB08
		private static void EatSpace(StyleSyntaxTokenizer tokenizer)
		{
			StyleSyntaxToken current = tokenizer.current;
			bool flag = current.type == StyleSyntaxTokenType.Space;
			if (flag)
			{
				tokenizer.MoveNext();
			}
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x0009D934 File Offset: 0x0009BB34
		private static bool IsExpressionEnd(StyleSyntaxToken token)
		{
			StyleSyntaxTokenType type = token.type;
			StyleSyntaxTokenType styleSyntaxTokenType = type;
			return styleSyntaxTokenType == StyleSyntaxTokenType.CloseBracket || styleSyntaxTokenType == StyleSyntaxTokenType.End;
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x0009D964 File Offset: 0x0009BB64
		private static bool IsCombinator(StyleSyntaxToken token)
		{
			StyleSyntaxTokenType type = token.type;
			StyleSyntaxTokenType styleSyntaxTokenType = type;
			return styleSyntaxTokenType - StyleSyntaxTokenType.Space <= 3;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x0009D98C File Offset: 0x0009BB8C
		private static bool IsMultiplier(StyleSyntaxToken token)
		{
			StyleSyntaxTokenType type = token.type;
			StyleSyntaxTokenType styleSyntaxTokenType = type;
			return styleSyntaxTokenType - StyleSyntaxTokenType.Asterisk <= 4 || styleSyntaxTokenType == StyleSyntaxTokenType.OpenBrace;
		}

		// Token: 0x04001212 RID: 4626
		private List<Expression> m_ProcessExpressionList = new List<Expression>();

		// Token: 0x04001213 RID: 4627
		private Stack<Expression> m_ExpressionStack = new Stack<Expression>();

		// Token: 0x04001214 RID: 4628
		private Stack<ExpressionCombinator> m_CombinatorStack = new Stack<ExpressionCombinator>();

		// Token: 0x04001215 RID: 4629
		private Dictionary<string, Expression> m_ParsedExpressionCache = new Dictionary<string, Expression>();
	}
}
