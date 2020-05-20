using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace thunder146.MySpaceShooter
{
    internal class GameState
    {
        public bool start { get; set; }
        public bool lost { get; set; }
        public bool IsFinalBossDeath { get; set; }
        public int Score { get; set; }
        public LevelSelection CurrentLevel { get; set; }
        public int Bg1_posY { get; set; }
        public int Bg2_posY { get; set; }
        public int Bg3_posY { get; set; }
        public int FinalBoss_PosX { get; set; }
        public int FinalBoss_PosY { get; set; }
        public bool DrawCollisionAnimation { get; set; }
        public float CollisionTimer { get; set; }
        public float ElapsedGameTime { get; set; }
        public string LvlName { get; set; }
        public int PlayerLives { get; set; }
        public int Meteor_move_speed {get;set;}
        public int Meteorit2_speed_Y { get; set; }
        public int Meteorit2_speed_X { get; set; }
        public int LaserMoveSpeed {get;set;}

        public GameState()
        {
            this.CurrentLevel = LevelSelection.Level1;
            this.Bg1_posY = 0;
            this.Bg2_posY = -800;
            this.Bg3_posY = -1600;
            this.FinalBoss_PosX = 200;
            this.FinalBoss_PosY = 50;
            this.LvlName = "1";
            this.PlayerLives = 3;
            this.Meteor_move_speed = 200;
            this.Meteorit2_speed_Y = 200;
            this.Meteorit2_speed_X = 200;
            this.LaserMoveSpeed = -400;
        }

        internal void Reset()
        {
            PlayerLives = 3;
            ElapsedGameTime = 0;
            Score = 0;
            LvlName = "1";
            CurrentLevel = LevelSelection.Level1;
        }
    }
}
