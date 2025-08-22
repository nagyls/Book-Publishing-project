using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPublishing
{
    internal class BookEdition
    {
        public int year;
        public int huEdition;
        public int huPublishing;
        public int abroadEdition;
        public int abroadPublishing;

        public BookEdition(int year)
        {
            this.year = year;
        }
    }
}
