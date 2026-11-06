using System;
using UnityEngine;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BE6 RID: 3046
	[DisallowMultipleComponent]
	public class TrashcanLidClamp : MonoBehaviour
	{
		// Token: 0x06005788 RID: 22408 RVA: 0x00222B00 File Offset: 0x00220D00
		private void Awake()
		{
			if (this.lidBone == null)
			{
				foreach (Transform transform in base.GetComponentsInChildren<Transform>(true))
				{
					if (transform.name == this.lidBoneName)
					{
						this.lidBone = transform;
						break;
					}
				}
			}
			if (this.lidBone == null)
			{
				Debug.LogError(string.Concat(new string[] { "[TrashcanLidClamp] Bone '", this.lidBoneName, "' non trovato sotto '", base.name, "'. Clamp disattivato." }), this);
				base.enabled = false;
			}
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x00222BA4 File Offset: 0x00220DA4
		private void LateUpdate()
		{
			if (this.lidBone == null)
			{
				return;
			}
			if (!this._hasClosed)
			{
				this._closedLocalRotation = this.lidBone.localRotation;
				this._hasClosed = true;
				return;
			}
			float num;
			Vector3 vector;
			(Quaternion.Inverse(this._closedLocalRotation) * this.lidBone.localRotation).ToAngleAxis(out num, out vector);
			if (num > 180f)
			{
				num = 360f - num;
				vector = -vector;
			}
			if (float.IsNaN(vector.x) || vector.sqrMagnitude < 1E-06f)
			{
				return;
			}
			if (num <= this.maxAngle)
			{
				return;
			}
			Quaternion quaternion = Quaternion.AngleAxis(this.maxAngle, vector);
			this.lidBone.localRotation = this._closedLocalRotation * quaternion;
		}

		// Token: 0x040057E7 RID: 22503
		[Tooltip("Il bone del coperchio (Waste_Can/WasteCan_Root/WasteCan_Lid). Auto-risolto per nome se lasciato vuoto.")]
		[SerializeField]
		private Transform lidBone;

		// Token: 0x040057E8 RID: 22504
		[Tooltip("Angolo massimo di apertura, in gradi, rispetto alla posa chiusa. Il coperchio non supererà mai questo valore.")]
		[Range(0f, 180f)]
		[SerializeField]
		private float maxAngle = 90f;

		// Token: 0x040057E9 RID: 22505
		[Tooltip("Nome del bone del coperchio da cercare se 'lidBone' è vuoto.")]
		[SerializeField]
		private string lidBoneName = "WasteCan_Lid";

		// Token: 0x040057EA RID: 22506
		private Quaternion _closedLocalRotation;

		// Token: 0x040057EB RID: 22507
		private bool _hasClosed;
	}
}
