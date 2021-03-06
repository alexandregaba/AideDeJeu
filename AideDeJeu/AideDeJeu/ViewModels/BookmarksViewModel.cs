﻿using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace AideDeJeu.ViewModels
{
    public class BookmarksViewModel : BaseViewModel
    {
        public BookmarksViewModel()
        {
            LoadBookmarkCollection(BookmarkCollectionNames[BookmarkCollectionIndex]);
        }

        public ObservableCollection<string> BookmarkCollectionNames { get; set; } = new ObservableCollection<string>()
        {
            "Général",
            "Grimoire",
            "Bestiaire",
            "Sac",
        };
        private int _BookmarkCollectionIndex = 0;
        public int BookmarkCollectionIndex
        {
            get
            {
                return _BookmarkCollectionIndex;
            }
            set
            {
                SetProperty(ref _BookmarkCollectionIndex, value);
                LoadBookmarkCollection(BookmarkCollectionNames[BookmarkCollectionIndex]);
            }
        }
        private ObservableCollection<Item> _BookmarkCollection = new ObservableCollection<Item>();
        public ObservableCollection<Item> BookmarkCollection
        {
            get
            {
                return _BookmarkCollection;
            }
            set
            {
                SetProperty(ref _BookmarkCollection, value);
            }
        } 

        public async Task<List<Item>> GetBookmarkCollection(string key)
        {
            if (key != null)
            {
                if (App.Current.Properties.ContainsKey(key))
                {
                    var property = App.Current.Properties[key] as string;
                    if (property != null)
                    {
                        return (await ToItems(property)).ToList();
                    }
                }
            }
            return null;
        }
        public async Task LoadBookmarkCollection(string key)
        {
            var items = await GetBookmarkCollection(key);
            BookmarkCollection.Clear();
            if (items != null)
            {
                items.ForEach(item => BookmarkCollection.Add(item));
            }
        }

        public async Task AddBookmarkAsync(string key, Item item)
        {
            var linkItem = new LinkItem() { Name = item.Name, AltName = item.AltName, Link = item.Id };
            var items = await GetBookmarkCollection(key);
            if(items == null)
            {
                items = new List<Item>();
            }
            items.Add(linkItem);
            await SaveBookmarksAsync(key, items);
            BookmarkCollectionIndex = BookmarkCollectionNames.IndexOf(key);
        }

        public async Task SaveBookmarksAsync(string key, List<Item> items)
        {
            App.Current.Properties[key] = ToString(items);
            await App.Current.SavePropertiesAsync();
        }

        public string ToString(List<Item> items)
        {
            string md = string.Empty;
            md += "\n<!--Items-->\n\n";
            foreach(var item in items)
            {
                md += item.Markdown;
            }
            md += "\n\n<!--/Items-->\n";
            return md;
        }

        public async Task<IEnumerable<Item>> ToItems(string md)
        {
            var item = Store.ToItem(null, md);
            //if(item is Items)
            //{
            var items = item; // as Items;
                return await items.GetChildrenAsync();
            //}
            //return new List<Item> { item };
        }

            /*
            public string ToString(List<Item> items)
            {
                var serializer = ItemJsonSerializer;
                using(var stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, items);
                    stream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            public List<Item> ToItems(string str)
            {
                var serializer = ItemJsonSerializer;
                byte[] byteArray = Encoding.UTF8.GetBytes(str);
                using (var stream = new MemoryStream(byteArray))
                {
                    return serializer.ReadObject(stream) as List<Item>;
                }
            }

            public DataContractJsonSerializer ItemJsonSerializer
            {
                get
                {
                    var settings = new DataContractJsonSerializerSettings();
                    settings.KnownTypes = new List<Type>()
                    {
                        typeof(HomeItem),
                        typeof(Spell),
                        typeof(Monster),
                        //typeof(Items),
                        typeof(LinkItem),
                        typeof(Equipment),
                        //typeof(Spells),
                        //typeof(Monsters),
                        //typeof(Equipments),
                        typeof(PageItem),
                    };
                    return new DataContractJsonSerializer(typeof(List<Item>), settings);
                }
            }
            */
        }
    }
