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
    /// Class defining a grasshopper type for an <see cref="Euc3D.Sphere"/>.
    /// </summary>
    public class Gh_Sphere : GH_Types.GH_Goo<Euc3D.Sphere>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                this.Value.CastTo(out RH_Geo.Sphere rh_Sphere);
                return rh_Sphere.BoundingBox; 
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
        /// Initialises a new instance of <see cref= "Gh_Sphere" /> class.
        /// </summary>
        public Gh_Sphere() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Sphere" /> class from another <see cref="Gh_Sphere"/>..
        /// </summary>
        /// <param name="gh_Sphere"> <see cref="Gh_Sphere"/> to duplicate. </param>
        public Gh_Sphere(Gh_Sphere gh_Sphere)
        {
            this.Value = gh_Sphere.Value;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Sphere"/> class from a <see cref="Euc3D.Sphere"/>.
        /// </summary>
        /// <param name="sphere"> <see cref="Euc3D.Sphere"/> to duplicate.</param>
        public Gh_Sphere(Euc3D.Sphere sphere)
        {
            this.Value = sphere;
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
            Draw.Sphere(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        /********** Properties **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Sphere)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Sphere); } }


        /********** Methods **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return string.Format($"Sphere centred at {this.Value.Centre}, of radius {this.Value.Radius}.");
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Sphere(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();

            /******************** BRIDGES Objects ********************/

            // Cast a Euc3D.Sphere to a Gh_Sphere
            if (typeof(Euc3D.Sphere).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Sphere)source;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Cast a RH_Geo.Sphere to a Gh_Sphere
            if (typeof(RH_Geo.Sphere).IsAssignableFrom(type))
            {
                RH_Geo.Sphere rh_Sphere = (RH_Geo.Sphere)source;

                rh_Sphere.CastTo(out Euc3D.Sphere sphere);
                this.Value = sphere;

                return true;
            }
            // Cast a RH_Geo.Surface to a Gh_Sphere
            if (typeof(RH_Geo.Surface).IsAssignableFrom(type))
            {
                RH_Geo.Surface rh_Surface = (RH_Geo.Surface)source;

                if (rh_Surface.TryGetSphere(out RH_Geo.Sphere rh_Sphere))
                {
                    rh_Sphere.CastTo(out Euc3D.Sphere sphere);
                    this.Value = sphere;

                    return true;
                }
            }
            // Cast a RH_Geo.Brep to a Gh_Sphere
            if (typeof(RH_Geo.Brep).IsAssignableFrom(type))
            {
                RH_Geo.Brep rh_Brep = (RH_Geo.Brep)source;

                if (rh_Brep.IsSurface)
                {
                    RH_Geo.Surface rh_Surface = rh_Brep.Surfaces[0];

                    if (rh_Surface.TryGetSphere(out RH_Geo.Sphere rh_Sphere))
                    {
                        rh_Sphere.CastTo(out Euc3D.Sphere sphere);
                        this.Value = sphere;

                        return true;
                    }
                }
            }


            /******************** Grasshopper Objects ********************/

            // Cast a GH_Types.GH_Surface to a Gh_Sphere
            if (typeof(GH_Types.GH_Surface).IsAssignableFrom(type))
            {
                RH_Geo.Surface rh_Surface = ((GH_Types.GH_Surface)source).Value.Surfaces[0];

                if (rh_Surface.TryGetSphere(out RH_Geo.Sphere rh_Sphere))
                {
                    rh_Sphere.CastTo(out Euc3D.Sphere sphere);
                    this.Value = sphere;

                    return true;
                }
            }
            // Cast a GH_Types.GH_Surface to a Gh_Sphere
            if (typeof(GH_Types.GH_Brep).IsAssignableFrom(type))
            {
                RH_Geo.Brep rh_Brep = ((GH_Types.GH_Brep)source).Value;

                if (rh_Brep.IsSurface)
                {
                    RH_Geo.Surface rh_Surface = rh_Brep.Surfaces[0];

                    if (rh_Surface.TryGetSphere(out RH_Geo.Sphere rh_Sphere))
                    {
                        rh_Sphere.CastTo(out Euc3D.Sphere sphere);
                        this.Value = sphere;

                        return true;
                    }
                } 
            }


            /******************** Otherwise ********************/

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            /******************** BRIDGES Objects ********************/

            // Casts a Gh_Sphere to a Euc3D.Sphere
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Sphere)))
            {
                object sphere = this.Value;
                target = (T)sphere;
                return true;
            }


            /******************** Rhino Objects ********************/

            // Casts a Gh_Sphere to a RH_Geo.Sphere
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Sphere)))
            {
                this.Value.CastTo(out RH_Geo.Sphere rh_Sphere);

                target = (T)(object)rh_Sphere;
                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Casts a Gh_Sphere to a GH_Types.GH_Surface
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Surface)))
            {
                this.Value.CastTo(out RH_Geo.Sphere rh_Sphere);
                RH_Geo.Brep rh_Brep = rh_Sphere.ToBrep();
                
                GH_Types.GH_Surface gh_Surface = new GH_Types.GH_Surface(rh_Brep);
                target = (T)(object)gh_Surface;

                return true;
            }
            // Casts a Gh_Sphere to a GH_Types.GH_Surface
            else if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Brep)))
            {
                this.Value.CastTo(out RH_Geo.Sphere rh_Sphere);
                RH_Geo.Brep rh_Brep = rh_Sphere.ToBrep();

                GH_Types.GH_Brep gh_Brep = new GH_Types.GH_Brep(rh_Brep);
                target = (T)(object)gh_Brep;

                return true;
            }

            /******************** Otherwise ********************/

            return false;
        }

        #endregion
    }
}
