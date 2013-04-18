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

        private ContentService() { }
    }
}
