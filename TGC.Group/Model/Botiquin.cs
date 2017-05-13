using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    class Botiquin
    {
        public TgcMesh meshBotiquin { get; set; }
        public Vector3 Position;

        public void setMesh(TgcMesh newMesh)
        {
            meshBotiquin = newMesh;
        }

        public void changePosicion(Vector3 newPosition)
        {
            meshBotiquin.Position = newPosition;
        }

        public void consumir(Vector3 posicion)
        {
            if (distance(posicion, meshBotiquin.Position) < 80 )
            {

                changePosicion(new Vector3(0, 0, 0));
            }       
               
        }

        public float distance(Vector3 a, Vector3 b)
        {
            return (FastMath.Sqrt(FastMath.Pow2(a.X - b.X) + FastMath.Pow2(a.Y - b.Y) + FastMath.Pow2(a.Z - b.Z)));
        }
    }
}
