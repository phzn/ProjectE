using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000384 RID: 900
	[AddComponentMenu("")]
	public class FeelSpringsDemoManager : MonoBehaviour
	{
		// Token: 0x060018A0 RID: 6304 RVA: 0x00068BA6 File Offset: 0x00066DA6
		protected virtual void Start()
		{
			this.EnableCurrentDemo();
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00068BAE File Offset: 0x00066DAE
		public virtual void NextDemo()
		{
			this.CurrentIndex++;
			if (this.CurrentIndex >= this.DemoObjects.Count)
			{
				this.CurrentIndex = 0;
			}
			this.EnableCurrentDemo();
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x00068BDE File Offset: 0x00066DDE
		public virtual void PreviousDemo()
		{
			this.CurrentIndex--;
			if (this.CurrentIndex < 0)
			{
				this.CurrentIndex = this.DemoObjects.Count - 1;
			}
			this.EnableCurrentDemo();
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x00068C10 File Offset: 0x00066E10
		protected virtual void EnableCurrentDemo()
		{
			foreach (GameObject gameObject in this.DemoObjects)
			{
				gameObject.gameObject.SetActive(false);
			}
			this.DemoObjects[this.CurrentIndex].SetActive(true);
		}

		// Token: 0x04001CB7 RID: 7351
		[Header("Bindings")]
		public List<GameObject> DemoObjects;

		// Token: 0x04001CB8 RID: 7352
		[MMReadOnly]
		public int CurrentIndex;
	}
}
