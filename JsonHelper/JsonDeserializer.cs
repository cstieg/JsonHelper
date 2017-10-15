// Written by: Brad Parks (https://stackoverflow.com/questions/4611031/convert-json-string-to-c-sharp-object)

/* USAGE:
 var d = new JsonDeserializer(json);
  d.GetString("glossary.title").Dump();
  d.GetString("glossary.GlossDiv.title").Dump();  
  d.GetString("glossary.GlossDiv.GlossList.GlossEntry.ID").Dump();  
  d.GetInt("glossary.GlossDiv.GlossList.GlossEntry.ItemNumber").Dump();    
  d.GetObject("glossary.GlossDiv.GlossList.GlossEntry.GlossDef").Dump();   
  d.GetObject("glossary.GlossDiv.GlossList.GlossEntry.GlossDef.GlossSeeAlso").Dump(); 
  d.GetObject("Some Path That Doesnt Exist.Or.Another").Dump();   
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cstieg.JsonHelper
{
    /// <summary>
    /// Deserializes JSON string back into an object
    /// </summary>
    public class JsonDeserializer
    {
        private IDictionary<string, object> jsonData { get; set; }

        public JsonDeserializer(string json)
        {
            jsonData = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
        }

        public string GetString(string path)
        {
            return (string)GetObject(path);
        }

        public int? GetInt(string path)
        {
            int? result = null;

            object o = GetObject(path);
            if (o == null)
            {
                return result;
            }

            if (o is string)
            {
                result = Int32.Parse((string)o);
            }
            else
            {
                result = (Int32)o;
            }

            return result;
        }

        public object GetObject(string path)
        {
            object result = null;

            var curr = jsonData;
            var paths = path.Split('.');
            var pathCount = paths.Count();

            try
            {
                for (int i = 0; i < pathCount; i++)
                {
                    var key = paths[i];
                    if (i == (pathCount - 1))
                    {
                        result = curr[key];
                    }
                    else
                    {
                        curr = (IDictionary<string, object>)curr[key];
                    }
                }
            }
            catch
            {
                // Probably means an invalid path (ie object doesn't exist)
            }

            return result;
        }
    }
}
