using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System;

namespace simple_messaging_system.Data
{
    public class XpoDataStore
    {
        private static readonly Lazy<XpoDataStore> instance = new Lazy<XpoDataStore>(() => new XpoDataStore());
        public static XpoDataStore Instance => instance.Value;

        private readonly string connectionString;
        private IDataLayer dataLayer;
        private readonly XPDictionary dictionary;

        private XpoDataStore()
        {
            connectionString = "XpoProvider=SQLite;Data Source=Messages.db";
            dictionary = new ReflectionDictionary();
            dictionary.GetDataStoreSchema(typeof(Models.Message).Assembly);
        }

        public IDataLayer GetDataLayer()
        {
            if (dataLayer == null)
            {
                dataLayer = new SimpleDataLayer(dictionary, XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.DatabaseAndSchema));
            }
            return dataLayer;
        }

        public UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(GetDataLayer());
        }
    }
}
