using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Interfaces
{
    public interface IMergableListItem<T> where T : class
    {
        public void MergeToList(T target);
    }
}
