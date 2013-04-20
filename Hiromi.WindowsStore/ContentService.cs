using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Hiromi
{
    public class ContentService
    {
        private static ContentService _instance;
        public static ContentService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ContentService();
                }
                return _instance;
            }
        }

        public ContentManager Content { get; set; }

        private Dictionary<string, object> _cachedAssets;

        private ContentService() 
        {
            _cachedAssets = new Dictionary<string, object>();
        }

        public T GetAsset<T>(string assetName)
        {
            if (!_cachedAssets.ContainsKey(assetName))
            {
                _cachedAssets.Add(assetName, this.Content.Load<T>(assetName));
            }
            return (T)_cachedAssets[assetName];
        }
    }
}
