using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class HideCell
    {
        private TgcBox cell;

        /// <summary>
        ///     establece una celda dado un tamaño y una posicion.
        ///     El primer parametro es un vector de 2 dimensiones que representa el ancho y largo del area dado que la altura es siempre la misma (100 unidades),
        ///     el segundo parametro es un vector de 3 dimensiones que representa la posicion de la celda.
        /// </summary>
        public void setCell(Vector2 size, Vector3 position)
        {
            cell = new TgcBox();
            cell.Position = position;
            cell.Size = new Vector3(size.X,100,size.Y);
        }

        public TgcBox getCell()
        {
            return cell;
        }

        /// <summary>
        /// Verifica si el jugador se encuentra dentro de la celda para ocultarse del enemigo
        /// </summary>
        /// <param name="positionPlayer"></param>
        /// <returns>Devuelve un bool que indica si esta dentro o fuera del la celda</returns>
        public bool isPlayerInCell(Vector3 positionPlayer)
        {
            return cell.BoundingBox.PMax.X >= positionPlayer.X && cell.BoundingBox.PMax.Y >= positionPlayer.Y && cell.BoundingBox.PMax.Z >= positionPlayer.Z && cell.BoundingBox.PMin.X <= positionPlayer.X && cell.BoundingBox.PMin.Y <= positionPlayer.Y && cell.BoundingBox.PMin.Z <= positionPlayer.Z;
        }

        /// <summary>
        /// Renderiza el bounding box de la celda a fines de debug del entorno
        /// </summary>
        public void render()
        {
            cell.BoundingBox.render();
        }
    }
}
