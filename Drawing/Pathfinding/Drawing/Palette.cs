using System;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x02000054 RID: 84
	public static class Palette
	{
		// Token: 0x02000055 RID: 85
		public static class Pure
		{
			// Token: 0x04000158 RID: 344
			public static readonly Color Yellow = new Color(1f, 1f, 0f, 1f);

			// Token: 0x04000159 RID: 345
			public static readonly Color Clear = new Color(0f, 0f, 0f, 0f);

			// Token: 0x0400015A RID: 346
			public static readonly Color Grey = new Color(0.5f, 0.5f, 0.5f, 1f);

			// Token: 0x0400015B RID: 347
			public static readonly Color Magenta = new Color(1f, 0f, 1f, 1f);

			// Token: 0x0400015C RID: 348
			public static readonly Color Cyan = new Color(0f, 1f, 1f, 1f);

			// Token: 0x0400015D RID: 349
			public static readonly Color Red = new Color(1f, 0f, 0f, 1f);

			// Token: 0x0400015E RID: 350
			public static readonly Color Black = new Color(0f, 0f, 0f, 1f);

			// Token: 0x0400015F RID: 351
			public static readonly Color White = new Color(1f, 1f, 1f, 1f);

			// Token: 0x04000160 RID: 352
			public static readonly Color Blue = new Color(0f, 0f, 1f, 1f);

			// Token: 0x04000161 RID: 353
			public static readonly Color Green = new Color(0f, 1f, 0f, 1f);
		}

		// Token: 0x02000056 RID: 86
		public static class Colorbrewer
		{
			// Token: 0x02000057 RID: 87
			public static class Set1
			{
				// Token: 0x04000162 RID: 354
				public static readonly Color Red = new Color(0.89411765f, 0.101960786f, 0.10980392f, 1f);

				// Token: 0x04000163 RID: 355
				public static readonly Color Blue = new Color(0.21568628f, 0.49411765f, 0.72156864f, 1f);

				// Token: 0x04000164 RID: 356
				public static readonly Color Green = new Color(0.3019608f, 0.6862745f, 0.2901961f, 1f);

				// Token: 0x04000165 RID: 357
				public static readonly Color Purple = new Color(0.59607846f, 0.30588236f, 0.6392157f, 1f);

				// Token: 0x04000166 RID: 358
				public static readonly Color Orange = new Color(1f, 0.49803922f, 0f, 1f);

				// Token: 0x04000167 RID: 359
				public static readonly Color Yellow = new Color(1f, 1f, 0.2f, 1f);

				// Token: 0x04000168 RID: 360
				public static readonly Color Brown = new Color(0.6509804f, 0.3372549f, 0.15686275f, 1f);

				// Token: 0x04000169 RID: 361
				public static readonly Color Pink = new Color(0.96862745f, 0.5058824f, 0.7490196f, 1f);

				// Token: 0x0400016A RID: 362
				public static readonly Color Grey = new Color(0.6f, 0.6f, 0.6f, 1f);
			}

			// Token: 0x02000058 RID: 88
			public static class Blues
			{
				// Token: 0x060002A4 RID: 676 RVA: 0x0000E994 File Offset: 0x0000CB94
				public static Color GetColor(int classes, int index)
				{
					if (index < 0 || index >= classes)
					{
						throw new ArgumentOutOfRangeException("index", "Index must be less than classes and at least 0");
					}
					if (classes <= 0 || classes > 9)
					{
						throw new ArgumentOutOfRangeException("classes", "Only up to 9 classes are supported");
					}
					return Palette.Colorbrewer.Blues.Colors[(classes - 1) * classes / 2 + index];
				}

				// Token: 0x0400016B RID: 363
				private static readonly Color[] Colors = new Color[]
				{
					new Color(0.16862746f, 0.54901963f, 0.74509805f),
					new Color(0.6509804f, 0.7411765f, 0.85882354f),
					new Color(0.16862746f, 0.54901963f, 0.74509805f),
					new Color(0.9254902f, 0.90588236f, 0.9490196f),
					new Color(0.6509804f, 0.7411765f, 0.85882354f),
					new Color(0.16862746f, 0.54901963f, 0.74509805f),
					new Color(0.94509804f, 0.93333334f, 0.9647059f),
					new Color(0.7411765f, 0.7882353f, 0.88235295f),
					new Color(0.45490196f, 0.6627451f, 0.8117647f),
					new Color(0.019607844f, 0.4392157f, 0.6901961f),
					new Color(0.94509804f, 0.93333334f, 0.9647059f),
					new Color(0.7411765f, 0.7882353f, 0.88235295f),
					new Color(0.45490196f, 0.6627451f, 0.8117647f),
					new Color(0.16862746f, 0.54901963f, 0.74509805f),
					new Color(0.015686275f, 0.3529412f, 0.5529412f),
					new Color(0.94509804f, 0.93333334f, 0.9647059f),
					new Color(0.8156863f, 0.81960785f, 0.9019608f),
					new Color(0.6509804f, 0.7411765f, 0.85882354f),
					new Color(0.45490196f, 0.6627451f, 0.8117647f),
					new Color(0.16862746f, 0.54901963f, 0.74509805f),
					new Color(0.015686275f, 0.3529412f, 0.5529412f),
					new Color(0.94509804f, 0.93333334f, 0.9647059f),
					new Color(0.8156863f, 0.81960785f, 0.9019608f),
					new Color(0.6509804f, 0.7411765f, 0.85882354f),
					new Color(0.45490196f, 0.6627451f, 0.8117647f),
					new Color(0.21176471f, 0.5647059f, 0.7529412f),
					new Color(0.019607844f, 0.4392157f, 0.6901961f),
					new Color(0.011764706f, 0.30588236f, 0.48235294f),
					new Color(1f, 0.96862745f, 0.9843137f),
					new Color(0.9254902f, 0.90588236f, 0.9490196f),
					new Color(0.8156863f, 0.81960785f, 0.9019608f),
					new Color(0.6509804f, 0.7411765f, 0.85882354f),
					new Color(0.45490196f, 0.6627451f, 0.8117647f),
					new Color(0.21176471f, 0.5647059f, 0.7529412f),
					new Color(0.019607844f, 0.4392157f, 0.6901961f),
					new Color(0.011764706f, 0.30588236f, 0.48235294f),
					new Color(1f, 0.96862745f, 0.9843137f),
					new Color(0.9254902f, 0.90588236f, 0.9490196f),
					new Color(0.8156863f, 0.81960785f, 0.9019608f),
					new Color(0.6509804f, 0.7411765f, 0.85882354f),
					new Color(0.45490196f, 0.6627451f, 0.8117647f),
					new Color(0.21176471f, 0.5647059f, 0.7529412f),
					new Color(0.019607844f, 0.4392157f, 0.6901961f),
					new Color(0.015686275f, 0.3529412f, 0.5529412f),
					new Color(0.007843138f, 0.21960784f, 0.34509805f)
				};
			}
		}
	}
}
