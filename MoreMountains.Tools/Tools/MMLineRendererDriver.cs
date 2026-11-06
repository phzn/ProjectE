using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200008A RID: 138
	[RequireComponent(typeof(LineRenderer))]
	public class MMLineRendererDriver : MonoBehaviour
	{
		// Token: 0x06000311 RID: 785 RVA: 0x0000FBFF File Offset: 0x0000DDFF
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000FC07 File Offset: 0x0000DE07
		protected virtual void Initialization()
		{
			this._lineRenderer = base.gameObject.GetComponent<LineRenderer>();
			this._countsMatch = this.CheckPositionCounts();
			if (!this._countsMatch)
			{
				Debug.LogWarning(base.name + ", MMLineRendererDriver's Targets list doesn't have the same amount of entries as the LineRender's Positions array. It won't work.");
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000FC43 File Offset: 0x0000DE43
		protected virtual void Update()
		{
			if (this.BindPositionsToTargetsAtUpdate)
			{
				this.BindPositionsToTargets();
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000FC53 File Offset: 0x0000DE53
		protected virtual void Bind()
		{
			this.Initialization();
			this.BindPositionsToTargets();
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000FC64 File Offset: 0x0000DE64
		public virtual void BindPositionsToTargets()
		{
			if (!this._countsMatch)
			{
				return;
			}
			for (int i = 0; i < this.Targets.Count; i++)
			{
				this._lineRenderer.SetPosition(i, this.Targets[i].position);
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000FCAD File Offset: 0x0000DEAD
		protected virtual bool CheckPositionCounts()
		{
			return this.Targets.Count == this._lineRenderer.positionCount;
		}

		// Token: 0x0400044C RID: 1100
		[Header("Position Drivers")]
		public List<Transform> Targets;

		// Token: 0x0400044D RID: 1101
		public bool BindPositionsToTargetsAtUpdate = true;

		// Token: 0x0400044E RID: 1102
		[Header("Binding")]
		[MMInspectorButton("Bind")]
		public bool BindButton;

		// Token: 0x0400044F RID: 1103
		protected LineRenderer _lineRenderer;

		// Token: 0x04000450 RID: 1104
		protected bool _countsMatch;
	}
}
