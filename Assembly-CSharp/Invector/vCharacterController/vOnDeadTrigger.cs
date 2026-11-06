using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController
{
	// Token: 0x02000695 RID: 1685
	public class vOnDeadTrigger : MonoBehaviour
	{
		// Token: 0x060027B8 RID: 10168 RVA: 0x000E2C74 File Offset: 0x000E0E74
		private void Start()
		{
			vCharacter component = base.GetComponent<vCharacter>();
			if (component)
			{
				component.onDead.AddListener(new UnityAction<GameObject>(this.OnDeadHandle));
			}
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000E2CA7 File Offset: 0x000E0EA7
		public void OnDeadHandle(GameObject target)
		{
			this.OnDead.Invoke();
		}

		// Token: 0x04002843 RID: 10307
		public UnityEvent OnDead;
	}
}
