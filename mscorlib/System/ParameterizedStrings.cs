using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
	// Token: 0x0200024F RID: 591
	internal static class ParameterizedStrings
	{
		// Token: 0x06001B84 RID: 7044 RVA: 0x00066B70 File Offset: 0x00064D70
		public static string Evaluate(string format, params ParameterizedStrings.FormatParam[] args)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			ParameterizedStrings.LowLevelStack lowLevelStack = ParameterizedStrings._cachedStack;
			if (lowLevelStack == null)
			{
				lowLevelStack = (ParameterizedStrings._cachedStack = new ParameterizedStrings.LowLevelStack());
			}
			else
			{
				lowLevelStack.Clear();
			}
			ParameterizedStrings.FormatParam[] array = null;
			ParameterizedStrings.FormatParam[] array2 = null;
			int num = 0;
			return ParameterizedStrings.EvaluateInternal(format, ref num, args, lowLevelStack, ref array, ref array2);
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x00066BCC File Offset: 0x00064DCC
		private static string EvaluateInternal(string format, ref int pos, ParameterizedStrings.FormatParam[] args, ParameterizedStrings.LowLevelStack stack, ref ParameterizedStrings.FormatParam[] dynamicVars, ref ParameterizedStrings.FormatParam[] staticVars)
		{
			StringBuilder stringBuilder = new StringBuilder(format.Length);
			bool flag = false;
			while (pos < format.Length)
			{
				if (format[pos] == '%')
				{
					pos++;
					char c = format[pos];
					if (c <= 'X')
					{
						switch (c)
						{
						case '!':
							goto IL_0533;
						case '"':
						case '#':
						case '$':
						case '(':
						case ')':
						case ',':
						case '.':
						case '@':
						case 'B':
						case 'C':
						case 'D':
						case 'E':
						case 'F':
						case 'G':
						case 'H':
						case 'I':
						case 'J':
						case 'K':
						case 'L':
						case 'M':
						case 'N':
							goto IL_0682;
						case '%':
							stringBuilder.Append('%');
							goto IL_068D;
						case '&':
						case '*':
						case '+':
						case '-':
						case '/':
						case '<':
						case '=':
						case '>':
						case 'A':
						case 'O':
							goto IL_03B3;
						case '\'':
							stack.Push((int)format[pos + 1]);
							pos += 2;
							goto IL_068D;
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
						case ':':
							break;
						case ';':
							goto IL_065F;
						case '?':
							flag = true;
							goto IL_068D;
						case 'P':
						{
							pos++;
							int num;
							ParameterizedStrings.GetDynamicOrStaticVariables(format[pos], ref dynamicVars, ref staticVars, out num)[num] = stack.Pop();
							goto IL_068D;
						}
						default:
							if (c != 'X')
							{
								goto IL_0682;
							}
							break;
						}
					}
					else
					{
						switch (c)
						{
						case '^':
						case 'm':
							goto IL_03B3;
						case '_':
						case '`':
						case 'a':
						case 'b':
						case 'f':
						case 'h':
						case 'j':
						case 'k':
						case 'n':
						case 'q':
						case 'r':
							goto IL_0682;
						case 'c':
							stringBuilder.Append((char)stack.Pop().Int32);
							goto IL_068D;
						case 'd':
							stringBuilder.Append(stack.Pop().Int32);
							goto IL_068D;
						case 'e':
							goto IL_065F;
						case 'g':
						{
							pos++;
							int num2;
							ParameterizedStrings.FormatParam[] dynamicOrStaticVariables = ParameterizedStrings.GetDynamicOrStaticVariables(format[pos], ref dynamicVars, ref staticVars, out num2);
							stack.Push(dynamicOrStaticVariables[num2]);
							goto IL_068D;
						}
						case 'i':
							args[0] = 1 + args[0].Int32;
							args[1] = 1 + args[1].Int32;
							goto IL_068D;
						case 'l':
							stack.Push(stack.Pop().String.Length);
							goto IL_068D;
						case 'o':
							break;
						case 'p':
							pos++;
							stack.Push(args[(int)(format[pos] - '1')]);
							goto IL_068D;
						case 's':
							stringBuilder.Append(stack.Pop().String);
							goto IL_068D;
						case 't':
						{
							bool flag2 = ParameterizedStrings.AsBool(stack.Pop().Int32);
							pos++;
							string text = ParameterizedStrings.EvaluateInternal(format, ref pos, args, stack, ref dynamicVars, ref staticVars);
							if (flag2)
							{
								stringBuilder.Append(text);
							}
							if (!ParameterizedStrings.AsBool(stack.Pop().Int32))
							{
								pos++;
								string text2 = ParameterizedStrings.EvaluateInternal(format, ref pos, args, stack, ref dynamicVars, ref staticVars);
								if (!flag2)
								{
									stringBuilder.Append(text2);
								}
								if (!ParameterizedStrings.AsBool(stack.Pop().Int32))
								{
									throw new InvalidOperationException("Terminfo database contains invalid values");
								}
							}
							if (!flag)
							{
								stack.Push(1);
								return stringBuilder.ToString();
							}
							flag = false;
							goto IL_068D;
						}
						default:
							switch (c)
							{
							case 'x':
								break;
							case 'y':
							case 'z':
							case '}':
								goto IL_0682;
							case '{':
							{
								pos++;
								int num3 = 0;
								while (format[pos] != '}')
								{
									num3 = num3 * 10 + (int)(format[pos] - '0');
									pos++;
								}
								stack.Push(num3);
								goto IL_068D;
							}
							case '|':
								goto IL_03B3;
							case '~':
								goto IL_0533;
							default:
								goto IL_0682;
							}
							break;
						}
					}
					int i;
					for (i = pos; i < format.Length; i++)
					{
						char c2 = format[i];
						if (c2 == 'd' || c2 == 'o' || c2 == 'x' || c2 == 'X' || c2 == 's')
						{
							break;
						}
					}
					if (i >= format.Length)
					{
						throw new InvalidOperationException("Terminfo database contains invalid values");
					}
					string text3 = format.Substring(pos - 1, i - pos + 2);
					if (text3.Length > 1 && text3[1] == ':')
					{
						text3 = text3.Remove(1, 1);
					}
					stringBuilder.Append(ParameterizedStrings.FormatPrintF(text3, stack.Pop().Object));
					goto IL_068D;
					IL_03B3:
					int @int = stack.Pop().Int32;
					int int2 = stack.Pop().Int32;
					char c3 = format[pos];
					int num4;
					if (c3 <= 'A')
					{
						if (c3 != '&')
						{
							switch (c3)
							{
							case '*':
								num4 = int2 * @int;
								break;
							case '+':
								num4 = int2 + @int;
								break;
							case ',':
							case '.':
								goto IL_051E;
							case '-':
								num4 = int2 - @int;
								break;
							case '/':
								num4 = int2 / @int;
								break;
							default:
								switch (c3)
								{
								case '<':
									num4 = ParameterizedStrings.AsInt(int2 < @int);
									break;
								case '=':
									num4 = ParameterizedStrings.AsInt(int2 == @int);
									break;
								case '>':
									num4 = ParameterizedStrings.AsInt(int2 > @int);
									break;
								case '?':
								case '@':
									goto IL_051E;
								case 'A':
									num4 = ParameterizedStrings.AsInt(ParameterizedStrings.AsBool(int2) && ParameterizedStrings.AsBool(@int));
									break;
								default:
									goto IL_051E;
								}
								break;
							}
						}
						else
						{
							num4 = int2 & @int;
						}
					}
					else if (c3 <= '^')
					{
						if (c3 != 'O')
						{
							if (c3 != '^')
							{
								goto IL_051E;
							}
							num4 = int2 ^ @int;
						}
						else
						{
							num4 = ParameterizedStrings.AsInt(ParameterizedStrings.AsBool(int2) || ParameterizedStrings.AsBool(@int));
						}
					}
					else if (c3 != 'm')
					{
						if (c3 != '|')
						{
							goto IL_051E;
						}
						num4 = int2 | @int;
					}
					else
					{
						num4 = int2 % @int;
					}
					IL_0521:
					stack.Push(num4);
					goto IL_068D;
					IL_051E:
					num4 = 0;
					goto IL_0521;
					IL_0533:
					int int3 = stack.Pop().Int32;
					stack.Push((format[pos] == '!') ? ParameterizedStrings.AsInt(!ParameterizedStrings.AsBool(int3)) : (~int3));
					goto IL_068D;
					IL_065F:
					stack.Push(ParameterizedStrings.AsInt(format[pos] == ';'));
					return stringBuilder.ToString();
					IL_0682:
					throw new InvalidOperationException("Terminfo database contains invalid values");
				}
				stringBuilder.Append(format[pos]);
				IL_068D:
				pos++;
			}
			stack.Push(1);
			return stringBuilder.ToString();
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x00023B87 File Offset: 0x00021D87
		private static bool AsBool(int i)
		{
			return i != 0;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x00023D0A File Offset: 0x00021F0A
		private static int AsInt(bool b)
		{
			if (!b)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x0006728C File Offset: 0x0006548C
		private static string StringFromAsciiBytes(byte[] buffer, int offset, int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			char[] array = new char[length];
			int i = 0;
			int num = offset;
			while (i < length)
			{
				array[i] = (char)buffer[num];
				i++;
				num++;
			}
			return new string(array);
		}

		// Token: 0x06001B89 RID: 7049
		[DllImport("libc")]
		private unsafe static extern int snprintf(byte* str, IntPtr size, string format, string arg1);

		// Token: 0x06001B8A RID: 7050
		[DllImport("libc")]
		private unsafe static extern int snprintf(byte* str, IntPtr size, string format, int arg1);

		// Token: 0x06001B8B RID: 7051 RVA: 0x000672C8 File Offset: 0x000654C8
		private unsafe static string FormatPrintF(string format, object arg)
		{
			string text = arg as string;
			int num = ((text != null) ? ParameterizedStrings.snprintf(null, IntPtr.Zero, format, text) : ParameterizedStrings.snprintf(null, IntPtr.Zero, format, (int)arg));
			if (num == 0)
			{
				return string.Empty;
			}
			if (num < 0)
			{
				throw new InvalidOperationException("The printf operation failed");
			}
			byte[] array = new byte[num + 1];
			byte[] array2;
			byte* ptr;
			if ((array2 = array) == null || array2.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array2[0];
			}
			if (((text != null) ? ParameterizedStrings.snprintf(ptr, (IntPtr)array.Length, format, text) : ParameterizedStrings.snprintf(ptr, (IntPtr)array.Length, format, (int)arg)) != num)
			{
				throw new InvalidOperationException("Invalid printf operation");
			}
			array2 = null;
			return ParameterizedStrings.StringFromAsciiBytes(array, 0, num);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x00067384 File Offset: 0x00065584
		private static ParameterizedStrings.FormatParam[] GetDynamicOrStaticVariables(char c, ref ParameterizedStrings.FormatParam[] dynamicVars, ref ParameterizedStrings.FormatParam[] staticVars, out int index)
		{
			if (c >= 'A' && c <= 'Z')
			{
				index = (int)(c - 'A');
				ParameterizedStrings.FormatParam[] array;
				if ((array = staticVars) == null)
				{
					ParameterizedStrings.FormatParam[] array2;
					staticVars = (array2 = new ParameterizedStrings.FormatParam[26]);
					array = array2;
				}
				return array;
			}
			if (c >= 'a' && c <= 'z')
			{
				index = (int)(c - 'a');
				ParameterizedStrings.FormatParam[] array3;
				if ((array3 = dynamicVars) == null)
				{
					ParameterizedStrings.FormatParam[] array2;
					dynamicVars = (array2 = new ParameterizedStrings.FormatParam[26]);
					array3 = array2;
				}
				return array3;
			}
			throw new InvalidOperationException("Terminfo database contains invalid values");
		}

		// Token: 0x040017D0 RID: 6096
		[ThreadStatic]
		private static ParameterizedStrings.LowLevelStack _cachedStack;

		// Token: 0x02000250 RID: 592
		public struct FormatParam
		{
			// Token: 0x06001B8D RID: 7053 RVA: 0x000673E1 File Offset: 0x000655E1
			public FormatParam(int value)
			{
				this = new ParameterizedStrings.FormatParam(value, null);
			}

			// Token: 0x06001B8E RID: 7054 RVA: 0x000673EB File Offset: 0x000655EB
			public FormatParam(string value)
			{
				this = new ParameterizedStrings.FormatParam(0, value ?? string.Empty);
			}

			// Token: 0x06001B8F RID: 7055 RVA: 0x000673FE File Offset: 0x000655FE
			private FormatParam(int intValue, string stringValue)
			{
				this._int32 = intValue;
				this._string = stringValue;
			}

			// Token: 0x06001B90 RID: 7056 RVA: 0x0006740E File Offset: 0x0006560E
			public static implicit operator ParameterizedStrings.FormatParam(int value)
			{
				return new ParameterizedStrings.FormatParam(value);
			}

			// Token: 0x06001B91 RID: 7057 RVA: 0x00067416 File Offset: 0x00065616
			public static implicit operator ParameterizedStrings.FormatParam(string value)
			{
				return new ParameterizedStrings.FormatParam(value);
			}

			// Token: 0x17000328 RID: 808
			// (get) Token: 0x06001B92 RID: 7058 RVA: 0x0006741E File Offset: 0x0006561E
			public int Int32
			{
				get
				{
					return this._int32;
				}
			}

			// Token: 0x17000329 RID: 809
			// (get) Token: 0x06001B93 RID: 7059 RVA: 0x00067426 File Offset: 0x00065626
			public string String
			{
				get
				{
					return this._string ?? string.Empty;
				}
			}

			// Token: 0x1700032A RID: 810
			// (get) Token: 0x06001B94 RID: 7060 RVA: 0x00067437 File Offset: 0x00065637
			public object Object
			{
				get
				{
					return this._string ?? this._int32;
				}
			}

			// Token: 0x040017D1 RID: 6097
			private readonly int _int32;

			// Token: 0x040017D2 RID: 6098
			private readonly string _string;
		}

		// Token: 0x02000251 RID: 593
		private sealed class LowLevelStack
		{
			// Token: 0x06001B95 RID: 7061 RVA: 0x0006744E File Offset: 0x0006564E
			public LowLevelStack()
			{
				this._arr = new ParameterizedStrings.FormatParam[4];
			}

			// Token: 0x06001B96 RID: 7062 RVA: 0x00067464 File Offset: 0x00065664
			public ParameterizedStrings.FormatParam Pop()
			{
				if (this._count == 0)
				{
					throw new InvalidOperationException("Terminfo: Invalid Stack");
				}
				ParameterizedStrings.FormatParam[] arr = this._arr;
				int num = this._count - 1;
				this._count = num;
				ParameterizedStrings.FormatParam formatParam = arr[num];
				this._arr[this._count] = default(ParameterizedStrings.FormatParam);
				return formatParam;
			}

			// Token: 0x06001B97 RID: 7063 RVA: 0x000674B8 File Offset: 0x000656B8
			public void Push(ParameterizedStrings.FormatParam item)
			{
				if (this._arr.Length == this._count)
				{
					ParameterizedStrings.FormatParam[] array = new ParameterizedStrings.FormatParam[this._arr.Length * 2];
					Array.Copy(this._arr, 0, array, 0, this._arr.Length);
					this._arr = array;
				}
				ParameterizedStrings.FormatParam[] arr = this._arr;
				int count = this._count;
				this._count = count + 1;
				arr[count] = item;
			}

			// Token: 0x06001B98 RID: 7064 RVA: 0x0006751F File Offset: 0x0006571F
			public void Clear()
			{
				Array.Clear(this._arr, 0, this._count);
				this._count = 0;
			}

			// Token: 0x040017D3 RID: 6099
			private const int DefaultSize = 4;

			// Token: 0x040017D4 RID: 6100
			private ParameterizedStrings.FormatParam[] _arr;

			// Token: 0x040017D5 RID: 6101
			private int _count;
		}
	}
}
