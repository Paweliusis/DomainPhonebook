using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Classes
{
    public static class ObservableCollectionExtensions
    {
        public static void UpdateItems(
            this ObservableCollection<Models.Employee> targetCollection,
            ObservableCollection<Models.Employee> updateCollection,
            Func<Models.Employee, Models.Employee, bool> matcher,
            Action<Models.Employee, Models.Employee> updater)
        {
            if (updateCollection == null) return;
            var itemsToRemove = new List<Models.Employee>();

            UpdateExistingItemsAndIdentifyRemovedItems(targetCollection, updateCollection, matcher, updater, itemsToRemove);

            RemoveOldItems(targetCollection, itemsToRemove);

            AddNewItemsToTarget(targetCollection, updateCollection, matcher);
        }
        private static void UpdateExistingItemsAndIdentifyRemovedItems(
            ObservableCollection<Models.Employee> targetCollection,
            ObservableCollection<Models.Employee> updateCollection,
            Func<Models.Employee, Models.Employee, bool> matcher,
            Action<Models.Employee, Models.Employee> updater,
            List<Models.Employee> itemsToRemove)
        {
            foreach (var targetItem in targetCollection)
            {
                var updateItem = updateCollection.FirstOrDefault(i => matcher(targetItem, i));
                if (updateItem == null)
                {
                    itemsToRemove.Add(targetItem);
                    continue;
                }
                updater?.Invoke(targetItem, updateItem);
            }
        }
        private static void RemoveOldItems(
            ObservableCollection<Models.Employee> targetCollection,
            IEnumerable<Models.Employee> itemsToRemove)
        {
            foreach (var item in itemsToRemove)
            {
                targetCollection.Remove(item);
            }
        }
        private static void AddNewItemsToTarget(
            ObservableCollection<Models.Employee> targetCollection,
            ObservableCollection<Models.Employee> updateCollection,
            Func<Models.Employee, Models.Employee, bool> matcher
            )
        {
            var itemsToAdd = updateCollection.Where(updateItem =>
            !targetCollection.Any(existingItem => matcher(existingItem, updateItem))).ToList();

            foreach (var item in itemsToAdd)
            {
                targetCollection.Add(item);
            }
        }
    }
}
