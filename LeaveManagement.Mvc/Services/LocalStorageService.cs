using Hanssens.Net;
using LeaveManagement.Mvc.Contracts;
using System.Collections.Generic;

namespace LeaveManagement.Mvc.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private LocalStorage _localStorage;

        public LocalStorageService()
        {
            var config = new LocalStorageConfiguration()
            {
                AutoLoad = true,
                AutoSave = true,
                Filename = "LEAVEMNGT"
            };

            _localStorage = new LocalStorage(config);
        }

        public void ClearStorage(List<string> keys)
        {
            foreach (var key in keys)
                _localStorage.Remove(key);
        }

        public bool Exists(string key)
        {
            return _localStorage.Exists(key);
        }

        public T GetStorageValue<T>(string key)
        {
            return _localStorage.Get<T>(key);
        }

        public void SetStorageValue<T>(string key, T value)
        {
            _localStorage.Store<T>(key, value);
            _localStorage.Persist();
        }
    }
}
