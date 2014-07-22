using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BtrieveWrapper.Orm.Models.Generator.Gui
{
    static class Extensions
    {
        public static void Up(this DataGrid dataGrid) {
            var list = dataGrid.ItemsSource as IList;
            if (list != null) {
                var indexes = new List<int>();
                foreach (var item in dataGrid.SelectedItems) {
                    indexes.Add(list.IndexOf(item));
                }
                var validIndexes=indexes.Distinct().OrderBy(i => i).Where((i1, i2) => i1 != i2);
                var invalidIndexes=indexes.Distinct().OrderBy(i => i).Where((i1, i2) => i1 == i2);
                foreach (var index in validIndexes) {
                    var item = list[index];
                    list.RemoveAt(index);
                    list.Insert(index - 1, item);
                }
                dataGrid.SelectedItems.Clear();
                foreach (var index in invalidIndexes) {
                    dataGrid.SelectedItems.Add(list[index]);
                }
                foreach (var index in validIndexes) {
                    dataGrid.SelectedItems.Add(list[index - 1]);
                }
            }
        }

        public static void Down(this DataGrid dataGrid) {
            var list = dataGrid.ItemsSource as IList;
            if (list != null) {
                var indexes = new List<int>();
                foreach (var item in dataGrid.SelectedItems) {
                    indexes.Add(list.IndexOf(item));
                }
                var validIndexes = indexes.Distinct().OrderBy(i => i).Where((i1, i2) => list.Count - i1 - 1 != i2);
                var invalidIndexes = indexes.Distinct().OrderBy(i => i).Where((i1, i2) => list.Count - i1 - 1 == i2);
                foreach (var index in indexes.Distinct().OrderByDescending(i => i).Where((i1, i2) => list.Count - i1 - 1 != i2)) {
                    var item = list[index];
                    list.RemoveAt(index);
                    list.Insert(index + 1, item);
                }
                dataGrid.SelectedItems.Clear();
                foreach (var index in invalidIndexes) {
                    dataGrid.SelectedItems.Add(list[index]);
                }
                foreach (var index in validIndexes) {
                    dataGrid.SelectedItems.Add(list[index + 1]);
                }
            }
        }
    }
}
