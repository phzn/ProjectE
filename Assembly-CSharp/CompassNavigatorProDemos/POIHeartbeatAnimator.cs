using System;
using CompassNavigatorPro;
using UnityEngine;

namespace CompassNavigatorProDemos
{
	// Token: 0x020002B5 RID: 693
	public class POIHeartbeatAnimator : MonoBehaviour
	{
		// Token: 0x06001397 RID: 5015 RVA: 0x0007ED98 File Offset: 0x0007CF98
		private void Start()
		{
			this.poi = base.GetComponent<CompassProPOI>();
			if (this.poi == null)
			{
				Object.Destroy(this);
				return;
			}
			CompassProPOI compassProPOI = this.poi;
			compassProPOI.OnHeartbeat = (OnHeartbeatEvent)Delegate.Combine(compassProPOI.OnHeartbeat, new OnHeartbeatEvent(this.OnHeartbeatHandler));
			this.rb = base.GetComponent<Rigidbody>();
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0007EDFC File Offset: 0x0007CFFC
		private void OnHeartbeatHandler()
		{
			if (this.rb.IsSleeping())
			{
				this.rb.AddForce(Vector3.up * (100f + Random.value * 100f));
				this.rb.AddTorque(Random.onUnitSphere * 10f);
			}
		}

		// Token: 0x04001570 RID: 5488
		private CompassProPOI poi;

		// Token: 0x04001571 RID: 5489
		private Rigidbody rb;
	}
}
