using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200004D RID: 77
	[RequireComponent(typeof(Rigidbody))]
	public class GPUISimpleVehicleController : GPUIInputHandler
	{
		// Token: 0x06000297 RID: 663 RVA: 0x0000EB84 File Offset: 0x0000CD84
		private void Awake()
		{
			this._rigidbody = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000EB94 File Offset: 0x0000CD94
		private void FixedUpdate()
		{
			this._rigidbody.AddRelativeTorque(Vector3.up * base.GetAxis("Horizontal") * this.engineTorque * Time.deltaTime);
			this._rigidbody.AddRelativeForce(Vector3.forward * base.GetAxis("Vertical") * this.enginePower * Time.deltaTime);
		}

		// Token: 0x040001FC RID: 508
		public float engineTorque = 3500f;

		// Token: 0x040001FD RID: 509
		public float enginePower = 4000f;

		// Token: 0x040001FE RID: 510
		private Rigidbody _rigidbody;
	}
}
