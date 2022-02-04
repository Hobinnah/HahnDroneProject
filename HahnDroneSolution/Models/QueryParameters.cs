using System;

namespace HahnDroneAPI.Models
{
    public class QueryParameters
    {
        const int _maxSize = 100;
        private int _size = 50;

        public int Page { get; set; } = 1;
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value < 1 ? _size : Math.Min(_maxSize, value);
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        private string _sortOrder = "asc";
        public string SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                if (value == "asc" || value == "desc")
                {
                    _sortOrder = value;
                }
            }
        }


    }
}
