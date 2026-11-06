using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.VFX;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200036E RID: 878
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you set a property on a target VisualEffect")]
	[FeedbackPath("Particles/VisualEffectSetProperty")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.VisualEffectGraph", null)]
	public class MMF_VisualEffectSetProperty : MMF_Feedback
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600182C RID: 6188 RVA: 0x00066993 File Offset: 0x00064B93
		// (set) Token: 0x0600182D RID: 6189 RVA: 0x000669A1 File Offset: 0x00064BA1
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.DeclaredDuration);
			}
			set
			{
				this.DeclaredDuration = value;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x000669AA File Offset: 0x00064BAA
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x000669AD File Offset: 0x00064BAD
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x000669B0 File Offset: 0x00064BB0
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this._propertyID = Shader.PropertyToID(this.PropertyID);
			this.GetInitialValue();
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x000669D0 File Offset: 0x00064BD0
		protected virtual void GetInitialValue()
		{
			switch (this.PropertyType)
			{
			case MMF_VisualEffectSetProperty.PropertyTypes.AnimationCurve:
				this._initialAnimationCurve = this.TargetVisualEffect.GetAnimationCurve(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Bool:
				this._initialBool = this.TargetVisualEffect.GetBool(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Float:
				this._initialFloat = this.TargetVisualEffect.GetFloat(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Gradient:
				this._initialGradient = this.TargetVisualEffect.GetGradient(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Int:
				this._initialInt = this.TargetVisualEffect.GetInt(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Mesh:
				this._initialMesh = this.TargetVisualEffect.GetMesh(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Texture:
				this._initialTexture = this.TargetVisualEffect.GetTexture(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.UInt:
				this._initialUInt = this.TargetVisualEffect.GetUInt(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Vector2:
				this._initialVector2 = this.TargetVisualEffect.GetVector2(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Vector3:
				this._initialVector3 = this.TargetVisualEffect.GetVector3(this._propertyID);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Vector4:
				this._initialVector4 = this.TargetVisualEffect.GetVector4(this._propertyID);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x00066B20 File Offset: 0x00064D20
		protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1f)
		{
			if (!this.Active || !MMF_VisualEffectSetProperty.FeedbackTypeAuthorized || this.TargetVisualEffect == null)
			{
				return;
			}
			switch (this.PropertyType)
			{
			case MMF_VisualEffectSetProperty.PropertyTypes.AnimationCurve:
				this.TargetVisualEffect.SetAnimationCurve(this._propertyID, this.NewAnimationCurve);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Bool:
				this.TargetVisualEffect.SetBool(this._propertyID, this.NewBool);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Float:
				this.TargetVisualEffect.SetFloat(this._propertyID, this.NewFloat);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Gradient:
				this.TargetVisualEffect.SetGradient(this._propertyID, this.NewGradient);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Int:
				this.TargetVisualEffect.SetInt(this._propertyID, this.NewInt);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Mesh:
				this.TargetVisualEffect.SetMesh(this._propertyID, this.NewMesh);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Texture:
				this.TargetVisualEffect.SetTexture(this._propertyID, this.NewTexture);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.UInt:
				this.TargetVisualEffect.SetUInt(this._propertyID, this.NewUInt);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Vector2:
				this.TargetVisualEffect.SetVector2(this._propertyID, this.NewVector2);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Vector3:
				this.TargetVisualEffect.SetVector3(this._propertyID, this.NewVector3);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Vector4:
				this.TargetVisualEffect.SetVector4(this._propertyID, this.NewVector4);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x00066C8C File Offset: 0x00064E8C
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_VisualEffectSetProperty.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.PropertyType)
			{
			case MMF_VisualEffectSetProperty.PropertyTypes.AnimationCurve:
				this.TargetVisualEffect.SetAnimationCurve(this._propertyID, this._initialAnimationCurve);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Bool:
				this.TargetVisualEffect.SetBool(this._propertyID, this._initialBool);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Float:
				this.TargetVisualEffect.SetFloat(this._propertyID, this._initialFloat);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Gradient:
				this.TargetVisualEffect.SetGradient(this._propertyID, this._initialGradient);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Int:
				this.TargetVisualEffect.SetInt(this._propertyID, this._initialInt);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Mesh:
				this.TargetVisualEffect.SetMesh(this._propertyID, this._initialMesh);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Texture:
				this.TargetVisualEffect.SetTexture(this._propertyID, this._initialTexture);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.UInt:
				this.TargetVisualEffect.SetUInt(this._propertyID, this._initialUInt);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Vector2:
				this.TargetVisualEffect.SetVector2(this._propertyID, this._initialVector2);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Vector3:
				this.TargetVisualEffect.SetVector3(this._propertyID, this._initialVector3);
				return;
			case MMF_VisualEffectSetProperty.PropertyTypes.Vector4:
				this.TargetVisualEffect.SetVector4(this._propertyID, this._initialVector4);
				return;
			default:
				return;
			}
		}

		// Token: 0x04001BF3 RID: 7155
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001BF4 RID: 7156
		[MMFInspectorGroup("Visual Effect Property", true, 41, false, false)]
		[Tooltip("the duration for the player to consider. This won't impact your visual effect, but is a way to communicate to the MMF Player the duration of this feedback. Usually you'll want it to match your actual particle system, and setting it can be useful to have this feedback work with holding pauses.")]
		public float DeclaredDuration;

		// Token: 0x04001BF5 RID: 7157
		[Tooltip("the visual effect on which to set a property")]
		public VisualEffect TargetVisualEffect;

		// Token: 0x04001BF6 RID: 7158
		[Tooltip("the ID of the property to set, as exposed by the Visual Effect Graph")]
		public string PropertyID;

		// Token: 0x04001BF7 RID: 7159
		[Tooltip("the type of the property to set")]
		public MMF_VisualEffectSetProperty.PropertyTypes PropertyType = MMF_VisualEffectSetProperty.PropertyTypes.Float;

		// Token: 0x04001BF8 RID: 7160
		[Tooltip("if the property is an animation curve, the new animation curve to set")]
		[MMFEnumCondition("PropertyType", new int[] { 0 })]
		public AnimationCurve NewAnimationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001BF9 RID: 7161
		[Tooltip("if the property is a bool, the new bool to set")]
		[MMFEnumCondition("PropertyType", new int[] { 1 })]
		public bool NewBool = true;

		// Token: 0x04001BFA RID: 7162
		[Tooltip("if the property is a float, the new float to set")]
		[MMFEnumCondition("PropertyType", new int[] { 2 })]
		public float NewFloat = 1f;

		// Token: 0x04001BFB RID: 7163
		[Tooltip("if the property is a gradient, the new gradient to set")]
		[MMFEnumCondition("PropertyType", new int[] { 3 })]
		[GradientUsage(true)]
		public Gradient NewGradient = new Gradient();

		// Token: 0x04001BFC RID: 7164
		[Tooltip("if the property is an int, the new int to set")]
		[MMFEnumCondition("PropertyType", new int[] { 4 })]
		public int NewInt;

		// Token: 0x04001BFD RID: 7165
		[Tooltip("if the property is a mesh, the new mesh to set")]
		[MMFEnumCondition("PropertyType", new int[] { 5 })]
		public Mesh NewMesh;

		// Token: 0x04001BFE RID: 7166
		[Tooltip("if the property is a texture, the new texture to set")]
		[MMFEnumCondition("PropertyType", new int[] { 6 })]
		public Texture NewTexture;

		// Token: 0x04001BFF RID: 7167
		[Tooltip("if the property is an unsigned int, the new unsigned int to set")]
		[MMFEnumCondition("PropertyType", new int[] { 7 })]
		public uint NewUInt;

		// Token: 0x04001C00 RID: 7168
		[Tooltip("if the property is a vector2, the new vector2 to set")]
		[MMFEnumCondition("PropertyType", new int[] { 8 })]
		public Vector2 NewVector2;

		// Token: 0x04001C01 RID: 7169
		[Tooltip("if the property is a vector3, the new vector3 to set")]
		[MMFEnumCondition("PropertyType", new int[] { 9 })]
		public Vector3 NewVector3;

		// Token: 0x04001C02 RID: 7170
		[Tooltip("if the property is a vector4, the new vector4 to set")]
		[MMFEnumCondition("PropertyType", new int[] { 10 })]
		public Vector4 NewVector4;

		// Token: 0x04001C03 RID: 7171
		protected int _propertyID;

		// Token: 0x04001C04 RID: 7172
		protected AnimationCurve _initialAnimationCurve;

		// Token: 0x04001C05 RID: 7173
		protected bool _initialBool;

		// Token: 0x04001C06 RID: 7174
		protected float _initialFloat;

		// Token: 0x04001C07 RID: 7175
		protected Gradient _initialGradient;

		// Token: 0x04001C08 RID: 7176
		protected int _initialInt;

		// Token: 0x04001C09 RID: 7177
		protected Mesh _initialMesh;

		// Token: 0x04001C0A RID: 7178
		protected Texture _initialTexture;

		// Token: 0x04001C0B RID: 7179
		protected uint _initialUInt;

		// Token: 0x04001C0C RID: 7180
		protected Vector2 _initialVector2;

		// Token: 0x04001C0D RID: 7181
		protected Vector3 _initialVector3;

		// Token: 0x04001C0E RID: 7182
		protected Vector4 _initialVector4;

		// Token: 0x02000595 RID: 1429
		public enum PropertyTypes
		{
			// Token: 0x040024F7 RID: 9463
			AnimationCurve,
			// Token: 0x040024F8 RID: 9464
			Bool,
			// Token: 0x040024F9 RID: 9465
			Float,
			// Token: 0x040024FA RID: 9466
			Gradient,
			// Token: 0x040024FB RID: 9467
			Int,
			// Token: 0x040024FC RID: 9468
			Mesh,
			// Token: 0x040024FD RID: 9469
			Texture,
			// Token: 0x040024FE RID: 9470
			UInt,
			// Token: 0x040024FF RID: 9471
			Vector2,
			// Token: 0x04002500 RID: 9472
			Vector3,
			// Token: 0x04002501 RID: 9473
			Vector4
		}
	}
}
