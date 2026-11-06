using System;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A38 RID: 2616
	public sealed class ControllerNavigationScope : MonoBehaviour
	{
		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06004A43 RID: 19011 RVA: 0x001CCA76 File Offset: 0x001CAC76
		// (set) Token: 0x06004A44 RID: 19012 RVA: 0x001CCA7E File Offset: 0x001CAC7E
		public GameObject InitialFocus { get; private set; }

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06004A45 RID: 19013 RVA: 0x001CCA87 File Offset: 0x001CAC87
		// (set) Token: 0x06004A46 RID: 19014 RVA: 0x001CCA8F File Offset: 0x001CAC8F
		public Action Cancel { get; private set; }

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06004A47 RID: 19015 RVA: 0x001CCA98 File Offset: 0x001CAC98
		// (set) Token: 0x06004A48 RID: 19016 RVA: 0x001CCAA0 File Offset: 0x001CACA0
		public int Priority { get; private set; }

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06004A49 RID: 19017 RVA: 0x001CCAA9 File Offset: 0x001CACA9
		// (set) Token: 0x06004A4A RID: 19018 RVA: 0x001CCAB1 File Offset: 0x001CACB1
		public bool WaitForInitialFocus { get; private set; }

		// Token: 0x06004A4B RID: 19019 RVA: 0x001CCABC File Offset: 0x001CACBC
		public static ControllerNavigationScope Configure(GameObject root, Action cancel, GameObject initialFocus = null, int priority = 100, bool waitForInitialFocus = false)
		{
			if (root == null)
			{
				return null;
			}
			ControllerNavigationScope controllerNavigationScope = root.GetComponent<ControllerNavigationScope>();
			if (controllerNavigationScope == null)
			{
				controllerNavigationScope = root.AddComponent<ControllerNavigationScope>();
			}
			controllerNavigationScope.Cancel = cancel;
			controllerNavigationScope.InitialFocus = initialFocus;
			controllerNavigationScope.Priority = priority;
			controllerNavigationScope.WaitForInitialFocus = waitForInitialFocus;
			ControllerUINavigation.PrepareScope(controllerNavigationScope);
			return controllerNavigationScope;
		}
	}
}
