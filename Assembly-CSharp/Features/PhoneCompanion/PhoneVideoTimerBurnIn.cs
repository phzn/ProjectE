using System;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E57 RID: 3671
	internal static class PhoneVideoTimerBurnIn
	{
		// Token: 0x06006D96 RID: 28054 RVA: 0x002C0298 File Offset: 0x002BE498
		internal static string FormatElapsed(int frameIndex, int frameRate)
		{
			int num = Math.Max(0, frameIndex) / Math.Max(1, frameRate);
			int num2 = Math.Min(99, num / 3600);
			int num3 = num / 60 % 60;
			int num4 = num % 60;
			return string.Format("{0:00}:{1:00}:{2:00}", num2, num3, num4);
		}

		// Token: 0x06006D97 RID: 28055 RVA: 0x002C02F0 File Offset: 0x002BE4F0
		internal static void Draw(byte[] rgb24, int width, int height, int frameIndex, int frameRate)
		{
			if (rgb24 == null || width < 120 || height < 80 || rgb24.Length < width * height * 3)
			{
				return;
			}
			int num = Mathf.Clamp(Mathf.CeilToInt((float)height / 900f), 1, 3);
			int num2 = num;
			int num3 = 6 * num;
			int num4 = 10 * num;
			int num5 = 2 * num;
			int num6 = 2 * num;
			int num7 = 4 * num;
			int num8 = 4 * num;
			int num9 = 6 * num;
			int num10 = 3 * num;
			int num11 = 12 * num;
			int num12 = 6 * num3 + 2 * num5 + 7 * num6;
			int num13 = num9 * 2 + num7 + num8 + num12;
			int num14 = num4 + num10 * 2;
			int num15 = width - num11 - num13;
			int num16 = height - num11 - num14;
			if (num15 < 0 || num16 < 0)
			{
				return;
			}
			PhoneVideoTimerBurnIn.BlendRoundedRect(rgb24, width, height, num15, num16, num13, num14, 4 * num, 0, 0, 0, 112);
			int num17 = num16 + num10;
			int num18 = num15 + num9;
			int num19 = num16 + (num14 - num7) / 2;
			PhoneVideoTimerBurnIn.BlendCircle(rgb24, width, height, num18, num19, num7, byte.MaxValue, 69, 58, byte.MaxValue);
			string text = PhoneVideoTimerBurnIn.FormatElapsed(frameIndex, frameRate);
			int num20 = num18 + num7 + num8;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == ':')
				{
					PhoneVideoTimerBurnIn.DrawColon(rgb24, width, height, num20, num17, num5, num4, num);
					num20 += num5;
				}
				else
				{
					PhoneVideoTimerBurnIn.DrawDigit(rgb24, width, height, num20, num17, (int)(c - '0'), num3, num4, num2);
					num20 += num3;
				}
				if (i < text.Length - 1)
				{
					num20 += num6;
				}
			}
		}

		// Token: 0x06006D98 RID: 28056 RVA: 0x002C046C File Offset: 0x002BE66C
		private static void DrawDigit(byte[] pixels, int width, int height, int x, int y, int digit, int digitWidth, int digitHeight, int thickness)
		{
			if (digit < 0 || digit > 9)
			{
				return;
			}
			byte b = PhoneVideoTimerBurnIn.DigitMasks[digit];
			int num = digitWidth - thickness * 2;
			int num2 = (digitHeight - thickness * 3) / 2;
			int num3 = y + (digitHeight - thickness) / 2;
			int num4 = num3 + thickness;
			int num5 = y + thickness;
			if ((b & 1) != 0)
			{
				PhoneVideoTimerBurnIn.DrawSegment(pixels, width, height, x + thickness, y + digitHeight - thickness, num, thickness);
			}
			if ((b & 2) != 0)
			{
				PhoneVideoTimerBurnIn.DrawSegment(pixels, width, height, x + digitWidth - thickness, num4, thickness, num2);
			}
			if ((b & 4) != 0)
			{
				PhoneVideoTimerBurnIn.DrawSegment(pixels, width, height, x + digitWidth - thickness, num5, thickness, num2);
			}
			if ((b & 8) != 0)
			{
				PhoneVideoTimerBurnIn.DrawSegment(pixels, width, height, x + thickness, y, num, thickness);
			}
			if ((b & 16) != 0)
			{
				PhoneVideoTimerBurnIn.DrawSegment(pixels, width, height, x, num5, thickness, num2);
			}
			if ((b & 32) != 0)
			{
				PhoneVideoTimerBurnIn.DrawSegment(pixels, width, height, x, num4, thickness, num2);
			}
			if ((b & 64) != 0)
			{
				PhoneVideoTimerBurnIn.DrawSegment(pixels, width, height, x + thickness, num3, num, thickness);
			}
		}

		// Token: 0x06006D99 RID: 28057 RVA: 0x002C0558 File Offset: 0x002BE758
		private static void DrawColon(byte[] pixels, int width, int height, int x, int y, int colonWidth, int digitHeight, int scale)
		{
			int num = Mathf.Max(scale, colonWidth);
			PhoneVideoTimerBurnIn.DrawSegment(pixels, width, height, x, y + digitHeight * 2 / 3, num, num);
			PhoneVideoTimerBurnIn.DrawSegment(pixels, width, height, x, y + digitHeight / 3 - num, num, num);
		}

		// Token: 0x06006D9A RID: 28058 RVA: 0x002C0598 File Offset: 0x002BE798
		private static void DrawSegment(byte[] pixels, int width, int height, int x, int y, int rectWidth, int rectHeight)
		{
			PhoneVideoTimerBurnIn.BlendRect(pixels, width, height, x, y, rectWidth, rectHeight, byte.MaxValue, 69, 58, byte.MaxValue);
		}

		// Token: 0x06006D9B RID: 28059 RVA: 0x002C05C4 File Offset: 0x002BE7C4
		private static void BlendRoundedRect(byte[] pixels, int width, int height, int x, int y, int rectWidth, int rectHeight, int radius, byte red, byte green, byte blue, byte alpha)
		{
			int num = radius * radius;
			for (int i = 0; i < rectHeight; i++)
			{
				for (int j = 0; j < rectWidth; j++)
				{
					int num2 = Mathf.Clamp(j, radius, rectWidth - radius - 1);
					int num3 = Mathf.Clamp(i, radius, rectHeight - radius - 1);
					int num4 = j - num2;
					int num5 = i - num3;
					if (num4 * num4 + num5 * num5 <= num)
					{
						PhoneVideoTimerBurnIn.BlendPixel(pixels, width, height, x + j, y + i, red, green, blue, alpha);
					}
				}
			}
		}

		// Token: 0x06006D9C RID: 28060 RVA: 0x002C0640 File Offset: 0x002BE840
		private static void BlendCircle(byte[] pixels, int width, int height, int x, int y, int diameter, byte red, byte green, byte blue, byte alpha)
		{
			float num = (float)diameter * 0.5f;
			float num2 = (float)(diameter - 1) * 0.5f;
			float num3 = num * num;
			for (int i = 0; i < diameter; i++)
			{
				for (int j = 0; j < diameter; j++)
				{
					float num4 = (float)j - num2;
					float num5 = (float)i - num2;
					if (num4 * num4 + num5 * num5 <= num3)
					{
						PhoneVideoTimerBurnIn.BlendPixel(pixels, width, height, x + j, y + i, red, green, blue, alpha);
					}
				}
			}
		}

		// Token: 0x06006D9D RID: 28061 RVA: 0x002C06AC File Offset: 0x002BE8AC
		private static void BlendRect(byte[] pixels, int width, int height, int x, int y, int rectWidth, int rectHeight, byte red, byte green, byte blue, byte alpha)
		{
			for (int i = 0; i < rectHeight; i++)
			{
				for (int j = 0; j < rectWidth; j++)
				{
					PhoneVideoTimerBurnIn.BlendPixel(pixels, width, height, x + j, y + i, red, green, blue, alpha);
				}
			}
		}

		// Token: 0x06006D9E RID: 28062 RVA: 0x002C06EC File Offset: 0x002BE8EC
		private static void BlendPixel(byte[] pixels, int width, int height, int x, int y, byte red, byte green, byte blue, byte alpha)
		{
			if (x >= width || y >= height)
			{
				return;
			}
			int num = (y * width + x) * 3;
			int num2 = (int)(byte.MaxValue - alpha);
			pixels[num] = (byte)(((int)(red * alpha) + (int)pixels[num] * num2 + 127) / 255);
			pixels[num + 1] = (byte)(((int)(green * alpha) + (int)pixels[num + 1] * num2 + 127) / 255);
			pixels[num + 2] = (byte)(((int)(blue * alpha) + (int)pixels[num + 2] * num2 + 127) / 255);
		}

		// Token: 0x0400703B RID: 28731
		private const byte RedR = 255;

		// Token: 0x0400703C RID: 28732
		private const byte RedG = 69;

		// Token: 0x0400703D RID: 28733
		private const byte RedB = 58;

		// Token: 0x0400703E RID: 28734
		private static readonly byte[] DigitMasks = new byte[] { 63, 6, 91, 79, 102, 109, 125, 7, 127, 111 };
	}
}
