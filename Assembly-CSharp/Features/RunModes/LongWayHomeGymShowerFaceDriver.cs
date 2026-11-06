using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CBB RID: 3259
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(20005)]
	public sealed class LongWayHomeGymShowerFaceDriver : MonoBehaviour
	{
		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06005E90 RID: 24208 RVA: 0x00256BAE File Offset: 0x00254DAE
		public bool IsActive
		{
			get
			{
				return this._renderer != null;
			}
		}

		// Token: 0x06005E91 RID: 24209 RVA: 0x00256BBC File Offset: 0x00254DBC
		public void Begin(GameObject root, SkinnedMeshRenderer renderer)
		{
			this.Release();
			if (renderer == null || renderer.sharedMesh == null)
			{
				return;
			}
			this._renderer = renderer;
			this._mesh = renderer.sharedMesh;
		}

		// Token: 0x06005E92 RID: 24210 RVA: 0x00256BEF File Offset: 0x00254DEF
		public void Show(LongWayHomeFaceAnimationSO face)
		{
			if (face == null)
			{
				this.RestoreOwnedShapes();
				this._animation = null;
				return;
			}
			this.Bind(face);
			this._blending = false;
			this._time = 0f;
			this.ApplyCurrentPose();
		}

		// Token: 0x06005E93 RID: 24211 RVA: 0x00256C28 File Offset: 0x00254E28
		public void BlendTo(LongWayHomeFaceAnimationSO face, float seconds)
		{
			if (face == null)
			{
				return;
			}
			if (seconds <= 0f)
			{
				this.Show(face);
				return;
			}
			this.Bind(face);
			foreach (LongWayHomeGymShowerFaceDriver.OwnedShape ownedShape in this._owned)
			{
				ownedShape.BlendStart = (ownedShape.HasApplied ? ownedShape.LastApplied : ownedShape.Original);
			}
			this._time = (this._blendElapsed = 0f);
			this._blendSeconds = seconds;
			this._blending = true;
			this.ApplyCurrentPose();
		}

		// Token: 0x06005E94 RID: 24212 RVA: 0x00256CDC File Offset: 0x00254EDC
		private void LateUpdate()
		{
			this.Tick(Time.deltaTime);
		}

		// Token: 0x06005E95 RID: 24213 RVA: 0x00256CEC File Offset: 0x00254EEC
		public void Tick(float deltaTime)
		{
			if (this._renderer == null || this._animation == null)
			{
				return;
			}
			deltaTime = Mathf.Max(0f, deltaTime);
			this._time += deltaTime;
			if (this._blending)
			{
				this._blendElapsed += deltaTime;
				if (this._blendElapsed >= this._blendSeconds)
				{
					this._blending = false;
				}
			}
			this.ApplyCurrentPose();
		}

		// Token: 0x06005E96 RID: 24214 RVA: 0x00256D64 File Offset: 0x00254F64
		private void Bind(LongWayHomeFaceAnimationSO animation)
		{
			this.RefreshMesh();
			this._animation = animation;
			foreach (LongWayHomeGymShowerFaceDriver.OwnedShape ownedShape in this._owned)
			{
				ownedShape.Curve = null;
			}
			if (animation.curves == null)
			{
				return;
			}
			foreach (LongWayHomeFaceAnimationSO.CurveBinding curveBinding in animation.curves)
			{
				if (curveBinding != null && !string.IsNullOrEmpty(curveBinding.blendShapeName) && curveBinding.curve != null && curveBinding.curve.length != 0)
				{
					LongWayHomeGymShowerFaceDriver.OwnedShape ownedShape2;
					if (!this._byName.TryGetValue(curveBinding.blendShapeName, out ownedShape2))
					{
						ownedShape2 = new LongWayHomeGymShowerFaceDriver.OwnedShape
						{
							Name = curveBinding.blendShapeName
						};
						this._byName.Add(ownedShape2.Name, ownedShape2);
						this._owned.Add(ownedShape2);
						this.ResolveShape(ownedShape2);
					}
					ownedShape2.Curve = curveBinding.curve;
				}
			}
		}

		// Token: 0x06005E97 RID: 24215 RVA: 0x00256E70 File Offset: 0x00255070
		private void RefreshMesh()
		{
			Mesh mesh = ((this._renderer != null) ? this._renderer.sharedMesh : null);
			if (mesh != null && mesh == this._mesh)
			{
				return;
			}
			this._mesh = mesh;
			foreach (LongWayHomeGymShowerFaceDriver.OwnedShape ownedShape in this._owned)
			{
				this.ResolveShape(ownedShape);
			}
		}

		// Token: 0x06005E98 RID: 24216 RVA: 0x00256F00 File Offset: 0x00255100
		private void ResolveShape(LongWayHomeGymShowerFaceDriver.OwnedShape shape)
		{
			shape.Index = ((this._mesh != null) ? this._mesh.GetBlendShapeIndex(shape.Name) : (-1));
			if (shape.Index < 0 || shape.HasOriginal || this._renderer == null)
			{
				return;
			}
			shape.Original = this._renderer.GetBlendShapeWeight(shape.Index);
			shape.HasOriginal = true;
		}

		// Token: 0x06005E99 RID: 24217 RVA: 0x00256F74 File Offset: 0x00255174
		private void ApplyCurrentPose()
		{
			if (this._renderer == null || this._animation == null)
			{
				return;
			}
			this.RefreshMesh();
			float num = this._animation.SampleTime(this._time);
			float num2 = (this._blending ? Mathf.SmoothStep(0f, 1f, this._blendElapsed / this._blendSeconds) : 1f);
			foreach (LongWayHomeGymShowerFaceDriver.OwnedShape ownedShape in this._owned)
			{
				if (ownedShape.Index >= 0)
				{
					float num3 = ((ownedShape.Curve != null) ? ownedShape.Curve.Evaluate(num) : ownedShape.Original);
					float num4 = (this._blending ? Mathf.Lerp(ownedShape.BlendStart, num3, num2) : num3);
					this._renderer.SetBlendShapeWeight(ownedShape.Index, num4);
					ownedShape.LastApplied = num4;
					ownedShape.HasApplied = true;
				}
			}
		}

		// Token: 0x06005E9A RID: 24218 RVA: 0x00257088 File Offset: 0x00255288
		private void RestoreOwnedShapes()
		{
			this.RefreshMesh();
			if (this._renderer != null)
			{
				foreach (LongWayHomeGymShowerFaceDriver.OwnedShape ownedShape in this._owned)
				{
					if (ownedShape.HasOriginal && ownedShape.Index >= 0)
					{
						this._renderer.SetBlendShapeWeight(ownedShape.Index, ownedShape.Original);
					}
				}
			}
			this._owned.Clear();
			this._byName.Clear();
			this._blending = false;
		}

		// Token: 0x06005E9B RID: 24219 RVA: 0x00257130 File Offset: 0x00255330
		public void Release()
		{
			this.RestoreOwnedShapes();
			this._renderer = null;
			this._mesh = null;
			this._animation = null;
			this._time = (this._blendElapsed = 0f);
		}

		// Token: 0x06005E9C RID: 24220 RVA: 0x0025716C File Offset: 0x0025536C
		private void OnDisable()
		{
			this.Release();
		}

		// Token: 0x06005E9D RID: 24221 RVA: 0x0025716C File Offset: 0x0025536C
		private void OnDestroy()
		{
			this.Release();
		}

		// Token: 0x04005FE2 RID: 24546
		private SkinnedMeshRenderer _renderer;

		// Token: 0x04005FE3 RID: 24547
		private Mesh _mesh;

		// Token: 0x04005FE4 RID: 24548
		private LongWayHomeFaceAnimationSO _animation;

		// Token: 0x04005FE5 RID: 24549
		private readonly Dictionary<string, LongWayHomeGymShowerFaceDriver.OwnedShape> _byName = new Dictionary<string, LongWayHomeGymShowerFaceDriver.OwnedShape>(StringComparer.Ordinal);

		// Token: 0x04005FE6 RID: 24550
		private readonly List<LongWayHomeGymShowerFaceDriver.OwnedShape> _owned = new List<LongWayHomeGymShowerFaceDriver.OwnedShape>();

		// Token: 0x04005FE7 RID: 24551
		private float _time;

		// Token: 0x04005FE8 RID: 24552
		private float _blendElapsed;

		// Token: 0x04005FE9 RID: 24553
		private float _blendSeconds;

		// Token: 0x04005FEA RID: 24554
		private bool _blending;

		// Token: 0x02000CBC RID: 3260
		private sealed class OwnedShape
		{
			// Token: 0x04005FEB RID: 24555
			public string Name;

			// Token: 0x04005FEC RID: 24556
			public int Index = -1;

			// Token: 0x04005FED RID: 24557
			public bool HasOriginal;

			// Token: 0x04005FEE RID: 24558
			public float Original;

			// Token: 0x04005FEF RID: 24559
			public bool HasApplied;

			// Token: 0x04005FF0 RID: 24560
			public float LastApplied;

			// Token: 0x04005FF1 RID: 24561
			public float BlendStart;

			// Token: 0x04005FF2 RID: 24562
			public AnimationCurve Curve;
		}
	}
}
