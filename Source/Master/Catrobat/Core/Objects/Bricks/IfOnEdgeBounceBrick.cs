﻿using System.Xml.Linq;

namespace Catrobat.Core.Objects.Bricks
{
    public class IfOnEdgeBounceBrick : Brick
    {
        public IfOnEdgeBounceBrick()
        {
        }

        public IfOnEdgeBounceBrick(Sprite parent) : base(parent)
        {
        }

        public IfOnEdgeBounceBrick(XElement xElement, Sprite parent) : base(xElement, parent)
        {
        }

        internal override void LoadFromXML(XElement xRoot)
        {
        }

        internal override XElement CreateXML()
        {
            var xRoot = new XElement("ifOnEdgeBounceBrick");

            //CreateCommonXML(xRoot);

            return xRoot;
        }

        public override DataObject Copy(Sprite parent)
        {
            var newBrick = new IfOnEdgeBounceBrick(parent);

            return newBrick;
        }
    }
}