using System.Configuration;

namespace zapad.crm.WebApiSync.Helpers
{
    public class SelfHostConfigRetriever
    {
        public static ConfigRetrieverSection _Config = ConfigurationManager.GetSection("selfHostConfigRetriever") as ConfigRetrieverSection;
        public static ConfigElementCollection GetHostConfig()
        {
            return _Config.ConfigItems;
        }
    }
    public class ConfigRetrieverSection : ConfigurationSection
    {
        [ConfigurationProperty("selfHostConfigs")]
        public ConfigElementCollection ConfigItems
        {
            get { return (ConfigElementCollection)this["selfHostConfigs"]; }
        }
    }

    [ConfigurationCollection(typeof(ConfigItem))]
    public class ConfigElementCollection : ConfigurationElementCollection
    {
        public ConfigItem this[int index]
        {
            get { return (ConfigItem)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConfigItem)element).Name;
        }

        public ConfigItem GetElementByName(string name)
        {
            foreach (ConfigItem element in this)
            {
                if (element.Name == name)
                {
                    return element;
                }
            }
            return new ConfigItem();
        }
    }

    public class ConfigItem : ConfigurationElement
    {
        public ConfigItem() { }

        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}
