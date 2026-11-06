using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x020000CD RID: 205
	[ReflectedPropertyBag]
	internal class ReflectedPropertyBag<TContainer> : ContainerPropertyBag<TContainer>
	{
		// Token: 0x06000406 RID: 1030 RVA: 0x0000C7C8 File Offset: 0x0000A9C8
		internal new void AddProperty<TValue>(Property<TContainer, TValue> property)
		{
			TContainer tcontainer = default(TContainer);
			IProperty<TContainer> property2;
			bool flag = base.TryGetProperty(ref tcontainer, property.Name, out property2);
			if (flag)
			{
				bool flag2 = property2.DeclaredValueType() == typeof(TValue);
				if (!flag2)
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"Detected multiple return types for PropertyBag=[",
						TypeUtility.GetTypeDisplayName(typeof(TContainer)),
						"] Property=[",
						property.Name,
						"]. The property will use the most derived Type=[",
						TypeUtility.GetTypeDisplayName(property2.DeclaredValueType()),
						"] and IgnoreType=[",
						TypeUtility.GetTypeDisplayName(property.DeclaredValueType()),
						"]."
					}));
				}
			}
			else
			{
				base.AddProperty<TValue>(property);
			}
		}
	}
}
