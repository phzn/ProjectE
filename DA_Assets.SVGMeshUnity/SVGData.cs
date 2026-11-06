using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DA_Assets.SVGMeshUnity.Internals;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity
{
	// Token: 0x02000003 RID: 3
	public class SVGData
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public void Clear()
		{
			this.Curves.Clear();
			this.Start = Vector2.zero;
			this.Current = Vector2.zero;
			this.Bezier = null;
			this.Quad = null;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020FB File Offset: 0x000002FB
		public void Move(float x, float y)
		{
			this.Move(new Vector2(x, y));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000210A File Offset: 0x0000030A
		public void MoveRelative(float x, float y)
		{
			this.Move(this.Current.x + x, this.Current.y + y);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000212C File Offset: 0x0000032C
		public void Move(Vector2 v)
		{
			this.Curves.Add(new Curve
			{
				IsMove = true,
				Position = v
			});
			this.Current = v;
			this.Bezier = null;
			this.Quad = null;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000217C File Offset: 0x0000037C
		public void MoveRelative(Vector2 v)
		{
			this.Move(this.Current + v);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002190 File Offset: 0x00000390
		public void Curve(float inX, float inY, float outX, float outY, float x, float y)
		{
			this.Curve(new Vector2(inX, inY), new Vector2(outX, outY), new Vector2(x, y));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021B0 File Offset: 0x000003B0
		public void Curve(Vector2 inControl, Vector2 outControl, Vector2 v)
		{
			this.CurveInternal(inControl, outControl, v);
			this.Bezier = new Vector2?(outControl);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C8 File Offset: 0x000003C8
		private void CurveInternal(Vector2 inControl, Vector2 outControl, Vector2 v)
		{
			this.Curves.Add(new Curve
			{
				Position = v,
				InControl = inControl,
				OutControl = outControl
			});
			this.Current = v;
			this.Bezier = null;
			this.Quad = null;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002220 File Offset: 0x00000420
		public void CurveRelative(float inX, float inY, float outX, float outY, float x, float y)
		{
			this.CurveRelative(new Vector2(inX, inY), new Vector2(outX, outY), new Vector2(x, y));
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002240 File Offset: 0x00000440
		public void CurveRelative(Vector2 inControl, Vector2 outControl, Vector2 v)
		{
			this.Curve(this.Current + inControl, this.Current + outControl, this.Current + v);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000226C File Offset: 0x0000046C
		public void CurveSmooth(float controlX, float controlY, float x, float y)
		{
			this.CurveSmooth(new Vector2(controlX, controlY), new Vector2(x, y));
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002283 File Offset: 0x00000483
		public void CurveSmooth(Vector2 control, Vector2 v)
		{
			this.Curve((this.Bezier != null) ? (this.Current * 2f - this.Bezier.Value) : this.Current, control, v);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022C2 File Offset: 0x000004C2
		public void CurveSmoothRelative(float controlX, float controlY, float x, float y)
		{
			this.CurveSmoothRelative(new Vector2(controlX, controlY), new Vector2(x, y));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022D9 File Offset: 0x000004D9
		public void CurveSmoothRelative(Vector2 control, Vector2 v)
		{
			this.CurveSmooth(this.Current + control, this.Current + v);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022F9 File Offset: 0x000004F9
		public void Quadratic(float controlX, float controlY, float x, float y)
		{
			this.Quadratic(new Vector2(controlX, controlY), new Vector2(x, y));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002310 File Offset: 0x00000510
		public void Quadratic(Vector2 control, Vector2 v)
		{
			this.CurveInternal(this.Current / 3f + 0.6666667f * control, v / 3f + 0.6666667f * control, v);
			this.Quad = new Vector2?(control);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000236B File Offset: 0x0000056B
		public void QuadraticRelative(float controlX, float controlY, float x, float y)
		{
			this.QuadraticRelative(new Vector2(controlX, controlY), new Vector2(x, y));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002382 File Offset: 0x00000582
		public void QuadraticRelative(Vector2 control, Vector2 v)
		{
			this.Quadratic(this.Current + control, this.Current + v);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023A2 File Offset: 0x000005A2
		public void QuadraticSmooth(float x, float y)
		{
			this.QuadraticSmooth(new Vector2(x, y));
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023B1 File Offset: 0x000005B1
		public void QuadraticSmooth(Vector2 v)
		{
			this.Quadratic((this.Quad != null) ? (this.Current * 2f - this.Quad.Value) : this.Current, v);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023EF File Offset: 0x000005EF
		public void QuadraticSmoothRelative(float x, float y)
		{
			this.QuadraticSmooth(this.Current.x + x, this.Current.y + y);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002411 File Offset: 0x00000611
		public void QuadraticSmoothRelative(Vector2 v)
		{
			this.QuadraticSmooth(this.Current + v);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002425 File Offset: 0x00000625
		public void Arc(float radiusX, float radiusY, float xAxisRotation, bool largeArcFlag, bool sweepFlag, float x, float y)
		{
			this.Arc(new Vector2(radiusX, radiusY), xAxisRotation, largeArcFlag, sweepFlag, new Vector2(x, y));
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002442 File Offset: 0x00000642
		public void ArcRelative(float radiusX, float radiusY, float xAxisRotation, bool largeArcFlag, bool sweepFlag, float x, float y)
		{
			this.ArcRelative(new Vector2(radiusX, radiusY), xAxisRotation, largeArcFlag, sweepFlag, new Vector2(x, y));
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000245F File Offset: 0x0000065F
		public void ArcRelative(Vector2 radius, float xAxisRotation, bool largeArcFlag, bool sweepFlag, Vector2 v)
		{
			this.Arc(radius, xAxisRotation, largeArcFlag, sweepFlag, this.Current + v);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002479 File Offset: 0x00000679
		public void Line(float x, float y)
		{
			this.Line(new Vector2(x, y));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002488 File Offset: 0x00000688
		public void Line(Vector2 v)
		{
			this.CurveInternal(this.Current, v, v);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002498 File Offset: 0x00000698
		public void LineRelative(float x, float y)
		{
			this.Line(this.Current.x + x, this.Current.y + y);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024BA File Offset: 0x000006BA
		public void LineRelative(Vector2 v)
		{
			this.Line(this.Current + v);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000024CE File Offset: 0x000006CE
		public void LineHorizontal(float x)
		{
			this.Line(x, this.Current.y);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000024E2 File Offset: 0x000006E2
		public void LineHorizontalRelative(float x)
		{
			this.LineHorizontal(this.Current.x + x);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000024F7 File Offset: 0x000006F7
		public void LineVertical(float y)
		{
			this.Line(this.Current.x, y);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000250B File Offset: 0x0000070B
		public void LineVerticalRelative(float y)
		{
			this.LineVertical(this.Current.y + y);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002520 File Offset: 0x00000720
		public void Close()
		{
			this.Line(this.Start);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002530 File Offset: 0x00000730
		public void Arc(Vector2 radius, float xAxisRotation, bool largeArcFlag, bool sweepFlag, Vector2 v)
		{
			if (radius.x == 0f || radius.y == 0f)
			{
				return;
			}
			float num = Mathf.Sin(xAxisRotation * 6.2831855f / 360f);
			float num2 = Mathf.Cos(xAxisRotation * 6.2831855f / 360f);
			float num3 = num2 * (this.Current.x - v.x) / 2f + num * (this.Current.y - v.y) / 2f;
			float num4 = -num * (this.Current.x - v.x) / 2f + num2 * (this.Current.y - v.y) / 2f;
			if (num3 == 0f && num4 == 0f)
			{
				return;
			}
			float num5 = Mathf.Abs(radius.x);
			float num6 = Mathf.Abs(radius.y);
			float num7 = Mathf.Pow(num3, 2f) / Mathf.Pow(num5, 2f) + Mathf.Pow(num4, 2f) / Mathf.Pow(num6, 2f);
			if (num7 > 1f)
			{
				num5 *= Mathf.Sqrt(num7);
				num6 *= Mathf.Sqrt(num7);
			}
			float num8 = Mathf.Pow(num5, 2f);
			float num9 = Mathf.Pow(num6, 2f);
			float num10 = Mathf.Pow(num3, 2f);
			float num11 = Mathf.Pow(num4, 2f);
			float num12 = num8 * num9 - num8 * num11 - num9 * num10;
			if (num12 < 0f)
			{
				num12 = 0f;
			}
			num12 /= num8 * num11 + num9 * num10;
			num12 = Mathf.Sqrt(num12) * ((largeArcFlag == sweepFlag) ? (-1f) : 1f);
			float num13 = num12 * num5 / num6 * num4;
			float num14 = num12 * -num6 / num5 * num3;
			float num15 = num2 * num13 - num * num14 + (this.Current.x + v.x) / 2f;
			float num16 = num * num13 + num2 * num14 + (this.Current.y + v.y) / 2f;
			float num17 = (num3 - num13) / num5;
			float num18 = (num4 - num14) / num6;
			float num19 = (-num3 - num13) / num5;
			float num20 = (-num4 - num14) / num6;
			float num21 = this.VectorAngle(1f, 0f, num17, num18);
			float num22 = this.VectorAngle(num17, num18, num19, num20);
			if (!sweepFlag && num22 > 0f)
			{
				num22 -= 6.2831855f;
			}
			if (sweepFlag && num22 < 0f)
			{
				num22 += 6.2831855f;
			}
			float num23 = Mathf.Max(Mathf.Ceil(Mathf.Abs(num22) / 1.5707964f), 1f);
			num22 /= num23;
			int num24 = 0;
			while ((float)num24 < num23)
			{
				float num25 = 1.3333334f * Mathf.Tan(num22 / 4f);
				float num26 = Mathf.Cos(num21);
				float num27 = Mathf.Sin(num21);
				float num28 = Mathf.Cos(num21 + num22);
				float num29 = Mathf.Sin(num21 + num22);
				Vector2 vector = new Vector2(num26 - num27 * num25, num27 + num26 * num25);
				Vector2 vector2 = new Vector2(num28 + num29 * num25, num29 - num28 * num25);
				Vector2 vector3 = new Vector2(num28, num29);
				this.MapToEllipse(ref vector, num5, num6, num2, num, num15, num16);
				this.MapToEllipse(ref vector2, num5, num6, num2, num, num15, num16);
				this.MapToEllipse(ref vector3, num5, num6, num2, num, num15, num16);
				this.CurveInternal(vector, vector2, vector3);
				num21 += num22;
				num24++;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000028D0 File Offset: 0x00000AD0
		private float VectorAngle(float ux, float uy, float vx, float vy)
		{
			float num = Mathf.Sign(ux * vy - uy * vx);
			float num2 = Mathf.Sqrt(ux * ux + uy * uy);
			float num3 = Mathf.Sqrt(ux * ux + uy * uy);
			float num4 = (ux * vx + uy * vy) / (num2 * num3);
			if (num4 > 1f)
			{
				num4 = 1f;
			}
			if (num4 < -1f)
			{
				num4 = -1f;
			}
			return num * Mathf.Acos(num4);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002934 File Offset: 0x00000B34
		private void MapToEllipse(ref Vector2 v, float rx, float ry, float cosphi, float sinphi, float centerx, float centery)
		{
			float num = v.x * rx;
			float num2 = v.y * ry;
			v.x = cosphi * num - sinphi * num2 + centerx;
			v.y = sinphi * num + cosphi * num2 + centery;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002978 File Offset: 0x00000B78
		public void Path(string data)
		{
			float[] array = new float[32];
			int num = 0;
			foreach (object obj in SVGData.Segment.Matches(data))
			{
				Match match = (Match)obj;
				string text = match.Groups[1].Value;
				string text2 = text.ToLower();
				this.ParseArgs(match.Groups[2].Value, ref array, out num);
				int num2 = 0;
				if (text2 == "m" && num > 2)
				{
					this.LoadCommand(text, text2, array, num, ref num2);
					text2 = "l";
					text = ((text == "m") ? "l" : "L");
				}
				while (!this.LoadCommand(text, text2, array, num, ref num2))
				{
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A70 File Offset: 0x00000C70
		private bool LoadCommand(string command, string type, float[] args, int numArgs, ref int argsIndex)
		{
			if (argsIndex == numArgs)
			{
				return true;
			}
			int num = SVGData.ArgumentLengthes[type];
			if (argsIndex + num > numArgs)
			{
				throw new ArgumentException("Malformed path data");
			}
			int num2 = argsIndex;
			uint num3 = <PrivateImplementationDetails>.ComputeStringHash(command);
			if (num3 <= 3742114125U)
			{
				if (num3 <= 3440116983U)
				{
					if (num3 <= 3322673650U)
					{
						if (num3 != 3289118412U)
						{
							if (num3 != 3322673650U)
							{
								goto IL_04E8;
							}
							if (!(command == "C"))
							{
								goto IL_04E8;
							}
							this.Curve(args[num2], args[num2 + 1], args[num2 + 2], args[num2 + 3], args[num2 + 4], args[num2 + 5]);
							goto IL_04E8;
						}
						else
						{
							if (!(command == "A"))
							{
								goto IL_04E8;
							}
							this.Arc(args[num2], args[num2 + 1], args[num2 + 2], args[num2 + 3] > 0f, args[num2 + 4] > 0f, args[num2 + 5], args[num2 + 6]);
							goto IL_04E8;
						}
					}
					else if (num3 != 3356228888U)
					{
						if (num3 != 3373006507U)
						{
							if (num3 != 3440116983U)
							{
								goto IL_04E8;
							}
							if (!(command == "H"))
							{
								goto IL_04E8;
							}
							this.LineHorizontal(args[num2]);
							goto IL_04E8;
						}
						else
						{
							if (!(command == "L"))
							{
								goto IL_04E8;
							}
							this.Line(args[num2], args[num2 + 1]);
							goto IL_04E8;
						}
					}
					else
					{
						if (!(command == "M"))
						{
							goto IL_04E8;
						}
						this.Move(args[num2], args[num2 + 1]);
						goto IL_04E8;
					}
				}
				else if (num3 <= 3540782697U)
				{
					if (num3 != 3507227459U)
					{
						if (num3 != 3540782697U)
						{
							goto IL_04E8;
						}
						if (!(command == "V"))
						{
							goto IL_04E8;
						}
						this.LineVertical(args[num2]);
						goto IL_04E8;
					}
					else
					{
						if (!(command == "T"))
						{
							goto IL_04E8;
						}
						this.QuadraticSmooth(args[num2], args[num2 + 1]);
						goto IL_04E8;
					}
				}
				else if (num3 != 3557560316U)
				{
					if (num3 != 3591115554U)
					{
						if (num3 != 3742114125U)
						{
							goto IL_04E8;
						}
						if (!(command == "Z"))
						{
							goto IL_04E8;
						}
					}
					else
					{
						if (!(command == "S"))
						{
							goto IL_04E8;
						}
						this.CurveSmooth(args[num2], args[num2 + 1], args[num2 + 2], args[num2 + 3]);
						goto IL_04E8;
					}
				}
				else
				{
					if (!(command == "Q"))
					{
						goto IL_04E8;
					}
					this.Quadratic(args[num2], args[num2 + 1], args[num2 + 2], args[num2 + 3]);
					goto IL_04E8;
				}
			}
			else if (num3 <= 3977000791U)
			{
				if (num3 <= 3859557458U)
				{
					if (num3 != 3826002220U)
					{
						if (num3 != 3859557458U)
						{
							goto IL_04E8;
						}
						if (!(command == "c"))
						{
							goto IL_04E8;
						}
						this.CurveRelative(args[num2], args[num2 + 1], args[num2 + 2], args[num2 + 3], args[num2 + 4], args[num2 + 5]);
						goto IL_04E8;
					}
					else
					{
						if (!(command == "a"))
						{
							goto IL_04E8;
						}
						this.ArcRelative(args[num2], args[num2 + 1], args[num2 + 2], args[num2 + 3] > 0f, args[num2 + 4] > 0f, args[num2 + 5], args[num2 + 6]);
						goto IL_04E8;
					}
				}
				else if (num3 != 3893112696U)
				{
					if (num3 != 3909890315U)
					{
						if (num3 != 3977000791U)
						{
							goto IL_04E8;
						}
						if (!(command == "h"))
						{
							goto IL_04E8;
						}
						this.LineHorizontalRelative(args[num2]);
						goto IL_04E8;
					}
					else
					{
						if (!(command == "l"))
						{
							goto IL_04E8;
						}
						this.LineRelative(args[num2], args[num2 + 1]);
						goto IL_04E8;
					}
				}
				else
				{
					if (!(command == "m"))
					{
						goto IL_04E8;
					}
					this.MoveRelative(args[num2], args[num2 + 1]);
					goto IL_04E8;
				}
			}
			else if (num3 <= 4077666505U)
			{
				if (num3 != 4044111267U)
				{
					if (num3 != 4077666505U)
					{
						goto IL_04E8;
					}
					if (!(command == "v"))
					{
						goto IL_04E8;
					}
					this.LineVerticalRelative(args[num2]);
					goto IL_04E8;
				}
				else
				{
					if (!(command == "t"))
					{
						goto IL_04E8;
					}
					this.QuadraticSmoothRelative(args[num2], args[num2 + 1]);
					goto IL_04E8;
				}
			}
			else if (num3 != 4094444124U)
			{
				if (num3 != 4127999362U)
				{
					if (num3 != 4278997933U)
					{
						goto IL_04E8;
					}
					if (!(command == "z"))
					{
						goto IL_04E8;
					}
				}
				else
				{
					if (!(command == "s"))
					{
						goto IL_04E8;
					}
					this.CurveSmoothRelative(args[num2], args[num2 + 1], args[num2 + 2], args[num2 + 3]);
					goto IL_04E8;
				}
			}
			else
			{
				if (!(command == "q"))
				{
					goto IL_04E8;
				}
				this.QuadraticRelative(args[num2], args[num2 + 1], args[num2 + 2], args[num2 + 3]);
				goto IL_04E8;
			}
			this.Close();
			IL_04E8:
			argsIndex += num;
			return false;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002F70 File Offset: 0x00001170
		private void ParseArgs(string s, ref float[] args, out int numArgs)
		{
			numArgs = 0;
			int length = s.Length;
			StringBuilder stringBuilder = new StringBuilder(16);
			bool flag = false;
			bool flag2 = false;
			int i = 0;
			while (i < length)
			{
				bool flag3 = false;
				char c = s[i];
				switch (c)
				{
				case '+':
				case '-':
					if (stringBuilder.Length > 0 && !flag)
					{
						flag3 = true;
						i--;
					}
					else
					{
						stringBuilder.Append(c);
						flag = false;
					}
					break;
				case ',':
				case '/':
					goto IL_00D4;
				case '.':
					if (flag2)
					{
						flag3 = true;
						i--;
					}
					else
					{
						stringBuilder.Append(c);
						flag2 = true;
						flag = false;
					}
					break;
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
					stringBuilder.Append(c);
					flag = false;
					break;
				default:
					if (c != 'e')
					{
						goto IL_00D4;
					}
					stringBuilder.Append(c);
					flag = true;
					break;
				}
				IL_00D7:
				if ((flag3 || i == length - 1) && stringBuilder.Length > 0)
				{
					if (args.Length == numArgs)
					{
						float[] array = new float[args.Length + 32];
						args.CopyTo(array, 0);
						args = array;
					}
					args[numArgs] = float.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture);
					numArgs++;
					stringBuilder.Length = 0;
					flag = false;
					flag2 = false;
				}
				i++;
				continue;
				IL_00D4:
				flag3 = true;
				goto IL_00D7;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000030C0 File Offset: 0x000012C0
		public string Dump()
		{
			return this.Curves.Select<Curve, string>(delegate(Curve _)
			{
				if (_.IsMove)
				{
					return string.Format("M {0} {1}", _.Position.x, _.Position.y);
				}
				return string.Format("C {0} {1}, {2} {3}, {4} {5}", new object[]
				{
					_.InControl.x,
					_.InControl.y,
					_.OutControl.x,
					_.OutControl.y,
					_.Position.x,
					_.Position.y
				});
			}).Aggregate("", (string _, string s) => _ + s + " ");
		}

		// Token: 0x04000001 RID: 1
		internal List<Curve> Curves = new List<Curve>();

		// Token: 0x04000002 RID: 2
		private Vector2 Start;

		// Token: 0x04000003 RID: 3
		private Vector2 Current;

		// Token: 0x04000004 RID: 4
		private Vector2? Bezier;

		// Token: 0x04000005 RID: 5
		private Vector2? Quad;

		// Token: 0x04000006 RID: 6
		private static readonly Regex Segment = new Regex("([astvzqmhlc])([^astvzqmhlc]*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

		// Token: 0x04000007 RID: 7
		private static readonly Dictionary<string, int> ArgumentLengthes = new Dictionary<string, int>
		{
			{ "a", 7 },
			{ "c", 6 },
			{ "h", 1 },
			{ "l", 2 },
			{ "m", 2 },
			{ "q", 4 },
			{ "s", 4 },
			{ "t", 2 },
			{ "v", 1 },
			{ "z", 0 }
		};
	}
}
