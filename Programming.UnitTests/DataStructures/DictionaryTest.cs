﻿using Programming.DataStructures;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;


namespace Programming.UnitTests.DataStructures
{
    [ExcludeFromCodeCoverage]
    public class DictionaryTest
    {
        public DictionaryTest() { }

        [Fact]
        public void Dictionary_CreateNew()
        {
            var newDictionary = new Dictionary<string, string>();
            Assert.NotNull(newDictionary);
        }

        [Theory]
        [InlineData("a", "b")]
        [InlineData(1, 2)]
        [InlineData(true, 2)]
        public void Dictionary_CreateNewAndAdd<tkey, tvalue>(tkey key, tvalue value)
        {
            var newDictionary = new Dictionary<tkey, tvalue>();
            newDictionary.Add(key, value);

            Assert.Equal(newDictionary[key], value);
        }

        [Theory]
        [InlineData("a", "B")]
        [InlineData(1, 2)]
        [InlineData(true, 2)]
        public void Dictionary_DeleteValue<tkey, tvalue>(tkey key, tvalue value)
        {
            var newDictionary = new Dictionary<tkey, tvalue>();
            newDictionary.Add(key, value);
            newDictionary.Remove(key);

            Assert.True(newDictionary.Count == 0);
        }

        [Theory]
        [InlineData("a", "b")]
        [InlineData(1, 2)]
        [InlineData(false, 2)]
        public void Dictionary_CreateAddDeleteAdd<tkey, tvalue>(tkey key, tvalue value)
        {
            var newDictionary = new Dictionary<tkey, tvalue>();
            newDictionary.Add(key, value);
            newDictionary.Remove(key);
            newDictionary.Add(key, value);

            Assert.Equal(newDictionary[key], value);
        }

        [Fact]
        public void Dictionary_IteratedOverValues()
        {
            int loopCount = 0;
            var newDictionary = new Dictionary<string, string>();
            newDictionary.Add("a", "b");
            newDictionary.Add("c", "D");
            newDictionary.Add("e", "a");
            newDictionary.Remove("c");

            foreach (var item in newDictionary)
            {
                Assert.True(item.Key != "c");
                Assert.True(item.Key == "a" || item.Key == "e");
                loopCount++;
            }

            Assert.Equal(loopCount, newDictionary.Count);
        }

        [Fact]
        public void Dictionary_Clear()
        {
            var newDictionary = new Dictionary<string, string>();
            newDictionary.Add("a", "b");
            newDictionary.Add("c", "D");
            newDictionary.Add("e", "a");

            newDictionary.Clear();

            Assert.Equal(0, newDictionary.Count);
        }

        [Fact]
        public void Dictionary_GetValueFromIndexer()
        {
            var newDictionary = new Dictionary<string, string>();
            newDictionary.Add("a", "b");

            Assert.Equal("b", newDictionary["a"]);
        }

        [Fact]
        public void Dictionary_SetValueFromIndexer()
        {
            var newDictionary = new Dictionary<string, string>();
            newDictionary.Add("a", "b");
            newDictionary["a"] = "c";

            Assert.Equal("c", newDictionary["a"]);
        }

        [Fact]
        public void Dictionary_KeyAlreadyExists_ThrowsInvalidOperationException()
        {
            var newDictionary = new Dictionary<string, string>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                newDictionary.Add("a", "b");
                newDictionary.Add("a", "c");
            });
        }

        [Fact]
        public void Dictionary_NoKeyAtIndex_ThrowsInvalidOperationException()
        {
            var newDictionary = new Dictionary<string, string>();

            Assert.Throws<InvalidOperationException>(() => newDictionary["a"] = "b");
        }
    }
}
