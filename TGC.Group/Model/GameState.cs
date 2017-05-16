using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    class GameState
    {
        /*
         * Los seteamos como queremos
         * Esto permite tener varios estados que podemos setear
         * capturando los atributos privados del Juego
         */
        public Action Update { get; set; }
        public Action Render { get; set; }
    }
}
