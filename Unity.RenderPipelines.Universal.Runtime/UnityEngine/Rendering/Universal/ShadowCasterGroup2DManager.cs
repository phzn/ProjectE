using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200003B RID: 59
	internal class ShadowCasterGroup2DManager
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00011FC4 File Offset: 0x000101C4
		public static List<ShadowCasterGroup2D> shadowCasterGroups
		{
			get
			{
				return ShadowCasterGroup2DManager.s_ShadowCasterGroups;
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00011FCC File Offset: 0x000101CC
		public static void CacheValues()
		{
			if (ShadowCasterGroup2DManager.shadowCasterGroups != null)
			{
				for (int i = 0; i < ShadowCasterGroup2DManager.shadowCasterGroups.Count; i++)
				{
					if (ShadowCasterGroup2DManager.shadowCasterGroups[i] != null)
					{
						ShadowCasterGroup2DManager.shadowCasterGroups[i].CacheValues();
					}
				}
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00012018 File Offset: 0x00010218
		public static void AddShadowCasterGroupToList(ShadowCasterGroup2D shadowCaster, List<ShadowCasterGroup2D> list)
		{
			int num = 0;
			while (num < list.Count && shadowCaster.GetShadowGroup() != list[num].GetShadowGroup())
			{
				num++;
			}
			list.Insert(num, shadowCaster);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00012054 File Offset: 0x00010254
		public static void RemoveShadowCasterGroupFromList(ShadowCasterGroup2D shadowCaster, List<ShadowCasterGroup2D> list)
		{
			list.Remove(shadowCaster);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00012060 File Offset: 0x00010260
		private static CompositeShadowCaster2D FindTopMostCompositeShadowCaster(ShadowCaster2D shadowCaster)
		{
			CompositeShadowCaster2D compositeShadowCaster2D = null;
			Transform transform = shadowCaster.transform.parent;
			while (transform != null)
			{
				CompositeShadowCaster2D compositeShadowCaster2D2;
				if (transform.TryGetComponent<CompositeShadowCaster2D>(out compositeShadowCaster2D2))
				{
					compositeShadowCaster2D = compositeShadowCaster2D2;
				}
				transform = transform.parent;
			}
			return compositeShadowCaster2D;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0001209C File Offset: 0x0001029C
		public static bool AddToShadowCasterGroup(ShadowCaster2D shadowCaster, ref ShadowCasterGroup2D shadowCasterGroup)
		{
			ShadowCasterGroup2D shadowCasterGroup2D = ShadowCasterGroup2DManager.FindTopMostCompositeShadowCaster(shadowCaster);
			if (shadowCasterGroup2D == null)
			{
				shadowCaster.TryGetComponent<ShadowCasterGroup2D>(out shadowCasterGroup2D);
			}
			if (shadowCasterGroup2D != null && shadowCasterGroup != shadowCasterGroup2D)
			{
				shadowCasterGroup2D.RegisterShadowCaster2D(shadowCaster);
				shadowCasterGroup = shadowCasterGroup2D;
				return true;
			}
			return false;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x000120E2 File Offset: 0x000102E2
		public static void RemoveFromShadowCasterGroup(ShadowCaster2D shadowCaster, ShadowCasterGroup2D shadowCasterGroup)
		{
			if (shadowCasterGroup != null)
			{
				shadowCasterGroup.UnregisterShadowCaster2D(shadowCaster);
			}
			if (shadowCasterGroup == shadowCaster)
			{
				ShadowCasterGroup2DManager.RemoveGroup(shadowCasterGroup);
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00012103 File Offset: 0x00010303
		public static void AddGroup(ShadowCasterGroup2D group)
		{
			if (group == null)
			{
				return;
			}
			if (ShadowCasterGroup2DManager.s_ShadowCasterGroups == null)
			{
				ShadowCasterGroup2DManager.s_ShadowCasterGroups = new List<ShadowCasterGroup2D>();
			}
			ShadowCasterGroup2DManager.AddShadowCasterGroupToList(group, ShadowCasterGroup2DManager.s_ShadowCasterGroups);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0001212B File Offset: 0x0001032B
		public static void RemoveGroup(ShadowCasterGroup2D group)
		{
			if (group != null && ShadowCasterGroup2DManager.s_ShadowCasterGroups != null)
			{
				ShadowCasterGroup2DManager.RemoveShadowCasterGroupFromList(group, ShadowCasterGroup2DManager.s_ShadowCasterGroups);
			}
		}

		// Token: 0x04000194 RID: 404
		private static List<ShadowCasterGroup2D> s_ShadowCasterGroups;
	}
}
