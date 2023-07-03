using System;
using System.Drawing;

using Euc3D = BRIDGES.Geometry.Euclidean3D;
using He = BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh;
using Fv = BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh;

using RH_Geo = Rhino.Geometry;
using RH_Disp = Rhino.Display;

using BRIDGES.McNeel.Rhino.Extensions.Geometry.Euclidean3D;


namespace BRIDGES.McNeel.Grasshopper.Display.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining methods for the display of three-dimensional euclidean objects.
    /// </summary>
    public static class Draw
    {
        /// <summary>
        /// Class defining methods for the display of three-dimensional euclidean objects in wireframe view.
        /// </summary>
        public static class Wireframe
        {
            /********** Manifold 0D **********/

            /// <summary>
            /// Draws a <see cref="Euc3D.Point"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="point"> <see cref="Euc3D.Point"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Point(RH_Disp.DisplayPipeline display, Euc3D.Point point, bool isSelected)
            {            
                point.CastTo(out RH_Geo.Point3d rh_Point);

                if (isSelected)
                {
                    display.DrawPoint(rh_Point, RH_Disp.PointStyle.Circle, Color.Green, Color.Green, 3.5f, 0.0f, 0.0f, 0.0f, true, true);
                }
                else
                {
                    display.DrawPoint(rh_Point, RH_Disp.PointStyle.Circle, ColourPalette.Blue, ColourPalette.Blue, 3.0f, 0.0f, 0.0f, 0.0f, true, true);
                }

            }


            /// <summary>
            /// Draws a <see cref="Euc3D.Vector"/> in the <see cref="Rhino"/> viewports anchored at (0.0, 0.0, 0.0).
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="vector"> <see cref="Euc3D.Vector"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Vector(RH_Disp.DisplayPipeline display, Euc3D.Vector vector, bool isSelected)
            {
                vector.CastTo(out RH_Geo.Vector3d rh_Vector);
                RH_Geo.Line line = new RH_Geo.Line(new RH_Geo.Point3d(0.0, 0.0, 0.0), rh_Vector);

                if (isSelected)
                {
                    display.DrawArrow(line , Color.Green, 20d, 0d) ;
                }
                else
                {
                    display.DrawArrow(line, ColourPalette.Blue, 20d, 0d);
                }
            }


            /********** Manifold 1D **********/

            /// <summary>
            /// Draws a <see cref="Euc3D.Segment"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="segment"> <see cref="Euc3D.Segment"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Segment(RH_Disp.DisplayPipeline display, Euc3D.Segment segment, bool isSelected)
            {
                segment.CastTo(out RH_Geo.Line rh_Line);

                if (isSelected)
                {
                    display.DrawLine(rh_Line, Color.Green, 3);
                }
                else
                {
                    display.DrawLine(rh_Line, ColourPalette.Blue, 2);
                }
            }

            /// <summary>
            /// Draws a <see cref="Euc3D.Ray"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="ray"> <see cref="Euc3D.Ray"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Ray(RH_Disp.DisplayPipeline display, Euc3D.Ray ray, bool isSelected)
            {
                ray.CastTo(out RH_Geo.Ray3d rh_Ray);
                RH_Geo.Line line = new RH_Geo.Line(rh_Ray.Position, rh_Ray.Direction);

                if (isSelected)
                {
                    display.DrawArrow(line, Color.Green, 20d, 0d);

                    display.DrawPoint(rh_Ray.Position, RH_Disp.PointStyle.Circle, Color.Green, Color.Green, 3.5f, 0.0f, 0.0f, 0.0f, true, true);
                }
                else
                {
                    display.DrawArrow(line, ColourPalette.Blue, 20d, 0d);

                    display.DrawPoint(rh_Ray.Position, RH_Disp.PointStyle.Circle, ColourPalette.Blue, ColourPalette.Blue, 2.5f, 0.0f, 0.0f, 0.0f, true, true);
                }
            }


            /// <summary>
            /// Draws a <see cref="Euc3D.Line"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="line"> <see cref="Euc3D.Line"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Line(RH_Disp.DisplayPipeline display, Euc3D.Line line, bool isSelected)
            {
                line.CastTo(out RH_Geo.Line rh_Line);

                line.Axis.CastTo(out RH_Geo.Vector3d rh_Axis);

                if (isSelected)
                {
                    display.DrawArrow(rh_Line, Color.Green, 20d, 0d);

                    display.DrawDottedLine(rh_Line.From, rh_Line.From - rh_Axis, Color.Green);

                    display.DrawPoint(rh_Line.From, RH_Disp.PointStyle.Circle, Color.Green, Color.Green, 3.5f, 0.0f, 0.0f, 0.0f, true, true);
                }
                else
                {
                    display.DrawArrow(rh_Line, ColourPalette.Blue, 20d, 0d);

                    display.DrawDottedLine(rh_Line.From, rh_Line.From - rh_Axis, ColourPalette.Blue);

                    display.DrawPoint(rh_Line.From, RH_Disp.PointStyle.Circle, ColourPalette.Blue, ColourPalette.Blue, 2.5f, 0.0f, 0.0f, 0.0f, true, true);
                }
            }



            /// <summary>
            /// Draws a <see cref="Euc3D.Polyline"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="polyline"> <see cref="Euc3D.Polyline"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Polyline(RH_Disp.DisplayPipeline display, Euc3D.Polyline polyline, bool isSelected)
            {
                polyline.CastTo(out RH_Geo.Polyline rh_Polyine);

                if (isSelected)
                {
                    display.DrawPolyline(rh_Polyine, Color.Green, 3);
                    display.DrawPoints(rh_Polyine, RH_Disp.PointStyle.Circle, Color.Green, Color.Green, 3.0f, 0.0f, 0.0f, 0.0f, true, true);
                }
                else
                {
                    display.DrawPolyline(rh_Polyine, ColourPalette.Blue, 2);
                    display.DrawPoints(rh_Polyine, RH_Disp.PointStyle.Circle, ColourPalette.Blue, ColourPalette.Blue, 2.0f, 0.0f, 0.0f, 0.0f, true, true);
                }

            
            }


            /// <summary>
            /// Draws a <see cref="Euc3D.Circle"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="circle"> <see cref="Euc3D.Circle"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Circle(RH_Disp.DisplayPipeline display, Euc3D.Circle circle, bool isSelected)
            {
                circle.CastTo(out RH_Geo.Circle rh_Circle);

                if (isSelected)
                {
                    display.DrawCircle(rh_Circle, Color.Green, 3);
                }
                else
                {
                    display.DrawCircle(rh_Circle, ColourPalette.Blue, 2);
                }
            }


            /********** Manifold 2D **********/

            /// <summary>
            /// Draws a <see cref="Euc3D.Plane"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="plane"> <see cref="Euc3D.Plane"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Plane(RH_Disp.DisplayPipeline display, Euc3D.Plane plane, bool isSelected)
            {
                plane.Origin.CastTo(out RH_Geo.Point3d rh_Origin);
                plane.UAxis.CastTo(out RH_Geo.Vector3d rh_UAxis); RH_Geo.Line rh_ULine = new RH_Geo.Line(rh_Origin, rh_UAxis);
                plane.VAxis.CastTo(out RH_Geo.Vector3d rh_VAxis); RH_Geo.Line rh_VLine = new RH_Geo.Line(rh_Origin, rh_VAxis);

                int max = 5;

                if (isSelected)
                {
                    display.DrawPoint(rh_Origin, RH_Disp.PointStyle.Circle, Color.Green, Color.Green, 3.5f, 0.0f, 0.0f, 0.0f, true, true);

                    display.DrawArrow(rh_ULine, Color.Green, 0d, rh_ULine.Length * 0.2);
                    display.DrawArrow(rh_VLine, Color.Green, 0d, rh_VLine.Length * 0.2);

                    for (int i = -max; i < max + 1; i++)
                    {
                        display.DrawLine(rh_Origin - (max * rh_UAxis) + (i * rh_VAxis), rh_Origin + (max * rh_UAxis) + (i * rh_VAxis), Color.Green, 1);
                        display.DrawLine(rh_Origin + (i * rh_UAxis) - (max * rh_VAxis), rh_Origin + (i * rh_UAxis) + (max * rh_VAxis), Color.Green, 1);
                    }
                }
                else
                {
                    display.DrawPoint(rh_Origin, RH_Disp.PointStyle.Circle, ColourPalette.Blue, ColourPalette.Blue, 3.0f, 0.0f, 0.0f, 0.0f, true, true);

                    display.DrawArrow(rh_ULine, ColourPalette.Blue, 0d, rh_ULine.Length * 0.2);
                    display.DrawArrow(rh_VLine, ColourPalette.Blue, 0d, rh_VLine.Length * 0.2);

                    for(int i = -max; i < max + 1; i++)
                    {
                        display.DrawLine(rh_Origin - (max * rh_UAxis) + (i * rh_VAxis), rh_Origin + (max * rh_UAxis) + (i * rh_VAxis), ColourPalette.Blue, 1);
                        display.DrawLine(rh_Origin + (i * rh_UAxis) - (max * rh_VAxis), rh_Origin + (i * rh_UAxis) + (max * rh_VAxis), ColourPalette.Blue, 1);
                    }

                    // Bad
                    // 0x11110000 => Full Line
                    // 0x01001001 => Not dense enough
                    // 0x11001100 & 0x10101010 => Big-Small pattern
                    // 0x01111110 = > Large Dot, Samll dots
                    // 0x01100110 => Small Dots, Large Space
                    // 0x00110011 => Small Dotted
                    // 0x01010101 => Larger Dotted : 
                    //
                    // Good

                }

            }


            /// <summary>
            /// Draws a <see cref="Euc3D.Sphere"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="sphere"> <see cref="Euc3D.Sphere"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Sphere(RH_Disp.DisplayPipeline display, Euc3D.Sphere sphere, bool isSelected)
            {
                sphere.CastTo(out RH_Geo.Sphere rh_Sphere);

                if (isSelected)
                {
                    display.DrawSphere(rh_Sphere, Color.Green, 3);
                }
                else
                {
                    display.DrawSphere(rh_Sphere, ColourPalette.Blue, 2);
                }

            }


            /// <summary>
            /// Draws a <see cref="He.Mesh{TPosition}"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="mesh"> <see cref="He.Mesh{TPosition}"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="He.Mesh{TPosition}"/> is selected or not. </param>
            public static void Mesh(RH_Disp.DisplayPipeline display, He.Mesh<Euc3D.Point> mesh, bool isSelected)
            {
                mesh.ConvertTo(out RH_Geo.Mesh rh_Mesh);

                if (isSelected)
                {
                    display.DrawMeshWires(rh_Mesh, Color.Green, 3);
                }
                else
                {
                    display.DrawMeshWires(rh_Mesh, ColourPalette.Blue, 2);
                }

            }

            /// <summary>
            /// Draws a <see cref="Fv.Mesh{TPosition}"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="mesh"> <see cref="Fv.Mesh{TPosition}"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Fv.Mesh{TPosition}"/> is selected or not. </param>
            public static void Mesh(RH_Disp.DisplayPipeline display, Fv.Mesh<Euc3D.Point> mesh, bool isSelected)
            {
                mesh.CastTo(out RH_Geo.Mesh rh_Mesh);

                if (isSelected)
                {
                    display.DrawMeshWires(rh_Mesh, Color.Green, 3);
                }
                else
                {
                    display.DrawMeshWires(rh_Mesh, ColourPalette.Blue, 2);
                }

            }


            /********** Manifold 3D **********/

            /// <summary>
            /// Draws a <see cref="Euc3D.Frame"/> in the <see cref="Rhino"/> viewports.
            /// </summary>
            /// <param name="display"> The Rhino helper for diplaying objects. </param>
            /// <param name="frame"> <see cref="Euc3D.Frame"/> to display. </param>
            /// <param name="isSelected"> Evaluates whether the <see cref="Euc3D.Point"/> is selected or not. </param>
            public static void Frame(RH_Disp.DisplayPipeline display, Euc3D.Frame frame, bool isSelected)
            {
                frame.Origin.CastTo(out RH_Geo.Point3d rh_Origin);
                frame.XAxis.CastTo(out RH_Geo.Vector3d rh_XAxis); RH_Geo.Line rh_XLine = new RH_Geo.Line(rh_Origin, rh_XAxis);
                frame.YAxis.CastTo(out RH_Geo.Vector3d rh_YAxis); RH_Geo.Line rh_YLine = new RH_Geo.Line(rh_Origin, rh_YAxis);
                frame.ZAxis.CastTo(out RH_Geo.Vector3d rh_ZAxis); RH_Geo.Line rh_ZLine = new RH_Geo.Line(rh_Origin, rh_ZAxis);



                if (isSelected)
                {
                    display.DrawPoint(rh_Origin, RH_Disp.PointStyle.Circle, Color.Green, Color.Green, 3.5f, 0.0f, 0.0f, 0.0f, true, true);

                    display.DrawArrow(rh_XLine, Color.Green, 0d, rh_XLine.Length * 0.2);
                    display.DrawArrow(rh_YLine, Color.Green, 0d, rh_YLine.Length * 0.2);
                    display.DrawArrow(rh_ZLine, Color.Green, 0d, rh_ZLine.Length * 0.2);
                }
                else
                {
                    display.DrawPoint(rh_Origin, RH_Disp.PointStyle.Circle, ColourPalette.Blue, ColourPalette.Blue, 3.0f, 0.0f, 0.0f, 0.0f, true, true);

                    display.DrawArrow(rh_XLine, ColourPalette.Blue, 0d, rh_XLine.Length * 0.2);
                    display.DrawArrow(rh_YLine, ColourPalette.Blue, 0d, rh_YLine.Length * 0.2);
                    display.DrawArrow(rh_ZLine, ColourPalette.Blue, 0d, rh_ZLine.Length * 0.2);
                }
            }
        
        }
    }
}
