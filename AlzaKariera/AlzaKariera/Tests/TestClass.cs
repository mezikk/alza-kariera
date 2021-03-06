﻿using AlzaKariera.Classes;
using NUnit.Framework;

namespace AlzaKariera.Tests
{
    /// <summary>Předpis pro všechny testy</summary>
    public class TestClass
    {
        /// <summary><see cref="Driver"/></summary>
        public Driver Driver;

        /// <summary>Inicializace <see cref="Driver"/> na začátku testu</summary>
        /// <param name="testClass">Objekt typu <see cref="TestClass"/>, který test spouští</param>
        public void InitDriver(TestClass testClass)
        {
            Driver = new Driver(testClass);
            Driver.InitializeDriver();
        }

        /// <summary>Zrušení <see cref="Driver"/> na konci testu</summary>
        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
        }
    }
}