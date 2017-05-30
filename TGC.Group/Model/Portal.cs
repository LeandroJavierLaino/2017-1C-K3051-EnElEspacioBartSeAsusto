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
    class Portal
    {
        private Celda celdaA = new Celda();
        private Celda celdaB = new Celda();
        private TgcBox portal;

        public void establecerPortal(Vector3 size, Vector3 position,Celda cellA, Celda cellB)
        {
            portal = new TgcBox();
            portal.Position = position;
            portal.Size = size;
            celdaA = cellA;
            celdaB = cellB;
        }

        public TgcBoundingAxisAlignBox getBoundingBox()
        {
            return portal.BoundingBox;
        }

        public TgcBox getPortal()
        {
            return portal;
        }

        public List<TgcMesh> render(Vector3 positionPlayer, TgcFrustum frustum)
        {
            List<TgcMesh> meshesCandidatos = new List<TgcMesh>();
            //Renderizo ambas celdas si estoy justo en el portal
            if (estaEnPortal(positionPlayer))
            {
                meshesCandidatos.AddRange( celdaA.render(frustum));
                meshesCandidatos.AddRange( celdaB.render(frustum));
            }
            //si el jugador esta en la celdaA renderizo la celdaB
            if (celdaA.estaJugadorEnCelda(positionPlayer))
            {
                meshesCandidatos.AddRange(celdaB.render(frustum));
            }
            //si el jugador esta en la celdaB renderizo la celdaA
            if (celdaB.estaJugadorEnCelda(positionPlayer))
            {
                meshesCandidatos.AddRange(celdaA.render(frustum));
            }
            var r = TgcCollisionUtils.classifyFrustumAABB(frustum, portal.BoundingBox);
            if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
            {
                if (TgcCollisionUtils.classifyFrustumAABB(frustum, celdaA.obtenerCelda().BoundingBox) != TgcCollisionUtils.FrustumResult.OUTSIDE )
                {
                    meshesCandidatos.AddRange(celdaA.render(frustum));
                }
                if (TgcCollisionUtils.classifyFrustumAABB(frustum, celdaB.obtenerCelda().BoundingBox) != TgcCollisionUtils.FrustumResult.OUTSIDE)
                {
                    meshesCandidatos.AddRange(celdaB.render(frustum));
                }
            }
            return meshesCandidatos;

        }

        public bool estaEnPortal(Vector3 positionPlayer)
        {
            return portal.BoundingBox.PMax.X >= positionPlayer.X && portal.BoundingBox.PMax.Y >= positionPlayer.Y && portal.BoundingBox.PMax.Z >= positionPlayer.Z && portal.BoundingBox.PMin.X <= positionPlayer.X && portal.BoundingBox.PMin.Y <= positionPlayer.Y && portal.BoundingBox.PMin.Z <= positionPlayer.Z;
        }
    }
}
