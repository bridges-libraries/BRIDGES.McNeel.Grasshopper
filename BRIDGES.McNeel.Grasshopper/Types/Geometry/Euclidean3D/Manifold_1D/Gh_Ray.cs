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
    /// Class defining a grasshopper type for an <see cref="Euc3D.Ray"/>.
    /// </summary>
    public class Gh_Ray : GH_Types.GH_Goo<Euc3D.Ray>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                this.Value.Origin.CastTo(out RH_Geo.Point3d rh_Start);
                this.Value.Axis.CastTo(out RH_Geo.Vector3d rh_Direction);

                return new RH_Geo.BoundingBox(rh_Start, rh_Start + rh_Direction);
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
        /// Initialises a new instance of <see cref= "Gh_Ray" /> class.
        /// </summary>
        public Gh_Ray() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Ray" /> class from another <see cref="Gh_Ray"/>..
        /// </summary>
        /// <param name="gh_Ray"> <see cref="Gh_Ray"/> to duplicate. </param>
        public Gh_Ray(Gh_Ray gh_Ray)
        {
            this.Value = gh_Ray.Value;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Ray"/> class from a <see cref="Euc3D.Ray"/>.
        /// </summary>
        /// <param name="ray"> <see cref="Euc3D.Ray"/> to duplicate.</param>
        public Gh_Ray(Euc3D.Ray ray)
        {
            this.Value = ray;
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
            Draw.Wireframe.Ray(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        /********** Properties **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Ray)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Ray); } }


        /********** Methods **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return $"Ray (O:{this.Value.Origin}, V:{this.Value.Axis})";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Ray(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();

            /******************** BRIDGES Objects ********************/

            // Cast a Euc3D.Ray to a Gh_Ray
            if (typeof(Euc3D.Ray).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Ray)source;
                return true;
            }
            // Cast a Euc3D.Line to a Gh_Ray
            if (typeof(Euc3D.Segment).IsAssignableFrom(type))
            {
                Euc3D.Line line = (Euc3D.Line)source;
                this.Value = new Euc3D.Ray(line.Origin, line.Axis);

                return true;
            }


            /******************** Rhino Objects ********************/

            // Cast a RH_Geo.Ray3d to a Gh_Ray
            if (typeof(RH_Geo.Ray3d).IsAssignableFrom(type))
            {
                ((RH_Geo.Ray3d)source).CastTo(out Euc3D.Ray ray);
                this.Value = ray;

                return true;
            }


            /******************** BRIDGES.McNeel.Grasshopper Objects ********************/

            // Cast a Gh_Line to a Gh_Ray
            if (typeof(Gh_Line).IsAssignableFrom(type))
            {
                Gh_Line gh_Line = (Gh_Line)source;

                this.Value = new Euc3D.Ray(gh_Line.Value.Origin, gh_Line.Value.Axis);

                return true;
            }

            /******************** Grasshopper Objects ********************/

            /* Do Nothing */

            /******************** Otherwise ********************/

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            /******************** BRIDGES Objects ********************/

            // Casts a Gh_Ray to a Euc3D.Ray
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Ray)))
            {
                target = (T)(object)this.Value;
                return true;
            }
            // Casts a Gh_Ray to a Euc3D.Line
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Segment)))
            {
                Euc3D.Line segment = new Euc3D.Line(this.Value.Origin, this.Value.Axis);

                target = (T)(object)segment;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Casts a Gh_Ray to a RH_Geo.Ray3d
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Ray3d)))
            {
                this.Value.CastTo(out RH_Geo.Ray3d rh_Ray3d);

                target = (T)(object)rh_Ray3d;
                return true;
            }


            /******************** BRIDGES.McNeel.Grasshopper Objects ********************/

            // Casts a Gh_Ray to a Gh_Line
            if (typeof(T).IsAssignableFrom(typeof(Gh_Line)))
            {
                Euc3D.Line line = new Euc3D.Line(this.Value.Origin, this.Value.Axis);

                Gh_Line gh_Line = new Gh_Line(line);
                target = (T)(object)gh_Line;

                return true;
            }


            /******************** Grasshopper Objects ********************/

            /* Do Nothing */

            /******************** Otherwise ********************/

            return false;
        }

        #endregion
    }
}
