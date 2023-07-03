using System;

using Euc3D = BRIDGES.Geometry.Euclidean3D;
using Fv = BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh;
using He = BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh;

using RH_Geo = Rhino.Geometry;

using BRIDGES.McNeel.Rhino.Extensions.Geometry.Euclidean3D;

using GH_Kernel = Grasshopper.Kernel;
using GH_Types = Grasshopper.Kernel.Types;

using BRIDGES.McNeel.Grasshopper.Display.Geometry.Euclidean3D;


namespace BRIDGES.McNeel.Grasshopper.Types.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a grasshopper type for an <see cref="Fv.Mesh{TPosition}"/>.
    /// </summary>
    public class Gh_FvMesh : GH_Types.GH_Goo<Fv.Mesh<Euc3D.Point>>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                double minX = 0d, maxX = 0d, minY = 0d, maxY = 0d, minZ = 0d, maxZ = 0d;
                foreach (Fv.Vertex<Euc3D.Point> vertices in Value.GetVertices())
                {
                    if (vertices.Position.X < minX) { minX = vertices.Position.X; }
                    else if (maxX < vertices.Position.X) { minX = vertices.Position.X; }

                    if (vertices.Position.Y < minY) { minY = vertices.Position.Y; }
                    else if (maxY < vertices.Position.Y) { maxY = vertices.Position.Y; }

                    if (vertices.Position.Z < minZ) { minZ = vertices.Position.Z; }
                    else if (maxZ < vertices.Position.Z) { maxZ = vertices.Position.Z; }
                }

                return new RH_Geo.BoundingBox(minX, maxX, minY, maxY, minZ, maxZ);
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
        /// Initialises a new instance of <see cref= "Gh_FvMesh" /> class.
        /// </summary>
        public Gh_FvMesh() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_FvMesh" /> class from another <see cref="Gh_FvMesh"/>..
        /// </summary>
        /// <param name="gh_FvMesh"> <see cref="Gh_FvMesh"/> to duplicate. </param>
        public Gh_FvMesh(Gh_FvMesh gh_FvMesh)
        {
            this.Value = (Fv.Mesh<Euc3D.Point>)gh_FvMesh.Value.Clone();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_FvMesh"/> class from a <see cref="Fv.Mesh{TPosition}"/>.
        /// </summary>
        /// <param name="fvMesh"> <see cref="Fv.Mesh{TPosition}"/> to duplicate.</param>
        public Gh_FvMesh(Fv.Mesh<Euc3D.Point> fvMesh)
        {
            this.Value = (Fv.Mesh<Euc3D.Point>)fvMesh.Clone();
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
            Draw.Wireframe.Mesh(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        /********** Properties **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Fv.Mesh<Euc3D.Point>)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_FvMesh); } }


        /********** Methods **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return $"FvMesh (V:{Value.VertexCount}, E:{Value.EdgeCount}, F:{Value.FaceCount})";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_FvMesh(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();

            /******************** BRIDGES Objects ********************/

            // Cast a Fv.Mesh<Euc3D.Point> to a Gh_FvMesh
            if (typeof(Fv.Mesh<Euc3D.Point>).IsAssignableFrom(type))
            {
                Value = (Fv.Mesh<Euc3D.Point>)source;
                return true;
            }
            // Cast a He.Mesh<Euc3D.Point> to a Gh_FvMesh
            if (typeof(He.Mesh<Euc3D.Point>).IsAssignableFrom(type))
            {
                He.Mesh<Euc3D.Point> heMesh = (He.Mesh<Euc3D.Point>)source;
                Value = heMesh.ToFaceVertexMesh();
                return true;
            }


            /******************** Rhino Objects ********************/

            // Cast a RH_Geo.Mesh to a Gh_FvMesh
            if (typeof(RH_Geo.Mesh).IsAssignableFrom(type))
            {
                RH_Geo.Mesh rh_Mesh = (RH_Geo.Mesh)source;

                rh_Mesh.CastTo(out Fv.Mesh<Euc3D.Point> mesh);
                Value = mesh;

                return true;
            }


            /******************** BRIDGES.McNeel.Grasshopper Objects ********************/

            // Cast a Gh_HeMesh to a Gh_FvMesh
            if (typeof(Gh_HeMesh).IsAssignableFrom(type))
            {
                He.Mesh<Euc3D.Point> heMesh = ((Gh_HeMesh)source).Value;
                Value = heMesh.ToFaceVertexMesh();

                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Cast a GH_Types.GH_Mesh to a Gh_FvMesh
            if (typeof(GH_Types.GH_Mesh).IsAssignableFrom(type))
            {
                RH_Geo.Mesh rh_Mesh = ((GH_Types.GH_Mesh)source).Value;

                rh_Mesh.CastTo(out Fv.Mesh<Euc3D.Point> mesh);
                Value = mesh;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            /******************** BRIDGES Objects ********************/

            // Casts a Gh_FvMesh to a Fv.Mesh<Euc3D.Point>
            if (typeof(T).IsAssignableFrom(typeof(Fv.Mesh<Euc3D.Point>)))
            {
                object mesh = this.Value;
                target = (T)mesh;
                return true;
            }
            // Casts a Gh_FvMesh to a He.Mesh<Euc3D.Point>
            if (typeof(T).IsAssignableFrom(typeof(He.Mesh<Euc3D.Point>)))
            {
                Fv.Mesh<Euc3D.Point> fvMesh = this.Value;
                He.Mesh<Euc3D.Point> heMesh = fvMesh.ToHalfedgeMesh();
                target = (T)(object)heMesh;
                return true;
            }

            /******************** Rhino Objects ********************/

            // Casts a Gh_FvMesh to a RH_Geo.Mesh
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Plane)))
            {
                this.Value.CastTo(out RH_Geo.Mesh rh_Mesh);

                target = (T)(object)rh_Mesh;
                return true;
            }


            /******************** BRIDGES.McNeel.Grasshopper Objects ********************/

            // Casts a Gh_FvMesh to a Gh_HeMesh
            if (typeof(T).IsAssignableFrom(typeof(Gh_HeMesh)))
            {
                Fv.Mesh<Euc3D.Point> fvMesh = this.Value;
                He.Mesh<Euc3D.Point> heMesh = fvMesh.ToHalfedgeMesh();

                Gh_HeMesh gh_HeMesh = new Gh_HeMesh(heMesh);
                target = (T)(object)gh_HeMesh;

                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Casts a Gh_FvMesh to a GH_Types.GH_Mesh
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Mesh)))
            {
                this.Value.CastTo(out RH_Geo.Mesh rh_Mesh);

                GH_Types.GH_Mesh gh_Mesh = new GH_Types.GH_Mesh(rh_Mesh);
                target = (T)(object)gh_Mesh;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        #endregion
    }
}