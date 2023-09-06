namespace prjCoreWebWantWant.Hubs
{
    public static class ConnectionManager
    {
        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public static void AddConnection(string userId, string connectionId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(connectionId))
            {
                throw new ArgumentException("UserId and ConnectionId cannot be null or empty.");
            }

            lock (_connections)
            {
                _connections[userId] = connectionId;
            }
        }

        public static string GetConnectionId(string userId)
        {
            lock (_connections)
            {
                _connections.TryGetValue(userId, out string connectionId);
                return connectionId;
            }
        }

        public static void RemoveConnection(string userId)
        {
            lock (_connections)
            {
                _connections.Remove(userId);
            }
        }
    }

}
