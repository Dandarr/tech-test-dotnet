using System.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public interface IAccountDataStoreFactory
    {
        IAccountDataStore Create();
    }

    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        public IAccountDataStore Create()
        {
            //configuration logic
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            if (dataStoreType == "Backup")
            {
                return new BackupAccountDataStore();
            }

            return new AccountDataStore();
        }
    }
}