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

        public void establecerPortal(Vector3 size, Vector3 position)
        {
            portal = new TgcBox();
            portal.Position = position;
            portal.Size = size;
        }

        public void render(Vector3 positionPlayer)
        {
            celdaA.render(positionPlayer);
            celdaB.render(positionPlayer);
        }
    }
}
