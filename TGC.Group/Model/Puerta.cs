﻿using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    class Puerta
    {
        private TgcMesh modeloPuerta { get; set; }
        private bool puertaAbierta = false;

        public void changePosition(Vector3 newPosition)
        {
            modeloPuerta.Position = newPosition;
            modeloPuerta.UpdateMeshTransform();
        }

        public void setMesh(TgcMesh newMesh)
        {
            modeloPuerta = newMesh;
        }

        public TgcMesh getMesh()
        {
            return modeloPuerta;
        }

        public Vector3 getPosition()
        {
            return modeloPuerta.Position;
        }

        internal void abrirPuerta(Vector3 position)
        {
            if ( (distance(position, modeloPuerta.Position)) < 55 && !puertaAbierta)
            {
                changePosition(new Vector3(modeloPuerta.Position.X, 210f + modeloPuerta.Position.Y, modeloPuerta.Position.Z));
                puertaAbierta = !puertaAbierta;
            }
            else
            {
                if ((distance(position, modeloPuerta.Position)) < 255 && puertaAbierta)
                {
                    changePosition(new Vector3(modeloPuerta.Position.X, modeloPuerta.Position.Y - 210f, modeloPuerta.Position.Z));
                    puertaAbierta = !puertaAbierta;
                }
            }
        }

        public float distance(Vector3 a, Vector3 b)
        {
            return (FastMath.Sqrt(FastMath.Pow2(a.X - b.X) + FastMath.Pow2(a.Y - b.Y) + FastMath.Pow2(a.Z - b.Z)));
        }
    }
}
