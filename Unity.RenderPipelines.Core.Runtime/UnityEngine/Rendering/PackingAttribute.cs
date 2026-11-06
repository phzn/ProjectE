using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000B2 RID: 178
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class PackingAttribute : Attribute
	{
		// Token: 0x06000558 RID: 1368 RVA: 0x0001BD98 File Offset: 0x00019F98
		public PackingAttribute(string[] displayNames, FieldPacking packingScheme = FieldPacking.NoPacking, int bitSize = 32, int offsetInSource = 0, float minValue = 0f, float maxValue = 1f, bool isDirection = false, bool sRGBDisplay = false, bool checkIsNormalized = false, string preprocessor = "")
		{
			this.displayNames = displayNames;
			this.packingScheme = packingScheme;
			this.offsetInSource = offsetInSource;
			this.isDirection = isDirection;
			this.sRGBDisplay = sRGBDisplay;
			this.checkIsNormalized = checkIsNormalized;
			this.sizeInBits = bitSize;
			this.range = new float[] { minValue, maxValue };
			this.preprocessor = preprocessor;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001BE00 File Offset: 0x0001A000
		public PackingAttribute(string displayName = "", FieldPacking packingScheme = FieldPacking.NoPacking, int bitSize = 0, int offsetInSource = 0, float minValue = 0f, float maxValue = 1f, bool isDirection = false, bool sRGBDisplay = false, bool checkIsNormalized = false, string preprocessor = "")
		{
			this.displayNames = new string[1];
			this.displayNames[0] = displayName;
			this.packingScheme = packingScheme;
			this.offsetInSource = offsetInSource;
			this.isDirection = isDirection;
			this.sRGBDisplay = sRGBDisplay;
			this.checkIsNormalized = checkIsNormalized;
			this.sizeInBits = bitSize;
			this.range = new float[] { minValue, maxValue };
			this.preprocessor = preprocessor;
		}

		// Token: 0x040003E3 RID: 995
		public string[] displayNames;

		// Token: 0x040003E4 RID: 996
		public float[] range;

		// Token: 0x040003E5 RID: 997
		public FieldPacking packingScheme;

		// Token: 0x040003E6 RID: 998
		public int offsetInSource;

		// Token: 0x040003E7 RID: 999
		public int sizeInBits;

		// Token: 0x040003E8 RID: 1000
		public bool isDirection;

		// Token: 0x040003E9 RID: 1001
		public bool sRGBDisplay;

		// Token: 0x040003EA RID: 1002
		public bool checkIsNormalized;

		// Token: 0x040003EB RID: 1003
		public string preprocessor;
	}
}
