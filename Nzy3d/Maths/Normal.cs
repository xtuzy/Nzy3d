namespace Nzy3d.Maths
{
	public class Normal
	{
		public static Coord3d compute(Coord3d p0, Coord3d p1, Coord3d p2)
		{
			var v1 = new Vector3d(p0, p1);
			var v2 = new Vector3d(p1, p2);
			var norm = v1.cross(v2);
			double d = norm.distance(Coord3d.ORIGIN);
			return norm.divide(d);
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
