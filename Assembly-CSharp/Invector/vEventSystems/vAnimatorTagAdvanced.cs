using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vEventSystems
{
	// Token: 0x0200062A RID: 1578
	public class vAnimatorTagAdvanced : vAnimatorTagBase
	{
		// Token: 0x0600242A RID: 9258 RVA: 0x000CB8EC File Offset: 0x000C9AEC
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (this.stateInfos != null)
			{
				for (int i = 0; i < this.tags.Count; i++)
				{
					this.tags[i].Init(this.stateInfos, layerIndex);
					if (this.debug)
					{
						Debug.Log("Init " + this.tags[i].tagName + " OnStateEnter  ");
					}
					if (this.tags[i].tagType == vAnimatorTagAdvanced.vAnimatorEventTriggerType.EnterStateExitState || this.tags[i].tagType == vAnimatorTagAdvanced.vAnimatorEventTriggerType.EnterStateExitByNormalized)
					{
						if (this.debug)
						{
							Debug.Log("ADD TAG " + this.tags[i].tagName + " OnStateEnter  ");
						}
						this.tags[i].AddTag(this.stateInfos, layerIndex);
					}
					else
					{
						this.tags[i].UpdateEventTrigger(stateInfo.normalizedTime, this.stateInfos, layerIndex, animator.speed, stateInfo.loop, false, this.debug);
					}
				}
			}
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000CBA10 File Offset: 0x000C9C10
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.stateInfos != null)
			{
				for (int i = 0; i < this.tags.Count; i++)
				{
					if (this.tags[i].tagType != vAnimatorTagAdvanced.vAnimatorEventTriggerType.EnterStateExitState)
					{
						this.tags[i].UpdateEventTrigger(stateInfo.normalizedTime, this.stateInfos, layerIndex, animator.speed, stateInfo.loop, false, this.debug);
					}
				}
				for (int j = 0; j < this.stateInfos.Count; j++)
				{
					this.stateInfos[j].UpdateStateInfo(layerIndex, stateInfo.normalizedTime, stateInfo.shortNameHash);
				}
			}
			base.OnStateUpdate(animator, stateInfo, layerIndex);
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x000CBAC4 File Offset: 0x000C9CC4
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.stateInfos != null)
			{
				for (int i = 0; i < this.tags.Count; i++)
				{
					if (this.tags[i].tagType == vAnimatorTagAdvanced.vAnimatorEventTriggerType.EnterStateExitState || this.tags[i].tagType == vAnimatorTagAdvanced.vAnimatorEventTriggerType.EnterByNormalizedExitState)
					{
						if (this.debug)
						{
							Debug.Log("REMOVE TAG " + this.tags[i].tagName + " OnStateExit  ");
						}
						this.tags[i].RemoveTag(this.stateInfos, layerIndex);
					}
					else
					{
						this.tags[i].UpdateEventTrigger(stateInfo.normalizedTime, this.stateInfos, layerIndex, animator.speed, stateInfo.loop, true, this.debug);
					}
				}
			}
			base.OnStateExit(animator, stateInfo, layerIndex);
		}

		// Token: 0x0400240A RID: 9226
		public bool debug;

		// Token: 0x0400240B RID: 9227
		public List<vAnimatorTagAdvanced.vAdvancedTags> tags = new List<vAnimatorTagAdvanced.vAdvancedTags>
		{
			new vAnimatorTagAdvanced.vAdvancedTags("CustomAction")
		};

		// Token: 0x0200062B RID: 1579
		public enum vAnimatorEventTriggerType
		{
			// Token: 0x0400240D RID: 9229
			AllByNormalizedTime,
			// Token: 0x0400240E RID: 9230
			EnterStateExitByNormalized,
			// Token: 0x0400240F RID: 9231
			EnterByNormalizedExitState,
			// Token: 0x04002410 RID: 9232
			EnterStateExitState
		}

		// Token: 0x0200062C RID: 1580
		[Serializable]
		public class vAdvancedTags
		{
			// Token: 0x0600242E RID: 9262 RVA: 0x000CBBC6 File Offset: 0x000C9DC6
			public vAdvancedTags(string tag)
			{
				this.tagName = tag;
				this.tagType = vAnimatorTagAdvanced.vAnimatorEventTriggerType.AllByNormalizedTime;
			}

			// Token: 0x0600242F RID: 9263 RVA: 0x000CBBF4 File Offset: 0x000C9DF4
			public void UpdateEventTrigger(float normalizedTime, List<vAnimatorStateInfos> stateInfos, int layer, float speed = 1f, bool looping = false, bool inExit = false, bool debug = false)
			{
				float num = normalizedTime % 1f;
				if (!this.isEnter && !inExit && this.tagType != vAnimatorTagAdvanced.vAnimatorEventTriggerType.EnterStateExitByNormalized && this.tagType != vAnimatorTagAdvanced.vAnimatorEventTriggerType.EnterStateExitState && num >= this.normalizedTime.x)
				{
					if (debug)
					{
						Debug.Log("ADD TAG " + this.tagName + " in  " + normalizedTime.ToString());
					}
					this.AddTag(stateInfos, layer);
				}
				if (!this.isExit && this.isEnter && this.tagType != vAnimatorTagAdvanced.vAnimatorEventTriggerType.EnterByNormalizedExitState && this.tagType != vAnimatorTagAdvanced.vAnimatorEventTriggerType.EnterStateExitState && (num >= this.normalizedTime.y || inExit))
				{
					this.RemoveTag(stateInfos, layer);
					if (debug)
					{
						Debug.Log("REMOVE TAG " + this.tagName + " in  " + normalizedTime.ToString());
					}
				}
				if (looping && normalizedTime > (float)(this.loopCount + 1))
				{
					this.isEnter = false;
					this.isExit = false;
					this.loopCount++;
				}
			}

			// Token: 0x06002430 RID: 9264 RVA: 0x000CBCF4 File Offset: 0x000C9EF4
			public void AddTag(List<vAnimatorStateInfos> stateInfos, int layer)
			{
				for (int i = 0; i < stateInfos.Count; i++)
				{
					stateInfos[i].AddStateInfo(this.tagName, layer);
				}
				this.isEnter = true;
			}

			// Token: 0x06002431 RID: 9265 RVA: 0x000CBD2C File Offset: 0x000C9F2C
			public void RemoveTag(List<vAnimatorStateInfos> stateInfos, int layer)
			{
				for (int i = 0; i < stateInfos.Count; i++)
				{
					stateInfos[i].RemoveStateInfo(this.tagName, layer);
					this.isExit = true;
				}
			}

			// Token: 0x06002432 RID: 9266 RVA: 0x000CBD64 File Offset: 0x000C9F64
			public void Init(List<vAnimatorStateInfos> stateInfos, int layer)
			{
				if (this.isEnter && !this.isExit)
				{
					this.RemoveTag(stateInfos, layer);
				}
				this.isEnter = false;
				this.isExit = false;
				this.loopCount = 0;
			}

			// Token: 0x04002411 RID: 9233
			public string tagName;

			// Token: 0x04002412 RID: 9234
			public vAnimatorTagAdvanced.vAnimatorEventTriggerType tagType;

			// Token: 0x04002413 RID: 9235
			public Vector2 normalizedTime = new Vector2(0.1f, 0.8f);

			// Token: 0x04002414 RID: 9236
			private int loopCount;

			// Token: 0x04002415 RID: 9237
			private bool isEnter;

			// Token: 0x04002416 RID: 9238
			private bool isExit;
		}
	}
}
