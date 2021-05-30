using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleWalk.Classes
{
    // Bude vždy o kousek napřed před želvičkou, a bude na ni pálit malé kapky lávy (LavaDrop)
    // Pokud vyblokujeme plošinkou, odrazí se to pod stejným úhlem
    // Nahoře bude healthbar se životy FlyingEnemy
    // Level budeme moci dokončit až poté, co FlyingEnemy zahyne (do té doby: zátarasa)

    sealed class FlyingEnemy : Enemy
    {
        public bool WasHit;

        public FlyingEnemy()
        {
            
        }
    }
}
