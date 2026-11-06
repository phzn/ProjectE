using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vEventSystems
{
	// Token: 0x0200061C RID: 1564
	[Serializable]
	public class vAnimatorStateInfos
	{
		// Token: 0x060023F6 RID: 9206 RVA: 0x000CACB6 File Offset: 0x000C8EB6
		public vAnimatorStateInfos(Animator animator)
		{
			this.animator = animator;
			this.Init();
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x000CACD8 File Offset: 0x000C8ED8
		public void Init()
		{
			if (this.animator)
			{
				this.stateInfos = new vAnimatorStateInfos.vStateInfo[this.animator.layerCount];
				for (int i = 0; i < this.stateInfos.Length; i++)
				{
					this.stateInfos[i] = new vAnimatorStateInfos.vStateInfo(i);
				}
			}
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000CAD2C File Offset: 0x000C8F2C
		public void RegisterListener()
		{
			if (this.animator == null)
			{
				return;
			}
			for (int i = 0; i < this.stateInfos.Length; i++)
			{
				vAnimatorStateInfos.vStateInfo vStateInfo = this.stateInfos[i];
				if (vStateInfo != null)
				{
					vStateInfo.tags.Clear();
					vStateInfo.normalizedTime = 0f;
					vStateInfo.layer = i;
					vStateInfo.shortPathHash = 0;
				}
				else
				{
					this.stateInfos[i] = new vAnimatorStateInfos.vStateInfo(i);
				}
			}
			vAnimatorTagBase[] behaviours = this.animator.GetBehaviours<vAnimatorTagBase>();
			for (int j = 0; j < behaviours.Length; j++)
			{
				behaviours[j].RemoveStateInfoListener(this);
				behaviours[j].AddStateInfoListener(this);
			}
			if (this.debug)
			{
				Debug.Log("Listeners Registered", this.animator);
			}
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000CADE0 File Offset: 0x000C8FE0
		public void RemoveListener()
		{
			if (this.animator)
			{
				vAnimatorTagBase[] behaviours = this.animator.GetBehaviours<vAnimatorTagBase>();
				for (int i = 0; i < behaviours.Length; i++)
				{
					behaviours[i].RemoveStateInfoListener(this);
				}
				if (this.debug)
				{
					Debug.Log("Listeners Removed", this.animator);
				}
			}
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000CAE38 File Offset: 0x000C9038
		public void AddStateInfo(string tag, int layer)
		{
			if (this.stateInfos.Length != 0 && layer < this.stateInfos.Length)
			{
				vAnimatorStateInfos.vStateInfo vStateInfo = this.stateInfos[layer];
				vStateInfo.tags.Add(tag);
				vStateInfo.shortPathHash = 0;
				vStateInfo.normalizedTime = 0f;
			}
			if (this.debug)
			{
				Debug.Log(string.Format("<color=green>Add tag : <b><i>{0}</i></b></color>,in the animator layer :{1}", tag, layer), this.animator);
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x000CAEA2 File Offset: 0x000C90A2
		public void UpdateStateInfo(int layer, float normalizedTime, int fullPathHash)
		{
			if (this.stateInfos.Length != 0 && layer < this.stateInfos.Length)
			{
				vAnimatorStateInfos.vStateInfo vStateInfo = this.stateInfos[layer];
				vStateInfo.normalizedTime = normalizedTime;
				vStateInfo.shortPathHash = fullPathHash;
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000CAED0 File Offset: 0x000C90D0
		public void RemoveStateInfo(string tag, int layer)
		{
			if (this.stateInfos.Length != 0 && layer < this.stateInfos.Length)
			{
				vAnimatorStateInfos.vStateInfo vStateInfo = this.stateInfos[layer];
				if (vStateInfo.tags.Contains(tag))
				{
					vStateInfo.tags.Remove(tag);
					if (vStateInfo.tags.Count == 0)
					{
						vStateInfo.shortPathHash = 0;
						vStateInfo.normalizedTime = 0f;
					}
					if (this.debug)
					{
						Debug.Log(string.Format("<color=red>Remove tag : <b><i>{0}</i></b></color>, in the animator layer :{1}", tag, layer), this.animator);
					}
				}
			}
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x000CAF58 File Offset: 0x000C9158
		public bool HasTag(string tag)
		{
			for (int i = 0; i < this.stateInfos.Length; i++)
			{
				if (this.stateInfos[i].tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000CAF90 File Offset: 0x000C9190
		public bool HasAllTags(params string[] tags)
		{
			bool flag = tags.Length != 0;
			for (int i = 0; i < tags.Length; i++)
			{
				if (!this.HasTag(tags[i]))
				{
					flag = false;
					break;
				}
			}
			return flag;
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000CAFC4 File Offset: 0x000C91C4
		public bool HasAnyTag(params string[] tags)
		{
			bool flag = false;
			for (int i = 0; i < tags.Length; i++)
			{
				if (this.HasTag(tags[i]))
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x000CAFF4 File Offset: 0x000C91F4
		public vAnimatorStateInfos.vStateInfo GetStateInfoUsingTag(string tag)
		{
			return Array.Find<vAnimatorStateInfos.vStateInfo>(this.stateInfos, (vAnimatorStateInfos.vStateInfo info) => info.tags.Contains(tag));
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x000CB025 File Offset: 0x000C9225
		public float GetCurrentNormalizedTime(int layer)
		{
			if (this.stateInfos.Length != 0 && layer < this.stateInfos.Length)
			{
				return this.stateInfos[layer].normalizedTime;
			}
			return 0f;
		}

		// Token: 0x040023EC RID: 9196
		public bool debug;

		// Token: 0x040023ED RID: 9197
		public Animator animator;

		// Token: 0x040023EE RID: 9198
		public vAnimatorStateInfos.vStateInfo[] stateInfos = new vAnimatorStateInfos.vStateInfo[0];

		// Token: 0x0200061D RID: 1565
		[Serializable]
		public class vStateInfo
		{
			// Token: 0x06002402 RID: 9218 RVA: 0x000CB04E File Offset: 0x000C924E
			public vStateInfo(int layer)
			{
				this.layer = layer;
			}

			// Token: 0x040023EF RID: 9199
			public int layer;

			// Token: 0x040023F0 RID: 9200
			public int shortPathHash;

			// Token: 0x040023F1 RID: 9201
			public float normalizedTime;

			// Token: 0x040023F2 RID: 9202
			public List<string> tags = new List<string>();
		}
	}
}
