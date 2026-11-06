using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200006E RID: 110
	[AddComponentMenu("More Mountains/Tools/Particles/MMAutoDestroyParticleSystem")]
	public class MMAutoDestroyParticleSystem : MonoBehaviour
	{
		// Token: 0x06000294 RID: 660 RVA: 0x0000C98C File Offset: 0x0000AB8C
		protected virtual void Start()
		{
			this._started = false;
			this._particleSystem = base.GetComponent<ParticleSystem>();
			if (this.DestroyDelay != 0f)
			{
				this._startTime = Time.time;
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000C9BC File Offset: 0x0000ABBC
		protected virtual void Update()
		{
			if (this.DestroyDelay != 0f && Time.time - this._startTime > this.DestroyDelay)
			{
				this.DestroyParticleSystem();
			}
			if (this._particleSystem.isPlaying)
			{
				this._started = true;
				return;
			}
			this.DestroyParticleSystem();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000CA0C File Offset: 0x0000AC0C
		protected virtual void DestroyParticleSystem()
		{
			if (!this._started)
			{
				return;
			}
			if (base.transform.parent != null && this.DestroyParent)
			{
				Object.Destroy(base.transform.parent.gameObject);
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x040003C2 RID: 962
		public bool DestroyParent;

		// Token: 0x040003C3 RID: 963
		public float DestroyDelay;

		// Token: 0x040003C4 RID: 964
		protected ParticleSystem _particleSystem;

		// Token: 0x040003C5 RID: 965
		protected float _startTime;

		// Token: 0x040003C6 RID: 966
		protected bool _started;
	}
}
