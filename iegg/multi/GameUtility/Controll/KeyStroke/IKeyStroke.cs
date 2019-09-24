using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Engine.GameUtility.Controll.KeyStroke
{

    public delegate void ClickTrigger();

    public interface IKeyStroke
    {
        Keys Key { get; set; }
        void Update();
        ClickTrigger ClickAction();
    }
}