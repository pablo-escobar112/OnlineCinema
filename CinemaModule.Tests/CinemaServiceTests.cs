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

        [Test]
        public void CreateCollection_ValidName_ReturnsTrue()
        {
            bool result = _service.CreateCollection("Любимые фильмы", "Описание", out string error);
            ClassicAssert.IsTrue(result);
            ClassicAssert.IsEmpty(error);
        }

        [Test]
        public void CreateCollection_EmptyName_ReturnsFalse()
        {
            bool result = _service.CreateCollection("", "", out string error);
            ClassicAssert.IsFalse(result);
            ClassicAssert.IsNotEmpty(error);
        }

        [Test]
        public void AddToCollection_NewMovie_ReturnsTrue()
        {
            var ids = new List<int> { 1, 2 };
            bool result = _service.AddToCollection(ids, 3);
            ClassicAssert.IsTrue(result);
            ClassicAssert.Contains(3, ids);
        }

        [Test]
        public void AddToCollection_ExistingMovie_ReturnsFalse()
        {
            var ids = new List<int> { 1, 2 };
            bool result = _service.AddToCollection(ids, 2);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void RemoveFromCollection_ExistingMovie_ReturnsTrue()
        {
            var ids = new List<int> { 1, 2, 3 };
            bool result = _service.RemoveFromCollection(ids, 2);
            ClassicAssert.IsTrue(result);
            ClassicAssert.AreEqual(2, ids.Count);
        }

        [Test]
        public void RemoveFromCollection_NotExistingMovie_ReturnsFalse()
        {
            var ids = new List<int> { 1, 3 };
            bool result = _service.RemoveFromCollection(ids, 5);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void GetMovieCount_ThreeMovies_Returns3()
        {
            var ids = new List<int> { 1, 2, 3 };
            int count = _service.GetMovieCount(ids);
            ClassicAssert.AreEqual(3, count);
        }

        [Test]
        public void GetMovieCount_EmptyList_Returns0()
        {
            var ids = new List<int>();
            int count = _service.GetMovieCount(ids);
            ClassicAssert.AreEqual(0, count);
        }

        [Test]
        public void IsMovieAvailableBySubscription_NoSubscription_ReturnsFalse()
        {
            bool result = _service.IsMovieAvailableBySubscription(12, false, "Базовый");
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void IsMovieAvailableBySubscription_BasicAdultMovie_ReturnsFalse()
        {
            bool result = _service.IsMovieAvailableBySubscription(18, true, "Базовый");
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void IsMovieAvailableBySubscription_PremiumAdultMovie_ReturnsTrue()
        {
            bool result = _service.IsMovieAvailableBySubscription(18, true, "Премиум");
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void IsMovieAvailableBySubscription_BasicRegularMovie_ReturnsTrue()
        {
            bool result = _service.IsMovieAvailableBySubscription(12, true, "Базовый");
            ClassicAssert.IsTrue(result);
        }

    }
}