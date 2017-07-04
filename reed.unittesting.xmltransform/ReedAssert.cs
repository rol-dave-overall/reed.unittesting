using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reed.UnitTesting
{
    public static class ReedAssert
    {
        public static void Contains(object anObject, ICollection collection)
        {
            CollectionAssert.Contains(collection, anObject);
        }

        public static void Contains(object anObject, ICollection collection, string message, params object[] parms)
        {
            CollectionAssert.Contains(collection, anObject, message, parms);
        }

        public static void DoesNotThrow(Action code)
        {
            try
            {
                code();
            }
            catch
            {
                Assert.Fail();
            }
        }

        public static void DoesNotThrow(Action code, string message, params object[] parms)
        {
            try
            {
                code();
            }
            catch
            {
                Assert.Fail(message, parms);
            }
        }

        /// <summary>
        /// Verifies that the first value is greater than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be greater.</param>
        /// <param name="arg2">The second value, expected to be less.</param>
        public static void Greater<T>(T arg1, T arg2) where T : IComparable
        {
            Greater(arg1, arg2, "{0} is less than or equal to {1}.", arg2, arg1);
        }

        /// <summary>
        /// Verifies that the first value is greater than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be greater.</param>
        /// <param name="arg2">The second value, expected to be less.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void Greater<T>(T arg1, T arg2, string message) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) <= 0)
            {
                Assert.Fail(message);
            }
        }

        /// <summary>
        /// Verifies that the first value is greater than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be greater.</param>
        /// <param name="arg2">The second value, expected to be less.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void Greater<T>(T arg1, T arg2, string message, params object[] parameters) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) <= 0)
            {
                Assert.Fail(message, parameters);
            }
        }

        public static void Ignore()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// Asserts that an object may be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        public static void IsAssignableFrom(object value, Type expectedType)
        {
            IsAssignableFrom(value, expectedType, "Expected {0} to be assignable from {1}", value, expectedType);
        }

        /// <summary>
        /// Asserts that an object may be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsAssignableFrom(object value, Type expectedType, string message)
        {
            IsAssignableFrom(value, expectedType, message, null);
        }

        /// <summary>
        /// Asserts that an object may be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsAssignableFrom(object value, Type expectedType, string message, params object[] parameters)
        {
            if (!value.GetType().IsAssignableFrom(expectedType))
            {
                Assert.Fail(message, parameters);
            }
        }

        /// <summary>
        /// Assert that an array, list or other collection is empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        public static void IsEmpty(IEnumerable collection)
        {
            int collectionCount;
            var is3 = collection as ICollection;

            var collection1 = collection as IList<object> ?? collection.Cast<object>().ToList();
            if (is3 != null)
            {
                collectionCount = is3.Count;
            }
            else
            {
                var enumerable = collection as IList<object> ?? collection1.Cast<object>().ToList();
                collectionCount = enumerable.Count;
            }

            IsEmpty(collection1, "Expected a collection containing &lt;0&gt; items but actual was &lt;{0}&gt; items.", 0, collectionCount);
        }

        /// <summary>
        /// Assert that an array, list or other collection is empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsEmpty(IEnumerable collection, string message)
        {
            IsEmpty(collection, message, null);
        }

        /// <summary>
        /// Assert that an array, list or other collection is empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsEmpty(IEnumerable collection, string message, params object[] parameters)
        {
            int collectionCount;
            var is3 = collection as ICollection;

            if (is3 != null)
            {
                collectionCount = is3.Count;
            }
            else
            {
                var enumerable = collection as IList<object> ?? collection.Cast<object>().ToList();
                collectionCount = enumerable.Count;
            }

            Assert.IsTrue(collectionCount == 0, message, parameters);
        }

        /// <summary>
        /// Asserts that a string is empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        public static void IsEmpty(string value)
        {
            IsEmpty(value, "Expected &lt;{0}&gt; but actual was &lt;{1}&gt;.", String.Empty, value);
        }

        /// <summary>
        /// Asserts that a string is empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsEmpty(string value, string message)
        {
            IsEmpty(value, message, null);
        }

        /// <summary>
        /// Asserts that a string is empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsEmpty(string value, string message, params object[] parameters)
        {
            Assert.IsTrue(value.Length == 0, message, parameters);
        }

        public static void IsInstanceOf<T>(object value)
        {
            Assert.IsInstanceOfType(value, typeof(T));
        }

        /// <summary>
        /// Asserts that an object may not be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        public static void IsNotAssignableFrom(object value, Type expectedType)
        {
            IsNotAssignableFrom(value, expectedType, "Expected {0} to be assignable from {1}", value, expectedType);
        }

        /// <summary>
        /// Asserts that an object may not be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsNotAssignableFrom(object value, Type expectedType, string message)
        {
            IsNotAssignableFrom(value, expectedType, message, null);
        }

        /// <summary>
        /// Asserts that an object may not be assigned a value of a given <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="expectedType">The expected <see cref="Type"/>.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsNotAssignableFrom(object value, Type expectedType, string message, params object[] parameters)
        {
            if (value.GetType().IsAssignableFrom(expectedType))
            {
                Assert.Fail(message, parameters);
            }
        }

        /// <summary>
        /// Assert that an array, list or other collection is not empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        public static void IsNotEmpty(ICollection collection)
        {
            IsNotEmpty(collection, "Expected a collection containing &lt;0&gt; items but actual was &lt;{0}&gt; items.", collection.Count, 0);
        }

        /// <summary>
        /// Assert that an array, list or other collection is not empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsNotEmpty(ICollection collection, string message)
        {
            IsNotEmpty(collection, message, null);
        }

        /// <summary>
        /// Assert that an array, list or other collection is not empty.
        /// </summary>
        /// <param name="collection">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsNotEmpty(ICollection collection, string message, params object[] parameters)
        {
            Assert.IsFalse(collection.Count == 0, message, parameters);
        }

        /// <summary>
        /// Asserts that a string is not empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        public static void IsNotEmpty(string value)
        {
            IsNotEmpty(value, "Expected &lt;{0}&gt; but actual was &lt;{1}&gt;.", value, String.Empty);
        }

        /// <summary>
        /// Asserts that a string is not empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void IsNotEmpty(string value, string message)
        {
            IsNotEmpty(value, message, null);
        }

        /// <summary>
        /// Asserts that a string is not empty.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void IsNotEmpty(string value, string message, params object[] parameters)
        {
            Assert.IsFalse(value.Length == 0, message, parameters);
        }

        /// <summary>
        /// Verifies that the first value is less than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be less.</param>
        /// <param name="arg2">The second value, expected to be greater.</param>
        public static void Less<T>(T arg1, T arg2) where T : IComparable
        {
            Less(arg1, arg2, "{0} is greater than or equal to {1}.", arg2, arg1);
        }

        /// <summary>
        /// Verifies that the first value is less than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be less.</param>
        /// <param name="arg2">The second value, expected to be greater.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void Less<T>(T arg1, T arg2, string message) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) >= 0)
            {
                Assert.Fail(message);
            }
        }

        /// <summary>
        /// Verifies that the first value is less than the second value.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg1">The first value, expected to be less.</param>
        /// <param name="arg2">The second value, expected to be greater.</param>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        /// <param name="parameters">An array of parameters to use when formatting <paramref name="message"/>.</param>
        public static void Less<T>(T arg1, T arg2, string message, params object[] parameters) where T : IComparable
        {
            if (((IComparable)arg1).CompareTo(arg2) >= 0)
            {
                Assert.Fail(message, parameters);
            }
        }
    }
}