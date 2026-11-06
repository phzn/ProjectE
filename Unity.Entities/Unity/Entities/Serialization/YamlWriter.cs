using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Mathematics;

namespace Unity.Entities.Serialization
{
	// Token: 0x020003A2 RID: 930
	internal class YamlWriter
	{
		// Token: 0x060018A2 RID: 6306 RVA: 0x0005EB4C File Offset: 0x0005CD4C
		public YamlWriter(StreamWriter writer)
		{
			this.m_CurrentIndent = 0;
			this.m_IndentText = "";
			this.m_Writer = writer;
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0005EB6D File Offset: 0x0005CD6D
		public YamlWriter WriteLine(string line)
		{
			this.m_Writer.Write(this.m_IndentText);
			this.m_Writer.WriteLine(line);
			return this;
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0005EB90 File Offset: 0x0005CD90
		public YamlWriter WriteCommentLine<TC>(TC comment)
		{
			string text = comment.ToString();
			YamlWriter.ThrowMultiline(text, "WriteCommentLine");
			this.m_Writer.WriteLine("# " + text);
			return this;
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0005EBD0 File Offset: 0x0005CDD0
		public YamlWriter WriteKeyValue<TK, TV>(TK key, TV value)
		{
			string text = YamlWriter.FormatValue<TV>(value);
			YamlWriter.ThrowMultiline(text, "WriteKeyValue");
			this.WriteLine(key.ToString() + ": " + text);
			return this;
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0005EC0F File Offset: 0x0005CE0F
		public YamlWriter.Collection WriteCollection<TC>(TC collectionName)
		{
			return new YamlWriter.Collection(this, collectionName.ToString());
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0005EC24 File Offset: 0x0005CE24
		public YamlWriter WriteInlineSequence<TS>(TS sequenceName, IEnumerable<object> values)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			bool flag = true;
			foreach (object obj in values)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				string text = YamlWriter.FormatValue<object>(obj);
				YamlWriter.ThrowMultiline(text, "WriteInlineSequence");
				stringBuilder.Append(text);
				flag = false;
			}
			stringBuilder.Append(']');
			this.WriteLine(string.Format("{0}: {1}", sequenceName, stringBuilder));
			return this;
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0005ECC4 File Offset: 0x0005CEC4
		public YamlWriter WriteInlineMap<TM>(TM mapName, IEnumerable<KeyValuePair<object, object>> map)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('{');
			bool flag = true;
			foreach (KeyValuePair<object, object> keyValuePair in map)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				string text = YamlWriter.FormatValue<object>(keyValuePair.Value);
				YamlWriter.ThrowMultiline(text, "WriteInlineMap");
				stringBuilder.Append(string.Format("{0}: {1}", keyValuePair.Key, text));
				flag = false;
			}
			stringBuilder.Append('}');
			this.WriteLine(string.Format("{0}: {1}", mapName, stringBuilder));
			return this;
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x0005ED80 File Offset: 0x0005CF80
		public YamlWriter WriteInlineMap(string mapName, Dictionary<string, string> map)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('{');
			bool flag = true;
			foreach (KeyValuePair<string, string> keyValuePair in map)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				string text = YamlWriter.FormatValue<string>(keyValuePair.Value);
				YamlWriter.ThrowMultiline(text, "WriteInlineMap");
				stringBuilder.Append(keyValuePair.Key + ": " + text);
				flag = false;
			}
			stringBuilder.Append('}');
			this.WriteLine(string.Format("{0}: {1}", mapName, stringBuilder));
			return this;
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0005EE3C File Offset: 0x0005D03C
		public unsafe YamlWriter WriteFormattedBinaryData(string name, void* buffer, int size, int displayOffset = 0)
		{
			if (size % 4 != 0)
			{
				throw new ArgumentException(string.Format("Size must be at least integer aligned, but is {0}", size), "size");
			}
			int* ptr = (int*)buffer;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			int i = size / 4;
			using (this.WriteCollection<string>(name))
			{
				while (i > 0)
				{
					int num2 = math.min(16, i);
					stringBuilder.Clear();
					stringBuilder.Append(string.Format("0x{0:X8}: [", num + displayOffset));
					for (int j = 0; j < num2; j++)
					{
						if (j != 0)
						{
							stringBuilder.Append(", ");
						}
						int num3 = *ptr;
						ptr++;
						stringBuilder.Append(string.Format("0x{0:X8}", num3));
					}
					stringBuilder.Append("]");
					this.WriteLine(stringBuilder.ToString());
					num += 64;
					i -= num2;
				}
			}
			return this;
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0005EF3C File Offset: 0x0005D13C
		private static string FormatValue<T>(T value)
		{
			if (typeof(T) == typeof(string))
			{
				return string.Format("\"{0}\"", value);
			}
			return value.ToString();
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x0005EF77 File Offset: 0x0005D177
		private static bool IsMultiLineString(string text)
		{
			return text.Contains("\r") || text.Contains("\n");
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x0005EF93 File Offset: 0x0005D193
		private static void ThrowMultiline(string text, [CallerMemberName] string caller = "")
		{
			if (YamlWriter.IsMultiLineString(text))
			{
				throw new NotImplementedException("Method " + caller + " does not currently support multi-lines text");
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x0005EFB3 File Offset: 0x0005D1B3
		public int CurrentIndent
		{
			get
			{
				return this.m_CurrentIndent;
			}
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0005EFBB File Offset: 0x0005D1BB
		private void IncrementIndent()
		{
			this.m_CurrentIndent++;
			this.m_IndentText += "  ";
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0005EFE4 File Offset: 0x0005D1E4
		private void DecrementIndent()
		{
			int num = this.m_CurrentIndent - 1;
			this.m_CurrentIndent = num;
			if (num < 0)
			{
				throw new InvalidOperationException("Indent can't be less than 0");
			}
			this.m_IndentText = this.m_IndentText.Substring(0, this.m_CurrentIndent * 2);
		}

		// Token: 0x0400097B RID: 2427
		private int m_CurrentIndent;

		// Token: 0x0400097C RID: 2428
		private string m_IndentText;

		// Token: 0x0400097D RID: 2429
		private StreamWriter m_Writer;

		// Token: 0x020003A3 RID: 931
		public class Scope : IDisposable
		{
			// Token: 0x060018B1 RID: 6321 RVA: 0x0005F02A File Offset: 0x0005D22A
			internal Scope(YamlWriter owner, bool deferIndentIncrement)
			{
				this.m_Owner = owner;
				if (!deferIndentIncrement)
				{
					this.m_Owner.IncrementIndent();
				}
			}

			// Token: 0x060018B2 RID: 6322 RVA: 0x0005F047 File Offset: 0x0005D247
			public void Dispose()
			{
				this.m_Owner.DecrementIndent();
			}

			// Token: 0x0400097E RID: 2430
			protected YamlWriter m_Owner;
		}

		// Token: 0x020003A4 RID: 932
		public class Collection : YamlWriter.Scope
		{
			// Token: 0x060018B3 RID: 6323 RVA: 0x0005F054 File Offset: 0x0005D254
			public Collection(YamlWriter owner, string collectionName)
				: base(owner, true)
			{
				this.m_Owner.WriteLine(collectionName + ":");
				this.m_Owner.IncrementIndent();
				this.CollectionName = collectionName;
			}

			// Token: 0x17000229 RID: 553
			// (get) Token: 0x060018B4 RID: 6324 RVA: 0x0005F087 File Offset: 0x0005D287
			public string CollectionName { get; }
		}
	}
}
