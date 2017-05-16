using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void agregarPortal(Portal unPortal)
        {
            portales.Add(unPortal);
        }

        public bool estaJugadorEnCelda(Vector3 positionPlayer)
        {
            return celda.Size.Y > positionPlayer.Y && (positionPlayer.X + celda.Position.X < celda.Size.X || positionPlayer.X - celda.Position.X > celda.Size.X) && (positionPlayer.Z + celda.Position.Z < celda.Size.Z || positionPlayer.Z - celda.Position.Z > celda.Size.Z);
        }

        public void render(Vector3 positionPlayer)
        {
            if (estaJugadorEnCelda(positionPlayer))
            {
                foreach (var mesh in meshesDeLaCelda)
                {
                    mesh.render();
                }

                foreach (var portal in portales)
                {
                    portal.render(positionPlayer);
                }
            }            
        }
    }
}
