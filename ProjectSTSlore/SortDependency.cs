using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Dependencies
{
    public class SortDependency
    {
        public static string GetSortProperty(DependencyObject obj)
        {
            return (string)obj.GetValue(SortProperty);
        }
        public static void SetSortProperty(DependencyObject obj, string value)
        {
            obj.SetValue(SortProperty, value);
        }
        public static readonly DependencyProperty SortProperty = DependencyProperty.RegisterAttached(
            "Sort",
            typeof(string),
            typeof(SortDependency),
            new UIPropertyMetadata(null)
            );


        public static bool GetEnableSortProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableSortProperty);
        }
        public static void SetEnableSortProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableSortProperty, value);
        }
        public static readonly DependencyProperty EnableSortProperty = DependencyProperty.RegisterAttached(
            "EnableSort",
            typeof(bool),
            typeof(SortDependency),
            new UIPropertyMetadata(
                false,
                (d, e) => {
                    ListView listview = d as ListView;
                    if (listview != null)
                    {
                        if ((bool)e.OldValue && !(bool)e.NewValue)
                            listview.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                        else if(!(bool)e.OldValue && (bool)e.NewValue)
                            listview.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                    }
                }
                )
            );


        private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(e.OriginalSource);
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null)
            {
                string propertyName = GetSortProperty(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    ListView listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null)
                    {
                        if (GetEnableSortProperty(listView))
                        {
                            ApplySort(listView.Items, propertyName);
                        }
                    }
                }
            }
        }
        public static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent != null)
                return (T)parent;
            else
                return null;
        }
        public static void ApplySort(ICollectionView view, string propertyName)
        {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                SortDescription currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                {
                    if (currentSort.Direction == ListSortDirection.Ascending)
                        direction = ListSortDirection.Descending;
                    else
                        direction = ListSortDirection.Ascending;
                }
                view.SortDescriptions.Clear();
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
            }
        }
    }
}
