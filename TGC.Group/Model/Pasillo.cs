using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;
using System;

namespace TGC.Group.Model
{
    class Pasillo :IRenderObject, ITransformObject
    {
        //private TgcPlane ParedDerecha { get; set; }
        //private TgcPlane ParedIzquerda { get; set; }
        //private TgcPlane Techo { get; set; }
        public TgcTexture TexturaPiso { get; private set; }
        public bool AlphaBlendEnable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Matrix Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoTransformEnable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector3 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector3 Rotation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Vector3 Scale { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TgcPlane Piso { get; set; }
        public bool Enabled { get; private set; }

        public Pasillo(TgcTexture texturaPiso)
        {
            Enabled = true;
            Piso = new TgcPlane(new Vector3( 0f, 0f, 0f), new Vector3(0f,100f,100f), TgcPlane.Orientations.XZplane, texturaPiso, 1f ,1f );
        }

        public void setTexturaPiso(TgcTexture texture)
        {
            if (TexturaPiso != null)
            {
                TexturaPiso.dispose();
            }
            TexturaPiso = texture;
        }

        public void render()
        {
            Piso.render();
        }

        public void dispose()
        {
            Piso.dispose();
        }

        public void move(Vector3 v)
        {
            throw new NotImplementedException();
        }

        public void move(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public void moveOrientedY(float movement)
        {
            throw new NotImplementedException();
        }

        public void getPosition(Vector3 pos)
        {
            throw new NotImplementedException();
        }

        public void rotateX(float angle)
        {
            throw new NotImplementedException();
        }

        public void rotateY(float angle)
        {
            throw new NotImplementedException();
        }

        public void rotateZ(float angle)
        {
            throw new NotImplementedException();
        }
    }
}
