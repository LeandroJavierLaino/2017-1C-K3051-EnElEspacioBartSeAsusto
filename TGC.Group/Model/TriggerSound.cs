using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Sound;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;

namespace TGC.Group.Model
{
    class TriggerSound
    {
        private TgcBox area;
        private TgcStaticSound sound;

        public TriggerSound(Vector3 aPosition,string path,Device device)
        {
            area = new TgcBox();
            area.Size = new Vector3(60,60,60);
            area.Position = aPosition;
            sound = new TgcStaticSound();
            sound.loadSound(path, device);
        }

        public void throwSound(Vector3 position)
        {
            if ( isInArea(position) )
            {
                sound.play(false);
            }
        }

        public bool isInArea(Vector3 position)
        {
            return position.X <= area.BoundingBox.PMax.X && position.X >= area.BoundingBox.PMin.X &&
                   position.Y <= area.BoundingBox.PMax.Y && position.Y >= area.BoundingBox.PMin.Y &&
                   position.Z <= area.BoundingBox.PMax.Z && position.Z >= area.BoundingBox.PMin.Z;
        }

        public void dispose()
        {
            sound.dispose();
            area.dispose();
        }
    }
}
