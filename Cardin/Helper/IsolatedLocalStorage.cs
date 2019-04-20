using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardin.Helper
{
    public class IsolatedLocalStorage
    {
        IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
        public void Write(string file,string content)
        {
            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(file, FileMode.Create, isoStore))
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    writer.WriteLine(content);
                }
            }
        }

        public string Read(string file)
        {
            if (isoStore.FileExists(file))
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(file, FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            else
            {
                return "";
            }
        }
    }
}
