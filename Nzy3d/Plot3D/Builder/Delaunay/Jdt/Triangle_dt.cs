namespace Nzy3d.Plot3D.Builder.Delaunay.Jdt
{
    /// <summary>
    /// This class represents a 3D triangle in a Triangulation
    /// </summary>
    public class Triangle_dt
	{
        private Circle_dt _circum;

        //private int _counter = 0;

        //private int _c2 = 0;

        /// <summary>
        /// Constructs a triangle form 3 point - store it in counterclockwised order.
        /// A should be before B and B before C in counterclockwise order.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        public Triangle_dt(Point_dt A, Point_dt B, Point_dt C)
		{
			a = A;
			int res = C.pointLineTest(A, B);
			if (res <= Point_dt.LEFT | res == Point_dt.INFRONTOFA | res == Point_dt.BEHINDB)
			{
				b = B;
				c = C;
				//RIGHT
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Warning, Triangle_dt(A,B,C) expects points in counterclockwise order.");
				System.Diagnostics.Debug.WriteLine(A.ToString() + B.ToString() + C.ToString());
				b = C;
				c = B;
			}
			circumcircle();
		}

		/// <summary>
		/// Creates a half plane using the segment (A,B).
		/// </summary>
		public Triangle_dt(Point_dt A, Point_dt B)
		{
			a = A;
			b = B;
			IsHalfplane = true;
		}

        /// <summary>
        /// Returns true if this triangle is actually a half plane
        /// </summary>
        public bool IsHalfplane { get; set; }

        /// <summary>
        /// tag - for bfs algorithms
        /// </summary>
        public bool Mark { get; set; } = false;

        /// <summary>
        /// Returns the first vertex of this triangle.
        /// </summary>
        public Point_dt P1
		{
			get { return a; }
		}

		/// <summary>
		/// Returns the second vertex of this triangle.
		/// </summary>
		public Point_dt P2
		{
			get { return b; }
		}

		/// <summary>
		/// Returns the third vertex of this triangle.
		/// </summary>
		public Point_dt P3
		{
			get { return c; }
		}

		/// <summary>
		/// Returns the consecutive triangle which shares this triangle p1,p2 edge.
		/// </summary>
		public Triangle_dt Next_12
		{
			get { return abnext; }
		}

		/// <summary>
		/// Returns the consecutive triangle which shares this triangle p2,p3 edge.
		/// </summary>
		public Triangle_dt Next_23
		{
			get { return bcnext; }
		}

		/// <summary>
		/// Returns the consecutive triangle which shares this triangle p3,p1 edge.
		/// </summary>
		public Triangle_dt Next_31
		{
			get { return canext; }
		}

		/// <summary>
		/// The bounding rectange between the minimum and maximum coordinates of the triangle.
		/// </summary>
		public BoundingBox BoundingBox
		{
			get
			{
				var lowerLeft = new Point_dt(Math.Min(a.x, Math.Min(b.x, c.x)), Math.Min(a.y, Math.Min(b.y, c.y)));
				var upperRight = new Point_dt(Math.Max(a.x, Math.Max(b.x, c.x)), Math.Max(a.y, Math.Max(b.y, c.y)));
				return new BoundingBox(lowerLeft, upperRight);
			}
		}

		public void SwitchNeighbors(Triangle_dt old_t, Triangle_dt new_t)
		{
			if (abnext.Equals(old_t))
			{
				abnext = new_t;
			}
			else if (bcnext.Equals(old_t))
			{
				bcnext = new_t;
			}
			else if (canext.Equals(old_t))
			{
				canext = new_t;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Error, switchneighbors can't find Old.");
			}
		}

		public Triangle_dt Neighbor(Point_dt p)
		{
			if (a.Equals(p))
			{
				return canext;
			}
			else if (b.Equals(p))
			{
				return abnext;
			}
			else if (c.Equals(p))
			{
				return bcnext;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Error, neighbors can't find p.");
				return null;
			}
		}

		/// <summary>
		/// Returns the neighbors that shares the given corner and is not the previous triangle.
		/// </summary>
		/// <param name="p">The given corner.</param>
		/// <param name="prevTriangle">The previous triangle.</param>
		/// <returns>The neighbors that shares the given corner and is not the previous triangle.</returns>
		public object NextNeighbor(Point_dt p, Triangle_dt prevTriangle)
		{
			Triangle_dt neighbor = null;

			if (a.Equals(p))
			{
				neighbor = canext;
			}
			else if (b.Equals(p))
			{
				neighbor = abnext;
			}
			else if (c.Equals(p))
			{
				neighbor = bcnext;
			}

			if (neighbor.Equals(prevTriangle) || neighbor.IsHalfplane)
			{
				if (a.Equals(p))
				{
					neighbor = abnext;
				}
				else if (b.Equals(p))
				{
					neighbor = bcnext;
				}
				else if (c.Equals(p))
				{
					neighbor = canext;
				}
			}

			return neighbor;
		}

		public Circle_dt circumcircle()
		{
			double u = ((a.x - b.x) * (a.x + b.x) + (a.y - b.y) * (a.y + b.y)) / 2.0;
			double v = ((b.x - c.x) * (b.x + c.x) + (b.y - c.y) * (b.y + c.y)) / 2.0;
			double den = (a.x - b.x) * (b.y - c.y) - (b.x - c.x) * (a.y - b.y);
			// oops, degenerate case
			if ((den == 0))
			{
				_circum = new Circle_dt(a, double.PositiveInfinity);
			}
			else
			{
				Point_dt cen = new Point_dt((u * (b.y - c.y) - v * (a.y - b.y)) / den, (v * (a.x - b.x) - u * (b.x - c.x)) / den);
				_circum = new Circle_dt(cen, cen.distance2(a));
			}
			return _circum;
		}

		public bool circumcircle_contains(Point_dt p)
		{
			// Fix from https://github.com/rtrusso/nzy3d-api/commit/b39a673c522ac49a6727f9b7890a154824581d42
			if (IsHalfplane)
			{
				return false;
			}
			// End

			return _circum.Radius > _circum.Center.distance2(p);
		}

		public override string ToString()
		{
			string res = "";
			res += "A: " + a.ToString() + " B: " + b.ToString();
			if ((!IsHalfplane))
			{
				res += " C: " + c.ToString();
			}
			return res;
		}

		/// <summary>
		/// Determines if this triangle contains the point p.
		/// </summary>
		/// <param name="p">The query point</param>
		/// <returns>True if p is not null and is inside this triangle</returns>
		/// <remarks>Note: on boundary is considered inside</remarks>
		public bool contains(Point_dt p)
		{
			if (IsHalfplane | p == null)
			{
				return false;
			}
			if (isCorner(p))
			{
				return true;
			}
			int a12 = p.pointLineTest(a, b);
			int a23 = p.pointLineTest(b, c);
			int a31 = p.pointLineTest(c, a);
            return (a12 == Point_dt.LEFT && a23 == Point_dt.LEFT && a31 == Point_dt.LEFT)
                || (a12 == Point_dt.RIGHT && a23 == Point_dt.RIGHT && a31 == Point_dt.RIGHT)
                || (a12 == Point_dt.ONSEGMENT || a23 == Point_dt.ONSEGMENT || a31 == Point_dt.ONSEGMENT);
        }

		/// <summary>
		/// Determines if this triangle contains the point p.
		/// </summary>
		/// <param name="p">The query point</param>
		/// <returns>True if p is not null and is inside this triangle</returns>
		/// <remarks>Note: on boundary is considered outside</remarks>
		public bool contains_BoundaryIsOutside(Point_dt p)
		{
			if (IsHalfplane | p == null)
			{
				return false;
			}
			if (isCorner(p))
			{
				return false;
			}
			int a12 = p.pointLineTest(a, b);
			int a23 = p.pointLineTest(b, c);
			int a31 = p.pointLineTest(c, a);
            return (a12 == Point_dt.LEFT && a23 == Point_dt.LEFT && a31 == Point_dt.LEFT)
                || (a12 == Point_dt.RIGHT && a23 == Point_dt.RIGHT && a31 == Point_dt.RIGHT);
        }

		/// <summary>
		/// Checks if the given point is a corner of this triangle.
		/// </summary>
		/// <param name="p">The given point.</param>
		/// <returns>True if the given point is a corner of this triangle.</returns>
		public bool isCorner(Point_dt p)
		{
			return (p.x == a.x & p.y == a.y) | (p.x == b.x & p.y == b.y) | (p.x == c.x & p.y == c.y);
		}

		/// <summary>
		/// compute the Z value for the X, Y values of q.
		/// Assume current triangle represent a plane --> q does NOT need to be contained in this triangle.
		/// </summary>
		/// <param name="q">A x/y point.</param>
		/// <returns></returns>
		/// <remarks>Current triangle must not be a halfplane.</remarks>
		public double z_value(Point_dt q)
		{
			if (q == null)
			{
				throw new ArgumentException("Input point cannot be Nothing", "q");
			}
			if (IsHalfplane)
			{
				throw new Exception("Cannot approximate the z value from a halfplane triangle");
			}
			if (q.x == a.x & q.y == a.y)
				return a.z;
			if (q.x == b.x & q.y == b.y)
				return b.z;
			if (q.x == c.x & q.y == c.y)
				return c.z;
			double X = 0;
			double x0 = q.x;
			double x1 = a.x;
			double x2 = b.x;
			double x3 = c.x;
			double Y = 0;
			double y0 = q.y;
			double y1 = a.y;
			double y2 = b.y;
			double y3 = c.y;
			double Z = 0;
			double m01 = 0;
			double k01 = 0;
			double m23 = 0;
			double k23 = 0;

			// 0 - regular, 1-horizontal , 2-vertical
			int flag01 = 0;
			if (x0 != x1)
			{
				m01 = (y0 - y1) / (x0 - x1);
				k01 = y0 - m01 * x0;
				if (m01 == 0)
					flag01 = 1;
				//2-vertical
			}
			else
			{
				flag01 = 2;
				//x01 = x0
			}

			int flag23 = 0;
			if (x2 != x3)
			{
				m23 = (y2 - y3) / (x2 - x3);
				k23 = y2 - m23 * x2;
				if (m23 == 0)
					flag23 = 1;
				//2-vertical
			}
			else
			{
				flag23 = 2;
				//x01 = x0
			}

			if (flag01 == 2)
			{
				X = x0;
				Y = m23 * X + k23;
			}
			else if (flag23 == 2)
			{
				X = x2;
				Y = m01 * X + k01;
			}
			else
			{
				X = (k23 - k01) / (m01 - m23);
				Y = m01 * X + k01;
			}

			double r = 0;
			if (flag23 == 2)
			{
				r = (y2 - Y) / (y2 - y3);
			}
			else
			{
				r = (x2 - X) / (x2 - x3);
			}

			Z = b.z + (c.z - b.z) * r;
			if (flag01 == 2)
			{
				r = (y1 - y0) / (y1 - Y);
			}
			else
			{
				r = (x1 - x0) / (x1 - X);
			}

			double qZ = a.z + (Z - a.z) * r;
			return qZ;
		}

		/// <summary>
		/// Compute the Z value for the X, Y values 
		/// Assume current triangle represent a plane --> q does NOT need to be contained in this triangle.
		/// </summary>
		/// <returns></returns>
		/// <remarks>Current triangle must not be a halfplane.</remarks>
		public double z(double x, double y)
		{
			return z_value(new Point_dt(x, y));
		}

		/// <summary>
		/// compute the Z value for the X, Y values of q.
		/// Assume current triangle represent a plane --> q does NOT need to be contained in this triangle.
		/// </summary>
		/// <param name="q">A x/y point.</param>
		/// <returns>A new <see cref="Point_dt"/> with same x/y than <paramref name="q"/> and computed z value</returns>
		/// <remarks>Current triangle must not be a halfplane.</remarks>
		public Point_dt z(Point_dt q)
		{
			double newz = z_value(q);
			return new Point_dt(q.x, q.y, newz);
		}

        public Point_dt a { get; set; }

        public Point_dt b { get; set; }

        public Point_dt c { get; set; }

        public Triangle_dt abnext { get; set; }

        public Triangle_dt bcnext { get; set; }

        public Triangle_dt canext { get; set; }

        public int mc { get; set; }

        public Circle_dt circum
		{
			get { return _circum; }
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
