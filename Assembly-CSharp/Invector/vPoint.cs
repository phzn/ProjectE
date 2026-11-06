using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004E0 RID: 1248
	[Serializable]
	public class vPoint : MonoBehaviour
	{
		// Token: 0x06001E10 RID: 7696 RVA: 0x000B711E File Offset: 0x000B531E
		private void Start()
		{
			this.visitors = new List<Transform>();
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001E11 RID: 7697 RVA: 0x00008891 File Offset: 0x00006A91
		// (set) Token: 0x06001E12 RID: 7698 RVA: 0x000B712B File Offset: 0x000B532B
		public Vector3 position
		{
			get
			{
				return base.transform.position;
			}
			set
			{
				base.transform.position = value;
			}
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000B7139 File Offset: 0x000B5339
		public virtual void Enter(Transform visitor)
		{
			if (!this.visitors.Contains(visitor))
			{
				this.visitors.Add(visitor);
			}
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000B7155 File Offset: 0x000B5355
		public virtual void Exit(Transform visitor)
		{
			if (this.visitors.Contains(visitor))
			{
				this.visitors.Remove(visitor);
			}
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000B7172 File Offset: 0x000B5372
		public virtual bool IsOnWay(Transform visitor)
		{
			return this.visitors.Contains(visitor);
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000B7180 File Offset: 0x000B5380
		public virtual bool CanEnter(Transform visitor)
		{
			return this.visitors.Contains(visitor) || (this.isValid && this.visitors.Count != this.maxVisitors);
		}

		// Token: 0x04001F11 RID: 7953
		public float timeToStay;

		// Token: 0x04001F12 RID: 7954
		public int maxVisitors = 1;

		// Token: 0x04001F13 RID: 7955
		public bool isValid = true;

		// Token: 0x04001F14 RID: 7956
		public List<Transform> visitors;

		// Token: 0x04001F15 RID: 7957
		public float areaRadius = 1f;
	}
}
