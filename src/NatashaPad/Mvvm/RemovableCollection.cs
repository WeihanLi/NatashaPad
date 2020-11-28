﻿using System;
using System.Collections.ObjectModel;

namespace NatashaPad.Mvvm
{
    public class RemovableCollection<T> : ObservableCollection<T>
        where T : CollectionItem
    {
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            item.NeedDeleteMe += Item_NeedDeleteMe;
        }

        protected virtual void Item_NeedDeleteMe(object sender, EventArgs e)
        {
            Remove((T)sender);
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            base.RemoveItem(index);
            item.NeedDeleteMe -= Item_NeedDeleteMe;
        }

        protected override void ClearItems()
        {
            foreach (var item in Items)
            {
                item.NeedDeleteMe -= Item_NeedDeleteMe;
            }

            base.ClearItems();
        }
    }
}