using System;

using Euc3D = BRIDGES.Geometry.Euclidean3D;

using RH_Geo = Rhino.Geometry;

using BRIDGES.McNeel.Rhino.Extensions.Geometry.Euclidean3D;

using GH_Kernel = Grasshopper.Kernel;
using GH_Types = Grasshopper.Kernel.Types;

using BRIDGES.McNeel.Grasshopper.Display.Geometry.Euclidean3D;


namespace BRIDGES.McNeel.Grasshopper.Types.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a grasshopper type for an <see cref="Euc3D.Polyline"/>.
    /// </summary>
    public class Gh_Polyline : GH_Types.GH_Goo<Euc3D.Polyline>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                this.Value.CastTo(out RH_Geo.Polyline rh_Polyline);

                return new RH_Geo.BoundingBox(rh_Polyline);
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
        /// Initialises a new instance of <see cref= "Gh_Polyline" /> class.
        /// </summary>
        public Gh_Polyline() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Polyline" /> class from another <see cref="Gh_Polyline"/>..
        /// </summary>
        /// <param name="gh_Polyline"> <see cref="Gh_Polyline"/> to duplicate. </param>
        public Gh_Polyline(Gh_Polyline gh_Polyline)
        {
            this.Value = gh_Polyline.Value;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Polyline"/> class from a <see cref="Euc3D.Segment"/>.
        /// </summary>
        /// <param name="polyline"> <see cref="Euc3D.Segment"/> to duplicate.</param>
        public Gh_Polyline(Euc3D.Polyline polyline)
        {
            this.Value = polyline;
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
            Draw.Wireframe.Polyline(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        /********** Properties **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Polyline)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Polyline); } }


        /********** Methods **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return this.Value.IsClosed ? $"Closed Polyline (V:{this.Value.VertexCount}" : $"Open Polyline (V:{this.Value.VertexCount}";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Polyline(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();

            /******************** BRIDGES Objects ********************/

            // Cast a Euc3D.Polyline to a Gh_Polyline
            if (typeof(Euc3D.Polyline).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Polyline)source;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Cast a RH_Geo.Polyline to a Gh_Polyline
            if (typeof(RH_Geo.Polyline).IsAssignableFrom(type))
            {
                RH_Geo.Polyline rh_Polyline = (RH_Geo.Polyline)source;

                rh_Polyline.CastTo(out Euc3D.Polyline polyline);
                this.Value = polyline;

                return true;
            }
            // Cast a RH_Geo.PolylineCurve to a Gh_Polyline
            if (typeof(RH_Geo.PolylineCurve).IsAssignableFrom(type))
            {
                RH_Geo.PolylineCurve rh_PolylineCurve = (RH_Geo.PolylineCurve)source;

                rh_PolylineCurve.CastTo(out Euc3D.Polyline polyline);
                this.Value = polyline;

                return true;
            }
            // Cast a RH_Geo.Curve to a Gh_Polyline
            if (typeof(RH_Geo.Curve).IsAssignableFrom(type))
            {
                RH_Geo.Curve rh_Curve = (RH_Geo.Curve)source;

                if (rh_Curve.TryGetPolyline(out RH_Geo.Polyline rh_Polyline))
                {
                    rh_Polyline.CastTo(out Euc3D.Polyline polyline);
                    this.Value = polyline;

                    return true;
                }
            }


            /******************** Grasshopper Objects ********************/

            // Cast a GH_Types.GH_Curve to a Gh_Polyline
            if (typeof(GH_Types.GH_Curve).IsAssignableFrom(type))
            {
                RH_Geo.Curve rh_Curve = ((GH_Types.GH_Curve)source).Value;

                if (rh_Curve.TryGetPolyline(out RH_Geo.Polyline rh_Polyline))
                {
                    rh_Polyline.CastTo(out Euc3D.Polyline polyline);
                    this.Value = polyline;

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

            // Casts a Gh_Polyline to a Euc3D.Polyline
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Polyline)))
            {
                object polyline = this.Value;
                target = (T)polyline;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Casts a Gh_Polyline to a RH_Geo.Polyline
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Polyline)))
            {
                this.Value.CastTo(out RH_Geo.Polyline rh_Polyline);

                target = (T)(object)rh_Polyline;
                return true;
            }
            // Casts a Gh_Polyline to a RH_Geo.Polyline
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.PolylineCurve)))
            {
                this.Value.CastTo(out RH_Geo.PolylineCurve rh_PolylineCurve);

                target = (T)(object)rh_PolylineCurve;
                return true;
            }
            // Casts a Gh_Polyline to a RH_Geo.Curve
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Curve)))
            {
                this.Value.CastTo(out RH_Geo.PolylineCurve rh_PolylineCurve);

                target = (T)(object)rh_PolylineCurve;
                return true;
            }

            /******************** Grasshopper Objects ********************/

            // Casts a Gh_Polyline to a GH_Types.GH_Curve
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Curve)))
            {
                this.Value.CastTo(out RH_Geo.PolylineCurve rh_PolylineCurve);

                GH_Types.GH_Curve gh_Curve = new GH_Types.GH_Curve(rh_PolylineCurve);
                target = (T)(object)gh_Curve;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        #endregion
    }
}
