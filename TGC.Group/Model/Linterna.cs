using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    class Linterna
    {
        private float energia;
        private bool select;

        public void setEnergia(int nuevaEnergia)
        {
            this.energia = nuevaEnergia;
        }

        public float getEnergia()
        {
            return this.energia;
        }

        public void setSelect (bool valor)
        {
            this.select = valor;
        }

        public bool getSelect()
        {
            return this.select;
        }

        public void perderEnergia(float valor)
        {
            this.energia -= valor;
        }

        public void ganarEnergia (int valor)
        {
            this.energia += valor;
        }

        public void ganarEnergia(float valor)
        {
            this.energia += valor;
        }
    }
}
