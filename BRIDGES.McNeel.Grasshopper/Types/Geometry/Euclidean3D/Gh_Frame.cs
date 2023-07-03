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
    /// Class defining a grasshopper type for an <see cref="Euc3D.Frame"/>.
    /// </summary>
    public class Gh_Frame : GH_Types.GH_Goo<Euc3D.Frame>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                this.Value.Origin.CastTo(out RH_Geo.Point3d origin);

                this.Value.XAxis.CastTo(out RH_Geo.Vector3d xAxis);
                this.Value.YAxis.CastTo(out RH_Geo.Vector3d yAxis);
                this.Value.ZAxis.CastTo(out RH_Geo.Vector3d zAxis);

                return new RH_Geo.BoundingBox(origin, origin + xAxis + yAxis + zAxis);
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
        /// Initialises a new instance of <see cref= "Gh_Frame" /> class.
        /// </summary>
        public Gh_Frame() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Frame" /> class from another <see cref="Gh_Frame"/>..
        /// </summary>
        /// <param name="gh_Frame"> <see cref="Gh_Frame"/> to duplicate. </param>
        public Gh_Frame(Gh_Frame gh_Frame)
        {
            this.Value = gh_Frame.Value;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Frame"/> class from a <see cref="Euc3D.Frame"/>.
        /// </summary>
        /// <param name="frame"> <see cref="Euc3D.Frame"/> to duplicate.</param>
        public Gh_Frame(Euc3D.Frame frame)
        {
            this.Value = frame;
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
            Draw.Wireframe.Frame(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        /********** Properties **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Frame)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Frame); } }


        /********** Methods **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return $"Frame (O:{this.Value.Origin}, {this.Value.Dimension}D)";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Frame(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();


            /******************** BRIDGES Objects ********************/

            // Cast a Euc3D.Frame to a Gh_Frame
            if (typeof(Euc3D.Frame).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Frame)source;

                return true;
            }
            // Cast a Euc3D.Plane to a Gh_Frame
            if (typeof(Euc3D.Plane).IsAssignableFrom(type))
            {
                Euc3D.Plane plane = (Euc3D.Plane)source;

                this.Value = new Euc3D.Frame(plane.Origin, plane.UAxis, plane.VAxis, plane.Normal);

                return true;
            }

            /******************** Rhino Objects ********************/

            // Casts a RH_Geo.Plane to a Gh_Frame
            if (typeof(RH_Geo.Plane).IsAssignableFrom(type))
            {
                ((RH_Geo.Plane)source).ConvertTo(out Euc3D.Frame frame);
                this.Value = frame;

                return true;
            }


            /******************** BRIDGES.McNeel.Grasshopper Objects ********************/

            // Casts a Gh_Plane to a Gh_Frame
            if (typeof(Gh_Plane).IsAssignableFrom(type))
            {
                Gh_Plane gh_Vector = (Gh_Plane)source;

                this.Value = new Euc3D.Frame(gh_Vector.Value.Origin, gh_Vector.Value.UAxis, gh_Vector.Value.VAxis, gh_Vector.Value.Normal);

                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Casts a GH_Types.GH_Plane to a Gh_Frame
            if (typeof(GH_Types.GH_Plane).IsAssignableFrom(type))
            {
                GH_Types.GH_Plane rh_Plane = (GH_Types.GH_Plane)source;

                rh_Plane.Value.ConvertTo(out Euc3D.Frame frame);
                this.Value = frame;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            /******************** BRIDGES Objects ********************/

            // Casts a Gh_Frame to a Euc3D.Frame
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Frame)))
            {
                target = (T)(object)this.Value;

                return true;
            }
            // Casts a Gh_Frame to a Euc3D.Plane
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Plane)))
            {
                Euc3D.Plane plane = new Euc3D.Plane(this.Value.Origin, this.Value.XAxis, this.Value.YAxis, this.Value.ZAxis);

                target = (T)(object)plane;

                return true;
            }

            /******************** Rhino Objects ********************/

            // Casts a Gh_Frame to a RH_Geo.Plane
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Plane)))
            {
                try
                {
                    this.Value.CastTo(out RH_Geo.Plane rh_Plane);

                    target = (T)(object)rh_Plane;

                    return true;
                }
                catch(InvalidCastException) { /* Do Nothing */ }
            }


            /******************** BRIDGES.McNeel.Grasshopper Objects ********************/

            // Casts a Gh_Frame to a Gh_Plane
            if (typeof(T).IsAssignableFrom(typeof(Gh_Plane)))
            {
                Euc3D.Plane plane = new Euc3D.Plane(this.Value.Origin, this.Value.XAxis, this.Value.YAxis, this.Value.ZAxis);

                Gh_Plane gh_Vector = new Gh_Plane(plane);
                target = (T)(object)gh_Vector;

                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Casts a Gh_Frame to a GH_Types.GH_Plane
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Plane)))
            {
                try
                {
                    this.Value.CastTo(out RH_Geo.Plane rh_Plane);

                    GH_Types.GH_Plane gh_Plane = new GH_Types.GH_Plane(rh_Plane);
                    target = (T)(object)gh_Plane;

                    return true;
                }
                catch (InvalidCastException) { /* Do Nothing */ }
            }


            /******************** Otherwise ********************/

            return false;
        }

        #endregion
    }
}
