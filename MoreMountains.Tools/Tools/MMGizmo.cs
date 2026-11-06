using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200004A RID: 74
	public class MMGizmo : MonoBehaviour
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000784A File Offset: 0x00005A4A
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00007852 File Offset: 0x00005A52
		public virtual bool Initialized { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000785B File Offset: 0x00005A5B
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00007863 File Offset: 0x00005A63
		public virtual SphereCollider _sphereCollider { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000786C File Offset: 0x00005A6C
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00007874 File Offset: 0x00005A74
		public virtual BoxCollider _boxCollider { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000787D File Offset: 0x00005A7D
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00007885 File Offset: 0x00005A85
		public virtual MeshCollider _meshCollider { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000788E File Offset: 0x00005A8E
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00007896 File Offset: 0x00005A96
		public virtual CircleCollider2D _circleCollider2D { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000789F File Offset: 0x00005A9F
		// (set) Token: 0x06000183 RID: 387 RVA: 0x000078A7 File Offset: 0x00005AA7
		public virtual BoxCollider2D _boxCollider2D { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000184 RID: 388 RVA: 0x000078B0 File Offset: 0x00005AB0
		// (set) Token: 0x06000185 RID: 389 RVA: 0x000078B8 File Offset: 0x00005AB8
		public virtual Vector3 _vector3Zero { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000186 RID: 390 RVA: 0x000078C1 File Offset: 0x00005AC1
		// (set) Token: 0x06000187 RID: 391 RVA: 0x000078C9 File Offset: 0x00005AC9
		public virtual Vector3 _newPosition { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000188 RID: 392 RVA: 0x000078D2 File Offset: 0x00005AD2
		// (set) Token: 0x06000189 RID: 393 RVA: 0x000078DA File Offset: 0x00005ADA
		public virtual Vector2 _worldToGUIPosition { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000078E3 File Offset: 0x00005AE3
		// (set) Token: 0x0600018B RID: 395 RVA: 0x000078EB File Offset: 0x00005AEB
		public virtual Rect _textureRect { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000078F4 File Offset: 0x00005AF4
		// (set) Token: 0x0600018D RID: 397 RVA: 0x000078FC File Offset: 0x00005AFC
		public virtual GUIStyle _textGUIStyle { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00007905 File Offset: 0x00005B05
		// (set) Token: 0x0600018F RID: 399 RVA: 0x0000790D File Offset: 0x00005B0D
		public virtual string _textToDisplay { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00007916 File Offset: 0x00005B16
		// (set) Token: 0x06000191 RID: 401 RVA: 0x0000791E File Offset: 0x00005B1E
		public virtual bool _sphereColliderNotNull { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00007927 File Offset: 0x00005B27
		// (set) Token: 0x06000193 RID: 403 RVA: 0x0000792F File Offset: 0x00005B2F
		public virtual bool _boxColliderNotNull { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00007938 File Offset: 0x00005B38
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00007940 File Offset: 0x00005B40
		public virtual bool _meshColliderNotNull { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00007949 File Offset: 0x00005B49
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00007951 File Offset: 0x00005B51
		public virtual bool _circleCollider2DNotNull { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000795A File Offset: 0x00005B5A
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00007962 File Offset: 0x00005B62
		public virtual bool _boxCollider2DNotNull { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000796B File Offset: 0x00005B6B
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00007973 File Offset: 0x00005B73
		public virtual bool _positionTextureNotNull { get; set; }

		// Token: 0x0600019C RID: 412 RVA: 0x0000797C File Offset: 0x00005B7C
		protected virtual void Awake()
		{
			base.enabled = false;
		}

		// Token: 0x040001E6 RID: 486
		[Header("Modes")]
		[Tooltip("if this is true, gizmos will be displayed, if this is false, gizmos won't be displayed")]
		public bool DisplayGizmo = true;

		// Token: 0x040001E7 RID: 487
		[Tooltip("what the gizmos should represent. Collider will show the bounds of the associated collider, Position will show the position of the object")]
		public MMGizmo.GizmoTypes GizmoType = MMGizmo.GizmoTypes.Position;

		// Token: 0x040001E8 RID: 488
		[Tooltip("whether gizmos should always be displayed, or only when selected")]
		public MMGizmo.DisplayModes DisplayMode;

		// Token: 0x040001E9 RID: 489
		[Header("Settings")]
		[Tooltip("the color of the collider or position gizmo")]
		public Color GizmoColor = MMColors.ReunoYellow;

		// Token: 0x040001EA RID: 490
		[Tooltip("the shape of the gizmo when in position mode")]
		[MMEnumCondition("GizmoType", new int[] { 2 })]
		public MMGizmo.PositionModes PositionMode;

		// Token: 0x040001EB RID: 491
		[Tooltip("the texture to display as a gizmo when in position & texture mode")]
		[MMEnumCondition("PositionMode", new int[] { 5 })]
		public Texture PositionTexture;

		// Token: 0x040001EC RID: 492
		[Tooltip("the size of the texture to display as a gizmo")]
		[MMEnumCondition("PositionMode", new int[] { 5 })]
		public Vector2 TextureSize = new Vector2(50f, 50f);

		// Token: 0x040001ED RID: 493
		[Tooltip("the size of the gizmo when in position mode")]
		[MMEnumCondition("GizmoType", new int[] { 2 })]
		public float PositionSize = 0.2f;

		// Token: 0x040001EE RID: 494
		[Tooltip("whether to display the collider gizmo as a wire or a full mesh")]
		[MMEnumCondition("GizmoType", new int[] { 1 })]
		public MMGizmo.ColliderRenderTypes ColliderRenderType;

		// Token: 0x040001EF RID: 495
		[Tooltip("the distance from the scene view camera beyond which the gizmo won't be displayed")]
		public float ViewDistance = 20f;

		// Token: 0x040001F0 RID: 496
		[Header("Offsets")]
		[Tooltip("an offset to apply when drawing a collider or position gizmo")]
		public Vector3 GizmoOffset = Vector3.zero;

		// Token: 0x040001F1 RID: 497
		[Tooltip("whether or not to lock the position of the gizmo on the x axis, regardless of the position of the object")]
		public bool LockX;

		// Token: 0x040001F2 RID: 498
		[Tooltip("the position at which to put the gizmo when locked on the x axis")]
		[MMCondition("LockX", true)]
		public float LockedX;

		// Token: 0x040001F3 RID: 499
		[Tooltip("whether or not to lock the position of the gizmo on the y axis, regardless of the position of the object")]
		public bool LockY;

		// Token: 0x040001F4 RID: 500
		[Tooltip("the position at which to put the gizmo when locked on the y axis")]
		[MMCondition("LockY", true)]
		public float LockedY;

		// Token: 0x040001F5 RID: 501
		[Tooltip("whether or not to lock the position of the gizmo on the z axis, regardless of the position of the object")]
		public bool LockZ;

		// Token: 0x040001F6 RID: 502
		[Tooltip("the position at which to put the gizmo when locked on the z axis")]
		[MMCondition("LockZ", true)]
		public float LockedZ;

		// Token: 0x040001F7 RID: 503
		[Header("Text")]
		[Tooltip("whether or not to display text on that gizmo")]
		public bool DisplayText;

		// Token: 0x040001F8 RID: 504
		[Tooltip("what to display as text for that gizmo (some custom text, the object's name, position, rotation, scale, or a target property)")]
		[MMCondition("DisplayText", true)]
		public MMGizmo.TextModes TextMode;

		// Token: 0x040001F9 RID: 505
		[Tooltip("when in CustomText mode, the text to display on that gizmo")]
		[MMEnumCondition("TextMode", new int[] { 1 })]
		public string TextToDisplay = "Some Text";

		// Token: 0x040001FA RID: 506
		[Tooltip("the offset to apply to the text")]
		[MMCondition("DisplayText", true)]
		public Vector3 TextOffset = new Vector3(0f, 0.5f, 0f);

		// Token: 0x040001FB RID: 507
		[Tooltip("what style to use for the text's font")]
		[MMCondition("DisplayText", true)]
		public FontStyle TextFontStyle;

		// Token: 0x040001FC RID: 508
		[Tooltip("the size of the text's font")]
		[MMCondition("DisplayText", true)]
		public int TextSize = 12;

		// Token: 0x040001FD RID: 509
		[Tooltip("the color in which to display the gizmo's text")]
		[MMCondition("DisplayText", true)]
		public Color TextColor = MMColors.ReunoYellow;

		// Token: 0x040001FE RID: 510
		[Tooltip("the color of the background behind the text")]
		[MMCondition("DisplayText", true)]
		public Color TextBackgroundColor = new Color(0f, 0f, 0f, 0.3f);

		// Token: 0x040001FF RID: 511
		[Tooltip("the padding to apply to the text's background")]
		[MMCondition("DisplayText", true)]
		public Vector4 TextPadding = new Vector4(5f, 0f, 5f, 0f);

		// Token: 0x04000200 RID: 512
		[Tooltip("the distance from the scene view camera beyond which the gizmo text won't be displayed")]
		[MMCondition("DisplayText", true)]
		public float TextMaxDistance = 14f;

		// Token: 0x04000201 RID: 513
		[Tooltip("when in Property mode, the property whose value to display on the gizmo")]
		public MMPropertyPicker TargetProperty;

		// Token: 0x020003B9 RID: 953
		public enum GizmoTypes
		{
			// Token: 0x04001DA4 RID: 7588
			None,
			// Token: 0x04001DA5 RID: 7589
			Collider,
			// Token: 0x04001DA6 RID: 7590
			Position
		}

		// Token: 0x020003BA RID: 954
		public enum DisplayModes
		{
			// Token: 0x04001DA8 RID: 7592
			Always,
			// Token: 0x04001DA9 RID: 7593
			OnlyWhenSelected
		}

		// Token: 0x020003BB RID: 955
		public enum PositionModes
		{
			// Token: 0x04001DAB RID: 7595
			Point,
			// Token: 0x04001DAC RID: 7596
			Cube,
			// Token: 0x04001DAD RID: 7597
			WireCube,
			// Token: 0x04001DAE RID: 7598
			Sphere,
			// Token: 0x04001DAF RID: 7599
			WireSphere,
			// Token: 0x04001DB0 RID: 7600
			Texture,
			// Token: 0x04001DB1 RID: 7601
			Arrows,
			// Token: 0x04001DB2 RID: 7602
			RightArrow,
			// Token: 0x04001DB3 RID: 7603
			UpArrow,
			// Token: 0x04001DB4 RID: 7604
			ForwardArrow,
			// Token: 0x04001DB5 RID: 7605
			Lines,
			// Token: 0x04001DB6 RID: 7606
			RightLine,
			// Token: 0x04001DB7 RID: 7607
			UpLine,
			// Token: 0x04001DB8 RID: 7608
			ForwardLine
		}

		// Token: 0x020003BC RID: 956
		public enum TextModes
		{
			// Token: 0x04001DBA RID: 7610
			GameObjectName,
			// Token: 0x04001DBB RID: 7611
			CustomText,
			// Token: 0x04001DBC RID: 7612
			Position,
			// Token: 0x04001DBD RID: 7613
			Rotation,
			// Token: 0x04001DBE RID: 7614
			Scale,
			// Token: 0x04001DBF RID: 7615
			Property
		}

		// Token: 0x020003BD RID: 957
		public enum ColliderRenderTypes
		{
			// Token: 0x04001DC1 RID: 7617
			Full,
			// Token: 0x04001DC2 RID: 7618
			Wire
		}
	}
}
