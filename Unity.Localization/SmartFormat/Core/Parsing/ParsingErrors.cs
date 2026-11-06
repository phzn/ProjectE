using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Localization.SmartFormat.Core.Parsing
{
	// Token: 0x02000084 RID: 132
	public class ParsingErrors : Exception
	{
		// Token: 0x06000378 RID: 888 RVA: 0x0000DCF4 File Offset: 0x0000BEF4
		public void Init(Format result)
		{
			this.result = result;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000DCFD File Offset: 0x0000BEFD
		public void Clear()
		{
			this.Issues.Clear();
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000DD0A File Offset: 0x0000BF0A
		public List<ParsingErrors.ParsingIssue> Issues { get; } = new List<ParsingErrors.ParsingIssue>();

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000DD12 File Offset: 0x0000BF12
		public bool HasIssues
		{
			get
			{
				return this.Issues.Count > 0;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000DD24 File Offset: 0x0000BF24
		public string MessageShort
		{
			get
			{
				return string.Format("The format string has {0} issue{1}: {2}", this.Issues.Count, (this.Issues.Count == 1) ? "" : "s", string.Join(", ", this.Issues.Select<ParsingErrors.ParsingIssue, string>((ParsingErrors.ParsingIssue i) => i.Issue).ToArray<string>()));
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		public override string Message
		{
			get
			{
				string text = "";
				int num = 0;
				foreach (ParsingErrors.ParsingIssue parsingIssue in this.Issues)
				{
					text += new string('-', parsingIssue.Index - num);
					if (parsingIssue.Length > 0)
					{
						text += new string('^', Math.Max(parsingIssue.Length, 1));
						num = parsingIssue.Index + parsingIssue.Length;
					}
					else
					{
						text += "^";
						num = parsingIssue.Index + 1;
					}
				}
				string text2 = "The format string has {0} issue{1}:\n{2}\nIn: \"{3}\"\nAt:  {4} ";
				object[] array = new object[5];
				array[0] = this.Issues.Count;
				array[1] = ((this.Issues.Count == 1) ? "" : "s");
				array[2] = string.Join(", ", this.Issues.Select<ParsingErrors.ParsingIssue, string>((ParsingErrors.ParsingIssue i) => i.Issue).ToArray<string>());
				array[3] = this.result.baseString;
				array[4] = text;
				return string.Format(text2, array);
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000DEE0 File Offset: 0x0000C0E0
		public void AddIssue(string issue, int startIndex, int endIndex)
		{
			this.Issues.Add(new ParsingErrors.ParsingIssue(issue, startIndex, endIndex - startIndex));
		}

		// Token: 0x040000F9 RID: 249
		private Format result;

		// Token: 0x02000145 RID: 325
		public class ParsingIssue
		{
			// Token: 0x060007C5 RID: 1989 RVA: 0x000192CA File Offset: 0x000174CA
			public ParsingIssue(string issue, int index, int length)
			{
				this.Issue = issue;
				this.Index = index;
				this.Length = length;
			}

			// Token: 0x1700018A RID: 394
			// (get) Token: 0x060007C6 RID: 1990 RVA: 0x000192E7 File Offset: 0x000174E7
			public int Index { get; }

			// Token: 0x1700018B RID: 395
			// (get) Token: 0x060007C7 RID: 1991 RVA: 0x000192EF File Offset: 0x000174EF
			public int Length { get; }

			// Token: 0x1700018C RID: 396
			// (get) Token: 0x060007C8 RID: 1992 RVA: 0x000192F7 File Offset: 0x000174F7
			public string Issue { get; }
		}
	}
}
