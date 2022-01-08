namespace Nzy3d.Colors.ColorMaps
{
	/// <summary>
	/// <para>
	/// Creates a new instance of ColorMapWhiteBlue.
	/// </para>
	/// <para>
	/// A ColorMapWhiteGreen objects provides a color for points standing
	/// between a Zmin and Zmax values.
	/// </para>
	/// <para>
	/// The points standing outside these [Zmin;Zmax] boundaries are assigned
	/// to the same color than the points standing on the boundaries.
	/// </para>
	/// <para>The white-red colormap is a progressive transition from white to blue.</para>
	/// </summary>
	public class ColorMapWhiteBlue : IColorMap
	{
		public bool Direction { get; set; }

		public Color GetColor(IColorMappable colorable, double v)
		{
			return GetColor(0, 0, v, colorable.ZMin, colorable.ZMax);
		}

		public Color GetColor(IColorMappable colorable, double x, double y, double z)
		{
			return GetColor(x, y, z, colorable.ZMin, colorable.ZMax);
		}

		/// <summary>
		/// Helper function 
		/// </summary>
		private Color GetColor(double x, double y, double z, double zMin, double zMax)
		{
			double rel_value = 0;
			if (z < zMin)
			{
				rel_value = 0;
			}
			else if (z > zMax)
			{
				rel_value = 1;
			}
			else
			{
				if (Direction)
				{
					rel_value = (z - zMin) / (zMax - zMin);
				}
				else
				{
					rel_value = (zMax - z) / (zMax - zMin);
				}
			}
			return new Color(rel_value, rel_value, 1);
		}

		/// <summary>
		/// Returns the string representation of this colormap
		/// </summary>
		public override string ToString()
		{
			return "ColorMapWhiteBlue";
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
