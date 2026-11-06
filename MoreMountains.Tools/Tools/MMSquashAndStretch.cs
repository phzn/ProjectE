using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200006C RID: 108
	[AddComponentMenu("More Mountains/Tools/Movement/MMSquashAndStretch")]
	public class MMSquashAndStretch : MonoBehaviour
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000C113 File Offset: 0x0000A313
		public virtual float TimescaleTime
		{
			get
			{
				if (this.Timescale != MMSquashAndStretch.Timescales.Regular)
				{
					return Time.unscaledTime;
				}
				return Time.time;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000C128 File Offset: 0x0000A328
		public virtual float TimescaleDeltaTime
		{
			get
			{
				if (this.Timescale != MMSquashAndStretch.Timescales.Regular)
				{
					return Time.unscaledDeltaTime;
				}
				return Time.deltaTime;
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000C13D File Offset: 0x0000A33D
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000C148 File Offset: 0x0000A348
		protected virtual void Initialization()
		{
			this._initialScale = base.transform.localScale;
			this._springScale = this._initialScale;
			this._rigidbody = base.transform.parent.GetComponent<Rigidbody>();
			this._rigidbody2D = base.transform.parent.GetComponent<Rigidbody2D>();
			this._childTransform = base.transform.GetChild(0).transform;
			this._parentTransform = base.transform.parent.GetComponent<Transform>();
			this._previousPosition = this._parentTransform.position;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000C1DC File Offset: 0x0000A3DC
		protected virtual void LateUpdate()
		{
			this.SquashAndStretch();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000C1E4 File Offset: 0x0000A3E4
		protected virtual void SquashAndStretch()
		{
			if (this.TimescaleDeltaTime <= 0f)
			{
				return;
			}
			this.ComputeVelocityAndDirection();
			this.ComputeNewRotation();
			this.ComputeNewLocalScale();
			this.StorePreviousPosition();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000C20C File Offset: 0x0000A40C
		protected virtual void ComputeVelocityAndDirection()
		{
			this.Velocity = Vector3.zero;
			switch (this.Mode)
			{
			case MMSquashAndStretch.Modes.Rigidbody:
				this.Velocity = this._rigidbody.velocity;
				break;
			case MMSquashAndStretch.Modes.Rigidbody2D:
				this.Velocity = this._rigidbody2D.velocity;
				break;
			case MMSquashAndStretch.Modes.Position:
				this.Velocity = (this._previousPosition - this._parentTransform.position) / this.TimescaleDeltaTime;
				break;
			}
			this.VelocityMagnitude = this.Velocity.magnitude;
			this.RemappedVelocity = MMMaths.Remap(this.VelocityMagnitude, 0f, this.MaximumVelocity, 0f, 1f);
			this._direction = Vector3.Normalize(this.Velocity);
			if (this.AutoSquashOnStop)
			{
				if (this.VelocityMagnitude > this.SquashVelocityThreshold)
				{
					this._movementStarted = true;
					this._lastVelocity = Mathf.Clamp(this.VelocityMagnitude, 0f, this.MaximumVelocity);
					return;
				}
				if (this._movementStarted)
				{
					this._movementStarted = false;
					this._squashing = true;
					float num = MMMaths.Remap(this._lastVelocity, 0f, this.MaximumVelocity, this.SquashDuration.x, this.SquashDuration.y);
					float num2 = MMMaths.Remap(this._lastVelocity, 0f, this.MaximumVelocity, this.SquashIntensity.x, this.SquashIntensity.y);
					this.Squash(num, num2);
				}
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000C390 File Offset: 0x0000A590
		protected virtual void ComputeNewRotation()
		{
			if (!this.RotateToMatchDirection)
			{
				return;
			}
			if (this.VelocityMagnitude > 0.01f)
			{
				this._newRotation = Quaternion.FromToRotation(Vector3.up, this._direction);
			}
			this._deltaRotation = this._parentTransform.rotation;
			base.transform.rotation = this._newRotation;
			this._childTransform.rotation = this._deltaRotation;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000C3FC File Offset: 0x0000A5FC
		protected virtual void ComputeNewLocalScale()
		{
			if (this._squashing)
			{
				float num = MMMaths.Remap(this.TimescaleTime - this._squashStartedAt, 0f, this._squashDuration, 0f, 1f);
				float num2 = this.SquashCurve.Evaluate(num);
				this._newLocalScale.x = this._initialScale.x + num2 * this._squashIntensity;
				this._newLocalScale.y = this._initialScale.y - num2 * this._squashIntensity;
				this._newLocalScale.z = this._initialScale.z + num2 * this._squashIntensity;
				if (num >= 1f)
				{
					this._squashing = false;
				}
			}
			else
			{
				this._newLocalScale.x = Mathf.Clamp01(1f / (this.RemappedVelocity + 0.001f));
				this._newLocalScale.y = this.RemappedVelocity;
				this._newLocalScale.z = Mathf.Clamp01(1f / (this.RemappedVelocity + 0.001f));
				this._newLocalScale = Vector3.Lerp(Vector3.one, this._newLocalScale, this.VelocityMagnitude * this.Intensity);
			}
			this._newLocalScale.x = Mathf.Clamp(this._newLocalScale.x, this.MinimumScale.x, this.MaximumScale.x);
			this._newLocalScale.y = Mathf.Clamp(this._newLocalScale.y, this.MinimumScale.y, this.MaximumScale.y);
			this._newLocalScale.z = Mathf.Clamp(this._newLocalScale.z, this.MinimumScale.z, this.MaximumScale.z);
			if (this.Spring)
			{
				MMMaths.Spring(ref this._springScale, this._newLocalScale, ref this._springVelocity, this.SpringDamping, this.SpringFrequency, Time.deltaTime);
				this._newLocalScale = this._springScale;
			}
			if (!this.RescaleX)
			{
				this._newLocalScale.x = this._initialScale.x;
			}
			if (!this.RescaleY)
			{
				this._newLocalScale.y = this._initialScale.y;
			}
			if (!this.RescaleZ)
			{
				this._newLocalScale.z = this._initialScale.z;
			}
			base.transform.localScale = this._newLocalScale;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000C66C File Offset: 0x0000A86C
		protected virtual void StorePreviousPosition()
		{
			this._previousPosition = this._parentTransform.position;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000C67F File Offset: 0x0000A87F
		public virtual void Squash(float duration, float intensity)
		{
			this._squashStartedAt = this.TimescaleTime;
			this._squashing = true;
			this._squashIntensity = intensity;
			this._squashDuration = duration;
		}

		// Token: 0x0400038D RID: 909
		[MMInformation("This component will apply squash and stretch based on velocity (either position based or computed from a Rigidbody. It has to be put on an intermediary level in the hierarchy, between the logic (top level) and the model (bottom level).", MMInformationAttribute.InformationType.Info, false)]
		[Header("Velocity Detection")]
		[Tooltip("the possible ways to get velocity from")]
		public MMSquashAndStretch.Modes Mode = MMSquashAndStretch.Modes.Position;

		// Token: 0x0400038E RID: 910
		[Tooltip("whether we should use deltaTime or unscaledDeltaTime")]
		public MMSquashAndStretch.Timescales Timescale;

		// Token: 0x0400038F RID: 911
		[Header("Settings")]
		[Tooltip("the intensity of the squash and stretch")]
		public float Intensity = 0.02f;

		// Token: 0x04000390 RID: 912
		[Tooltip("the maximum velocity of your parent object, used to remap the computed one")]
		public float MaximumVelocity = 1f;

		// Token: 0x04000391 RID: 913
		[Header("Rescale")]
		[Tooltip("the minimum scale to apply to this object")]
		public Vector3 MinimumScale = new Vector3(0.5f, 0.5f, 0.5f);

		// Token: 0x04000392 RID: 914
		[Tooltip("the maximum scale to apply to this object")]
		public Vector3 MaximumScale = new Vector3(2f, 2f, 2f);

		// Token: 0x04000393 RID: 915
		[Tooltip("whether or not to rescale on the x axis")]
		public bool RescaleX = true;

		// Token: 0x04000394 RID: 916
		[Tooltip("whether or not to rescale on the y axis")]
		public bool RescaleY = true;

		// Token: 0x04000395 RID: 917
		[Tooltip("whether or not to rescale on the z axis")]
		public bool RescaleZ = true;

		// Token: 0x04000396 RID: 918
		[Tooltip("whether or not to rotate the transform to align with the current direction")]
		public bool RotateToMatchDirection = true;

		// Token: 0x04000397 RID: 919
		[Header("Squash")]
		[Tooltip("if this is true, the object will squash once velocity goes below the specified threshold")]
		public bool AutoSquashOnStop;

		// Token: 0x04000398 RID: 920
		[Tooltip("the curve to apply when squashing the object (this describes scale on x and z, will be inverted for y to maintain mass)")]
		public AnimationCurve SquashCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000399 RID: 921
		[Tooltip("the velocity threshold after which a squash can be triggered if the object stops")]
		public float SquashVelocityThreshold = 0.1f;

		// Token: 0x0400039A RID: 922
		[Tooltip("the maximum duration of the squash (will be reduced if velocity is low)")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 SquashDuration = new Vector2(0.25f, 0.5f);

		// Token: 0x0400039B RID: 923
		[Tooltip("the maximum intensity of the squash")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 SquashIntensity = new Vector2(0f, 1f);

		// Token: 0x0400039C RID: 924
		[Header("Spring")]
		[Tooltip("whether or not to add extra spring to the squash and stretch")]
		public bool Spring;

		// Token: 0x0400039D RID: 925
		[Tooltip("the damping to apply to the spring")]
		[MMCondition("Spring", true)]
		public float SpringDamping = 0.3f;

		// Token: 0x0400039E RID: 926
		[Tooltip("the spring's frequency")]
		[MMCondition("Spring", true)]
		public float SpringFrequency = 3f;

		// Token: 0x0400039F RID: 927
		[Header("Debug")]
		[MMReadOnly]
		[Tooltip("the current velocity of the parent object")]
		public Vector3 Velocity;

		// Token: 0x040003A0 RID: 928
		[MMReadOnly]
		[Tooltip("the remapped velocity")]
		public float RemappedVelocity;

		// Token: 0x040003A1 RID: 929
		[MMReadOnly]
		[Tooltip("the current velocity magnitude")]
		public float VelocityMagnitude;

		// Token: 0x040003A2 RID: 930
		protected Rigidbody2D _rigidbody2D;

		// Token: 0x040003A3 RID: 931
		protected Rigidbody _rigidbody;

		// Token: 0x040003A4 RID: 932
		protected Transform _childTransform;

		// Token: 0x040003A5 RID: 933
		protected Transform _parentTransform;

		// Token: 0x040003A6 RID: 934
		protected Vector3 _direction;

		// Token: 0x040003A7 RID: 935
		protected Vector3 _previousPosition;

		// Token: 0x040003A8 RID: 936
		protected Vector3 _newLocalScale;

		// Token: 0x040003A9 RID: 937
		protected Vector3 _initialScale;

		// Token: 0x040003AA RID: 938
		protected Quaternion _newRotation = Quaternion.identity;

		// Token: 0x040003AB RID: 939
		protected Quaternion _deltaRotation;

		// Token: 0x040003AC RID: 940
		protected float _squashStartedAt;

		// Token: 0x040003AD RID: 941
		protected bool _squashing;

		// Token: 0x040003AE RID: 942
		protected float _squashIntensity;

		// Token: 0x040003AF RID: 943
		protected float _squashDuration;

		// Token: 0x040003B0 RID: 944
		protected bool _movementStarted;

		// Token: 0x040003B1 RID: 945
		protected float _lastVelocity;

		// Token: 0x040003B2 RID: 946
		protected Vector3 _springScale;

		// Token: 0x040003B3 RID: 947
		protected Vector3 _springVelocity = Vector3.zero;

		// Token: 0x020003E2 RID: 994
		public enum Timescales
		{
			// Token: 0x04001E4A RID: 7754
			Regular,
			// Token: 0x04001E4B RID: 7755
			Unscaled
		}

		// Token: 0x020003E3 RID: 995
		public enum Modes
		{
			// Token: 0x04001E4D RID: 7757
			Rigidbody,
			// Token: 0x04001E4E RID: 7758
			Rigidbody2D,
			// Token: 0x04001E4F RID: 7759
			Position
		}
	}
}
