using System;
using UnityEngine;
using UnityEngine.UI;

namespace Invector.Utils
{
	// Token: 0x02000610 RID: 1552
	public class vRandomFloat : MonoBehaviour
	{
		// Token: 0x060023C4 RID: 9156 RVA: 0x000CA6C0 File Offset: 0x000C88C0
		private void Start()
		{
			if (this.setOnStart)
			{
				this.Set();
			}
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x000CA6D0 File Offset: 0x000C88D0
		public void Set()
		{
			if (this.randomValue)
			{
				this.onSet.Invoke(Random.Range(this.min, this.max));
				return;
			}
			this.onSet.Invoke(this.max);
		}

		// Token: 0x040023C6 RID: 9158
		public bool randomValue = true;

		// Token: 0x040023C7 RID: 9159
		[vHideInInspector("randomValue", false)]
		public float min;

		// Token: 0x040023C8 RID: 9160
		public float max;

		// Token: 0x040023C9 RID: 9161
		public bool setOnStart;

		// Token: 0x040023CA RID: 9162
		public Slider.SliderEvent onSet;
	}
}
