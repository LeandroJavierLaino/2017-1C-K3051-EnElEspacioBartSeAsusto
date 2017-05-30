using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class Celda
    {
        private List<TgcMesh> meshesDeLaCelda = new List<TgcMesh>();
        private List<Portal> portales = new List<Portal>();
        private TgcBox celda;

        public void agregarMesh(TgcMesh unMesh)
        {
            meshesDeLaCelda.Add(unMesh);
        }

        public void establecerCelda(Vector3 size, Vector3 position)
        {
            celda = new TgcBox();
            celda.Position = position;
            celda.Size = size;
        }

        public TgcBox obtenerCelda()
        {
            return celda;
        }

        public void agregarPortal(Portal unPortal)
        {
            portales.Add(unPortal);
        }

        public bool estaJugadorEnCelda(Vector3 positionPlayer)
        {
            return celda.BoundingBox.PMax.X >= positionPlayer.X && celda.BoundingBox.PMax.Y >= positionPlayer.Y && celda.BoundingBox.PMax.Z >= positionPlayer.Z && celda.BoundingBox.PMin.X <= positionPlayer.X && celda.BoundingBox.PMin.Y <= positionPlayer.Y && celda.BoundingBox.PMin.Z <= positionPlayer.Z;
        }

        public List<TgcMesh> render(Vector3 positionPlayer,TgcFrustum frustum)
        {

            List<TgcMesh> meshesCandidatos = new List<TgcMesh>();
            List<Portal> portalesCandidatos = new List<Portal>();

            if (estaJugadorEnCelda(positionPlayer))
            {
                foreach (var mesh in meshesDeLaCelda)
                {
                    var r = TgcCollisionUtils.classifyFrustumAABB(frustum, mesh.BoundingBox);
                    if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                    {
                        meshesCandidatos.Add(mesh);
                    } 
                }
                
                //Si el portal cae dentro del Frustum o de la vista del jugador renderizamos aquello que pertenezca a la(s) celda(s) contigua
                
                foreach (var portal in portales)
                {
                    var r = TgcCollisionUtils.classifyFrustumAABB(frustum, portal.getPortal().BoundingBox);
                    if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                    {
                        portalesCandidatos.Add(portal);
                    }
                }

                foreach (var candidato in portalesCandidatos)
                {
                    meshesCandidatos.AddRange( candidato.render(positionPlayer, frustum));
                }

                return meshesCandidatos;
            }
            else
            {
                foreach (var portal in portales)
                {
                    var r = TgcCollisionUtils.classifyFrustumAABB(frustum, portal.getPortal().BoundingBox);
                    if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                    {
                        portalesCandidatos.Add(portal);
                    }
                }

                foreach (var candidato in portalesCandidatos)
                {
                    meshesCandidatos.AddRange(candidato.render(positionPlayer, frustum));
                }

                return meshesCandidatos;
            }
        }

        public List<TgcMesh> render(TgcFrustum frustum)
        {
            List<TgcMesh> meshesCandidatos = new List<TgcMesh>();

            foreach (var mesh in meshesDeLaCelda)
            {
                var r = TgcCollisionUtils.classifyFrustumAABB(frustum, mesh.BoundingBox);
                if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                {
                    meshesCandidatos.Add(mesh);
                }
            }

            return meshesCandidatos;            
        }
    }
}
