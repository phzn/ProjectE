using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020002E0 RID: 736
	[HelpURL("https://arongranberg.com/astar/documentation/stable/animationlinktraverser.html")]
	public class AnimationLinkTraverser : VersionedMonoBehaviour
	{
		// Token: 0x06001242 RID: 4674 RVA: 0x00070594 File Offset: 0x0006E794
		private void OnEnable()
		{
			this.ai = base.GetComponent<RichAI>();
			if (this.ai != null)
			{
				RichAI richAI = this.ai;
				richAI.onTraverseOffMeshLink = (Func<RichSpecial, IEnumerator>)Delegate.Combine(richAI.onTraverseOffMeshLink, new Func<RichSpecial, IEnumerator>(this.TraverseOffMeshLink));
			}
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x000705E3 File Offset: 0x0006E7E3
		private void OnDisable()
		{
			if (this.ai != null)
			{
				RichAI richAI = this.ai;
				richAI.onTraverseOffMeshLink = (Func<RichSpecial, IEnumerator>)Delegate.Remove(richAI.onTraverseOffMeshLink, new Func<RichSpecial, IEnumerator>(this.TraverseOffMeshLink));
			}
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0007061B File Offset: 0x0006E81B
		protected virtual IEnumerator TraverseOffMeshLink(RichSpecial rs)
		{
			AnimationLink link = rs.nodeLink.gameObject.GetComponent<AnimationLink>();
			if (link == null)
			{
				Debug.LogError("Unhandled RichSpecial");
				yield break;
			}
			for (;;)
			{
				Quaternion rotation = this.ai.rotation;
				Quaternion quaternion = this.ai.SimulateRotationTowards(rs.first.rotation * Vector3.forward, this.ai.rotationSpeed * Time.deltaTime);
				if (rotation == quaternion)
				{
					break;
				}
				this.ai.FinalizeMovement(this.ai.position, quaternion);
				yield return null;
			}
			base.transform.parent.position = base.transform.position;
			base.transform.parent.rotation = base.transform.rotation;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			if (rs.reverse && link.reverseAnim)
			{
				this.anim[link.clip].speed = -link.animSpeed;
				this.anim[link.clip].normalizedTime = 1f;
				this.anim.Play(link.clip);
				this.anim.Sample();
			}
			else
			{
				this.anim[link.clip].speed = link.animSpeed;
				this.anim.Rewind(link.clip);
				this.anim.Play(link.clip);
			}
			base.transform.parent.position -= base.transform.position - base.transform.parent.position;
			yield return new WaitForSeconds(Mathf.Abs(this.anim[link.clip].length / link.animSpeed));
			yield break;
		}

		// Token: 0x04000D77 RID: 3447
		public Animation anim;

		// Token: 0x04000D78 RID: 3448
		private RichAI ai;
	}
}
