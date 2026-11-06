using System;

namespace Features.Localization
{
	// Token: 0x020010D1 RID: 4305
	public static class NavigationLocalization
	{
		// Token: 0x06008282 RID: 33410 RVA: 0x00354D44 File Offset: 0x00352F44
		public static string PlaceName(string english)
		{
			string[] scopes = NavigationLocalization.Scopes;
			for (int i = 0; i < scopes.Length; i++)
			{
				string text;
				if (GameLocalization.TryGetSourceKey(scopes[i], english, out text))
				{
					return GameLocalization.Get(text, english);
				}
			}
			return english;
		}

		// Token: 0x040085B0 RID: 34224
		private static readonly string[] Scopes = new string[] { "navigation_ui", "portal_ui", "poi_arrival", "maps_ui", "poi", "interaction_objects" };
	}
}
