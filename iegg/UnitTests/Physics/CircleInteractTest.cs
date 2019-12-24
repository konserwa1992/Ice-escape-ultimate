using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Engine.GameUtility.Physic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnitTests.Physics
{
    class CircleInteractTest
    {
        private Polygon TESTPOLYGON;
        private Circle TESTCIRCLE;
        private Line TESTLINE;
        private List<Circle> CircleList;

        [SetUp]
        public void Setup()
        {
            TESTPOLYGON = new Polygon("TESTPOLYGON");
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(-4f,0, 4f), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(6,0,2), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(9,0,-2), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(3,0,-2), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(-8,0,1), Color.Red));
            CircleList = new List<Circle>();
            CircleList.Add(new Circle(new Vector2(-4,2), 1.03f));//Pan jest w polygonie
            CircleList.Add(new Circle(new Vector2(1, 0), 2.06f));//Pan jest poza ale okrag zachacza czyli jest kolizja
            CircleList.Add(new Circle(new Vector2(-9,4), 0.94f));//Pan jest w polygonie




        }


        [Test]
        public void InsidePolygon()
        {

            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[0]),false);
            //Assert.Equals();
        }

        [Test]
        public void OutSideButStillCollideByRadius()
        {

            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[1]), true);
        }

        [Test]
        //No panowie życie 
            public void OutSide ()
        {
            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[2]), false);
        }
    }
}
