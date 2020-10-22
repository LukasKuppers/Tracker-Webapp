using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker_Server.Services.DataAccess;

namespace UnitTests
{
    class MockDBClient : Mock<IDbClient>
    {
        public MockDBClient MockGetAll<T>(string table, List<T> output)
        {
            Setup(x => x.GetAll<T>(
               table
            )).Returns(output);
            return this;
        }

        public MockDBClient MockFindByField<T, U>(string table, string fieldName, U data, List<T> output)
        {
            Setup(x => x.FindByField<T, U>(
                table,
                fieldName,
                data
            )).Returns(output);
            return this;
        }

        public MockDBClient MockInsertRecord<T>(string table, T record, DbStatus output)
        {
            Setup(x => x.InsertRecord<T>(
                table,
                record
            )).Returns(output);
            return this;
        }

        public MockDBClient MockDeleteRecord<T>(string table, Guid id, DbStatus output)
        {
            Setup(x => x.DeleteRecord<T>(
                table,
                id
            )).Returns(output);
            return this;
        }

        public MockDBClient MockUpdateRecord<T, U>(string table, Guid id, string fieldName, U data, DbStatus output)
        {
            Setup(x => x.UpdateRecord<T, U>(
                table,
                id,
                fieldName,
                data
            )).Returns(output);
            return this;
        }

        public MockDBClient MockContains<T, U>(string table, string fieldName, U data, bool output)
        {
            Setup(x => x.Contains<T, U>(
                table,
                fieldName,
                data
            )).Returns(output);
            return this;
        }
    }
}
