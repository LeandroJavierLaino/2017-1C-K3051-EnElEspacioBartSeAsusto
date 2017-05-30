using Microsoft.DirectX;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using Microsoft.DirectX.Direct3D;
using TGC.Core.Shaders;

namespace TGC.Group.Model
{
    class Boton
    {
        public TgcMesh meshBoton { get; set; }
        public bool isGreen = false;
        private Color color;

        internal void changeColor(Color newColor)
        {
            meshBoton.setColor(newColor);
            color = newColor;
        }

        public Color getColor()
        {
            return color;
        }

        public void changePosicion(Vector3 newPosition)
        {
            meshBoton.Position = newPosition;
        }

        public void setMesh(TgcMesh newMesh)
        {
            meshBoton = newMesh;
        }

        internal void applyEffect(Effect shaderBoton)
        {
            meshBoton.Effect = shaderBoton;
            meshBoton.Technique = TgcShaders.Instance.getTgcMeshTechnique(meshBoton.RenderType);
        }
    }
}
