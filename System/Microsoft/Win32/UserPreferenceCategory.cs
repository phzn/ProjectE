using System;

namespace Microsoft.Win32
{
	/// <summary>Defines identifiers that represent categories of user preferences.</summary>
	// Token: 0x0200012D RID: 301
	public enum UserPreferenceCategory
	{
		/// <summary>Indicates user preferences associated with accessibility features of the system for users with disabilities.</summary>
		// Token: 0x040004E7 RID: 1255
		Accessibility = 1,
		/// <summary>Indicates user preferences associated with system colors. This category includes such as the default color of windows or menus.</summary>
		// Token: 0x040004E8 RID: 1256
		Color,
		/// <summary>Indicates user preferences associated with the system desktop. This category includes the background image or background image layout of the desktop.</summary>
		// Token: 0x040004E9 RID: 1257
		Desktop,
		/// <summary>Indicates user preferences that are not associated with any other category.</summary>
		// Token: 0x040004EA RID: 1258
		General,
		/// <summary>Indicates user preferences for icon settings, including icon height and spacing.</summary>
		// Token: 0x040004EB RID: 1259
		Icon,
		/// <summary>Indicates user preferences for keyboard settings, such as the key down repeat rate and delay.</summary>
		// Token: 0x040004EC RID: 1260
		Keyboard,
		/// <summary>Indicates user preferences for menu settings, such as menu delays and text alignment.</summary>
		// Token: 0x040004ED RID: 1261
		Menu,
		/// <summary>Indicates user preferences for mouse settings, such as double-click time and mouse sensitivity.</summary>
		// Token: 0x040004EE RID: 1262
		Mouse,
		/// <summary>Indicates user preferences for policy settings, such as user rights and access levels.</summary>
		// Token: 0x040004EF RID: 1263
		Policy,
		/// <summary>Indicates the user preferences for system power settings. This category includes power feature settings, such as the idle time before the system automatically enters low power mode.</summary>
		// Token: 0x040004F0 RID: 1264
		Power,
		/// <summary>Indicates user preferences associated with the screensaver.</summary>
		// Token: 0x040004F1 RID: 1265
		Screensaver,
		/// <summary>Indicates user preferences associated with the dimensions and characteristics of windows on the system.</summary>
		// Token: 0x040004F2 RID: 1266
		Window,
		/// <summary>Indicates changes in user preferences for regional settings, such as the character encoding and culture strings.</summary>
		// Token: 0x040004F3 RID: 1267
		Locale,
		/// <summary>Indicates user preferences associated with visual styles, such as enabling or disabling visual styles and switching from one visual style to another.</summary>
		// Token: 0x040004F4 RID: 1268
		VisualStyle
	}
}
