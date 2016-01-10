namespace ApkBatchInstaller
{
    /// <summary>
    /// Config.User.Devices
    /// </summary>
    class Device
    {
        /// <summary>
        /// 设备ID(ip + port)
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// 设备标识
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 设备Mac地址
        /// </summary>
        public string Mac { get; private set; }

        public Device(string id, string name, string mac)
        {
            ID = id;
            Name = name;
            Mac = mac;
        }
    }
}
