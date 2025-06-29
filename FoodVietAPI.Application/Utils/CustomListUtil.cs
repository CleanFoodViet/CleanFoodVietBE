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
           List<T> itemsToRemove,
           List<T> itemsToAdd,
           List<T> itemsToKeep
        ) SplitObjectsById<T>(
           List<T> oldList,
           List<T> newList,
           Func<T, Ulid?> idSelector)
        {
            var oldDict = oldList
                .Where(x => idSelector(x).HasValue && idSelector(x) != Ulid.Empty)
                .ToDictionary(x => idSelector(x).Value);

            var newWithValidIds = newList
                .Where(x => idSelector(x).HasValue && idSelector(x) != Ulid.Empty)
                .ToList();

            var newWithNoIds = newList
                .Where(x => !idSelector(x).HasValue || idSelector(x) == Ulid.Empty)
                .ToList();

            List<T> itemsToKeep = newWithValidIds
                .Where(x => oldDict.ContainsKey(idSelector(x).Value))
                .Select(x => oldDict[idSelector(x).Value])
                .ToList();

            List<T> itemsToAdd = newWithNoIds
                .Concat(newWithValidIds.Where(x => !oldDict.ContainsKey(idSelector(x).Value)))
                .ToList();

            List<T> itemsToRemove = oldDict
                .Where(kv => !newWithValidIds.Any(x => idSelector(x).Value == kv.Key))
                .Select(kv => kv.Value)
                .ToList();

            return (itemsToRemove, itemsToAdd, itemsToKeep);
        }
    }
}
