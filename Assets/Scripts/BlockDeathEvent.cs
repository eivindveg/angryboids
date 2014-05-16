using System;

using UnityEngine;

namespace Assets.Scripts
{
    public class BlockDeathEvent : EventArgs
    {
        private readonly int value;

        private readonly BrickCollision.BrickType type;

        // Use this for initialization

        public BlockDeathEvent(BrickCollision.BrickType type, GameObject obj)
        {
            BrickCollision coll = obj.GetComponent<BrickCollision>();
            double value, hpValue = coll.Maxhp;
            if (type == BrickCollision.BrickType.Pig)
            {
                value = 10 * hpValue;
            }
            else
            {
                value = hpValue;
            }
            this.type = type;
            this.value = (int)(value*11.3);

        }

        #region Methods

   
        #endregion

        public double Value
        {
            get
            {
                return this.value;
            }
        }

        public BrickCollision.BrickType Type
        {
            get
            {
                return this.type;
            }
        }
    }
}