using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility
{
    class ContentContainer
    {
        private static ContentContainer _instance = new ContentContainer();

        private Hashtable TexturesContainer = new Hashtable();
        private Hashtable ModelsContainer = new Hashtable();

        public static void LoadContent(ContentManager content)
        {
            // StreamReader reader = new StreamReader(Path.Combine(content.RootDirectory, "TextureList.json"));
            string path = Path.Combine(content.RootDirectory, "TextureList.json");
            string json = "";
            using (StreamReader stream = new StreamReader(TitleContainer.OpenStream(path)))
            {
                json = stream.ReadToEnd();
            }

            Hashtable ContentList = JsonConvert.DeserializeObject<Hashtable>(json);

            foreach(string key in ContentList.Keys)
            {
                AddContent<Texture2D>(key, content.Load<Texture2D>(ContentList[key] as string));
            }


            path = Path.Combine(content.RootDirectory, "ObjectList.json");
            using (StreamReader stream = new StreamReader(TitleContainer.OpenStream(path)))
            {
                json = stream.ReadToEnd();
            }

            ContentList = JsonConvert.DeserializeObject<Hashtable>(json);

            foreach (string key in ContentList.Keys)
            {
                AddContent<Model>(key, content.Load<Model>(ContentList[key] as string));
            }
        }

        public static void AddContent<T>(string name,T obj)
        {
            
            if (typeof(T) == typeof(Texture2D))
                _instance.TexturesContainer.Add(name, obj);
            else
                _instance.ModelsContainer.Add(name, obj);
        }

        public static T GetObjectFromContainer<T>(string Name)
        {
            if(typeof(T) == typeof(Texture2D))
                 return (T)_instance.TexturesContainer[Name] ;
            else
                return (T)_instance.ModelsContainer[Name];
        }

        public ContentContainer()
        {
           
        }
    }
}
