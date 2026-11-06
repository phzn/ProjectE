using System;
using System.Globalization;

namespace System.Reflection
{
	// Token: 0x020008F5 RID: 2293
	internal abstract class RtFieldInfo : FieldInfo
	{
		// Token: 0x06004D14 RID: 19732
		internal abstract object UnsafeGetValue(object obj);

		// Token: 0x06004D15 RID: 19733
		internal abstract void UnsafeSetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture);

		// Token: 0x06004D16 RID: 19734
		internal abstract void CheckConsistency(object target);
	}
}
