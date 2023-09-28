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
    /// Class defining a grasshopper type for an <see cref="Euc3D.Vector"/>.
    /// </summary>
    public class Gh_Vector : GH_Types.GH_Goo<Euc3D.Vector>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                this.Value.CastTo(out RH_Geo.Vector3d vector);
                return new RH_Geo.BoundingBox(new RH_Geo.Point3d(0.0, 0.0, 0.0), (RH_Geo.Point3d) vector);
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
        /// Initialises a new instance of <see cref= "Gh_Vector" /> class.
        /// </summary>
        public Gh_Vector() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Vector" /> class from another <see cref="Gh_Vector"/>..
        /// </summary>
        /// <param name="gh_Vector"> <see cref="Gh_Vector"/> to duplicate. </param>
        public Gh_Vector(Gh_Vector gh_Vector)
        {
            this.Value = gh_Vector.Value;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Vector"/> class from a <see cref="Euc3D.Vector"/>.
        /// </summary>
        /// <param name="vector"> <see cref="Euc3D.Vector"/> for the grasshopper type. </param>
        public Gh_Vector(Euc3D.Vector vector)
        {
            this.Value = vector;
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
            Draw.Wireframe.Vector(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        // ---------- Properties ---------- //

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Vector)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Vector); } }


        // ---------- Methods ---------- //

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return $"({Value.X}, {Value.Y}, {Value.Z})";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Vector(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();


            // ----- BRIDGES Objects ----- //

            // Cast a Euc3D.Vector to a Gh_Vector
            if (typeof(Euc3D.Vector).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Vector)source;

                return true;
            }
            // Cast a Euc3D.Point to a Gh_Vector
            if (typeof(Euc3D.Point).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Point)source;

                return true;
            }


            // ----- Rhino Objects ----- //

            // Casts a RH_Geo.Vector3d to a Gh_Vector
            if (typeof(RH_Geo.Vector3d).IsAssignableFrom(type))
            {
                ((RH_Geo.Vector3d)source).CastTo(out Euc3D.Vector vector);
                this.Value = vector;

                return true;
            }
            // Casts a RH_Geo.Point3d to a Gh_Vector
            if (typeof(RH_Geo.Point3d).IsAssignableFrom(type))
            {
                ((RH_Geo.Point3d)source).CastTo(out Euc3D.Point point);
                this.Value = (Euc3D.Vector)point;

                return true;
            }


            // ----- BRIDGES.McNeel.Grasshopper Objects ----- //

            // Casts a Gh_Vector to a Gh_Vector
            if (typeof(Gh_Vector).IsAssignableFrom(type))
            {
                Euc3D.Vector point = ((Gh_Vector)source).Value;

                this.Value = point;

                return true;
            }
            // Casts a Gh_Point to a Gh_Vector
            if (typeof(Gh_Point).IsAssignableFrom(type))
            {
                Gh_Point gh_Point = (Gh_Point)source;

                this.Value = (Euc3D.Vector)gh_Point.Value;

                return true;
            }


            // ----- Grasshopper Objects ----- //

            // Casts a GH_Types.GH_Vector to a Gh_Vector
            if (typeof(GH_Types.GH_Vector).IsAssignableFrom(type))
            {
                GH_Types.GH_Vector gh_Vector = (GH_Types.GH_Vector)source;

                gh_Vector.Value.CastTo(out Euc3D.Vector vector);
                this.Value = vector;

                return true;
            }
            // Casts a GH_Types.GH_Point to a Gh_Vector
            if (typeof(GH_Types.GH_Point).IsAssignableFrom(type))
            {
                GH_Types.GH_Point gh_Point = (GH_Types.GH_Point)source;

                gh_Point.Value.CastTo(out Euc3D.Point point);
                this.Value = (Euc3D.Vector)point;

                return true;
            }


            // ----- Otherwise ----- //

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            // ----- BRIDGES Objects ----- //

            // Casts a Gh_Vector to a Euc3D.Vector
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Vector)))
            {
                target = (T)(object)this.Value;

                return true;
            }
            // Casts a Gh_Vector to a Euc3D.Point
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Point)))
            {
                object point = (Euc3D.Point)this.Value;
                target = (T)point;

                return true;
            }


            // ----- Rhino Objects ----- //

            // Casts a Gh_Vector to a RH_Geo.Vector3d
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Vector3d)))
            {
                this.Value.CastTo(out RH_Geo.Vector3d rh_Point);

                target = (T)(object)rh_Point;

                return true;
            }
            // Casts a Gh_Vector to a RH_Geo.Point3d
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Point3d)))
            {
                this.Value.CastTo(out RH_Geo.Vector3d rh_Vector);

                RH_Geo.Point3d rh_Point = (RH_Geo.Point3d)rh_Vector;
                target = (T)(object)rh_Point;

                return true;
            }


            // ----- BRIDGES.McNeel.Grasshopper Objects ----- //

            // Casts a Gh_Vector to a Gh_Vector
            if (typeof(T).IsAssignableFrom(typeof(Gh_Vector)))
            {
                target = (T)(object)this;

                return true;
            }
            // Casts a Gh_Vector to a Gh_Point
            if (typeof(T).IsAssignableFrom(typeof(Gh_Point)))
            {
                Gh_Point gh_Point = new Gh_Point(this.Value);
                target = (T)(object)gh_Point;

                return true;
            }


            // ----- Grasshopper Objects ----- //

            // Casts a Gh_Vector to a GH_Types.GH_Vector
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Vector)))
            {
                this.Value.CastTo(out RH_Geo.Vector3d rh_Vector);

                GH_Types.GH_Vector gh_Vector = new GH_Types.GH_Vector(rh_Vector);
                target = (T)(object)gh_Vector;

                return true;
            }
            // Casts a Gh_Vector to a GH_Types.GH_Point
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Point)))
            {
                this.Value.CastTo(out RH_Geo.Vector3d rh_Vector);

                GH_Types.GH_Point gh_Point = new GH_Types.GH_Point((RH_Geo.Point3d)rh_Vector);
                target = (T)(object)gh_Point;

                return true;
            }


            // ----- Otherwise ----- //

            return false;
        }

        #endregion
    }
}
