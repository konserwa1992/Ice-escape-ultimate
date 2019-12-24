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
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(-5.1318f,0f, 0.79f), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(-1.53182f,0f, 4.00818f), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(4.14091f,0f, 4.02636f), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(3.75909f,0, -1.93727f), Color.Red));
            TESTPOLYGON.AddPoint(new VertexPositionColor(new Vector3(-4.47727f,0, -3.02818f), Color.Red));
            CircleList = new List<Circle>();
            CircleList.Add(new Circle(new Vector2(-6.05909f, 3.68091f), 1.36f)); // okrąg poza polygonem
            CircleList.Add(new Circle(new Vector2(-6.11364f, -0.59182f), 2.04f)); // okrąg poza polygonem
            CircleList.Add(new Circle(new Vector2(-0.13182f, -0.57364f), 1.36f)); // okrąg poza polygonem
            CircleList.Add(new Circle(new Vector2(1.01364f, 3.75364f), 1.71f)); // okrąg poza polygonem


        }


        [Test]
        public void Poza()
        {

            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[0]),false);
            //Assert.Equals();
        }

        [Test]
        public void WAleNazwenatrz()
        {

            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[1]), true);
        }
        [Test]
        public void WewnatrzWielokata()
        {

            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[2]), true);
        }
        [Test]
        public void WewnatrzAlePromienJestPozaWielokatem()
        {
            Assert.AreEqual(TESTPOLYGON.IsCollide(CircleList[3]), true);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
