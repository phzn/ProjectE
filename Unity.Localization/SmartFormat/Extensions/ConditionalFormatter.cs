using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Localization.SmartFormat.Net.Utilities;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x0200006B RID: 107
	[Serializable]
	public class ConditionalFormatter : FormatterBase, IFormatterLiteralExtractor
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x0000A803 File Offset: 0x00008A03
		public ConditionalFormatter()
		{
			base.Names = this.DefaultNames;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000A817 File Offset: 0x00008A17
		public override string[] DefaultNames
		{
			get
			{
				return new string[] { "conditional", "cond", "" };
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000A838 File Offset: 0x00008A38
		public override bool TryEvaluateFormat(IFormattingInfo formattingInfo)
		{
			Format format = formattingInfo.Format;
			object currentValue = formattingInfo.CurrentValue;
			if (format == null)
			{
				return false;
			}
			if (format.baseString[format.startIndex] == ':')
			{
				format = format.Substring(1);
			}
			IList<Format> list = format.Split('|');
			if (list.Count == 1)
			{
				return false;
			}
			bool flag = currentValue is IConvertible && !(currentValue is DateTime) && !(currentValue is string) && !(currentValue is bool);
			decimal num = (flag ? Convert.ToDecimal(currentValue) : 0m);
			int num2;
			if (flag)
			{
				num2 = -1;
				Format format2;
				for (;;)
				{
					num2++;
					if (num2 == list.Count)
					{
						break;
					}
					bool flag2;
					if (!ConditionalFormatter.TryEvaluateCondition(list[num2], num, out flag2, out format2))
					{
						if (num2 == 0)
						{
							goto IL_00C1;
						}
						flag2 = true;
					}
					if (flag2)
					{
						goto Block_12;
					}
				}
				return true;
				Block_12:
				formattingInfo.Write(format2, currentValue);
				return true;
			}
			IL_00C1:
			int count = list.Count;
			if (flag)
			{
				if (num < 0m)
				{
					num2 = count - 1;
				}
				else
				{
					num2 = Math.Min((int)Math.Floor(num), count - 1);
				}
			}
			else
			{
				object obj = currentValue;
				if (obj is bool)
				{
					bool flag3 = (bool)obj;
					num2 = (flag3 ? 0 : 1);
				}
				else if (obj is DateTime)
				{
					DateTime dateTime = (DateTime)obj;
					if (count == 3 && dateTime.ToUniversalTime().Date == SystemTime.Now().ToUniversalTime().Date)
					{
						num2 = 1;
					}
					else
					{
						DateTime dateTime2 = dateTime;
						if (dateTime2.ToUniversalTime() <= SystemTime.Now().ToUniversalTime())
						{
							num2 = 0;
						}
						else
						{
							num2 = count - 1;
						}
					}
				}
				else if (obj is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)obj;
					if (count == 3 && dateTimeOffset.UtcDateTime.Date == SystemTime.OffsetNow().UtcDateTime.Date)
					{
						num2 = 1;
					}
					else
					{
						DateTimeOffset dateTimeOffset2 = dateTimeOffset;
						if (dateTimeOffset2.UtcDateTime <= SystemTime.OffsetNow().UtcDateTime)
						{
							num2 = 0;
						}
						else
						{
							num2 = count - 1;
						}
					}
				}
				else if (obj is TimeSpan)
				{
					TimeSpan timeSpan = (TimeSpan)obj;
					if (count == 3 && timeSpan == TimeSpan.Zero)
					{
						num2 = 1;
					}
					else
					{
						TimeSpan timeSpan2 = timeSpan;
						if (timeSpan2.CompareTo(TimeSpan.Zero) <= 0)
						{
							num2 = 0;
						}
						else
						{
							num2 = count - 1;
						}
					}
				}
				else
				{
					string text = obj as string;
					if (text == null)
					{
						num2 = ((currentValue != null) ? 0 : 1);
					}
					else
					{
						num2 = ((!string.IsNullOrEmpty(text)) ? 0 : 1);
					}
				}
			}
			Format format3 = list[num2];
			formattingInfo.Write(format3, currentValue);
			return true;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000AB14 File Offset: 0x00008D14
		private static bool TryEvaluateCondition(Format parameter, decimal value, out bool conditionResult, out Format outputItem)
		{
			conditionResult = false;
			Match match = ConditionalFormatter._complexConditionPattern.Match(parameter.baseString, parameter.startIndex, parameter.endIndex - parameter.startIndex);
			if (!match.Success)
			{
				outputItem = parameter;
				return false;
			}
			CaptureCollection captures = match.Groups[1].Captures;
			CaptureCollection captures2 = match.Groups[2].Captures;
			CaptureCollection captures3 = match.Groups[3].Captures;
			int i = 0;
			while (i < captures.Count)
			{
				decimal num = decimal.Parse(captures3[i].Value, CultureInfo.InvariantCulture);
				bool flag = false;
				string value2 = captures2[i].Value;
				uint num2 = global::<PrivateImplementationDetails>.ComputeStringHash(value2);
				if (num2 <= 957132539U)
				{
					if (num2 <= 604802540U)
					{
						if (num2 != 284975636U)
						{
							if (num2 == 604802540U)
							{
								if (value2 == "!")
								{
									goto IL_01EE;
								}
							}
						}
						else if (value2 == ">=")
						{
							flag = value >= num;
						}
					}
					else if (num2 != 940354920U)
					{
						if (num2 == 957132539U)
						{
							if (value2 == "<")
							{
								flag = value < num;
							}
						}
					}
					else if (value2 == "=")
					{
						goto IL_01CA;
					}
				}
				else if (num2 <= 2428715011U)
				{
					if (num2 != 990687777U)
					{
						if (num2 == 2428715011U)
						{
							if (value2 == "!=")
							{
								goto IL_01EE;
							}
						}
					}
					else if (value2 == ">")
					{
						flag = value > num;
					}
				}
				else if (num2 != 2431966415U)
				{
					if (num2 == 2499223986U)
					{
						if (value2 == "<=")
						{
							flag = value <= num;
						}
					}
				}
				else if (value2 == "==")
				{
					goto IL_01CA;
				}
				IL_01F8:
				if (i == 0)
				{
					conditionResult = flag;
				}
				else if (captures[i].Value == "/")
				{
					conditionResult = conditionResult || flag;
				}
				else
				{
					conditionResult = conditionResult && flag;
				}
				i++;
				continue;
				IL_01CA:
				flag = value == num;
				goto IL_01F8;
				IL_01EE:
				flag = value != num;
				goto IL_01F8;
			}
			int num3 = match.Index + match.Length - parameter.startIndex;
			outputItem = parameter.Substring(num3);
			return true;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000AD80 File Offset: 0x00008F80
		public void WriteAllLiterals(IFormattingInfo formattingInfo)
		{
			Format format = formattingInfo.Format;
			if (format == null)
			{
				return;
			}
			if (format.baseString[format.startIndex] == ':')
			{
				format = format.Substring(1);
			}
			IList<Format> list = format.Split('|');
			if (list.Count == 1)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				Format format2 = list[i];
				Match match = ConditionalFormatter._complexConditionPattern.Match(format2.baseString, format2.startIndex, format2.endIndex - format2.startIndex);
				if (match.Success)
				{
					int num = match.Index + match.Length - format2.startIndex;
					format2 = format2.Substring(num);
				}
				formattingInfo.Write(format2, null);
			}
		}

		// Token: 0x040000BE RID: 190
		private static readonly Regex _complexConditionPattern = new Regex("^  (?:   ([&/]?)   ([<>=!]=?)   ([0-9.-]+)   )+   \\?", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
	}
}
