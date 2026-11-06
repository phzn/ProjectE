using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000B0 RID: 176
	[AttributeUsage(AttributeTargets.Field)]
	public class SurfaceDataAttributes : Attribute
	{
		// Token: 0x06000555 RID: 1365 RVA: 0x0001BCFC File Offset: 0x00019EFC
		public SurfaceDataAttributes(string displayName = "", bool isDirection = false, bool sRGBDisplay = false, FieldPrecision precision = FieldPrecision.Default, bool checkIsNormalized = false, string preprocessor = "")
		{
			this.displayNames = new string[1];
			this.displayNames[0] = displayName;
			this.isDirection = isDirection;
			this.sRGBDisplay = sRGBDisplay;
			this.precision = precision;
			this.checkIsNormalized = checkIsNormalized;
			this.preprocessor = preprocessor;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001BD4A File Offset: 0x00019F4A
		public SurfaceDataAttributes(string[] displayNames, bool isDirection = false, bool sRGBDisplay = false, FieldPrecision precision = FieldPrecision.Default, bool checkIsNormalized = false, string preprocessor = "")
		{
			this.displayNames = displayNames;
			this.isDirection = isDirection;
			this.sRGBDisplay = sRGBDisplay;
			this.precision = precision;
			this.checkIsNormalized = checkIsNormalized;
			this.preprocessor = preprocessor;
		}

		// Token: 0x040003DB RID: 987
		public string[] displayNames;

		// Token: 0x040003DC RID: 988
		public bool isDirection;

		// Token: 0x040003DD RID: 989
		public bool sRGBDisplay;

		// Token: 0x040003DE RID: 990
		public FieldPrecision precision;

		// Token: 0x040003DF RID: 991
		public bool checkIsNormalized;

		// Token: 0x040003E0 RID: 992
		public string preprocessor;
	}
}
