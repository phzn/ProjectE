using System;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.Utils
{
	// Token: 0x02000614 RID: 1556
	public class vSetParentOfController : MonoBehaviour
	{
		// Token: 0x060023D5 RID: 9173 RVA: 0x000CA86D File Offset: 0x000C8A6D
		private void Start()
		{
			this.cc = base.GetComponentInParent<vThirdPersonController>();
			base.transform.parent = this.cc.transform;
			this.onStart.Invoke();
		}

		// Token: 0x040023D0 RID: 9168
		[vHelpBox("Set this GameObject as parent of the Controller", vHelpBoxAttribute.MessageType.None)]
		private vThirdPersonController cc;

		// Token: 0x040023D1 RID: 9169
		public UnityEvent onStart;
	}
}
