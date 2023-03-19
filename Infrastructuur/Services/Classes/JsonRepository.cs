using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructuur.Services.Interfaces;
using Newtonsoft.Json;
namespace Infrastructuur.Services.Classes
{
    public class JsonDatabase<T> : IJsonDatabase<T>
    {
        private readonly string _filename;

        public JsonDatabase(string filename)
        {
            _filename = filename;
        }
        public async Task<bool> CreateAsync(T item)
        {
            try
            {
                List<T> items = await GetAllAsync();
                if(items is null)
                {
                    items = new List<T>();
                }
                items.Add(item);
                await SaveAllAsync(items);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Expression<Func<T, bool>> predicate, T updateTo)
        {
            try
            {
                List<T> items = await GetAllAsync();
                var item = items.FirstOrDefault(predicate.Compile());

                if (item is not null)
                {
                    // update the item with the new data
                    var mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, T>()).CreateMapper();
                    mapper.Map(updateTo, item);

                    await SaveAllAsync(items);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate) 
        {
            try
            {
                List<T> items = await GetAllAsync();
                var dataToRemove = items.AsQueryable<T>().FirstOrDefault(predicate);
                if (dataToRemove is null) return false;
                bool removed = items.Remove(dataToRemove);
                if (removed)
                {
                    await SaveAllAsync(items);
                    return true;
                }
                 return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task SaveAllAsync(List<T> items)
        {
            string json = JsonConvert.SerializeObject(items);
            await File.WriteAllTextAsync(_filename, json);
        }

        public async Task<List<T>> GetAllAsync()
        {
            if (!File.Exists(_filename))
            {
                return new List<T>();
            }

            string json = await File.ReadAllTextAsync(_filename);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        //This method takes an Expression<Func<T, bool>> as input, which is used as a predicate to match items by their ID.
        //The Compile() method is called on the predicate to convert it to a delegate that can be used with the FirstOrDefault
        //LINQ method to retrieve the first item that matches the predicate.
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate)
        {
            List<T> items = await GetAllAsync();
            T item = items.FirstOrDefault(predicate.Compile());
            return item;
        }
    }
}
