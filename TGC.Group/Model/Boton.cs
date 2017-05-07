using Microsoft.DirectX;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class Boton
    {
        public TgcMesh meshBoton { get; set; }
        public bool isGreen = false;

        public void changeColor(Color newColor)
        {
            meshBoton.setColor(newColor);
        }

        public void changePosicion(Vector3 newPosition)
        {
            meshBoton.Position = newPosition;
        }

        public void setMesh(TgcMesh newMesh)
        {
            meshBoton = newMesh;
        }
    }
}
