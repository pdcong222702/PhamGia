using PhamGia.Core.DataTableObject.Attributes;
using Radzen;
using System.Data;

namespace PhamGia.Core.DataTableObject.Mapping
{
    public class DataNamesMapper<TEntity> where TEntity : class, new()
    {
        public TEntity Map(DataRow row)
        {
            var entity = new TEntity();
            return this.Map(row, entity);
        }

        public TEntity Map(DataRow row, TEntity entity)
        {
            var properties = (typeof(TEntity)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                              .ToList();
            foreach (var prop in properties)
            {
                PropertyMapHelper.Map(typeof(TEntity), row, prop, entity);
            }

            return entity;
        }

        public IEnumerable<TEntity> Map(DataTable table)
        {
            var entities = new List<TEntity>();
            var properties = (typeof(TEntity)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
            .ToList();

            foreach (DataRow row in table.Rows)
            {
                var entity = new TEntity();
                foreach (var prop in properties)
                {
                    PropertyMapHelper.Map(typeof(TEntity), row, prop, entity);
                }
                entities.Add(entity);
            }

            return entities;
        }

        public IEnumerable<TEntity> Map_v2(DataTable table)
        {
            var entities = new List<TEntity>();
            var properties = typeof(TEntity).GetProperties()
                                              .ToList();
            foreach (DataRow row in table.Rows)
            {
                var entity = Activator.CreateInstance<TEntity>();
                foreach (var prop in properties)
                {
                    PropertyMapHelper.Map(typeof(TEntity), row, prop, entity);
                }

                entities.Add(entity);
            }

            return entities;
        }
    }
}
