using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001112 RID: 4370
	[CreateAssetMenu(fileName = "CollarIdPortraitPose", menuName = "Features/Inventory/Collar ID Portrait Pose")]
	public sealed class CollarIdPortraitPose : ScriptableObject
	{
		// Token: 0x060084A9 RID: 33961 RVA: 0x00361B78 File Offset: 0x0035FD78
		public IDisposable Apply(Animator animator)
		{
			if (animator == null)
			{
				throw new ArgumentNullException("animator");
			}
			if (animator.avatar == null || !animator.avatar.isValid || !animator.isHuman)
			{
				throw new ArgumentException("The ID portrait pose requires a valid humanoid Animator.", "animator");
			}
			if (this.rotations == null || this.rotations.Length == 0)
			{
				throw new InvalidOperationException("The ID portrait pose contains no baked bone rotations.");
			}
			return new CollarIdPortraitPose.RotationScope(animator, this.rotations);
		}

		// Token: 0x04008766 RID: 34662
		public const string ResourcePath = "CollarIdPortraitPose";

		// Token: 0x04008767 RID: 34663
		[SerializeField]
		private CollarIdPortraitPose.BoneRotation[] rotations = Array.Empty<CollarIdPortraitPose.BoneRotation>();

		// Token: 0x02001113 RID: 4371
		[Serializable]
		public struct BoneRotation
		{
			// Token: 0x04008768 RID: 34664
			public HumanBodyBones bone;

			// Token: 0x04008769 RID: 34665
			public Quaternion localRotation;
		}

		// Token: 0x02001114 RID: 4372
		private sealed class RotationScope : IDisposable
		{
			// Token: 0x060084AB RID: 33963 RVA: 0x00361C08 File Offset: 0x0035FE08
			public RotationScope(Animator animator, CollarIdPortraitPose.BoneRotation[] rotations)
			{
				Transform[] array = new Transform[rotations.Length];
				for (int i = 0; i < rotations.Length; i++)
				{
					CollarIdPortraitPose.BoneRotation boneRotation = rotations[i];
					if (boneRotation.bone < HumanBodyBones.Hips || boneRotation.bone >= HumanBodyBones.LastBone)
					{
						throw new InvalidOperationException("The ID portrait pose contains an invalid humanoid bone.");
					}
					Quaternion localRotation = boneRotation.localRotation;
					if (!CollarIdPortraitPose.RotationScope.Finite(localRotation.x) || !CollarIdPortraitPose.RotationScope.Finite(localRotation.y) || !CollarIdPortraitPose.RotationScope.Finite(localRotation.z) || !CollarIdPortraitPose.RotationScope.Finite(localRotation.w) || Quaternion.Dot(localRotation, localRotation) < 1E-06f)
					{
						throw new InvalidOperationException("The ID portrait pose contains an invalid bone rotation.");
					}
					Transform boneTransform = animator.GetBoneTransform(boneRotation.bone);
					array[i] = boneTransform;
					if (boneTransform != null && !this._originals.ContainsKey(boneTransform))
					{
						this._originals.Add(boneTransform, boneTransform.localRotation);
					}
				}
				if (this._originals.Count == 0)
				{
					throw new InvalidOperationException("The ID portrait pose has no bones mapped on this player.");
				}
				try
				{
					for (int j = 0; j < rotations.Length; j++)
					{
						if (array[j] != null)
						{
							array[j].localRotation = rotations[j].localRotation;
						}
					}
				}
				catch
				{
					this.Dispose();
					throw;
				}
			}

			// Token: 0x060084AC RID: 33964 RVA: 0x00361D68 File Offset: 0x0035FF68
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				foreach (KeyValuePair<Transform, Quaternion> keyValuePair in this._originals)
				{
					if (keyValuePair.Key != null)
					{
						keyValuePair.Key.localRotation = keyValuePair.Value;
					}
				}
				this._originals.Clear();
			}

			// Token: 0x060084AD RID: 33965 RVA: 0x0002920B File Offset: 0x0002740B
			private static bool Finite(float value)
			{
				return !float.IsNaN(value) && !float.IsInfinity(value);
			}

			// Token: 0x0400876A RID: 34666
			private readonly Dictionary<Transform, Quaternion> _originals = new Dictionary<Transform, Quaternion>();

			// Token: 0x0400876B RID: 34667
			private bool _disposed;
		}
	}
}
