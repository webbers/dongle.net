using System.Collections.Generic;
using System.Linq;
using Dongle.Data;

namespace Dongle.System.Collections
{
    public static class OrderedListExtensions
    {
        public static IList<T> MoveItemDown<T>(this IList<T> list, int itemOrder) where T : IHaveOrder
        {
            list = list.OrderBy(i => i.Order).ToList();
            if (itemOrder < list.Count() - 1)
            {
                var newPos = itemOrder + 1;
                Swap(list, itemOrder, newPos);
            }
            RearrangeOrder(list);
            return list;
        }

        public static IList<T> MoveItemUp<T>(this IList<T> list, int itemOrder) where T : IHaveOrder
        {
            list = list.OrderBy(i => i.Order).ToList();
            if (itemOrder > 0)
            {
                var newPos = itemOrder - 1;
                Swap(list, itemOrder, newPos);
            }
            RearrangeOrder(list);
            return list;
        }

        public static void RearrangeOrder<T>(this IList<T> list) where T : IHaveOrder
        {
            list = list.OrderBy(i => i.Order).ToList();
            for (var i = 0; i < list.Count; i++)
            {
                var element = list[i];
                element.Order = i;
            }
        }

        private static void Swap<T>(IList<T> list, int itemOrder1, int itemOrder2) where T : IHaveOrder
        {
            list[itemOrder2].Order = itemOrder1;
            list[itemOrder1].Order = itemOrder2;
        }
    }
}