using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ToDoList
{
    public partial class FrmMain : Form
    {
        private BindingList<ToDoEntry> entries = new BindingList<ToDoEntry>();

        public FrmMain()
        {
            InitializeComponent();

            titleText.DataBindings.Add("Text", entriesSource, "Title", true, DataSourceUpdateMode.OnPropertyChanged);
            dueDatePicker.DataBindings.Add("Value", entriesSource, "DueDate", true, DataSourceUpdateMode.OnPropertyChanged);

            entriesSource.DataSource = entries;

            dueDatePicker.Enabled = false;
            deleteButton.Enabled = false;
            descriptionText.Enabled = false;
            titleText.Enabled = false;
        }

        public void Save()
        {
            StreamWriter FileCreator = new StreamWriter(Application.StartupPath + @"\userdata.txt");

            foreach (ListViewItem l in entriesListView.Items)
            {
                FileCreator.WriteLine(l.Text);
            }

            FileCreator.Close();
        }

        public void Read()
        {
            /*StreamReader FileReader = new StreamReader(Application.StartupPath + @"\userdata.txt");
            txtOriginal.Text = FileReader.ReadToEnd();
            FileReader.Close();*/
        }

        private void CreateNewItem()
        {
            if (deleteButton.Enabled == false)
            {
                dueDatePicker.Enabled = true;
                deleteButton.Enabled = true;
                descriptionText.Enabled = true;
                titleText.Enabled = true;
            }

            ToDoEntry newEntry = (ToDoEntry)entriesSource.AddNew();
            newEntry.Title = "New Entry";
            entriesSource.ResetCurrentItem();
        }

        private void MakeListViewItemForNewEntry(int newItemIndex)
        {
            ListViewItem item = new ListViewItem();
            item.SubItems.Add("");
            entriesListView.Items.Insert(newItemIndex, item);
        }

        private void UpdateListViewItem(int itemIndex)
        {
            ListViewItem item = entriesListView.Items[itemIndex];
            ToDoEntry entry = entries[itemIndex];
            item.SubItems[0].Text = entry.Title;
            if (dueDatePicker.Checked == true)
                item.SubItems[1].Text = entry.DueDate.ToShortDateString();
            else
                item.SubItems[1].Text = "";
        }

        private void RemoveListViewItem(int deletedItemIndex)
        {
            entriesListView.Items.RemoveAt(deletedItemIndex);
        }

        private void FrmMain_ResizeEnd(object sender, EventArgs e)
        {
            if (this.Height < 360)
                this.Height = 360;
            if (this.Width < 345)
                this.Width = 345;
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.N:
                    CreateNewItem();
                    break;
                case Keys.Delete:
                    deleteButton.PerformClick();
                    break;
            }
        }

        private void entriesSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    MakeListViewItemForNewEntry(e.NewIndex);
                    break;
                case ListChangedType.ItemDeleted:
                    RemoveListViewItem(e.NewIndex);
                    break;
                case ListChangedType.ItemChanged:
                    UpdateListViewItem(e.NewIndex);
                    break;
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            CreateNewItem();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (entriesListView.SelectedIndices.Count != 0)
            {
                int entryIndex = entriesListView.SelectedIndices[0];
                entriesSource.RemoveAt(entryIndex);
            }

            if (entriesListView.Items.Count <= 0)
            {
                dueDatePicker.Enabled = false;
                deleteButton.Enabled = false;
                descriptionText.Enabled = false;
                titleText.Enabled = false;
            }
        }

        private void entriesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            dueDatePicker.Checked = false;

            if (entriesListView.SelectedIndices.Count != 0)
            {
                int entryIndex = entriesListView.SelectedIndices[0];
                entriesSource.Position = entryIndex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }

    }
}