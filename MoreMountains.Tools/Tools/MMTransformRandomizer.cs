using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000097 RID: 151
	public class MMTransformRandomizer : MonoBehaviour
	{
		// Token: 0x0600033F RID: 831 RVA: 0x000105FA File Offset: 0x0000E7FA
		protected virtual void Awake()
		{
			if (Application.isPlaying && this.AutoExecutionMode == MMTransformRandomizer.AutoExecutionModes.OnAwake)
			{
				this.Randomize();
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00010612 File Offset: 0x0000E812
		protected virtual void Start()
		{
			if (Application.isPlaying && this.AutoExecutionMode == MMTransformRandomizer.AutoExecutionModes.OnStart)
			{
				this.Randomize();
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0001062A File Offset: 0x0000E82A
		protected virtual void OnEnable()
		{
			if (Application.isPlaying && this.AutoExecutionMode == MMTransformRandomizer.AutoExecutionModes.OnEnable)
			{
				this.Randomize();
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00010642 File Offset: 0x0000E842
		public virtual void Randomize()
		{
			this.ProcessRandomizePosition();
			this.ProcessRandomizeRotation();
			this.ProcessRandomizeScale();
			this.RemoveColliders();
			this.Cleanup();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00010664 File Offset: 0x0000E864
		protected virtual void ProcessRandomizePosition()
		{
			if (!this.RandomizePosition)
			{
				return;
			}
			Vector3 vector = MMMaths.RandomVector3(this.MinRandomPosition, this.MaxRandomPosition);
			base.transform.localPosition += vector;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000106A4 File Offset: 0x0000E8A4
		protected virtual void ProcessRandomizeRotation()
		{
			if (!this.RandomizeRotation)
			{
				return;
			}
			Vector3 vector = MMMaths.RandomVector3(this.MinRandomRotation, this.MaxRandomRotation);
			base.transform.localRotation = Quaternion.Euler(vector);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000106E0 File Offset: 0x0000E8E0
		protected virtual void ProcessRandomizeScale()
		{
			if (!this.RandomizeScale)
			{
				return;
			}
			Vector3 vector = MMMaths.RandomVector3(this.MinRandomScale, this.MaxRandomScale);
			base.transform.localScale = vector;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00010714 File Offset: 0x0000E914
		protected virtual void RemoveColliders()
		{
			bool removeAllColliders = this.RemoveAllColliders;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0001071D File Offset: 0x0000E91D
		protected virtual void Cleanup()
		{
			bool autoRemoveAfterRandomize = this.AutoRemoveAfterRandomize;
		}

		// Token: 0x0400046D RID: 1133
		[Header("Position")]
		public bool RandomizePosition = true;

		// Token: 0x0400046E RID: 1134
		[MMCondition("RandomizePosition", true)]
		public Vector3 MinRandomPosition;

		// Token: 0x0400046F RID: 1135
		[MMCondition("RandomizePosition", true)]
		public Vector3 MaxRandomPosition;

		// Token: 0x04000470 RID: 1136
		[Header("Rotation")]
		public bool RandomizeRotation = true;

		// Token: 0x04000471 RID: 1137
		[MMCondition("RandomizeRotation", true)]
		public Vector3 MinRandomRotation;

		// Token: 0x04000472 RID: 1138
		[MMCondition("RandomizeRotation", true)]
		public Vector3 MaxRandomRotation;

		// Token: 0x04000473 RID: 1139
		[Header("Scale")]
		public bool RandomizeScale = true;

		// Token: 0x04000474 RID: 1140
		[MMCondition("RandomizeScale", true)]
		public Vector3 MinRandomScale;

		// Token: 0x04000475 RID: 1141
		[MMCondition("RandomizeScale", true)]
		public Vector3 MaxRandomScale;

		// Token: 0x04000476 RID: 1142
		[Header("Settings")]
		public bool AutoRemoveAfterRandomize;

		// Token: 0x04000477 RID: 1143
		public bool RemoveAllColliders;

		// Token: 0x04000478 RID: 1144
		public MMTransformRandomizer.AutoExecutionModes AutoExecutionMode;

		// Token: 0x020003F2 RID: 1010
		public enum AutoExecutionModes
		{
			// Token: 0x04001E96 RID: 7830
			Never,
			// Token: 0x04001E97 RID: 7831
			OnAwake,
			// Token: 0x04001E98 RID: 7832
			OnStart,
			// Token: 0x04001E99 RID: 7833
			OnEnable
		}
	}
}
