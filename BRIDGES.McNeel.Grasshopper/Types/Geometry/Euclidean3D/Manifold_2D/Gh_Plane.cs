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
    /// Class defining a grasshopper type for an <see cref="Euc3D.Plane"/>.
    /// </summary>
    public class Gh_Plane : GH_Types.GH_Goo<Euc3D.Plane>, GH_Kernel.IGH_PreviewData
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

                this.Value.UAxis.CastTo(out RH_Geo.Vector3d xAxis);
                this.Value.VAxis.CastTo(out RH_Geo.Vector3d yAxis);
                this.Value.Normal.CastTo(out RH_Geo.Vector3d zAxis);

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
        /// Initialises a new instance of <see cref= "Gh_Plane" /> class.
        /// </summary>
        public Gh_Plane() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Plane" /> class from another <see cref="Gh_Plane"/>..
        /// </summary>
        /// <param name="gh_Plane"> <see cref="Gh_Plane"/> to duplicate. </param>
        public Gh_Plane(Gh_Plane gh_Plane)
        {
            this.Value = gh_Plane.Value;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Plane"/> class from a <see cref="Euc3D.Plane"/>.
        /// </summary>
        /// <param name="plane"> <see cref="Euc3D.Plane"/> for the grasshopper type.</param>
        public Gh_Plane(Euc3D.Plane plane)
        {
            this.Value = plane;
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
            Draw.Wireframe.Plane(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        /********** Properties **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Plane)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Plane); } }


        /********** Methods **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return $"Plane (O:{this.Value.Origin}, N:{this.Value.Normal})";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Plane(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();

            /******************** BRIDGES Objects ********************/

            // Cast a Euc3D.Plane to a Gh_Plane
            if (typeof(Euc3D.Plane).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Plane)source;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Cast a RH_Geo.Plane to a Gh_Plane
            if (typeof(RH_Geo.Plane).IsAssignableFrom(type))
            {
                RH_Geo.Plane rh_Plane = (RH_Geo.Plane)source;

                rh_Plane.CastTo(out Euc3D.Plane plane);
                this.Value = plane;

                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Cast a GH_Types.GH_Plane to a Gh_Plane
            if (typeof(GH_Types.GH_Plane).IsAssignableFrom(type))
            {
                RH_Geo.Plane rh_Plane = ((GH_Types.GH_Plane)source).Value;

                rh_Plane.CastTo(out Euc3D.Plane plane);
                this.Value = plane;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            /******************** BRIDGES Objects ********************/

            // Casts a Gh_Plane to a Euc3D.Plane
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Plane)))
            {
                object plane = this.Value;
                target = (T)plane;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Casts a Gh_Plane to a RH_Geo.Plane
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Plane)))
            {
                this.Value.CastTo(out RH_Geo.Plane rh_Plane);

                target = (T)(object)rh_Plane;
                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Casts a Gh_Plane to a GH_Types.GH_Plane
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Plane)))
            {
                this.Value.CastTo(out RH_Geo.Plane rh_Plane);

                GH_Types.GH_Plane gh_Plane = new GH_Types.GH_Plane(rh_Plane);
                target = (T)(object)gh_Plane;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        #endregion
    }
}
