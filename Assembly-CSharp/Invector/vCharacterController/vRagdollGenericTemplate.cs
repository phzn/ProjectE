using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006B8 RID: 1720
	public class vRagdollGenericTemplate : ScriptableObject
	{
		// Token: 0x06002A3B RID: 10811 RVA: 0x000ED3E9 File Offset: 0x000EB5E9
		public Transform GetRoot(Transform rootTransform)
		{
			return this.GetBone(this.root, rootTransform);
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000ED3F8 File Offset: 0x000EB5F8
		public Transform GetLeftHips(Transform rootTransform)
		{
			return this.GetBone(this.leftHips, rootTransform);
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000ED407 File Offset: 0x000EB607
		public Transform GetLeftKnee(Transform rootTransform)
		{
			return this.GetBone(this.leftKnee, rootTransform);
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x000ED416 File Offset: 0x000EB616
		public Transform GetLeftFoot(Transform rootTransform)
		{
			return this.GetBone(this.leftFoot, rootTransform);
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000ED425 File Offset: 0x000EB625
		public Transform GetRightHips(Transform rootTransform)
		{
			return this.GetBone(this.rightHips, rootTransform);
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000ED434 File Offset: 0x000EB634
		public Transform GetRightKnee(Transform rootTransform)
		{
			return this.GetBone(this.rightKnee, rootTransform);
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000ED443 File Offset: 0x000EB643
		public Transform GetRightFoot(Transform rootTransform)
		{
			return this.GetBone(this.rightFoot, rootTransform);
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000ED452 File Offset: 0x000EB652
		public Transform GetLeftArm(Transform rootTransform)
		{
			return this.GetBone(this.leftArm, rootTransform);
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000ED461 File Offset: 0x000EB661
		public Transform GetLeftElbow(Transform rootTransform)
		{
			return this.GetBone(this.leftElbow, rootTransform);
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000ED470 File Offset: 0x000EB670
		public Transform GetRightArm(Transform rootTransform)
		{
			return this.GetBone(this.rightArm, rootTransform);
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000ED47F File Offset: 0x000EB67F
		public Transform GetRightElbow(Transform rootTransform)
		{
			return this.GetBone(this.rightElbow, rootTransform);
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000ED48E File Offset: 0x000EB68E
		public Transform GetMiddleSpine(Transform rootTransform)
		{
			return this.GetBone(this.middleSpine, rootTransform);
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000ED49D File Offset: 0x000EB69D
		public Transform GetHead(Transform rootTransform)
		{
			return this.GetBone(this.head, rootTransform);
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000ED4AC File Offset: 0x000EB6AC
		private Transform GetBone(string boneName, Transform rootTransform)
		{
			Transform[] componentsInChildren = rootTransform.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.name.Contains(boneName))
				{
					return componentsInChildren[i];
				}
				if (componentsInChildren[i].gameObject.name.ToUpper().Contains(boneName))
				{
					return componentsInChildren[i];
				}
				if (componentsInChildren[i].gameObject.name.ToUpper().Contains(boneName.ToUpper()))
				{
					return componentsInChildren[i];
				}
				if (componentsInChildren[i].gameObject.name.ToLower().Contains(boneName.ToUpper()))
				{
					return componentsInChildren[i];
				}
				if (componentsInChildren[i].gameObject.name.ToLower().Contains(boneName.ToLower()))
				{
					return componentsInChildren[i];
				}
				if (componentsInChildren[i].gameObject.name.ToLower().Contains(boneName))
				{
					return componentsInChildren[i];
				}
			}
			return null;
		}

		// Token: 0x04002A50 RID: 10832
		[Header("--- Bones Names ---")]
		public string root = "Hips";

		// Token: 0x04002A51 RID: 10833
		public string leftHips = "LeftUpperLeg";

		// Token: 0x04002A52 RID: 10834
		public string leftKnee = "LeftLowerLeg";

		// Token: 0x04002A53 RID: 10835
		public string leftFoot = "LeftFoot";

		// Token: 0x04002A54 RID: 10836
		public string rightHips = "RightUpperLeg";

		// Token: 0x04002A55 RID: 10837
		public string rightKnee = "RightLowerLeg";

		// Token: 0x04002A56 RID: 10838
		public string rightFoot = "RightFoot";

		// Token: 0x04002A57 RID: 10839
		public string leftArm = "LeftUpperArm";

		// Token: 0x04002A58 RID: 10840
		public string leftElbow = "LeftLowerArm";

		// Token: 0x04002A59 RID: 10841
		public string rightArm = "RightUpperArm";

		// Token: 0x04002A5A RID: 10842
		public string rightElbow = "RightLowerArm";

		// Token: 0x04002A5B RID: 10843
		public string middleSpine = "Chest";

		// Token: 0x04002A5C RID: 10844
		public string head = "Head";
	}
}
