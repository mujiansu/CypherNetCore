﻿namespace CypherTwo.Core
{
    using System;
    using System.Linq;

    using NeoPlayground.Tests;

    public interface ICypherDataReader
    {
        string[] Columns { get; }

        bool Next();

        T Get<T>(int index);
    }

    public class CypherDataReader : ICypherDataReader
    {
        private readonly NeoResponse data;

        private int rowPointer = -1;

        internal CypherDataReader(NeoResponse data)
        {
            this.data = data;
        }

        public string[] Columns
        {
            get { return this.data.results.First().columns.Select(c => c.ToString()).ToArray(); }
        }

        public bool Next()
        {
            return this.data.results.First().data.Length > ++rowPointer;
        }

        public T Get<T>(int index)
        {
            if (index < 0 || index > this.data.results.First().columns.Length)
                throw new IndexOutOfRangeException("index");

            if (this.data.results.First().data.Length <= this.rowPointer)
                throw new InvalidOperationException("exceed result count");

            return this.data.results.First().data[this.rowPointer].row[index].ToObject<T>();
        }
    }
}