using System;
using UnityEngine.Events;

namespace UnityEngine.Localization.PropertyVariants.TrackedObjects
{
	// Token: 0x02000103 RID: 259
	[CustomTrackedObject(typeof(MonoBehaviour), true)]
	[Serializable]
	public class TrackedMonoBehaviourObject : JsonSerializerTrackedObject
	{
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x00016A4A File Offset: 0x00014C4A
		public UnityEvent Changed
		{
			get
			{
				return this.m_Changed;
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00016A52 File Offset: 0x00014C52
		protected override void PostApplyTrackedProperties()
		{
			base.PostApplyTrackedProperties();
			this.m_Changed.Invoke();
		}

		// Token: 0x04000236 RID: 566
		[SerializeField]
		private UnityEvent m_Changed = new UnityEvent();
	}
}
