using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DMTool.Source
{
    public class Rider : ViewModel, IEquatable<Rider>, ICloneable
    {
        public Rider()
        {
            Progression = 0;
        }

        [JsonIgnore]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public event EventHandler Remove;
        
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public float Progression
        {
            get { return GetProperty<float>(); }
            set { SetProperty(value); }
        }

        public Rider(int duration)
        {
            Duration = duration;
        }

        public bool ClockTick(float step)
        {
            Progression += step;
            return Progression >= Duration;
        }

        public void Removed()
        {
            Remove?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Rider other)
        {
            return Equals(other as object);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Rider))
            {
                return false;
            }

            return (obj as Rider).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public object Clone()
        {
            return new Rider()
            {
                Description = Description,
                Duration = Duration,
                Name = Name
            };
        }
    }
}
