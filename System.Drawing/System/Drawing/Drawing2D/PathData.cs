using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Contains the graphical data that makes up a <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object. This class cannot be inherited.</summary>
	// Token: 0x02000149 RID: 329
	public sealed class PathData
	{
		/// <summary>Gets or sets an array of <see cref="T:System.Drawing.PointF" /> structures that represents the points through which the path is constructed.</summary>
		/// <returns>An array of <see cref="T:System.Drawing.PointF" /> objects that represents the points through which the path is constructed.</returns>
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x000206CF File Offset: 0x0001E8CF
		// (set) Token: 0x06000E48 RID: 3656 RVA: 0x000206D7 File Offset: 0x0001E8D7
		public PointF[] Points { get; set; }

		/// <summary>Gets or sets the types of the corresponding points in the path.</summary>
		/// <returns>An array of bytes that specify the types of the corresponding points in the path.</returns>
		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x000206E0 File Offset: 0x0001E8E0
		// (set) Token: 0x06000E4A RID: 3658 RVA: 0x000206E8 File Offset: 0x0001E8E8
		public byte[] Types { get; set; }
	}
}
