﻿namespace Common.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Create(T obj);
        Task<T> GetById(int id);
        Task<T> Update(T obj);
        Task<T> Delete(int id);
    }
}
