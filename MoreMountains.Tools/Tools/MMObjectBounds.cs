using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200010D RID: 269
	[AddComponentMenu("More Mountains/Tools/Object Bounds/MMObjectBounds")]
	public class MMObjectBounds : MonoBehaviour
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001CF44 File Offset: 0x0001B144
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0001CF4C File Offset: 0x0001B14C
		public virtual Vector3 Size { get; set; }

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001CF55 File Offset: 0x0001B155
		protected virtual void Reset()
		{
			this.DefineBoundsChoice();
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001CF60 File Offset: 0x0001B160
		protected virtual void DefineBoundsChoice()
		{
			this.BoundsBasedOn = MMObjectBounds.WaysToDetermineBounds.Undefined;
			if (base.GetComponent<Renderer>() != null)
			{
				this.BoundsBasedOn = MMObjectBounds.WaysToDetermineBounds.Renderer;
			}
			if (base.GetComponent<Collider>() != null)
			{
				this.BoundsBasedOn = MMObjectBounds.WaysToDetermineBounds.Collider;
			}
			if (base.GetComponent<Collider2D>() != null)
			{
				this.BoundsBasedOn = MMObjectBounds.WaysToDetermineBounds.Collider2D;
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0001CFB4 File Offset: 0x0001B1B4
		public virtual Bounds GetBounds()
		{
			if (this.BoundsBasedOn == MMObjectBounds.WaysToDetermineBounds.Renderer)
			{
				if (base.GetComponent<Renderer>() == null)
				{
					throw new Exception("The PoolableObject " + base.gameObject.name + " is set as having Renderer based bounds but no Renderer component can be found.");
				}
				return base.GetComponent<Renderer>().bounds;
			}
			else if (this.BoundsBasedOn == MMObjectBounds.WaysToDetermineBounds.Collider)
			{
				if (base.GetComponent<Collider>() == null)
				{
					throw new Exception("The PoolableObject " + base.gameObject.name + " is set as having Collider based bounds but no Collider component can be found.");
				}
				return base.GetComponent<Collider>().bounds;
			}
			else
			{
				if (this.BoundsBasedOn != MMObjectBounds.WaysToDetermineBounds.Collider2D)
				{
					return new Bounds(Vector3.zero, Vector3.zero);
				}
				if (base.GetComponent<Collider2D>() == null)
				{
					throw new Exception("The PoolableObject " + base.gameObject.name + " is set as having Collider2D based bounds but no Collider2D component can be found.");
				}
				return base.GetComponent<Collider2D>().bounds;
			}
		}

		// Token: 0x0400070E RID: 1806
		[Header("Bounds")]
		public MMObjectBounds.WaysToDetermineBounds BoundsBasedOn;

		// Token: 0x02000432 RID: 1074
		public enum WaysToDetermineBounds
		{
			// Token: 0x04001F97 RID: 8087
			Collider,
			// Token: 0x04001F98 RID: 8088
			Collider2D,
			// Token: 0x04001F99 RID: 8089
			Renderer,
			// Token: 0x04001F9A RID: 8090
			Undefined
		}
	}
}
