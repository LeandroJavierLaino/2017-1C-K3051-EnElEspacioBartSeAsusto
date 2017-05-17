using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;

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

        public void render(Vector3 positionPlayer)
        {
            //Renderizo ambas celdas si estoy justo en el portal
            if (estaEnPortal(positionPlayer))
            {
                celdaA.render();
                celdaB.render();
            }
            //si el jugador esta en la celdaA renderizo la celdaB
            if (celdaA.estaJugadorEnCelda(positionPlayer))
            {
                celdaB.render();
            }
            //si el jugador esta en la celdaB renderizo la celdaA
            if (celdaB.estaJugadorEnCelda(positionPlayer))
            {
                celdaA.render();
            }
        }

        public bool estaEnPortal(Vector3 positionPlayer)
        {
            return portal.BoundingBox.PMax.X >= positionPlayer.X && portal.BoundingBox.PMax.Y >= positionPlayer.Y && portal.BoundingBox.PMax.Z >= positionPlayer.Z && portal.BoundingBox.PMin.X <= positionPlayer.X && portal.BoundingBox.PMin.Y <= positionPlayer.Y && portal.BoundingBox.PMin.Z <= positionPlayer.Z;
        }
    }
}
