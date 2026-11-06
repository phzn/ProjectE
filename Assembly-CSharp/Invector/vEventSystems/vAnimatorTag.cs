using System;
using UnityEngine;

namespace Invector.vEventSystems
{
	// Token: 0x02000629 RID: 1577
	public class vAnimatorTag : vAnimatorTagBase
	{
		// Token: 0x06002426 RID: 9254 RVA: 0x000CB794 File Offset: 0x000C9994
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (this.stateInfos != null)
			{
				for (int i = 0; i < this.tags.Length; i++)
				{
					for (int j = 0; j < this.stateInfos.Count; j++)
					{
						this.stateInfos[j].AddStateInfo(this.tags[i], layerIndex);
					}
				}
			}
			this.OnStateEnterEvent(this.tags.vToList<string>());
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000CB808 File Offset: 0x000C9A08
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateUpdate(animator, stateInfo, layerIndex);
			if (this.stateInfos != null)
			{
				for (int i = 0; i < this.stateInfos.Count; i++)
				{
					this.stateInfos[i].UpdateStateInfo(layerIndex, stateInfo.normalizedTime, stateInfo.shortNameHash);
				}
			}
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000CB85C File Offset: 0x000C9A5C
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.stateInfos != null)
			{
				for (int i = 0; i < this.tags.Length; i++)
				{
					for (int j = 0; j < this.stateInfos.Count; j++)
					{
						this.stateInfos[j].RemoveStateInfo(this.tags[i], layerIndex);
					}
				}
			}
			base.OnStateExit(animator, stateInfo, layerIndex);
			this.OnStateExitEvent(this.tags.vToList<string>());
		}

		// Token: 0x04002409 RID: 9225
		public string[] tags = new string[] { "CustomAction" };
	}
}
