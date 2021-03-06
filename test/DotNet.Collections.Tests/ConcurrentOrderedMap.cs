﻿using System;
using System.Collections.Generic;
using Xunit;

namespace DotNet.Collections.Tests
{
    public class ConcurrentOrderedMapTests
    {
        [Fact]
        public void ConcurrentOrderedMap_Supports_ReadingCount()
        {
            var dict = new ConcurrentOrderedDictionary<string, string>();
            Assert.Empty(dict);
        }

        [Fact]
        public void ConcurrentOrderedMap_Supports_Adds()
        {
            var dict = new ConcurrentOrderedDictionary<string, string>();
            dict.Add("foo", "bar");
            Assert.Single(dict);
        }

        [Fact]
        public void ConcurrentOrderedMap_Add_ReturnsTrueIfItemIsAdded()
        {
            var dict = new ConcurrentOrderedDictionary<string, string>();
            Assert.True(dict.Add("foo", "bar"));
        }

        [Fact]
        public void ConcurrentOrderedMap_Add_ReturnsFalseIfKeyExists()
        {
            const string key = "foo";
            const string value = "bar";

            var dict = new ConcurrentOrderedDictionary<string, string>();
            dict.Add(key, value);

            Assert.False(dict.Add(key, value));
        }

        [Fact]
        public void ConcurrentOrderedMap_Supports_AddWithKeyValuePair()
        {
            var dict = new ConcurrentOrderedDictionary<string, string>();
            dict.Add(new KeyValuePair<string, string>("foo", "bar"));
            Assert.Single(dict);
        }

        [Fact]
        public void ConcurrentOrderedMap_Supports_AddOrUpdate()
        {
            var dict = new ConcurrentOrderedDictionary<string, string>();
            var result = dict.AddOrUpdate("foo", "bar", (k, v) => "baz");

            Assert.Single(dict);
            Assert.Equal("bar", result);

            result = dict.AddOrUpdate("foo", "baz", (k, v) => "baz");

            Assert.Single(dict);
            Assert.Equal("baz", result);
        }

        [Fact]
        public void ConcurrentOrderedMap_AddOrUpdate_UpdateCallbackNotCalledForNewItem()
        {
            var dict = new ConcurrentOrderedDictionary<string, string>();

            string callbackValue = null;
            dict.AddOrUpdate("foo", "bar", (k, v) => {
                callbackValue = v;
                return "baz";
            });

            Assert.Null(callbackValue);
        }

        [Fact]
        public void ConcurrentOrderedMap_AddOrUpdate_PassesOldValueForExistingItem()
        {
            var dict = new ConcurrentOrderedDictionary<string, string>();
            dict.AddOrUpdate("foo", "bar", (k, v) => "baz");

            string callbackValue = null;
            dict.AddOrUpdate("foo", "baz", (k, v) => {
                callbackValue = v;
                return "baz";
            });

            Assert.Equal("bar", callbackValue);
        }

        [Fact]
        public void ConcurrentOrderedMap_ContainsKey_ReturnsTrueIfKeyExists()
        {
            const string expectedKey = "foo";

            var dict = new ConcurrentOrderedDictionary<string, string>();
            dict.Add(expectedKey, "bar");
            Assert.True(dict.ContainsKey(expectedKey));
        }

        [Fact]
        public void ConcurrentOrderedMap_ContainsKey_ReturnsFalseIfKeyDoesNotExist()
        {
            var dict = new ConcurrentOrderedDictionary<string, string>();
            Assert.False(dict.ContainsKey("foo"));
        }

        [Fact]
        public void ConcurrentOrderedMap_TryGetValue_ReturnsValueOutIfKeyExists()
        {
            const string key = "foo";
            const string expectedValue = "bar";

            var dict = new ConcurrentOrderedDictionary<string, string>();
            dict.Add(key, expectedValue);

            dict.TryGetValue(key, out var value);
            Assert.Equal(expectedValue, value);
        }

        [Fact]
        public void ConcurrentOrderedMap_TryGetValue_ReturnsTrueIfKeyExists()
        {
            const string key = "foo";

            var dict = new ConcurrentOrderedDictionary<string, string>();
            dict.Add(key, "bar");

            Assert.True(dict.TryGetValue(key, out var value));
        }

        [Fact]
        public void ConcurrentOrderedMap_TryGetValue_ReturnsFalseIfKeyDoesNotExist()
        {
            var dict = new ConcurrentOrderedDictionary<string, string>();
            Assert.False(dict.TryGetValue("foo", out var value));
        }
    }
}
