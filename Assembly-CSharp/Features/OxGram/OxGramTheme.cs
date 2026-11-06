using System;
using TMPro;
using UnityEngine;

namespace Features.OxGram
{
	// Token: 0x02000EB3 RID: 3763
	[CreateAssetMenu(menuName = "OxGram/Theme", fileName = "OxGramTheme")]
	public class OxGramTheme : ScriptableObject
	{
		// Token: 0x060070DF RID: 28895 RVA: 0x002DD1B0 File Offset: 0x002DB3B0
		public Color AvatarColor(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return this.authorAccent;
			}
			if (name.StartsWith("Oxpath", StringComparison.Ordinal))
			{
				return OxGramTheme.AvatarPalette[0];
			}
			if (name == "Tokyo")
			{
				return OxGramTheme.AvatarPalette[1];
			}
			if (name == "Night Owls")
			{
				return OxGramTheme.AvatarPalette[2];
			}
			int num = 0;
			foreach (char c in name)
			{
				num = num * 31 + (int)c;
			}
			return OxGramTheme.AvatarPalette[num % OxGramTheme.AvatarPalette.Length];
		}

		// Token: 0x04007394 RID: 29588
		[Header("Fonts")]
		public TMP_FontAsset font;

		// Token: 0x04007395 RID: 29589
		public TMP_FontAsset fontBold;

		// Token: 0x04007396 RID: 29590
		[Header("Shared sprites")]
		[Tooltip("Rounded-rect sprite (sliced) used for bubbles, header, input bar.")]
		public Sprite roundedSprite;

		// Token: 0x04007397 RID: 29591
		[Tooltip("Circular sprite used for avatars.")]
		public Sprite circleSprite;

		// Token: 0x04007398 RID: 29592
		[Tooltip("Error/warning icon shown in the 'censorship failed' critical-error popup. A filled circle with '!' reads best. If null, a textual glyph is used as fallback.")]
		public Sprite errorIcon;

		// Token: 0x04007399 RID: 29593
		[Tooltip("Native phone-toolkit Back sprite (chevron + 'Back'), so CielGram's back button looks EXACTLY like the native apps. Assign Assets/ThirdParty/CompanionToolkit/MobileCompanion/UI/Icons/Back.png. If null, a textual '‹ Back' fallback is built.")]
		public Sprite backSprite;

		// Token: 0x0400739A RID: 29594
		[Tooltip("Send icon for the chat input bar's send button. Shown preserving aspect ratio inside the button. Assign Assets/Features/Smartphone Feature/UI/send.png. If null, a '➤' glyph is used.")]
		public Sprite sendSprite;

		// Token: 0x0400739B RID: 29595
		[Header("Sounds")]
		[Tooltip("Played once when the 'censorship failed' critical-error popup appears. If null, no sound.")]
		public AudioClip errorSound;

		// Token: 0x0400739C RID: 29596
		[Header("CielGram midnight palette")]
		public Color appBackground = new Color(0.055f, 0.067f, 0.114f, 1f);

		// Token: 0x0400739D RID: 29597
		public Color headerColor = new Color(0.055f, 0.067f, 0.114f, 1f);

		// Token: 0x0400739E RID: 29598
		public Color bubbleOther = new Color(0.105f, 0.125f, 0.196f, 1f);

		// Token: 0x0400739F RID: 29599
		public Color bubbleMine = new Color(0.365f, 0.294f, 0.62f, 1f);

		// Token: 0x040073A0 RID: 29600
		public Color textPrimary = new Color(0.95f, 0.95f, 0.99f, 1f);

		// Token: 0x040073A1 RID: 29601
		public Color textSecondary = new Color(0.64f, 0.68f, 0.79f, 1f);

		// Token: 0x040073A2 RID: 29602
		public Color authorAccent = new Color(0.72f, 0.65f, 1f, 1f);

		// Token: 0x040073A3 RID: 29603
		public Color inputBar = new Color(0.075f, 0.09f, 0.15f, 1f);

		// Token: 0x040073A4 RID: 29604
		public Color listRowDivider = new Color(0.5f, 0.55f, 0.8f, 0.12f);

		// Token: 0x040073A5 RID: 29605
		[Tooltip("On-header text/icons (title, close X, back arrow) — white reads well on the dark bar.")]
		public Color headerForeground = new Color(0.95f, 0.95f, 0.99f, 1f);

		// Token: 0x040073A6 RID: 29606
		[Tooltip("Lavender brand mark tint.")]
		public Color logoColor = new Color(0.72f, 0.65f, 1f, 1f);

		// Token: 0x040073A7 RID: 29607
		[Tooltip("Primary action color for confirm, send and selected filters.")]
		public Color accent = new Color(0.49f, 0.39f, 0.84f, 1f);

		// Token: 0x040073A8 RID: 29608
		private static readonly Color[] AvatarPalette = new Color[]
		{
			new Color(0.6f, 0.52f, 0.85f),
			new Color(0.34f, 0.67f, 0.65f),
			new Color(0.84f, 0.53f, 0.53f),
			new Color(0.41f, 0.59f, 0.81f),
			new Color(0.73f, 0.53f, 0.72f)
		};
	}
}
