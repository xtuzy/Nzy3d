﻿using Nzy3d.Maths;
using Nzy3d.Plot3D.Builder.Delaunay.Jdt;
using System.Collections.Generic;
using Xunit;

/*
 Scenario: Triangle setup in counterclockwise
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(1.0,0.0)-(0.0,1.0)
	Then the triangle has coordinates (0.0,0.0)-(1.0,0.0)-(0.0,1.0)
	
Scenario: Triangle setup in clockwise
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
	Then the triangle has coordinates (0.0,0.0)-(1.0,0.0)-(0.0,1.0)

Scenario: Point inside triangle
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
	And I have a point with coordinates (0.25,0.25)
	Then The point is inside the triangle

Scenario: Point outside triangle
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
	And I have a point with coordinates (0.75,0.75)
	Then The point is outside the triangle

Scenario: Point on the boundary of triangle
	Given I have an initial test setup
	And I have a triangle with coordinates (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
	And I have a point with coordinates (0.5,0.5)
	Then The point is on the boundary of the triangle
 */

namespace Nzy3d.Tests.Plot3D.Builder.Delaunay.Jdt
{
    public class Triangle_DtSteps
    {
        [Fact]
        public void TriangleCoordinatesCounterClockwiseTest()
        {
            const double p0 = 0.0;
            const double p1 = 0.0;
            const double p2 = 1.0;
            const double p3 = 0.0;
            const double p4 = 0.0;
            const double p5 = 1.0;

            // (0.0,0.0)-(1.0,0.0)-(0.0,1.0)
            var triangle = new Triangle_dt(new Point_dt(0.0, 0.0),
                                           new Point_dt(1.0, 0.0),
                                           new Point_dt(0.0, 1.0));
            Assert.Equal(p0, triangle.A.X);
            Assert.Equal(p1, triangle.A.Y);
            Assert.Equal(p2, triangle.B.X);
            Assert.Equal(p3, triangle.B.Y);
            Assert.Equal(p4, triangle.C.X);
            Assert.Equal(p5, triangle.C.Y);
        }

        [Fact]
        public void TriangleCoordinatesClockwiseTest()
        {
            const double p0 = 0.0;
            const double p1 = 0.0;
            const double p2 = 1.0;
            const double p3 = 0.0;
            const double p4 = 0.0;
            const double p5 = 1.0;

            // (0.0,0.0)-(0.0,1.0)-(1.0,0.0)
            var triangle = new Triangle_dt(new Point_dt(0.0, 0.0),
                                           new Point_dt(0.0, 1.0),
                                           new Point_dt(1.0, 0.0));
            Assert.Equal(p0, triangle.A.X);
            Assert.Equal(p1, triangle.A.Y);
            Assert.Equal(p2, triangle.B.X);
            Assert.Equal(p3, triangle.B.Y);
            Assert.Equal(p4, triangle.C.X);
            Assert.Equal(p5, triangle.C.Y);
        }

        [Fact]
        public void PointInsideTriangle()
        {
            var triangle = new Triangle_dt(new Point_dt(0.0, 0.0),
                                           new Point_dt(0.0, 1.0),
                                           new Point_dt(1.0, 0.0));
            Assert.True(triangle.Contains(new Point_dt(0.25, 0.25)));
        }

        [Fact]
        public void PointOutsideTriangle()
        {
            var triangle = new Triangle_dt(new Point_dt(0.0, 0.0),
                                           new Point_dt(0.0, 1.0),
                                           new Point_dt(1.0, 0.0));
            Assert.False(triangle.Contains(new Point_dt(0.75, 0.75)));
        }

        [Fact]
        public void PointOnBoundaryTriangle()
        {
            var triangle = new Triangle_dt(new Point_dt(0.0, 0.0),
                                           new Point_dt(0.0, 1.0),
                                           new Point_dt(1.0, 0.0));

            var point = new Point_dt(0.5, 0.5);
            Assert.True(triangle.Contains(point));
            Assert.False(triangle.ContainsBoundaryIsOutside(point), "Point is inside the triangle, boundary excluded");
        }

        [Fact]
        public void Issue16()
        {
            var data = new List<Coord3d>();
            data.Add(new Coord3d(-4.000000, -4.000000, -0.586176));
            data.Add(new Coord3d(-2.000000, -4.000000, -0.971278));
            data.Add(new Coord3d(0.000000, -4.000000, -0.756803));
            data.Add(new Coord3d(2.000000, -4.000000, -0.971278));
            data.Add(new Coord3d(-4.000000, -2.000000, -0.971278));
            data.Add(new Coord3d(-2.000000, -2.000000, 0.308072));
            data.Add(new Coord3d(0.000000, -2.000000, 0.909297));
            data.Add(new Coord3d(2.000000, -2.000000, 0.308072));
            data.Add(new Coord3d(-4.000000, 0.000000, -0.756803));
            data.Add(new Coord3d(-2.000000, 0.000000, 0.909297));
            data.Add(new Coord3d(0.000000, 0.000000, 0.000000));
            data.Add(new Coord3d(2.000000, 0.000000, 0.909297));
            data.Add(new Coord3d(-4.000000, 2.000000, -0.971278));
            data.Add(new Coord3d(-2.000000, 2.000000, 0.308072));
            data.Add(new Coord3d(0.000000, 2.000000, 0.909297));
            data.Add(new Coord3d(2.000000, 2.000000, 0.308072));
            var surface = Nzy3d.Plot3D.Builder.Builder.BuildDelaunay(data);

            Assert.NotNull(surface); // Basically testing it does not throws
        }
    }
}
