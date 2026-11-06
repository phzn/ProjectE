using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000022 RID: 34
	public abstract class ColliderComponent : ClothBehaviour, IDataValidate, ITransform
	{
		// Token: 0x06000093 RID: 147
		public abstract ColliderManager.ColliderType GetColliderType();

		// Token: 0x06000094 RID: 148
		public abstract void DataValidate();

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000070A3 File Offset: 0x000052A3
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000070AB File Offset: 0x000052AB
		public ColliderSymmetryMode? ActiveSymmetryMode { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000070B4 File Offset: 0x000052B4
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000070BC File Offset: 0x000052BC
		public Transform ActiveSymmetryTarget { get; private set; }

		// Token: 0x06000099 RID: 153 RVA: 0x000070C5 File Offset: 0x000052C5
		public virtual Vector3 GetSize()
		{
			return this.size;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000070CD File Offset: 0x000052CD
		public void SetSize(Vector3 size)
		{
			this.size = size;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000070D6 File Offset: 0x000052D6
		public void SetSizeX(float size)
		{
			this.size.x = size;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000070E4 File Offset: 0x000052E4
		public void SetSizeY(float size)
		{
			this.size.y = size;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000070F2 File Offset: 0x000052F2
		public void SetSizeZ(float size)
		{
			this.size.z = size;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00007100 File Offset: 0x00005300
		public virtual float GetScale()
		{
			return base.transform.lossyScale.x;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002137 File Offset: 0x00000337
		public virtual bool IsReverseDirection()
		{
			return false;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00007112 File Offset: 0x00005312
		internal void Register(int teamId)
		{
			this.teamIdSet.Add(teamId);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00007121 File Offset: 0x00005321
		internal bool Exit(int teamId)
		{
			this.teamIdSet.Remove(teamId);
			return this.teamIdSet.Count == 0;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00007140 File Offset: 0x00005340
		public void UpdateParameters()
		{
			this.DataValidate();
			ColliderSymmetryMode? activeSymmetryMode = this.ActiveSymmetryMode;
			Transform activeSymmetryTarget = this.ActiveSymmetryTarget;
			this.SetActiveSymmetryMode(false);
			ColliderSymmetryMode? colliderSymmetryMode = activeSymmetryMode;
			ColliderSymmetryMode? activeSymmetryMode2 = this.ActiveSymmetryMode;
			bool flag = !((colliderSymmetryMode.GetValueOrDefault() == activeSymmetryMode2.GetValueOrDefault()) & (colliderSymmetryMode != null == (activeSymmetryMode2 != null))) || activeSymmetryTarget != this.ActiveSymmetryTarget;
			foreach (int num in this.teamIdSet)
			{
				MagicaManager.Collider.UpdateParameters(this, num, flag);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000071F4 File Offset: 0x000053F4
		public ColliderSymmetryMode CalcSymmetryMode(out Transform symmetryParent)
		{
			symmetryParent = this.symmetryTarget;
			Transform parent = base.transform.parent;
			if (parent == null)
			{
				return ColliderSymmetryMode.None;
			}
			ColliderSymmetryMode colliderSymmetryMode = this.symmetryMode;
			if (colliderSymmetryMode <= ColliderSymmetryMode.AutomaticTarget)
			{
				if (colliderSymmetryMode == ColliderSymmetryMode.None)
				{
					return ColliderSymmetryMode.None;
				}
				if (colliderSymmetryMode - ColliderSymmetryMode.AutomaticHumanBody <= 1)
				{
					Animator animator = ((this.symmetryMode == ColliderSymmetryMode.AutomaticHumanBody) ? base.gameObject.GetComponentInParent<Animator>(true) : null);
					Transform transform = ((this.symmetryMode == ColliderSymmetryMode.AutomaticTarget) ? this.symmetryTarget : null);
					if (transform == null && animator)
					{
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.Hips, HumanBodyBones.Hips);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.LeftUpperLeg, HumanBodyBones.RightUpperLeg);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.RightUpperLeg, HumanBodyBones.LeftUpperLeg);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.LeftLowerLeg, HumanBodyBones.RightLowerLeg);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.RightLowerLeg, HumanBodyBones.LeftLowerLeg);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.LeftFoot, HumanBodyBones.RightFoot);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.RightFoot, HumanBodyBones.LeftFoot);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.Spine, HumanBodyBones.Spine);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.Chest, HumanBodyBones.Chest);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.Neck, HumanBodyBones.Neck);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.Head, HumanBodyBones.Head);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.LeftShoulder, HumanBodyBones.RightShoulder);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.RightShoulder, HumanBodyBones.LeftShoulder);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.LeftUpperArm, HumanBodyBones.RightUpperArm);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.RightUpperArm, HumanBodyBones.LeftUpperArm);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.LeftLowerArm, HumanBodyBones.RightLowerArm);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.RightLowerArm, HumanBodyBones.LeftLowerArm);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.LeftHand, HumanBodyBones.RightHand);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.RightHand, HumanBodyBones.LeftHand);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.LeftToes, HumanBodyBones.RightToes);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.RightToes, HumanBodyBones.LeftToes);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.Jaw, HumanBodyBones.Jaw);
						this.GetHumanoidSymmetryBone(ref transform, parent, animator, HumanBodyBones.UpperChest, HumanBodyBones.UpperChest);
					}
					if (transform == null)
					{
						transform = parent;
					}
					symmetryParent = transform;
					bool flag = transform == parent;
					Vector3 right = parent.right;
					Vector3 up = parent.up;
					Vector3 forward = parent.forward;
					Vector3 right2 = transform.right;
					Vector3 up2 = transform.up;
					Vector3 forward2 = transform.forward;
					Vector3 vector = Vector3.right;
					if (animator)
					{
						vector = animator.transform.right;
					}
					else
					{
						Transform transform2 = this.FindCommonParent(base.transform, symmetryParent);
						if (transform2)
						{
							vector = transform2.right;
						}
					}
					float num = Mathf.Abs(Vector3.Dot(vector, right));
					float num2 = Mathf.Abs(Vector3.Dot(vector, up));
					float num3 = Mathf.Abs(Vector3.Dot(vector, forward));
					bool flag2 = Vector3.Dot(right, right2) >= 0f;
					bool flag3 = Vector3.Dot(up, up2) >= 0f;
					bool flag4 = Vector3.Dot(forward, forward2) >= 0f;
					if (num > num2 && num > num3)
					{
						if (flag)
						{
							return ColliderSymmetryMode.X_Symmetry;
						}
						if (Vector3.Dot(vector, right) * Vector3.Dot(vector, right2) > 0f)
						{
							if (!flag3 && !flag4)
							{
								return ColliderSymmetryMode.XYZ_Symmetry;
							}
							return ColliderSymmetryMode.X_Symmetry;
						}
						else
						{
							if (flag4)
							{
								return ColliderSymmetryMode.Y_Symmetry;
							}
							return ColliderSymmetryMode.Z_Symmetry;
						}
					}
					else if (num2 > num && num2 > num3)
					{
						if (flag)
						{
							return ColliderSymmetryMode.Y_Symmetry;
						}
						if (Vector3.Dot(vector, up) * Vector3.Dot(vector, up2) > 0f)
						{
							if (!flag2 && !flag4)
							{
								return ColliderSymmetryMode.XYZ_Symmetry;
							}
							return ColliderSymmetryMode.Y_Symmetry;
						}
						else
						{
							if (flag4)
							{
								return ColliderSymmetryMode.X_Symmetry;
							}
							return ColliderSymmetryMode.Z_Symmetry;
						}
					}
					else
					{
						if (flag)
						{
							return ColliderSymmetryMode.Z_Symmetry;
						}
						if (Vector3.Dot(vector, forward) * Vector3.Dot(vector, forward2) > 0f)
						{
							if (!flag2 && !flag3)
							{
								return ColliderSymmetryMode.XYZ_Symmetry;
							}
							return ColliderSymmetryMode.Z_Symmetry;
						}
						else
						{
							if (flag2)
							{
								return ColliderSymmetryMode.Y_Symmetry;
							}
							return ColliderSymmetryMode.X_Symmetry;
						}
					}
				}
			}
			else if (colliderSymmetryMode - ColliderSymmetryMode.X_Symmetry <= 2 || colliderSymmetryMode == ColliderSymmetryMode.XYZ_Symmetry)
			{
				if (symmetryParent == null)
				{
					symmetryParent = parent;
				}
				return this.symmetryMode;
			}
			object obj = "Unknown symmetry mode.";
			Develop.LogError(in obj);
			return ColliderSymmetryMode.None;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000075BC File Offset: 0x000057BC
		private bool GetHumanoidSymmetryBone(ref Transform target, Transform parent, Animator ani, HumanBodyBones src, HumanBodyBones dst)
		{
			Transform boneTransform = ani.GetBoneTransform(src);
			if (boneTransform && parent == boneTransform)
			{
				Transform boneTransform2 = ani.GetBoneTransform(dst);
				if (boneTransform2)
				{
					target = boneTransform2;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000075FC File Offset: 0x000057FC
		private Transform FindCommonParent(Transform at, Transform bt)
		{
			if (at == null || bt == null)
			{
				return null;
			}
			HashSet<Transform> hashSet = new HashSet<Transform>(16);
			Transform transform = at;
			while (transform != null)
			{
				hashSet.Add(transform);
				transform = transform.parent;
			}
			transform = bt;
			while (transform != null)
			{
				if (hashSet.Contains(transform))
				{
					return transform;
				}
				transform = transform.parent;
			}
			return null;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00007664 File Offset: 0x00005864
		internal void SetActiveSymmetryMode(bool firstOnly)
		{
			if (this.ActiveSymmetryMode == null || !firstOnly)
			{
				Transform transform;
				this.ActiveSymmetryMode = new ColliderSymmetryMode?(this.CalcSymmetryMode(out transform));
				this.ActiveSymmetryTarget = transform;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000769E File Offset: 0x0000589E
		public int UseTeamCount
		{
			get
			{
				return this.teamIdSet.Count;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000076AB File Offset: 0x000058AB
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			if (this.symmetryTarget)
			{
				transformSet.Add(this.symmetryTarget);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000076C8 File Offset: 0x000058C8
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			if (this.symmetryTarget)
			{
				int instanceID = this.symmetryTarget.GetInstanceID();
				if (instanceID != 0 && replaceDict.ContainsKey(instanceID))
				{
					this.symmetryTarget = replaceDict[instanceID];
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00007707 File Offset: 0x00005907
		protected virtual void Start()
		{
			this.SetActiveSymmetryMode(true);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00007710 File Offset: 0x00005910
		protected virtual void OnValidate()
		{
			this.UpdateParameters();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007718 File Offset: 0x00005918
		protected virtual void OnEnable()
		{
			foreach (int num in this.teamIdSet)
			{
				MagicaManager.Collider.EnableCollider(this, num, true);
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00007774 File Offset: 0x00005974
		protected virtual void OnDisable()
		{
			foreach (int num in this.teamIdSet)
			{
				ColliderManager collider = MagicaManager.Collider;
				if (collider != null)
				{
					collider.EnableCollider(this, num, false);
				}
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000077D4 File Offset: 0x000059D4
		protected virtual void OnDestroy()
		{
			if (this.teamIdSet.Count > 0)
			{
				foreach (int num in this.teamIdSet.ToList<int>())
				{
					ColliderManager collider = MagicaManager.Collider;
					if (collider != null)
					{
						collider.RemoveCollider(this, num);
					}
				}
				this.teamIdSet.Clear();
			}
		}

		// Token: 0x040000EE RID: 238
		public Vector3 center;

		// Token: 0x040000EF RID: 239
		[SerializeField]
		protected Vector3 size;

		// Token: 0x040000F0 RID: 240
		public ColliderSymmetryMode symmetryMode;

		// Token: 0x040000F1 RID: 241
		public Transform symmetryTarget;

		// Token: 0x040000F2 RID: 242
		private HashSet<int> teamIdSet = new HashSet<int>();
	}
}
