using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000109 RID: 265
	public class MMTime : MonoBehaviour
	{
		// Token: 0x060005BC RID: 1468 RVA: 0x0001C18C File Offset: 0x0001A38C
		public static string FloatToTimeString(float t, bool displayHours = false, bool displayMinutes = true, bool displaySeconds = true, bool displayMilliseconds = false)
		{
			int num = (int)t;
			int num2 = num / 3600;
			int num3 = num / 60;
			int num4 = num % 60;
			int num5 = Mathf.FloorToInt(t * 1000f % 1000f);
			if (displayHours && displayMinutes && displaySeconds && displayMilliseconds)
			{
				return string.Format("{0:00}:{1:00}:{2:00}.{3:D3}", new object[] { num2, num3, num4, num5 });
			}
			if (!displayHours && displayMinutes && displaySeconds && displayMilliseconds)
			{
				return string.Format("{0:00}:{1:00}.{2:D3}", num3, num4, num5);
			}
			if (!displayHours && !displayMinutes && displaySeconds && displayMilliseconds)
			{
				return string.Format("{0:D2}.{1:D3}", num4, num5);
			}
			if (!displayHours && !displayMinutes && displaySeconds && !displayMilliseconds)
			{
				return string.Format("{0:00}", num4);
			}
			if (displayHours && displayMinutes && displaySeconds && !displayMilliseconds)
			{
				return string.Format("{0:00}:{1:00}:{2:00}", num2, num3, num4);
			}
			if (!displayHours && displayMinutes && displaySeconds && !displayMilliseconds)
			{
				return string.Format("{0:00}:{1:00}", num3, num4);
			}
			return null;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001C2C4 File Offset: 0x0001A4C4
		public static float TimeStringToFloat(string timeInStringNotation)
		{
			if (timeInStringNotation.Length != 12)
			{
				throw new Exception("The time in the TimeStringToFloat method must be specified using a hh:mm:ss:SSS syntax");
			}
			string[] array = timeInStringNotation.Split(new string[] { ":", "." }, StringSplitOptions.None);
			float num = 0f;
			float num2;
			if (float.TryParse(array[0], out num2))
			{
				num += num2 * 3600f;
			}
			if (float.TryParse(array[1], out num2))
			{
				num += num2 * 60f;
			}
			if (float.TryParse(array[2], out num2))
			{
				num += num2;
			}
			if (float.TryParse(array[3], out num2))
			{
				num += num2 / 1000f;
			}
			return num;
		}
	}
}
