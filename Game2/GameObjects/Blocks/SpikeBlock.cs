using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RGR.GameClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.GameObjects.Blocks
{
    class SpikeBlock : Block
    {
        public SpikeBlock(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            this.rectangle = rectangle;
            passable = false;
            damaged = true;
            drawLayer = 0.1f;
        }
    }
}