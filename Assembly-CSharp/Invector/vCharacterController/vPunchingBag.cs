using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController
{
	// Token: 0x020006AA RID: 1706
	public class vPunchingBag : MonoBehaviour
	{
		// Token: 0x06002985 RID: 10629 RVA: 0x000E9AF2 File Offset: 0x000E7CF2
		private void Start()
		{
			this._rigidbody = base.GetComponent<Rigidbody>();
			this.character = base.GetComponent<vHealthController>();
			this.character.onReceiveDamage.AddListener(new UnityAction<vDamage>(this.TakeDamage));
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000E9B28 File Offset: 0x000E7D28
		public void TakeDamage(vDamage damage)
		{
			Vector3 hitPosition = damage.hitPosition;
			Vector3 position = base.transform.position;
			position.y = hitPosition.y;
			Vector3 vector = position - hitPosition;
			if (this.character != null && this.joint != null && this.character.currentHealth < 0f)
			{
				this.joint.connectedBody = null;
				if (this.removeComponentsAfterDie)
				{
					foreach (MonoBehaviour monoBehaviour in this.character.gameObject.GetComponentsInChildren<MonoBehaviour>())
					{
						if (monoBehaviour != this)
						{
							Object.Destroy(monoBehaviour);
						}
					}
				}
			}
			if (this._rigidbody != null)
			{
				this._rigidbody.AddForce(vector * (damage.damageValue * this.forceMultipler), ForceMode.Impulse);
			}
		}

		// Token: 0x040029A7 RID: 10663
		public Rigidbody _rigidbody;

		// Token: 0x040029A8 RID: 10664
		public float forceMultipler = 0.5f;

		// Token: 0x040029A9 RID: 10665
		public SpringJoint joint;

		// Token: 0x040029AA RID: 10666
		public vHealthController character;

		// Token: 0x040029AB RID: 10667
		public bool removeComponentsAfterDie;
	}
}
