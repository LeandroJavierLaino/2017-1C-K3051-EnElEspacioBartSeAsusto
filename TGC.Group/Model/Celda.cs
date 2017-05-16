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
            return celda.BoundingBox.PMax.X >= positionPlayer.X && celda.BoundingBox.PMax.Y >= positionPlayer.Y && celda.BoundingBox.PMax.Z >= positionPlayer.Z && celda.BoundingBox.PMin.X <= positionPlayer.X && celda.BoundingBox.PMin.Y <= positionPlayer.Y && celda.BoundingBox.PMin.Z <= positionPlayer.Z;
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

        public void render()
        {
            celda.BoundingBox.computeFaces();
            celda.BoundingBox.render();
            foreach(var mesh in meshesDeLaCelda)
            {
                mesh.render();
            }
        }
    }
}
