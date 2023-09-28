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
    /// Class defining a grasshopper type for an <see cref="Euc3D.Point"/>.
    /// </summary>
    public class Gh_Point : GH_Types.GH_Goo<Euc3D.Point>, GH_Kernel.IGH_PreviewData
    {
        #region Properties
        
        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                this.Value.CastTo(out RH_Geo.Point3d point);
                return new RH_Geo.BoundingBox(point, point);
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
        /// Initialises a new instance of <see cref= "Gh_Point" /> class.
        /// </summary>
        public Gh_Point() { /* Do Nothing */ }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Point" /> class from another <see cref="Gh_Point"/>..
        /// </summary>
        /// <param name="gh_Point"> <see cref="Gh_Point"/> to duplicate. </param>
        public Gh_Point(Gh_Point gh_Point)
        {
            this.Value = new Euc3D.Point(gh_Point.Value);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Point"/> class from a <see cref="Euc3D.Point"/>.
        /// </summary>
        /// <param name="point"> <see cref="Euc3D.Point"/> for the grasshopper type.</param>
        public Gh_Point(Euc3D.Point point)
        {
            this.Value = new Euc3D.Point(point);
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
            Draw.Wireframe.Point(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        // ---------- Properties ---------- //

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Point)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Point); } }


        // ---------- Methods ---------- //

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return $"({Value.X}, {Value.Y}, {Value.Z})";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Point(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();


            // ----- BRIDGES Objects ----- //

            // Cast a Euc3D.Point to a Gh_Point
            if (typeof(Euc3D.Point).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Point)source;

                return true;
            }
            // Cast a Euc3D.Vector to a Gh_Point
            if (typeof(Euc3D.Vector).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Vector)source;

                return true;
            }


            // ----- Rhino Objects ----- //

            // Casts a RH_Geo.Point3d to a Gh_Point
            if (typeof(RH_Geo.Point3d).IsAssignableFrom(type))
            {
                ((RH_Geo.Point3d)source).CastTo(out Euc3D.Point point);
                this.Value = point;

                return true;
            }
            // Casts a RH_Geo.Vector3d to a Gh_Point
            if (typeof(RH_Geo.Vector3d).IsAssignableFrom(type))
            {
                ((RH_Geo.Vector3d)source).CastTo(out Euc3D.Vector vector);
                this.Value = (Euc3D.Point)vector;

                return true;
            }


            // ----- BRIDGES.McNeel.Grasshopper Objects ----- //

            // Casts a Gh_Point to a Gh_Point
            if (typeof(Gh_Point).IsAssignableFrom(type))
            {
                Euc3D.Point vector = ((Gh_Point)source).Value;

                this.Value = vector;

                return true;
            }
            // Casts a Gh_Vector to a Gh_Point
            if (typeof(Gh_Vector).IsAssignableFrom(type))
            {
                Gh_Vector gh_Vector = (Gh_Vector)source;

                this.Value = (Euc3D.Point)gh_Vector.Value;

                return true;
            }

            // ----- Grasshopper Objects ----- //

            // Casts a GH_Types.GH_Point to a Gh_Point
            if (typeof(GH_Types.GH_Point).IsAssignableFrom(type))
            {
                GH_Types.GH_Point gh_Point = (GH_Types.GH_Point)source;

                gh_Point.Value.CastTo(out Euc3D.Point point);
                this.Value = point;

                return true;
            }
            // Casts a GH_Types.GH_Vector to a Gh_Point
            if (typeof(GH_Types.GH_Vector).IsAssignableFrom(type))
            {
                GH_Types.GH_Vector gh_Vector = ((GH_Types.GH_Vector)source);

                gh_Vector.Value.CastTo(out Euc3D.Vector vector);
                this.Value = (Euc3D.Point)vector;

                return true;
            }


            // ----- Otherwise ----- //

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            // ----- BRIDGES Objects ----- //

            // Casts a Gh_Point to a Euc3D.Point
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Point)))
            {
                target = (T)(object)this.Value;

                return true;
            }
            // Casts a Gh_Point to a Euc3D.Vector
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Vector)))
            {
                object vector = (Euc3D.Vector)this.Value;
                target = (T)vector;

                return true;
            }


            // ----- Rhino Objects ----- //

            // Casts a Gh_Point to a RH_Geo.Point3d
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Point3d)))
            {
                this.Value.CastTo(out RH_Geo.Point3d rh_Point);

                target = (T)(object)rh_Point;
                return true;
            }
            // Casts a Gh_Point to a RH_Geo.Vector3d
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Vector3d)))
            {
                this.Value.CastTo(out RH_Geo.Point3d rh_Point);

                RH_Geo.Vector3d rh_Vector = (RH_Geo.Vector3d)rh_Point;
                target = (T)(object)rh_Vector;

                return true;
            }


            // ----- BRIDGES.McNeel.Grasshopper Objects ----- //

            // Casts a Gh_Point to a Gh_Point
            if (typeof(T).IsAssignableFrom(typeof(Gh_Point)))
            {
                target = (T)(object)this;

                return true;
            }
            // Casts a Gh_Point to a Gh_Vector
            if (typeof(T).IsAssignableFrom(typeof(Gh_Vector)))
            {
                Gh_Vector gh_Vector = new Gh_Vector(this.Value);
                target = (T)(object)gh_Vector;

                return true;
            }


            // ----- Grasshopper Objects ----- //

            // Casts a Gh_Point to a GH_Types.GH_Point
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Point)))
            {
                this.Value.CastTo(out RH_Geo.Point3d rh_Point);

                GH_Types.GH_Point gh_Point = new GH_Types.GH_Point(rh_Point);
                target = (T)(object)gh_Point;

                return true;
            }
            // Casts a Gh_Point to a GH_Types.GH_Vector
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Vector)))
            {
                this.Value.CastTo(out RH_Geo.Point3d rh_Point);

                GH_Types.GH_Vector gh_Vector = new GH_Types.GH_Vector((RH_Geo.Vector3d)rh_Point);
                target = (T)(object)gh_Vector;

                return true;
            }


            // ----- Otherwise ----- //

            return false;
        }

        #endregion
    }
}
