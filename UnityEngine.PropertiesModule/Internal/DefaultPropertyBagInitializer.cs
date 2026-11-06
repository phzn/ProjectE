using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x02000099 RID: 153
	internal static class DefaultPropertyBagInitializer
	{
		// Token: 0x0600033B RID: 827 RVA: 0x0000BCD8 File Offset: 0x00009ED8
		internal static void Initialize()
		{
			PropertyBag.Register<Color>(new ColorPropertyBag());
			PropertyBag.Register<Vector2>(new Vector2PropertyBag());
			PropertyBag.Register<Vector3>(new Vector3PropertyBag());
			PropertyBag.Register<Vector4>(new Vector4PropertyBag());
			PropertyBag.Register<Vector2Int>(new Vector2IntPropertyBag());
			PropertyBag.Register<Vector3Int>(new Vector3IntPropertyBag());
			PropertyBag.Register<Rect>(new RectPropertyBag());
			PropertyBag.Register<RectInt>(new RectIntPropertyBag());
			PropertyBag.Register<Bounds>(new BoundsPropertyBag());
			PropertyBag.Register<BoundsInt>(new BoundsIntPropertyBag());
			PropertyBag.Register<Version>(new SystemVersionPropertyBag());
		}
	}
}
