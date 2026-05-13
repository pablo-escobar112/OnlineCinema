// CinemaModule.Tests/CinemaServiceTests.cs
using NUnit.Framework;
using NUnit.Framework.Legacy;  // ← ВАЖНО: добавляем этот using
using System;
using System.Collections.Generic;
using CinemaModule.Services;

namespace CinemaModule.Tests
{
    [TestFixture]
    public class CinemaServiceTests
    {
        private CinemaService _service;

        [SetUp]
        public void Setup()
        {
            _service = new CinemaService();
        }

        [Test]
        public void IsSubscriptionActive_Tomorrow_ReturnsTrue()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            var result = _service.IsSubscriptionActive(tomorrow);
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void IsSubscriptionActive_Yesterday_ReturnsFalse()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var result = _service.IsSubscriptionActive(yesterday);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void GetRemainingDays_10Days_Returns10()
        {
            var future = DateTime.Today.AddDays(10);
            var days = _service.GetRemainingDays(future);
            ClassicAssert.AreEqual(10, days);
        }

        [Test]
        public void GetRemainingDays_Expired_Returns0()
        {
            var past = DateTime.Today.AddDays(-5);
            var days = _service.GetRemainingDays(past);
            ClassicAssert.AreEqual(0, days);
        }

        [Test]
        public void AddToFavorites_NewMovie_ReturnsTrue()
        {
            var favorites = new List<int> { 1, 2 };
            bool added = _service.AddToFavorites(favorites, 3);
            ClassicAssert.IsTrue(added);
            ClassicAssert.Contains(3, favorites);
        }

        [Test]
        public void AddToFavorites_ExistingMovie_ReturnsFalse()
        {
            var favorites = new List<int> { 1, 2 };
            bool added = _service.AddToFavorites(favorites, 2);
            ClassicAssert.IsFalse(added);
            ClassicAssert.AreEqual(2, favorites.Count);
        }

        [Test]
        public void GetRecommendedGenre_MostFrequent_ReturnsCorrect()
        {
            var watched = new List<int> { 1, 2, 2, 3, 2, 1 };
            int genre = _service.GetRecommendedGenre(watched);
            ClassicAssert.AreEqual(2, genre);
        }

        [Test]
        public void GetRecommendedGenre_EmptyList_ReturnsMinusOne()
        {
            var watched = new List<int>();
            int genre = _service.GetRecommendedGenre(watched);
            ClassicAssert.AreEqual(-1, genre);
        }
    }
}