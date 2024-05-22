
using Microsoft.Data.Sqlite;
using System.Reflection;
using System.Collections.Generic;

namespace CSharp2Projekt
{
    public class QueryBuilder : IDisposable, IAsyncDisposable
    {
        private SqliteConnection connection;

        public QueryBuilder(string connectionString = "Data Source=D:\\Skola\\SecondYear\\2ndSemester\\C#\\Projekt\\WebServerPrihlasky\\databaze.db")
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
        }

        public async Task<long?> InsertAsync<T>(T entity)
        {
            //var tableName = typeof(T).Name;
            //var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");

            //var columns = string.Join(", ", properties.Select(p => p.Name));
            //var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            //var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values});";

            //using var command = new SqliteCommand(sql, connection);

            //foreach (var property in properties)
            //{
            //    var value = property.GetValue(entity);
            //    command.Parameters.AddWithValue($"@{property.Name}", value ?? DBNull.Value);
            //}

            //var res = await command.ExecuteNonQueryAsync();

            //return res;

            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");

            var columns = string.Join(", ", properties.Select(p => p.Name));
            var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values}); SELECT last_insert_rowid();";

            using var command = new SqliteCommand(sql, connection);

            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                command.Parameters.AddWithValue($"@{property.Name}", value ?? DBNull.Value);
            }

            var res = await command.ExecuteScalarAsync();

            return res as long?;
        }


        public async Task<IEnumerable<T>> SelectAllAsync<T>()
        {
            var tableName = typeof(T).Name;
            var sql = $"SELECT * FROM {tableName}";

            using var command = new SqliteCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            var results = new List<T>();

            while (await reader.ReadAsync())
            {
                var instance = Activator.CreateInstance<T>();

                foreach (var property in typeof(T).GetProperties())
                {
                    var value = reader[property.Name];
                    if (value != DBNull.Value)
                    {
                        property.SetValue(instance, value);
                    }
                }

                results.Add(instance);
            }

            return results;
        }


        public async Task<T> SelectByIdAsync<T>(long id)
        {
            var tableName = typeof(T).Name;
            var sql = $"SELECT * FROM {tableName} WHERE Id = @Id";

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var instance = Activator.CreateInstance<T>();

                foreach (var property in typeof(T).GetProperties())
                {
                    var value = reader[property.Name];
                    if (value != DBNull.Value)
                    {
                        property.SetValue(instance, value);
                    }
                }

                return instance;
            }

            return default;
        }

        public async Task<List<T>> SelectWhereAsync<T>(string column, string value)
        {
            var tableName = typeof(T).Name;
            var sql = $"SELECT * FROM {tableName} WHERE {column} LIKE @Value";

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Value", value);

            using var reader = await command.ExecuteReaderAsync();

            var results = new List<T>();

            while (await reader.ReadAsync())
            {
                var instance = Activator.CreateInstance<T>();

                foreach (var property in typeof(T).GetProperties())
                {
                    var val = reader[property.Name];
                    if (val != DBNull.Value)
                    {
                        property.SetValue(instance, val);
                    }
                }

                results.Add(instance);
            }

            return results;
        }

        public async Task<int> UpdateAsync<T>(T entity)
        {
            var tableName = typeof(T).Name;
            var sql = $"UPDATE {tableName} SET ";
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (property.Name != "Id")
                {
                    sql += $"{property.Name} = @{property.Name}, ";
                }
            }
            sql = sql.Remove(sql.Length - 2);
            sql += $" WHERE Id = @Id";

            using var command = new SqliteCommand(sql, connection);


            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                if (value != null)
                {
                    command.Parameters.AddWithValue($"@{property.Name}", value);
                }
                else
                {
                    command.Parameters.AddWithValue($"@{property.Name}", DBNull.Value);
                }
            }

            var res = await command.ExecuteNonQueryAsync();

            return res;
        }

        public async Task<int> DeleteAsync<T>(long Id)
        {
            var tableName = typeof(T).Name;
            var sql = $"DELETE FROM {tableName} WHERE Id = @Id";
            var properties = typeof(T).GetProperties();

            using var command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", Id);

            var res = await command.ExecuteNonQueryAsync();

            return res;
        }

        public void Dispose()
        {
            //((IDisposable)connection).Dispose();
            connection.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            //return ((IAsyncDisposable)connection).DisposeAsync();
            await connection.DisposeAsync();

        }
    }
}
