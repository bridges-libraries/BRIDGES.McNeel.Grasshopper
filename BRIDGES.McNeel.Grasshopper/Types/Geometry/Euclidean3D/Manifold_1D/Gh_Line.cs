﻿using System;

using Euc3D = BRIDGES.Geometry.Euclidean3D;

using RH_Geo = Rhino.Geometry;

using BRIDGES.McNeel.Rhino.Extensions.Geometry.Euclidean3D;

using GH_Kernel = Grasshopper.Kernel;
using GH_Types = Grasshopper.Kernel.Types;

using BRIDGES.McNeel.Grasshopper.Display.Geometry.Euclidean3D;


namespace BRIDGES.McNeel.Grasshopper.Types.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a grasshopper type for an <see cref="Euc3D.Line"/>.
    /// </summary>
    public class Gh_Line : GH_Types.GH_Goo<Euc3D.Line>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                Euc3D.Point origin = this.Value.Origin;
                Euc3D.Point point = this.Value.Origin + this.Value.Axis;

                origin.CastTo(out RH_Geo.Point3d rh_Origin);
                point.CastTo(out RH_Geo.Point3d rh_Point);

                return new RH_Geo.BoundingBox(rh_Origin, rh_Point);
            }
        }

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewData.ClippingBox"/>
        public RH_Geo.BoundingBox ClippingBox
        {
            get { return this.Boundingbox; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Line" /> class.
        /// </summary>
        public Gh_Line() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Line" /> class from another <see cref="Gh_Line"/>..
        /// </summary>
        /// <param name="gh_Line"> <see cref="Gh_Line"/> to duplicate. </param>
        public Gh_Line(Gh_Line gh_Line)
        {
            this.Value = gh_Line.Value;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Line"/> class from a <see cref="Euc3D.Line"/>.
        /// </summary>
        /// <param name="line"> <see cref="Euc3D.Line"/> to duplicate.</param>
        public Gh_Line(Euc3D.Line line)
        {
            this.Value = line;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewData.DrawViewportMeshes(GH_Kernel.GH_PreviewMeshArgs)"/>
        public void DrawViewportMeshes(GH_Kernel.GH_PreviewMeshArgs args)
        {
            /* Do Nothing */
        }

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewData.DrawViewportWires(GH_Kernel.GH_PreviewWireArgs)"/>
        public void DrawViewportWires(GH_Kernel.GH_PreviewWireArgs args)
        {
            Draw.Line(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        /********** Properties **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Line)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Line); } }


        /********** Methods **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return string.Format($"Line starting at {this.Value.Origin}, parallel to {this.Value.Axis}.");
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Line(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();

            /******************** BRIDGES Objects ********************/

            // Cast a Euc3D.Line to a Gh_Line
            if (typeof(Euc3D.Line).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Line)source;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Cast a RH_Geo.Line to a Gh_Line
            if (typeof(RH_Geo.Line).IsAssignableFrom(type))
            {
                RH_Geo.Line rh_Line = (RH_Geo.Line)source;

                rh_Line.CastTo(out Euc3D.Line line);
                this.Value = line;

                return true;
            }
            // Cast a RH_Geo.LineCurve to a Gh_Line
            else if (typeof(RH_Geo.LineCurve).IsAssignableFrom(type))
            {
                RH_Geo.LineCurve rh_LineCurve = (RH_Geo.LineCurve)source;

                rh_LineCurve.CastTo(out Euc3D.Line line);
                this.Value = line;

                return true;
            }
            // Cast a RH_Geo.Curve to a Gh_Line
            else if(typeof(RH_Geo.Curve).IsAssignableFrom(type))
            {
                RH_Geo.Curve rh_Curve = (RH_Geo.Curve)source;

                if (rh_Curve.IsLinear())
                {
                    rh_Curve.PointAtStart.CastTo(out Euc3D.Point start);
                    rh_Curve.PointAtEnd.CastTo(out Euc3D.Point end);

                    this.Value = new Euc3D.Line(start, end);

                    return true;
                }
            }


            /******************** Grasshopper Objects ********************/

            // Cast a GH_Types.GH_Line to a Gh_Line
            if (typeof(GH_Types.GH_Line).IsAssignableFrom(type))
            {
                RH_Geo.Line rh_Line = ((GH_Types.GH_Line)source).Value;

                rh_Line.CastTo(out Euc3D.Line line);
                this.Value = line;

                return true;
            }
            // Cast a GH_Types.GH_Curve to a Gh_Curve
            else if (typeof(GH_Types.GH_Curve).IsAssignableFrom(type))
            {
                RH_Geo.Curve rh_Curve = ((GH_Types.GH_Curve)source).Value;

                if (rh_Curve.IsLinear())
                {
                    rh_Curve.PointAtStart.CastTo(out Euc3D.Point start);
                    rh_Curve.PointAtEnd.CastTo(out Euc3D.Point end);

                    this.Value = new Euc3D.Line(start, end);

                    return true;
                }
            }


            /******************** Otherwise ********************/

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            /******************** BRIDGES Objects ********************/

            // Casts a Gh_Line to a Euc3D.Line
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Line)))
            {
                object line = this.Value;
                target = (T)line;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Casts a Gh_Line to a RH_Geo.Line
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Line)))
            {
                this.Value.CastTo(out RH_Geo.Line rh_Line);

                target = (T)(object)rh_Line;
                return true;
            }
            // Casts a Gh_Line to a RH_Geo.LineCurve
            else if (typeof(T).IsAssignableFrom(typeof(RH_Geo.LineCurve)))
            {
                this.Value.CastTo(out RH_Geo.LineCurve rh_LineCurve);

                target = (T)(object)rh_LineCurve;

                return true;
            }
            // Casts a Gh_Line to a RH_Geo.Curve
            else if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Curve)))
            {
                this.Value.CastTo(out RH_Geo.LineCurve rh_LineCurve);
                target = (T)(object)rh_LineCurve;

                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Casts a Gh_Line to a GH_Types.Gh_Line
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Line)))
            {
                this.Value.CastTo(out RH_Geo.Line rh_Line);

                GH_Types.GH_Line gh_Line = new GH_Types.GH_Line(rh_Line);
                target = (T)(object)gh_Line;

                return true;
            }
            // Casts a Gh_Line to a GH_Types.GH_Curve
            else if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Curve)))
            {
                this.Value.CastTo(out RH_Geo.LineCurve rh_LineCurve);

                GH_Types.GH_Curve gh_Curve = new GH_Types.GH_Curve(rh_LineCurve);
                target = (T)(object)gh_Curve;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        #endregion
    }
}
