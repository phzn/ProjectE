using System;

// Token: 0x02000108 RID: 264
public static class vBodyStructHelper
{
	// Token: 0x06000989 RID: 2441 RVA: 0x000440CC File Offset: 0x000422CC
	public static bool ToEnum<T>(this string value, ref T enumTarget)
	{
		object obj = Enum.Parse(typeof(T), value);
		if (obj != null)
		{
			enumTarget = (T)((object)obj);
		}
		return obj != null;
	}
}
