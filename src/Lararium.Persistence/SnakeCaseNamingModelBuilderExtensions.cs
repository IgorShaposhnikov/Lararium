using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Lararium.Persistence
{
    internal static partial class SnakeCaseNamingModelBuilderExtensions
    {
        extension(ModelBuilder builder) 
        {
            internal ModelBuilder UseSnakeCaseNamingConvention() 
            {

                // Применяем snake_case ко всем сущностям
                foreach (var entity in builder.Model.GetEntityTypes())
                {
                    // Таблицы
                    entity.SetTableName(ToSnakeCase(entity.GetTableName()));

                    // Колонки
                    foreach (var property in entity.GetProperties())
                    {
                        property.SetColumnName(ToSnakeCase(property.Name));
                    }

                    // Ключи
                    foreach (var key in entity.GetKeys())
                    {
                        key.SetName(ToSnakeCase(key.GetName()));
                    }

                    // Индексы
                    foreach (var index in entity.GetIndexes())
                    {
                        index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
                    }

                    // Внешние ключи
                    foreach (var fk in entity.GetForeignKeys())
                    {
                        fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));
                    }
                }

                return builder;
            }
        }


        [GeneratedRegex(@"^_+")]
        private static partial Regex LeadingUnderscoresRegex();
        [GeneratedRegex(@"([a-z0-9])([A-Z])")]
        private static partial Regex CamelCaseToSnakeCaseRegex();

        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var startUnderscores = LeadingUnderscoresRegex().Match(input);
            return startUnderscores + CamelCaseToSnakeCaseRegex().Replace(input, "$1_$2")
                .ToLowerInvariant();
        }
    }
}
