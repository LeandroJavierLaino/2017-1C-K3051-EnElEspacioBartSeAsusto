using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;

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
    }
}
