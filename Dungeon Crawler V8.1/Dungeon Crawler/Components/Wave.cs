using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawler.Components
{
    public class Wave
    {
        public int waveNumber;
        public float spawnRate;
        public float healthModifier;

        public int spawnAmountN;    //NORMAL
        public int spawnAmountF;    //FAST
        public int spawnAmountS;    //STRONG
        public int spawnAmountA;    //AIRBOURNE

        public Wave()
        {
        }

    }
}
