using System;
using System.Text.RegularExpressions;
using UnityEngine.UIElements.StyleSheets.Syntax;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x020004A2 RID: 1186
	internal class StyleMatcher : BaseStyleMatcher
	{
		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x060024FB RID: 9467 RVA: 0x0009BD9A File Offset: 0x00099F9A
		private string current
		{
			get
			{
				return base.hasCurrent ? this.m_PropertyParts[base.currentIndex] : null;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x060024FC RID: 9468 RVA: 0x0009BDB4 File Offset: 0x00099FB4
		public override int valueCount
		{
			get
			{
				return this.m_PropertyParts.Length;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x0009BDBE File Offset: 0x00099FBE
		public override bool isCurrentVariable
		{
			get
			{
				return base.hasCurrent && this.current.StartsWith("var(");
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x060024FE RID: 9470 RVA: 0x0009BDDB File Offset: 0x00099FDB
		public override bool isCurrentComma
		{
			get
			{
				return base.hasCurrent && this.current == ",";
			}
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x0009BDF8 File Offset: 0x00099FF8
		private void Initialize(string propertyValue)
		{
			base.Initialize();
			this.m_PropertyParts = this.m_Parser.Parse(propertyValue);
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x0009BE14 File Offset: 0x0009A014
		public MatchResult Match(Expression exp, string propertyValue)
		{
			MatchResult matchResult = new MatchResult
			{
				errorCode = MatchResultErrorCode.None
			};
			bool flag = string.IsNullOrEmpty(propertyValue);
			MatchResult matchResult2;
			if (flag)
			{
				matchResult.errorCode = MatchResultErrorCode.EmptyValue;
				matchResult2 = matchResult;
			}
			else
			{
				this.Initialize(propertyValue);
				string current = this.current;
				bool flag2 = current == "initial" || current.StartsWith("env(");
				bool flag3;
				if (flag2)
				{
					base.MoveNext();
					flag3 = true;
				}
				else
				{
					flag3 = base.Match(exp);
				}
				bool flag4 = !flag3;
				if (flag4)
				{
					matchResult.errorCode = MatchResultErrorCode.Syntax;
					matchResult.errorValue = this.current;
				}
				else
				{
					bool hasCurrent = base.hasCurrent;
					if (hasCurrent)
					{
						matchResult.errorCode = MatchResultErrorCode.ExpectedEndOfValue;
						matchResult.errorValue = this.current;
					}
				}
				matchResult2 = matchResult;
			}
			return matchResult2;
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x0009BEE8 File Offset: 0x0009A0E8
		protected override bool MatchKeyword(string keyword)
		{
			return string.Compare(this.current, keyword, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x0009BF0C File Offset: 0x0009A10C
		protected override bool MatchNumber()
		{
			string current = this.current;
			Match match = StyleMatcher.s_NumberRegex.Match(current);
			return match.Success;
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x0009BF38 File Offset: 0x0009A138
		protected override bool MatchInteger()
		{
			string current = this.current;
			Match match = StyleMatcher.s_IntegerRegex.Match(current);
			return match.Success;
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x0009BF64 File Offset: 0x0009A164
		protected override bool MatchLength()
		{
			string current = this.current;
			Match match = StyleMatcher.s_LengthRegex.Match(current);
			bool success = match.Success;
			bool flag;
			if (success)
			{
				flag = true;
			}
			else
			{
				match = StyleMatcher.s_ZeroRegex.Match(current);
				flag = match.Success;
			}
			return flag;
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x0009BFAC File Offset: 0x0009A1AC
		protected override bool MatchPercentage()
		{
			string current = this.current;
			Match match = StyleMatcher.s_PercentRegex.Match(current);
			bool success = match.Success;
			bool flag;
			if (success)
			{
				flag = true;
			}
			else
			{
				match = StyleMatcher.s_ZeroRegex.Match(current);
				flag = match.Success;
			}
			return flag;
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x0009BFF4 File Offset: 0x0009A1F4
		protected override bool MatchColor()
		{
			string current = this.current;
			Match match = StyleMatcher.s_HexColorRegex.Match(current);
			bool success = match.Success;
			bool flag;
			if (success)
			{
				flag = true;
			}
			else
			{
				match = StyleMatcher.s_RgbRegex.Match(current);
				bool success2 = match.Success;
				if (success2)
				{
					flag = true;
				}
				else
				{
					match = StyleMatcher.s_RgbaRegex.Match(current);
					bool success3 = match.Success;
					if (success3)
					{
						flag = true;
					}
					else
					{
						Color clear = Color.clear;
						bool flag2 = StyleSheetColor.TryGetColor(current, out clear);
						flag = flag2;
					}
				}
			}
			return flag;
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x0009C080 File Offset: 0x0009A280
		protected override bool MatchResource()
		{
			string current = this.current;
			Match match = StyleMatcher.s_ResourceRegex.Match(current);
			bool flag = !match.Success;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				string text = match.Groups[1].Value.Trim();
				match = StyleMatcher.s_VarFunctionRegex.Match(text);
				flag2 = !match.Success;
			}
			return flag2;
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x0009C0E8 File Offset: 0x0009A2E8
		protected override bool MatchUrl()
		{
			string current = this.current;
			Match match = StyleMatcher.s_UrlRegex.Match(current);
			bool flag = !match.Success;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				string text = match.Groups[1].Value.Trim();
				match = StyleMatcher.s_VarFunctionRegex.Match(text);
				flag2 = !match.Success;
			}
			return flag2;
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x0009C150 File Offset: 0x0009A350
		protected override bool MatchTime()
		{
			string current = this.current;
			Match match = StyleMatcher.s_TimeRegex.Match(current);
			return match.Success;
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x0009C17C File Offset: 0x0009A37C
		protected override bool MatchAngle()
		{
			string current = this.current;
			Match match = StyleMatcher.s_AngleRegex.Match(current);
			bool success = match.Success;
			bool flag;
			if (success)
			{
				flag = true;
			}
			else
			{
				match = StyleMatcher.s_ZeroRegex.Match(current);
				flag = match.Success;
			}
			return flag;
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x0009C1C4 File Offset: 0x0009A3C4
		protected override bool MatchCustomIdent()
		{
			string current = this.current;
			Match match = BaseStyleMatcher.s_CustomIdentRegex.Match(current);
			return match.Success && match.Length == current.Length;
		}

		// Token: 0x040011CA RID: 4554
		private StylePropertyValueParser m_Parser = new StylePropertyValueParser();

		// Token: 0x040011CB RID: 4555
		private string[] m_PropertyParts;

		// Token: 0x040011CC RID: 4556
		private static readonly Regex s_NumberRegex = new Regex("^[+-]?\\d+(?:\\.\\d+)?$", RegexOptions.Compiled);

		// Token: 0x040011CD RID: 4557
		private static readonly Regex s_IntegerRegex = new Regex("^[+-]?\\d+$", RegexOptions.Compiled);

		// Token: 0x040011CE RID: 4558
		private static readonly Regex s_ZeroRegex = new Regex("^0(?:\\.0+)?$", RegexOptions.Compiled);

		// Token: 0x040011CF RID: 4559
		private static readonly Regex s_LengthRegex = new Regex("^[+-]?\\d+(?:\\.\\d+)?(?:px)$", RegexOptions.Compiled);

		// Token: 0x040011D0 RID: 4560
		private static readonly Regex s_PercentRegex = new Regex("^[+-]?\\d+(?:\\.\\d+)?(?:%)$", RegexOptions.Compiled);

		// Token: 0x040011D1 RID: 4561
		private static readonly Regex s_HexColorRegex = new Regex("^#[a-fA-F0-9]{3}(?:[a-fA-F0-9]{3})?$", RegexOptions.Compiled);

		// Token: 0x040011D2 RID: 4562
		private static readonly Regex s_RgbRegex = new Regex("^rgb\\(\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*(\\d+)\\s*\\)$", RegexOptions.Compiled);

		// Token: 0x040011D3 RID: 4563
		private static readonly Regex s_RgbaRegex = new Regex("rgba\\(\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*(\\d+)\\s*,\\s*([\\d.]+)\\s*\\)$", RegexOptions.Compiled);

		// Token: 0x040011D4 RID: 4564
		private static readonly Regex s_VarFunctionRegex = new Regex("^var\\(.+\\)$", RegexOptions.Compiled);

		// Token: 0x040011D5 RID: 4565
		private static readonly Regex s_ResourceRegex = new Regex("^resource\\((.+)\\)$", RegexOptions.Compiled);

		// Token: 0x040011D6 RID: 4566
		private static readonly Regex s_UrlRegex = new Regex("^url\\((.+)\\)$", RegexOptions.Compiled);

		// Token: 0x040011D7 RID: 4567
		private static readonly Regex s_TimeRegex = new Regex("^[+-]?\\.?\\d+(?:\\.\\d+)?(?:s|ms)$", RegexOptions.Compiled);

		// Token: 0x040011D8 RID: 4568
		private static readonly Regex s_AngleRegex = new Regex("^[+-]?\\d+(?:\\.\\d+)?(?:deg|grad|rad|turn)$", RegexOptions.Compiled);
	}
}
