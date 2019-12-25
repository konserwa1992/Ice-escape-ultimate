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
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(-7f,0, 1f), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(-2,0,4), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(5,0,4), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(0,0,0), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(-2,0,2), Color.Red));
            CircleList = new List<Circle>();
            CircleList.Add(new Circle(new Vector2(-6,4), 2f));//Poza 0
            CircleList.Add(new Circle(new Vector2(-2, 0), 2f));//Kolizja 1
            CircleList.Add(new Circle(new Vector2(0,2), 1f));//W polygonie 2
            CircleList.Add(new Circle(new Vector2(6, 4), 5.0f));//kolizja 3
            CircleList.Add(new Circle(new Vector2(0, 6), 1.0f));//Brak kolizji 4
        }


        [Test]
        public void InsidePolygon()
        {

            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[2]),false);
            //Assert.Equals();
        }

        [Test]
        public void OutSideButStillCollideByRadius()
        {

            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[1]), true);
        }

        [Test]
        public void OutSideButStillCollideByRadius2()
        {

            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[3]), true);
        }


        [Test]
        public void OutSide()
        {
            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[0]), false);
        }


        [Test]
        public void OutSide2()
        {
            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[4]), false);
        }
    }
}
