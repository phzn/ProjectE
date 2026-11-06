using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000079 RID: 121
	public class MMRigidbodyCenterOfMass : MonoBehaviour
	{
		// Token: 0x060002B8 RID: 696 RVA: 0x0000DD1E File Offset: 0x0000BF1E
		protected virtual void Awake()
		{
			this.Initialization();
			if (this.AutomaticSetMode == MMRigidbodyCenterOfMass.AutomaticSetModes.Awake)
			{
				this.SetCenterOfMass();
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000DD34 File Offset: 0x0000BF34
		protected virtual void Start()
		{
			if (this.AutomaticSetMode == MMRigidbodyCenterOfMass.AutomaticSetModes.Start)
			{
				this.SetCenterOfMass();
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000DD45 File Offset: 0x0000BF45
		protected virtual void Initialization()
		{
			this._rigidbody = base.gameObject.MMGetComponentNoAlloc<Rigidbody>();
			this._rigidbody2D = base.gameObject.MMGetComponentNoAlloc<Rigidbody2D>();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		public virtual void SetCenterOfMass()
		{
			if (this._rigidbody != null)
			{
				this._rigidbody.centerOfMass = this.CenterOfMassOffset;
			}
			if (this._rigidbody2D != null)
			{
				this._rigidbody2D.centerOfMass = this.CenterOfMassOffset;
			}
			if (this.AutoDestroyComponentAfterSet)
			{
				Object.Destroy(this);
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000DDCA File Offset: 0x0000BFCA
		protected virtual void OnDrawGizmosSelected()
		{
			this._gizmoCenter = base.transform.TransformPoint(this.CenterOfMassOffset);
			MMDebug.DrawGizmoPoint(this._gizmoCenter, this.GizmoPointSize, Color.yellow);
		}

		// Token: 0x040003EA RID: 1002
		[Header("CenterOfMass")]
		public Vector3 CenterOfMassOffset;

		// Token: 0x040003EB RID: 1003
		[Header("Automation")]
		public MMRigidbodyCenterOfMass.AutomaticSetModes AutomaticSetMode;

		// Token: 0x040003EC RID: 1004
		public bool AutoDestroyComponentAfterSet = true;

		// Token: 0x040003ED RID: 1005
		[Header("Test")]
		public float GizmoPointSize = 0.05f;

		// Token: 0x040003EE RID: 1006
		[MMInspectorButton("SetCenterOfMass")]
		public bool SetCenterOfMassButton;

		// Token: 0x040003EF RID: 1007
		protected Vector3 _gizmoCenter;

		// Token: 0x040003F0 RID: 1008
		protected Rigidbody _rigidbody;

		// Token: 0x040003F1 RID: 1009
		protected Rigidbody2D _rigidbody2D;

		// Token: 0x020003E8 RID: 1000
		public enum AutomaticSetModes
		{
			// Token: 0x04001E5F RID: 7775
			Awake,
			// Token: 0x04001E60 RID: 7776
			Start,
			// Token: 0x04001E61 RID: 7777
			ScriptOnly
		}
	}
}
