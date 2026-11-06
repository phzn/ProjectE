using System;
using UnityEngine;

namespace Features.HideFlash
{
	// Token: 0x020011A2 RID: 4514
	public class BushCarryEffects : MonoBehaviour
	{
		// Token: 0x06008A34 RID: 35380 RVA: 0x00387C60 File Offset: 0x00385E60
		private void OnEnable()
		{
			if (this.visual == null && base.transform.childCount > 0)
			{
				this.visual = base.transform.GetChild(0);
			}
			if (this.visual != null)
			{
				this._visualRestRotation = this.visual.localRotation;
			}
			this._hasPrev = false;
			this._tilt = Vector2.zero;
			this._tiltVel = Vector2.zero;
			this._noiseSeed = Random.value * 100f;
		}

		// Token: 0x06008A35 RID: 35381 RVA: 0x00387CE8 File Offset: 0x00385EE8
		private void OnDisable()
		{
			if (this.visual != null)
			{
				this.visual.localRotation = this._visualRestRotation;
			}
		}

		// Token: 0x06008A36 RID: 35382 RVA: 0x00387D0C File Offset: 0x00385F0C
		private void LateUpdate()
		{
			if (this.visual == null)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			if (deltaTime <= 0f)
			{
				return;
			}
			Vector3 position = base.transform.position;
			if (!this._hasPrev)
			{
				this._prevPos = position;
				this._prevVel = Vector3.zero;
				this._hasPrev = true;
				return;
			}
			Vector3 vector = (position - this._prevPos) / deltaTime;
			Vector3 vector2 = (vector - this._prevVel) / deltaTime;
			this._prevPos = position;
			this._prevVel = vector;
			Vector3 vector3 = base.transform.InverseTransformDirection(vector2);
			Vector2 vector4 = (Vector2.ClampMagnitude(new Vector2(vector3.z, -vector3.x) * this.tiltPerAccel, this.maxTiltDegrees) - this._tilt) * this.springStiffness - this._tiltVel * this.springDamping;
			this._tiltVel += vector4 * deltaTime;
			this._tilt += this._tiltVel * deltaTime;
			float num = Mathf.Clamp01(new Vector2(vector.x, vector.z).magnitude / Mathf.Max(0.01f, this.speedForFullRustle));
			float num2 = Time.time * this.rustleFrequency;
			float num3 = (Mathf.PerlinNoise(this._noiseSeed, num2) - 0.5f) * 2f;
			float num4 = (Mathf.PerlinNoise(this._noiseSeed + 37f, num2) - 0.5f) * 2f;
			Vector2 vector5 = new Vector2(num3, num4) * (this.rustleAmplitude * num);
			this.visual.localRotation = this._visualRestRotation * Quaternion.Euler(this._tilt.x + vector5.x, 0f, this._tilt.y + vector5.y);
		}

		// Token: 0x04008CFC RID: 36092
		[Header("Visual target (auto: first child)")]
		[Tooltip("Transform that receives the wobble rotation. Auto-assigned to the first child if empty.")]
		[SerializeField]
		private Transform visual;

		// Token: 0x04008CFD RID: 36093
		[Header("Sway spring")]
		[Tooltip("Max tilt (degrees) the bush reaches under hard acceleration.")]
		[SerializeField]
		private float maxTiltDegrees = 14f;

		// Token: 0x04008CFE RID: 36094
		[Tooltip("Degrees of tilt per m/s² of acceleration.")]
		[SerializeField]
		private float tiltPerAccel = 2.2f;

		// Token: 0x04008CFF RID: 36095
		[Tooltip("Spring stiffness pulling the tilt back to rest (higher = snappier).")]
		[SerializeField]
		private float springStiffness = 70f;

		// Token: 0x04008D00 RID: 36096
		[Tooltip("Spring damping (higher = less oscillation).")]
		[SerializeField]
		private float springDamping = 9f;

		// Token: 0x04008D01 RID: 36097
		[Header("Speed rustle")]
		[Tooltip("Extra shake amplitude (degrees) at full speed.")]
		[SerializeField]
		private float rustleAmplitude = 2.5f;

		// Token: 0x04008D02 RID: 36098
		[Tooltip("Shake frequency in Hz.")]
		[SerializeField]
		private float rustleFrequency = 9f;

		// Token: 0x04008D03 RID: 36099
		[Tooltip("Carrier speed (m/s) at which the rustle reaches full amplitude.")]
		[SerializeField]
		private float speedForFullRustle = 4f;

		// Token: 0x04008D04 RID: 36100
		private Vector3 _prevPos;

		// Token: 0x04008D05 RID: 36101
		private Vector3 _prevVel;

		// Token: 0x04008D06 RID: 36102
		private bool _hasPrev;

		// Token: 0x04008D07 RID: 36103
		private Vector2 _tilt;

		// Token: 0x04008D08 RID: 36104
		private Vector2 _tiltVel;

		// Token: 0x04008D09 RID: 36105
		private Quaternion _visualRestRotation;

		// Token: 0x04008D0A RID: 36106
		private float _noiseSeed;
	}
}
