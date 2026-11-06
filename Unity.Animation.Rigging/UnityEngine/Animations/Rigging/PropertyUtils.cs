using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200006F RID: 111
	[Obsolete("PropertyUtils is deprecated. Use ConstraintsUtils instead. (UnityUpgradable) -> ConstraintsUtils")]
	public static class PropertyUtils
	{
		// Token: 0x060002DA RID: 730 RVA: 0x000096DE File Offset: 0x000078DE
		[Obsolete("ConstructConstraintDataPropertyName is deprecated. Use ConstraintsUtils.ConstructConstraintDataPropertyName instead.")]
		public static string ConstructConstraintDataPropertyName(string property)
		{
			return ConstraintsUtils.ConstructConstraintDataPropertyName(property);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x000096E6 File Offset: 0x000078E6
		[Obsolete("ConstructCustomPropertyName is deprecated. Use ConstraintsUtils.ConstructCustomPropertyName instead.")]
		public static string ConstructCustomPropertyName(Component component, string property)
		{
			return ConstraintsUtils.ConstructCustomPropertyName(component, property);
		}
	}
}
