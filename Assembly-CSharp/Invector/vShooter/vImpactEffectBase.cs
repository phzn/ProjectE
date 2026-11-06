using System;
using UnityEngine;

namespace Invector.vShooter
{
	// Token: 0x02000652 RID: 1618
	public abstract class vImpactEffectBase : ScriptableObject
	{
		// Token: 0x06002534 RID: 9524
		public abstract void DoImpactEffect(Vector3 position, Quaternion rotation, GameObject sender, GameObject receiver);
	}
}
