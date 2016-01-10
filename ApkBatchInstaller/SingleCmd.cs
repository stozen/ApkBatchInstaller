namespace ApkBatchInstaller
{
    /// <summary>
    /// Config.User.Cmds
    /// </summary>
    class SingleCmd
    {
        /// <summary>
        /// 命令的顺序编号
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// 命令行说明
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 命令
        /// </summary>
        public string Cmd { get; private set; }

        public SingleCmd(string id, string name, string cmd)
        {
            ID = id;
            Name = name;
            Cmd = cmd;
        }
    }
}
