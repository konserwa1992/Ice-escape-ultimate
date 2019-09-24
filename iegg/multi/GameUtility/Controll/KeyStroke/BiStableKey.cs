using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Engine.GameUtility.Controll.KeyStroke
{
    public class BiStableKey: IKeyStroke
    {
        public Keys Key { get; set; }
        private bool isPushed = false;
        public ClickTrigger action;

        public BiStableKey(Keys key)
        {
            this.Key = key;
        }

        public ClickTrigger ClickAction()
        {
            action();
            return action;
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(this.Key) && isPushed == false)
            {
                ClickAction();
            }

            isPushed = Keyboard.GetState().IsKeyDown(this.Key);
        }
    }
}