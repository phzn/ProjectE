using System;
using UnityEngine;

namespace GPUInstancerPro.PrefabModule
{
	// Token: 0x02000011 RID: 17
	[DefaultExecutionOrder(-100)]
	[RequireComponent(typeof(GPUIPrefab))]
	public class GPUIRigidbodyReplacer : GPUIPrefabExtension
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00003490 File Offset: 0x00001690
		protected override void Start()
		{
			base.Start();
			GPUIRigidbodySimulator.InitializeInstance(this);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000349E File Offset: 0x0000169E
		internal void ReplaceRigidbody(Rigidbody rigidbody)
		{
			if (this.rigidbodyData == null)
			{
				this.rigidbodyData = new GPUIRigidbodyReplacer.GPUIRigidbodyData(rigidbody);
			}
			Object.Destroy(rigidbody);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000034BA File Offset: 0x000016BA
		internal void AddRigidbody()
		{
			if (this.rigidbodyData != null && !base.gameObject.HasComponent<Rigidbody>())
			{
				this.rigidbodyData.SetValuesToRigidbody(base.gameObject.AddComponent<Rigidbody>());
			}
		}

		// Token: 0x0400005A RID: 90
		[SerializeField]
		public GPUIRigidbodyReplacer.GPUIRigidbodyData rigidbodyData;

		// Token: 0x02000012 RID: 18
		[Serializable]
		public class GPUIRigidbodyData
		{
			// Token: 0x06000044 RID: 68 RVA: 0x000034F0 File Offset: 0x000016F0
			public GPUIRigidbodyData(Rigidbody rigidbody)
			{
				this.useGravity = rigidbody.useGravity;
				this.angularDrag = rigidbody.angularDrag;
				this.mass = rigidbody.mass;
				this.constraints = rigidbody.constraints;
				this.drag = rigidbody.drag;
				this.isKinematic = rigidbody.isKinematic;
				this.interpolation = rigidbody.interpolation;
			}

			// Token: 0x06000045 RID: 69 RVA: 0x00003558 File Offset: 0x00001758
			public void SetValuesToRigidbody(Rigidbody rigidbody)
			{
				rigidbody.useGravity = this.useGravity;
				rigidbody.angularDrag = this.angularDrag;
				rigidbody.mass = this.mass;
				rigidbody.constraints = this.constraints;
				rigidbody.detectCollisions = true;
				rigidbody.drag = this.drag;
				rigidbody.isKinematic = this.isKinematic;
				rigidbody.interpolation = this.interpolation;
			}

			// Token: 0x0400005B RID: 91
			public bool useGravity;

			// Token: 0x0400005C RID: 92
			public float angularDrag;

			// Token: 0x0400005D RID: 93
			public float mass;

			// Token: 0x0400005E RID: 94
			public RigidbodyConstraints constraints;

			// Token: 0x0400005F RID: 95
			public float drag;

			// Token: 0x04000060 RID: 96
			public bool isKinematic;

			// Token: 0x04000061 RID: 97
			public RigidbodyInterpolation interpolation;
		}
	}
}
