using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x0200046B RID: 1131
	[vClassHeader("Body Snapping Control", true, "icon_v2", false, "", openClose = false)]
	public class vBodySnappingControl : vMonoBehaviour
	{
		// Token: 0x06001BF4 RID: 7156 RVA: 0x000AF7CD File Offset: 0x000AD9CD
		protected virtual void Reset()
		{
			this.LoadBones();
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000AF7D8 File Offset: 0x000AD9D8
		public virtual void LoadBones()
		{
			Animator componentInParent = base.GetComponentInParent<Animator>();
			List<vBodyStruct.Bone> bones = (this.bodyStruct ? this.bodyStruct.bones : vBodyStruct.GetHumanBones());
			if (this.bodyStruct)
			{
				List<vBodySnappingControl.vBoneTransformSnapping> list = this.boneSnappingList.FindAll((vBodySnappingControl.vBoneTransformSnapping _b) => !bones.Exists((vBodyStruct.Bone _b2) => _b2.name.Equals(_b.name)));
				for (int j = 0; j < list.Count; j++)
				{
					this.boneSnappingList.Remove(list[j]);
				}
			}
			if (bones.Count > 0)
			{
				int i2;
				int i;
				for (i = 0; i < bones.Count; i = i2 + 1)
				{
					Transform transform;
					if (bones[i].isHuman && componentInParent && componentInParent.isHuman)
					{
						transform = componentInParent.GetBoneTransform(bones[i].humanBone);
					}
					else
					{
						transform = this.GetBoneByName(bones[i].genericBone);
					}
					vBodySnappingControl.vBoneTransformSnapping vBoneTransformSnapping = this.boneSnappingList.Find((vBodySnappingControl.vBoneTransformSnapping _b) => _b.name.Equals(bones[i].name));
					if (vBoneTransformSnapping == null)
					{
						vBoneTransformSnapping = new vBodySnappingControl.vBoneTransformSnapping();
						vBoneTransformSnapping.name = bones[i].name;
						vBoneTransformSnapping.bone = transform;
						this.boneSnappingList.Add(vBoneTransformSnapping);
					}
					else
					{
						vBoneTransformSnapping.bone = transform;
					}
					i2 = i;
				}
			}
			this.boneSnappingList = (from x in this.boneSnappingList
				orderby x.bone != null && x.name.ToUpper().Contains("LEFT"), x.bone != null && x.name.ToUpper().Contains("RIGHT")
				select x).ToList<vBodySnappingControl.vBoneTransformSnapping>();
			if (!Application.isPlaying)
			{
				this.bonesIsLoaded = true;
			}
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000AFA0D File Offset: 0x000ADC0D
		protected virtual void Awake()
		{
			this.LoadBones();
			this.SnapAll();
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000AFA1C File Offset: 0x000ADC1C
		public virtual void SnapAll()
		{
			foreach (vBodySnappingControl.vBoneTransformSnapping vBoneTransformSnapping in this.boneSnappingList)
			{
				vBoneTransformSnapping.Snap();
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000AFA6C File Offset: 0x000ADC6C
		public virtual Transform GetBone(string name)
		{
			if (!this.bonesIsLoaded)
			{
				this.LoadBones();
			}
			vBodySnappingControl.vBoneTransformSnapping vBoneTransformSnapping = this.boneSnappingList.Find((vBodySnappingControl.vBoneTransformSnapping b) => b.name.Equals(name));
			if (vBoneTransformSnapping == null)
			{
				return null;
			}
			return vBoneTransformSnapping.bone;
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000AFAB8 File Offset: 0x000ADCB8
		protected virtual Transform GetBoneByName(string name)
		{
			Animator componentInParent = base.GetComponentInParent<Animator>();
			if (!componentInParent)
			{
				return null;
			}
			Transform transform = componentInParent.GetBoneTransform(HumanBodyBones.Hips);
			if (transform == null)
			{
				transform = componentInParent.transform;
			}
			List<Transform> list = transform.gameObject.GetComponentsInChildren<Transform>(true).vToList<Transform>();
			Transform transform2 = null;
			if (list.Count > 0 && !string.IsNullOrEmpty(name.Trim()))
			{
				string[] nameSplited = name.Trim().Split(';', StringSplitOptions.None);
				transform2 = list.Find((Transform child) => this.ContainsName(nameSplited, child.gameObject.name.Trim()));
			}
			return transform2;
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x000AFB50 File Offset: 0x000ADD50
		protected virtual bool ContainsName(string[] nameSplited, string targetName)
		{
			bool flag = false;
			for (int i = 0; i < nameSplited.Length; i++)
			{
				if (targetName.Contains(nameSplited[i]))
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		// Token: 0x04001D58 RID: 7512
		[vButton("Create New BodyStruct", "NewBodyStruct", typeof(vBodySnappingControl), false)]
		[vButton("Load Bones", "LoadBones", typeof(vBodySnappingControl), false)]
		public vBodyStruct bodyStruct;

		// Token: 0x04001D59 RID: 7513
		public bool showLabels;

		// Token: 0x04001D5A RID: 7514
		[HideInInspector]
		public List<vBodySnappingControl.vBoneTransformSnapping> boneSnappingList = new List<vBodySnappingControl.vBoneTransformSnapping>();

		// Token: 0x04001D5B RID: 7515
		private bool bonesIsLoaded;

		// Token: 0x0200046C RID: 1132
		[Serializable]
		public class vBoneTransformSnapping
		{
			// Token: 0x06001BFC RID: 7164 RVA: 0x000AFB90 File Offset: 0x000ADD90
			public void Snap()
			{
				if (this.bone && this.target)
				{
					if (Application.isPlaying && this.target.parent != this.bone)
					{
						this.target.parent = this.bone;
						this.onSnap.Invoke();
					}
					this.target.rotation = this.targetRotation;
					this.target.position = this.bone.position;
				}
			}

			// Token: 0x17000525 RID: 1317
			// (get) Token: 0x06001BFD RID: 7165 RVA: 0x000AFC1C File Offset: 0x000ADE1C
			public Quaternion targetRotation
			{
				get
				{
					Quaternion quaternion = Quaternion.LookRotation(Vector3.forward);
					Vector3 vector = Vector3.forward;
					if (this.bone && this.target && this.bone.parent)
					{
						switch (this.orientation)
						{
						case vBodySnappingControl.vBoneTransformSnapping.Orientation.Back:
							vector = Vector3.back;
							break;
						case vBodySnappingControl.vBoneTransformSnapping.Orientation.Right:
							vector = Vector3.right;
							break;
						case vBodySnappingControl.vBoneTransformSnapping.Orientation.Left:
							vector = Vector3.left;
							break;
						case vBodySnappingControl.vBoneTransformSnapping.Orientation.Up:
							vector = Vector3.up;
							break;
						case vBodySnappingControl.vBoneTransformSnapping.Orientation.Down:
							vector = Vector3.down;
							break;
						}
						quaternion = Quaternion.LookRotation(this.bone.TransformDirection(vector), this.bone.up);
					}
					return quaternion;
				}
			}

			// Token: 0x04001D5C RID: 7516
			public string name;

			// Token: 0x04001D5D RID: 7517
			public Transform bone;

			// Token: 0x04001D5E RID: 7518
			public Transform target;

			// Token: 0x04001D5F RID: 7519
			public vBodySnappingControl.vBoneTransformSnapping.Orientation orientation;

			// Token: 0x04001D60 RID: 7520
			public UnityEvent onSnap;

			// Token: 0x0200046D RID: 1133
			public enum Orientation
			{
				// Token: 0x04001D62 RID: 7522
				Forward,
				// Token: 0x04001D63 RID: 7523
				Back,
				// Token: 0x04001D64 RID: 7524
				Right,
				// Token: 0x04001D65 RID: 7525
				Left,
				// Token: 0x04001D66 RID: 7526
				Up,
				// Token: 0x04001D67 RID: 7527
				Down
			}
		}
	}
}
