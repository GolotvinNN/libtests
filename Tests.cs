using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SF2022UserLib.Tests
{
    [TestClass]
    public class CalculationTests
    {
        private Calculations _calculations;

        // Метод, который инициализирует экземпляр класса Calculations перед каждым тестом
        [TestInitialize]
        public void Setup()
        {
            _calculations = new Calculations();
        }

        // Тест на выброс исключения, если массив начальных времен является null
        [TestMethod]
        public void AvailablePeriods_ShouldThrow_WhenStartTimesIsNull()
        {
            TimeSpan[] startTimes = null;
            int[] durations = { 30, 45 };

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);
            });
        }

        // Тест на выброс исключения, если массив длительностей является null
        [TestMethod]
        public void AvailablePeriods_ShouldThrow_WhenDurationsIsNull()
        {
            TimeSpan[] startTimes = { TimeSpan.FromHours(9), TimeSpan.FromHours(10) };
            int[] durations = null;

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);
            });
        }

        // Тест на выброс исключения, если длина массивов начальных времен и длительностей не совпадает
        [TestMethod]
        public void AvailablePeriods_ShouldThrow_WhenArraysLengthMismatch()
        {
            TimeSpan[] startTimes = { TimeSpan.FromHours(9), TimeSpan.FromHours(10) };
            int[] durations = { 30 };

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);
            });
        }

        // Тест на выброс исключения, если время консультации отрицательное
        [TestMethod]
        public void AvailablePeriods_ShouldThrow_WhenConsultationTimeIsNegative()
        {
            TimeSpan[] startTimes = { TimeSpan.FromHours(9), TimeSpan.FromHours(10) };
            int[] durations = { 30, 45 };

            Assert.ThrowsException<ArgumentException>(() =>
            {
                _calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), -30);
            });
        }

        // Тест на выброс исключения, если время начала рабочего дня больше времени окончания
        [TestMethod]
        public void AvailablePeriods_ShouldThrow_WhenEndBeforeStart()
        {
            TimeSpan[] startTimes = { TimeSpan.FromHours(15) };
            int[] durations = { 30 };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                _calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(17), TimeSpan.FromHours(9), 60);
            });
        }

        // Тест на выброс исключения, если консультация начинается вне рабочего времени
        [TestMethod]
        public void AvailablePeriods_ShouldThrow_WhenOutsideWorkingHours()
        {
            TimeSpan[] startTimes = { TimeSpan.FromHours(2) };
            int[] durations = { 30 };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                _calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);
            });
        }

        // Тест на правильное количество свободных слотов, когда нет запланированных консультаций
        [TestMethod]
        public void AvailablePeriods_ShouldReturnFullDay_WhenNoConsultations()
        {
            TimeSpan[] startTimes = { };
            int[] durations = { };

            var result = _calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);

            Assert.AreEqual(8, result.Length);
        }

        // Тест на правильное количество свободных слотов, когда запланирована одна консультация
        [TestMethod]
        public void AvailablePeriods_ShouldReturnSlots_WhenSingleConsultation()
        {
            TimeSpan[] startTimes = { TimeSpan.FromHours(9) };
            int[] durations = { 30 };

            var result = _calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);

            Assert.AreEqual(7, result.Length);
        }

        // Тест на правильное количество свободных слотов, когда запланировано несколько консультаций
        [TestMethod]
        public void AvailablePeriods_ShouldReturnCorrectSlots_WhenMultipleConsultations()
        {
            TimeSpan[] startTimes = { TimeSpan.FromHours(9), TimeSpan.FromHours(11), TimeSpan.FromHours(13) };
            int[] durations = { 30, 45, 60 };

            var result = _calculations.AvailablePeriods(startTimes, durations, TimeSpan.FromHours(9), TimeSpan.FromHours(17), 60);

            Assert.AreEqual(5, result.Length);
        }

        // Тест на правильное форматирование TimeSpan в строку
        [TestMethod]
        public void Format_ShouldReturnCorrectFormat_WhenGivenTimeSpan()
        {
            TimeSpan ts = new TimeSpan(0, 0, 0);

            string formattedTime = Calculations.Format(ts);

            Assert.AreEqual("00:00", formattedTime);
        }
    }
}
