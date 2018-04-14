using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMTool.Source
{
    public class RidersViewModel : ViewModel
    {
        public ObservableCollection<Rider> DiscoveredRiders;

        public RidersViewModel()
        {
            DiscoveredRiders = new ObservableCollection<Rider>();
            string path = "./Riders";

            if (Directory.Exists(path))
            {
                foreach (string file in Directory.EnumerateFiles(path))
                {
                    try
                    {
                        string s = File.ReadAllText(file);
                        Rider r = JsonConvert.DeserializeObject<Rider>(s);
                        DiscoveredRiders.Add(r);
                    }
                    catch { }
                }
            }
        }
    }
}
