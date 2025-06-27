using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Utils
{
    public static class CustomListUtil
    {
        public static (
            List<T> idsToRemove,
            List<T> idsToAdd,
            List<T> idsToKeep
            ) splitIdsToAddAndRemove<T>(List<T> oldList, List<T> newList) where T : class
        {
            var oldDict = oldList.ToDictionary(item => GetId(item));
            var newDict = newList.ToDictionary(item => GetId(item));

            List<T> itemsToKeep = new List<T>();
            List<T> itemsToAdd = new List<T>();
            List<T> itemsToRemove = new List<T>();

            //Categroize list to Keep, Add, Remove
            foreach (var id in newDict.Keys)
            {
                if (oldDict.ContainsKey(id))
                {
                    itemsToKeep.Add(oldDict[id]);
                }
                else
                {
                    itemsToAdd.Add(newDict[id]);
                }
            }

            foreach (var id in oldDict.Keys)
            {
                if (!newDict.ContainsKey(id))
                {
                    itemsToRemove.Add(oldDict[id]);
                }
            }

            return (itemsToRemove, itemsToAdd, itemsToKeep);
        }

        private static Ulid GetId<T>(T obj)
        {
            var idProperty = typeof(T)
                                .GetProperties()
                                .FirstOrDefault(item =>
                                            item.Name.IndexOf("Id", StringComparison.OrdinalIgnoreCase) >= 0 &&
                                            item.PropertyType == typeof(Ulid));
            if (idProperty == null || idProperty.PropertyType != typeof(Ulid))
                throw new InvalidOperationException("Type must have a Ulid Id property.");

            return (Ulid)idProperty.GetValue(obj)!;
        }
    }
}
