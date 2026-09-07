using System;
using System.Collections.Generic;
using System.Text;

namespace ibDiary_data.Models.Interfaces
{
    public interface IUpdatableObject<T> where T : class
    {
        public void UpdateProperties(T source);
    }
}
