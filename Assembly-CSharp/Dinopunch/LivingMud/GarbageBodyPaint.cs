using System;
using Features.MainMenu;
using ProjectExhibitionist;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200073A RID: 1850
	public static class GarbageBodyPaint
	{
		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x0010EB67 File Offset: 0x0010CD67
		public static SkinDecal DefaultDecal
		{
			get
			{
				if (GarbageBodyPaint._defaults == null)
				{
					GarbageBodyPaint._defaults = Resources.Load<GarbageBodyPaintDefaults>("GarbageBodyPaintDefaults");
				}
				if (!(GarbageBodyPaint._defaults != null))
				{
					return null;
				}
				return GarbageBodyPaint._defaults.Decal;
			}
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x0010EBA0 File Offset: 0x0010CDA0
		public static bool TryApply(GameObject player, SkinDecal decal = null)
		{
			if (!GameSettingsService.GarbageMudBodyPaint || player == null)
			{
				return false;
			}
			if (decal == null)
			{
				decal = GarbageBodyPaint.DefaultDecal;
			}
			if (decal == null)
			{
				return false;
			}
			MudBodyPaintRuntimeController mudBodyPaintRuntimeController = player.GetComponentInChildren<MudBodyPaintRuntimeController>();
			if (mudBodyPaintRuntimeController == null)
			{
				mudBodyPaintRuntimeController = player.GetComponentInParent<MudBodyPaintRuntimeController>();
			}
			if (mudBodyPaintRuntimeController == null)
			{
				return false;
			}
			mudBodyPaintRuntimeController.ApplyTimedBodyPaint(decal);
			return true;
		}

		// Token: 0x04003058 RID: 12376
		private static GarbageBodyPaintDefaults _defaults;
	}
}
