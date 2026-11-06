using System;
using UnityEngine;

namespace Invector.vEventSystems
{
	// Token: 0x0200062F RID: 1583
	public class vAnimatorTagByParamenter : vAnimatorTag
	{
		// Token: 0x06002440 RID: 9280 RVA: 0x000CBEE9 File Offset: 0x000CA0E9
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.paramenter == null)
			{
				this.paramenter = new vAnimatorParameter(animator, this.paramenterName);
			}
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000CBF05 File Offset: 0x000CA105
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateUpdate(animator, stateInfo, layerIndex);
			this.CheckForParamenter(animator, layerIndex);
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x000CBF18 File Offset: 0x000CA118
		private void CheckForParamenter(Animator animator, int layerIndex)
		{
			if (this.paramenter.isValid)
			{
				bool flag = false;
				switch (this.paramenterType)
				{
				case vAnimatorTagByParamenter.ParamenterType.Bool:
					flag = this.boolValue == animator.GetBool(this.paramenter);
					break;
				case vAnimatorTagByParamenter.ParamenterType.Float:
					flag = this.CompareNumber(this.floatValue, animator.GetFloat(this.paramenter), this.compare);
					break;
				case vAnimatorTagByParamenter.ParamenterType.Int:
					flag = this.CompareNumber((float)this.intValue, (float)animator.GetInteger(this.paramenter), this.compare);
					break;
				}
				if (flag != this.tagAdded)
				{
					this.tagAdded = flag;
					if (flag)
					{
						this.AddTags(layerIndex);
						return;
					}
					this.RemoveTags(layerIndex);
				}
			}
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x000CBFDC File Offset: 0x000CA1DC
		private void AddTags(int layerIndex)
		{
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
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x000CC034 File Offset: 0x000CA234
		private void RemoveTags(int layerIndex)
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
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x000CC08C File Offset: 0x000CA28C
		private bool CompareNumber(float a, float b, vAnimatorTagByParamenter.NumberCompare compare)
		{
			switch (compare)
			{
			case vAnimatorTagByParamenter.NumberCompare.Equals:
				Debug.Log(string.Format("{0} == {1}", b, a));
				return b == a;
			case vAnimatorTagByParamenter.NumberCompare.Greater:
				Debug.Log(string.Format("{0} > {1}", b, a));
				return b > a;
			case vAnimatorTagByParamenter.NumberCompare.Less:
				Debug.Log(string.Format("{0} < {1}", b, a));
				return b < a;
			default:
				return false;
			}
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x000CC10E File Offset: 0x000CA30E
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.tagAdded)
			{
				this.tagAdded = false;
				base.OnStateExit(animator, stateInfo, layerIndex);
			}
		}

		// Token: 0x0400241A RID: 9242
		public string paramenterName;

		// Token: 0x0400241B RID: 9243
		public vAnimatorTagByParamenter.ParamenterType paramenterType;

		// Token: 0x0400241C RID: 9244
		[vCheckProperty("paramenterType", new object[] { vAnimatorTagByParamenter.ParamenterType.Bool }, hideInInspector = true)]
		public bool boolValue;

		// Token: 0x0400241D RID: 9245
		[vCheckProperty("paramenterType", new object[] { vAnimatorTagByParamenter.ParamenterType.Float }, hideInInspector = true)]
		public float floatValue;

		// Token: 0x0400241E RID: 9246
		[vCheckProperty("paramenterType", new object[] { vAnimatorTagByParamenter.ParamenterType.Int }, hideInInspector = true)]
		public int intValue;

		// Token: 0x0400241F RID: 9247
		[vCheckProperty("paramenterType", new object[] { vAnimatorTagByParamenter.ParamenterType.Bool }, hideInInspector = true, invertResult = true)]
		public vAnimatorTagByParamenter.NumberCompare compare;

		// Token: 0x04002420 RID: 9248
		[vReadOnly(true)]
		public bool tagAdded;

		// Token: 0x04002421 RID: 9249
		private vAnimatorParameter paramenter;

		// Token: 0x02000630 RID: 1584
		public enum ParamenterType
		{
			// Token: 0x04002423 RID: 9251
			Bool,
			// Token: 0x04002424 RID: 9252
			Float,
			// Token: 0x04002425 RID: 9253
			Int
		}

		// Token: 0x02000631 RID: 1585
		public enum NumberCompare
		{
			// Token: 0x04002427 RID: 9255
			Equals,
			// Token: 0x04002428 RID: 9256
			Greater,
			// Token: 0x04002429 RID: 9257
			Less
		}
	}
}
