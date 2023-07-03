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
    /// Class defining a grasshopper type for an <see cref="Euc3D.Circle"/>.
    /// </summary>
    public class Gh_Circle : GH_Types.GH_Goo<Euc3D.Circle>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                this.Value.CastTo(out RH_Geo.Circle rh_Circle);

                return rh_Circle.BoundingBox;
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
        /// Initialises a new instance of <see cref= "Gh_Circle" /> class.
        /// </summary>
        public Gh_Circle() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Circle" /> class from another <see cref="Gh_Circle"/>..
        /// </summary>
        /// <param name="gh_Circle"> <see cref="Gh_Circle"/> to duplicate. </param>
        public Gh_Circle(Gh_Circle gh_Circle)
        {
            this.Value = gh_Circle.Value;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Segment"/> class from a <see cref="Euc3D.Circle"/>.
        /// </summary>
        /// <param name="circle"> <see cref="Euc3D.Circle"/> to duplicate.</param>
        public Gh_Circle(Euc3D.Circle circle)
        {
            this.Value = circle;
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
            Draw.Wireframe.Circle(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        /********** Properties **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Circle)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Circle); } }


        /********** Methods **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return $"Circle (O:{this.Value.Centre}, R:{this.Value.Radius})";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Circle(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();

            /******************** BRIDGES Objects ********************/

            // Cast a Euc3D.Cirle to a Gh_Circle
            if (typeof(Euc3D.Circle).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Circle)source;

                return true;
            }


            /******************** Rhino Objects ********************/

            // Cast a RH_Geo.Circle to a Gh_Circle
            if (typeof(RH_Geo.Circle).IsAssignableFrom(type))
            {
                RH_Geo.Circle rh_Circle = (RH_Geo.Circle)source;

                rh_Circle.CastTo(out Euc3D.Circle circle);
                this.Value = circle;

                return true;
            }
            // Cast a RH_Geo.Arc to a Gh_Circle
            if (typeof(RH_Geo.Arc).IsAssignableFrom(type))
            {
                RH_Geo.Arc rh_Arc= (RH_Geo.Arc)source;

                if (rh_Arc.IsCircle)
                {
                    rh_Arc.ConvertTo(out Euc3D.Circle circle);
                    this.Value = circle;

                    return true;
                }
            }
            // Cast a RH_Geo.ArcCurve to a Gh_Circle
            if (typeof(RH_Geo.ArcCurve).IsAssignableFrom(type))
            {
                RH_Geo.ArcCurve rh_ArcCurve = (RH_Geo.ArcCurve)source;

                if (rh_ArcCurve.TryGetCircle(out RH_Geo.Circle rh_Circle))
                {
                    rh_Circle.CastTo(out Euc3D.Circle circle);
                    this.Value = circle;

                    return true;
                }
            }
            // Cast a RH_Geo.Curve to a Gh_Circle
            if (typeof(RH_Geo.Curve).IsAssignableFrom(type))
            {
                RH_Geo.Curve rh_Curve = (RH_Geo.Curve)source;

                if (rh_Curve.TryGetCircle(out RH_Geo.Circle rh_Circle))
                {
                    rh_Circle.CastTo(out Euc3D.Circle circle);
                    this.Value = circle;

                    return true;
                }
            }


            /******************** Grasshopper Objects ********************/

            // Cast a GH_Types.GH_Circle to a Gh_Circle
            if (typeof(GH_Types.GH_Circle).IsAssignableFrom(type))
            {
                RH_Geo.Circle rh_Circle = ((GH_Types.GH_Circle)source).Value;

                rh_Circle.CastTo(out Euc3D.Circle circle);
                this.Value = circle;

                return true;
            }
            // Cast a GH_Types.GH_Arc to a Gh_Circle
            if (type == typeof(GH_Types.GH_Arc))
            {
                RH_Geo.Arc rh_Arc = ((GH_Types.GH_Arc)source).Value;

                if (rh_Arc.IsCircle)
                {
                    rh_Arc.ConvertTo(out Euc3D.Circle circle);
                    this.Value = circle;

                    return true;
                }
            }
            // Cast a GH_Types.GH_Curve to a Gh_Circle
            if (type == typeof(GH_Types.GH_Curve))
            {
                RH_Geo.Curve rh_Curve = ((GH_Types.GH_Curve)source).Value;

                if (rh_Curve.TryGetCircle(out RH_Geo.Circle rh_Circle))
                {
                    rh_Circle.CastTo(out Euc3D.Circle circle);
                    this.Value = circle;

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

            // Casts a Gh_Circle to a Euc3D.Circle
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Circle)))
            {
                object segment = this.Value;
                target = (T)segment;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Casts a Gh_Circle to a RH_Geo.Line
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Circle)))
            {
                this.Value.CastTo(out RH_Geo.Circle rh_Circle);

                target = (T)(object)rh_Circle;
                return true;
            }
            // Casts a Gh_Circle to a RH_Geo.Arc
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Arc)))
            {
                this.Value.CastTo(out RH_Geo.Arc rh_Arc);

                target = (T)(object)rh_Arc;
                return true;
            }
            // Casts a Gh_Circle to a RH_Geo.LineCurve
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.ArcCurve)))
            {
                this.Value.CastTo(out RH_Geo.ArcCurve rh_ArcCurve);

                target = (T)(object)rh_ArcCurve;

                return true;
            }
            // Casts a Gh_Circle to a RH_Geo.Curve
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Curve)))
            {
                this.Value.CastTo(out RH_Geo.ArcCurve rh_ArcCurve);
                target = (T)(object)rh_ArcCurve;

                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Casts a Gh_Circle to a GH_Types.GH_Circle
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Circle)))
            {
                this.Value.CastTo(out RH_Geo.Circle rh_Circle);

                GH_Types.GH_Circle gh_Circle = new GH_Types.GH_Circle(rh_Circle);
                target = (T)(object)gh_Circle;

                return true;
            }
            // Casts a Gh_Circle to a GH_Types.GH_Arc
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Arc)))
            {
                this.Value.CastTo(out RH_Geo.Arc rh_Arc);

                GH_Types.GH_Arc gh_Arc = new GH_Types.GH_Arc(rh_Arc);
                target = (T)(object)gh_Arc;

                return true;
            }
            // Casts a Gh_Circle to a GH_Types.GH_Curve
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Curve)))
            {
                this.Value.CastTo(out RH_Geo.ArcCurve rh_ArcCurve);

                GH_Types.GH_Curve gh_Curve = new GH_Types.GH_Curve(rh_ArcCurve);
                target = (T)(object)gh_Curve;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        #endregion
    }
}
