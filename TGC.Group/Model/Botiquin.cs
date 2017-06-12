using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Sound;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    class Botiquin
    {
        public TgcMesh meshBotiquin { get; set; }
        public Vector3 Position;
        private TgcStaticSound soundBotiquin;

        public void setMesh(TgcMesh newMesh)
        {
            meshBotiquin = newMesh;
        }

        public void setSoundBotiquin(string path, Microsoft.DirectX.DirectSound.Device device)
        {
            soundBotiquin = new TgcStaticSound();
            soundBotiquin.loadSound(path, device);
        }

        public void changePosicion(Vector3 newPosition)
        {
            Position = newPosition;
            meshBotiquin.Position = newPosition;
        }

        public void consumir(Vector3 posicion)
        {
            soundBotiquin.play(false);
            changePosicion(new Vector3(0, 0, 0)); 
        }

        public float distance(Vector3 a, Vector3 b)
        {
            return (FastMath.Sqrt(FastMath.Pow2(a.X - b.X) + FastMath.Pow2(a.Y - b.Y) + FastMath.Pow2(a.Z - b.Z)));
        }
    }
}
