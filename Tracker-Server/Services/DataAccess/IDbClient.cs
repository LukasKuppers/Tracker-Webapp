﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Services.DataAccess
{
    public interface IDbClient
    {
        // get all records in the given table
        public List<T> GetAll<T>(string table);

        // Find and return (if extant) all records in the given table thats value under the given field matches 'data'
        public List<T> FindByField<T, U>(string table, string fieldName, U data);

        // insert record into the given table
        public void InsertRecord<T>(string table, T record);

        // deletes the record (if extant) with the given id from the table specified
        public void DeleteRecord<T>(string table, Guid id);

        // updates the fieldName field with data in the record (if extant) with the given id
        public void UpdateRecord<T, U>(string table, Guid id, string fieldName, U data);

        // returns true if the specified table contains a record with the given data under field 'fieldName'
        public bool Contains<T, U>(string table, string fieldName, U data);
    }
}
