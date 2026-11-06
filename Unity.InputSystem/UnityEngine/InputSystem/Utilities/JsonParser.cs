using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000137 RID: 311
	internal struct JsonParser
	{
		// Token: 0x0600114A RID: 4426 RVA: 0x00052721 File Offset: 0x00050921
		public JsonParser(string json)
		{
			this = default(JsonParser);
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			this.m_Text = json;
			this.m_Length = json.Length;
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0005274B File Offset: 0x0005094B
		public void Reset()
		{
			this.m_Position = 0;
			this.m_MatchAnyElementInArray = false;
			this.m_DryRun = false;
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00052764 File Offset: 0x00050964
		public override string ToString()
		{
			if (this.m_Text != null)
			{
				return string.Format("{0}: {1}", this.m_Position, this.m_Text.Substring(this.m_Position));
			}
			return base.ToString();
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x000527B0 File Offset: 0x000509B0
		public bool NavigateToProperty(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			int length = path.Length;
			int i = 0;
			this.m_DryRun = true;
			if (!this.ParseToken('{'))
			{
				return false;
			}
			while (this.m_Position < this.m_Length && i < length)
			{
				this.SkipWhitespace();
				if (this.m_Position == this.m_Length)
				{
					return false;
				}
				if (this.m_Text[this.m_Position] != '"')
				{
					return false;
				}
				this.m_Position++;
				int num = i;
				while (i < length)
				{
					char c = path[i];
					if (c == '/' || c == '[' || this.m_Text[this.m_Position] != c)
					{
						break;
					}
					this.m_Position++;
					i++;
				}
				if (this.m_Position < this.m_Length && this.m_Text[this.m_Position] == '"' && (i >= length || path[i] == '/' || path[i] == '['))
				{
					this.m_Position++;
					if (!this.SkipToValue())
					{
						return false;
					}
					if (i >= length)
					{
						return true;
					}
					if (path[i] == '/')
					{
						i++;
						if (!this.ParseToken('{'))
						{
							return false;
						}
					}
					else if (path[i] == '[')
					{
						i++;
						if (i == length)
						{
							throw new ArgumentException("Malformed JSON property path: " + path, "path");
						}
						if (path[i] != ']')
						{
							throw new NotImplementedException("Navigating to specific array element");
						}
						this.m_MatchAnyElementInArray = true;
						i++;
						if (i == length)
						{
							return true;
						}
					}
				}
				else
				{
					i = num;
					while (this.m_Position < this.m_Length && this.m_Text[this.m_Position] != '"')
					{
						this.m_Position++;
					}
					if (this.m_Position == this.m_Length || this.m_Text[this.m_Position] != '"')
					{
						return false;
					}
					this.m_Position++;
					if (!this.SkipToValue() || !this.ParseValue())
					{
						return false;
					}
					this.SkipWhitespace();
					if (this.m_Position == this.m_Length || this.m_Text[this.m_Position] == '}' || this.m_Text[this.m_Position] != ',')
					{
						return false;
					}
					this.m_Position++;
				}
			}
			return false;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00052A20 File Offset: 0x00050C20
		public bool CurrentPropertyHasValueEqualTo(JsonParser.JsonValue expectedValue)
		{
			int position = this.m_Position;
			this.m_DryRun = false;
			JsonParser.JsonValue jsonValue;
			if (!this.ParseValue(out jsonValue))
			{
				this.m_Position = position;
				return false;
			}
			this.m_Position = position;
			bool flag = false;
			if (jsonValue.type == JsonParser.JsonValueType.Array && this.m_MatchAnyElementInArray)
			{
				List<JsonParser.JsonValue> arrayValue = jsonValue.arrayValue;
				int num = 0;
				while (!flag)
				{
					if (num >= arrayValue.Count)
					{
						break;
					}
					flag = arrayValue[num] == expectedValue;
					num++;
				}
			}
			else
			{
				flag = jsonValue == expectedValue;
			}
			return flag;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00052AA4 File Offset: 0x00050CA4
		public bool ParseToken(char token)
		{
			this.SkipWhitespace();
			if (this.m_Position == this.m_Length)
			{
				return false;
			}
			if (this.m_Text[this.m_Position] != token)
			{
				return false;
			}
			this.m_Position++;
			this.SkipWhitespace();
			return this.m_Position < this.m_Length;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00052B00 File Offset: 0x00050D00
		public bool ParseValue()
		{
			JsonParser.JsonValue jsonValue;
			return this.ParseValue(out jsonValue);
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00052B18 File Offset: 0x00050D18
		public bool ParseValue(out JsonParser.JsonValue result)
		{
			result = default(JsonParser.JsonValue);
			this.SkipWhitespace();
			if (this.m_Position == this.m_Length)
			{
				return false;
			}
			char c = this.m_Text[this.m_Position];
			if (c <= 'f')
			{
				if (c != '"')
				{
					if (c != '[')
					{
						if (c != 'f')
						{
							goto IL_008D;
						}
					}
					else
					{
						if (this.ParseArrayValue(out result))
						{
							return true;
						}
						return false;
					}
				}
				else
				{
					if (this.ParseStringValue(out result))
					{
						return true;
					}
					return false;
				}
			}
			else if (c != 'n')
			{
				if (c != 't')
				{
					if (c != '{')
					{
						goto IL_008D;
					}
					if (this.ParseObjectValue(out result))
					{
						return true;
					}
					return false;
				}
			}
			else
			{
				if (this.ParseNullValue(out result))
				{
					return true;
				}
				return false;
			}
			if (this.ParseBooleanValue(out result))
			{
				return true;
			}
			return false;
			IL_008D:
			if (this.ParseNumber(out result))
			{
				return true;
			}
			return false;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00052BC0 File Offset: 0x00050DC0
		public bool ParseStringValue(out JsonParser.JsonValue result)
		{
			result = default(JsonParser.JsonValue);
			this.SkipWhitespace();
			if (this.m_Position == this.m_Length || this.m_Text[this.m_Position] != '"')
			{
				return false;
			}
			this.m_Position++;
			int position = this.m_Position;
			bool flag = false;
			while (this.m_Position < this.m_Length)
			{
				char c = this.m_Text[this.m_Position];
				if (c == '\\')
				{
					this.m_Position++;
					if (this.m_Position == this.m_Length)
					{
						break;
					}
					flag = true;
				}
				else if (c == '"')
				{
					this.m_Position++;
					result = new JsonParser.JsonString
					{
						text = new Substring(this.m_Text, position, this.m_Position - position - 1),
						hasEscapes = flag
					};
					return true;
				}
				this.m_Position++;
			}
			return false;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00052CC0 File Offset: 0x00050EC0
		public bool ParseArrayValue(out JsonParser.JsonValue result)
		{
			result = default(JsonParser.JsonValue);
			this.SkipWhitespace();
			if (this.m_Position == this.m_Length || this.m_Text[this.m_Position] != '[')
			{
				return false;
			}
			this.m_Position++;
			if (this.m_Position == this.m_Length)
			{
				return false;
			}
			if (this.m_Text[this.m_Position] == ']')
			{
				result = new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.Array
				};
				this.m_Position++;
				return true;
			}
			List<JsonParser.JsonValue> list = null;
			if (!this.m_DryRun)
			{
				list = new List<JsonParser.JsonValue>();
			}
			while (this.m_Position < this.m_Length)
			{
				JsonParser.JsonValue jsonValue;
				if (!this.ParseValue(out jsonValue))
				{
					return false;
				}
				if (!this.m_DryRun)
				{
					list.Add(jsonValue);
				}
				this.SkipWhitespace();
				if (this.m_Position == this.m_Length)
				{
					return false;
				}
				char c = this.m_Text[this.m_Position];
				if (c == ']')
				{
					this.m_Position++;
					if (!this.m_DryRun)
					{
						result = list;
					}
					return true;
				}
				if (c == ',')
				{
					this.m_Position++;
				}
			}
			return false;
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00052E00 File Offset: 0x00051000
		public bool ParseObjectValue(out JsonParser.JsonValue result)
		{
			result = default(JsonParser.JsonValue);
			if (!this.ParseToken('{'))
			{
				return false;
			}
			if (this.m_Position < this.m_Length && this.m_Text[this.m_Position] == '}')
			{
				result = new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.Object
				};
				this.m_Position++;
				return true;
			}
			while (this.m_Position < this.m_Length)
			{
				JsonParser.JsonValue jsonValue;
				if (!this.ParseStringValue(out jsonValue))
				{
					return false;
				}
				if (!this.SkipToValue())
				{
					return false;
				}
				JsonParser.JsonValue jsonValue2;
				if (!this.ParseValue(out jsonValue2))
				{
					return false;
				}
				if (!this.m_DryRun)
				{
					throw new NotImplementedException();
				}
				this.SkipWhitespace();
				if (this.m_Position < this.m_Length && this.m_Text[this.m_Position] == '}')
				{
					if (!this.m_DryRun)
					{
						throw new NotImplementedException();
					}
					this.m_Position++;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00052EFC File Offset: 0x000510FC
		public bool ParseNumber(out JsonParser.JsonValue result)
		{
			result = default(JsonParser.JsonValue);
			this.SkipWhitespace();
			if (this.m_Position == this.m_Length)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			long num = 0L;
			double num2 = 0.0;
			double num3 = 10.0;
			int num4 = 0;
			if (this.m_Text[this.m_Position] == '-')
			{
				flag = true;
				this.m_Position++;
			}
			if (this.m_Position == this.m_Length || !char.IsDigit(this.m_Text[this.m_Position]))
			{
				return false;
			}
			while (this.m_Position < this.m_Length)
			{
				char c = this.m_Text[this.m_Position];
				if (c == '.' || c < '0' || c > '9')
				{
					break;
				}
				num = num * 10L + (long)((ulong)c) - 48L;
				this.m_Position++;
			}
			if (this.m_Position < this.m_Length && this.m_Text[this.m_Position] == '.')
			{
				flag2 = true;
				this.m_Position++;
				if (this.m_Position == this.m_Length || !char.IsDigit(this.m_Text[this.m_Position]))
				{
					return false;
				}
				while (this.m_Position < this.m_Length)
				{
					char c2 = this.m_Text[this.m_Position];
					if (c2 < '0' || c2 > '9')
					{
						break;
					}
					num2 = (double)(c2 - '0') / num3 + num2;
					num3 *= 10.0;
					this.m_Position++;
				}
			}
			if (this.m_Position < this.m_Length && (this.m_Text[this.m_Position] == 'e' || this.m_Text[this.m_Position] == 'E'))
			{
				this.m_Position++;
				bool flag3 = false;
				if (this.m_Position < this.m_Length && this.m_Text[this.m_Position] == '-')
				{
					flag3 = true;
					this.m_Position++;
				}
				else if (this.m_Position < this.m_Length && this.m_Text[this.m_Position] == '+')
				{
					this.m_Position++;
				}
				int num5 = 1;
				while (this.m_Position < this.m_Length && char.IsDigit(this.m_Text[this.m_Position]))
				{
					int num6 = (int)(this.m_Text[this.m_Position] - '0');
					num4 *= num5;
					num4 += num6;
					num5 *= 10;
					this.m_Position++;
				}
				if (flag3)
				{
					num4 *= -1;
				}
			}
			if (!this.m_DryRun)
			{
				if (!flag2 && num4 == 0)
				{
					if (flag)
					{
						result = -num;
					}
					else
					{
						result = num;
					}
				}
				else
				{
					float num7;
					if (flag)
					{
						num7 = (float)(-(float)((double)num + num2));
					}
					else
					{
						num7 = (float)((double)num + num2);
					}
					if (num4 != 0)
					{
						num7 *= Mathf.Pow(10f, (float)num4);
					}
					result = (double)num7;
				}
			}
			return true;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00053224 File Offset: 0x00051424
		public bool ParseBooleanValue(out JsonParser.JsonValue result)
		{
			this.SkipWhitespace();
			if (this.SkipString("true"))
			{
				result = true;
				return true;
			}
			if (this.SkipString("false"))
			{
				result = false;
				return true;
			}
			result = default(JsonParser.JsonValue);
			return false;
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00053275 File Offset: 0x00051475
		public bool ParseNullValue(out JsonParser.JsonValue result)
		{
			result = default(JsonParser.JsonValue);
			return this.SkipString("null");
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0005328C File Offset: 0x0005148C
		public bool SkipToValue()
		{
			this.SkipWhitespace();
			if (this.m_Position == this.m_Length || this.m_Text[this.m_Position] != ':')
			{
				return false;
			}
			this.m_Position++;
			this.SkipWhitespace();
			return true;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x000532DC File Offset: 0x000514DC
		private bool SkipString(string text)
		{
			this.SkipWhitespace();
			int length = text.Length;
			if (this.m_Position + length >= this.m_Length)
			{
				return false;
			}
			for (int i = 0; i < length; i++)
			{
				if (this.m_Text[this.m_Position + i] != text[i])
				{
					return false;
				}
			}
			this.m_Position += length;
			return true;
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00053341 File Offset: 0x00051541
		private void SkipWhitespace()
		{
			while (this.m_Position < this.m_Length && char.IsWhiteSpace(this.m_Text[this.m_Position]))
			{
				this.m_Position++;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x00053379 File Offset: 0x00051579
		public bool isAtEnd
		{
			get
			{
				return this.m_Position >= this.m_Length;
			}
		}

		// Token: 0x04000710 RID: 1808
		private readonly string m_Text;

		// Token: 0x04000711 RID: 1809
		private readonly int m_Length;

		// Token: 0x04000712 RID: 1810
		private int m_Position;

		// Token: 0x04000713 RID: 1811
		private bool m_MatchAnyElementInArray;

		// Token: 0x04000714 RID: 1812
		private bool m_DryRun;

		// Token: 0x02000246 RID: 582
		public enum JsonValueType
		{
			// Token: 0x04000C71 RID: 3185
			None,
			// Token: 0x04000C72 RID: 3186
			Bool,
			// Token: 0x04000C73 RID: 3187
			Real,
			// Token: 0x04000C74 RID: 3188
			Integer,
			// Token: 0x04000C75 RID: 3189
			String,
			// Token: 0x04000C76 RID: 3190
			Array,
			// Token: 0x04000C77 RID: 3191
			Object,
			// Token: 0x04000C78 RID: 3192
			Any
		}

		// Token: 0x02000247 RID: 583
		public struct JsonString : IEquatable<JsonParser.JsonString>
		{
			// Token: 0x06001620 RID: 5664 RVA: 0x00065C8C File Offset: 0x00063E8C
			public override string ToString()
			{
				if (!this.hasEscapes)
				{
					return this.text.ToString();
				}
				StringBuilder stringBuilder = new StringBuilder();
				int length = this.text.length;
				for (int i = 0; i < length; i++)
				{
					char c = this.text[i];
					if (c == '\\')
					{
						i++;
						if (i == length)
						{
							break;
						}
						c = this.text[i];
					}
					stringBuilder.Append(c);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06001621 RID: 5665 RVA: 0x00065D08 File Offset: 0x00063F08
			public bool Equals(JsonParser.JsonString other)
			{
				if (this.hasEscapes == other.hasEscapes)
				{
					return Substring.Compare(this.text, other.text, StringComparison.InvariantCultureIgnoreCase) == 0;
				}
				int length = this.text.length;
				int length2 = other.text.length;
				int num = 0;
				int num2 = 0;
				while (num < length && num2 < length2)
				{
					char c = this.text[num];
					char c2 = other.text[num2];
					if (c == '\\')
					{
						num++;
						if (num == length)
						{
							return false;
						}
						c = this.text[num];
					}
					if (c2 == '\\')
					{
						num2++;
						if (num2 == length2)
						{
							return false;
						}
						c2 = other.text[num2];
					}
					if (char.ToUpperInvariant(c) != char.ToUpperInvariant(c2))
					{
						return false;
					}
					num++;
					num2++;
				}
				return num == length && num2 == length2;
			}

			// Token: 0x06001622 RID: 5666 RVA: 0x00065DE0 File Offset: 0x00063FE0
			public override bool Equals(object obj)
			{
				if (obj is JsonParser.JsonString)
				{
					JsonParser.JsonString jsonString = (JsonParser.JsonString)obj;
					return this.Equals(jsonString);
				}
				return false;
			}

			// Token: 0x06001623 RID: 5667 RVA: 0x00065E05 File Offset: 0x00064005
			public override int GetHashCode()
			{
				return (this.text.GetHashCode() * 397) ^ this.hasEscapes.GetHashCode();
			}

			// Token: 0x06001624 RID: 5668 RVA: 0x00065E2A File Offset: 0x0006402A
			public static bool operator ==(JsonParser.JsonString left, JsonParser.JsonString right)
			{
				return left.Equals(right);
			}

			// Token: 0x06001625 RID: 5669 RVA: 0x00065E34 File Offset: 0x00064034
			public static bool operator !=(JsonParser.JsonString left, JsonParser.JsonString right)
			{
				return !left.Equals(right);
			}

			// Token: 0x06001626 RID: 5670 RVA: 0x00065E44 File Offset: 0x00064044
			public static implicit operator JsonParser.JsonString(string str)
			{
				return new JsonParser.JsonString
				{
					text = str
				};
			}

			// Token: 0x04000C79 RID: 3193
			public Substring text;

			// Token: 0x04000C7A RID: 3194
			public bool hasEscapes;
		}

		// Token: 0x02000248 RID: 584
		public struct JsonValue : IEquatable<JsonParser.JsonValue>
		{
			// Token: 0x06001627 RID: 5671 RVA: 0x00065E68 File Offset: 0x00064068
			public bool ToBoolean()
			{
				switch (this.type)
				{
				case JsonParser.JsonValueType.Bool:
					return this.boolValue;
				case JsonParser.JsonValueType.Real:
					return NumberHelpers.Approximately(0.0, this.realValue);
				case JsonParser.JsonValueType.Integer:
					return this.integerValue != 0L;
				case JsonParser.JsonValueType.String:
					return Convert.ToBoolean(this.ToString());
				default:
					return false;
				}
			}

			// Token: 0x06001628 RID: 5672 RVA: 0x00065ED0 File Offset: 0x000640D0
			public long ToInteger()
			{
				switch (this.type)
				{
				case JsonParser.JsonValueType.Bool:
					return this.boolValue ? 1L : 0L;
				case JsonParser.JsonValueType.Real:
					return (long)this.realValue;
				case JsonParser.JsonValueType.Integer:
					return this.integerValue;
				case JsonParser.JsonValueType.String:
					return Convert.ToInt64(this.ToString());
				default:
					return 0L;
				}
			}

			// Token: 0x06001629 RID: 5673 RVA: 0x00065F30 File Offset: 0x00064130
			public double ToDouble()
			{
				switch (this.type)
				{
				case JsonParser.JsonValueType.Bool:
					return (double)(this.boolValue ? 1 : 0);
				case JsonParser.JsonValueType.Real:
					return this.realValue;
				case JsonParser.JsonValueType.Integer:
					return (double)this.integerValue;
				case JsonParser.JsonValueType.String:
					return (double)Convert.ToSingle(this.ToString());
				default:
					return 0.0;
				}
			}

			// Token: 0x0600162A RID: 5674 RVA: 0x00065F98 File Offset: 0x00064198
			public override string ToString()
			{
				switch (this.type)
				{
				case JsonParser.JsonValueType.None:
					return "null";
				case JsonParser.JsonValueType.Bool:
					return this.boolValue.ToString();
				case JsonParser.JsonValueType.Real:
					return this.realValue.ToString(CultureInfo.InvariantCulture);
				case JsonParser.JsonValueType.Integer:
					return this.integerValue.ToString(CultureInfo.InvariantCulture);
				case JsonParser.JsonValueType.String:
					return this.stringValue.ToString();
				case JsonParser.JsonValueType.Array:
					if (this.arrayValue == null)
					{
						return "[]";
					}
					return "[" + string.Join(",", this.arrayValue.Select<JsonParser.JsonValue, string>((JsonParser.JsonValue x) => x.ToString())) + "]";
				case JsonParser.JsonValueType.Object:
				{
					if (this.objectValue == null)
					{
						return "{}";
					}
					IEnumerable<string> enumerable = this.objectValue.Select<KeyValuePair<string, JsonParser.JsonValue>, string>((KeyValuePair<string, JsonParser.JsonValue> pair) => string.Format("\"{0}\" : \"{1}\"", pair.Key, pair.Value));
					return "{" + string.Join(",", enumerable) + "}";
				}
				case JsonParser.JsonValueType.Any:
					return this.anyValue.ToString();
				default:
					return base.ToString();
				}
			}

			// Token: 0x0600162B RID: 5675 RVA: 0x000660E0 File Offset: 0x000642E0
			public static implicit operator JsonParser.JsonValue(bool val)
			{
				return new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.Bool,
					boolValue = val
				};
			}

			// Token: 0x0600162C RID: 5676 RVA: 0x00066108 File Offset: 0x00064308
			public static implicit operator JsonParser.JsonValue(long val)
			{
				return new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.Integer,
					integerValue = val
				};
			}

			// Token: 0x0600162D RID: 5677 RVA: 0x00066130 File Offset: 0x00064330
			public static implicit operator JsonParser.JsonValue(double val)
			{
				return new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.Real,
					realValue = val
				};
			}

			// Token: 0x0600162E RID: 5678 RVA: 0x00066158 File Offset: 0x00064358
			public static implicit operator JsonParser.JsonValue(string str)
			{
				return new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.String,
					stringValue = new JsonParser.JsonString
					{
						text = str
					}
				};
			}

			// Token: 0x0600162F RID: 5679 RVA: 0x00066194 File Offset: 0x00064394
			public static implicit operator JsonParser.JsonValue(JsonParser.JsonString str)
			{
				return new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.String,
					stringValue = str
				};
			}

			// Token: 0x06001630 RID: 5680 RVA: 0x000661BC File Offset: 0x000643BC
			public static implicit operator JsonParser.JsonValue(List<JsonParser.JsonValue> array)
			{
				return new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.Array,
					arrayValue = array
				};
			}

			// Token: 0x06001631 RID: 5681 RVA: 0x000661E4 File Offset: 0x000643E4
			public static implicit operator JsonParser.JsonValue(Dictionary<string, JsonParser.JsonValue> obj)
			{
				return new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.Object,
					objectValue = obj
				};
			}

			// Token: 0x06001632 RID: 5682 RVA: 0x0006620C File Offset: 0x0006440C
			public static implicit operator JsonParser.JsonValue(Enum val)
			{
				return new JsonParser.JsonValue
				{
					type = JsonParser.JsonValueType.Any,
					anyValue = val
				};
			}

			// Token: 0x06001633 RID: 5683 RVA: 0x00066234 File Offset: 0x00064434
			public bool Equals(JsonParser.JsonValue other)
			{
				if (this.type == other.type)
				{
					switch (this.type)
					{
					case JsonParser.JsonValueType.None:
						return true;
					case JsonParser.JsonValueType.Bool:
						return this.boolValue == other.boolValue;
					case JsonParser.JsonValueType.Real:
						return NumberHelpers.Approximately(this.realValue, other.realValue);
					case JsonParser.JsonValueType.Integer:
						return this.integerValue == other.integerValue;
					case JsonParser.JsonValueType.String:
						return this.stringValue == other.stringValue;
					case JsonParser.JsonValueType.Array:
						throw new NotImplementedException();
					case JsonParser.JsonValueType.Object:
						throw new NotImplementedException();
					case JsonParser.JsonValueType.Any:
						return this.anyValue.Equals(other.anyValue);
					default:
						return false;
					}
				}
				else
				{
					if (this.anyValue != null)
					{
						return JsonParser.JsonValue.Equals(this.anyValue, other);
					}
					return other.anyValue != null && JsonParser.JsonValue.Equals(other.anyValue, this);
				}
			}

			// Token: 0x06001634 RID: 5684 RVA: 0x00066318 File Offset: 0x00064518
			private static bool Equals(object obj, JsonParser.JsonValue value)
			{
				if (obj == null)
				{
					return false;
				}
				Regex regex = obj as Regex;
				if (regex != null)
				{
					return regex.IsMatch(value.ToString());
				}
				string text = obj as string;
				if (text != null)
				{
					switch (value.type)
					{
					case JsonParser.JsonValueType.Bool:
						if (value.boolValue)
						{
							return text == "True" || text == "true" || text == "1";
						}
						return text == "False" || text == "false" || text == "0";
					case JsonParser.JsonValueType.Real:
					{
						double num;
						return double.TryParse(text, out num) && NumberHelpers.Approximately(num, value.realValue);
					}
					case JsonParser.JsonValueType.Integer:
					{
						long num2;
						return long.TryParse(text, out num2) && num2 == value.integerValue;
					}
					case JsonParser.JsonValueType.String:
						return value.stringValue == text;
					}
				}
				if (obj is float)
				{
					float num3 = (float)obj;
					if (value.type == JsonParser.JsonValueType.Real)
					{
						return NumberHelpers.Approximately((double)num3, value.realValue);
					}
					if (value.type == JsonParser.JsonValueType.String)
					{
						float num4;
						return float.TryParse(value.ToString(), out num4) && Mathf.Approximately(num3, num4);
					}
				}
				if (obj is double)
				{
					double num5 = (double)obj;
					if (value.type == JsonParser.JsonValueType.Real)
					{
						return NumberHelpers.Approximately(num5, value.realValue);
					}
					if (value.type == JsonParser.JsonValueType.String)
					{
						double num6;
						return double.TryParse(value.ToString(), out num6) && NumberHelpers.Approximately(num5, num6);
					}
				}
				if (obj is int)
				{
					int num7 = (int)obj;
					if (value.type == JsonParser.JsonValueType.Integer)
					{
						return (long)num7 == value.integerValue;
					}
					if (value.type == JsonParser.JsonValueType.String)
					{
						int num8;
						return int.TryParse(value.ToString(), out num8) && num7 == num8;
					}
				}
				if (obj is long)
				{
					long num9 = (long)obj;
					if (value.type == JsonParser.JsonValueType.Integer)
					{
						return num9 == value.integerValue;
					}
					if (value.type == JsonParser.JsonValueType.String)
					{
						long num10;
						return long.TryParse(value.ToString(), out num10) && num9 == num10;
					}
				}
				if (obj is bool)
				{
					bool flag = (bool)obj;
					if (value.type == JsonParser.JsonValueType.Bool)
					{
						return flag == value.boolValue;
					}
					if (value.type == JsonParser.JsonValueType.String)
					{
						if (flag)
						{
							return value.stringValue == "true" || value.stringValue == "True" || value.stringValue == "1";
						}
						return value.stringValue == "false" || value.stringValue == "False" || value.stringValue == "0";
					}
				}
				if (obj is Enum)
				{
					if (value.type == JsonParser.JsonValueType.Integer)
					{
						return Convert.ToInt64(obj) == value.integerValue;
					}
					if (value.type == JsonParser.JsonValueType.String)
					{
						return value.stringValue == Enum.GetName(obj.GetType(), obj);
					}
				}
				return false;
			}

			// Token: 0x06001635 RID: 5685 RVA: 0x00066658 File Offset: 0x00064858
			public override bool Equals(object obj)
			{
				if (obj is JsonParser.JsonValue)
				{
					JsonParser.JsonValue jsonValue = (JsonParser.JsonValue)obj;
					return this.Equals(jsonValue);
				}
				return false;
			}

			// Token: 0x06001636 RID: 5686 RVA: 0x00066680 File Offset: 0x00064880
			public override int GetHashCode()
			{
				return (int)((((((((((((((this.type * (JsonParser.JsonValueType)397) ^ (JsonParser.JsonValueType)this.boolValue.GetHashCode()) * (JsonParser.JsonValueType)397) ^ (JsonParser.JsonValueType)this.realValue.GetHashCode()) * (JsonParser.JsonValueType)397) ^ (JsonParser.JsonValueType)this.integerValue.GetHashCode()) * (JsonParser.JsonValueType)397) ^ (JsonParser.JsonValueType)this.stringValue.GetHashCode()) * (JsonParser.JsonValueType)397) ^ (JsonParser.JsonValueType)((this.arrayValue != null) ? this.arrayValue.GetHashCode() : 0)) * (JsonParser.JsonValueType)397) ^ (JsonParser.JsonValueType)((this.objectValue != null) ? this.objectValue.GetHashCode() : 0)) * (JsonParser.JsonValueType)397) ^ (JsonParser.JsonValueType)((this.anyValue != null) ? this.anyValue.GetHashCode() : 0));
			}

			// Token: 0x06001637 RID: 5687 RVA: 0x00066738 File Offset: 0x00064938
			public static bool operator ==(JsonParser.JsonValue left, JsonParser.JsonValue right)
			{
				return left.Equals(right);
			}

			// Token: 0x06001638 RID: 5688 RVA: 0x00066742 File Offset: 0x00064942
			public static bool operator !=(JsonParser.JsonValue left, JsonParser.JsonValue right)
			{
				return !left.Equals(right);
			}

			// Token: 0x04000C7B RID: 3195
			public JsonParser.JsonValueType type;

			// Token: 0x04000C7C RID: 3196
			public bool boolValue;

			// Token: 0x04000C7D RID: 3197
			public double realValue;

			// Token: 0x04000C7E RID: 3198
			public long integerValue;

			// Token: 0x04000C7F RID: 3199
			public JsonParser.JsonString stringValue;

			// Token: 0x04000C80 RID: 3200
			public List<JsonParser.JsonValue> arrayValue;

			// Token: 0x04000C81 RID: 3201
			public Dictionary<string, JsonParser.JsonValue> objectValue;

			// Token: 0x04000C82 RID: 3202
			public object anyValue;
		}
	}
}
