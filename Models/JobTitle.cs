using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    class JobTitle : IEquatable<JobTitle>
    {
        private string _name;
        private int _weight;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        public int Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
            }
        }
        public bool Equals(JobTitle other)
        {
            if (other is null)
                return false;
            return this.Name == other.Name && this.Weight == other.Weight;
        }
        public override bool Equals(object obj) => Equals(obj as JobTitle);
        public override int GetHashCode() => (Name, Weight).GetHashCode();
    }
}
