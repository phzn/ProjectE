using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vEventSystems
{
	// Token: 0x0200062D RID: 1581
	public abstract class vAnimatorTagBase : StateMachineBehaviour
	{
		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06002433 RID: 9267 RVA: 0x000CBD94 File Offset: 0x000C9F94
		// (remove) Token: 0x06002434 RID: 9268 RVA: 0x000CBDCC File Offset: 0x000C9FCC
		public event vAnimatorTagBase.OnStateTrigger onStateEnter;

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06002435 RID: 9269 RVA: 0x000CBE04 File Offset: 0x000CA004
		// (remove) Token: 0x06002436 RID: 9270 RVA: 0x000CBE3C File Offset: 0x000CA03C
		public event vAnimatorTagBase.OnStateTrigger onStateExit;

		// Token: 0x06002437 RID: 9271 RVA: 0x000CBE71 File Offset: 0x000CA071
		public virtual void AddStateInfoListener(vAnimatorStateInfos stateInfo)
		{
			if (!this.stateInfos.Contains(stateInfo))
			{
				this.stateInfos.Add(stateInfo);
			}
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000CBE8D File Offset: 0x000CA08D
		public virtual void RemoveStateInfoListener(vAnimatorStateInfos stateInfo)
		{
			if (this.stateInfos.Contains(stateInfo))
			{
				this.stateInfos.Remove(stateInfo);
			}
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000CBEAA File Offset: 0x000CA0AA
		protected virtual void OnStateEnterEvent(List<string> tags)
		{
			if (this.onStateEnter != null)
			{
				this.onStateEnter(tags);
			}
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000CBEC0 File Offset: 0x000CA0C0
		protected virtual void OnStateExitEvent(List<string> tags)
		{
			if (this.onStateEnter != null)
			{
				this.onStateExit(tags);
			}
		}

		// Token: 0x04002417 RID: 9239
		public List<vAnimatorStateInfos> stateInfos = new List<vAnimatorStateInfos>();

		// Token: 0x0200062E RID: 1582
		// (Invoke) Token: 0x0600243D RID: 9277
		public delegate void OnStateTrigger(List<string> tags);
	}
}
