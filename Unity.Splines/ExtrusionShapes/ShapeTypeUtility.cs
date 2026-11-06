using System;

namespace UnityEngine.Splines.ExtrusionShapes
{
	// Token: 0x0200004C RID: 76
	internal static class ShapeTypeUtility
	{
		// Token: 0x0600031D RID: 797 RVA: 0x00010148 File Offset: 0x0000E348
		public static ShapeType GetShapeType(object obj)
		{
			ShapeType shapeType;
			if (!(obj is Circle))
			{
				if (!(obj is Square))
				{
					if (!(obj is Road))
					{
						if (!(obj is SplineShape))
						{
							throw new ArgumentException("obj is not a recognized shape", "obj");
						}
						shapeType = ShapeType.Spline;
					}
					else
					{
						shapeType = ShapeType.Road;
					}
				}
				else
				{
					shapeType = ShapeType.Square;
				}
			}
			else
			{
				shapeType = ShapeType.Circle;
			}
			return shapeType;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00010198 File Offset: 0x0000E398
		public static IExtrudeShape CreateShape(ShapeType type)
		{
			IExtrudeShape extrudeShape;
			switch (type)
			{
			case ShapeType.Circle:
				extrudeShape = new Circle();
				break;
			case ShapeType.Square:
				extrudeShape = new Square();
				break;
			case ShapeType.Road:
				extrudeShape = new Road();
				break;
			case ShapeType.Spline:
				extrudeShape = new SplineShape();
				break;
			default:
				throw new ArgumentOutOfRangeException("type");
			}
			return extrudeShape;
		}
	}
}
