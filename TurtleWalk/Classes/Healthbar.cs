using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TurtleWalk.Enemies;

namespace TurtleWalk.ClassManager
{
    // budeme předávat instanci Enemy (jinak bychom museli podle mě udělat několik konstruktorů pro každý typ zvlášť
    // (FlyingEnemy enemy, WalkingEnemy enemy, apod.)

    sealed class Healthbar
    {
        private ProgressBar _healthBar;

        private Enemy _enemy;

        public Healthbar(Enemy enemy, Grid gridLvl)   // Enemy enemy = new FlyingEnemy();
        {
            // TO-DO: Vykreslení HealthBaru a přidání na grid

            _enemy = enemy;

            _healthBar = new ProgressBar()
            {
                Minimum = 0,
                Maximum = 100,
                Value = 100,
            };
        }

        public int GetEnemyHp()
        {
            return _enemy.Hp;
        }
    }
}
