using System;
using System.Drawing;

using Euc3D = BRIDGES.Geometry.Euclidean3D;
using He = BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh;
using Fv = BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh;

using RH_Geo = Rhino.Geometry;
using RH_Disp = Rhino.Display;

using BRIDGES.McNeel.Rhino.Extensions.Geometry.Euclidean3D;

using GH = Grasshopper;


namespace BRIDGES.McNeel.Grasshopper.Display.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining methods for the display of three-dimensional euclidean objects.
    /// </summary>
    public static class Draw
    {
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
                display.DrawPoint(rh_Point, RH_Disp.PointStyle.Circle, Color.Green, Color.Green, 5.0f, 1.0f, 0.0f, 0.0f, true, true);
            }
            else
            {
                display.DrawPoint(rh_Point, RH_Disp.PointStyle.Circle, Color.Black, ColorTranslator.FromHtml("#47B3D8"), 4.0f, 1.0f, 0.0f, 0.0f, true, true);
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

            if(isSelected)
            {
                display.DrawArrowHead((RH_Geo.Point3d)rh_Vector, rh_Vector, Color.Green, 0.0, vector.Length() * 0.25);
                display.DrawLine(new RH_Geo.Line(new RH_Geo.Point3d(0.0, 0.0, 0.0), (RH_Geo.Point3d)rh_Vector), Color.Green, 3);
            }
            else
            {
                display.DrawArrowHead((RH_Geo.Point3d)rh_Vector, rh_Vector, ColorTranslator.FromHtml("#47B3D8"), 0.0, vector.Length() * 0.20);
                display.DrawLine(new RH_Geo.Line(new RH_Geo.Point3d(0.0, 0.0, 0.0), (RH_Geo.Point3d)rh_Vector), ColorTranslator.FromHtml("#47B3D8"), 2);
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
                display.DrawLine(rh_Line, Color.Green, 5);
            }
            else
            {
                display.DrawLine(rh_Line, ColorTranslator.FromHtml("#47B3D8"), 5);
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
            
            if (isSelected)
            {
                display.DrawLineNoClip(rh_Line.From, rh_Line.To, Color.Green, 5);
            }
            else
            {
                display.DrawLineNoClip(rh_Line.From, rh_Line.To, ColorTranslator.FromHtml("#47B3D8"), 5);
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

            /* version A */
            display.DrawLineNoClip(rh_Ray.Position, rh_Ray.Position + rh_Ray.Direction, Color.LightGray, 5);


            /* version B */

            RH_Disp.PointStyle previewPointStyle = GH.CentralSettings.PreviewPointStyle;
            display.DrawPoint(rh_Ray.Position, previewPointStyle, 5, Color.DarkGray);

            display.DrawArrow(new RH_Geo.Line(rh_Ray.Position, rh_Ray.Position + rh_Ray.Direction), Color.DarkGray, 10.0, 0.0) ;
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
                display.DrawPoints(rh_Polyine, RH_Disp.PointStyle.Circle, Color.Green, Color.Green, 5.0f, 1.0f, 0.0f, 0.0f, true, true);
                display.DrawPolyline(rh_Polyine, Color.Green, 5);
            }
            else
            {
                display.DrawPoints(rh_Polyine, RH_Disp.PointStyle.Circle, ColorTranslator.FromHtml("#47B3D8"), ColorTranslator.FromHtml("#47B3D8"), 5.0f, 1.0f, 0.0f, 0.0f, true, true);
                display.DrawPolyline(rh_Polyine, ColorTranslator.FromHtml("#47B3D8"), 5);
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
                display.DrawCircle(rh_Circle, Color.Green, 5);
            }
            else
            {
                display.DrawCircle(rh_Circle, ColorTranslator.FromHtml("#47B3D8"), 5);
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
            Draw.Point(display, plane.Origin, isSelected);

            Draw.Vector(display, plane.UAxis, isSelected);
            Draw.Vector(display, plane.VAxis, isSelected);
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
                display.DrawSphere(rh_Sphere, Color.Green, 5);
            }
            else
            {
                display.DrawSphere(rh_Sphere, ColorTranslator.FromHtml("#47B3D8"), 5);
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
                display.DrawMeshWires(rh_Mesh, Color.Green, 5);
            }
            else
            {
                display.DrawMeshWires(rh_Mesh, ColorTranslator.FromHtml("#47B3D8"), 5);
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
                display.DrawMeshWires(rh_Mesh, Color.Green, 5);
            }
            else
            {
                display.DrawMeshWires(rh_Mesh, ColorTranslator.FromHtml("#47B3D8"), 5);
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
            Draw.Point(display, frame.Origin, isSelected);

            Draw.Vector(display, frame.XAxis, isSelected);
            Draw.Vector(display, frame.YAxis, isSelected);
            Draw.Vector(display, frame.ZAxis, isSelected);
        }
    }
}
